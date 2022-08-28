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
 *@subject GraphicsPath -- 
 *@subject LinearGradientBrush
 *@subject PathGradientBrush
 *
 *@see ImagePathGradientBrushSample.jpg
 *@see 
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
        [STAThread]
        static void Main()
        //public void Main()
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
        private readonly Bitmap bitmap;

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
                Image = bitmap,
                Size = new Size(
                    this.ClientSize.Width - 5, 
                    (int)(this.ClientSize.Height * 0.83)),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
            };
            graph = pic.CreateGraphics();
            rect = new Rectangle(pic.Location, pic.ClientSize);
            bitmap = new Bitmap(pic.Width, pic.Height);
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
