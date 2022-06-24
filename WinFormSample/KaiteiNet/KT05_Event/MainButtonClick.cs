/**
 *@title WinFormGUI / WinFormSample / KaiteiNet / 
 *       KT05_Event / MainButtonClick.cs
 *@class FormButtonClick.cs
 *@see   MainFormClick.cs
 *@see   FormButtonClick.jpg
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT05_Event
{
    class MainButtonClick
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormButtonClick());
        }//Main()
    }//class

    class FormButtonClick : Form
    {
        private Label label = new Label();
        private Button button = new Button();
        private int count;

        public FormButtonClick()
        {
            this.Text = "FormButtonClick";

            //---- Label ----
            label.Text = "Please click the Button.";
            label.Location = new Point(10, 10);
            label.AutoSize = true;

            //---- Button ----
            button.Text = "Click";
            button.Location = new Point(10, 50);
            button.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            button.Size = new Size(160, 40);
            
            button.Click += new EventHandler(button_Click);

            //---- Add() ----
            this.Controls.Add(label);
            this.Controls.Add(button);
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            count++;
            label.Text = $"Clicked {count} times.";
        }
    }//class

}
