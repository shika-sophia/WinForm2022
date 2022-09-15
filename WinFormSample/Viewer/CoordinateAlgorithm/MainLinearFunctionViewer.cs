/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@based MainCoordinateAxisViewer.cs
 *@class MainLinearFunctionViewer.cs
 *@class   └ new FormLinearFunctionViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content LinearFunctionViewer
 *         １次関数  y = a x + b の描画
 *
 *@subject Matrix 平行移動
 *         =>〔MainCoordinateAxisViewer.cs〕
 *         
 *@subject AlgoLinearParam  ２点の座標から、１次関数のパラメータを求める
 *         ・２点を通る直線は一意に決定できる
 *         ・２点の X座標, Y座標の差をそれぞれ取り、
 *           傾き a = dy / dx 
 *           Ｙ切片 b は aの値を代入した y = (定数 a) x + bに
 *           １点の座標(x, y)を代入して求める。
 *         ・複数の戻り値は C#「タプル」を参照 〔CS 58〕
 *         
 *         (decimal a , decimal b)  AlgoLinearParam(PointF pt1, PointF pt2)
 *         decimal                  AlgoLinearParam(decimal a, PointF pt1)
 *         
 *@subject LinearFunction  X, Y の関係を示す式
 *         ・引数 a, bを渡して、Methodで関係性を再現する。
 *         ・X -> Y,  Y -> X で計算式が異なる
 *         ・「a = 0」のときの 0除算にならないよう if文で条件分岐することに注意
 *         
 *         decimal  LinearFunctionXtoY(decimal x, decimal a, decimal b)
 *         decimal  LinearFunctionYtoX(decimal y, decimal a, decimal b)
 *
 *@subject DrawLinearFunction
 *         ・１次関数  y = a x + b の a, bを確定させ、
 *         ・X軸の右端と左端の X座標を代入し、それぞれの Y座標を求める
 *         ・Yの値が 座標範囲より大きくなっても OK
 *         ・graphics.DrawLine(Pen, PointF, PointF)で描画
 *           Y座標は、数学座標と Graphics座標で反転しているため、
 *           DrawLine()のときだけ Y座標にのみ「-」をつけて反転させる。(描画だけ反転する)
 *           計算時に反転させる必要はない。
 *           
 *@see ImageLinearFunctionViewer.jpg
 *@see MainCoordinateAxisViewer.cs
 *@author shika
 *@date 2022-09-16
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainLinearFunctionViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormLinearFunctionViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormLinearFunctionViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormLinearFunctionViewer : Form
    {
        private readonly PictureBox pic;
        private readonly Graphics g;
        private readonly PointF centerPoint;
        private readonly Pen penBlue = new Pen(Color.CornflowerBlue, 1);
        private readonly Pen penPink = new Pen(Color.HotPink, 2);

        public FormLinearFunctionViewer()
        {
            this.Text = "FormLinearFunctionViewer";
            this.Font = new Font("ＭＳ 明朝", 12, FontStyle.Bold);
            this.ClientSize = new Size(480, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;

            pic = new PictureBox()
            {
                ClientSize = this.ClientSize,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };

            centerPoint = new PointF(
                (float)((decimal)pic.ClientSize.Width / 2M),
                (float)((decimal)pic.ClientSize.Height / 2M));

            Bitmap bitmap = new Bitmap(
                pic.ClientSize.Width, pic.ClientSize.Height);
            g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;
            penBlue.SetLineCap(LineCap.ArrowAnchor, LineCap.ArrowAnchor, DashCap.Flat);

            Matrix mx = new Matrix();
            mx.Translate(centerPoint.X, centerPoint.Y);
            g.Transform = mx;
            pic.Image = bitmap;

            DrawCoordinateAxis();
            DrawLinearFunction();

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor

        private void DrawCoordinateAxis()
        {
            g.DrawLine(penBlue, 0, -220, 0, 220);  // X軸
            g.DrawLine(penBlue, -220, 0, 220, 0);  // Y軸

            Brush brushBlue = penBlue.Brush;
            g.DrawString("Ｏ", this.Font, brushBlue, -25, 8);
            g.DrawString("Ｘ", this.Font, brushBlue, 220 - 18, 8);
            g.DrawString("Ｙ", this.Font, brushBlue, -25, -220);
        }//DrawCoordinateAxis

        private void DrawLinearFunction()
        {
            var pt1 = new PointF(0, 50);
            var pt2 = new PointF(50, 100);
            var (a, b) = AlgoLinearParam(pt1, pt2);

            float minX = -220f;
            float minY = (float)LinearFunctionXtoY((decimal)minX, a, b);
            float maxX = 220f;
            float maxY = (float)LinearFunctionXtoY((decimal)maxX, a, b);

            g.DrawLine(penPink, minX, -minY, maxX, -maxY);  //Y座標を反転
            g.DrawString($"y = {a} x + {b}", this.Font, penPink.Brush, 20, -pt1.Y);
        }//DrawLinearFunction

        private (decimal a, decimal b) AlgoLinearParam(PointF pt1, PointF pt2)
        {
            decimal dx = (decimal)pt1.X - (decimal)pt2.X;
            decimal dy = (decimal)pt1.Y - (decimal)pt2.Y;

            decimal a;
            if (dx == 0) { a = 0; }
            else { a = dy / dx; }

            decimal b = AlgoLinearParam(a, pt1);

            return (a, b);
        }//AlgoLinerParam(pt1, pt2)

        private decimal AlgoLinearParam(decimal a, PointF pt1)
        {
            return (decimal)pt1.Y - a * (decimal)pt1.X;
        }

        private decimal LinearFunctionXtoY(decimal x, decimal a, decimal b)
        {
            return a * x + b;
        }//AlgoLinearFunction(x) -> y

        private decimal LinearFunctionYtoX(decimal y, decimal a, decimal b)
        {
            decimal x;
            if (a == 0)
            {
                x = y;
            }
            else
            {
                x = (y - b) / a;
            }

            return x;
        }//AlgoLinearFunction(y) -> x
    }//class
}
