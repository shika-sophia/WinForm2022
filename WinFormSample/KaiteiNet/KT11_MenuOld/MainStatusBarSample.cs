/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT11_MenuOld 
 *@class MainStatusBarSample.cs
 *@class FormStatusBarSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menus/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm11_MenuOld.txt〕
 *           
 *@content KT11 MenuOld / StatusBar
 *         Formの最下段の枠線に テキストを表示するコントロール
 *
 *@subject ◆StatusBar : Control
 *         StatusBar    new StatusBar()
 *         bool         statusBar.ShowPanels      Panelを表示するか / デフォルト false
 *         StatusBarPanelCollection  statusBar.Panels 
 *         int            statusBar.Panels.Add(StatusBarPanel value)
 *         StatusBarPanel statusBar.Panels.Add(string text);
 *         void           statusBar.Panels.AddRange(StatusBarPanel[] panels)
 *                         :  他 IList, ICollection 系メソッド
 *                         
 *@subject StatusBarPanel : Component
 *         StatusBarPanel  new StatusBarPanel()
 *         string          statusBar.Panel.Text             表示する文字列
 *         StatusBarPanelAutoSize  statusBarPanel.AutoSize
 *           └ enum StatusBarPanelAutoSize
 *             {    
 *                 None = 1,     //ControlのSizeが変更されても、StatusBarSizeは不変
 *                 Spring = 2,   //StatusBar 上の使用できる領域のうち None, Contents以外の領域を
 *                                 Springに指定した他のPanel と共有する (？意味不明)
 *                 Contents = 3  //StatusBarPanel の幅は、その内容によって決定
 *             }
 *             
 *@subject Event
 *         EventHandler  menuItem.Click    クリック時のイベント
 *         EventHandler  menuItem.Select   フォーカスを得たときのイベント
 *         EventHandler  form.MenuComplete フォーカスが外れたときのイベント
 *         
 *@subject Deployment / 配置
 *         Form / Panel                               Controlとして乗せる
 *           └ form.Controls.Add(statusBar)           StatusBarインスタンス
 *               statusBar.Panels.Add(statusBarPanel) SatatusBarに StatusBarPanelを乗せる
 *         
 *@see FormStatusBarSample.jpg
 *@author shika
 *@date 2022-07-23
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT11_MenuOld
{
    class MainStatusBarSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormStatusBarSample());
        }//Main()
    }//class

    class FormStatusBarSample : Form
    {
        private MainMenu menu;
        private StatusBar status;
        private StatusBarPanel statusPanel;

        public FormStatusBarSample()
        {
            this.Text = "FormStatusBarSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            menu = new MainMenu();
            //---- Parent ----
            var menuFile = new MenuItem("File");

            //---- Children ----
            var menuNew = new MenuItem("New File (&N)");
            var menuLine = new MenuItem("-");
            var menuExit = new MenuItem("Close (&X)");

            //---- StatusBar ----
            status = new StatusBar()
            {
                ShowPanels = true,
            };

            statusPanel = new StatusBarPanel()
            {
                AutoSize = StatusBarPanelAutoSize.Spring,
            };

            status.Panels.Add(statusPanel);

            //---- Event ----
            menuNew.Click += new EventHandler(menuNew_Click);
            menuNew.Select += new EventHandler(menuNew_Select);
            menuExit.Click += new EventHandler(menuExit_Click);
            menuExit.Select += new EventHandler(menuExit_Select);
            this.MenuComplete += new EventHandler(form_MenuComplete);

            //---- Deployment ----
            menu.MenuItems.AddRange(new MenuItem[]
            {
                menuFile,
            });

            menuFile.MenuItems.AddRange(new MenuItem[]
            {
                menuNew, menuLine, menuExit,
            });

            this.Controls.Add(status);
            this.Menu = menu;
        }//constructor

        private void menuNew_Click(object sender, EventArgs e)
        {
            new FormStatusBarSample().Show();
        }

        private void menuNew_Select(object sender, EventArgs e)
        {
            statusPanel.Text = "It will create new Form.";
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuExit_Select(object sender, EventArgs e)
        {
            statusPanel.Text = "It will close this Form.";
        }

        private void form_MenuComplete(object sender, EventArgs e)
        {
            statusPanel.Text = "";
        }
    }//class
}
