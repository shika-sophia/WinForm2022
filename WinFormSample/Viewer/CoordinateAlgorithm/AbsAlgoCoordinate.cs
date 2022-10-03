/*
 *@content 座標と関数のアルゴリズム
 *
 *@subject Inherit 継承関係
 *         AbsAlgoCoordinate     共通フィールドの保持
 *             ↑
 *         AlgoCoordinateAxis    十字座標系
 *             ↑
 *         AlgoCoordinateLinear : AlgoCoordinateAxis      線形 １次関数、垂直、
 *             ↑
 *         AlgoCoordinateQuadratic : AlgoCoordinateLinear 放物線 ２次関数
 *             ↑
 *         AlgoCoordinateDifferentiate : AlgoCoordinateQuadratic  微分、接線
 *             ↑
 *         AlgoCoordinateCircle : AlgoCoordinateDifferentiate     円、三角比
 *         
 */
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    abstract class AbsAlgoCoordinate
    {
        protected readonly PictureBox pic;
        protected readonly Pen penBlue = new Pen(Color.CornflowerBlue, 1);
        protected readonly Pen penPink = new Pen(Color.HotPink, 2);
        protected readonly Pen penViolet = new Pen(Color.Violet, 1);
        protected readonly Font font = new Font("ＭＳ 明朝", 12, FontStyle.Bold);
        protected readonly Font fontSmall = new Font("ＭＳ 明朝", 8, FontStyle.Regular);
        protected readonly PointF centerPoint;
        protected readonly decimal ratioWidthHeight;  // = height / width
        protected Graphics g;
        protected GraphicsState defaultGrapics;
        protected decimal scaleRate = 2.0M;

        protected AbsAlgoCoordinate(PictureBox pic)
        {
            this.pic = pic;
            centerPoint = new PointF(
                (float)((decimal)pic.ClientSize.Width / 2M),
                (float)((decimal)pic.ClientSize.Height / 2M));
            ratioWidthHeight =
                (decimal)pic.ClientSize.Height /
                (decimal)pic.ClientSize.Width;
        }//constructor

    }//class
}
