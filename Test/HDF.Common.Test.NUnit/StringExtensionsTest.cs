using NUnit.Framework;

namespace HDF.Common.Test.NUnit
{
    public class StringExtensionsTest
    {


        [Test]
        public void IsNullOrEmptyTest()
        {
            var res = "����û".IsNullOrEmpty();
            Assert.False(res);

            res = "    ".IsNullOrEmpty();
            Assert.False(res);

            res = string.Empty.IsNullOrEmpty();
            Assert.True(res);

            res = ((string)null).IsNullOrEmpty();
            Assert.True(res);
        }

        [Test]
        public void IsNullOrWhiteSpaceTest()
        {
            var res = "����û".IsNullOrWhiteSpace();
            Assert.False(res);

            res = "    ".IsNullOrWhiteSpace();
            Assert.True(res);

            res = string.Empty.IsNullOrWhiteSpace();
            Assert.True(res);

            res = ((string)null).IsNullOrWhiteSpace();
            Assert.True(res);
        }

        [Test]
        public void ToSnakeCaseTest()
        {
            var res = "����û".ToSnakeCase();
            Assert.AreEqual("����û", res);

            res = "HuangDeFu".ToSnakeCase();
            Assert.AreEqual("huang_de_fu", res);

            res = "HUANGDEFU".ToSnakeCase();
            Assert.AreEqual("huangdefu", res);

            res = "HuangDEFU".ToSnakeCase();
            Assert.AreEqual("huang_defu", res);

            res = "HuangDEFU".ToSnakeCase();
            Assert.AreEqual("huang_defu", res);

            res = string.Empty.ToSnakeCase();
            Assert.AreEqual(string.Empty, res);

            res = ((string)null).ToSnakeCase();
            Assert.Null(res);

            res = " ".ToSnakeCase();
            Assert.AreEqual(" ", res);
        }


    }
}
