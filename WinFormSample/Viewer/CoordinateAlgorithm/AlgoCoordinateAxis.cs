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
 *@subject 点の描画
 *         void  +DrawPointLine(PointF pt, bool withLine = false)
 *         
 *@subject 軸の目盛り AxisScale
 *         void  -DrawAxisScale()  自己定義メソッド
 *                100, -100ごとに目盛りを付加 (座標範囲によって 1000, -1000)
 *
 *@subject decimal  scaleRate プロパティ
 *         void  SetScaleRate(decimal)
 *         ・DrawCoordinateAxis(), DrawAxisScale()
 *         ・DrawPointLine()
 *         ・DrawLine(), DrawString()
 *         各 X, Y座標に scaleRateを乗算する。
 *         これにより、座標範囲を超えた点も、描画可能。
 *         座標の目盛りのほうを 50%, 25%, 12.5%...と縮小していく
 *         縮小していくと目盛りが細かくなりすぎて、見ずらくなるので、
 *         1000, -1000単位で区切り、1k, 2k, ...と表示
 *         
 *         ※ 軸と目盛りはデフォルトで再描画するが、DrawPointLine()以前の
 *         DrawLine(), DrawString()は消えてしまうので、手動で再描画することに注意
 *         
 *@subject 自動スケール調整
 *         void  PointAutoScale(PointF)
 *         ・引数 PointFが 座標範囲外の場合、座標範囲内になるまで scaleRateを調整
 *         ・scaleRate = 2.0M, 1.0M, 0.5M, 2.5M, 1.25M ...
 *         
 *         while (((decimal)centerPoint.X / scaleRate) < (decimal)pt.X
 *              || ((decimal)centerPoint.Y / scaleRate) < (decimal)pt.Y)
 *          {
 *              SetScaleRate(scaleRate / 2M);
 *          }//while
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
    class AlgoCoordinateAxis : AbsAlgoCoordinate
    {
        public AlgoCoordinateAxis(PictureBox pic) : base(pic)
        {
            g = BuildGraphics();
        }

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

        public void DrawCoordinateAxis()
        {
            penBlue.DashStyle = DashStyle.Solid;
            penBlue.SetLineCap(LineCap.ArrowAnchor, LineCap.ArrowAnchor, DashCap.Flat);
            g.DrawLine(penBlue, 0, -centerPoint.Y, 0, centerPoint.Y);  // X軸
            g.DrawLine(penBlue, -centerPoint.X, 0, centerPoint.X, 0);  // Y軸

            Brush brushBlue = penBlue.Brush;
            g.DrawString("Ｏ", font, brushBlue, -25, 8);
            g.DrawString("Ｘ", font, brushBlue, (float)((decimal)centerPoint.X - 18M), 8);
            g.DrawString("Ｙ", font, brushBlue, -25, -centerPoint.Y);

            DrawAxisScale(brushBlue);
            brushBlue.Dispose();
        }//DrawCoordinateAxis

        private void DrawAxisScale(Brush brushBlue)
        {
            penBlue.DashStyle = DashStyle.Solid;
            penBlue.SetLineCap(LineCap.Flat, LineCap.Flat, DashCap.Flat);
            int scaleWidth = 4;
            SizeF plusSize = g.MeasureString("200", fontSmall);
            SizeF minusSize = g.MeasureString("-200", fontSmall);

            for (int i = 0;
                i < (int)centerPoint.X / scaleRate;
                i += (scaleRate > 0.25M ? 100 : 1000))
            {
                if (i == 0) { continue; }
                g.DrawLine(penBlue, 
                    (float)(i * scaleRate), scaleWidth,
                    (float)(i * scaleRate), -scaleWidth);     // X-Axis +
                g.DrawLine(penBlue, 
                    (float)(-i * scaleRate), scaleWidth,
                    (float)(-i * scaleRate), -scaleWidth);   // X-Axis -

                g.DrawString(scaleRate > 0.1M ? $"{i}" : $"{i / 1000}k",
                    fontSmall, brushBlue,
                    (float)((i * scaleRate) - (decimal)plusSize.Width / 2M), 10f);
                g.DrawString(scaleRate > 0.1M ? $"{i}" : $"{i / 1000}k",
                    fontSmall, brushBlue,
                    (float)((-i * scaleRate) - (decimal)minusSize.Width / 2M), 10f);
            }//for

            for (int i = 0;
                i < (int)centerPoint.Y / scaleRate;
                i += (scaleRate > 0.25M ? 100 : 1000))
            {
                if (i == 0) { continue; }
                g.DrawLine(penBlue,
                    -scaleWidth, (float)(-i * scaleRate),
                    scaleWidth, (float)(-i * scaleRate));   // Y-Axis +
                g.DrawLine(penBlue, 
                    -scaleWidth, (float)(i * scaleRate), 
                    scaleWidth, (float)(i * scaleRate));    // Y-Axis -
                
                g.DrawString(scaleRate > 0.05M ? $"{i}" : $"{i / 1000}k",
                    fontSmall, brushBlue,
                    -plusSize.Width - 5f, 
                    (float)((decimal)-i * scaleRate - (decimal)plusSize.Height / 2M));
                g.DrawString(scaleRate > 0.05M ? $"{i}" : $"{i / 1000}k",
                    fontSmall, brushBlue,
                    -minusSize.Width - 5f,
                    (float)((decimal)i * scaleRate - (decimal)minusSize.Height / 2M));
            }//for
        }//DrawAxisScale()

        public void DrawPointLine(PointF pt, bool withLine = false)
        {
            //PointAutoScale(pt);

            if (float.IsNaN(pt.X) || float.IsNaN(pt.Y)) { return; }

            Brush brushPink = penPink.Brush;
            g.FillEllipse(brushPink,
                (float)((decimal)pt.X * scaleRate - 3M),
                -(float)((decimal)pt.Y * scaleRate + 3M), 6, 6);

            SizeF pointSize = g.MeasureString($"({pt.X},{pt.Y})", fontSmall);
            g.DrawString($"({pt.X:0.##},{pt.Y:0.##})", fontSmall, brushPink,
                (float)((decimal)pt.X * scaleRate - (decimal)pointSize.Width / 2M),
                -(float)((decimal)pt.Y * scaleRate +
                    ((pt.Y > 0) ? (decimal)pointSize.Height + 5M: (decimal)-pointSize.Height + 5M)));

            if (withLine)
            {
                penBlue.DashStyle = DashStyle.Dash;
                penBlue.SetLineCap(LineCap.Flat, LineCap.Flat, DashCap.Flat);
                g.DrawLine(penBlue,
                    (float)((decimal)pt.X * scaleRate), (float)((decimal)-pt.Y * scaleRate),
                    0f, (float)((decimal)-pt.Y * scaleRate));

                g.DrawLine(penBlue,
                    (float)((decimal)pt.X * scaleRate), (float)((decimal)-pt.Y * scaleRate),
                    (float)((decimal)pt.X * scaleRate), 0f);

                if(pt.X != 0)
                {
                    SizeF ptXSize = g.MeasureString($"{pt.X}", fontSmall);
                    g.DrawString($"{pt.X:0.##}", fontSmall, brushPink,
                        (float)((decimal)pt.X * scaleRate -(decimal)ptXSize.Width / 2M),
                        (pt.Y > 0) ? 5f : -ptXSize.Height - 5f);
                }

                if(pt.Y != 0)
                {
                    SizeF ptYSize = g.MeasureString($"{pt.Y}", fontSmall);
                    g.DrawString($"{pt.Y:0.##}", fontSmall, brushPink,
                        (pt.X > 0) ? -ptYSize.Width -5f : 5f,
                    -(float)((decimal)pt.Y * scaleRate + (decimal)ptYSize.Height / 2M));
                }
            }//if withLine

            brushPink.Dispose();
        }//DrawPointLine()

        public void DrawMultiPointLine(PointF[] pointAry, bool withLine = false)
        {
            //絶対値で最大値(X座標とY座標の比を考慮した最大値)を持つ PointFで
            //PointAutoScale()してから DrawPointLine()

            decimal max = 0M;
            PointF maxPoint = new PointF(0, 0);
            foreach (PointF pt in pointAry)
            {
                if(float.IsNaN(pt.X) || float.IsNaN(pt.Y)) { continue; }

                decimal lager = Math.Max(
                    Math.Abs((decimal)pt.X * ratioWidthHeight),  
                    Math.Abs((decimal)pt.Y)); 

                if(max < lager)
                {
                    max = lager;
                    maxPoint = pt;
                }
            }//foreach
            
            PointAutoScale(maxPoint);

            foreach (PointF pt in pointAry)
            {
                DrawPointLine(pt, withLine);
            }//forech
        }//DrawMultiPointLine()

        public void SetScaleRate(decimal scaleRate)
        {
            this.scaleRate = scaleRate;
            g.Clear(SystemColors.Window);
            DrawCoordinateAxis();
        }//SetScaleRate()

        public void PointAutoScale(PointF pt)
        {
            while ((Math.Abs((decimal)centerPoint.X) / scaleRate) < Math.Abs((decimal)pt.X)
                || (Math.Abs((decimal)centerPoint.Y) / scaleRate) < Math.Abs((decimal)pt.Y))
            {
                SetScaleRate(scaleRate / 2M);

                if(scaleRate < 0.02M) { break; }  
            }//while
        }//PointAutoScale()
    }//class
}
