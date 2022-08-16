/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainPenDrawLineSample.cs
 *@class   └ new FormPenDrawLineSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics 
 *@subject ◆Graphics : MarshalByRefObject, IDisposable, IDeviceContext
 *             -- System.Drawing
 *         ・描画キャンバスのような存在
 *         ・グラフィックス処理を行うためには、
 *           描画対象の Form, Controlに関連付けた Graphicsオブジェクトを取得する必要がある
 *         ・描画対象に結び付けられた Graphics オブジェクトは，
 *           その描画対象の Paint イベントハンドラの引数 PaintEventArgs e から取得可。
 *         ・PaintEventHandlerは control.Paint += new PaintEventHandler(...); が必要
 *         ・OnPaint()を overrideする場合は 「+=」で追加する必要はないが、
 *           base.OnPaint(e);で基底クラスのイベントハンドラも呼び出す必要がある。
 *         
 *         PaintEventHandler   control.Paint
 *           └ delegate void Paint(object sender, PaintEventArgs e)
 *                ||
 *         protected vertural void control.OnPaint(PaintEventArgs e)   virtualを overrideして利用
 *           
 *         PaintEventArgsクラス -- System.Windows.Forms. 再描画イベントに関するデータを提供
 *         Graphics   e.Graphics
 *         
 *         Graphics   control.CreateGraphics()   // Paint イベントハンドラ以外の場所で Graphics オブジェクトを取得したい場合に利用
 *         void       graphics.Dispose()         // CreateGraphics()で生成した Graphicsオブジェクトは 使い終わる度に 破棄する
 *         
 *         ＊直線の描画
 *         void  graphics.DrawLine(Pen, Point p1, Point p2)             始点 p1 から 終点 p2 の直線を描画
 *         void  graphics.DrawLine(Pen, int x1, int y1, int x2, int y2) 始点 (x1, y1) から 終点 (x2, y2) の直線を描画
 *         
 *         ＊Penクラス  : MarshalByRefObject, ICloneable, IDisposable
 *             -- System.Drawing.
 *         ・Pen クラスを用いて，特定の色と太さを持つ，仮想のペンを定義
 *         ・自己定義した Pen オブジェクトは，使い終る度に Dispose メソッドで破棄する
 *         
 *         Pen   　new Pen(Brush)
 *         Pen     new Pen(Bursh, float width)
 *         Pen   　new Pen(Color)
 *         Pen     new Pen(Color, float width)
 *         
 *         void    pen.Dispose()
 *         
 *         ＊SystemPensクラス -- Syatem.Drawing
 *         Pen     SystemPens.Window       // static ウィンドゥの背景色
 *         Pen     SystemPens.WindowText   // static ウィンドゥの文字色
 *                   :
 *         
 *@see ImagePenDrawLineSample.jpg
 *@see 
 *@copyTo ~/WinFormSample/GraphicsReference.txt
 *@author shika
 *@date 2022-08-16
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainPenDrawLineSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormPenDrawLineSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormPenDrawLineSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormPenDrawLineSample : Form
    {
        public FormPenDrawLineSample()
        {
            this.Text = "FormPenDrawLineSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //this.Controls.AddRange(new Control[]
            //{

            //});
        }//constructor

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Blue, 2);
            g.DrawLine(pen, 20, 20, 200, 200);

            pen.Dispose();
        }//OnPaint()
    }//class
}
