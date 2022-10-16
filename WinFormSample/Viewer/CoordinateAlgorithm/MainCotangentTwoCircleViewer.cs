/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainCotangentTwoCircleViewer.cs
 *@class   └ new FormCotangentTwoCircleViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content CotangentTwoCircleViewer
 *@subject ２円の共通接線の描画
 *
 *@see ImageCotangentTwoCircleViewer.jpg
 *@see 
 *@author shika
 *@date 2022-10-16
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainCotangentTwoCircleViewer
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormCotangentTwoCircleViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormCotangentTwoCircleViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormCotangentTwoCircleViewer : Form
    {
        private readonly PictureBox pic;
        private readonly AlgoCoordinateCircle circle;

        public FormCotangentTwoCircleViewer()
        {
            this.Text = "FormCotangentTwoCircleViewer";
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

            var eqCircle1 = new EquationCircle(radius: 100M, new PointF(0, 0));
            var eqCircle2 = new EquationCircle(radius: 80M, new PointF(150, 160));

            EquationLinear[] cotangentLineAry = circle.AlgoCotangentLineTwoCircle(
                eqCircle1, eqCircle2, 
                out PointF[] pointAry, 
                out AbsAlgoCoordinate.SegmentPair[] segmentPairAry,
                out EquationLinear[] virticalLineAry);

            List<ICoordinateEquation> eqList = new List<ICoordinateEquation>();
            eqList.Add(eqCircle1);
            eqList.Add(eqCircle2);
            eqList.AddRange(cotangentLineAry);

            circle.SetScaleRate(1.0M);
            circle.DrawMultiCircleFunction(
                eqList.ToArray(), virticalLineAry, 
                segmentPairAry, pointAry, isAutoScale: false);

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}
