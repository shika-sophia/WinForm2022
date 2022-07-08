/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT06_Control
 *@class MainTextBoxSample.cs
 *@class FormTextBoxSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/controls/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm06_Control.txt〕
 *           
 *@content KT 6.Control / TextBox
 *@subject ◆TextBoxクラス System.Windows.Forms
 *         TextBox new TextBox()
 *         bool       textBox.Multiline   // 複数行
 *         bool       textBox.WordWrap    // 折り返し
 *         ScrollBars textBox.ScrollBars  // スクロールバー
 *             enum ScrollBars
 *             {
 *                 None = 0,
 *                 Horizontal = 1,
 *                 Vertical = 2,
 *                 Both = 3,
 *             }
 *         void       textBox.AppendText(string)
 *         void       textBox.Clear()
 *         bool       textBox.AcceptReturn  デフォルトボタン設定時も[Enter]で改行
 *         bool       textBox.AcceptTabs
 *         
 *@author shika
 *@date 2022-07-01
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT06_Control
{
    class MainTextBoxSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormTextBoxSample());
        }//Main()
    }//class

    class FormTextBoxSample : Form
    {
        private TextBox txt;
        
        public FormTextBoxSample()
        {
            this.Text = "FormTextBoxSample";

            txt = new TextBox()
            {
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Both,
                Multiline = true,
                WordWrap = true,
                Font = new Font("ＭＳ ゴシック", 12, FontStyle.Regular),
            };
            this.Controls.Add(txt);

        }//constructor
    }//class
}
