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
        public string Text { get; set; }

        public EquationLinear(float slope, PointF pt) 
        {
            this.Slope = slope;
            this.Intercept = CalcIntercept(slope, pt);
            this.Text = BuildText(slope, Intercept);
        }

        public EquationLinear(float slope, float intercept) 
        {
            this.Slope = slope;
            this.Intercept = intercept;
            this.Text = BuildText(slope, intercept);
        }//constructor

        private float CalcIntercept(float slope, PointF pt)
        {
            // y = a x + b
            // b = y - a x 
            return (float)((decimal)pt.Y - (decimal)slope * (decimal)pt.X);
        }

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

        public (decimal a, decimal b, decimal c) GetGeneralParam()
        {
            if (float.IsInfinity(Slope))
            {
                return (0M, Decimal.MaxValue, (decimal)Intercept);
            }

            return (0M, (decimal)Slope, (decimal)Intercept);
        }//GetGeneralParam()
    }//class
}
