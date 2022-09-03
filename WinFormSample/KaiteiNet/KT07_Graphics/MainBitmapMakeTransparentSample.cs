/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainBitmapMakeTransparentSample.cs
 *@class   └ new FormBitmapMakeTransparentSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics / Bitmap.MakeTransparent()
 *@subject 【部分透過】 画像を透過させて重ねる方法
 *         ・PNG形式「.png」の画像は 透過度情報をデータ内に保持している
 *         ・背景が透明の PNGを利用するか
 *         ・背景が一色に塗られている画像で、MakeTransparent()で透過色を指定する
 *         ・MakeTransparent()で指定した色を背景以外で利用していた場合、そこも透明になってしまう
 *         ・背景が透明の画像のほうが、きれいに描画できる
 *         
 *@subject Bitmap =>〔MainBitmapGetPixelSample.cs〕
 *         void    bitmap.MakeTransparent()       既定の透過色を透明にする
 *         void    bitmap.MakeTransparent(Color)  透過色を指定して、その色を透明にする
 *         Color   bitmap.GetPixel(int x, int y)  指定座標の Colorを取得
 *         
 *@see ImageBitmapMakeTransparentSample.jpg
 *@see MainBitmapGetPixelSample.cs
 *@copyTo ~/WinFormSample/GraphicsReference.txt
 *@author shika
 *@date 2022-08-24
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainBitmapMakeTransparentSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormBitmapMakeTransparentSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormBitmapMakeTransparentSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormBitmapMakeTransparentSample : Form
    {
        private PictureBox pictureBox;
        private Bitmap penguinImage;
        private Bitmap backgroundImage;

        public FormBitmapMakeTransparentSample()
        {
            this.Text = "FormBitmapMakeTransparentSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            //---- PictureBox ----
            pictureBox = new PictureBox()
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.Transparent,
            };

            //---- CharacterImage ----
            penguinImage = new Bitmap("../../Image/penguinBackcolor.png"); //青の背景色を持つ画像 = Color.FromArgb(0, 0, 0xFF)
            penguinImage.MakeTransparent(penguinImage.GetPixel(0, 0));     //(0, 0)の色を取得
            pictureBox.Image = penguinImage;

            //---- BackgreoundImage ----
            backgroundImage = new Bitmap("../../Image/SF101.JPG");
            this.BackgroundImage = backgroundImage;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                pictureBox,
            });
        }//constructor
    }//class
}
