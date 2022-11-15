/** <!--
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainHttpClientPostFormSample.cs
 *@class   └ new FormHttpClientPostFormSample() : Form
 *@class       └ new new HttpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[430] p725 / HttpClientPostFormSample
 *         ・HTML <form method="POST">に 入力値を送信する
 *         ・Apache側の WebAPIは、未実装 => 404 Not Found となるが OK
 *         
 *         [Method]
 *         Task<HttpResponseMessage>  httpClient.PostAsync(string/Uri, HttpContent);
 *         Task<string>               httpContent.ReadAsStringAsync()
 *         
 *         [Example] 
 *         HttpClient client = new HttpClient();
 *         var dic = new Dictionary<string, string>();
 *         dic.Add("form_name", textBox.Text);
 *         var content = new FormUrlEncodedContent(dic);
 *         
 *         HttpResponseMessage res = await client.PostAsync(uri, content);
 *         HttpContent resContent = res.Content;
 *         string resString = await resContent.ReadAsStringAsync();

 *@subject ◆class FormUrlEncodedContent : ByteArrayContent
 *                     -- System.Net.Http     └ class ByteArrayContent : HttpContent
 *         ・MIME Type: application/x-www-form-urlencoded を使用してエンコードされた
 *           名前と値の taple のコンテナー。
 *         ・constructor のみのクラス
 *           
 *         + FormUrlEncodedContent  
 *              new FormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
 *              
 *@subject ◆class ByteArrayContent : HttpContent -- System.Net.Http
 *         + ByteArrayContent  new ByteArrayContent(byte[] content) 
 *         + ByteArrayContent  new ByteArrayContent(byte[] content, int offset, int count) 
 *         
 *         # Task<Stream>   byteArrayContent.CreateContentReadStreamAsync() 
 *         # Task           byteArrayContent.SerializeToStreamAsync(Stream stream, TransportContext context) 
 *         # internal bool  byteArrayContent.TryComputeLength(out long length) 
 *
 *@subject ◆abstract class TransportContext -- System.Net
 *         # TransportContext  TransportContext()
 *           [×] 'new' is not available, but 'base()' is OK from constructor of inherited class ONLY.
 *         
 *         + abstract ChannelBinding    transportContext.GetChannelBinding(ChannelBindingKind kind) 
 *         + IEnumerable<TokenBinding>  transportContext.GetTlsTokenBindings() 
 *
 *@see ImageHttpClientPostFormSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-15
 * -->
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainHttpClientPostFormSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormHttpClientPostFormSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormHttpClientPostFormSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHttpClientPostFormSample : Form
    {
        private readonly HttpClient client;
        private readonly TableLayoutPanel table;
        private readonly Button button;
        private readonly TextBox textBox;

        public FormHttpClientPostFormSample()
        {
            this.Text = "FormHttpClientPostFormSample";
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
                Text = "Post Form Value",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button);

            //---- TextBox ----
            textBox = new TextBox()
            {
                Multiline = true,
                Height = 60,
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
            if (String.IsNullOrEmpty(textBox.Text)) { return; }

            var uri = new Uri("http://localhost:80/api/Sample");
            var dic = new Dictionary<string, string>();
            dic.Add("form_name", textBox.Text);

            var content = new FormUrlEncodedContent(dic);
            try
            {
                using (HttpResponseMessage res = await client.PostAsync(uri, content))
                {
                    HttpContent resContent = res.Content;
                    string resString = await resContent.ReadAsStringAsync();

                    textBox.Text += resString;
                }//using
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{ex.GetType()}:\n" +
                    $"{ex.Message}\n");
            }
        }//Button_Click()
    }//class
}
