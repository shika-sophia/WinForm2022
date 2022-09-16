/*
 *@base    AlgoCoordinateAxis
 *@inherit AlgoCoordinateLinear : AlgoCoordinateAxis
 *@used MainLinearFunction.cs
 *
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class AlgoCoordinateLinear : AlgoCoordinateAxis
    {
        public AlgoCoordinateLinear(PictureBox pic) : base(pic) { }

        public void DrawLinearFunction()
        {
            Font font = new Font("ＭＳ 明朝", 12, FontStyle.Bold);
            var pt1 = new PointF(0, 50);
            var pt2 = new PointF(100, 150);
            var (a, b) = AlgoLinearParam(pt1, pt2);

            if(a == Decimal.MaxValue)  // x = c (virtical)
            {
                g.DrawLine(penPink,
                    pt1.X, -centerPoint.Y, 
                    pt1.X, centerPoint.Y);
                g.DrawString($"x = {pt1.X}", font, penPink.Brush, pt1.X + 20, -40);
                return;
            }

            if (a == 0)  // y = b (Horizontal)
            {
                g.DrawLine(penPink,
                    -centerPoint.X, (float)(-b),
                    centerPoint.X, (float)(-b));
                g.DrawString($"y = {b}", font, penPink.Brush, 10f, (float)(-b - 30M));
            }
            else
            {
                float minX = -centerPoint.X;
                float minY = (float)LinearFunctionXtoY((decimal)minX, a, b);
                float maxX = centerPoint.X;
                float maxY = (float)LinearFunctionXtoY((decimal)maxX, a, b);

                g.DrawLine(penPink, minX, -minY, maxX, -maxY);  //Y座標を反転
                g.DrawString($"y = {a} x + {b}", font, penPink.Brush, 20f, (float)(-b));
            }
            
        }//DrawLinearFunction

        private (decimal a, decimal b) AlgoLinearParam(PointF pt1, PointF pt2)
        {
            decimal dx = (decimal)pt1.X - (decimal)pt2.X;
            decimal dy = (decimal)pt1.Y - (decimal)pt2.Y;

            decimal a, b;
            if (dx == 0)
            {
                a = Decimal.MaxValue;     // a = ∞
                b = Decimal.MinValue;     // b = (No Solution)
                return (a, b);
            }
            else if (dy == 0) { a = 0; }  // y = b 
            else { a = dy / dx; }

            b = AlgoLinearParam(a, pt1);

            return (a, b);
        }//AlgoLinerParam(pt1, pt2)

        private decimal AlgoLinearParam(decimal a, PointF pt1)
        {
            // b = y - a x
            return (decimal)pt1.Y - a * (decimal)pt1.X;
        }

        private decimal LinearFunctionXtoY(decimal x, decimal a, decimal b)
        {
            // y = a x + b
            return a * x + b;
        }//AlgoLinearFunction(x) -> y

        private decimal LinearFunctionYtoX(decimal y, decimal a, decimal b)
        {
            decimal x;
            if (a == 0 || a == Decimal.MinValue)  
            {
                x = Decimal.MaxValue;
            }
            else
            {
                x = (y - b) / a;
            }

            return x;
        }//AlgoLinearFunction(y) -> x
    }//class
}
