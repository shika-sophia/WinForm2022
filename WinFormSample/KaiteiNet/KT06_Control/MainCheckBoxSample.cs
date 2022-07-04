/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT06_Control
 *@class MainCheckBoxSample.cs
 *@class FontCheckBoxSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/controls/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm06_Control.txt〕
 *           
 *@content KT 6. Control / CheckBox
 *@subject ◆CheckBoxクラス System.Windows.Forms
 *         CheckBox new CheckBox()
 *         bool         check.Checked        チェック状態か
 *         EventHandler check.CheckedChanged チェック状態が変更されたときに発生するイベント
 *
 *@subject イベントハンドラー
 *         ・２つの CheckBoxのイベントハンドラー・メソッドを作るのではなく、
 *           １つのイベントハンドラーで複合条件分岐する。
 *         ・Checkedのデフォルト値の設定をイベントハンドラーの後ろに記述する
 *           (コンストラクタなどで Checkedを先に記述すると、
 *            チェック状態なのに、チェック時の処理は行われていない状態で描画)
 *            
 *         ・三項条件式:   [条件式] ? [true時の値] : [false時の値] 
 *         
 *         ・複合条件は ビット論理和演算子「|」で区切る
 *          FontStyle fontstyle = 0 | FontStyle.Italic; 
 *          FontStyle fontstyle = FontStyle.Bold | 0;
 *          FontStyle fontstyle = FontStyle.Bold | FontStyle.Italic;
 *          
 *@see FormCheckBoxSample.jpg
 *@author shika
 *@date 2022-07-04
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT06_Control
{
    class MainCheckBoxSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormCheckBoxSample());
        }//Main()
    }//class

    class FormCheckBoxSample : Form
    {
        private Font font;
        private Label label;
        private CheckBox cbBold;
        private CheckBox cbItalic;

        public FormCheckBoxSample()
        {
            this.Text = "FormCheckBoxSample";
            font = new Font("Times New Roman", 20);

            label = new Label()
            {
                Text = "Times New Roman",
                Font = font,
                ForeColor = Color.CornflowerBlue,
                Location = new Point(20, 70),
                AutoSize = true,
            };

            cbBold = new CheckBox()
            {
                Text = "Bold (太字)",
                Location = new Point(20, 15),
            };
            cbBold.CheckedChanged += new EventHandler(checkBox_CheckedChanged);

            cbItalic = new CheckBox()
            {
                Text = "Italic (斜体)",
                Location = new Point(20, 40),
            };
            cbItalic.CheckedChanged += new EventHandler(checkBox_CheckedChanged);

            cbBold.Checked = true;
            cbItalic.Checked = true;

            this.Controls.AddRange(new Control[]
            {
                cbBold, cbItalic, label,
            });
        }//constructor

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            font.Dispose();

            FontStyle fontStyle =
                (cbBold.Checked   ? FontStyle.Bold : 0) |
                (cbItalic.Checked ? FontStyle.Italic : 0);

            font = new Font("Times New Roman", 20, fontStyle);

            label.Font = font;
        }//check_CheckedChanged()
    }//class
}
