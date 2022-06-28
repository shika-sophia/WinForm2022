/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT05_Event
 *@class MainRepaintSample.cs
 *@class FormRepaintSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/events/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm05_Event.txt〕
 *           
 *@content KT 5. Event / Paintイベント
 *@subject ◆Paintイベント
 *         再描画イベント repaint: パラメータや画面に変化があったとき、
 *                               再び表示し直すイベントが発生する。
 *                               
 *        【再描画イベントが発生するデフォルト仕様】
 *        ・Formが初めて表示されたとき
 *        ・Formのサイズが拡大されたとき
 *        ・最小化していた状態から、Formが再表示されるとき
 *        ・control.Invalidate() 再描画の命令を出されたとき
 *        
 *        ＊イベント
 *        PaintEventHandler control.Paint
 *         
 *        ＊イベントハンドラ
 *        delegate void PaintEventHamdler(object sender, PaintEventArgs e)
 *         
 *        control.Paint += new PaintEventHandler(form_Pain);
 *         
 *        ＊Control共通メソッド
 *         control.Invalidate() 現在のコントロール表示を無効にし、再描画を促す
 *
 *@see FormRepaintSample.jpg
 *@author shika
 *@date 2022-06-28
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT05_Event
{
    class MainRepaintSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormRepaintSample());
        }//Main()
    }//class

    class FormRepaintSample : Form
    {
        private Timer timer;
        private Label label;
        private int paintCount = 0;

        public FormRepaintSample()
        {
            this.Text = "FormRepaintSample";

            this.timer = new Timer()
            {
                Interval = 1000,
                Enabled = true,
            };

            this.label = new Label()
            {
                Location = new Point(10, 10),
                AutoSize = true,
            };
            this.Controls.Add(label);

            timer.Tick += new EventHandler(form_Tick);
            this.Paint += new PaintEventHandler(form_Paint);
        }//constructor

        private void form_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void form_Paint(object sender, PaintEventArgs e)
        {
            paintCount++;
            label.Text = 
                $"Repaint: {paintCount} times\n" +
                $"Evented: {DateTime.Now.ToLongTimeString()}\n";
        }
    }//class

}
