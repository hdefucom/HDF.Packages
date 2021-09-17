using System;
using System.Diagnostics;
using System.Reflection;

namespace HDF.Common
{
    /// <summary>
    /// 应用程序调用堆栈信息帮助类
    /// </summary>
    public sealed class StackTraceExtrensions
    {
        /// <summary>
        /// 检查调用本方法的方法是否发生了递归
        /// </summary>
        /// <remarks>本函数是利用应用程序调用堆栈来判断是否存在递归</remarks>
        /// <param name="skipFrames">需要跳过的栈帧数量</param>
        /// <returns>若发生了递归则返回true,否则返回false</returns>
        public static bool CheckRecursion(int skipFrames = 0)
        {
            if (skipFrames < 0)
                throw new ArgumentOutOfRangeException(nameof(skipFrames));

            StackTrace trace = new(skipFrames);
            // 若堆栈小于三层则不可能出现递归
            if (trace.FrameCount < 3)
                return false;
#if NETCOREAPP3_1_OR_GREATER
#pragma warning disable CS8602 // 解引用可能出现空引用。
#endif
            var mh = trace.GetFrame(1).GetMethod().MethodHandle.Value;
            for (int i = 2; i < trace.FrameCount; i++)
            {
                var m = trace.GetFrame(i).GetMethod().MethodHandle.Value;
                if (m == mh)
                    return true;
            }
#if NETCOREAPP3_1_OR_GREATER
#pragma warning restore CS8602 // 解引用可能出现空引用。
#endif
            return false;
        }

        /// <summary>
        /// 输出堆栈信息
        /// </summary>
        public static string OutputStackTrace() => new StackTrace(1).ToString();


    }

}
