using HDF.Common.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HDF.Common.Test.Windows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);



        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x00A2)
            {
                System.Diagnostics.Debug.WriteLine("0x00A2");
            }
            else if (m.Msg == 0x00A1)
            {
                System.Diagnostics.Debug.WriteLine("0x00A1");

            }
            else if (m.Msg == 0x0201)
            {
                System.Diagnostics.Debug.WriteLine("0x0201");

            }
            this.SetDragSize(ref m, true);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);


        }

    }
}
