/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class MainCodeMaskedTextBoxSample.cs
 *@class FormCodeMaskedTextBoxSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[63] MaskedTextBox / p124
 *@subject ◆MaskedTextBox : TextBoxBase
 *         MaskedTextBox new MaskedTextBox()
 *         MaskedTextBox new MaskedTextBox(string mask)
 *         string mask.Mask           入力マスク(=形式フォーマット)文字列
 *         string mask.Text           入力した文字列
 *         Type   mask.ValidatingType 検証に用いる型
 *         object mask.ValidateText() 入力文字列を検証する型に変換
 *         bool   mask.MaskCompleted  マスク形式か (上記の型かは検証しない)
 *         char   mask.PromptChar     入力欄の文字 デフォルト「_」以外に設定する場合に利用
 *         int    control.TabIndex    デフォルトでカーソルを表示する場合に、このコントロールのインデックスにしておく
 *                                    ２番目のコントロールなら、TabIdex = 1
 *
 *@subject 入力マスクのフォーマット
 *         0: 0-9までの１桁の数字。省略不可。
 *         9: 数字または空白。省略可。
 *         #: 数字または空白。省略可。「+」「-」も可。
 *         L: a-z, A-Z の英字。省略不可。
 *         ?: a-z, A-Z の英字。省略可。
 *         &: 文字。省略不可。
 *         C: 文字。省略可。制御文字は不可。
 *         A: 英数字。省略不可。
 *         a: 英数字。省略可。
 *         <: 下へシフト。これ以後の文字を小文字に変換。
 *         >: 上へシフト。これ以後の文字を大文字に変換。
 *         ¥: エスケープ。これに続く１文字をそのまま表示。
 *
 *@NOTE【考察】ValidatingType
 *      ValidatingTypeで検証する型を設定しても、型検証するメソッドが見当たらない。
 *      このコードでは、bool DateTime.TryParse()で変換可能かを判定しているため、
 *      ValidatingTypeをコメントアウトしても動作する。
 *      DateTime以外の型判定ならどうするのだろう。
 *      
 *@design FormGuiMaskedTextBoxSample.cs
 *@see FormCodeMaskedTextBoxSample.jpg
 *@author shika
 *@date 2022-07-11
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR04_Control
{
    class MainCodeMaskedTextBoxSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormCodeMaskedTextboxSample());
        }//Main()
    }//class

    class FormCodeMaskedTextboxSample : Form
    {
        private Label label1;
        private Label label2;
        private Button button;
        private MaskedTextBox mask;

        public FormCodeMaskedTextboxSample()
        {
            this.Text = "FormCodeMaskedTextboxSample";
            
            label1 = new Label()
            {
                Text = "日付を「西暦<4桁>/月<2桁/日<2桁>」の形式で\n" +
                       "入力してください。",
                Location = new Point(10, 10),
                AutoSize = true,
            };

            mask = new MaskedTextBox()
            {
                Mask = "0000/90/90",
                Location = new Point(10, 50),
                TabIndex = 1,
                ValidatingType = typeof(DateTime),
            };
            
            button = new Button()
            {
                Text = "入力文字列を表示",
                Location = new Point(10, 80),
                AutoSize = true,
            };

            label2 = new Label()
            {
                Location = new Point(10, 120),
                Size = new Size(150, 50),
                BorderStyle = BorderStyle.Fixed3D,
            };

            button.Click += new EventHandler(button_Click);

            this.Controls.AddRange(new Control[]
            {
                label1, mask, button, label2,
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            if (mask.MaskCompleted)
            {
                if(DateTime.TryParse(mask.Text, out DateTime input))
                {
                    label2.Text = mask.Text;
                }
                else
                {
                    label2.Text = "正しい形式で入力してください。";
                }
            }
            else
            {
                label2.Text = "最後まで入力してください。";
            }          
        }
    }//class
}
