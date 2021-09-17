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
            var res = "����û".GetPYCode();
            Assert.AreEqual("CLM", res);

            res = "��ð���".GetPYCode();
            Assert.AreEqual("NHA", res);

            res = "��!�ð���".GetPYCode();
            Assert.AreEqual("NHA", res);

            res = "asd��!�ð���".GetPYCode();
            Assert.AreEqual("ASDNHA", res);

            res = "as2d��!�ð���".GetPYCode();
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

            res = "ʯͷ".GetPYCode();
            Assert.AreEqual("ST", res);

            //�����ַ��ص�ƴ������ĸ�ǱȽϳ��õ����ڣ����²��ԾͲ���ͨ��
            //res = "ʯͷ".GetPYCode();
            //Assert.AreEqual("DT", res);
        }

        [Test]
        public void GetWBCodeTest()
        {
            var res = "����û".GetWBCode();
            Assert.AreEqual("KBI", res);

            res = "��ð���".GetWBCode();
            Assert.AreEqual("WVK", res);

            res = "��!�ð���".GetWBCode();
            Assert.AreEqual("WVK", res);

            res = "asd��!�ð���".GetWBCode();
            Assert.AreEqual("ASDWVK", res);

            res = "as2d��!�ð���".GetWBCode();
            Assert.AreEqual("AS2DWVK", res);

            res = "ʯͷ".GetWBCode();
            Assert.AreEqual("DU", res);
        }


    }
}
