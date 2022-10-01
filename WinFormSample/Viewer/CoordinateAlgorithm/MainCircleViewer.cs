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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainCircleViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormCircleViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormCircleViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormCircleViewer : Form
    {
        private readonly PictureBox pic;
        private readonly AlgoCoordinateCircle circle;

        public FormCircleViewer()
        {
            this.Text = "FormCircleViewer";
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
            //circle.DrawMultiPointLine(new PointF[] { new PointF(0, 0) });
            
            var eqCircle = new EquationCircle(100, 0, 0);
            var eqCircle2 = new EquationCircle(100, 50, 50);

            Console.WriteLine(eqCircle);
            Console.WriteLine(eqCircle.Text);
            circle.DrawCircleFunction(eqCircle);
            circle.DrawCircleFunction(eqCircle2);

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}
