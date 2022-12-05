/** <!--
 *@title WinFormGUI / WinFormSample / 
 *@class MainExcelHtmlAgilitySample.cs
 *@class   └ new FormExcelHtmlAgilitySample() : Form
 *@class       └ new HttpClient()
 *@class       └ new Excel.Application()
 *@class       └ new HtmlAgilityPack.HtmlDocument()
 *@class       └ new BookInfomation()  //self defined
 *@using Excel = Microsoft.Office.Interop.Excel;
 *@using HtmlAgilityPack
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[499][500] p843 / Excel / HtmlAgilityPack
 *         秀和システム(=RRの出版社)の Home Page から、新刊情報を取得し、
 *         HTMLを解析した後、Excelで表示
 *         
 *         HtmlAgilityPackをインストール
 *         ・HtmlDocument.LoadHtml()  HTMLデータを解析できる
 *         ・DOMツリーを探索可能
 *         
 *@prepare HtmlAgilityPack Install
 *         NuGet
 *         PM> Install-Package HtmlAgilityPack
 *           :
 *         パッケージ 'HtmlAgilityPack.1.11.46' を 'packages.config' に追加しました
 *         'HtmlAgilityPack 1.11.46' が WinFormGUI に正常にインストールされました
 *         
 *@subject ButtonNewBook_Click() / 新刊情報の取得
 *         ・HTML<li> タグ内   class=""属性 'items' の NodeCollection を取得
 *         ・HTML<img>タグ内の alt=""  属性の値を取得 -> Title
 *         ・HTML<a>  タグ内   href="" 属性の値を取得 -> Link
 *         ・new BookInfoRR18() クラスの Property に保存して、
 *         ・Excelで表示
 *         
 *        【註】サンプルコードの HTML解析が、ここのページに特化し過ぎていて一般的ではない。
 *         <img alt="">に、書籍タイトルが記述されているかどうかは Webページの制作者次第で、
 *         検索の仕方に正確性を欠く
 */
