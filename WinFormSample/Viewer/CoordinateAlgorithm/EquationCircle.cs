using System;
using System.Collections.Generic;
using System.Drawing;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class EquationCircle : ICoordinateEquation
    {
        public decimal Radius { get; private set; }
        public PointF CircleCenterPoint { get; private set; }
        public PointF[] InterceptPointX { get; private set; }
        public PointF[] InterceptPointY { get; private set; }
        public PointF[] EqPointAry { get; private set; }
        public string Text { get; set; }

        public EquationCircle(decimal radius, PointF cirleCenterPoint)
        {
            if (radius <= 0)
            {
                throw new ArgumentException("Circle radius should be plus value.");
            }

            this.Radius = radius;
            this.CircleCenterPoint = cirleCenterPoint;
            this.InterceptPointX = AlgoInterceptX();
            this.InterceptPointY = AlgoInterceptY();
            this.EqPointAry = BuildEqPointAry();
            this.Text = BuildText(radius, cirleCenterPoint);
        }//constructor

        public EquationCircle(decimal radius, float p, float q)
            : this(radius, new PointF(p, q)) { }

        private PointF[] BuildEqPointAry()
        {
            List<PointF> pointList = new List<PointF>();
            pointList.Add(CircleCenterPoint);
            pointList.AddRange(InterceptPointX);
            pointList.AddRange(InterceptPointY);

            return pointList.ToArray();
        }//BuildEqPointAry()

        private PointF[] AlgoInterceptX()
        {
            List<PointF> pointList = new List<PointF>();

            if ((decimal)CircleCenterPoint.Y < Radius)       // d < r
            {
                float[] xAry = AlgoFunctionYtoX(y: 0f);      // y = 0 | (x - p) ^ 2 + (y - q) ^ 2 = r ^ 2
                pointList.Add(new PointF(xAry[0], 0));
                pointList.Add(new PointF(xAry[1], 0));
            }
            else if ((decimal)CircleCenterPoint.Y == Radius) // d == r
            {
                pointList.Add(new PointF(CircleCenterPoint.X, 0)); 
            }

            return pointList.ToArray();
        }//AlgoInterceptX()

        private PointF[] AlgoInterceptY()
        {
            List<PointF> pointList = new List<PointF>();

            if ((decimal)CircleCenterPoint.X < Radius)  // d < r
            {
                float[] yAry = AlgoFunctionXtoY(x: 0f); // x = 0 | (x - p) ^ 2 + (y - q) ^ 2 = r ^ 2
                pointList.Add(new PointF(0, yAry[0]));
                pointList.Add(new PointF(0, yAry[1]));
            }
            else if ((decimal)CircleCenterPoint.X == Radius) // d == r
            {
                pointList.Add(new PointF(0, CircleCenterPoint.Y));
            }

            return pointList.ToArray();
        }//AlgoInterceptY()

        //====== Text ======
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

        public override string ToString()
        {
            if (Text == null)
            {
                this.Text = BuildText(Radius, CircleCenterPoint);
            }

            return Text;
        }//ToString()

        //====== Getter for ICoordinateEquation ======
        public (decimal a, decimal b, decimal c) GetGeneralParameter()
        {
            decimal p = (decimal)CircleCenterPoint.X;
            decimal q = (decimal)CircleCenterPoint.Y;
            decimal r = Radius;

            return (p, q, r);
        }//GetGeneralParam()

        public PointF[] GetEqPointAry()
        {
            return EqPointAry;
        }

        //====== Algo ======
        public bool CheckOnLine(PointF pt)
        {
            // (x - p) ^ 2 + (y - q) ^ 2 = r ^ 2
            decimal dx = (decimal)pt.X - (decimal)CircleCenterPoint.X;
            decimal dy = (decimal)pt.Y - (decimal)CircleCenterPoint.Y;

            return dx * dx + dy * dy == Radius * Radius;
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

        public int AlgoJudgeCircle(PointF pt)
        {
            decimal dx = (decimal)CircleCenterPoint.X - (decimal)pt.X;
            decimal dy = (decimal)CircleCenterPoint.Y - (decimal)pt.Y;
            decimal distanceSqure = dx * dx + dy * dy;

            int solutionNum = -1;
            if (distanceSqure > Radius * Radius) { solutionNum = 0; }
            if (distanceSqure == Radius * Radius) { solutionNum = 1; }
            if (distanceSqure < Radius * Radius) { solutionNum = 2; }

            return solutionNum;
        }//AlgoJudgeCircle()
    }//class
}
