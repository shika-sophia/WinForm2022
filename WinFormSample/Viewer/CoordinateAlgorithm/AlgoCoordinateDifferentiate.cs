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
 *@content 微分, 接線, 垂直, 距離, 漸近線に関するアルゴリズム
 *         [英] differentiate  微分する
 *         [英] tangent        接する
 *         [英] virtical       垂直な
 *         
 *@subject 微分 DifferentiateQuad()
 *         [ y = a x ^ 2 + b x + c ] ->  [ y' = 2 a x + b ]
 *         float  AlgoDifferentiateQuad(float x, EquationQuadratic)
 *
 *@subject 接線 PointF(x, y)を通る接線
 *         EquationLinear  AlgoTangentLine(EquationQuadratic, PointF)
 *         ・接線を表す直線の傾き 2ax + b の xに代入
 *         ・接線 y = c x + d の 傾き cは上記で求まるので、
 *           PointF(x, y)を代入して dを決定
 *         ・【註】このメソッドの引数に ２次関数外の点を代入すると、
 *            この点を通る y = d の直線になる
 *           (２次関数外の点なので連立すると虚数解になる)
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

        public bool CheckOnLine(ICoordinateEquation eq, PointF pt)
        {
            float onY = AlgoFunctionXtoY(pt.X, eq);
            return pt.Y == onY; 
        }//CheckOnLine()

        public EquationLinear[] AlgoTangentLineFree(
            ICoordinateEquation eq, PointF pt)
        {
            if(eq is EquationLinear)
            {
                throw new ArgumentException("y = a x + b の接線は定義しない");
            }

            var eqQuad = (EquationQuadratic)eq;
            if(CheckOnLine(eqQuad, pt))
            {
                return new EquationLinear[]{
                    AlgoTangentLineOnContact(eqQuad, pt)};
            }

            // c = 2 a x + b  | y = c x + d
            // y = (2 a x + b) x + d
            // y = 2 a x ^ 2 + b x + d
            // d = y - 2 a x ^ 2 - b x | PointF (pt.X, pt.Y)
            float intercept = (float)((decimal)pt.Y 
                - 2M * eqQuad.A * (decimal)pt.X * (decimal)pt.X 
                - eqQuad.B * (decimal)pt.X);

            return new EquationLinear[] { };
        }//AlgoTangentLineFree()

        public EquationLinear AlgoTangentLineOnContact(ICoordinateEquation eq, PointF pt)
        {
            if (eq is EquationLinear)
            {   
                throw new ArgumentException("y = a x + b の接線は定義しない");
            }

            if (!CheckOnLine(eq, pt))
            {   
                throw new ArgumentException("このメソッドの引数は接点のみ");
            }

            var eqQuad = (EquationQuadratic)eq;
            float slope = AlgoDifferentiateQuad(pt.X, eqQuad);

            // 接線 y = c x + d に PointF(x, y)を代入  d = y - cx
            float intercept =
                (float)((decimal)pt.Y - (decimal)slope * (decimal)pt.X);

            DrawPointLine(pt, true);
            return new EquationLinear(slope, intercept);
        }//AlgoTangentLine()

        protected float AlgoDifferentiateQuad(float x, EquationQuadratic eqQuad)
        {   // [ y = a x ^ 2 + b x + c ] ->  [ y' = 2 a x + b ] に xを代入
            return (float)(2M * eqQuad.A * (decimal)x + eqQuad.B);
        }//AlgoDifferentiateQuad()
    }//class
}
