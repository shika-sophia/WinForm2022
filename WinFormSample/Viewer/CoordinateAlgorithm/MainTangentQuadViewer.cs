/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainTangentQuadViewer.cs
 *@class   └ new FormTangentQuadViewer() : Form
 *@class       └ new AlgoCoordinateQuadratic() : AlgoCoordinateLinear
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content TangentQuadViewer
 *         接線, 接線を共有する２次関数の描画
 *         
 *         [英] tangent        接する
 *         [英] differentiate  微分する
 *         
 *@subject 
 *
 *@see ImageTangentQuadViewer.jpg
 *@see 
 *@author shika
 *@date 2022-09-26
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainTangentQuadViewer
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormTangentQuadViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormTangentQuadViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormTangentQuadViewer : Form
    {
        private readonly PictureBox pic;
        private readonly AlgoCoordinateQuadratic quad;

        public FormTangentQuadViewer()
        {
            this.Text = "FormTangentQuadViewer";
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

            quad = new AlgoCoordinateQuadratic(pic);
            quad.DrawCoordinateAxis();

            EquationQuadratic eqQuad = new EquationQuadratic(0.005f, new PointF(0, -50));
            float ptY = quad.AlgoFunctionXtoY(100f, eqQuad);
            EquationLinear eqLinear = quad.DifferentiateQuad(eqQuad, new PointF(100, ptY));

            quad.DrawMultiQuadraticFunction(new ICoordinateEquation[] { eqQuad, eqLinear });
            
            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}
