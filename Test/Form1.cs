using HDF.Common.Windows;
using HDF.Framework.Common.WinForm;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            this.BindMovePositionControls();

        }



        protected override void WndProc(ref Message m)
        {


            base.WndProc(ref m);




        }


    }










}
