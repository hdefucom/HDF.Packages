using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HDF.Common
{

    /// <summary>
    /// 字符拓展
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 指示指定的字符串是 null 还是<see cref="string.Empty"/>字符串。
        /// </summary>
        /// <param name="value">要测试的字符串</param>
        /// <returns>如果 value 参数为 null 或空字符串 ("")，则为 true；否则为 false。</returns>
        public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="value">要测试的字符串</param>
        /// <returns>如果 value 参数为 null 或空字符串 ("") 或空白字符，则为 true；否则为 false。</returns>
        public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);


        /// <summary>
        /// 将指定字符串从驼峰命名法(camel case)转换为蛇形命名法(snake case)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string? ToSnakeCase(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return Regex.Replace(value, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        /// <summary>
        /// 串联集合的字符，其中在每个字符之间使用指定的分隔符
        /// </summary>
        /// <param name="values">一个包含要串联的字符的集合</param>
        /// <param name="separator">分隔符</param>
        /// <returns>一个由 values 的元素组成的字符串，这些元素以 separator 字符串分隔</returns>
        /// <exception cref="ArgumentNullException"/>
        public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);


    }



}
