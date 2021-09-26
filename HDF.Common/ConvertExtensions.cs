using System;
using System.Globalization;
using System.Reflection;

namespace HDF.Common
{
    /// <summary>
    /// 转换拓展
    /// </summary>
    public static class ConvertExtensions
    {
        /// <summary>
        /// 字符串转换任意基础类型<br/>
        /// 转换为<see cref="Enum"/>请使用<see cref="ToEnum{T}(string)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T To<T>(this string val) where T : IConvertible => (T)((IConvertible)val).ToType(typeof(T), CultureInfo.CurrentCulture);


        /// <summary>
        /// 将对象转换为目标类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="val">转换对象</param>
        /// <returns>转换后的值，可能为null</returns>
        public static T? As<T>(this object val) where T : class => val as T;

        /// <summary>
        /// 将字符转换为枚举
        /// </summary>
        /// <typeparam name="T">指定的枚举类型</typeparam>
        /// <param name="val">转换的值</param>
        /// <returns>抓换后的枚举值</returns>
        public static T ToEnum<T>(this string val) => (T)Enum.Parse(typeof(T), val);

        /// <summary>
        /// 检索应用于类型成员的自定义特性
        /// </summary>
        /// <typeparam name="T">指定类型的Attribute</typeparam>
        /// <param name="info">成员信息对象</param>
        /// <param name="inherit">是否检索父类的成员</param>
        /// <returns>一个特性对象，如果未检索到则返回null</returns>
        public static T? GetCustomAttribute<T>(this MemberInfo info, bool inherit = false) where T : Attribute => Attribute.GetCustomAttribute(info, typeof(T), inherit) as T;




















    }
}
