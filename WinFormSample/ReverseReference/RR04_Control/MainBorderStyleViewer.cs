/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainBorderStyleViewer.cs
 *@class   └ new FormBorderStyleViewer() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[38] p79 FormBorderStyle
 *@subject Form -- System.Windows.Forms
 *         FormBorderStyle  form.FormBorderStyle { get; set; }
 *                              Formのサイズ変更の可否、境界線のスタイル
 *                              FormBorderStyle.Xxxxx で指定
 *           └ enum FormBorderStyle  -- System.Windows.Forms
 *             { 
 *                None = 0,             // 境界線なし
 *                FixedSingle = 1,      // サイズ固定，一重線
 *                Fixed3D = 2,          // サイズ固定，3D スタイル
 *                FixedDialog = 3,      // サイズ固定，ダイアログ用
 *                Sizable = 4,          // サイズ可変 (既定)
 *                FixedToolWindow = 5,  // サイズ固定，ツールウィンドウ用
 *                SizableToolWindow = 6 // サイズ可変，ツールウィンドウ用
 *             }
 *             
 *@subject コントロール共通 BorderStyle
 *         BorderStyle  control.BorderStyle { get; set; }
 *           └ enum BorderStyle  -- System.Windows.Forms
 *             {
 *                None = 0,             // 境界線なし
 *                FixedSingle = 1,      // サイズ固定，一重線
 *                Fixed3D = 2,          // サイズ固定，3D スタイル
 *             }
 *             
 *@NOTE【Problem】
 *      ・FormBorderStyleを変更しても、Formの境界スタイルは、たいして変わらない
 *      ・サイズ変更の可否は、ちゃんと反映する
 *      ・Noneにすると、タイトルバーも消える
 *      ・XxxxToolWindowにすると、タイトルバーの最小化/最大化ボタンが消える
 *      ・Labelの BorderStyle.Fixed3Dにすると、 左辺と下辺は うまく描画されない
 *      ・Border3DStyle  ToolStripStatusLabel.BorderStyle は種類が充実している
 *      
 *@see ImageBorderStyleViewer.jpg
 *@see MainBorderStyleToolStripStatusLabel.cs
 *@author shika
 *@date 2022-09-04
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainBorderStyleViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormBorderStyleViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormBorderStyleViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormBorderStyleViewer : Form
    {
        private readonly TableLayoutPanel table;
        private readonly Button[] buttonAry;
        private readonly Label label;

        public FormBorderStyleViewer()
        {
            this.Text = "FormBorderStyleViewer";
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

            Array borderStyleAry = Enum.GetValues(typeof(FormBorderStyle));
            buttonAry = new Button[borderStyleAry.Length];

            foreach (object value in borderStyleAry)
            {
                string name = value.ToString();
                int index = (int)value;

                buttonAry[index] = new Button()
                {
                    Text = name,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    UseVisualStyleBackColor = true,
                };
                buttonAry[index].Click += new EventHandler((sender, e) =>
                {
                    label.Text = name;
                    this.FormBorderStyle = (FormBorderStyle)value;
                    this.Refresh();
                });
                table.Controls.Add(buttonAry[index]);
            }//foreach

            label = new Label()
            {
                Text = "Sizable",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
            };
            table.Controls.Add(label, 0, 4);
            table.SetColumnSpan(label, 2);

            this.Controls.Add(table);
        }//constructor

    }//class
}
