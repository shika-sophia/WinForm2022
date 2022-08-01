/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT09_CustomDialog
 *@class MainCustomMessageBoxSample.cs
 *@class FormCustomMessageBoxSample
 *@class CustomMessageBoxSample
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/dialogs/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm09_CustomDialog.txt〕
 *           
 *@content KT09 CustomDialog / MessageBox
 *         MessageBoxを Formで再現
 *         
 *@subject Form
 *         DialogResult  form.ShowDialog()     Modal表示
 *         DialogResult  button.DialogResult   ボタンClick時、自動的にDialogを閉じる
 *           |                                 (button.Clickイベントを内包しているので記述不要)
 *           └ enum DialogResult
 *             {
 *                 None = 0,    // 戻り値 Nothing / Modalの実行が継続。
 *                 OK = 1,      // [OK]
 *                 Cancel = 2,  // [キャンセル]
 *                 Abort = 3,   // [中止]
 *                 Retry = 4,   // [再試行]
 *                 Ignore = 5,  // [無視]
 *                 Yes = 6,     // [はい]
 *                 No = 7       // [いいえ]
 *             }
 *             
 *@subject デフォルトKey 設定
 *         IButtonControl  form.AccessButton  [Enter]時 Clickするボタンを指定
 *         IButtonControl  form.CancelButton  [Esc]時 Clickするボタンを指定
 *         
 *@NOTE    [Esc]時が機能していない
 *
 *@see ImageCustomMessageBoxSample.jpg
 *@see ~/WinFormSample/FormReference.txt
 *@author shika
 *@date 2022-08-01
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT09_CustomDialog
{
    class MainCustomMessageBoxSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormCustomMessageBoxSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormCustomMessageBoxSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormCustomMessageBoxSample : Form
    {
        public FormCustomMessageBoxSample()
        {
            this.FormClosing += new FormClosingEventHandler((sender, e) => 
            {
                DialogResult result = 
                    new CustomMessageBoxSample().ShowDialog();

                if(result != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            });
        }//constructor
    }//class

    class CustomMessageBoxSample : Form
    {
        private Label label;
        private Button btnYes;
        private Button btnNo;

        public CustomMessageBoxSample()
        {
            this.Text = "CustomMessageBoxSample";
            this.Font = new Font("メイリオ", 12, FontStyle.Regular);
            this.AutoSize = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.AcceptButton = btnYes;
            this.CancelButton = btnNo;

            label = new Label()
            {
                Text = "Closing OK?",
                Location = new Point(40, 10),
                AutoSize = true,
            };

            btnYes = new Button()
            {
                Text = "&YES (Y)",
                Location = new Point(40, 60),
                DialogResult = DialogResult.Yes,
                AutoSize = true,
            };

            btnNo = new Button()
            {
                Text = "&NO (N)",
                Location = new Point(40, 120),
                DialogResult = DialogResult.No,
                AutoSize = true,
            };

            this.Controls.AddRange(new Control[]
            {
                label, btnYes, btnNo,
            });
        }//constructor
    }//class
}
