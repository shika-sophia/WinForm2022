/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR08_Graphics
 *@class MainPathGradientBrushSample.cs
 *@class   └ new FormPathGradientBrushSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content RR[370] p624 / PathGradientBrush
 *@subject GraphicsPath =>〔KT07_Graphics\MainGraphicsPathSample.cs〕
 *         void  graphics.Clear(Color)
 *                    └ 引数 Color  SystemColors.Window
 *                Graphicsオブジェクトに以前に描画したものは残り続けるので、
 *                画面を切り替える場合は 別の Graphicsオブジェクトに描画するか、
 *                同一Graphicsなら Clear()して使う
 *                
 *@subject LinearGradientBrush =>〔KT07_Graphics\MainBrushAppendix.cs〕
 *@subject ◆PathGradientBrush : Brush -- System.Drawing.Drawing2D
 *         ・コンストラクタで 塗りつぶし範囲を指定
 *         ・色情報は コンストラクタに入っていないので、
 *           CenterColor, SurroundColorsを追加で指定する必要がある
 *           (コンストラクタで生成したオブジェクトのままだと何も描画されない)
 *           
 *         PathGradientBrush     new PathGradientBrush(GraphicsPath, [WrapMode])
 *         PathGradientBrush     new PathGradientBrush(Point[], [WrapMode])  ※Point[]も可
 *           └ enum WrapMode  -- System.Drawing.Drawing2D.  塗りつぶし範囲より Brush範囲が小さい場合の並べ方
 *             {
 *                 Tile = 0,        //グラデーションまたはテクスチャを並べて表示
 *                 TileFlipX = 1,   //水平方向に反転し、並べて表示
 *                 TileFlipY = 2,   //垂直方向に反転し、並べて表示
 *                 TileFlipXY = 3,  //水平および垂直方向に反転し、並べて表示
 *                 Clamp = 4        //並べて表示されません
 *             }
 *             
 *         Color       pathGradientBrush.CenterColor { get; set; }    塗りつぶし範囲の中央の色  
 *         Color[]     pathGradientBrush.SurroundColors { get; set; } 塗りつぶし範囲の path周辺の色
 *         RectangleF  pathGradientBrush.Rectangle { get; }           塗りつぶしpathに外接する四角形
 *         PointF      pathGradientBrush.CenterPoint { get; set; }    グラデーションの中央点
 *         WrapMode    pathGradientBrush.WrapMode { get; set; }       塗りつぶし範囲より Brush範囲が小さい場合の並べ方
 *
 *@NOTE【考察】PictureBox.Size と Dock, SizeMode
 *      ・BorderStyle, Imageは Dock, SizeModeで拡大されるが 
 *        Graphicsの描画は Dock, SizeModeで拡大しない
 *      ・DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoomなどは
 *        pictureBox.Imageで Imageオブジェクトを登録した場合のみ有効
 *      ・pictrureBox.CreateGraphics()に g.DrawXxxx()/g.FillXxxx()した
 *        グラフィックスには Dock, SizeModeの値が影響しない
 *        (サイズが小さいまま表示される)
 *      ・Graphicsを PictureBoxの枠線いっぱいに表示するには
 *        pictureBox.Size / ClientSizeを 正しく指定する必要がある
 *        (pic.Size / pic.ClientSizeは DockStyle.Fill, SizeModeで拡大する前の大きさに設定されている)
 *      ・Size / ClientSizeを定数指定することは望ましくないので、
 *        PictureBoxの親コントロールである Form / Panelのサイズから取得すべき
 *      ・Panelである TableLayoutPanelも同様に Dock, ColumnStyles, RowStylesで
 *        拡大・縮小する前のサイズが Size, Width, Heightの値になっている
 *        
 *       ＊例
 *      ・form.ClientSize.Width - 5: 親Formの横幅から、PictureBoxの枠線分の 5pxを除いた横幅
 *      ・(int)(form.ClientSize.Height * 0.83): 親Formの高さの 83%の高さ
 *        intキャストは pic.Sizeが int指定なので要キャスト
 *        intキャストなのに、* 0.85 -> * 0.83などの変更も ちゃんとサイズに反映する
 *        (本来の intキャストだと、どちらの値も 0になるはずだが、
 *         元の 0.85などの値がカプセル化されて、型のみ intになっている様子)
 *      ・間に Bitmapオブジェクトを入れて
 *        pic.Image = new Bitmap(pic.Width, pic.Height); として、
 *        pic.CreateGraphics().DrawXxxx()で描いても、Dock, SizeModeは適用されない
 *        
 *@see ImagePathGradientBrushSample.jpg
 *@copyTo ~/WinFormSample/GraphicsRefernce.txt
 *@copyTo ~/WinFormSample/ColorRefernce.txt
 *@author shika
 *@date 2022-08-28
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR08_Graphics
{
    class MainPathGradientBrushSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormPathGradientBrushSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormPathGradientBrushSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormPathGradientBrushSample : Form
    {
        private readonly TableLayoutPanel table;
        private readonly PictureBox pic;
        private readonly Button btnLinear;
        private readonly Button btnPath;
        private readonly Graphics graph;
        private readonly Rectangle rect;

        public FormPathGradientBrushSample()
        {
            this.Text = "FormPathGradientBrushSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;
            
            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 85f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));

            pic = new PictureBox()
            {
                Size = new Size(
                    this.ClientSize.Width - 5, 
                    (int)(this.ClientSize.Height * 0.83)),
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
            };
            graph = pic.CreateGraphics();
            rect = new Rectangle(pic.Location, pic.ClientSize);
            
            table.Controls.Add(pic, 0, 0);
            table.SetColumnSpan(pic, 2);

            btnLinear = new Button()
            {
                Text = "Linear Brush",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            btnLinear.Click += new EventHandler(btnLinear_Click);
            table.Controls.Add(btnLinear, 0, 1);

            btnPath = new Button()
            {
                Text = "Path Brush",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            btnPath.Click += new EventHandler(btnPath_Click);
            table.Controls.Add(btnPath, 1, 1);

            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void btnLinear_Click(object sender, EventArgs e)
        {
            graph.Clear(SystemColors.Window);

            Brush linearBrush = new LinearGradientBrush(
                rect, Color.DeepPink, Color.White,
                LinearGradientMode.ForwardDiagonal);
            graph.FillRectangle(linearBrush, rect);
            linearBrush.Dispose();
        }//btnLinear_Click()

        private void btnPath_Click(object sender, EventArgs e)
        {
            graph.Clear(SystemColors.Window);
            
            var gPath = new GraphicsPath();
            gPath.AddEllipse(rect);

            PathGradientBrush pathBrush = new PathGradientBrush(gPath);
            pathBrush.CenterColor = Color.DeepPink;
            pathBrush.SurroundColors = new Color[] { Color.White, };
            pathBrush.WrapMode = WrapMode.Clamp;

            graph.FillRectangle(pathBrush, rect);
            pathBrush.Dispose();
        }//btnPath_Click()
    }//class
}
