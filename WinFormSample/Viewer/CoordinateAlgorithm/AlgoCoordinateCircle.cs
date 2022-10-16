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
 *@content Algorithm and Draw about Circle
 *         円に関するアルゴリズムと描画
 */
#region Method Algorithm
/*
 *@subject 円と複数関数の描画
 *         void DrawMultiCircleFunction(
 *                ICoordinateEquation[] eqAry, params PointF[] pointAryArgs)
 *         
 *         【註】AlgoSimultaneousCircleBoth()から、Draw, Algo の分離が難しく、
 *          SetScaleRate(decimal)で適正な scaleRateを設定する必要がある。
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
 *       => copyTo〔~\WinFormSample\TriangularRatioReference.txt〕
 *          
 *@NOTE【Problem】２円が１点で接する場合
 *      判別式は小数だと ほぼ無理で２解 or 解なしになってしまう
 *      => Math.Round()でも解決しない
 *      
 *      整数交点(72,96) なら接点１つで描画可能 
 *      (整数比[3 : 4 : 5]の三角形になるよう設定)
 *      new EquationCircle(radius: 120M, new PointF(0, 0));
 *      new EquationCircle(radius: 80M, new PointF(120f, 160f)
 *      
 *      外接円の整数交点(72,96) は
 *      AlgoCoordinateCiecle.AlgoSimultaneousCircleBoth()から
 *      AlgoCoordinateLinear.AlgoInternalPoint()において
 *      (0,0)-(120, 160) r1 : r2 の内分点で導出  (y = 1.33333333 x を利用していない)
 *      
 *      y = 1.33333333 x        <- 原点(0, 0)から接点までの直線
 *      y = 0.75000002 x + 150  <- 接点における接線
 *      
 *      existSolution = True
 *      (72,96),                <- AlgoInternalPoint()内で
 *                                 (0,0)-(120, 160) r1 : r2 の内分点
 *      existSolution = True
 *      (72.00002,96), (-72.00002,-96),  <- 円１と y = 1.33333333 x の連立解
 *      existSolution = True  
 *      (168,223.9999), (72.00002,96),   <- 円２と y = 1.33333333 x の連立解
 *      
 *      やはり 1.33333333のような 循環小数の計算で誤差が出る様子
 *
 *      内接円は 
 *      new EquationCircle(radius: 120M, new PointF(0, 0));
 *      new EquationCircle(radius: 80M, new PointF(-24, -32));
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
 *@subject 円上の点における接線
 *         EquationLinear  AlgoTangentLineOnCircle(PointF, EquationCircle)
 *         
 *         ・中心点と円上の点を結ぶ半径線の方程式を求め、
 *           接線は半径線と垂直に交わるので、接線の傾きを求める
 *         ・傾きと一点から接線の方程式を導く
 *           EquationLinear  AlgoRadiusLine(PointF, EquationCircle)
 *           EquationLinear  AlgoVirticalLine(PointF, EquationLinear)
 *           
 *@subject 円外の点からの接線
 *         EquationLinear[]  AlgoTangentLineOutCircle(PointF, EquationCircle)
 *         
 *         ・円外の点の位置関係により接線の個数が異なる
 *         ・接点と 接線の方程式が不明
 *         ＊【代数的解法】は 接線の公式, 距離の公式を利用し、円の方程式との連立
 *            接線の公式: 円の中心 O (x0, y0) , 極 P (p, q), 接点 C (s, t)
 *                       (s - x0)(x - x0) + (t - y0)(y - y0) = r^2   
 *            距離の公式  | (s - x0)(x - x0) + (t - y0)(y - y0) - r^2 | / √ [(s - x0)^2 +(t - y0)^2] 
 *            
 *            円の方程式  (x - x0)^2 + (y - y0)^2 = r^2
 *            
 *            => 代数計算がとても複雑で計算ミス, 記述ミスが起こりやすい
 *            
 *         ＊【幾何的解法】極線を求めて、円の方程式と連立
 *         ・極 P: 円外の点, 極線 CC' = CQ: 極から引いた接線の接点を結ぶ線 (1)
 *         ・円の中心点から極点までの線 OP (2)
 *         ・極線の傾き: (2)と垂直 OP ⊥ CQ (二等辺三角形の角二等分線)から求まる
 *         
 *         ・相似から、(1),(2)交点の内分点 Q の内分比が分かる
 *              △OCP ∽ △ OCQ (相似: 三角相等) d : r = r : OQ 
 *              -> OQ = r^2 / d
 *              △OCP ∽ △ CQP (相似: 三角相等) d : √(d^2 - r^2) = √(d^2 - r^2) : PQ  
 *              -> PQ = (d^2 - r^2) / d
 *           OQ : PQ = r^2 : (d^2 - r^2)
 *           
 *         ・【別解】円の中心点 O, 円外の点 P, 接点 C による三角形 △OCP -> ３辺の長さがわかる
 *           余弦定理で cosθ の値が求まる。 OQ : PQ = (r * cosθ) : (d - r * cosθ)
 *           (この cosθ は X軸と平行ではないので直接に座標は求められないことに注意)
 *           
 *         ・極線: 傾きと一点 Q から決定。
 *         ・接点: 円と極線の交点 C, C' を求め、接線: 極点 P と接点 C, C' を結ぶ直線を導出
 *         
 *                       \ Ｃ   <- Contact Point  OC ⊥ CP (接線と半径線)
 *                      └/|\
 *                     ／ | \  
 *                 r ／   |  \ √ (d^2 - r^2)
 *                 ／     |   \
 *    (x0, y0) Ｏ ∠_θ____┌|Q __\ Ｐ  <- polar: point out of Circle
 *                 r*cosθ            <- OP ⊥ CQ (二等辺三角形の角二等分線)
 *               └----   d   ---┘
 *               
 *@subject ２円の共通接線
 *         ・２円の位置関係は ２円の交点〔上記〕
 *         ・乖離 d > r1 + r2                 共通接線 4
 *           外接 d == r1 + r2                共通接線 3
 *           交円 | r1 - r2 | < d < r1 + r2   共通接線 2
 *           内接 d == | r1 - r2 |            共通接線 1
 *           内包 d <  | r1 - r2 |            共通接線 0
 *           
 *           EquationLinear[]  AlgoCotangentTwoCircle(
 *               EquationCircle, EquationCircle, out contactPointAry)
 *               
 *@subject 乖離 d > r1 + r2  共通接線 4
 *        【幾何的解法】共通外接線(両側) 2, 共通内接線(交差) 2で場合分けが必要
 *         ＊共通外接線(両側) 中心線の延長上の r1 : r2 の外分点で 両接線は交わるので、極の接線を引く。
 *           平行の場合分けが必要。平行時は直径と垂直な接線となる。
 *           
 *         ＊共通内接線(交差) 中心線を r1 : r2 の内分点で交差するので、極の接線
 *           AlgoTangentLineOutCircle()
 */
