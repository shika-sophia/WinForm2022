/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR18_Excel
 *@class MainExcelLineStyleSample.cs
 *@class   └ new FormExcelLineStyleSample() : Form
 *@class       └ new Excel.Application()
 *@using Excel = Microsoft.Office.Interop.Excel;
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[490] p826 / Excel / GridLine
 *         罫線を引く
 *         
 *@subject ◆interface Borders : IEnumerable
 *                       -- Microsoft.Office.Interop.Excel
 *         Borders range.Borders { get; }     [×] 'new' is not available.
 *         Border  this[XlBordersIndex Index] { get; } 
 *         Border  Borders.get_Item(XlBordersIndex Index) 
 *           └ interface Border -- Microsoft.Office.Interop.Excel    〔below〕
 *           └ enum XlBordersIndex -- Microsoft.Office.Interop.Excel 〔below〕
 *         IEnumerator  Borders.GetEnumerator() 
 *         Application  Borders.Application { get; } 
 *         XlCreator    Borders.Creator { get; } 
 *         int          Borders.Count { get; } 
 *         dynamic      Borders.Value { get; set; } 
 *         dynamic      Borders.LineStyle { get; set; } 
 *         dynamic      Borders.Weight { get; set; } 
 *         dynamic      Borders.Parent { get; } 
 *         dynamic      Borders.Color { get; set; } 
 *         dynamic      Borders.ColorIndex { get; set; } 
 *         dynamic      Borders.ThemeColor { get; set; } 
 *         dynamic      Borders.TintAndShade { get; set; } 
 *         
 *@subject ◆interface Border -- Microsoft.Office.Interop.Excel
 *         Application  Border.Application { get; } 
 *         XlCreator  Border.Creator { get; } 
 *         dynamic  Border.LineStyle { get; set; } 
 *         dynamic  Border.Weight { get; set; } 
 *         dynamic  Border.Parent { get; } 
 *         dynamic  Border.Color { get; set; } 
 *         dynamic  Border.ColorIndex { get; set; } 
 *         dynamic  Border.ThemeColor { get; set; } 
 *         dynamic  Border.TintAndShade { get; set; } 
 *
 *@subject enum XlBordersIndex : System.Enum
 *                         -- Microsoft.Office.Interop.Excel
 *         {
 *             xlDiagonalDown = 5,
 *             xlDiagonalUp = 6,
 *             xlEdgeLeft = 7,
 *             xlEdgeTop = 8,
 *             xlEdgeBottom = 9,
 *             xlEdgeRight = 10,
 *             xlInsideVertical = 11,
 *             xlInsideHorizontal = 12,
 *         }
 *
 *@subject enum XlLineStyle : System.Enum
 *                      -- Microsoft.Office.Interop.Excel
 *         {
 *             xlContinuous = 1,
 *             xlDashDot = 4,
 *             xlDashDotDot = 5,
 *             xlSlantDashDot = 13,
 *             xlLineStyleNone = -4142,
 *             xlDouble = -4119,
 *             xlDot = -4118,
 *             xlDash = -4115,
 *         }
 *         
 *@subject enum XlBorderWeight : System.Enum
 *                         -- Microsoft.Office.Interop.Excel
 *         {
 *             xlHairline = 1,
 *             xlThin = 2,
 *             xlThick = 4,
 *             xlMedium = -4138,
 *         }
 *         
 *@see ImageExcelLineStyleSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-27
 */
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelLineStyleSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelLineStyleSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelLineStyleSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelLineStyleSample : Form
    {
        private readonly Mutex mutex;
        private readonly Button button;

        public FormExcelLineStyleSample()
        {
            this.Text = "FormExcelLineStyleSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(360, 60);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelLineStyleSample");
            this.Load += new EventHandler(FormExcelLineStyleSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormExcelLineStyleSample_FormClosed);

            //---- Controls ----
            button = new Button()
            {
                Text = "Add Grid Line",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                button,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            try
            {
                Excel.Workbook wb = excelApp.Workbooks.Open(
                    Path.GetFullPath(@"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelGridFileSample.xlsx"));
                Excel.Worksheet sheet1 = (Excel.Worksheet)wb.Sheets[1];

                //---- Seek Termination / 終端を探す ----
                int columnMax = 2;
                while (sheet1.Cells[1, columnMax].Text != "")
                {
                    columnMax++;
                }//while
                columnMax--;

                int rowMax = 2;
                while(sheet1.Cells[rowMax, 1].Text != "")
                {
                    rowMax++;
                }//while
                rowMax--;

                //---- Draw GridLine ----
                Excel.Range range = 
                    (Excel.Range) sheet1.Range["A1", sheet1.Cells[rowMax, columnMax]];
                
                //Borders.LineStyle
                range.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle =
                    Excel.XlLineStyle.xlContinuous;
                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle =
                    Excel.XlLineStyle.xlContinuous;
                range.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle =
                    Excel.XlLineStyle.xlContinuous;
                range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle =
                    Excel.XlLineStyle.xlContinuous;
                range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle =
                    Excel.XlLineStyle.xlContinuous;
                range.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle =
                    Excel.XlLineStyle.xlContinuous;
                
                //Borders.Weight
                range.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight =
                    Excel.XlBorderWeight.xlThick;
                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight =
                    Excel.XlBorderWeight.xlThick;
                range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight =
                    Excel.XlBorderWeight.xlThick;
                range.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight =
                    Excel.XlBorderWeight.xlThick;
                range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight =
                    Excel.XlBorderWeight.xlThin;
                range.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight =
                    Excel.XlBorderWeight.xlThin;

                //---- Save changes ----
                MessageBox.Show("Saved Addition of GridLine.", "Notation");
                wb.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                excelApp.Quit();
            }
        }//Button_Click()

        //====== Form Event ======
        private void FormExcelLineStyleSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//Form1_Load()

        private void FormExcelLineStyleSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.Close();
        }//Form1_FormClosed()
    }//class
}
