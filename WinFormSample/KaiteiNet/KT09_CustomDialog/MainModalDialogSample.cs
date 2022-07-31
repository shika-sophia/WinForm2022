/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT09_CustomDialog
 *@class MainModalDialogSample.cs
 *@class FormModalDialogSample : Form
 *@class ModalDialogSample : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/dialogs/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm09_CustomDialog.txt〕
 *           
 *@content KT09 CustomDialog 自己定義ダイアログ
 *         ・Modal:    ダイアログが表示されている間はオーナーウィンドウの操作が効かなくなる。
 *         ・Modaless: 上記の制限がない
 *         ・「Dialog」という Controlがあるわけではなく、
 *            既存の Formを継承した 新たな Formとして表示される。
 *         
 *@subject Form : ContainerControl
 *         Form          new Form()
 *         DialogResult  form.ShowDialog()  Modal表示
 *         void          form.Show()        Modaless表示
 *         bool          form.Modal         Modalか      { get; }
 *         
 *         string        form.Text          FormのTitle         
 *         bool          form.MaximizeBox   Show():       最大化ボタン"□"を表示するか
 *                                          ShowDialog(): VS「最大化ボタン"□"を表示するか」となっているが
 *                                                        false時も 表示。Click無効
 *         bool          form.MaximizeBox   Show():       最小化ボタン"＿"を表示するか
 *                                          ShowDialog(): VS「最小化ボタン"＿"を表示するか」となっているが
 *                                                        false時も 表示。Click有効(親Formを最小化)
 *                                          
 *         bool          form.ShowInTaskbar WindowsのTaskbarにアイコン表示するか
 *         FormBorderStyle    form.FormBorderStyle =>〔../../FormReference.txt〕
 *         FormStartPosition  form.StartPosition   =>〔../../FormReference.txt〕
 *
 *@result 実行結果
 *        新たな Dialog が表示される。
 *        Modalなので、この Dialog を閉じないと、
 *        他の動作は受け付ないことを確認する。
 *        
 *@see ImageModalDialogSample.jpg
 *@see ~/WinFromSample/FormReference.txt
 *@author shika
 *@date 2022-07-29
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT09_CustomDialog
{
    class MainModalDialogSample
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormModalDialogSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormModalDialogSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormModalDialogSample : Form
    {
        private Button button;

        public FormModalDialogSample()
        {
            this.Text = "FormModalDialogSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            button = new Button()
            {
                Text = "Dialog",
                Location = new Point(100, 100),
                AutoSize = true,
            };
            button.Click += new EventHandler(button_Click);

            this.Controls.AddRange(new Control[]
            {
                button,
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            var dialog = new ModalDialogSample();
            dialog.ShowDialog();
        }
    }//class

    class ModalDialogSample : Form
    {
        public ModalDialogSample()
        {
            this.Text = "ModalDialogSample";
            this.MaximizeBox = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label label = new Label() 
            { 
                Text = "(Dialog Label)",
                Location = new Point(100, 100),
                AutoSize = true,
            };

            this.Controls.Add(label);
        }//constructor
    }//class
}
