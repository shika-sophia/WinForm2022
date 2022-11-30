/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR18_Excel
 *@class MainHttpClientJsonXmlSample.cs
 *@class   └ new FormHttpClientJsonXmlSample() : Form
 *@class       └ new Excel.Application()
 *@class       └ new HttpClient()
 *@using Excel = Microsoft.Office.Interop.Excel
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[496] p834 / Excel, HttpClient / JsonXmlSample
 *@subject 
 *
 *@see ImageHttpClientJsonXmlSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-30
 */
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelHttpClientJsonXmlSample
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Console.WriteLine("new FormHttpClientJsonXmlSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormHttpClientJsonXmlSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHttpClientJsonXmlSample : Form
    {
        private readonly Excel.Application excelApp;
        private readonly Excel.Workbook workbook;
        private readonly Excel.Worksheet sheet2;
        private readonly HttpClient client;
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly TextBox textBoxUrl;
        private readonly TextBox textBoxContent;
        private readonly Button buttonJson;
        private readonly Button buttonXml;
        private const string url = "https://www.shuwasystem.co.jp/";

        public FormHttpClientJsonXmlSample()
        {
            this.Text = "FormHttpClientJsonXmlSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormHttpClientJsonXmlSample");
            this.Load += new EventHandler(FormHttpClientJsonXmlSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormHttpClientJsonXmlSample_FormClosed);

            //---- Excel Application ----
            excelApp = new Excel.Application();
            workbook = excelApp.Workbooks.Open(
                Path.GetFullPath(@"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelFileSample.xlsx"));
            sheet2 = (Excel.Worksheet)workbook.Sheets[2];

            if (sheet2.Range["A1"].Value != "ID")
            {
                sheet2.Range["A1"].Value = "ID";
                sheet2.Range["B1"].Value = "Tag";
                sheet2.Range["C1"].Value = "Project";
                sheet2.Range["D1"].Value = "Description";
            }

            //---- HttpClient ----
            client = new HttpClient();

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
                Text = "URL: ",
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(label, 0, 0);

            textBoxUrl = new TextBox()
            {
                Text = url,
                Multiline = false,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxUrl, 1, 0);

            buttonJson = new Button()
            {
                Text = "Convert to JSON",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonJson.Click += new EventHandler(ButtonJson_Click);
            table.Controls.Add(buttonJson, 0, 1);
            
            buttonXml = new Button()
            {
                Text = "Convert to XML",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonXml.Click += new EventHandler(ButtonXml_Click);
            table.Controls.Add(buttonXml, 2, 1);

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

        private void ButtonJson_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void ButtonXml_Click(object sender, EventArgs e)
        {
            try
            {
                string htmlContent = await ConnectHttpGetHtmlString();

                //---- XDocument / XMLの解析 ----
                XDocument doc = XDocument.Load(new StringReader(htmlContent));

                //---- Excel ----
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                client.CancelPendingRequests();
            }
        }//ButtonXml_Click()

        private async Task<string> ConnectHttpGetHtmlString() //self defined
        {
            //---- HttpClient / urlに接続 ----
            string htmlContent = await client.GetStringAsync(url);

            textBoxContent.Text = htmlContent;
            return htmlContent;
        }//TryGetHtmlStringAsync()

        //====== Form Event ======
        private void FormHttpClientJsonXmlSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//Form1_Load()

        private void FormHttpClientJsonXmlSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Dispose();
            workbook.Save();
            excelApp.Quit();
            mutex.Close();
        }//Form1_FormClosed()
    }//class
}
