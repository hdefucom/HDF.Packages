using System;
using Xunit;

namespace HDF.Common.Test.xUnit
{
    public class StringExtensionsTest
    {


        [Fact]
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

        [Fact]
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

        [Fact]
        public void ToSnakeCaseTest()
        {
            var res = "吃了没".ToSnakeCase();
            Assert.Equal("吃了没", res);

            res = "HuangDeFu".ToSnakeCase();
            Assert.Equal("huang_de_fu", res);

            res = "HUANGDEFU".ToSnakeCase();
            Assert.Equal("huangdefu", res);

            res = "HuangDEFU".ToSnakeCase();
            Assert.Equal("huang_defu", res);


        }


    }
}
