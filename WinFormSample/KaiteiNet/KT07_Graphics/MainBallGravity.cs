/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainBallGravity.cs
 *@class   └ new FormBallGravity() : Form
 *@class       └ new BallBehavior() : PictureBox
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics / BallGravity
 *         ボールの画像に重力を働かせ自然落下し、バウンドさせる
 *         behaivior [英] 動作, 挙動, 振る舞い
 *         gravity [英]   重力
 *
 *@based   MainMouseDragSample.cs
 *@subject 重力, バウンド  / parent: Form, base: PictureBox
 *         private double velocity = 0d;  //速度
 *         private double accel = 0.5d;   //加速度
 *         
 *         private void timer_Tick(object sender, EventArgs e)
 *         {
 *             //---- Gravity / 重力 ----
 *             velocity += accel;           //速度を徐々に加速
 *             base.Top += (int)velocity;   //Top座標を速度分だけ落下「+=」
 *
 *             //---- Bound / バウンド ----
 *             if(base.Bottom >= parent.ClientSize.Height)  //底に着いたら
 *             {
 *                 velocity *= -0.8;                        //速度を 80%に減速
 *                 base.Top = parent.ClientSize.Height - base.Height;  
 *                                                          //ボールの高さ分だけ底から上げる
 *             }
 *         }//timer_Tick()
 *
 *@NOTE【調整】Padding
 *      control.Padding = new Padding(5);
 *      Padding: コントロール内側の余白
 *      ドラッグ時や、重力移動時に再描画のチラつきでボールが欠けるので
 *      内側周囲に 5pxずつの余白を挿入
 *      底に着いたときも Padding分の余白ができるので注意
 *      
 *@see ImageBallGravity.jpg
 *@see MainMouseDragSample.cs
 *@author shika
 *@date 2022-08-27
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainBallGravity
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormBallGravity()");

            Application.EnableVisualStyles();
            Application.Run(new FormBallGravity());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormBallGravity : Form
    {
        private BallBehavior ball; // : PictureBox, as self defined class〔below〕

        public FormBallGravity()
        {
            this.Text = "FormBallGravity";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.Size = new Size(640, 480);
            this.BackColor = SystemColors.Window;

            ball = new BallBehavior(this);

            this.Controls.AddRange(new Control[]
            {
                ball,
            });
        }//constructor
    }//class

    class BallBehavior : PictureBox
    {
        private readonly Form parent;
        private readonly Timer timer;
        private Point dragPoint;
        private bool dragged;
        private double velocity = 0d;  //速度
        private double accel = 0.5d;   //加速度

        public BallBehavior() { throw new ArgumentNullException(); }

        public BallBehavior(Form parent)
        {
            this.parent = parent;
            this.timer = new Timer()
            {
                Interval = 20,
                Enabled = true,
            };
            timer.Tick += new EventHandler(timer_Tick);

            //base: PictureBox
            base.Image = new Bitmap("../../Image/ballTransparent.png");
            base.SizeMode = PictureBoxSizeMode.AutoSize;
            base.Padding = new Padding(5);
        }//constructor

        private void timer_Tick(object sender, EventArgs e)
        {
            //---- Gravity / 重力 ----
            velocity += accel;           //速度を徐々に加速
            base.Top += (int)velocity;   //Top座標を速度分だけ落下「+=」

            //---- Bound / バウンド ----
            if(base.Bottom >= parent.ClientSize.Height)  //底に着いたら
            {
                velocity *= -0.8;                        //速度を 80%に減速
                base.Top = parent.ClientSize.Height - base.Height;  //ボールの高さ分だけ底から上げる
            }
        }//timer_Tick()

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            timer.Enabled = false;  //Mouse Drag中は Timer OFF
            dragged = true;
            dragPoint = e.Location;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            timer.Enabled = true;   //Mouse Drag終了 Timer ON
            velocity = 0;           //ドラッグ移動後は 速度 0
            dragged = false;
            dragPoint = e.Location;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (dragged)
            {
                base.Left += e.Location.X - dragPoint.X;
                base.Top += e.Location.Y - dragPoint.Y;
            }
        }
    }//class
}
