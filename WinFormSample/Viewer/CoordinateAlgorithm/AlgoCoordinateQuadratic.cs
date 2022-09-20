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

            DrawMultiPointLine(new PointF[]
            {
                new PointF(vertexX, vertexY),
                new PointF(
                0, AlgoParabolaFunctionXtoY(0, quadCoefficient, vertexX, vertexY))
            });
            
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

            pointList.Clear();
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
            //var x = - p + √　q
            return new float[] { };
        }//AlgoParabolaFunctionYtoX()
    }//class
}
