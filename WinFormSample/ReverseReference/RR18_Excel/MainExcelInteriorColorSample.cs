/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR18_Excel
 *@class MainExcelInteriorColorSample.cs
 *@class   └ new FormExcelInteriorColorSample() : Form
 *@class       └ new Excel.Application()
 *@using Excel = Microsoft.Office.Interop.Excel;
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[489] p824 / Excel / range.Interior.Color
 *
 *@summary Cell 範囲を指定して、色を設定
 *         dynamic  workSheet.Range[object from, object to];
 *         Interior range.Interior { get; }
 *           └ interface Interior --  Microsoft.Office.Interop.Excel 〔below〕
 *         dynamic  range.Interior.Color { get; set;}
 *         
 *         [Example]
 *         Excel.Range range = sheet1.Range["A1", "C1"];
 *         range.Interior.Color = Color.Lavender;
 *         
 *@NOTE   【Problem】一度、設定すると、2度目以降の変更は反映されない
 *         Cache, ConnectionPool, Build などで、初回のソースを保存していて、
 *         ２度目以降も、それを適用している可能性がある。
 *         
 *         ・Excel画面で [MenuBar] -> [Data] -> [すべてを更新]            => 解決せず
 *         ・プログラム実行 -> Excelファイルを閉じる -> Excelファイルを開く => 解決せず
 *         ・VS画面で [ビルド]   => 解決せず
 *         ・VS画面で [リビルド] => 解決せず
 *         
 *         ・Excel画面で 対象Excelファイル[.xlsx]を閉じている状態で、
 *           このプログラムを実行 -> Excelファイルを開いて確認              => 解決
 *           (Excel画面で 対象Excelファイル[.xlsx]を開いている状態で
 *           プログラム実行しても２回目以降の更新を反映しない)
 *
 *@NOTE    【Problem】Color情報を double型で取得
 *          {get; set;}であるが、現在の状態を取得しようとしても、
 *          Color情報を double型で取得するため、
 *          「戻り値 double型を 暗黙的に Color型に返還できません」という
 *          RuntimeExceptionを throwする。
 *          キャストをしても同様「戻り値 double型を Color型に返還できません」となり解決せず
 *          
 *          dynamic  range.Interior.Color {get; set;}
 *          Color  Color.FromArgb(int)
 *          int    Color.ToArgb()
 *          
 *          [Example]
 *          [×] Color currentColor = (Color) range.Interior.Color;
 *          
 *          int型にキャストできるが、
 *          Colorオブジェクトにするには RGB値 Red, Green, Blueの 3つの値が必要。
 *          １つの int値の FromArgb(int), ToArgb()を試してみたが、問題あり。〔下記〕
 *          
 *          [Example]
 *          [〇] int colorValue = (int) range.Interior.Color;
 *          Console.WriteLine($"colorValue: {colorValue}");
 *          Result: case of Set Color.Lavender
 *          colorValue: 16443110
 *          
 *          [Example]
 *          int colorValue = (int)range.Interior.Color;
 *           if (colorValue == Color.Lavender.ToArgb())
 *           {
 *               colorValue = SystemColors.Window.ToArgb();
 *           }
 *           else
 *           {
 *               colorValue = Color.Lavender.ToArgb();
 *           }
 *           Console.WriteLine($"Color.Lavender: {Color.Lavender.ToArgb()}");
 *           Console.WriteLine($"colorValue: {colorValue}");
 *           range.Interior.Color = -colorValue;
 *           
 *          ＊Result 
 *          なぜか、マイナス値。このままだと何も変わらない。
 *          代入時に 「-」を付けて正の値ににすると、Excel画面は 当該範囲を Blackで Fillされる。
 *          Color.Lavender: -1644806
 *          colorValue: -1644806
 *          
 *@subject ◆interface Interior -- Microsoft.Office.Interop.Excel
 *         Application  Interior.Application { get; } 
 *         XlCreator    Interior.Creator { get; } 
 *         dynamic      Interior.Parent { get; } 
 *         dynamic      Interior.Color { get; set; } 
 *         dynamic      Interior.ColorIndex { get; set; } 
 *         dynamic      Interior.InvertIfNegative { get; set; } 
 *         dynamic      Interior.Pattern { get; set; } 
 *         dynamic      Interior.PatternColor { get; set; } 
 *         dynamic      Interior.PatternColorIndex { get; set; } 
 *         dynamic      Interior.ThemeColor { get; set; } 
 *         dynamic      Interior.TintAndShade { get; set; } 
 *         dynamic      Interior.PatternThemeColor { get; set; } 
 *         dynamic      Interior.PatternTintAndShade { get; set; } 
 *         dynamic      Interior.Gradient { get; } 
 *
 *@see ImageExcelInteriorColorSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-26
 */
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelInteriorColorSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelInteriorColorSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelInteriorColorSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelInteriorColorSample : Form
    {
        private readonly Mutex mutex;
        private readonly Button button;

        public FormExcelInteriorColorSample()
        {
            this.Text = "FormExcelInteriorColorSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(320, 60);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelInteriorColorSample");
            this.Load += new EventHandler(FormExcelInteriorColorSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormExcelInteriorColorSample_FormClosed);

            //---- Controls ----
            button = new Button()
            {
                Text = "Set Color",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                button,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();

            try
            {
                Excel.Workbook wb = excelApp.Workbooks.Open(
                    Path.GetFullPath(@"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelGridFileSample.xlsx"));
                Excel.Worksheet sheet1 = (Excel.Worksheet)wb.Sheets[1];

                Excel.Range range = sheet1.Range["A1", "C1"];
                range.Interior.Color = Color.Lavender;
                
                MessageBox.Show("Changed Color");
                wb.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                excelApp.Quit();
            }
        }//Button_Click()

        //====== Form Event ======
        private void FormExcelInteriorColorSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//Form1_Load()

        private void FormExcelInteriorColorSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.Close();
        }//Form1_FormClosed()
    }//class
}
