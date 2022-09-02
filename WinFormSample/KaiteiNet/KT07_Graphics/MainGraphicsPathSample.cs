/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainGraphicsPathSample.cs
 *@class   └ new FormGraphicsPathSample) : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics / Clipping (= Trimming)
 *         指定領域のみ画像を表示する
 *
 *@subject Graphics
 *         void        graphics.DrawPath(Pen, GraphicsPath)
 *         Region      graphics.Clip { get; set;}           Graphicsオブジェクトの描画領域を設定
 *           └ Region      new Region(GraphicPath)
 *         void        graphics.SetClip(Graphics, [CombineMode])
 *         void        graphics.SetClip(Region, [CombineMode])       ※ Rectangle, RectangleF 可
 *         void        graphics.SetClip(GraphicsPath, [CombineMode])
 *           └ enum CombineMode  クリッピング領域の結合方法を指定
 *                     -- System.Drawing.Drawing2D
 *             {
 *                 Replace = 0,    //別のクリッピング領域で置換
 *                 Intersect = 1,  //2つのクリッピング領域は、積集合
 *                 Union = 2,      //2つのクリッピング領域は、両方の和集合
 *                 Xor = 3,        //2つのクリッピング領域は、一方または他方で囲まれた領域だけ
 *                 Exclude = 4,    //既存の領域から新しい領域の差集合で、既存の領域を置換
 *                 Complement = 5  //新しい領域から既存の領域の差集合で、既存の領域を置換
 *             }
 *             
 *         bool        graphics.IsClipEmpty { get; }        Clipプロパティが空か
 *         RectangleF  graphics.ClipBounds { get; }         Clip領域の外接四角形
 *         RectangleF  graphics.VisibleClipBounds  { get; } 表示されているClip領域の外接四角形
 *         bool        graphics.IsVisibleClipEmpty { get; } 表示されているClip領域が 空か
 *         bool        graphics.IsVisible(Rectangle)        引数領域が 表示Clip部分であるか  ※ RectangleF, Point[], PointF[], int, float 可
 *         void        graphics.TranslateClip(int dx, int dy)  Clip領域を平行移動           ※ float可
 *         void        graphics.IntersectClip(Region)       graphicsオブジェクトの既存Clip領域と、引数の領域の交差部分  ※Rectangle, RectangleF
 *         void        graphics.ExcludeClip(Region)         graphicsオブジェクトの既存Clip領域と、引数の領域の差集合    ※Rectangle
 *         void        graphics.ResetClip()                 Clip領域を削除
 *         
 *         ＊配置
 *         void graphics.DrawImage(Image, RectangleF)
 *           └ RectangleF  graphics.VisibleClipBounds
 *               └ Region  　 graphics.Clip
 *                   └ new Region(GraphicPath)
 *                   
 *         例: 
 *         var gPath = new GraphicsPath( ... );
 *         var region = new Region(gPath)
 *         graphics.Clip = region;
 *         graphics.DrawImage(Image, graphics.VisibleClipBounds);
 *         
 *@subject ◆Region : MarshalByRefObject, IDisposable
 *         ・領域を表すオブジェクト
 *         ・集合計算に関するメソッドを持つ
 *         
 *         Region  new Region(GraphicPath)
 *         Region  new Region(Rectangle)   ※RectangleF
 *         void    region.Union(※)        和集合  A or B  ※Rectangle, RectangleF, GraphicPath, Region
 *         void    region.void Exclude(※) 差集合  A - B
 *         void    region.Intersect(※)    積集合  A and B
 *         void    region.Complement(※)   補集合  not A
 *         void    region.Xor(※)          排他論理和  A xor B (和集合のうち交差部分を除外)
 *         bool    region.Equals(Region region, Graphics g) 一致するか
 *         bool    region.IsEmpty(Graphics g)               空集合であるか
 *         bool    region.IsInfinite(Graphics g)            無限要素であるか
 *         void    region.MakeEmpty()                       Regionを初期化し、空集合にする
 *         
 *@subject ◆GraphicsPath : MarshalByRefObject, ICloneable, IDisposable
 *         ・System.Drawing.Drawing2D 名前空間
 *         ・接続した一連の直線と曲線を表すオブジェクト
 *         ・輪郭に囲まれていれば 必ずRegionになる訳ではない
 *         ・外から偶数個の輪郭を越えて到達できる箇所は，領域外という扱いになります。
 *         
 *         GraphicsPath    new GraphicsPath()
 *         GraphicsPath    new GraphicsPath(FillMode)
 *         GraphicsPath    new GraphicsPath(Point[])   ※PointF[]も可
 *         GraphicsPath    new GraphicsPath(Point[], [byte[] types], [FillMode])
 *           引数  FileMode 
 *             └ enum FileMode  -- System.Drawing.Drawing2D.
 *               {
 *                   Alternate = 0, //交互塗りつぶしモード
 *                   Winding = 1    //全域塗りつぶしモード
 *               }
 *               
 *           引数 byte[] types
 *             └ enum PathPointType -- System.Drawing.Drawing2D.
 *               {
 *                   Start = 0,         //GraphicsPathオブジェクトの始点
 *                   Line = 1,          //線分
 *                   Bezier = 3,        //既定のベジエ曲線
 *                   Bezier3 = 3,       //3次ベジエ曲線
 *                   PathTypeMask = 7,  //マスク ポイント
 *                   DashMode = 16,     //対応するセグメントはダッシュで描画
 *                   PathMarker = 32,   //パス マーカー
 *                   CloseSubpath = 128 //サブパスの終了点
 *               }
 *               
 *               FillMode  graphicsPath.FillMode { get; set; }
 *               PointF[]  graphicsPath.PathPoints { get; }
 *               byte[]    graphicsPath.PathTypes  { get; }
 *               int       graphicsPath.PointCount { get; }   PathTypesの要素数
 *               
 *               void      graphicsPath.AddLine(Point, Point)  ※int, PointF, floatも可
 *               void      graphicsPath.AddLines(Point[])      ※PointF[]も可
 *               void      graphicsPath.AddArc(Rectangle rect, float startAngle, float sweepAngle)  楕円の弧 ※RectangleFも可
 *               void      graphicsPath.AddBeziers(params Point[] points)　                         ３次ベジエ曲線 ※PointF[]
 *               void      graphicsPath.AddCurve(Point[], [int offset, int numberOfSegments], [float tension]) 曲線 ※PointF[]
 *               void      graphicsPath.AddClosedCurve(Point[], [float tension])                    閉じた曲線  ※PointF[]
 *               void      graphicsPath.AddEllipse(Rectangle rect)  ※ RectangleF, floatも可
 *               void      graphicsPath.AddEllipse(int x, int y, int width, int height);
 *               void      graphicsPath.AddPie(Rectangle, float startAngle, float sweepAngle)       扇形 ※RectangleF, float
 *               void      graphicsPath.AddPie(int x, int y, int width, int height, float startAngle, float sweepAngle)
 *               void      graphicsPath.AddPolygon(Point[])              多角形  ※PointF[]
 *               void      graphicsPath.AddRectangle(Rectangle)          四角形  ※RectangleF
 *               void      graphicsPath.AddRectangles(Rectangle[] rect)   四角形の配列  ※RectangleF[]
 *               void AddString(string, FontFamily, int style, float emSize, Point origin, StringFormat)          ※PointF
 *               void AddString(string, FontFamily, int style, float emSize, Rectangle layoutRect, StringFormat); ※RectangleF
 *               void      graphicsPath.AddPath(GraphicsPath, bool connect)  
 *                           引数 connect: 追加パスの最初の図形が、このパスの最後の図形の一部になるか
 *               void      graphicsPath.SetMarkers();      マーカーを設定
 *               void      graphicsPath.ClearMarkers()     すべてのマーカーを消去
 *               void      graphicsPath.CloseAllFigures()  開いている すべての図形を閉じて、新しい図形を開始。開いている図形の開始点と終了点を接続することで閉じた図形になる
 *               void      graphicsPath.CloseFigures()     開いている 現在の図形を閉じて、新しい図形を開始。開いている図形の開始点と終了点を接続することで閉じた図形になる
 *               void      graphicsPath.Reset()            PathPoints, PathTypes配列を空にする。FileMode.Alternateに設定。
 *               void      graphicsPath.Dispose()          このオブジェクトで利用している すべてのリソースを解放
 *               
 *@see ImageGraphicsPathSample.jpg
 *@copyTo ~/WinformSample/GraphicsReference.txt
 *@author shika
 *@date 2022-08-25
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainGraphicsPathSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormGraphicsPathSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormGraphicsPathSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormGraphicsPathSample : Form
    {
        private Bitmap bitmap;

        public FormGraphicsPathSample()
        {
            this.Text = "FormGraphicsPathSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(800, 400);
            this.BackColor = SystemColors.Window;

            bitmap = new Bitmap(
                "../../../../SelfAspNet/SelfAspNet/Image/S0003.jpg");

            //this.Controls.AddRange(new Control[]
            //{

            //});
        }//constructor

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            var gPath = new GraphicsPath();
            g.DrawImage(bitmap, 360, 10, 400, 300); //原画

            Point[] pointAry = new Point[]   // ◇型 Point[]
            {
                new Point(150, 0), new Point(0, 150),
                new Point(150, 300), new Point(300, 150),
            };

            gPath.AddPolygon(pointAry); // ◇
            gPath.AddRectangle(new Rectangle(50, 50, 200, 200)); // □
            
            Region region = new Region(gPath);
            g.Clip = region;
            g.DrawImage(bitmap, g.VisibleClipBounds);

            gPath.Dispose();
            region.Dispose();
            g.Dispose();
        }//OnPaint()
    }//class
}
