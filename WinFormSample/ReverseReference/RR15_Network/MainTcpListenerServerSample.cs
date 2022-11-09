/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainTcpListenerServerSample.cs
 *@class   └ new FormTcpListenerServerSample() : Form
 *@class       └ new TcpListener()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[424] p717 / TcpListener
 *         TcpListener class make a TCP/IP Web Server
 *         
 *@subject ◆class TcpListener -- System.Net.Sockets
 *         + TcpListener  new TcpListener(int port)
 *         + TcpListener  new TcpListener(IPEndPoint localEP)
 *         + TcpListener  new TcpListener(IPAddress localaddr, int port)
 *         + static TcpLitener  TcpListener.Create(int port)
 *         
 *         + Socket    tcpLitener.Server { get; }
 *         + EndPoint  tcpLitener.LocalEndpoint { get; }
 *         + bool      tcpLitener.ExclusiveAddressUse { get; set; }
 *         # bool      tcpLitener.Active { get; }
 *         
 *         + void      tcpLitener.Start()
 *         + void      tcpLitener.Start(int backlog)
 *         + void      tcpLitener.Stop()
 *         + bool      tcpLitener.Pending()
 *         + Socket    tcpLitener.AcceptSocket()
 *         + TcpClient tcpLitener.AcceptTcpClient()
 *         + void      tcpLitener.AllowNatTraversal(bool allowed)
 *         
 *         + Task<Socket>     tcpLitener.AcceptSocketAsync()
 *         + Task<TcpClient>  tcpLitener.AcceptTcpClientAsync()
 *         + IAsyncResult     tcpLitener.BeginAcceptSocket(AsyncCallback callback, object state)
 *         + Socket           tcpLitener.EndAcceptSocket(IAsyncResult asyncResult)
 *         + IAsyncResult     tcpLitener.BeginAcceptTcpClient(AsyncCallback callback, object state)
 *         + TcpClient        tcpLitener.EndAcceptTcpClient(IAsyncResult asyncResult)
 *         
 *@subject 実行 Taskが UI Threadと異なる場合: 下記を true
 *         static bool  Control.CheckForIllegalCrossThreadCalls {get; set:}
 *                  アプリケーションのデバッグ中に、
 *                  Control.Handle プロパティにアクセスする
 *                  間違ったスレッドによる呼び出しをキャッチするかどうか  default: false
 *               
 *@NOTE【Problem】Sample Program
 *      UI制御に try-catchを利用しているので、
 *      Server実行中の予期せぬ例外が発生しても、"Server Finished"と表示される。
 *      Button [Finish Server]時も下記の例外が throwされて、"Server Finished"
 *      
 *      System.Net.Sockets.SocketException:
 *      ブロック操作は WSACancelBlockingCall の呼び出しに 割り込まれました。
 *      
 *@see ImageTcpListenerServerSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-09
 */
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainTcpListenerServerSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormTcpListenerServerSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormTcpListenerServerSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormTcpListenerServerSample : Form
    {
        private readonly TcpListener server;
        private readonly TableLayoutPanel table;
        private readonly Button buttonStart;
        private readonly Button buttonFinish;
        private readonly TextBox textBox;

        public FormTcpListenerServerSample()
        {
            this.Text = "FormTcpListenerServerSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 480);
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- TcpLisener ----
            server = new TcpListener(IPAddress.Loopback, 9000);
            
            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 80f));

            //---- Button ----
            buttonStart = new Button()
            {
                Text = "Start Server",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonStart.Click += new EventHandler(ButtonStart_Click);
            table.Controls.Add(buttonStart, 0, 0);

            buttonFinish = new Button()
            {
                Text = "Finish Server",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonFinish.Click += new EventHandler(ButtonFinish_Click);
            table.Controls.Add(buttonFinish, 1, 0);

            //---- TextBox ----
            textBox = new TextBox()
            {
                ReadOnly = true,
                Multiline = true,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBox, 0, 1);
            table.SetColumnSpan(textBox, 2);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            Task.Run(() => ServerWorkerThread());
        }//ButtonStart_Click()

        private void ButtonFinish_Click(object sender, EventArgs e)
        {
            server.Stop();
        }//ButtonFinish_Click()

        private void ServerWorkerThread()
        {
            server.Start();

            this.Invoke(new Action(() => 
            {
                textBox.Text = "Server started.";
            }));

            try
            {
                while (true)
                {
                    using (TcpClient client = server.AcceptTcpClient())
                    {
                        NetworkStream stream = client.GetStream();

                        //---- Recieve ----
                        byte[] dataAry = new byte[101];
                        int readlength = stream.Read(dataAry, 0, dataAry.Length);
                        string readString = Encoding.ASCII.GetString(dataAry, 0, readlength);

                        this.Invoke(new Action(() =>
                        {
                            textBox.Text = $"Recieved Data:\n{readString}";
                        }));
                       
                        client.Close();
                    }//using
                }//while
            }
            catch (SocketException ex)
            {
                this.Invoke(new Action(() =>
                {
                    textBox.Text = $"Server Finished";
                    Console.WriteLine(
                        $"{ex.GetType()}: {Environment.NewLine}"
                        + $"{ex.Message}");
                }));
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    textBox.Text = $"Server Error: {Environment.NewLine}" +
                        $"  {ex.GetType()}: {Environment.NewLine}" +
                        $"  {ex.Message}";
                }));
            }
        }//ServerWorkerThread()

    }//class
}
