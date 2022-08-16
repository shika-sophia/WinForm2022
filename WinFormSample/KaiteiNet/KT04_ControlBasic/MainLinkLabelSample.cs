/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT06_Control
 *@class MainLinkLabelSample.cs
 *@class FormLinkLabelSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/controls/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm06_Control.txt〕
 *           
 *@content KT 6. Control / LinkLabel, Process
 *@subject ◆LinkLabel : Label, IButtonControl -- System.Windows.Forms
 *         ・Label派生クラス, Button派生クラス
 *         
 *         LinkLabel new LinkLabel()
 *         bool      link.Visited  訪問済
 *         Color     link.LinkColor         リンクの色
 *         Color     link.ActiveLinkColor   アクティブ時のリンクの色
 *         Color     link.VisitedLinkColor  訪問後のリンクの色
 *         LinkLabel.LinkCollection link.Links  リンク項目のコレクション
 *         int       link.Links.Add(int start, int length, [object linkData]])
 *                     リンクラベルには複数リンクを登録可なので、第 1, 2 引数の指定が必要
 *         ＊イベント
 *         LinkLabelLinkClickedEventHandler link.LinkClicked	リンクのクリック
 *           └ delegate void LinkLabelLinkClickedEventHandler(
 *                object sender, LinkLabelLinkClickedEventArgs e);
 *                
 *         LinkLabelLinkClickedEventArgs クラス
 *         LinkLabel.Link e.Link             //Add()で追加した Linkオブジェクト
 *         object         e.Link.LinkClicked //Add()の第 3 引数 linkDataを取得
 *
 *@subject ◆Process -- System.Diagnostics
 *         ・Windows「ファイル名を指定して実行」([Windows] + [R]) を起動し実行
 *         ・「.exe」ファイルを指定するとアプリケーションを実行
 *         ・URLを指定すると Web接続
 *         
 *         Process Process.Start(string filename)
 *         Process Process.Start(ProcessStartInfo)
 *           └  ProcessStartInfo([string fileName, [string arguments]]);
 *                //Start()の内容を詳細に設定できる
 *         void    process.Close()  
 *                //関連リソースを閉じる。 try-finallyで利用すべき
 *         
 *         =>〔~/WinFormSample/ProcessReference.txt〕
 *         
 *@see FormLinkLabelSample.jpg
 *@author shika
 *@date 2022-07-05
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT06_Control
{
    class MainLinkLabelSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormLinkLabelSample());
        }//Main()
    }//class

    class FormLinkLabelSample : Form
    {
        private LinkLabel link;

        public FormLinkLabelSample()
        {
            this.Text = "FormLinkLabelSample";
            const string url = "http://kaitei.net/csforms/controls/";

            link = new LinkLabel()
            {
                Text = url,
                Location = new Point(10, 10),                
                AutoSize = true,
            };
            link.Links.Add(0, url.Length, url);
            link.LinkClicked +=
                new LinkLabelLinkClickedEventHandler(link_Clicked);

            this.BackColor = SystemColors.Window;
            this.Controls.Add(link);
        }//constructor

        private void link_Clicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            link.LinkVisited = true;

            Process process = null;
            try
            {
                process = Process.Start(e.Link.LinkData.ToString());
            }
            catch (Exception exc)
            {
                MessageBox.Show(
                    $"Exception:\n{exc.Message}",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }            
            finally
            {
                process.Close();
            }
        }//link_Clicked()
    }//class
}
