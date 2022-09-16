/*
 *@title   AlgoCoordinateAxis
 *@inherit AlgoCoordinateLinear : AlgoCoordinateAxis
 *@used MainCoordinateAxis.cs
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
            mx.Translate(
                (float)((decimal)centerPoint.X),
                (float)((decimal)centerPoint.Y));
            mx.Scale(0.96f, 0.96f);
            g.Transform = mx;
            pic.Image = bitmap;

            return g;
        }//BuildGraphics()

        public void DrawCoordinateAxis()
        {
            Font font = new Font("ＭＳ 明朝", 12, FontStyle.Bold);
            g.DrawLine(penBlue, 0, -centerPoint.Y, 0, centerPoint.Y);  // X軸
            g.DrawLine(penBlue, -centerPoint.X, 0, centerPoint.X, 0);  // Y軸

            Brush brushBlue = penBlue.Brush;
            g.DrawString("Ｏ", font, brushBlue, -25, 8);
            g.DrawString("Ｘ", font, brushBlue, centerPoint.X - 18, 8);
            g.DrawString("Ｙ", font, brushBlue, -25, -centerPoint.Y);
        }//DrawCoordinateAxis
    }//class
}
