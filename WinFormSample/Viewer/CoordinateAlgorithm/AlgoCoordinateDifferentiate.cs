/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class AlgoCoordinateDifferentiate : AlgoCoordinateQuadratic
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content 微分, 接線 に関するアルゴリズム
 *         [英] differentiate  微分する
 *         [英] tangent        接する
 *         [英] virtical       垂直な
 *         
 *@subject 微分 DifferentiateQuad()
 *         [ y = a x ^ 2 + b x + c ] ->  [ y' = 2 a x + b ]
 *         float  AlgoDifferentiateQuad(float x, EquationQuadratic)
 *
 *@subject 接線:  接点 PointF(x, y)を通る接線
 *         EquationLinear  AlgoTangentLine(EquationQuadratic, PointF)
 *         ・接線を表す直線の傾き 2ax + b の xに代入
 *         ・接線 y = c x + d の 傾き cは上記で求まるので、
 *           PointF(x, y)を代入して dを決定
 *         ・【註】このメソッドの引数に ２次関数外の点を代入すると、
 *            この点を通る y = d の直線になる
 *           (２次関数外の点なので連立すると虚数解になる)
 *
 *@subject 接線公式 Tangent Formula
 *         微分可能な y = f(x) 上の点 A(a, f(a)) における接線公式
 *             y - f(a) = f'(a) ( x - a )
 *             
 *         https://math-travel.com/a-tangent-line/
 *         =>〔~\Reference\Mathematics\Article_TangentLineLineQuad.txt〕
 *         
 *@subject 問 y = a x ^ 2 + b x + c 上の接点 (m, f(m))における接線
 *         ２次関数外の点 (p, q)を通る直線の式
 *         
 *         [Question] Culculate the Tangent Lines expression. and How much?
 *         Quadratic Equation: y = a x ^ 2 + b x + c
 *         tangent line on contact point (m, f(m))
 *         point (p, q) on tangent Linear Equation, but out of Quadratic Equation
 *         constant: a, b, c, p, q  定数
 *         unknown:  m              未知数
 *         variable: x, y           変数
 *         
 *         [Answer]
 *         Tangent Formula: y - f(m) = f'(m) ( x - m )  〔see above〕
 *         
 *         f'(m) = 2 a m + b              // differential
 *         y - f(m) = (2 a m + b)(x - m)  // <- substitute 代入 (x, y) = (p, q)
 *         q - (a m ^ 2 + b m + c) = (2 a m + b)(p - m)     // only m as unknown
 *            - 2 a m ^ 2 + 2 a p m - b m + b p 
 *            +   a m ^ 2           + b m + c - q = 0
 *            -----------------------------------------
 *              - a m ^ 2 + 2 a p m + c + b p - q = 0       // both side * (-1)
 *                a m ^ 2 - 2 a p m - c - b p + q = 0       // to Quadratic Formula
 *              
 *        judge expression  判別式
 *        D > 0:  solution m are two.
 *        D = 0:  solution m is one.
 *        D < 0:  solution m is none.
 *        
 *        m is contact point x-coordinate.                       m は接点の x座標
 *        ->　y-coordinate is calculated by Quadratic Equation.  ２次関数から y座標が求まる
 *        ->  m, f(m) lead to Linear Equation as tangent line.   接線公式に接点を代入し直線の式が求まる
 *          ([or] slope 2 a m + b, one point lead to Linear Equation.)   (別解: 傾き m, 一点から 直線の式が導ける)
 *           
 *        【NOTE】接線を y = d x + e と置くと未知数が多く解けなくなる
 *        
 *@NOTE 【考察】Test Print の計算誤差は なぜ起こるのか 〔下記〕
 *
 *@see MainTangentQuadViewer.cs
 *@author shika
 *@date 2022-09-26
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class AlgoCoordinateDifferentiate : AlgoCoordinateQuadratic
    {
        public AlgoCoordinateDifferentiate(PictureBox pic) : base(pic) { }

        public EquationLinear[] AlgoTangentLineQuadFree(
            ICoordinateEquation eq, PointF pt, out PointF[] contactPointAry)
        {
            if(eq is EquationLinear)
            {
                throw new ArgumentException("y = a x + b の接線は定義しない");
            }

            var eqQuad = (EquationQuadratic)eq;
            
            if(eqQuad.CheckOnLine(pt))
            {
                EquationLinear eqLinear = AlgoTangentLineQuadOnContact(eqQuad, pt, out PointF contactPoint);
                contactPointAry = new PointF[] { contactPoint };
                return new EquationLinear[] { eqLinear };
            }

            //a m ^ 2 - 2 a p m - c - b p + q = 0   〔see above〕
            var eqContactX = new EquationQuadratic(
                eqQuad.A,
                - 2M * eqQuad.A * (decimal)pt.X,
                - eqQuad.C - eqQuad.B * (decimal)pt.X + (decimal)pt.Y);

            //---- Quadratic Formula ----
            float[] contactXAry = eqContactX.AlgoQuadSolutionFormula();
            
            //---- EquationLinear as tangent line ----
            contactPointAry = new PointF[contactXAry.Length];
            EquationLinear[] tangentLineAry = new EquationLinear[contactXAry.Length];

            for(int i = 0; i < contactXAry.Length; i++)
            {
                float contactX = contactXAry[i];
                float contactY = eqQuad.AlgoFunctionXtoY(contactX)[0];
                float slope = AlgoDifferentiateQuad(contactX, eqQuad);
                PointF contactPoint = new PointF(contactX, contactY);

                contactPointAry[i] = contactPoint;
                
                //tangentLineAry[i] = new EquationLinear(slope, contactPoint); //【非推奨】y切片に誤差発生〔see below〕
                tangentLineAry[i] = new EquationLinear(slope, pt);
            }//for
            
            return tangentLineAry;
        }//AlgoTangentLineQuadFree()

        public EquationLinear AlgoTangentLineQuadOnContact(
            ICoordinateEquation eq, PointF pt, out PointF contactPoint)
        {
            if (eq is EquationLinear)
            {   
                throw new ArgumentException("y = a x + b の接線は定義しない");
            }

            if (!eq.CheckOnLine(pt))
            {   
                throw new ArgumentException("このメソッドの引数は接点のみ");
            }

            var eqQuad = (EquationQuadratic)eq;
            float slope = AlgoDifferentiateQuad(pt.X, eqQuad);

            // 接線 y = c x + d に PointF(x, y)を代入  d = y - cx
            float intercept =
                (float)((decimal)pt.Y - (decimal)slope * (decimal)pt.X);

            contactPoint = pt;
            return new EquationLinear(slope, intercept);
        }//AlgoTangentLine()

        protected float AlgoDifferentiateQuad(float x, EquationQuadratic eqQuad)
        {   // [ y = a x ^ 2 + b x + c ] ->  [ y' = 2 a x + b ] に xを代入
            return (float)(2M * eqQuad.A * (decimal)x + eqQuad.B);
        }//AlgoDifferentiateQuad()

        
    }//class
}

/*
//【考察】下記 Test Print の計算誤差は なぜ起こるのか
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

=> tangentLineAry[i] = new EquationLinear(slope, pt); にして、
   判別式 judgeのみ Math.Round(judge, 4)を行い、微細な差異を同一視すると解決
   (decimal -> float 全てに Math.Round()すると、更に不正確な値になるので、変更せず)

=> AlgoCoordinateLinear.CalcIntercept() を Math.Round(value, 4)すると、
   tangentLineAry[i] = new EquationLinear(slope, contactPoint); でも、y切片 -50になるが、
   この計算にだけ Math.Round()するのは、仕様統一性に問題があるし、チートぽくもある。
   その上、直線の交点で、２重交点になることがある。
   (例: EquationQuadratic(0.005f, new PointF(20, 30)) | PointF(20, -50)で二重交点)   

【結論】２次方程式の解の公式で出たルート計算の小数値の微小誤差が、
        数学的に同一の点を 異なる点として認識してしまう可能性がある。

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

//==== AlgoCoordinateDifferenciate.AlgoTangentLineQuadFree() ====
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