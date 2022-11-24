/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR18_Excel
 *@class MainExcelDataGridViewSample.cs
 *@class   └ new FormExcelDataGridViewSample() : Form
 *@class       └ new Microsoft.Office.Interop.Excel.Application()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[487] p822 / Excel / DataGridView
 *         Excelファイルから、表形式でデータを取得し、表形式で表示
 *         
 *@Prepare ExcelDataに対応したクラスを作成する必要がある
 *         [Example]
 *         class BookExcelDataObjectSample
 *         {
 *            public int Id { get; set; }
 *            public string Title { get; set; }
 *            public int Price { get; set; }
 *         }//class BookDataSample
 *         
 *@subject Microsoft.Office.Interop.Excel.Application
 *         ・Excelの Cells[row, cloumn].Value の数値は double型で取得するので、
 *           必要に応じて int などに 要キャスト
 *           
 *         =>〔MainExcelWorkbookOpenSample.cs〕
 *         =>〔MianExcelCellsValueSample.cs〕
 *         
 *@subject DataGridView 〔WF 30-2〕
 *         object  dataGridView.DataSource
 *           └ List<BookExcelDataObjectSample> に 各行のデータを Add()して
 *             Listごと dataGridView.DataSourceに渡す
 *             
 *         [Example]
 *         List<BookExcelDataObjectSample> 
 *             dataList = new List<BookExcelDataObjectSample>();
 *         int row = 2;
 *         while (sheet1.Cells[row, 1].Text != "")
 *         {
 *            var bookData = new BookExcelDataObjectSample()
 *            {
 *               Id = (int)sheet1.Cells[row, 1].Value,
 *               Title = sheet1.Cells[row, 2].Value,
 *               Price = (int)sheet1.Cells[row, 3].Value,
 *            };
 *            dataList.Add(bookData);
 *            row++;
 *         }//while
 *         grid.DataSource = dataList;
 *
 *@NOTE【註】Move File
 *      [×] VSの機能で Excelファイル移動してはいけない
 *      Excelで保存したディレクトリを変更すると、Excelからファイルを開けなくなる。
 *      VSでも利用し、「編集中のためロック」と表示される。
 *      こうなると、Excel, VSとも open, deleteができなくなる。
 *      => PC Shutdownしてファイルを強制終了して解決
 *      
 *      [×] Don't use File Move Operation of Visual Studio.
 *      If you would change the directory of this file where Excel saved, 
 *      Excel cannot open this file.
 *      If VS would use this file, VS said "Locking this file, due to be editing".
 *      On these cases, both Excel and VS cannot open or delete either.
 *      => 【Solve】by PC Shutdown, this file should close enforcely.
 *      
 *@see ImageExcelDataGridViewSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-24
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelDataGridViewSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelDataGridViewSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelDataGridViewSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelDataGridViewSample : Form
    {
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly TextBox textBox;
        private readonly Button button;
        private readonly DataGridView grid;

        public FormExcelDataGridViewSample()
        {
            this.Text = "FormExcelDataGridViewSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelDataGridViewSample");
            this.Load += new EventHandler(FormExcelDataGridViewSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormExcelDataGridViewSample_FormClosed);

            //---- Controls ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 3,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            textBox = new TextBox()
            {
                Text = "RR18_ExcelGridFileSample.xlsx",
                Multiline = false,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(textBox);

            button = new Button()
            {
                Text = "Show Excel Grid",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button);

            grid = new DataGridView()
            {
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(grid);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            //---- Connect Excel File ----
            string dir = @"..\..\WinFormSample\ReverseReference\RR18_Excel\";
            string filePath = Path.GetFullPath(dir + textBox.Text);

            Excel.Application excelApp= new Excel.Application();
            try
            {
                Excel.Workbook wb = excelApp.Workbooks.Open(filePath);
                Excel.Worksheet sheet1 = (Excel.Worksheet)wb.Sheets[1];

                //---- Read Excel Data ----
                List<BookExcelDataObjectSample> dataList = new List<BookExcelDataObjectSample>();
                int row = 2;
                while (sheet1.Cells[row, 1].Text != "")
                {
                    var bookData = new BookExcelDataObjectSample()
                    {
                        Id = (int)sheet1.Cells[row, 1].Value,
                        Title = sheet1.Cells[row, 2].Value,
                        Price = (int)sheet1.Cells[row, 3].Value,
                    };

                    dataList.Add(bookData);
                    row++;
                }//while

                grid.DataSource = dataList;

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                excelApp.Quit();
            }
        }//Button_Click()

        //====== Form Event ======
        private void FormExcelDataGridViewSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//Form1_Load()

        private void FormExcelDataGridViewSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.Close();
        }//Form1_FormClosed()
    }//class

    class BookExcelDataObjectSample
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
    }//class BookDataSample
}
