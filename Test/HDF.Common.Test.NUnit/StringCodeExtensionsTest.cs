using NUnit.Framework;

namespace HDF.Common.Test.NUnit
{
    public class StringCodeExtensionsTest
    {
        [SetUp]
        public void Setup()
        {
        }



        [Test]
        public void GetPYCodeTest()
        {
            var res = "吃了没".GetPYCode();
            Assert.AreEqual("CLM", res);

            res = "你好啊！".GetPYCode();
            Assert.AreEqual("NHA", res);

            res = "你!好啊！".GetPYCode();
            Assert.AreEqual("NHA", res);

            res = "asd你!好啊！".GetPYCode();
            Assert.AreEqual("ASDNHA", res);

            res = "as2d你!好啊！".GetPYCode();
            Assert.AreEqual("AS2DNHA", res);

            res = string.Empty.GetPYCode();
            Assert.AreEqual(string.Empty, res);

            res = ((string)null).GetPYCode();
            Assert.AreEqual(string.Empty, res);

            res = "abc".GetPYCode();
            Assert.AreEqual("ABC", res);


            res = "ABC".GetPYCode();
            Assert.AreEqual("ABC", res);


            res = "123".GetPYCode();
            Assert.AreEqual("123", res);

            res = "石头".GetPYCode();
            Assert.AreEqual("ST", res);

            //多音字返回的拼音首字母是比较常用的音节，如下测试就不能通过
            //res = "石头".GetPYCode();
            //Assert.AreEqual("DT", res);
        }

        [Test]
        public void GetWBCodeTest()
        {
            var res = "吃了没".GetWBCode();
            Assert.AreEqual("KBI", res);

            res = "你好啊！".GetWBCode();
            Assert.AreEqual("WVK", res);

            res = "你!好啊！".GetWBCode();
            Assert.AreEqual("WVK", res);

            res = "asd你!好啊！".GetWBCode();
            Assert.AreEqual("ASDWVK", res);

            res = "as2d你!好啊！".GetWBCode();
            Assert.AreEqual("AS2DWVK", res);

            res = "石头".GetWBCode();
            Assert.AreEqual("DU", res);
        }


    }
}
