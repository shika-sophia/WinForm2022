/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainBrushAppendix.cs
 *@class   └ new FormBrushAppendix() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content BrushAppendix
 *@subject abstract Brush =>〔~/WinFormSample/ColorRefernce.txt〕
 *@subject ◆HatchBrush   => 〔MainHatchBrushViewer.cs〕
 *@subject ◆LinearGradientBrush : Brush  -- System.Drawing.Drawing2D.
 *         ・線形グラデーション: 一定割合で、一方向へ色の濃淡を変化
 *         ・コンストラクタで、四角形, 開始色, 終了色, Mode, 
 *           [角度][角度の影響を受けるか]を設定
 *         LinearGradientBrush    new LinearGradientBrush(
 *             PointF, PointF, Color, Color, LinearGradientMode
 *             [, float angle, [bool isAngleScaleable]])
 *         LinearGradientBrush    new LinearGradientBrush(
 *             RectangleF, Color, Color, LinearGradientMode
 *             [, float angle, [bool isAngleScaleable]])
 *         ※ Point, Rectangleも可
 *         
 *         RectangleF  linearGradientBrush.Rectangle { get; }
 *         Color[]     linearGradientBrush.LinearColors
 *         LinearGradientMode   (None Property)
 *           └ enum LinearGradientMode -- System.Drawing.Drawing2D.
 *             {
 *                 Horizontal = 0,      //左から右へのグラデーション
 *                 Vertical = 1,        //上から下へのグラデーション
 *                 ForwardDiagonal = 2, //左上から右下へのグラデーション
 *                 BackwardDiagonal = 3 //右上から左下へのグラデーション
 *             }
 *             
 *@NOTE【】DrawXxxx() と FillXxxx()の順
 *         輪郭をきれいに描画するには、Fill -> Draw の順がいい
 *         (逆だと、輪郭線が消えて塗りつぶしだけが見える)
 *         
 *@see ImageBrushAppendix.jpg
 *@see 
 *@copyTo ~/WinFormSample/GraphicsRefernce.txt
 *@copyTo ~/WinFormSample/ColorRefernce.txt
 *@author shika
 *@date 2022-08-19
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainBrushAppendix
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormBrushAppendix()");

            Application.EnableVisualStyles();
            Application.Run(new FormBrushAppendix());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormBrushAppendix : Form
    {
        public FormBrushAppendix()
        {
            this.Text = "FormBrushAppendix";
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
            var g = e.Graphics;
            Rectangle rect = new Rectangle(20, 20, 200, 200);
            Pen pen1 = new Pen(Color.Blue);
            Pen pen2 = new Pen(Color.Red);
            Brush hatch = new HatchBrush(
                HatchStyle.Cross, Color.Blue, Color.Azure);
            Brush gradient = new LinearGradientBrush(
                rect, Color.DeepPink, Color.White,
                LinearGradientMode.ForwardDiagonal);

            g.FillRectangle(hatch, rect);
            g.DrawRectangle(pen1, rect);

            g.FillEllipse(gradient, rect);
            g.DrawEllipse(pen2, rect);

            pen1.Dispose();
            pen2.Dispose();
            hatch.Dispose();
            gradient.Dispose();
            g.Dispose();
        }//OnPaint
    }//class
}

