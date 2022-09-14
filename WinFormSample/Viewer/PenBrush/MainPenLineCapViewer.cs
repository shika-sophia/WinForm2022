/** 
 *@title WinFormGUI / WinFormSample / Viewer / PenBrush
 *@class MainPenLineCapViewer.cs
 *@class   └ new FormPenLineCapViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content RR[366] p616 PenLineCapViewer
 *         enum LineCap を PictureBoxに描画して一覧表示
 *         
 *@subject Pen
 *         =>〔KT07_Graphics/MainPenDrawLineSample.cs〕
 *         
 *         LineCap    pen.EndCap      線の終点の形状
 *           └ enum LineCap  -- System.Drawing.Drawing2D.
 *             {
 *                 Flat = 0,           //平坦なラインキャップ
 *                 Square = 1,         //四角形のラインキャップ
 *                 Round = 2,          //丸いラインキャップ
 *                 Triangle = 3,       //三角形のラインキャップ
 *                 NoAnchor = 16,      //アンカーなし
 *                 SquareAnchor = 17,  //四角形のアンカー ラインキャップ
 *                 RoundAnchor = 18,   //丸いアンカーキャップ
 *                 DiamondAnchor = 19, //菱形のアンカーキャップ
 *                 ArrowAnchor = 20,   //矢印形のアンカーキャップ
 *                 AnchorMask = 240,   //ラインキャップがアンカーキャップかどうかをチェックする際に使用するマスクを指定
 *                 Custom = 255        //CustomLineCapクラスで定義
 *             }
 *
 *@NOTE【考察】PictureBoxの Graphicsオブジェクト
 *      ・Graphics g = pic.CreateGraphics(); 
 *        g.DrawLine();をしても、表示されなかった。
 *        PictureBox 1つのときは描画されるので、
 *        複数の PictureBoxで pic.CreateGraphics();が再描画のため消えてしまうのか
 *        
 *      ・pic.Paint += new PaintEventHandler((sender, e) => 
 *        {  Graphics g = e.Graphics; }
 *        g.DrawLine();をしても、ArgumentException
 *        
 *      ・Bitmap bitmap = new Bitmap(pic.Width, pic.Height)
 *        Graphics g = Graphics.FromImage(bitmap);
 *        g.DrawLine();
 *        pic.Image = bitmap; で解決
 *        
 *      ・pic.ClientSize = new Size(140, 70) 必須
 *        指定しないと PictureBox デフォルトの Graphics領域 (100?, 50?)ぐらいになり
 *        DrawXxxx()の結果が切れてしまう
 *         
 *@see ImagePenLineCapViewer.jpg
 *@see KT07_Graphics/MainPenDrawLineSample.cs
 *@author shika
 *@date 2022-09-07
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.PenBrush
{
    class MainPenLineCapViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormPenLineCapViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormPenLineCapViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormPenLineCapViewer : Form
    {
        private readonly TableLayoutPanel table;
        private readonly Pen pen = new Pen(Color.CornflowerBlue, 5);
        private readonly Point start = new Point(10, 30);
        private readonly Point end = new Point(130, 30);
        private const int COLUMN = 4;

        public FormPenLineCapViewer()
        {
            this.Text = "FormPenLineCapViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(640, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = COLUMN,
                RowCount = 6,
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };

            for (int i = 0; i < table.ColumnCount; i++)
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
            var labelPart = new Label()
            {
                Text = "◆ enum LineCap",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            table.Controls.Add(labelPart);
            table.SetColumnSpan(labelPart, COLUMN);

            foreach (object value in Enum.GetValues(typeof(LineCap)))
            {
                string name = value.ToString();
                var lineCap = (LineCap)value;

                if (lineCap == LineCap.Custom) { break; }

                var labelName = new Label()
                {
                    Text = name,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                table.Controls.Add(labelName);                

                var pic = new PictureBox()
                {
                    ClientSize = new Size(140, 70),
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.Fixed3D,
                };

                Bitmap bitmap = new Bitmap(
                    pic.ClientSize.Width, pic.ClientSize.Height);
                var g = Graphics.FromImage(bitmap);
                pen.DashStyle = DashStyle.Solid;
                pen.SetLineCap(lineCap, lineCap, DashCap.Flat);
                g.DrawLine(pen, start, end);

                pic.Image = bitmap;
                g.Dispose();
                table.Controls.Add(pic);
            }//foreach

            this.Controls.Add(table);
        }//constructor
    }//class
}
