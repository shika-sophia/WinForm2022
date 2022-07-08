/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT06_Control
 *@class MainButtonSetting.cs
 *@class FormButtonSetting.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/controls/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm06_Control.txt〕
 *           
 *@content KT 6. Control / Button
 *@subject ◆Buttonクラス System.Windows.Forms
 *         Button new Button()
 *         bool    button.Enabled
 *         string  button.Text
 *         int     button.TabIndex
 *         bool    button.UseVisualStyleBackColor	ビジュアルスタイルの背景色
 *         Cursors button.Cursor   Cursorsクラスのオブジェクト
 *              Cursors.Hand | No | Arrow など
 *              
 *         EventHandler button.Click
 *
 *subject 型キャスト (型) xxxx
 *        as演算子   xxxx as 型
 *        
 *@see FormButtonSetting.jpg
 *@author shika
 *@date 2022-07-01
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT06_Control
{
    class MainButtonSetting
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormButtonSetting());
        }//Main()
    }//class

    class FormButtonSetting : Form
    {
        private Label label;
        private Button[] buttonAry = new Button[3];
        
        public FormButtonSetting()
        {
            this.Text = "FormButtonSetting";

            label = new Label()
            {
                Location = new Point(10, 10),
            };
            this.Controls.Add(label);

            buttonAry[0] = new Button()
            {
                Text = "Button A",
                Location = new Point(10, 40),
                TabIndex = 0,
                UseVisualStyleBackColor = true,
            };

            buttonAry[1] = new Button()
            {
                Text = "Button B",
                Location = new Point(10, 70),
                TabIndex = 1,
                UseVisualStyleBackColor = true,
                Enabled = false,
            };

            buttonAry[2] = new Button()
            {
                Text = "Button C",
                Location = new Point(10, 100),
                TabIndex = 2,
                UseVisualStyleBackColor = true,
                Cursor = Cursors.Hand,
            };

            buttonAry[0].Click += new EventHandler(btn_Click);
            buttonAry[1].Click += new EventHandler(btn_Click);
            buttonAry[2].Click += new EventHandler(btn_Click);
            this.Controls.AddRange(buttonAry);

            this.BackColor = SystemColors.Window;
        }//constructor

        private void btn_Click(object sender, EventArgs e)
        {
            label.Text = (sender as Button).Text;
        }
    }//class
}
