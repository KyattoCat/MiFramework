using MiFramework.Stream;

ByteArray byteArray1 = new ByteArray();
ByteArray byteArray2 = new ByteArray();

byteArray1.WriteIntAdaptive(8192);
byteArray1.WriteFloat(-0.01f);
byteArray1.WriteString("测试字符占用量");

byteArray1.WriteTo(byteArray2);
Console.WriteLine(byteArray2.ReadIntAdaptive());
Console.WriteLine(byteArray2.ReadFloat());
Console.WriteLine(byteArray2.ReadString());