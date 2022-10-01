using System;
using System.Drawing;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class EquationCircle : ICoordinateEquation
    {
        public decimal Radius { get; private set; }
        public PointF CircleCenterPoint { get; private set; }
        public string Text { get; set; }

        public EquationCircle(decimal radius, float p, float q)
            : this(radius, new PointF(p, q)) { }

        public EquationCircle(decimal radius, PointF cirleCenterPoint)
        {
            this.Radius = radius;
            this.CircleCenterPoint = cirleCenterPoint;
            this.Text = BuildText(radius, cirleCenterPoint);
        }//constructor

        private string BuildText(decimal radius, PointF circleCenterPoint)
        {
            // Circle Equation  (x - p) ^ 2 + (y - q) ^ 2 = r ^ 2 
            if( radius <= 0)
            {
                throw new ArgumentException("Circle should have plus radius.");
            }

            string pStr = circleCenterPoint.X > 0 ?
                $"- {circleCenterPoint.X:0.##}" : $"+ {-circleCenterPoint.X:0.##}";
            string qStr = circleCenterPoint.Y > 0 ?
                $"- {circleCenterPoint.Y:0.##}" : $"+ {-circleCenterPoint.Y:0.##}";

            if (circleCenterPoint.X == 0 && circleCenterPoint.Y == 0)
            {
                return $"x ^ 2 + y ^ 2 = {radius:0.##} ^ 2";
            }
            else if (circleCenterPoint.X == 0)
            { 
                return $"x ^ 2 + (y {qStr}) ^ 2 = {radius:0.##} ^ 2";
            }
            else if (circleCenterPoint.Y == 0)
            { 
                return $"(x {pStr}) ^ 2 + y ^ 2 = {radius:0.##} ^ 2"; 
            }

            return $"(x {pStr}) ^ 2 + (y {qStr}) ^ 2 = {radius:0.##} ^ 2";
        }//BuildText()

        (decimal a, decimal b, decimal c) ICoordinateEquation.GetGeneralParam()
        {
            decimal r = Radius;
            decimal p = (decimal)CircleCenterPoint.X;
            decimal q = (decimal)CircleCenterPoint.Y;

            return (r, p, q);
        }//GetGeneralParam()

        string ICoordinateEquation.ToString()
        {
            if (Text == null)
            {
                this.Text = BuildText(Radius, CircleCenterPoint);
            }

            return Text;
        }//ToString()
    }//class
}
