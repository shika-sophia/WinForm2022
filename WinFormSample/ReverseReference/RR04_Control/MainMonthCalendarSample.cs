/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainMonthCalendarSample.cs
 *@class FormMonthCalendarSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[79] MonthCalendar / p153
 *         日付範囲を選択できる月カレンダーを表示するコントロール
 *         
 *@subject ◆MonthCalendar : Control
 *         MonthCalendar new MonthCalendar()
 *         int        calen.MaxSelectionCount   選択可能な最大日数
 *         DateTime   calen.SelectionStart      選択範囲の開始日
 *         DateTime   calen.SelectionEnd        選択範囲の終了日
 *        (TimeSpan   dateTime.Subtract(DateTime) 差分       )
 *        (int        TimeSpan.Days               差分の日数  )
 *         DateTime   calen.MaxDate             カレンダー日付の最大値 9998/12/31まで
 *         DateTime   calen.MinDate             カレンダー日付の最小値 1753/01/01まで
 *         DateTime[] calen.BoldedDates         太字にする日
 *         DateTime[] calen.MonthlyBoldedDates  月ごとの太字日
 *         DateTime[] calen.AnnuallyBoldedDates 年ごとの太字日
 *         Size       calen.CalendarDimensions  表示月の(列数, 行数) 
 *                       複数月のカレンダーを表示可。最大で 12ヶ月分。
 *                       例: calen.CalendarDimensions = new Size(2, 1);
 *                       横に２ヶ月分のカレンダーを表示
 *         
 *@see FormMonthCalendarSample.jpg
 *@author shika
 *@date 2022-07-15
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainMonthCalendarSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormMonthCalendarSample());
        }//Main()
    }//class

    class FormMonthCalendarSample : Form
    {
        private TableLayoutPanel table;
        private MonthCalendar calen;
        private Button button;
        private Label[] labelAry;
        private TextBox[] textBoxAry;
        private string[] itemAry = new string[]
        {
            "Start Date:","End Date:","Duration:"
        };

        public FormMonthCalendarSample()
        {
            this.Text = "FormMonthCalendarSample";
            this.AutoSize = true;

            table = new TableLayoutPanel()
            {
                ColumnCount = 3,
                RowCount = itemAry.Length + 1,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            calen = new MonthCalendar()
            {
                MaxSelectionCount = 10,                
                CalendarDimensions = new Size(2, 1),
            };
            table.Controls.Add(calen, 0, 0);
            table.SetRowSpan(calen, itemAry.Length + 1);

            button = new Button()
            {
                Text = "Get Selected Duration",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(button_Click);
            table.Controls.Add(button, 1, 0);
            table.SetColumnSpan(button, 2);

            labelAry = new Label[itemAry.Length];
            textBoxAry = new TextBox[itemAry.Length];
            for(int i = 0; i < itemAry.Length; i++)
            {
                labelAry[i] = new Label()
                {
                    Text = itemAry[i],
                    TextAlign = ContentAlignment.MiddleLeft,
                    AutoSize = true,
                };

                textBoxAry[i] = new TextBox()
                {
                    ReadOnly = true,
                    Multiline = false,
                    TextAlign = HorizontalAlignment.Center,
                    Dock = DockStyle.Fill,
                };

                table.Controls.Add(labelAry[i], 1, (i + 1));
                table.Controls.Add(textBoxAry[i], 2, (i + 1));
            }//for

            this.Controls.Add(table);
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            DateTime start = calen.SelectionStart;
            DateTime end = calen.SelectionEnd;
            int duration = end.Subtract(start).Days + 1;

            textBoxAry[0].Text = start.ToLongDateString();
            textBoxAry[1].Text = end.ToLongDateString();
            textBoxAry[2].Text = duration.ToString();
        }//button_Click()
    }//class

}
