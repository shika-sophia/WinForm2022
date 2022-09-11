/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainHexagonViewer.cs
 *@class   └ new FormHexagonViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content HexagonViewer
 *@subject AlgoHexagon 中心点と外接円の半径から正六角形の６点を計算
 *         ・正六角形の一辺 == 外接円の半径
 *         ・正六角形の各点と中心点を結ぶと、６個の正三角形となる
 *         ・正三角形のアルゴリズムと同様、
 *           三平方の定理 30, 60, 90°の辺比 2 : 1 : √3 を用いて、各点の座標を算出
 *           
 *         PointF  AlgoHexagon(PointF centerPoint, decimal radius)
 *                 
 *@subject Davide Star
 *         ・ダビデの星: イスラエルの国旗に使われている六芒星のこと
 *         ・正三角形を逆向きに２つ重ね合わせた意匠
 *         ・AlgoHexagon()を利用し、まず六角形を作り、
 *           その対角線で正三角形を描画する。
 *           
 *@subject AlgoMiddlePoint  ２点の中点座標
 *         両点の X, Y座標を それぞれ + して / 2をする
 *         
 *         PointF  AlgoMiddlePoint(PointF pt1, PointF pt2)
 *         
 *@subject DrawString()
 *         ・点の名前, 角度, 辺の長さなどのテキスト描画
 *         ・位置を合わせるのが大変
 *         ・指定座標は１文字の領域 Rectangleの左上隅の点
 *         ・文字幅, 文字の高さ分だけ ずれるので、それを微調整する
 *         
 *@subject AlgoCircle()
 *         =>〔MainSqureCircleViewer.cs〕
 *
 *@subject AlgoTriangle()
 *         =>〔MainEquilaterTriangleViewer.cs〕
 *         
 *@see ImageHexagonViewer.jpg
 *@see 
 *@author shika
 *@date 2022-09-12
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.FigureAlgorithm
{
    class MainHexagonViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormHexagonViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormHexagonViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHexagonViewer : Form
    {
        private readonly PictureBox pic;
        private readonly PointF centerHexgon;
        private readonly PointF centerDavide;
        private readonly Graphics g;
        private readonly Pen penBlue = new Pen(Color.CornflowerBlue, 2);
        private readonly Pen penPink = new Pen(Color.DeepPink, 1);
        private const decimal RADIUS = 100M;

        public FormHexagonViewer()
        {
            this.Text = "FormHexagonViewer";
            this.Font = new Font("ＭＳ 明朝", 12, FontStyle.Regular);
            this.Size = new Size(640, 480);
            this.BackColor = SystemColors.Window;

            pic = new PictureBox()
            {
                ClientSize = this.ClientSize,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            Bitmap bitmap = new Bitmap(
                pic.ClientSize.Width, pic.ClientSize.Height);
            g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;

            centerHexgon = new PointF(
                (float)((decimal)pic.ClientSize.Width / 4M),
                (float)((decimal)pic.ClientSize.Height / 2M));

            centerDavide = new PointF(
                (float)((decimal)pic.ClientSize.Width * 3M / 4M),
                (float)((decimal)pic.ClientSize.Height / 2M));

            DrawHexagon();
            DrawDavide();

            penBlue.Dispose();
            penPink.Dispose();
            g.Dispose();
            pic.Image = bitmap;

            this.Controls.AddRange(new Control[]
            {
                pic,
            });
        }//constructor

        private void DrawHexagon()
        {
            //====== Hexagon ======
            //---- centerPoint ----
            g.FillEllipse(penBlue.Brush,
                (float)((decimal)centerHexgon.X - 3M),
                (float)((decimal)centerHexgon.Y - 3M), 6, 6);

            //---- Hexagon ----
            PointF[] hexgonPointAry = AlgoHexagon(centerHexgon, RADIUS);
            g.DrawPolygon(penBlue, hexgonPointAry);

            //---- center Line ----
            foreach (PointF point in hexgonPointAry)
            {
                penBlue.Width = 0.5f;
                penBlue.DashStyle = DashStyle.Dash;
                g.DrawLine(penBlue, centerHexgon, point);
            }//foreach

            //---- virtical Line ----
            PointF virticalPoint = new PointF(
                centerHexgon.X,
                (float)((decimal)centerHexgon.Y - RADIUS / 2M));

            g.DrawLine(penBlue,
                virticalPoint, hexgonPointAry[1]);  //pointB

            //---- circumscribed Circle of Hexagon ----
            RectangleF rectHexagon = AlgoCircle(centerHexgon, RADIUS);
            g.DrawEllipse(penPink, rectHexagon);

            //---- DrawString() ----
            DrawName(hexgonPointAry, virticalPoint);
        }//DrawHexagon()

        private void DrawName(PointF[] hexgonPointAry, PointF virticalPoint)
        {
            //---- pointName ----
            string[] pointName = new string[]
            {
                "Ａ", "Ｂ", "Ｃ", "Ｄ", "Ｅ", "Ｆ"
            };

            decimal pointDistance = 20M;
            decimal charAdjustX = 10M;
            for (int i = 0; i < hexgonPointAry.Length; i++)
            {
                float nameX = (float)((decimal)hexgonPointAry[i].X - charAdjustX);
                float nameY = hexgonPointAry[i].Y;

                switch (i)
                {
                    case 0: //pointA
                        nameY = (float)((decimal)nameY - pointDistance);
                        break;
                    case 1: //pointB
                        nameX = (float)((decimal)nameX + pointDistance);
                        nameY = (float)((decimal)nameY - pointDistance);
                        break;
                    case 2: //pointC
                        nameX = (float)((decimal)nameX + pointDistance);
                        nameY = (float)((decimal)nameY + pointDistance);
                        break;
                    case 3: //pointD
                        nameY = (float)((decimal)nameY + pointDistance);
                        break;
                    case 4: //pointE
                        nameX = (float)((decimal)nameX - pointDistance);
                        nameY = (float)((decimal)nameY + pointDistance);
                        break;
                    case 5: //pointF
                        nameX = (float)((decimal)nameX - pointDistance);
                        nameY = (float)((decimal)nameY - pointDistance);
                        break;
                }//switch

                g.DrawString(pointName[i], this.Font,
                    Brushes.CornflowerBlue,
                    new PointF(nameX, nameY));
            }//for

            //---- "30, 60, ┐" ----
            decimal angleDistance = 10M;
            decimal angleCharAdjust = 10M;
            Font fontAngle = new Font(this.Font.FontFamily, 8, FontStyle.Regular);
            Brush brushAngle = new SolidBrush(penBlue.Color);

            g.DrawString("60", fontAngle, brushAngle,
                (float)((decimal)hexgonPointAry[0].X + angleDistance - angleCharAdjust),
                (float)((decimal)hexgonPointAry[0].Y + angleDistance - 2M));
            
            g.DrawString("30", fontAngle, brushAngle,
                (float)((decimal)hexgonPointAry[1].X - angleDistance - angleCharAdjust - 3M),
                (float)((decimal)hexgonPointAry[1].Y - angleDistance + 2M));
            
            g.DrawString("┐", fontAngle, brushAngle,
                (float)((decimal)virticalPoint.X + angleDistance - angleCharAdjust - 1M),
                (float)((decimal)virticalPoint.Y - angleDistance));

            brushAngle.Dispose();

            //---- "r" ----
            var middleAF = AlgoMiddlePoint(hexgonPointAry[0], hexgonPointAry[5]);
            var middleAo = AlgoMiddlePoint(hexgonPointAry[0], centerHexgon);
            var middleFo = AlgoMiddlePoint(hexgonPointAry[5], centerHexgon);

            decimal radiusDistance = 10M;
            g.DrawString("ｒ", this.Font, penPink.Brush,
                middleAF.X, middleAF.Y);

            g.DrawString("ｒ", this.Font, penPink.Brush,
                (float)((decimal)middleAo.X - radiusDistance - 9M),
                (float)((decimal)middleAo.Y - 6M));

            g.DrawString("ｒ", this.Font, penPink.Brush,
                (float)((decimal)middleFo.X - 5M),
                (float)((decimal)middleFo.Y - radiusDistance - 5M));
        }//DrawName()

        private void DrawDavide()
        {
            //====== Davide Star ======
            var davidePointAry = AlgoHexagon(centerDavide, RADIUS);

            penBlue.Width = 12.5f;
            penBlue.DashStyle = DashStyle.Solid;
            g.DrawPolygon(penBlue, new PointF[]
            {
                davidePointAry[0], davidePointAry[2], davidePointAry[4]
            });

            g.DrawPolygon(penBlue, new PointF[]
            {
                davidePointAry[1], davidePointAry[3], davidePointAry[5]
            });
        }//DrawDavide()

        private RectangleF AlgoCircle(PointF centerPoint, decimal radius)
        {
            float rectX = (float)((decimal)centerPoint.X - radius);  //Rectangleの原点 X座標
            float rectY = (float)((decimal)centerPoint.Y - radius);  //Rectangleの原点 Y座標
            float width = (float)(radius * 2M);

            return new RectangleF(rectX, rectY, width, width);
        }//AlgoCircle()

        private PointF[] AlgoHexagon(PointF centerPoint, decimal radius)
        {
            decimal root3 = (decimal)Math.Sqrt(3d);  // √3

            var pointA = new PointF(                 // 上頂点を Aとし、時計回りに B, C, D, E, F
                centerPoint.X,
                (float)((decimal)centerPoint.Y - radius));

            var pointB = new PointF(
                (float)((decimal)pointA.X + radius / 2M * root3),
                (float)((decimal)pointA.Y + radius / 2M));

            var pointC = new PointF(
                pointB.X, 
                (float)((decimal)pointB.Y + radius));

            var pointF = new PointF(
                (float)((decimal)pointA.X - radius / 2M * root3),
                (float)((decimal)pointA.Y + radius / 2M));

            var pointE = new PointF(
                pointF.X, 
                (float)((decimal)pointF.Y + radius));

            var pointD = new PointF(
                centerPoint.X,
                (float)((decimal)centerPoint.Y + radius));

            return new PointF[]
            {
                pointA, pointB, pointC, pointD, pointE, pointF
            };
        }//AlgoHexagon()

        private PointF AlgoMiddlePoint(PointF pt1, PointF pt2)
        {
            return new PointF(
                (float)(((decimal)pt1.X + (decimal)pt2.X) / 2M),
                (float)(((decimal)pt1.Y + (decimal)pt2.Y) / 2M));
        }//AlgoMiddlePoint()
    }//class
}
