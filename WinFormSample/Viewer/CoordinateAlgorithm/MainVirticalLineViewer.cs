/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainVirticalLineViewer.cs
 *@class   └ new FormVirticalLineViewer() : Form
 *@class       └ new AlgoCoordinateDifferentiate() : AlgoCoordinateQuadratic
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content VirticalLineViewer
 *         垂直線と距離の描画
 *         
 *@subject 
 *
 *@see ImageVirticalLineViewer.jpg
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
    class MainVirticalLineViewer
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormVirticalLineViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormVirticalLineViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormVirticalLineViewer : Form
    {
        private readonly PictureBox pic;
        private readonly AlgoCoordinateLinear linear;

        public FormVirticalLineViewer()
        {
            this.Text = "FormVirticalLineViewer";
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

            linear = new AlgoCoordinateLinear(pic);
            linear.DrawCoordinateAxis();

            var originLine = new EquationLinear(2f, -50f);
            var pt = new PointF(100f, 50f);
            var virticalLine = linear.AlgoVirticalLine(originLine, pt);
            
            linear.DrawMultiLinearFunciton(
                new EquationLinear[] { originLine, virticalLine }, pt);

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}
