/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainWebClientSample.cs
 *@class   └ new FormWebClientSample() : Form
 *@class       └ new WebClient() : Component -- System.Net.
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[425]改 p719 / WebClientSample
 *         WebClient  Sync:  CostTime = 1188 milliSeconds
 *         WebClient  Async: CostTime = 1028 milliSeconds
 *         HttpClient Async: CostTime = 1278 milliSeconds
 *         
 *@subject ◆class WebClient : Component  -- System.Net
 *         + WebClient  new WebClient() 
 *         
 *         ＊Property
 *         + string               webClient.BaseAddress { get; set; } 
 *         + WebHeaderCollection  webClient.Headers { get; set; } 
 *         + WebHeaderCollection  webClient.ResponseHeaders { get; } 
 *         + NameValueCollection  webClient.QueryString { get; set; } 
 *         + Encoding             webClient.Encoding { get; set; } 
 *         + IWebProxy            webClient.Proxy { get; set; } 
 *         + RequestCachePolicy   webClient.CachePolicy { get; set; } 
 *         + ICredentials         webClient.Credentials { get; set; } 
 *         + bool                 webClient.UseDefaultCredentials { get; set; } 
 *         + bool                 webClient.IsBusy { get; } 
 *         + bool                 webClient.AllowReadStreamBuffering { get; set; } 
 *         + bool                 webClient.AllowWriteStreamBuffering { get; set; } 
 *         
 *         ＊Method
 *         + Stream  webClient.OpenRead(Uri address) 
 *         + Stream  webClient.OpenRead(string address) 
 *         + Stream  webClient.OpenWrite(string address) 
 *         + Stream  webClient.OpenWrite(string address, string method) 
 *         + Stream  webClient.OpenWrite(Uri address, string method) 
 *         + Stream  webClient.OpenWrite(Uri address) 
 *         
 *         + string  webClient.DownloadString(string address) 
 *         + string  webClient.DownloadString(Uri address) 
 *         + byte[]  webClient.DownloadData(string address) 
 *         + byte[]  webClient.DownloadData(Uri address) 
 *         + void    webClient.DownloadFile(string address, string fileName) 
 *         + void    webClient.DownloadFile(Uri address, string fileName) 
 *
 *         + string  webClient.UploadString(string address, string data) 
 *         + string  webClient.UploadString(Uri address, string data) 
 *         + string  webClient.UploadString(string address, string method, string data) 
 *         + string  webClient.UploadString(Uri address, string method, string data) 
 *         + byte[]  webClient.UploadValues(string address, NameValueCollection data) 
 *         + byte[]  webClient.UploadValues(string address, string method, NameValueCollection data) 
 *         + byte[]  webClient.UploadValues(Uri address, NameValueCollection data) 
 *         + byte[]  webClient.UploadValues(Uri address, string method, NameValueCollection data) 
 *         + byte[]  webClient.UploadData(string address, string method, byte[] data) 
 *         + byte[]  webClient.UploadData(string address, byte[] data) 
 *         + byte[]  webClient.UploadData(Uri address, string method, byte[] data) 
 *         + byte[]  webClient.UploadData(Uri address, byte[] data) 
 *         + byte[]  webClient.UploadFile(string address, string method, string fileName) 
 *         + byte[]  webClient.UploadFile(Uri address, string method, string fileName) 
 *         + byte[]  webClient.UploadFile(string address, string fileName) 
 *         + byte[]  webClient.UploadFile(Uri address, string fileName)
 *         
 *         # WebRequest   webClient.GetWebRequest(Uri address) 
 *         # WebResponse  webClient.GetWebResponse(WebRequest request) 
 *         # WebResponse  webClient.GetWebResponse(WebRequest request, IAsyncResult result) 
 *         
 *         ＊Async Method
 *         + void          webClient.OpenReadAsync(Uri address) 
 *         + void          webClient.OpenReadAsync(Uri address, object userToken) 
 *         + Task<Stream>  webClient.OpenReadTaskAsync(string address) 
 *         + Task<Stream>  webClient.OpenReadTaskAsync(Uri address) 
 *         + void          webClient.OpenWriteAsync(Uri address) 
 *         + void          webClient.OpenWriteAsync(Uri address, string method) 
 *         + void          webClient.OpenWriteAsync(Uri address, string method, object userToken) 
 *         + Task<Stream>  webClient.OpenWriteTaskAsync(string address) 
 *         + Task<Stream>  webClient.OpenWriteTaskAsync(string address, string method)
 *         + Task<Stream>  webClient.OpenWriteTaskAsync(Uri address) 
 *         + Task<Stream>  webClient.OpenWriteTaskAsync(Uri address, string method) 
 *         
 *         + void          webClient.DownloadStringAsync(Uri address) 
 *         + void          webClient.DownloadStringAsync(Uri address, object userToken) 
 *         + Task<string>  webClient.DownloadStringTaskAsync(string address) 
 *         + Task<string>  webClient.DownloadStringTaskAsync(Uri address) 
 *         + void          webClient.DownloadDataAsync(Uri address) 
 *         + void          webClient.DownloadDataAsync(Uri address, object userToken) 
 *         + Task<byte[]>  webClient.DownloadDataTaskAsync(string address) 
 *         + Task<byte[]>  webClient.DownloadDataTaskAsync(Uri address) 
 *         + void          webClient.DownloadFileAsync(Uri address, string fileName) 
 *         + void          webClient.DownloadFileAsync(Uri address, string fileName, object userToken) 
 *         + Task          webClient.DownloadFileTaskAsync(string address, string fileName) 
 *         + Task          webClient.DownloadFileTaskAsync(Uri address, string fileName) 
 *         
 *         + void          webClient.UploadStringAsync(Uri address, string data) 
 *         + void          webClient.UploadStringAsync(Uri address, string method, string data) 
 *         + void          webClient.UploadStringAsync(Uri address, string method, string data, object userToken) 
 *         + Task<string>  webClient.UploadStringTaskAsync(string address, string data) 
 *         + Task<string>  webClient.UploadStringTaskAsync(string address, string method, string data) 
 *         + Task<string>  webClient.UploadStringTaskAsync(Uri address, string data) 
 *         + Task<string>  webClient.UploadStringTaskAsync(Uri address, string method, string data)
 *         + void          webClient.UploadValuesAsync(Uri address, NameValueCollection data) 
 *         + void          webClient.UploadValuesAsync(Uri address, string method, NameValueCollection data) 
 *         + void          webClient.UploadValuesAsync(Uri address, string method, NameValueCollection data, object userToken) 
 *         + Task<byte[]>  webClient.UploadValuesTaskAsync(string address, NameValueCollection data) 
 *         + Task<byte[]>  webClient.UploadValuesTaskAsync(string address, string method, NameValueCollection data) 
 *         + Task<byte[]>  webClient.UploadValuesTaskAsync(Uri address, NameValueCollection data) 
 *         + Task<byte[]>  webClient.UploadValuesTaskAsync(Uri address, string method, NameValueCollection data) 
 *         + void          webClient.UploadDataAsync(Uri address, byte[] data) 
 *         + void          webClient.UploadDataAsync(Uri address, string method, byte[] data) 
 *         + void          webClient.UploadDataAsync(Uri address, string method, byte[] data, object userToken) 
 *         + Task<byte[]>  webClient.UploadDataTaskAsync(string address, byte[] data) 
 *         + Task<byte[]>  webClient.UploadDataTaskAsync(string address, string method, byte[] data) 
 *         + Task<byte[]>  webClient.UploadDataTaskAsync(Uri address, byte[] data) 
 *         + Task<byte[]>  webClient.UploadDataTaskAsync(Uri address, string method, byte[] data) 
 *         + void          webClient.UploadFileAsync(Uri address, string fileName) 
 *         + void          webClient.UploadFileAsync(Uri address, string method, string fileName) 
 *         + void          webClient.UploadFileAsync(Uri address, string method, string fileName, object userToken) 
 *         + Task<byte[]>  webClient.UploadFileTaskAsync(string address, string fileName) 
 *         + Task<byte[]>  webClient.UploadFileTaskAsync(string address, string method, string fileName) 
 *         + Task<byte[]>  webClient.UploadFileTaskAsync(Uri address, string fileName) 
 *         + Task<byte[]>  webClient.UploadFileTaskAsync(Uri address, string method, string fileName)
 *         + void          webClient.CancelAsync() 
 *         
 *         ＊Event 
 *         + event OpenReadCompletedEventHandler        webClient.OpenReadCompleted 
 *         + event OpenWriteCompletedEventHandler       webClient.OpenWriteCompleted 
 *         + event WriteStreamClosedEventHandler        webClient.WriteStreamClosed 
 *         + event DownloadStringCompletedEventHandler  webClient.DownloadStringCompleted 
 *         + event DownloadDataCompletedEventHandler    webClient.DownloadDataCompleted 
 *         + event DownloadProgressChangedEventHandler  webClient.DownloadProgressChanged 
 *         + event UploadStringCompletedEventHandler    webClient.UploadStringCompleted 
 *         + event UploadValuesCompletedEventHandler    webClient.UploadValuesCompleted 
 *         + event UploadDataCompletedEventHandler      webClient.UploadDataCompleted 
 *         + event UploadFileCompletedEventHandler      webClient.UploadFileCompleted 
 *         + event UploadProgressChangedEventHandler    webClient.UploadProgressChanged 
 *         + event AsyncCompletedEventHandler           webClient.DownloadFileCompleted 
 *
 *@see ImageWebClientSample.jpg
 *@see MainHttpClientSample.cs
 *@author shika
 *@date 2022-11-12
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainWebClientSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormWebClientSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormWebClientSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormWebClientSample : Form
    {
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly TextBox textBoxUrl;
        private readonly TextBox textBoxBody;
        private readonly Button button;

        public FormWebClientSample()
        {
            this.Text = "FormWebClientSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 480);
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 3,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 90f));

            //---- Label ----
            label = new Label()
            {
                Text = "URL: ",
                TextAlign = ContentAlignment.MiddleRight,
                Margin = new Padding(10),
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(label, 0, 0);

            //---- TextBox ----
            textBoxUrl = new TextBox()
            {
                Text = "https://www.shuwasystem.co.jp/",
                Multiline = false,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxUrl, 1, 0);

            textBoxBody = new TextBox()
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxBody, 0, 1);
            table.SetColumnSpan(textBoxBody, table.ColumnCount);

            //---- Button ----
            button = new Button()
            {
                Text = "Submit",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button, 2, 0);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private /*async*/ void Button_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var client = new WebClient();
            var bld = new StringBuilder();

            try
            {
                using (Stream stream = client.OpenRead(textBoxUrl.Text))                  // Sync:
                //using (Stream stream = await client.OpenReadTaskAsync(textBoxUrl.Text)) // Async:  when use, change 'await' Button_Click() too.
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        bld.Append(reader.ReadLine());
                        bld.Append(Environment.NewLine);
                    }//while

                    textBoxBody.Text = bld.ToString();

                    reader.Close();
                    stream.Close();
                }//using
            }
            catch (Exception ex)
            {
                textBoxBody.Text =
                    $"{ex.GetType()}:{Environment.NewLine}" +
                    $"{ex.Message}{Environment.NewLine}";
            }
            finally
            {
                sw.Stop();
            }

            Console.WriteLine(
                $"WebClient Sync: CostTime = {sw.ElapsedMilliseconds} milliSeconds");
                //$"WebClient Async: CostTime = {sw.ElapsedMilliseconds} milliSeconds");
        }//Button_Click()
    }//class
}
