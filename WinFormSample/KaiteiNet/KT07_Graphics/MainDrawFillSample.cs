/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainDrawFillSample.cs
 *@class   └ new FormDrawFillSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics / DrawRectangle(), DrawEllipse() | RR[367]-[369] p618-p622
 *         長方形, 楕円の描画
 *         
 *@subject Graphics -- System.Drawing.
 *         ＊長方形 Rectangle: 始点, 横幅, 高さを指定
 *         void   graphics.DrawRectangle(Pen, Rectangle)
 *         void   graphics.DrawRectangle(Pen, int x, int y, int width, int height)
 *         void   graphics.DrawRectangle(Pen, float x, float y, float width, float height)
 *         void   graphics.DrawRectangles(Pen, Rectangle[]);
 *         void   graphics.DrawRectangles(Pen, RectangleF[]);
 *         
 *         ＊多角形 Polygon: 単位の点となる配列を指定
 *         void   graphics.DrawLines(Pen, Point[])    始点と終点は結合せず
 *         void   graphics.DrawLines(Pen, PointF[])
 *         void   graphics.DrawPolygon(Pen, Point[])  始点と終点を結合し、閉じた多角形を描画
 *         void   graphics.DrawPolygon(Pen, PointF[])
 *         
 *         ＊楕円 Ellipse: 四角形を指定して、それに内接する。正方形なら円、長方形なら楕円になる
 *                        傾いた楕円を描画することはできない => 図形を回転 〔Matrixクラス | RR[374] p630〕
 *         void   graphics.DrawEllipse(Pen, Rectangle)
 *         void   graphics.DrawEllipse(Pen, RectangleF)
 *         void   graphics.DrawEllipse(Pen, int x, int y, int width, int height)
 *         void   graphics.DrawEllipse(Pen, float x, float y, float width, float height)
 *         
 *         ＊おうぎ型 Pie: 元になる楕円の外接四角形を指定し、X軸からの始まり角、終辺の角を度単位で指定 
 *         void   graphics.DrawPie(Pen, Rectangle, float startAngle, float sweepAngle)
 *         void   graphics.DrawPie(Pen, RectangleF, float startAngle, float sweepAngle)
 *         void   graphics.DrawPie(Pen, int x, int y, int width, int height, float startAngle, float sweepAngle)
 *         void   graphics.DrawPie(Pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
 *         
 *         ＊長方形 (rectangle) 塗りつぶし
 *         void   graphics.FillRectangle(Brush, Rectangle)
 *         void   graphics.FillRectangle(Brush, RectangleF)
 *         void   graphics.FillRectangle(Brush, int x, int y, int width, int height)
 *         void   graphics.FillRectangle(Brush, float x, float y, float width, float height)
 *         void   graphics.FillRectangles(Brush, Rectangle[])
 *         void   graphics.FillRectangles(Brush, RectangleF[])
 *         
 *         ＊楕円 (ellipse) 塗りつぶし
 *         void   graphics.FillEllipse(Brush, Rectangle)
 *         void   graphics.FillEllipse(Brush, RectangleF)
 *         void   graphics.FillEllipse(Brush, int x, int y, int width, int height)
 *         void   graphics.FillEllipse(Brush, float x, float y, float width, float height)
 *
 *@NOTE【考察】DrawXxxx() と FillXxxx()の順
 *      輪郭をきれいに描画するには、Fill -> Draw の順がいい
 *      (逆だと、輪郭線が消えて塗りつぶしだけが見える)
 *      
 *@subject abstract Brush =>〔~/WinFormSample/ColorRefernce.txt〕
 *         ＊GDI+以外のライブラリを利用する Unmangedリソース
 *         SolidBrushクラス : Brush           単色ブラシ
 *         HatchBrushクラス : Brush           enum HatchStyleで規定の柄で塗りつぶし
 *         TextureBrushクラス : Brush         イメージを指定して塗りつぶし
 *         LinierGradientBrushクラス : Brush  グラデーション
 *           これらのBrushオブジェクトは自動破棄が行われず、アプリケーション実行の間、メモリを占有し続ける。
 *           Brushオブジェクトを大量に利用する場合は、メモリ領域を圧迫する可能性があるので、
 *           利用が終了するたびに Dispose()しておく
 *      
 *@see ImageFillSample.jpg
 *@see 
 *@copyTo ~/WinFormSample/GraphicsRefernce.txt
 *@copyTo ~/WinFormSample/ColorRefernce.txt
 *@author shika
 *@date 2022-08-18
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainDrawFillSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormDrawFillSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormDrawFillSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormDrawFillSample : Form
    {
        public FormDrawFillSample()
        {
            this.Text = "FormDrawFillSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //this.Controls.AddRange(new Control[]
            //{

            //});
        }//constructor

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            Pen pen1 = new Pen(Color.Blue);
            Pen pen2 = new Pen(Color.Red);

            g.DrawRectangle(pen1, 20, 20, 200, 200);
            g.DrawEllipse(pen2, 20, 20, 200, 200);
            //g.FillEllipse(Brushes.Red, 20, 20, 200, 200);

            pen1.Dispose();
            pen2.Dispose();
            g.Dispose();
        }//OnPaint()
    }//class
}
