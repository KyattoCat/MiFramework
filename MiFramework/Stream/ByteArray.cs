using System.Text;

namespace MiFramework.Stream
{
    public class ByteArray
    {
        private byte[] buffer;
        private int position;

        public byte[] Buffer
        {
            get { return buffer; }
        }

        public int Length
        {
            get { return buffer.Length; }
        }

        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        public ByteArray(int size)
        {
            buffer = new byte[size];
        }

        public void Clear()
        {
            Array.Clear(buffer, 0, buffer.Length);
            position = 0;
        }

        public unsafe void Read(byte[] data, int offset, int count)
        {
            if (position + count > buffer.Length)
                throw new InvalidOperationException("Read beyond the buffer length.");

            fixed (byte* pSrc = &buffer[position], pDst = &data[offset])
            {
                byte* src = pSrc;
                byte* dst = pDst;
                for (int i = 0; i < count; i++)
                {
                    *dst++ = *src++;
                }
            }
            position += count;
        }

        public bool ReadBoolean()
        {
            return ReadByte() == 1;
        }

        public byte ReadByte()
        {
            if (position >= buffer.Length)
                throw new InvalidOperationException("Read beyond the buffer length.");

            byte value = buffer[position];
            position++;
            return value;
        }

        public double ReadDouble()
        {
            unsafe
            {
                long longValue = ReadLong();
                double* pDoubleValue = (double*)&longValue;
                return *pDoubleValue;
            }
        }

        public float ReadFloat()
        {
            unsafe
            {
                int intValue = ReadInt();
                float* pFloatValue = (float*)&intValue;
                return *pFloatValue;
            }
        }

        public int ReadInt()
        {
            if (position + 4 > buffer.Length)
                throw new InvalidOperationException("Read beyond the buffer length.");

            int value = (buffer[position] << 24) | (buffer[position + 1] << 16) | (buffer[position + 2] << 8) | buffer[position + 3];
            position += 4;
            return value;
        }

        public long ReadLong()
        {
            if (position + 8 > buffer.Length)
                throw new InvalidOperationException("Read beyond the buffer length.");

            long value = ((long)ReadInt() << 32) | (uint)ReadInt();
            return value;
        }

        public short ReadShort()
        {
            if (position + 2 > buffer.Length)
                throw new InvalidOperationException("Read beyond the buffer length.");

            short value = (short)((buffer[position] << 8) | buffer[position + 1]);
            position += 2;
            return value;
        }

        public unsafe string ReadString()
        {
            int length = ReadInt();
            if (position + length > buffer.Length)
                throw new InvalidOperationException("Read beyond the buffer length.");

            char* charBuffer = stackalloc char[length];
            byte* byteBuffer = stackalloc byte[length];

            fixed (byte* pSrc = &buffer[position])
            {
                byte* src = pSrc;
                for (int i = 0; i < length; i++)
                {
                    byteBuffer[i] = *src++;
                }
            }
            position += length;

            Encoding.UTF8.GetChars(byteBuffer, length, charBuffer, length);
            return new string(charBuffer, 0, length);
        }

        public uint ReadUInt()
        {
            if (position + 4 > buffer.Length)
                throw new InvalidOperationException("Read beyond the buffer length.");

            uint value = (uint)((buffer[position] << 24) | (buffer[position + 1] << 16) | (buffer[position + 2] << 8) | buffer[position + 3]);
            position += 4;
            return value;
        }

        public ulong ReadULong()
        {
            if (position + 8 > buffer.Length)
                throw new InvalidOperationException("Read beyond the buffer length.");

            ulong value = ((ulong)ReadUInt() << 32) | ReadUInt();
            return value;
        }

        public ushort ReadUShort()
        {
            if (position + 2 > buffer.Length)
                throw new InvalidOperationException("Read beyond the buffer length.");

            ushort value = (ushort)((buffer[position] << 8) | buffer[position + 1]);
            position += 2;
            return value;
        }

