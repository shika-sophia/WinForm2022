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
 *
 *@subject HttpClient => 〔MainHttpClientSample.cs〕
 */
#region -> Uri, UriBuilder
/*
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
 *         + Type  serializationInfo.ObjectType { get; } 
 *         + int  serializationInfo.MemberCount { get; } 
 *         + string  serializationInfo.AssemblyName { get; set; } 
 *         + string  serializationInfo.FullTypeName { get; set; } 
 *         + bool  serializationInfo.IsFullTypeNameSetExplicit { get; } 
 *         + bool  serializationInfo.IsAssemblyNameSetExplicit { get; } 
 *         + void  serializationInfo.AddValue(string name, sbyte value) 
 *         + void  serializationInfo.AddValue(string name, object value, Type type) 
 *         + void  serializationInfo.AddValue(string name, bool value) 
 *         + void  serializationInfo.AddValue(string name, DateTime value) 
 *         + void  serializationInfo.AddValue(string name, decimal value) 
 *         + void  serializationInfo.AddValue(string name, double value) 
 *         + void  serializationInfo.AddValue(string name, object value) 
 *         + void  serializationInfo.AddValue(string name, float value) 
 *         + void  serializationInfo.AddValue(string name, long value) 
 *         + void  serializationInfo.AddValue(string name, uint value) 
 *         + void  serializationInfo.AddValue(string name, int value) 
 *         + void  serializationInfo.AddValue(string name, ushort value) 
 *         + void  serializationInfo.AddValue(string name, short value) 
 *         + void  serializationInfo.AddValue(string name, byte value) 
 *         + void  serializationInfo.AddValue(string name, ulong value) 
 *         + void  serializationInfo.AddValue(string name, char value) 
 *         + bool  serializationInfo.GetBoolean(string name) 
 *         + byte  serializationInfo.GetByte(string name) 
 *         + char  serializationInfo.GetChar(string name) 
 *         + DateTime  serializationInfo.GetDateTime(string name) 
 *         + decimal  serializationInfo.GetDecimal(string name) 
 *         + double  serializationInfo.GetDouble(string name) 
 *         + SerializationInfoEnumerator  serializationInfo.GetEnumerator() 
 *         + short  serializationInfo.GetInt16(string name) 
 *         + int  serializationInfo.GetInt32(string name) 
 *         + long  serializationInfo.GetInt64(string name) 
 *         + sbyte  serializationInfo.GetSByte(string name) 
 *         + float  serializationInfo.GetSingle(string name) 
 *         + string  serializationInfo.GetString(string name) 
 *         + ushort  serializationInfo.GetUInt16(string name) 
 *         + uint  serializationInfo.GetUInt32(string name) 
 *         + ulong  serializationInfo.GetUInt64(string name) 
 *         + object  serializationInfo.GetValue(string name, Type type) 
 *         + void  serializationInfo.SetType(Type type) 
 *
 *@subject ◆struct StreamingContext -- System.Runtime.Serialization
 *         + StreamingContext  new StreamingContext(StreamingContextStates state) 
 *         + StreamingContext  new StreamingContext(StreamingContextStates state, object additional) 
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
 *         + UriBuilder  new UriBuilder() 
 *         + UriBuilder  new UriBuilder(string uri) 
 *         + UriBuilder  new UriBuilder(Uri uri) 
 *         + UriBuilder  new UriBuilder(string schemeName, string hostName) 
 *         + UriBuilder  new UriBuilder(string scheme, string host, int portNumber) 
 *         + UriBuilder  new UriBuilder(string scheme, string host, int port, string pathValue) 
 *         + UriBuilder  new UriBuilder(string scheme, string host, int port, string path, string extraValue) 
 *         + string  uriBuilder.Scheme { get; set; } 
 *         + string  uriBuilder.Query { get; set; } 
 *         + int  uriBuilder.Port { get; set; } 
 *         + string  uriBuilder.Path { get; set; } 
 *         + string  uriBuilder.Password { get; set; } 
 *         + string  uriBuilder.UserName { get; set; } 
 *         + string  uriBuilder.Fragment { get; set; } 
 *         + Uri  uriBuilder.Uri { get; } 
 *         + string  uriBuilder.Host { get; set; } 
 *         + bool  uriBuilder.Equals(object rparam) 
 *         + int  uriBuilder.GetHashCode() 
 *         + string  uriBuilder.ToString() 
 *
 */
#endregion
/*
 *@see ImageHttpClientUriQuerySample.jpg
 *@see 
 *@author shika
 *@date 2022-11-12
 */
using System;
using System.Drawing;
using System.Net.Http;
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
        private readonly TextBox textBoxUrl;
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

        private void Button_Click(object sender, EventArgs e)
        {

        }//Button_Click()
    }//class
}
