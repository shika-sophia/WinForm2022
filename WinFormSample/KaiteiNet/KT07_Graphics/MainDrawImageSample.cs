/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainDrawImageSample.cs
 *@class   └ new FormDrawImageSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics / DrawImageSample
 *@subject Graphics
 *         void   graphics.DrawImage(Image, int x, int y)
 *         void   graphics.DrawImage(Image, float x, float y)
 *         void   graphics.DrawImage(Image, int x, int y, int width, int height)
 *         void   graphics.DrawImage(Image, float x, float y, float width, float height)
 *         
 *         ※ Point, PointF, Point[], PointF[], Rectangle, RectangleFを用いる オーバーロードも可
 *         ※ enum GraphicsUnit 長さの単位を指定する引数もある
 *            {
 *                World = 0,   //ワールド座標系を長さの単位に指定
 *                Display = 1, //表示デバイスの長さの単位を指定します。 通常、ビデオ ディスプレイにはピクセル、プリンターには 1/100 インチを指定します。
 *                Pixel = 2,   //デバイス ピクセルを長さの単位
 *                Point = 3,   //プリンターポイント (1/72 インチ) を長さの単位に指定
 *                Inch = 4,    //インチを長さの単位に指定
 *                Document = 5,//ドキュメント単位 (1/300 インチ) を長さの単位に指定
 *                Millimeter = 6//ミリメートルを長さの単位に指定
 *            }
 *            
 *@subject abstract Image : MarshalByRefObject, ISerializable, ICloneable, IDisposable
 *         ・抽象クラス Image: new Image()は不可
 *         ・インスタンスは Bitmapオブジェクトを生成している〔KT07〕
 *         
 *         Image  Image.FromFile(string path)       static / ファイルからImageをロード
 *         void   image.RotateFlip(RotateFlipType)  回転と反転のタイプを適用
 *           └ enum RotateFlipType
 *             {
 *                 RotateNoneFlipNone = 0,  //回転も反転も行わない
 *                 Rotate180FlipXY = 0,     //時計回りに 180 度回転してから、水平と垂直に反転
 *                 Rotate90FlipNone = 1,    //反転せずに時計回りに 90 度回転
 *                 Rotate270FlipXY = 1,     //時計回りに 270 度回転してから、水平と垂直に反転
 *                 Rotate180FlipNone = 2,   //反転せずに時計回りに 180 度回転
 *                 RotateNoneFlipXY = 2,    //回転せずに水平と垂直に反転
 *                 Rotate270FlipNone = 3,  //反転せずに時計回りに 270 度回転
 *                 Rotate90FlipXY = 3,   //時計回りに 90 度回転してから、水平方向と垂直方向に反転
 *                 RotateNoneFlipX = 4,  //回転せずに水平方向に反転
 *                 Rotate180FlipY = 4,   //時計回りに 180 度回転してから、垂直方向に反転
 *                 Rotate90FlipX = 5,    //時計回りに 90 度回転してから、水平方向に反転
 *                 Rotate270FlipY = 5,   //時計回りに 270 度回転してから、垂直方向に反転
 *                 Rotate180FlipX = 6,   //時計回りに 180 度回転してから、水平方向に反転
 *                 RotateNoneFlipY = 6,  //回転せずに垂直方向に反転
 *                 Rotate270FlipX = 7,   //時計回りに 270 度回転してから、水平方向に反転
 *                 Rotate90FlipY = 7     //時計回りに 90 度回転してから、垂直方向に反転
 *             }
 *             
 *@NOTE【考察】画像の反転
 *      ・KTのコード image.RotateFlip(RotateFlipType.RotateNoneFlipX)では反転しない
 *      ・RRのコード graphics.DrawImage()の 
 *        * 反転したい Width/Heightを マイナスにして反転する方法
 *        * -Widthにすると、DrawImageの開始点より左に描画され反転するので
 *          開始点を image.Width分 右にずらして置く必要がある。
 *          
 *        * マイナスにする方法で意図どうりの結果となるが、
 *          ２箇所(開始点と画像サイズ)を変更する必要があり、
 *          保守性,可読性に欠くので、できれば避けたいコード
 *        
 *        *image.RotateFlip()の解決方法を探すべき
 *          ・KT サンプルコードの (int) (image.Width * 0.8)は 関係がない。
 *            intでも floatでも DrawImage()は機能する
 *          ・imageRotated.RotateFlip(RotateFlipType.RotateNoneFlipX)を
 *            DrawImage(imageRotated, ...)の後に置いても解決せず。
 *          ・form.Refresh()を OnPaint()内に記述 
 *            => 解決せず。画面が何度も Refresh()されチラつく
 *          ・CommandPromptで実行 => 解決せず。([Visual C#]が原因ではない)
 *          ・ネットには [Windows Form]に同様の不具合報告あり。原因不明
 *          
 *@see ImageDrawImageSample.jpg
 *@see 
 *@author shika
 *@date 2022-08-20
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainDrawImageSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormDrawImageSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormDrawImageSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormDrawImageSample : Form
    {
        private Image image;
        private Image imageRotated;
        private string fileName;

        public FormDrawImageSample()
        {
            this.Text = "FormDrawImageSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(500, 500);
            this.BackColor = SystemColors.Window;

            string path = "../../../../SelfAspNet/SelfAspNet/Image/A0001.jpg";
            fileName = path.Substring(path.LastIndexOf("/"))
                .Replace("/", "");

            image = imageRotated = Image.FromFile(path);
            imageRotated.RotateFlip(RotateFlipType.RotateNoneFlipX);
            
            //this.Controls.AddRange(new Control[]
            //{

            //});
        }//constructor

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            
            g.DrawImage(image, 20, 20,
                image.Width, image.Height);
            g.DrawString($"◆{fileName} \n  {image.Width} × {image.Height}",
                this.Font, Brushes.Navy, new Point(230, 20));

            //g.DrawImage(imageRotated, (20 + imageRotated.Width * 1.2F), 200,
            //    -imageRotated.Width * 1.2F, imageRotated.Height * 1.2F);
            g.DrawImage(imageRotated, 20, 200,
                imageRotated.Width * 1.2F, imageRotated.Height * 1.2F);
            g.DrawString($"◆{fileName} Rotated-X\n" +
                $"{imageRotated.Width * 1.2F} × {imageRotated.Height * 1.2F}",
                this.Font, Brushes.Blue, new Point(230, 200));
        }//OnPaint()
    }//class
}
