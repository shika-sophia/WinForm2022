/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR18_Excel
 *@class MainExcelSheetsAddSample.cs
 *@class   └ new FormExcelSheetsAddSample() : Form
 *@class       └ new Excel.Application()
 *@using Excel = Microsoft.Office.Interop.Excel;
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[492][493] p829 / Excel / Sheets.Add()
 *         新しいシートを追加、シートの一覧を表示する
 *         
 *@summary Sheets     workbook.Sheets
 *         dynamic    workbook.Sheets[i]
 *         dynamic    sheets.Add([object Before], [object After], [object Count], [object Type]);
 *           └ [Return] the Added Worksheet as dynamic -> need cast to Worksheet, if use it's property.
 *         string     worksheet.Name
 *         
 *         =>〔MainExcelWorkbookOpenSample.cs〕
 *         =>〔MianExcelCellsValueSample.cs〕
 *
 *@see ImageExcelSheetsAddSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-28
 */
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelSheetsAddSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelSheetsAddSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelSheetsAddSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelSheetsAddSample : Form
    {
        private readonly Excel.Application excelApp;
        private readonly Excel.Workbook workbook;
        private readonly Excel.Sheets sheetCollection;
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly TextBox textBoxName;
        private readonly TextBox textBoxSheets;
        private readonly Button buttonCreate;
        private readonly Button buttonSheets;

        public FormExcelSheetsAddSample()
        {
            this.Text = "FormExcelSheetsAddSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(480, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelSheetsAddSample");
            this.Load += new EventHandler(FormExcelSheetsAddSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormExcelSheetsAddSample_FormClosed);

            //---- Excel Application ----
            excelApp = new Excel.Application();
            workbook = excelApp.Workbooks.Open(
                Path.GetFullPath(@"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelFileSample.xlsx"));
            sheetCollection = workbook.Sheets;

            //---- Controls ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 3,
                RowCount = 3,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            for(int i = 0; i < table.ColumnCount; i++)
            {
                table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100f / table.ColumnCount));
            }//for

            label = new Label()
            {
                Text = "Sheet Name: ",
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(label, 0, 0);

            textBoxName = new TextBox()
            {
                Text = $"Sheet{sheetCollection.Count + 1}",
                Multiline = false,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxName, 1, 0);

            buttonCreate = new Button()
            {
                Text = "Create New Sheet",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonCreate.Click += new EventHandler(ButtonCreate_Click);
            table.Controls.Add(buttonCreate, 2, 0);

            buttonSheets = new Button()
            {
                Text = "Show Sheet List",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonSheets.Click += new EventHandler(ButtonSheets_Click);
            table.Controls.Add(buttonSheets, 2, 1);

            textBoxSheets = new TextBox()
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxSheets, 0, 2);
            table.SetColumnSpan(textBoxSheets, table.ColumnCount);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void ButtonCreate_Click(object sender, EventArgs e)
        {
            //---- ValidateInput ----
            bool canInput = ValidateInput(textBoxName.Text);
            if (!canInput) { return; }

            //---- Create New Sheet ----
            Excel.Worksheet newSheet = sheetCollection.Add(
                After: sheetCollection[sheetCollection.Count]);
            newSheet.Name = textBoxName.Text;

            MessageBox.Show($"{newSheet.Name} is added.", "Done");
        }//ButtonCreate_Click()

        private void ButtonSheets_Click(object sender, EventArgs e)
        {
            foreach (Excel.Worksheet sheet in sheetCollection)
            {
                string sheetName = sheet.Name;
                textBoxSheets.Text += $"{sheetName}{Environment.NewLine}";
            }//foreach
        }//ButtonSheets_Click()

        private bool ValidateInput(string input)
        {
            if (String.IsNullOrEmpty(textBoxName.Text)) { return false; }

            input = input.Trim();
            StringBuilder errorMessage = new StringBuilder();

            //---- Name Length ----
            if (textBoxName.Text.Length > 20) 
            {
                errorMessage.Append("Sheet Name should be in 20 chatacters.\n");
            }

            //---- isAlphabet, isDigit ----
            Regex regexName = new Regex("[a-zA-Z0-9_-]+");
            if (!regexName.IsMatch(input))
            {
                errorMessage.Append("Sheet Name should be described by alphabet, digit, '_', or '-'\n");
            }

            //---- Anti-Script ----
            Regex regexScript = new Regex("[<>&;]+");
            if (regexScript.IsMatch(input))
            {
                errorMessage.Append("Invalid Input\n");
            }

            //---- Duplicate Name ----
            foreach (Excel.Worksheet sheet in sheetCollection)
            {
                if (sheet.Name == input)
                {
                    errorMessage.Append("Input Name already has been.\n ");
                    break;
                }
            }//foreach

            //---- canInput ----
            if(errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage.ToString());
                return false;
            }

            return true; //canInput
        }//ValidateInput()

        //====== Form Event ======
        private void FormExcelSheetsAddSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//Form1_Load()

        private void FormExcelSheetsAddSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            workbook.Save();
            excelApp.Quit();
            mutex.Close();
        }//Form1_FormClosed()
    }//class
}
