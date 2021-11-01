using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HDF.Common.Windows;

/// <summary>
/// 控件移动位置拓展
/// </summary>
public static class ControlMovePositionExtensions
{
    /// <summary>
    /// 绑定 控制 控件位置 的 控件集合，如果是自身，则<paramref name="controls"/>参数可为空或者包含<paramref name="control"/>
    /// </summary>
    /// <remarks>通过绑定鼠标事件实现</remarks>
    /// <param name="control">移动位置的目标控件，例如无边框的Form</param>
    /// <param name="controls">操控位置的控件集合，例如自定义的FormTItlePanel，如果为空或者传递<paramref name="control"/>则自身控制</param>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    public static void BindMovePositionControls(this Control control, params Control[] controls)
    {
        if (control == null)
            throw new ArgumentNullException(nameof(control));

        if (controls.IsNullOrEmpty())
            BindEvent(control, control);

        foreach (var c in controls)
        {
            var c1 = c;
            while (c1.Parent != control && c1 != control)
            {
                if (c1.Parent == null)
                    throw new ArgumentException("绑定的控件不是目标控件的子控件", nameof(controls));
                c1 = c1.Parent;
            }

            BindEvent(control, c);
        }
    }

    private readonly static Dictionary<Control, Point> dict = new();


    private static void BindEvent(Control target, Control c)
    {
        var control = c;

        c.MouseDown += (sender, e) =>
        {
                //鼠标按下时记录  点击坐标  相对 需要移动控件的客户区  的坐标
                var p = target.PointToClient(Control.MousePosition);
            dict[target] = new Point(-p.X, -p.Y); ;
        };

        c.MouseMove += (sender, e) =>
        {
            if (e.Button != MouseButtons.Left)
                return;
            Point p = Control.MousePosition;
                //鼠标移动时进行偏移 点击位置 相对 移动控件原点 的大小，
                p.Offset(dict[target]);
                //如果移动的是顶级窗体，直接设置屏幕坐标即可，如果是子控件，则把坐标转换成父控件的客户区坐标
                if (target.Parent != null)
                p = target.Parent.PointToClient(p);
            target.Location = p;
        };
    }






    private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
    private const int HTCLIENT = 0x1;//1鼠标位置为客户区
    private const int HTCAPTION = 0x2;//2鼠标位置为标题栏


    /// <summary>
    /// 设置鼠标拖动客户区移动控件位置
    /// </summary>
    /// <remarks>重写WndProc，调用后此方法后不可再调用base.WndProc()，否则无效</remarks>
    /// <param name="control"></param>
    /// <param name="m"></param>

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
    public static void SetDragPosition(this Control control, ref Message m)
    {
        if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)     // drag the form
            m.Result = (IntPtr)HTCAPTION;
    }





}



