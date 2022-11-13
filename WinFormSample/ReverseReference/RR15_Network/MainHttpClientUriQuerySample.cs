/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainHttpClientUriQuerySample.cs
 *@class   └ new FormHttpClientUriQuerySample() : Form
 *@class       └ new HttpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[426] p721 / HttpClient, Uri. Query, UriBuilder
 *         ◆class UriBuilder:         to build automatically Uri path with Query
 *         string  uriBuilder.Uri:     whole Uri path to send Web Server
 *         string  uriBuilder.Query:   Uri Query such as '?q=xxxxx&hl=XXXX'  
 *         
 *         ◆class HttpUtility:        to encode from 2 Bytes Character
 *         NameValueCollection  httpUtility.ParseQueryString(string)
 *         string               httpUtility.UrlEncode(string)
 *         
 *         [Example]
 *         var uriBuilder = new UriBuilder("http://www.google.com/search");
 *           
 *         NameValueCollection queryCollection = HttpUtility.ParseQueryString("");
 *         queryCollection["q"] = HttpUtility.UrlEncode(searchWord);
 *         queryCollection["hl"] = "jp";
 *
 *         uriBuilder.Query = queryCollection.ToString();
 *         
 *@NOTE 【Result】
 *       //---- Test Print ----
 *       //Console.WriteLine($"searchWord: {searchWord}");
 *       //Console.WriteLine($"Uri with Query: {uriBuilder.Uri}");
 *
 *       //---- Test Result ----
 *       //searchWord:     Eron Musk Twitter 解雇
 *       //Uri with Query: http://www.google.com/search?q=Eron%2bMusk%2bTwitter%2b%25e8%25a7%25a3%25e9%259b%2587&hl=jp
 */
