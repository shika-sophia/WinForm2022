/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT06_Control
 *@class MainProgressBarSample.cs
 *@class FormProgressBarSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/controls/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm06_Control.txt〕
 *           
 *@content KT 6. Control / ProgressBar, NumericUpDown | RR[89] p169, RR[98] p186
 *@subject ◆ProgressBar -- System.Windows.Forms 〔RR[89] p169〕
 *         ・進捗状況を表示するBar
 *         ProgressBar new  ProgressBar()
 *         int              progressBar.Value   現在値
 *         int              progressBar.Maximun 最大値 デフォルト: 100
 *         int              progressBar.Minimum 最小値 デフォルト:   0
 *         void             progressBar.Refresh() 再描画を促す
 *         
 *         ProgressBarStyle progressBar.Style
 *           └ enum ProgressBarStyle
 *             {
 *                 Blocks = 0,      // ブロック
 *                 Continuous = 1,  // 連続
 *                 Marquee = 2,     // マーキー
 *             }
 *         
 *@subject ◆NumericUpDown -- System.Windows.Forms 〔RR[98] p186〕
 *         ・数字の大小をスピンボタン(▲▼ボタン)で変更するか、直接入力できるTextBox
 *         ・直接入力時は [Enter]により、ValueChangedイベントを起動
 *         
 *         NumericUpDown new NumericUpDown()
 *         decimal      numeric.Value        現在値の取得/設定
 *         decimal      numeric.Maximun      最大値
 *         decimal      numeric.Minimum      最小値
 *         decimal      numeric.Increment    増減幅
 *         bool         numeric.ReadOnly     読取専用か (true: 書き込み不可。ボタン増減可)
 *         bool         numeric.ThousandSeparator  ３桁ごとに「,」で区切るか
 *         
 *         EventHandler numeric.ValueChanged 増減時イベント
 *         
 *@see FormProgressBarSample_withNumericUpDown.jpg
 *@see ~/WinFormSample/ReverseReference/R04_Control/MainTrackBarSample.cs
 *@author shika
 *@date 2022-07-05
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT06_Control
{
    class MainProgressBarSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormProgressBarSample());
        }//Main()
    }//class

    class FormProgressBarSample : Form
    {
        private ProgressBar bar;
        private NumericUpDown numeric;

        public FormProgressBarSample()
        {
            this.Text = "FormProgressBarSample";

            bar = new ProgressBar()
            {
                Location = new Point(20, 60),
                Width = 200,
                Style = ProgressBarStyle.Continuous,
            };

            numeric = new NumericUpDown()
            {
                Location = new Point(20, 20),
                Width = 50,
                TextAlign = HorizontalAlignment.Center,
                Minimum = 0,
                Maximum = 100,
            };
            numeric.ValueChanged +=
                new EventHandler(numeric_ValueChanged);

            this.Controls.AddRange(new Control[]
            {
                numeric, bar
            });
        }//constructor

        private void numeric_ValueChanged(object sender, EventArgs e)
        {
            bar.Value = (int)numeric.Value;
        }
    }//class
}
