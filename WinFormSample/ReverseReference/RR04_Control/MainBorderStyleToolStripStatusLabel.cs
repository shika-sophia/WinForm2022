/** 
 *@title WinFormGUI / WinFormSample / ReverseReference.RR04_Control
 *@based MainBorderStyleViewer.cs
 *@class MainBorderStyleToolStripStatusLabel.cs
 *@class   └ new FormBorderStyleToolStripStatusLabel() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menu-strips/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm12_MenuToolStrip.txt〕
 *           
 *@content ToolStripStatusLabel.BorderStyle  〔WF41〕
 *@subject StatusStrip
 *         =>〔KT12_MenuToolStrip\MainStatusStripSample.cs〕
 *         
 *         Border3DStyle          toolStripStatusLabel.BorderStyle
 *           └ enum  Border3DStyle
 *             {
 *                 RaisedOuter = 1, //外縁だけ凸表示、内縁は非表示
 *                 SunkenOuter = 2, //外縁だけ凹表示、内縁は非表示
 *                 RaisedInner = 4, //内縁だけが凸表示され、外縁は非表示
 *                 Raised = 5,      //内縁と外縁が、凸表示
 *                 Etched = 6,      //内縁と外縁が、凹表示
 *                 SunkenInner = 8, //凹表示され、外縁は非表示
 *                 Bump = 9,        //内縁と外縁が、凸表示
 *                 Sunken = 10,     //内縁と外縁が凹表示
 *                 Adjust = 8192,   //境界線が指定した四角形の外側に描画され、四角形の大きさは保持
 *                 Flat = 16394     //平面表示
 *             }
 *
 *@see ImageBorderStyleToolStripStatusLabel.jpg
 *@see MainBorderStyleViewer.cs
 *@see KT12_MenuToolStrip\MainStatusStripSample.cs
 *@author shika
 *@date 2022-09-04
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainBorderStyleToolStripStatusLabel
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormBorderStyleToolStripStatusLabel()");

            Application.EnableVisualStyles();
            Application.Run(new FormBorderStyleToolStripStatusLabel());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormBorderStyleToolStripStatusLabel : Form
    {
        private readonly TableLayoutPanel table;
        private readonly Button[] buttonAry;
        private readonly StatusStrip status;
        private readonly ToolStripStatusLabel stLabel;

        public FormBorderStyleToolStripStatusLabel()
        {
            this.Text = "FormBorderStyleToolStripStatusLabel";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(480, 320);
            this.BackColor = SystemColors.Window;

            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 5,
                Dock = DockStyle.Fill,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));

            status = new StatusStrip();
            stLabel = new ToolStripStatusLabel()
            {
                Text = "RaisedOuter",
                Spring = true,
                BorderStyle = Border3DStyle.RaisedOuter,
                BorderSides = ToolStripStatusLabelBorderSides.All,
            };
            status.Items.Add(stLabel);

            Array borderStyleAry = Enum.GetValues(typeof(Border3DStyle));
            buttonAry = new Button[borderStyleAry.Length];
            int index = 0;
            foreach(object value in borderStyleAry)
            {
                string name = value.ToString();

                buttonAry[index] = new Button()
                {
                    Text = name,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    UseVisualStyleBackColor = true,
                };
                buttonAry[index].Click += new EventHandler((sender, e) =>
                {
                    stLabel.Text = name;
                    stLabel.BorderStyle = (Border3DStyle)value;
                    status.Refresh();
                });
                table.Controls.Add(buttonAry[index]);

                index++;
            }//foreach

            this.Controls.AddRange(new Control[]
            {
                table, status,
            });
        }//constructor
    }//class
}
