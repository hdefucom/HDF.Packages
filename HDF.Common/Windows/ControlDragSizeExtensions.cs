using System;
using System.Drawing;
using System.Windows.Forms;

namespace HDF.Common.Windows
{
    /// <summary>
    /// 控件拖拽大小拓展
    /// </summary>
    public static class ControlDragSizeExtensions
    {
        const int WM_NCHITTEST = 0x0084;
        const int HTLEFT = 10;      //左边界
        const int HTRIGHT = 11;     //右边界
        const int HTTOP = 12;       //上边界
        const int HTTOPLEFT = 13;   //左上角
        const int HTTOPRIGHT = 14;  //右上角
        const int HTBOTTOM = 15;    //下边界
        const int HTBOTTOMLEFT = 0x10;    //左下角
        const int HTBOTTOMRIGHT = 17;     //右下角



        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;


        /// <summary>
        /// 设置鼠标拖动边框修改控件大小
        /// </summary>
        /// <remarks>
        /// 重写WndProc，调用后此方法后不可再调用base.WndProc()，否则无效<br/>
        /// 如果设置isSetDragPostion为true，由于使用拦截WM_NCHITTEST消息返回HTCAPTION实现，所以会导致该组件的鼠标事件失效<br/>
        /// 可自己处理WM_NC开头的消息进行实现消息事件，此拓展后续可能会添加该功能;
        /// </remarks>
        /// <param name="control"></param>
        /// <param name="m"></param>
        /// <param name="isSetDragPostion">是否设置鼠标拖拽移动位置</param>
        public static void SetDragSize(this Control control, ref Message m, bool isSetDragPostion = false)
        {
            if (control is Form form && form.FormBorderStyle != FormBorderStyle.None)
                return;
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    //必须最后转成int16，多屏幕情况下坐标可能是负数，位运算得到结果是int16，如果使用int32，就无法使溢出的int16得到正确的坐标
                    var vPoint = new Point(
                        (short)((int)m.LParam & 0xFFFF),
                        (short)((int)m.LParam >> 16 & 0xFFFF)
                        );
                    vPoint = control.PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)HTTOPLEFT;
                        else if (vPoint.Y >= control.ClientSize.Height - 5)
                            m.Result = (IntPtr)HTBOTTOMLEFT;
                        else m.Result = (IntPtr)HTLEFT;
                    else if (vPoint.X >= control.ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)HTTOPRIGHT;
                        else if (vPoint.Y >= control.ClientSize.Height - 5)
                            m.Result = (IntPtr)HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)HTTOP;
                    else if (vPoint.Y >= control.ClientSize.Height - 5)
                        m.Result = (IntPtr)HTBOTTOM;
                    else if (isSetDragPostion && (int)m.Result == HTCLIENT)
                        m.Result = (IntPtr)HTCAPTION;
                    break;
            }

        }
    }
}
