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
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class AlgoCoordinateLinear : AlgoCoordinateAxis
    {
        public AlgoCoordinateLinear(PictureBox pic) : base(pic) { }

        public void DrawMultiLinearFunciton(EquationLinear[] eqAry)
        {
            List<PointF> pointList = new List<PointF>();
            foreach(EquationLinear eqLinear in eqAry)
            {
                if (float.IsNaN(AlgoInterceptX(eqLinear).X)) { continue; }
                if (float.IsNaN(AlgoInterceptY(eqLinear).Y)) { continue; }
                pointList.Add(AlgoInterceptX(eqLinear));
                pointList.Add(AlgoInterceptY(eqLinear));
            }

            for (int i = 0; i < eqAry.Length; i++) 
            {
                for(int j = i; j < eqAry.Length; j++)
                {
                    
                    if(j == i) { continue; }

                    bool existSolution = TrySolution(
                        eqAry[i].Slope, eqAry[i].Intercept,
                        eqAry[j].Slope, eqAry[j].Intercept,
                        out PointF solutionPoint);

                    if (existSolution) 
                    {
                        pointList.Add(solutionPoint);
                    }
                }//for j
            }//for i
            
            DrawMultiPointLine(pointList.ToArray(), false);

            foreach(EquationLinear eqLinear in eqAry)
            {
                DrawLinearFunction(eqLinear);
            }
        }//DrawMultiLinearFunciton()

        public void DrawLinearFunction(EquationLinear eqLinear)
        {
            float slope = eqLinear.Slope;
            float intercept = eqLinear.Intercept;
            //Console.WriteLine($"slope = {slope}, intercept = {intercept}");

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
                    minX + 10f : minX - textSize.Width - 5f) * scaleRate);
                textLocation.Y = (float)((decimal)-(minY + 20f) * scaleRate);
            }
            else
            {   // 傾きが緩やかで、y切片 0 のとき、x座標の境界が直線の端になる
                minX = (float)((decimal)-centerPoint.X / scaleRate);
                minY = AlgoLinearFunctionXtoY(minX, slope, intercept);
                maxX = (float)((decimal)centerPoint.X / scaleRate);
                maxY = AlgoLinearFunctionXtoY(maxX, slope, intercept);

                textLocation.X = (float)((decimal)maxX * scaleRate - (decimal)textSize.Width- 5M);
                textLocation.Y = (float)((decimal)((slope > 0) ?
                    -(maxY + textSize.Height) : -maxY) * scaleRate);
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

        private (float slope, float intercept) AlgoLinearParam(PointF pt1, PointF pt2)
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

        private float AlgoLinearParam(float slope, PointF pt1)
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
            // y = a x + b
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
    }//class
}
