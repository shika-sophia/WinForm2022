/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class MainCircleRadiusViewer.cs
 *@class   └ new FormCircleRadiusViewer() : Form
 *@class       └ new AlgoCoordinateCircle() : AlgoCoordinateDifferenciate
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content CircleRadiusViewer
 *@subject 
 *
 *@see ImageCircleRadiusViewer.jpg
 *@see 
 *@author shika
 *@date 
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainCircleRadiusViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormCircleRadiusViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormCircleRadiusViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormCircleRadiusViewer : Form
    {
        private readonly PictureBox pic;
        private readonly AlgoCoordinateCircle circle;

        public FormCircleRadiusViewer()
        {
            this.Text = "FormCircleRadiusViewer";
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

            decimal angle = 30;
            var eqCircle = new EquationCircle(100, 0, 0);
            PointF radiusPoint = circle.AlgoRadiusPoint(angle, eqCircle);
            //circle.SetScaleRate(1.0M);
            circle.DrawTriangleTheta(angle, eqCircle);
            circle.DrawCircleFunction(eqCircle);

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}
