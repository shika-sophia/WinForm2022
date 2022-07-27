/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR05_MenuToolStrip
 *@class MainMenuStripCheck.cs
 *@class FormMenuStripCheck.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menu-strips/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm12_MenuToolStrip.txt〕
 *           
 *@content RR[83] MenuStrip -- Check / p160
 *
 *@subject MenuStrip, ToolStripMenuItem
 *         =>〔KaiteiNet / KT12_MenuToolStrip / MainMenuSample.cs〕
 *         
 *@subject EventHandler Method
 *         bool  toolStripMenuItem.CheckOnClick  チェックできるか
 *         bool  toolStripMenuItem.Checked       チェックされているか
 *         
 *         ・FontStyleは ビット論理和「|」で複合可能なので、
 *           foreachで Checked をすべての項目で調べ、複合させた FontStyleを表示
 *         ・ToolStripItemには Checkedが定義されていない
 *         ・ToolStripSeperetorは ToolStripItemなので、foreachから除外する       
 *        
 *@see FormMenuStripCheck.jpg
 *@see KaiteiNet / KT12_MenuToolStrip / MainMenuSample.cs
 *@author shika
 *@date 2022-07-26
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR05_MenuToolStrip
{
    class MainMenuStripCheck
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormMenuStripCheck()");

            Application.EnableVisualStyles();
            Application.Run(new FormMenuStripCheck());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMenuStripCheck : Form
    {
        private MenuStrip menu;
        private ToolStripMenuItem menuStyle;
        private Label label;

        public FormMenuStripCheck()
        {
            this.Text = "FormMenuStripCheck";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            var menuBold = new ToolStripMenuItem("Bold   (&B)")
            {
                CheckOnClick = true,
            };
            var menuItalic = new ToolStripMenuItem("Italic (&I)")
            {
                CheckOnClick = true,
            };
            var menuUnderline = new ToolStripMenuItem("Underline (&U)")
            {
                CheckOnClick = true,
            }; ;
            var menuStrikeout = new ToolStripMenuItem("Strikeout (&S)")
            {
                CheckOnClick = true,
            }; ;

            menuBold.CheckedChanged += new EventHandler(style_CheckedChanged);
            menuItalic.CheckedChanged += new EventHandler(style_CheckedChanged);
            menuUnderline.CheckedChanged += new EventHandler(style_CheckedChanged);
            menuStrikeout.CheckedChanged += new EventHandler(style_CheckedChanged);

            menuStyle = new ToolStripMenuItem("Style (&S)");
            menuStyle.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuBold, menuItalic, new ToolStripSeparator(),
                menuUnderline, menuStrikeout,
            });

            menu = new MenuStrip();
            menu.Items.AddRange(new ToolStripItem[]
            {
                menuStyle,
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

        private void style_CheckedChanged(object sender, EventArgs e)
        {
            FontStyle style = FontStyle.Regular;

            foreach (ToolStripItem children in menuStyle.DropDownItems)
            {
                if (children is ToolStripSeparator)
                {
                    continue;
                }

                if (((ToolStripMenuItem)children).Checked)
                {
                    switch (children.Text)
                    {
                        case "Bold   (&B)":
                            style = style | FontStyle.Bold;
                            break;
                        case "Italic (&I)":
                            style = style | FontStyle.Italic;
                            break;
                        case "Underline (&U)":
                            style = style | FontStyle.Underline;
                            break;
                        case "Strikeout (&S)":
                            style = style | FontStyle.Strikeout;
                            break;
                    }//switch
                }
            }//foreach 

            label.Font = new Font(
                label.Font.FontFamily, label.Font.Size, style);
        }//style_CheckedChanged()
    }//class
}
