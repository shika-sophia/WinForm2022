/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class MainCoordinateAxisViewer.cs
 *@class   └ new FormCoordinateAxisViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content CoordinateAxisViewer
 *         座標軸のみを描画
 *         
 *@subject Matrix 平行移動
 *         void  matrix.Translate(float offsetX, float offsetY) 
 *         g.Transform = matrix;
 *         を用いて、Graphicsの原点(0, 0)を平行移動し、PictureBoxの中心点と 一致させる。
 *         
 *         ・これにより、数学の点の座標のまま、Graphicsの座標にすることができる。
 *         ・Y座標の反転を image.RotateFlip()で行うと、文字まで反転してしまうので、
 *           DrawLine()時に Y座標に「-」を付けて反転させる。
 *         
 *@see ImageCoordinateAxisViewer.jpg
 *@see 
 *@author shika
 *@date 2022-09-16
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class MainCoordinateAxisViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormCoordinateAxisViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormCoordinateAxisViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormCoordinateAxisViewer : Form
    {
        private readonly PictureBox pic;
        private readonly Graphics g;
        private readonly PointF centerPoint;
        private readonly Pen penBlue = new Pen(Color.CornflowerBlue, 1);
        private readonly Pen penPink = new Pen(Color.HotPink, 2);

        public FormCoordinateAxisViewer()
        {
            this.Text = "FormCoordinateAxisViewer";
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
            g.DrawString("Ｘ", this.Font, brushBlue, 220 -18, 8);
            g.DrawString("Ｙ", this.Font, brushBlue, -25, -220);
        }//DrawCoordinateAxis
    }//class
}
