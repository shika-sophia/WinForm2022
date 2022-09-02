/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainPenDrawLineSample.cs
 *@class   └ new FormPenDrawLineSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics | RR[5] p31, RR[266] p616
 *@subject ◆Graphics : MarshalByRefObject, IDisposable, IDeviceContext
 *             -- System.Drawing
 *         ・GDI+ (Graphics Device Interface): C言語?で記述されたグラフィックス機能を内部的に呼出
 *         ・描画キャンバスのような存在
 *         ・グラフィックス処理を行うためには、
 *           描画対象の Form, Controlに関連付けた Graphicsオブジェクトを取得する必要がある
 *         ・Controlは PictureBoxなど, Bitmapクラスも可
 *         ・描画対象に結び付けられた Graphics オブジェクトは，
 *           その描画対象の Paint イベントハンドラの引数 PaintEventArgs e から取得可。
 *         ・PaintEventHandlerは control.Paint += new PaintEventHandler(...); が必要
 *         ・OnPaint()を overrideする場合は 「+=」で追加する必要はないが、
 *           base.OnPaint(e);で基底クラスのイベントハンドラも呼び出す必要がある。
 *         
 *         PaintEventHandler   control.Paint
 *           └ delegate void Paint(object sender, PaintEventArgs e)
 *                ||
 *         protected vertural void control.OnPaint(PaintEventArgs e)   virtualを overrideして利用
 *           
 *         PaintEventArgsクラス e -- System.Windows.Forms. 再描画イベントに関するデータを提供
 *         Graphics   e.Graphics
 *         
 *         Graphics   control.CreateGraphics()   // Paint イベントハンドラ以外の場所で Graphics オブジェクトを取得したい場合に利用
 *                                               // CreateGraphics()で生成した描画は、Form/Controlが再描画される際に消えてしまう
 *                                               // => PaintEventArgsから Graphicsオブジェクトを取得すべき
 *                                               // => Bitmapオブジェクトに描画して保存する方法もある
 *
 *         Graphics   Graphics.FromImage(Image)  // Bitmapオブジェクトを生成して、そこに描画
 *                      └ 引数 Image <- new Bitmap(pictureBox.Width, pictureBox.Height)
 *         
 *         [×] new Graphics() の定義なく不可
 *         
 *         float      graphics.DpiX { get; }            X方向の DPI(解像度)
 *         float      graphics.DpiY { get; }            Y方向の DPI(解像度)
 *         Matrix     graphics.Transform { get; set; }  図形変形の行列を登録
 *                    => 〔RR08_Graphics/MainMatrixRotateSample.cs〕
 *         
 *         Region     graphics.Clip { get; set; }       描画の部分取得
 *                    => 〔KT07_Graphics/MainGraphicsPathSample.cs〕
 *         
 *         SmoothingMode  graphics.SmoothingMode { get; set; }
 *           └ enum SmoothingMode  直線、曲線、塗りつぶし領域の境界線に、スムージング (アンチエイリアス処理) を適用するか
 *                    -- System.Drawing.Drawing2D.
 *             {
 *                 Invalid = -1,    //無効なモード
 *                 Default = 0,     //アンチエイリアス処理しない
 *                 HighSpeed = 1,   //アンチエイリアス処理しない
 *                 HighQuality = 2, //アンチエイリアス処理されたレタリングを指定
 *                 None = 3,        //アンチエイリアス処理しない
 *                 AntiAlias = 4    //アンチエイリアス処理されたレタリングを指定
 *             }
 *             
 *         void       graphics.Flush([FlushIntention])   保留中の Graphics操作を強制実行
 *           └ enum FlushIntention  -- System.Drawing.Drawing2D
 *             {
 *                Flush = 0,  //Graphics操作の Stackをすぐに実行し、制御は すぐに戻す
 *                Sync = 1    //できる限り早く実行し、制御は処理完了まで同期的に待機してから戻す
 *             }
 *             
 *         void       graphics.Dispose()         // CreateGraphics()で生成した Graphicsオブジェクトは 使い終わる度に 破棄する
 *           
 *         ＊直線の描画
 *         void  graphics.DrawLine(Pen, Point p1, Point p2)             始点 p1 から 終点 p2 の直線を描画
 *         void  graphics.DrawLine(Pen, int x1, int y1, int x2, int y2) 始点 (x1, y1) から 終点 (x2, y2) の直線を描画
 *         
 *@subject ◆Penクラス : MarshalByRefObject, ICloneable, IDisposable
 *             -- System.Drawing.
 *         ・Pen クラスを用いて，特定の色と太さを持つ，仮想のペンを定義
 *         ・自己定義した Pen オブジェクトは，使い終る度に Dispose メソッドで破棄する
 *         
 *         Pen   　new Pen(Color)
 *         Pen     new Pen(Color, float width)
 *         Pen   　new Pen(Brush)
 *         Pen     new Pen(Bursh, float width)
 *         Pen     Pens.Xxxx      標準色 Xxxxを指定 Pens struct
 *         
 *         Color      pen.Color
 *         Brush      pen.Brush
 *         float      pen.Width
 *         
 *         DashStyle  pen.DashStyle  
 *           └ enum DashStyle  -- System.Drawing.Drawing2D.
 *             {
 *                Solid = 0,      //実線
 *                Dash = 1,       //ダッシュ「―」で構成される直線
 *                Dot = 2,        //ドット「・」で構成される直線
 *                DashDot = 3,    //ダッシュとドットの繰り返しパターン
 *                DashDotDot = 4, //ダッシュと 2つのドットの繰り返しパターン
 *                Custom = 5      //ユーザー定義のカスタム ダッシュ スタイル
 *             }
 *         float[]    pen.DashPattern   DashStyle.Customのとき
 *                                      破線内の代替ダッシュと空白の長さを指定する実数の配列。
 *         float[]    pen.CompoundArray 平行線の複線。0 ～ 1 の値を昇順に並べた配列
 *         void    pen.Dispose()
 *         
 *@subject ◆SystemPens class -- System.Drawing.
 *         ・Windows System32で使用している色による Penオブジェクトを取得
 *
 *         Pen   SystemPens.ActiveBorder  { get; }
 *         Pen   SystemPens.ActiveCaption  { get; }
 *         Pen   SystemPens.ActiveCaptionText  { get; }
 *         Pen   SystemPens.AppWorkspace  { get; }
 *         Pen   SystemPens.ButtonFace  { get; }
 *         Pen   SystemPens.ButtonHighlight  { get; }
 *         Pen   SystemPens.ButtonShadow  { get; }
 *         Pen   SystemPens.Control  { get; }
 *         Pen   SystemPens.ControlText  { get; }
 *         Pen   SystemPens.ControlDark  { get; }
 *         Pen   SystemPens.ControlDarkDark  { get; }
 *         Pen   SystemPens.ControlLight  { get; }
 *         Pen   SystemPens.ControlLightLight  { get; }
 *         Pen   SystemPens.Desktop  { get; }
 *         Pen   SystemPens.GradientActiveCaption  { get; }
 *         Pen   SystemPens.GradientInactiveCaption  { get; }
 *         Pen   SystemPens.GrayText  { get; }
 *         Pen   SystemPens.Highlight  { get; }
 *         Pen   SystemPens.HighlightText  { get; }
 *         Pen   SystemPens.HotTrack  { get; }
 *         Pen   SystemPens.InactiveBorder  { get; }
 *         Pen   SystemPens.InactiveCaption  { get; }
 *         Pen   SystemPens.InactiveCaptionText  { get; }
 *         Pen   SystemPens.Info  { get; }
 *         Pen   SystemPens.InfoText  { get; }
 *         Pen   SystemPens.Menu  { get; }
 *         Pen   SystemPens.MenuBar  { get; }
 *         Pen   SystemPens.MenuHighlight  { get; }
 *         Pen   SystemPens.MenuText  { get; }
 *         Pen   SystemPens.ScrollBar  { get; }
 *         Pen   SystemPens.Window  { get; }
 *         Pen   SystemPens.WindowFrame  { get; }
 *         Pen   SystemPens.WindowText  { get; }
 *         
 *@see ImagePenDrawLineSample.jpg
 *@see 
 *@copyTo ~/WinFormSample/GraphicsReference.txt
 *@author shika
 *@date 2022-08-16
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainPenDrawLineSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormPenDrawLineSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormPenDrawLineSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormPenDrawLineSample : Form
    {
        public FormPenDrawLineSample()
        {
            this.Text = "FormPenDrawLineSample";
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
            Graphics g = e.Graphics;
            
            Pen pen1 = new Pen(Color.Blue, 2);
            g.DrawLine(pen1, 20, 20, 200, 20);
            
            Pen pen2 = new Pen(Color.Red, 2)
            {
                DashStyle = DashStyle.Dot,
            };
            g.DrawLine(pen2, 20, 100, 200, 100);

            pen1.Dispose();
            pen2.Dispose();
            g.Dispose();
        }//OnPaint()
    }//class
}
