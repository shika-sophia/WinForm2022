/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT12_MenuToolStrip
 *@class MainContextMenuStripSample.cs
 *@class FormContextMenuStripSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menu-strips/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm12_MenuToolStrip.txt〕
 *           
 *@content KT12 MenuToolStrip / ContextMenuStrip
 *         ToolStrip版の ContectMenu
 *         右クリックでメニューを表示するコントロール
 *
 *@subject ◆ToolStrip =>〔MainMenuStripSample.cs〕
 *               ↑
 *@subject ◆ToolStripDropDown : ToolStrip
 *         ToolStripDropDown    new ToolStripDropDown()
 *         (主なメンバーは ToolStripで定義済)
 *         
 *         CancelEventHandler toolStripDropDown.Opening;
 *         EventHandler       toolStripDropDown.Opened;
 *         EventHandler       toolStripDropDown.Enter;
 *         EventHandler       toolStripDropDown.Leave;
 *         ToolStripDropDownClosingEventHandler 
 *                            toolStripDropDown.Closing;
 *         ToolStripDropDownClosedEventHandler 
 *                            toolStripDropDown.Closed;      
 *               ↑
 *@subject ◆ToolStripDropDownMenu : ToolStripDropDown
 *          ToolStripDropDownMenu   new ToolStripDropDownMenu();
 *          
 *          ToolStripLayoutStyle    toolStripDropDownMenu.LayoutStyle
 *            └ enum ToolStripLayoutStyle { } =>〔MainMenuStripSample.cs〕
 *               ↑
 *@subject ◆ContextMenuStrip : ToolStripDropDownMenu
 *         ContextMenuStrip   new ContextMenuStrip();
 *         ContextMenuStrip   new ContextMenuStrip(IContainer);
 *         
 *         Control      contextMenuStrip.SourceControl
 *
 *@subject Deployment 配置
 *         ContextMenuStrip  control.ContextMenuStrip
 *        〔 form.Controls.Add() は必要ない 〕
 *         
 *@see FormContextMenuStripSample.jpg
 *@see MainMenuStripSample.cs
 *@author shika
 *@date 2022-07-25
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT12_MenuToolStrip
{
    class MainContextMenuStripSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormContextMenuStripSample());
        }//Main()
    }//class

    class FormContextMenuStripSample : Form
    {
        private Label label;
        private ContextMenuStrip contextMenu;

        public FormContextMenuStripSample()
        {
            this.Text = "FormContextMenuStripSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            label = new Label()
            {
                Text = "(Formを右クリックでコンテキストメニューを表示)",
                Location = new Point(10, 100),
                AutoSize = true,
            };

            var menuNew = new ToolStripMenuItem("New File (&N)");
            var menuExit = new ToolStripMenuItem("Close (&X)");

            menuNew.Click += new EventHandler(menuNew_Click);
            menuExit.Click += new EventHandler(menuExit_Click);

            contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                menuNew, new ToolStripSeparator(), menuExit,
            });

            this.Controls.Add(label);
            this.ContextMenuStrip = contextMenu;
        }//constructor

        private void menuNew_Click(object sender, EventArgs e)
        {
            new FormContextMenuStripSample().Show();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }//class
}
