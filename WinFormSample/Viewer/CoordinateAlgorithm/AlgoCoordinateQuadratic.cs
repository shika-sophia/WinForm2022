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
            //---- pointList ----
            List<PointF> pointList = new List<PointF>(pointAryArgs);
            for (int i = 0; i < eqAry.Length; i++)                
            {
                ICoordinateEquation eq = eqAry[i];
                pointList.Add(AlgoInterceptY(eq));
                pointList.AddRange(AlgoInterceptX(eq));
                
                if(eq is EquationQuadratic)
                {
                    var eqQuad = (EquationQuadratic)eq;
                    pointList.Add(eqQuad.Vertex);
                }
                
                //---- TrySolutionQuad() ----
                for(int j = i; j < eqAry.Length; j++)
                {
                    if(j == i) { continue; }

                    bool existSolution = TrySolutionQuad(
                        eqAry[i], eqAry[j], out PointF[] solutionAry);

                    if (existSolution)
                    {
                        pointList.AddRange(solutionAry);
                    }
                }//for j
            }//for i

            //Console.WriteLine("\n\npointList:");
            //pointList.ForEach(pt => { Console.Write($"({pt.X},{pt.Y}), "); });

            //---- Remove at overlapped point as object ----
            PointF[] pointAry = pointList.Select(pt => pt)
                .Distinct()
                .ToArray();

            //Console.WriteLine("\n\npointAry:");
            //foreach (PointF pt in pointAry) { Console.Write($"({pt.X},{pt.Y}), "); }

            //---- Draw ----
            DrawMultiPointLine(pointAry);

            foreach (ICoordinateEquation eq in eqAry)
            {
                if(eq is EquationQuadratic)
                {
                    DrawParabolaFunction(eq as EquationQuadratic);
                }
                else if (eq is EquationLinear)
                {
                    DrawLinearFunction(eq as EquationLinear);
                }
            }//foreach
        }//MultiDrawQuadraticFunction()

        public void DrawParabolaFunction(EquationQuadratic eqQuad)
        {
            DrawParabolaFunction(eqQuad.QuadCoefficient, eqQuad.Vertex);
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
                        (float)((decimal)-AlgoParabolaFunctionXtoY(
                            (float)i, quadCoefficient, vertexX, vertexY)
                        * scaleRate)
                    )
                );
            }//for
            
            var gPath = new GraphicsPath();
            gPath.AddLines(pointList.ToArray());
            g.DrawPath(penPink, gPath);
            DrawPointLine(new PointF(vertexX, vertexY));
        }//DrawParabolaFunction(float, float, float)

        public float AlgoFunctionXtoY(float x, ICoordinateEquation eq)
        {
            if (eq is EquationLinear)
            {
                return base.AlgoFunctionXtoY(x, eq as EquationLinear);
            }

            var eqQuad = (EquationQuadratic)eq;
            return AlgoParabolaFunctionXtoY(x, eqQuad.A, eqQuad.B, eqQuad.C);
        }

        public float[] AlgoFunctionYtoX(float y, ICoordinateEquation eq)
        {
            if (eq is EquationLinear)
            {
                return new float[] { base.AlgoFunctionYtoX(y, eq as EquationLinear) };
            }

            var eqQuad = (EquationQuadratic)eq;
            return AlgoQuadSolutionFormula(eqQuad.A, eqQuad.B, eqQuad.C - (decimal)y);
        }

        private float AlgoParabolaFunctionXtoY(float x, decimal a, decimal b, decimal c)
        {   // y = a x ^ 2 + b x + c
            return (float)(a * (decimal)x * (decimal)x + b * (decimal)x + c);
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

        public PointF AlgoInterceptY(ICoordinateEquation eq)
        {
            if (eq is EquationLinear)
            {
                return base.AlgoInterceptY(eq as EquationLinear);
            }

            var eqQuad = (EquationQuadratic)eq;
            return new PointF(
                0, AlgoFunctionXtoY(x: 0f, eqQuad));
        }//AlgoInterceptY()

        public PointF[] AlgoInterceptX(ICoordinateEquation eq)
        {
            if (eq is EquationLinear)
            {
                return new PointF[] { base.AlgoInterceptX(eq as EquationLinear) };
            }

            var eqQuad = (EquationQuadratic)eq;
            List<PointF> pointList = new List<PointF>();
            
            float[] interceptXAry = AlgoFunctionYtoX(y: 0f, eqQuad);            
            foreach (float x in interceptXAry)
            {
                pointList.Add(new PointF(x, 0));
            }

            return pointList.ToArray();
        }//AlgoInterceptX()
        
        public bool TrySolutionQuad(
            ICoordinateEquation eq1, ICoordinateEquation eq2, out PointF[] solutionAry)
        {   
            (decimal eq1A, decimal eq1B, decimal eq1C) = eq1.GetGeneralParam();
            (decimal eq2A, decimal eq2B, decimal eq2C) = eq2.GetGeneralParam();

            decimal subA = eq1A - eq2A;
            decimal subB = eq1B - eq2B;
            decimal subC = eq1C - eq2C;
            
            if(subA == 0 && subB == 0)  // case: parallel
            {
                solutionAry = new PointF[0];
                return false;
            }
            else if (subA == 0)  // case: a = 0, bx + c = 0  解はあっても、0除算のため解の公式を利用できない
            {
                float solutionX = (float)(-subC / subB);
                float solutionY = AlgoFunctionXtoY(solutionX, eq1);
                solutionAry = new PointF[] { new PointF(solutionX, solutionY) };
                return true;
            }

            float[] xAry = AlgoQuadSolutionFormula(subA, subB, subC);
            solutionAry = new PointF[xAry.Length];
            for(int i = 0; i < xAry.Length; i++)
            {
                solutionAry[i] = new PointF(
                    xAry[i], AlgoFunctionXtoY(xAry[i], eq1));
            }//for
            
            if (solutionAry.Length == 0) { return false; }

            return true;
        }//TrySolutionQuad()

        protected float[] AlgoQuadSolutionFormula(EquationQuadratic eqQuad)
        {
            return AlgoQuadSolutionFormula(eqQuad.A, eqQuad.B, eqQuad.C);
        }

        protected float[] AlgoQuadSolutionFormula(decimal a, decimal b, decimal c)
        {   // ２次方程式の解の公式
            if(a == 0)
            {
                throw new ArgumentException();
            }
            
            int solutionNum = AlgoJudge(a, b, c, out decimal judge);
            float[] solutionXAry = new float[solutionNum];
            
            if(judge > 0)
            {
                solutionXAry[0] = 
                    (float)((-b + (decimal)Math.Sqrt((double)judge)) / (2M * a));
                solutionXAry[1] = 
                    (float)((-b - (decimal)Math.Sqrt((double)judge)) / (2M * a));
            }
            else if(judge == 0)
            {
                solutionXAry[0] = (float)(-b / (2M * a));
            }
            
            return solutionXAry;
        }//AlgoQuadSolutionFormula()

        protected int AlgoJudge(EquationQuadratic eqQuad, out decimal judge)
        {
            return AlgoJudge(eqQuad.A, eqQuad.B, eqQuad.C, out  judge);
        }

        protected int AlgoJudge(
            decimal a, decimal b, decimal c, out decimal judge)
        {   // 判別式 D = b ^ 2 - 4 a c
            judge = Math.Round(b * b - 4M * a * c, 4);

            int solutionNum = 0;
            if (judge > 0) { solutionNum = 2; }
            if (judge == 0) { solutionNum = 1; }
            if (judge < 0) { solutionNum = 0; }

            return solutionNum;
        }//AlgoJudge()
        
    }//class
}

