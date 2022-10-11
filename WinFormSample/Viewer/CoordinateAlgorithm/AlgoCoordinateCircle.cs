/*/** 
 *@title WinFormGUI / WinFormSample / 
 *@class Main.cs
 *@class   └ new Form1() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content 円と複数関数の描画
 *@subject void DrawMultiCircleFunction(
 *                ICoordinateEquation[] eqAry, params PointF[] pointAryArgs)
 *         
 *         【註】AlgoSimultaneousCircleBoth()から、Drawの分離が難しく、
 *          SetScaleRate(decimal)で適正な scaleRateを設定する必要がある
 *         〔AlgoAutoScale()が起こると、それ以前の描画が消えてしまうので注意〕
 *         
 *@subject 角度から円上の点  angle from X-Axis => PointF on Circle.
 *         PointF AlgoRadiusPoint(decimal angle, EquationCircle)
 *         
 *@NOTE【註】Math.Cos(), Math.Sin() double計算により誤差が多い
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
 *
 *@subjct ２円の交点  Two circles solution
 *        PointF[] AlgoSimultaneousCircleBoth(EquationCircle, EquationCircle)
 *        
 *        ＊２円の位置関係と解の個数:  5 cases of two circles location relationship 
 *            d:      distance between two circle center points
 *            r1, r2: each radius
 *        ・Separated 乖離:     d > r1 + r2              -> solution 0
 *        ・Circumscribed 外接: d == r1 + r2             -> solution 1
 *        ・Overlappd 重複:     |r1 - r2| < d < r1 + r2  -> solution 2
 *        ・Inscribed 内接:     d == |r1 - r2|           -> solution 1
 *        ・Included  内包:     d < |r1 - r2|            -> solution 0
 *        
 *@subject ２円の交点を通る直線  The linear equation on two circles solution points
 *         ＊【代数的解法】 Algebraic Solver
 *         ・２円の方程式を連立 -> 二乗項を消去すると、求める直線の式になっている
 *         ・複雑な代数計算をする、0除算の場合分けが必要
 *         
 *          // solutionNum = 2;  d < r1 + r2, d > r1 - r2
 *          // (x - p) ^ 2 + (y - q) ^ 2 = r ^ 2 | (x - m) ^ 2 + (y - n) ^ 2 = R ^ 2
 *          //    x ^ 2 - 2 p x + p ^ 2 + y ^ 2 - 2 q y + q ^ 2 = r ^ 2
 *          // -) x ^ 2 - 2 m x + m ^ 2 + y ^ 2 - 2 n y + n ^ 2 = R ^ 2
 *          //          - 2 p x + 2 m x         - 2 q y + 2 n y = r^2 - R^2 - p^2 + m^2 - q^2 + n^2
 *          // y = (r^2 - R^2 - p^2 - q^2 + m^2 + n^2 + 2 (p - m) x) / 2(q - n)
 *
 *          decimal p = (decimal)origin1.X;
 *          decimal q = (decimal)origin1.Y;
 *          decimal m = (decimal)origin2.X;
 *          decimal n = (decimal)origin2.Y;
 *
 *          if(q - n == 0) 
 *          {
 *              //eqLinear = new EquationLinear(
 *              //    slope: float.PositiveInfinity,
 *              //    intercept: );
 *              
 *               throw new ArgumentException("q - n == 0  in AlgoSimultaneousCircleBoth()");
 *          }
 *               
 *          var eqLinear = new EquationLinear(
 *              slope: (float)((p - m) / (q - n)),
 *               intercept: (float)((r1 * r1 - r2 * r2 - p * p - q * q + m * m + n * n) 
 *                   / (2M * (q - n)))
 *          );
 *           
 *          pointList.AddRange(AlgoSimultaneousCircleLinear(eqCircle1, eqLinear));
 *          
 *@subject ＊【幾何的解法】 Geometrical Solver
 *         ・２円の交点を通る直線は、２円の中心を結ぶ線と垂直に交わる (r による二等辺三角形の角２等分線)
 *           -> 直線の傾き slopeが求まる
 *         ・３辺の長さ d, r1, r2 が分っているので、余弦定理より cosθの値を得て、
 *           -> 上記２直線の交点座標が求まる
 *         ・複雑な代数計算をせずに済み、0除算の場合分けも不要
 *         
 *         余弦定理: c ^ 2 = a ^ 2 + b ^ 2 - 2 a b cosC
 *         cosC = (a ^ 2 + b ^ 2 - c ^ 2) / 2 a b
 *         ∠C: 中心線と eqCircle1の半径のなす角 ->  a = r1, b = d, c = r2
 *         ∠C が鈍角の場合 (90 < ∠C < 270)   cos(180 - C) = -cosC
 *         
 *                 |   <-- eqVirticalLine
 *                /|\
 *               ／| \
 *          r1 ／  |  \ r2
 *           ／    |   \
 *       O1 ∠C___┌|____\ O2   <- eqCenterLine
 *          r1*cosC 
 *          └-    d    -┘
 *          
 *@NOTE【Problem】２円が１点で接する場合
 *      判別式は小数だと ほぼ無理で２解 or 解なしになってしまう
 *      => Math.Round()でも解決しない
 *
 *@subject 円と直線の交点
 *         //---- 一般式 ----
 *         //y = d x + e | x ^ 2 + y ^ 2 + a x + b y + c = 0
 *         //x ^ 2 + (d x + e) ^ 2 + a x + b (d x + e) + c = 0
 *         //x ^ 2 + d^2x^2 + 2dex + e^2 + ax +bdx + be + c = 0
 *         //(1 + d^2) x ^ 2 + (2de + a + bd) x + e^2 + be + c = 0
 *
 *@NOTE【Problem】
 *      円と直線の連立を２次方程式の一般式で定義したら、交点が正しく表示されたが、
 *      下記のような平方式では、正しい解が求められなかった。
 *      パラメータの計算ミスや記述ミスかと思い、かなりの時間 確認したが、
 *      原因不明のままである。
 *      
 *      //----平方式----
 *      //y = a x + b | (x - p) ^ 2 + (y - q) ^ 2 = r ^ 2
 *      //x ^ 2 - 2 p x + p ^ 2 + y ^ 2 - 2 q y + q ^ 2 = r ^ 2
 *      //x ^ 2 - 2 p x + p ^ 2 + (a x + b) ^2 - 2 q(a x + b) + q ^ 2 = r ^ 2
 *      //x ^ 2 - 2px + p ^ 2 + a ^ 2 x ^ 2 + 2abx + b ^ 2 - 2aqx - 2bq + q ^ 2 = r ^ 2
 *      //(1 + a ^ 2) x ^ 2 + (2ab - 2p - 2aq) x + p ^ 2 + q ^ 2 - 2bq - r ^ 2 = 0
 *      decimal a = (decimal)eqLinear.Slope;
 *      decimal b = (decimal)eqLinear.Intercept;
 *      decimal p = (decimal)eqCircle.CircleCenterPoint.X;
 *      decimal q = (decimal)eqCircle.CircleCenterPoint.Y;
 *
 *      var eqSolutionQuad = new EquationQuadratic(
 *          a: 1 + a * a,               // 必ず 1 + a ^ 2 > 0
 *          b: 2M * (a * b - p - a * q),
 *          c: p * p + q * q - 2M * b * q - eqCircle.RadiusSq
 *      );
 *     
 *@see 
 *@author shika
 *@date 2022-10-11
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class AlgoCoordinateCircle : AlgoCoordinateDifferentiate
    {
        public AlgoCoordinateCircle(PictureBox pic) : base(pic) { }

        //====== Draw ======
        public void DrawMultiCircleFunction(ICoordinateEquation[] eqAry, params PointF[] pointAryArgs)
        {
            //---- Test Print ----
            Console.WriteLine("Equation Array:");
            foreach (var eq in eqAry) { Console.WriteLine(eq); };
            Console.WriteLine("\nArgument pointAry:");
            foreach (var pt in pointAryArgs) { Console.Write($"({pt.X},{pt.Y}), "); }
            Console.WriteLine("\n");

            //---- pointList ----
            List<PointF> pointList = new List<PointF>(pointAryArgs);
            List<EquationLinear> virticalLineList = new List<EquationLinear>();

            for (int i = 0; i < eqAry.Length; i++)
            {
                ICoordinateEquation eq = eqAry[i];
                
                for(int j = i; j < eqAry.Length; j++)
                {
                    //case same
                    if(j == i) { continue; }

                    //---- TrySolutionCircle() ----
                    bool existSolution = TrySolutionCircle(eqAry[i], eqAry[j], out PointF[] solutionAry);

                    if (existSolution)
                    {
                        pointList.AddRange(solutionAry);
                    }

                    //case virtical
                    if (eqAry[i] is EquationLinear && eqAry[j] is EquationLinear
                        && IsVirtical(eqAry[i] as EquationLinear, eqAry[j] as EquationLinear))
                    {
                        virticalLineList.Add(eqAry[i] as EquationLinear);
                        virticalLineList.Add(eqAry[j] as EquationLinear);
                        Console.WriteLine($"\nIsVirticle = True");
                    }

                    //---- Test Print TrySolution() ----
                    Console.WriteLine($"\nexistSolution = {existSolution}");
                    foreach (var solution in solutionAry) { Console.Write($"({solution.X},{solution.Y}), "); }
                }//for j

                pointList.AddRange(eq.GetEqPointAry());
            }//for i

            //---- Test Print before Distinct() ----
            Console.WriteLine("\n\npointList before Distinct():");
            pointList.ForEach(pt => { Console.Write($"({pt.X},{pt.Y}), "); });

            //---- Remove overlapped point and NaN ----
            PointF[] pointAry = pointList.Select(pt => pt)
                .Where(pt => !float.IsNaN(pt.X) || !float.IsNaN(pt.Y))
                .Distinct()
                .ToArray();

            //---- Test Print after Distinct() ----
            Console.WriteLine("\n\npointAry after Distinct():");
            foreach (PointF pt in pointAry) { Console.Write($"({pt.X},{pt.Y}), "); }
            Console.WriteLine("\n");

            //---- Draw ----
            DrawMultiPointLine(pointList.ToArray());

            foreach (ICoordinateEquation eq in eqAry)
            {
                if (eq is EquationCircle)
                {
                    Console.WriteLine($"DrawCircleFunction({eq})");
                    DrawCircleFunction(eq as EquationCircle);
                }
                else if (eq is EquationQuadratic)
                {
                    Console.WriteLine($"DrawParabolaFunction({eq})");
                    DrawParabolaFunction(eq as EquationQuadratic);
                }
                else if (eq is EquationLinear)
                {
                    Console.WriteLine($"DrawLinearFunction({eq})");
                    DrawLinearFunction(eq as EquationLinear);
                }

                if (virticalLineList.Count > 0)
                {
                    DrawVirticalMark(virticalLineList.ToArray());
                }
            }//foreach
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
        //【Deprecated】非推奨: because of including Math.Cos(double) | double計算を含み誤差の可能性
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
            
            return new PointF(
                (float)((decimal)eqCircle.CircleCenterPoint.X + radius * cos),
                (float)((decimal)eqCircle.CircleCenterPoint.Y + radius * sin));
        }//AlgoRadiusPoint()

        public EquationLinear AlgoRadiusLine(PointF radiusPoint, EquationCircle eqCircle)
        {
            return new EquationLinear(pt1: radiusPoint, pt2: eqCircle.CircleCenterPoint);
        }//AlgoRadiusLine()

        public EquationLinear AlgoTangentLineCircle(PointF pt, EquationCircle eqCircle)
        {
            if (!eqCircle.CheckOnLine(pt))
            {
                throw new ArgumentException("PointF pt is not on the EquationCircle.");
            }

            EquationLinear radiusLine = AlgoRadiusLine(pt, eqCircle);

            return AlgoVirticalLine(pt, radiusLine);
        }//AlgoTangentLineCircle()

        //====== TrySolutionCircle() ======
        public bool TrySolutionCircle(
            ICoordinateEquation eq1, ICoordinateEquation eq2, out PointF[] solutionAry)
        {
            List<PointF> solutionList = new List<PointF>();
            if(eq1 is EquationCircle && eq2 is EquationCircle)
            {
                solutionList.AddRange(AlgoSimultaneousCircleBoth(
                    eq1 as EquationCircle, eq2 as EquationCircle));
            }
             
            if (eq1 is EquationCircle && eq2 is EquationQuadratic)
            {
                // (Editing...)
            }

            if (eq2 is EquationCircle && eq1 is EquationQuadratic)
            {
                // (Editing...)
            }

            if (eq1 is EquationCircle && eq2 is EquationLinear)
            {
                solutionList.AddRange(AlgoSimultaneousCircleLinear(
                    eq1 as EquationCircle, eq2 as EquationLinear));
            }

            if (eq2 is EquationCircle && eq1 is EquationLinear)
            {
                solutionList.AddRange(AlgoSimultaneousCircleLinear(
                    eq2 as EquationCircle, eq1 as EquationLinear));
            }

            if (!(eq1 is EquationCircle) && !(eq2 is EquationCircle))
            {
                TrySolutionQuad(eq1, eq2, out PointF[] solutionAryQuad);
                solutionList.AddRange(solutionAryQuad);
            }

            solutionAry = solutionList.ToArray();
            return solutionAry.Length > 0;
        }//TrySolutionCircle()

        private PointF[] AlgoSimultaneousCircleBoth
            (EquationCircle eqCircle1, EquationCircle eqCircle2)
        {
            decimal r1 = eqCircle1.Radius;  //【Deprecated】非推奨: including Math.Sqrt(double)
            decimal r2 = eqCircle2.Radius;  //【Deprecated】非推奨: including Math.Sqrt(double)
            PointF origin1 = eqCircle1.CircleCenterPoint;
            PointF origin2 = eqCircle2.CircleCenterPoint;

            decimal distanceSq = AlgoDistanceSq(origin1, origin2);     // d ^ 2 : distance between both circle center points as squre.
            decimal distance = (decimal)Math.Sqrt((double)distanceSq); //【Deprecated】非推奨: including Math.Sqrt(double)
            decimal radiusSumSq = (r1 + r2) * (r1 + r2);               // (r1 + r2) ^ 2 : sum of both radius as squre.
            decimal radiusSubtractSq = (r1 - r2) * (r1 - r2);          // (r1 - r2) ^ 2 : subtract of both radius as squre.

            SetScaleRate(1.5M);
            penViolet.Width = 1f;
            penViolet.DashStyle = DashStyle.Dash;

            //---- center point line 中心線 ----
            var eqCenterLine = new EquationLinear(origin1, origin2);
            DrawLinearSegment(penViolet, origin1, origin2);  // d: eqCenterLine

            List<PointF> pointList = new List<PointF>();
            if (distanceSq < radiusSumSq && distanceSq > radiusSubtractSq)
            {
                // 余弦定理: c ^ 2 = a ^ 2 + b ^ 2 - 2 a b cosC
                // cosC = (a ^ 2 + b ^ 2 - c ^ 2) / 2 a b
                // ∠C: 中心線と eqCircle1の半径のなす角 ->  a = r1, b = d, c = r2
                // ∠C が鈍角の場合 (90 < ∠C < 270)  cos(180 - C) = -cosC

                decimal cos = (r1 * r1 + distanceSq - r2 * r2) / (2M * r1 * distance);

                if (origin2.X < origin1.X)  //∠C が鈍角の場合  -cosC
                {
                    cos *= -1;
                }

                PointF centerVirticalPoint = AlgoDistanceOnLinePoint(
                    r1 * cos, origin1, eqCenterLine);

                EquationLinear eqVirticalLine =
                    AlgoVirticalLine(centerVirticalPoint, eqCenterLine);

                PointF[] solutionPointAry = AlgoSimultaneousCircleLinear(eqCircle1, eqVirticalLine);
                pointList.AddRange(solutionPointAry);

                //---- Draw ----
                if (solutionPointAry.Length == 2)
                {
                    DrawPointLine(centerVirticalPoint);

                    foreach (PointF solutionPoint in solutionPointAry)
                    {
                        DrawLinearSegment(penViolet, origin1, solutionPoint);  // r1
                        DrawLinearSegment(penViolet, origin2, solutionPoint);  // r2
                    }//foreach
                    DrawLinearSegment(penViolet, solutionPointAry[0], solutionPointAry[1]);    //eqVirticalLine
                    DrawVirticalMark(eqCenterLine, eqVirticalLine, plusX: false, plusY: true);
                }
            }
            else if (Math.Round(distanceSq, 1) == Math.Round(radiusSumSq, 1))
            {   // solutionNum = 1;  d == r1 + r2  ２円外接、内分点
                PointF internalPoint = AlgoInternalPoint(r1, r2, origin1, origin2);
                pointList.Add(internalPoint);

                EquationLinear eqTangentLine = AlgoTangentLineCircle(internalPoint, eqCircle1);
                DrawLinearFunction(eqTangentLine);
            }
            else if (Math.Round(distanceSq, 1) == Math.Round(radiusSubtractSq, 1) && r1 - r2 != 0)
            {   // solutionNum = 1;  d == r1 - r2  ２円内接、外分点
                PointF externalPoint = AlgoExternalPoint(r1, r2, origin1, origin2);
                pointList.Add(externalPoint);

                EquationLinear eqTangentLine = AlgoTangentLineCircle(externalPoint, eqCircle1);
                DrawLinearFunction(eqTangentLine);
            }

            DrawLinearSegment(penViolet, origin1, origin2);
            return pointList.ToArray();
        }//AlgoSimultaneousCircleBoth()

        private PointF[] AlgoSimultaneousCircleLinear(EquationCircle eqCircle, EquationLinear eqLinear)
        {
            List<PointF> pointList = new List<PointF>();
            if (float.IsInfinity(eqLinear.Slope))  // x = c  (virtical)
            {
                float[] yAry = eqCircle.AlgoFunctionXtoY(x: eqLinear.Intercept);
                foreach (float solutionY in yAry)
                {
                    pointList.Add(new PointF(eqLinear.Intercept, solutionY));
                }//foreach

                return pointList.ToArray();
            }

            //---- 一般式 ----
            //y = d x + e | x ^ 2 + y ^ 2 + a x + b y + c = 0
            //x ^ 2 + (d x + e) ^ 2 + a x + b (d x + e) + c = 0
            //x ^ 2 + d^2x^2 + 2dex + e^2 + ax +bdx + be + c = 0
            //(1 + d^2) x ^ 2 + (2de + a + bd) x + e^2 + be + c = 0
            decimal d = (decimal)eqLinear.Slope;
            decimal e = (decimal)eqLinear.Intercept;
            decimal a = eqCircle.A;
            decimal b = eqCircle.B;
            decimal c = eqCircle.C;

            var eqSolutionQuad = new EquationQuadratic(
                a: 1 + d * d,              // 必ず 1 + d ^ 2 > 0
                b: 2M * d * e + a + b * d,
                c: e * e + b * e + c
            );

            float[] xAry = eqSolutionQuad.AlgoQuadSolutionFormula();

            foreach (float solutionX in xAry)
            {
                float solutionY = eqLinear.AlgoFunctionXtoY(solutionX)[0];
                pointList.Add(new PointF(solutionX, solutionY));
            }//foreach

            return pointList.ToArray();
        }//AlgoSimultaneousCircleLinear()
    }//class
}
