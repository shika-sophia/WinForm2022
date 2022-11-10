/** 
 *@title WinFormGUI / WinFormSample / 
 *@class MainTcpClientSample.cs
 *@class   └ new FormTcpClientSample() : Form
 *@class       └ new TcpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[421] p712- / TcpClient
 *         TCP/IP: Transmission Control Protocol / Internet Protcol
 *                 4層の階層で定義した通信プロトコル階層
 *         OSI参照モデル: 7層で定義した通信プロトコル階層
 *         
 *        【実行】実行前に Web Serverを起動しないと TCP/IP の Connectに失敗する
 *         ・IIS 
 *         ・Appache など
 *         
 *         port 80: Web Serverで利用しているポート番号
 */
#region -> TcpClient, Socket
/*
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
 *@subject ◆abstract class EndPoint -- System.Net
 *         # EndPoint  EndPoint()  [×] 'new' is not avaliable, except to inherit class.
 *         + EndPoint       endPoint.Create(SocketAddress socketAddress)
 *         + AddressFamily  endPoint.AddressFamily { get; }
 *         + SocketAddress  endPoint.Serialize()
 *             └ class SocketAddress 〔below〕
 *          ↑
 *@subject ◆class IPEndPoint : EndPoint -- System.Net
 *         IPEndPoint  new IPEndPoint(IPAddress address, int port)
 *         IPEndPoint  new IPEndPoint(long address, int port)
 *         
 *         + const int MinPort = 0
 *         + const int MaxPort = 65535
 *         
 *         + AddressFamily  ipEndPoint.AddressFamily { get; }
 *         + IPAddress      ipEndPoint.Address { get; set; }
 *         + int            ipEndPoint.Port { get; set; }
 *         + EndPoint       ipEndPoint.Create(SocketAddress socketAddress)
 *         + SocketAddress  ipEndPoint.Serialize()
 *             └ class SocketAddress 〔below〕
 *             
 *@subject ◆class SocketAddress -- System.Net
 *         SocketAddress  new SocketAddress(AddressFamily family)
 *         SocketAddress  new SocketAddress(AddressFamily family, int size)
 *         + byte this[int offset] { get; set; }
 *         + int            socketAddress.Size { get; }
 *         + AddressFamily  socketAddress.Family { get; }
 *
 *@subject ◆class Socket : IDisposable -- System.Net.Sockets
 *         Socket  new Socket(SocketInformation)
 *         Socket  new Socket(SocketType, ProtocolType)
 *         Socket  new Socket(AddressFamily, SocketType, ProtocolType)
 *           └ Arguments:
 *               ・struct SocketInformation -- System.Net.Sockets
 *                 + byte[] ProtocolInformation { get; set; }
 *                 + SocketInformationOptions Options { get; set; }
 *                     └ struct SocketInformationOptions 〔below〕
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
 *         int   socket.ReceiveBufferSize { get; set; }     〔上記〕
 *         int   socket.SendBufferSize { get; set; }        〔上記〕
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
 *         IntPtr socket.Handle { get; }                   OS Handleを取得
 *          
 *         ＊Method
 *         static void  Socket.Select(IList checkRead, IList checkWrite, IList checkError, int microSeconds)
 *         void Bind(EndPoint localEP)          ローカル EndPointと関連付ける
 *         void Connect(string host, int port)
 *         void Connect(EndPoint remoteEP)
 *         void Connect(IPAddress address, int port)
 *         void Connect(IPAddress[] addresses, int port);
 *         void Disconnect(bool reuseSocket);
 *         void Listen(int backlog)  
 *            Socket を新しい接続を待機する Listen 状態にします
 *            Argument int backlog: 保留中の接続のキューの最大長
 *            
 *         bool Poll(int microSeconds, SelectMode mode); 
 *            Socket の状態を確認します。
 *            └ enum SelectMode  -- System.Net.Sockets
 *              {
 *                 SelectRead = 0,  読み取りステータス(=状態) モード。
 *                 SelectWrite = 1, 書き込みステータス モード。
 *                 SelectError = 2  エラー ステータス モード。
 *              }
 *              
 *            戻り値  SelectModeに基づいた Socketの状態
 *            true: 
 *            ・socket.Listen()  接続が保留中 or データ読取可 or 接続が閉じている、リセットされている、終了している場合
 *            ・socket.Connect() 接続に成功した or データを送信可 
 *            ・SelectMode.SelectError ブロックしない socket.Connect() を処理し、接続に失敗した場合 or
 *            ・SocketOptionName.OutOfBandInline が設定されておらず、帯域外データを使用できる場合
 *                
 *         int Receive(byte[] buffer, SocketFlags)  SocketFlags を使用し、バインドされた Socket からデータを受信して受信バッファーに格納。
 *         int Receive(byte[] buffer, int size, SocketFlags)
 *         int Receive(byte[] buffer, int offset, int size, SocketFlags)
 *         int Receive(byte[] buffer, int offset, int size, SocketFlags, out SocketError)
 *         int Receive(IList<ArraySegment<byte>> buffers)
 *         int Receive(IList<ArraySegment<byte>> buffers, SocketFlags);
 *         int Receive(IList<ArraySegment<byte>> buffers, SocketFlags, out SocketError)
 *         int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP)  データグラムを受信してデータバッファーに格納します。さらに、エンドポイントを格納します。
 *         int ReceiveFrom(byte[] buffer, SocketFlags, ref EndPoint remoteEP)
 *         int ReceiveFrom(byte[] buffer, int size, SocketFlags, ref EndPoint remoteEP)
 *         int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags, ref EndPoint remoteEP)  
 *         int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags, ref EndPoint remoteEP, out IPPacketInformation)
 *               戻り値: 受信されたバイト数
 *               int size: 受信するバイト数
 *               int offset: 受信データを格納する buffer内の開始点
 *               enum SocketFlags
 *               {
 *                  None = 0,
 *                  OutOfBand = 1,
 *                  Peek = 2,
 *                  DontRoute = 4,
 *                  MaxIOVectorLength = 16,
 *                  Truncated = 256,
 *                  ControlDataTruncated = 512,
 *                  Broadcast = 1024,
 *                  Multicast = 2048,
 *                  Partial = 32768,
 *               }
 *         
 *               struct IPPacketInformation -- System.Net.Sockets
 *               {  
 *                  IPAddress Address { get; }
 *                  int Interface { get; }
 *               }
 *               
 *          int Send(byte[] buffer)       接続された Socketにデータを送信
 *          int Send(byte[] buffer, SocketFlags)
 *          int Send(byte[] buffer, int size, SocketFlags)
 *          int Send(byte[] buffer, int offset, int size, SocketFlags)
 *          int Send(byte[] buffer, int offset, int size, SocketFlags, out SocketError)
 *          int Send(IList<ArraySegment<byte>> buffers)
 *          int Send(IList<ArraySegment<byte>> buffers, SocketFlags)
 *          int SendTo(byte[] buffer, EndPoint remoteEP)         指定したエンドポイントに送信
 *          int SendTo(byte[] buffer, SocketFlags, EndPoint remoteEP)
 *          int SendTo(byte[] buffer, int size, SocketFlags, EndPoint remoteEP)
 *          int SendTo(byte[] buffer, int offset, int size, SocketFlags, EndPoint remoteEP)  
 *          void SendFile(string fileName)  接続された Socketに ファイルを送信
 *          void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags)
 *             TransmitFileOptions 値を使用して、接続された Socketオブジェクトにファイル fileName およびデータのバッファーを送信。
 *                └ enum TransmitFileOptions
 *                  {
 *                     UseDefaultWorkerThread = 0,
 *                     Disconnect = 1,
 *                     ReuseSocket = 2,
 *                     WriteBehind = 4,
 *                     UseSystemThread = 16,
 *                     UseKernelApc = 32,
 *                  }
 *
 *          SocketInformation DuplicateAndClose(int targetProcessId);
 *            └ struct SocketInformation -- System.Net.Sockets
 *                 Socket を複製するために必要な情報をカプセル化
 *              {
 *                 byte[] ProtocolInformation { get; set; }
 *                 SocketInformationOptions Options { get; set; }
 *              }
 *              
 *            └ enum SocketInformationOptions    Socket の状態
 *                     -- System.Net.Sockets
 *                   {
 *                      NonBlocking = 1,  非ブロッキング状態
 *                      Connected = 2, 接続状態
 *                      Listening = 4,  新しい接続を待機
 *                      UseOnlyOverlappedIO = 8  重複 I/O を使用
 *                   }
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
 *         void SetSocketOption(SocketOptionLevel, SocketOptionName, bool optionValue);
 *         void SetSocketOption(SocketOptionLevel, SocketOptionName, object optionValue);
 *         void SetSocketOption(SocketOptionLevel, SocketOptionName, byte[] optionValue);
 *         void SetIPProtectionLevel(IPProtectionLevel level)
 *           └ Argument
 *           ・enum IPProtectionLevel
 *             {
 *                Unspecified = -1,  IP 保護レベルは未指定 (Windows 7, Windows Server 2008 R2)
 *                Unrestricted = 10,  IP 保護レベルは無制限
 *                EdgeRestricted = 20,  IP 保護レベルはエッジ制限付き
 *                Restricted = 30  IP 保護レベルは制限付き
 *             }
 *             
 *             enum SocketOptionLevel
 *             {
 *                IP = 0,
 *                Tcp = 6,
 *                Udp = 17,
 *                IPv6 = 41,
 *                Socket = 65535
 *             }
 *               
 *             enum SocketOptionName
 *             {
 *                IPOptions = 1,
 *                Debug = 1,
 *                NoChecksum = 1,
 *                NoDelay = 1,
 *                HeaderIncluded = 2,
 *                AcceptConnection = 2,
 *                BsdUrgent = 2,
 *                Expedited = 2,
 *                TypeOfService = 3,
 *                ReuseAddress = 4,
 *                IpTimeToLive = 4,
 *                KeepAlive = 8,
 *                MulticastInterface = 9,
 *                MulticastTimeToLive = 10,
 *                MulticastLoopback = 11,
 *                AddMembership = 12,
 *                DropMembership = 13,
 *                DontFragment = 14,
 *                AddSourceMembership = 15,
 *                DropSourceMembership = 16,
 *                DontRoute = 16,
 *                BlockSource = 17,
 *                UnblockSource = 18,
 *                PacketInformation = 19,
 *                ChecksumCoverage = 20,
 *                HopLimit = 21,
 *                IPProtectionLevel = 23,
 *                IPv6Only = 27,
 *                Broadcast = 32,
 *                UseLoopback = 64,
 *                Linger = 128,
 *                OutOfBandInline = 256,
 *                SendBuffer = 4097,
 *                ReceiveBuffer = 4098,
 *                SendLowWater = 4099,
 *                ReceiveLowWater = 4100,
 *                SendTimeout = 4101,
 *                ReceiveTimeout = 4102,
 *                Error = 4103,
 *                Type = 4104,
 *                ReuseUnicastPort = 12295,
 *                UpdateAcceptContext = 28683,
 *                UpdateConnectContext = 28688,
 *                MaxConnections = 2147483647,
 *                DontLinger = -129,
 *                ExclusiveAddressUse = -5,
 *            }
 *         
 *         int IOControl(IOControlCode, byte[] optionInValue, byte[] optionOutValue)      制御コードを指定し、Socketの下位操作モードを設定
 *         int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)  数値制御コードを指定し、Socketの下位操作モードを設定
 *               └ enum IOControlCode : long  { 略 } -- System.Net.Sockets
 *         
 *         void Close(int timeout)
 *         void Close()
 *         
 *         ＊Async Method / 非同期メソッド
 *         static bool ConnectAsync(SocketType, ProtocolType, SocketAsyncEventArgs e)     リモート ホストに接続する非同期要求を開始
 *         static void CancelConnectAsync(SocketAsyncEventArgs e)
 *         bool AcceptAsync(SocketAsyncEventArgs e)
 *         bool ConnectAsync(SocketAsyncEventArgs e)
 *         bool DisconnectAsync(SocketAsyncEventArgs e)
 *         bool ReceiveAsync(SocketAsyncEventArgs e)
 *         bool ReceiveFromAsync(SocketAsyncEventArgs e)
 *         bool ReceiveMessageFromAsync(SocketAsyncEventArgs e)
 *         bool SendAsync(SocketAsyncEventArgs e)
 *         bool SendToAsync(SocketAsyncEventArgs e)
 *         bool SendPacketsAsync(SocketAsyncEventArgs e)
 *           └ 戻り値 bool:
 *               I/O 操作が保留中の場合は true。操作の完了時に、SocketAsyncEventArgs.Completedイベントが発生します。
 *               I/O 操作が同期的に完了した場合は false。 この場合、SocketAsyncEventArgs.Completedイベントは発生しません。
 *               メソッド呼び出しから制御が戻った直後に、パラメーターとして渡された e オブジェクトを調べて操作の結果を取得できます。
 *           └ 引数 class SocketAsyncEventArgs〔below〕
 *           
 *         IAsyncResult BeginAccept(AsyncCallback callback, object state)
 *         IAsyncResult BeginAccept([int receiveSize], AsyncCallback callback, object state)
 *         IAsyncResult BeginAccept([Socket acceptSocket], [int receiveSize], AsyncCallback callback, object state)
 *         
 *         IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
 *         IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
 *         IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
 *         IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state)
 *         IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state)
 *         
 *         IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
 *         IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
 *         IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
 *         IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
 *         IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state)
 *         IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state)
 *         
 *         IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
 *         IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
 *         IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
 *         IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
 *         IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP, AsyncCallback callback, object state)
 *         IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state)
 *         IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, AsyncCallback callback, object state)
 *           
 *         Socket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult)
 *         Socket EndAccept(out byte[] buffer, IAsyncResult asyncResult)
 *         Socket EndAccept(IAsyncResult asyncResult)
 *         void EndConnect(IAsyncResult asyncResult)
 *         void EndDisconnect(IAsyncResult asyncResult)
 *         int EndReceive(IAsyncResult asyncResult, out SocketError errorCode)
 *         int EndReceive(IAsyncResult asyncResult)
 *         int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint)
 *         int EndReceiveMessageFrom(IAsyncResult, ref SocketFlags, ref EndPoint endPoint, out IPPacketInformation);
 *         int EndSend(IAsyncResult asyncResult)
 *         int EndSend(IAsyncResult asyncResult, out SocketError errorCode)
 *         int EndSendTo(IAsyncResult asyncResult)
 *         void EndSendFile(IAsyncResult asyncResult)
 *         
 *@subject ◆class SocketAsyncEventArgs : EventArgs, IDisposable
 *                    -- System.Net.Sockets
 *         SocketAsyncEventArgs  new SocketAsyncEventArgs()
 *         
 *         int     e.Count { get; }
 *         int     e.Offset { get; }
 *         int     e.BytesTransferred { get; }
 *         byte[]  e.Buffer { get; }
 *         IList<ArraySegment<byte>>  e.BufferList { get; set; }
 *         int     e.SendPacketsSendSize { get; set; }
 *         bool    e.DisconnectReuseSocket { get; set; }
 *         object  e.UserToken { get; set; }
 *         EndPoint e.RemoteEndPoint { get; set; }
 *         Socket   e.ConnectSocket { get; }
 *         Socket   e.AcceptSocket { get; set; }
 *         SocketFlags  e.SocketFlags { get; set; }
 *         SocketError  e.SocketError { get; set; }
 *         SocketAsyncOperation  e.LastOperation { get; }
 *         SocketClientAccessPolicyProtocol  e.SocketClientAccessPolicyProtocol { get; set; }
 *         TransmitFileOptions   e.SendPacketsFlags { get; set; }
 *         SendPacketsElement[]  e.SendPacketsElements { get; set; }
 *         IPPacketInformation   e.ReceiveMessageFromPacketInfo { get; }
 *         Exception  e.ConnectByNameError { get; }
 *                 
 *         void  e.SetBuffer([byte[] buffer], int offset, int count)
 *   event EventHandler<SocketAsyncEventArgs>  e.Completed
 */
#endregion
/*
 *@see ImageTcpClientSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-06
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
                ClientSize = this.ClientSize,
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
