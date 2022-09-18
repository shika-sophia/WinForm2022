/** 
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class CoordinateAxis(PictureBox) 
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
 *@subject Matrix 平行移動, 拡大縮小
 *         void  matrix.Translate(float offsetX, float offsetY) 
 *         void  matrix.Scale(float sx, float sy)  拡大縮小
 *         g.Transform = matrix;
 *         を用いて、Graphicsの原点(0, 0)を平行移動し、PictureBoxの中心点と 一致させる。
 *         
 *         ・これにより、数学の点の座標のまま、Graphicsの座標にすることができる。
 *         ・Y座標の反転を image.RotateFlip()で行うと、文字まで反転してしまうので、
 *           DrawLine(), DrawString()時に Y座標に「-」を付けて反転させる。
 *         
 *@see ImageCoordinateAxisViewer.jpg
 *@see 
 *@author shika
 *@date 2022-09-16
 */
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    class AlgoCoordinateAxis
    {
        protected readonly PictureBox pic;
        protected readonly Graphics g;
        protected readonly PointF centerPoint;
        protected readonly Pen penBlue = new Pen(Color.CornflowerBlue, 1);
        protected readonly Pen penPink = new Pen(Color.HotPink, 2);
        protected readonly Font font = new Font("ＭＳ 明朝", 12, FontStyle.Bold);

        public AlgoCoordinateAxis(PictureBox pic)
        {
            this.pic = pic;
            centerPoint = new PointF(
                (float)((decimal)pic.ClientSize.Width / 2M),
                (float)((decimal)pic.ClientSize.Height / 2M));
            g = BuildGraphics();
        }//constructor

        private Graphics BuildGraphics()
        {
            Bitmap bitmap = new Bitmap(
                pic.ClientSize.Width, pic.ClientSize.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;
            penBlue.SetLineCap(LineCap.ArrowAnchor, LineCap.ArrowAnchor, DashCap.Flat);

            Matrix mx = new Matrix();
            mx.Translate(centerPoint.X, centerPoint.Y);
            mx.Scale(0.96f, 0.96f);
            g.Transform = mx;
            pic.Image = bitmap;

            return g;
        }//BuildGraphics()

        public void DrawCoordinateAxis()
        {
            g.DrawLine(penBlue, 0, -centerPoint.Y, 0, centerPoint.Y);  // X軸
            g.DrawLine(penBlue, -centerPoint.X, 0, centerPoint.X, 0);  // Y軸

            Brush brushBlue = penBlue.Brush;
            g.DrawString("Ｏ", font, brushBlue, -25, 8);
            g.DrawString("Ｘ", font, brushBlue, (float)((decimal)centerPoint.X - 18M), 8);
            g.DrawString("Ｙ", font, brushBlue, -25, -centerPoint.Y);
        }//DrawCoordinateAxis

        public void DrawPointGrid(PointF pt, bool withGrid = false)
        {
            Brush brushPink = penPink.Brush;
            g.FillEllipse(brushPink, 
                (float)((decimal)pt.X - 3M),
                -(float)((decimal)pt.Y + 3M), 6, 6);
                g.DrawString($"({pt.X},{pt.Y})", font, brushPink,
                (float)((decimal)pt.X - 40M),
                -(float)((decimal)pt.Y + ((pt.Y > 0) ? 30M : -10M)));
            
            brushPink.Dispose();

            if (withGrid)
            {
                penBlue.DashStyle = DashStyle.Dash;
                penBlue.SetLineCap(LineCap.Flat, LineCap.Flat ,DashCap.Flat);
                g.DrawLine(penBlue, pt.X, -pt.Y, 0, -pt.Y);
                g.DrawLine(penBlue, pt.X, -pt.Y, pt.X, 0);

                Brush brushBlue = penBlue.Brush;
                g.DrawString($"{pt.X}", font, brushBlue,
                    (float)((decimal)pt.X - 15M),
                    (pt.Y > 0) ? 5f : -20f);
                g.DrawString($"{pt.Y}", font, brushBlue,
                    (pt.X > 0) ? -35f : 5f,
                    -(float)((decimal)pt.Y + 5M));
                
                brushBlue.Dispose();
            }
        }//DrawPointGrid()
    }//class
}
