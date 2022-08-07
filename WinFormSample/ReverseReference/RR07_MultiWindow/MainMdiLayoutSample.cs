/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR07_MultiWindow
 *@class MainMdiLayoutSample.cs
 *@class   └ new FormMdiLayoutSample() : Form      //MidParent
 *@class       └ new FormNewDocument() : Form      //MdiChildren
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *
 *@content RR[119] MDI / LayoutMdi() / p218
 *@subject Form
 *         MdiLayout   親form.LayoutMdi(MdiLayout)
 *           └ enum MdiLayout
 *             {
 *                 Cascade = 0,        //重ねて表示
 *                 TileHorizontal = 1, //水平に並べて表示
 *                 TileVertical = 2,   //垂直に並べて表示
 *                 ArrangeIcons = 3    //子アイコンを並べて表示
 *             }
 *
 *@subject 親メニュー項目オブジェクトの取得
 *         delegate void ToolStripItemClickedEventHandler(
 *             object sender, ToolStripItemClickedEventArgs e)
 *             
 *         ・このイベントは親メニュー項目のイベントなので、senderは親メニュー項目のオブジェクト
 *         ・ToolStripItemClickedEventArgs e
 *           ToolStripItem    e.ChildClicked      クリックされた子メニュー項目オブジェクトはこちらを利用
 *           
 *         ToolStripMenuItem menuLayout = (ToolStripMenuItem) sender;
 *         foreach(ToolStripMenuItem menuChild in menuLayout.DropDownItems)
 *         {
 *              menuChild.Checked = false;
 *         }//foreach
 *         
 *         ToolStripItem selectedItem = e.ClickedItem;
 *
 *@NOTE【考察】新規追加時は MdiLayout.Cascadeで追加
 *      ・新規追加前の Layoutにチェックされている
 *      ・子Formが何もない状態で Layoutメニューを指定すると、
 *        チェック状態にはなるが、新規追加時は MdiLayout.Cascadeで追加
 *      
 *@NOTE【考察】Checkの単一選択
 *      指定したメニュー項目のみチェック状態にしたい場合、
 *      CheckOnClicked = true;のままだと、複数項目にチェックされる。
 *         
 *      => Clickイベント内で、全ての Checked = false;にする。
 *         指定メニュー項目を trueにしておくと、全ての Checkが外れてしまう。
 *         
 *      => 全ての項目を falseにしただけなのに、(どこも trueにしていないのに)
 *         実行動作は意図した動作(=新たにクリックした項目だけチェック)になっている。
 *         おそらく、メニュー項目の処理が完了したときのイベントが暗黙的に設定されている可能性あり。
 *         暗黙的に、クリックした項目を trueにしているコードが存在するのは
 *         望ましいコードではない。
 *         
 *@see ImageMdiLayoutSample.jpg
 *@see MainMdiSample.cs
 *@see MainMdiWindowListSample.cs
 *@author shika
 *@date 2022-08-07
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR07_MultiWindow
{
    class MainMdiLayoutSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormMdiLayoutSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormMdiLayoutSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMdiLayoutSample : Form
    {
        private MenuStrip menu;
        private Font font = new Font("consolas", 12, FontStyle.Regular);

        public FormMdiLayoutSample()
        {
            //==== Form ====
            this.Text = "FormMdiWindowListSample";
            this.Font = font;
            this.Size = new Size(600, 400);
            this.IsMdiContainer = true;

            //==== MenuStrip ====
            //---- File ----
            var menuFile = new ToolStripMenuItem("&File");
            var menuNew = new ToolStripMenuItem("New Document (&N)");
            var menuExit = new ToolStripMenuItem("Exit (&X)");
            menuNew.Click += new EventHandler(menuNew_Click);
            menuExit.Click += new EventHandler(menuExit_Click);

            menuFile.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuNew, menuExit,
            });

            //---- Window ----
            var menuWindow = new ToolStripMenuItem("&Window");

            //---- Layout ----
            var menuLayout = new ToolStripMenuItem("&Layout");
            var menuCascade = new ToolStripMenuItem("Cascade") { CheckOnClick = true };
            var menuHorizontal = new ToolStripMenuItem("Horizontal") { CheckOnClick = true }; ;
            var menuVertical = new ToolStripMenuItem("Vertical") { CheckOnClick = true };
            menuCascade.Checked = true;
            
            menuLayout.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuCascade, menuHorizontal, menuVertical,
            });
            menuLayout.DropDownItemClicked += 
                new ToolStripItemClickedEventHandler(menuLayout_Click);

            //---- MenuStrip ----
            menu = new MenuStrip()
            {
                Font = font,
                MdiWindowListItem = menuWindow,
            };

            menu.Items.AddRange(new ToolStripItem[]
            {
                menuFile, menuWindow, menuLayout,
            });

            this.Controls.AddRange(new Control[]
            {
                menu,
            });
            this.MainMenuStrip = menu;
        }//constructor

        private void menuNew_Click(object sender, EventArgs e)
        {
            new FormNewDocument(font, this).Show();
        }//menuNew_Click()

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuLayout_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem menuLayout = (ToolStripMenuItem) sender;
            foreach(ToolStripMenuItem menuChild in menuLayout.DropDownItems)
            {
                menuChild.Checked = false;
            }//foreach

            ToolStripItem selectedItem = e.ClickedItem;
            switch (selectedItem.Text)
            {
                case "Cascade":
                    this.LayoutMdi(MdiLayout.Cascade);
                    break;
                case "Horizontal":
                    this.LayoutMdi(MdiLayout.TileHorizontal);
                    break;
                case "Vertical":
                    this.LayoutMdi(MdiLayout.TileVertical);
                    break;
                default:
                    throw new ArgumentException();
            }//switch
        }

    }//class

    //class FormNewDocument : Form 
    // =>〔MainMdiSample.cs〕
}
