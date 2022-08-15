/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT15_WinAPI
 *@class MainWinApiWindowText.cs
 *@class   └ new FormWinApiWindowText() : Form
 *@class       └  new FormDammy(int count) : form
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
 *         ・現在の全Processのウィンドウのタイトルを全て「HogeHoge」に変えるプログラム
 *
 *@subject Windows API ( C 関数)  〔KT15〕
 *         BOOL SetWindowText(
 *             HWND hWnd,         // ウィンドウへのハンドル
 *             LPCTSTR lpString   // タイトル
 *         );
 *         
 *         この関数を C# で使うためには，
 *         次のような宣言を C# のコード中に書いておく必要があります。
 *         BOOL は bool または int に，
 *         HWND は System.IntPtr に，
 *         LPCSTR は string に対応します。
 *         
 *         [DllImport("user32.dll")]
 *         static extern bool SetWindowText(IntPtr hWnd, string lpString);
 *         
 *         ＊System.Runtime.InteropServices 名前空間
 *         [DllImport(string)] 属性   アンマネージド DLL を利用する際に指定
 *         
 *         ＊extern 修飾子
 *         https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/extern
 *         extern 修飾子は、外部で実装されるメソッドを宣言するために使用。
 *         extern 修飾子は一般に、相互運用サービスを使用してアンマネージ コードを呼び出すときに、
 *         DllImport 属性と共に使用します。
 *         この場合、メソッドを static として宣言する必要もあります。
 *         
 *@subject struct System.IntPtr 構造体    ポインタあるいはハンドルを表現します。
 *         
 *         ウィンドウへのハンドルとは，個々のウィンドウを指し示す識別値のことです。
 *         C をご存知の方には，ウィンドウを指し示す広義のポインタ，という説明もできます。
 *         ポインタやハンドルを C# で扱う際には System.IntPtr を使います。
 *         
 *         一方，ウィンドウへのハンドルを取得するための処理は，
 *         C# のコードだけで実現できます。
 *         Process.GetProcess メソッドを実行すると，
 *         実行中のすべてのプロセスについての Process オブジェクトが得られます。
 *         ウィンドウを持つプロセスであれば，そのメインウィンドウへのハンドルを
 *         MainWindowHandle プロパティより取得することができます。
 *         
 *         MainWindowHandle プロパティより取得したハンドルに対して
 *         SetWindowText を実行することで，
 *         そのハンドルの指し示すウィンドウのタイトルを設定することができます。
 *         これを，ウィンドウを持つすべてのプロセスについて行うことで，
 *         すべてのメインウィンドウのタイトルを改竄することができます。
 *         ただ，今回の作例ではうまくタイトルを変更できないウィンドウも存在します。
 *         
 *@subject ◆Process : Component -- System.Diagnostics
 *           Process   new Prcess()         
 *         ＊Member
 *         int       process.Id          { get; } システムが生成した一意のID番号
 *         string    process.MachineName { get; } プロセスを実行している PCの名前
 *         string    process.ProcessName { get; } プロセス名
 *         int       process.WorkingSet          { get; } プロセスに割り当てられた物理メモリ量 Byte単位
 *         int       process.VirtualMemorySize   { get; } プロセスに割り当てられた仮想メモリ量 Byte単位
 *         long      process.WorkingSet64        { get; } プロセスに割り当てられた物理メモリ量 Byte単位
 *         long      process.VirtualMemorySize64 { get; } プロセスに割り当てられた仮想メモリ量 Byte単位
 *         bool      process.Responding          { get; } プロセスの UI が応答するか
 *         DateTime  process.StartTime           { get; } プロセスの Start日時
 *         DateTime  process.ExitTime            { get; } プロセスの 終了日時
 *         TimeSpan  process.UserProcessorTime   { get; }  ユーザープロセッサー時間: OSコア外部のアプリケーション処理時間
 *         TimeSpan  process.PrivilegedProcessorTime  { get; } 特権プロセッサー時間: OSコア内部のコード処理時間
 *         TimeSpan  process.TotalProcessorTime  { get; }  CPUを使用した合計時間 = UserProcessorTime + PrivilegedProcessorTime
 *         ProcessStartInfo         process.StartInfo { get; set; }  〔ProcessStartInfo | 下記〕
 *         ProcessThreadCollection  process.Threads   { get; }  プロセスに関連した すべてのThreadコレクション
 *         IntPtr    process.ProcessorAffinity { get; set; }　  プロセスの Threadを実行できるプロセッサーの取得/設定  | デフォルト値 2n-1 (n: PCのプロセッサー数)
 *         string    process.MainWindowTitle { get; } メインウィンドウのタイトル
 *         int       process.HandleCount     { get; } プロセスが開いた OS ハンドル数         
 *         IntPtr    process.Handle          { get; } プロセス起動時に OSが割り当てたハンドルを取得。システムは このハンドルを通じてプロセス属性の追跡を続ける
 *         IntPtr    process.MainWindowHandle  { get; } メインウインドウに割り当てられたハンドルを取得
 *           └ IntPtr.Zero  ウィンドウを持たない Processは
 *             process.MainWindowHandle == IntPtr.Zero が成立する
 *         int       process.BasePriority    { get; }  ProcessPriorityClassから算出された基本優先順位
 *         ProcessPriorityClass  process.PriorityClass { get; set; }  優先順位のカテゴリーを取得/設定
 *           └ enum ProcessPriorityClass
 *             {
 *                 Normal = 32,          //特別なスケジューリングを必要としないよう指定
 *                 Idle = 64,            //システムがアイドル状態のときにだけ実行。高い優先順位クラスで実行Threadを優先
 *                 High = 128,           //システムの負荷に関わらず すぐに応答する必要のある、即時実行を要求される重要なタスクに指定
 *                 RealTime = 256,       //優先順位をできるだけ高く指定
 *                 BelowNormal = 16384,  //優先順位を Idle より高く Normal より低く指定
 *                 AboveNormal = 32768   //Normal より高く High より低く指定
 *             }
 *             
 *         ＊Method
 *         Process   Process.GetCurrentProcess();              新しい Processを取得し、呼出元のアプリケーションを実行しているプロセスに関連付ける
 *         Process   Process.GetProcessById(                   ID, machineNameを指定して一意のプロセスを取得
 *                     int processId, [string machineName]);
 *         Process[] Process.GetProcesses()                    実行中の全ての Process配列を取得
 *         Process[] Process.GetProcesses(string machineName); machineNameを指定して、実行中の全ての Process配列を取得
 *         Process[] Process.GetProcessesByName(               processName, machineNameを指定して共有するリモートPCで実行中の全ての Process配列を取得
 *                     string processName, [string machineName])  
 *         void      Process.EnterDebugMode();                    デバッグモードを ONにする
 *         void      Process.LeaveDebugMode();                    デバッグモードを OFFにする
 *         bool      process.Start();                             プロセスを Start / 
 *                                                                true:  新しいプロセスリソースを起動, 
 *                                                                false: 既存のプロセスリソースを利用した際など、新しいプロセスが起動しなかった
 *         Process   Process.Start(
 *                     string fileName, [string arguments]);      アプリケーションファイル名「.exe」「.dll」 / URL ,引数をしてプロセスを Start
 *         Process   Process.Start(ProcessStartInfo startInfo);   ProcessStartInfoを渡してプロセス Start
 *         void      process.Kill();                              プロセスを即時中断
 *         void      process.Refresh();                           キャッシュを破棄
 *         void      process.Close();                             プロセスのすべてのリソースを解放
 *         bool      process.CloseMainWindow();                   UIのメインウィンドゥにCloseメッセージを送信して、プロセスを終了
 *                                                                true: 正常に送信完了
 *                                                                false: ウィンドゥが存在しない, Modal Dialogなどでウィンドゥを利用できない
 *         bool      process.WaitForExit([int milliseconds]);     プロセス終了までの待機時間 ミリ秒 / 0: 即時終了 / -1 or 無指定: 無限待機
 *         bool      process.WaitForInputIdle([int milliseconds]);プロセスがアイドル状態になるまでの待機時間 ミリ秒 / 0: 即時終了 / -1 or 無指定: 無限待機
 *         
 *         ＊Event
 *         bool      process.HasExited { get; }                プロセスが終了しているか
 *         bool      process.EnableRaisingEvents { get; set; } Exitedイベントを発生させるか
 *         void      process.OnExited();                       Exitedイベントを発生させる
 *         EventHandler process.Exited;                        プロセス終了時のイベント
 *         
 *@ubject ◆ProcessStartInfo
 *        ProcessStartInfo    new ProcessStartInfo();
 *        ProcessStartInfo(string fileName);
 *        ProcessStartInfo(string fileName, string arguments);
 *        
 *        string       processStartInfo.FileName { get; set; }            アプリケーションファイル名「.exe」「.dll」
 *        string       processStartInfo.Arguments { get; set; }           アプリケーションに渡す引数
 *        string       processStartInfo.UserName { get; set; }            ユーザー名
 *        string       processStartInfo.Domain { get; set; }              ユーザードメイン名
 *        string       processStartInfo.PasswordInClearText { get; set; } プロセス開始時のパスワードを取得/設定
 *        SecureString processStartInfo.Password { get; set; }            ユーザーパスワードを格納するセキュリティ文字列
 *        IDictionary<string, string>  processStartInfo.Environment { get; }  環境変数の key, valueの Dictionary
 *        StringDictionary EnvironmentVariables { get; }                      環境変数の key, valueの Dictionary
 *        
 *@subject Process.Start()の利用例
 *         Process.Start("cmd");          コマンドプロンプトを起動 「C:\WINDOWS\system32\cmd.exe」
 *         Process.Start("notepad.exe");  メモ帳を起動            「C:\WINDOWS\system32\notepad.exe」
 *         Process.Start(@"C:\Program Files (x86)\sakura\sakura.exe");
 *                    「C:\Windows\System32\」外？は 絶対パスを付加して「.exe」を起動？
 *                    
 *         foreach (Process process in Process.GetProcesses())  すべてのProcessを取得
 *         foreach (Process process in processAry)              Process[] processAryに登録した 各Processを取得
 *                  
 *@NOTE【実行結果】
 *      ・Formから newした FormDammyはタイトルは変更しなかった
 *      ・Visual Studio ウィンドウや、VS内のファイルには影響なし
 *      ・Process[]で foreachした場合は、変更しないものもある
 *      ・Formが終了すると、変更していたタイトルが元に戻るものがある
 *        (これは、おそらく再描画イベントが発生するため、内部的なタイトルまでは変更していない様子)
 *      ・体系的な Windows API は C言語習得後に学習すべし。
 *        (KaiteiNetには、Windows APIのテキストがある)
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
