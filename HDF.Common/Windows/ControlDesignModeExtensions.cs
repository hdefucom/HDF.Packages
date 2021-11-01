using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace HDF.Common.Windows;

/// <summary>
/// 控件设计模式状态拓展
/// </summary>
public static class ControlDesignModeExtensions
{


    /// <summary>
    /// 判断该控件是否处于设计器模式
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static bool IsDesignMode(this Control c) => c.Site.DesignMode
        || LicenseManager.UsageMode == LicenseUsageMode.Designtime
        || Process.GetCurrentProcess().ProcessName == "devenv";





}

