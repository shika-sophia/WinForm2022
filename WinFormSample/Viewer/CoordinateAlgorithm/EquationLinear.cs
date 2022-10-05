/** 
 *@title WinFormGUI / WinFormSample / 
 *@class EquationLinear : ICoordinateEquation
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content EquationLinear
 *         線形, １次関数の係数値を保持するクラス
 *         y = a x + b
 *         
 *@subject 直線の決定条件
 *         ・傾き slope, 通る点 PointF
 *         ・傾き slope, y切片 intercept
 *         ・２点 =>〔AlgoCoordinateLinear.AlgoLinearParam〕
 *
 *@subject LinearFunction  X, Y の関係を示す式
 *         ・引数 slope, interceptを渡して、Methodで関係性を再現する。
 *         ・X -> Y,  Y -> X で計算式が異なる
 *         ・「slope = 0」のときの 0除算にならないよう if文で条件分岐することに注意
 *         
 *         float  LinearFunctionXtoY(float x, float slope, float intercept)
 *         float  LinearFunctionYtoX(float y, float slope, float intercept)
 *
 *@see AlgoCoordinateLinear.cs
 *@see 
 *@author shika
 *@date 2022-09-30
 */

using System;
using System.Drawing;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class EquationLinear : ICoordinateEquation
    {
        public float Slope { get; private set; }
        public float Intercept { get; private set; }
        public PointF InterceptPointX { get; private set; }
        public PointF InterceptPointY { get; private set; }
        public PointF[] EqPointAry { get; private set; }
        public string Text { get; set; }

        public EquationLinear(float slope, float intercept) 
        {   
            this.Slope = slope;
            this.Intercept = intercept;
            this.InterceptPointX = AlgoInterceptX();
            this.InterceptPointY = AlgoInterceptY();
            this.EqPointAry = new PointF[] { InterceptPointX, InterceptPointY };
            this.Text = BuildText(slope, intercept);
        }//constructor

        public EquationLinear(float slope, PointF pathPoint)
            : this(slope, CalcIntercept(slope, pathPoint)) { }

        public EquationLinear(PointF pt1, PointF pt2)
            : this(AlgoLinearParam(pt1, pt2)) { }

        public EquationLinear((float slope, float intercept)taple)
            : this(taple.slope, taple.intercept) { }

        //====== static Method for constructor =====
        private static (float slope, float intercept) AlgoLinearParam(PointF pt1, PointF pt2)
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

            intercept = CalcIntercept(slope, pt1);

            return (slope, intercept);
        }//AlgoLinearParam(pt1, pt2)

        private static float CalcIntercept(float slope, PointF pt)
        {
            if (float.IsInfinity(slope)) { return pt.X; } // x = c

            // y = a x + b
            // b = y - a x 
            return (float)((decimal)pt.Y - (decimal)slope * (decimal)pt.X);
        }

        //====== Intercept ======
        public PointF AlgoInterceptX()
        {
            PointF pt = new PointF(float.NaN, float.NaN);

            if (float.IsInfinity(Slope))
            {
                pt.X = Intercept;
                pt.Y = 0;
            }
            else if (Slope == 0)
            {
                return pt;
            }
            else
            {
                pt.X = AlgoFunctionYtoX(y: 0)[0];
                pt.Y = 0;
            }

            return pt;
        }//DrawInterceptX()

        public PointF AlgoInterceptY()
        {
            if (float.IsInfinity(Slope))
            { return new PointF(float.NaN, float.NaN); }

            return new PointF(0, Intercept);
        }//AlgoInterceptY()
       
        //====== Text ======
        private string BuildText(float slope, float intercept)
        {
            string text = null;
            if (float.IsInfinity(slope))  // x = c
            {
                return $"x = {intercept}";
            }
            
            if (slope == 0)  // y = b
            {
                return $"y = {intercept}";
            }

            if(intercept > 0)
            { 
                text = $"y = {slope} x + {intercept}";
            }
            else if (intercept == 0)
            {
                text = $"y = {slope} x";
            }
            else if (intercept < 0)
            {
                text = $"y = {slope} x - {-intercept}";
            }

            return text;
        }//BuildText()

        public override string ToString()
        {
            if(Text == null)
            {
                Text = BuildText(this.Slope, this.Intercept);
            }

            return this.Text;
        }//ToString()

        //【Deprecated】Quadratic should not contain Linear, because of readablility.
        // 非推奨: 型変換は可能だが可読性の観点から避けるべき 
        public EquationQuadratic ToQuad()
        {
            return new EquationQuadratic(
                0M, (decimal)this.Slope, (decimal)this.Intercept);
        }//ToQuad()

        //====== Getter for ICoordinateEquation ======
        public (decimal a, decimal b, decimal c) GetGeneralParameter()
        {
            if (float.IsInfinity(Slope))
            {
                return (0M, Decimal.MaxValue, (decimal)Intercept);
            }

            return (0M, (decimal)Slope, (decimal)Intercept);
        }//GetGeneralParam()

        public PointF[] GetEqPointAry()
        {
            return EqPointAry;
        }//GetEqPointAry()

        //====== Algo ======
        public bool CheckOnLine(PointF pt)
        {
            float onY = AlgoFunctionXtoY(pt.X)[0];
            return pt.Y == onY;
        }//CheckOnLine()

        public float[] AlgoFunctionXtoY(float x)
        {
            return new float[] { AlgoLinearFunctionXtoY(x, Slope, Intercept) };
        }

        public float[] AlgoFunctionYtoX(float y)
        {
            return new float[] { AlgoLinearFunctionYtoX(y, Slope, Intercept) };
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
            else if (slope == 0)            // y = b
            {
                x = float.NaN;
            }
            else                          // x = (y - b) / a
            {
                x = (float)(((decimal)y - (decimal)intercept) / (decimal)slope);
            }

            return x;
        }//AlgoLinearFunction(y) -> x
    }//class
}
