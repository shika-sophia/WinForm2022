/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR08_Graphics
 *@class MainColorMatrixSepia.cs
 *@class   └ new FormColorMatrixSepia() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content RR[372] p626 ColorMatrix Sepia
 *         画像をセピア色に変化させる
 *@subject ImageAttributes
 *@subject ColorMatrix
 *         =>〔MainColorMatrixSemiTrasparent.cs〕
 *         ＊色調の変更 RR[372] p626
 *           変更後の R = r * Matrix00 + g * Matrix01 + b * Matrix02
 *           変更後の G = r * Matrix10 + g * Matrix11 + b * Matrix12
 *           変更後の B = r * Matrix20 + g * Matrix21 + b * Matrix22
 *           
 *@see ImageColorMatrixSepia.jpg
 *@see 
 *@author shika
 *@date 2022-08-30
 */
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR08_Graphics
{
    class MainColorMatrixSepia
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormColorMatrixSepia()");

            Application.EnableVisualStyles();
            Application.Run(new FormColorMatrixSepia());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormColorMatrixSepia : Form
    {
        private readonly TableLayoutPanel table;
        private readonly ListBox list;
        private readonly PictureBox pic;
        private readonly Button btnOrigin;
        private readonly Button btnSepia;
        private Graphics g;
        private Image image;
        private ImageAttributes imageAttr;
        private Rectangle rect;

        public FormColorMatrixSepia()
        {
            this.Text = "FormColorMatrixSepia";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(600, 400);
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

            list = new ListBox()
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            list.Items.Add(imgName1);
            list.Items.Add(imgName2);
            list.SelectedIndexChanged += new EventHandler(list_SelectedIndexChanged);
            list.SelectedItem = imgName1;
            table.Controls.Add(list, 0, 0);
            table.SetColumnSpan(list, 2);

            list_SelectedIndexChanged(list, new EventArgs()); // initialize image
            imageAttr = BuildImageAttr();                     //self defined 〔below〕

            pic = new PictureBox()
            {
                ClientSize = new Size(
                    this.ClientSize.Width - 5,
                    (int)(this.ClientSize.Height * 0.7)),
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
            };
            g = pic.CreateGraphics();
            rect = BuildRectangle();       //self defined 〔below〕
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
        }//constructor

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = (sender as ListBox).SelectedItem.ToString();
            image = new Bitmap($"../../Image/{selectedName}");
        }

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

        private ImageAttributes BuildImageAttr()
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
        }//BuildImageAttr()

        private Rectangle BuildRectangle()
        {
            int widthRate = pic.Width / image.Width * 100;
            int heightRate = pic.Height / image.Height * 100;
            int adjustRate = (widthRate < heightRate) ? widthRate : heightRate;

            return new Rectangle(0, 0,
                pic.Width * adjustRate / 100,
                pic.Height * adjustRate / 100);
        }//BuildRectangle()

        private void PictureBoxGraphicsDrawImage()
        {
            g.DrawImage(image, rect, 0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel, imageAttr);
        }
    }//class
}
