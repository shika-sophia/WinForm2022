/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT07_Graphics
 *@class MainMouseDragSample.cs
 *@class   └ new FormMouseDragSample() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/graphics/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm07_Graphics.txt〕
 *           
 *@content KT07 Graphics / MouseDragSample
 *         マウス ドラッグによる画像の移動
 *         ・PictureBoxに画像を載せる
 *         ・pictureBox.MouseDown, MouseUp, MouseMoveイベントだけで
 *           マウスドラッグを表現できる
 *
 *@subject ドラッグ移動
 *         ・現地点からドラッグ移動分だけ PictureBoxの開始点を移動させる
 *         ・PictureBoxの幅, 高さは考慮する必要がない
 *           control.Top, Bottom, Left, Right で 四辺の座標を取得/設定できる
 *           (form.AutoSize = true; になっているなら、自動でFormが拡大する)
 *         
 *          if (dragged)
 *          {
 *              pictureBox.Left += e.Location.X - dragPoint.X;
 *              pictureBox.Top += e.Location.Y - dragPoint.Y;
 *          }
 *
 *@see ImageBallGravity.jpg
 *@copyTo ~/WinFormSample/GraphicsReference.txt
 *@author shika
 *@date 2022-08-26
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT07_Graphics
{
    class MainMouseDragSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormMouseDragSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormMouseDragSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMouseDragSample : Form
    {
        private PictureBox pictureBox;
        private Point dragPoint;
        private bool dragged;

        public FormMouseDragSample()
        {
            this.Text = "FormMouseDragSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            pictureBox = new PictureBox()
            {
                Image = new Bitmap("../../Image/ballTransparent.png"),
                SizeMode = PictureBoxSizeMode.AutoSize,
            };
            pictureBox.MouseDown += new MouseEventHandler(pictureBox_MouseDown);
            pictureBox.MouseUp += new MouseEventHandler(pictureBox_MouseUp);
            pictureBox.MouseMove += new MouseEventHandler(pictureBox_MouseMove);
            
            this.Controls.AddRange(new Control[]
            {
                pictureBox,
            });
        }//constructor

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            dragged = true;
            dragPoint = e.Location;
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);
            dragged = false;
            dragPoint = e.Location;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (dragged)
            {
                pictureBox.Left += e.Location.X - dragPoint.X;
                pictureBox.Top += e.Location.Y - dragPoint.Y;
            }
        }
    }//class
}
