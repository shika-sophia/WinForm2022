/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainTrackBarSample.cs
 *@class FormTrackBarSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[99] TrackBar / p187
 *         スライドバーによる数値の入力
 *@subject ◆TrackBar -- System.Windows.Forms
 *         TrackBar    new TrackBar()
 *         Orientation track.Orientation    コントロールの方向
 *           └ enum Orientation { Horizontal, Vertical }
 *         int         track.Value          現在値の取得/設定
 *         int         track.Minimum        最小値
 *         int         track.Maximum        最大値
 *         int         track.TickFrequency  目盛間隔 / デフォルト: 1
 *         TickStyle   track.TickStyle      目盛位置
 *           └ enum TickStyle { None = 0, TopLeft = 1, BottomRight = 2, Both = 3 }
 *         int     SmallChange              ドラッグによる増減幅
 *         int     LargeChange              クリックによる増減幅
 *         
 *         EventHandler track.Scroll        目盛変更時のイベント
 *
 *@see ~/WinFormSample/KaiteiNet/KT06_Control/MainProgressBarSample.cs
 *@see FormTrackBarSample.jpg
 *@author shika
 *@date 2022-07-12
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainTrackBarSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormTrackBarSample());
        }//Main()
    }//class

    class FormTrackBarSample : Form
    {
        private TrackBar track;
        private TextBox textBox;
        private Label label;

        public FormTrackBarSample()
        {
            this.Text = "FormTrackBarSample";
            this.AutoSize = true;

            track = new TrackBar()
            {
                Location = new Point(10, 10),
                Width = 200,
                Orientation = Orientation.Horizontal,
                Value = 10,
                Minimum = 0,
                Maximum = 100,
                TickFrequency = 5,
                TickStyle = TickStyle.TopLeft,
                SmallChange = 1,
                LargeChange = 5,
            };
            track.Scroll += new EventHandler(track_Scroll);

            textBox = new TextBox()
            {
                Text = track.Value.ToString(),
                Location = new Point(220, 10),
                Width = 50,
                TextAlign = HorizontalAlignment.Center,
                ReadOnly = true,
            };

            label = new Label()
            {
                Text = "VC#2019",
                Location = new Point(10, 100),
                Font = new Font("Times New Roman", track.Value),
                AutoSize = true,
            };
            
            this.Controls.AddRange(new Control[]
            {
                track, textBox, label,
            });
        }//constructor

        private void track_Scroll(object sender, EventArgs e)
        {
            textBox.Text = track.Value.ToString();
            label.Font = new Font(label.Font.FontFamily, track.Value);
        }
    }//class
}
