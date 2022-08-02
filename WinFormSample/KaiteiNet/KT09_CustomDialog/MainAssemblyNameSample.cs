/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT09_CustomDialog
 *@class 
 *@class Form1
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/dialogs/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm09_CustomDialog.txt〕
 *           
 *@content KT09 CustomDialog / AssenblyName
 *
 *@subject アセンブリ情報
 *         VS [プロジェクト] -> [プロパティ] -> [アプリケーション]タブ
 *         -> [アセンブリ情報]クリック -> 編集 -> [OK]
 *         
 *         => 「~/Properties/AssemblyInfo.cs」内の [xxx]: attribute / 属性
 *         
 *@subject ◆AssemblyName -- System.Reflection.
 *         Assembly       Assembly.GetExecutingAssembly()  現在実行中のコードを含む Assembly
 *         AssemblyName   Assembly.GetName()
 *         string         assemblyName.Name
 *         string         assemblyName.FullName
 *         string         assemblyName.CultureName
 *         CultureInfo    assemblyName.CultureInfo
 *         Version        assemblyName.Version          Versionオブジェクト x.x.x.x
 *         int            assemblyName.Version.Major    Version 1桁目  メジャーバージョン
 *         int            assemblyName.Version.Miner    Version 2桁目  マイナーバージョン
 *         int            assemblyName.Version.build    Version 3桁目  ビルド番号
 *         int            assemblyName.Version.Revision	Version 4桁目  リビジョン番号
 *
 *         => Applicationクラス
 *           〔~\WinFromSample\ReverseReference\RR05_MenuToolStrip\MainStartPageSample.cs〕
 *           
 *@subject form.OnPaint(PaintEventArgs e)
 *         再描画イベントハンドラー
 *         
 *         ・PaintEventArgsクラス e
 *           Graphics  e.Graphics
 *           
 *@subject Graphicsによる Icon描画
 *         void   graphics.DrawIcon(Icon, int x, int y)  // (x, y)の位置に Iconを描画
 *
 *@see ImageAsseblyNameSample.jpg
 *@see ~\WinFromSample\ReverseReference\RR05_MenuToolStrip\MainStartPageSample.cs
 *@author shika
 *@date 2022-08-02
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT09_CustomDialog
{
    class MainAssemblyNameSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormAssemblyNameSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormAssemblyNameSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormAssemblyNameSample : Form
    {
        public FormAssemblyNameSample()
        {
            this.Text = "FormAssemblyNameSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            var button = new Button()
            {
                Text = "Version",
                Location = new Point(100, 100),
                AutoSize = true,
            };

            button.Click += new EventHandler((sender, e) =>
            {
                new DialogAssemblyNameSample().ShowDialog();
            });

            this.Controls.AddRange(new Control[]
            {
                button,
            });
        }//constructor
    }//class

    class DialogAssemblyNameSample : Form
    {
        private Label label;
        private LinkLabel link;
        private Button btnOk;
        private Icon icon;

        public DialogAssemblyNameSample()
        {
            //---- Dialog ----
            this.Text = "DialogAssemblyNameSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.AcceptButton = btnOk;

            //---- Icon ----
            icon = new Icon("../../Image/Icon/triColorIcon48px.ico", 48, 48);

            //---- AssemblyName ----
            AssemblyName assem = Assembly.GetExecutingAssembly().GetName();
            string version = 
                $"{assem.Version.Major}.{assem.Version.Minor}.{assem.Version.Build}";
            string project = 
                $"Project: {version}\nCopyright (c) 2022 Nagano";

            //---- Controls ----
            label = new Label()
            {
                Text = project,
                Location = new Point(90, 25),
                AutoSize = true,
                TextAlign = ContentAlignment.TopLeft,
            };

            string url = "http://kaitei.net/csforms/dialogs/";
            link = new LinkLabel()
            {
                Text = url,
                Location = new Point(90, 75),
                AutoSize = true,
                TextAlign = ContentAlignment.TopLeft,
            };
            link.Links.Add(0, url.Length, url);
            link.LinkClicked += new LinkLabelLinkClickedEventHandler(
                (sender, e) =>
                {
                    link.LinkVisited = true;
                    Process.Start(e.Link.LinkData.ToString());
                }
            );

            btnOk = new Button()
            {
                Text = "OK",
                Location = new Point(160, 200),
                DialogResult = DialogResult.OK,
            };

            this.Controls.AddRange(new Control[]
            {
                label, link, btnOk,
            });
        }//constructor

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawIcon(icon, 24, 24);
        }
    }//class
}
