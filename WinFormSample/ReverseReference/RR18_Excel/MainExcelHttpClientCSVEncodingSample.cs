/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainExcelHttpClientCSVEncodingSample.cs
 *@class   └ new FormExcelHttpClientCSVEncodingSample() : Form
 *@class       └ new HttpClient()
 *@class       └ new Excel.Application()
 *@using Excel = Microsoft.Office.Interop.Excel;
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[498] p839 / HttpClient, Excel / CSV形式, Encoding
 *         外部サイトに HttpClientでアクセスして、そこの CSVデータを取得。
 *         表示するデータを Dataクラスの Propertyとして定義。
 *         取得したデータを Excelで表示
 *         
 *         ◆気象庁/最新データ
 *         https://www.data.jma.go.jp/obd/stats/data/mdrr/docs/csv_dl_readme.html
 *           └ 最高気温: https://www.data.jma.go.jp/obd/stats/data/mdrr/docs/csv_dl_format_mxtem.html
 *               └ ダウンロード https://www.data.jma.go.jp/obd/stats/data/mdrr/tem_rct/alltable/mxtemsadext00_rct.csv
 *           └ 最低気温: https://www.data.jma.go.jp/obd/stats/data/mdrr/docs/csv_dl_format_mntem.html
 *               └ ダウンロード https://www.data.jma.go.jp/obd/stats/data/mdrr/tem_rct/alltable/mntemsadext00_rct.csv
 *         
 *         ・警報,速報:          Atom形式 (XML形式)
 *         ・「最新の気象データ」
 *           CSV形式のファイルとしてダウンロードすることが可能です
 *           CSVファイルの仕様: カンマ区切りCSV形式
 *           文字コード：Shift_JIS
 *           改行コード：CRLF
 *           1行目：    ヘッダ部（各要素の項目名）
 *           2行目以降：データ部（掲載内容については気象要素ごと異なり、それぞれ以下を参照）
 *           1時間降水量, 3時間降水量, 6時間降水量, 12時間降水量, 24時間降水量,
 *           48時間降水量, 72時間降水量, 日降水量, 降水量全要素,
 *           最大風速, 最大瞬間風速, 最高気温, 最低気温, 現在の積雪, 最深積雪,
 *           3時間降雪量, 6時間降雪量, 12時間降雪量, 24時間降雪量, 48時間降雪量, 72時間降雪量, 
 *           降雪量全要素, 累積降雪量, 
 *           データ部に付加される品質情報 → 品質情報
 *         
 *@subject 新規 Excel ファイル
 *         Workbook  excelApp.Workbooks.Add()  新規 Excel Workbookを作成
 *                                             Add()のみだと、実行のたびに、新規ファイルを生成してしまうので、
 *                                             一旦、SaveAs()後、Open()すべき。
 *         string    workbook.Name { get; }    Nameプロパティは getのみなので、ファイル名を設定できない
 *         void      workbook.SaveAs([string FileName], ...)  名前をつけて保存
 *           └ [Argument] string FileName:    ここに 絶対パスのファイル名を代入
 *                        省略可能な引数が大量にあるので、引数名を指定する。引数名は Pascal記法(先頭大文字)。
 *         Workbook  excelApp.Workbooks.Open(string path)
 *          
 *         [×] File.Create(string fileName)    
 *               これだと Excel.Applicationから開けない
 *               おそらく、Excel側に Pathの値を設定できていないからかも
 *               => 上記のように Excel.Application から操作してファイル作成すべき。
 *        
 *        [Example]
 *        string path = Path.GetFullPath(
 *           @"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelWeatherSample.xlsx");
 *           
 *        if (!File.Exists(path))
 *        {
 *           Excel.Workbook newWorkbook = excelApp.Workbooks.Add();
 *           newWorkbook.SaveAs(Filename: path);
 *        }
 *        
 *        Excel.Workbook workbook = excelApp.Workbooks.Open(path);
 *          :
 *          
 *@NOTE 【註】初回の実行完了に、かなり時間が掛かる (１分ぐらい)
 *       ・HttpClientで、気象庁サイトから CSV取得までは、すぐに完了
 *       ・File.Exists(path) ファイルが存在するか探して、
 *       ・存在しない場合は 新規 Excelファイルを作成、それを保存。
 *       ・改めて Excelファイルを開いて、
 *       ・ColumnHeaderを書き込み
 *       ・最高気温の data に 同 IDを検索して、の最低気温の dataを追加
 *       ・北海道～沖縄まで、915行の dataがある
 *       ・各行の dataを new TemperatureData() としてオブジェクト化
 *       ・dataList値を取得して、TemperatureDataの 各Propertyに値を代入
 *       ・TemperatureDataの 各Propertyを取得して、Excelの 各Cellに値を代入
 *       
 *@see ImageExcelCSVEncodingSample.jpg
 *@see 
 *@author shika
 *@date 2022-12-03
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelHttpClientCSVEncodingSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelHttpClientCSVEncodingSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelHttpClientCSVEncodingSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelHttpClientCSVEncodingSample : Form
    {
        private readonly HttpClient client;
        private readonly Excel.Application excelApp;
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly Button button;
        private readonly TextBox textBox;
        private const string urlMaxTemp =
            "https://www.data.jma.go.jp/obd/stats/data/mdrr/tem_rct/alltable/mxtemsadext00_rct.csv";
        private const string urlMinTemp =
            "https://www.data.jma.go.jp/obd/stats/data/mdrr/tem_rct/alltable/mntemsadext00_rct.csv";

        public FormExcelHttpClientCSVEncodingSample()
        {
            this.Text = "FormExcelHttpClientCSVEncodingSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(480, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelHttpClientCSVEncodingSample");
            this.Load += new EventHandler(FormExcelHttpClientCSVEncodingSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormExcelHttpClientCSVEncodingSample_FormClosed);

            //---- HttpClient ----
            client = new HttpClient();

            //---- Excel.Application ----
            excelApp = new Excel.Application();

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
                Text = "Get Temperature to Excel",
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
            try
            {
                //---- HttpClient ----
                Encoding shiftJis = Encoding.GetEncoding("shift_jis");

                string csvMax, csvMin;
                using (Stream streamMax = await client.GetStreamAsync(urlMaxTemp))
                using (Stream streamMin = await client.GetStreamAsync(urlMinTemp))
                using (TextReader readerMax = new StreamReader(streamMax, shiftJis, false))
                using (TextReader readerMin = new StreamReader(streamMin, shiftJis, false))
                {
                    csvMax = await readerMax.ReadToEndAsync();
                    csvMin = await readerMin.ReadToEndAsync();

                    streamMax.Close();
                    streamMin.Close();
                    readerMax.Close();
                    readerMin.Close();
                }//using

                //---- Parse CSV ----
                List<TemperatureData> dataList = new List<TemperatureData>();
                
                //MaxTemperature
                List<string> lineList = csvMax.Split(
                    new string[] { "\r\n" },
                    StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                lineList.RemoveAt(0); // 1行目: ColumnHeader 削除

                foreach(string line in lineList)
                {
                    string[] valueAry = line.Split(
                        new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if(valueAry.Length > 13)
                    {
                        var data = new TemperatureData()
                        {
                            Id = Int32.Parse(valueAry[0]),
                            Place1 = valueAry[1],
                            Place2 = valueAry[2],
                            TempMax = Double.Parse(valueAry[9]),
                            HourMax = Int32.Parse(valueAry[11]),
                            MinuteMax = Int32.Parse(valueAry[12]),
                        };
                        dataList.Add(data);
                    }
                }//foreach

                //MinTemperature を取得して、上記で定義した data に追加
                lineList.Clear();
                lineList = csvMin.Split(
                    new string[] { "\r\n" },
                    StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                lineList.RemoveAt(0); // 1行目: ColumnHeader 削除

                foreach (string line in lineList)
                {
                    string[] valueAry = line.Split(
                        new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (valueAry.Length > 13)
                    {

                        int id = Int32.Parse(valueAry[0]);
                        double tempMin = Double.Parse(valueAry[9]);
                        int hour = Int32.Parse(valueAry[11]);
                        int minute = Int32.Parse(valueAry[12]);

                        var data = dataList.First(d => d.Id == id);

                        if (data != null)
                        {
                            data.TempMin = tempMin;
                            data.HourMin = hour;
                            data.MinuteMin = minute;
                        }
                    }
                }//foreach

                textBox.Text = $"Got Temperature Data.{Environment.NewLine}";

                //---- Output to Excel ----
                string path = Path.GetFullPath(
                    @"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelWeatherSample.xlsx");

                if (!File.Exists(path))
                {
                    Excel.Workbook newWorkbook = excelApp.Workbooks.Add();
                    newWorkbook.SaveAs(Filename: path);
                }

                Excel.Workbook workbook = excelApp.Workbooks.Open(path);
                Excel.Worksheet sheet1 = (Excel.Worksheet) workbook.Sheets[1];

                //ColumnHeader
                if (sheet1.Cells[1, 1].Value != "ID")
                {
                    sheet1.Cells[1, 1].Value = "ID";
                    sheet1.Cells[1, 2].Value = "Province";
                    sheet1.Cells[1, 3].Value = "Region";
                    sheet1.Cells[1, 4].Value = "Max Temperature";
                    sheet1.Cells[1, 5].Value = "Max Time";
                    sheet1.Cells[1, 6].Value = "Min Temperature";
                    sheet1.Cells[1, 7].Value = "Min Time";
                }

                //Data
                int row = 2;
                foreach (var data in dataList)
                {
                    sheet1.Cells[row, 1].Value = data.Id;
                    sheet1.Cells[row, 2].Value = data.Place1;
                    sheet1.Cells[row, 3].Value = data.Place2;
                    sheet1.Cells[row, 4].Value = data.TempMax;
                    sheet1.Cells[row, 5].Value = $"{data.HourMax}:{data.MinuteMax}";
                    sheet1.Cells[row, 6].Value = data.TempMin;
                    sheet1.Cells[row, 7].Value = $"{data.HourMin}:{data.MinuteMin}";

                    row++;
                }//foreach

                textBox.Text += $"Output to Excel.{Environment.NewLine}";
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
        }//Button_Click()

        //====== Form Event ======
        private void FormExcelHttpClientCSVEncodingSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//Form1_Load()

        private void FormExcelHttpClientCSVEncodingSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Dispose();
            excelApp.Quit();
            mutex.Close();
        }//Form1_FormClosed()
    }//class

    class TemperatureData
    {
        public int Id { get; set; }
        public string Place1 { get; set; }
        public string Place2 { get; set; }
        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public int HourMax { get; set; }
        public int MinuteMax { get; set; }
        public int HourMin { get; set; }
        public int MinuteMin { get; set; }
    }//class TemperatureData
}
