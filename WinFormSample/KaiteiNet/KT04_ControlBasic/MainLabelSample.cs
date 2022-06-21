using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT04_ControlBasic
{
    class MainLabelSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormLabelSample());
        }//Main()
    }//class

    class FormLabelSample : Form
    {
        Label label = new Label();

        public FormLabelSample()
        {
            this.Text = "FormLabelSample";
            this.Location = new Point(10, 10);

            label.Text = "Hello Label";
            label.Location = new Point(20, 20);
            label.AutoSize = true;

            this.Controls.Add(label);
        }
    }//class
}
