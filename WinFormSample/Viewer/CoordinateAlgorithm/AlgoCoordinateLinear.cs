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
 *@see ImageLinearFunctionViewer.jpg
 *@see MainCoordinateAxisViewer.cs
 *@see AlgoCoordinateAxis.cs
 *@see AlgoCoordinateLinear.cs
 *@author shika
 *@date 2022-09-18
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
            (float slope, float intercept) = AlgoLinearParam(pt1, pt2);
            DrawLinearFunction(slope, intercept);
        }//DrawLinearFunction(PointF, PointF)

        public void DrawLinearFunction(float slope, float intercept)
        {
            if(float.IsInfinity(slope))  // x = c (Virtical)
            {
                g.DrawLine(penPink,
                    intercept, -centerPoint.Y, 
                    intercept, centerPoint.Y);
                g.DrawString($"x = {intercept}", font, penPink.Brush, 
                    (float)((decimal)intercept + 10M),
                    -(float)((decimal)centerPoint.Y + 20M));
                return;
            }

            if (slope == 0)  // y = b (Horizontal)
            {
                g.DrawLine(penPink,
                    -centerPoint.X, -intercept,
                    centerPoint.X, -intercept);
                g.DrawString($"y = {intercept}", font, penPink.Brush, 
                    (float)((decimal)centerPoint.X - 70M),
                    -(float)((decimal)intercept + 20M));
            }
            else
            {
                float minX = -centerPoint.X;
                float minY = LinearFunctionXtoY(minX, slope, intercept);
                float maxX = centerPoint.X;
                float maxY = LinearFunctionXtoY(maxX, slope, intercept);

                g.DrawLine(penPink, minX, -minY, maxX, -maxY);  //Y座標を反転
                g.DrawString($"y = {slope} x + {intercept}", font, penPink.Brush,
                    -120f, -(float)((decimal)LinearFunctionXtoY(-120f, slope, intercept) + 20M));
            }           
        }//DrawLinearFunction(double, double)

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

        private float LinearFunctionXtoY(float x, float slope, float intercept)
        {
            // y = a x + b
            return (float)((decimal)slope * (decimal)x + (decimal)intercept);
        }//AlgoLinearFunction(x) -> y

        private float LinearFunctionYtoX(float y, float slope, float intercept)
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
            float slope1, float intercept1,
            float slope2, float intercept2, out PointF solution)
        {
            // y = a x + b | y = cx + d の連立方程式の解
            solution = new PointF(float.NaN, float.NaN);

            if (float.IsInfinity(slope1))  // x = □, 
            {
                solution.X = intercept1;
                solution.Y = LinearFunctionXtoY(
                    solution.X, slope2, intercept2);
                return true;
            }

            if (float.IsInfinity(slope2))  // x = △, 
            {
                solution.X = intercept2;
                solution.Y = LinearFunctionXtoY(
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
                solution.X = LinearFunctionYtoX(
                    solution.Y, slope2, intercept2);
                return true;
            }

            if (slope2 == 0)  // y = a x + b | y = d
            {
                solution.Y = intercept2;
                solution.X = LinearFunctionYtoX(
                    solution.Y, slope1, intercept1);
                return true;
            }

            // y = a x + b | y = cx + d の連立方程式の解
            // x = -(b - d) / (a - c)
            // y = a x + b に xを代入
            solution.X = -(float)(((decimal)intercept1 - (decimal)intercept2) 
                            / ((decimal)slope1 - (decimal)slope2));
            solution.Y = LinearFunctionXtoY(solution.X, slope1, intercept1);
            return true;
        }//TrySolution()
    }//class
}
