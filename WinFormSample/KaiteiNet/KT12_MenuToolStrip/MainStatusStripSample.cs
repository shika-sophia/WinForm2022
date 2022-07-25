/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT12_MenuToolStrip
 *@class MainStatusStripSample.cs
 *@class FormStatusStripSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menu-strips/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm12_MenuToolStrip.txt〕
 *           
 *@content KT12 MenuToolStrip / StatusStrip
 *@subject ◆StatusStrip : ToolStrip
 *         StatusStrip();
 *         ToolStripItemCollection    toolStrip.Items
 *                                      :  Add(), AddRange() 他 IList, ICollection系 メソッド
 *
 *@subject ◆ToolStripLabel : ToolStripItem
 *         ToolStripLabel   new ToolStripLabel()
 *         ToolStripLabel   new ToolStripLabel(string text);
 *         ToolStripLabel   new ToolStripLabel(Image image);
 *         ToolStripLabel   new ToolStripLabel(string text, Image);
 *         ToolStripLabel   new ToolStripLabel(string text, Image, bool isLink);
 *         ToolStripLabel   new ToolStripLabel(string text, Image, bool isLink, EventHandler onClick);
 *         
 *         LinkBehavior     toolStripLabel.LinkBehavior
 *           └ enum LinkBehavior
 *             {
 *                 SystemDefault = 0,  //コントロールパネル[インターネット オプション] で設定
 *                 AlwaysUnderline = 1,//リンクは常に下線付きテキストで表示
 *                 HoverUnderline = 2, //マウスポイント時だけ、リンクが下線付きテキストで表示
 *                 NeverUnderline = 3　//リンクに下線なし。LinkColorで識別可
 *             }
 *         (他 LinkLabel系 メンバー)
 *         
 *@subject ◆ToolStripStatusLabel : ToolStripLabel
 *         ToolStripStatusLabel   new ToolStripStatusLabel()
 *         ToolStripStatusLabel   new ToolStripStatusLabel(string text);
 *         ToolStripStatusLabel   new ToolStripStatusLabel(Image image);
 *         ToolStripStatusLabel   new ToolStripStatusLabel(string text, Image image);
 *         ToolStripStatusLabel   new ToolStripStatusLabel(string text, Image, EventHandler onClick);
 *         
 *         bool toolStripStatusLabel.Spring   自動的に利用できる領域いっぱいに表示するか / デフォルト false
 *         ToolStripItemAlignment toolStripItem.Alignment
 *           └ enum ToolStripItemAlignment { Left = 0, Right = 1 }  〔ToolStripItem〕
 *           
 *         Border3DStyle          toolStripStatusLabel.BorderStyle
 *           └ enum  Border3DStyle
 *             {
 *                 RaisedOuter = 1, //外縁だけ凸表示、内縁は非表示
 *                 SunkenOuter = 2, //外縁だけ凹表示、内縁は非表示
 *                 RaisedInner = 4, //内縁だけが凸表示され、外縁は非表示
 *                 Raised = 5,      //内縁と外縁が、凸表示
 *                 Etched = 6,      //内縁と外縁が、凹表示
 *                 SunkenInner = 8, //凹表示され、外縁は非表示
 *                 Bump = 9,        //内縁と外縁が、凸表示
 *                 Sunken = 10,     //内縁と外縁が凹表示
 *                 Adjust = 8192,   //境界線が指定した四角形の外側に描画され、四角形の大きさは保持
 *                 Flat = 16394     //平面表示
 *             }
 *             
 *         ToolStripStatusLabelBorderSides 
 *           |                    toolStripStatusLabel.BorderSides
 *           └ enum ToolStripStatusLabelBorderSides
 *             {
 *                 None = 0,  //境界線なし
 *                 Left = 1,  //左側にのみ境界線
 *                 Top = 2,   //上側にのみ境界線
 *                 Right = 4, //右側にのみ境界線
 *                 Bottom = 8,//下側にのみ境界線
 *                 All = 15   //すべての側に境界線
 *             }
 *
 *@subject ◆ToolStripControlHost : ToolStripItem
 *         ToolStripControlHost    new ToolStripControlHost(Control c);
 *         ToolStripControlHost    new ToolStripControlHost(Control c, string name)
 *         
 *         Control  toolStripControlHost.Control    コントロールを親に持ち、
 *                                                  ToolStripItemのメンバーを付与
 *                                                  
 *@subject ◆ToolStripProgressBar : ToolStripControlHost
 *         ToolStripProgressBar    new ToolStripProgressBar();
 *         ToolStripProgressBar    new ToolStripProgressBar(string name);
 *         (ProgressBar系 メンバー)
 *         
 *@subject ToolStripDropDownButton : ToolStripDropDownItem
 *         (SelectBox系？)
 *
 *@subject ToolStripSplitButton : ToolStripDropDownItem
 *         EventHandler toolStripSplitButton.ButtonClick
 *         (通常ボタンと UpDownボタンの融合？)
 *
 *@subject Deplyment 配置
 *         Form.Controls.Add(Control)
 *           └ StatusStrip
 *               └ status.Items.Add(ToolStripItem)
 *                   └ ToolStripStatusLabel
 *                   └ ToolStripProgressBar
 *                   └ ToolStripDropDownButton
 *                   └ ToolStripSplitButton
 *           
 *@see FormStatusStripSample.jpg
 *@author shika
 *@date 2022-07-25
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT12_MenuToolStrip
{
    class MainStatusStripSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormStatusStripSample());
        }//Main()
    }//class

    class FormStatusStripSample : Form
    {
        private MenuStrip menu;
        private StatusStrip status;
        private ToolStripStatusLabel statusLabel;
        private ToolStripProgressBar tsProgressBar;

        public FormStatusStripSample()
        {
            this.Text = "FormStatusStripSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            //---- MenuStrip ----
            var menuNew = new ToolStripMenuItem("New File (&N)");
            var menuExit = new ToolStripMenuItem("Close (&X)");
            var menuFile = new ToolStripMenuItem("File (&F)");

            menuNew.Click += new EventHandler(menuNew_Click);
            menuNew.MouseEnter += new EventHandler(menu_MouseEnter);
            menuNew.MouseLeave += new EventHandler(menu_MouseLeave);
            menuExit.Click += new EventHandler(menuExit_Click);
            menuExit.MouseEnter += new EventHandler(menu_MouseEnter);
            menuExit.MouseLeave += new EventHandler(menu_MouseLeave);

            menuFile.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuNew, new ToolStripSeparator(), menuExit,
            });

            menu = new MenuStrip();
            menu.Items.AddRange(new ToolStripItem[]
            {
                menuFile,
            });

            //---- StatusStrip ----
            statusLabel = new ToolStripStatusLabel();
            tsProgressBar = new ToolStripProgressBar()
            {
                Alignment = ToolStripItemAlignment.Right,
                Value = 80,
                Size = new Size(80, 16),
            };

            status = new StatusStrip()
            {
                Dock = DockStyle.Bottom,
                LayoutStyle = ToolStripLayoutStyle.StackWithOverflow,
            };
            status.Items.AddRange(new ToolStripItem[]
            {
                statusLabel, tsProgressBar,
            });

            //---- Form ----
            this.Controls.AddRange(new Control[]
            {
                menu, status,
            });
            this.MainMenuStrip = menu;
        }//constructor

        private void menuNew_Click(object sender, EventArgs e)
        {
            new FormStatusStripSample().Show();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menu_MouseEnter(object sender, EventArgs e)
        {
            switch (sender.ToString())
            {
                case "New File (&N)":
                    statusLabel.Text = "It create new Form.";
                    break;
                case "Close (&X)":
                    statusLabel.Text = "It close this Form.";
                    break;
            }
        }

        private void menu_MouseLeave(object sender, EventArgs e)
        {
            statusLabel.Text = "";
        }
    }//class
}