#endregion
/*
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
        public void DrawMultiCircleFunction(
            ICoordinateEquation[] eqAry,
            EquationLinear[] virticalLineAryArgs,
            SegmentPair[] segmentPairAryArgs,
            PointF[] pointAryArgs,
            bool isAutoScale = true)
        {
            //---- Test Print ----
            Console.WriteLine("Equation Array:");
            foreach (var eq in eqAry) { Console.WriteLine(eq); };
            Console.WriteLine("\nArgument pointAry:");
            foreach (var pt in pointAryArgs) { Console.Write($"({pt.X},{pt.Y}), "); }
            Console.WriteLine("\n");

            //---- pointList ----
            List<PointF> pointList = new List<PointF>(pointAryArgs);
            List<EquationLinear> virticalLineList = new List<EquationLinear>(virticalLineAryArgs);
            List<SegmentPair> segmentPairList = new List<SegmentPair>(segmentPairAryArgs);

            for (int i = 0; i < eqAry.Length; i++)
            {
                ICoordinateEquation eq = eqAry[i];
                
                for(int j = i; j < eqAry.Length; j++)
                {
                    //case same
                    if(j == i) { continue; }

                    //---- TrySolutionCircle() ----
                    bool existSolution = TrySolutionCircle(eqAry[i], eqAry[j],
                        out PointF[] pointAryTry,
                        out EquationLinear[] virticalLineAry,
                        out SegmentPair[] segmentPairAryTry);
                    pointList.AddRange(pointAryTry);
                    virticalLineList.AddRange(virticalLineAry);
                    segmentPairList.AddRange(segmentPairAryTry);

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
                    foreach (var pt in pointAryTry) { Console.Write($"({pt.X},{pt.Y}), "); }
                }//for j

                pointList.AddRange(eq.GetEqPointAry());
            }//for i

            //---- Test Print before Distinct() ----
            Console.WriteLine("\n\npointList before Distinct():");
            pointList.ForEach(pt => { Console.Write($"({pt.X},{pt.Y}), "); });

            //---- Remove overlapped point and NaN ----
            PointF[] pointAry = pointList
                .Where(pt => !float.IsNaN(pt.X) || !float.IsNaN(pt.Y))
                .Select<PointF,PointF>(pt =>
                {
                    pt.X = (float)Math.Round((double)pt.X, 2);
                    pt.Y = (float)Math.Round((double)pt.Y, 2);
                    return pt;
                })
                .Distinct()
                .ToArray();

            //---- Test Print after Distinct() ----
            Console.WriteLine("\n\npointAry after Distinct():");
            foreach (PointF pt in pointAry) { Console.Write($"({pt.X},{pt.Y}), "); }
            Console.WriteLine("\n");

            //---- Draw ----
            DrawMultiPointLine(pointList.ToArray(), autoScale: isAutoScale);
            //autoScale false: need SetScaleRate(decimal) in AlgoSimultaneousCircleBoth 
            
            Console.WriteLine($"scaleRate = {scaleRate}");

            foreach (ICoordinateEquation eq in eqAry)
            {
                if (eq is EquationCircle)
                {
                    //Console.WriteLine($"DrawCircleFunction({eq})");
                    DrawCircleFunction(eq as EquationCircle);
                }
                else if (eq is EquationQuadratic)
                {
                    //Console.WriteLine($"DrawParabolaFunction({eq})");
                    DrawParabolaFunction(eq as EquationQuadratic);
                }
                else if (eq is EquationLinear)
                {
                    //Console.WriteLine($"DrawLinearFunction({eq})");
                    DrawLinearFunction(eq as EquationLinear);
                }

                if (virticalLineList.Count > 0)
                {
                    DrawVirticalMark(virticalLineList.ToArray());
                }
            }//foreach

            penViolet.Width = 1.0f;
            penViolet.DashStyle = DashStyle.Dash;
            foreach (SegmentPair pair in segmentPairList)
            {
                DrawLinearSegment(penViolet, pair.startPt, pair.endPt);
            }

            if (virticalLineList.Count > 0)
            {
                DrawVirticalMark(virticalLineList.ToArray());
            }
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

            // PointF(p + r * cosθ, q + r * sinθ)
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

        public EquationLinear AlgoTangentLineOnCircle(PointF ptOnCircle, EquationCircle eqCircle)
        {
            if (!eqCircle.CheckOnLine(ptOnCircle))
            {
                Console.WriteLine("PointF pt is not on the EquationCircle.");
                return AlgoTangentLineOutCircle(
                    ptOnCircle, eqCircle, 
                    out PointF[] pointAry, 
                    out SegmentPair[] segmentPairAry,
                    out EquationLinear[] virticalLineAry)[0];
            }

            EquationLinear radiusLine = AlgoRadiusLine(ptOnCircle, eqCircle);

            return AlgoVirticalLine(ptOnCircle, radiusLine);
        }//AlgoTangentLineOnCircle()

        public EquationLinear[] AlgoTangentLineOutCircle(PointF polar, EquationCircle eqCircle, 
            out PointF[] pointAryOut,
            out SegmentPair[] segmentPairAryOut,
            out EquationLinear[] virticalLineAryOut)
        {
            if (eqCircle.CheckOnLine(polar))
            {
                Console.WriteLine("PointF pt is on the EquationCircle.");
                pointAryOut = new PointF[] { polar };
                segmentPairAryOut = new SegmentPair[0];
                virticalLineAryOut = new EquationLinear[0];
                return new EquationLinear[] { AlgoTangentLineOnCircle(polar, eqCircle) };
            }

            PointF origin = eqCircle.CircleCenterPoint;
            decimal distanceSq = AlgoDistanceSq(origin, polar);// d^2
            decimal radiusSq = eqCircle.RadiusSq;              // r^2
            decimal polarToContactSq = distanceSq - radiusSq;  // CP^2

            List<PointF> pointList = new List<PointF>();
            List<SegmentPair> segmentPairList = new List<SegmentPair>();
            List<EquationLinear> virticalLineList = new List<EquationLinear>();
            List<EquationLinear> tangentLineList = new List<EquationLinear>();

            if(polarToContactSq > 0)  // d > r
            {
                EquationLinear centerPolarLine = new EquationLinear(origin, polar);
                segmentPairList.Add(new SegmentPair(origin, polar));
                virticalLineList.Add(centerPolarLine);

                // △OCP ∽ △ OCQ (相似: 三角相等) d : r = r : OQ  -> OQ = r^2 / d
                // △OCP ∽ △ CQP (相似: 三角相等) d : √(d^2 - r^2) = √(d^2 - r^2) : PQ  -> PQ = (d^2 - r^2) / d
                // OQ : PQ = r^2 : (d^2 - r^2)
                PointF internalPoint = AlgoInternalPoint(
                    radiusSq, polarToContactSq, origin, polar);

                // OP ⊥ CQ  (二等辺三角形の角二等分線)
                EquationLinear polarVirticalLine = AlgoVirticalLine(internalPoint, centerPolarLine);
                virticalLineList.Add(polarVirticalLine);   // OP ⊥ CQ

                // 円と極線の連立
                pointList.AddRange(AlgoSimultaneousCircleLinear(eqCircle, polarVirticalLine));
                
                //---- 接線 ----
                foreach(PointF contactPoint in pointList)
                {
                    var tangentLine = new EquationLinear(polar, contactPoint);
                    tangentLineList.Add(tangentLine);
                    virticalLineList.Add(tangentLine);  // OC ⊥ OP
                    segmentPairList.Add(new SegmentPair(origin, contactPoint));   // OC, OC'
                }//foreach

                if (pointList.Count == 2)
                {
                    segmentPairList.Add(
                        new SegmentPair(pointList[0], pointList[1]));  // CC'
                }

                pointList.Add(internalPoint); //pointListは接点の判定に利用しているので、この位置で Add()
            }// if d > r
            //else if (pointToContactSq == 0)  // d == r  -> AlgoTangentOnCircle()
            //else if (pointToContactSq < 0)   // d < r   -> (No solution)

            pointAryOut = pointList.ToArray();
            segmentPairAryOut = segmentPairList.ToArray();
            virticalLineAryOut = virticalLineList.ToArray();
            return tangentLineList.ToArray();
        }//AlgoTangentLineOutCircle()

        public EquationLinear[] AlgoCotangentLineTwoCircle(
            EquationCircle eqCircle1, EquationCircle eqCircle2,
            out PointF[] pointAry, 
            out SegmentPair[] segmentPairAry,
            out EquationLinear[] virticalLineAry)
        {      
            List<PointF> pointList = new List<PointF>();
            List<SegmentPair> segmentPairList = new List<SegmentPair>();
            List<EquationLinear> virticalLineList = new List<EquationLinear>();
            List<EquationLinear> cotangentLineList = new List<EquationLinear>();

            cotangentLineList.AddRange(
                AlgoInternalCotangentTwoCircle(eqCircle1, eqCircle2,
                    out PointF[] pointAryInternal,
                    out SegmentPair[] segmentPairAryInternal,
                    out EquationLinear[] virticalLineAryInternal));
            pointList.AddRange(pointAryInternal);
            segmentPairList.AddRange(segmentPairAryInternal);
            virticalLineList.AddRange(virticalLineAryInternal);

            cotangentLineList.AddRange(
                AlgoExternalCotangentTwoCircle(eqCircle1, eqCircle2,
                    out PointF[] pointAryExternal,
                    out SegmentPair[] segmentPairAryExternal,
                    out EquationLinear[] virticalLineAryExternal));
            pointList.AddRange(pointAryExternal);
            segmentPairList.AddRange(segmentPairAryExternal);
            virticalLineList.AddRange(virticalLineAryExternal);

            pointAry = pointList.ToArray();
            segmentPairAry = segmentPairList.ToArray();
            virticalLineAry = virticalLineList.ToArray();
            return cotangentLineList.ToArray();
        }//AlgoCotangentLineTwoCircle()

        public EquationLinear[] AlgoInternalCotangentTwoCircle(
            EquationCircle eqCircle1, EquationCircle eqCircle2,
            out PointF[] pointAryInternal,
            out SegmentPair[] segmentPairAryInternal,
            out EquationLinear[] virticalLineAryOut)
        {
            decimal r1 = eqCircle1.Radius;
            decimal r2 = eqCircle2.Radius;
            PointF origin1 = eqCircle1.CircleCenterPoint;
            PointF origin2 = eqCircle2.CircleCenterPoint;

            List<PointF> pointList = new List<PointF>();
            List<SegmentPair> segmentPairList = new List<SegmentPair>();
            List<EquationLinear> virticalLineList = new List<EquationLinear>();
            List<EquationLinear> cotangentLineList = new List<EquationLinear>();
            
            EquationLinear centerLine = new EquationLinear(origin1, origin2);
            segmentPairList.Add(new SegmentPair(origin1, origin2));

            //======= Internal co-tangent line ======
            PointF internalPoint = AlgoInternalPoint(r1, r2, origin1, origin2);
            pointList.Add(internalPoint);
            segmentPairList.Add(new SegmentPair(internalPoint, origin1));
            segmentPairList.Add(new SegmentPair(internalPoint, origin2));

            EquationLinear[] internalCotangentLineAry1 = 
                AlgoTangentLineOutCircle(internalPoint, eqCircle1, 
                out PointF[] internalContactPointAry1,
                out SegmentPair[] internalSegmentPairAry1,
                out EquationLinear[] virticalLineAry1);
            pointList.AddRange(internalContactPointAry1);
            virticalLineList.AddRange(virticalLineAry1);
            segmentPairList.AddRange(internalSegmentPairAry1);

            EquationLinear[] internalCotangentLineAry2 = 
                AlgoTangentLineOutCircle(internalPoint, eqCircle2, 
                out PointF[] internalContactPointAry2,
                out SegmentPair[] internalSegmentPairAry2,
                out EquationLinear[] virticalLineAry2);
            pointList.AddRange(internalContactPointAry2);
            virticalLineList.AddRange(virticalLineAry2);
            segmentPairList.AddRange(internalSegmentPairAry2);

            cotangentLineList.AddRange(internalCotangentLineAry1);
            virticalLineList.AddRange(internalCotangentLineAry1);
            virticalLineList.AddRange(internalCotangentLineAry2);

            //for (int i = 0; i < internalContactPointAry1.Length; i++)
            //{
            //    segmentPairList.Add(new SegmentPair(
            //        internalContactPointAry1[i], origin1));
            //    virticalLineList.Add(
            //        new EquationLinear(internalContactPointAry1[i], origin1));

            //    if (internalContactPointAry2.Length < internalContactPointAry1.Length)
            //    {
            //        Console.WriteLine("internalContactAry2 is lacked value.");
            //    }
            //    else
            //    {
            //        segmentPairList.Add(new SegmentPair(
            //            internalContactPointAry2[i], origin2));
            //        virticalLineList.Add(
            //            new EquationLinear(internalContactPointAry2[i], origin2));
            //    }
            //}//for

            pointAryInternal = pointList.ToArray();
            segmentPairAryInternal = segmentPairList.ToArray();
            virticalLineAryOut = virticalLineList.ToArray();
            return cotangentLineList.ToArray();
        }//AlgoInternalCotangentTwoCircle()

        public EquationLinear[] AlgoExternalCotangentTwoCircle(
            EquationCircle eqCircle1, EquationCircle eqCircle2,
            out PointF[] pointAryExternal, 
            out SegmentPair[] segmentPairAryExternal,
            out EquationLinear[] virticalLineAryExternal)
        {
            decimal r1 = eqCircle1.Radius;
            decimal r2 = eqCircle2.Radius;
            PointF origin1 = eqCircle1.CircleCenterPoint;
            PointF origin2 = eqCircle2.CircleCenterPoint;

            List<PointF> pointList = new List<PointF>();
            List<SegmentPair> segmentPairList = new List<SegmentPair>();
            List<EquationLinear> virticalLineList = new List<EquationLinear>();
            List<EquationLinear> cotangentLineList = new List<EquationLinear>();

            EquationLinear centerLine = new EquationLinear(origin1, origin2);
            segmentPairList.Add(new SegmentPair(origin1, origin2));
            virticalLineList.Add(centerLine);

            //======= External co-tangent line ======
            if (r1 == r2) //接線は平行線
            {   //直径線と円の交点
                EquationLinear diameterLine = AlgoVirticalLine(origin1, centerLine);
                PointF[] externalContactPointAry1 = AlgoSimultaneousCircleLinear(eqCircle1, diameterLine);
                PointF[] externalContactPointAry2 = AlgoSimultaneousCircleLinear(eqCircle2, diameterLine);
                
                virticalLineList.Add(diameterLine);
                pointList.AddRange(externalContactPointAry1);
                pointList.AddRange(externalContactPointAry2);

                for (int i = 0; i < externalContactPointAry1.Length; i++)
                {
                    EquationLinear externalCotangentLine = 
                        AlgoVirticalLine(externalContactPointAry1[i], diameterLine);
                    cotangentLineList.Add(externalCotangentLine);
                    virticalLineList.Add(externalCotangentLine);

                    //if (externalContactPointAry2.Length < externalContactPointAry1.Length)
                    //{
                    //    Console.WriteLine("externalContactAry2 is lacked value.");
                    //}
                    //else
                    //{
                    //    segmentPairList.Add(new SegmentPair(
                    //        externalContactPointAry1[i], externalContactPointAry2[i]));
                    //    virticalLineList.Add(new EquationLinear(
                    //        externalContactPointAry1[i], externalContactPointAry2[i]));
                    //}
                }//for
            }//if r1 == r2
            else if (Math.Abs(r1 - r2) > 0)
            {
                //外分点 T
                PointF externalPoint = AlgoExternalPoint(r1, r2, origin1, origin2);
                pointList.Add(externalPoint);
                segmentPairList.Add(new SegmentPair(externalPoint, origin1));
                segmentPairList.Add(new SegmentPair(externalPoint, origin2));
                
                //外分点を極とする接線
                EquationLinear[] externalCotangentLineAry1 = 
                    AlgoTangentLineOutCircle(externalPoint, eqCircle1,
                    out PointF[] externalContactPointAry1,
                    out SegmentPair[] externalSegmentPairAry1,
                    out EquationLinear[] externalVirticalLineAry1);
                pointList.AddRange(externalContactPointAry1);
                segmentPairList.AddRange(externalSegmentPairAry1);
                virticalLineList.AddRange(externalVirticalLineAry1);

                EquationLinear[] externalCotangentLineAry2 = 
                    AlgoTangentLineOutCircle(externalPoint, eqCircle2,
                    out PointF[] externalContactPointAry2,
                    out SegmentPair[] externalSegmentPairAry2,
                    out EquationLinear[] externalVirticalLineAry2);
                pointList.AddRange(externalContactPointAry2);
                segmentPairList.AddRange(externalSegmentPairAry2);
                virticalLineList.AddRange(externalVirticalLineAry2);

                cotangentLineList.AddRange(externalCotangentLineAry1);
                virticalLineList.AddRange(externalCotangentLineAry1);
                virticalLineList.AddRange(externalCotangentLineAry2);

                //for (int i = 0; i < externalContactPointAry1.Length; i++)
                //{
                //    for (int j = i; j < externalContactPointAry1.Length; j++)
                //    {
                //        if (j == i) { continue; }

                //        segmentPairList.Add(new SegmentPair(
                //            externalContactPointAry1[i],
                //            externalContactPointAry1[j]));
                //        virticalLineList.Add(new EquationLinear(
                //            externalContactPointAry1[i],
                //            externalContactPointAry1[j]));

                //        if (externalContactPointAry2.Length < externalContactPointAry1.Length)
                //        {
                //            Console.WriteLine("externalContactPointAry2 is lacked value.");
                //        }
                //        else
                //        {
                //            segmentPairList.Add(new SegmentPair(
                //                externalContactPointAry2[i],
                //                externalContactPointAry2[j]));
                //            virticalLineList.Add(new EquationLinear(
                //                externalContactPointAry2[i],
                //                externalContactPointAry2[j]));
                //        }
                //    }//for j
                //}//for i
            }//if (r1 - r2) > 0

            pointAryExternal = pointList.ToArray();
            segmentPairAryExternal = segmentPairList.ToArray();
            virticalLineAryExternal = virticalLineList.ToArray();
            return cotangentLineList.ToArray();
        }//AlgoExternalCotangentTwoCircle()

        //====== TrySolutionCircle() ======
        public bool TrySolutionCircle(
            ICoordinateEquation eq1, ICoordinateEquation eq2, 
            out PointF[] pointAryOut,
            out EquationLinear[] virticalLineAryOut,
            out SegmentPair[] segmentPairAryOut)
        {
            List<PointF> pointList = new List<PointF>();
            List<SegmentPair> segmentPairList = new List<SegmentPair>();
            List<EquationLinear> virticalLineList = new List<EquationLinear>();

            if(eq1 is EquationCircle && eq2 is EquationCircle)
            {
                pointList.AddRange(AlgoSimultaneousCircleBoth(
                    eq1 as EquationCircle, eq2 as EquationCircle,
                    out SegmentPair[] segmentPairAry1,
                    out EquationLinear[] virticalLineAry1));
                segmentPairList.AddRange(segmentPairAry1);
                virticalLineList.AddRange(virticalLineAry1);
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
                pointList.AddRange(AlgoSimultaneousCircleLinear(
                    eq1 as EquationCircle, eq2 as EquationLinear));
            }

            if (eq2 is EquationCircle && eq1 is EquationLinear)
            {
                pointList.AddRange(AlgoSimultaneousCircleLinear(
                    eq2 as EquationCircle, eq1 as EquationLinear));
            }

            if (!(eq1 is EquationCircle) && !(eq2 is EquationCircle))
            {
                TrySolutionQuad(eq1, eq2, out PointF[] solutionAryQuad);
                pointList.AddRange(solutionAryQuad);
            }

            pointAryOut = pointList.ToArray();
            virticalLineAryOut = virticalLineList.ToArray();
            segmentPairAryOut = segmentPairList.ToArray();
            return pointAryOut.Length > 0;
        }//TrySolutionCircle()

        private PointF[] AlgoSimultaneousCircleBoth
            (EquationCircle eqCircle1, EquationCircle eqCircle2,
            out SegmentPair[] segmentPairAryOut,
            out EquationLinear[] virticalLineAryOut)
        {
            decimal r1 = eqCircle1.Radius;
            decimal r2 = eqCircle2.Radius;
            PointF origin1 = eqCircle1.CircleCenterPoint;
            PointF origin2 = eqCircle2.CircleCenterPoint;

            decimal distanceSq = AlgoDistanceSq(origin1, origin2);     // d ^ 2 : distance between both circle center points as squre.
            decimal distance = (decimal)Math.Sqrt((double)distanceSq); //【Deprecated】非推奨: including Math.Sqrt(double)
            decimal radiusSumSq = (r1 + r2) * (r1 + r2);               // (r1 + r2) ^ 2 : sum of both radius as squre.
            decimal radiusSubtractSq = (r1 - r2) * (r1 - r2);          // (r1 - r2) ^ 2 : subtract of both radius as squre.

            List<PointF> pointList = new List<PointF>();
            List<SegmentPair> segmentPairList = new List<SegmentPair>();
            List<EquationLinear> virticalLineList = new List<EquationLinear>();

            //---- center point line 中心線 ----
            var eqCenterLine = new EquationLinear(origin1, origin2);
            segmentPairList.Add(new SegmentPair(origin1, origin2));  // d: eqCenterLine
            virticalLineList.Add(eqCenterLine);

            if (distanceSq < radiusSumSq && distanceSq > radiusSubtractSq)
            {
                // 余弦定理: c ^ 2 = a ^ 2 + b ^ 2 - 2 a b cosC
                // cosC = (a ^ 2 + b ^ 2 - c ^ 2) / 2 a b
                // ∠C: 中心線と eqCircle1の半径のなす角 ->  a = r1, b = d, c = r2
                // ∠C が鈍角の場合 (90 < ∠C < 270)  cos(180 - C) = -cosC

                decimal cos = (r1 * r1 + distanceSq - r2 * r2) / (2M * r1 * distance);
                cos = (origin1.X < origin2.X) ? cos : -cos;  // ∠C が鈍角の場合  -cosC

                PointF centerVirticalPoint = AlgoDistanceOnLinePoint(
                    r1 * cos, origin1, eqCenterLine);
                pointList.Add(centerVirticalPoint);

                EquationLinear eqVirticalLine =
                    AlgoVirticalLine(centerVirticalPoint, eqCenterLine);
                virticalLineList.Add(eqVirticalLine);

                PointF[] solutionPointAry = AlgoSimultaneousCircleLinear(
                    eqCircle1, eqVirticalLine);
                pointList.AddRange(solutionPointAry);
             
                //---- Segment ----
                if (solutionPointAry.Length == 2)
                {
                    foreach (PointF solutionPoint in solutionPointAry)
                    {
                        segmentPairList.Add(new SegmentPair(origin1, solutionPoint));  // r1
                        segmentPairList.Add(new SegmentPair(origin2, solutionPoint));  // r2
                    }//foreach

                    segmentPairList.Add(new SegmentPair(solutionPointAry[0], solutionPointAry[1]));    //eqVirticalLine
                }
            }
            else if (Math.Round(distanceSq, 4) == Math.Round(radiusSumSq, 1))
            {   // solutionNum = 1;  d == r1 + r2  ２円外接、内分点
                PointF internalPoint = AlgoInternalPoint(r1, r2, origin1, origin2);
                pointList.Add(internalPoint);

                EquationLinear eqTangentLine = AlgoTangentLineOnCircle(internalPoint, eqCircle1);
                DrawLinearFunction(eqTangentLine);
                virticalLineList.Add(eqTangentLine);
            }
            else if (Math.Round(distanceSq, 4) == Math.Round(radiusSubtractSq, 1) && r1 - r2 != 0)
            {   // solutionNum = 1;  d == r1 - r2  ２円内接、外分点
                PointF externalPoint = AlgoExternalPoint(r1, r2, origin1, origin2);
                pointList.Add(externalPoint);

                EquationLinear eqTangentLine = AlgoTangentLineOnCircle(externalPoint, eqCircle1);
                DrawLinearFunction(eqTangentLine);
                virticalLineList.Add(eqTangentLine);
            }

            segmentPairAryOut = segmentPairList.ToArray();
            virticalLineAryOut = virticalLineList.ToArray();
            return pointList.ToArray();
        }//AlgoSimultaneousCircleBoth()

        private PointF[] AlgoSimultaneousCircleLinear(
            EquationCircle eqCircle, EquationLinear eqLinear)
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
