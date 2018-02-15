using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace LiqPaySDK.Tests
{
    [TestClass]
    public class LiqPayUtilTest
    {
        [TestMethod]
        public void LiqPayUtilTest_Sha1()
        {
            Assert.AreEqual("i0XkvRxqy4i+v2QH0WIF9WfmKj4=", "some string".SHA1Hash().ToBase64String());
        }

        [TestMethod]
        public void LiqPayUtilTest_Base64Encode()
        {
            Assert.AreEqual("c29tZSBzdHJpbmc=", "some string".ToBase64String());
        }

        [TestMethod]
        public void LiqPayUtilTest_BytesBase64Encode()
        {
            Assert.AreEqual("c29tZSBzdHJpbmc=", Encoding.UTF8.GetBytes("some string").ToBase64String());
        }
    }
}