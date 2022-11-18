/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR17_WindowsSystem
 *@class MainProcessStartExitedSample.cs
 *@class   └ new FormProcessStartExitedSample() : Form
 *@class       └ new Process()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[474][478] p806 / Process / Start(), event Exited
 *         Process クラス
 *         ・Process.Start()  他 Applicationを Start
 *         ・環境変数 PATH を検索して、一致したファイルを実行する
 *
 *@subject ◆class Process : Component  -- System.Diagnostics
 *         + Process  new Process() 
 *         
 *         ＊Property
 *         + string  process.ProcessName { get; } 
 *         + string  process.MainWindowTitle { get; } 
 *         + string  process.MachineName { get; } 
 *         + int     process.Id { get; } 
 *         + int     process.SessionId { get; } 
 *         + bool    process.EnableRaisingEvents { get; set; } 
 *         + ProcessStartInfo  process.StartInfo { get; set; } 
 *             └ class ProcessStartInfo 〔below〕
 *         + int  process.BasePriority { get; } 
 *         + ProcessPriorityClass  process.PriorityClass { get; set; } 
 *             └ enum ProcessPriorityClass : System.Enum -- System.Diagnostics
 *               {
 *                  Normal = 32,
 *                  Idle = 64,
 *                  High = 128,
 *                  RealTime = 256,
 *                  BelowNormal = 16384,
 *                  AboveNormal = 32768,
 *               }
 *
 *         + ProcessModule            process.MainModule { get; } 
 *         + ProcessModuleCollection  process.Modules { get; } 
 *         + ProcessThreadCollection  process.Threads { get; } 
 *         
 *         + DateTime  process.StartTime { get; } 
 *         + DateTime  process.ExitTime { get; } 
 *         + TimeSpan  process.UserProcessorTime { get; } 
 *         + TimeSpan  process.TotalProcessorTime { get; } 
 *         + TimeSpan  process.PrivilegedProcessorTime { get; } 
 *         
