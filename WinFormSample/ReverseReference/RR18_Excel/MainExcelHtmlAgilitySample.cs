/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainExcelHtmlAgilitySample.cs
 *@class   └ new FormExcelHtmlAgilitySample() : Form
 *@class       └ new HttpClient()
 *@class       └ new Excel.Application()
 *@class       └ new HtmlAgilityPack.HtmlDocument()
 *@using Excel = Microsoft.Office.Interop.Excel;
 *@using HtmlAgilityPack = 
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[499] p843 / Excel / HtmlAgilityPack
 *         HtmlAgilityPackをインストール
 *         ・HtmlDocument.LoadHtml()  HTMLデータを解析できる
 *         ・DOMツリーを探索可能 ??
 *         
 *@prepare HtmlAgilityPack Install
 *         NuGet
 *         PM> Install-Package HtmlAgilityPack
 *           :
 *         パッケージ 'HtmlAgilityPack.1.11.46' を 'packages.config' に追加しました
 *         'HtmlAgilityPack 1.11.46' が WinFormGUI に正常にインストールされました
 *
 *@subject ◆interface IXPathNavigable -- System.Xml.XPath
 *         XPathNavigator  IXPathNavigable.CreateNavigator() 
 *
 *@subject ◆class HtmlDocument : IXPathNavigable
 *                            -- HtmlAgilityPack
 *         + HtmlDocument  new HtmlDocument() 
 *         
 *         ＊Field
 *         + string  htmlDocument.Text 
 *         + int     htmlDocument.OptionExtractErrorSourceTextMaxLength 
 *         + Encoding  htmlDocument.OptionDefaultStreamEncoding 
 *         + int     htmlDocument.OptionMaxNestedChildNodes 
 *         + string  htmlDocument.OptionStopperNodeName 
 *         + bool    htmlDocument.OptionWriteEmptyNodes 
 *         + bool    htmlDocument.OptionUseIdAttribute 
 *         + bool    htmlDocument.OptionDefaultUseOriginalName 
 *         + bool    htmlDocument.OptionReadEncoding 
 *         + bool    htmlDocument.OptionOutputOriginalCase 
 *         + AttributeValueQuote?  htmlDocument.GlobalAttributeValueQuote 
 *         + bool    htmlDocument.OptionOutputOptimizeAttributeValues 
 *         + bool    htmlDocument.OptionPreserveXmlNamespaces 
 *         + bool    htmlDocument.OptionOutputAsXml 
 *         + bool    htmlDocument.OptionOutputUpperCase 
 *         + bool    htmlDocument.OptionExtractErrorSourceText 
 *         + bool    htmlDocument.OptionXmlForceOriginalComment 
 *         + bool    htmlDocument.DisableServerSideCode 
 *         + bool    htmlDocument.OptionFixNestedTags 
 *         + bool    htmlDocument.OptionEmptyCollection 
 *         + bool    htmlDocument.OptionComputeChecksum 
 *         + bool    htmlDocument.OptionCheckSyntax 
 *         + bool    htmlDocument.OptionAutoCloseOnEnd 
 *         + bool    htmlDocument.OptionAddDebuggingAttributes 
 *         + bool    htmlDocument.BackwardCompatibility 
 *         
 *         ＊Property
 *         + static int  HtmlDocument.MaxDepthLevel { get; set; } 
 *         + static Action<HtmlDocument>  HtmlDocument.DefaultBuilder { get; set; } 
 *         + static bool  HtmlDocument.DisableBehaviorTagP { get; set; } 
 *         + string  htmlDocument.ParsedText { get; } 
 *         + int  htmlDocument.CheckSum { get; } 
 *         + HtmlNode  htmlDocument.DocumentNode { get; } 
 *         + IEnumerable<HtmlParseError>  htmlDocument.ParseErrors { get; } 
 *         + Action<HtmlDocument>  htmlDocument.ParseExecuting { get; set; } 
 *         + Encoding  htmlDocument.Encoding { get; } 
 *         + Encoding  htmlDocument.DeclaredEncoding { get; } 
 *         + string  htmlDocument.Remainder { get; } 
 *         + int  htmlDocument.RemainderOffset { get; } 
 *         + Encoding  htmlDocument.StreamEncoding { get; } 
 *         
 *         ＊Method
 *         + static string  HtmlDocument.GetXmlName(string name) 
 *         + static string  HtmlDocument.GetXmlName(string name, bool isAttribute, bool preserveXmlNamespaces) 
 *         + static string  HtmlDocument.HtmlEncode(string html) 
 *         + static bool  HtmlDocument.IsWhiteSpace(int c) 
 *         + HtmlAttribute  htmlDocument.CreateAttribute(string name) 
 *         + HtmlAttribute  htmlDocument.CreateAttribute(string name, string value) 
 *         + HtmlCommentNode  htmlDocument.CreateComment() 
 *         + HtmlCommentNode  htmlDocument.CreateComment(string comment) 
 *         + HtmlNode  htmlDocument.CreateElement(string name) 
 *         + XPathNavigator  htmlDocument.CreateNavigator() 
 *         + HtmlTextNode  htmlDocument.CreateTextNode(string text) 
 *         + HtmlTextNode  htmlDocument.CreateTextNode() 
 *         + Encoding  htmlDocument.DetectEncoding(Stream stream) 
 *         + Encoding  htmlDocument.DetectEncoding(string path) 
 *         + Encoding  htmlDocument.DetectEncoding(Stream stream, bool checkHtml) 
 *         + Encoding  htmlDocument.DetectEncoding(TextReader reader) 
 *         + void  htmlDocument.DetectEncodingAndLoad(string path) 
 *         + void  htmlDocument.DetectEncodingAndLoad(string path, bool detectEncoding) 
 *         + Encoding  htmlDocument.DetectEncodingHtml(string html) 
 *         + HtmlNode  htmlDocument.GetElementbyId(string id) 
 *         + void  htmlDocument.Load(string path) 
 *         + void  htmlDocument.Load(string path, bool detectEncodingFromByteOrderMarks) 
 *         + void  htmlDocument.Load(string path, Encoding encoding) 
 *         + void  htmlDocument.Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks) 
 *         + void  htmlDocument.Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize) 
 *         + void  htmlDocument.Load(Stream stream) 
 *         + void  htmlDocument.Load(Stream stream, bool detectEncodingFromByteOrderMarks) 
 *         + void  htmlDocument.Load(Stream stream, Encoding encoding) 
 *         + void  htmlDocument.Load(TextReader reader) 
 *         + void  htmlDocument.Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize) 
 *         + void  htmlDocument.Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks) 
 *         + void  htmlDocument.LoadHtml(string html) 
 *         + void  htmlDocument.Save(Stream outStream) 
 *         + void  htmlDocument.Save(string filename, Encoding encoding) 
 *         + void  htmlDocument.Save(string filename) 
 *         + void  htmlDocument.Save(Stream outStream, Encoding encoding) 
 *         + void  htmlDocument.Save(StreamWriter writer) 
 *         + void  htmlDocument.Save(TextWriter writer) 
 *         + void  htmlDocument.Save(XmlWriter writer) 
 *         + void  htmlDocument.UseAttributeOriginalName(string tagName) 
 *
 *@see ImageExcelHtmlAgilitySample.jpg
 *@see 
 *@author shika
 *@date 2022-12-04
 */
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelHtmlAgilitySample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelHtmlAgilitySample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelHtmlAgilitySample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelHtmlAgilitySample : Form
    {
        private readonly HttpClient client;
        private const string url = "https://www.shuwasystem.co.jp/";
        private readonly Excel.Application excelApp;
        private readonly Excel.Workbook workbook;
        private readonly Excel.Worksheet sheet1;
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly Button button;
        private readonly TextBox textBox;

        public FormExcelHtmlAgilitySample()
        {
            this.Text = "FormExcelHtmlAgilitySample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(480, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelHtmlAgilitySample");
            this.Load += new EventHandler(FormExcelHtmlAgilitySample_Load);
            this.FormClosed += new FormClosedEventHandler(FormExcelHtmlAgilitySample_FormClosed);

            //---- HttpClient ----
            client = new HttpClient();

            //---- Excel ----
            string path = Path.GetFullPath(
                @"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelBookInfoSample.xlsx");
            excelApp = new Excel.Application();

            if (!File.Exists(path))
            {
                Excel.Workbook newWorkbook = excelApp.Workbooks.Add();
                newWorkbook.SaveAs(Filename: path);
            }

            workbook = excelApp.Workbooks.Open(path);
            sheet1 = workbook.Sheets[1];

            //---- Controls ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            button = new Button()
            {
                Text = "Get New Book Information",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button);

            textBox = new TextBox()
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(textBox);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private async void Button_Click(object sender, EventArgs e)
        {
            try
            {
                //---- HttpClient Web Connect ----
                string htmlContent = await client.GetStringAsync(url);

                //---- HtmlDocument ----
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);
                  
                //(Editing...)

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                client.CancelPendingRequests();
            }
        }//Button_Click()

        //====== Form Event ======
        private void FormExcelHtmlAgilitySample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//Form1_Load()

        private void FormExcelHtmlAgilitySample_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Dispose();
            excelApp.Quit();
            mutex.Close();
        }//Form1_FormClosed()
    }//class
}
