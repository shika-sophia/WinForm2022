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

        //Deprecated 非推奨: 型変換は可能だが可読性の観点から避けるべき 
        public EquationQuadratic ToQuad()
        {
            return new EquationQuadratic(
                0M, (decimal)this.Slope, (decimal)this.Intercept);
        }//ToQuad()

        public (decimal a, decimal b, decimal c) GetGeneralParam()
        {
            return (0M, (decimal)Slope, (decimal)Intercept);
        }//GetGeneralParam()
    }//class
}
