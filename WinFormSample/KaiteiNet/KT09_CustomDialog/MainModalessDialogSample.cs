/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT09_CustomDialog
 *@class MainModalessDiaolgSample.cs
 *@class FormModalessDialogSample
 *@class ModalessDialogSample
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/dialogs/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm09_CustomDialog.txt〕
 *           
 *@content KT09 CustomDialog
 *@subject ModalessDialog : Form
 *         void  form.Show()    Modaless表示
 *         Form  form.Owner     親Formを指定。Modaless時は重要
 *                              親Formの Close()時、子Formも Close()
 *         
 * ※〔~/Reference/Article_KaiteiNet/WinForm09_CustomDialog.txt〕
 *    気を付けるべきことは，Owner プロパティに必ずオーナーウィンドウを設定するということです。
 *    Show メソッドはダイアログ表示用のものでは特にないので，特に設定しなければ，
 *    呼び出し元のフォームとダイアログとの間に主従の関係はありません。
 *    Owner プロパティを設定しないと，オーナーが閉じられてもダイアログだけ残ってしまう，
 *    というような変な挙動を許してしまうことになります。
 *    (モードレスダイアログを呼び出し元とするモードレスダイアログを自分で作ってみると，
 *     こうなりうることが確かめられます。)
 *     
 *@subject EventHandler as Lambda
 *         ・一度しか利用しない EventHandler Methodに名前を付ける必要はない
 *         ・private void xxxx_Click(object sender, EventArgs e)も記述不要
 *         
 *         EventHandler control.Click 
 *         control.Click += new EventHandler(xxxx_Click);
 *           └ delegate void EventHandler(object sender, EventArgs e)
 *               └ private void xxxx_Click(object sender, EventArgs e) { ... }
 *               ↓
 *         control.Click += new EventHandler((sender, e) => { ... });
 *
 *@subject オブジェクト初期化子
 *         ・ new Dialog1()する際に オブジェクト初期化子{  }で
 *            そのオブジェクトのプロパティを設定
 *         ・ Owner = this;だけは、thisに 元Formを入れるので、元Form内に記述。
 *         ・ Dialog1のコンストラクタをオブジェクト初期化子に入れることも可能だが、
 *            class Dialog1 { ... } の宣言は必要なので、コンストラクタを付して、
 *            プロパティを設定を外部化した。
 *         ・匿名クラスは、Formを継承して Show()するので、ここでは利用不可
 *         ・「var 変数名」は new Xxxxx().Show();とすれば名前を付ける必要はない
 *         
 *@see ImageFormModalessDialogSample.jpg
 *@author shika
 *@date 2022-07-31
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT09_CustomDialog
{
    class MainModalessDialogSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormModalessDialogSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormModalessDialogSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormModalessDialogSample : Form
    {
        private Button button;

        public FormModalessDialogSample()
        {
            this.Text = "FormModalessDialogSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            button = new Button()
            {
                Text = "Dialog",
                Location = new Point(100, 100),
                AutoSize = true,
            };
            button.Click += new EventHandler((sender, e) =>
            {
                Console.WriteLine("new ModalessDialogSample()");

                new ModalessDialogSample()
                {
                    Owner = this,
                }.Show();
            });

            this.Controls.AddRange(new Control[]
            {
                button,
            });
        }//constructor
    }//class

    class ModalessDialogSample : Form
    {
        public ModalessDialogSample()
        {
            Text = "ModalessDialogSample";
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
        }//constructor
    }//class
}
