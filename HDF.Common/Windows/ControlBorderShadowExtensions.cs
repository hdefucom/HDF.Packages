using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HDF.Common.Windows
{
    /// <summary>
    /// 控件边框阴影拓展
    /// </summary>
    public static class ControlBorderShadowExtensions
    {
        #region WindowsAPI


        [EditorBrowsable(EditorBrowsableState.Never)]
        private struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        [DllImport("dwmapi.dll")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);


        #endregion

        /// <summary>
        /// 设置控件边框阴影
        /// </summary>
        /// <param name="control"></param>
        /// <exception cref="ArgumentNullException"/>
        public static void SetBorderShadows(this Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (control is Form f && f.FormBorderStyle != FormBorderStyle.None)
                return;

            var v = 2;

            _ = DwmSetWindowAttribute(control.Handle, 2, ref v, 4);

            var margins = new MARGINS()
            {
                bottomHeight = 1,
                leftWidth = 0,
                rightWidth = 0,
                topHeight = 0
            };

            _ = DwmExtendFrameIntoClientArea(control.Handle, ref margins);
        }


    }


    //class DemoForm : Form
    //{
    //    private DemoForm()
    //    {
    //        //InitializeComponent();
    //    }
    //    [DllImport("dwmapi.dll")]
    //    private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
    //    [DllImport("dwmapi.dll")]
    //    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
    //    [DllImport("dwmapi.dll")]
    //    private static extern int DwmIsCompositionEnabled(ref int pfEnabled);
    //    private bool m_aeroEnabled = false;                     // variables for box shadow
    //    private const int CS_DROPSHADOW = 0x00020000;
    //    private const int WM_NCPAINT = 0x0085;
    //    public struct MARGINS                           // struct for box shadow
    //    {
    //        public int leftWidth;
    //        public int rightWidth;
    //        public int topHeight;
    //        public int bottomHeight;
    //    }
    //    protected override CreateParams CreateParams
    //    {
    //        get
    //        {
    //            m_aeroEnabled = CheckAeroEnabled();
    //            CreateParams cp = base.CreateParams;
    //            if (!m_aeroEnabled)
    //                cp.ClassStyle |= CS_DROPSHADOW;
    //            return cp;
    //        }
    //    }
    //    private bool CheckAeroEnabled()
    //    {
    //        if (Environment.OSVersion.Version.Major >= 6)
    //        {
    //            int enabled = 0;
    //            DwmIsCompositionEnabled(ref enabled);
    //            return (enabled == 1) ? true : false;
    //        }
    //        return false;
    //    }
    //    private void SetShadow()
    //    {
    //        if (m_aeroEnabled)
    //        {
    //            var v = 2;
    //            DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
    //            var margins = new MARGINS()
    //            {
    //                bottomHeight = 1,
    //                leftWidth = 1,
    //                rightWidth = 1,
    //                topHeight = 1
    //            };
    //            DwmExtendFrameIntoClientArea(this.Handle, ref margins);
    //        }
    //    }
    //    protected override void WndProc(ref Message m)
    //    {
    //        switch (m.Msg)
    //        {
    //            case WM_NCPAINT:                        // box shadow
    //                SetShadow();
    //                break;
    //            default:
    //                break;
    //        }
    //        base.WndProc(ref m);
    //    }
    //}


}