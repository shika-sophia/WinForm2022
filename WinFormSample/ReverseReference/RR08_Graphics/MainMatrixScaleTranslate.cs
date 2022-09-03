/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR08_Graphics
 *@based MainColorMatrixSepia.cs
 *@class MainMatrixScaleTaranslate.cs
 *@class   └ new FormMatrixScaleTaranslate() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content RR[378] p635 Matrixクラス Scale(), Taranslate()
 *         画像の拡大縮小、平行移動
 *         => 〔MainMatrixRotateSample.cs〕
 *           
 *@NOTE【Problem】Matrixクラス Scale(), Translate()
 *      ・matrix.Scale()によって、元画像の縦横比を維持したまま、
 *        PictureBoxのサイズに拡大縮小することには成功
 *      ・matrix.Translate()でセンタリングを試みたが、intキャスト時の誤差のためか
 *        元画像のサイズによって、表示位置が変わってしまう問題あり〔未解決〕
 *        
 *      ・Matrix利用前は
 *        rect = new Rectangle(0, 0, pic.Width, pic.Height); で うまくいっていたが
 *      ・Matrix利用後は
 *        rect = new Rectangle(0, 0, image.Width, image.Height);でないと
 *        Scale()の倍率が大幅にずれてしまう問題あり。
 *        image.Width, image.Heightで Matrix修正後のサイズになっている様子
 *      
 *      ・double計算の精度を上げるため、decimalで計算
 *        decimal  Math.Round(decimal, int)  int値は小数点以下 第何位に丸めるか
 *        小数点以下 2 -> 4に変更すると、うまくいかない。
 *        
 *      private void BuildTransform()
        { 
            decimal widthRate = Math.Round(
                (decimal)pic.ClientSize.Width / (decimal)image.Width, 2);
            decimal heightRate = Math.Round(
                (decimal)pic.ClientSize.Height / (decimal)image.Height, 2);
            decimal adjustRate = (widthRate < heightRate) ? widthRate : heightRate;

            Matrix mx = new Matrix();
            mx.Scale((float)adjustRate, (float)adjustRate);
            mx.Translate(
                (float)((pic.ClientSize.Width - image.Width * adjustRate) / 2),
                (float)((pic.ClientSize.Height - image.Height * adjustRate) / 2));

            g.Transform = mx;
        }//BuildTransform()

 *@see ImageMatrixScaleTranslate.jpg
 *@copyTo ~/WinFormSample/GraphicsReference.txt
 *@author shika
 *@date 2022-09-01
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR08_Graphics
{
    class MainMatrixScaleTranslate
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormMatrixScaleTaranslate()");

            Application.EnableVisualStyles();
            Application.Run(new FormMatrixScaleTaranslate());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMatrixScaleTaranslate : Form
    {
        private readonly TableLayoutPanel table;
        private readonly ListBox list;
        private readonly PictureBox pic;
        private readonly Button btnOrigin;
        private readonly Button btnSepia;
        private readonly Graphics g;
        private readonly Bitmap bitmap1;
        private readonly Bitmap bitmap2;
        private readonly ImageAttributes imageAttr;
        private Rectangle rect;
        private Image image;

        public FormMatrixScaleTaranslate()
        {
            this.Text = "FormMatrixScaleTaranslate";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(640, 480);
            this.BackColor = SystemColors.Window;

            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 70f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));

            string imgName1 = "NaziElephant.jpg";    //250×150
            string imgName2 = "NaziSpinRocket.jpg";  //740×505
            bitmap1 = new Bitmap(Image.FromFile($"../../Image/{imgName1}"), new Size(250, 150));
            bitmap2 = new Bitmap(Image.FromFile($"../../Image/{imgName2}"), new Size(740, 505));

            list = new ListBox()
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            list.Items.Add($"{imgName1}  ({bitmap1.Width} × {bitmap1.Height})");
            list.Items.Add($"{imgName2}  ({bitmap2.Width} × {bitmap2.Height})");
            list.SelectedIndexChanged += new EventHandler(list_SelectedIndexChanged);
            table.Controls.Add(list, 0, 0);
            table.SetColumnSpan(list, 2);

            pic = new PictureBox()
            {
                Image = bitmap1,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(
                    this.ClientSize.Width - 5,
                    (int)(this.ClientSize.Height * 0.7)),
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
            };
            table.Controls.Add(pic, 0, 1);
            table.SetColumnSpan(pic, 2);

            btnOrigin = new Button()
            {
                Text = "Original",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            btnOrigin.Click += new EventHandler(btnOrigin_Click);
            table.Controls.Add(btnOrigin, 0, 2);

            btnSepia = new Button()
            {
                Text = "Sepia",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            btnSepia.Click += new EventHandler(btnSepia_Click);
            table.Controls.Add(btnSepia, 1, 2);

            this.Controls.Add(table);

            //---- initialize ----
            list.SelectedIndex = 0;
            image = bitmap1;
            g = pic.CreateGraphics();
            imageAttr = BuildSepiaImageAttr();  //self defined 〔below〕            
        }//constructor

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = (sender as ListBox).SelectedItem.ToString();

            if (selected.Contains("Elephant"))
            {
                image = bitmap1;
            }
            else if (selected.Contains("SpinRocket"))
            {
                image = bitmap2;
            }
        }//list_SelectedIndexChanged()

        private void btnOrigin_Click(object sender, EventArgs e)
        {
            imageAttr.SetNoOp();
            PictureBoxGraphicsDrawImage();
        }//btnOrigin_Click()

        private void btnSepia_Click(object sender, EventArgs e)
        {
            imageAttr.ClearNoOp();
            PictureBoxGraphicsDrawImage();
        }//btnSepia_Click()

        private ImageAttributes BuildSepiaImageAttr()  //self defined
        {
            var cm = new ColorMatrix() //Change to Sepia Color
            {
                Matrix00 = 0.393f,
                Matrix01 = 0.349f,
                Matrix02 = 0.272f,
                Matrix10 = 0.769f,
                Matrix11 = 0.686f,
                Matrix12 = 0.534f,
                Matrix20 = 0.189f,
                Matrix21 = 0.168f,
                Matrix22 = 0.131f,
                Matrix33 = 1f,
                Matrix44 = 1f,
            };

            var imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(cm);

            return imageAttr;
        }//BuildSepiaImageAttr()

        private void BuildTransform()
        { 
            decimal widthRate = Math.Round(
                (decimal)pic.ClientSize.Width / (decimal)image.Width, 2);
            decimal heightRate = Math.Round(
                (decimal)pic.ClientSize.Height / (decimal)image.Height, 2);
            decimal adjustRate = (widthRate < heightRate) ? widthRate : heightRate;

            Matrix mx = new Matrix();
            mx.Scale((float)adjustRate, (float)adjustRate);
            mx.Translate(
                (float)((pic.ClientSize.Width - image.Width * adjustRate) / 2),
                (float)((pic.ClientSize.Height - image.Height * adjustRate) / 2));

            g.Transform = mx;
        }//BuildTransform()

        private void PictureBoxGraphicsDrawImage()
        {
            g.Clear(SystemColors.Window);
            BuildTransform();
            rect = new Rectangle(0, 0, image.Width, image.Height);
            g.DrawImage(image, rect, 0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel, imageAttr);
        }
    }//class
}
