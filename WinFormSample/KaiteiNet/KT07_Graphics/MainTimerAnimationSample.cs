/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainTimerAnimationSample.cs
 *@class   └ new FormTimerAnimationSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics / TimerAnimationSample
 *@subject アニメーション
 *         ＊描画
 *         ・Timer -> Timer.Tickイベント
 *         ・Rectangel -> FillXxxx(), DrawXxxx()
 *         Timerで 一定時間ごとに再描画
 *         座標処理は Rectangleの始点(左上の座標)で行う
 *         図形幅も考慮して座標を判定すべし
 *         
 *         ＊前図形の削除と新図形の再描画
 *         void   control.Invalidate();
 *
 *         ＊ダブル・バッファー
 *         bool   control.DoubleBuffered   
 *           ・画面のチラつきを抑制するためにダブルバッファーを使用するか
 *           ・頻繁に再描画する場合に利用する
 *           ・この画面のチラつきは，コンピュータが描画をしている様子が，
 *             そのまま見えてしまっていることが原因です。
 *             軽減するためには，一旦メモリ上のキャンバスで描画を完成させ，
 *             完成した描画を画面に表示するようにします。
 *             
 *@NOTE【考察】Formの Size, ClientSize
 *      Formのサイズは、X座標, Y座標に 見えていない余分の幅が存在し、
 *      Size = 見えている幅 + 見えていない余分の幅 
 *      ClientSize = 見えている幅
 *      
 *      おそらくTitleBar, ScrollBar, StatusBarの幅が Formのサイズになっている。
 *      可視範囲の Formの枠で座標処理する場合は ClientSizeで処理すべし
 *@see ImageTimerAnimationSample.jpg
 *@copyTo ~/WinFormSample/GraphicsReference.txt
 *@author shika
 *@date 2022-08-26
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainTimerAnimationSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormTimerAnimationSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormTimerAnimationSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormTimerAnimationSample : Form
    {
        private Timer timer;
        private const int formWidth = 400;  
        private const int formHeight = 300;
        private const int pointWidth = 20;
        private const int pointHeight = 20;
        private int accelX = 5;  // X方向の加速度
        private int accelY = 5;  // Y方向の加速度
        private int x;           // 現在の X座標
        private int y;           // 現在の Y座標

        public FormTimerAnimationSample()
        {
            this.Text = "FormTimerAnimationSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(formWidth, formHeight);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = SystemColors.Window;
            this.DoubleBuffered = true;

            timer = new Timer()
            {
                Interval = 20,
                Enabled = true,
            };
            timer.Tick += new EventHandler(timer_Tick);

            //this.Controls.AddRange(new Control[]
            //{

            //});
        }//constructor

        private void timer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            Pen pen = new Pen(Color.Orange);
            Brush brush = new SolidBrush(Color.Orange);

            DecideAccel();  //self defined method: change move-direction and coordinate
            Rectangle rect = new Rectangle(
                x, y, pointWidth, pointHeight);
            g.FillEllipse(brush, rect);
            g.DrawEllipse(pen, rect);

            pen.Dispose();
            brush.Dispose();
        }//OnPaint()

        //self defined method: change move-direction and coordinate
        //自己定義メソッド: 移動方向と座標処理
        private void DecideAccel()
        {
            if(x < 0 || x > (formWidth - pointWidth))
            {
                accelX *= -1;
            }

            if(y < 0 || y > (formHeight - pointHeight))
            {
                accelY *= -1;
            }

            x += accelX;
            y += accelY;
        }//DecideAccel()
    }//class
}
