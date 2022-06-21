using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT04_ControlBasic
{
    class MainFormControls
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormControlsSample());
        }//Main()
    }//class

    class FormControlsSample : Form
    {
        public FormControlsSample()
        {
            this.Text = "FormControlsSample";

            Label label1 = new Label()
            {
                Text = "label1",
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label label2 = new Label()
            {
                Text = "label2",
                Location = new Point(10, 50),
                AutoSize = true
            };

            Button button1 = new Button()
            {
                Text = $"button1",
                Location = new Point(50, 10),
                AutoSize = true
            };

            Button button2 = new Button()
            {
                Text = $"button2",
                Location = new Point(50, 50),
                AutoSize = true
            };

            Control[] controlAry = new Control[]
            {
                label1, label2, button1, button2
            };

            this.Controls.AddRange(controlAry);
        }//FormControlsSample()
    }//class
}
