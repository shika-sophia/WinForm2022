/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR17_WindowsSystem
 *@class MainMutexWaitOneSample.cs
 *@class   └ new FormMutexWaitOneSample() : Form
 *@class       └ new Mutex()  -- -- System.Threading.
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[476] p808 / Mutex / WaitOne()
 *         Mutex new Mutex(bool initiallyOwned, string name)
 *           └ class Mutex : WaitHandle  -- System.Threading
 *           
 *         bool  waitHandle.WaitOne(int milliSeconds, bool exitContext)
 *         //現在の WaitHandle がシグナルを受け取るまで、現在のスレッドをブロックします。
 *             └ [Argument] bool exitContext:
 *                 待機する前にコンテキストの同期ドメインを終了し (同期されたコンテキスト内にいる場合)、
 *                 後で再取得する場合は、true。それ以外の場合は false。
 *                 
 *@subject Form event 〔WF 10〕
 *         EventHandler            form.Load(object sender, EventArgs e)
 *         FormClosedEventHandler  form.FormClosed(object sender, FormClosedEventArgs e)
 *           └ class FormClosedEventArgs 
 *             bool  e.Cancel  //true: キャンセル
 *             
 *@subject ◆sealed class Mutex : WaitHandle  -- System.Threading
 *         ・mutex: mutual exclusion 相互排他  
 *           =>〔MT MultiThread / SingleThreadExecution | MT p84, p478〕
 *           =>〔[java] ReentrantLockクラス〕
 *         
 *         + Mutex  new Mutex() 
 *         + Mutex  new Mutex(bool initiallyOwned) 
 *         + Mutex  new Mutex(bool initiallyOwned, string name) 
 *         + Mutex  new Mutex(bool initiallyOwned, string name, out bool createdNew) 
 *         + Mutex  new Mutex(bool initiallyOwned, string name, out bool createdNew, MutexSecurity mutexSecurity) 
 *             └ [Argument] bool initiallyOwned:
 *                          呼び出し元Thread に Mutex の初期所有権を与える場合は true。それ以外の場合は false。
 *             └ [Argument] sealed class MutexSecurity : NativeObjectSecurity  〔略〕
 *                                -- System.Security.AccessControl
 *         
 *         + static Mutex  Mutex.OpenExisting(string name) 
 *         + static Mutex  Mutex.OpenExisting(string name, MutexRights rights) 
 *         + static bool   Mutex.TryOpenExisting(string name, out Mutex result) 
 *         + static bool   Mutex.TryOpenExisting(string name, MutexRights rights, out Mutex result)
 *         + MutexSecurity mutex.GetAccessControl() 
 *         + void          mutex.ReleaseMutex() 
 *         + void          mutex.SetAccessControl(MutexSecurity mutexSecurity) 
 *             └ [Argument] enum MutexRights : System.Enum
 *                                -- System.Security.AccessControl
 *                          {
 *                             Modify = 1,
 *                             Delete = 65536,
 *                             ReadPermissions = 131072,
 *                             ChangePermissions = 262144,
 *                             TakeOwnership = 524288,
 *                             Synchronize = 1048576,
 *                          }
 *                                
 *@subject ◆abstract class WaitHandle : MarshalByRefObject, IDisposable
 *                          -- System.Threading
 *         + const int  waitHandle.WaitTimeout = 258 
 *         # static readonly IntPtr  WaitHandle.InvalidHandle 
 *         # WaitHandle  WaitHandle()
 *           [×] 'new' is not available, but 'base()' is OK from constructor of inherited class ONLY.
 *         
 *         + IntPtr       waitHandle.Handle { get; set; } 
 *         + SafeWaitHandle  waitHandle.SafeWaitHandle { get; set; } 
 *         
 *         + static bool  WaitHandle.SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn) 
 *         + static bool  WaitHandle.SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn, TimeSpan timeout, bool exitContext) 
 *         + static bool  WaitHandle.SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn, int millisecondsTimeout, bool exitContext) 
 *             └ [Argument] bool exitContext:
 *               待機する前にコンテキストの同期ドメインを終了し (同期されたコンテキスト内にいる場合)、
 *               後で再取得する場合は、true。それ以外の場合は false。
 *               
 *         + bool         waitHandle.WaitOne()   //現在の WaitHandle がシグナルを受け取るまで、現在のスレッドをブロックします。
 *         + bool         waitHandle.WaitOne(int millisecondsTimeout) 
 *         + bool         waitHandle.WaitOne(int millisecondsTimeout, bool exitContext) 
 *         + bool         waitHandle.WaitOne(TimeSpan timeout) 
 *         + bool         waitHandle.WaitOne(TimeSpan timeout, bool exitContext) 
 *         + static int   WaitHandle.WaitAny(WaitHandle[] waitHandles) 
 *         + static int   WaitHandle.WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout) 
 *         + static int   WaitHandle.WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext) 
 *         + static int   WaitHandle.WaitAny(WaitHandle[] waitHandles, TimeSpan timeout) 
 *         + static int   WaitHandle.WaitAny(WaitHandle[] waitHandles, TimeSpan timeout, bool exitContext) 
 *         + static bool  WaitHandle.WaitAll(WaitHandle[] waitHandles) 
 *         + static bool  WaitHandle.WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout) 
 *         + static bool  WaitHandle.WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext) 
 *         + static bool  WaitHandle.WaitAll(WaitHandle[] waitHandles, TimeSpan timeout) 
 *         + static bool  WaitHandle.WaitAll(WaitHandle[] waitHandles, TimeSpan timeout, bool exitContext) 
 *         + void         waitHandle.Close() 
 *         + void         waitHandle.Dispose() 
 *         # void         waitHandle.Dispose(bool explicitDisposing) 
 *         
 *@subject ◆sealed class SafeWaitHandle : SafeHandleZeroOrMinusOneIsInvalid
 *                              -- Microsoft.Win32.SafeHandles
 *         + SafeWaitHandle  new SafeWaitHandle(IntPtr existingHandle, bool ownsHandle) 
 *         # bool  safeWaitHandle.ReleaseHandle() 
 *
 *@see ImageMutexWaitOneSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-19
 */
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR17_WindowsSystem
{
    class MainMutexWaitOneSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormMutexWaitOneSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormMutexWaitOneSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormMutexWaitOneSample : Form
    {
        private readonly Mutex mutex;
        private readonly Label label;

        public FormMutexWaitOneSample()
        {
            this.Text = "FormMutexWaitOneSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormMutexWaitOneSample");
            this.Load += new EventHandler(FormMutexWaitOneSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormMutexWaitOneSample_FormClosed);

            //---- Controls ----
            label = new Label()
            {
                Text = "This will protect from the duplicate Form.",
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = true,
            };

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                label,
            });
        }//constructor

        private void FormMutexWaitOneSample_Load(object sender, EventArgs e)
        {
            if(!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//FormMutexWaitOneSample_Load()

        private void FormMutexWaitOneSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.Close();
        }//FormMutexWaitOneSample_FormClosed()
    }//class
}
