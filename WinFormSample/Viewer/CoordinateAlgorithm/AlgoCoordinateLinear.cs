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
        protected readonly PointF pt1;
        protected readonly PointF pt2;

        public AlgoCoordinateLinear(PictureBox pic) : base(pic)
        {
            // y = 1 x + 50
            this.pt1 = new PointF(0, 50);
            this.pt2 = new PointF(100, 150);

            // y = 100
            //this.pt1 = new PointF(0, 100);
            //this.pt2 = new PointF(100, 100);

            // x = -100
            //this.pt1 = new PointF(-100, 50);
            //this.pt2 = new PointF(-100, 150);
        }

        public AlgoCoordinateLinear(
            PictureBox pic, PointF pt1, PointF pt2) : base(pic)
        {
            this.pt1 = pt1;
            this.pt2 = pt2;
        }

        public void DrawLinearFunction()
        {
            DrawLinearFunction(this.pt1, this.pt2);
        }

        public void DrawLinearFunction(PointF pt1, PointF pt2) 
        {
            var (a, b) = AlgoLinearParam(pt1, pt2);

            if(Double.IsInfinity(a))  // x = c (virtical)
            {
                g.DrawLine(penPink,
                    pt1.X, -centerPoint.Y, 
                    pt1.X, centerPoint.Y);
                g.DrawString($"x = {pt1.X}", font, penPink.Brush, pt1.X + 10, -centerPoint.Y + 20);
                return;
            }

            if (a == 0)  // y = b (Horizontal)
            {
                g.DrawLine(penPink,
                    -centerPoint.X, (float)(-b),
                    centerPoint.X, (float)(-b));
                g.DrawString($"y = {b}", font, penPink.Brush, 
                    centerPoint.X - 70, (float)(-b + 20));
            }
            else
            {
                float minX = -centerPoint.X;
                float minY = (float)LinearFunctionXtoY((double)minX, a, b);
                float maxX = centerPoint.X;
                float maxY = (float)LinearFunctionXtoY((double)maxX, a, b);

                g.DrawLine(penPink, minX, -minY, maxX, -maxY);  //Y座標を反転
                g.DrawString($"y = {a} x + {b}", font, penPink.Brush,
                    120f, (float)(- LinearFunctionXtoY(120d, a, b) + 20));
            }           
        }//DrawLinearFunction

        private (double a, double b) AlgoLinearParam(PointF pt1, PointF pt2)
        {
            decimal dx = (decimal)pt1.X - (decimal)pt2.X;
            decimal dy = (decimal)pt1.Y - (decimal)pt2.Y;

            double a, b;
            if (dx == 0M)
            {
                a = dy > 0M ? Double.PositiveInfinity : Double.NegativeInfinity;     // a = ∞ or -∞
                b = Double.NaN;     // b = (No Solution)
                return (a, b);
            }
            else if (dy == 0M) { a = 0d; }  // y = b 
            else { a = (double)(dy / dx); }

            b = AlgoLinearParam(a, pt1);

            return (a, b);
        }//AlgoLinerParam(pt1, pt2)

        private double AlgoLinearParam(double a, PointF pt1)
        {
            // b = y - a x
            return (double)((decimal)pt1.Y - (decimal)a * (decimal)pt1.X);
        }

        private double LinearFunctionXtoY(double x, double a, double b)
        {
            // y = a x + b
            return (double)((decimal)a * (decimal)x + (decimal)b);
        }//AlgoLinearFunction(x) -> y

        private double LinearFunctionYtoX(double y, double a, double b)
        {
            double x;
            if (a == 0 || Double.IsInfinity(a))  
            {
                x = Double.NaN;
            }
            else
            {
                x = (double)(((decimal)y - (decimal)b) / (decimal)a);
            }

            return x;
        }//AlgoLinearFunction(y) -> x
    }//class
}
