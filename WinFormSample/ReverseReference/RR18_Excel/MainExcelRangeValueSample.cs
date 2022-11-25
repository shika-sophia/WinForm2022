/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR18_Excel
 *@class MainExcelRangeValueSample.cs
 *@class   └ new FormExcelRangeValueSample() : Form
 *@class       └ new Excel.Application()
 *@using Excel = Microsoft.Office.Interop.Excel;
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content ExcelRangeValueSample
 *         write value to Excel Cell / Excel Cell に値を書き込み
 *         
 *@subject Summary
 *         Sheets   workbook.Sheets
 *         dynamic  workbook.Sheets[i]
 *         dynamic  workBook.ActiveSheet
 *         Range    workSheet.Range[string] // Argument is Excel CellName
 *         Range    workSheet.Cells[i, j]   // [row, column] Arguments start from 1 (not 0)
 *         string   range.Value
 *         double   range.Value  数値
 *         
 *         =>〔MainExcelWorkbookOpenSample.cs〕
 *         =>〔MianExcelCellsValueSample.cs〕
 *         
 *@see ImageExcelRangeValueSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-25
 */
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelRangeValueSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelRangeValueSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelRangeValueSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelRangeValueSample : Form
    {
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly Label labelRange;
        private readonly Label labelValue;
        private readonly TextBox textBoxRange;
        private readonly TextBox textBoxValue;
        private readonly Button button;

        public FormExcelRangeValueSample()
        {
            this.Text = "FormExcelRangeValueSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(480, 120);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelRangeValueSample");
            this.Load += new EventHandler(FormExcelRangeValueSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormExcelRangeValueSample_FormClosed);

            //---- Controls ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            labelRange = new Label()
            {
                Text = "Cell Name:",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelRange, 0, 0);

            labelValue = new Label()
            {
                Text = "Cell Value:",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelValue, 0, 1);

            textBoxRange = new TextBox()
            {
                Multiline = false,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxRange, 1, 0);

            textBoxValue = new TextBox()
            {
                Multiline = false,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxValue, 1, 1);

            button = new Button()
            {
                Text = "Insert Excel Value",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button, 0, 2);
            table.SetColumnSpan(button, 2);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();

            try
            {
                Excel.Workbook wb = excelApp.Workbooks.Open(
                    Path.GetFullPath(@"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelFileSample.xlsx"));
                Excel.Worksheet sheet1 = (Excel.Worksheet)wb.Sheets[1];

                //---- ValidateInput ----
                

                //---- write value to Excel Cell ----
                sheet1.Range[textBoxRange.Text].Value = textBoxValue.Text;
                MessageBox.Show($"worte {textBoxRange.Text}: {textBoxValue.Text}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                excelApp.Quit();
            }
        }//Button_Click()

        //====== Form Event ======
        private void FormExcelRangeValueSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//Form1_Load()

        private void FormExcelRangeValueSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.Close();
        }//Form1_FormClosed()
    }//class
}
