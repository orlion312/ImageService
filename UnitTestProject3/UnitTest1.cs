using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageService.Modal;

namespace UnitTestProject3
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int tumb = 120;
            bool result;
            string OutputDir = "C:\\Users\\or\\Desktop\\ImageServiceDir\\Images";
            IImageServiceModal imageModal = new ImageServiceModal(OutputDir, tumb);
            string ss = imageModal.AddFile("C:\\Users\\or\\Desktop\\ImageServiceDir\\Handler1\\lala.jpg", out result);
            Assert.AreEqual(result, true, ss); 
        }
    }
}
