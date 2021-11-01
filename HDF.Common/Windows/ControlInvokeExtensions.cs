using System;
using System.Windows.Forms;

namespace HDF.Common.Windows;

/// <summary>
/// 控件跨线程调用Invoke拓展
/// </summary>
public static class ControlInvokeExtensions
{

    /// <summary>
    /// 在拥有此控件的基础窗口句柄的线程上执行委托
    /// </summary>
    /// <param name="c"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static void Invoke(this Control c, Action action) => c.Invoke(action);


    /// <summary>
    /// 在拥有此控件的基础窗口句柄的线程上执行委托
    /// </summary>
    /// <param name="c"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static T Invoke<T>(this Control c, Func<T> func) => (T)c.Invoke(func);

    /// <summary>
    /// 在拥有此控件的基础窗口句柄的线程上执行委托
    /// </summary>
    /// <param name="c"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static void Invoke(this Control c, EventHandler handler) => c.Invoke(handler);





}

