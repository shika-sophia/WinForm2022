/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainHttpClientPostJsonConvertSample.cs
 *@class   └ new FormHttpClientPostJsonConvertSample() : Form
 *@class       └ new HttpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[431][433] p727, p729 / HttpClient / JsonConvert, StringContent
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
 *@subject JSONによる送信
 *         ・事前に Serializeするオブジェクトを準備
 *         ・---- Serialize ----
 *           オブジェクトのプロパティを JSON形式に整形
 *         string  JsonConvert.SerializeObject(object)
 *         
 *         ・Request Header: ContentType = "application/json"
 *           =>〔MainHttpClientRequestHeaderSample.cs〕
 *         
 *          var content = new StringContent(json);
 *          content.Headers.ContentType =
 *              new MediaTypeHeaderValue("application/json");
 *          
 *          ・---- POST送信 ----
 *          HttpResponseMessage res = await client.PostAsync(uri, content);
 *
 *          ・---- Deserialize ----
 *            受信した JSON形式 string を Deserializeして オブジェクト化、戻り値を dynamicで取得する
 *            
 *          string jsonReturn = await res.Content.ReadAsStringAsync();
 *          dynamic objReturn = JsonConvert.DeserializeObject(jsonReturn);
 *          textBoxName.Text = objReturn.Name;
 *          textBoxAge.Text = objReturn.Age;
 */
#region -> XDocument, XmlDocument
/*
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
 *         + static object?  JsonConvert.DeserializeObject(string value) 
 *         + static object?  JsonConvert.DeserializeObject(string value, Type type) 
 *         + static object?  JsonConvert.DeserializeObject(string value, JsonSerializerSettings settings) 
 *         + static object?  JsonConvert.DeserializeObject(string value, Type? type, JsonSerializerSettings? settings) 
 *         + static object?  JsonConvert.DeserializeObject(string value, Type type, params JsonConverter[] converters) 
 *         + static T?       JsonConvert.DeserializeObject<T>(string value) 
 *         + static T?       JsonConvert.DeserializeObject<T>(string value, JsonSerializerSettings? settings) 
 *         + static T?       JsonConvert.DeserializeObject<T>(string value, params JsonConverter[] converters) 
 *         + static T?       JsonConvert.DeserializeAnonymousType<T>(string value, T anonymousTypeObject) 
 *         + static T?       JsonConvert.DeserializeAnonymousType<T>(string value, T anonymousTypeObject, JsonSerializerSettings settings)          
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
 *             └ class XmlDocument -- System.Xml       =>〔MainHttpClientXmlDocumentSample.cs〕
 *             └ class XDocument   -- System.Xml.Linq  =>〔MainHttpClientXmlDocumentSample.cs〕
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
 *         + IEqualityComparer?             jsonSerializerSettings.EqualityComparer { get; set; } 
 *         + ITraceWriter?                  jsonSerializerSettings.TraceWriter { get; set; } 
 *         + EventHandler<ErrorEventArgs>?  jsonSerializerSettings.Error { get; set; } 
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
 */
#endregion
/*
 *@see No Image (ImageHttpClientPostJsonConvertSample.jpg)
 *@see 
 *@author shika
 *@date 2022-11-15
 */
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly Label labelName;
        private readonly Label labelAge;
        private readonly Label labelNameResult;
        private readonly Label labelAgeResult;
        private readonly TextBox textBoxName;
        private readonly TextBox textBoxAge;
        private readonly TextBox textBoxNameResult;
        private readonly TextBox textBoxAgeResult;
        private readonly Button button;

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

            //==== Post ====
            var uri = new Uri("http://localhost:80/api/Sample");
            object obj = new
            {
                Name = "Sophia",
                Age = 24,
                Address = "Berlin",
            };

            //---- Selialize ----
            string json = JsonConvert.SerializeObject(obj);
            Console.WriteLine($"result: {json}");
            // result: {"Name":"Sophia","Age":24,"Address":"Berlin"}

            var content = new StringContent(json);
            content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json");
            HttpResponseMessage res = await client.PostAsync(uri, content);
            string jsonReturn = await res.Content.ReadAsStringAsync();

            //---- Deserialize ----
            dynamic objReturn = JsonConvert.DeserializeObject(jsonReturn);
            textBoxName.Text = objReturn.Name;
            textBoxAge.Text = objReturn.Age;
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
            if (inputInt < 0)
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
