/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR05_MenuToolStrip
 *@class MainNotifyIconSample.cs
 *@class FormNotifyIconSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[111] NotifyIcon / p207
 *         ・Windows下部のタスクバーにアイコンを表示するコントロール
 *         ・ContextMenuStrip, ToolTipも載せられる
 *         ・ToolStrip系ではないが、類似コントロールとして収録
 *
 *@subject Icon : MarshalByRefObject, ISerializable, ICloneable, IDisposable
 *         Icon   new Icon(string fileName)
 *         Icon   new Icon(Stream stream)
 *         Icon   new Icon(string fileName, Size size)
 *         Icon   new Icon(Icon original, Size size)
 *         Icon   new Icon(Type type, string resource)
 *         Icon   new Icon(Stream stream, Size size)
 *         Icon   new Icon(string fileName, int width, int height)
 *         Icon   new Icon(Icon original, int width, int height)
 *         Icon   new Icon(Stream stream, int width, int height)
 *         
 *         int    icon.Width    { get; }
 *         int    icon.Height   { get; }
 *         Size   icon.Size     { get; }
 *         
 *         Icon   static Icon.ExtractAssociatedIcon(string filename)
 *         void   icon.Save(Stream outputStream)
 *         Bitmap icon.ToBitmap()
 *         
 *@subject ◆NotifyIcon : Component
 *         NotifyIcon    new NotifyIcon();
 *         NotifyIcon    new NotifyIcon(IContainer container);
 *         
 *         bool             notify.Visible           アイコンを表示するか / デフォルト false
 *         Icon             notify.Icon             「.ico」アイコンファイルのみ
 *         string           notify.Text              ToolTipText
 *         
 *         ToolTipIcon      notify.BalloonTipIcon    バルーンToolTipに載せるアイコン
 *         string           notify.BalloonTipText    × ToolTipTextではない
 *         string           notify.BalloonTipTitle
 *         void             notify.ShowBalloonTip(
 *                               int timeout,
 *                              [string tipTitle,
 *                               string tipText,
 *                               ToolTipIcon tipIcon]);
 *                               
 *         ContextMenuStrip notify.ContextMenuStrip  (ToolStrip版)
 *         ContextMenu      notify.ContextMenu       (旧版)
 *         
 *         EventHandler     notify.Click
 *         EventHandler     notify.DoubleClick
 *         EventHandler     notify.BalloonTipClicked
 *
 *@NOTE【Exception】
 *      System.ArgumentException:
 *      引数 'picture' は Icon として使用できるピクチャ でなければなりません。
 *      
 *      Icon   new Icon(string filename, [int width, int height])
 *      は「.ico」アイコンファイルでないと受け付ていない様子。
 *      
 *      => Icon   static Icon.ExtractAssociatedIcon(string filename)
 *         画像ファイルをアイコン表現に変換する staticメソッドで解決
 *
 *@subject Deployment 配置
 *         new NotifyIcon()をすると、Windowsのタスクバーにアイコンが表示される。
 *         
 *         NotifyIcon : Componentのため
 *         × form.Controls.Add(Control) 利用不可。載せる必要はない。
 *                  
 *         new Form1()も タスクバーに現在開いているファイルとしてアイコンが表示される。
 *         Formは必要ないが ContextMenuStripを利用するなら、
 *         using System.Windows.Forms; は必要になる。
 *         
 *@see FormNotifyIconSample.jpg
 *@author shika
 *@date 2022-07-28
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR05_MenuToolStrip
{
    class MainNotifyIconSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormNotifyIconSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormNotifyIconSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormNotifyIconSample : Form
    {
        private ContextMenuStrip contextStrip;
        private NotifyIcon notify;

        public FormNotifyIconSample()
        {
            this.Text = "FormNotifyIconSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            //---- ContextMenuStrip ----
            var menuNew = new ToolStripMenuItem("New Form (&N)");
            var menuExit = new ToolStripMenuItem("Close (&X)");

            //---- Event ----
            // menuXxxx.Click += new EventHandler(xxxx_Click);

            contextStrip = new ContextMenuStrip();
            contextStrip.Items.AddRange(new ToolStripItem[]
            {
                menuNew, menuExit,
            });
            
            //---- NotifyIcon ----
            var icon = Icon.ExtractAssociatedIcon(
                "../../Image/Icon/windowIcon40px.jpg");
            
            notify = new NotifyIcon()
            { 
                Visible = true,
                Icon = new Icon(icon, 40, 40),
                Text = "WinForm",
                //BalloonTipText = "WinForm",
            };
            notify.ContextMenuStrip = contextStrip;

            //this.Controls.AddRange(new Control[]
            //{
                
            //});
        }//constructor
    }//class
}
