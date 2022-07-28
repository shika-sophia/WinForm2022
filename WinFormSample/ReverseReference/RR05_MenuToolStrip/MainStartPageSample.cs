/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR05_MenuToolStrip
 *@class MainStartPageSample.cs
 *@class FormStartPageSample.cs
 *@class FormContentPageSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[52] Splash Window (= Start Page) / p105
 *         ・本来のコンテンツの前に、Applicationの最初だけ表示するページ
 *         ・同じ Formクラスを利用
 *         ・form.ShowDialog()  モーダル表示をする
 *         ・form.Show()        モーダレス表示だと、先に ContentPageが表示される
 *         
 *         おそらく、Show()は、制御がすぐに MainThreadに戻ってしまうのかも
 *
 *@subject Applicaton情報
 *         ・[プロジェクト] -> [プロパティ] -> [アプリケーション]
 *           -> [アセンブリ情報] クリック -> 編集 -> [OK]
 *           
 *         => 「~/Properties/AssemblyInfo.cs」に記録されている
 *         
 *@subject ◆Application
 *         bool        Application.EnableVisualStyles();
 *         string      Application.ProductName     製品名          { get; }
 *         string      Application.ProductVersion  Version情報     { get; }
 *         string      Application.CompanyName     会社名          { get; }
 *         FormCollection Application.OpenForms    実行時に開くFormのコレクション { get; }
 *         CultureInfo    Application.CurrentCulture  カルチャー情報
 *         
 *         void  Application.Run()                   実行
 *         void  Application.Run(Form)               Formを実行
 *         void  Application.Run(ApplicationContext) 
 *         void  Application.Restart()               Applicationをシャットダウン後、再実行
 *         void  Application.Exit()                  すべて終了
 *         void  Application.Exit(CancelEventArgs e) すべて終了、終了をキャンセルしたかを eで取得
 *         void  Application.ExitThread();           現在Threadのみ終了
 *
 *@subject MultiThread
 *         ・Thread.Priopity(=優先度)を設定しても、
 *           new FormContentPageSample()が先に処理される。
 *           おそらく、new Thread(), new FormStartPageSample()の起動に
 *           オーバーヘッド(=処理負荷)が掛かるからだと思われる。
 *         ・理想は StartPageの表示中に、ContentPageの初期化処理を行うことだが、
 *           ここはダミーなので、ContentPageのほうが軽いからかも。
 *         ・StartPageは、重いContentPageを起動する際に利用する機能。
 *
 *@result  実行結果
 *         new FormContentPageSample()
 *         new FormStartPageSample()
 *         ---- StartPage 表示 ----
 *                    :   3 seconds later
 *         FormStartPageSample Close()
 *         ---- ContentPage 表示 ----
 *                    :   [×] Click to Close()
 *         FormContentPageSample Close()
 *         
 *@see FormStartPageSample.jpg
 *@author shika
 *@date 2022-07-28
 */
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR05_MenuToolStrip
{
    class MainStartPageSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            
            Thread th = new Thread(ShowStartPage);
            th.Priority = ThreadPriority.Highest;
            th.Start();

            Thread.CurrentThread.Priority = ThreadPriority.Lowest;
            Console.WriteLine("new FormContentPageSample()");
            Form form = new FormContentPageSample();

            th.Join();
            Application.Run(form);
            Console.WriteLine("FormContentPageSample Close()");
        }//Main()

        private static void ShowStartPage()
        {
            Console.WriteLine("new FormStartPageSample()");
            new FormStartPageSample().ShowDialog();
            Console.WriteLine("FormStartPageSample Close()");
        }
    }//class

    class FormStartPageSample : Form
    {
        private Label label1;
        private Label label2;
        private Label label3;
        private System.Windows.Forms.Timer timer;

        public FormStartPageSample()
        {
            this.Text = "FormStartPageSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ControlBox = false;
            this.AutoSize = true;

            //---- Label ----
            label1 = new Label()
            {
                Location = new Point(10, 10),
                Text = $"Name: {Application.ProductName}",
                AutoSize = true,
            };

            label2 = new Label()
            {
                Location = new Point(10, 30),
                Text = $"Prodction: {Application.CompanyName}",
                AutoSize = true,
            };

            label3 = new Label()
            {
                Location = new Point(10, 50),
                Text = $"Version: {Application.ProductVersion}",
                AutoSize = true,
            };

            //---- Timer ----
            timer = new System.Windows.Forms.Timer()
            {
                Interval = 3000,
            };
            timer.Start();
            timer.Tick += new EventHandler(timer_Tick);

            //---- Form ----
            this.Controls.AddRange(new Control[]
            {
                label1, label2, label3,
            });
        }//constructor

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            this.Close();
            Application.ExitThread();
        }
    }//class

    class FormContentPageSample : Form
    {
        private Label labelContent;

        public FormContentPageSample()
        {
            this.Text = "FormContentPageSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            labelContent = new Label()
            {
                Text = "(Main Content)",
                Location = new Point(30, 100),
                AutoSize = true,
            };

            this.Controls.AddRange(new Control[]
            {
                labelContent,
            });
        }//constructor
    }//class
}
