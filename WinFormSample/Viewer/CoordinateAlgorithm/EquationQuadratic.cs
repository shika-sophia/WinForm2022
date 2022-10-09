/*
 *@content ２次方程式を表現するクラス
 *         平方式 y = a (x - p) ^ 2 + q
 *         一般式 y = a x ^ 2 + b x + c
 *         
 *         ・AlgoCoordinateQuadratic の各メソッド引数に代入して利用する
 *           (同じ式の係数を何度も記述することを防ぐ)
 *         ・AlgoCoordinateQuadratic の各メソッド引数に代入するので、相互参照にならないよう
 *           描画のアルゴリズムを記述しない (AlgoCoordinateQuadraticに集約)
 *           
 *         ・計算アルゴリズムは、このクラス
 *           (ただし 解の公式は AlgoCoordinateQuadraticにも重複)
 *         ・ICoordinateEquation で EquationLinear, EquationQuadraticを同一視が可能
 *         ・１次方程式は ２次方程式の一般式 y = a x ^ 2 + b x + c の a = 0で表現可
 *           ※ Linearを EquationQuadraticオブジェクトにするのは可読性に問題がある
 *
 *@subject 平方式 -> 一般式  y = a (x - p) ^ 2 + q を展開して y = a x ^ 2 + b x + c 
 *         y = a (x - p) ^ 2 + q
 *         y = a x ^ 2 - 2 a p x + a * p ^ 2 + q
 *         
 *         (decimal a, decimal b, decimal c)
 *             BuildGeneralParameterQuad(float quadCoefficient, PointF vertex)
 *
 *@subject 一般式 -> 平方式  y = a x ^ 2 + b x + c から 平方完成 y = a (x - p) ^ 2 + q
 *         a x ^ 2 + b x + c = 0
 *         x ^ 2 + (b / a) x + c / a = 0
 *         [x ^ 2 + (b / a) x + b ^ 2 / 4 a ^ 2] - b ^ 2 / 4 a ^ 2 + c / a = 0
 *         (x + b / 2 a) ^ 2 - b ^ 2 / 4 a ^ 2 + c / a = 0
 *         (x - (- b / 2 a)) ^ 2 + (b ^ 2 - 4 a c) / 4 a ^ 2 = 0
 *         
 *         PointF BuildSqureQuad(decimal a, decimal b, decimal c)
 *@subject ２次関数上の点 x座標 -> y座標
 *         ・２次関数の xに代入し、yの値を取得
 *         
 *         float  AlgoParabolaFunctionXtoY(
 *                  float x, EquationQuadratic eqQuad)
 *         float  AlgoParabolaFunctionXtoY(
 *                  float x, float quadCoefficient, float vertexX, float vertexY)
 *                  
 *@subject ２次関数上の点 y座標 -> x座標
 *         ・２次関数の xに代入し、yの値を取得
 *         ・判別式により、解は 2個, 1個, 0個
 *         
 *         float[]  AlgoParabolaFunctionYtoX(
 *                    float y, EquationQuadratic eqQuad)
 *         float[]  AlgoParabolaFunctionYtoX(
 *                    float y, float quadCoefficient, float vertexX, float vertexY)
 *
 *@subject 解の公式 a x ^ 2 + b x + c = 0 の解
 *         ・判別式 AlgoJudge()により、解は 2個, 1個, 0個
 *         
 *         PointF[] AlgoQuadSolutionFormula(
 *                    decimal a, decimal b, decimal c)
 *                    
 *@subject 判別式  D = b ^ 2 - 4 a c
 *         int  AlgoJudge(
 *                decimal a, decimal b, decimal c, out decimal judge)
 *         
 */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class EquationQuadratic : ICoordinateEquation
    {
        public float QuadCoefficient { get; private set; }
        public PointF Vertex { get; private set; }
        public decimal A { get; private set; }
        public decimal B { get; private set; }
        public decimal C { get; private set; }
        public PointF[] InterceptPointX { get; private set; }
        public PointF InterceptPointY { get; private set; }
        public PointF[] EqPointAry { get; private set; }
        public string Text { get; set; }

        public EquationQuadratic(float quadCoefficient, PointF vertex)
        {   
            if(quadCoefficient == 0) 
            {
                throw new ArgumentException("Quadratic should not be 'quadCoefficient == 0' ");
            }

            // y = a (x - p) ^ 2 + q
            this.QuadCoefficient = quadCoefficient;
            this.Vertex = vertex;
            var (a, b, c) = 
                BuildGeneralParameterQuad(quadCoefficient, vertex);
            this.A = a;
            this.B = b;
            this.C = c;
            this.EqPointAry = BuildEqPointAry();
            this.Text = BuildText();
        }//constructor

        public EquationQuadratic(decimal a, decimal b, decimal c)
        {
            if (a == 0M)
            {
                throw new ArgumentException("Quadratic should not be 'a == 0' ");
            }

            // y = a x ^ 2 + b x + c
            this.QuadCoefficient = (float)a;
            this.Vertex = BuildSqureQuad(a, b, c);
            this.A = a;
            this.B = b;
            this.C = c;
            this.EqPointAry = BuildEqPointAry();
            this.Text = BuildText();
        }//constructor

        private (decimal a, decimal b, decimal c) BuildGeneralParameterQuad(float quadCoefficient, PointF vertex)
        {  // y = a (x - p) ^ 2 + q を展開して y = a x ^ 2 + b x + c 
            if( float.IsInfinity(Vertex.X) || float.IsInfinity(vertex.Y) ||
                float.IsNaN(vertex.X) || float.IsNaN(vertex.Y))
            {
                throw new ArgumentException("Vertex should not be Infinity or NaN");
            }
            //y = a x ^ 2 - 2 a p x + a * p ^ 2 + q
            decimal a = (decimal)quadCoefficient;
            decimal b = -2M * a * (decimal)vertex.X;   // b = -2ap
            decimal c = a * (decimal)vertex.X * (decimal)vertex.X
                + (decimal)vertex.Y;                   // c = a * p ^ 2 + q  

            return (a, b, c);
        }//BuildGeneral()

        private PointF BuildSqureQuad(decimal a, decimal b, decimal c)
        {   // y = a x ^ 2 + b x + c から 平方完成 y = a (x - p) ^ 2 + q
            // y = (x - (- b / 2 a)) ^ 2 + (b ^ 2 - 4 a c) / 4 a ^ 2 
            float vertexX = (float)(-b / (2M * a));                       // p = -b / 2a
            float vertexY = (float)((b * b - 4M * a * c) / (4M * a * a)); // q = (b ^ 2 - 4ac) / 4a ^ 2

            return new PointF(vertexX, vertexY);
        }//BuildQuad()

        private PointF[] BuildEqPointAry()
        {
            List<PointF> pointList = new List<PointF>();
            this.InterceptPointX = AlgoInterceptX();
            this.InterceptPointY = AlgoInterceptY();

            pointList.Add(Vertex);
            pointList.AddRange(InterceptPointX);
            pointList.Add(InterceptPointY);

            return pointList.ToArray();
        }//BuildEqPointAry()

        public PointF AlgoInterceptY()
        {
            return new PointF(0, AlgoFunctionXtoY(x: 0f)[0]);
        }//AlgoInterceptY()

        public PointF[] AlgoInterceptX()
        {
            List<PointF> pointList = new List<PointF>();

            float[] interceptXAry = AlgoFunctionYtoX(y: 0f);
            foreach (float x in interceptXAry)
            {
                pointList.Add(new PointF(x, 0));
            }

            return pointList.ToArray();
        }//AlgoInterceptX()

        //====== Text ======
        private string BuildText()
        {
            string text;
            string quadCoefficientStr = (QuadCoefficient == 1) ? "" : $"{QuadCoefficient}";
            string vertexXStr = (Vertex.X > 0) ? $"- {Vertex.X}" : $"+ {-Vertex.X}";
            string vertexYStr = (Vertex.Y > 0) ? $"+ {Vertex.Y}" : $"- {-Vertex.Y}";

            if(Vertex.X == 0 && Vertex.Y == 0)
            {
                text = $"y = {quadCoefficientStr} x ^ 2";
            }
            else if (Vertex.X == 0)
            {
                text = $"y = {quadCoefficientStr} x ^ 2 {vertexYStr}";
            }
            else if (Vertex.Y == 0)
            {
                text = $"y = {quadCoefficientStr} ( x {vertexXStr} ) ^ 2";
            }
            else
            {
                text = $"y = {quadCoefficientStr} ( x {vertexXStr} ) ^ 2 {vertexYStr}";
            }

            return text;
        }//BuildText()

        public override string ToString()
        {
            if(Text == null)
            {
                this.Text = BuildText();
            }

            return Text;
        }//ToString()

        //【Deprecated】非推奨 Quadratic should not contain Linear, because of readablility. 
        // 非推奨: 型変換は可能だが可読性の観点から避けるべき 
        public EquationLinear ToLinear()
        {
            return new EquationLinear((float)this.B, (float)this.C);
        }//ToLinear()

        //====== Getter for ICoordinateEquation ======
        public (decimal a, decimal b, decimal c) GetGeneralParameter()
        {
            return (A, B, C);
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
            return new float[] { AlgoParabolaFunctionXtoY(x, A, B, C) };
        }

        public float[] AlgoFunctionYtoX(float y)
        {
            return AlgoQuadSolutionFormula(A, B, C - (decimal)y);
        }

        protected float AlgoParabolaFunctionXtoY(float x, decimal a, decimal b, decimal c)
        {   // y = a x ^ 2 + b x + c
            return (float)(a * (decimal)x * (decimal)x + b * (decimal)x + c);
        }

        protected float AlgoParabolaFunctionXtoY(
            float x, float quadCoefficient, float vertexX, float vertexY)
        {
            // ２次関数 y = a(x - p)^2 + q  
            return (float)((decimal)quadCoefficient
                * ((decimal)x - (decimal)vertexX) * ((decimal)x - (decimal)vertexX)
                + (decimal)vertexY);
        }//AlgoParabolaFunctionXtoY()

        public float[] AlgoQuadSolutionFormula()
        {
            return AlgoQuadSolutionFormula(A, B, C);
        }

        public float[] AlgoQuadSolutionFormula(decimal a, decimal b, decimal c)
        {   // ２次方程式の解の公式
            if (a == 0)
            {
                throw new ArgumentException();
            }

            int solutionNum = AlgoJudge(a, b, c, out decimal judge);
            float[] solutionXAry = new float[solutionNum];

            if (judge > 0)
            {
                solutionXAry[0] =
                    (float)((-b + (decimal)Math.Sqrt((double)judge)) / (2M * a));
                solutionXAry[1] =
                    (float)((-b - (decimal)Math.Sqrt((double)judge)) / (2M * a));
            }
            else if (judge == 0)
            {
                solutionXAry[0] = (float)(-b / (2M * a));
            }

            //---- Test Print ----
            //Console.WriteLine("AlgoQuadSolutionFormula.solutionXAry:");
            //foreach (float solutionX in solutionXAry)
            //{
            //    Console.Write($"{solutionX}, ");
            //}
            //Console.WriteLine("\n");

            return solutionXAry;
        }//AlgoQuadSolutionFormula()

        public int AlgoJudge(EquationQuadratic eqQuad, out decimal judge)
        {   
            return AlgoJudge(eqQuad.A, eqQuad.B, eqQuad.C, out judge);
        }

        public int AlgoJudge(
            decimal a, decimal b, decimal c, out decimal judge)
        {   // 判別式 D = b ^ 2 - 4 a c  
            judge = b * b - 4M * a * c;

            int solutionNum = 0;
            if (Math.Round(judge, 4) > 0) { solutionNum = 2; }
            if (Math.Round(judge, 4) == 0) { solutionNum = 1; }
            if (Math.Round(judge, 4) < 0) { solutionNum = 0; }
            //※Math.Round()の理由 =>〔AlgoCoordinateDifferentiate.cs〕

            return solutionNum;
        }//AlgoJudge()
    }//class
}

