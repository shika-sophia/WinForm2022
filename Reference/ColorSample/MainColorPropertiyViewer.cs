/** 
 *@title WinFormGUI / Reference / ColorSample
 *@class MainColorPropertiyViewer.cs
 *@class   └ new FormColorPropertiyViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet〕
 *           
 *@content ColorPropertiyViewer
 *         struct Colorの Colorプロパティの色見本を一覧表示
 *         
 *@subject ◆class System.Drawing.Color
 *         ＊All Properties -- Property Count: 150
 *         Color   Color.Transparent  { get; }
 *         Color   Color.AliceBlue  { get; }
 *         Color   Color.AntiqueWhite  { get; }
 *         Color   Color.Aqua  { get; }
 *         Color   Color.Aquamarine  { get; }
 *         Color   Color.Azure  { get; }
 *         Color   Color.Beige  { get; }
 *         Color   Color.Bisque  { get; }
 *         Color   Color.Black  { get; }
 *         Color   Color.BlanchedAlmond  { get; }
 *         Color   Color.Blue  { get; }
 *         Color   Color.BlueViolet  { get; }
 *         Color   Color.Brown  { get; }
 *         Color   Color.BurlyWood  { get; }
 *         Color   Color.CadetBlue  { get; }
 *         Color   Color.Chartreuse  { get; }
 *         Color   Color.Chocolate  { get; }
 *         Color   Color.Coral  { get; }
 *         Color   Color.CornflowerBlue  { get; }
 *         Color   Color.Cornsilk  { get; }
 *         Color   Color.Crimson  { get; }
 *         Color   Color.Cyan  { get; }
 *         Color   Color.DarkBlue  { get; }
 *         Color   Color.DarkCyan  { get; }
 *         Color   Color.DarkGoldenrod  { get; }
 *         Color   Color.DarkGray  { get; }
 *         Color   Color.DarkGreen  { get; }
 *         Color   Color.DarkKhaki  { get; }
 *         Color   Color.DarkMagenta  { get; }
 *         Color   Color.DarkOliveGreen  { get; }
 *         Color   Color.DarkOrange  { get; }
 *         Color   Color.DarkOrchid  { get; }
 *         Color   Color.DarkRed  { get; }
 *         Color   Color.DarkSalmon  { get; }
 *         Color   Color.DarkSeaGreen  { get; }
 *         Color   Color.DarkSlateBlue  { get; }
 *         Color   Color.DarkSlateGray  { get; }
 *         Color   Color.DarkTurquoise  { get; }
 *         Color   Color.DarkViolet  { get; }
 *         Color   Color.DeepPink  { get; }
 *         Color   Color.DeepSkyBlue  { get; }
 *         Color   Color.DimGray  { get; }
 *         Color   Color.DodgerBlue  { get; }
 *         Color   Color.Firebrick  { get; }
 *         Color   Color.FloralWhite  { get; }
 *         Color   Color.ForestGreen  { get; }
 *         Color   Color.Fuchsia  { get; }
 *         Color   Color.Gainsboro  { get; }
 *         Color   Color.GhostWhite  { get; }
 *         Color   Color.Gold  { get; }
 *         Color   Color.Goldenrod  { get; }
 *         Color   Color.Gray  { get; }
 *         Color   Color.Green  { get; }
 *         Color   Color.GreenYellow  { get; }
 *         Color   Color.Honeydew  { get; }
 *         Color   Color.HotPink  { get; }
 *         Color   Color.IndianRed  { get; }
 *         Color   Color.Indigo  { get; }
 *         Color   Color.Ivory  { get; }
 *         Color   Color.Khaki  { get; }
 *         Color   Color.Lavender  { get; }
 *         Color   Color.LavenderBlush  { get; }
 *         Color   Color.LawnGreen  { get; }
 *         Color   Color.LemonChiffon  { get; }
 *         Color   Color.LightBlue  { get; }
 *         Color   Color.LightCoral  { get; }
 *         Color   Color.LightCyan  { get; }
 *         Color   Color.LightGoldenrodYellow  { get; }
 *         Color   Color.LightGreen  { get; }
 *         Color   Color.LightGray  { get; }
 *         Color   Color.LightPink  { get; }
 *         Color   Color.LightSalmon  { get; }
 *         Color   Color.LightSeaGreen  { get; }
 *         Color   Color.LightSkyBlue  { get; }
 *         Color   Color.LightSlateGray  { get; }
 *         Color   Color.LightSteelBlue  { get; }
 *         Color   Color.LightYellow  { get; }
 *         Color   Color.Lime  { get; }
 *         Color   Color.LimeGreen  { get; }
 *         Color   Color.Linen  { get; }
 *         Color   Color.Magenta  { get; }
 *         Color   Color.Maroon  { get; }
 *         Color   Color.MediumAquamarine  { get; }
 *         Color   Color.MediumBlue  { get; }
 *         Color   Color.MediumOrchid  { get; }
 *         Color   Color.MediumPurple  { get; }
 *         Color   Color.MediumSeaGreen  { get; }
 *         Color   Color.MediumSlateBlue  { get; }
 *         Color   Color.MediumSpringGreen  { get; }
 *         Color   Color.MediumTurquoise  { get; }
 *         Color   Color.MediumVioletRed  { get; }
 *         Color   Color.MidnightBlue  { get; }
 *         Color   Color.MintCream  { get; }
 *         Color   Color.MistyRose  { get; }
 *         Color   Color.Moccasin  { get; }
 *         Color   Color.NavajoWhite  { get; }
 *         Color   Color.Navy  { get; }
 *         Color   Color.OldLace  { get; }
 *         Color   Color.Olive  { get; }
 *         Color   Color.OliveDrab  { get; }
 *         Color   Color.Orange  { get; }
 *         Color   Color.OrangeRed  { get; }
 *         Color   Color.Orchid  { get; }
 *         Color   Color.PaleGoldenrod  { get; }
 *         Color   Color.PaleGreen  { get; }
 *         Color   Color.PaleTurquoise  { get; }
 *         Color   Color.PaleVioletRed  { get; }
 *         Color   Color.PapayaWhip  { get; }
 *         Color   Color.PeachPuff  { get; }
 *         Color   Color.Peru  { get; }
 *         Color   Color.Pink  { get; }
 *         Color   Color.Plum  { get; }
 *         Color   Color.PowderBlue  { get; }
 *         Color   Color.Purple  { get; }
 *         Color   Color.Red  { get; }
 *         Color   Color.RosyBrown  { get; }
 *         Color   Color.RoyalBlue  { get; }
 *         Color   Color.SaddleBrown  { get; }
 *         Color   Color.Salmon  { get; }
 *         Color   Color.SandyBrown  { get; }
 *         Color   Color.SeaGreen  { get; }
 *         Color   Color.SeaShell  { get; }
 *         Color   Color.Sienna  { get; }
 *         Color   Color.Silver  { get; }
 *         Color   Color.SkyBlue  { get; }
 *         Color   Color.SlateBlue  { get; }
 *         Color   Color.SlateGray  { get; }
 *         Color   Color.Snow  { get; }
 *         Color   Color.SpringGreen  { get; }
 *         Color   Color.SteelBlue  { get; }
 *         Color   Color.Tan  { get; }
 *         Color   Color.Teal  { get; }
 *         Color   Color.Thistle  { get; }
 *         Color   Color.Tomato  { get; }
 *         Color   Color.Turquoise  { get; }
 *         Color   Color.Violet  { get; }
 *         Color   Color.Wheat  { get; }
 *         Color   Color.White  { get; }
 *         Color   Color.WhiteSmoke  { get; }
 *         Color   Color.Yellow  { get; }
 *         Color   Color.YellowGreen  { get; }
 *         Byte   Color.R  { get; }
 *         Byte   Color.G  { get; }
 *         Byte   Color.B  { get; }
 *         Byte   Color.A  { get; }
 *         Boolean   Color.IsKnownColor  { get; }
 *         Boolean   Color.IsEmpty  { get; }
 *         Boolean   Color.IsNamedColor  { get; }
 *         Boolean   Color.IsSystemColor  { get; }
 *         String   Color.Name  { get; }
 *
 *@NOTE【Problem】
 *      table.RowCount = length / COLUMN; とすると、
 *      最終行が 無限に続く大きさになってしまう問題。
 *      Color型のプロパティ数 141 / (Column数 6 / Label数 2) = 47 を リテラルとして代入すると解決。
 *      おそらく、割算を intにする際の誤差バグと思われる。
 *      
 *@see ~/Reference/ColorSample/ImageColorPropertiyViewer1.jpg - 6.jpg
 *@see ~/CsharpCode/ShowProrertyAll.cs
 *@author shika
 *@date 2022-09-05
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinFormGUI.Reference.ColorSample
{
    class MainColorPropertiyViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormColorPropertyViewer()");
            Console.WriteLine("  Calculating now...");
            Console.WriteLine("  Please wait few minutes.");

            Application.EnableVisualStyles();
            Application.Run(new FormColorPropertyViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormColorPropertyViewer : Form
    {
        private readonly PropertyInfo[] colorOnlyAry;
        private const int COLUMN = 6;
        private readonly TableLayoutPanel table;
        private readonly Label[] nameAry;
        private readonly Label[] viewAry;
        private readonly Padding padding = new Padding(5);

        public FormColorPropertyViewer()
        {
            this.Text = "FormColorPropertyViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(960, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;

            colorOnlyAry = BuildColorOnlyAry();
            int length = colorOnlyAry.Length;
            nameAry = new Label[length];
            viewAry = new Label[length];
            //Console.WriteLine($"length: {length}"); // 141

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = COLUMN,
                RowCount = 47,        // = length / (COLUMN / 2)
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };

            for (int i = 0; i < table.ColumnCount; i++)
            {
                table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100 / COLUMN));
            }//for

            for (int i = 0; i < table.RowCount; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));  //px単位
            }//for

            //---- Label ----
            int index = 0;
            foreach(PropertyInfo info in colorOnlyAry)
            {
                string name = info.Name;
                Color color = (Color)info.GetValue(name);
                string colorRGB = $"R{color.R} G{color.G} B{color.B}";        // byte表記:   0-255
                string colorRGB16 = $"#{color.R :x}{color.G :x}{color.B :x}"; // 16進数表記: $"{value : x}"  00-ff
            
                nameAry[index] = new Label()
                {
                    Text = $"{name}\n {colorRGB16}\n {colorRGB}",
                    Dock = DockStyle.Fill,
                    Margin = padding,
                    AutoSize = true,
                };
                table.Controls.Add(nameAry[index]);

                viewAry[index] = new Label()
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = color,
                    Dock = DockStyle.Fill,
                    Margin = padding,
                    AutoSize = true,
                };
                table.Controls.Add(viewAry[index]);

                index++;
            }//foreach
            
            this.Controls.Add(table);
        }//constructor

        private PropertyInfo[] BuildColorOnlyAry()
        {
            PropertyInfo[] colorPropertyAry = typeof(Color).GetProperties();
            List<PropertyInfo> colorOnlyList = 
                new List<PropertyInfo>(colorPropertyAry.Length);
            
            foreach (PropertyInfo info in colorPropertyAry)
            {
                if (info.PropertyType == typeof(Color))
                {
                    colorOnlyList.Add(info);
                }
            }//foreach

            return colorOnlyList.ToArray();
        }//BuildColorOnlyList()
    }//class
}
