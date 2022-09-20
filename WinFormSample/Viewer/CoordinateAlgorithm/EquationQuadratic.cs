using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class EquationQuadratic
    {
        public float QuadCoefficient { get; private set; }
        public PointF Vertex { get; private set; }
        public string Text { get; set; }

        public EquationQuadratic(float quadCoefficient, PointF vertex)
        {   // y = a (x - p) ^ 2 + q
            this.QuadCoefficient = quadCoefficient;
            this.Vertex = vertex;
            this.Text = BuildText();
        }

        public EquationQuadratic(float quadCoefficient, float vertexX, float vertexY)
           : this(quadCoefficient, new PointF(vertexX, vertexY)) { }

        public EquationQuadratic(decimal a, decimal b, decimal c)
        {   // y = a x ^ 2 + b x + c
            this.QuadCoefficient = (float)a;
            this.Vertex = BuildQuad(a, b, c);
        }//constructor

        private PointF BuildQuad(decimal a, decimal b, decimal c)
        {   // y = a x ^ 2 + b x + c から 平方完成
            if (a == 0) { throw new ArgumentException(); }

            float vertexX = (float)(-b / 2 * a); 
            float vertexY = (float)((b * b - 4 * a * c) / 4 * a);

            return new PointF(vertexX, vertexY);
        }//BuildQuad()

        private string BuildText()
        {
            string text = null;
            string quadCoefficientStr = QuadCoefficient == 1 ? "" : $"{QuadCoefficient}";
            string vertexXStr = Vertex.X > 0 ? $"+ {Vertex.X}" : $"- {-Vertex.X}";
            string vertexYStr = Vertex.Y > 0 ? $"+ {Vertex.Y}" : $"- {-Vertex.Y}";

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
    }//class
}
