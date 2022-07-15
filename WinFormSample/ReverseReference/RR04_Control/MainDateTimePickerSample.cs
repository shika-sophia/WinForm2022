/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainDateTimePickerSample.cs
 *@class FormDateTimePickerSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[78] DateTimePicker / p152
 *         日付取得のためカレンダーを表示するコントロール
 *         
 *@subject ◆DateTimePicker : Control
 *         DateTimePicker new DateTimePicker()
 *         string   date.Text
 *         DateTime date.Value
 *         DateTime date.MaxDate
 *         DateTime date.MinDate
 *         bool     date.ShowUpDown   スピンボタンを表示するか
 *         bool     date.ShowCheckBox チェックボックスを表示するか
 *         DateTimePickerFormat date.Format  コントロールで表示する書式
 *             └ enum DateTimePickerFormat
 *               {
 *                   Long = 1,
 *                   Short = 2,
 *                   Time = 4,
 *                   Custom = 8
 *               }
 *         string   date.CustomFormat
 *               
 *         Color    date.CalendarForeColor
 *         Color    date.CalendarMonthBackground
 *         Color    date.CalendarTitleForeColor
 *         Color    date.CalendarTitleBackColor
 *         Color    date.CalendarTrailingForeColor 前後月の数日間の文字色
 *         Font     date.CalendarFont
 *
 *@see FormDateTimePickerSample.jpg
 *@author shika
 *@date 2022-07-15
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainDateTimePickerSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormDateTimePickerSample());
        }//Main()
    }//class

    class FormDateTimePickerSample : Form
    {
        private DateTimePicker date;
        private Button button;
        private Label label;
        private TextBox textBox;

        public FormDateTimePickerSample()
        {
            this.Text = "FormDateTimePickerSample";
            this.AutoSize = true;

            DateTime now = DateTime.Now;
            DateTime min = new DateTime(2000, 1, 1);
            DateTime max = new DateTime(2030, 12, 31);

            date = new DateTimePicker()
            {
                Location = new Point(10, 10),
                Width = 100,
                Format = DateTimePickerFormat.Short,
                Value = now,
                MinDate = min,
                MaxDate = max,
            };

            button = new Button()
            {
                Text = "OK",
                Location = new Point(120, 10),
            };
            button.Click += new EventHandler(button_Click);

            label = new Label()
            {
                Text = "Selected Date:",
                Location = new Point(10, 200),
                AutoSize = true,
            };

            textBox = new TextBox()
            {
                Location = new Point(100, 200),
                ReadOnly = true,
            };

            this.Controls.AddRange(new Control[]
            {
                date, button, label, textBox,
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            textBox.Text = date.Value.ToLongDateString();
        }
    }//class

}
