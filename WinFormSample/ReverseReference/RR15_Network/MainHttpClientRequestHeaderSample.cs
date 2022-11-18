/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainHttpClientRequestHeaderSample.cs
 *@class   └ new FormHttpClientRequestHeaderSample() : Form
 *@class       └ new HttpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[434][435][436][437] p731-736 / HttpClient / RequestHeader
 *         RequestHeaderを指定して送信
 *         ・ContentType  送信データの種類
 *         ・User-Agent   ブラウザの種類
 *         ・Cookie       Cookie Token
 *         ・(Self-Defined Header)  自己定義の Header
 *
 *@subject ContentType  送信データの種類〔Response ContentType | NT 91〕
 *         ・Web Serverで自動判定するが、
 *           ContentTypeを指定することで、データの種類を限定できる
 *           
 *         "text/plain"        「.txt」
 *         "text/html"         「.html, .htm」
 *         "text/xml"          「.xml」
 *         "text/xsl"          「.xsl」XSLTスタイルシート
 *         "text/css"          「.css」
 *         "text/csv"
 *         "application/pdf"   「.pdf」
 *         "application/json"   JSON 
 *         "application/xml"   「.xml」
 *         "application/vnd.ms-excel" 「.xls」 Microsoft EXCEL文書 
 *         "application/octet-stream"  任意のBinaryデータ
 *         "image/jpg"         「.jpg, .jpeg」
 *         "image/png"         「.png」
 *         "image/gif"         「.gif」
 *         "video/mpeg"        「.mpg, .mpeg」
 *         
 *@subject POST送信時に ContentTypeを指定
 *         var content =  new StringContent(string content)
 *         var content.Headers = new MediaTypeHeaderValue("application/json")
 *         Task<string>  httpClient.PostAsync(string/Uri uri, HttpContent)
 *         
 *         =>〔MainHttpClientPostJsonConvertSample.cs〕
 *         
 *@subject defaultの RequestHeaderを指定
 *         HttpRequestHeaders  httpClient.DefaultRequestHeaders
 *         void                httpHeaders.Add(string name, IEnumerable<string> values)
 *         
 *@subject ＊Uaser-Agent  ブラウザの種類
 *         httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
 *         
 *@subject ＊Cookieの利用  
 *         ・HttpClientのコンストラクタ時に 引数 HttpClentHanderを new し、
 *           UseCookies = true;に設定する
 *         ・RequestHeaderに Cookie を追加する必要はない
 *         
 *         HttpClient  new HttpClient(HttpMessageHandler)
 *           └ [Argument] HttpClientHandler : HttpMessageHandler
 *         bool  httpClientHandler.UseCookie = true;
 *           
 *         [Example]
 *         client = new HttpClient(new HttpClientHandler()
 *         {
 *             UseCookies = true,
 *         });
 *         
 *@subject ＊独自の RequestHeaderを設定する場合
 *         httpClient.DefaultRequestHeaders.Add( "X-API-KEY", "XXXX-XXXX-XXXX");
 *         
 *@subject ◆class StringContent : ByteArrayContent
 *                             -- System.Net.Http
 *         + StringContent  new StringContent(string content) 
 *         + StringContent  new StringContent(string content, Encoding encoding) 
 *         + StringContent  new StringContent(string content, Encoding encoding, string mediaType) 
 *
 *@subject ◆class MediaTypeHeaderValue : ICloneable
 *                                    -- System.Net.Http.Headers
 *         + MediaTypeHeaderValue  new MediaTypeHeaderValue(string mediaType) 
 *         # MediaTypeHeaderValue  MediaTypeHeaderValue(MediaTypeHeaderValue source)
 *         
 *         + string  mediaTypeHeaderValue.MediaType { get; set; } 
 *         + string  mediaTypeHeaderValue.CharSet { get; set; } 
 *         + ICollection<NameValueHeaderValue> 
 *                   mediaTypeHeaderValue.Parameters { get; } 
 *         + static MediaTypeHeaderValue  MediaTypeHeaderValue.Parse(string input) 
 *         + static bool                  MediaTypeHeaderValue.TryParse(string input, out MediaTypeHeaderValue parsedValue) 
 *
 *@subject ◆class HttpClientHandler : HttpMessageHandler
 *                                 -- System.Net.Http
 *         + HttpClientHandler  new HttpClientHandler() 
 *         
 *         + bool  httpClientHandler.UseCookies { get; set; } 
 *         + bool  httpClientHandler.UseProxy { get; set; } 
 *         + bool  httpClientHandler.SupportsProxy { get; } 
 *         + bool  httpClientHandler.AllowAutoRedirect { get; set; } 
 *         + bool  httpClientHandler.SupportsRedirectConfiguration { get; } 
 *         + bool  httpClientHandler.UseDefaultCredentials { get; set; } 
 *         + bool  httpClientHandler.PreAuthenticate { get; set; } 
 *         + bool  httpClientHandler.SupportsAutomaticDecompression { get; } 
 *         + bool  httpClientHandler.CheckCertificateRevocationList { get; set; } 
 *         + long  httpClientHandler.MaxRequestContentBufferSize { get; set; } 
 *         + int   httpClientHandler.MaxResponseHeadersLength { get; set; } 
 *         + int   httpClientHandler.MaxConnectionsPerServer { get; set; } 
 *         + int   httpClientHandler.MaxAutomaticRedirections { get; set; } 
 *         + IDictionary<string, object>  httpClientHandler.Properties { get; } 
 *         + IWebProxy     httpClientHandler.Proxy { get; set; } 
 *         + ICredentials  httpClientHandler.DefaultProxyCredentials { get; set; } 
 *         + ICredentials  httpClientHandler.Credentials { get; set; } 
 *         + SslProtocols  httpClientHandler.SslProtocols { get; set; } 
 *         + CookieContainer          httpClientHandler.CookieContainer { get; set; } 
 *         + DecompressionMethods     httpClientHandler.AutomaticDecompression { get; set; } 
 *         + ClientCertificateOption  httpClientHandler.ClientCertificateOptions { get; set; } 
 *         + X509CertificateCollection  httpClientHandler.ClientCertificates { get; } 
 *         + static Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> 
 *                         HttpClientHandler.DangerousAcceptAnyServerCertificateValidator { get; } 
 *         + Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> 
 *                         httpClientHandler.ServerCertificateCustomValidationCallback { get; set; } 
 *         # internal Task<HttpResponseMessage>  httpClientHandler.SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) 
 *         # void  httpClientHandler.Dispose(bool disposing) 
 *
 *@see No Image (ImageHttpClientRequestHeaderSample.jpg)
 *@see 
 *@author shika
 *@date 2022-11-17
 */
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainHttpClientRequestHeaderSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormHttpClientRequestHeaderSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormHttpClientRequestHeaderSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHttpClientRequestHeaderSample : Form
    {
        private readonly HttpClient client;
        private readonly TableLayoutPanel table;
        private readonly Button button;
        private readonly TextBox textBox;

        public FormHttpClientRequestHeaderSample()
        {
            this.Text = "FormHttpClientRequestHeaderSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(480, 480);
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- HttpClient ----
            client = new HttpClient(new HttpClientHandler()
            {
                UseCookies = true,
            });
            
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
                Text = "Set ContentType",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button);

            //---- TextBox ----
            textBox = new TextBox()
            {
                Multiline = true,
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
            HttpRequestHeaders requestHeader = client.DefaultRequestHeaders;
            requestHeader.Add("User-Agent", "Mozilla/5.0");
            requestHeader.Add("X-API-KEY", "XXXX-XXXX-XXXX");
            var uri = new Uri("http://localhost:80/api/Sample");
            var obj = new
            {
                Name = "Sophia",
                Age = 24,
                Address = "Berlin",
            };

            string json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json);

            HttpResponseMessage res = await client.PostAsync(uri, content);
            textBox.Text = await res.Content.ReadAsStringAsync();
        }//Button_Click()
    }//class
}
