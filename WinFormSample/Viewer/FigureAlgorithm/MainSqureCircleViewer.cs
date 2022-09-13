/*
//==== Template DrawFigure() ====

    [STAThread]
    static void Main()
    //public void Main()
    {
        Console.WriteLine("new Form1()");

        Application.EnableVisualStyles();
        Application.Run(new Form1());

        Console.WriteLine("Close()");
    }//Main()
}//class

class Form1 : Form
{
    private readonly PictureBox pic;
    private readonly PointF centerPoint;
    private const decimal RADIUS = 150M;

    public Form1()
    {
        this.Text = "Form1";
        this.Font = new Font("consolas", 12, FontStyle.Regular);
        this.Size = new Size(480, 480);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.BackColor = SystemColors.Window;

        pic = new PictureBox()
        {
            ClientSize = this.ClientSize,
            BorderStyle = BorderStyle.Fixed3D,
            Dock = DockStyle.Fill,
        };

        centerPoint = new PointF(
            (float)((decimal)pic.ClientSize.Width / 2M),
            (float)((decimal)pic.ClientSize.Height / 2M));

        DrawFigure();

        this.Controls.AddRange(new Control[]
        {
            pic,
        });
    }//constructor

    private void DrawFigure()
    {
        Bitmap bitmap = new Bitmap(
            pic.ClientSize.Width, pic.ClientSize.Height);
        var g = Graphics.FromImage(bitmap);
        g.SmoothingMode = SmoothingMode.HighQuality;
        Pen penBlue = new Pen(Color.CornflowerBlue, 3);
        Pen penPink = new Pen(Color.HotPink, 1);

        //---- centerPoint ----
        g.FillEllipse(penBlue.Brush,
            (float)((decimal)centerPoint.X - 2M),
            (float)((decimal)centerPoint.Y - 2M), 4, 4);

        penBlue.Dispose();
        penPink.Dispose();
        g.Dispose();
        pic.Image = bitmap;
    }//DrawFigure()
}//class

*/
/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainSqureCircleViewer.cs
 *@class   └ new FormSqureCircleViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content SqureCircleViewer
 *@subject AlgoCircle  中心点と半径から、円の外接四角形を計算
 *         RectangleF  AlgoCircle(PointF centerPoint, decimal radius)
 *                     引数 PointF centerPoint:  中心点
 *                          decimal radius:      半径
 *                          
 *         rectX: 円の外接四角形の原点(左上隅) X座標
 *                中心点より radiusだけ 左方(-)
 *               
 *         rectY: 円の外接四角形の原点(左上隅) Y座標
 *                中心点より radiusだけ 上方(-)
 *              
 *         width: 円の外接四角形は正方形になり、一辺は 円の直径に等しい
 *         
 *@subject AlogoSqure  中心点と一辺から、正方形のRectangleFを計算
 *         RectangleF  AlogoSqure(PointF centerPoint, decimal length)
 *         引数  centerPoint: 中心点
 *               length:      正方形の一辺
 *               
 *         円の直径 = 正方形の一辺 なので、
 *         円の半径 = length / 2  となり、AlgoCircle()を適用
 *
 *@NOTE【Problem】Font
 *      ・g.DrawString()で描画される文字は Fontによって、きれいに描画できない場合がある
 *      ・Fontの文字に 横幅が太い字体は、とても太くなることがある。
 *      ・太くなると、他の色が混ざることがある
 *      ・Font.Size, Pen.Widthが影響している様子
 *      ・"consolas", "Times New Roman"は NG
 *      ・"ＭＳ 明朝", "ＭＳ ゴシック"は OK
 *      
 *@see ImageSqureCircleViewer.jpg
 *@see 
 *@author shika
 *@date 2022-09-11
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.FigureAlgorithm
{
    class MainSqureCircleViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormSqureCircleViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormSqureCircleViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormSqureCircleViewer : Form
    {
        private readonly PictureBox pic;
        private readonly PointF centerPoint;
        private const decimal LENGTH = 200.0M;

        public FormSqureCircleViewer()
        {
            this.Text = "FormSqureCircleViewer";
            this.Font = new Font("ＭＳ 明朝", 10, FontStyle.Bold);
            this.Size = new Size(480, 480);
            this.BackColor = SystemColors.Window;

            pic = new PictureBox()
            {
                ClientSize = new Size(
                    this.ClientSize.Width - 10,
                    this.ClientSize.Height - 10),
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            centerPoint = new PointF(
                (float)((decimal)pic.ClientSize.Width / 2M),
                (float)((decimal)pic.ClientSize.Height / 2M));

            DrawFigure();

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor

        private void DrawFigure()
        {
            Bitmap bitmap = new Bitmap(
                pic.ClientSize.Width, pic.ClientSize.Height);
            var g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;

            Pen pen = new Pen(Color.CornflowerBlue, 2);
            Pen penPink = new Pen(Color.HotPink, 0.75f);
            RectangleF rect = AlgoSqure(centerPoint, LENGTH);

            //---- centerPoint ----
            g.FillEllipse(pen.Brush, 
                (float)((decimal)centerPoint.X - 3M),
                (float)((decimal)centerPoint.Y - 3M), 6, 6);

            //---- Squre ----
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

            //---- Circle ----
            g.DrawEllipse(penPink, rect);

            //---- centerLine ----
            g.DrawLine(penPink,
                rect.X, centerPoint.Y,
                (float)((decimal)rect.X + (decimal)rect.Width), centerPoint.Y);
            g.DrawLine(penPink,
                centerPoint.X, rect.Y,
                centerPoint.X, (float)((decimal)rect.Y + (decimal)rect.Height));

            //---- "r" ----
            g.DrawString("r", this.Font, penPink.Brush,
                (float)((decimal)centerPoint.X + 10M),
                (float)((decimal)centerPoint.Y - LENGTH / 4M - 10M));

            g.DrawString("r", this.Font, penPink.Brush,
                (float)((decimal)centerPoint.X - LENGTH / 4M - 10M),
                (float)((decimal)centerPoint.Y + 10M));

            //---- "length" ----
            pen.Width = 0.5f;
            pen.DashStyle = DashStyle.Dash;

            // Left
            g.DrawCurve(pen, new PointF[]
            {
                rect.Location,
                new PointF(
                    (float)((decimal)rect.X - 20M), centerPoint.Y),
                new PointF(rect.X, rect.Bottom),
            },
            tension: 0.9f);

            g.DrawString("length", this.Font, pen.Brush,
                (float)((decimal)rect.X - 75M),
                (float)((decimal)centerPoint.Y - 8M));

            // Top
            g.DrawCurve(pen, new PointF[]
            {
                rect.Location,
                new PointF(
                     centerPoint.X, (float)((decimal)rect.Y - 20M)),
                new PointF(rect.Right, rect.Y),
            },
            tension: 0.9f);

            g.DrawString("length", this.Font, pen.Brush,
                (float)((decimal)rect.X + 78M),
                (float)((decimal)rect.Y - 42M));

            //---- Dispose() ----
            pen.Dispose();
            penPink.Dispose();
            g.Dispose();

            //---- Deplyment ----
            pic.Image = bitmap;
        }//DrawFigure()

        private RectangleF AlgoCircle(PointF centerPoint, decimal radius)
        {
            float rectX = (float)((decimal)centerPoint.X - radius);  //Rectangleの原点 X座標
            float rectY = (float)((decimal)centerPoint.Y - radius);  //Rectangleの原点 Y座標
            float width = (float)(radius * 2M);

            return new RectangleF(rectX, rectY, width, width);
        }//AlgoCircle()

        private RectangleF AlgoSqure(PointF centerPoint, decimal length)
        {
            return AlgoCircle(centerPoint, length / 2M);  //円の直径 = 正方形の一辺
        }//AlgoSqure()
    }//class
}
