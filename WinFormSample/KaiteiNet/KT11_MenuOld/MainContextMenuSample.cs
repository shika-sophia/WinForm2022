/** 
 * @title WinFormGUI / WinFormSample / KaiteiNet / KT11_MenuOld
 *@class MainContextMenuSample.cs
 *@class FormContextMenuSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menus/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm11_MenuOld.txt〕
 *           
 *@content KT11 MenuOld / ContextMenu
 *         ContextMenuは Form上部のメニューバーではなく、
 *         Form/Controlを「右クリック」時に出るメニュー
 *         
 *@subject ◆ContextMenu : Menu
 *         ContextMenu   new ContextMenu()
 *         ContextMenu   new ContextMenu(MenuItem[])
 *         
 *         Control  contextMenu.SourceControl この ContextMenu に関連付けた Control { get; }
 *         void     contextMenu.Show(         Control, 座標, 位置 を指定してコンテキストメニューを表示。
 *                     Control, Point, [LeftRightAlignment])
 *                     
 *         EventHandler contextMenu.Popup     コンテキストメニューを開いたときのイベント
 *         EventHandler contextMenu.Collapse  コンテキストメニューを閉じたときのイベント
 *         
 *         MenuItemCollection  menu.MenuItems 〔 Add(), AddRange() 以下同様 〕
 *         EventHandler menuItem.Click
 *         EventHandler menuItem.Select
 *         
 *@subject 配置
 *         ContextMenu   control.ContextMenu
 *         
 *@see MainMenuSample.cs
 *@see FormContextMenuSample.jpg
 *@author shika
 *@date 2022-07-23
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT11_MenuOld
{
    class MainContextMenuSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormContextMenuSample());
        }//Main()
    }//class

    class FormContextMenuSample : Form
    {
        private Label label;
        private ContextMenu contextMenu;

        public FormContextMenuSample()
        {
            this.Text = "FormContextMenuSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            label = new Label()
            {
                Text = "(Form内を右クリックで ContextMenuを表示)",
                AutoSize = true,
            };

            contextMenu = new ContextMenu();
            var menuNew = new MenuItem("new File (&F)");
            var menuLine = new MenuItem("-");
            var menuExit = new MenuItem("Close (&X)");

            menuNew.Click += new EventHandler(menuNew_Click);
            menuExit.Click += new EventHandler(menuExit_Click);

            contextMenu.MenuItems.AddRange(new MenuItem[]
            {
                menuNew, menuLine, menuExit,
            });

            this.Controls.Add(label);
            this.ContextMenu = contextMenu;
        }//constructor

        private void menuNew_Click(object sender, EventArgs e)
        {
            new FormContextMenuSample().Show();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }//class
}
