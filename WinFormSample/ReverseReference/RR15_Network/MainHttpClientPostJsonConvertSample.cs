﻿/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainHttpClientPostJsonConvertSample.cs
 *@class   └ new FormHttpClientPostJsonConvertSample() : Form
 *@class       └ new HttpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[431] p727 / HttpClient / JsonConvert, StringContent
 *         ・JSON: JavaScript Object Notation: 
 *                 JavaScriptの ObjectLiteral形式を元にした記法 =>〔NT196〕
 *                 [Example] { "key":"value", "key":"value", ... }
 *                 
 *         ・JSON形式で POST送信
 *         ・Apache WebAPI 未実装 -> 404 Not Found
 *         
 *@prepare NuGet -> JsonConvertクラス
 *         PM> Install-Package Newtonsoft.Json
 *               '.NETFramework,Version=v4.8' を対象とするプロジェクト 'WinFormGUI' に関して、
 *               パッケージ 'Newtonsoft.Json.13.0.1' の依存関係情報の収集を試行しています
 *               パッケージ 'Newtonsoft.Json.13.0.1' をインストールするアクションが解決されました
 *               'nuget.org' からパッケージ 'Newtonsoft.Json 13.0.1' を取得しています。
 *               パッケージ 'Newtonsoft.Json.13.0.1' を 'packages.config' に追加しました
 *               'Newtonsoft.Json 13.0.1' が WinFormGUI に正常にインストールされました 
 *
 *@subject ◆static class JsonConvert -- Newtonsoft.Json
 *         + static readonly string  JsonConvert.True 
 *         + static readonly string  JsonConvert.False 
 *         + static readonly string  JsonConvert.Null 
 *         + static readonly string  JsonConvert.Undefined 
 *         + static readonly string  JsonConvert.PositiveInfinity 
 *         + static readonly string  JsonConvert.NegativeInfinity 
 *         + static readonly string  JsonConvert.NaN 
 *         
 *         + static string  JsonConvert.SerializeObject(object? value) 
 *         + static string  JsonConvert.SerializeObject(object? value, Formatting formatting) 
 *         + static string  JsonConvert.SerializeObject(object? value, JsonSerializerSettings? settings) 
 *         + static string  JsonConvert.SerializeObject(object? value, Formatting formatting, JsonSerializerSettings? settings) 
 *         + static string  JsonConvert.SerializeObject(object? value, Type? type, JsonSerializerSettings? settings) 
 *         + static string  JsonConvert.SerializeObject(object? value, Type? type, Formatting formatting, JsonSerializerSettings? settings) 
 *         + static string  JsonConvert.SerializeObject(object? value, params JsonConverter[] converters) 
 *         + static string  JsonConvert.SerializeObject(object? value, Formatting formatting, params JsonConverter[] converters) 
 *         + static string  JsonConvert.SerializeXNode(XObject? node) 
 *         + static string  JsonConvert.SerializeXNode(XObject? node, Formatting formatting) 
 *         + static string  JsonConvert.SerializeXNode(XObject? node, Formatting formatting, bool omitRootObject) 
 *         + static string  JsonConvert.SerializeXmlNode(XmlNode? node) 
 *         + static string  JsonConvert.SerializeXmlNode(XmlNode? node, Formatting formatting) 
 *         + static string  JsonConvert.SerializeXmlNode(XmlNode? node, Formatting formatting, bool omitRootObject) 
 *             └ class JsonSerializerSettings 〔below〕
 *             
 *         + static Func<JsonSerializerSettings>?  
 *                           JsonConvert.DefaultSettings { get; set; } 
 *         + static T?       JsonConvert.DeserializeAnonymousType<T>(string value, T anonymousTypeObject) 
 *         + static T?       JsonConvert.DeserializeAnonymousType<T>(string value, T anonymousTypeObject, JsonSerializerSettings settings)          
 *         + static object?  JsonConvert.DeserializeObject(string value) 
 *         + static object?  JsonConvert.DeserializeObject(string value, Type type) 
 *         + static object?  JsonConvert.DeserializeObject(string value, JsonSerializerSettings settings) 
 *         + static object?  JsonConvert.DeserializeObject(string value, Type? type, JsonSerializerSettings? settings) 
 *         + static object?  JsonConvert.DeserializeObject(string value, Type type, params JsonConverter[] converters) 
 *         + static T?       JsonConvert.DeserializeObject<T>(string value) 
 *         + static T?       JsonConvert.DeserializeObject<T>(string value, JsonSerializerSettings? settings) 
 *         + static T?       JsonConvert.DeserializeObject<T>(string value, params JsonConverter[] converters) 
 *         + static XmlDocument?  JsonConvert.DeserializeXmlNode(string value, string? deserializeRootElementName, bool writeArrayAttribute, bool encodeSpecialCharacters) 
 *         + static XmlDocument?  JsonConvert.DeserializeXmlNode(string value, string? deserializeRootElementName, bool writeArrayAttribute) 
 *         + static XmlDocument?  JsonConvert.DeserializeXmlNode(string value, string? deserializeRootElementName) 
 *         + static XmlDocument?  JsonConvert.DeserializeXmlNode(string value) 
 *         + static XDocument?    JsonConvert.DeserializeXNode(string value) 
 *         + static XDocument?    JsonConvert.DeserializeXNode(string value, string? deserializeRootElementName) 
 *         + static XDocument?    JsonConvert.DeserializeXNode(string value, string? deserializeRootElementName, bool writeArrayAttribute) 
 *         + static XDocument?    JsonConvert.DeserializeXNode(string value, string? deserializeRootElementName, bool writeArrayAttribute, bool encodeSpecialCharacters) 
 *         + static void          JsonConvert.PopulateObject(string value, object target) 
 *         + static void          JsonConvert.PopulateObject(string value, object target, JsonSerializerSettings? settings) 
 *         
 *         + static string  JsonConvert.ToString(object? value) 
 *         + static string  JsonConvert.ToString(DateTimeOffset value) 
 *         + static string  JsonConvert.ToString(DateTimeOffset value, DateFormatHandling format) 
 *         + static string  JsonConvert.ToString(bool value) 
 *         + static string  JsonConvert.ToString(char value) 
 *         + static string  JsonConvert.ToString(Enum value) 
 *         + static string  JsonConvert.ToString(int value) 
 *         + static string  JsonConvert.ToString(short value) 
 *         + static string  JsonConvert.ToString(ushort value) 
 *         + static string  JsonConvert.ToString(uint value) 
 *         + static string  JsonConvert.ToString(ulong value) 
 *         + static string  JsonConvert.ToString(float value) 
 *         + static string  JsonConvert.ToString(double value) 
 *         + static string  JsonConvert.ToString(byte value) 
 *         + static string  JsonConvert.ToString(sbyte value) 
 *         + static string  JsonConvert.ToString(decimal value) 
 *         + static string  JsonConvert.ToString(Guid value) 
 *         + static string  JsonConvert.ToString(TimeSpan value) 
 *         + static string  JsonConvert.ToString(Uri? value) 
 *         + static string  JsonConvert.ToString(string? value) 
 *         + static string  JsonConvert.ToString(string? value, char delimiter) 
 *         + static string  JsonConvert.ToString(DateTime value) 
 *         + static string  JsonConvert.ToString(string? value, char delimiter, StringEscapeHandling stringEscapeHandling) 
 *         + static string  JsonConvert.ToString(long value) 
 *         + static string  JsonConvert.ToString(DateTime value, DateFormatHandling format, DateTimeZoneHandling timeZoneHandling)
 *         
 *@subject ◆class JsonSerializerSettings -- Newtonsoft.Json
 *         + JsonSerializerSettings  new JsonSerializerSettings() 
 *         
 *         + StreamingContext  jsonSerializerSettings.Context { get; set; } 
 *         + bool              jsonSerializerSettings.CheckAdditionalContent { get; set; } 
 *         + CultureInfo       jsonSerializerSettings.Culture { get; set; } 
 *         + string            jsonSerializerSettings.DateFormatString { get; set; } 
 *         + Formatting        jsonSerializerSettings.Formatting { get; set; } 
 *         + int?              jsonSerializerSettings.MaxDepth { get; set; } 
 *         + SerializationBinder?   jsonSerializerSettings.Binder { get; set; } 
 *         + ISerializationBinder?  jsonSerializerSettings.SerializationBinder { get; set; } 
 *         
 *         + IList<JsonConverter>           jsonSerializerSettings.Converters { get; set; } 
 *         + ITraceWriter?                  jsonSerializerSettings.TraceWriter { get; set; } 
 *         + EventHandler<ErrorEventArgs>?  jsonSerializerSettings.Error { get; set; } 
 *         + IEqualityComparer?             jsonSerializerSettings.EqualityComparer { get; set; } 
 *         + IContractResolver?             jsonSerializerSettings.ContractResolver { get; set; } 
 *         + IReferenceResolver?            jsonSerializerSettings.ReferenceResolver { get; set; } 
 *         + Func<IReferenceResolver?>?     jsonSerializerSettings.ReferenceResolverProvider { get; set; } 
 *         + FormatterAssemblyStyle         jsonSerializerSettings.TypeNameAssemblyFormat { get; set; } 
 *         
 *         + ConstructorHandling   jsonSerializerSettings.ConstructorHandling { get; set; } 
 *         + DefaultValueHandling  jsonSerializerSettings.DefaultValueHandling { get; set; } 
 *         + StringEscapeHandling  jsonSerializerSettings.StringEscapeHandling { get; set; } 
 *         + FloatParseHandling    jsonSerializerSettings.FloatParseHandling { get; set; } 
 *         + FloatFormatHandling   jsonSerializerSettings.FloatFormatHandling { get; set; } 
 *         + DateParseHandling     jsonSerializerSettings.DateParseHandling { get; set; } 
 *         + DateTimeZoneHandling  jsonSerializerSettings.DateTimeZoneHandling { get; set; } 
 *         + DateFormatHandling    jsonSerializerSettings.DateFormatHandling { get; set; } 
 *         + TypeNameHandling      jsonSerializerSettings.TypeNameHandling { get; set; } 
 *         + NullValueHandling       jsonSerializerSettings.NullValueHandling { get; set; } 
 *         + ObjectCreationHandling  jsonSerializerSettings.ObjectCreationHandling { get; set; } 
 *         + MissingMemberHandling   jsonSerializerSettings.MissingMemberHandling { get; set; } 
 *         + ReferenceLoopHandling   jsonSerializerSettings.ReferenceLoopHandling { get; set; } 
 *         + TypeNameAssemblyFormatHandling  jsonSerializerSettings.TypeNameAssemblyFormatHandling { get; set; } 
 *         + PreserveReferencesHandling      jsonSerializerSettings.PreserveReferencesHandling { get; set; } 
 *         + MetadataPropertyHandling        jsonSerializerSettings.MetadataPropertyHandling { get; set; } 
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
 *@subject ◆class XmlDocument : XmlNode  -- System.Xml
 *         ・XML形式
 *         
 *         + XmlDocument  new XmlDocument() 
 *         + XmlDocument  new XmlDocument(XmlNameTable nt) 
 *         # XmlDocument  internal XmlDocument(XmlImplementation imp)
 *         
 *         + string  xmlDocument.Name { get; } 
 *         + string  xmlDocument.LocalName { get; } 
 *         + string  xmlDocument.InnerText { set; } 
 *         + string  xmlDocument.InnerXml { get; set; } 
 *         + string  xmlDocument.BaseURI { get; } 
 *         + bool    xmlDocument.PreserveWhitespace { get; set; } 
 *         + bool    xmlDocument.IsReadOnly { get; } 
 *         + XmlNode            xmlDocument.ParentNode { get; } 
 *         + XmlDocumentType    xmlDocument.DocumentType { get; } 
 *         + XmlImplementation  xmlDocument.Implementation { get; } 
 *         + XmlElement         xmlDocument.DocumentElement { get; } 
 *         + XmlDocument        xmlDocument.OwnerDocument { get; } 
 *         + XmlSchemaSet       xmlDocument.Schemas { get; set; } 
 *         + XmlResolver        xmlDocument.XmlResolver { set; } 
 *         + XmlNameTable       xmlDocument.NameTable { get; } 
 *         + IXmlSchemaInfo     xmlDocument.SchemaInfo { get; } 
 *         + XmlNodeType        xmlDocument.NodeType { get; }
 *         
 *         + XmlElement  xmlDocument.CreateElement(string name) 
 *         + XmlElement  xmlDocument.CreateElement(string qualifiedName, string namespaceURI) 
 *         + XmlElement  xmlDocument.CreateElement(string prefix, string localName, string namespaceURI) 
 *         + XmlAttribute  xmlDocument.CreateAttribute(string qualifiedName, string namespaceURI) 
 *         + XmlAttribute  xmlDocument.CreateAttribute(string name) 
 *         + XmlAttribute  xmlDocument.CreateAttribute(string prefix, string localName, string namespaceURI) 
 *         + XmlCDataSection      xmlDocument.CreateCDataSection(string data) 
 *         + XmlComment           xmlDocument.CreateComment(string data) 
 *         + XmlDocumentFragment  xmlDocument.CreateDocumentFragment() 
 *         + XmlDocumentType      xmlDocument.CreateDocumentType(string name, string +Id, string systemId, string internalSubset) 
 *         + XmlEntityReference   xmlDocument.CreateEntityReference(string name) 
 *         + XPathNavigator       xmlDocument.CreateNavigator() 
 *         + XmlText  xmlDocument.CreateTextNode(string text) 
 *         + XmlNode  xmlDocument.CreateNode(string nodeTypeString, string name, string namespaceURI) 
 *         + XmlNode  xmlDocument.CreateNode(XmlNodeType type, string prefix, string name, string namespaceURI) 
 *         + XmlNode  xmlDocument.CreateNode(XmlNodeType type, string name, string namespaceURI) 
 *         + XmlNode  xmlDocument.ImportNode(XmlNode node, bool deep) 
 *         + XmlNode  xmlDocument.CloneNode(bool deep) 
 *         + XmlProcessingInstruction  xmlDocument.CreateProcessingInstruction(string target, string data) 
 *         + XmlSignificantWhitespace  xmlDocument.CreateSignificantWhitespace(string text) 
 *         + XmlWhitespace   xmlDocument.CreateWhitespace(string text) 
 *         + XmlDeclaration  xmlDocument.CreateXmlDeclaration(string version, string encoding, string standalone) 
 *         + XmlElement      xmlDocument.GetElementById(string elementId) 
 *         + XmlNodeList     xmlDocument.GetElementsByTagName(string name) 
 *         + XmlNodeList     xmlDocument.GetElementsByTagName(string localName, string namespaceURI) 
 *         + void  xmlDocument.Load(string filename) 
 *         + void  xmlDocument.Load(Stream inStream) 
 *         + void  xmlDocument.Load(XmlReader reader) 
 *         + void  xmlDocument.Load(TextReader txtReader) 
 *         + void  xmlDocument.LoadXml(string xml) 
 *         + XmlNode  xmlDocument.ReadNode(XmlReader reader) 
 *         + void  xmlDocument.Save(XmlWriter w) 
 *         + void  xmlDocument.Save(TextWriter writer) 
 *         + void  xmlDocument.Save(string filename) 
 *         + void  xmlDocument.Save(Stream outStream) 
 *         + void  xmlDocument.Validate(ValidationEventHandler validationEventHandler) 
 *         + void  xmlDocument.Validate(ValidationEventHandler validationEventHandler, XmlNode nodeToValidate) 
 *         + void  xmlDocument.WriteContentTo(XmlWriter xw) 
 *         + void  xmlDocument.WriteTo(XmlWriter w) 
 *         # internal XmlAttribute  xmlDocument.CreateDefaultAttribute(string prefix, string localName, string namespaceURI) 
 *         # internal XPathNavigator  xmlDocument.CreateNavigator(XmlNode node) 
 *         
 *         + event XmlNodeChangedEventHandler  xmlDocument.NodeInserting 
 *         + event XmlNodeChangedEventHandler  xmlDocument.NodeInserted 
 *         + event XmlNodeChangedEventHandler  xmlDocument.NodeRemoving 
 *         + event XmlNodeChangedEventHandler  xmlDocument.NodeRemoved 
 *         + event XmlNodeChangedEventHandler  xmlDocument.NodeChanging 
 *         + event XmlNodeChangedEventHandler  xmlDocument.NodeChanged 
 *         
 *@subject ◆abstract class XmlNode : ICloneable, IEnumerable, IXPathNavigable
 *                       -- System.Xml
 *         + XmlElement   xmlNode.this[string name] { get; } 
 *         + XmlElement   xmlNode.this[string localname, string ns] { get; } 
 *         
 *         + abstract string  xmlNode.Name { get; } 
 *         + abstract string  xmlNode.LocalName { get; } 
 *         + abstract XmlNodeType  xmlNode.NodeType { get; } 
 *         + string  xmlNode.Value { get; set; } 
 *         + string  xmlNode.NamespaceURI { get; } 
 *         + string  xmlNode.BaseURI { get; } 
 *         + string  xmlNode.Prefix { get; set; } 
 *         + string  xmlNode.InnerText { get; set; } 
 *         + string  xmlNode.InnerXml { get; set; } 
 *         + string  xmlNode.OuterXml { get; } 
 *         + bool    xmlNode.IsReadOnly { get; } 
 *         + XmlNode      xmlNode.PreviousText { get; } 
 *         + XmlNode      xmlNode.ParentNode { get; } 
 *         + XmlNode      xmlNode.PreviousSibling { get; } 
 *         + XmlNode      xmlNode.NextSibling { get; } 
 *         + XmlNodeList  xmlNode.ChildNodes { get; } 
 *         + IXmlSchemaInfo  xmlNode.SchemaInfo { get; } 
 *         + XmlAttributeCollection  xmlNode.Attributes { get; } 
 *         + XmlDocument  xmlNode.OwnerDocument { get; } 
 *         + XmlNode      xmlNode.FirstChild { get; } 
 *         + XmlNode      xmlNode.LastChild { get; } 
 *         + bool         xmlNode.HasChildNodes { get; } 
 *         
 *         + XmlNode         xmlNode.AppendChild(XmlNode newChild) 
 *         + void            xmlNode.Normalize() 
 *         + bool            xmlNode.Supports(string feature, string version) 
 *         + IEnumerator     xmlNode.GetEnumerator() 
 *         + string          xmlNode.GetNamespaceOfPrefix(string prefix) 
 *         + string          xmlNode.GetPrefixOfNamespace(string namespaceURI) 
 *         + XmlNode         xmlNode.InsertBefore(XmlNode newChild, XmlNode refChild) 
 *         + XmlNode         xmlNode.InsertAfter(XmlNode newChild, XmlNode refChild) 
 *         + XmlNode         xmlNode.PrependChild(XmlNode newChild) 
 *         + XPathNavigator  xmlNode.CreateNavigator() 
 *         + XmlNode         xmlNode.SelectSingleNode(string xpath, XmlNamespaceManager nsmgr) 
 *         + XmlNode         xmlNode.SelectSingleNode(string xpath) 
 *         + XmlNodeList     xmlNode.SelectNodes(string xpath, XmlNamespaceManager nsmgr) 
 *         + XmlNodeList     xmlNode.SelectNodes(string xpath) 
 *         + XmlNode         xmlNode.Clone() 
 *         + XmlNode         xmlNode.RemoveChild(XmlNode oldChild) 
 *         + XmlNode         xmlNode.ReplaceChild(XmlNode newChild, XmlNode oldChild) 
 *         + void            xmlNode.RemoveAll() 
 *         + abstract void     xmlNode.WriteTo(XmlWriter w) 
 *         + abstract void     xmlNode.WriteContentTo(XmlWriter w) 
 *         + abstract XmlNode  xmlNode.CloneNode(bool deep) 
 *         
 *@see No Image (ImageHttpClientPostJsonConvertSample.jpg)
 *@see 
 *@author shika
 *@date 2022-11-15
 */
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainHttpClientPostJsonConvertSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormHttpClientPostJsonConvertSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormHttpClientPostJsonConvertSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHttpClientPostJsonConvertSample : Form
    {
        private readonly HttpClient client;
        private readonly TableLayoutPanel table;
        private readonly Button button;
        private readonly TextBox textBox;

        public FormHttpClientPostJsonConvertSample()
        {
            this.Text = "FormHttpClientPostJsonConvertSample";
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
                Text = "Post Form JSON Value",
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
            var uri = new Uri("http://localhost:80/api/Sample");
            object obj = new
            {
                Name = "Sophia",
                Age = 24,
                Address = "Berlin",
            };

            string json = JsonConvert.SerializeObject(obj);
            textBox.Text = json + Environment.NewLine;
            // result: {"Name":"Sophia","Age":24,"Address":"Berlin"}

            var content = new StringContent(json);
            content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json");
            HttpResponseMessage res = await client.PostAsync(uri, content);

            textBox.Text += await res.Content.ReadAsStringAsync();
        }//Button_Click()
    }//class
}