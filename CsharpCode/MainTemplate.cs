//==== Document Template ====

/** 
 *@title WinFormGUI / WinFormSample / 
 *@class Main.cs
 *@class   └ new Form1() : Form
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm_.txt〕
 *           
 *@content 
 *@subject 
 *
 *@see Image.jpg
 *@see 
 *@author shika
 *@date 
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

/*
//==== Main() Template / Form class ====

    [STAThread]
    static void Main()
    //public void Main()
    {
        Console.WriteLine("new Form1()");

        Application.EnableVisualStyles();
        Application.Run(new Form1());

        Console.WriteLine("Close()");
    }//Main()
}//class

class Form1 : Form
{
    private readonly PictureBox pic;

    public Form1()
    {
        this.Text = "Form1";
        this.Font = new Font("consolas", 12, FontStyle.Regular);
        this.ClientSize = new Size(640, 640);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.AutoSize = true;
        this.BackColor = SystemColors.Window;

        pic = new PictureBox()
        {
            ClientSize = this.ClientSize,
            BorderStyle = BorderStyle.Fixed3D,
            Dock = DockStyle.Fill,
        };


        this.Controls.AddRange(new Control[]
        {
            pic,
        });
    }//constructor
}//class

 */
namespace WinFormGUI.CsharpCode
{
    class MainTemplate
    {
        //static void Main(string[] args)
        public void Main()
        {
            // Csharp2022 / CsharpBeginプロジェクト (未参照)
            // new Utility.FileDocumentDiv.FileDocExecute().ReadWriteExe();
        }//Main()
    }//class
}
