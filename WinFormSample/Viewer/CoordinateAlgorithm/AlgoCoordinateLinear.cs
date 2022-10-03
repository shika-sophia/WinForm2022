/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class AlgoCoordinateLinear(PictureBox) : AlgoCoordinateAxis
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content LinearFunctionViewer
 *         １次関数  y = a x + b の描画
 *         
 *         [英] linear:    線形、１次方程式
 *         [英] solution:  解
 *         [英] slope:     傾き
 *         [英] gradient:  勾配
 *         [英] intercept: 切片  x-intercept, y-intercept
 *         
 *@subject Matrix 平行移動
 *         =>〔AlgoCoordinateAxis.cs〕
 *
 *@subject データ型
 *         float:    PointF, graphics.DrawLine()/DrawString()の座標で利用
 *                   float.PositiveInfinity「∞」 
 *                   float.NegativeInfinity「-∞」
 *                   float.NaN              Not a Number 非数
 *                   bool  float.IsInfinity(float)
 *                   bool  float.IsNaN(float)
 *                   
 *                   ※ Single (= float), Double も可
 *                   
 *         decimal:  10進数で小数を表す型。
 *                   浮動小数点数 float, doubleは ２進数に変換して保持。
 *                   float, doubleの計算は２進数変換時の誤差の可能性があるため、
 *                   正確な座標計算には、decimalに変換して行う
 *         
 *@subject AlgoLinearParam  ２点の座標から、１次関数のパラメータを求める
 *         ・２点を通る直線は一意に決定できる
 *         ・２点の X座標, Y座標の差をそれぞれ取り、
 *           傾き a = dy / dx 
 *           Ｙ切片 b は aの値を代入した y = (定数 a) x + bに
 *           １点の座標(x, y)を代入して求める。
 *         ・複数の戻り値は C#「タプル」を参照 〔CS 58〕
 *         
 *         (float slope , float intercept)  AlgoLinearParam(PointF pt1, PointF pt2)
 *         float                            AlgoLinearParam(float slope, PointF pt1)
 *         
 *@subject LinearFunction  X, Y の関係を示す式
 *         ・引数 slope, interceptを渡して、Methodで関係性を再現する。
 *         ・X -> Y,  Y -> X で計算式が異なる
 *         ・「slope = 0」のときの 0除算にならないよう if文で条件分岐することに注意
 *         
 *         float  LinearFunctionXtoY(float x, float slope, float intercept)
 *         float  LinearFunctionYtoX(float y, float slope, float intercept)
 *
 *@subject DrawLinearFunction
 *         ・１次関数  y = a x + b の a, bを確定させ、
 *         ・X軸の右端と左端の X座標を代入し、それぞれの Y座標を求める
 *         ・Yの値が 座標範囲より大きくなっても OK
 *         ・graphics.DrawLine(Pen, PointF, PointF)で描画
 *           Y座標は、数学座標と Graphics座標で反転しているため、
 *           DrawLine(), DrawString()のときだけ Y座標にのみ「-」をつけて反転させる。(描画だけ反転する)
 *           計算時に反転させる必要はない。
 *           
 *@subject 複数の直線を同時に描画
 *         void  DrawMultiLinearFunciton(EquationLinear[] eqAry)
 *         
 *         for(int j = i; j < eqAry.Length; j++)
 *         ・TrySolution() の for文は 自分自身を除外して、他の全ての直線との解
 *         ・j = i から始めることで、重複を除外
 *         
 *         => 対角線の描画アルゴリズム〔FigureAlgorithm/ApplicationFigureViewer.DrawDiagonalLine()〕
 *
 *@subject 垂線 virtical line
 *         ・垂直条件 virtical condition: a c = -1  when y = a x + b | y = c x + d 
 *
 *           EquationLinear  AlgoVirticalLine(EquationLinear, PointF)
 *           
 *@subject 距離 distance
 *         ・三平方の定理 Three Squre Theorem:  z ^ 2 = x ^ 2 + y ^ 2 
 *         
 *         decimal  AlgoDistance(PointF pt1, PointF pt2)
 *
 *@subject 直線上の点の距離
 *         AlgoDistanceOnLinePoint(
 *              decimal distance, bool pulsX, PointF startPoint, EquationLinear eqLinear)
 *         
 *        【三角比の公式】Triangular ratio Formula: 
 *         三角比の定義より、tanθ = sinθ / cosθ
 *         三平方の定理より、cos^2 θ + sin^2 θ = 1
 *         両式の連立により、tan^2 θ + 1 = 1 / cos^2 θ
 *         
 *         これらの公式により、cosθ, sinθ, tanθ のいずれかの値がわかると、
 *         他のすべての値を求めることができる。
 *          =>〔三角比 | FigureAlgorithm\MainMultiAngleViewer.cs〕
 *         
 *@subject AlgoVirticleMark()
 *         tanθ = dy / dx = 傾き slope (直線の式から値がわかる)。
 *         斜辺の長さ distanceは所与 (引数で与えられる)
 *         求める点の座標 (x, y) = (distance * cosθ, distance * sinθ)
 *         点 (p, q)からの点は (p + distance * cosθ, q + distance * sinθ)
 *         上記の公式より、cosθ = √ (1 / tan^2 θ)
 *         
 *        【註】平行線は、その線上の点を指定すると、同一の直線になる
 *         傾きが同じで、直線外の点を指定して EquationLinearを作成する。
 *          
 *@see ImageLinearFunctionViewer.jpg
 *@see MainCoordinateAxisViewer.cs
 *@see AlgoCoordinateAxis.cs
 *@see AlgoCoordinateLinear.cs
 *@author shika
 *@date 2022-09-18
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class AlgoCoordinateLinear : AlgoCoordinateAxis
    {
        public AlgoCoordinateLinear(PictureBox pic) : base(pic) { }

        public void DrawMultiLinearFunciton(
            EquationLinear[] eqAry, params PointF[] pointAryArgs)
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
            
            foreach(EquationLinear eqLinear in eqAry)
            {
                if (float.IsNaN(AlgoInterceptX(eqLinear).X)) { continue; }
                if (float.IsNaN(AlgoInterceptY(eqLinear).Y)) { continue; }
                pointList.Add(AlgoInterceptX(eqLinear));
                pointList.Add(AlgoInterceptY(eqLinear));
            }

            //---- Simultaneous Equations 連立方程式 ----
            for (int i = 0; i < eqAry.Length; i++) 
            {
                for(int j = i; j < eqAry.Length; j++)
                {
                    if(j == i) { continue; }

                    bool existSolution = TrySolution(
                        eqAry[i], eqAry[j], out PointF solutionPoint);

                    if (existSolution)
                    {
                        pointList.Add(solutionPoint);
                    }

                    if (IsVirtical(eqAry[i], eqAry[j]))
                    {
                        virticalLineList.Add(eqAry[i]);
                        virticalLineList.Add(eqAry[j]);
                    }

                    Console.WriteLine($"existSolution = {existSolution}");
                    Console.WriteLine($"solution: ({solutionPoint.X},{solutionPoint.Y})");
                    Console.WriteLine($"IsVirticle = {IsVirtical(eqAry[i],eqAry[j])}");
                }//for j
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
            DrawMultiPointLine(pointList.ToArray(), false);
            Console.WriteLine($"scaleRate = {scaleRate}");
            Console.WriteLine($"height / width = {ratioWidthHeight:0.##}");

            foreach (EquationLinear eqLinear in eqAry)
            {
                DrawLinearFunction(eqLinear);
            }

            if(virticalLineList.Count > 0)
            {
                DrawVirticalMark(virticalLineList.ToArray());
            }
        }//DrawMultiLinearFunciton()

        public void DrawLinearFunction(EquationLinear eqLinear)
        {
            float slope = eqLinear.Slope;
            float intercept = eqLinear.Intercept;

            if (float.IsInfinity(slope))  // x = c (Virtical)
            {
                g.DrawLine(penPink,
                    (float)((decimal)intercept * scaleRate),
                    (float)((decimal)-centerPoint.Y * scaleRate),
                    (float)((decimal)intercept * scaleRate),
                    (float)((decimal)centerPoint.Y * scaleRate));
         
                g.DrawString($"x = {intercept}", font, penPink.Brush,
                    (float)((decimal)intercept * scaleRate + 5M), 20f);
                return;
            }

            if (slope == 0)  // y = b (Horizontal)
            {
                g.DrawLine(penPink,
                    (float)((decimal)-centerPoint.X * scaleRate),
                    (float)((decimal)-intercept * scaleRate),
                    (float)((decimal)centerPoint.X * scaleRate),
                    (float)((decimal)-intercept * scaleRate));

                SizeF textSizeHorizontal = g.MeasureString(eqLinear.Text, font);
                g.DrawString($"y = {intercept}", font, penPink.Brush,
                    -textSizeHorizontal.Width - 5f,
                    (float)((decimal)-intercept * scaleRate + 10M));
                return;
            }

            //---- y = a x + b ----
            float minX, minY, maxX, maxY;
            PointF textLocation = new PointF(0, 0);
            SizeF textSize = g.MeasureString(eqLinear.Text, font);

            if (Math.Abs(ratioWidthHeight) < Math.Abs((decimal)slope))
            {   // 傾きが急で、y切片 0 のとき、y座標の境界が直線の端になる
                minY = (float)((decimal)-centerPoint.Y / scaleRate);
                minX = AlgoLinearFunctionYtoX(minY, slope, intercept);
                maxY = (float)((decimal)centerPoint.Y / scaleRate);
                maxX = AlgoLinearFunctionYtoX(maxY, slope, intercept);

                textLocation.X = (float)((decimal)((slope > 0) ?
                    ((decimal)minX * scaleRate - (decimal)textSize.Width - 5M) : (decimal)minX * scaleRate + 10M));
                textLocation.Y = (float)(-(decimal)minY * scaleRate - (decimal)textSize.Height);
            }
            else
            {   // 傾きが緩やかで、y切片 0 のとき、x座標の境界が直線の端になる
                minX = (float)((decimal)-centerPoint.X / scaleRate);
                minY = AlgoLinearFunctionXtoY(minX, slope, intercept);
                maxX = (float)((decimal)centerPoint.X / scaleRate);
                maxY = AlgoLinearFunctionXtoY(maxX, slope, intercept);

                textLocation.X = (float)((decimal)maxX * scaleRate - (decimal)textSize.Width- 5M);
                textLocation.Y = (float)((slope > 0 ?
                    -((decimal)maxY * scaleRate + (decimal)textSize.Height) :
                    -((decimal)maxY * scaleRate - (decimal)textSize.Height)));
             }
            
            //---- Draw ----
            g.DrawLine(penPink,
                (float)((decimal)minX * scaleRate),
                (float)((decimal)-minY * scaleRate),   //Y座標を反転
                (float)((decimal)maxX * scaleRate),
                (float)((decimal)-maxY * scaleRate));  //Y座標を反転

            g.DrawString(eqLinear.Text, font, penPink.Brush, textLocation);
        }//DrawLinearFunction(EquationLinear)

        public void DrawLinearFunction(PointF pt1, PointF pt2)
        {
            (float slope, float intercept) = AlgoLinearParam(pt1, pt2);
            DrawLinearFunction(new EquationLinear(slope, intercept));
        }//DrawLinearFunction(PointF, PointF)

        public void DrawLinearFunction(float slope, float intercept)
        {
            DrawLinearFunction(new EquationLinear(slope, intercept));
        }//DrawLinearFunction(float, float)

        public bool CheckOnLine(PointF pt, EquationLinear eqLinear)
        {
            float onY = AlgoFunctionXtoY(pt.X, eqLinear);
            return pt.Y == onY;
        }//CheckOnLine()

        public PointF AlgoInterceptY(EquationLinear eqLinear)
        {
            if(float.IsInfinity(eqLinear.Slope))
            { return new PointF(float.NaN, float.NaN); }

            return new PointF(0, eqLinear.Intercept);
        }//AlgoInterceptY()

        public PointF AlgoInterceptX(EquationLinear eqLinear)
        {
            PointF pt = new PointF(float.NaN, float.NaN);

            if (float.IsInfinity(eqLinear.Slope)) 
            {
                pt.X = eqLinear.Intercept;
                pt.Y = 0;
            }
            else if (eqLinear.Slope == 0)
            {
                return pt;
            }
            else
            {
                pt.X = AlgoFunctionYtoX(0, eqLinear);
                pt.Y = 0;
            }

            return pt;
        }//DrawInterceptY()

        protected (float slope, float intercept) AlgoLinearParam(PointF pt1, PointF pt2)
        {
            decimal dx = (decimal)pt1.X - (decimal)pt2.X;
            decimal dy = (decimal)pt1.Y - (decimal)pt2.Y;

            float slope, intercept;
            if (dx == 0M)
            {
                slope = dy > 0M ? float.PositiveInfinity : float.NegativeInfinity;     // a = ∞ or -∞
                intercept = pt1.X;     // x = c
                return (slope, intercept);
            }
            else if (dy == 0M) { slope = 0f; }  // y = b 
            else { slope = (float)(dy / dx); }

            intercept = AlgoLinearParam(slope, pt1);

            return (slope, intercept);
        }//AlgoLinerParam(pt1, pt2)

        protected float AlgoLinearParam(float slope, PointF pt1)
        {
            if (float.IsInfinity(slope))
            {
                throw new ArgumentException("Invalid value 'a = ∞, -∞'");
            }

            // b = y - a x
            return (float)((decimal)pt1.Y - (decimal)slope * (decimal)pt1.X);
        }

        protected float AlgoFunctionXtoY(float x, EquationLinear eqLinear)
        {
            return AlgoLinearFunctionXtoY(x, eqLinear.Slope, eqLinear.Intercept);
        }

        protected float AlgoFunctionYtoX(float y, EquationLinear eqLinear)
        {
            return AlgoLinearFunctionYtoX(y, eqLinear.Slope, eqLinear.Intercept);
        }

        private float AlgoLinearFunctionXtoY(float x, float slope, float intercept)
        {
            if (float.IsInfinity(slope)) { return float.NaN; }  // x = c

            // y = a x + b | y = b
            return (float)((decimal)slope * (decimal)x + (decimal)intercept);
        }//AlgoLinearFunction(x) -> y


        private float AlgoLinearFunctionYtoX(float y, float slope, float intercept)
        {
            float x;
            if (float.IsInfinity(slope))  // x = c
            {
                x = intercept;
            }
            else if(slope == 0)            // y = b
            {
                x = float.NaN;
            }
            else                          // x = (y - b) / a
            {   
                x = (float)(((decimal)y - (decimal)intercept) / (decimal)slope);
            }

            return x;
        }//AlgoLinearFunction(y) -> x

        public bool TrySolution(
            EquationLinear eqLinear1, EquationLinear eqLinear2, out PointF solutionPoint)
        {
            bool existSolution = TrySolution(
                eqLinear1.Slope, eqLinear1.Intercept,
                eqLinear2.Slope, eqLinear2.Intercept,
                out PointF solution);
            solutionPoint = solution;
            return existSolution;
        }//TrySolution()

        public bool TrySolution(
            float slope1, float intercept1,
            float slope2, float intercept2, out PointF solution)
        {
            // y = a x + b | y = cx + d の連立方程式の解
            solution = new PointF(float.NaN, float.NaN);

            if (float.IsInfinity(slope1))  // x = □, 
            {
                solution.X = intercept1;
                solution.Y = AlgoLinearFunctionXtoY(
                    solution.X, slope2, intercept2);
                return true;
            }

            if (float.IsInfinity(slope2))  // x = △, 
            {
                solution.X = intercept2;
                solution.Y = AlgoLinearFunctionXtoY(
                    solution.X, slope1, intercept1);
                return true;
            }

            if (slope1 == slope2)  // (a - c) == 0, parallel 平行
            {
                return false;
            }

            if (slope1 == 0)  // y = b | y = c x + d
            {
                solution.Y = intercept1;
                solution.X = AlgoLinearFunctionYtoX(
                    solution.Y, slope2, intercept2);
                return true;
            }

            if (slope2 == 0)  // y = a x + b | y = d
            {
                solution.Y = intercept2;
                solution.X = AlgoLinearFunctionYtoX(
                    solution.Y, slope1, intercept1);
                return true;
            }

            // y = a x + b | y = cx + d の連立方程式の解
            // x = -(b - d) / (a - c)
            // y = a x + b に xを代入
            solution.X = -(float)(((decimal)intercept1 - (decimal)intercept2) 
                            / ((decimal)slope1 - (decimal)slope2));
            solution.Y = AlgoLinearFunctionXtoY(solution.X, slope1, intercept1);
            return true;
        }//TrySolution()

        public bool IsVirtical(EquationLinear eqLinear1, EquationLinear eqLinear2)
        {
            if (float.IsInfinity(eqLinear1.Slope)) { return eqLinear2.Slope == 0; }
            if (float.IsInfinity(eqLinear2.Slope)) { return eqLinear1.Slope == 0; }
            if (eqLinear1.Slope == 0) { return float.IsInfinity(eqLinear2.Slope); }
            if (eqLinear2.Slope == 0) { return float.IsInfinity(eqLinear1.Slope); }

            return eqLinear1.Slope * eqLinear2.Slope == -1;
        }//IsVirtical()

        public EquationLinear AlgoVirticalLine(EquationLinear eqLinear, PointF pt)
        {
            EquationLinear virticalLine;
            if (float.IsInfinity(eqLinear.Slope))  //x = c  ->  y = b
            {
                virticalLine = new EquationLinear(slope: 0f, intercept: pt.Y);
            }
            else if (eqLinear.Slope == 0f)  // y = b  -> x = c
            {
                virticalLine = new EquationLinear(
                    slope: float.PositiveInfinity, intercept: pt.X);
            }
            else
            {
                // 垂直条件 virtical condition: a c = -1  when y = a x + b | y = c x + d  
                float slope = (float)(-1M / (decimal)eqLinear.Slope);

                virticalLine = new EquationLinear(slope, pt);
            }

            return virticalLine;
        }//AlgoVirticalLine()

        protected void DrawVirticalMark(EquationLinear[] eqLinearAry)
        {
            for(int i = 0; i < eqLinearAry.Length; i += 2)
            {
                 DrawVirticalMark(eqLinearAry[i], eqLinearAry[i + 1]);
            }//for
        }

        protected void DrawVirticalMark(
            EquationLinear eqLinear, EquationLinear virticalLine, 
            bool plusX = true, bool plusY = true)
        {
            if (!IsVirtical(eqLinear, virticalLine))
            {
                throw new ArgumentException("2 lines are not virtical.");
            }

            TrySolution(eqLinear, virticalLine, out PointF solutionPoint);

            PointF pt1 = AlgoDistanceOnLinePoint(
                8M / scaleRate, solutionPoint, eqLinear, plusX, plusY);
            PointF pt3 = AlgoDistanceOnLinePoint(
                8M / scaleRate, solutionPoint, virticalLine, plusX, plusY);

            EquationLinear eq1Parallel = new EquationLinear(eqLinear.Slope, pt3);
            EquationLinear eq2Parallel = new EquationLinear(virticalLine.Slope, pt1);
            TrySolution(eq1Parallel, eq2Parallel, out PointF pt2);

            penPink.Width = 0.5f;
            g.DrawLine(penPink,
                (float)((decimal)pt1.X * scaleRate),
                (float)((decimal)-pt1.Y * scaleRate),
                (float)((decimal)pt2.X * scaleRate),
                (float)((decimal)-pt2.Y * scaleRate));
            g.DrawLine(penPink,
                (float)((decimal)pt2.X * scaleRate),
                (float)((decimal)-pt2.Y * scaleRate),
                (float)((decimal)pt3.X * scaleRate),
                (float)((decimal)-pt3.Y * scaleRate));
            penPink.Width = 2f;
        }//DrawVirticalMark()

        public decimal AlgoDistance(PointF pt1, PointF pt2)
        {
            // 三平方の定理 Three Squre Theorem:  z ^ 2 = x ^ 2 + y ^ 2 
            decimal dx = (decimal)pt1.X - (decimal)pt2.X;
            decimal dy = (decimal)pt1.Y - (decimal)pt2.Y;

            return (decimal)Math.Sqrt((double)(dx * dx + dy * dy));
        }//AlgoDistance()

        public PointF AlgoDistanceOnLinePoint(
            decimal distance, PointF startPoint, EquationLinear eqLinear,
            bool plusX = true, bool plusY = true)
        {
            PointF pt = new PointF(0, 0);
            float slope = eqLinear.Slope;
            decimal signX = plusX ? 1 : -1;
            decimal signY = plusY ? 1 : -1;

            if (float.IsInfinity(slope))  // x = c
            {
                pt.X = startPoint.X;
                pt.Y = (float)((decimal)startPoint.Y + signY * distance);
            }
            else if (slope == 0)  // y = b
            {
                pt.X = (float)((decimal)startPoint.X + signX * distance);
                pt.Y = startPoint.Y;
            }
            else
            {
                //点(p, q)からの点は(p + distance * cosθ, q + distance * sinθ)
                //上記の公式より、cosθ = √ (1 / tan ^ 2 θ + 1) | tanθ = slope 
                // tan ^ 2 θ > 0なので、 tan ^ 2 θ + 1 != 0
                decimal tan = (decimal)slope;
                decimal cos = (decimal)Math.Sqrt((double)( 1M /(tan * tan + 1M)));
                
                pt.X = (float)((decimal)startPoint.X + signX * distance * cos);
                pt.Y = AlgoFunctionXtoY(pt.X, eqLinear);

                //Console.WriteLine($"tan = {tan}, cos = {cos}, PointF({pt.X},{pt.Y})");
            }

            return pt;
        }//AlgoDistanceOnLinePoint()
    }//class
}

/*
//==== Test Print ====
new FormVirticalLineViewer()
//---- AlgoVirticleLine(), AlgoDistanceOnLine(), AlgoVirticleMark() ----

Equation Array:
y = 2 x - 50
y = -0.5 x + 100

Argument pointAry:
(100,50),

existSolution = True
solution: (60,70)
IsVirticle = True


pointList before Distinct():
(100,50), (25,0), (0,-50), (200,0), (0,100), (60,70),

pointAry after Distinct():
(100,50), (25,0), (0,-50), (200,0), (0,100), (60,70),

tan = 2, cos = 0.447213595499958, PointF(63.57771,77.15542)
tan = -0.5, cos = 0.894427190999916, PointF(67.15542,66.42229)

Close()
 */