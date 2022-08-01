/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT09_CustomDialog
 *@class MainSettingDialogSample.cs
 *@class FormSettingDialogSample
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/dialogs/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm09_CustomDialog.txt〕
 *           
 *@content KT09 CustomDialog / SettingDialog
 *@subject Dialog内の Controlを参照
 *         Dialogの privateフィールド -> publicプロパティ {get; private set;}
 *
 *@see ImageSettingDialogSample.jpg
 *@see 
 *@author shika
 *@date 2022-08-01
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT09_CustomDialog
{
    class MainSettingDialogSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormSettingDialogSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormSettingDialogSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormSettingDialogSample : Form
    {
        public FormSettingDialogSample()
        {
            var dialog = new SettingDialogSample();
            DialogResult result = dialog.ShowDialog();

            if(result == DialogResult.OK)
            {
                //---- Created Form ----
                this.Text = dialog.textBox.Text;
                this.TopMost = dialog.checkAry[0].Checked;
                this.MaximizeBox = dialog.checkAry[1].Checked;
                this.MinimizeBox = dialog.checkAry[2].Checked;
            }
        }//constructor
    }//class

    class SettingDialogSample : Form
    {
        private Label label;
        private Button btnOk;
        public TextBox textBox { get; private set; }
        public CheckBox[] checkAry { get; private set; }
        private readonly string[] itemAry = new string[]
        {
            "Show Top Layer", "Enable MaximizeBox", "Enable MinimizeBox",
        };

        public SettingDialogSample()
        {
            //---- Dialog : Form ----
            this.Text = "FormSettingDialogSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.AcceptButton = btnOk;

            //---- Controls ----
            label = new Label()
            {
                Text = "Form Title:",
                Location = new Point(20, 10),
                AutoSize = true,
            };

            textBox = new TextBox()
            {
                Location = new Point(20, 35),
                Width = 150,
            };

            btnOk = new Button()
            {
                Text = "OK",
                Location = new Point(60, 160),
                DialogResult = DialogResult.OK,
                AutoSize = true,
            };

            //---- CheckBox ----
            checkAry = new CheckBox[itemAry.Length];

            for (int i = 0; i < itemAry.Length; i++)
            {
                checkAry[i] = new CheckBox()
                {
                    Text = itemAry[i],
                    Location = new Point(20, (i * 25 + 70)),
                    AutoSize = true,
                };
            }//for
            checkAry[0].Checked = true;

            //---- Deployment ----
            this.Controls.AddRange(checkAry);
            this.Controls.AddRange(new Control[]
            {
                label, textBox, btnOk, 
            });
        }//constructor
    }//class
}
