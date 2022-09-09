/** 
 *@title WinFormGUI / WinFormSample / Viewer / ColorSample
 *@based MainColorPropertiyViewer.cs
 *@class MainSystemColorsViewer.cs
 *@class   └ new FormSystemColorsViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content SystemColorsViewer
 *         SystemColorsの Propertyを 色見本と伴に一覧表示
 *         
 *@subject ◆class System.Drawing.SystemColors
 *         ＊All Properties  33個
 *         Color   SystemColors.ActiveBorder  { get; }
 *         Color   SystemColors.ActiveCaption  { get; }
 *         Color   SystemColors.ActiveCaptionText  { get; }
 *         Color   SystemColors.AppWorkspace  { get; }
 *         Color   SystemColors.ButtonFace  { get; }
 *         Color   SystemColors.ButtonHighlight  { get; }
 *         Color   SystemColors.ButtonShadow  { get; }
 *         Color   SystemColors.Control  { get; }
 *         Color   SystemColors.ControlDark  { get; }
 *         Color   SystemColors.ControlDarkDark  { get; }
 *         Color   SystemColors.ControlLight  { get; }
 *         Color   SystemColors.ControlLightLight  { get; }
 *         Color   SystemColors.ControlText  { get; }
 *         Color   SystemColors.Desktop  { get; }
 *         Color   SystemColors.GradientActiveCaption  { get; }
 *         Color   SystemColors.GradientInactiveCaption  { get; }
 *         Color   SystemColors.GrayText  { get; }
 *         Color   SystemColors.Highlight  { get; }
 *         Color   SystemColors.HighlightText  { get; }
 *         Color   SystemColors.HotTrack  { get; }
 *         Color   SystemColors.InactiveBorder  { get; }
 *         Color   SystemColors.InactiveCaption  { get; }
 *         Color   SystemColors.InactiveCaptionText  { get; }
 *         Color   SystemColors.Info  { get; }
 *         Color   SystemColors.InfoText  { get; }
 *         Color   SystemColors.Menu  { get; }
 *         Color   SystemColors.MenuBar  { get; }
 *         Color   SystemColors.MenuHighlight  { get; }
 *         Color   SystemColors.MenuText  { get; }
 *         Color   SystemColors.ScrollBar  { get; }
 *         Color   SystemColors.Window  { get; }
 *         Color   SystemColors.WindowFrame  { get; }
 *         Color   SystemColors.WindowText  { get; }
 *
 *@see ImageSystemColorsViewer.jpg
 *@see WinFormGUI / CsharpCode / ShowPropertyAll.cs
 *@author shika
 *@date 2022-09-09
 */
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.ColorSample
{
    class MainSystemColorsViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormSystemColorsViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormSystemColorsViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormSystemColorsViewer : Form
    {
        private readonly TableLayoutPanel table;
        private readonly PropertyInfo[] colorAry = typeof(Color).GetProperties();
        private readonly PropertyInfo[] sysColorAry = typeof(SystemColors).GetProperties();
        private const int COLUMN = 6;

        public FormSystemColorsViewer()
        {
            this.Text = "FormSystemColorsViewer";
            this.Font = new Font("consolas", 10, FontStyle.Regular);
            this.Size = new Size(1200, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;

            table = new TableLayoutPanel()
            {
                ColumnCount = COLUMN,
                RowCount = 13,
                Padding = new Padding(10),
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };

            for(int i = 0; i < table.ColumnCount; i++)
            {
                table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100 / COLUMN));
            }//for

            for (int i = 0; i < table.RowCount; i++)
            {
                table.RowStyles.Add(
                    new RowStyle(SizeType.Absolute, 70));  //px単位
            }//for

            //---- Label ----
            Label labelPart = new Label()
            {
                Text = "◆ class System.Drawing.SystemColors",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelPart, 0, 0);
            table.SetColumnSpan(labelPart, COLUMN);
       
            foreach (PropertyInfo info in sysColorAry)
            {
                string name = info.Name;
                Color color = (Color)info.GetValue(name);
                string colorName = SearchColorName(color);
                string colorR16 = Convert.ToString(color.R, 16);
                string colorG16 = Convert.ToString(color.G, 16);
                string colorB16 = Convert.ToString(color.B, 16);
                colorR16 = (colorR16.Length == 1) ? ("0" + colorR16) : colorR16;
                colorG16 = (colorG16.Length == 1) ? ("0" + colorG16) : colorG16;
                colorB16 = (colorB16.Length == 1) ? ("0" + colorB16) : colorB16;

                Label labelName = new Label()
                {
                    Text = $"{name}\n {colorName}\n #{colorR16}{colorG16}{colorB16}",
                    Margin = new Padding(5),
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                table.Controls.Add(labelName);

                Label labelView = new Label()
                {
                    BackColor = color,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(5),
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                table.Controls.Add(labelView);
            }//foreach

            this.Controls.Add(table);
        }//constructor

        private string SearchColorName(Color sampleColor)
        {
            string srcName = "";
            
            foreach(PropertyInfo colorInfo in colorAry)
            {
                if(colorInfo.PropertyType != typeof(Color)) { continue; }
                
                string name = colorInfo.Name;
                Color color = (Color)colorInfo.GetValue(name);
                
                if(color.ToArgb() == sampleColor.ToArgb())
                {
                    srcName = name;
                    break;
                }//if
            }//foreach

            return srcName;
        }//SearchColorName()
    }//class
}
