using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HDF.Common.Windows;

/// <summary>
/// 光标拓展
/// </summary>
public static class CaretExtensions
{
    /// <summary>
    /// 创建光标对象
    /// </summary>
    /// <param name="win"></param>
    /// <param name="bitmap">图片句柄</param>
    /// <param name="width">光标宽度</param>
    /// <param name="height">光标高度</param>
    /// <returns>操作是否成功</returns>
    public static bool CaretCreate(this IWin32Window win, int bitmap, int width, int height) => win != null && CreateCaret(win.Handle, bitmap, width, height);

    /// <summary>
    /// 设置光标位置
    /// </summary>
    /// <param name="win"></param>
    /// <param name="x">X坐标</param>
    /// <param name="y">Y坐标</param>
    /// <returns>操作是否成功</returns>
    public static bool CaretSetPos(this IWin32Window win, int x, int y) => win != null && SetCaretPos(x, y);

    /// <summary>
    /// 显示光标
    /// </summary>
    /// <returns>操作是否成功</returns>
    public static bool CaretShow(this IWin32Window win) => win != null && ShowCaret(win.Handle);

    /// <summary>
    /// 隐藏光标
    /// </summary>
    /// <returns>操作是否成功</returns>
    public static bool CaretHide(this IWin32Window win) => win != null && HideCaret(win.Handle);

    /// <summary>
    /// 删除光标
    /// </summary>
    /// <returns>操作是否成功</returns>
    public static bool CaretDestroy(this IWin32Window win) => win != null && DestroyCaret();


    #region Windows API

    [DllImport("user32.dll")]
    private static extern bool CreateCaret(IntPtr handle, int bitmap, int width, int height);

    [DllImport("user32.dll")]
    private static extern bool SetCaretPos(int x, int y);

    [DllImport("user32.dll")]
    private static extern bool DestroyCaret();

    [DllImport("user32.dll")]
    private static extern bool ShowCaret(IntPtr handle);

    [DllImport("user32.dll")]
    private static extern bool HideCaret(IntPtr handle);

    #endregion


}
