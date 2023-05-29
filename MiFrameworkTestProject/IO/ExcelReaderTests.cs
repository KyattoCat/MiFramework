using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiFramework.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFramework.IO.Tests
{
    [TestClass()]
    public class ExcelReaderTests
    {
        [TestMethod()]
        public void ExcelReaderTest()
        {
            using (ExcelReader excelReader = new ExcelReader("D:\\Projects\\Gal\\Assets\\Resources\\Story\\chapter1.xlsx"))
            {
                Assert.AreEqual("id", excelReader.GetNextColumn());
            }
        }
    }
}