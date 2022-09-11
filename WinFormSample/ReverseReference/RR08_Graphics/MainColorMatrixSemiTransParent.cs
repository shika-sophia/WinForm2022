/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR08_Graphics
 *@class MainColorMatrixSemiTransParent.cs
 *@class   └ new FormColorMatrixSemiTransParent() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content RR[371] p625 ColorMatrix SemiTransParent
 *         画像を半透明にして描画
 *         semitransparent [英]: 半透明な
 *         translucent  [英]: 半透明な, 透明感
 *         translucency [英]: 透明度, 透明性, 透過性
 *         matrix [英]: 行列
 *         
 *@subject Graphics
 *         void  graphics.DrawImage(
 *                 Image, Rectangle destRect, Rectangle srcRect, GraphicsUnit)
 *                   └ 引数 destRect: 描画する位置とサイズを四角形で指定
 *                     引数 srcRect:  描画するImageの部分画像の範囲を四角形で指定
 *                 ※ RectangleFも可、int, float, Point[], PointF[]に置換するオーバーロードあり
 *                 ※ 元画像　srcRect -> 表示領域 destRectの比率で自動的に拡大縮小
 *                 (= g.DrawImageに SizeModeは作用しないが、PictrueBoxSizeMode.StretchImage と同様の結果)
 *                 
 *         void  graphics.DrawImage(
 *                 Image, Rectangle, int x, int y, int width, int height,
 *                 GraphicsUnit, ImageAttributes)
 *                   └ enum GraphicsUnits =>〔KT07_Graphics/MainDrawImageSample.cs〕
 *                 ※ RectangleF不可, floatも可
 *                 
 *         [×] System.NotImplementedException: 実装されていません。(VSのコンパイルは通る)
 *         void  graphics.DrawImage(
 *                 Image, PointF[] destPoints, RectangleF srcRect,
 *                 GraphicsUnit, ImageAttributes)
 *                 ※ Point[]不可, Rectangle不可
 *         ※表示位置の PointF[]が利用できないため、倍率による拡大縮小ができない
 *         
 *@subject ImageAttributes : ICloneable, IDisposable
 *            -- System.Drawing.Imaging.
 *         ImageAttributes   new ImageAttributes()
 *         
 *         void  imageAttr.SetColorMatrix(ColorMatrix)          色調変更のColorMatrixを設定
 *         void  imageAttr.SetColorMatrix(ColorMatrix, [ColorMatrixFlag, [ColorAdjustType]])
 *         void  imageAttr.ClearColorMatrix([ColorAdjustType])  指定した型のColorMatrixを削除
 *           └ enum  ColorMatrixFlag
 *             {
 *                 Default = 0,   //すべてのカラー値 (灰色の網かけを含む) が同じカラー調整行列によって調整
 *                 SkipGrays = 1, //色はすべて調整、灰色の網かけは調整されません。 灰色の網かけは、赤、緑、青の各要素の値が同じである色。
 *                 AltGrays = 2   //灰色の網かけのみ調整
 *             }
 *             
 *           └ enum ColorAdjustType      色調整する型を選択。部分的な色調整ができる
 *             {
 *                 Default = 0, //独自の色の調整情報がないすべての GDI+ オブジェクトにより使用される色の調整情報
 *                 Bitmap = 1,  //Bitmap オブジェクトの色の調整情報
 *                 Brush = 2,   //Brush オブジェクトの色の調整情報
 *                 Pen = 3,     //Pen オブジェクトの色の調整情報
 *                 Text = 4,    //テキストの色の調整情報
 *                 Count = 5,   //指定した型の数
 *                 Any = 6      //指定した型の数
 *             }
 *         
 *         void  bitmap.MakeTransparent(Color)                透過色を指定して、その色を透明にする
 *         void  imageAttr.SetColorKey(                       透過色の範囲や型を指定して、その色を透明にする
 *                 Color colorLow, Color colorHigh, [ColorAdjustType])    
 *         void  imageAttr.ClearColorKey([ColorAdjustType]);  型を指定した ColorKeyを削除
 *         
 *         void  imageAttr.SetNoOp([ColorAdjustType type])    指定した型の 色調整を停止。SetOnOp()呼出前に戻す
 *         void  imageAttr.ClearNoOp([ColorAdjustType type])  SetOnOp()で停止していた 色調整を元に戻す
 *
 *         void  imageAttr.SetBrushRemapTable(ColorMap[] map)            Brush型のカラー リマップ テーブルを設定
 *           └ class ColorMap -- System.Drawing.Imaging.
 *             ColorMap   new ColorMap()
 *             Color      colorMap.OldColor { get; set; }
 *             Color      colorMap.NewColor { get; set; }
 *         void  imageAttr.ClearBrushRemapTable()                
 *         void  imageAttr.SetRemapTable(ColorMap[], [ColorAdjustType])  指定した型のカラー リマップ テーブルを設定
 *         void  imageAttr.ClearRemapTable([ColorAdjustType type])
 *
 *         void  imageAttr.SetWrapMode(WrapMode mode, Color color)
 *           └ enum WrapMode  -- System.Drawing.Drawing2D.  塗りつぶし範囲より Brush範囲が小さい場合の並べ方
 *             {
 *                 Tile = 0,        //グラデーションまたはテクスチャを並べて表示
 *                 TileFlipX = 1,   //水平方向に反転し、並べて表示
 *                 TileFlipY = 2,   //垂直方向に反転し、並べて表示
 *                 TileFlipXY = 3,  //水平および垂直方向に反転し、並べて表示
 *                 Clamp = 4        //並べて表示されません
 *             }
 *             
 *@subject ◆ColorMatrix class -- System.Drawing.Imaging.
 *         ・RGBAW空間の 5 × 5 行列を表すクラス
 *         ・R: Red / G: Green / B: Blue / A: Alpha 透明度 / W: ?? を表す
 *         ・ImageAttributesクラスの コンストラクタの引数やプロパティの値として利用
 *         
 *         ＊透明度の変更 Matrix33 = XXf;  
 *           XX は [0 ～ 1]の float値   0f:透明, 0.xf: 半透明, 1f:不透明
 *           
 *         ＊色調の変更 RR[372] p626
 *           変更後の R = r * Matrix00 + g * Matrix01 + b * Matrix02
 *           変更後の G = r * Matrix10 + g * Matrix11 + b * Matrix12
 *           変更後の B = r * Matrix20 + g * Matrix21 + b * Matrix22
 *         
 *         ColorMatrix   new ColorMatrix();
 *         ColorMatrix   new ColorMatrix(float[][] newColorMatrix)
 *         
 *         float colorMatrix[int row, int column]
 *         float this[int row, int column]
 *         float colorMatrix.MatrixXX       XX: row-column, 0-4
 *         例: colorMatrix.Matrix33 = 0.5f; とすると半透明の画像になる
 *         
 *@see ImageColorMatrixSemiTransParent.jpg
 *@copyTo ~/WinFormSample/GraphicsReference.txt 
 *@copyTo ~/WinFormSample/ColorReference.txt 
 *@author shika
 *@date 2022-08-29
 */
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR08_Graphics
{
    class MainColorMatrixSemiTransParent
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormColorMatrixSemiTransParent()");

            Application.EnableVisualStyles();
            Application.Run(new FormColorMatrixSemiTransParent());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormColorMatrixSemiTransParent : Form
    {
        private readonly TableLayoutPanel table;
        private readonly PictureBox pic;
        private readonly Button btnOrigin;
        private readonly Button btnSemi;
        private readonly Graphics g;
        private Image image;
        private Rectangle rect;

        public FormColorMatrixSemiTransParent()
        {
            this.Text = "FormColorMatrixSemiTransParent";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;

            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 85f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));

            image = new Bitmap("../../Image/SF101.JPG");

            pic = new PictureBox()
            {
                ClientSize = image.Size,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
            };
            g = pic.CreateGraphics();
            rect = new Rectangle(pic.Location, pic.Size);
            table.Controls.Add(pic, 0, 0);
            table.SetColumnSpan(pic, 2);

            btnOrigin = new Button()
            {
                Text = "Original",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            btnOrigin.Click += new EventHandler(btn_Click);
            table.Controls.Add(btnOrigin, 0, 1);

            btnSemi = new Button()
            {
                Text = "Semitransparent",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            btnSemi.Click += new EventHandler(btn_Click);
            table.Controls.Add(btnSemi, 1, 1);

            this.Controls.Add(table);
        }//constructor

        private void btn_Click(object sender, EventArgs e)
        {
            g.Clear(SystemColors.Window);
            string btnText = (sender as Button).Text;

            var cm = new ColorMatrix();
            cm.Matrix00 = 1f;
            cm.Matrix11 = 1f;
            cm.Matrix22 = 1f;
            //cm.Matrix33 = xx;  -> switch〔below〕
            cm.Matrix44 = 1f;

            switch (btnText)
            {
                case "Original":
                    cm.Matrix33 = 1f;
                    break;
                case "Semitransparent":
                    cm.Matrix33 = 0.5f;
                    break;
            }//switch

            var imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(cm);

            g.DrawImage(image, rect, 0, 0, image.Width, image.Height,
                GraphicsUnit.Pixel, imageAttr);
        }//btn_Click()
    }//class
}
