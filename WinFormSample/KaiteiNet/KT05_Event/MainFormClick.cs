/** 
 *@title WinFormGUI / WinFormSample / KT05_Event / MainFormClick.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/events/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm05_Event.txt〕
 *@content KT 5. Event
 *         Formをクリックすると、MessageBoxを表示する
 *
 *@class   MainConsoleEvent.cs
 *@subject イベントの基本
 *         
 *@class   FormClickSample.cs : Form         
 *@subject Formクラスのイベント/イベントハンドラ
 *         event EventHandlar Click
 *         delegate void EventHandler(object sender, EventArgs e)
 *         
 *         this.Click += new EventHandler(form_Click)
 *           └ private void form_Click(object sender, EventArgs e)
 *
 *@class   FormButtonClick : Form
 *@subject Buttonクラスのイベントを追加
 *         button.Click += new EventHandlar(button_Click)
 *           └ private void button_Click(object sender, EventArgs e)
 *
 *@class   FormCloseSample : Form
 *@subject ＊終了イベント
 *         event FormClosing
 *         event FormClosed
 *         
 *         ＊イベントハンドラ delegate
 *         delegate void FormClosingEventHandler(
 *            object sender, FormClosingEventArgs e);
 *         delegate void FormClosedEventHandler(
 *            object sender, FormClosedEventArgs e);
 *            
 *@subject GUI「×」ボタン
 *         void form.Close()  //Formの終了,GUI「×」ボタンと同じ
 *         メソッドによっても FormClosingイベントは発生するため、
 *         FormClosingイベントから、イベントハンドラの解除が必要。
 *         (解除をしないと、MessageBoxで終了[OK]時、またMessageBoxが出力される。)
 *         
 *         this.FormClosing -= this.form_Closing;
 *            
 *@see FormClickSample.jpg
 *@see FormButtonClick.jpg
 *@see MainConsoleEvent.cs
 *@author shika
 *@date 2022-06-24
 */
using System;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT05_Event
{
    class MainFormClick
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormClickSample());
        }//Main()
    }//class

    class FormClickSample : Form
    {
        public FormClickSample()
        {
            this.Text = "FormClickSample";
            this.Click += new EventHandler(form_Click);
        }//constructor

        private void form_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            MessageBox.Show($"Form clicked in {dt:yyyy年MM月dd日 HH:mm:ss}.",
                "Clicked",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
        }
    }//class


}
