/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainWebBrowserSampla.cs
 *@class FormWebBrowserSampla.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[101] WebBrowser / p190
 *@subject ◆WebBrowser : WebBrowserBase
 *         WebBrowser  new WebBrowser()
 *         HttpDocument web.Document    読み込んだWebページの内容 / getのみ
 *         string web.DocumentText      HTMLテキスト
 *         string web.DocumentTitle     現在ページのタイトル / getのみ
 *         string web.DocumentType      現在ページのコンテンツ種類 / getのみ
 *         
 *         bool   web.ScriptErrorsSuppressed  スクリプトエラーを抑止するか / デフォルト false
 *                                            (trueにしておかないとエラー・ダイアログが連続で出される)
 *         bool   web.AllowNavigation   初期ページ以外のページに移動できるか
 *         bool   web.CanGoBack         ページ履歴に前ページがあるか / getのみ
 *         bool   web.CanGoForward      ページ履歴に次ページがあるか / getのみ
 *         Uri    web.Url               現在ページの URL (Uriオブジェクト)
 *         
 *         void   web.GoBack()          履歴がある場合に前ページに移動
 *         void   web.GoForward()       履歴がある場合に次ページに移動
 *         void   web.GoHome()          ホームページに移動
 *         void   web.GoSearch()        検索ページに移動
 *         void   web.Navigate(string url)  URLにWeb接続しページを表示
 *         void   web.Navigate(Uri url)     URLにWeb接続しページを表示
 *         void   control.Refresh()     コントロールの再描画を促す
 *         void   web.Stop()            保留中のナビゲーションをキャンセルし、
 *                                      BGMやアニメーションなどの動的なページ要素をすべて停止
 *         
 *         EventHandler web.FileDownload ファイルをダウンロード時のイベント
 *         WebBrowserDocumentCompletedEventHandler 
 *                web.DocumentCompleted ページの読込完了時のイベント
 *                
 *@subject ◆StatusStrip〔未述 StatusBar | RR[87] p166, KT11〕
 *         StatusStrip new StatusStrip()
 *         ToolStripStatusLabel new ToolStripStatusLabel()
 *         ToolStripStatusBar new ToolStripStatusBar()
 *         
 *         ToolStripItemCollection 
 *                     strip.Items(ToolStripItem)
 *         int         strip.Items.Add(ToolStripItem)
 *         string      striplabel.Text
 *         
 *@see FormWebBrowserSampla.jpg
 *@author shika
 *@date 2022-07-22
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainWebBrowserSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormWebBrowserSample());
        }//Main()
    }//class

    class FormWebBrowserSample : Form
    {
        private FlowLayoutPanel flow;
        private Label label;
        private TextBox textBox;
        private Button btnShow;
        private WebBrowser web;
        private StatusStrip strip;
        private ToolStripStatusLabel stripLabel;

        public FormWebBrowserSample()
        {
            this.Text = "FormWebBrowserSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            flow = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.LeftToRight,
                Size = new Size(600, 36),
                WrapContents = true,
            };

            label = new Label()
            {
                Text = "URL: ",
                Margin = new Padding(0, 10, 0, 0),
                AutoSize = true,
            };

            textBox = new TextBox()
            {
                Text = "http://kaitei.net/csforms/",
                Width = 250,
                Margin = new Padding(0, 5, 0, 0),
                Multiline = false,
            };

            btnShow= new Button()
            {
                Text = "Show",
                AutoSize = true,
            };
            btnShow.Click += new EventHandler(btnShow_Click);

            web = new WebBrowser()
            {
                Location = new Point(10, 100),
                Size = new Size(600, 400),
                ScriptErrorsSuppressed = true,
                Dock = DockStyle.Bottom,
            };
            web.DocumentCompleted +=
                new WebBrowserDocumentCompletedEventHandler(web_DocumentCompleted);

            strip = new StatusStrip();
            stripLabel = new ToolStripStatusLabel();
            strip.Items.Add(stripLabel);

            flow.Controls.AddRange(new Control[]
            {
                label, textBox, btnShow, 
            });

            this.Controls.Add(flow);
            this.Controls.Add(web);
            this.Controls.Add(strip);
        }//constructor

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                web.Navigate(textBox.Text);
            }
            catch (Exception exc)
            {
                stripLabel.Text = exc.Message;
            }
        }

        private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            stripLabel.Text = "Web Document Completed.";
        }

    }//class
}
