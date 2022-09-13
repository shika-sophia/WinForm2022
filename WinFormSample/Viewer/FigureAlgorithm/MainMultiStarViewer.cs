/** 
 *@title WinFormGUI / WinFormSample / Viewer / FigureAlgorithm
 *@class MainMultiStarViewer.cs
 *@class   └ new FormMultiStarViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content MultiStarViewer
 *         正n角形の対角線を用いた星型の描画
 *         
 *@subject AlgoMultiStar()
 *         正ｎ角形の ｎが 偶数(even)か 奇数(odd)によってアルゴリズムが異なる
 *         
 *@subject AlgoOddStar()  ｎが奇数のとき
 *         正ｎ角形の PointF[]を取得し、
 *         上頂点から１つずつスキップさせた点への対角線を結んでいくと、
 *         必ず上頂点に戻るので、一筆書きで GraphicsPathを作成する。
 *         
 *@subject AlgoEvenStar() ｎが偶数のとき
 *         ・正ｎ角形の PointF[]を取得し、
 *           上頂点から１つずつスキップさせると、
 *           必ず 正ｎ角形の半分の 正(n/2)角形が ２つできる
 *         ・一筆書きは不可能なので、いったん GraphicsPathを作成して、描画する必要がある
 *         ・それを２回行う。
 *         ・２回目は開始位置を１つずらす。
 *         ・ずらさないと１回目と同じものが２つ重なってしまう。
 *         
 *@subject AlgoMultiAngle()
 *         =>〔MainMultiAngleAlgorithm.cs〕
 *
 *@see ImageMultiStarViewer.jpg
 *@see 
 *@author shika
 *@date 2022-09-13
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.FigureAlgorithm
{
    class MainMultiStarViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormMultiStarViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormMultiStarViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMultiStarViewer : Form
    {
        private readonly PictureBox pic;
        private readonly Graphics g;
        private readonly Pen penBlue = new Pen(Color.CornflowerBlue, 2);
        private readonly Pen penPink = new Pen(Color.HotPink, 2);
        private readonly PointF centerPoint;
        private const decimal RADIUS = 150M;
        private int NUM = 5;

        public FormMultiStarViewer()
        {
            this.Text = "FormMultiStarViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(480, 480);
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

            AlgoMultiStar();

            pic.Image = bitmap;

            this.Controls.AddRange(new Control[]
            {
                pic,
            });

            penBlue.Dispose();
            penPink.Dispose();
            g.Dispose();
        }//constructor

        private PointF[] AlgoMultiAngle(PointF centerPoint, decimal radius, int num)
        {
            decimal centerAngleRadian = (360M / (decimal)num) * (decimal)Math.PI / 180M;  //中心角 θ = 360 / n (ラジアン単位: θπ / 180)

            PointF[] multiPointAry = new PointF[num];

            for (int i = 0; i < num; i++)
            {
                double angle = (double)((decimal)centerAngleRadian * i);

                multiPointAry[i] = new PointF(
                    (float)((decimal)centerPoint.X - radius * (decimal)Math.Sin(angle)),
                    (float)((decimal)centerPoint.Y - radius * (decimal)Math.Cos(angle)));
            }//for

            return multiPointAry;
        }//AlgoMultiAngle()

        private void AlgoMultiStar()
        {
            //---- OddStar ----
            if(NUM % 2 == 1)
            {
                AlgoOddStar(centerPoint, RADIUS, NUM);                
            }

            //---- EvenStar ----
            if(NUM % 2 == 0)
            {
                AlgoEvenStar(centerPoint, RADIUS, NUM);
            }
        }//AlgoMultiStar()

        private void AlgoOddStar(PointF centerPoint, decimal radius, int num)
        {
            PointF[] multiPointAry = AlgoMultiAngle(centerPoint, radius, num);      
            GraphicsPath gPath = new GraphicsPath(FillMode.Winding);

            PointF[] starPointAry = BuildSkipPointArray(num, multiPointAry);
            gPath.AddLines(starPointAry);

            g.FillPath(penBlue.Brush, gPath);            
            //g.DrawPath(penBlue, gPath);
        }//AlgoOddStar()

        private void AlgoEvenStar(PointF centerPoint, decimal radius, int num)
        {
            PointF[] multiPointAry = AlgoMultiAngle(centerPoint, radius, num);
            var gPath = new GraphicsPath(FillMode.Winding);

            int halfAngle = num / 2;
            PointF[] harfPointAry = BuildSkipPointArray(halfAngle, multiPointAry, start: 0);
            gPath.AddLines(harfPointAry);

            //g.FillPath(penPink.Brush, gPath);
            g.DrawPath(penPink, gPath);

            gPath.Reset();
            harfPointAry = BuildSkipPointArray(halfAngle, multiPointAry, start: 1);
            gPath.AddLines(harfPointAry);

            //gPath.FillMode = FillMode.Winding;
            //g.FillPath(penPink.Brush, gPath);
            g.DrawPath(penPink, gPath);
        }//AlgoEvenStar()

        private PointF[] BuildSkipPointArray(
            int num, PointF[] multiPointAry, int start = 0)
        {
            PointF[] starPointAry = new PointF[num + 1];
            int index = start;
            
            for (int i = 0; i < starPointAry.Length; i++)
            {
                starPointAry[i] = multiPointAry[index];
                index = (index + 2) % NUM;
            }//for

            return starPointAry;
        }//BuildSkipPointArray()
    }//class
}

/*
//==== Test Print ====
//Console.WriteLine($"NUM: {NUM}");
//Console.WriteLine($"start: {start}");
//Console.WriteLine($"{i}: {index}");

//---- AlgoOddStar(), BuildSkipPointArray() ----
NUM: 5
start: 0
0: 0
1: 2
2: 4
3: 1
4: 3
5: 0

//---- AlgoEvenStar(), BuildSkipPointArray() ----
NUM: 8
start: 0
0: 0
1: 2
2: 4
3: 6
4: 0
NUM: 8
start: 1
0: 1
1: 3
2: 5
3: 7
4: 1
 */
