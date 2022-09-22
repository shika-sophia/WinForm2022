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
    class MainMultiLinearFunctionViewer
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormMultiLinearFunctionViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormMultiLinearFunctionViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMultiLinearFunctionViewer : Form
    {
        private readonly PictureBox pic;
        private readonly AlgoCoordinateQuadratic quad;

        public FormMultiLinearFunctionViewer()
        {
            this.Text = "FormMultiLinearFunctionViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(1080, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;

            pic = new PictureBox()
            {
                ClientSize = this.ClientSize,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };

            quad = new AlgoCoordinateQuadratic(pic);
            //quad.DrawCoordinateAxis();

            var eqLinear1 = new EquationLinear(1.0f, -100);
            var eqLinear2 = new EquationLinear(-0.5f, 500);
            var eqLinear3 = new EquationLinear(-2f, -400);

            quad.DrawMultiLinearFunciton(
                new EquationLinear[] { eqLinear1, eqLinear2, eqLinear3 });

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}
