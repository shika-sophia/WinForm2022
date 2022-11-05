/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainTcpClientSample.cs
 *@class   └ new FormTcpClientSample() : Form
 *@class       └ new 
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[421][423] p712- / TcpClientSample
 *         TCP/IP: Transmission Control Protocol / Internet Protcol
 *                 4層の階層で定義した通信プロトコル階層
 *         OSI参照モデル: 7層で定義した通信プロトコル階層
 *
 *@subject ◆class TcpClient : IDisposable -- System.Net.Sockets
 *         + TcpClient  new TcpClient()
 *         + TcpClient  new TcpClient(IPEndPoint localEP)
 *         + TcpClient  new TcpClient(AddressFamily)             =>〔MainDnsSample.cs〕
 *         + TcpClient  new TcpClient(string hostname, int port)
 *         
 *         int   tcpCilent.SendTimeout { get; set; }          既定値 0 ミリ秒
 *         int   tcpCilent.ReceiveTimeout { get; set; }       既定値 0 ミリ秒
 *         int   tcpCilent.SendBufferSize { get; set; }       既定値は 8192 バイト
 *         int   tcpCilent.ReceiveBufferSize { get; set; }    既定値は 8192 バイト
 *         int   tcpCilent.Available { get; }     ネットワークから受信した、読み取り可能なデータのバイト数
 *         bool  tcpCilent.Connected { get; }     リモート ホストに接続されていたかどうか
 *       # bool  tcpCilent.Active  { get; set; }  接続されたかどうか
 *         bool  tcpCilent.NoDelay { get; set; }  送信バッファーまたは受信バッファーが設定されているサイズを超えていない場合に、遅延を無効にする値を取得または設定します。true 場合は、遅延が無効にします。それ以外の場合、falseします。
 *         bool  tcpCilent.ExclusiveAddressUse { get; set; }  
 *                 TcpClientが 1つの Client だけが特定のポートを使用できるか
 *                 (既定値は、Windows Server 2003 および Windows XP Service Pack 2 以降では true, 
 *                 その他のすべてのバージョンではfalse )
 *                 
 *         Socket  tcpCilent.Client { get; set; } 接続に利用している Socketオブジェクト
 *           └ class Socket 〔下記〕
 *         LingerOption  tcpCilent.LingerState { get; set; }  ソケットの待機状態に関する情報。既定では、接続の待機は無効です。
 *           └ class LingerOption -- System.Net.Sockets
 *             LingerOption  new LingerOption(bool enable, int seconds);
 *                引数 enable: Socket.Close メソッドが呼び出された後も接続を維持する場合は true。それ以外の場合は false。
 *                     seconds: 接続を維持する秒数。
 *                + bool  lingerOption.Enabled { get; set; }
 *                + int   lingerOption.LingerTime { get; set; }
 *                
 *         void  tcpCilent.Connect(string hostname, int port)
 *         void  tcpCilent.Connect(IPEndPoint remoteEP)
 *         void  tcpCilent.Connect(IPAddress address, int port)
 *         void  tcpCilent.Connect(IPAddress[] ipAddresses, int port)
 *         NetworkStream  tcpCilent.GetStream();
 *         
 *         ＊Async
 *         Task  tcpCilent.ConnectAsync(string host, int port)
 *         Task  tcpCilent.ConnectAsync(IPAddress address, int port)
 *         Task  tcpCilent.ConnectAsync(IPAddress[] addresses, int port)
 *         IAsyncResult  tcpCilent.BeginConnect(string host, int port, AsyncCallback, object state)
 *         IAsyncResult  tcpCilent.BeginConnect(IPAddress address, int port, AsyncCallback, object state)
 *         IAsyncResult  tcpCilent.BeginConnect(IPAddress[] addresses, int port, AsyncCallback, object state)
 *         void          tcpCilent.EndConnect(IAsyncResult asyncResult)
 *
 *@subject ◆class Socket : IDisposable -- System.Net.Sockets
 *         Socket  new Socket(SocketInformation)
 *         Socket  new Socket(SocketType, ProtocolType)
 *         Socket  new Socket(AddressFamily, SocketType, ProtocolType)
 *           └ Arguments:
 *               ・struct SocketInformation -- System.Net.Sockets
 *                 + byte[] ProtocolInformation { get; set; }
 *                 + SocketInformationOptions Options { get; set; }
 *                 
 *               ・enum SocketType -- System.Net.Sockets
 *                 {   
 *                   Unknown = -1,  //不明
 *                   Stream = 1,    //データの複製および境界の維持を行うことなく、信頼性が高く双方向の、接続ベースのバイト ストリームをサポート
 *                                  //単一のピアと通信し、通信を開始する前にリモート ホスト接続を確立しておく必要があります
 *                   Dgram = 2,     //データグラムをサポートしています。
 *                                  //これはコネクションレスで、固定 (通常は短い) 最大長の、信頼性のないメッセージです。 
 *                                  //メッセージが喪失または複製されたり、正しい順序で受信されなかったりする可能性があります。
 *                                  //データの送受信に先立って接続する必要がなく、複数のピアと通信できます。
 *                   Raw = 3,       //基になるトランスポート プロトコルへのアクセスをサポートします。
 *                                  //ProtocolType.Icmp: インターネット コントロール メッセージ プロトコルや
 *                                  //ProtocolType.Igmp: インターネット グループ管理プロトコル などのプロトコルを使用して通信を行うことができます。
 *                                  //ユーザーのアプリケーションが送信時に完全な IP ヘッダーを提供する必要があります。
 *                                  //受信データグラムは IP ヘッダーとオプションをそのまま返します。
 *                   Rdm = 4,       //コネクションレスでメッセージ指向の、配信の信頼性が高いメッセージをサポートし、データ内のメッセージ境界を維持します。 
 *                                  //Rdm (Reliably Delivered Messages) メッセージは複製されず、順番に到着します。
 *                                  //また、メッセージが失われたときには送信元に通知されます。 
 *                                  //SocketType.Rdmを使用して Socket を初期化した場合には、
 *                                  //データの送受信の前にリモート ホストに接続しておく必要はありません。
 *                                  //Rdm では複数のピアと通信できます。
 *                   Seqpacket = 5  //ネットワーク全体に、順序付きバイト ストリームの、コネクション指向で信頼性の高い双方向転送を提供します。 
 *                                  //SocketType.Seqpacketはデータを複製せず、データ ストリーム内の境界を維持します。
 *                                  Seqpacket 型の Socket は単一のピアと通信し、通信を開始する前にリモート ホスト接続を確立しておく必要があります。
 *                 }
 *                 
 *               ・enum ProtocolType -- System.Net.Sockets
 *                 {
 *                    IPv6HopByHopOptions = 0,
 *                    IP = 0,
 *                    Unspecified = 0,
 *                    Icmp = 1,
 *                    Igmp = 2,
 *                    Ggp = 3,
 *                    IPv4 = 4,
 *                    Tcp = 6,
 *                    Pup = 12,
 *                    Udp = 17,
 *                    Idp = 22,
 *                    IPv6 = 41,
 *                    IPv6RoutingHeader = 43,
 *                    IPv6FragmentHeader = 44,
 *                    IPSecEncapsulatingSecurityPayload = 50,
 *                    IPSecAuthenticationHeader = 51,
 *                    IcmpV6 = 58,
 *                    IPv6NoNextHeader = 59,
 *                    IPv6DestinationOptions = 60,
 *                    ND = 77,
 *                    Raw = 255,
 *                    Ipx = 1000,
 *                    Spx = 1256,
 *                    SpxII = 1257,
 *                    Unknown = -1,
 *                 }
 *              ・enum AddressFamily =>〔MainDnsSample.cs〕
 *              
 *         Socket  socket.Accept()  新しく作成された接続に対して Socket を作成
 *         Socket  socket.EndAccept(IAsyncResult asyncResult);
 *         Socket  socket.EndAccept(out byte[] buffer, IAsyncResult asyncResult);
 *         Socket  socket.EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult);
 *         
 *         ＊static Field
 *         static bool  Socket.OSSupportsIPv4 { get; } OS および ネットワーク アダプターが、IPv4 をサポートするかどうか
 *         static bool  Socket.OSSupportsIPv6 { get; } OS および ネットワーク アダプターが、IPv6 をサポートするかどうか
 *         static bool  Socket.SupportsIPv4 { get; }   [非推奨] 互換性のために残されている特定の System.Net.Dns メンバー向けに、フレームワークが IPv4 をサポートしているかどうか。OSSupportsIPv4 を利用。
 *         static bool  Socket.SupportsIPv6 { get; }   [非推奨] 互換性のために残されている特定の System.Net.Dns メンバー向けに、フレームワークが IPv6 をサポートしているかどうか。OSSupportsIPv6 を利用。
 *         
 *         ＊Property
 *         SocketType     socket.SocketType { get; }
 *         ProtocolType   socket.ProtocolType { get; }
 *         AddressFamily  socket.AddressFamily { get; }
 *         EndPoint       socket.LocalEndPoint { get; }
 *         EndPoint       socket.RemoteEndPoint { get; }
 *         int   socket.ReceiveTimeout { get; set; }        〔上記〕
 *         int   socket.SendTimeout { get; set; }           〔上記〕
 *         int   socket.SendBufferSize { get; set; }        〔上記〕
 *         int   socket.ReceiveBufferSize { get; set; }     〔上記〕
 *         int   socket.Available { get; }                  〔上記〕
 *         bool  socket.Connected { get; }                  〔上記〕
 *         bool  socket.NoDelay { get; set; }               〔上記〕
 *         bool  socket.ExclusiveAddressUse { get; set; }   〔上記〕
 *         LingerOption  socket.LingerState { get; set; }   〔上記〕
 *         bool  socket.IsBound { get; }       特定のローカル ポートにバインドされているか
 *         bool  socket.Blocking { get; set; } ブロッキング モードか
 *         bool  socket.UseOnlyOverlappedIO { get; set; }  ソケットが重複 I/O モードだけを使用する必要があるか  default: false
 *         bool  socket.DontFragment { get; set; }         データグラムの断片化を許可するか default: true
 *         bool  socket.MulticastLoopback { get; set; }    発信マルチキャスト パケットが送信元アプリケーションに配信されるかどうか
 *         bool  socket.EnableBroadcast { get; set; }      ブロードキャスト パケットの送受信を許可するかどうか
 *         bool  socket.DualMode { get; set; }             IPv4 と IPv6 の両方に使用されるデュアル モード ソケットであるかどうか
 *         short socket.Ttl { get; set; }                  Socket によって送信されたインターネット プロトコル (IP) パケットの有効期間 (TTL) の値
 *          
 *         ＊Method
 *         static void  Socket.Select(IList checkRead, IList checkWrite, IList checkError, int microSeconds)
 *         void Bind(EndPoint localEP);
 *         void Connect(string host, int port)
 *         void Connect(EndPoint remoteEP)
 *         void Connect(IPAddress address, int port)
 *         void Connect(IPAddress[] addresses, int port);
 *         void Disconnect(bool reuseSocket);
 *                
 *         SocketInformation DuplicateAndClose(int targetProcessId);
 *                :
 *                
 *         void Shutdown(SocketShutdown how);
 *           └ enum SocketShutdown
 *             {
 *                Receive = 0,  受信の Socket を無効にします。
 *                Send = 1,     送信の Socket を無効にします。
 *                Both = 2  送信と受信の両方の Socket を無効にします。
 *             }
 *             
 *         object GetSocketOption(SocketOptionLevel, SocketOptionName);
 *         byte[] GetSocketOption(SocketOptionLevel, SocketOptionName, int optionLength);
 *         void GetSocketOption(SocketOptionLevel, SocketOptionName, byte[] optionValue);
 *           └ Argument
 *               enum SocketOptionLevel
 *               {
 *                  IP = 0,
 *                  Tcp = 6,
 *                  Udp = 17,
 *                  IPv6 = 41,
 *                  Socket = 65535
 *               }
 *               
 *               enum SocketOptionName
 *               {
 *                  IPOptions = 1,
 *                  Debug = 1,
 *                  NoChecksum = 1,
 *                  NoDelay = 1,
 *                  HeaderIncluded = 2,
 *                  AcceptConnection = 2,
 *                  BsdUrgent = 2,
 *                  Expedited = 2,
 *                  TypeOfService = 3,
 *                  ReuseAddress = 4,
 *                  IpTimeToLive = 4,
 *                  KeepAlive = 8,
 *                  MulticastInterface = 9,
 *                  MulticastTimeToLive = 10,
 *                  MulticastLoopback = 11,
 *                  AddMembership = 12,
 *                  DropMembership = 13,
 *                  DontFragment = 14,
 *                  AddSourceMembership = 15,
 *                  DropSourceMembership = 16,
 *                  DontRoute = 16,
 *                  BlockSource = 17,
 *                  UnblockSource = 18,
 *                  PacketInformation = 19,
 *                  ChecksumCoverage = 20,
 *                  HopLimit = 21,
 *                  IPProtectionLevel = 23,
 *                  IPv6Only = 27,
 *                  Broadcast = 32,
 *                  UseLoopback = 64,
 *                  Linger = 128,
 *                  OutOfBandInline = 256,
 *                  SendBuffer = 4097,
 *                  ReceiveBuffer = 4098,
 *                  SendLowWater = 4099,
 *                  ReceiveLowWater = 4100,
 *                  SendTimeout = 4101,
 *                  ReceiveTimeout = 4102,
 *                  Error = 4103,
 *                  Type = 4104,
 *                  ReuseUnicastPort = 12295,
 *                  UpdateAcceptContext = 28683,
 *                  UpdateConnectContext = 28688,
 *                  MaxConnections = 2147483647,
 *                  DontLinger = -129,
 *                  ExclusiveAddressUse = -5,
 *              }
    }
 *         
 *         ＊Async 
 *         
 *@see ImageTcpClientSample.jpg
 *@see 
 *@author shika
 *@date 
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Sockets;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainTcpClientSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormTcpClientSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormTcpClientSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormTcpClientSample : Form
    {
        private readonly FlowLayoutPanel flow;
        private readonly TextBox textBox;
        private readonly Button button;

        public FormTcpClientSample()
        {
            this.Text = "FormTcpClientSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            //this.ClientSize = new Size(640, 320);
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            flow = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                //ClientSize = this.ClientSize,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            textBox = new TextBox()
            {
                Multiline = true,
                Height = 100,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            flow.Controls.Add(textBox);

            button = new Button()
            {
                Text = "To Connect with TCP/IP",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            flow.Controls.Add(button);

            this.Controls.AddRange(new Control[]
            {
                flow,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            using(var client = new TcpClient())
            {
                try
                {
                    client.Connect("localhost", 80);
                    textBox.Text = $"Connected with TcpClient";
                }
                catch (Exception ex)
                {
                    textBox.Text = ex.Message;
                }
                finally
                {
                    client.Close();
                }
            }//using
        }//Button_Click()
    }//class
}
