/*
 *@title WinFormSample / KaiteiNet / KT03_Form / MainFormSize.cs
 *@see   MainFormConstruct.cs
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT03_Form
{
    class MainFormSize
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.Run(new FormSizeSample());
        }//Main()
    }//class

    class FormSizeSample : Form
    {
        public FormSizeSample()
        {
            this.Text = "FormSizeSample";
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Size = new Size(400, 300);
            //this.ClientSize = new Size(100, 100);

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(200, 100);
            
        }
    }//class
}