#region -> HtmlAgilityPack.HtmlDocument, 
/*
 *@subject ◆interface IXPathNavigable -- System.Xml.XPath
 *         XPathNavigator  IXPathNavigable.CreateNavigator() 
 *             ↑
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
 *         + bool    htmlDocument.DisableServerSideCode 
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
 *         
 *         + string  htmlDocument.ParsedText { get; } 
 *         + int  htmlDocument.CheckSum { get; } 
 *         + HtmlNode  htmlDocument.DocumentNode { get; } 
 *         + IEnumerable<HtmlParseError>  htmlDocument.ParseErrors { get; } 
 *         + Action<HtmlDocument>  htmlDocument.ParseExecuting { get; set; } 
 *         + Encoding  htmlDocument.Encoding { get; } 
 *         + Encoding  htmlDocument.StreamEncoding { get; } 
 *         + Encoding  htmlDocument.DeclaredEncoding { get; } 
 *         + string    htmlDocument.Remainder { get; } 
 *         + int       htmlDocument.RemainderOffset { get; } 
 *         
 *         ＊Method
 *         + static string  HtmlDocument.GetXmlName(string name) 
 *         + static string  HtmlDocument.GetXmlName(string name, bool isAttribute, bool preserveXmlNamespaces) 
 *         + static string  HtmlDocument.HtmlEncode(string html) 
 *         + static bool    HtmlDocument.IsWhiteSpace(int c) 
 *         
 *         + XPathNavigator   htmlDocument.CreateNavigator() 
 *         + HtmlNode         htmlDocument.CreateElement(string name) 
 *         + HtmlNode         htmlDocument.GetElementbyId(string id) 
 *         + HtmlTextNode     htmlDocument.CreateTextNode() 
 *         + HtmlTextNode     htmlDocument.CreateTextNode(string text) 
 *         + HtmlCommentNode  htmlDocument.CreateComment() 
 *         + HtmlCommentNode  htmlDocument.CreateComment(string comment) 
 *         + HtmlAttribute  htmlDocument.CreateAttribute(string name) 
 *         + HtmlAttribute  htmlDocument.CreateAttribute(string name, string value) 
 *         + void           htmlDocument.UseAttributeOriginalName(string tagName) 
 *         + Encoding  htmlDocument.DetectEncoding(Stream stream) 
 *         + Encoding  htmlDocument.DetectEncoding(string path) 
 *         + Encoding  htmlDocument.DetectEncoding(Stream stream, bool checkHtml) 
 *         + Encoding  htmlDocument.DetectEncoding(TextReader reader) 
 *         + void      htmlDocument.DetectEncodingAndLoad(string path) 
 *         + void      htmlDocument.DetectEncodingAndLoad(string path, bool detectEncoding) 
 *         + Encoding  htmlDocument.DetectEncodingHtml(string html) 
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
 *
 *@subject ◆class HtmlNodeCollection : IList<HtmlNode>, ICollection<HtmlNode>, IEnumerable<HtmlNode>, IEnumerable
 *                                  -- HtmlAgilityPack
 *         + HtmlNodeCollection  new HtmlNodeCollection(HtmlNode parentnode) 
 *        
 *         + HtmlNode   this[string nodeName] { get; } 
 *         + int        this[HtmlNode node] { get; } 
 *         + HtmlNode   this[int index] { get; set; } 
 *         
 *         + int  htmlNodeCollection.Count { get; } 
 *         + bool  htmlNodeCollection.IsReadOnly { get; } 
 *         
 *         + int  htmlNodeCollection.IndexOf(HtmlNode item) 
 *         + int  htmlNodeCollection.GetNodeIndex(HtmlNode node) 
 *         + IEnumerable<HtmlNode>  htmlNodeCollection.Nodes() 
 *         + bool  htmlNodeCollection.Contains(HtmlNode item) 
 *         + void  htmlNodeCollection.Add(HtmlNode node) 
 *         + void  htmlNodeCollection.Add(HtmlNode node, bool setParent) 
 *         + void  htmlNodeCollection.Append(HtmlNode node) 
 *         + void  htmlNodeCollection.Replace(int index, HtmlNode node) 
 *         + void  htmlNodeCollection.Insert(int index, HtmlNode node) 
 *         + void  htmlNodeCollection.Prepend(HtmlNode node) 
 *         + void  htmlNodeCollection.CopyTo(HtmlNode[] array, int arrayIndex) 
 *         + IEnumerable<HtmlNode>  htmlNodeCollection.Elements() 
 *         + IEnumerable<HtmlNode>  htmlNodeCollection.Elements(string name) 
 *         + IEnumerable<HtmlNode>  htmlNodeCollection.Descendants() 
 *         + IEnumerable<HtmlNode>  htmlNodeCollection.Descendants(string name) 
 *         + static HtmlNode  HtmlNodeCollection.FindFirst(HtmlNodeCollection items, string name) 
 *         +        HtmlNode  htmlNodeCollection.FindFirst(string name) 
 *         + bool  htmlNodeCollection.Remove(HtmlNode item) 
 *         + bool  htmlNodeCollection.Remove(int index) 
 *         + void  htmlNodeCollection.RemoveAt(int index) 
 *         + void  htmlNodeCollection.Clear() 
 *         
 *@subject ◆class HtmlNode : IXPathNavigable
 *                        -- HtmlAgilityPack
 *         + HtmlNode  new HtmlNode(HtmlNodeType type, HtmlDocument ownerdocument, int index) 
 *         
 *         + static readonly string  HtmlNode.HtmlNodeTypeNameComment 
 *         + static readonly string  HtmlNode.HtmlNodeTypeNameDocument 
 *         + static readonly string  HtmlNode.HtmlNodeTypeNameText 
 *         + static Dictionary<string, HtmlElementFlag>  HtmlNode.ElementsFlags 
 *         
 *         + string    htmlNode.Name { get; set; } 
 *         + string    htmlNode.OriginalName { get; } 
 *         + string    htmlNode.OuterHtml { get; } 
 *         + string    htmlNode.InnerText { get; } 
 *         + string    htmlNode.InnerHtml { get; set; } 
 *         + string    htmlNode.Id { get; set; } 
 *         + string    htmlNode.XPath { get; } 
 *         + int       htmlNode.Line { get; } 
 *         + int       htmlNode.OuterStartIndex { get; } 
 *         + int       htmlNode.InnerStartIndex { get; } 
 *         + int       htmlNode.InnerLength { get; } 
 *         + int       htmlNode.OuterLength { get; } 
 *         + int       htmlNode.StreamPosition { get; } 
 *         + int       htmlNode.LinePosition { get; } 
 *         + int       htmlNode.Depth { get; set; } 
 *         + HtmlNodeType  htmlNode.NodeType { get; } 
 *         + HtmlDocument  htmlNode.OwnerDocument { get; } 
 *         + bool      htmlNode.Closed { get; } 
 *         + bool      htmlNode.HasChildNodes { get; } 
 *         + HtmlNodeCollection  htmlNode.ChildNodes { get; } 
 *         + HtmlNode  htmlNode.ParentNode { get; } 
 *         + HtmlNode  htmlNode.FirstChild { get; } 
 *         + HtmlNode  htmlNode.LastChild { get; } 
 *         + HtmlNode  htmlNode.EndNode { get; } 
 *         + HtmlNode  htmlNode.PreviousSibling { get; } 
 *         + HtmlNode  htmlNode.NextSibling { get; } 
 *         + bool      htmlNode.HasAttributes { get; } 
 *         + bool      htmlNode.HasClosingAttributes { get; } 
 *         + HtmlAttributeCollection  htmlNode.Attributes { get; } 
 *         + HtmlAttributeCollection  htmlNode.ClosingAttributes { get; }
 *         
 *         + static HtmlNode  HtmlNode.CreateNode(string html) 
 *         + static HtmlNode  HtmlNode.CreateNode(string html, Action<HtmlDocument> htmlDocumentBuilder) 
 *         + static bool  HtmlNode.IsCDataElement(string name) 
 *         + static bool  HtmlNode.IsClosedElement(string name) 
 *         + static bool  HtmlNode.IsEmptyElement(string name) 
 *         + static bool  HtmlNode.IsOverlappedClosingElement(string text) 
 *         + static bool  HtmlNode.CanOverlapElement(string name) 
 *         
 *         + XPathNavigator  htmlNode.CreateNavigator() 
 *         + XPathNavigator  htmlNode.CreateRootNavigator() 
 *         + bool      htmlNode.HasClass(string className) 
 *         + void      htmlNode.AddClass(string name) 
 *         + void      htmlNode.AddClass(string name, bool throwError) 
 *         + void      htmlNode.ReplaceClass(string newClass, string oldClass, bool throwError) 
 *         + void      htmlNode.ReplaceClass(string newClass, string oldClass) 
 *         + IEnumerable<string>    htmlNode.GetClasses() 
 *         + IEnumerable<HtmlNode>  htmlNode.Ancestors() 
 *         + IEnumerable<HtmlNode>  htmlNode.Ancestors(string name) 
 *         + IEnumerable<HtmlNode>  htmlNode.AncestorsAndSelf() 
 *         + IEnumerable<HtmlNode>  htmlNode.AncestorsAndSelf(string name) 
 *         + HtmlNode  htmlNode.AppendChild(HtmlNode newChild) 
 *         + void      htmlNode.AppendChildren(HtmlNodeCollection newChildren) 
 *         + HtmlNode  htmlNode.PrependChild(HtmlNode newChild) 
 *         + void      htmlNode.PrependChildren(HtmlNodeCollection newChildren) 
 *         + HtmlNodeCollection  htmlNode.SelectNodes(string xpath) 
 *         + HtmlNodeCollection  htmlNode.SelectNodes(XPathExpression xpath) 
 *         + HtmlNode  htmlNode.SelectSingleNode(string xpath) 
 *         + HtmlNode  htmlNode.SelectSingleNode(XPathExpression xpath) 
 *         + HtmlNode  htmlNode.InsertAfter(HtmlNode newChild, HtmlNode refChild) 
 *         + HtmlNode  htmlNode.InsertBefore(HtmlNode newChild, HtmlNode refChild) 
 *         + HtmlNode  htmlNode.Clone() 
 *         + HtmlNode  htmlNode.CloneNode(bool deep) 
 *         + HtmlNode  htmlNode.CloneNode(string newName) 
 *         + HtmlNode  htmlNode.CloneNode(string newName, bool deep) 
 *         + void      htmlNode.CopyFrom(HtmlNode node) 
 *         + void      htmlNode.CopyFrom(HtmlNode node, bool deep) 
 *         + void      htmlNode.MoveChild(HtmlNode child) 
 *         + void      htmlNode.MoveChildren(HtmlNodeCollection children) 
 *         + HtmlNode  htmlNode.ReplaceChild(HtmlNode newChild, HtmlNode oldChild) 
 *         + HtmlNode  htmlNode.Element(string name) 
 *         + IEnumerable<HtmlNode>  htmlNode.Elements(string name) 
 *         + IEnumerable<HtmlNode>  htmlNode.DescendantNodes(int level = 0) 
 *         + IEnumerable<HtmlNode>  htmlNode.DescendantNodesAndSelf() 
 *         + IEnumerable<HtmlNode>  htmlNode.Descendants() 
 *         + IEnumerable<HtmlNode>  htmlNode.Descendants(int level) 
 *         + IEnumerable<HtmlNode>  htmlNode.Descendants(string name) 
 *         + IEnumerable<HtmlNode>  htmlNode.DescendantsAndSelf() 
 *         + IEnumerable<HtmlNode>  htmlNode.DescendantsAndSelf(string name) 
 *         + IEnumerable<HtmlAttribute>  htmlNode.GetAttributes() 
 *         + IEnumerable<HtmlAttribute>  htmlNode.GetAttributes(params string[] attributeNames) 
 *         + IEnumerable<HtmlAttribute>  htmlNode.GetDataAttributes() 
 *         + IEnumerable<HtmlAttribute>  htmlNode.ChildAttributes(string name) 
 *         + string    htmlNode.GetAttributeValue(string name, string def) 
 *         + bool      htmlNode.GetAttributeValue(string name, bool def) 
 *         + int       htmlNode.GetAttributeValue(string name, int def) 
 *         + T         htmlNode.GetAttributeValue<T>(string name, T def) 
 *         + HtmlAttribute  htmlNode.GetDataAttribute(string key) 
 *         + string    htmlNode.GetDirectInnerText() 
 *         + T         htmlNode.GetEncapsulatedData<T>() 
 *         + T         htmlNode.GetEncapsulatedData<T>(HtmlDocument htmlDocument) 
 *         + object    htmlNode.GetEncapsulatedData(Type targetType, HtmlDocument htmlDocument = null) 
 *         + HtmlAttribute  htmlNode.SetAttributeValue(string name, string value) 
 *         + void      htmlNode.SetParent(HtmlNode parent) 
 *         + void      htmlNode.SetChildNodesId(HtmlNode chilNode) 
 *         + string    htmlNode.WriteTo() 
 *         + void      htmlNode.WriteTo(XmlWriter writer) 
 *         + void      htmlNode.WriteTo(TextWriter outText, int level = 0) 
 *         + string    htmlNode.WriteContentTo() 
 *         + void      htmlNode.WriteContentTo(TextWriter outText, int level = 0) 
 *         + void      htmlNode.RemoveClass() 
 *         + void      htmlNode.RemoveClass(string name) 
 *         + void      htmlNode.RemoveClass(bool throwError) 
 *         + void      htmlNode.RemoveClass(string name, bool throwError) 
 *         + void      htmlNode.Remove() 
 *         + void      htmlNode.RemoveAll() 
 *         + void      htmlNode.RemoveAllChildren() 
 *         + void      htmlNode.RemoveAllIDforNode(HtmlNode node) 
 *         + HtmlNode  htmlNode.RemoveChild(HtmlNode oldChild) 
 *         + HtmlNode  htmlNode.RemoveChild(HtmlNode oldChild, bool keepGrandChildren) 
 *         + void      htmlNode.RemoveChildren(HtmlNodeCollection oldChildren) 
 *
 *@subject ◆class HtmlAttributeCollection : IList<HtmlAttribute>, ICollection<HtmlAttribute>, IEnumerable<HtmlAttribute>, IEnumerable
 *                                       -- HtmlAgilityPack
 *         + HtmlAttribute  this[string name] { get; set; } 
 *         + HtmlAttribute  this[int index] { get; set; } 
 *         
 *         + int   htmlAttributeCollection.Count { get; } 
 *         + bool  htmlAttributeCollection.IsReadOnly { get; } 
 *         
 *         + void  htmlAttributeCollection.Add(string name, string value) 
 *         + void  htmlAttributeCollection.Add(HtmlAttribute item) 
 *         + void  htmlAttributeCollection.AddRange(IEnumerable<HtmlAttribute> items) 
 *         + void  htmlAttributeCollection.AddRange(Dictionary<string, string> items) 
 *         + HtmlAttribute  htmlAttributeCollection.Append(string name, string value) 
 *         + HtmlAttribute  htmlAttributeCollection.Append(HtmlAttribute newAttribute) 
 *         + HtmlAttribute  htmlAttributeCollection.Append(string name) 
 *         + HtmlAttribute  htmlAttributeCollection.Prepend(HtmlAttribute newAttribute) 
 *         + IEnumerable<HtmlAttribute>  htmlAttributeCollection.AttributesWithName(string attributeName) 
 *         + bool  htmlAttributeCollection.Contains(string name) 
 *         + bool  htmlAttributeCollection.Contains(HtmlAttribute item) 
 *         + void  htmlAttributeCollection.CopyTo(HtmlAttribute[] array, int arrayIndex) 
 *         + int   htmlAttributeCollection.IndexOf(HtmlAttribute item) 
 *         + void  htmlAttributeCollection.Insert(int index, HtmlAttribute item) 
 *         + void  htmlAttributeCollection.Remove() 
 *         + void  htmlAttributeCollection.Remove(string name) 
 *         + void  htmlAttributeCollection.Remove(HtmlAttribute attribute) 
 *         + void  htmlAttributeCollection.RemoveAll() 
 *         + void  htmlAttributeCollection.RemoveAt(int index) 
 *
 *@subject ◆class HtmlAttribute : IComparable
 *                             -- HtmlAgilityPack
 *         + string    htmlAttribute.Name { get; set; } 
 *         + string    htmlAttribute.Value { get; set; } 
 *         + int       htmlAttribute.ValueLength { get; } 
 *         + int       htmlAttribute.ValueStartIndex { get; } 
 *         + string    htmlAttribute.DeEntitizeValue { get; } 
 *         + int       htmlAttribute.Line { get; } 
 *         + int       htmlAttribute.LinePosition { get; } 
 *         + int       htmlAttribute.StreamPosition { get; } 
 *         + bool      htmlAttribute.UseOriginalName { get; set; } 
 *         + string    htmlAttribute.OriginalName { get; } 
 *         + string    htmlAttribute.XPath { get; } 
 *         + HtmlNode  htmlAttribute.OwnerNode { get; } 
 *         + HtmlDocument  htmlAttribute.OwnerDocument { get; } 
 *         + AttributeValueQuote  htmlAttribute.QuoteType { get; set; } 
 *         + HtmlAttribute  htmlAttribute.Clone() 
 *         + int     htmlAttribute.CompareTo(object obj) 
 *         + void    htmlAttribute.Remove() 
 *
 */
