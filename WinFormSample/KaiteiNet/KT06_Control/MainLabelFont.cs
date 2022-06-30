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
 *@subject ◆Fontクラス System.Windows.Forms
 *         Font new Font(string familyname, float size, [FontStyle])
 *             enum FontStyle
 *             {
 *                  Regular = 0,  //default 標準
 *                  Bold = 1,　   //太字
 *                  Italic = 2,   //斜体
 *                  Underline = 4,//下線
 *                  Strikeout = 8,//取消線
 *             }
 *             ※列挙体はビットフィールドの形式
 *             複数指定の場合はビット論理和「|」区切り
 *             FontStyle.Bold | FontStyle.Strikeout
 *          
 *          void font.Dispose()        Fontオブジェクトの破棄
 *          Font control.DefaultFont   既定のフォント
 *          Font control.CaptionFont   タイトルのフォント
 *          Font control.MenuFont      メニューのフォント
 *          
 *         ◆SystemFontsクラス システム既定のフォント
 *                            Dispose()する必要がない。
 *         ◆SystemColorsクラス システム既定の色
 *         
 *@subject ◆Labelクラス System.Windows.Forms
 *         Label new Label()
 *         string label.Text
 *         Point  control.Location
 *         Color  control.ForeColor
 *         Color  control.BackColor
 *         ContentAlignment label.TextAlign    Label領域内での配置位置
 *                                             Sizeを大きくして配置位置を変更するといい
 *            enum ContentAlignment
 *            {
 *                TopLeft = 1,         // 上端左寄せ
 *                TopCenter = 2,       // 上端中央
 *                TopRight = 4,        // 上端右寄せ
 *                MiddleLeft = 16,     // 中段左寄せ
 *                MiddleCenter = 32,   // 中段中央
 *                MiddleRight = 64,    // 中段右寄せ
 *                BottomLeft = 256,    // 下端左寄せ
 *                BottomCenter = 512,  // 下端中央
 *                BottomRight = 1024,  // 下端右寄せ
 *            }
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
        //[STAThread]
        //static void Main()
        public void Main()
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
