using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT04_ControlBasic
{
    class MainButtonSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormButtonSample());
        }//Main()
    }//class

    class FormButtonSample : Form
    {
        Button button = new Button();
        
        public FormButtonSample()
        {
            this.Text = "FormButtonSample";
            button.Text = "OK";
            button.Location = new Point(10, 10);
            button.AutoSize = true;

            this.Controls.Add(button);
        }
    }//class
}
