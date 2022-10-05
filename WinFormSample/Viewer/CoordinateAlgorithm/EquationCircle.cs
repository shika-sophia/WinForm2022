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

        public bool CheckOnLine(PointF pt)
        {
            // (x - p) ^ 2 + (y - q) ^ 2 = r ^ 2
            decimal radius = Radius;
            PointF circleCenterPoint = CircleCenterPoint;
            decimal dx = (decimal)pt.X - (decimal)circleCenterPoint.X;
            decimal dy = (decimal)pt.Y - (decimal)circleCenterPoint.Y;

            return dx * dx + dy * dy == radius * radius;
        }//CheckOnLine()

        public float[] AlgoFunctionXtoY(float x)
        {
            return AlgoCircleFunctionXtoY(x, this);
        }

        public float[] AlgoFunctionYtoX(float y)
        {
            return AlgoCircleFunctionYtoX(y, this);
        }

        public float[] AlgoCircleFunctionXtoY(float x, EquationCircle eqCircle)
        {
            decimal radius = eqCircle.Radius;
            decimal p = (decimal)eqCircle.CircleCenterPoint.X;
            decimal q = (decimal)eqCircle.CircleCenterPoint.Y;

            // (x - p) ^ 2 + (y - q) ^ 2 = r ^ 2
            // (x - p) ^ 2 + y ^ 2 - 2 q y + q ^ 2 = r ^ 2
            var eqQuadX = new EquationQuadratic(1f, new PointF((float)p, 0f));
            float solutionX = eqQuadX.AlgoFunctionXtoY(x)[0];   // z = (x - p) ^ 2 

            // y ^ 2 - 2 q y + q ^ 2 - (x - p) ^ 2 - r ^ 2 = 0
            var eqQuadY = new EquationQuadratic(
                a: 1M,
                b: -2M * q,
                c: q * q - (decimal)solutionX - radius * radius);

            return eqQuadY.AlgoQuadSolutionFormula();
        }//AlgoCircleFunctionXtoY()

        public float[] AlgoCircleFunctionYtoX(float y, EquationCircle eqCircle)
        {
            decimal radius = eqCircle.Radius;
            decimal p = (decimal)eqCircle.CircleCenterPoint.X;
            decimal q = (decimal)eqCircle.CircleCenterPoint.Y;

            // (x - p) ^ 2 + (y - q) ^ 2 = r ^ 2
            // x ^ 2 - 2 p x + p ^ 2 + (y - q) ^ 2 = r ^ 2
            var eqQuadY = new EquationQuadratic(1f, new PointF((float)q, 0f));
            float solutionY = eqQuadY.AlgoFunctionXtoY(y)[0];   // z = (y - p) ^ 2 

            // x ^ 2 - 2 p x + p ^ 2 - (y - q) ^ 2 - r ^ 2 = 0
            var eqQuadX = new EquationQuadratic(
                a: 1M,
                b: -2M * p,
                c: p * p - (decimal)solutionY - radius * radius);

            return eqQuadX.AlgoQuadSolutionFormula();
        }//AlgoCircleFunctionYtoX()
    }//class
}
