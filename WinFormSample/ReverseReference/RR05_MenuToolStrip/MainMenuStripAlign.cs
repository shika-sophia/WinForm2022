/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR05_MenuToolStrip
 *@class MainMenuStripAlign.cs
 *@class FormMenuStripAlign.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menu-strips/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm12_MenuToolStrip.txt〕
 *           
 *@content RR[81]-[84] MenuStrip / p157-p161
 *@subject Nameプロパティ
 *         string  control.Name  Controlの名前
 *                               他クラスからオブジェクト名として利用
 *                               (非推奨) 日本語でもオブジェクト名として登録可                              
 *
 *@see FormMenuStripAlign.jpg
 *@author shika
 *@date 2022-07-26
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR05_MenuToolStrip
{
    class MainMenuStripAlign
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormMenuStripAlign()");

            Application.EnableVisualStyles();
            Application.Run(new FormMenuStripAlign());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMenuStripAlign : Form
    {
        private MenuStrip menu;
        private Label label;

        public FormMenuStripAlign()
        {
            this.Text = "FormMenuStripAlign";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            var menuLeft = new ToolStripMenuItem("左寄せ (&L)");
            var menuCenter = new ToolStripMenuItem("中央揃え (&C)");
            var menuRight = new ToolStripMenuItem("右寄せ (&R)");

            menuLeft.Click += new EventHandler(menuLeft_Click);
            menuCenter.Click += new EventHandler(menuCenter_Click);
            menuRight.Click += new EventHandler(menuRight_Click);

            var menuAlign = new ToolStripMenuItem("配置 (&A)");
            menuAlign.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuLeft, menuCenter, menuRight,
            });

            menu = new MenuStrip();
            menu.Items.AddRange(new ToolStripItem[]
            {
                menuAlign,
            });

            label = new Label()
            {
                Text = "Visual C# 2019",
                Location = new Point(10, 150),
                Width = 280,
                TextAlign = ContentAlignment.MiddleLeft,
                BorderStyle = BorderStyle.FixedSingle,
            };

            this.Controls.AddRange(new Control[]
            {
                menu, label,
            });
            this.MainMenuStrip = menu;
        }//constructor

        private void menuLeft_Click(object sender, EventArgs e)
        {
            label.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void menuCenter_Click(object sender, EventArgs e)
        {
            label.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void menuRight_Click(object sender, EventArgs e)
        {
            label.TextAlign = ContentAlignment.MiddleRight;
        }
    }//class
}
