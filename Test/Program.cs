using HDF.Common;
using System;
using System.Windows.Forms;

namespace Test
{
    class Program
    {

        [STAThread]
        //[MTAThread]
        static void Main()
        {





            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Application.Run(new Form_SharpDX());




        }
    }
}
