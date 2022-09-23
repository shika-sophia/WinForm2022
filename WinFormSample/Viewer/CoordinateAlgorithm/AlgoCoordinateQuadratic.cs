/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class AlgoCoordinateQuadratic.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content AlgoCoordinateQuadratic
 *@subject 
 *
 *
 *         
 *@see AlgoCoordinateAxis.cs
 *@see AlgoCoordinateLinear.cs
 *@author shika
 *@date 2022-09-20
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class AlgoCoordinateQuadratic : AlgoCoordinateLinear
    {
        public AlgoCoordinateQuadratic(PictureBox pic) : base(pic) { }

        public void DrawParabolaFunction(EquationQuadratic quadEqu)
        {
            DrawParabolaFunction(quadEqu.QuadCoefficient, quadEqu.Vertex);
        }

        public void DrawParabolaFunction(float quadCoefficient, PointF vertex)
        {
            DrawParabolaFunction(quadCoefficient, vertex.X, vertex.Y);
        }//DrawParabolaFunction(float, PointF)

        public void DrawParabolaFunction(float quadCoefficient, float vertexX, float vertexY) 
        {
            if(quadCoefficient == 0)  // y = q
            {  
                throw new ArgumentException();
            }

            List<PointF> pointList = new List<PointF>(
                (int)((decimal)pic.ClientSize.Width / scaleRate));

            // ２次関数 y = a(x - p)^2 + q  【註】p, qの平行移動は AlgoParabolaFunctionXtoY()内で済み
            for (decimal i = (decimal)-centerPoint.X / scaleRate;
                i < (decimal)centerPoint.X / scaleRate; i++)
            {
                pointList.Add(
                    new PointF(
                        (float)((decimal)i * scaleRate),
                        (float)(((decimal)-AlgoParabolaFunctionXtoY(
                            (float)i, quadCoefficient, vertexX, vertexY)
                        * scaleRate))
                    )
                );
            }//for
            
            var gPath = new GraphicsPath();
            gPath.AddLines(pointList.ToArray());
            g.DrawPath(penPink, gPath);
        }//DrawParabolaFunction(float, float, float)

        private float AlgoParabolaFunctionXtoY(float x, EquationQuadratic quadEqu)
        {
            return AlgoParabolaFunctionXtoY(
                x, quadEqu.QuadCoefficient, quadEqu.Vertex.X, quadEqu.Vertex.Y);
        }

        private float AlgoParabolaFunctionXtoY(
            float x, float quadCoefficient, float vertexX, float vertexY)
        {
            // ２次関数 y = a(x - p)^2 + q  
            return (float)((decimal)quadCoefficient
                * ((decimal)x - (decimal)vertexX)
                * ((decimal)x - (decimal)vertexX)
                + (decimal)vertexY);
        }

        private float[] AlgoParabolaFunctionYtoX(float y, EquationQuadratic equQuad)
        {
            return AlgoParabolaFunctionYtoX(
                y, equQuad.QuadCoefficient, equQuad.Vertex.X, equQuad.Vertex.Y);
        }

        private float[] AlgoParabolaFunctionYtoX(
            float y, float quadCoefficient, float vertexX, float vertexY)
        {
            // y = a x ^ 2 + b x + c
            // quadCoefficent = a
            // vertexX = - b / 2a 
            // vertexY = - (b ^ 2 - 4ac) / 4 a
            // x = vertexX ± √ (vertexY - y)

            if (quadCoefficient == 0)
            {
                throw new ArgumentException();
            }

            List<float> solutionList = new List<float>(2);

            if (vertexY - y > 0)  // quad equation formula
            {
                solutionList.Add((float)((decimal)vertexX
                    + (decimal)Math.Sqrt((double)((decimal)vertexY - (decimal)y))));
                solutionList.Add((float)((decimal)vertexX
                    - (decimal)Math.Sqrt((double)((decimal)vertexY - (decimal)y))));
            }

            if (vertexY - y == 0)  // x = - b / 2a
            {
                solutionList.Add(vertexX);
            }

            if (vertexY - y < 0)  // x = (No solution)  虚数解
            {
                solutionList.Add(float.NaN);
            }

            return solutionList.ToArray();
        }//AlgoParabolaFunctionYtoX()

        public bool TrySolutionQuad(
            EquationQuadratic eq1, EquationQuadratic eq2, out PointF[] solutionAry)
        {   
            decimal subA = eq1.A - eq2.A;
            decimal subB = eq1.B - eq2.B;
            decimal subC = eq1.C - eq2.C;

            if(subA == 0 && subB == 0)  // case: parallel
            {
                solutionAry = new PointF[0];
                return false;
            }

            if (subA == 0)  // case: a = 0, bx + c = 0  解はあっても、0除算のため解の公式を利用できない
            {
                float solutionX = (float)(-subC / subB);
                float solutionY = AlgoParabolaFunctionXtoY(solutionX, eq1);
                solutionAry = new PointF[] { new PointF(solutionX, solutionY) };
                return true;
            }

            solutionAry = AlgoQuadSolutionFormula(subA, subB, subC);

            if(solutionAry.Length == 0) { return false; }

            return true;
        }//TrySolutionQuad()

        private PointF[] AlgoQuadSolutionFormula(decimal a, decimal b, decimal c)
        {   // ２次方程式の解の公式
            int solutionNum = AlgoJudge(a, b, c, out decimal judge);
            PointF[] solutionAry = new PointF[solutionNum];
            
            if(judge > 0)
            {
                float solutionX0 = 
                    (float)((-b + (decimal)Math.Sqrt((double)judge)) / (2M * a));
                float solutionX1 = 
                    (float)((-b - (decimal)Math.Sqrt((double)judge)) / (2M * a));
                float solutionY0 = AlgoParabolaFunctionXtoY(
                    solutionX0, (float)a, (float)b, (float)c);
                float solutionY1 = AlgoParabolaFunctionXtoY(
                    solutionX1, (float)a, (float)b, (float)c);

                solutionAry[0] = new PointF(solutionX0, solutionY0);
                solutionAry[1] = new PointF(solutionX1, solutionY1);
            }
            else if(judge == 0)
            {
                float solutionX0 = (float)(-b / (2M * a));
                float solutionY0 = AlgoParabolaFunctionXtoY(
                    solutionX0, (float)a, (float)b, (float)c);
                solutionAry[0] = new PointF(solutionX0, solutionY0);
            }
            
            return solutionAry;
        }//AlgoQuadSolutionFormula()

        private int AlgoJudge(
            decimal a, decimal b, decimal c, out decimal judge)
        {   // 判別式 D = b ^ 2 - 4 a c
            judge = b * b - 4M * a * c;

            int solutionNum = 0;
            if (judge > 0) { solutionNum = 2; }
            if (judge == 0) { solutionNum = 1; }
            if (judge < 0) { solutionNum = 0; }

            return solutionNum;
        }//AlgoJudge()
    }//class
}
