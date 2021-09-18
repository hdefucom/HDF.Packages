using System;
using Xunit;

namespace HDF.Common.Test.xUnit
{
    public class EncryptionExtensionsTest
    {


        [Fact]
        public void ToMD5Test()
        {
            Assert.Equal("C4CA4238A0B923820DCC509A6F75849B", "1".ToMD5());
            Assert.Equal("c4ca4238a0b923820dcc509a6f75849b", "1".ToMD5(true, false));
            Assert.Equal("A0B923820DCC509A", "1".ToMD5(false));
            Assert.Equal("a0b923820dcc509a", "1".ToMD5(false, false));

            Assert.Equal("C81E728D9D4C2F636F067F89CC14862C", "2".ToMD5());
            Assert.Equal("c81e728d9d4c2f636f067f89cc14862c", "2".ToMD5(true, false));
            Assert.Equal("9D4C2F636F067F89", "2".ToMD5(false));
            Assert.Equal("9d4c2f636f067f89", "2".ToMD5(false, false));

            Assert.Equal(string.Empty, string.Empty.ToMD5());
            Assert.Equal(string.Empty, default(string).ToMD5());
        }
















    }
}
