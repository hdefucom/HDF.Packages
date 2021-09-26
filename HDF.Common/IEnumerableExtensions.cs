using System;
using System.Collections.Generic;
using System.Linq;

namespace HDF.Common
{
    /// <summary>
    /// 枚举拓展
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 返回序列是否为Null或空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source) => !source?.Any() ?? true;
        //public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();
        //public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || source.Count() == 0;

        /// <summary>
        /// 对序列进行遍历操作，可用于引用类型，由于使用yield，最后不执行ToList等操作是无法获取结果的<br/>
        /// 此方法用于Linq链式便捷调用，很多情况下可使用<see cref="Enumerable.Select{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns>一个<see cref="IEnumerable{T}"/>，其元素为调用action后的结果</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action) where T : class
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// 遍历指定次数得到指定数量类型的序列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count">指定次数</param>
        /// <param name="func">每次循环执行的逻辑，参数为当前循环次数索引，从0开始</param>
        /// <returns>一个<see cref="IEnumerable{T}"/>，其元素为调用func后返回的结果</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> For<T>(this int count, Func<int, T> func)
        {
            if (count <= 0)
                yield break;

            if (func == null)
                throw new ArgumentNullException(nameof(func));

            for (var i = 0; i < count; i++)
            {
                yield return func(i);
            }
        }



        /// <summary>
        /// 获取序列中指定引用元素的下一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">目标集合</param>
        /// <param name="refele">当前元素</param>
        /// <returns>引用元素的下一个元素，如果引用元素为序列最后一个元素，则返回default</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T? Next<T>(this IEnumerable<T> source, T refele)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (refele == null)
                throw new ArgumentNullException(nameof(refele));

            IEnumerator<T> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current?.Equals(refele) ?? false)
                    return enumerator.MoveNext() ? enumerator.Current : default;
                continue;
            }
            throw new ArgumentOutOfRangeException(nameof(refele), "引用对象并非是序列中的元素");
        }

        /// <summary>
        /// 获取序列中指定引用元素的上一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">目标集合</param>
        /// <param name="refele">当前元素</param>
        /// <returns>引用元素的上一个元素，如果引用元素为序列第一个元素，则返回default</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T? Previous<T>(this IEnumerable<T> source, T refele)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (refele == null)
                throw new ArgumentNullException(nameof(refele));

            T? res = default;
            IEnumerator<T> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current?.Equals(refele) ?? false)
                    return res;
                res = enumerator.Current;
            }
            throw new ArgumentOutOfRangeException(nameof(refele), "引用对象并非是序列中的元素");
        }





    }

}
