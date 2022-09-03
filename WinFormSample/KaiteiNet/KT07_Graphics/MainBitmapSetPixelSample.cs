/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainBitmapSetPixcelSample.cs
 *@class   └ new FormBitmapSetPixcelSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics / Bitmap.SetPixcel()
 *         Bitmap   new Bitmap(int width, int height) によって、
 *         サイズだけ指定した 空のBitmapオブジェクトを作り、
 *         bitmap.SetPixel(int x, int y, Color)で、
 *         Bitmapの絵を描き、
 *         graphic.DrawImage(Image, int x, int y)で
 *         Bitmapを Formに描画
 *         SetPixel()を変更した場合は form.Reflesh()で再描画が必要
 *         
 *@subject Bitmap =>〔MainBitmapGetPixelSample.cs〕
 *         void    bitmap.SetPixcel(int x, int y, Color)
 *         
 *@subject Color  =>〔~/WinFormSample/ColorReference.txt〕
 *         Color   Color.FromArgb(int red, int green, int blue)
 *
 *@NOTE【註】
 *      ・for文 i, j, k での 三重ネストは 処理に 1-2分ぐらい掛かり、
 *        表示が停滞する
 *      ・bitmap.SetPixel()だけでは、Formの表示に変化なし
 *        form.Reflesh()でキャッシュを破棄し、再描画すると Formに反映する
 *        
 *@see ImageBitmapSetPixcelSample.jpg
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
    class MainBitmapSetPixelSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormBitmapSetPixcelSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormBitmapSetPixcelSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormBitmapSetPixcelSample : Form
    {
        private Bitmap bitmap;
        private Button buttonGB;
        private Button buttonRB;
        private Button buttonRG;

        public FormBitmapSetPixcelSample()
        {
            this.Text = "FormBitmapSetPixcelSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            bitmap = new Bitmap(256, 256);
            BuildBitmapGradiate("Black-White");

            buttonGB = new Button()
            {
                Text = "Green-Blue",
                Location = new Point(270, 20),
                Width = 120,
            };
            buttonGB.Click += new EventHandler(button_Click);

            buttonRG = new Button()
            {
                Text = "Red-Green",
                Location = new Point(270, 100),
                Width = 120,
            };
            buttonRG.Click += new EventHandler(button_Click);

            buttonRB = new Button()
            {
                Text = "Red-Blue",
                Location = new Point(270, 180),
                Width = 120,
            };
            buttonRB.Click += new EventHandler(button_Click);

            this.Controls.AddRange(new Control[]
            {
                buttonGB, buttonRB, buttonRG,
            });
        }//constructor
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.DrawImage(bitmap, 0, 0);

            g.Dispose();
        }//OnPaint()

        private void button_Click(object sender, EventArgs e)
        {
            string btText = (sender as Button).Text;
            BuildBitmapGradiate(btText);
        }//button_Click()

        private void BuildBitmapGradiate(string btText)
        {
            Color colorGradiate;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    switch (btText)
                    {
                        case "Black-White":
                            colorGradiate = Color.FromArgb(i, i, i);
                            break;
                        case "Green-Blue":
                            colorGradiate = Color.FromArgb(0, i, j);
                            break;
                        case "Red-Green":
                            colorGradiate = Color.FromArgb(i, j, 0);
                            break;
                        case "Red-Blue":
                            colorGradiate = Color.FromArgb(i, 0, j);
                            break;
                        default:
                            throw new ArgumentException();
                    }//switch

                    bitmap.SetPixel(i, j, colorGradiate);                    
                }//for j
            }//for i

            this.Refresh();
        }//BuildBitmapGradiate()
    }//class
}
