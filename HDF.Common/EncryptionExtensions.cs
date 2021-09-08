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
        /// <returns>加密后的字符串</returns>
        public static string ToMD5(this string value)
        {
            if (value.IsNullOrEmpty())
                return string.Empty;

            using MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(value);//将要加密的字符串转换为字节数组
            byte[] encryptdata = md5.ComputeHash(bytes);//将字符串加密后也转换为字符数组
            return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
        }

    }

}
