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
        /// <returns>若发生了递归则返回true,否则返回false</returns>
        public static bool CheckRecursion()
        {
            try
            {
                StackTrace trace = new();
                // 若堆栈小于三层则不可能出现递归
                if (trace.FrameCount < 3)
                    return false;
                var mh = trace.GetFrame(1)?.GetMethod()?.MethodHandle.Value;
                if (mh == null)
                    return false;
                for (int i = 2; i < trace.FrameCount; i++)
                {
                    var m = trace.GetFrame(i)?.GetMethod();
                    if (m?.MethodHandle.Value == mh)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void OutputStackTrace()
        {
            StackTrace myTrace = new();
            Console.WriteLine(myTrace.ToString());
        }

        /// <summary>
        /// 本对象不能实例化
        /// </summary>
        private StackTraceExtrensions()
        {
        }
    }

}
