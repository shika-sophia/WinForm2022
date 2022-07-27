/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR05_MenuToolStrip
 *@class MainContextMenuSourceControl.cs
 *@class FormContextMenuSourceControl.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menu-strips/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm12_MenuToolStrip.txt〕
 *           
 *@content RR[85] ContextMenuStrip -- SourceControl / p162
 *@subject ContextMenuStrip 
 *         =>〔KaiteiNet/KT12_MenuToolStrip/MainMenuSample〕
 *         =>〔KaiteiNet/KT12_MenuToolStrip/MainContextMenuSample〕
 *         
 *@subject 元Controlを取得
 *         Control  contextMenuStrip.SourceControl
 *           └ (各Controlのプロパティにアクセスする場合は、要キャスト)
 *           
 *@subject Deployment 配置
 *         各Control
 *           └ control.ContextMenuStrip  〔× control.Controls.Add()ではない〕
 *               └ ContextMenuStrip
 *                   └ toolStrip.Items.Add(ToolStripItem)
 *                       └ ToolStripItem
 *                       
 *@see FormContextMenuSourceControl.jpg
 *@author shika
 *@date 2022-07-27
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR05_MenuToolStrip
{
    class MainContextMenuSourceControl
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormContextMenuSourceControl()");

            Application.EnableVisualStyles();
            Application.Run(new FormContextMenuSourceControl());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormContextMenuSourceControl : Form
    {
        private ContextMenuStrip contextMenu;
        private Label label;
        private TextBox textBox1;
        private TextBox textBox2;

        public FormContextMenuSourceControl()
        {
            this.Text = "FormContextMenuSourceControl";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            label = new Label()
            {
                Text = "(Click-Right to each TextBoxes,\n then it Show ContextMenu)",
                Location = new Point(20, 50),
                AutoSize = true,
            };

            //---- ContextMenuStrip ----
            var menuLeft = new ToolStripMenuItem("Align Left (&L)");
            var menuCenter = new ToolStripMenuItem("Align Center (&C)");
            var menuRight = new ToolStripMenuItem("Align Right (&R)");

            menuLeft.Click += new EventHandler(menuLeft_Click);
            menuCenter.Click += new EventHandler(menuCenter_Click);
            menuRight.Click += new EventHandler(menuRight_Click);

            contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                menuLeft, menuCenter, menuRight,
            });

            //---- TextBox ----
            textBox1 = new TextBox()
            {
                Text = "Visual Studio",
                Location = new Point(20, 100),
                Width = 300,
                Multiline = false,
                SelectionStart = 0,
                SelectionLength = 0,
            };
            textBox1.ContextMenuStrip = contextMenu;

            textBox2 = new TextBox()
            {
                Text = "C# Reference",
                Location = new Point(20, 150),
                Width = 300,
                Multiline = false,
            };
            textBox2.ContextMenuStrip = contextMenu;

            //---- Form ----
            this.Controls.AddRange(new Control[]
            {
                label, textBox1, textBox2,
            });
        }//constructor

        private void menuLeft_Click(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)contextMenu.SourceControl;
            textBox.TextAlign = HorizontalAlignment.Left;
        }

        private void menuCenter_Click(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)contextMenu.SourceControl;
            textBox.TextAlign = HorizontalAlignment.Center;
        }

        private void menuRight_Click(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox) contextMenu.SourceControl;
            textBox.TextAlign = HorizontalAlignment.Right;
        }
    }//class
}
