using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HDF.Common;

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


    /// <summary>
    /// 将字符串中的一个或多个格式项替换为指定对象的字符串表示形式
    /// </summary>
    /// <remarks>
    /// string.Format的拓展，可实现实体类属性访问<br/>
    /// 普通Format用法：string.Format("name:{0}","hdf")<br/>
    /// 此拓展："name is {0:name},age is {0:age}".Format(new {name="hdf",age=22})
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="format"></param>
    /// <param name="arg"></param>
    /// <returns></returns>
    public static string FormatObject<T>(this string format, T arg)
    {
        format = Regex.Replace(format, @"\{(\w+)\}", "{0:$1}");
        return string.Format(new ObjectFormatProvider(), format, arg);
    }

}

/// <summary>
/// 字符格式化-->对象属性模式
/// </summary>
/// <remarks>
/// 正常用法 string.Format("name:{0}","hdf")<br/>
/// 此拓展用法 string.Format(new ObjectFormatProvider()，"name:{0:name},age{0:age}",new {name="hdf",age=22})<br/>
/// </remarks>
public class ObjectFormatProvider : IFormatProvider, ICustomFormatter
{
    /// <summary>
    /// IFormatProvider实现
    /// </summary>
    /// <param name="format"></param>
    /// <param name="arg"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    public virtual string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        if (format == null)
            throw new ArgumentNullException(nameof(format));
        return arg?.GetType()?.GetProperty(format)?.GetValue(arg, null)?.ToString() ?? "";
    }

    /// <summary>
    /// ICustomFormatter实现
    /// </summary>
    /// <param name="formatType"></param>
    /// <returns></returns>
    public object? GetFormat(Type? formatType) => formatType == typeof(ICustomFormatter) ? this : default;

}


