/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT10_CommonDialog
 *@class MainFontDialogSample.cs
 *@class   └ new FormFontDialogSample() : Form
 *@class       └ new FontDialog() : CommonDialog : Component
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/common-dialogs/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm10_CommonDialog.txt〕
 *           
 *@content KT10 CommonDialog / FontDialog | RR[276] p491 
 *@subject ◆FontDialog : CommonDialog
 *         FontDialog    new FontDialog()
 *         Font     fontDialog.Font           選択したフォント
 *         Color    fontDialog.Color          選択した色
 *         int      fontDialog.MaxSize        最大サイズ / 制限しない(デフォルト値): 0
 *         int      fontDialog.MinSize        最小サイズ / 制限しない(デフォルト値): 0
 *         bool     fontDialog.ShowApply      [適用]ボタンを表示するか / false
 *         bool     fontDialog.ShowColor      色の選択肢を表示するか   / false
 *         bool     fontDialog.ShowEffects    取消線, 下線, 色選択するか / true
 *         bool     fontDialog.ShowHelp       ヘルプボタンを表示するか / false
 *         bool     fontDialog.FontMustExist  存在しないフォント選択時に警告するか / false
 *         bool     fontDialog.FixedPitchOnly 固定ピッチ(=等幅)フォントのみか / false
 *         
 *         EventHandler  fontDialog.Apply     [適用]ボタンをクリック時イベント
 *         
 *         DialogResult  commonDialog.ShowDialog()  ダイアログを表示しボタン結果を返す
 *
 *@see ImageFontDialogSample.jpg
 *@see 
 *@author shika
 *@date 2022-08-08
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT10_CommonDialog
{
    class MainFontDialogSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormFontDialogSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormFontDialogSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormFontDialogSample : Form
    {
        private Label label;
        private Button button;

        public FormFontDialogSample()
        {
            this.Text = "FormFontDialogSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            label = new Label()
            {
                Text = "Sample サンプル 0OIl1.,:;",
                Location = new Point(20, 20),
                AutoSize = true,
            };

            button = new Button()
            {
                Text = "Change Font",
                Location = new Point(20, 50),
                UseVisualStyleBackColor = true,
                AutoSize = true,
            };
            button.Click += new EventHandler(button_Click);

            this.Controls.AddRange(new Control[]
            {
                label, button,
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog()
            {
                ShowColor = true,
                Font = label.Font,
                Color = label.ForeColor,
            };

            DialogResult result = fontDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                label.Font = fontDialog.Font;
                label.ForeColor = fontDialog.Color;
            }
        }//button_Click()
    }//class
}
