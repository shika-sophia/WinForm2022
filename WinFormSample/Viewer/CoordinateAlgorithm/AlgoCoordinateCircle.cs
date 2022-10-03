/*
 * 
 *@subject 角度から円上の点  angle from X-Axis => PointF on Circle.
 *         PointF AlgoRadiusPoint(decimal angle, EquationCircle)
 *         
 *@NOTE【註】誤差が多い
 *      // PointF(r * cosθ, r * sinθ)
 *      // ※ when θ = 0,  Y = 0  as expected
 *      // ※ when θ = 90, X = 1.615543E-13 (expected x = 0)
 *      // ※ when θ = 180 Y = 3.231085E-13 (expected y = 0)
 *      // ※ whrn θ = 360 Y = -6.46217E-13 (expected y = 0)
 *      decimal angleRadian = angle / 180M * (decimal) Math.PI;
 *      decimal cos = (decimal)Math.Cos((double)angleRadian);
 *      decimal sin = (decimal)Math.Sin((double)angleRadian);
 *      
 *      return new PointF((float)(radius * cos), (float)(radius * sin));
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class AlgoCoordinateCircle : AlgoCoordinateDifferentiate
    {
        public AlgoCoordinateCircle(PictureBox pic) : base(pic) { }

        public void DrawCircleFunction(EquationCircle eqCircle)
        {
            decimal radius = eqCircle.Radius;
            PointF circleCenterPoint = eqCircle.CircleCenterPoint;
            var rect = new RectangleF(
                (float)(((decimal)circleCenterPoint.X - radius) * scaleRate),
                (float)(((decimal)-circleCenterPoint.Y - radius) * scaleRate),
                (float)(2M * radius * scaleRate), (float)(2M * radius * scaleRate));
            
            g.DrawEllipse(penPink, rect);
            Brush brushPink = penPink.Brush;
            g.FillEllipse(penPink.Brush,
                (float)((decimal)circleCenterPoint.X * scaleRate - 3M),
                (float)((decimal)-circleCenterPoint.Y * scaleRate - 3M), 6f, 6f);

            SizeF textSize = g.MeasureString(eqCircle.Text, font);
            g.DrawString(eqCircle.Text, font, brushPink,
                (float)((decimal)circleCenterPoint.X * scaleRate 
                    - (decimal)textSize.Width / 2M),
                (float)((decimal)rect.Y - (decimal)textSize.Height * 1.5M));

            brushPink.Dispose();
        }//DrawCircleFunction()

        public PointF AlgoRadiusPoint(decimal angle, EquationCircle eqCircle)
        {
            decimal radius = eqCircle.Radius;
            PointF origin = eqCircle.CircleCenterPoint;

            if(Math.Abs(angle) > 360)
            {
                angle = Math.Sign(angle) * angle % 360M;
            }

            if(Math.Abs(angle) == 90 || Math.Abs(angle) == 270)
            {
                decimal sign = ((angle == 90 || angle == -270) ? 1 : -1);
                return new PointF(0, (float)(sign * radius));
            }

            if(Math.Abs(angle) == 180 || Math.Abs(angle) == 360)
            {
                decimal sign = (Math.Abs(angle) == 180 ? -1 : 1);
                return new PointF((float)(sign * radius), 0f);
            }

            // PointF(r * cosθ, r * sinθ)
            decimal angleRadian = angle / 180M * (decimal)Math.PI;
            decimal cos = (decimal)Math.Cos((double)angleRadian);
            decimal sin = (decimal)Math.Sin((double)angleRadian);
            
            return new PointF((float)(radius * cos), (float)(radius * sin));
        }//AlgoRadiusPoint()

        public bool CheckOnLine(PointF pt, EquationCircle eqCircle)
        { 
            float[] x = AlgoCircleFunctionXtoY(pt.X, eqCircle);
            //(Editing...)
            return false;
        }

        public float[] AlgoCircleFunctionXtoY(float x, EquationCircle eqCircle)
        {
            decimal radius = eqCircle.Radius;
            decimal p = (decimal)eqCircle.CircleCenterPoint.X;
            decimal q = (decimal)eqCircle.CircleCenterPoint.Y;

            // (x - p) ^ 2 + (y - q) ^ 2 = r ^ 2
            // (x - p) ^ 2 + y ^ 2 - 2 q y + q ^ 2 = r ^ 2
            float onlyX = AlgoParabolaFunctionXtoY(
                x, 1f, (float)p, 0f);   // y = (x - p) ^ 2 

            // y ^ 2 - 2 q y + q ^ 2 - (x - p) ^ 2 - r ^ 2 = 0
            var eqQuad = new EquationQuadratic(
                a: 1M,
                b: -2M * q,
                c: q * q -(decimal)onlyX - radius * radius);

            return AlgoQuadSolutionFormula(eqQuad);
        }//AlgoCircleFunctionXtoY()

        public EquationLinear AlgoRadiusLine(PointF radiusPoint, EquationCircle eqCircle)
        {
            PointF origin = eqCircle.CircleCenterPoint;

            (float slope, float intercept) = AlgoLinearParam(origin, radiusPoint);

            return new EquationLinear(slope, intercept);
        }//AlgoRadiusLine()

        protected void DrawRariusPointVirticalLineX(PointF radiusPoint)
        {
            penViolet.DashStyle = DashStyle.Dash;
            g.DrawLine(penViolet, 
                (float)((decimal)radiusPoint.X * scaleRate),
                0f,
                (float)((decimal)radiusPoint.X * scaleRate),
                (float)((decimal)-radiusPoint.Y * scaleRate));

            Console.WriteLine(radiusPoint);
            if(radiusPoint.Y == 0) { return; }

            DrawVirticalMark(
                new EquationLinear(0, 0),             // y = 0  (X-Axis)
                new EquationLinear(
                    float.PositiveInfinity,           // x = c  (virtical line)
                    (float)((decimal)radiusPoint.X)),  
                plusX: radiusPoint.X <= 0,            // if + => then -, if - or 0 then +
                plusY: radiusPoint.Y > 0);            // if + => then +, (0 not defined)
        }//DrawRariusPointVirticalLineX()

        public void DrawRadiusLine(PointF radiusPoint, EquationCircle eqCircle)
        {
            PointF origin = eqCircle.CircleCenterPoint;

            penViolet.DashStyle = DashStyle.Solid;
            g.DrawLine(penViolet, 
                origin.X, origin.Y, 
                (float)((decimal)radiusPoint.X * scaleRate),
                (float)((decimal)-radiusPoint.Y * scaleRate));
        }//DrawRadiusLine()

        protected void DrawAngleText(
            decimal angle, PointF radiusPoint, EquationCircle eqCircle)
        {
            PointF origin = eqCircle.CircleCenterPoint;
     
            decimal arcRadius = 10M;
            RectangleF rect = new RectangleF(
                (float)(((decimal)origin.X - arcRadius) * scaleRate),
                (float)(((decimal)-origin.Y - arcRadius) * scaleRate),
                (float)(2M * arcRadius * scaleRate), (float)(2M * arcRadius * scaleRate));
            
            penViolet.DashStyle = DashStyle.Solid;
            g.DrawArc(penViolet, rect, 0, 
                (float)(angle >= 0 ? -angle : -(360 + angle)));

            decimal angleHalf = angle / 2M;
            decimal textAngle = 15M;
            EquationCircle eqCircleText = new EquationCircle(textAngle, origin);
            PointF textLocation = AlgoRadiusPoint(textAngle, eqCircleText);
            SizeF textSize = g.MeasureString(angle.ToString(), fontSmall);
            g.DrawString(angle.ToString(), fontSmall, penViolet.Brush,
                (float)((decimal)textLocation.X * scaleRate - (decimal)textSize.Width / 2M),
                (float)((decimal)-textLocation.Y * scaleRate - (decimal)textSize.Height * scaleRate));
        }//DrawAngleText()

        public void DrawTriangleTheta(decimal angle, EquationCircle eqCircle)
        {
            if (Math.Abs(angle) > 360)
            {
                angle = Math.Sign(angle) * angle % 360M;
            }

            PointF origin = eqCircle.CircleCenterPoint;
            PointF radiusPoint = AlgoRadiusPoint(angle, eqCircle);
            DrawRadiusLine(radiusPoint, eqCircle);
            DrawPointLine(radiusPoint);
            DrawRariusPointVirticalLineX(radiusPoint);
            DrawAngleText(angle, radiusPoint, eqCircle);
        }//DrawTriangleTheta()
    }//class
}
