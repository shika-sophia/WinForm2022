/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainHttpClientSample.cs
 *@class   └ new FormHttpClientSample() : Form
 *@class       └ new HttpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[425]-[426] p719 / HttpClient
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
#region -> HttpClient, WebClient
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
 *         # void  httpClient.Dispose(bool disposing) 
 *
 *@subject ◆abstract class HttpMessageHandler : IDisposable
 *                                  -- System.Net.Http
 *         # HttpMessageHandler  new HttpMessageHandler() 
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
 *         + Uri          httpRequestMessage.RequestUri { get; set; } 
 *         + HttpMethod   httpRequestMessage.Method { get; set; } 
 *         + Version      httpRequestMessage.Version { get; set; } 
 *         + HttpContent  httpRequestMessage.Content { get; set; } 
 *         + IDictionary<string, object>  httpRequestMessage.Properties { get; } 
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
 *         + Version              httpResponseMessage.Version { get; set; } 
 *         + HttpContent          httpResponseMessage.Content { get; set; } 
 *         + string               httpResponseMessage.ReasonPhrase { get; set; } 
 *         + HttpRequestMessage   httpResponseMessage.RequestMessage { get; set; } 
 *             └ class HttpRequestMessage 〔below〕
 *             
 *         + HttpResponseMessage  httpResponseMessage.EnsureSuccessStatusCode() 
 *             └ this
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
 *         + HttpHeaderValueCollection<string>  
 *                  httpRequestHeaders.Trailer { get; } 
 *         + HttpHeaderValueCollection <MediaTypeWithQualityHeaderValue>  
 *                  httpRequestHeaders.Accept { get; } 
 *         + HttpHeaderValueCollection<ProductHeaderValue>  
 *                  httpRequestHeaders.Upgrade { get; }
 *         + HttpHeaderValueCollection<ProductInfoHeaderValue>
 *                  httpRequestHeaders.UserAgent { get; } 
 *         + HttpHeaderValueCollection<NameValueHeaderValue>
 *                  httpRequestHeaders.Pragma { get; } 
 *         + HttpHeaderValueCollection<TransferCodingHeaderValue>
 *                  httpRequestHeaders.TransferEncoding { get; } 
 *         + HttpHeaderValueCollection<TransferCodingWithQualityHeaderValue>
 *                  httpRequestHeaders.TE { get; } 
 *         + HttpHeaderValueCollection<ViaHeaderValue>
 *                  httpRequestHeaders.Via { get; } 
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
 *@subject ◆abstract class HttpContent : IDisposable
 *                           -- System.Net.Http
 *         # HttpContent  HttpContent() 
 *         [×] 'new' is not available, but 'base()' is OK from constructor of inherited class ONLY.
 *         
 *         + HttpContentHeaders  httpContent.Headers { get; } 
 *         + Task          httpContent.LoadIntoBufferAsync() 
 *         + Task          httpContent.LoadIntoBufferAsync(long maxBufferSize) 
 *         + Task          httpContent.CopyToAsync(Stream stream) 
 *         + Task          httpContent.CopyToAsync(Stream stream, TransportContext context) 
 *         + Task<byte[]>  httpContent.ReadAsByteArrayAsync() 
 *         + Task<string>  httpContent.ReadAsStringAsync() 
 *         + Task<Stream>  httpContent.ReadAsStreamAsync() 
 *         # Task<Stream>  httpContent.CreateContentReadStreamAsync() 
 *         # abstract Task httpContent.SerializeToStreamAsync(Stream stream, TransportContext context) 
 *         # internal abstract bool  httpContent.TryComputeLength(out long length) 
 *         + void  httpContent.Dispose() 
 *         # void  httpContent.Dispose(bool disposing) 
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
 */
#endregion
/*
 *@see ImageHttpClientSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-10
 */
using System;
using System.Drawing;
using System.IO;
using System.Net;
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
            var client = new HttpClient();
            
            try
            {
                using (Stream stream = await client.GetStreamAsync(textBoxUrl.Text))
                using (StreamReader reader = new StreamReader(stream))
                {
                    StringBuilder bld = new StringBuilder();
                    while (!reader.EndOfStream) 
                    {
                        bld.Append(reader.ReadLine());
                        bld.Append(Environment.NewLine);
                    }//while

                    textBoxBody.Text = bld.ToString();

                    reader.Close();
                    stream.Dispose();
                }//using
            }
            catch (Exception ex)
            {
                textBoxBody.Text =
                    $"{ex.GetType()}:{Environment.NewLine}" +
                    $"{ex.Message}{Environment.NewLine}";
            }
        }//Button_Click()
    }//class
}
