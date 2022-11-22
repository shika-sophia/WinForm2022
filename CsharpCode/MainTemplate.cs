//==== Document Template ====

/** 
 *@title WinFormGUI / WinFormSample / 
 *@class Main.cs
 *@class   └ new Form1() : Form
 *@class       └ new 
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
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
using System.Threading;
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
    private readonly Mutex mutex;

    public Form1()
    {
        this.Text = "Form1";
        this.Font = new Font("consolas", 12, FontStyle.Regular);
        this.ClientSize = new Size(640, 640);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.AutoSize = true;
        this.BackColor = SystemColors.Window;

        //---- Form Event ----
        mutex = new Mutex(initiallyOwned: false, "Form1");
        this.Load += new EventHandler(Form1_Load);
        this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);

        //---- Controls ----

        //---- Deployment ----
        this.Controls.AddRange(new Control[]
        {
            
        });
    }//constructor

    //====== Form Event ======
    private void Form1_Load(object sender, EventArgs e)
    {
        if(!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
        {
            MessageBox.Show("This Form already has been running.");
            this.Close();
        }
    }//Form1_Load()

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        mutex.Close();
    }//Form1_FormClosed()
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
