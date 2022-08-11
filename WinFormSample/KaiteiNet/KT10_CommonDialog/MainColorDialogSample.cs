/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT10_CommonDialog
 *@class MainColorDialogSample.cs
 *@class   └ new FormColorDialogSample() : Form
 *@class       └ new ColorDialog() : CommonDialog : Component
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/common-dialogs/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm10_CommonDialog.txt〕
 *           
 *@content KT10 CommonDialog / ColorDialog | RR[277] p493
 *@subject ◆ColorDialog : CommonDialog
 *         ColorDialog    new ColorDialog()
 *         Color    colorDialog.Color          選択色 struct Color
 *         int[]    colorDialog.CustomColors   カスタム色(= 自己作成した色)の配列
 *         bool     colorDialog.FullOpen       初期状態でカスタム色の作成コントロールも表示するか / デフォルト値: false
 *         bool     colorDialog.AllowFullOpen  カスタム色の作成コントロールを展開可能か / true
 *         bool     colorDialog.AnyColor       使用可能な基本色を全て表示するか        / false
 *         bool     colorDialog.ShowHelp       ヘルプボタンを表示するか               / false
 *         bool     colorDialog.SolidColorOnly 256色の純色のみ選択可能か (256色以下のシステムのみ適用) / false 
 
 *@subject abstract CommonDialog : Component
 *         CommonDialog     new CommonDialog();
 *         DialogResult     commonDialog.ShowDialog() ダイアログを表示し、ボタン結果を返す
 *         void             commonDialog.Reset()      派生クラスでoverride時、全てのプロパティを既定値にリセット
 *         
 *@see ImageColorDialogSample.jpg
 *@see 
 *@author shika
 *@date 2022-08-08
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT10_CommonDialog
{
    class MainColorDialogSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormColorDialogSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormColorDialogSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormColorDialogSample : Form
    {
        private Button button;

        public FormColorDialogSample()
        {
            this.Text = "FormColorDialogSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            button = new Button()
            {
                Text = "Select Color",
                Location = new Point(20, 20),
                UseVisualStyleBackColor = true,
            };
            button.Click += new EventHandler(button_Click);

            this.Controls.AddRange(new Control[]
            {
                button,
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog()
            {
                Color = this.BackColor,
                AnyColor = true,
            };

            DialogResult result = dialog.ShowDialog();

            if(result == DialogResult.OK)
            {
                this.BackColor = dialog.Color;
            }
        }//button_Click()
    }//class
}
