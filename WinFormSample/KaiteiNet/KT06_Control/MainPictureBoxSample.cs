/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT06_Control
 *@class MainPictureBoxSample.cs
 *@class FormPictureBoxSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/controls/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm06_Control.txt〕
 *           
 *@content KT 6. Control / PictureBox, Bitmap/Image
 *@subject ◆Bitmap : Image / Image -- System.Drawing.
 *         new Bitmap(string path)
 *         Image.FromFile(string path) //new Image()は不可
 *         int  image.Width
 *         int  image.Height
 *         Size image.Size
 *         
 *@subject ◆PictureBox -- System.Windows.Forms
 *         Image    pictureBox.Image
 *         SizeMode pictureBox.PictureBoxSizeMode
 *             enum PictureBoxSizeMode
 *             {
 *                 Normal = 0,       // PictureBox の左上端にそのまま配置
 *                 StretchImage = 1, // PictureBox に合わせてイメージを伸び縮み
 *                 AutoSize = 2,     // PictureBox のサイズをイメージに合わせる
 *                 CenterImage = 3,  // PictureBox の中央にイメージを配置
 *                 Zoom = 4          // 縦横比を維持したまま拡大または縮小
 *             }
 *
 *@see FormPictureBoxSample.jpg
 *@see ../../Image/VaioSX14COREi7_10710U.jpg   // 1000×750
 *@author shika
 *@date 2022-07-02
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT06_Control
{
    class MainPictureBoxSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormPictureBoxSample());
        }//Main()
    }//class

    class FormPictureBoxSample : Form
    {
        private Bitmap image;
        private PictureBox pic;

        public FormPictureBoxSample()
        {
            this.Text = "FormPictureBoxSample";
            this.AutoSize = true;

            image = new Bitmap("../../Image/VaioSX14COREi7_10710U.jpg");
            Size imageSizeHalf = new Size(image.Width / 2, image.Height / 2);

            pic = new PictureBox()
            {
                Image = image,
                Size = imageSizeHalf,
                SizeMode = PictureBoxSizeMode.Zoom,
            };
            this.Controls.Add(pic);
        }//constructor
    }//class
}