*          + bool  process.PriorityBoostEnabled { get; set; } 
 *         + bool  process.Responding { get; } 
 *         + int   process.PrivateMemorySize { get; } 
 *         + long  process.PrivateMemorySize64 { get; } 
 *         + int   process.VirtualMemorySize { get; } 
 *         + long  process.VirtualMemorySize64 { get; } 
 *         + int   process.PagedMemorySize { get; } 
 *         + long  process.PagedMemorySize64 { get; } 
 *         + int   process.PeakVirtualMemorySize { get; } 
 *         + long  process.PeakVirtualMemorySize64 { get; } 
 *         + int   process.PeakPagedMemorySize { get; } 
 *         + long  process.PeakPagedMemorySize64 { get; } 
 *         + int   process.PagedSystemMemorySize { get; } 
 *         + long  process.PagedSystemMemorySize64 { get; } 
 *         + int   process.NonpagedSystemMemorySize { get; } 
 *         + long  process.NonpagedSystemMemorySize64 { get; } 
 *         + int   process.WorkingSet { get; } 
 *         + long  process.WorkingSet64 { get; } 
 *         + int   process.PeakWorkingSet { get; } 
 *         + long  process.PeakWorkingSet64 { get; } 
 *         + StreamWriter  process.StandardInput { get; } 
 *         + StreamReader  process.StandardOutput { get; } 
 *         + StreamReader  process.StandardError { get; } 
 *         + IntPtr  process.Handle { get; } 
 *         + int     process.HandleCount { get; } 
 *         + IntPtr  process.MainWindowHandle { get; } 
 *         + IntPtr  process.MaxWorkingSet { get; set; } 
 *         + IntPtr  process.MinWorkingSet { get; set; } 
 *         + IntPtr  process.ProcessorAffinity { get; set; } 
 *         + SafeProcessHandle   process.SafeHandle { get; } 
 *         + ISynchronizeInvoke  process.SynchronizingObject { get; set; } 
 *         + bool    process.HasExited { get; } 
 *         + int     process.ExitCode { get; } 
 *         
 *         ＊static Method
 *         + static Process    Process.GetCurrentProcess() 
 *         + static Process    Process.GetProcessById(int processId) 
 *         + static Process    Process.GetProcessById(int processId, string machineName) 
 *         + static Process[]  Process.GetProcesses() 
 *         + static Process[]  Process.GetProcesses(string machineName) 
 *         + static Process[]  Process.GetProcessesByName(string processName) 
 *         + static Process[]  Process.GetProcessesByName(string processName, string machineName) 
 *         + static void       Process.EnterDebugMode() 
 *         + static void       Process.LeaveDebugMode() 
 *         + static Process    Process.Start(string fileName) 
 *         + static Process    Process.Start(ProcessStartInfo startInfo) 
 *         + static Process    Process.Start(string fileName, string arguments) 
 *         + static Process    Process.Start(string fileName, string userName, SecureString password, string domain) 
 *         + static Process    Process.Start(string fileName, string arguments, string userName, SecureString password, string domain) 
 *         
 *         ＊instance Method
 *         + bool  process.Start() 
 *         + void  process.Kill() 
 *         + void  process.Refresh() 
 *         + void  process.BeginOutputReadLine() 
 *         + void  process.BeginErrorReadLine() 
 *         + void  process.CancelOutputRead() 
 *         + void  process.CancelErrorRead() 
 *         + bool  process.WaitForInputIdle() 
 *         + bool  process.WaitForInputIdle(int milliseconds) 
 *         + void  process.WaitForExit() 
 *         + bool  process.WaitForExit(int milliseconds) 
 *         + void  process.Close() 
 *         + bool  process.CloseMainWindow() 
 *         # void  process.Dispose(bool disposing) 
 *         # void  process.OnExited() 
 *
 *         ＊Event
 *         + event EventHandler              process.Exited 
 *         + event DataReceivedEventHandler  process.OutputDataReceived 
 *         + event DataReceivedEventHandler  process.ErrorDataReceived 
 *         
 *@subject ◆sealed class ProcessStartInfo -- System.Diagnostics
 *         + ProcessStartInfo  new ProcessStartInfo() 
 *         + ProcessStartInfo  new ProcessStartInfo(string fileName) 
 *         + ProcessStartInfo  new ProcessStartInfo(string fileName, string arguments) 
 *         
 *         ＊Property
 *         + string  processStartInfo.FileName { get; set; } 
 *         + string  processStartInfo.UserName { get; set; } 
 *         + string  processStartInfo.PasswordInClearText { get; set; } 
 *         + string  processStartInfo.Arguments { get; set; } 
 *         + string  processStartInfo.Domain { get; set; } 
 *         + string  processStartInfo.WorkingDirectory { get; set; } 
 *         + SecureString  processStartInfo.Password { get; set; } 
 *         + Encoding  processStartInfo.StandardOutputEncoding { get; set; } 
 *         + Encoding  processStartInfo.StandardErrorEncoding { get; set; } 
 *         + bool  processStartInfo.LoadUserProfile { get; set; } 
 *         + bool  processStartInfo.UseShellExecute { get; set; } 
 *         + bool  processStartInfo.RedirectStandardInput { get; set; } 
 *         + bool  processStartInfo.RedirectStandardOutput { get; set; } 
 *         + bool  processStartInfo.RedirectStandardError { get; set; } 
 *         + bool  processStartInfo.ErrorDialog { get; set; } 
 *         + bool  processStartInfo.CreateNoWindow { get; set; } 
 *         + ProcessWindowStyle  processStartInfo.WindowStyle { get; set; } 
 *         + string    processStartInfo.Verb { get; set; } 
 *         + string[]  processStartInfo.Verbs { get; } 
 *         + IDictionary<string, string>  processStartInfo.Environment { get; } 
 *         + StringDictionary  processStartInfo.EnvironmentVariables { get; } 
 *         + IntPtr  processStartInfo.ErrorDialogParentHandle { get; set; } 
 *
 *@see ImageProcessStartExitedSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-18
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR17_WindowsSystem
{
    class MainProcessStartExitedSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormProcessStartExitedSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormProcessStartExitedSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormProcessStartExitedSample : Form
    {
        private readonly Button button;

        public FormProcessStartExitedSample()
        {
            this.Text = "FormProcessStartExitedSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            //this.ClientSize = new Size(640, 640);
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            button = new Button()
            {
                Text = "Open Notepad",
                Dock = DockStyle.Top,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);

            this.Controls.AddRange(new Control[]
            {
                button,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            var process = new Process()
            {
                EnableRaisingEvents = true,
            };

            process.Exited += new EventHandler((sender2, e2) =>
            {
                MessageBox.Show("Closed notepad");
            });

            process.StartInfo.FileName = "notepad.exe";
            process.Start();
        }
    }//class
}
