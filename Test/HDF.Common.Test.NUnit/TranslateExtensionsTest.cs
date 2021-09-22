using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;

namespace HDF.Common.Test.NUnit
{
    public class TranslateExtensionsTest
    {


        [Test]
        public void BaiduTranslateTest()
        {
            TranslateExtensions.BaiduAppId = "20201030000603177";
            TranslateExtensions.BaiduKey = "NUVP7iiSaFmGSGPlbx7m";

            //必须休眠，否则会因百度api调用频繁限制
            Assert.NotNull("".BaiduTranslate());

            Thread.Sleep(1000);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<TranslateExtensions.BaiduTranslateApiResult>("他".BaiduTranslate());
            Assert.True(data.trans_result.Any(r => r.dst == "he"));

            Thread.Sleep(1000);
            data = Newtonsoft.Json.JsonConvert.DeserializeObject<TranslateExtensions.BaiduTranslateApiResult>("她".BaiduTranslate());
            Assert.True(data.trans_result.Any(r => r.dst == "she"));

            Thread.Sleep(1000);
            data = Newtonsoft.Json.JsonConvert.DeserializeObject<TranslateExtensions.BaiduTranslateApiResult>("它".BaiduTranslate());
            Assert.True(data.trans_result.Any(r => r.dst == "it"));

            Thread.Sleep(1000);
            Assert.Throws<ArgumentNullException>(() => default(string).BaiduTranslate());

            TranslateExtensions.BaiduAppId = null;
            Thread.Sleep(1000);
            Assert.Null("".BaiduTranslate());
            TranslateExtensions.BaiduAppId = "20201030000603177";
            TranslateExtensions.BaiduKey = null;
            Thread.Sleep(1000);
            Assert.Null("".BaiduTranslate());

        }



        [Test]
        public void BaiduLanguageTest()
        {
            TranslateExtensions.BaiduAppId = "20201030000603177";
            TranslateExtensions.BaiduKey = "NUVP7iiSaFmGSGPlbx7m";

            //必须休眠，否则会因百度api调用频繁限制
            Assert.NotNull("".BaiduLanguage());

            Thread.Sleep(1000);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<TranslateExtensions.BaiduLanguageApiResult>("他".BaiduLanguage());
            Assert.AreEqual("zh", data.data.src);

            Thread.Sleep(1000);
            data = Newtonsoft.Json.JsonConvert.DeserializeObject<TranslateExtensions.BaiduLanguageApiResult>("she".BaiduLanguage());
            Assert.AreEqual("en", data.data.src);

            Thread.Sleep(1000);
            Assert.Throws<ArgumentNullException>(() => default(string).BaiduLanguage());

            TranslateExtensions.BaiduAppId = null;
            Thread.Sleep(1000);
            Assert.Null("".BaiduLanguage());
            TranslateExtensions.BaiduAppId = "20201030000603177";
            TranslateExtensions.BaiduKey = null;
            Thread.Sleep(1000);
            Assert.Null("".BaiduLanguage());

        }

        [Test]
        public void DTOTest()
        {
            var tran = new TranslateExtensions.BaiduTranslateApiResult()
            {
                from = null,
                to = null,
                error_code = null,
                trans_result = null,
            };
            _ = tran.from;
            _ = tran.to;
            _ = tran.error_code;
            _ = tran.trans_result;

            var tranres = new TranslateExtensions.BaiduTranslateApiResult.TranslateResult()
            {
                src = null,
                dst = null,
            };
            _ = tranres.src;
            _ = tranres.dst;

            var lang = new TranslateExtensions.BaiduLanguageApiResult()
            {
                error_code = null,
                error_msg = null,
                data = null,
            };
            _ = lang.error_code;
            _ = lang.error_msg;
            _ = lang.data;

            var langres = new TranslateExtensions.BaiduLanguageApiResult.LanguageResult() { src = null };
            _ = langres.src;

        }







    }
}
