/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainSimultaneousLinearEquation.cs
 *@class   └ new FormSimultaneousLinearEquation() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content SimultaneousLinearEquation
 *         ２元１次 連立方程式の解を求め、２直線の交点を表示する。
 *         
 *         [英] linear:         線形, 1次方程式
 *         [英] two variable:   ２変数, ２元
 *         [英] simultaneous equation: 連立方程式
 *         [英] intersectecion:  交点
 *         [英] solution:  解
 *         [英] slope:     傾き
 *         [英] gradient:  勾配
 *         [英] intercept: 切片  x-intercept, y-intercept
 *         
 *@subject 連立方程式の解
 *         =>〔AlgoCoordinateLinear.cs〕
 *
 *@see ImageSimultaneousLinearEquation.jpg
 *@see AlgoCoordinateLinear.cs
 *@author shika
 *@date 2022-09-18
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainSimultaneousLinearEquation
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormSimultaneousLinearEquation()");

            Application.EnableVisualStyles();
            Application.Run(new FormSimultaneousLinearEquation());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormSimultaneousLinearEquation : Form
    {
        private readonly PictureBox pic;
        private readonly AlgoCoordinateLinear linear;

        public FormSimultaneousLinearEquation()
        {
            this.Text = "FormSimultaneousLinearEquation";
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

            linear = new AlgoCoordinateLinear(pic);
            linear.DrawCoordinateAxis();

            // y = a x + b
            float slope1 = 2f;
            float intercept1 = 50f;

            // y = c x + d
            float slope2 = -0.1f;
            float intercept2 = 200f;

            //連立方程式の解
            bool existSolution = linear.TrySolution(
                slope1, intercept1, slope2, intercept2, out PointF solutionPoint);
            
            if(!existSolution)
            {
                Console.WriteLine("(No solution)");
            }
            else
            {
                linear.DrawPointLine(solutionPoint, true);
            }

            //DrawPointLine()で PointAutoScale()を行うので、その後に DrawLine()
            linear.DrawLinearFunction(slope1, intercept1);
            linear.DrawLinearFunction(slope2, intercept2);

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}
