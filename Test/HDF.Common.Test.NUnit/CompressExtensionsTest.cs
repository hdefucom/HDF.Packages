using NUnit.Framework;
using System;
using System.Linq;

namespace HDF.Common.Test.NUnit
{
    public class CompressExtensionsTest
    {


        [Test]
        public void GZipCompressStringTest()
        {
            //字符太短压缩后反而更大
            var str = "1111111111111111111111111111111111111111111111111111111111111111";
            var res = str.GZipCompressString();

            Assert.True(res.Length < str.Length);
            Assert.AreNotEqual(str, res);
            Assert.AreEqual(str, res.GZipDecompressString());

            Assert.AreEqual(string.Empty, string.Empty.GZipCompressString());
            Assert.AreEqual(string.Empty, default(string).GZipCompressString());

            Assert.AreEqual(string.Empty, string.Empty.GZipDecompressString());
            Assert.AreEqual(string.Empty, default(string).GZipDecompressString());

        }



        [Test]
        public void CompressTest()
        {
            var bytes = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
            var res = bytes.Compress();

            //Assert.True(res.Length < bytes.Length);

            res = res.Decompress();

            for (int i = 0; i < res.Length; i++)
                Assert.AreEqual(bytes[i], res[i]);

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
