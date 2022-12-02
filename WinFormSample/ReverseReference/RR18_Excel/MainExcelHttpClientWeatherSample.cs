/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR18_Excel
 *@class MainExcelHttpClientWeatherSample.cs
 *@class   └ new FormExcelHttpClientWeatherSample() : Form
 *@class       └ new HttpClient()
 *@class       └ new Excel.Application()
 *@using Excel = Microsoft.Office.Interop.Excel;
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[497] p837 / Excel, HttpClient / Weather Hacks
 *         ◆Weather Hacks: 天気予報 API (Link Out)
 *         http://weather.livedoor.com/weather_hacks/ (Link Out)
 *           └ http://weather.livedoor.com/weather_hacks/webservice             //仕様 (Link Out)
 *           └ http://weather.livedoor.com/forcast/webservice/json/v1?city=XX   //JSON形式のデータ (Link Out), XX: 都市ID 東京: 130010
 *           
 *@NOTE   【註】上記サイトがリンクアウトのため、サンプルコードのみ記述
 *         Here is described SampleCode ONLY, because above site is linkout.
 *
 *@see (No Image)  [×] ImageExcelHttpClientWeatherSample.jpg
 *@see 
 *@author shika
 *@date 2022-12-02
 */
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelHttpClientWeatherSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelHttpClientWeatherSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelHttpClientWeatherSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelHttpClientWeatherSample : Form
    {
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly Button button;
        private readonly TextBox textBox;

        public FormExcelHttpClientWeatherSample()
        {
            this.Text = "FormExcelHttpClientWeatherSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(480, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelHttpClientWeatherSample");
            this.Load += new EventHandler(FormExcelHttpClientWeatherSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormExcelHttpClientWeatherSample_FormClosed);

            //---- Controls ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            button = new Button()
            {
                Text = "Weather API",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button);

            textBox = new TextBox()
            {
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBox);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private async void Button_Click(object sender, EventArgs e)
        {
            int city = 130010; // cityID: Tokyo
            string url = $"http://weather.livedoor.com/forcast/webservice/json/v1?city={city}";

            HttpClient client = new HttpClient();
            Excel.Application excelApp = new Excel.Application();
            try
            {
                //---- HttpClient ----
                string jsonData = await client.GetStringAsync(url);
                textBox.Text = jsonData;

                //---- JSON ----
                JsonTextReader jsonReader = new JsonTextReader(new StringReader(jsonData));
                JToken root = JObject.ReadFrom(jsonReader);
                string title = root["title"].Value<string>();
                JArray jsonValueAry = (JArray)root["forcasts"];

                //---- get Weather Data ----
                JToken yesterdayData = jsonValueAry[1];
                string date = yesterdayData["date"].Value<string>();
                string dateLabel = yesterdayData["dateLabel"].Value<string>();
                string telop = yesterdayData["telop"].Value<string>();
                string minTemp = yesterdayData["temperature"]["min"]["celsius"].Value<string>();
                string maxTemp = yesterdayData["temperature"]["max"]["celsius"].Value<string>();

                //---- Excel ----
                Excel.Workbook workbook = excelApp.Workbooks.Open(
                    Path.GetFullPath(@"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelWeatherHacksSample.xlsx")); // (Not exist)
                Excel.Worksheet sheet1 = (Excel.Worksheet)workbook;

                //ColumnHeader
                sheet1.Cells[1, 1].Value = "City";
                sheet1.Cells[2, 1].Value = "Date";
                sheet1.Cells[3, 1].Value = "DateLabel";
                sheet1.Cells[4, 1].Value = "Min Temperature";
                sheet1.Cells[5, 1].Value = "Max Temperature";

                //Value
                sheet1.Cells[1, 2].Value = title;
                sheet1.Cells[2, 2].Value = date;
                sheet1.Cells[3, 2].Value = dateLabel;
                sheet1.Cells[4, 2].Value = minTemp;
                sheet1.Cells[5, 2].Value = maxTemp;

                workbook.Save();
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
                Application.Exit();
            }
            finally
            {
                client.CancelPendingRequests();
                excelApp.Quit();
            }
        }//Button_Click()

        //====== Form Event ======
        private void FormExcelHttpClientWeatherSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//Form1_Load()

        private void FormExcelHttpClientWeatherSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.Close();
        }//Form1_FormClosed()
    }//class
}