#region -> Uri, UriBuilder, HttpUtility
/*
 *@subject HttpClient => 〔MainHttpClientSample.cs〕
 *
 *@subject ◆class Uri : ISerializable -- System
 *         + Uri  new Uri(string uriString) 
 *         + Uri  new Uri(string uriString, bool dontEscape) 
 *         + Uri  new Uri(string uriString, UriKind uriKind) 
 *         + Uri  new Uri(Uri baseUri, Uri relativeUri) 
 *         + Uri  new Uri(Uri baseUri, string relativeUri) 
 *         + Uri  new Uri(Uri baseUri, string relativeUri, bool dontEscape) 
 *         # Uri  Uri(SerializationInfo, StreamingContext)
 *            └ enum UriKind -- System.
 *              {
 *                 RelativeOrAbsolute = 0,  //不確定
 *                 Absolute = 1,            //絶対 URI
 *                 Relative = 2             //相対 URI
 *              }
 *         
 *         ＊static Field
 *         + static readonly string  Uri.UriSchemeFile 
 *         + static readonly string  Uri.UriSchemeFtp 
 *         + static readonly string  Uri.UriSchemeGopher 
 *         + static readonly string  Uri.UriSchemeHttp 
 *         + static readonly string  Uri.UriSchemeHttps 
 *         + static readonly string  Uri.UriSchemeMailto 
 *         + static readonly string  Uri.UriSchemeNews 
 *         + static readonly string  Uri.UriSchemeNntp 
 *         + static readonly string  Uri.UriSchemeNetTcp 
 *         + static readonly string  Uri.UriSchemeNetPipe 
 *         + static readonly string  Uri.SchemeDelimiter 
 *         
 *         ＊Property
 *         + string  uri.Host { get; } 
 *         + string  uri.PathAndQuery { get; } 
 *         + string  uri.Query { get; } 
 *         + int     uri.Port { get; } 
 *         + string[]         uri.Segments { get; } 
 *         + UriHostNameType  uri.HostNameType { get; } 
 *         + string  uri.DnsSafeHost { get; } 
 *         + string  uri.IdnHost { get; } 
 *         + string  uri.AbsoluteUri { get; } 
 *         + string  uri.AbsolutePath { get; } 
 *         + string  uri.LocalPath { get; } 
 *         + string  uri.Authority { get; } 
 *         + string  uri.Fragment { get; } 
 *         + string  uri.Scheme { get; } 
 *         + string  uri.UserInfo { get; } 
 *         + string  uri.OriginalString { get; } 
 *         + bool    uri.IsAbsoluteUri { get; } 
 *         + bool    uri.IsDefaultPort { get; } 
 *         + bool    uri.IsFile { get; } 
 *         + bool    uri.IsLoopback { get; } 
 *         + bool    uri.IsUnc { get; } 
 *         + bool    uri.UserEscaped { get; } 
 *         
 *         ＊static Method
 *         + static UriHostNameType  Uri.CheckHostName(string name) 
 *             └ enum UriHostNameType -- System.
 *               {
 *                  Unknown = 0,  //不明 (未指定)
 *                  Basic = 1,    //ホストは設定されましたが、型を特定できない
 *                  Dns = 2,      //DNS: Domain Name System 形式
 *                  IPv4 = 3,     //Internet Protocol (IP) Version 4 形式
 *                  IPv6 = 4      //Internet Protocol (IP) Version 6 形式
 *               }
 *               
 *         + static bool    Uri.CheckSchemeName(string schemeName) 
 *         + static int     Uri.Compare(Uri uri1, Uri uri2, UriComponents partsToCompare, UriFormat compareFormat, StringComparison comparisonType) 
 *         # static string  Uri.EscapeString(string str) 
 *         + static string  Uri.EscapeDataString(string stringToEscape) 
 *         + static string  Uri.EscapeUriString(string stringToEscape) 
 *         + static string  Uri.UnescapeDataString(string stringToUnescape) 
 *         + static int     Uri.FromHex(char digit) 
 *         + static string  Uri.HexEscape(char character) 
 *         + static char    Uri.HexUnescape(string pattern, ref int index) 
 *         + static bool    Uri.IsHexDigit(char character) 
 *         + static bool    Uri.IsHexEncoding(string pattern, int index) 
 *         + static bool    Uri.IsWellFormedUriString(string uriString, UriKind uriKind) 
 *         # static bool    Uri.IsExcludedCharacter(char character)
 *         + static bool    Uri.TryCreate(string uriString, UriKind uriKind, out Uri result) 
 *         + static bool    Uri.TryCreate(Uri baseUri, string relativeUri, out Uri result) 
 *         + static bool    Uri.TryCreate(Uri baseUri, Uri relativeUri, out Uri result) 
 *         
 *         ＊instance Method
 *         + string  uri.MakeRelative(Uri toUri) 
 *         + Uri     uri.MakeRelativeUri(Uri uri) 
 *         # void    uri.Canonicalize() 
 *         # void    uri.CheckSecurity() 
 *         # void    uri.Parse() 
 *         # void    uri.Escape() 
 *         # string  uri.Unescape(string path) 
 *         + string  uri.GetLeftPart(UriPartial part) 
 *         + string  uri.GetComponents(UriComponents components, UriFormat format) 
 *         # void    uri.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext) 
 *         + bool    uri.IsBaseOf(Uri uri) 
 *         + bool    uri.IsWellFormedOriginalString() 
 *         # bool    uri.IsBadFileSystemCharacter(char character) 
 *         # bool    uri.IsReservedCharacter(char character) 
 *
 *@subject ◆sealed class SerializationInfo -- System.Runtime.Serialization
 *         + SerializationInfo  new SerializationInfo(Type type, IFormatterConverter converter) 
 *         + SerializationInfo  new SerializationInfo(Type type, IFormatterConverter converter, bool requireSameTokenInPartialTrust) 
 *        
 *         + string  serializationInfo.AssemblyName { get; set; } 
 *         + string  serializationInfo.FullTypeName { get; set; } 
 *         + Type    serializationInfo.ObjectType { get; } 
 *         + int     serializationInfo.MemberCount { get; } 
 *         + bool    serializationInfo.IsAssemblyNameSetExplicit { get; } 
 *         + bool    serializationInfo.IsFullTypeNameSetExplicit { get; }
 *         
 *         + void    serializationInfo.AddValue(string name, sbyte value) 
 *         + void    serializationInfo.AddValue(string name, object value, Type type) 
 *         + void    serializationInfo.AddValue(string name, bool value) 
 *         + void    serializationInfo.AddValue(string name, DateTime value) 
 *         + void    serializationInfo.AddValue(string name, decimal value) 
 *         + void    serializationInfo.AddValue(string name, double value) 
 *         + void    serializationInfo.AddValue(string name, object value) 
 *         + void    serializationInfo.AddValue(string name, float value) 
 *         + void    serializationInfo.AddValue(string name, long value) 
 *         + void    serializationInfo.AddValue(string name, uint value) 
 *         + void    serializationInfo.AddValue(string name, int value) 
 *         + void    serializationInfo.AddValue(string name, ushort value) 
 *         + void    serializationInfo.AddValue(string name, short value) 
 *         + void    serializationInfo.AddValue(string name, byte value) 
 *         + void    serializationInfo.AddValue(string name, ulong value) 
 *         + void    serializationInfo.AddValue(string name, char value) 
 *         
 *         + SerializationInfoEnumerator  serializationInfo.GetEnumerator() 
 *         + bool    serializationInfo.GetBoolean(string name) 
 *         + byte    serializationInfo.GetByte(string name) 
 *         + char    serializationInfo.GetChar(string name) 
 *         + DateTime  serializationInfo.GetDateTime(string name) 
 *         + decimal serializationInfo.GetDecimal(string name) 
 *         + double  serializationInfo.GetDouble(string name) 
 *         + short   serializationInfo.GetInt16(string name) 
 *         + int     serializationInfo.GetInt32(string name) 
 *         + long    serializationInfo.GetInt64(string name) 
 *         + sbyte   serializationInfo.GetSByte(string name) 
 *         + float   serializationInfo.GetSingle(string name) 
 *         + string  serializationInfo.GetString(string name) 
 *         + ushort  serializationInfo.GetUInt16(string name) 
 *         + uint    serializationInfo.GetUInt32(string name) 
 *         + ulong   serializationInfo.GetUInt64(string name) 
 *         + object  serializationInfo.GetValue(string name, Type type) 
 *         + void    serializationInfo.SetType(Type type) 
 *
 *@subject ◆struct StreamingContext -- System.Runtime.Serialization
 *         + StreamingContext  new StreamingContext(StreamingContextStates state) 
 *         + StreamingContext  new StreamingContext(StreamingContextStates state, object additional) 
 *         
 *         + object  streamingContext.Context { get; } 
 *         + StreamingContextStates  streamingContext.State { get; } 
 *             └ enum  enum StreamingContextStates  -- System.Runtime.Serialization
 *               {
 *                   CrossProcess = 1,
 *                   CrossMachine = 2,
 *                   File = 4,
 *                   Persistence = 8,
 *                   Remoting = 16,
 *                   Other = 32,
 *                   Clone = 64,
 *                   CrossAppDomain = 128,
 *                   All = 255,
 *               }
 *               
 *@subject ◆class UriBuilder -- System   
 *         ・２Byte文字の Encode, Decode
 *         ・URL Queryには、ASCII code (半角英数字)のみ記述可。
 *         ・日本語の２Byte文字(全角文字)は、このクラスのメソッドで Encode, Decode
 *         
 *         + UriBuilder  new UriBuilder() 
 *         + UriBuilder  new UriBuilder(string uri) 
 *         + UriBuilder  new UriBuilder(Uri uri) 
 *         + UriBuilder  new UriBuilder(string schemeName, string hostName) 
 *         + UriBuilder  new UriBuilder(string scheme, string host, int portNumber) 
 *         + UriBuilder  new UriBuilder(string scheme, string host, int port, string pathValue) 
 *         + UriBuilder  new UriBuilder(string scheme, string host, int port, string path, string extraValue) 
 *         
 *         + Uri     uriBuilder.Uri { get; } 
 *         + string  uriBuilder.Path { get; set; } 
 *         + string  uriBuilder.Host { get; set; } 
 *         + int     uriBuilder.Port { get; set; } 
 *         + string  uriBuilder.Query { get; set; } 
 *         + string  uriBuilder.Scheme { get; set; } 
 *         + string  uriBuilder.UserName { get; set; } 
 *         + string  uriBuilder.Password { get; set; } 
 *         + string  uriBuilder.Fragment { get; set; } 
 *         
 *@subject ◆sealed class HttpUtility -- System.Web
 *         + HttpUtility    new HttpUtility()
 *         
 *         + static string  HttpUtility.HtmlEncode(string s) 
 *         + static string  HttpUtility.HtmlEncode(object value) 
 *         + static void    HttpUtility.HtmlEncode(string s, TextWriter output) 
 *         + static string  HttpUtility.HtmlAttributeEncode(string s) 
 *         + static void    HttpUtility.HtmlAttributeEncode(string s, TextWriter output) 
 *         + static string  HttpUtility.JavaScriptStringEncode(string value) 
 *         + static string  HttpUtility.JavaScriptStringEncode(string value, bool addDoubleQuotes) 
 *         + static NameValueCollection  HttpUtility.ParseQueryString(string query) 
 *         + static NameValueCollection  HttpUtility.ParseQueryString(string query, Encoding encoding) 
 *         + static string  HttpUtility.UrlEncode(string str) 
 *         + static string  HttpUtility.UrlEncode(string str, Encoding e) 
 *         + static string  HttpUtility.UrlEncode(byte[] bytes) 
 *         + static string  HttpUtility.UrlEncode(byte[] bytes, int offset, int count) 
 *         + static string  HttpUtility.UrlPathEncode(string str) 
 *         + static byte[]  HttpUtility.UrlEncodeToBytes(string str) 
 *         + static byte[]  HttpUtility.UrlEncodeToBytes(byte[] bytes, int offset, int count) 
 *         + static byte[]  HttpUtility.UrlEncodeToBytes(byte[] bytes) 
 *         + static byte[]  HttpUtility.UrlEncodeToBytes(string str, Encoding e) 
 *         + static string  HttpUtility.UrlEncodeUnicode(string str) 
 *         + static byte[]  HttpUtility.UrlEncodeUnicodeToBytes(string str) 
 *         
 *         + static string  HttpUtility.HtmlDecode(string s) 
 *         + static void    HttpUtility.HtmlDecode(string s, TextWriter output) 
 *         + static string  HttpUtility.UrlDecode(byte[] bytes, int offset, int count, Encoding e) 
 *         + static string  HttpUtility.UrlDecode(string str) 
 *         + static string  HttpUtility.UrlDecode(string str, Encoding e) 
 *         + static string  HttpUtility.UrlDecode(byte[] bytes, Encoding e) 
 *         + static byte[]  HttpUtility.UrlDecodeToBytes(string str) 
 *         + static byte[]  HttpUtility.UrlDecodeToBytes(byte[] bytes) 
 *         + static byte[]  HttpUtility.UrlDecodeToBytes(byte[] bytes, int offset, int count) 
 *         + static byte[]  HttpUtility.UrlDecodeToBytes(string str, Encoding e) 
 *
 */
