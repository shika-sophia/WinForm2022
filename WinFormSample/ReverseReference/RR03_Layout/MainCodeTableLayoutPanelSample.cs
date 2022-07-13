/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR03_Layout
 *@class MainCodeTableLayoutPanelSample.cs
 *@class FomeCodeTableLayoutPanelSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@content RR[109] TableLayoutPanel / p203
 *         ・複数のコントロールを 列、行の表形式(=テーブル形式)で配置
 *         ・各コントロールで Dock = DockStyle.Fillをすると、
 *           各セル内いっぱいに拡大して、きれいに納まる。
 *           
 *@subject ◆TableLayoutPanel : Panel, IExtenderProvider
 *         TableLayoutPanel new TableLayoutPanel()
 *         int  table.ColumnCount  列数
 *         int  table.RowCount     行数
 *         
 *         //---- 列幅、行幅を定義 ----
 *         TableLayoutColumnStyleCollection table.ColumnStyles  列幅コレクション
 *         TableLayoutRowStyleCollection    table.RowStyles     行幅コレクション
 *         int table.ColumnStyles.Add(ColumnStyle)   各列幅を登録
 *         int table.RowStyles.Add(RowStyle)         各行幅を登録
 *           └ ColumnStyle new ColumnStyle(SizeType, float width)
 *           └ RowStyle    new RowStyle(SizeType, float height)
 *               └ enum SizeType
 *                 {  
 *                    AutoSize = 0,   他列、他行と共有する値に自動調整
 *                    Absolute = 1,   px単位に固定
 *                    Persent = 2     Parentコントロールのパーセントで指定
 *                 }
 *                 
 *         //---- 各コントロールを配置 ----
 *         table.Controls.Add(Control, int column, int row)  配置コントロールの(列, 行)を指定
 *         table.SetColumnSpan(Control, int value)           複数列にまたがる場合にコントロールと列数を指定
 *         table.SetRowSpan(Conteol, int value)              複数行にまたがる場合にコントロールと行数を指定
 *         form.Controls.Add(table)                          Formに TableLayoutPanelを乗せる
 *
 *@see FormCodeTableLayoutPanelSample.jpg
 *@author shika
 *@date 2022-07-13
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR03_Layout
{
    class MainCodeTableLayoutPanelSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormCodeTableLayoutPanelSample());
        }//Main()
    }//class

    class FormCodeTableLayoutPanelSample : Form
    {
        private TableLayoutPanel table;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button button;
        private TextBox textBox;
        private RadioButton radio1;
        private RadioButton radio2;
        private RichTextBox rich;

        public FormCodeTableLayoutPanelSample()
        {
            this.Text = "FormCodeTableLayoutPanelSample";
            this.Size = new Size(400,300);

            table = new TableLayoutPanel()
            {
                ColumnCount = 3,
                RowCount = 4,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));

            //---- Columns[0] ----
            label1 = new Label() 
            { 
                Text = "Name", 
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
            };

            label2 = new Label()
            {
                Text = "Sex",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
            };

            label3 = new Label()
            {
                Text = "Comment",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
            };

            button = new Button()
            {
                Text = "OK",
                Dock = DockStyle.Fill,
            };
            button.Click += new EventHandler(button_Click);

            table.Controls.Add(label1, 0, 0);
            table.Controls.Add(label2, 0, 1);
            table.Controls.Add(label3, 0, 2);
            table.Controls.Add(button, 0, 3);
            table.SetColumnSpan(button, 3);

            //---- Columns[1][2] ----
            textBox = new TextBox()
            {
                Dock = DockStyle.Fill,
                Multiline = false,
            };

            radio1 = new RadioButton()
            {
                Text = "Male",
                Dock = DockStyle.Fill,
            };

            radio2 = new RadioButton()
            {
                Text = "Female",
                Dock = DockStyle.Fill,
            };

            rich = new RichTextBox()
            {
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(textBox, 1, 0);
            table.Controls.Add(radio1, 1, 1);
            table.Controls.Add(radio2, 2, 1);
            table.Controls.Add(rich, 1, 2);
            table.SetColumnSpan(textBox, 2);
            table.SetColumnSpan(rich, 2);

            this.Controls.Add(table);
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            RadioButton radio;
            if (radio1.Checked)
            {
                radio = radio1;
            }
            else if (radio2.Checked)
            {
                radio = radio2;
            }
            else
            {
                radio = radio1;
                radio.Text = "Unknown";
            }

            MessageBox.Show(
                $"Name: {textBox.Text}\n" +
                $"Sex : {radio.Text}\n" +
                $"Comment: {rich.Text}\n",
                "Result");
        }//button_Click()
    }//class

}
