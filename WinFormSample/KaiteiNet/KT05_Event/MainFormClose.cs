/*
 *@see MainFormClick.cs
 */

using System;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT05_Event
{
    class MainFormClose
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormCloseSample());
        }//Main()
    }//class

    class FormCloseSample : Form
    {
        public FormCloseSample()
        {
            this.Text = "FormCloseSample";
            this.FormClosing += new FormClosingEventHandler(form_Closing);
        }//constructor

        private void form_Closing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "終了しますか？",
                "Confirm Fininsh",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if(result == DialogResult.OK)
            {
                this.FormClosing -= this.form_Closing;
                this.Close();
            }
            else if(result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }//form_Closing()
    }//class
}