#endregion
/*
 *@see ImageHttpClientUriQuerySample.jpg
 *@see MainHttpClientSample.cs
 *@author shika
 *@date 2022-11-12
 */
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainHttpClientUriQuerySample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormHttpClientUriQuerySample()");

            Application.EnableVisualStyles();
            Application.Run(new FormHttpClientUriQuerySample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHttpClientUriQuerySample : Form
    {
        private readonly HttpClient client;
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly TextBox textBoxSearch;
        private readonly TextBox textBoxBody;
        private readonly Button button;

        public FormHttpClientUriQuerySample()
        {
            this.Text = "FormHttpClientUriQuerySample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 480);
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- HttpClient ----
            client = new HttpClient();

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
                Text = "Search: ",
                TextAlign = ContentAlignment.MiddleRight,
                Margin = new Padding(10),
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(label, 0, 0);

            //---- TextBox ----
            textBoxSearch = new TextBox()
            {
                Text = "",
                Multiline = false,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxSearch, 1, 0);

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
            //---- Validation ----
            if(String.IsNullOrEmpty(textBoxSearch.Text)) { return; }
            string searchWord = textBoxSearch.Text;

            bool canOutput = ValidateInput(searchWord);
            if(!canOutput) { return; }

            //---- Build Uri with Query ----
            var uriBuilder = new UriBuilder("http://www.google.com/search");
            
            NameValueCollection queryCollection = HttpUtility.ParseQueryString("");
            queryCollection["q"] = HttpUtility.UrlEncode(searchWord);
            queryCollection["hl"] = "jp";

            uriBuilder.Query = queryCollection.ToString();
         
            //---- Connect to Uri, Search word ----
            try
            {
                using (Stream stream = await client.GetStreamAsync(uriBuilder.Uri))
                using (StreamReader reader = new StreamReader(stream))
                {
                    //---- Read content ----
                    StringBuilder readBld = new StringBuilder();
                    while (!reader.EndOfStream)
                    {
                        readBld.Append(reader.ReadLine());
                        readBld.Append(Environment.NewLine);
                    }//while

                    //---- Show content ----
                    textBoxBody.Text = readBld.ToString();

                    reader.Close();
                    stream.Close();
                }//using
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"{ex.GetType()}:{Environment.NewLine}" +
                    $"{ex.Message}");
            }
        }//Button_Click()

        private bool ValidateInput(string searchWord)
        {
            var errorMessageBld = new StringBuilder();
            
            //---- Less Length ----
            if (searchWord.Length <= 4) 
            {
                errorMessageBld.Append(
                    $"The Search word should be described over 5 characters.{Environment.NewLine}");
            }

            //---- Anti Script ----
            Regex regexAntiScript = new Regex(@"[<>&;/]+");
            bool isMatch = regexAntiScript.IsMatch(searchWord);

            if (isMatch)
            {
                errorMessageBld.Append(
                    $"<！> Invalid Input ! {Environment.NewLine}");
            }

            if(errorMessageBld.Length > 0)
            {
                ShowErrorMessage(errorMessageBld.ToString());
                return false;
            }

            return true; // bool canOutput
        }//ValidateInput()

        private void ShowErrorMessage(string errorMessage)
        {
            MessageBox.Show(
                                errorMessage,
                                "InputError",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
        }//ShowErrorMessage()
    }//class
}
