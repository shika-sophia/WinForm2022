/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainHttpClientUploadFileSample.cs
 *@class   └ new FormHttpClientUploadFileSample() : Form
 *@class       └ new HttpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[278] p723 / HttpClient UploadFile
 *         Task<HttpRequestMessage>  httpClient.PostAsync(string requestUri, HttpContent)
 *         
 *         HttpContentHeaders             streamContent.Headers  
 *         ContentDispositionHeaderValue  httpContentHeaders.ContentDisposition
 *           =>〔MainHttpClientSample.cs〕
 *           
 *NOTE【Problem】
 *     Though 'upload.php' is not existed, 
 *     somewhy it success to upload to Apache locallhost.
 
 *@subject ◆class MultipartContent : HttpContent, IEnumerable<HttpContent>, IEnumerable
 *                                -- System.Net.Http
 *         + MultipartContent  new MultipartContent() 
 *         + MultipartContent  new MultipartContent(string subtype) 
 *         + MultipartContent  new MultipartContent(string subtype, string boundary) 
 *         
 *         + IEnumerator<HttpContent>  multipartContent.GetEnumerator() 
 *         + void           multipartContent.Add(HttpContent content) 
 *         # Task           multipartContent.SerializeToStreamAsync(Stream stream, TransportContext context) 
 *         # internal bool  multipartContent.TryComputeLength(out long length) 
 *         # void           multipartContent.Dispose(bool disposing) 
 *             ↑
 *@subject ◆class MultipartFormDataContent : MultipartContent
 *                                        -- System.Net.Http
 *         + MultipartFormDataContent  new MultipartFormDataContent() 
 *         + MultipartFormDataContent  new MultipartFormDataContent(string boundary) 
 *         
 *         + void  multipartFormDataContent.Add(HttpContent content) 
 *         + void  multipartFormDataContent.Add(HttpContent content, string name) 
 *         + void  multipartFormDataContent.Add(HttpContent content, string name, string fileName) 
 *
 *@subject ◆class StreamContent : HttpContent
 *                             -- System.Net.Http
 *         + StreamContent  new StreamContent(Stream content) 
 *         + StreamContent  new StreamContent(Stream content, int bufferSize) 
 *         
 *         # Task<Stream>   streamContent.CreateContentReadStreamAsync() 
 *         # Task           streamContent.SerializeToStreamAsync(Stream stream, TransportContext context) 
 *         # internal bool  streamContent.TryComputeLength(out long length) 
 *         # void           streamContent.Dispose(bool disposing) 
 *
 *@subject ◆class ContentDispositionHeaderValue : ICloneable
 *                                             -- System.Net.Http.Headers
 *         + ContentDispositionHeaderValue  new ContentDispositionHeaderValue(string dispositionType) 
 *         # ContentDispositionHeaderValue  ContentDispositionHeaderValue(ContentDispositionHeaderValue source)
 *           [×] 'new' is not available, but 'base()' is OK from constructor of inherited class ONLY.
 *         
 *         + ICollection<NameValueHeaderValue>  
 *                   contentDispositionHeaderValue.Parameters { get; } 
 *         + string  contentDispositionHeaderValue.Name { get; set; } 
 *         + string  contentDispositionHeaderValue.FileName { get; set; } 
 *         + string  contentDispositionHeaderValue.FileNameStar { get; set; } 
 *         + string  contentDispositionHeaderValue.DispositionType { get; set; } 
 *         + DateTimeOffset?  contentDispositionHeaderValue.CreationDate { get; set; } 
 *         + DateTimeOffset?  contentDispositionHeaderValue.ModificationDate { get; set; } 
 *         + DateTimeOffset?  contentDispositionHeaderValue.ReadDate { get; set; } 
 *         + long?   contentDispositionHeaderValue.Size { get; set; } 
 *         + static  ContentDispositionHeaderValue
 *                         ContentDispositionHeaderValue.Parse(string input) 
 *         + static  bool  ContentDispositionHeaderValue.TryParse(string input, out ContentDispositionHeaderValue parsedValue) 
 *
 *@see ImageHttpClientUploadFileSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-14
 */
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainHttpClientUploadFileSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormHttpClientUploadFileSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormHttpClientUploadFileSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHttpClientUploadFileSample : Form
    {
        private readonly HttpClient client;
        private readonly TableLayoutPanel table;
        private readonly Button button;
        private readonly TextBox textBox;

        public FormHttpClientUploadFileSample()
        {
            this.Text = "FormHttpClientUploadFileSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(480, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- HttpClient ----
            client = new HttpClient();

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            //---- Button ----
            button = new Button()
            {
                Text = "Upload File",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button);

            //---- TextBox ----
            textBox = new TextBox()
            {
                Multiline = true,
                Height = 60,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBox);

            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private async void Button_Click(object sender, EventArgs e)
        {
            try
            {
                var content = new MultipartFormDataContent();
                string dir = "../../WinFormSample/";
                string fileName = "TriangularRatioReference.txt";
                string path = dir + fileName;

                var streamContent = new StreamContent(File.OpenRead(path));
                streamContent.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName,
                };
                textBox.Text = $"Found File of '{fileName}'. {Environment.NewLine}";

                content.Add(streamContent);
                await client.PostAsync("http://localhost:80/upload.php", content);
                // Actually 'upload.php' is not exist.
                textBox.Text += $"Uploaded File of '{fileName}' \nto 'upload.php'. {Environment.NewLine}";
            }
            catch (Exception ex)
            {
                textBox.Text += $"{ex.GetType()}\n";

                MessageBox.Show(ex.Message);
            }
        }//Button_Click()
    }//class
}
