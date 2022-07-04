/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT06_Control
 *@class MainRadioButtonSample.cs
 *@class FormRadioButtonSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/controls/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm06_Control.txt〕
 *           
 *@content KT 6.Control / RadioButton, GroupBox
 *@subject ◆RadioButton -- System.Windows.Forms
 *         ・CheckBoxと異なり，1 グループ内に必ず 1 つのボタンだけが選択された状態を取ります。
 *         ・そのため，各RadioButtonは，必ずどこかのGroupBoxに所属させます。
 *         bool         radio.Checked
 *         EventHandler radio.CheckedChanged
 *
 *@subject ◆GroupBox -- System.Windows.Forms
 *         ・GroupBoxに RadioButton[]を乗せ、Form/Panelには GroupBoxを乗せる
 *         string group.Text  GroupBoxのタイトル
 *         
 *@see FormRadioButtonSample_withGroupBox.jpg
 *@author shika
 *@date 2022-7-04
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT06_Control
{
    class MainRadioButtonSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormRadioButtonSample());
        }//Main()
    }//class

    class FormRadioButtonSample : Form
    {
        private Label label;
        private RadioButton[] radioAry;
        private GroupBox group;

        private readonly string[] itemAry = new string[]
        {
            "青巻紙", "赤巻紙", "黄巻紙"
        };
        
        public FormRadioButtonSample()
        {
            this.Text = "FormRadioButtonSample";

            label = new Label()
            {
                Location = new Point(20, 150),
                AutoSize = true,
            };

            radioAry = new RadioButton[itemAry.Length];
            for(int i = 0; i < itemAry.Length; i++)
            {
                radioAry[i] = new RadioButton()
                {
                    Text = itemAry[i],
                    Left = 20,
                    Top = i * 22 + 18,
                };

                radioAry[i].CheckedChanged +=
                    new EventHandler(radio_CheckedChanged);
            }//for
            radioAry[0].Checked = true;

            group = new GroupBox()
            {
                Text = "巻紙",
                Location = new Point(20, 20),
                Size = new Size(160, 100),
            };
            group.Controls.AddRange(radioAry);

            this.Controls.Add(label);
            this.Controls.Add(group);
        }//constructor

        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;

            if (radio.Checked)
            {
                label.Text = $"Selected: {radio.Text}";
            }
        }
    }//class
}
