using System;
using System.Security.Cryptography;
using System.Text;

namespace HDF.Common
{
    /// <summary>
    /// 加密拓展
    /// </summary>
    public static class EncryptionExtensions
    {
        /// <summary>
        /// 将传入的字符串以MD5的方式加密
        /// </summary>
        /// <param name="value">要加密的字符串</param>
        /// <param name="is32">是否返回32位，默认true，false则返回16位</param>
        /// <param name="isUpper">是否返回大写字符，默认true，false则返回小写字符</param>
        /// <returns>加密后的字符串</returns>
        public static string ToMD5(this string value, bool is32 = true, bool isUpper = true)
        {
            if (value.IsNullOrEmpty())
                return string.Empty;

            using MD5 md5 = MD5.Create();
            byte[] bytes = Encoding.Default.GetBytes(value);
            byte[] encryptdata = md5.ComputeHash(bytes);
            var res = is32 ? BitConverter.ToString(encryptdata) : BitConverter.ToString(encryptdata, 4, 8);
            res = res.Replace("-", "");
            return isUpper ? res : res.ToLower();
        }

    }


}
