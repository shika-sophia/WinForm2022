using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT04_ControlBasic
{
    class MainPanelSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormPanelSample());
        }//Main()
    }//class

    class FormPanelSample : Form
    {
        Panel panel = new Panel();

        public FormPanelSample()
        {
            this.Text = "FormPanelSample";
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Location = new Point(10, 20);
            panel.Size = new Size(200, 200);

            Button[] buttonAry = new Button[3];
            for(int i = 0; i < buttonAry.Length; i++)
            {
                buttonAry[i] = new Button()
                {
                    Text = $"button{i}",
                    Location = new Point(10, i * 50),
                    AutoSize = true
                };

                panel.Controls.Add(buttonAry[i]);
            }//for

            this.Controls.Add(panel);
        }//constructor
    }//class
}

/*
NOTE【Bug?】
for文の条件式に「=」を入れるとプログラムが終了して
ちゃんと描画されない不具合あり。
                  ↓
for(int i = 0; i <= buttonAry.Length; i++)
*/