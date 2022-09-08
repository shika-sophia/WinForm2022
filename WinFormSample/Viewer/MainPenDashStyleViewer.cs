/** 
 *@title WinFormGUI / WinFormSample / Viewer
 *@class MainPenDashStyleViewer.cs
 *@class   └ new FormPenDashStyleViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content PenDashStyleViewer
 *@subject Pen
 *         =>〔KT07_Graphics/MainPenDrawLineSample.cs〕
 *         
 *         DashStyle  pen.DashStyle  
 *           └ enum DashStyle  -- System.Drawing.Drawing2D.
 *             {
 *                Solid = 0,      //実線
 *                Dash = 1,       //ダッシュ「―」で構成される直線
 *                Dot = 2,        //ドット「・」で構成される直線
 *                DashDot = 3,    //ダッシュとドットの繰り返しパターン
 *                DashDotDot = 4, //ダッシュと 2つのドットの繰り返しパターン
 *                Custom = 5      //ユーザー定義のカスタム ダッシュ スタイル
 *             }
 *             
 *         float[]    pen.DashPattern   DashStyle.Customのとき
 *                                      破線内の代替ダッシュと空白の長さを指定する実数の配列。
 *                                      
 *@see ImagePenDashStyleViewer.jpg
 *@see KT07_Graphics/MainPenDrawLineSample.cs
 *@author shika
 *@date 2022-09-08
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer
{
    class MainPenDashStyleViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormPenDashStyleViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormPenDashStyleViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormPenDashStyleViewer : Form
    {
        private readonly TableLayoutPanel table;
        private readonly Pen pen = new Pen(Color.HotPink, 3);
        private readonly Point start = new Point(10, 30);
        private readonly Point end = new Point(130, 30);
        private const int COLUMN = 4;

        public FormPenDashStyleViewer()
        {
            this.Text = "FormPenDashStyleViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(640, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = COLUMN,
                RowCount = 6,
                Padding = new Padding(10),
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };

            for(int i = 0; i < table.ColumnCount; i++)
            {
                table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100 / COLUMN));
            }//for

            for (int i = 0; i < table.RowCount; i++)
            {
                table.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 100 / table.RowCount));
            }//for

            //---- Label, PictureBox ----
            Label labelPart1 = new Label()
            {
                Text = "◆ enum DashStyle",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelPart1, 0, 0);
            table.SetColumnSpan(labelPart1, COLUMN);

            foreach(object value in Enum.GetValues(typeof(DashStyle)))
            {
                string name = value.ToString();
                var dashStyle = (DashStyle)value;

                Label labelName = new Label()
                {
                    Text = name,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                table.Controls.Add(labelName);

                PictureBox pic = new PictureBox()
                {
                    ClientSize = new Size(140, 70),
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.Fixed3D,
                };

                Bitmap bitmap = new Bitmap(
                    pic.ClientSize.Width, pic.ClientSize.Height);
                var g = Graphics.FromImage(bitmap);

                if(dashStyle == DashStyle.Custom)
                {
                    pen.DashPattern = new float[]
                    {
                        10.0f, 1.0f, 2.0f, 1.0f
                    };
                }
                pen.DashCap = DashCap.Round;
                pen.DashStyle = dashStyle;

                g.DrawLine(pen, start, end);
                g.Dispose();
                pic.Image = bitmap;
                table.Controls.Add(pic);
            }//foreach

            Label labelPart2 = new Label()
            {
                Text = "◆ pen.CompoundArray / 平行線",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelPart2, 0, 4);
            table.SetColumnSpan(labelPart2, COLUMN);

            float[] compoundAry = new float[]
            {
                0.0f, 0.3f, 0.7f, 1.0f
            };

            Label labelCompound = new Label()
            {
                Text = "float[]\n0.0|0.3|0.7|1.0",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelCompound);

            PictureBox picCompound = new PictureBox()
            {
                ClientSize = new Size(140, 70),
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
            };

            Bitmap bitmapCompound = new Bitmap(
                picCompound.ClientSize.Width, picCompound.ClientSize.Height);
            var g2 = Graphics.FromImage(bitmapCompound);

            pen.DashStyle = DashStyle.Solid;
            pen.CompoundArray = compoundAry;
            g2.DrawLine(pen, start, end);
            g2.Dispose();
            picCompound.Image = bitmapCompound;
            table.Controls.Add(picCompound);

            this.Controls.Add(table);
        }//constructor
    }//class
}
