/** 
 *@title WinFormGUI / WinFormSample / Viewer / FigureAlgorithm
 *@class MainEquilateralTriangleViewer.cs
 *@class   └ new FormEquilateralTriangleViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content EquilateralTriangleViewer
 *         正三角形を描画するアルゴリズム
 *         [英] equilateral triangle:  正三角形
 *         [英] algorithm              アルゴリズム
 *         [幾] Three Square Theorem   三平方の定理  特に30,60,90°は 辺比 2 : 1 : √3
 *         [幾] circumscribe           外接
 *         [幾] inscribe               内接
 *         [幾] radius                 半径
 *         [幾] origin                 原点
 *
 *@subject 浮動小数点数の除算
 *         float, doubleの除算は、バイナリー変換時に誤差が出るので、
 *         decimalに変換して行う。(= 内部的に 10進数で計算 | [Java] BigDecimalクラス)
 *         suffix 接尾辞「M, m」:  decimal型のリテラルを表す記号
 *         
 *@subject AlgoTriangle
 *         PointF[]    AlgoTriangle()
 *                     引数 PointF centerPoint
 *                         decimal LENGTN
 *                         
 *@subject AlgoCircle
 *         RectangleF  AlgoCircle(PointF centerPoint, float radius)
 *         
 *@see ImageEquilateralTriangleViewer.jpg
 *@see 
 *@author shika
 *@date 2022-09-10
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.FigureAlgorithm
{
    class MainEquilateralTriangleViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormEquilateralTriangleViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormEquilateralTriangleViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormEquilateralTriangleViewer : Form
    {
        private readonly PictureBox pic;
        private readonly Pen pen = new Pen(Color.MediumOrchid, 3);
        private readonly PointF centerPoint;    //中心点 = 重心 = 外心 = 内心
        private RectangleF rectCircum;          //外接円
        private RectangleF rectInscribe;        //内接円
        private const decimal LENGTH = 200.0M;  //正三角形の一辺

        public FormEquilateralTriangleViewer()
        {
            this.Text = "FormEquilateralTriangleViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(480, 480);
            this.BackColor = SystemColors.Window;

            pic = new PictureBox()
            {
                ClientSize = new Size(
                    this.Width - 10, this.Height - 20),
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            centerPoint = new PointF(
                (float)(pic.ClientSize.Width / 2M),
                (float)(pic.ClientSize.Height / 2M));
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
                        
            PointF[] triPointAry = AlgoTriangle();

            g.FillEllipse(pen.Brush, 
                centerPoint.X - 2f, centerPoint.Y - 2f, 4, 4);  //中心点
            g.DrawPolygon(pen, triPointAry);       //正三角形
            g.DrawEllipse(Pens.Blue, rectCircum);  //外接円
            g.DrawEllipse(Pens.Red, rectInscribe); //内接円

            pic.Image = bitmap;
            g.Dispose();
        }

        private PointF[] AlgoTriangle()
        {
            decimal root3 = (decimal)Math.Sqrt(3.0d);  // √3
            decimal height = LENGTH / 2M * root3;      // Three Square Theorem

            var pointA = new PointF(                   // △の頂点を Aとする
                centerPoint.X,
                (float)((decimal)centerPoint.Y - height * 2M / 3M));

            var pointB = new PointF(
                (float)((decimal)pointA.X - LENGTH / 2M),
                (float)((decimal)pointA.Y + height));

            var pointC = new PointF(
                (float)((decimal)pointA.X + LENGTH / 2M),
                (float)((decimal)pointA.Y + height));

            rectCircum = AlgoCircle(centerPoint, (float)(height / 3M * 2M));
            rectInscribe = AlgoCircle(centerPoint, (float)(height / 3M));

            return new PointF[]
            {
                pointA, pointB, pointC,
            };
        }//ArgoTriangle()

        private RectangleF AlgoCircle(PointF centerPoint, float radius)
        {
            float oriX = centerPoint.X - radius;  //Rectangleの原点 X座標
            float oriY = centerPoint.Y - radius;  //Rectangleの原点 Y座標
            float width = radius * 2;
            float height = radius * 2;

            return new RectangleF(oriX, oriY, width, height);
        }//AlgoCircle()
    }//class
}
