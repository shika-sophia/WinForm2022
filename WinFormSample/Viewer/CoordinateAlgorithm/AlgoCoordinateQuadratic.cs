/** 
 *@title WinFormGUI / WinFormSample / 
 *@class Main.cs
 *@class   └ new Form1() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content 
 *@subject 
 *
 *@see Image.jpg
 *@see 
 *@author shika
 *@date 
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

        public void DrawParabolaFunction(float quadCoefficiency, float vertexX, float vertexY) 
        {
            if(quadCoefficiency == 0)  // y = q
            {  
                throw new ArgumentException();
            }

            DrawPointLine(new PointF(vertexX, vertexY));

            List<PointF> pointList = new List<PointF>((int)pic.Width);

            // ２次関数 y = a(x - p)^2 + q  
            for (int i = (int)-centerPoint.X; i < (int)centerPoint.X; i++)
            {
                pointList.Add(
                    new PointF(
                        (float)((decimal)i + (decimal)vertexX),
                        (float)((decimal)-AlgoParabolaFunctionXtoY(
                            i, quadCoefficiency, vertexX, vertexY) - (decimal)vertexY)
                    )
                );
            }//for

            var gPath = new GraphicsPath();
            gPath.AddLines(pointList.ToArray());
            g.DrawPath(penPink, gPath);
        }//DrawParabolaFunction()

        private float AlgoParabolaFunctionXtoY(float x, float quadCoefficiency, float vertexX, float vertexY)
        {
            // ２次関数 y = a(x - p)^2 + q  
            return quadCoefficiency * (x - vertexX) * (x - vertexX) + vertexY;
        }
    }//class
}
