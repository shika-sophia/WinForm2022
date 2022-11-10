/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainHttpClientSample.cs
 *@class   └ new FormHttpClientSample() : Form
 *@class       └ new HttpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[425]-[426] p719 / HttpClientSample
 *
 */
#region
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
 *@subject ◆class HttpResponseMessage : IDisposable
 *                                   -- System.Net.Http
 *         + HttpResponseMessage  new HttpResponseMessage() 
 *         + HttpResponseMessage  new HttpResponseMessage(HttpStatusCode statusCode) 
 *             └ enum HttpStatusCode
 *               {
 *                   Continue = 100,
 *                   SwitchingProtocols = 101,
 *                   OK = 200,
 *                   Created = 201,
 *                   Accepted = 202,
 *                   NonAuthoritativeInformation = 203,
 *                   NoContent = 204,
 *                   ResetContent = 205,
 *                   PartialContent = 206,
 *                   MultipleChoices = 300,
 *                   Ambiguous = 300,
 *                   MovedPermanently = 301,
 *                   Moved = 301,
 *                   Found = 302,
 *                   Redirect = 302,
 *                   SeeOther = 303,
 *                   RedirectMethod = 303,
 *                   NotModified = 304,
 *                   UseProxy = 305,
 *                   Unused = 306,
 *                   TemporaryRedirect = 307,
 *                   RedirectKeepVerb = 307,
 *                   BadRequest = 400,
 *                   Unauthorized = 401,
 *                   PaymentRequired = 402,
 *                   Forbidden = 403,
 *                   NotFound = 404,
 *                   MethodNotAllowed = 405,
 *                   NotAcceptable = 406,
 *                   ProxyAuthenticationRequired = 407,
 *                   RequestTimeout = 408,
 *                   Conflict = 409,
 *                   Gone = 410,
 *                   LengthRequired = 411,
 *                   PreconditionFailed = 412,
 *                   RequestEntityTooLarge = 413,
 *                   RequestUriTooLong = 414,
 *                   UnsupportedMediaType = 415,
 *                   RequestedRangeNotSatisfiable = 416,
 *                   ExpectationFailed = 417,
 *                   UpgradeRequired = 426,
 *                   InternalServerError = 500,
 *                   NotImplemented = 501,
 *                   BadGateway = 502,
 *                   ServiceUnavailable = 503,
 *                   GatewayTimeout = 504,
 *                   HttpVersionNotSupported = 505,
 *               }
 *
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
 *         + string  httpResponseMessage.ToString() 
 *         + void  httpResponseMessage.Dispose() 
 *         # void  httpResponseMessage.Dispose(bool disposing) 
 *
 *@subject ◆class HttpRequestMessage : IDisposable
 *                                  -- System.Net.Http
 *         + HttpRequestMessage  new HttpRequestMessage() 
 *         + HttpRequestMessage  new HttpRequestMessage(HttpMethod method, Uri requestUri) 
 *         + HttpRequestMessage  new HttpRequestMessage(HttpMethod method, string requestUri) 
 *         
 *         + Version      httpRequestMessage.Version { get; set; } 
 *         + HttpContent  httpRequestMessage.Content { get; set; } 
 *         + HttpMethod   httpRequestMessage.Method { get; set; } 
 *         + Uri          httpRequestMessage.RequestUri { get; set; } 
 *         + HttpRequestHeaders  httpRequestMessage.Headers { get; } 
 *         + IDictionary<string, object>  httpRequestMessage.Properties { get; } 
 *         + string ToString() 
 *         + void Dispose() 
 *         # void Dispose(bool disposing) 
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
 *         + HttpHeaderValueCollection <MediaTypeWithQualityHeaderValue>  
 *                  httpRequestHeaders.Accept { get; } 
 *         + HttpHeaderValueCollection<ProductHeaderValue>  
 *                  httpRequestHeaders.Upgrade { get; }
 *         + HttpHeaderValueCollection<TransferCodingHeaderValue>
 *                  httpRequestHeaders.TransferEncoding { get; } 
 *         + HttpHeaderValueCollection<string>  
 *                  httpRequestHeaders.Trailer { get; } 
 *         + HttpHeaderValueCollection<NameValueHeaderValue>
 *                  httpRequestHeaders.Pragma { get; } 
 *         + DateTimeOffset?  httpRequestHeaders.Date { get; set; } 
 *         + HttpHeaderValueCollection<string>
 *                  httpRequestHeaders.Connection { get; } 
 *         + CacheControlHeaderValue  httpRequestHeaders.CacheControl { get; set; } 
 *         + HttpHeaderValueCollection<ProductInfoHeaderValue>  httpRequestHeaders.UserAgent { get; } 
 *         + HttpHeaderValueCollection<TransferCodingWithQualityHeaderValue>  httpRequestHeaders.TE { get; } 
 *         + RangeHeaderValue  httpRequestHeaders.Range { get; set; } 
 *         + HttpHeaderValueCollection<ViaHeaderValue>  httpRequestHeaders.Via { get; } 
 *         + AuthenticationHeaderValue  httpRequestHeaders.ProxyAuthorization { get; set; } 
 *         + DateTimeOffset?  httpRequestHeaders.IfUnmodifiedSince { get; set; } 
 *         + RangeConditionHeaderValue  httpRequestHeaders.IfRange { get; set; } 
 *         + HttpHeaderValueCollection<EntityTagHeaderValue>  httpRequestHeaders.IfNoneMatch { get; } 
 *         + DateTimeOffset?  httpRequestHeaders.IfModifiedSince { get; set; } 
 *         + HttpHeaderValueCollection<EntityTagHeaderValue>  httpRequestHeaders.IfMatch { get; } 
 *         + HttpHeaderValueCollection<NameValueWithParametersHeaderValue>  httpRequestHeaders.Expect { get; } 
 *         + AuthenticationHeaderValue  httpRequestHeaders.Authorization { get; set; } 
 *         + HttpHeaderValueCollection<StringWithQualityHeaderValue>  httpRequestHeaders.AcceptLanguage { get; } 
 *         + HttpHeaderValueCollection<StringWithQualityHeaderValue>  httpRequestHeaders.AcceptEncoding { get; } 
 *         + HttpHeaderValueCollection<StringWithQualityHeaderValue>  httpRequestHeaders.AcceptCharset { get; } 
 *         + HttpHeaderValueCollection<WarningHeaderValue>  httpRequestHeaders.Warning { get; } 
 *         
 *@subject ◆abstract class HttpContent : IDisposable
 *                           -- System.Net.Http
 *         # HttpContent  HttpContent() 
 *         [×] 'new' is not available, but 'base()' is OK from constructor of inherited class ONLY.
 *         
 *         + HttpContentHeaders  httpContent.Headers { get; } 
 *         + Task  httpContent.CopyToAsync(Stream stream, TransportContext context) 
 *         + Task  httpContent.CopyToAsync(Stream stream) 
 *         + void  httpContent.Dispose() 
 *         + Task  httpContent.LoadIntoBufferAsync() 
 *         + Task  httpContent.LoadIntoBufferAsync(long maxBufferSize) 
 *         + Task<byte[]>  httpContent.ReadAsByteArrayAsync() 
 *         + Task<Stream>  httpContent.ReadAsStreamAsync() 
 *         + Task<string>  httpContent.ReadAsStringAsync() 
 *         # Task<Stream>  httpContent.CreateContentReadStreamAsync() 
 *         # void  httpContent.Dispose(bool disposing) 
 *         # abstract Task  httpContent.SerializeToStreamAsync(Stream stream, TransportContext context) 
 *         # internal abstract bool  httpContent.TryComputeLength(out long length) 
 *
 *@subject ◆class WebClient : Component
 *                         -- System.Net
 *         + WebClient  new WebClient() 
 *         + bool  webClient.AllowWriteStreamBuffering { get; set; } 
 *         + Encoding  webClient.Encoding { get; set; } 
 *         + string  webClient.BaseAddress { get; set; } 
 *         + ICredentials  webClient.Credentials { get; set; } 
 *         + bool  webClient.UseDefaultCredentials { get; set; } 
 *         + WebHeaderCollection  webClient.Headers { get; set; } 
 *         + NameValueCollection  webClient.QueryString { get; set; } 
 *         + WebHeaderCollection  webClient.ResponseHeaders { get; } 
 *         + IWebProxy  webClient.Proxy { get; set; } 
 *         + RequestCachePolicy  webClient.CachePolicy { get; set; } 
 *         + bool  webClient.IsBusy { get; } 
 *         + bool  webClient.AllowReadStreamBuffering { get; set; } 
 *         + event WriteStreamClosedEventHandler  webClient.WriteStreamClosed 
 *         + event OpenReadCompletedEventHandler  webClient.OpenReadCompleted 
 *         + event OpenWriteCompletedEventHandler  webClient.OpenWriteCompleted 
 *         + event DownloadStringCompletedEventHandler  webClient.DownloadStringCompleted 
 *         + event DownloadDataCompletedEventHandler  webClient.DownloadDataCompleted 
 *         + event AsyncCompletedEventHandler  webClient.DownloadFileCompleted 
 *         + event UploadStringCompletedEventHandler  webClient.UploadStringCompleted 
 *         + event UploadDataCompletedEventHandler  webClient.UploadDataCompleted 
 *         + event UploadFileCompletedEventHandler  webClient.UploadFileCompleted 
 *         + event UploadValuesCompletedEventHandler  webClient.UploadValuesCompleted 
 *         + event DownloadProgressChangedEventHandler  webClient.DownloadProgressChanged 
 *         + event UploadProgressChangedEventHandler  webClient.UploadProgressChanged 
 *         + void  webClient.CancelAsync() 
 *         + byte[]  webClient.DownloadData(string address) 
 *         + byte[]  webClient.DownloadData(Uri address) 
 *         + void  webClient.DownloadDataAsync(Uri address, object userToken) 
 *         + void  webClient.DownloadDataAsync(Uri address) 
 *         + Task<byte[]>  webClient.DownloadDataTaskAsync(string address) 
 *         + Task<byte[]>  webClient.DownloadDataTaskAsync(Uri address) 
 *         + void  webClient.DownloadFile(string address, string fileName) 
 *         + void  webClient.DownloadFile(Uri address, string fileName) 
 *         + void  webClient.DownloadFileAsync(Uri address, string fileName, object userToken) 
 *         + void  webClient.DownloadFileAsync(Uri address, string fileName) 
 *         + Task  webClient.DownloadFileTaskAsync(string address, string fileName) 
 *         + Task  webClient.DownloadFileTaskAsync(Uri address, string fileName) 
 *         + string  webClient.DownloadString(Uri address) 
 *         + string  webClient.DownloadString(string address) 
 *         + void  webClient.DownloadStringAsync(Uri address) 
 *         + void  webClient.DownloadStringAsync(Uri address, object userToken) 
 *         + Task<string>  webClient.DownloadStringTaskAsync(string address) 
 *         + Task<string>  webClient.DownloadStringTaskAsync(Uri address) 
 *         + Stream  webClient.OpenRead(Uri address) 
 *         + Stream  webClient.OpenRead(string address) 
 *         + void  webClient.OpenReadAsync(Uri address, object userToken) 
 *         + void  webClient.OpenReadAsync(Uri address) 
 *         + Task<Stream>  webClient.OpenReadTaskAsync(string address) 
 *         + Task<Stream>  webClient.OpenReadTaskAsync(Uri address) 
 *         + Stream  webClient.OpenWrite(string address) 
 *         + Stream  webClient.OpenWrite(string address, string method) 
 *         + Stream  webClient.OpenWrite(Uri address, string method) 
 *         + Stream  webClient.OpenWrite(Uri address) 
 *         + void  webClient.OpenWriteAsync(Uri address, string method, object userToken) 
 *         + void  webClient.OpenWriteAsync(Uri address) 
 *         + void  webClient.OpenWriteAsync(Uri address, string method) 
 *         + Task<Stream>  webClient.OpenWriteTaskAsync(Uri address) 
 *         + Task<Stream>  webClient.OpenWriteTaskAsync(Uri address, string method) 
 *         + Task<Stream>  webClient.OpenWriteTaskAsync(string address) 
 *         + Task<Stream>  webClient.OpenWriteTaskAsync(string address, string method) 
 *         + byte[]  webClient.UploadData(Uri address, string method, byte[] data) 
 *         + byte[]  webClient.UploadData(string address, string method, byte[] data) 
 *         + byte[]  webClient.UploadData(string address, byte[] data) 
 *         + byte[]  webClient.UploadData(Uri address, byte[] data) 
 *         + void  webClient.UploadDataAsync(Uri address, string method, byte[] data, object userToken) 
 *         + void  webClient.UploadDataAsync(Uri address, string method, byte[] data) 
 *         + void  webClient.UploadDataAsync(Uri address, byte[] data) 
 *         + Task<byte[]>  webClient.UploadDataTaskAsync(string address, string method, byte[] data) 
 *         + Task<byte[]>  webClient.UploadDataTaskAsync(Uri address, byte[] data) 
 *         + Task<byte[]>  webClient.UploadDataTaskAsync(string address, byte[] data) 
 *         + Task<byte[]>  webClient.UploadDataTaskAsync(Uri address, string method, byte[] data) 
 *         + byte[]  webClient.UploadFile(Uri address, string method, string fileName) 
 *         + byte[]  webClient.UploadFile(string address, string method, string fileName) 
 *         + byte[]  webClient.UploadFile(Uri address, string fileName) 
 *         + byte[]  webClient.UploadFile(string address, string fileName) 
 *         + void  webClient.UploadFileAsync(Uri address, string method, string fileName) 
 *         + void  webClient.UploadFileAsync(Uri address, string method, string fileName, object userToken) 
 *         + void  webClient.UploadFileAsync(Uri address, string fileName) 
 *         + Task<byte[]>  webClient.UploadFileTaskAsync(string address, string fileName) 
 *         + Task<byte[]>  webClient.UploadFileTaskAsync(Uri address, string fileName) 
 *         + Task<byte[]>  webClient.UploadFileTaskAsync(string address, string method, string fileName) 
 *         + Task<byte[]>  webClient.UploadFileTaskAsync(Uri address, string method, string fileName) 
 *         + string  webClient.UploadString(string address, string data) 
 *         + string  webClient.UploadString(Uri address, string data) 
 *         + string  webClient.UploadString(string address, string method, string data) 
 *         + string  webClient.UploadString(Uri address, string method, string data) 
 *         + void  webClient.UploadStringAsync(Uri address, string method, string data, object userToken) 
 *         + void  webClient.UploadStringAsync(Uri address, string method, string data) 
 *         + void  webClient.UploadStringAsync(Uri address, string data) 
 *         + Task<string>  webClient.UploadStringTaskAsync(string address, string method, string data) 
 *         + Task<string>  webClient.UploadStringTaskAsync(Uri address, string method, string data) 
 *         + Task<string>  webClient.UploadStringTaskAsync(string address, string data) 
 *         + Task<string>  webClient.UploadStringTaskAsync(Uri address, string data) 
 *         + byte[]  webClient.UploadValues(Uri address, string method, NameValueCollection data) 
 *         + byte[]  webClient.UploadValues(string address, NameValueCollection data) 
 *         + byte[]  webClient.UploadValues(string address, string method, NameValueCollection data) 
 *         + byte[]  webClient.UploadValues(Uri address, NameValueCollection data) 
 *         + void  webClient.UploadValuesAsync(Uri address, string method, NameValueCollection data, object userToken) 
 *         + void  webClient.UploadValuesAsync(Uri address, NameValueCollection data) 
 *         + void  webClient.UploadValuesAsync(Uri address, string method, NameValueCollection data) 
 *         + Task<byte[]>  webClient.UploadValuesTaskAsync(Uri address, string method, NameValueCollection data) 
 *         + Task<byte[]>  webClient.UploadValuesTaskAsync(Uri address, NameValueCollection data) 
 *         + Task<byte[]>  webClient.UploadValuesTaskAsync(string address, string method, NameValueCollection data) 
 *         + Task<byte[]>  webClient.UploadValuesTaskAsync(string address, NameValueCollection data) 
 *         # WebRequest  webClient.GetWebRequest(Uri address) 
 *         # WebResponse  webClient.GetWebResponse(WebRequest request) 
 *         # WebResponse  webClient.GetWebResponse(WebRequest request, IAsyncResult result) 
 *         # void  webClient.OnDownloadDataCompleted(DownloadDataCompletedEventArgs e) 
 *         # void  webClient.OnDownloadFileCompleted(AsyncCompletedEventArgs e) 
 *         # void  webClient.OnDownloadProgressChanged(DownloadProgressChangedEventArgs e) 
 *         # void  webClient.OnDownloadStringCompleted(DownloadStringCompletedEventArgs e) 
 *         # void  webClient.OnOpenReadCompleted(OpenReadCompletedEventArgs e) 
 *         # void  webClient.OnOpenWriteCompleted(OpenWriteCompletedEventArgs e) 
 *         # void  webClient.OnUploadDataCompleted(UploadDataCompletedEventArgs e) 
 *         # void  webClient.OnUploadFileCompleted(UploadFileCompletedEventArgs e) 
 *         # void  webClient.OnUploadProgressChanged(UploadProgressChangedEventArgs e) 
 *         # void  webClient.OnUploadStringCompleted(UploadStringCompletedEventArgs e) 
 *         # void  webClient.OnUploadValuesCompleted(UploadValuesCompletedEventArgs e) 
 *         # void  webClient.OnWriteStreamClosed(WriteStreamClosedEventArgs e) 

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
using System.Drawing.Drawing2D;
using System.Net;
using System.Net.Http;
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
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 85f));

            //---- Label ----
            label = new Label()
            {
                Text = "URL: ",
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(label, 0, 0);

            //---- TextBox ----
            textBoxUrl = new TextBox()
            {
                Multiline = false,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxUrl, 1, 0);

            textBoxBody = new TextBox()
            {
                Multiline = true,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxUrl, 0, 1);
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

        private void Button_Click(object sender, EventArgs e)
        {
            using(var client = new HttpClient())
            {
                
            }//using
        }//Button_Click()
    }//class
}
