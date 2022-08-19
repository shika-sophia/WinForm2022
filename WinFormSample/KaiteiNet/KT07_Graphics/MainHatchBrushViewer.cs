/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainHatchBrushViewer.cs
 *@class   └ new FormHatchBrushViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics / HatchBrushViewer
 *         HatchStyleを すべて表示するプログラム
 *         
 *@subject abstract Brush =>〔~/WinFormSample/ColorRefernce.txt〕
 *@subject ◆HatchBrush : Brush -- System.Drawing.Drawing2D.
 *         new HatchBrush(HatchStyle, Color foreColor)
 *         new HatchBrush(HatchStyle, Color foreColor, Color backColor)
 *         
 *         HatchStyle   hatchBrush.HatchStyle { get; }
 *           └ enum HatchStyle { ... }  〔文末〕
 *         Color ForegroundColor { get; }
 *         Color BackgroundColor { get; }
 *
 *@see ImageHatchBrushViewer.jpg
 *@see ~/CsharpCode/ShowEnumValue.cs
 *@copyTo ~/WinFormSample/GraphicsRefernce.txt
 *@copyTo ~/WinFormSample/ColorRefernce.txt
 *@author shika
 *@date 2022-08-19
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainHatchBrushViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormHatchBrushViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormHatchBrushViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHatchBrushViewer : Form
    {
        public FormHatchBrushViewer()
        {
            this.Text = "FormHatchBrushViewer";
            this.Font = new Font("consolas", 7, FontStyle.Regular);
            this.Size = new Size(700, 650);
            this.BackColor = SystemColors.Window;

            //this.Controls.AddRange(new Control[]
            //{

            //});
        }//constructor

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            Color colorFore = Color.Blue;
            Color colorBack = Color.Azure;
            Pen pen = new Pen(colorFore);
            Brush textBrush = new SolidBrush(colorFore);

            int x = 20; 
            int y = 20;
            // get all items of 'enum HatchStyle'.
            foreach (object value in Enum.GetValues(typeof(HatchStyle)))
            {
                HatchStyle hatch = (HatchStyle)value;
                Brush hatchBrush = new HatchBrush(
                    hatch, colorFore, colorBack);
                
                int index = (int)value;

                // each 10 items return new line.
                x = 20 + (index % 10) * 60 ; 

                if(index % 10 == 0 && index != 0)
                {
                    y += 100; 
                }
                Rectangle rect = new Rectangle(x, y, 50, 50);
                
                // Fill and Draw
                g.FillRectangle(hatchBrush, rect);
                g.DrawRectangle(pen, rect);
                g.DrawString(
                    value.ToString(), 
                    this.Font,
                    textBrush, 
                    new PointF(rect.X, rect.Y + 55 + (index % 2) * 10)); 
                    // more 55px than Rectangle Y coordinate,
                    // each items are alternately different lines.

                hatchBrush.Dispose();
            }//foreach

            pen.Dispose();
            textBrush.Dispose();
            g.Dispose();
        }//OnPaint()
    }//class
}

/* 
enum HatchStyle
{
    Horizontal = 0,
    Horizontal = 0,
    Vertical = 1,
    ForwardDiagonal = 2,
    BackwardDiagonal = 3,
    LargeGrid = 4,
    LargeGrid = 4,   //Cross = 4
    LargeGrid = 4,
    DiagonalCross = 5,
    Percent05 = 6,
    Percent10 = 7,
    Percent20 = 8,
    Percent25 = 9,
    Percent30 = 10,
    Percent40 = 11,
    Percent50 = 12,
    Percent60 = 13,
    Percent70 = 14,
    Percent75 = 15,
    Percent80 = 16,
    Percent90 = 17,
    LightDownwardDiagonal = 18,
    LightUpwardDiagonal = 19,
    DarkDownwardDiagonal = 20,
    DarkUpwardDiagonal = 21,
    WideDownwardDiagonal = 22,
    WideUpwardDiagonal = 23,
    LightVertical = 24,
    LightHorizontal = 25,
    NarrowVertical = 26,
    NarrowHorizontal = 27,
    DarkVertical = 28,
    DarkHorizontal = 29,
    DashedDownwardDiagonal = 30,
    DashedUpwardDiagonal = 31,
    DashedHorizontal = 32,
    DashedVertical = 33,
    SmallConfetti = 34,
    LargeConfetti = 35,
    ZigZag = 36,
    Wave = 37,
    DiagonalBrick = 38,
    HorizontalBrick = 39,
    Weave = 40,
    Plaid = 41,
    Divot = 42,
    DottedGrid = 43,
    DottedDiamond = 44,
    Shingle = 45,
    Trellis = 46,
    Sphere = 47,
    SmallGrid = 48,
    SmallCheckerBoard = 49,
    LargeCheckerBoard = 50,
    OutlinedDiamond = 51,
    SolidDiamond = 52,
}
 => see〔ImageHatchBrushViewer.jpg〕
 */