using System.Text;

namespace MiFramework.Stream
{
    public class ByteArray
    {
        private const int INT32_BYTE_COUNT = 4;
        private const int INT16_BYTE_COUNT = 2;

        private byte[] data;
        private uint offset;
        private uint capacity = 1;

        public ByteArray()
        {
            data = new byte[1];
        }

        public byte[] GetData()
        {
            return data;
        }
#if DEBUG
        public void CopyTo(ByteArray destination)
        {
            destination.capacity = capacity;
            destination.offset = 0;
            destination.data = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                destination.data[i] = data[i];
            }
        }
#endif
        private void ExpandCapacity()
        {
            ulong newCapacity = capacity * INT16_BYTE_COUNT;
            
            if (newCapacity > uint.MaxValue)
            {
                throw new OverflowException();
            }

            byte[] bytes = new byte[newCapacity];
            
            Array.Copy(data, bytes, capacity);
            
            capacity = (uint)newCapacity;

            data = bytes;
        }

        public void WriteByte(byte value)
        {
            if (offset + 1 > capacity)
            {
                ExpandCapacity();
            }
            data[offset++] = value;
        }

        public byte ReadByte()
        {
            return data[offset++];
        }

        private void WriteBytes(byte[] bytes, int index, int length)
        {
            for (int i = index; i < index + length; i++)
            {
                WriteByte(bytes[i]);
            }
        }

        private byte[] ReadBytes(int length)
        {
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                bytes[i] = ReadByte();
            }
            return bytes;
        }

        public void WriteInt(int value)
        {
            WriteBytes(BitConverter.GetBytes(value), 0, INT32_BYTE_COUNT);
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(INT32_BYTE_COUNT), 0);
        }

        public void WriteUInt16(ushort value)
        {
            WriteByte((byte)(value >> 8));
            WriteByte((byte)value);
        }

        public ushort ReadUInt16()
        {
            ushort result = 0;
            result |= (ushort)(ReadByte() << 8);
            result |= (ushort)(ReadByte() << 0);
            return result;
        }

        public void WriteUInt32(uint value)
        {
            WriteByte((byte)(value >> 24));
            WriteByte((byte)(value >> 16));
            WriteByte((byte)(value >> 8));
            WriteByte((byte)value);
        }

        public uint ReadUInt32()
        {
            uint result = 0;
            result |= (uint)(ReadByte() << 24);
            result |= (uint)(ReadByte() << 16);
            result |= (uint)(ReadByte() << 8);
            result |= (uint)(ReadByte() << 0);
            return result;
        }

        public void WriteFloat(float value)
        {
            WriteInt(BitConverter.SingleToInt32Bits(value));
        }

        public float ReadFloat()
        {
            return BitConverter.Int32BitsToSingle(ReadInt());
        }

        public void WriteString(string data)
        {
            char[] charArray = data.ToCharArray();
            
            WriteInt(charArray.Length);

            for (int i = 0; i < charArray.Length; i++)
            {
                WriteUInt16(charArray[i]);
            }
        }

        public string ReadString()
        {
            int length = ReadInt();
            
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                char temp = (char)ReadUInt16();
                stringBuilder.Append(temp);
            }

            return stringBuilder.ToString();
        }

        const uint NEG_FLAG = 0x80;
        const ushort INT16_FLAG = 0x2000;
        const uint INT32_FLAG = 0x40000000;

        public void WriteIntAdaptive(int data)
        {
            bool isPositive = data >= 0;
            int absData = isPositive ? data : -data;

            if (isPositive)
            {
                if (data < 32)
                {
                    byte temp = (byte)data;
                    WriteByte(temp);
                }
                else if (data < 8192)
                {
                    ushort temp = (ushort)data;
                    temp |= INT16_FLAG;
                    WriteUInt16(temp);
                }
                else if (data < 536870912)
                {
                    uint temp = (uint)data;
                    temp |= INT32_FLAG;
                    WriteUInt32(temp);
                }
            }
            else
            {
                if (absData < 32)
                {
                    byte temp = (byte)(absData | NEG_FLAG);
                    WriteByte(temp);
                }
                else if (absData < 8192)
                {
                    ushort temp = (ushort)(absData | (NEG_FLAG << 8));
                    temp |= INT16_FLAG;
                    WriteUInt16(temp);
                }
                else if (absData < 536870912)
                {
                    uint temp = (uint)(absData | (NEG_FLAG << 24));
                    temp |= INT32_FLAG;
                    WriteUInt32(temp);
                }
            }
        }

        public int ReadIntAdaptive()
        {
            byte temp1 = ReadByte();
            bool isPositive = (temp1 & NEG_FLAG) == 0;
            int flag = (temp1 >> 5) & 0b11;

            int result = 0;
            if (flag == 0)
            {
                result = temp1 & 0x1F;
            }
            else if (flag == 1)
            {
                byte temp2 = ReadByte();

                result |= (temp1 & 0x1F) << 8;
                result |= temp2;
            }
            else if (flag == 2)
            {
                byte temp2 = ReadByte();
                byte temp3 = ReadByte();
                byte temp4 = ReadByte();

                result |= (temp1 & 0x1F) << 24;
                result |= temp2 << 16;
                result |= temp3 << 8;
                result |= temp4;
            }

            return isPositive ? result : -result;
        }
    }
}
