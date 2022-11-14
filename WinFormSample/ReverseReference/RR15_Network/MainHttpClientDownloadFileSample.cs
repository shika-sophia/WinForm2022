/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainHttpClientDownloadFileSample.cs
 *@class   └ new FormHttpClientDownloadFileSample() : Form
 *@class       └ new HttpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[427] p722 / HttpClientDownloadFileSample
 *@subject Download File 【Uncompleted / 未完成】
 *         Task<byte[]>  httpClient.GetByteArrayAsync(string / Uri)
 *
 *@NOTE【Problem】Apache 404 Not Found
 *      ◆I tried below code as RR[427] Sample Code.
 *        Ofcause "README.txt" file is in Apache24 as default.
 *        But Apache 404 Not Found.
 *        
 *      byte[] dataAry = await client.GetByteArrayAsync(
 *               "http://localhost:80/../" + $"{textBoxSearch.Text}");
 *               
 *      ・I did below too, but as same (Not solved).
 *        http://localhost/
 *        http://localhost/Apache24/
 *        http://localhost/README.txt
 *        http://localhost/../README.txt
 *        http://localhost/Apache24/README.txt
 *        
 *      => Probably Apache configuration ?
 *      
 *      ◆I modified below in 'Apache24/conf/httpd.conf',
 *        but as same. (Not solved)
 *      
 *      ServerName localhost:80
 *      #
 *      # Deny access to the entirety of your server's filesystem. You must
 *      # explicitly permit access to web content directories in other 
 *      # <Directory> blocks below.
 *      #
 *      <Directory />
 *          AllowOverride all
 *          Require all granted
 *      </Directory>
 *      
 *@see ImageHttpClientDownloadFileSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-14
 */
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainHttpClientDownloadFileSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormHttpClientDownloadFileSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormHttpClientDownloadFileSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormHttpClientDownloadFileSample : Form
    {
        private readonly HttpClient client;
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly TextBox textBoxSearch;
        private readonly TextBox textBoxBody;
        private readonly Button buttonSubmit;
        private readonly Button buttonSave;
        private byte[] dataAry;

        public FormHttpClientDownloadFileSample()
        {
            this.Text = "FormHttpClientDownloadFileSample";
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
                ColumnCount = 4,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 90f));

            //---- Label ----
            label = new Label()
            {
                Text = "Download File: ",
                TextAlign = ContentAlignment.MiddleRight,
                Margin = new Padding(10),
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(label, 0, 0);

            //---- TextBox ----
            textBoxSearch = new TextBox()
            {
                Text = "README.txt",
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
            buttonSubmit = new Button()
            {
                Text = "Submit",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonSubmit.Click += new EventHandler(ButtonSubmit_Click);
            table.Controls.Add(buttonSubmit, 2, 0);

            buttonSave = new Button()
            {
                Text = "Save File",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonSave.Click += new EventHandler(ButtonSave_Click);
            table.Controls.Add(buttonSave, 3, 0);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor


        private async void ButtonSubmit_Click(object sender, EventArgs e)
        {
            //---- Validation ----
            if(String.IsNullOrEmpty(textBoxSearch.Text)) { return; }

            bool canInput = ValidateInput(textBoxSearch.Text);
            if(!canInput) { return; }

            //---- Download File ----
            try
            {
                //Stream stream = await client.GetStreamAsync("http://localhost:80");
                byte[] dataAry = await client.GetByteArrayAsync(
                    "http://localhost:80/../" + $"{textBoxSearch.Text}");

                textBoxBody.Text = "The File Download Completed.";
            }
            catch (Exception ex)
            {
                ShowErrorMessage(
                    $"{ex.GetType()}:{Environment.NewLine}" +
                    $"{ex.Message}{Environment.NewLine}");
            }
            finally
            {
                client.CancelPendingRequests();
            }
        }//ButtonSubmit_Click()

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if(dataAry == null) { return; }

            string dir = "../../WinFormSample/ReverseReference/RR15_Network/";
            
            using (FileStream fs = File.OpenWrite(dir + textBoxSearch.Text))
            {

            }
        }//ButtonSave_Click()

        private bool ValidateInput(string input)
        {
            var errorMessageBld = new StringBuilder();

            //---- Less Length ----
            if (input.Length <= 4)
            {
                errorMessageBld.Append(
                    $"<！> The Search word should be described over 5 characters.{Environment.NewLine}");
            }

            //---- File Form ----
            Regex fileFormRegex = new Regex(@"[a-zA-Z0-9/_]+\.{1}[a-zA-Z]+");
            if (!fileFormRegex.IsMatch(input))
            {
                errorMessageBld.Append($"<！> Not File Name.{Environment.NewLine}");
            }

            //---- Anti Script ----
            Regex regexAntiScript = new Regex(@"[<>&;]+");
    
            if (regexAntiScript.IsMatch(input))
            {
                errorMessageBld.Append(
                    $"<！> Invalid Input ! {Environment.NewLine}");
            }

            if (errorMessageBld.Length > 0)
            {
                ShowErrorMessage(errorMessageBld.ToString());
                return false;
            }

            return true; // bool canInput
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
