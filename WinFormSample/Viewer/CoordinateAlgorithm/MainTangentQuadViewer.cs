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
 *@subject 微分, 接線
 *         =>〔AlgoCoordinateDifferentiate.cs〕
 *
 *@see ImageTangentQuadViewer.jpg
 *@see 
 *@author shika
 *@date 2022-09-26
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainTangentQuadViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
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
        private readonly AlgoCoordinateDifferentiate diff;

        public FormTangentQuadViewer()
        {
            this.Text = "FormTangentQuadViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(960, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;

            pic = new PictureBox()
            {
                ClientSize = this.ClientSize,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };

            diff = new AlgoCoordinateDifferentiate(pic);
            diff.DrawCoordinateAxis();

            EquationQuadratic eqQuad = new EquationQuadratic(0.005f, new PointF(0, 30));

            List<ICoordinateEquation> eqList = new List<ICoordinateEquation>();
            eqList.Add(eqQuad);
            List<PointF> pointList = new List<PointF>();

            //---- 接点 PointF(100, ptY)における接線 ----
            //float ptX = 100f;
            //float ptY = diff.AlgoFunctionXtoY(ptX, eqQuad);
            //EquationLinear eqLinear = diff.AlgoTangentLineOnContact(eqQuad, new PointF(ptX, ptY));
            //eqList.Add(eqLinear);

            //---- 任意の点 PointF(x, y)を通る接線 ----
            float ptX = 0f;
            float ptY = -50f;
            PointF givenPoint = new PointF(ptX, ptY);
            pointList.Add(givenPoint);

            EquationLinear[] tangentLineAry = 
                diff.AlgoTangentLineFree(
                    eqQuad, givenPoint, out PointF[] contactAry);
            eqList.AddRange(tangentLineAry);
            pointList.AddRange(contactAry);

            diff.DrawMultiQuadraticFunction(eqList.ToArray(), pointList.ToArray());
            
            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor
    }//class
}
