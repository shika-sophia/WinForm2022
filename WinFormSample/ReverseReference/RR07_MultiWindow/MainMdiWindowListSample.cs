/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR07_MultiWindow
 *@class MainMdiWindowListSample.cs
 *@class   └ new FormMdiWindowListSample() : Form  //MdiParent
 *@class       └ new FormNewDocument() : Form      //MdiChildren
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[118] MDI / MdiWindowListItem
 *         親Form内で new 子FormをしたWindowをメニュー項目として表示
 *         
 *@subject MenuStrip
 *         ToolStripMenuItem   menuStrip.MdiWindowListItem
 *         
 *         例: var menuWindow = new ToolStripMenuItem("&Window");
 *             menu.Items.Add(menuWindow);            MenuStripにメニュー項目を登録
 *             menu.MdiWindowListItem = menuWiodow;   Window項目に newされたFormのリストを表示
 *             
 *         ※ menuWindow.DropDownItems.AddRange(ToolStripItem[]); を利用しない
 *           「menuWindow」の子メニューを登録しなくても、
 *            menu.MdiWindowListItem = menuWiodow;〔上記〕をするだけで、
 *            現在表示中の MdiFormのリスト一覧を「menuWindow」の子メニュー項目として表示される
 *
 *@see ImageMdiWindowListSample.jpg
 *@see MainMdiSample.cs
 *@author shika
 *@date 2022-08-06
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR07_MultiWindow
{
    class MainMdiWindowListSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormMdiWindowListSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormMdiWindowListSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMdiWindowListSample : Form
    {
        private MenuStrip menu;
        private Font font = new Font("consolas", 12, FontStyle.Regular);        

        public FormMdiWindowListSample()
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

            menu = new MenuStrip()
            {
                Font = font,
            };
            menu.MdiWindowListItem = menuWindow;

            menu.Items.AddRange(new ToolStripItem[]
            {
                menuFile, menuWindow,
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

    //class FormNewDocument : Form 
    // =>〔MainMdiSample.cs〕
}
