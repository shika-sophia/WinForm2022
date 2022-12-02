/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR18_Excel
 *@class MainHttpClientJsonXmlSample.cs
 *@class   └ new FormHttpClientJsonXmlSample() : Form
 *@class       └ new HttpClient()
 *@class       └ new Excel.Application()
 *@using Excel = Microsoft.Office.Interop.Excel
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[496] p834 / Excel, HttpClient / JsonXmlSample
 *         HttpClientでネット接続し、
 *         Webページを JSON形式, XML形式で Excelに出力 〔未完成〕
 *         
 *        【註】Webページの元データが、JSON形式, XML形式でないと、機能しない
 *         (HTMLなどを JSON, XMLに Convertするわけではない)
 *        
 *@NOTE【Exception】System.Xml.XmlException: 
 *      'src' は予期しないトークンです。予期していたトークンは '=' です。 行 40、位置 15。
 *
 *@summary 
 *         void  XDocument.Load(TextReader)
 *@subject ◆abstract class TextReader : MarshalByRefObject, IDisposable
 *                          -- System.IO
 *         + static readonly TextReader  TextReader.Null 
 *         # TextReader  TextReader()
 *           [×] 'new' is not available, but 'base()' is OK from constructor of inherited class ONLY.
 *         
 *         + static TextReader  TextReader.Synchronized(TextReader reader) 
 *         + int  textReader.Read() 
 *         + int  textReader.Read(char[] buffer, int index, int count) 
 *         + int  textReader.ReadBlock(char[] buffer, int index, int count) 
 *         + string  textReader.ReadLine() 
 *         + string  textReader.ReadToEnd() 
 *         + int  textReader.Peek() 
 *         + Task<int>  textReader.ReadAsync(char[] buffer, int index, int count) 
 *         + Task<int>  textReader.ReadBlockAsync(char[] buffer, int index, int count) 
 *         + Task<string>  textReader.ReadLineAsync() 
 *         + Task<string>  textReader.ReadToEndAsync() 
 *         + void  textReader.Close() 
 *         + void  textReader.Dispose() 
 *         # void  textReader.Dispose(bool disposing) 
 *                    ↑
 *@subject ◆class StringReader : TextReader -- System.IO
 *         + StringReader  new StringReader(string s) 
 *         + int  stringReader.Read() 
 *         + int  stringReader.Read(char[] buffer, int index, int count) 
 *         + string  stringReader.ReadLine() 
 *         + string  stringReader.ReadToEnd() 
 *         + int  stringReader.Peek() 
 *         + Task<int>  stringReader.ReadAsync(char[] buffer, int index, int count) 
 *         + Task<int>  stringReader.ReadBlockAsync(char[] buffer, int index, int count) 
 *         + Task<string>  stringReader.ReadLineAsync() 
 *         + Task<string>  stringReader.ReadToEndAsync() 
 *         + void  stringReader.Close() 
 *         # void  stringReader.Dispose(bool disposing) 
 *
 *@subject ◆class JsonTextReader : JsonReader, IJsonLineInfo
 *                              -- Newtonsoft.Json
 *         + JsonTextReader  new JsonTextReader(TextReader reader) 
 *         + int        jsonTextReader.LineNumber { get; } 
 *         + int        jsonTextReader.LinePosition { get; } 
 *         + bool       jsonTextReader.HasLineInfo() 
 *         + JsonNameTable?     jsonTextReader.PropertyNameTable { get; set; } 
 *         + IArrayPool<char>?  jsonTextReader.ArrayPool { get; set; } 
 *         
 *         ＊Method
 *         + bool       jsonTextReader.Read() 
 *         + bool?      jsonTextReader.ReadAsBoolean() 
 *         + string?    jsonTextReader.ReadAsString() 
 *         + int?       jsonTextReader.ReadAsInt32() 
 *         + double?    jsonTextReader.ReadAsDouble() 
 *         + decimal?   jsonTextReader.ReadAsDecimal() 
 *         + byte[]?    jsonTextReader.ReadAsBytes() 
 *         + DateTime?  jsonTextReader.ReadAsDateTime() 
 *         + DateTimeOffset?  jsonTextReader.ReadAsDateTimeOffset() 
 *         + void  jsonTextReader.Close() 
 *         
 *         ＊Async Method
 *         + Task<bool>       jsonTextReader.ReadAsync(CancellationToken cancellationToken = default) 
 *         + Task<bool?>      jsonTextReader.ReadAsBooleanAsync(CancellationToken cancellationToken = default) 
 *         + Task<string?>    jsonTextReader.ReadAsStringAsync(CancellationToken cancellationToken = default) 
 *         + Task<int?>       jsonTextReader.ReadAsInt32Async(CancellationToken cancellationToken = default) 
 *         + Task<double?>    jsonTextReader.ReadAsDoubleAsync(CancellationToken cancellationToken = default) 
 *         + Task<decimal?>   jsonTextReader.ReadAsDecimalAsync(CancellationToken cancellationToken = default) 
 *         + Task<byte[]?>    jsonTextReader.ReadAsBytesAsync(CancellationToken cancellationToken = default) 
 *         + Task<DateTime?>  jsonTextReader.ReadAsDateTimeAsync(CancellationToken cancellationToken = default) 
 *         + Task<DateTimeOffset?>  jsonTextReader.ReadAsDateTimeOffsetAsync(CancellationToken cancellationToken = default) 
 *
 *@subject JsonConvert, JsonSerializer, JObject, JToken, JArray
 *         => 〔RR15_Network\MainHttpClientPostJsonConvertSample.cs〕
 *         
 *@subject XDocument
 *         => 〔RR15_Network\MainHttpClientXmlDocumentSample.cs〕
 *         
 *@see (No Image) [×] ImageHttpClientJsonXmlSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-30
 */
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormHttpClientJsonXmlSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormHttpClientJsonXmlSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHttpClientJsonXmlSample : Form
    {
        private readonly HttpClient client;
        private const string url = "https://www.shuwasystem.co.jp/";
        private string htmlContent;
        private readonly Excel.Application excelApp;
        private readonly Excel.Workbook workbook;
        private readonly Excel.Worksheet sheet2;
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly TextBox textBoxUrl;
        private readonly TextBox textBoxContent;
        private readonly Button buttonJson;
        private readonly Button buttonXml;

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
            
            //---- HttpClient ----
            client = new HttpClient();

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
                Text = "Show JSON",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonJson.Click += new EventHandler(ButtonJson_Click);
            table.Controls.Add(buttonJson, 0, 1);
            
            buttonXml = new Button()
            {
                Text = "Show XML",
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

        private async void ButtonJson_Click(object sender, EventArgs e)
        {
            try
            {
                if (htmlContent == null)
                {
                    htmlContent = await ConnectHttpGetHtmlString();
                }
                textBoxContent.Text = htmlContent;

                //---- JsonSerializer / JSONの解析 ----
                //JsonSerializer json = new JsonSerializer();  //利用していない
                var reader = new JsonTextReader(new StringReader(htmlContent));
                JArray jsonAry = (JArray) JObject.ReadFrom(reader)["projects"];
                    
                //---- Excel ----
                int row = 2;
                foreach (JToken element in jsonAry)
                {
                    sheet2.Cells[row, 1] = element["id"].Value<int>();
                    sheet2.Cells[row, 2] = element["identifer"].Value<string>();
                    sheet2.Cells[row, 3] = element["name"].Value<string>();
                    sheet2.Cells[row, 4] = element["description"].Value<string>().Replace("\r\n", "");

                    row++;
                }//foreach

                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                client.CancelPendingRequests();
            }
        }//ButtonJson_Click()

        private async void ButtonXml_Click(object sender, EventArgs e)
        {
            try
            {
                if(htmlContent == null)
                {
                    htmlContent = await ConnectHttpGetHtmlString();
                }

                //---- XDocument / XMLの解析 ----
                XDocument doc = XDocument.Load(new StringReader(htmlContent));

                //---- Excel ----
                int row = 2;
                foreach (XElement element in doc.Root.Elements())
                {
                    sheet2.Cells[row, 1] = element.Element(XName.Get("id")).Value;
                    sheet2.Cells[row, 2] = element.Element(XName.Get("identifer")).Value;
                    sheet2.Cells[row, 3] = element.Element(XName.Get("name")).Value;
                    sheet2.Cells[row, 4] = element.Element(XName.Get("description")).Value;

                    row++;
                }//foreach

                excelApp.Visible = true;
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, ex.GetType().Name);
            //}
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
            //workbook.Save();
            excelApp.Quit();
            mutex.Close();
        }//Form1_FormClosed()
    }//class
}
