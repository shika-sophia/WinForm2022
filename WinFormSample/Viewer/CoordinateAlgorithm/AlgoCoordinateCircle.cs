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
 *        【幾何的解法】両側 2, 交差 2で場合分けが必要
 *         ＊両側:  相似で接点座標を求める
 *         ・平行  O1 C1 // O2 C2 
 *           円の中心 O1, O2 / 接点 C1, C2 とすると、O1 C1 // O2 C2 平行 (接線 ⊥ 半径線)
 * 
 *         ・O1 の接点 C1 (dx, dy)と置く。 dx, dy は O1との差。
 *               ・ C1                ・ C2
 *               |\                   |\
 *               | \                  | \
 *            dy |  \ r1          dy2 |  \ r2
 *               |   \                |   \
 *           V1  |┐___\ O1            |    \
 *                 dx             V2  |┐____\ O2
 *                                      dx2
 *                                      
 *         ・O1 C1 // O2 C2 平行より、三角相当で相似
 *           dx : dx2 = r1 : r2  ->  dx2 = (r1 / r2)dx = R dx  (R = r1 / r2)
 *           dy : dy2 = r1 : r2  ->  dy2 = (r1 / r2)dy = R dy
 *           
 *         ・三平方の定理
 *           dy^2 = r1^2 - dx^2
 *           dy2 ^2 = (r1 / r2)^2 (r1^2 - dx^2) = R^2 (r1^2 - dx^2)
 *
 *         ・O1 C1, O2 C2の傾きは平行のため等しい
 *         (dy / dx) = (dy2 / dx2)  
 *         dx dy2 = dy dx2         <- 両辺２乗して、dx2, dy, dy2を代入
 *         dx^2 * [R^2 (r1^2 - dx^2)] = (r1^2 - dx^2) R dx
 *         dx^2 * (R^2 r1^2 - R^2 dx^2) = R r1^2 dx^3 ...
 *         
 *         
 *         
 *     
 * 
 * 
 * 
 * 
 * 
 * 
 *                 
 *           O2 の接点 C2 は 
 *         
 *         
 *         ＊交差 中心線を r1 : r2 の内分点で交差するので、極の接線
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
        public void DrawMultiCircleFunction(decimal scaleRateHere, 
            ICoordinateEquation[] eqAry, 
            params PointF[] pointAryArgs)
        {
            SetScaleRate(scaleRateHere);

            //---- Test Print ----
            Console.WriteLine("Equation Array:");
            foreach (var eq in eqAry) { Console.WriteLine(eq); };
            Console.WriteLine("\nArgument pointAry:");
            foreach (var pt in pointAryArgs) { Console.Write($"({pt.X},{pt.Y}), "); }
            Console.WriteLine("\n");

            //---- pointList ----
            List<PointF> pointList = new List<PointF>(pointAryArgs);
            List<EquationLinear> virticalLineList = new List<EquationLinear>();
            List<PointF> segmentList = new List<PointF>();

            for (int i = 0; i < eqAry.Length; i++)
            {
                ICoordinateEquation eq = eqAry[i];
                
                for(int j = i; j < eqAry.Length; j++)
                {
                    //case same
                    if(j == i) { continue; }

                    //---- TrySolutionCircle() ----
                    bool existSolution = TrySolutionCircle(eqAry[i], eqAry[j], 
                        out PointF[] solutionAry);

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
            DrawMultiPointLine(pointList.ToArray(), autoScale: false);
            //autoScale false: need SetScaleRate(decimal) in AlgoSimultaneousCircleBoth 
            
            Console.WriteLine($"scaleRate = {scaleRate}");
            Console.WriteLine($"scaleRateHere = {scaleRateHere}");

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
                return AlgoTangentLineOutCircle(ptOnCircle, eqCircle, out PointF[] contactPointAry)[0];
            }

            EquationLinear radiusLine = AlgoRadiusLine(ptOnCircle, eqCircle);

            return AlgoVirticalLine(ptOnCircle, radiusLine);
        }//AlgoTangentLineOnCircle()

        public EquationLinear[] AlgoTangentLineOutCircle(PointF polar, EquationCircle eqCircle, out PointF[] contactPointAry)
        {
            if (eqCircle.CheckOnLine(polar))
            {
                Console.WriteLine("PointF pt is on the EquationCircle.");
                contactPointAry = new PointF[] { polar };
                return new EquationLinear[] { AlgoTangentLineOnCircle(polar, eqCircle) };
            }

            PointF origin = eqCircle.CircleCenterPoint;
            decimal distanceSq = AlgoDistanceSq(origin, polar);         // d^2
            decimal distance = (decimal)Math.Sqrt((double)distanceSq);  // d  【Deprecated】非推奨: Math.Sqrt(double)
            decimal radiusSq = eqCircle.RadiusSq;              // r^2
            decimal radius = eqCircle.Radius;                  // r
            decimal polarToContactSq = distanceSq - radiusSq;  // CP^2

            List<EquationLinear> tangentLineList = new List<EquationLinear>();
            List<PointF> contactPointList = new List<PointF>();

            if(polarToContactSq > 0)  // d > r
            {
                EquationLinear centerPolarLine = new EquationLinear(origin, polar);

                // △OCP ∽ △ OCQ (相似: 三角相等) d : r = r : OQ  -> OQ = r^2 / d
                // △OCP ∽ △ CQP (相似: 三角相等) d : √(d^2 - r^2) = √(d^2 - r^2) : PQ  -> PQ = (d^2 - r^2) / d
                // OQ : PQ = r^2 : (d^2 - r^2)
                PointF pointQ = AlgoInternalPoint(radiusSq, polarToContactSq, origin, polar);

                // OP ⊥ CQ  (二等辺三角形の角二等分線)
                EquationLinear polarLine = AlgoVirticalLine(pointQ, centerPolarLine);

                // 円と極線の連立
                contactPointList.AddRange(AlgoSimultaneousCircleLinear(eqCircle, polarLine));
                
                //---- 接線 ----
                //---- Draw ----
                penViolet.Width = 1;
                penViolet.DashStyle = DashStyle.Dash;
                DrawLinearSegment(penViolet, origin, polar);      // OP

                foreach(PointF contactPoint in contactPointList)
                {
                    var tangentLine = new EquationLinear(polar, contactPoint);
                    tangentLineList.Add(tangentLine);

                    DrawLinearSegment(penViolet, origin, contactPoint);   // OC, OC'
                    DrawVirticalMark(tangentLine, new EquationLinear(origin, contactPoint),  // OC ⊥ CP
                        plusX: (origin.X < contactPoint.X) ? false : true, 
                        plusY: (origin.Y < contactPoint.Y) ? false : true);
                }//foreach

                DrawPointLine(pointQ);
                if(contactPointList.Count == 2)
                {
                    DrawLinearSegment(penViolet, contactPointList[0], contactPointList[1]);  // CC'
                    DrawVirticalMark(centerPolarLine, polarLine);                            // OP ⊥ CQ
                }

            }// if d > r
            //else if (pointToContactSq == 0)  // d == r  -> AlgoTangentOnCircle()
            //else if (pointToContactSq < 0)   // d < r   -> (No solution)

            contactPointAry = contactPointList.ToArray();
            return tangentLineList.ToArray();
        }//AlgoTangentLineOutCircle()

        public EquationLinear[] AlgoCotangentLineTwoCircle(EquationCircle eqCircle1, EquationCircle eqCircle2)
        {
            decimal r1 = eqCircle1.Radius;
            decimal r2 = eqCircle2.Radius;
            decimal radiusSumSq = (r1 + r2) * (r1 + r2);
            decimal radiusSubtractSq = (r1 - r2) * (r1 - r2);
            PointF origin1 = eqCircle1.CircleCenterPoint;
            PointF origin2 = eqCircle2.CircleCenterPoint;
            decimal distanceSq = AlgoDistanceSq(origin1, origin2);

            List<EquationLinear> cotangentList = new List<EquationLinear>();

            //(editing...)

            return cotangentList.ToArray();
        }//AlgoCotangentLineTwoCircle()

        //====== TrySolutionCircle() ======
        public bool TrySolutionCircle(
            ICoordinateEquation eq1, ICoordinateEquation eq2, 
            out PointF[] solutionAry)
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

            penViolet.Width = 1.0f;
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
                cos = (origin1.X < origin2.X) ? cos : -cos;  // ∠C が鈍角の場合  -cosC

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
            else if (Math.Round(distanceSq, 4) == Math.Round(radiusSumSq, 1))
            {   // solutionNum = 1;  d == r1 + r2  ２円外接、内分点
                PointF internalPoint = AlgoInternalPoint(r1, r2, origin1, origin2);
                pointList.Add(internalPoint);

                EquationLinear eqTangentLine = AlgoTangentLineOnCircle(internalPoint, eqCircle1);
                DrawLinearFunction(eqTangentLine);
            }
            else if (Math.Round(distanceSq, 4) == Math.Round(radiusSubtractSq, 1) && r1 - r2 != 0)
            {   // solutionNum = 1;  d == r1 - r2  ２円内接、外分点
                PointF externalPoint = AlgoExternalPoint(r1, r2, origin1, origin2);
                pointList.Add(externalPoint);

                EquationLinear eqTangentLine = AlgoTangentLineOnCircle(externalPoint, eqCircle1);
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
