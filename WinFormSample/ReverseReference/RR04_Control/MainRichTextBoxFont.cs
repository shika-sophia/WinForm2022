/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainRichTextBoxFont.cs
 *@class FormRichTextBoxFont.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *
 *@content RR[94] RichTextBox -- SelectionFont, SelectionColor
 *@subject RichTextBox : TextBoxBase
 *         Font      rich.SelectionFont   選択範囲のFont
 *         Color     rich.SelectionColor  選択範囲のColor
 *         
 *         FontFamily font.FontFamily   ※getのみ
 *         Size       font.Size
 *         FontStyle  font.Style
 *         
 *@subject class FontFamily
 *         FontFamily  new FontFamily(string familyname)
 *         引数 familyname: "ＭＳ ゴシック","ＭＳ 明朝"など
 *                          全角文字, 半角スペースで記述
 *         
 *@see FormRichTextBoxFont.jpg
 *@author shika
 *@date 2022-07-19
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainRichTextBoxFont
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormRichTextBoxFont());
        }//Main()
    }//class

    class FormRichTextBoxFont : Form
    {
        private TableLayoutPanel table;
        private RichTextBox rich;
        private Button[] buttonAry;
        private string[] itemAry = new string[]
        {
            "ＭＳ ゴシック","ＭＳ 明朝",
            "Plain", "Bold", "Italic", "Black", "Red",
        };

        public FormRichTextBoxFont()
        {
            this.Text = "FormRichTextBoxFont";
            this.AutoSize = true;

            //---- Table ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 6,
                RowCount = 3,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            for(int i = 0; i < itemAry.Length; i++)
            {
                table.ColumnStyles.Add(
                    new ColumnStyle(
                        SizeType.Percent, 
                        (float)(100 / table.ColumnCount)));
            }//for

            //---- RichTextBox ----
            rich = new RichTextBox()
            {
                Text = "色は匂へど 散りぬるを\n我が世 誰ぞ常ならむ",
                Font = new Font("ＭＳ ゴシック", 14, FontStyle.Regular),
                Size = new Size(300, 300),
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(rich, 0, 0);
            table.SetColumnSpan(rich, 6);

            //---- Button ----
            buttonAry = new Button[itemAry.Length];
            for(int i = 0; i < itemAry.Length; i++)
            {
                buttonAry[i] = new Button()
                {
                    Text = itemAry[i],
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                buttonAry[i].Click += new EventHandler(button_Click);
                
                if(i == 0 || i == 1)
                {
                    table.Controls.Add(buttonAry[i], i, 1);
                    table.SetColumnSpan(buttonAry[i], 3);
                }
                else if(i >= 2)
                {
                    table.Controls.Add(buttonAry[i], (i - 2), 2);
                }
            }//for

            this.Controls.Add(table);
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            if(rich.SelectionFont == null) { return; }
            
            Font font = rich.SelectionFont;
            FontStyle style = rich.SelectionFont.Style;
            Color color = rich.SelectionColor;

            string command = ((Button) sender).Text;
            switch (command)
            {
                case "ＭＳ ゴシック":
                case "ＭＳ 明朝":
                    rich.SelectionFont = new Font(command, font.Size, style);
                    return;
                case "Plain":
                    style = FontStyle.Regular;
                    break;
                case "Bold":
                    style = FontStyle.Bold;
                    break;
                case "Italic":
                    style = FontStyle.Italic;
                    break;
                case "Black":
                    color = Color.Black;
                    break;
                case "Red":
                    color = Color.Red;
                    break;                    
            }//switch

            rich.SelectionFont = 
                new Font(font.FontFamily, font.Size, style);
            rich.SelectionColor = color;
        }//button_Click()
    }//class
}
