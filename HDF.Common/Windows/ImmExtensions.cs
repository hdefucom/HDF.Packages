using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HDF.Common.Windows
{
    /// <summary>
    /// 输入法拓展
    /// </summary>
    public static class ImmExtensions
    {

        /// <summary>
        /// 判断指定的窗口中输入法是否打开
        /// </summary>
        /// <returns>输入法是否打开</returns>
        public static bool ImmIsOpen(this IWin32Window win)
        {
            int hImc = ImmGetContext(win.Handle);
            if (hImc == 0)
                return false;
            bool res = ImmGetOpenStatus(hImc);
            ImmReleaseContext(win.Handle, hImc);
            return res;
        }

        /// <summary>
        /// 为指定的窗口设置输入法的位置
        /// </summary>
        /// <param name="win"></param>
        /// <param name="x">输入法位置的X坐标</param>
        /// <param name="y">输入法位置的Y坐标</param>
        public static void ImmSetPos(this IWin32Window win, int x, int y)
        {
            int hImc = ImmGetContext(win.Handle);
            if (hImc != 0)
            {
                CompositionForm frm2 = new();
                frm2.CurrentPos.X = x;
                frm2.CurrentPos.Y = y;
                frm2.Style = (int)CandidateFormStyle.CFS_POINT;
                ImmSetCompositionWindow(hImc, ref frm2);
                //iError = Kernel32.GetLastError();
                Marshal.GetLastWin32Error();
                ImmReleaseContext(win.Handle, hImc);
            }
        }


        private const int IME_CMODE_FULLSHAPE = 0x8;
        private const int IME_CHOTKEY_SHAPE_TOGGLE = 0x11;
        /// <summary>
        /// 为指定的窗口打开输入法并设置为中文半角状态。
        /// </summary>
        /// <returns>操作是否成功。</returns>
        public static bool ImmSetHalf(this IWin32Window win)
        {
            int hIMC = ImmGetContext(win.Handle);
            if (hIMC == 0)
                return false;
            int iMode = 0;
            int iSentence = 0;
            bool isSuccess = ImmGetConversionStatus(hIMC, ref iMode, ref iSentence); //检索输入法信息
            if (isSuccess)
            {
                if ((iMode & IME_CMODE_FULLSHAPE) > 0) //如果处于全角状态
                {
                    _ = ImmSimulateHotKey(win.Handle, IME_CHOTKEY_SHAPE_TOGGLE); //转换成半角状态
                    return true;
                }
            }
            return false;

        }



        private static int IMode = int.MinValue;
        private static int ISentence = int.MinValue;


        /// <summary>
        /// 备份转换状态
        /// </summary>
        /// <returns>操作是否成功</returns>
        public static bool ImmBackConversionStatus(this IWin32Window win)
        {
            int hIMC = ImmGetContext(win.Handle);
            if (hIMC == 0)
            {
                ISentence = int.MinValue;
                IMode = int.MinValue;
                return false;
            }
            bool result = ImmGetConversionStatus(hIMC, ref IMode, ref ISentence);
            ImmReleaseContext(win.Handle, hIMC);
            return result;
        }

        /// <summary>
        /// 还原转换状态
        /// </summary>
        /// <returns>操作是否成功</returns>
        public static bool ImmRestoreConversionStatus(this IWin32Window win)
        {
            if (IMode == int.MinValue)
                return false;
            int hIMC = ImmGetContext(win.Handle);
            if (hIMC == 0)
                return false;

            int m = 0;
            int s = 0;
            ImmGetConversionStatus(hIMC, ref m, ref s);
            ImmSetConversionStatus(hIMC, IMode, ISentence);
            ImmReleaseContext(win.Handle, hIMC);
            return true;
        }


        /// <summary>
        /// 是否为更新输入法位置的消息
        /// </summary>
        public static bool Imm_IsWM_IME_NOTIFY_IMN_SETOPENSTATUS(this IWin32Window win, Message msg) => win != null && msg.Msg == 642 && msg.WParam.ToInt32() == 8;




        #region Data Struct

        private enum CandidateFormStyle
        {
            CFS_DEFAULT = 0x0000,
            CFS_RECT = 0x0001,
            CFS_POINT = 0x0002,
            CFS_FORCE_POSITION = 0x0020,
            CFS_CANDIDATEPOS = 0x0040,
            CFS_EXCLUDE = 0x0080
        }


        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        private struct CandidateForm
        {
#pragma warning disable 0649
            public int dwIndex;
            public int dwStyle;
            public Point ptCurrentPos;
            public RECT rcArea;
#pragma warning restore 0649
            //public Rectangle rcArea;
        }

        private struct CompositionForm
        {
            public int Style;
            public Point CurrentPos;
            public RECT Area;
            //public Rectangle Area;
        }

        #endregion


        #region Windows API

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        private static extern int ImmGetContext(IntPtr hwnd);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        private static extern int ImmCreateContext();

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        private static extern bool ImmDestroyContext(int hImc);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        private static extern bool ImmSetCandidateWindow(int hImc, ref CandidateForm frm);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        private static extern bool ImmSetStatusWindowPos(int hImc, ref Point pos);


        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        private static extern bool ImmReleaseContext(IntPtr hwnd, int hImc);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        private static extern bool ImmGetOpenStatus(int hImc);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        private static extern bool ImmSetCompositionWindow(int hImc, ref CompositionForm frm);


        /// <summary>
        /// 获取当前转换状态，用于判断中文半角或全角。
        /// </summary>
        [DllImport("Imm32.dll", CharSet = CharSet.Auto)]
        private static extern bool ImmGetConversionStatus(int hIMC, ref int iMode, ref int iSentence);
        /// <summary>
        /// 设置当前转换状态，用于设置中文半角或全角。
        /// </summary>
        [DllImport("Imm32.dll", CharSet = CharSet.Auto)]
        private static extern bool ImmSetConversionStatus(int hIMC, int iMode, int iSentence);
        /// <summary>
        /// 在指定的窗口中模拟一个特定的IME热键动作，以触发该窗口相应的响应动作。
        /// </summary>
        [DllImport("Imm32.dll", CharSet = CharSet.Auto)]
        private static extern int ImmSimulateHotKey(IntPtr hWnd, int lngHotkey);

        #endregion


    }
}