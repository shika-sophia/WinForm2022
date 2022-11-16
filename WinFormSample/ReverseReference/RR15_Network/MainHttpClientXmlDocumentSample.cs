/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainHttpClientXmlDocumentSample.cs
 *@class   └ new FormHttpClientXmlDocumentSample() : Form
 *@class       └ new HttpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[432] p728 / HttpClientXmlDocumentSample
 *         ・Apache WebAPI 未実装 
 *             Web Serverの戻り値を XML形式にすると、XDocumentクラスで取得可
 *
 *@subject ◆class XDocument : XContainer
 *                         -- System.Xml.Linq
 *         + XDocument  new XDocument() 
 *         + XDocument  new XDocument(params object[] content) 
 *         + XDocument  new XDocument(XDocument other) 
 *         + XDocument  new XDocument(XDeclaration declaration, params object[] content) 
 *         
 *         + XDocumentType  xDocument.DocumentType { get; } 
 *         + XDeclaration   xDocument.Declaration { get; set; } 
 *         + XmlNodeType    xDocument.NodeType { get; } 
 *         + XElement       xDocument.Root { get; } 
 *         + static XDocument  XDocument.Load(TextReader textReader) 
 *         + static XDocument  XDocument.Load(TextReader textReader, LoadOptions options) 
 *         + static XDocument  XDocument.Load(XmlReader reader) 
 *         + static XDocument  XDocument.Load(XmlReader reader, LoadOptions options) 
 *         + static XDocument  XDocument.Load(Stream stream) 
 *         + static XDocument  XDocument.Load(string uri) 
 *         + static XDocument  XDocument.Load(string uri, LoadOptions options) 
 *         + static XDocument  XDocument.Load(Stream stream, LoadOptions options) 
 *         + static XDocument  XDocument.Parse(string text) 
 *         + static XDocument  XDocument.Parse(string text, LoadOptions options) 
 *         + void  xDocument.Save(Stream stream, SaveOptions options) 
 *         + void  xDocument.Save(TextWriter textWriter) 
 *         + void  xDocument.Save(TextWriter textWriter, SaveOptions options) 
 *         + void  xDocument.Save(XmlWriter writer) 
 *         + void  xDocument.Save(string fileName) 
 *         + void  xDocument.Save(string fileName, SaveOptions options) 
 *         + void  xDocument.Save(Stream stream) 
 *         + void  xDocument.WriteTo(XmlWriter writer) 
 *         
 *@subject ◆abstract class XContainer : XNode
 *                          -- System.Xml.Linq
 *         + XNode  xContainer.LastNode { get; } 
 *         + XNode  xContainer.FirstNode { get; } 
 *         
 *         + void   xContainer.Add(object content) 
 *         + void   xContainer.Add(params object[] content) 
 *         + void   xContainer.AddFirst(params object[] content) 
 *         + void   xContainer.AddFirst(object content) 
 *         + XmlWriter              xContainer.CreateWriter() 
 *         + IEnumerable<XElement>  xContainer.Descendants() 
 *         + IEnumerable<XElement>  xContainer.Descendants(XName name) 
 *         + IEnumerable<XNode>     xContainer.DescendantNodes()  
 *         + XElement               xContainer.Element(XName name) 
 *         + IEnumerable<XElement>  xContainer.Elements() 
 *         + IEnumerable<XElement>  xContainer.Elements(XName name) 
 *         + IEnumerable<XNode>     xContainer.Nodes() 
 *         + void  xContainer.RemoveNodes() 
 *         + void  xContainer.ReplaceNodes(object content) 
 *         + void  xContainer.ReplaceNodes(params object[] content) 
 *         
 *@subject ◆abstract class XNode : XObject
 *                     -- System.Xml.Linq
 *         + static XNodeEqualityComparer       XNode.EqualityComparer { get; } 
 *         + static XNodeDocumentOrderComparer  XNode.DocumentOrderComparer { get; } 
 *         + XNode  xNode.NextNode { get; } 
 *         + XNode  xNode.PreviousNode { get; } 
 *         + static int    XNode.CompareDocumentOrder(XNode n1, XNode n2) 
 *         + static bool   XNode.DeepEquals(XNode n1, XNode n2) 
 *         + static XNode  XNode.ReadFrom(XmlReader reader) 
 *         + void  xNode.AddBeforeSelf(object content) 
 *         + void  xNode.AddBeforeSelf(params object[] content) 
 *         + void  xNode.AddAfterSelf(object content) 
 *         + void  xNode.AddAfterSelf(params object[] content) 
 *         + IEnumerable<XElement>  xNode.Ancestors() 
 *         + IEnumerable<XElement>  xNode.Ancestors(XName name) 
 *         + XmlReader  xNode.CreateReader() 
 *         + XmlReader  xNode.CreateReader(ReaderOptions readerOptions) 
 *         + IEnumerable<XElement>  xNode.ElementsBeforeSelf() 
 *         + IEnumerable<XElement>  xNode.ElementsBeforeSelf(XName name) 
 *         + IEnumerable<XElement>  xNode.ElementsAfterSelf() 
 *         + IEnumerable<XElement>  xNode.ElementsAfterSelf(XName name) 
 *         + bool  xNode.IsBefore(XNode node) 
 *         + bool  xNode.IsAfter(XNode node) 
 *         + IEnumerable<XNode>  xNode.NodesAfterSelf() 
 *         + IEnumerable<XNode>  xNode.NodesBeforeSelf() 
 *         + void  xNode.Remove() 
 *         + void  xNode.ReplaceWith(object content) 
 *         + void  xNode.ReplaceWith(params object[] content) 
 *         + string  xNode.ToString() 
 *         + string  xNode.ToString(SaveOptions options) 
 *         + abstract void  xNode.WriteTo(XmlWriter writer) 
 *       
 *@see No Image (ImageHttpClientXmlDocumentSample.jpg)
 *@see 
 *@author shika
 *@date 2022-11-16
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainHttpClientXmlDocumentSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormHttpClientXmlDocumentSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormHttpClientXmlDocumentSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHttpClientXmlDocumentSample : Form
    {
        private readonly HttpClient client;
        private readonly TableLayoutPanel table;
        private readonly Label labelName;
        private readonly Label labelAge;
        private readonly Label labelNameResult;
        private readonly Label labelAgeResult;
        private readonly TextBox textBoxName;
        private readonly TextBox textBoxAge;
        private readonly TextBox textBoxNameResult;
        private readonly TextBox textBoxAgeResult;
        private readonly Button button;

        public FormHttpClientXmlDocumentSample()
        {
            this.Text = "FormHttpClientXmlDocumentSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(480, 240);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- HttpClient ----
            client = new HttpClient();

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 5,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80f));

            //---- Label ----
            labelName = new Label()
            {
                Text = "Name:",
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelName, 0, 0);

            labelAge = new Label()
            {
                Text = "Age:",
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelAge, 0, 1);

            labelNameResult = new Label()
            {
                Text = "Name:",
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelNameResult, 0, 3);

            labelAgeResult = new Label()
            {
                Text = "Age:",
                TextAlign = ContentAlignment.TopRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelAgeResult, 0, 4);

            //---- TextBox ----
            textBoxName = new TextBox()
            {
                Multiline = false,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxName, 1, 0);

            textBoxAge = new TextBox()
            {
                Multiline = false,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxAge, 1, 1);

            textBoxNameResult = new TextBox()
            {
                Multiline = false,
                ReadOnly = true,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxNameResult, 1, 3);

            textBoxAgeResult = new TextBox()
            {
                Multiline = false,
                ReadOnly = true,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBoxAgeResult, 1, 4);

            //---- Button ----
            button = new Button()
            {
                Text = "XML Value",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button, 0, 2);
            table.SetColumnSpan(button, 2);

            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private async void Button_Click(object sender, EventArgs e)
        {
            //---- Validation ----
            if (String.IsNullOrEmpty(textBoxName.Text) 
                || String.IsNullOrEmpty(textBoxAge.Text)) { return; }

            bool canInputName = ValidateInputName(textBoxName.Text);
            bool canInputAge = ValidateInputAge(textBoxAge.Text);
            if (!canInputName || !canInputAge) { return; }

            //---- Post ---
            var uri = new Uri("http://localhost:80/api/Sample");
            var dic = new Dictionary<string, string>();
            dic.Add("name", textBoxName.Text);
            dic.Add("age", textBoxAge.Text);

            var content = new FormUrlEncodedContent(dic);
            HttpResponseMessage res = await client.PostAsync(uri, content);

            //---- Load ----
            XDocument xDoc = XDocument.Load(
                await res.Content.ReadAsStreamAsync());
            textBoxNameResult.Text = 
                xDoc.Document.Element("person").Element("name").Value;
            textBoxAgeResult.Text = 
                xDoc.Document.Element("person").Element("age").Value;
        }//Button_Click()

        private bool ValidateInputName(string input)
        {
            var errorMessageBld = new StringBuilder();

            //----Less Length----
            if (input.Length <= 2)
            {
                errorMessageBld.Append(
                    $"The Name should be described over 3 characters.{Environment.NewLine}");
            }

            //---- Anti Script ----
            Regex regexAntiScript = new Regex(@"[<>&;/.]+");
            bool isMatch = regexAntiScript.IsMatch(input);

            if (isMatch)
            {
                errorMessageBld.Append(
                    $"<！> Invalid Input ! {Environment.NewLine}");
            }

            if (errorMessageBld.Length > 0)
            {
                ShowErrorMessage(errorMessageBld.ToString());
                return false;
            }

            return true; // bool canInputName
        }//ValidateInputName()

        private bool ValidateInputAge(string input)
        {
            var errorMessageBld = new StringBuilder();

            //----Not Digit ----
            if (!input.Trim().ToCharArray().All(c => Char.IsDigit(c)))
            {
                errorMessageBld.Append(
                    $"The Age should be described by Digit ONLY.{Environment.NewLine}");
            }

            int inputInt = Int32.Parse(input.Trim());
            if(inputInt < 0) 
            {
                errorMessageBld.Append(
                    $"The Age should be described by plus value.{Environment.NewLine}");
            }

            //---- Anti Script ----
            Regex regexAntiScript = new Regex(@"[<>&;/.]+");
            bool isMatch = regexAntiScript.IsMatch(input);

            if (isMatch)
            {
                errorMessageBld.Append(
                    $"<！> Invalid Input ! {Environment.NewLine}");
            }

            if (errorMessageBld.Length > 0)
            {
                ShowErrorMessage(errorMessageBld.ToString());
                return false;
            }

            return true; // bool canInputName
        }//ValidateInputName()
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
