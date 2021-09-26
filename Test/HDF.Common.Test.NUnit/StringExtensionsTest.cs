using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDF.Common.Test.NUnit
{
    public class StringExtensionsTest
    {


        [Test]
        public void IsNullOrEmptyTest()
        {
            var res = "吃了没".IsNullOrEmpty();
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
            var res = "吃了没".IsNullOrWhiteSpace();
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
            var res = "吃了没".ToSnakeCase();
            Assert.AreEqual("吃了没", res);

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


        [Test]
        public void JoinTest()
        {
            var res = Enumerable.Range(0, 10).Select(i => i.ToString()).Join("");
            Assert.AreEqual("0123456789", res);

            res = Enumerable.Range(0, 10).Select(i => i.ToString()).Join(",");
            Assert.AreEqual("0,1,2,3,4,5,6,7,8,9", res);

            Assert.Throws<ArgumentNullException>(() => ((IEnumerable<string>)null).Join(","));
        }

    }
}
