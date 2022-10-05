/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class AlgoCoordinateQuadratic.cs : AlgoCoordinateLinear
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content AlgoCoordinateQuadratic
 *         ２次関数の計算と描画のアルゴリズムをまとめるクラス
 *         
 *         [英] parabola            放物線, ２次関数
 *         [英] quad                本来は「４」の意味。四角形の面積から慣例的に「２次方程式」を表す
 *         [英] quadratic           平方の, ２乗の
 *         [英] quadratic equation  ２次方程式
 *         [英] vertex              頂点
 *         [英] coefficient         係数
 *         [英] complete the squre  平方完成
 *
 *@subject ２次関数(放物線)の描画
 *         void  DrawParabolaFunction(EquationQuadratic eqQuad)
 *         void  DrawParabolaFunction(float quadCoefficient, float vertexX, float vertexY) 
 *         引数: EquationQuadraticクラス
 *         引数: float quadCoefficient  y = a ( x - p ) ^ 2 + q の　a
 *               float vertexX, float vertexY: 頂点の座標
 *
 *@subject ２個の２次方程式どうしの連立解
 *         ・a1 x1 ^ 2 + b1 x1 + c1 = a2 x2 ^ 2 + b2 x2 + c2
 *              -> subA x ^ 2 + subB x + subC = 0 を満たす x
 *         ・判別式 AlgoJudge()により、解は 2個, 1個, 0個
 *         ・解の公式を利用できない場合を条件分岐
 *         ・連立解を解の公式で求める
 *        
 *         bool  TrySolutionQuad(
 *                 EquationQuadratic eq1, EquationQuadratic eq2, out PointF[] solutionAry)
 *
 *@see AlgoCoordinateAxis.cs
 *@see AlgoCoordinateLinear.cs
 *@see EquationQuadratic.cs
 *@author shika
 *@date 2022-09-20
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class AlgoCoordinateQuadratic : AlgoCoordinateLinear
    {
        public AlgoCoordinateQuadratic(PictureBox pic) : base(pic) { }

        public void DrawMultiQuadraticFunction(
            ICoordinateEquation[] eqAry, params PointF[] pointAryArgs)
        {
            //---- Test Print ----
            Console.WriteLine("Equation Array:");
            foreach (var eq in eqAry) { Console.WriteLine(eq); };
            Console.WriteLine("\nArgument pointAry:");
            foreach(var pt in pointAryArgs) { Console.Write($"({pt.X},{pt.Y}), "); }
            Console.WriteLine("\n");

            //---- pointList ----
            List<PointF> pointList = new List<PointF>(pointAryArgs);
            List<EquationLinear> virticalLineList = new List<EquationLinear>();

            for (int i = 0; i < eqAry.Length; i++)
            {
                ICoordinateEquation eq = eqAry[i];
                pointList.AddRange(eq.GetEqPointAry());

                //---- TrySolutionQuad() ----
                for (int j = i; j < eqAry.Length; j++)
                {
                    //case same
                    if (j == i) { continue; }

                    bool existSolution = TrySolutionQuad(
                        eqAry[i], eqAry[j], out PointF[] solutionAry);

                    if (existSolution)
                    {
                        pointList.AddRange(solutionAry);
                    }

                    //case virtical
                    if (eqAry[i] is EquationLinear && eqAry[j] is EquationLinear
                        && IsVirtical(eqAry[i] as EquationLinear, eqAry[j] as EquationLinear))
                    {
                        virticalLineList.Add(eqAry[i] as EquationLinear);
                        virticalLineList.Add(eqAry[j] as EquationLinear);
                        Console.WriteLine($"\nIsVirticle = True");
                    }
                
                    //---- Test Print TrySolution() ----
                    Console.WriteLine($"\nexistSolution = {existSolution}");
                    foreach (var solution in solutionAry) { Console.Write($"({solution.X},{solution.Y}), "); }
                }//for j
            }//for i

            //---- Test Print before Distinct() ----
            Console.WriteLine("\n\npointList before Distinct():");
            pointList.ForEach(pt => { Console.Write($"({pt.X},{pt.Y}), "); });

            //---- Remove overlapped point and NaN ----
            PointF[] pointAry = pointList.Select(pt => pt)
                .Where(pt => !float.IsNaN(pt.X) || !float.IsNaN(pt.Y))
                .Distinct()
                .ToArray();

            //---- Test Print after Distinct() ----
            Console.WriteLine("\n\npointAry after Distinct():");
            foreach (PointF pt in pointAry) { Console.Write($"({pt.X},{pt.Y}), "); }
            Console.WriteLine("\n");

            //---- Draw ----
            DrawMultiPointLine(pointAry);
            Console.WriteLine($"scaleRate = {scaleRate}");
            Console.WriteLine($"height / width = {ratioWidthHeight:0.##}");

            foreach (ICoordinateEquation eq in eqAry)
            {
                if(eq is EquationQuadratic)
                {
                    Console.WriteLine($"DrawParabolaFunction({eq})");
                    DrawParabolaFunction(eq as EquationQuadratic);
                }
                else if (eq is EquationLinear)
                {
                    Console.WriteLine($"DrawLinearFunction({eq})");
                    DrawLinearFunction(eq as EquationLinear);
                }

                if(virticalLineList.Count > 0)
                {
                    DrawVirticalMark(virticalLineList.ToArray());
                }
            }//foreach
        }//MultiDrawQuadraticFunction()

        public void DrawParabolaFunction(float quadCoefficient, PointF vertex)
        {
            DrawParabolaFunction(new EquationQuadratic(quadCoefficient, vertex));
        }//DrawParabolaFunction(float, PointF)

        public void DrawParabolaFunction(float quadCoefficient, float vertexX, float vertexY)
        {
            DrawParabolaFunction(new EquationQuadratic(quadCoefficient, new PointF(vertexX, vertexY)));
        }//DrawParabolaFunction(float, float, float)
        public void DrawParabolaFunction(EquationQuadratic eqQuad)
        {
            float quadCoefficient = eqQuad.QuadCoefficient;
            float vertexX = eqQuad.Vertex.X;
            float vertexY = eqQuad.Vertex.Y;

            if (quadCoefficient == 0)  // y = q
            {
                throw new ArgumentException();
            }

            List<PointF> drawPointList = new List<PointF>(
                (int)((decimal)pic.ClientSize.Width / scaleRate));

            // ２次関数 y = a(x - p)^2 + q  【註】p, qの平行移動は AlgoParabolaFunctionXtoY()内で済み
            for (decimal i = (decimal)-centerPoint.X / scaleRate;
                i < (decimal)centerPoint.X / scaleRate; i++)
            {
                float ptY = (float)((decimal)-eqQuad.AlgoFunctionXtoY((float)i)[0] * scaleRate);

                if (Math.Abs((decimal)ptY) > Math.Abs((decimal)centerPoint.Y))
                { continue; }

                drawPointList.Add(
                    new PointF((float)((decimal)i * scaleRate), ptY));
            }//for

            DrawPointLine(new PointF(vertexX, vertexY));

            var gPath = new GraphicsPath();
            gPath.AddLines(drawPointList.ToArray());
            g.DrawPath(penPink, gPath);

            SizeF textSize = g.MeasureString(eqQuad.Text, font);
            PointF textLoction = new PointF(0, 0);
            textLoction.X = (float)(quadCoefficient > 0.05f ?
                (decimal)drawPointList[0].X - (decimal)textSize.Width - 10M :
                (decimal)drawPointList[0].X + 10M);
            textLoction.Y = quadCoefficient > 0 ?
                drawPointList[0].Y : drawPointList[0].Y - textSize.Height;

            g.DrawString(eqQuad.Text, font, penPink.Brush, textLoction);
        }//DrawParabolaFunction(EquationQuadratic)
        
        public bool TrySolutionQuad(
            ICoordinateEquation eq1, ICoordinateEquation eq2, out PointF[] solutionAry)
        {   
            //---- GetGeneralParam() ---- by Polymorphism
            (decimal eq1A, decimal eq1B, decimal eq1C) = eq1.GetGeneralParam();
            (decimal eq2A, decimal eq2B, decimal eq2C) = eq2.GetGeneralParam();

            //---- solution with x = c  ----
            if(eq1B == Decimal.MaxValue && eq2B == Decimal.MaxValue)  // x = c && x = d
            {
                solutionAry = new PointF[0];
                return false;
            }
            else if (eq1B == Decimal.MaxValue)
            {
                float solutionX = (float)eq1C;
                float solutionY = eq2.AlgoFunctionXtoY(solutionX)[0];
                solutionAry = new PointF[] { new PointF(solutionX, solutionY) };
                return true;
            }
            else if (eq2B == Decimal.MaxValue)
            {
                float solutionX = (float)eq2C;
                float solutionY = eq1.AlgoFunctionXtoY(solutionX)[0];
                solutionAry = new PointF[] { new PointF(solutionX, solutionY) };
                return true;
            }

            //---- Quaratic Simultaneous Equations / Solution Formula ---- ２次連立方程式, 解の公式
            decimal subA = eq1A - eq2A;
            decimal subB = eq1B - eq2B;
            decimal subC = eq1C - eq2C;
            var eqSimulaneousQuad = new EquationQuadratic(subA, subB, subC);

            if(subA == 0 && subB == 0)  // case: parallel
            {
                solutionAry = new PointF[0];
                return false;
            }
            else if (subA == 0)  // case: a = 0, bx + c = 0  解はあっても、0除算のため解の公式を利用できない
            {
                float solutionX = (float)(-subC / subB);
                float solutionY = eq1.AlgoFunctionXtoY(solutionX)[0];
                solutionAry = new PointF[] { new PointF(solutionX, solutionY) };
                return true;
            }

            float[] xAry = eqSimulaneousQuad.AlgoQuadSolutionFormula();
            solutionAry = new PointF[xAry.Length];
            for(int i = 0; i < xAry.Length; i++)
            {
                solutionAry[i] = new PointF(
                    xAry[i], eq1.AlgoFunctionXtoY(xAry[i])[0]);
            }//for
            
            if (solutionAry.Length == 0) { return false; }

            return true;
        }//TrySolutionQuad()
    }//class
}

