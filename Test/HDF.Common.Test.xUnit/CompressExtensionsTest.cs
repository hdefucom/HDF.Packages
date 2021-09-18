using System;
using System.Collections;
using System.Linq;
using Xunit;

namespace HDF.Common.Test.xUnit
{
    public class CompressExtensionsTest
    {


        [Fact]
        public void GZipCompressStringTest()
        {
            //字符太短压缩后反而更大
            var str = "1111111111111111111111111111111111111111111111111111111111111111";
            var res = str.GZipCompressString();

            Assert.True(res.Length < str.Length);
            Assert.NotEqual(str, res);
            Assert.Equal(str, res.GZipDecompressString());

            Assert.Equal(string.Empty, string.Empty.GZipCompressString());
            Assert.Equal(string.Empty, default(string).GZipCompressString());

            Assert.Equal(string.Empty, string.Empty.GZipDecompressString());
            Assert.Equal(string.Empty, default(string).GZipDecompressString());

        }



        [Fact]
        public void CompressTest()
        {
            var bytes = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
            var res = bytes.Compress();

            //Assert.True(res.Length < bytes.Length);

            res = res.Decompress();

            for (int i = 0; i < res.Length; i++)
                Assert.Equal(bytes[i], res[i]);

            Assert.Throws<ArgumentNullException>(() => ((byte[])null).Compress());
            Assert.Throws<ArgumentNullException>(() => ((byte[])null).Decompress());

#if NET5_0
            Assert.Throws<ArgumentOutOfRangeException>(() => Array.Empty<byte>().Compress());
            Assert.Throws<ArgumentOutOfRangeException>(() => Array.Empty<byte>().Decompress());
#else
            Assert.Throws<ArgumentOutOfRangeException>(() => new byte[0].Compress());
            Assert.Throws<ArgumentOutOfRangeException>(() => new byte[0].Decompress());
#endif
        }
















    }
}
