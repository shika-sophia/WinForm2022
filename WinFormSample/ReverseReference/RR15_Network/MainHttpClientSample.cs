/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainHttpClientSample.cs
 *@class   └ new FormHttpClientSample() : Form
 *@class       └ new HttpClient() : HttpMessageInvoker  -- System.Net.Http
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[425]-[426] p719 / HttpClient
 *         ・HTTPの開発者 Henrik F Nielson による HTTPの規格に準拠したAPI設計
 *         ・WebClientより、HTTPに適合した API = HttpClientを優先的に利用すべき
 *         ・このプログラムでもパフォーマンスを計測してみた
 *             WebClient  Sync:  CostTime = 1188 milliSeconds
 *             WebClient  Async: CostTime = 1028 milliSeconds
 *             HttpClient Async: CostTime = 1278 milliSeconds
 *         ・using() で利用すべきではない。Dispose()しない。
 *           https://qiita.com/superriver/items/91781bca04a76aec7dc0
 *           => 〔Article_HttpClient_DontUseUsingDispose.txt〕
 *           
 *           MSDN: HttpClientは、1回インスタンス化し、
 *           アプリケーションの有効期間全体に再利用することを目的としています。
 *           すべての要求に対して HttpClient クラスをインスタンス化すると、
 *           大量の読み込みで使用可能な Socket の数が枯渇します。
 *           これにより、SocketException エラーが発生します。
 *           
 *         ・It connect to Web page and read HTML source.
 *         
 *         ・async - await: asynchronous
 *             While it try to be connecting, UI event can be accepted.
 *
 *         ・WebClient   syncronous and asynchronous
 *           HttpClient  asynchronous ?
 *           => 〔Article_HttpClient_WebClient.txt〕
 *               Notation:
 *               The Article said the oppsite conclusion to above.
 *               But I watched these APIs in Visual Studio Reference〔as below〕, 
 *               HttpClient has only asynchronous Method.
 *               
 *               It is interesting that:
 *               The Article wrote each performances per milliseccons 
 *               about WebCient synchronous and HttpClient synchronous, asynchronous.
 *           
 *@subject To read Web page
 *         HttpCient  client = new HttpCient();
 *         
 *         ＊synchronous
 *         Task<Stream> streamTask = client.GetStreamAsync(string address);
 *         TResult      Task<TResult>.Result
 *         
 *         ＊asynchronous
 *         Stream stream = await client.GetStreamAsync(string address);
 *         
 *         ＊StreamReader
 *         StreamReader reader = new StreamReader(Stream);
 */
