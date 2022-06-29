/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT06_Control
 *@class MainLabelFont.cs
 *@class FormLabelFont.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/controls/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content KT 6. Control
 *@subject
 *
 *@see FormLabelFont.jpg
 *@author shika
 *@date 2022-06-29
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT06_Control
{
    class MainLabelFont
    {
        [STAThread]
        static void Main()
        //public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormLabelFont());
        }//Main()
    }//class

    class FormLabelFont : Form
    {
        private Font font;
        private Label label;
        
        public FormLabelFont()
        {
            this.Text = "FormLabelFont";
            this.BackColor = SystemColors.ControlDark;

            this.font = new Font("Times New Roman", 24, FontStyle.Regular);            
            this.label = new Label()
            {
                Text = "Hello Font",                
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 10),
                Font = font,
                ForeColor = Color.Blue,
                AutoSize = true,
            };
            this.Controls.Add(label);
        }//constructor
    }//class

}
