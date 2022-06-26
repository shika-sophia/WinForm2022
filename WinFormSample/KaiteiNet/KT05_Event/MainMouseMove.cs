/** 
 *@title WinFormGUI / WinFormSample / KT05_Event / MainMaouseMove.cs
 *@class FormMouseMove.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/events/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm05_Event.txt〕
 *           
 *@content KT 5. Event / MouseEvent
 *@subject ◆MouseEvent
 *         EventHandler control.Click        マウスクリック
 *         EventHandler control.DoubleClick ダブルクリック
 *         EventHandler control.MouseEnter  マウスポインタが入る
 *         EventHandler control.MouseHover  コントロール上にある
 *         EventHandler control.MouseLeave  コントロールから離れる
 *         MouseEventHandler control.Click       マウスクリック(位置情報)
 *         MouseEventHandler control.DoubleClick ダブルクリック(位置情報)
 *         MouseEventHandler control.MouseUp     マウスボタンを押す
 *         MouseEventHandler control.MouseDown   マウスボタンから離れる
 *         MouseEventHandler control.MouseMove   マウスポインタが動く
 *         
 *@subject ◆MouseEventHandler
 *         delegate void MouseEventHandler(object sender, MouseEventArgs e)
 *         
 *         ＊MouseEventArgs e
 *         MouseButtons e.Button   ボタンの種類〔下記〕
 *         int          e.Clicks   クリック回数
 *         Point        e.Location 位置
 *         int          e.X        X座標
 *         int          e.Y        Y座標
 *         
 *         ＊ボタンの種類
 *         enum MouseButtons
 *         {
 *             None = 0,
 *             Left = 1048576,     //左ボタン
 *             Right = 2097152,    //右ボタン
 *             Middle = 4194304,   //中央ボタン
 *             XButton1 = 8388608, //１番目のＸボタン
 *             XButton2 = 16777216 //２番目のＸボタン
 *         }
 *         
 *@see FormMouseMove.jpg
 *@author shika
 *@date 2022-06-26
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT05_Event
{
    class MainMouseMove
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormMouseMove());
        }//Main()
    }//class

    class FormMouseMove : Form
    {
        private Label label;
        
        public FormMouseMove()
        {
            this.Text = "FormMouseMove";
            this.Size = new Size(600, 400);
            this.label = new Label() { AutoSize = true };
            this.Controls.Add(label);

            this.MouseMove += new MouseEventHandler(form_MouseMove);
        }//constructor

        private void form_MouseMove(object sender, MouseEventArgs e)
        {
            label.Location = e.Location;
            label.Text = $"(X: {e.X},Y: {e.Y})";
        }
    }//class
}
