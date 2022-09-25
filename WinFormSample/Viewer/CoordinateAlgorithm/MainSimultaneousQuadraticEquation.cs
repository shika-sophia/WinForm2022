/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class MainSimultaneousQuadraticEquation.cs
 *@class   └ new FormSimultaneousQuadraticEquation() : Form
 *@class       └ new AlgoCoordinateQuagratic() : AlgoCoordinateLinear
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content SimultaneousQuadraticEquation
 *         ２次方程式どうしの連立方程式の解
 *         １次方程式は ２次方程式の一般式 a = 0 で表現可
 *         
 *@subject 
 *
 *@see ImageSimultaneousQuadraticEquation.jpg
 *@see 
 *@author shika
 *@date 2022-09-23
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainSimultaneousQuadraticEquation
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormSimultaneousQuadraticEquation()");

            Application.EnableVisualStyles();
            Application.Run(new FormSimultaneousQuadraticEquation());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormSimultaneousQuadraticEquation : Form
    {
        private readonly PictureBox pic;
        private readonly AlgoCoordinateQuadratic quad;

        public FormSimultaneousQuadraticEquation()
        {
            this.Text = "FormSimultaneousQuadraticEquation";
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

            //---- Equation, TrySolution() ----
            var eq1 = new EquationQuadratic(0.005f, new PointF(0, -100));
            var eq2 = new EquationQuadratic(-0.005f, new PointF(120, 200));
            var eq3 = new EquationLinear(0.25f, 50f);

            quad.DrawMultiQuadraticFunction(
                new ICoordinateEquation[] { eq1, eq2, eq3, });

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}

/*
//==== old code of here ====
//=> quad.DrawMultiQuadraticFunction()

    bool existSolution = quad.TrySolutionQuad(
        eq1, eq2, out PointF[] solutionAry);

    List<PointF> pointList = new List<PointF>();
    if (existSolution)
    {
        foreach (PointF pt in solutionAry)
        {
            if (float.IsNaN(pt.X)) { continue; }

            pointList.Add(pt);
        }//foreach
    }
    else
    {
        Console.WriteLine("(No Solution)");                
    }

    //---- eq1 pointList ----
    pointList.Add(eq1.Vertex);
    pointList.Add(quad.AlgoInterceptY(eq1));
    pointList.AddRange(quad.AlgoInterceptX(eq1));

    //---- eq2 pointList ----
    //pointList.Add(eq2.Vertex);
    pointList.Add(quad.AlgoInterceptY(eq2));
    pointList.AddRange(quad.AlgoInterceptX(eq2));
    //pointList.Add(quad.AlgoInterceptX(eq2));

    //---- Draw ----
    quad.DrawMultiPointLine(pointList.ToArray());
    quad.DrawParabolaFunction(eq1);
    //quad.DrawParabolaFunction(eq2);
    quad.DrawLinearFunction(eq2);
 */
