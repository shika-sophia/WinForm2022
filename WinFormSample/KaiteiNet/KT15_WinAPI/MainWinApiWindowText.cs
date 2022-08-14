/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT15_WinAPI
 *@class MainWinApiWindowText.cs
 *@class   └ new FormWinApiWindowText()
 *@class       └  new FormDammy(int count)
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/windows-api/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm15_WinAPI.txt〕
 *           
 *@content KT15 WinAPI / SetWindowText()
 *         ・Windows API: C言語で記述されている Windows制御用の関数群
 *         ・C#から、Windows APIを呼出て利用するサンプル
 *         
 *@subject ◆Process : Component -- System.Diagnostics
 *         Process   new Prcess()
 *         Process[]   Process.GetProcesses()
 *
 *@see ImageWinApiWindowText.jpg
 *@see 
 *@author shika
 *@date 2022-08-14
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT15_WinAPI
{
    class MainWinApiWindowText
    {      
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormWinApiWindowText()");

            Application.EnableVisualStyles();
            Application.Run(new FormWinApiWindowText());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormWinApiWindowText : Form
    {
        private MenuStrip menu;
        private Button btnProcess;
        private Button btnRename;
        private int count;
        private Process[] processAry;

        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string lpString);

        public FormWinApiWindowText()
        {
            this.Text = "FormWinApiWindowText";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            //---- MenuStrip ----
            var menuFile = new ToolStripMenuItem("&File");
            var menuNew = new ToolStripMenuItem("&New File");
            var menuClose = new ToolStripMenuItem("&Close");

            menuNew.Click += new EventHandler(menuNew_Click);
            menuClose.Click += new EventHandler(menuClose_Click);

            menuFile.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuNew, new ToolStripSeparator(), menuClose,
            });

            menu = new MenuStrip();
            menu.Items.Add(menuFile);

            //---- Button ----
            btnProcess = new Button()
            {
                Text = "Process Start",
                Location = new Point(20, 40),
                AutoSize = true,
            };
            btnProcess.Click += new EventHandler(btnProcess_Click);

            btnRename = new Button()
            {
                Text = "Rename",
                Location = new Point(20, 100),
                AutoSize = true,
            };
            btnRename.Click += new EventHandler(btnRename_Click);

            this.Controls.AddRange(new Control[]
            {
                menu, btnProcess, btnRename,
            });
        }//constructor

        private void menuNew_Click(object sender, EventArgs e)
        {
            count++;
            new FormDammy(count).Show();
        }//menuNew_Click()

        private void menuClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if(processAry == null)
            {
                processAry = new Process[]
                {
                    Process.Start("cmd"),
                    Process.Start("notepad.exe"),
                    Process.Start(@"C:\Program Files (x86)\sakura\sakura.exe"),
                };
            }
        }//btnProcess_Click()

        private void btnRename_Click(object sender, EventArgs e)
        {           
            DialogResult resultConfirm = MessageBox.Show(
                "All Window Title will be changed to 'HogeHoge'. OK?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if(resultConfirm != DialogResult.Yes) { return; }

            // 起動中の全プロセスを取得
            foreach (Process process in Process.GetProcesses())
            {
                // ウィンドウハンドルを取得
                IntPtr windowHandle = process.MainWindowHandle;

                // ウィンドウが存在すれば
                if (windowHandle != IntPtr.Zero)
                {
                    // ウィンドウタイトルを改竄
                    SetWindowText(windowHandle, "HogeHoge");
                }
            }//foreach

            MessageBox.Show("All Window Title were changed.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }//class

    class FormDammy : Form
    {
        public FormDammy(int count)
        {
            this.Text = $"FormDammy {count}";
        }
    }//class
}
