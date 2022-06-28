/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT05_Event
 *@class MainEventOverride.cs
 *@class FormEventOverride.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/events/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm05_Event.txt〕
 *           
 *@content KT 5. Event / EventHandler override
 *@subject ◆イベントハンドラの override
 *         ・各コントロールには OnXxxx(XxxxEventArgs) という既製のイベントハンドラーが用意されている。
 *         ・Xxxxには イベント名が入る
 *         ・overrideする場合は control.Xxxx += new XxxxEventHandler();
 *           を行ってイベント追加する必要がない。
 *         ・override時に必ず base.OnXxxx(e); を記述し、基底クラスの同メソッドを起動する
 *
 *         ＊メソッド
 *         void control.OnClick(EventArgs)
 *         void control.OnMouseClick(MouseEventArgs)
 *           : など
 *           
 *@see 
 *@author shika
 *@date 2022-06-28
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT05_Event
{
    class MainEventOverride
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormEventOverride());
        }//Main()
    }//class

    class FormEventOverride : Form
    {
        private Label label;

        public FormEventOverride()
        {
            this.Text = "FormEventOverride";
            this.label = new Label()
            {
                Text = "Click this Form.",
                Location = new Point(10, 10),
                AutoSize = true,
            };
            this.Controls.Add(label);
        }//constructor

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            MessageBox.Show("Clicked", "Clicked");
        }
    }//class

}
