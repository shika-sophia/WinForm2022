/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR18_Excel
 *@class MainExcelPdfExportSample.cs
 *@class   └ new FormExcelPdfExportSample() : Form
 *@class       └ new Excel.Application()
 *@using Excel = Microsoft.Office.Interop.Excel;
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[494][495] p832 / Excel / PDF Export, Print Out
 *
 *@subject interface _WorkSheet -- Microsoft.Office.Interop.Excel
 *         void  _worksheet.ExportAsFixedFormat(
 *            XlFixedFormatType Type, 
 *            [object Filename,]
 *            [object Quality,]
 *            [object IncludeDocProperties,]
 *            [object IgnorePrintAreas,]
 *            [object From, object To,]
 *            [object OpenAfterPublish,]
 *            [object FixedFormatExtClassPtr]
 *         );
 *         
 *         void  _worksheet.PrintOutEx(
 *            [object From, object To,]
 *            [object Copies,]
 *            [object Preview,]
 *            [object ActivePrinter,]
 *            [object PrintToFile,]
 *            [object Collate,]
 *            [object PrToFileName,]
 *            [object IgnorePrintAreas]
 *         );
 *         
 *         =>〔MainExcelWorkbookOpenSample.cs〕
 *         =>〔MianExcelCellsValueSample.cs〕
 *
 *@subject enum XlFixedFormatType -- Microsoft.Office.Interop.Excel
 *         {
 *            xlTypePDF = 0,
 *            xlTypeXPS = 1
 *         }
 *
 *@NOTE【註】Absolute Path / Relative Path
 *      ・File.Exists(string path), Create(string path)のファイルアクセスは
 *       「WinFormGUI.exe」からの 相対パス
 *      ・_worksheet.ExportAsFixedFormat(), PrintOutEx() のファイルアクセスは
 *        Excel「.exe」からのアクセスなので 絶対パスが必要
 *      
 *      string  Path.GetFullPath(string relativePath)
 *      
 *@see ImageExcelPdfExportSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-29
 */
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelPdfExportSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelPdfExportSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelPdfExportSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelPdfExportSample : Form
    {
        private readonly Excel.Application excelApp;
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly TextBox textBoxName;
        private readonly TextBox textBoxContent;
        private readonly Button buttonPdf;
        private readonly Button buttonPrint;

        public FormExcelPdfExportSample()
        {
            this.Text = "FormExcelPdfExportSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelPdfExportSample");
            this.Load += new EventHandler(FormExcelPdfExportSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormExcelPdfExportSample_FormClosed);

            //---- Excel Application ----
            excelApp = new Excel.Application();
            
            //---- Controls ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            for (int i = 0; i < table.ColumnCount; i++)
            {
                table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100f / table.ColumnCount));
            }//for

            label = new Label()
            {
                Text = "Sheet Name: ",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(label, 0, 0);

            textBoxName = new TextBox()
            {
                Text = "RR18_ExcelGridFileSample.xlsx",
                Multiline = false,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxName, 1, 0);
           
            buttonPdf = new Button()
            {
                Text = "Export by PDF",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonPdf.Click += new EventHandler(ButtonPdf_Click);
            table.Controls.Add(buttonPdf, 0, 1);

            buttonPrint = new Button()
            {
                Text = "Export by Printer",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonPrint.Click += new EventHandler(ButtonPrint_Click);
            table.Controls.Add(buttonPrint, 2, 1);

            textBoxContent = new TextBox()
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxContent, 0, 2);
            table.SetColumnSpan(textBoxContent, table.ColumnCount);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void ButtonPdf_Click(object sender, EventArgs e)
        {
            //---- ValidateInput ----
            if (String.IsNullOrEmpty(textBoxName.Text)) { return; }
            //bool canInput = ValidateInput(textBoxName.Text);

            //---- Excel ----
            string dir = @"..\..\WinFormSample\ReverseReference\RR18_Excel\";
            string loadFileName = $"{dir}{textBoxName.Text}";
            string saveFileName = $"{dir}RR18_PdfGridFileSample.pdf";
            
            if (!File.Exists(saveFileName))
            {
                File.Create(saveFileName);
            }

            Excel.Workbook workbook = excelApp.Workbooks.Open(
                Path.GetFullPath(loadFileName));
            Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];
            sheet1.ExportAsFixedFormat(
                Excel.XlFixedFormatType.xlTypePDF,
                Path.GetFullPath(saveFileName));

            textBoxContent.Text += $"Saved: {workbook.Name} to PDF File. {Environment.NewLine}";
        }//ButtonPdf_Click()

        private void ButtonPrint_Click(object sender, EventArgs e)
        {
            //---- ValidateInput ----
            if (String.IsNullOrEmpty(textBoxName.Text)) { return; }
            //bool canInput = ValidateInput(textBoxName.Text);

            //---- Excel ----
            Excel.Workbook workbook = excelApp.Workbooks.Open(
                Path.GetFullPath($@"..\..\WinFormSample\ReverseReference\RR18_Excel\{textBoxName.Text}"));
            Excel.Worksheet sheetNow = (Excel.Worksheet)workbook.ActiveSheet;

            //sheetNow.PrintOutEx();  //PC default printer
            textBoxContent.Text += $"Printing: {workbook.Name} / {sheetNow.Name} {Environment.NewLine}";
        }//ButtonPrint_Click()

        //====== Form Event ======
        private void FormExcelPdfExportSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//Form1_Load()

        private void FormExcelPdfExportSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            excelApp.Quit();
            mutex.Close();
        }//Form1_FormClosed()
    }//class
}
