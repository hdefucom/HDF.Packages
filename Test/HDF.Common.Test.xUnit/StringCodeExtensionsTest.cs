using System;
using Xunit;

namespace HDF.Common.Test.xUnit
{
    public class StringCodeExtensionsTest
    {


        [Fact]
        public void GetPYCodeTest()
        {
            var res = "吃了没".GetPYCode();
            Assert.Equal("CLM", res);

            res = "你好啊！".GetPYCode();
            Assert.Equal("NHA", res);

            res = "你!好啊！".GetPYCode();
            Assert.Equal("NHA", res);

            res = "asd你!好啊！".GetPYCode();
            Assert.Equal("ASDNHA", res);

            res = "as2d你!好啊！".GetPYCode();
            Assert.Equal("AS2DNHA", res);

            res = string.Empty.GetPYCode();
            Assert.Equal(string.Empty, res);

            res = ((string)null).GetPYCode();
            Assert.Equal(string.Empty, res);

            res = "abc".GetPYCode();
            Assert.Equal("ABC", res);


            res = "ABC".GetPYCode();
            Assert.Equal("ABC", res);


            res = "123".GetPYCode();
            Assert.Equal("123", res);

            res = "石头".GetPYCode();
            Assert.Equal("ST", res);

            //多音字返回的拼音首字母是比较常用的音节，如下测试就不能通过
            //res = "石头".GetPYCode();
            //Assert.Equal("DT", res);
        }

        [Fact]
        public void GetWBCodeTest()
        {
            var res = "吃了没".GetWBCode();
            Assert.Equal("KBI", res);

            res = "你好啊！".GetWBCode();
            Assert.Equal("WVK", res);

            res = "你!好啊！".GetWBCode();
            Assert.Equal("WVK", res);

            res = "asd你!好啊！".GetWBCode();
            Assert.Equal("ASDWVK", res);

            res = "as2d你!好啊！".GetWBCode();
            Assert.Equal("AS2DWVK", res);

            res = "石头".GetWBCode();
            Assert.Equal("DU", res);
        }


    }
}
