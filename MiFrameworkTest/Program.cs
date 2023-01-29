using MiFramework.Stream;

ByteArray byteArray1 = new ByteArray();
ByteArray byteArray2 = new ByteArray();


//byteArray1.WriteString("你好，世界！");
byteArray1.WriteIntAdaptive(-91);


byteArray1.CopyTo(byteArray2);

//Console.WriteLine(byteArray2.ReadString());

Console.WriteLine(byteArray2.ReadIntAdaptive());