/*
////==== Test Main() ====
//static void Main()
//{
//    var eq1 = new EquationQuadratic(2.0f, new PointF(-100, -200));
//    TestPrint(eq1);
//    Console.WriteLine();

//    var eq2 = new EquationQuadratic(eq1.A, eq1.B, eq1.C);
//    TestPrint(eq2);
//    Console.WriteLine();

//    var eq3 = new EquationQuadratic(1M, -2M, 2M);
//    TestPrint(eq3);
//}//Main()

//private static void TestPrint(EquationQuadratic eq)
//{
//    Console.WriteLine($"QuadCoefficient: {eq.QuadCoefficient}");
//    Console.WriteLine($"Vertex: ({eq.Vertex.X},{eq.Vertex.Y})");
//    Console.WriteLine($"A = {eq.A}");
//    Console.WriteLine($"B = {eq.B}");
//    Console.WriteLine($"C = {eq.C}");
//    Console.WriteLine($"Text: {eq.ToString()}");
//}

QuadCoefficient: 2
Vertex: (-100,-200)
A = 2
B = 400
C = 19800
Text: y = 2 ( x + 100 ) ^ 2 - 200

QuadCoefficient: 2
Vertex: (-100,-200)
A = 2
B = 400
C = 19800
Text: y = 2 ( x + 100 ) ^ 2 - 200

QuadCoefficient: 1
Vertex: (1,1)
A = 1
B = -2
C = 2
Text: y =  ( x - 1 ) ^ 2 + 1
 */