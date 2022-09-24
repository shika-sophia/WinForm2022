/*
 *@content ２次方程式の係数の値を保持しておくためのクラス
 *         ・AlgoCoordinateQuadratic の各メソッド引数に代入して利用する
 *           (同じ式の係数を何度も記述することを防ぐ)
 *         ・AlgoCoordinateQuadratic の各メソッド引数に代入するので、相互参照にならないよう
 *           計算や描画のアルゴリズムを記述しない
 *         ・計算や描画のアルゴリズムは AlgoCoordinateQuadraticに集約
 *           (ただし、平方完成と式の展開は、このクラス)
 *         ・ICoordinateEquation で EquationLinear, EquationQuadraticを同一視が可能
 *         ・１次方程式は ２次方程式の一般式 y = a x ^ 2 + b x + c の a = 0で表現可
 *           ※ Linearを EquationQuadraticオブジェクトにするのは可読性に問題がある
 */
using System;
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
        public string Text { get; set; }

        public EquationQuadratic(float quadCoefficient, PointF vertex)
        {   // y = a (x - p) ^ 2 + q
            this.QuadCoefficient = quadCoefficient;
            this.Vertex = vertex;

            var (a, b, c) = BuildGeneral(quadCoefficient, vertex);
            this.A = (decimal)quadCoefficient;
            this.B = b;
            this.C = c;

            this.Text = BuildText();
        }

        public EquationQuadratic(decimal a, decimal b, decimal c)
        {   // y = a x ^ 2 + b x + c
            this.QuadCoefficient = (float)a;
            this.Vertex = BuildQuad(a, b, c);
            this.A = a;
            this.B = b;
            this.C = c;
            this.Text = BuildText();
        }//constructor

        private (decimal a, decimal b, decimal c) BuildGeneral(float quadCoefficient, PointF vertex)
        {  // y = a (x - p) ^ 2 + q を展開して y = a x ^ 2 + b x + c 
            if(float.IsNaN(vertex.X) || float.IsNaN(vertex.Y))
            {
                throw new ArgumentException();
            }
            
            decimal a = (decimal)quadCoefficient;
            decimal b = -2M * a * (decimal)vertex.X;   // b = -2ap
            decimal c = a * (decimal)vertex.X * (decimal)vertex.X
                + (decimal)vertex.Y;               // c = a * p ^ 2 + q  

            return (a, b, c);
        }//BuildGeneral()

        private PointF BuildQuad(decimal a, decimal b, decimal c)
        {   // y = a x ^ 2 + b x + c から 平方完成 y = a (x - p) ^ 2 + q
            
            PointF pt = new PointF(float.NaN, float.NaN);
            
            if (a == 0)
            { // y = b x + c, y = c, x = c
                return pt;
            }

            float vertexX = (float)(-b / (2M * a));                        // p = -b / 2a
            float vertexY = (float)(-(b * b - 4M * a * c) / (4M * a)); // q = -(b ^ 2 - 4ac) / 4a

            return new PointF(vertexX, vertexY);
        }//BuildQuad()

        private string BuildText()
        {
            string text = null;
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

        public EquationLinear ToLinear()
        {
            return new EquationLinear((float)this.B, (float)this.C);
        }//ToLinear()
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