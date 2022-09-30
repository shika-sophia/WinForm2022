/** 
 *@title WinFormGUI / WinFormSample / Viewer / FigureAlgorithm
 *@class MainMultiAngleViewer.cs
 *@class   └ new FormMultiAngleViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content MultiAngleViewer 
 *         正n角形の描画
 *         
 *@subject AlgoMultiAngle
 *         ・正n角形は中心角が均等なので、三角比を利用して各点の座標を算出
 *         ・AlgoTriangle(), AlgoHexagon()は 三平方の定理で算出したが、もはや必要なくなり、
 *           AlogoMultiAngle()において、正n角形の全てに対応しているので、
 *           n = 3, 4, 5, ...を入れ替えるだけでいい。
 *
 *@subject 三角比 Triangular ratio
 *         直角三角形において 角θのとき、三角形の辺比を以下のように定義する
 *                                 . (a, b)
 *         sinθ = b / c           /|  
 *         cosθ = a / c       c ／ ｜ b
 *         tanθ = b / a       ／    |
 *                        Ｏ ∠θ___┌| 
 *                              a
 *         例 三平方の定理より、 sin60° = b / c = √3 / 2
 *                             cos60° = a / c =  1 / 2
 *                          
 *         原点Ｏ(0, 0) を中心点とする半径 r の円において、x軸との角 θ とすると、
 *         a = X座標, b = Y座標 となる。
 *         a, b は c, θによって変化するが、
 *         c = r のとき (r * cosθ) = r * a / r = a 
 *                     (r * sinθ) = r * b / r = b  
 *         つまり、c = r のとき、円上の点の座標は (r * cosθ, r * sinθ) で表せる。
 *         正n角形の頂点ごとに中心角を増やしていくと、sinθ, cosθの正負の符号も切り替わり、
 *         正n角形の各点を算出できる
 *         
 *@subject Mathクラス
 *         double  Math.Sin(double radian)
 *         double  Math.Cos(double radian)
 *         double  Math.Tan(double radian)
 *         引数: 角のラジアン単位  
 *               ラジアン = θπ / 180  (π: 円周率)
 *                       = θ * Math.PI / 180
 *               
 *         正n角形の中心角は θ = 360 / n
 *         
 *         decimal centerAngleRadian = 
 *             (360M / (decimal)num) * (decimal)Math.PI / 180M;
 *         
 *@NOTE【考察】
 *      数学の原点(0, 0)に対し (+, +)領域は 右上方向だが、
 *      Graphicsの原点は 画面の左上隅で、(+, +)領域は 右下方向になる。
 *      数学座標を 時計回りに 90°回転すると、Graphics座標になる。
 *      そして、中心点の座標から +- する必要がある。
 *      
 *      数学のように、X座標 = r * cosθ, Y座標 = r * sinθとすると、
 *      上頂点からではなく、他のところから始まるが、(初期角を +90° / -90° しても解決しない)
 *      それでも図形は ちゃんと描画できる。
 *      上頂点から始めるには、sin, cosを入れ替えて 反時計回りで座標を算出した
 *      
 *      
 *      for (int i = 0; i < num; i++)
 *      {
 *          double angle = (double)((decimal)centerAngleRadian * i);
 *          
 *          multiPointAry[i] = new PointF(
 *              (float)((decimal)centerPoint.X - radius * (decimal)Math.Sin(angle)),
 *              (float)((decimal)centerPoint.Y - radius * (decimal)Math.Cos(angle)));
 *      }//for
 *      
 *      例 NUM = 6のとき、中心角 360 / 6 = 60°
 *      i = 0:  angle = 60 * 0 = 0
 *              sin0 = 0, cos0 = 1
 *              new PointF( centerPoint.X - radius * 0,
 *                          centerPoint.Y - radius * 1);
 *              point0 は 中心点から 
 *              radius分 X座標そのままで、Y座標は radius分だけ上方(-)
 *              
 *      i = 1:  angle = 60 * 1 = 60
 *              sin60 = √3 / 2, cos60 = 1 / 2
 *              new PointF( centerPoint.X - radius * (√3 / 2),
 *                          centerPoint.Y - radius * ( 1 / 2),
 *              point1 は 中心点より X座標 左方(-), Y座標 radiusの半分だけ上方(-)
 *              
 *      i = 2:  angle = 60 * 2 = 120
 *              sin120 = √3 / 2, cos120 = -(1 /2)
 *              new PointF( centerPoint.X - radius * (√3 / 2),
 *                          centerPoint.Y - radius * (-1 / 2),
 *              point2 は 中心点より X座標　左方(-), Y座標 radiusの半分だけ 下方(+)
 *        : 
 *        :
 *        
 *@subject【公式】Formula: 
 *         三角比の定義より、tanθ = sinθ / cosθ
 *         三平方の定理より、cos^2 θ + sin^2 θ = 1
 *         両式の連立により、tan^2 θ = 1 / cos^2 θ
 *         
 *         これらの公式により、cosθ, sinθ, tanθ のいずれかの値がわかると、
 *         他のすべての値を求めることができる。
 *         =>〔CoordinateAlgorithm/AlgoCoordinateDifferenciate.AlgoDiatanceOnLine()〕
 *         
 *@see ImageMultiAngleViewer.jpg
 *@see CoordinateAlgorithm/AlgoCoordinateDifferenciate.AlgoDiatanceOnLine()
 *@author shika
 *@date 2022-09-12
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.FigureAlgorithm
{
    class MainMultiAngleViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormMultiAngleViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormMultiAngleViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMultiAngleViewer : Form
    {
        private readonly PictureBox pic;
        private readonly PointF centerPoint;
        private const decimal RADIUS = 150M;
        private const int NUM = 5;

        public FormMultiAngleViewer()
        {
            this.Text = "FormMultiAngleViewer";
            this.Font = new Font("ＭＳ 明朝", 12, FontStyle.Bold);
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

            //---- Equilater n angle, centerPoint ----
            PointF[] multiPointAry = AlgoMultiAngle(centerPoint, RADIUS, NUM);
            g.DrawPolygon(penBlue, multiPointAry);
            g.FillEllipse(new SolidBrush(penBlue.Color),
                (float)((decimal)centerPoint.X - 3M),
                (float)((decimal)centerPoint.Y - 3M), 6, 6);

            //---- centerAngle Line ----
            penBlue.Width = 0.5f;
            penBlue.DashStyle = DashStyle.Dash;
            g.DrawLines(penBlue, new PointF[]
            {
                multiPointAry[0], centerPoint, multiPointAry[NUM - 1]
            });

            //---- centerAngle Text ----
            string centerAngleStr = $"{Math.Round(360M / (decimal)NUM, 1):###.0}°";
            g.DrawString(centerAngleStr, this.Font, penBlue.Brush,
                (float)((decimal)centerPoint.X + 3M),
                (float)((decimal)centerPoint.Y - 30M));

            //---- circumscribed Circle, n Text ----
            RectangleF rect = AlgoCircle(centerPoint, RADIUS);
            g.DrawEllipse(penPink, rect);
            g.DrawString($"ｎ = {NUM}", this.Font, penBlue.Brush, rect);
            
            pic.Image = bitmap;
        }//DrawFigure()

        private PointF[] AlgoMultiAngle(PointF centerPoint, decimal radius, int num)
        {
            decimal centerAngleRadian = (360M / (decimal)num) * (decimal)Math.PI / 180M;

            PointF[] multiPointAry = new PointF[num];

            for (int i = 0; i < num; i++)
            {
                double angle = (double)((decimal)centerAngleRadian * i);

                multiPointAry[i] = new PointF(
                    (float)((decimal)centerPoint.X - radius * (decimal)Math.Sin(angle)),
                    (float)((decimal)centerPoint.Y - radius * (decimal)Math.Cos(angle)));
            }//for

            return multiPointAry;
        }//AlgoMultiAngle()

        private RectangleF AlgoCircle(PointF centerPoint, decimal radius)
        {
            float rectX = (float)((decimal)centerPoint.X - radius);  //Rectangleの原点 X座標
            float rectY = (float)((decimal)centerPoint.Y - radius);  //Rectangleの原点 Y座標
            float width = (float)(radius * 2M);

            return new RectangleF(rectX, rectY, width, width);
        }//AlgoCircle()
    }//class
}

/*
//---- Test Print ----
Console.WriteLine($"NUM: {NUM}");
Console.WriteLine($"RADIUS: {RADIUS}");
Console.WriteLine($"centerPoint: ({centerPoint.X} , {centerPoint.Y})");
Console.WriteLine($"centerAngle: {centerAngle}");
Console.WriteLine("multiPointAry:");
foreach(PointF point in multiPointAry)
{
    Console.WriteLine($"  ({point.X} , {point.Y})");
}//foreach

new FormMultiAngleAlgorithm()
NUM: 5
RADIUS: 150
centerPoint: (232 , 220.5)
centerAngle: 1.256637061435916
multiPointAry:
  (232 , 70.5)
  (89.34152 , 174.1474)
  (143.8322 , 341.8525)
  (320.1678 , 341.8525)
  (374.6585 , 174.1474)
Close()

new FormMultiAngleAlgorithm()
NUM: 8
RADIUS: 150
centerPoint: (232 , 220.5)
centerAngle: 0.7853981633974475
multiPointAry:
  (232 , 70.5)
  (125.934 , 114.434)
  (82 , 220.5)
  (125.934 , 326.566)
  (232 , 370.5)
  (338.066 , 326.566)
  (382 , 220.5)
  (338.066 , 114.434)
Close()
*/