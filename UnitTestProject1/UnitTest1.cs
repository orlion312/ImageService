using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageService.Modal;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAddFile()
        {
            bool result;
            string output = "C:\\Users\\or\\Desktop\\ImageServiceDir\\Images";
            IImageServiceModal model = new ImageServiceModal(output, 5);
            model.AddFile("C:\\Users\\or\\Desktop\\ImageServiceDir\\Handler1\\or.jpg", out result);
            Assert.AreEqual(result, true, "Could not add the file.");
        }
    }
}
