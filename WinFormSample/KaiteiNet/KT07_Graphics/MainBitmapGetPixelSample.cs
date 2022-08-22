/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet.KT07_Graphics
 *@class MainBitmapGetPixelSample.cs
 *@class   └ new FormBitmapGetPixelSample() : Form
 *@class       └  new DialogShowColor() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics / Bitmap.GetPixel()
 *@subject ◆abstract Image : MarshalByRefObject, ISerializable, ICloneable, IDisposable
 *         [×]      new Image()
 *         Image    Image.FromFile(string fileName)
 *         Image    Image.FromStream(Stream)
 *         
 *         int      image.Width   { get; }
 *         int      image.Height  { get; }
 *         Size     image.Size    { get; }
 *         ImageFormat  image.RawFormat   { get; }
 *         PixelFormat  image.PixelFormat { get; }
 *           └ enum PixelFormat { ... }   カラーのデータ形式
 *           
 *         void     image.RotateFlip(RotateFlipType) 
 *         void     image.Save(string filename, ImageFormat format);
 *         void     image.Save(Stream stream, ImageFormat format);
 *           └ class ImageFormat -- System.Drawing.Imaging.ImageFormat
 *             ImageFormat   new ImageFormat(Guid guid)
 *             
 *             Guid          ImageFormat.Guid { get; }   GUID (Grobal Unique Identifer) グローバル 一意の識別子 ??
 *               └ struct System.Guid 構造体
 *             ImageFormat   ImageFormat.MemoryBmp { get; }   メモリ上のビットマップの形式
 *             ImageFormat   ImageFormat.Bmp  { get; }   ビットマップ (BMP) イメージ形式
 *             ImageFormat   ImageFormat.Emf  { get; }   拡張メタファイル (EMF) イメージ形式
 *             ImageFormat   ImageFormat.Wmf  { get; }   Windows メタファイル (WMF) イメージ形式
 *             ImageFormat   ImageFormat.Gif  { get; }   GIF (Graphics Interchange Format) イメージ形式
 *             ImageFormat   ImageFormat.Jpeg { get; }   JPEG (Joint Photographic Experts Group) イメージ形式
 *             ImageFormat   ImageFormat.Png  { get; }   W3C PNG (Portable Network Graphics) イメージ形式
 *             ImageFormat   ImageFormat.Tiff { get; }   TIFF (Tagged Image File Format) イメージ形式
 *             ImageFormat   ImageFormat.Exif { get; }   Exif (Exchangeable Image File) 形式
 *             ImageFormat   ImageFormat.Icon { get; }   Windows アイコン イメージ形式
 *         
 *         void     image.Dispose();
 *         
 *@subject ◆Bitmap : Image
 *         ・ピクセルデータで表現されるイメージを扱うクラス
 *         ・abstract Image クラスを継承
 *         ・BMP 形式以外に JPG，GIF，PNG，EXIF，TIFF 形式も可。
 *         ・Bitmap オブジェクトでは，1 ピクセルごとに色の情報を得ることができる。
 *
 *         Bitmap   new Bitmap(string fileName)
 *         Bitmap   new Bitmap(Stream)
 *         Bitmap   new Bitmap(Image original)
 *         Bitmap   new Bitmap(Image original, Size newSize)
 *         Bitmap   new Bitmap(string fileName, bool useIcm)   
 *         Bitmap   new Bitmap(Stream stream, bool useIcm)
 *         Bitmap   new Bitmap(Type type, string resource)
 *         Bitmap   new Bitmap(int width, int height)
 *         Bitmap   new Bitmap(int width, int height, PixelFormat format)
 *         Bitmap   new Bitmap(int width, int height, Graphics g)
 *         Bitmap   new itmap(Image original, int width, int height)
 *         Bitmap   new Bitmap(int width, int height, int stride, PixelFormat, IntPtr scan0)
 *           引数 bool useIcm: 色補正を利用するか
 *                int stride: スキャンラインの間のバイトオフセット数を指定する整数。 
 *                            これには、通常 (必須ではありません)、ピクセルあたりのバイト数という形式 
 *                            (16 ビット/ピクセルの場合は 2) にビットマップの幅を乗じた値を指定します。
 *                            このパラメーターに渡す値は、4 の倍数である必要があります。
 *                IntPtr scan0: ピクセル データを格納するバイトの配列へのポインター。
 *               
 *         Color    bitmap.GetPixel(int x, int y)
 *         void     bitmap.SetPixel(int x, int y, Color)
 *         
 *@see ImageGetPixelSample.jpg
 *@see 
 *@author shika
 *@date 2022-08-22
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainBitmapGetPixelSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormBitmapGetPixelSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormBitmapGetPixelSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormBitmapGetPixelSample : Form
    {
        private Bitmap bitmap;
        private DialogShowColor dialogColor;  // self-defined class : Form 〔below〕

        public FormBitmapGetPixelSample()
        {
            this.Text = "FormBitmapGetPixelSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            this.MouseClick += new MouseEventHandler(form_MouseClick);
            
            bitmap = new Bitmap(
                "../../../../SelfAspNet/SelfAspNet/Image/A0003.jpg");

            dialogColor = new DialogShowColor();
            dialogColor.Owner = this;
            dialogColor.Show();
            
            //this.Controls.AddRange(new Control[]
            //{

            //});
        }//constructor

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(
                bitmap, 0, 0, bitmap.Width, bitmap.Height);
        }//OnPaint()

        private void form_MouseClick(object sender, MouseEventArgs e)
        {
            Point pt = e.Location;

            if(pt.X > bitmap.Width || pt.Y > bitmap.Height) { return; }
            
            dialogColor.BackColor = bitmap.GetPixel(pt.X, pt.Y);
        }//form_MouseClick()
    }//class

    class DialogShowColor : Form
    {
         public DialogShowColor()
        {
            Text = "PixelColor";
            Size = new Size(150, 150);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }
    }//class
}
