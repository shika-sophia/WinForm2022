/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR08_Graphics
 *@class MainMatrixRotateSample.cs
 *@class   └ new FormMatrixRotateSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content RR[374][378] Matrixクラス RotateAt(), Scale() / p630, p635
 *         matrix    [英]: 行列
 *         geometric [英]: 幾何学
 *         geometry transformation [数]: 
 *             幾何学的変換, ジオメトリック変換
 *             3Dグラフィックスの処理
 *             多角形の座標データを、描画(表示)のためのデータに変換すること
 *             
 *@subject Graphics
 *         Matrix   greaphics.Transform { get; set; }   図形の変換をする行列(matrix)を取得/設定
 *         例 matrix.RotateAt(...);など変更のたびに
 *            greaphics.Transform = matrix; と代入しないと変更を反映しない
 *        
 *         void     graphics.RotateTransform(float angle, [MatrixOrder])           Transformで登録した Matrixを指定角度と順序で回転
 *         void     graphics.ScaleTransform(float sx, float sy, [MatrixOrder])     Transformで登録した Matrixを指定倍率と順序で拡大縮小
 *         void     graphics.TranslateTransform(float dx, float dy, [MatrixOrder]) Transformで登録した Matrixを指定幅と順序で平行移動
 *         void     graphics.ResetTransform()                                      変換行列 Matrixを削除
 *         
 *@subject ◆Matrix : MarshalByRefObject, IDisposable
 *            -- System.Drawing.Drawing2D.
 *         ・図形の回転(360°内で時計回りの角度を指定可), 反転, 拡大縮小を行う変換行列を表すクラス
 *         ・void     image.RotateFlip(RotateFlipType)   90°単位の回転, 反転
 *         ・複数回の変換を行うと、内部的に 行列を乗算している様子
 *         
 *         Matrix   new Matrix()
 *         Matrix   new Matrix(Rectangle, Point[])
 *            変換する四角形と、ジオメトリック変換〔上記〕の変換先の平行四辺形
 *            ※ RectangleF, PointF[], float可
 *         
 *         void  matrix.Rotate(float angle, [MatrixOrder])             角度を指定して原点(0, 0)を中心とする回転
 *         void  matrix.RotateAt(float angle, PointF, [MatrixOrder])   角度と中心点を指定して回転
 *           └ enum MatrixOrder -- System.Drawing.Drawing2D.
 *             {
 *                 Prepend = 0,  //新しい操作が、古い操作の前に適用
 *                 Append = 1    //新しい操作が、古い操作の後に適用
 *             }
 *         bool  matrix.IsInvertible { get; }    反転可能か
 *         void  matrix.Invert()                 反転可能なら反転
 *         void  matrix.Translate(float offsetX, float offsetY, [MatrixOrder]) x方向, y方向に平行移動
 *         void  matrix.Shear(float shearX, float shearY, [MatrixOrder])  画像原点(0, 0)を基準に x方向, y方向の傾斜率を指定して傾斜
 *         void  matrix.Scale(float scaleX, float scaleY, [MatrixOrder])  x方向, y方向の拡大率を指定して拡大縮小
 *         void  matrix.Multiply(Matrix, [MatrixOrder])                   対象オブジェクトの行列に、引数の行列を乗算
 *         float[]  matrix.Elements { get; }   Matrixの要素である float[]を取得
 *         bool     matrix.IsIdentity { get; } 単位行列であるか
 *         void     matrix.Reset()             単位行列の要素を持つようにリセット
 *                   ※単位行列 unit matrix: 
 *                     https://kotobank.jp/word/%E5%8D%98%E4%BD%8D%E8%A1%8C%E5%88%97-94854
 *                     ・n 次の正方行列 A＝(aij) において，
 *                       主対角線上の要素がすべて 1で，その他はすべて 0であるもの
 *                     ・行列は乗算の順によって解が異なるが
 *                       単位行列を E とすれば AE＝EA＝A なる関係が成り立つ
 *                       | 1 0 |   | 1 0 0 |
 *                       | 0 1 | , | 0 1 0 | , ... のような要素になっている。
 *                                 | 0 0 1 |
 *         
 *@NOTE【考察】回転の中心点と、描画の原点
 *      ・PictureBoxSizeMode.CenterImage を設定しているので、
 *        ここでは PictureBox.ClientSizeの中心点と、画像の中心点は一致する
 *      ・回転の中心点を 上記に設定
 *      
 *        centerPoint = new Point(
 *             pic.ClientSize.Width / 2, pic.ClientSize.Height / 2);
 *
 *      ・画像描画の原点を指定する必要があるので
 *        PictureBoxの中心点だと、画像が ずれていく。
 *        PictureBoxの中心点から、画像の横幅 / 2 と高さ /2 の差を 描画の原点とする
 *        
 *        g.DrawImage(image, new Point(                     
 *           (pic.ClientSize.Width - image.Width) / 2,
 *           (pic.ClientSize.Height - image.Height) / 2));
 *           
 *@see ImageMatrixRotateSample.jpg
 *@see MainMatrixScaleTranslate.cs
 *@copyTo ~/WinFormSample/WinFormSample_analysis.txt
 *@author shika
 *@date 2022-09-01
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR08_Graphics
{
    class MainMatrixRotateSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormMatrixRotateSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormMatrixRotateSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMatrixRotateSample : Form
    {
        private readonly TableLayoutPanel table;
        private readonly PictureBox pic;
        private readonly Button btnOrigin;
        private readonly Button btnRotateLeft;
        private readonly Button btnRotateRight;
        private readonly Graphics g;
        private readonly Image image;
        private readonly Matrix mx = new Matrix();
        private readonly Point centerPoint;        //PictureBoxの中心点

        public FormMatrixRotateSample()
        {
            this.Text = "FormMatrixRotateSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(640, 480);
            this.BackColor = SystemColors.Window;

            table = new TableLayoutPanel()
            {
                ColumnCount = 3,
                RowCount = 2,
                Dock = DockStyle.Fill,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 85f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));

            image = new Bitmap(
                Image.FromFile("../../Image/NaziElephant.jpg"),
                new Size(250, 150));

            pic = new PictureBox()
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.CenterImage,
                ClientSize = new Size(
                    this.ClientSize.Width - 5,
                    (int)(this.ClientSize.Height * 0.85)),
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
            };
            g = pic.CreateGraphics();
            centerPoint = new Point(
                pic.ClientSize.Width / 2, pic.ClientSize.Height / 2);
            table.Controls.Add(pic, 0, 0);
            table.SetColumnSpan(pic, 3);

            btnOrigin = new Button()
            {
                Text = "Original",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            btnOrigin.Click += new EventHandler(btnOrigin_Click);
            table.Controls.Add(btnOrigin, 0, 1);

            btnRotateLeft = new Button()
            {
                Text = "Rotate Left 45° ",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            btnRotateLeft.Click += new EventHandler(btnRotateLeft_Click);
            table.Controls.Add(btnRotateLeft, 1, 1);

            btnRotateRight = new Button()
            {
                Text = "Rotate Right 45° ",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            btnRotateRight.Click += new EventHandler(btnRotateRight_Click);
            table.Controls.Add(btnRotateRight, 2, 1);
            
            this.Controls.Add(table);
        }//constructor

        private void btnOrigin_Click(object sender, EventArgs e)
        {
            g.Clear(SystemColors.Window);
            mx.Reset();
            g.Transform = mx;
            pic.Invalidate();
        }//btnOrigin_Click()

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            mx.RotateAt(315, centerPoint);   //360 - 45 = 315
            g.Transform = mx;
            GraphicsDraw();  //self defined 〔below〕
        }//btnRotateLeft_Click()

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            mx.RotateAt(45, centerPoint);
            g.Transform = mx;
            GraphicsDraw();  //self defined 〔below〕
        }//btnRotateRight_Click()

        private void GraphicsDraw()   //self defined
        {
            g.Clear(SystemColors.Window);
            g.DrawImage(image, new Point(
                (pic.ClientSize.Width - image.Width) / 2,
                (pic.ClientSize.Height - image.Height) / 2));
        }
    }//class
}
