/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR07_MultiWindow
 *@class MainMdiSample.cs
 *@class   └ new FormMdiSample() : Form          //MdiParent
 *@class       └ new FormNewDocument() : Form    //MdiChildren
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[117]-[120] MDI / p215-220
 *@subject ◆MDI: Multiple Document Interface
 *         ・Form内に 複数のFormを含むことができる
 *         ・最大化は Form クライアント領域に最大化。元に戻すアイコンも表示
 *         ・最小化は Form下部にタブとして表示。元に戻すアイコンも表示
 *         ・上記の機能も含むので起動時は少し重い
 *         ・Formのプロパティで設定できる
 *         ・Templateとしても用意されている
 *             VS[プロジェクト] -> [追加] -> [新しい項目]
 *             -> [MDI親フォーム] -> (名前) -> [追加]
 *             
 *             => Menu, ToolBar, StatusBarがデフォルトで設置されている
 *
 *@subject Form -- System.Windows.Froms.
 *         bool   親form.IsMdiContainer   親 MdiFormか
 *         Form   子form.MdiParent        親 Formを登録
 *         Form[] 親form.MdiChildren      子 Formの配列
 *                                        MdiParentを登録すると自動で配列に追加
 *         int    親form.MdiChildren.Length  子 Formの数を取得
 *         
 *         
 *@NOTE【考察】Form部品はコンストラクタで記述すべき
 *      Form部品を Form_Load(object sender, EventArgs e)に記述すると
 *      new Form1().Show();で空のFormを表示し、Form部品は反映しない。
 *      
 *@see ImageMdiSample.jpg
 *@see 
 *@author shika
 *@date 2022-08-06
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR07_MultiWindow
{
    class MainMdiSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormMdiSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormMdiSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMdiSample : Form
    {
        private MenuStrip menu;
        private Font font = new Font("consolas", 12, FontStyle.Regular);

        public FormMdiSample()
        {
            this.Text = "FormMdiSample";
            this.Font = font;
            this.Size = new Size(600, 400);
            this.IsMdiContainer = true;

            //---- MenuStrip ----
            var menuFile = new ToolStripMenuItem("File (&F)");
            var menuNew = new ToolStripMenuItem("New Document (&N)");
            var menuExit = new ToolStripMenuItem("All Close (&X)");

            menuNew.Click += new EventHandler(menuNew_Click);
            menuExit.Click += new EventHandler(menuExit_Click);

            menuFile.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuNew, menuExit,
            });

            menu = new MenuStrip() 
            {
                Font = font,
            };

            menu.Items.AddRange(new ToolStripItem[]
            {
                menuFile,
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
    }//class

    class FormNewDocument : Form
    {
        private Font font;
        private Form parent;
        private RichTextBox rich;

        public FormNewDocument(Font font, Form parent)
        {
            this.font = font;
            this.parent = parent;
        
            //---- Form ----
            this.Text = $"Document {parent.MdiChildren.Length}";
            this.Font = font;
            this.MdiParent = parent;
            this.Size = new Size(300, 200);

            //---- Controls ----
            rich = new RichTextBox()
            {
                Font = font,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            this.Controls.AddRange(new Control[]
            {
                rich,
            });
        }//constructor
    }//class
}
