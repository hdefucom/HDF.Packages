using System;
using Xunit;

namespace HDF.Common.Test.xUnit
{
    public class StringExtensionsTest
    {


        [Fact]
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

        [Fact]
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

        [Fact]
        public void ToSnakeCaseTest()
        {
            var res = "����û".ToSnakeCase();
            Assert.Equal("����û", res);

            res = "HuangDeFu".ToSnakeCase();
            Assert.Equal("huang_de_fu", res);

            res = "HUANGDEFU".ToSnakeCase();
            Assert.Equal("huangdefu", res);

            res = "HuangDEFU".ToSnakeCase();
            Assert.Equal("huang_defu", res);


        }


    }
}
