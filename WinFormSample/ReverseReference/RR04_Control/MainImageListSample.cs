/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainImageListSample.cs
 *@class FormImageListSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@content RR[92] ImageList with ListView / p175
 *@subject ◆ImageList : Component
 *         ImageList new ImageList() / ImageList(IContainer)
 *         ImageList.ImageCollection
 *         Size      imageList.ImageSize  画像サイズ / デフォルト 16×16, 最大 256×256
 *                   imageList.Images
 *         void      imageList.Images.Add(Image)
 *         void      imageList.Images.Add(string key, Image)  keyも同時に登録可
 *         void      imageList.Images.AddRange(Image[])
 *         StringCollection  imageList.Images.Keys     ImageCollection内のKeyコレクション
 *         bool      imageList.Images.Contains(Image)
 *         bool      imageList.Images.ContainsKey(string key)
 *         Color     imageList.TransparentColor  親Controlの色 (=透明になる色)
 *         void      imageList.Draw(Graphics, int x, int y, int width, int height)
 *         void      imageList.Draw(Graphics, int x, int y, int index)
 *         void      imageList.Draw(Graphics, Point pt, int index)
 *
 *@subject ListViewで表示
 *         ListView  listView.LargeImageList
 *  例:  listView.LargeImageList = imageList;
 *       imageList.Images.Add(Image);                      Imageの登録
 *       listView.Items.Add(string text, int image index); 画像下のテキストを登録
 *
 *@subject System.IO.
 *         string   Path.GetFullPath(string)   引数の相対パスから絶対パスを取得
 *         string[] Directory.GetFiles(string path, string searchPattern)
 *         
 *@subject 別プロジェクト「SelfAspNet\Image」への相対パス:
 *         アプリケーション・ルート「~」は ASP.NETのみで Windows.Formでは機能せず。
 *         カレントフォルダ: 「~\bin\Debug\WinFormGUI.exe」から
 *         プロジェクト上位に「ソリューション」フォルダが存在するので
 *         更に上位の「repos」から他ソリューションに入る。
 *         つまり 「\Debug」 から見て4つ上位にある別ソリューションのため
 *         @"..\..\..\..\SelfAspNet\SelfAspNet\Image" となる。
 *         
 *@see FormImageListSample.jpg
 *@see ~\WinFormSample\KaiteiNet\KT06_Control\MainListViewSample.cs
 *@see SelfAspNet\Image
 *@author shika
 *@date 2022-07-17
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainImageListSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormImageListSample());
        }//Main()
    }//class

    class FormImageListSample : Form
    {
        private Button button;
        private ImageList imageList;
        private ListView listView;

        public FormImageListSample()
        {
            this.Text = "FormImageListSample";
            this.Size = new Size(420, 320);

            button = new Button()
            {
                Text = "Show Image Files",
                Location = new Point(10, 10),
                AutoSize = true,
            };
            button.Click += new EventHandler(button_Click);

            imageList = new ImageList()
            {
                ImageSize = new Size(100, 100),
            };

            listView = new ListView()
            {
                Location = new Point(10, 50),
                View = View.LargeIcon,
                Size = new Size(360, 200),
                Anchor = AnchorStyles.Left | AnchorStyles.Right 
                       | AnchorStyles.Top | AnchorStyles.Bottom,                
            };
            listView.LargeImageList = imageList;

            this.Controls.AddRange(new Control[]
            {
                button, listView
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            string path = Path.GetFullPath(
                @"..\..\..\..\SelfAspNet\SelfAspNet\Image"); 
            string[] imageFileAry = Directory.GetFiles(path, "*.jpg");

            for(int i = 0; i < imageFileAry.Length; i++)
            {
                Image image = Bitmap.FromFile(imageFileAry[i]);
                imageList.Images.Add(image);   //Imageの登録

                string imageText = imageFileAry[i]
                    .Substring(imageFileAry[i].LastIndexOf(@"\"))
                    .Replace(@"\","");
                listView.Items.Add(imageText, i);  //画像下テキストの登録

                image.Dispose();
            }//for
        }
    }//class

}
