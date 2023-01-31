using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiFramework.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFramework.Stream.Tests
{
    [TestClass()]
    public class ByteArrayTests
    {
        [TestMethod()]
        public void WriteStringTest()
        {
            ByteArray byteArray1 = new ByteArray();
            ByteArray byteArray2 = new ByteArray();

            byteArray1.WriteString("abc");
            byteArray1.WriteString("你好世界");

            byteArray1.WriteTo(byteArray2);

            Assert.AreEqual("abc", byteArray2.ReadString());
            Assert.AreEqual("你好世界", byteArray2.ReadString());
        }

        [TestMethod()]
        public void WriteIntAdaptiveTest()
        {
            ByteArray byteArray1 = new ByteArray();
            ByteArray byteArray2 = new ByteArray();

            byteArray1.WriteIntAdaptive(536870911);
            byteArray1.WriteIntAdaptive(-536870911);
            byteArray1.WriteIntAdaptive(1243);
            byteArray1.WriteIntAdaptive(124626);
            byteArray1.WriteIntAdaptive(0);
            byteArray1.WriteIntAdaptive(-0);
            byteArray1.WriteIntAdaptive(88888);
            byteArray1.WriteIntAdaptive(32);
            byteArray1.WriteIntAdaptive(64);

            byteArray1.WriteTo(byteArray2);

            Assert.AreEqual(536870911, byteArray2.ReadIntAdaptive());
            Assert.AreEqual(-536870911, byteArray2.ReadIntAdaptive());
            Assert.AreEqual(1243, byteArray2.ReadIntAdaptive());
            Assert.AreEqual(124626, byteArray2.ReadIntAdaptive());
            Assert.AreEqual(0, byteArray2.ReadIntAdaptive());
            Assert.AreEqual(-0, byteArray2.ReadIntAdaptive());
            Assert.AreEqual(88888, byteArray2.ReadIntAdaptive());
            Assert.AreEqual(32, byteArray2.ReadIntAdaptive());
            Assert.AreEqual(64, byteArray2.ReadIntAdaptive());
        }

        [TestMethod()]
        public void WriteFloatTest()
        {
            ByteArray byteArray1 = new ByteArray();
            ByteArray byteArray2 = new ByteArray();

            byteArray1.WriteFloat(536870911);
            byteArray1.WriteFloat(-536870911);
            byteArray1.WriteFloat(1243);
            byteArray1.WriteFloat(124626);
            byteArray1.WriteFloat(0);
            byteArray1.WriteFloat(-0);
            byteArray1.WriteFloat(88888);
            byteArray1.WriteFloat(32);
            byteArray1.WriteFloat(64);

            byteArray1.WriteFloat(1.0f);
            byteArray1.WriteFloat(-1.0f);
            byteArray1.WriteFloat(125.5f);
            byteArray1.WriteFloat(100.0f);
            byteArray1.WriteFloat(0.8f);
            byteArray1.WriteFloat(3600.0f);
            byteArray1.WriteFloat(0.01f);
            byteArray1.WriteFloat(1309218.57f);

            byteArray1.WriteTo(byteArray2);

            Assert.AreEqual(536870911, byteArray2.ReadFloat());
            Assert.AreEqual(-536870911, byteArray2.ReadFloat());
            Assert.AreEqual(1243, byteArray2.ReadFloat());
            Assert.AreEqual(124626, byteArray2.ReadFloat());
            Assert.AreEqual(0, byteArray2.ReadFloat());
            Assert.AreEqual(-0, byteArray2.ReadFloat());
            Assert.AreEqual(88888, byteArray2.ReadFloat());
            Assert.AreEqual(32, byteArray2.ReadFloat());
            Assert.AreEqual(64, byteArray2.ReadFloat());

            Assert.AreEqual(1.0f, byteArray2.ReadFloat());
            Assert.AreEqual(-1.0f, byteArray2.ReadFloat());
            Assert.AreEqual(125.5f, byteArray2.ReadFloat());
            Assert.AreEqual(100.0f, byteArray2.ReadFloat());
            Assert.AreEqual(0.8f, byteArray2.ReadFloat());
            Assert.AreEqual(3600.0f, byteArray2.ReadFloat());
            Assert.AreEqual(0.01f, byteArray2.ReadFloat());
            Assert.AreEqual(1309218.57f, byteArray2.ReadFloat());
        }
    }
}