        public unsafe int ReadVarInt()
        {
            int result = 0;
            int shift = 0;
            byte b;
            do
            {
                if (position >= buffer.Length)
                    throw new InvalidOperationException("Read beyond the buffer length.");

                b = ReadByte();
                result |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return result;
        }

        public unsafe long ReadVarLong()
        {
            long result = 0;
            int shift = 0;
            byte b;
            do
            {
                if (position >= buffer.Length)
                    throw new InvalidOperationException("Read beyond the buffer length.");

                b = ReadByte();
                result |= (long)(b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return result;
        }

        public unsafe uint ReadVarUInt()
        {
            uint result = 0;
            int shift = 0;
            byte b;
            do
            {
                if (position >= buffer.Length)
                    throw new InvalidOperationException("Read beyond the buffer length.");

                b = ReadByte();
                result |= (uint)(b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return result;
        }

        public unsafe ulong ReadVarULong()
        {
            ulong result = 0;
            int shift = 0;
            byte b;
            do
            {
                if (position >= buffer.Length)
                    throw new InvalidOperationException("Read beyond the buffer length.");

                b = ReadByte();
                result |= (ulong)(b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return result;
        }

        public unsafe void Write(byte[] data, int offset, int count)
        {
            EnsureCapacity(count);
            fixed (byte* pSrc = &data[offset], pDst = &buffer[position])
            {
                byte* src = pSrc;
                byte* dst = pDst;
                for (int i = 0; i < count; i++)
                {
                    *dst++ = *src++;
                }
            }
            position += count;
        }

        public void WriteBoolean(bool value)
        {
            EnsureCapacity(1);
            buffer[position++] = (byte)(value ? 1 : 0);
        }

        public void WriteByte(byte value)
        {
            EnsureCapacity(1);
            buffer[position++] = value;
        }

        public void WriteDouble(double value)
        {
            EnsureCapacity(8);
            unsafe
            {
                double* pValue = &value;
                long* pLongValue = (long*)pValue;
                WriteLong(*pLongValue);
            }
        }

        public void WriteFloat(float value)
        {
            EnsureCapacity(4);
            unsafe
            {
                float* pValue = &value;
                int* pIntValue = (int*)pValue;
                WriteInt(*pIntValue);
            }
        }

        public void WriteInt(int value)
        {
            EnsureCapacity(4);
            buffer[position++] = (byte)(value >> 24);
            buffer[position++] = (byte)(value >> 16);
            buffer[position++] = (byte)(value >> 8);
            buffer[position++] = (byte)value;
        }

        public void WriteLong(long value)
        {
            EnsureCapacity(8);
            buffer[position++] = (byte)(value >> 56);
            buffer[position++] = (byte)(value >> 48);
            buffer[position++] = (byte)(value >> 40);
            buffer[position++] = (byte)(value >> 32);
            buffer[position++] = (byte)(value >> 24);
            buffer[position++] = (byte)(value >> 16);
            buffer[position++] = (byte)(value >> 8);
            buffer[position++] = (byte)value;
        }

        public void WriteShort(short value)
        {
            EnsureCapacity(2);
            buffer[position++] = (byte)(value >> 8);
            buffer[position++] = (byte)value;
        }

        public void WriteString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            WriteInt(bytes.Length);
            Write(bytes, 0, bytes.Length);
        }

        public void WriteUInt(uint value)
        {
            EnsureCapacity(4);
            buffer[position++] = (byte)(value >> 24);
            buffer[position++] = (byte)(value >> 16);
            buffer[position++] = (byte)(value >> 8);
            buffer[position++] = (byte)value;
        }

        public void WriteULong(ulong value)
        {
            EnsureCapacity(8);
            buffer[position++] = (byte)(value >> 56);
            buffer[position++] = (byte)(value >> 48);
            buffer[position++] = (byte)(value >> 40);
            buffer[position++] = (byte)(value >> 32);
            buffer[position++] = (byte)(value >> 24);
            buffer[position++] = (byte)(value >> 16);
            buffer[position++] = (byte)(value >> 8);
            buffer[position++] = (byte)value;
        }

        public void WriteUShort(ushort value)
        {
            EnsureCapacity(2);
            buffer[position++] = (byte)(value >> 8);
            buffer[position++] = (byte)value;
        }

        public unsafe void WriteVarInt(int value)
        {
            while ((value & ~0x7F) != 0)
            {
                WriteByte((byte)((value & 0x7F) | 0x80));
                value >>= 7;
            }
            WriteByte((byte)value);
        }

        public unsafe void WriteVarLong(long value)
        {
            while ((value & ~0x7FL) != 0)
            {
                WriteByte((byte)((value & 0x7F) | 0x80));
                value >>= 7;
            }
            WriteByte((byte)value);
        }

        public unsafe void WriteVarUInt(uint value)
        {
            while ((value & ~0x7F) != 0)
            {
                WriteByte((byte)((value & 0x7F) | 0x80));
                value >>= 7;
            }
            WriteByte((byte)value);
        }

        public unsafe void WriteVarULong(ulong value)
        {
            while ((value & ~0x7FUL) != 0)
            {
                WriteByte((byte)((value & 0x7F) | 0x80));
                value >>= 7;
            }
            WriteByte((byte)value);
        }

        private void EnsureCapacity(int additionalBytes)
        {
            int requiredCapacity = position + additionalBytes;
            if (requiredCapacity > buffer.Length)
            {
                int newCapacity = Math.Max(buffer.Length * 2, requiredCapacity);
                byte[] newBuffer = new byte[newCapacity];
                Array.Copy(buffer, newBuffer, buffer.Length);
                buffer = newBuffer;
            }
        }
    }
}