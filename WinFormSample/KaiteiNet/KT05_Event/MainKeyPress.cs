/*
 *@see MainKeyEvent.cs 
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT05_Event
{
    class MainKeyPress
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormKeyPress());
        }//Main()
    }//class

    class FormKeyPress : Form
    {
        private Label label;

        public FormKeyPress()
        {
            this.Text = "FormKeyPress";
            label = new Label()
            {
                Text = "Hit any key.\n>",
                Location = new Point(10, 10),
                AutoSize = true
            };
            this.Controls.Add(label);

            this.KeyPress += new KeyPressEventHandler(form_KeyPress);
        }//constructor

        private void form_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                    label.Text += "\n";
                    break;

                case (char)Keys.Back:
                    label.Text = label.Text.Substring(0, label.Text.Length - 1);
                    break;

                default:
                    label.Text += e.KeyChar;
                    break;
            }//switch
        }//form_KeyPress
    }//class

}
