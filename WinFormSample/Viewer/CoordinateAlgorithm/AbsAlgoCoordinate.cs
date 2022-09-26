/*
 *@content 座標と関数のアルゴリズム
 *
 *@subject Inherit 継承関係
 *         AbsAlgoCoordinate     共通フィールドの保持
 *             ↑
 *         AlgoCoordinateAxis    十字座標系
 *             ↑
 *         AlgoCoordinateLinear : AlgoCoordinateAxis      線形 １次関数
 *             ↑
 *         AlgoCoordinateQuadratic : AlgoCoordinateLinear 放物線 ２次関数
 *             ↑
 *         AlgoCoordinateDifferentiate : AlgoCoordinateQuadratic  微分、接線、垂直、漸近線
 *
 *@subject Graphics BuildGraphics()
 *         AlgoCoordinateAxis 十字座標系に特化したメソッド
 */
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    abstract class AbsAlgoCoordinate
    {
        protected readonly PictureBox pic;
        protected readonly Graphics g;
        protected readonly Pen penBlue = new Pen(Color.CornflowerBlue, 1);
        protected readonly Pen penPink = new Pen(Color.HotPink, 2);
        protected readonly Font font = new Font("ＭＳ 明朝", 12, FontStyle.Bold);
        protected readonly Font fontSmall = new Font("ＭＳ 明朝", 8, FontStyle.Regular);
        protected readonly PointF centerPoint;
        protected readonly decimal ratioWidthHeight;
        protected GraphicsState defaultGrapics;
        protected decimal scaleRate = 2.0M;

        protected AbsAlgoCoordinate(PictureBox pic)
        {
            this.pic = pic;
            centerPoint = new PointF(
                (float)((decimal)pic.ClientSize.Width / 2M),
                (float)((decimal)pic.ClientSize.Height / 2M));
            ratioWidthHeight =
                (decimal)pic.ClientSize.Width /
                (decimal)pic.ClientSize.Height;
            g = BuildGraphics();
        }//constructor

        private Graphics BuildGraphics()
        {
            Bitmap bitmap = new Bitmap(
                pic.ClientSize.Width,
                pic.ClientSize.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;

            Matrix mx = new Matrix();
            mx.Translate(centerPoint.X, centerPoint.Y);
            mx.Scale(0.96f, 0.96f);  //fixed
            g.Transform = mx;
            pic.Image = bitmap;

            defaultGrapics = g.Save();
            return g;
        }//BuildGraphics()
    }//class
}
