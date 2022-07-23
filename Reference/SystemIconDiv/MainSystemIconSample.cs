//■ DOBON.Net
//◆システムのアイコンを取得する
//https://dobon.net/vb/dotnet/system/systemicon.html
//@see Article_SystemIcon.txt
//@see FormSystemIconSample.jpg

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.CsharpCode
{
    class MainSystemIconSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormSystemIconSample());
        }//Main()
    }//class

    class FormSystemIconSample : Form
    {
        private PictureBox pictureBox1;
        public FormSystemIconSample()
        {
            this.Text = "FormSystemIconSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            pictureBox1 = new PictureBox()
            {
                Size = new Size(600, 200),
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            GraphicDrawSystemIcon();  //システムのアイコンを表示する(自己定義メソッド)

            this.Controls.Add(pictureBox1);
        }//constructor

        //システムのアイコンを表示する
        private void GraphicDrawSystemIcon()
        {
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);

            //既定のアプリケーションアイコン(WIN32: IDI_APPLICATION)
            g.DrawIcon(SystemIcons.Application, 0, 0);
            //システムのアスタリスクアイコン(WIN32: IDI_ASTERISK)
            g.DrawIcon(SystemIcons.Asterisk, 40, 0);
            //システムのエラーアイコン(WIN32: IDI_ERROR)
            g.DrawIcon(SystemIcons.Error, 80, 0);
            //システムの感嘆符アイコン(WIN32: IDI_EXCLAMATION)
            g.DrawIcon(SystemIcons.Exclamation, 120, 0);
            //システムの手の形のアイコン(WIN32: IDI_HAND)
            g.DrawIcon(SystemIcons.Hand, 160, 0);
            //システムの情報アイコン(WIN32: IDI_INFORMATION)
            g.DrawIcon(SystemIcons.Information, 200, 0);
            //システムの疑問符アイコン(WIN32: IDI_QUESTION)
            g.DrawIcon(SystemIcons.Question, 240, 0);
            //システムの警告アイコン(WIN32: IDI_WARNING)
            g.DrawIcon(SystemIcons.Warning, 280, 0);
            //Windowsのロゴアイコン(WIN32: IDI_WINLOGO)
            //補足：WinLogoはWindows XPから既定のアプリケーションアイコンと同じになりました。
            g.DrawIcon(SystemIcons.WinLogo, 320, 0);
            //Windowsのシールドアイコン(.NET 3.5-)
            g.DrawIcon(SystemIcons.Shield, 360, 0);

            g.Dispose();

            //PictureBox1に表示する
            pictureBox1.Image = canvas;
        }//GraphicDrawSystemIcon()
    }//class

}