#endregion
/*
 *@see ImageExcelHtmlAgilityPackSample.jpg
 *@see 
 *@author shika
 *@date 2022-12-04
 * -->
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelHtmlAgilityPackSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelHtmlAgilityPackSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelHtmlAgilityPackSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelHtmlAgilityPackSample : Form
    {
        private readonly HttpClient client;
        private const string url = "https://www.shuwasystem.co.jp/";
        private readonly Excel.Application excelApp;
        private readonly Excel.Workbook workbook;
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly Button buttonNewBook;
        private readonly Button buttonBookInfo;
        private readonly ListBox listBox;

        public FormExcelHtmlAgilityPackSample()
        {
            this.Text = "FormExcelHtmlAgilityPackSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(480, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelHtmlAgilityPackSample");
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
                newWorkbook.Sheets.Add(After: newWorkbook.Sheets[1]);
                newWorkbook.SaveAs(Filename: path);
            }
            workbook = excelApp.Workbooks.Open(path);            

            //---- Controls ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            
            for (int i = 0; i < table.ColumnCount; i++)
            {
                table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100f / table.ColumnCount));
            }//for
            
            buttonNewBook = new Button()
            {
                Text = "New Book",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonNewBook.Click += new EventHandler(ButtonNewBook_Click);
            table.Controls.Add(buttonNewBook, 0, 0);

            buttonBookInfo = new Button()
            {
                Text = "Book Information",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonBookInfo.Click += new EventHandler(ButtonBookInfo_Click);
            table.Controls.Add(buttonBookInfo, 1, 0);

            listBox = new ListBox()
            {
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(listBox, 0, 1);
            table.SetColumnSpan(listBox, table.ColumnCount);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private async void ButtonNewBook_Click(object sender, EventArgs e)
        {
            try
            {
                //---- HttpClient Web Connect ----
                string htmlContent = await client.GetStringAsync(url);

                //---- HtmlDocument ----
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                HtmlAgilityPack.HtmlNodeCollection nodeCollection = 
                    htmlDoc.DocumentNode.SelectNodes("//li[@class='items']");
                List<string> itemList = new List<string>();
                List<BookInfoRR18> bookList = new List<BookInfoRR18>();

                foreach(HtmlAgilityPack.HtmlNode node in nodeCollection)
                {
                    HtmlAgilityPack.HtmlNode imgTag = node.SelectSingleNode(".//img");
                    string title = imgTag.GetAttributeValue("alt", def: "");
                    HtmlAgilityPack.HtmlNode aTag = node.SelectSingleNode(".//a");
                    string link = aTag.GetAttributeValue("href", def: "");

                    itemList.Add(title);
                    bookList.Add(new BookInfoRR18() { Title = title, Link = link, });
                }//foreach

                listBox.DataSource = itemList;

                //---- Excel ----
                Excel.Worksheet sheet1 = (Excel.Worksheet) workbook.Sheets[1];

                if(sheet1.Cells[1, 1].Value != "Title")
                {
                    sheet1.Cells[1, 1].Value = "Title";
                    sheet1.Cells[1, 2].Value = "Link";
                }

                int row = 2;
                foreach(BookInfoRR18 book in bookList)
                {
                    sheet1.Cells[row, 1] = book.Title;
                    sheet1.Cells[row, 2] = book.Link;

                    row++;
                }//foreach

                excelApp.Visible = true;
                workbook.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                client.CancelPendingRequests();
            }
        }//ButtonNewBook_Click()

        private void ButtonBookInfo_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                client.CancelPendingRequests();
            }
        }//ButtonBookInfo_Click()

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

    class BookInfoRR18
    {
        public string Title { get; set; }
        public string Link { get; set; }
        
    }//class BookInfoRR18
}
