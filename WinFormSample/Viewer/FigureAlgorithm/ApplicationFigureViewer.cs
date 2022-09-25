/** 
 *@title WinFormGUI / WinFormSample / Viewer / FigureAlgorithm
 *@class MainFigureViewer.cs
 *@class   └ new FormFigureViewer() : Form
 *@class       └ new AlgoRegularPolygon()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content FigureViewer
 *         正ｎ角形と星型の描画
 *         角の数 Angle、半径 Radius、ペンの太さ Width を入力可
 *         ボタンクリック時に入力検証を行い、不適正入力は MessageBoxで通知
 *         
 *         [英] Triangle        三角形
 *         [英] Rectangle       四角形
 *         [英] Pentagon        五角形
 *         [英] Hexagon         六角形
 *         [英] Polygon         多角形
 *         [英] Regular Polygon 正多角形
 *         [英] center line     中心線
 *         [英] diagonal line   対角線
 *         
 *@subject 描画座標のアルゴリズム
 *         FormFigureViewerクラスのプロパティに
 *         new AlgoRegularPolygon()をして、委譲。
 *
 *@subject DrawDiagonalLine(PointF[])
 *         自分自身の点どうし以外、全ての点に線を引いて、
 *         正多角形の輪郭をもう一度、上書き
 *         
 *@NOTE【Problem】TableLayoutPanelと PictureBox
 *      TableLayoutPanelの ColumnStyle, RowStyleを SizeType.Percentで指定すると
 *      PictureBoxの ClientSizeが、デフォルトのサイズに縮んでしまう問題
 *      SizeType.Absoluteで ピクセル指定すると解決。
 *      
 *@see ImageApplicationFigureViewer.jpg
 *@see AlgoRegularPolygon.cs
 *@author shika
 *@date 2022-09-15
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer.FigureAlgorithm
{
    class ApplicationFigureViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormFigureViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormFigureViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormFigureViewer : Form
    {
        private readonly AlgoRegularPolygon algo = new AlgoRegularPolygon();
        private readonly TableLayoutPanel table;
        private readonly ListBox list;
        private readonly Label labelAngle;
        private readonly Label labelRadius;
        private readonly Label labelWidth;
        private readonly TextBox tbAngle;
        private readonly TextBox tbRadius;
        private readonly TextBox tbWidth;
        private readonly CheckBox checkCenterLine;
        private readonly CheckBox checkDiagonalLine;
        private readonly Button button;
        private readonly PictureBox pic;
        private readonly Graphics g;
        private readonly PointF centerPoint;
        private readonly Pen penBlue = new Pen(Color.CornflowerBlue, 2);
        private readonly Pen penPink = new Pen(Color.HotPink, 1);
        private int angle;
        private decimal radius;

        public FormFigureViewer()
        {
            this.Text = "ApplicationFigureViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;

            //---- TanleLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 3,
                RowCount = 7,
                Dock = DockStyle.Fill,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 480));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 180));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            //---- ListBox ----
            list = new ListBox()
            {
                Dock = DockStyle.Fill,
            };

            list.Items.AddRange(new string[]
            {
                "Triangle", "Rectangle", "Pentagon", "Hexagon",
                "Polygon", "Star",
            });
            list.SelectedIndexChanged += new EventHandler(list_SelectedIndexChanged);
            
            table.Controls.Add(list, 0, 0);
            table.SetColumnSpan(list, 2);

            //---- Label ----
            labelAngle = new Label()
            {
                Text = "Angle:",
                TextAlign = ContentAlignment.TopLeft,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelAngle, 0, 1);

            labelRadius = new Label()
            {
                Text = "Radius:",
                TextAlign = ContentAlignment.TopLeft,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelRadius, 0, 2);

            labelWidth = new Label()
            {
                Text = "Pen Width:",
                TextAlign = ContentAlignment.TopLeft,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelWidth, 0, 3);

            //---- TextBox ----
            tbAngle = new TextBox()
            {
                Text = "3",
                TextAlign = HorizontalAlignment.Center,
                Enabled = false,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(tbAngle, 1, 1);

            tbRadius = new TextBox()
            {
                Text = "150.0",
                TextAlign = HorizontalAlignment.Center,
                Enabled = true,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(tbRadius, 1, 2);

            tbWidth = new TextBox()
            {
                Text = "2.0",
                TextAlign = HorizontalAlignment.Center,
                Enabled = true,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(tbWidth, 1, 3);

            checkCenterLine = new CheckBox()
            {
                Text = "Center Line",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(checkCenterLine, 0, 4);
            table.SetColumnSpan(checkCenterLine, 2);

            checkDiagonalLine = new CheckBox()
            {
                Text = "Diagonal Line",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(checkDiagonalLine, 0, 5);
            table.SetColumnSpan(checkDiagonalLine, 2);

            //---- Button ----
            button = new Button()
            {
                Text = "Draw Figure",
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(button_Click);
            table.Controls.Add(button, 0, 6);
            table.SetColumnSpan(button, 2);

            //---- PictureBox ----
            pic = new PictureBox()
            {
                ClientSize = new Size(480, 480),
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(pic, 2, 0);
            table.SetRowSpan(pic, 7);

            Bitmap bitmap = new Bitmap(
                pic.ClientSize.Width, pic.ClientSize.Height);
            g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;
            penBlue.LineJoin = LineJoin.Round;
            penBlue.MiterLimit = 20.0f;

            centerPoint = new PointF(
                (float)((decimal)pic.ClientSize.Width / 2M),
                (float)((decimal)pic.ClientSize.Height / 2M));
            pic.Image = bitmap;

            this.Controls.Add(table);
        }//constructor

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = (sender as ListBox).SelectedItem.ToString();

            switch (selected)
            {
                case "Triangle":
                    tbAngle.Text =
                        $"{(sender as ListBox).SelectedIndex + 3}";
                    tbAngle.Enabled = false;
                    checkCenterLine.Enabled = true;
                    checkDiagonalLine.Checked = false;
                    checkDiagonalLine.Enabled = false;
                    break;

                case "Rectangle":
                case "Pentagon":
                case "Hexagon":
                    tbAngle.Text = 
                        $"{(sender as ListBox).SelectedIndex + 3}";
                    tbAngle.Enabled = false;
                    checkCenterLine.Enabled = true;
                    checkDiagonalLine.Enabled = true;
                    break;
                case "Polygon":
                    tbAngle.Text = "7";
                    tbAngle.Enabled = true;
                    checkCenterLine.Enabled = true;
                    checkDiagonalLine.Enabled = true;
                    break;
                case "Star":
                    tbAngle.Text = "5";
                    tbAngle.Enabled = true;
                    checkCenterLine.Checked = false;
                    checkCenterLine.Enabled = false;
                    checkDiagonalLine.Checked = false;
                    checkDiagonalLine.Enabled = false;
                    break;
            }//switch
        }//list_SelectedIndexChanged()

        private void button_Click(object sender, EventArgs e)
        {
            if(list.SelectedItem == null) 
            { 
                list.SetSelected(0, true); 
            }

            string selected = list.SelectedItem.ToString();

            //---- Validate input ----
            List<string> messageList = new List<string>();
            bool canAngle = Int32.TryParse(tbAngle.Text, out int inputAngle);
            bool canRadius = Double.TryParse(tbRadius.Text, out double inputRadius);
            bool canWidth = Double.TryParse(tbWidth.Text, out double inputWidth);

            if (!canAngle)
            {
                messageList.Add("Angle was error. Please input by Integer.");
            }

            if(!canRadius)
            {
                messageList.Add("Radius was error. Please input by numeric. ");
            }

            if (!canWidth)
            {
                messageList.Add("Width was error. Please input by numeric. ");
            }

            if (inputAngle < 3)
            {
                messageList.Add("Angle shoud be input over 3.");
            }

            if (selected == "Star" && inputAngle < 5)
            {
                messageList.Add("Star angle should be input over 5.");
            }

            if (inputRadius < 10 || 240 < inputRadius)
            {
                messageList.Add("Radius shold be input over 10 under 240.");
            }

            if(inputWidth < 0.3 || 20 < inputWidth)
            {
                messageList.Add("Width shold be input over 0.3 and under 20.");
            }

            if(messageList.Count > 0)
            {
                StringBuilder bld = new StringBuilder(messageList.Count * 50);
                foreach(string mess in messageList)
                {
                    bld.Append(mess).Append("\n");
                }//foreach
                
                MessageBox.Show(bld.ToString(), "Input Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            //---- Adjust Parameter ----
            if(selected == "Polygon" && inputAngle < 7)
            {
                list.SetSelected(inputAngle - 3, true);
            }

            angle = inputAngle;
            radius = (decimal)inputRadius;
            penBlue.Width = (float)inputWidth;

            //---- DrawFigure(), DrawStar() ----
            if (selected != "Star")
            {
                DrawFigure();
            }
            else if (selected == "Star")
            {
                DrawStar();
            }
        }//button_Click()

        private void DrawFigure()
        {
            g.Clear(SystemColors.Window);
            g.FillEllipse(penBlue.Brush,
                (float)((decimal)centerPoint.X - 3M),
                (float)((decimal)centerPoint.Y - 3M), 6, 6);

            PointF[] polyPointAry = 
                algo.AlgoMultiAngle(centerPoint, radius, angle);
            g.DrawPolygon(penBlue, polyPointAry);

            RectangleF rect = algo.AlgoCircle(centerPoint, radius);
            g.DrawEllipse(penPink, rect);

            if (checkCenterLine.Checked)
            {
                DrawCenterLine(polyPointAry);
            }

            if (checkDiagonalLine.Checked)
            {
                DrawDiagonalLine(polyPointAry);
            }

            pic.Refresh();
        }//DrawFigure()

        private void DrawCenterLine(PointF[] pointAry)
        {
            foreach (PointF pt in pointAry)
            {
                g.DrawLine(penPink, centerPoint, pt);
            }
        }//DrawCenterLine()

        public void DrawDiagonalLine(PointF[] pointAry)
        {
            for (int i = 0; i < pointAry.Length; i++)
            {
                for (int j = 0; j < pointAry.Length; j++)
                {
                    if(j == i) { continue; } 
                    g.DrawLine(penPink, pointAry[i], pointAry[j]);
                }//for j
            }//for i

            g.DrawPolygon(penBlue, pointAry);
        }//DrawDiagonalLine()

        private void DrawStar()
        {
            g.Clear(SystemColors.Window);

            //---- OddStar ----            
            if (angle % 2 == 1)
            {
                GraphicsPath gPath = algo.AlgoOddStar(centerPoint, radius, angle);
                //g.FillPath(penBlue.Brush, gPath);
                g.DrawPath(penBlue, gPath);
            }

            //---- EvenStar ----
            if (angle % 2 == 0)
            {
                GraphicsPath[] gPathAry = algo.AlgoEvenStar(centerPoint, radius, angle);

                //g.FillPath(penPink.Brush, gPathAry[0]);
                //g.FillPath(penPink.Brush, gPathAry[1]);
                g.DrawPath(penBlue, gPathAry[0]);
                g.DrawPath(penBlue, gPathAry[1]);
            }

            pic.Refresh();
        }//DrawStar()
    }//class
}