/*
//【考察】下記 Teat Print の計算誤差は なぜ起こるのか
３種類の２次方程式の解法による計算誤差だろうか
y = a x ^ 2 + b x + c  ..①
y = a (x - p) ^ 2 + q  ..②
y = f'(a) (x - a) + f'(a)  ..③
① = ② は AlgoParabolaFunctionXtoY()で確認

数学的には どれも同じ結果になるはずだが、
接線の y切片 -50 と -49.99998 が 他の値に影響を及ぼしている。
判別式 で 重解 (D = 0)となるはずが、-50 -> 虚数解 (D < 0) | -49.99998 -> ２解 (D > 0)となる。

decimalで計算しているので、float, doubleの２進数計算の誤差とは考えにくい。
decimal -> float 型変換時の Math.Round() 丸め(=四捨五入)の誤差と思われる。

=> 判別式 judgeのみ Math.Round(judge, 4)を行い、微細な差異を同一視すると解決
   (decimal -> float 全てに Math.Round()すると、更に不正確な値になるので、変更せず)

//#### Test Print ####
//==== FormTangentQuadViewer ====
eqList.ForEach(eq => { Console.WriteLine(eq); } );

Console.WriteLine("Main pointList:");
pointList.ForEach(pt => { Console.Write($"({pt.X},{pt.Y}), "); });

Console.WriteLine("\n\ncontactAry:");
foreach (PointF pt in contactAry) { Console.Write($"({pt.X},{pt.Y}), "); }

//==== DrawMultiQuadraticFunction() ====
//---- Remove at overlapped point as object ----
Console.WriteLine("pointList:");
pointList.ForEach(pt => { Console.Write($"({pt.X},{pt.Y}), "); });

PointF[] pointAry = pointList.Select(pt => pt)
    .Distinct()
    .ToArray();

Console.WriteLine("\n\npointAry:");
foreach (PointF pt in pointAry) { Console.Write($"({pt.X},{pt.Y}), "); }

//#### Resault Print ####
y = 0.005 x ^ 2 + 30         // xの１次係数 eqQuad.B == 0 になっているのは b = -2ap なのでＯＫ
y = 1.264911 x - 49.99998    // b == 0 だと slopeが 接点の x座標になる
y = -1.264911 x - 49.99998   // 

Main pointList:
(0,-50), (126.4911,110), (-126.4911,110),

pointList:
(0,-50), (126.4911,110), (-126.4911,110), (0,30), (0,30),   // (0, 30)の重複は y切片と ２次関数の頂点なのでＯＫ
(126.5399,110.0617), (126.4423,109.9383),          // ２次関数と接線の交点が重解ではなく、２点になっている
(-126.4423,109.9383), (-126.5399,110.0617), 
(0,-49.99998), (39.52846,0),                       // y切片, x切片
(0,-49.99998),                                     // ２直線の交点 = 所与の値(0, -50)に計算誤差
(0,-49.99998),(-39.52846,0),                       // y切片, x切片

pointAry:
(0,-50), (126.4911,110), (-126.4911,110), (0,30),  // (0, 30)の重複は 別オブジェクトでも (x, y)の値が同じだと、Distinct()で重複解消される
(126.5399,110.0617), (126.4423,109.9383),          
(-126.4423,109.9383), (-126.5399,110.0617),
(0,-49.99998), (39.52846,0), 
(0,-49.99998),                                     // ここの(0,-49.99998)は重複解消しているが、上の点とは重複
(-39.52846,0),

//==== AlgoCoordinateDifferenciate.AlgoTangentLineFree() ====
tangentLineAry[i] = 
    new EquationLinear(slope, contactPoint) ->  new EquationLinear(slope, pt);

y = 0.005 x ^ 2 + 30
y = 1.264911 x - 50
y = -1.264911 x - 50

Main pointList:
(0,-50), (126.4911,110), (-126.4911,110),

pointList:
(0,-50), (126.4911,110), (-126.4911,110), 
(0,30), (0,30), 
                         // ２次関数と接線の交点が 虚数解となり算出されていない
(0,-50), (39.52847,0), 
(0,-50),
(0,-50), (-39.52847,0),

pointAry:
(0,-50), (126.4911,110), (-126.4911,110),
(0,30), (39.52847,0), (-39.52847,0), 

//
y = 0.005 x ^ 2 + 30
y = 1.264911 x - 50
y = -1.264911 x - 50
Main pointList:
(0,-50), (126.4911,110), (-126.4911,110),

pointList:
(0,-50), (126.4911,110), (-126.4911,110), 
(0,30), (0,30),
(126.4911,110), (-126.4911,110),          // ２次関数と接線の交点も算出
(0,-50), (39.52847,0), 
(0,-50), 
(0,-50), (-39.52847,0),

pointAry:
(0,-50), (126.4911,110), (-126.4911,110),
(0,30), 
(126.4911,110), (-126.4911,110), 
(39.52847,0), (-39.52847,0), 
 
 */