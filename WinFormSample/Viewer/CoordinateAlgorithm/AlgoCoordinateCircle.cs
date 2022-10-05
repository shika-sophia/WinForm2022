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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class AlgoCoordinateCircle : AlgoCoordinateDifferentiate
    {
        public AlgoCoordinateCircle(PictureBox pic) : base(pic) { }

        //====== Draw ======
        public void DrawMultiCircleFunction(ICoordinateEquation[] eqAry, params PointF[] pointAryArgs)
        {
            List<PointF> pointList = new List<PointF>(pointAryArgs);

            for(int i = 0; i < eqAry.Length; i++)
            {
                ICoordinateEquation eq = eqAry[i];
                pointList.AddRange(eq.GetEqPointAry());
                
                for(int j = i; j < eqAry.Length; j++)
                {
                    if(j == i) { continue; }
                    //(Editing...)
                }
            }//for i
        }//DrawMultiFunctionCircle()

        public void DrawTriangleTheta(decimal angle, EquationCircle eqCircle)
        {
            if (Math.Abs(angle) > 360)
            {
                angle %= 360M;
            }

            PointF radiusPoint = AlgoRadiusPoint(angle, eqCircle);
            DrawRadiusLine(radiusPoint, eqCircle);
            DrawPointLine(radiusPoint);
            DrawRariusPointVirticalLineX(radiusPoint);
            DrawAngleArc(angle, eqCircle);
            DrawAngleNarrow(angle, eqCircle);
        }//DrawTriangleTheta()

        public void DrawCircleFunction(EquationCircle eqCircle)
        {
            decimal radius = eqCircle.Radius;
            PointF circleCenterPoint = eqCircle.CircleCenterPoint;
            var rect = BuildCircleRectangle(radius, circleCenterPoint);
            
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

        public void DrawRadiusLine(PointF radiusPoint, EquationCircle eqCircle)
        {
            PointF origin = eqCircle.CircleCenterPoint;

            penViolet.DashStyle = DashStyle.Solid;
            penViolet.Width = 1.0f;
            penViolet.CompoundArray = new float[] { 0.0f, 1.0f };
            g.DrawLine(penViolet,
                origin.X, origin.Y,
                (float)((decimal)radiusPoint.X * scaleRate),
                (float)((decimal)-radiusPoint.Y * scaleRate));
        }//DrawRadiusLine()

        protected void DrawRariusPointVirticalLineX(PointF radiusPoint)
        {
            penViolet.DashStyle = DashStyle.Dash;
            penViolet.Width = 1.0f;
            penViolet.CompoundArray = new float[] { 0.0f, 1.0f };

            g.DrawLine(penViolet,
                (float)((decimal)radiusPoint.X * scaleRate),
                0f,
                (float)((decimal)radiusPoint.X * scaleRate),
                (float)((decimal)-radiusPoint.Y * scaleRate));

            if (radiusPoint.Y == 0) { return; }

            DrawVirticalMark(
                new EquationLinear(0, 0),             // y = 0  (X-Axis)
                new EquationLinear(
                    float.PositiveInfinity,           // x = c  (virtical line)
                    (float)((decimal)radiusPoint.X)),
                plusX: radiusPoint.X <= 0,            // if + => then -, if - or 0 then +
                plusY: radiusPoint.Y > 0);            // if + => then +, (0 not defined)
        }//DrawRariusPointVirticalLineX()

        protected RectangleF BuildCircleRectangle(
            decimal radius, PointF circleCenterPoint)
        {
            return new RectangleF(
                (float)(((decimal)circleCenterPoint.X - radius) * scaleRate),
                (float)(((decimal)-circleCenterPoint.Y - radius) * scaleRate),
                (float)(2M * radius * scaleRate), (float)(2M * radius * scaleRate));
        }//BuildCircleRectangle()

        protected void DrawAngleArc(
            decimal angle, EquationCircle eqCircle)
        {
            // draw absolute angle arc from X-Axis plus
            // X軸+から常に正の絶対角を描画

            if (Math.Abs(angle) > 360)
            {
                angle %= 360M;
            }

            if (angle == 90) { return; }

            decimal arcRadius = 10M;
            RectangleF rect = BuildCircleRectangle(arcRadius, eqCircle.CircleCenterPoint);

            penViolet.DashStyle = DashStyle.Solid;
            penViolet.Width = 1.0f;
            penViolet.CompoundArray = new float[] { 0.0f, 1.0f };
            g.DrawArc(penViolet, rect, 0,
                (float)(angle >= 0 ? -angle : -(360 + angle)));  // 時計回り角を指定

            DrawAngleText(angle, eqCircle.CircleCenterPoint);
        }//DrawAngleArc()        

        protected void DrawAngleNarrow(decimal angle, EquationCircle eqCircle)
        {
            // draw narrow angle from X-Axis both side
            // X軸+- との鋭角を描画
            
            if (Math.Abs(angle) > 360)
            {
                angle %= 360M;
            }
            angle = (angle > 0) ? angle : 360 + angle; // to absolute angle
            //Console.WriteLine($"angle = {angle}");
            
            if (0 <= angle && angle <= 90 || angle == 270)
            {
                return;
            }

            decimal narrowAngle = angle;
            decimal startAngle = 0M;

            if (90 < angle && angle <= 180)
            {
                narrowAngle = 180 - angle;
                startAngle = 360 - angle - narrowAngle;  //時計回り角
            }
            else if (180 < angle && angle < 270)
            {
                narrowAngle = angle - 180;
                startAngle = 180M - narrowAngle;
            }
            else if (270 < angle)
            {
                narrowAngle = 360 - angle;
            }
            //Console.WriteLine($"narrowAngle = {narrowAngle}");
            //Console.WriteLine($"startAngle = {startAngle}");

            decimal narrowRadius = 12M;
            RectangleF rect = BuildCircleRectangle(narrowRadius, eqCircle.CircleCenterPoint);
            penViolet.DashStyle = DashStyle.Solid;
            penViolet.Width = 3.0f;
            penViolet.CompoundArray =
                new float[] { 0.0f, 0.3f, 0.7f, 1.0f };
            g.DrawArc(penViolet, rect, (float)startAngle, (float)narrowAngle);

            DrawAngleText(narrowAngle, eqCircle.CircleCenterPoint, angle);
        }//DrawAngleNarrow()

        protected void DrawAngleText(decimal angle, PointF cicleCenterPoint, decimal startAngle = 0M)
        {
            if (Math.Abs(angle) > 360)
            {
                angle %= 360M;
            }
            angle = (angle >= 0) ? angle : -(360 + angle);

            if(180 < startAngle && startAngle < 270)
            {
                startAngle = 180;
            }

            decimal angleHalf = angle / 2M;
            decimal textRadius = 15M;
            EquationCircle eqCircleText = new EquationCircle(textRadius, cicleCenterPoint);
            PointF textLocation = AlgoRadiusPoint(angleHalf + startAngle, eqCircleText);
            SizeF textSize = g.MeasureString($"{angle}", fontSmall);
            
            g.DrawString($"{angle}", fontSmall, penViolet.Brush,
                (float)((decimal)textLocation.X * scaleRate - (decimal)textSize.Width / 2M),
                (float)((decimal)-textLocation.Y * scaleRate - (decimal)textSize.Height / 2M));
        }//DrawAngleText()

        //====== Algo ======
        public PointF AlgoRadiusPoint(
            decimal angle, EquationCircle eqCircle)
        {
            decimal radius = eqCircle.Radius;
            
            if (Math.Abs(angle) > 360)
            {
                angle %= 360M;
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

        public EquationLinear AlgoRadiusLine(PointF radiusPoint, EquationCircle eqCircle)
        {
            return new EquationLinear(pt1: radiusPoint, pt2: eqCircle.CircleCenterPoint);
        }//AlgoRadiusLine()

        //====== TrySolutionCircle() ======
        public bool TrySolutionCircle(
            ICoordinateEquation eq1, ICoordinateEquation eq2, out PointF[] solutionAry)
        {
            if(eq1 is EquationCircle && eq2 is EquationCircle)
            {                
                //(Editing...)
            }
            solutionAry = new PointF[0];
            return false;
        }//TrySolutionCircle()

        private PointF[] AlgoSimultaneousCircleBoth
            (EquationCircle eqCircle1, EquationCircle eqCircle2)
        {
            decimal r1 = eqCircle1.Radius;
            decimal r2 = eqCircle2.Radius;
            PointF origin1 = eqCircle1.CircleCenterPoint;
            PointF origin2 = eqCircle2.CircleCenterPoint;

            decimal distanceSq = AlgoDistanceSq(origin1, origin2); // d ^ 2 : distance between both circle center points as squre.
            decimal radiusSumSq = (r1 + r2) * (r1 + r2);           // (r1 + r2) ^ 2 : sum of both radius as squre.

            List<PointF> pointList = new List<PointF>();
            if(distanceSq > radiusSumSq)       // solutionNum = 2;
            {
                //(Editing...)
            }
            else if(distanceSq == radiusSumSq) // solutionNum = 1;
            {
                pointList.Add(AlgoInternalPoint(r1, r2, origin1, origin2));
            }

            return pointList.ToArray();
        }//AlgoSimultaneousCircleBoth()
    }//class
}