#region -> HttpClient
/*
 *@subject ◆class HttpClient : HttpMessageInvoker
 *                          -- System.Net.Http
 *         + HttpClient  new HttpClient() 
 *         + HttpClient  new HttpClient(HttpMessageHandler) 
 *         + HttpClient  new HttpClient(HttpMessageHandler, bool disposeHandler) 
 *             └ abstract class HttpMessageHandler 〔below〕
 *             
 *         + Uri         httpClient.BaseAddress { get; set; } 
 *         + long        httpClient.MaxResponseContentBufferSize { get; set; } 
 *         + TimeSpan    httpClient.Timeout { get; set; } 
 *         + HttpRequestHeaders  httpClient.DefaultRequestHeaders { get; } 
 *            └ class HttpRequestHeaders 〔below〕
 *            
 *         + Task<HttpResponseMessage>  httpClient.GetAsync(string requestUri) 
 *         + Task<HttpResponseMessage>  httpClient.GetAsync(string requestUri, CancellationToken) 
 *         + Task<HttpResponseMessage>  httpClient.GetAsync(string requestUri, HttpCompletionOption) 
 *         + Task<HttpResponseMessage>  httpClient.GetAsync(string requestUri, HttpCompletionOption, CancellationToken) 
 *         + Task<HttpResponseMessage>  httpClient.GetAsync(Uri requestUri) 
 *         + Task<HttpResponseMessage>  httpClient.GetAsync(Uri requestUri, CancellationToken) 
 *         + Task<HttpResponseMessage>  httpClient.GetAsync(Uri requestUri, HttpCompletionOption) 
 *         + Task<HttpResponseMessage>  httpClient.GetAsync(Uri requestUri, HttpCompletionOption, CancellationToken) 
 *             └ class HttpResponseMessage 〔below〕
 *             └ enum  HttpCompletionOption  -- System.Net.Http
 *               {
 *                  ResponseContentRead = 0,  //コンテンツを含む全体の応答を読んだ後に完了
 *                  ResponseHeadersRead = 1   //応答が使用できる状態になった後か、ヘッダーが読み取られた後すぐに完了
 *               }
 *               
 *         + Task<HttpResponseMessage>  httpClient.PostAsync(string requestUri, HttpContent) 
 *         + Task<HttpResponseMessage>  httpClient.PostAsync(string requestUri, HttpContent, CancellationToken) 
 *         + Task<HttpResponseMessage>  httpClient.PostAsync(Uri requestUri, HttpContent) 
 *         + Task<HttpResponseMessage>  httpClient.PostAsync(Uri requestUri, HttpContent, CancellationToken) 
 *              └ abstract class HttpContent 〔below〕
 *              
 *         + Task<HttpResponseMessage>  httpClient.PutAsync(string requestUri, HttpContent) 
 *         + Task<HttpResponseMessage>  httpClient.PutAsync(string requestUri, HttpContent, CancellationToken) 
 *         + Task<HttpResponseMessage>  httpClient.PutAsync(Uri requestUri, HttpContent content) 
 *         + Task<HttpResponseMessage>  httpClient.PutAsync(Uri requestUri, HttpContent, CancellationToken) 
 *         
 *         + Task<HttpResponseMessage>  httpClient.SendAsync(HttpRequestMessage request) 
 *         + Task<HttpResponseMessage>  httpClient.SendAsync(HttpRequestMessage request, CancellationToken) 
 *         + Task<HttpResponseMessage>  httpClient.SendAsync(HttpRequestMessage request, HttpCompletionOption) 
 *         + Task<HttpResponseMessage>  httpClient.SendAsync(HttpRequestMessage request, HttpCompletionOption, CancellationToken) 
 *         
 *         + Task<HttpResponseMessage>  httpClient.DeleteAsync(string requestUri) 
 *         + Task<HttpResponseMessage>  httpClient.DeleteAsync(string requestUri, CancellationToken) 
 *         + Task<HttpResponseMessage>  httpClient.DeleteAsync(Uri requestUri) 
 *         + Task<HttpResponseMessage>  httpClient.DeleteAsync(Uri requestUri, CancellationToken) 
 *         
 *         + Task<byte[]>  httpClient.GetByteArrayAsync(string requestUri) 
 *         + Task<byte[]>  httpClient.GetByteArrayAsync(Uri requestUri) 
 *         + Task<Stream>  httpClient.GetStreamAsync(string requestUri) 
 *         + Task<Stream>  httpClient.GetStreamAsync(Uri requestUri) 
 *         + Task<string>  httpClient.GetStringAsync(string requestUri) 
 *         + Task<string>  httpClient.GetStringAsync(Uri requestUri) 
 *         
 *         + void  httpClient.CancelPendingRequests() 
 *         + void  httpClient.Dispose() 
 *         # void  httpClient.Dispose(bool disposing) 
 *
 *@subject ◆abstract class HttpMessageHandler : IDisposable
 *                                  -- System.Net.Http
 *         # HttpMessageHandler  HttpMessageHandler() 
 *         [×] 'new' is not available, because of abstract class.
 *         
 *         # abstract Task<HttpResponseMessage>  
 *                 httpMessageHandler.SendAsync(HttpRequestMessage request, CancellationToken) 
 *         + void  httpMessageHandler.Dispose() 
 *         # void  httpMessageHandler.Dispose(bool disposing) 
 *         
 *@subject ◆class HttpMessageInvoker : IDisposable
 *                                  -- System.Net.Http
 *         + HttpMessageInvoker  new HttpMessageInvoker(HttpMessageHandler handler) 
 *         + HttpMessageInvoker  new HttpMessageInvoker(HttpMessageHandler handler, bool disposeHandler) 
 *         
 *         + Task<HttpResponseMessage>  
 *                 httpMessageInvoker.SendAsync(HttpRequestMessage request, CancellationToken) 
 *         + void  httpMessageInvoker.Dispose() 
 *         # void  httpMessageInvoker.Dispose(bool disposing) 
 *
 *@subject ◆class HttpRequestMessage : IDisposable
 *                                  -- System.Net.Http
 *         + HttpRequestMessage  new HttpRequestMessage() 
 *         + HttpRequestMessage  new HttpRequestMessage(HttpMethod method, Uri requestUri) 
 *         + HttpRequestMessage  new HttpRequestMessage(HttpMethod method, string requestUri) 
 *         
 *         + HttpRequestHeaders  httpRequestMessage.Headers { get; } 
 *         + IDictionary<string, object>  httpRequestMessage.Properties { get; } 
 *         + Uri          httpRequestMessage.RequestUri { get; set; } 
 *         + HttpMethod   httpRequestMessage.Method { get; set; } 
 *         + Version      httpRequestMessage.Version { get; set; } 
 *         + HttpContent  httpRequestMessage.Content { get; set; } 
 *         + string ToString() 
 *         + void Dispose() 
 *         # void Dispose(bool disposing) 
 *
 *@subject ◆class HttpResponseMessage : IDisposable
 *                                   -- System.Net.Http
 *         + HttpResponseMessage  new HttpResponseMessage() 
 *         + HttpResponseMessage  new HttpResponseMessage(HttpStatusCode statusCode) 
 *             └ enum HttpStatusCode 〔below〕
 *         + HttpResponseHeaders  httpResponseMessage.Headers { get; } 
 *         + HttpStatusCode       httpResponseMessage.StatusCode { get; set; } 
 *         + bool                 httpResponseMessage.IsSuccessStatusCode { get; } 
 *         + string               httpResponseMessage.ReasonPhrase { get; set; }    StatusCodeの文字列
 *         + Version              httpResponseMessage.Version { get; set; } 
 *         + HttpContent          httpResponseMessage.Content { get; set; } 
 *         + HttpRequestMessage   httpResponseMessage.RequestMessage { get; set; } 
 *             └ class HttpRequestMessage 〔above〕
 *             
 *         + HttpResponseMessage  httpResponseMessage.EnsureSuccessStatusCode() 
 *             └ 〔this〕
 *         + string  httpResponseMessage.ToString() 
 *         + void  httpResponseMessage.Dispose() 
 *         # void  httpResponseMessage.Dispose(bool disposing) 
 *         
 *@subject ◆enum HttpStatusCode
 *          {
 *             Continue = 100,
 *             SwitchingProtocols = 101,
 *             OK = 200,
 *             Created = 201,
 *             Accepted = 202,
 *             NonAuthoritativeInformation = 203,
 *             NoContent = 204,
 *             ResetContent = 205,
 *             PartialContent = 206,
 *             MultipleChoices = 300,
 *             Ambiguous = 300,
 *             MovedPermanently = 301,
 *             Moved = 301,
 *             Found = 302,
 *             Redirect = 302,
 *             SeeOther = 303,
 *             RedirectMethod = 303,
 *             NotModified = 304,
 *             UseProxy = 305,
 *             Unused = 306,
 *             TemporaryRedirect = 307,
 *             RedirectKeepVerb = 307,
 *             BadRequest = 400,
 *             Unauthorized = 401,
 *             PaymentRequired = 402,
 *             Forbidden = 403,
 *             NotFound = 404,
 *             MethodNotAllowed = 405,
 *             NotAcceptable = 406,
 *             ProxyAuthenticationRequired = 407,
 *             RequestTimeout = 408,
 *             Conflict = 409,
 *             Gone = 410,
 *             LengthRequired = 411,
 *             PreconditionFailed = 412,
 *             RequestEntityTooLarge = 413,
 *             RequestUriTooLong = 414,
 *             UnsupportedMediaType = 415,
 *             RequestedRangeNotSatisfiable = 416,
 *             ExpectationFailed = 417,
 *             UpgradeRequired = 426,
 *             InternalServerError = 500,
 *             NotImplemented = 501,
 *             BadGateway = 502,
 *             ServiceUnavailable = 503,
 *             GatewayTimeout = 504,
 *             HttpVersionNotSupported = 505,
 *          }
 *          
 *@subject ◆class HttpMethod : IEquatable<HttpMethod>
 *                          -- System.Net.Http
 *         + HttpMethod  new HttpMethod(string method) 
 *         + static HttpMethod  HttpMethod.Get { get; } 
 *         + static HttpMethod  HttpMethod.Post { get; } 
 *         + static HttpMethod  HttpMethod.Put { get; } 
 *         + static HttpMethod  HttpMethod.Delete { get; } 
 *         + static HttpMethod  HttpMethod.Head { get; } 
 *         + static HttpMethod  HttpMethod.Trace { get; } 
 *         + static HttpMethod  HttpMethod.Options { get; } 
 *         + string             httpMethod.Method { get; } 
 *
 *@subject ◆abstract class HttpContent : IDisposable
 *                           -- System.Net.Http
 *         # HttpContent  HttpContent() 
 *         [×] 'new' is not available, but 'base()' is OK from constructor of inherited class ONLY.
 *         
 *         + HttpContentHeaders  httpContent.Headers { get; } 
 *         + Task<string>  httpContent.ReadAsStringAsync() 
 *         + Task<byte[]>  httpContent.ReadAsByteArrayAsync() 
 *         + Task<Stream>  httpContent.ReadAsStreamAsync() 
 *         # Task<Stream>  httpContent.CreateContentReadStreamAsync() 
 *         + Task          httpContent.LoadIntoBufferAsync() 
 *         + Task          httpContent.LoadIntoBufferAsync(long maxBufferSize) 
 *         + Task          httpContent.CopyToAsync(Stream stream) 
 *         + Task          httpContent.CopyToAsync(Stream stream, TransportContext context) 
 *         # abstract Task httpContent.SerializeToStreamAsync(Stream stream, TransportContext context) 
 *         # internal abstract bool  httpContent.TryComputeLength(out long length) 
 *         + void  httpContent.Dispose() 
 *         # void  httpContent.Dispose(bool disposing) 
 *          
 *@subject ◆abstract class HttpHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable
 *                           -- System.Net.Http.Headers
 *         # HttpHeaders  HttpHeaders() 
 *           [×] 'new' is not available, but 'base()' is OK from constructor of inherited class ONLY.
 *         
 *         + IEnumerable<string>  httpHeaders.GetValues(string name) 
 *         + IEnumerator          httpHeaders.GetEnumerator() 
 *             <KeyValuePair<string, IEnumerable<string>>>
 *             
 *         + void  httpHeaders.Add(string name, string value) 
 *         + void  httpHeaders.Add(string name, IEnumerable<string> values) 
 *         + bool  httpHeaders.Contains(string name) 
 *         + bool  httpHeaders.TryGetValues(string name, out IEnumerable<string> values) 
 *         + bool  httpHeaders.TryAddWithoutValidation(string name, string value) 
 *         + bool  httpHeaders.TryAddWithoutValidation(string name, IEnumerable<string> values) 
 *         + bool  httpHeaders.Remove(string name) 
 *         + void  httpHeaders.Clear() 
 *         + string  httpHeaders.ToString() 
 *         
 *@subject ◆sealed class HttpRequestHeaders : HttpHeaders
 *                                  -- System.Net.Http.Headers
 *         + string  httpRequestHeaders.Host { get; set; } 
 *         + string  httpRequestHeaders.From { get; set; } 
 *         + Uri     httpRequestHeaders.Referrer { get; set; } 
 *         + bool?   httpRequestHeaders.TransferEncodingChunked { get; set; } 
 *         + bool?   httpRequestHeaders.ConnectionClose { get; set; }
 *         + bool?   httpRequestHeaders.ExpectContinue { get; set; } 
 *         + int?    httpRequestHeaders.MaxForwards { get; set; } 
 *         + DateTimeOffset?  httpRequestHeaders.Date { get; set; } 
 *         + DateTimeOffset?  httpRequestHeaders.IfUnmodifiedSince { get; set; } 
 *         + DateTimeOffset?  httpRequestHeaders.IfModifiedSince { get; set; } 
 *         + RangeHeaderValue           httpRequestHeaders.Range { get; set; } 
 *         + RangeConditionHeaderValue  httpRequestHeaders.IfRange { get; set; } 
 *         + AuthenticationHeaderValue  httpRequestHeaders.Authorization { get; set; } 
 *         + AuthenticationHeaderValue  httpRequestHeaders.ProxyAuthorization { get; set; } 
 *         + CacheControlHeaderValue    httpRequestHeaders.CacheControl { get; set; } 
 *         
 *         + HttpHeaderValueCollection<string>
 *                  httpRequestHeaders.Connection { get; } 
 *         + HttpHeaderValueCollection<ViaHeaderValue>
 *                  httpRequestHeaders.Via { get; } 
 *         + HttpHeaderValueCollection<ProductInfoHeaderValue>
 *                  httpRequestHeaders.UserAgent { get; } 
 *         + HttpHeaderValueCollection<TransferCodingHeaderValue>
 *                  httpRequestHeaders.TransferEncoding { get; } 
 *         + HttpHeaderValueCollection<string>  
 *                  httpRequestHeaders.Trailer { get; } 
 *         + HttpHeaderValueCollection <MediaTypeWithQualityHeaderValue>  
 *                  httpRequestHeaders.Accept { get; } 
 *         + HttpHeaderValueCollection<ProductHeaderValue>  
 *                  httpRequestHeaders.Upgrade { get; }
 *         + HttpHeaderValueCollection<NameValueHeaderValue>
 *                  httpRequestHeaders.Pragma { get; } 
 *         + HttpHeaderValueCollection<TransferCodingWithQualityHeaderValue>
 *                  httpRequestHeaders.TE { get; } 
 *         + HttpHeaderValueCollection<EntityTagHeaderValue>
 *                 httpRequestHeaders.IfNoneMatch { get; } 
 *         + HttpHeaderValueCollection<EntityTagHeaderValue> 
 *                 httpRequestHeaders.IfMatch { get; } 
 *         + HttpHeaderValueCollection<NameValueWithParametersHeaderValue>
 *                 httpRequestHeaders.Expect { get; } 
 *         + HttpHeaderValueCollection<StringWithQualityHeaderValue>
 *                 httpRequestHeaders.AcceptLanguage { get; } 
 *         + HttpHeaderValueCollection<StringWithQualityHeaderValue>
 *                 httpRequestHeaders.AcceptEncoding { get; } 
 *         + HttpHeaderValueCollection<StringWithQualityHeaderValue>
 *                 httpRequestHeaders.AcceptCharset { get; } 
 *         + HttpHeaderValueCollection<WarningHeaderValue> 
 *                 httpRequestHeaders.Warning { get; } 
 *                 
 *@subject ◆sealed class HttpResponseHeaders : HttpHeaders
 *                                   -- System.Net.Http.Headers
 *         + Uri                        httpResponseHeaders.Location { get; set; } 
 *         + bool?                      httpResponseHeaders.TransferEncodingChunked { get; set; } 
 *         + bool?                      httpResponseHeaders.ConnectionClose { get; set; } 
 *         + DateTimeOffset?            httpResponseHeaders.Date { get; set; } 
 *         + TimeSpan?                  httpResponseHeaders.Age { get; set; } 
 *         + EntityTagHeaderValue       httpResponseHeaders.ETag { get; set; } 
 *         + CacheControlHeaderValue    httpResponseHeaders.CacheControl { get; set; } 
 *         + RetryConditionHeaderValue  httpResponseHeaders.RetryAfter { get; set; } 
 *         
 *         + HttpHeaderValueCollection<ProductInfoHeaderValue>
 *              httpResponseHeaders.Server { get; } 
 *         + HttpHeaderValueCollection<string>
 *              httpResponseHeaders.Connection { get; } 
 *         + HttpHeaderValueCollection<ViaHeaderValue> 
 *              httpResponseHeaders.Via { get; } 
 *         + HttpHeaderValueCollection<TransferCodingHeaderValue> 
 *              httpResponseHeaders.TransferEncoding { get; } 
 *         + HttpHeaderValueCollection<string>  
 *              httpResponseHeaders.AcceptRanges { get; } 
 *         + HttpHeaderValueCollection<ProductHeaderValue>
 *              httpResponseHeaders.Upgrade { get; } 
 *         + HttpHeaderValueCollection<string>
 *              httpResponseHeaders.Trailer { get; } 
 *         + HttpHeaderValueCollection<NameValueHeaderValue> 
 *              httpResponseHeaders.Pragma { get; } 
 *         + HttpHeaderValueCollection<AuthenticationHeaderValue>
 *              httpResponseHeaders.WwwAuthenticate { get; } 
 *         + HttpHeaderValueCollection<string> 
 *              httpResponseHeaders.Vary { get; } 
 *         + HttpHeaderValueCollection<AuthenticationHeaderValue>
 *              httpResponseHeaders.ProxyAuthenticate { get; } 
 *         + HttpHeaderValueCollection<WarningHeaderValue>
 *              httpResponseHeaders.Warning { get; } 
 *
 *@subject ◆sealed class HttpContentHeaders : HttpHeaders
 *                                  -- System.Net.Http.Headers
 *         + Uri                   httpContentHeaders.ContentLocation { get; set; } 
 *         + long?                 httpContentHeaders.ContentLength { get; set; } 
 *         + MediaTypeHeaderValue  httpContentHeaders.ContentType { get; set; } 
 *         + ICollection<string>   httpContentHeaders.ContentEncoding { get; } 
 *         + ICollection<string>   httpContentHeaders.ContentLanguage { get; } 
 *         + byte[]                httpContentHeaders.ContentMD5 { get; set; } 
 *         + ContentRangeHeaderValue        httpContentHeaders.ContentRange { get; set; } 
 *         + ContentDispositionHeaderValue  httpContentHeaders.ContentDisposition { get; set; } 
 *         + ICollection<string>   httpContentHeaders.Allow { get; } 
 *         + DateTimeOffset?       httpContentHeaders.Expires { get; set; } 
 *         + DateTimeOffset?       httpContentHeaders.LastModified { get; set; } 
 *
 */
#endregion
/*
 *@see ImageHttpClientSample.jpg
 *@see MainWebClientSample.cs
 *@author shika
 *@date 2022-11-10
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainHttpClientSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormHttpClientSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormHttpClientSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHttpClientSample : Form
    {
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly TextBox textBoxUrl;
        private readonly TextBox textBoxBody;
        private readonly Button button;

        public FormHttpClientSample()
        {
            this.Text = "FormHttpClientSample";
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

        private async void Button_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var client = new HttpClient();
            var bld = new StringBuilder();
            
            try
            {
                using (Stream stream = await client.GetStreamAsync(textBoxUrl.Text))
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
                $"HttpClient Async: CostTime = {sw.ElapsedMilliseconds} milliSeconds");
        }//Button_Click()
    }//class
}
