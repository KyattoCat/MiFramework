using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiFramework.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFramework.Pool.Tests
{
    [TestClass()]
    public class ObjectPoolTests
    {

        public struct TestStruct
        {
            public int Value1;
            public float Value2;
            public double Value3;
            public byte Value4;
        }

        public ObjectPool<List<int>> pool = new(null, (list) => list.Clear());

        [TestMethod()]
        public void GetTest()
        {
            var list = pool.Get();
            Assert.IsNotNull(list);
        }

        [TestMethod()]
        public void ReleaseTest()
        {
            var list = pool.Get();
            Assert.IsNotNull(list);
            list.Add(1);
            list.Add(1);
            list.Add(1);
            list.Add(1);
            pool.Release(ref list);
            Assert.IsNull(list);

            list = pool.Get();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod()]
        public void RepeatReleaseTest()
        {
            var list = pool.Get();
            Assert.IsNotNull(list);
            pool.Release(ref list);
            pool.Release(ref list);
            pool.Release(ref list);
            Assert.IsNull(list);
        }
    }
}