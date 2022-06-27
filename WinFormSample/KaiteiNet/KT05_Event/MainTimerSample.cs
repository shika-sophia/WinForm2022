/** 
 *@title WinFormGUI / WinFormSample / kaieiNet / KT05_Event
 *@class MainTimerSample.cs
 *@class FormTimerSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/events/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm05_Event.txt〕
 *           
 *@content KT 5. Event / Timer
 *@subject ◆Timerクラス System.Windows.Forms.
 *         Timer new Timer()
 *         bool  timer.Enabled   default: true
 *         int   timer.Interval  時間間隔 (ミリ秒)
 *         void  timer.Start()
 *         void  timer.Stop()
 *         EventHandler timer.Tick 
 *               一定時間 Intervalで発生するイベント
 *               最初の表示では発生しない
 *               -> Form.Loadイベントを利用すると解決
 *         
 *         ＊イベントハンドラ 
 *         delegate void EventHandler(object sender, EventArgs e)
 *         
 *         timer.Tick += new EventHandler(form_Tick);
 *         
 *         ＊Form 
 *         EventHandler delegateが同じなので、そのままの形で Form.Loadイベントにも追加
 *         
 *         EventHandler form.Load        読込時
 *         EventHandler form.Shown       最初の表示時
 *         EventHandler form.Activated   アクティブ化
 *         EventHandler form.Deactivated 非アクティブ化
 *
 *         this.Load += new EventHandler(form_Tick);
 *         
 *@see 
 *@author shika
 *@date 2022-06-27
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT05_Event
{
    class MainTimerSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormTimerSample());
        }//Main()
    }//class

    class FormTimerSample : Form
    {
        private Timer timer;
        private Label label;

        public FormTimerSample()
        {
            this.Text = "FormTimerSample";

            timer = new Timer()
            {
                Interval = 1000, //1秒ごとに更新
            };

            label = new Label()
            {
                Text = "DateTime:",
                Location = new Point(10, 10),
                AutoSize = true,
            };
            this.Controls.Add(label);

            this.Load += new EventHandler(form_Tick);
            timer.Tick += new EventHandler(form_Tick);
            timer.Start();
        }//constructor

        private void form_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            label.Text = $"DateTime: {now:yyyy年MM月dd日(ddd) HH:mm:ss}";
        }
    }//class

}
