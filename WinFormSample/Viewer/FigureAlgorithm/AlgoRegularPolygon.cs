/** 
 *@title WinFormGUI / WinFormSample / Viewer / FigureAlgorithm
 *@class AlgoRegularPolygon.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content AlgoRegularPolygon  正ｎ角形のアルゴリズム・クラス
 *         ・各Formクラスは Formを継承するため、他クラスの継承不可
 *         ・プロパティに AlgoRegularPolygonを newして委譲しておくと
 *           「algo.AlgoXxxx()]で各アルゴリズム・メソッドを呼び出せる。
 *           
 *         private readonly AlgoRegularPolygon algo = new AlgoRegularPolygon();
 *         
 *@subject PointF[] AlgoMultiAngle(
 *            PointF centerPoint, decimal radius, int num)
 *
 *@subject //void DrawStar()
 *         ・PictureBox, Bitmap, Graphics, Penの各クラスが必要。
 *         ・Formクラスに コピー用
 *         ・ダイアモンド構造の call
 *              DrawStar()
 *                ↓ call         ↓ call 
 *              AlgoOddStar() | AlgoEvenStar()  --> AlgoMultiAngle()
 *                ↓ call         ↓ call
 *              AlgoSkipPoint()
 *              
 *@subject GraphicsPath AlgoOddStar(
 *            PointF centerPoint, decimal radius, int num)
 *@subject GraphicsPath[] AlgoEvenStar(
 *            PointF centerPoint, decimal radius, int num)
 *@subject PointF[] AlgoSkipPoint(
 *            int NUM_ANGLE, PointF[] multiPointAry, int start = 0)
 *            
 *@subject RectangleF AlgoCircle(PointF centerPoint, decimal radius)
 *                      ↑ call
 *@subject RectangleF AlgoSqure(PointF centerPoint, decimal length)
 *
 *@see MainMultiAngleViewer.cs
 *@see MainMultiStarViewer.cs
 *@see MainSqureCircleViewer.cs
 *@author shika
 *@date 2022-09-14
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.FigureAlgorithm
{
    class AlgoRegularPolygon
    {
        public AlgoRegularPolygon() { } //constructor

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

        //private void DrawStar()
        //{
        //    Bitmap bitmap = new Bitmap(
        //        pic.ClientSize.Width, pic.ClientSize.Height);
        //    var g = Graphics.FromImage(bitmap);
        //    g.SmoothingMode = SmoothingMode.HighQuality;

        //    //---- OddStar ----            
        //    if (NUM % 2 == 1)
        //    {
        //        GraphicsPath gPath = AlgoOddStar(centerPoint, RADIUS, NUM);
        //        g.FillPath(penBlue.Brush, gPath);
        //        //g.DrawPath(penBlue, gPath);
        //    }

        //    //---- EvenStar ----
        //    if (NUM % 2 == 0)
        //    {
        //        GraphicsPath[] gPathAry = AlgoEvenStar(centerPoint, RADIUS, NUM);

        //        //g.FillPath(penPink.Brush, gPathAry[0]);
        //        //g.FillPath(penPink.Brush, gPathAry[1]);
        //        g.DrawPath(penPink, gPathAry[0]);
        //        g.DrawPath(penPink, gPathAry[1]);
        //    }

        //    penBlue.Dispose();
        //    penPink.Dispose();
        //    g.Dispose();

        //    pic.Image = bitmap;
        //}//DrawStar()

        private GraphicsPath AlgoOddStar(PointF centerPoint, decimal radius, int num)
        {
            PointF[] multiPointAry = AlgoMultiAngle(centerPoint, radius, num);
            GraphicsPath gPath = new GraphicsPath(FillMode.Winding);

            PointF[] starPointAry = AlgoSkipPoint(num, multiPointAry);
            gPath.AddLines(starPointAry);

            return gPath;
        }//AlgoOddStar()

        private GraphicsPath[] AlgoEvenStar(PointF centerPoint, decimal radius, int num)
        {
            PointF[] multiPointAry = AlgoMultiAngle(centerPoint, radius, num);
            var gPath0 = new GraphicsPath(FillMode.Winding);

            PointF[] harfPointAry = AlgoSkipPoint(num, multiPointAry, start: 0);
            gPath0.AddLines(harfPointAry);

            var gPath1 = new GraphicsPath(FillMode.Winding);
            harfPointAry = AlgoSkipPoint(num, multiPointAry, start: 1);
            gPath1.AddLines(harfPointAry);

            return new GraphicsPath[] { gPath0, gPath1 };
        }//AlgoEvenStar()

        private PointF[] AlgoSkipPoint(
            int NUM_ANGLE, PointF[] multiPointAry, int start = 0)
        {
            int numPoint = (NUM_ANGLE % 2 == 0) ? (NUM_ANGLE / 2) : NUM_ANGLE;

            PointF[] starPointAry = new PointF[numPoint + 1];
            int index = start;
            for (int i = 0; i < starPointAry.Length; i++)
            {
                starPointAry[i] = multiPointAry[index];
                index = (index + 2) % NUM_ANGLE;
            }//for

            return starPointAry;
        }//AlgoSkipPoint()

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
