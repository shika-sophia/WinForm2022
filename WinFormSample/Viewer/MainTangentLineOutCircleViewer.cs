/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class MainTangentLineOutCircleViewer.cs
 *@class   └ new FormTangentLineOutCircleViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content TangentLineOutCircleViewer
 *         ２円の共有接線
 *         
 *@subject 
 *
 *@see ImageTangentLineOutCircleViewer.jpg
 *@see 
 *@author shika
 *@date 2022-10-14
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainTangentLineOutCircleViewer
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormTangentLineOutCircleViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormTangentLineOutCircleViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormTangentLineOutCircleViewer : Form
    {
        private readonly PictureBox pic;
        private readonly AlgoCoordinateCircle circle;

        public FormTangentLineOutCircleViewer()
        {
            this.Text = "FormTangentLineOutCircleViewer";
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
            //var eqCircle2 = new EquationCircle(radius: 60M, new PointF(150, 200));
      
            PointF ptOuter = new PointF(150, 150);

            circle.SetScaleRate(1.5M);
            EquationLinear[] tangentAry = circle.AlgoTangentLineOutCircle(
                ptOuter, eqCircle1, out PointF[] contactPointAry);

            circle.DrawMultiCircleFunction(scaleRateHere: 1.5M,
                new ICoordinateEquation[] { eqCircle1, tangentAry[0], tangentAry[1] },
                ptOuter, contactPointAry[0], contactPointAry[1]);

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}
