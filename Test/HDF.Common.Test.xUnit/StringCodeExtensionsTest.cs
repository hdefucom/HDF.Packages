using System;
using Xunit;

namespace HDF.Common.Test.xUnit
{
    public class StringCodeExtensionsTest
    {


        [Fact]
        public void GetPYCodeTest()
        {
            var res = "����û".GetPYCode();
            Assert.Equal("CLM", res);

            res = "��ð���".GetPYCode();
            Assert.Equal("NHA", res);

            res = "��!�ð���".GetPYCode();
            Assert.Equal("NHA", res);

            res = "asd��!�ð���".GetPYCode();
            Assert.Equal("ASDNHA", res);

            res = "as2d��!�ð���".GetPYCode();
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

            res = "ʯͷ".GetPYCode();
            Assert.Equal("ST", res);

            //�����ַ��ص�ƴ������ĸ�ǱȽϳ��õ����ڣ����²��ԾͲ���ͨ��
            //res = "ʯͷ".GetPYCode();
            //Assert.Equal("DT", res);
        }

        [Fact]
        public void GetWBCodeTest()
        {
            var res = "����û".GetWBCode();
            Assert.Equal("KBI", res);

            res = "��ð���".GetWBCode();
            Assert.Equal("WVK", res);

            res = "��!�ð���".GetWBCode();
            Assert.Equal("WVK", res);

            res = "asd��!�ð���".GetWBCode();
            Assert.Equal("ASDWVK", res);

            res = "as2d��!�ð���".GetWBCode();
            Assert.Equal("AS2DWVK", res);

            res = "ʯͷ".GetWBCode();
            Assert.Equal("DU", res);
        }


    }
}
