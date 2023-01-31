using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiFramework.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFramework.Event.Tests
{
    [TestClass()]
    public class EventManagerTests
    {
        public class TestEvent1 : EventArguments
        {
            public int id;
            public string? message;
        }

        public class TestEvent2 : EventArguments
        {
            public int id;
            public float price;
        }

        private static int CallBackCount = 0;

        public void TestEvent1CallBack1(object sender, TestEvent1 testEvent)
        {
            Assert.AreEqual(sender, this);
            Assert.AreEqual(0, testEvent.id);
            Assert.AreEqual("你好", testEvent.message);
        }

        // 测试重复注册的回调
        public void TestEvent1CallBack2(object sender, TestEvent1 testEvent)
        {
            Assert.AreEqual(sender, this);
            Assert.AreEqual(0, testEvent.id);
            Assert.AreEqual("你好", testEvent.message);
            CallBackCount++;
        }

        // 抛出异常的回调
        public void TestEvent1CallBack3(object sender, TestEvent1 testEvent)
        {
            throw new Exception("一个故意的异常");
        }

        public void TestEvent2CallBack(object sender, TestEvent2 testEvent)
        {
            Assert.AreEqual(sender, this);
            Assert.AreEqual(1, testEvent.id);
            Assert.AreEqual(9.15f, testEvent.price);
        }

        [TestMethod()]
        public void RegisterTest()
        {
            EventManager.Instance.Register<TestEvent1>(TestEvent1CallBack1);
            EventManager.Instance.Invoke(this, new TestEvent1() { id = 0, message = "你好" });
            EventManager.Instance.UnRegister<TestEvent1>(TestEvent1CallBack1);
        }

        [TestMethod()]
        public void DifferentTypeRegisterTest()
        {
            EventManager.Instance.Register<TestEvent1>(TestEvent1CallBack1);
            EventManager.Instance.Register<TestEvent2>(TestEvent2CallBack);
            EventManager.Instance.Invoke(this, new TestEvent2() { id = 1, price = 9.15f });
            EventManager.Instance.Invoke(this, new TestEvent1() { id = 0, message = "你好" });
            EventManager.Instance.UnRegister<TestEvent1>(TestEvent1CallBack1);
            EventManager.Instance.UnRegister<TestEvent2>(TestEvent2CallBack);
        }

        [TestMethod()]
        public void RepeatRegisterTest()
        {
            // 重复注册测试 当Invoke时应该只执行一次CallBack
            CallBackCount = 0;
            EventManager.Instance.Register<TestEvent1>(TestEvent1CallBack2);
            EventManager.Instance.Register<TestEvent1>(TestEvent1CallBack2);
            EventManager.Instance.Register<TestEvent1>(TestEvent1CallBack2);
            EventManager.Instance.Invoke(this, new TestEvent1() { id = 0, message = "你好" });
            EventManager.Instance.UnRegister<TestEvent1>(TestEvent1CallBack2);
            Assert.AreEqual(1, CallBackCount);
        }

        [TestMethod()]
        public void UnRegisterTest()
        {
            EventManager.Instance.Register<TestEvent1>(TestEvent1CallBack1);
            EventManager.Instance.Invoke(this, new TestEvent1() { id = 0, message = "你好" });
            EventManager.Instance.UnRegister<TestEvent1>(TestEvent1CallBack1);
            // 反注册后若进入CallBack则测试失败
            EventManager.Instance.Invoke(this, new TestEvent1() { id = 1, message = "你好1" });
        }

        [TestMethod()]
        public void InvokeTest()
        {
            // Invoke时的异常测试 当发生异常时应当继续输出日志并继续执行代码
            EventManager.Instance.Register<TestEvent1>(TestEvent1CallBack1);
            EventManager.Instance.Register<TestEvent1>(TestEvent1CallBack3);
            EventManager.Instance.Register<TestEvent1>(TestEvent1CallBack2);

            EventManager.Instance.Invoke(this, new TestEvent1() { id = 0, message = "你好" });
        }
    }
}