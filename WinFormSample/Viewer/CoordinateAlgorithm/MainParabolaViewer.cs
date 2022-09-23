/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class MainParabolaViewer.cs
 *@class   └ new FormParabolaViewer() : Form
 *@class       └ new AlgoCoordinateQuadratic() : AlgoCoordinateLinear
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content ParabolaViewer
 *         ２次関数の放物線を描画
 *         
 *@subject DrawParabolaFunction()
 *         =>〔AlgoCoordinateQuadratic.cs〕
 *        
 *@see ImageParabolaViewer.jpg
 *@see AlgoCoordinateQuadratic.cs
 *@author shika
 *@date 2022-09-20
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainParabolaViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormParabolaViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormParabolaViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormParabolaViewer : Form
    {
        private readonly PictureBox pic;
        private readonly AlgoCoordinateQuadratic quad;

        public FormParabolaViewer()
        {
            this.Text = "FormParabolaViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(720, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;

            pic = new PictureBox()
            {
                ClientSize = this.ClientSize,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };

            quad = new AlgoCoordinateQuadratic(pic);
            quad.DrawCoordinateAxis();

            var y1 = new EquationQuadratic(-0.005f, new PointF(-100, 200));
            var y2 = new EquationQuadratic(0.005f, new PointF(100, -500));

            quad.DrawParabolaFunction(y2);
            quad.DrawParabolaFunction(y1);

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}
