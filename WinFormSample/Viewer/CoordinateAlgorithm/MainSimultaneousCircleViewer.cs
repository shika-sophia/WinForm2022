﻿/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class MainSimultaneousCircleViewer.cs
 *@class   └ new FormSimultaneousCircleViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content SimultaneousCircleViewer
 *@subject 
 *
 *@see ImageSimultaneousCircleViewer.jpg
 *@see 
 *@author shika
 *@date 2022-10-08
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainSimultaneousCircleViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormSimultaneousCircleViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormSimultaneousCircleViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormSimultaneousCircleViewer : Form
    {
        private readonly PictureBox pic;
        private readonly AlgoCoordinateCircle circle;

        public FormSimultaneousCircleViewer()
        {
            this.Text = "FormSimultaneousCircleViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;

            pic = new PictureBox()
            {
                ClientSize = this.ClientSize,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            circle = new AlgoCoordinateCircle(pic);
            circle.DrawCoordinateAxis();

            var eqCircle1 = new EquationCircle(radius: 120M, new PointF(0, 0));
            var eqCircle2 = new EquationCircle(radius: 80M, new PointF(100, 50));    // | r1 - r2 | < d < r1 + r2
            //var eqCircle2 = new EquationCircle(radius: 80M, new PointF(160, 120)); // d == r1 + r2
            //var eqCircle2 = new EquationCircle(radius: 80M, new PointF(-24, -36)); // d == | r1 - r2 |

            //var eqLinear = new EquationLinear(eqCircle1.CircleCenterPoint, eqCircle2.CircleCenterPoint);
            //PointF circumscribePoint = circle.AlgoDistanceOnLinePoint(eqCircle1.Radius + eqCircle2.Radius,
            //    eqCircle1.CircleCenterPoint, eqLinear);       // d == r1 + r2 となる PointF
            //PointF inscribePoint = circle.AlgoDistanceOnLinePoint(eqCircle1.Radius - eqCircle2.Radius,
            //    eqCircle1.CircleCenterPoint, eqLinear);       // d == | r1 - r2  | となる PointF
            //Console.WriteLine($"circumscribePoint: ({circumscribePoint.X}f,{circumscribePoint.Y}f)");
            //Console.WriteLine($"inscribePoint: ({inscribePoint.X}f,{inscribePoint.Y}f)");

            circle.DrawMultiCircleFunction(
                new ICoordinateEquation[] { eqCircle1, eqCircle2, /*eqLinear*/ },
                virticalLineAryArgs: new EquationLinear[0],
                segmentPairAryArgs: new AbsAlgoCoordinate.SegmentPair[0],
                pointAryArgs: new PointF[0]
            );
            
            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}
