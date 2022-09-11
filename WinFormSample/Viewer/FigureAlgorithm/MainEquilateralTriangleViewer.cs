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
 *@subject AlgoTriangle 中心点と一辺から、正三角形の３点を計算
 *         PointF[]    AlgoTriangle(PointF centerPoint, decimal length)
 *                     引数 PointF centerPoint:  中心点
 *                         decimal length:       正三角形の一辺
 *                         
 *         height: 正三角形の上頂点から底辺への垂線
 *                 三平方の定理より、30, 60, 90°の 辺比は 2 : 1 : √3
 *                 height = length / 2 * √3
 *         
 *         pointA: 正三角形の上頂点をAとする
 *                 X座標は 中心点の X座標と同じ 
 *                 Y座標は 中心点の Y座標より、heightの 2 / 3 だけ上方(-)
 *                 
 *         pointB: 正三角形の底点左
 *                 X座標は 中心点の X座標より、length の 1 / 2 だけ左方(-)
 *                 Y座標は 中心点の Y座標より、heightの 1 / 3 だけ下方(+)
 *                       = pointAのY座標より、heigthだけ下方(+)
 *                 
 *         pointC: 正三角形の底点右
 *                 X座標は 中心点の X座標より、length の 1 / 2 だけ右方(+)
 *                 Y座標は 中心点の Y座標より、heightの 1 / 3 だけ下方(+)
 *                       = pointAのY座標より、heigthだけ下方(+)
 *                       
 *         外接円:  heigthの 2 / 3を半径とする円
 *         内接円:  heigthの 1 / 3を半径とする円
 *
 *@subject AlgoCircle
 *         =>〔MainSqureCircleViewer.cs〕
 *@see ImageEquilateralTriangleViewer.jpg
 *@see MainSqureCircleViewer.cs
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
                        
            PointF[] triPointAry = AlgoTriangle(centerPoint, LENGTH);

            g.FillEllipse(pen.Brush,               //中心点
                (float)((decimal)centerPoint.X - 2M), 
                (float)((decimal)centerPoint.Y - 2M), 4, 4);
            g.DrawPolygon(pen, triPointAry);       //正三角形
            g.DrawEllipse(Pens.Blue, rectCircum);  //外接円
            g.DrawEllipse(Pens.Red, rectInscribe); //内接円

            pic.Image = bitmap;
            g.Dispose();
        }//DrawFigure()

        private PointF[] AlgoTriangle(PointF centerPoint, decimal length)  //中心点, 一辺の長さ
        {
            decimal root3 = (decimal)Math.Sqrt(3d);    // √3
            decimal height = length / 2M * root3;      // Three Square Theorem

            var pointA = new PointF(                   // △の上頂点を Aとする
                centerPoint.X,
                (float)((decimal)centerPoint.Y - height * 2M / 3M));

            var pointB = new PointF(
                (float)((decimal)pointA.X - length / 2M),
                (float)((decimal)pointA.Y + height));

            var pointC = new PointF(
                (float)((decimal)pointA.X + length / 2M),
                (float)((decimal)pointA.Y + height));

            rectCircum = AlgoCircle(centerPoint, (height / 3M * 2M));
            rectInscribe = AlgoCircle(centerPoint, (height / 3M));

            return new PointF[]
            {
                pointA, pointB, pointC,
            };
        }//ArgoTriangle()

        private RectangleF AlgoCircle(PointF centerPoint, decimal radius)
        {
            float oriX = (float)((decimal)centerPoint.X - radius);  //Rectangleの原点 X座標
            float oriY = (float)((decimal)centerPoint.Y - radius);  //Rectangleの原点 Y座標
            float width = (float)(radius * 2M);
            
            return new RectangleF(oriX, oriY, width, width);
        }//AlgoCircle()

    }//class
}
