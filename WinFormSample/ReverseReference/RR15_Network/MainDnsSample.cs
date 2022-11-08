/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainDnsSample.cs
 *@class   └ new FormDnsSample() : Form
 *@class       └ static System.Net.Dns 
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *         
 *@content RR 第15章 Network / RR[419][420] p710
 *         Dnsクラス
 *        ・PCの HostName, IP Addressを取得するクラス
 *        ・互換性のために残されているメンバーを含む
 *        ・引数 hostNameは 255 文字以内 
 *          -> 超えると ArgumentOutOfRangeException
 *        
 *        ・[VS] Dns class: 単純なドメイン名の名前解決機能を提供するクラス
 *        ・[英] DNS: Dimain Name Server   (as usual)
 *        ・[英] Dns: Dimain Name Solution ?
 *        ・[英] IP:  Internet Protocol
 */
#region -> Dns, IPHostEntry, IPAddress
/*
 *@subject ◆static class Dns -- System.Net.
 *         [×] 'new' is not avaliable, because of static class.
 *         
 *         static string      Dns.GetHostName()
 *         static IPAddress[] Dns.GetHostAddresses(string hostNameOrAddress);
 *         
 *         static IPHostEntry Dns.GetHostEntry(string hostNameOrAddress);
 *         static IPHostEntry Dns.GetHostEntry(IPAddress);
 *         static IPHostEntry Dns.GetHostByName(string hostName);
 *         static IPHostEntry Dns.GetHostByAddress(string address);
 *         static IPHostEntry Dns.GetHostByAddress(IPAddress);
 *         
 *         ＊Async
 *         static Task<IPHostEntry>  Dns.GetHostEntryAsync(string hostNameOrAddress)
 *         static Task<IPHostEntry>  Dns.GetHostEntryAsync(IPAddress)
 *         static Task<IPAddress[]>  Dns.GetHostAddressesAsync(string hostNameOrAddress);
 *         
 *         static IAsyncResult  Dns.BeginGetHostEntry(string hostNameOrAddress, AsyncCallback requestCallback, object stateObject);
 *         static IAsyncResult  Dns.BeginGetHostEntry(IPAddress, AsyncCallback, object stateObject);
 *         static IAsyncResult  Dns.BeginGetHostByName(string hostName, AsyncCallback, object stateObject);       [非推奨] use BeginGetHostEntry()
 *         static IAsyncResult  Dns.BeginGetHostAddresses(string hostNameOrAddress, AsyncCallback, object state);
 *         
 *         static IPHostEntry   Dns.EndGetHostEntry(IAsyncResult asyncResult);
 *         static IPHostEntry   Dns.EndGetHostByName(IAsyncResult asyncResult);    [非推奨] use EndGetHostEntry()
 *         static IPAddress[]   Dns.EndGetHostAddresses(IAsyncResult asyncResult);
 *         
 *@subject ◆class IPHostEntry -- System.Net
 *         IPHostEntry  new IPHostEntry()
 *         string       ipHostEntry.HostName { get; set; }
 *         string[]     ipHostEntry.Aliases { get; set; }
 *         IPAddress[]  ipHostEntry.AddressList { get; set; }
 *         
 *@subject ◆class IPAddress   -- System.Net
 *         IPAddress  new IPAddress(long newAddress)
 *         IPAddress  new IPAddress(byte[] address)
 *         IPAddress  new IPAddress(byte[] address, long scopeid)
 *         IPAddress  ipAddress.MapToIPv4();
 *         IPAddress  ipAddress.MapToIPv6();
 *  static IPAddress  IPAddress.Parse(string ipString);
 *  static bool       IPAddress.TryParse(string ipString, out IPAddress address);
 *         
 *         ＊static readonly Field
 *         static readonly IPAddress  IPAddress.Any;       //Server が すべてのNetwork interfaceで Client によるネットワーク利用を待機する必要がある IPAddress
 *         static readonly IPAddress  IPAddress.Loopback;  //IP ループバック アドレス
 *         static readonly IPAddress  IPAddress.Broadcast; //IP ブロードキャスト アドレス
 *         static readonly IPAddress  IPAddress.None;      //IP アドレスを提供し、ネットワーク インターフェイスを使用しないことを示す。
 *         static readonly IPAddress  IPAddress.IPv6Any;   
 *                 //System.Net.Sockets.Socket.Bind(System.Net.EndPoint) は、
 *                 //System.Net.IPAddress.IPv6Any フィールドを使用して、
 *                 //System.Net.Sockets.Socket が、すべてのネットワーク インターフェイスでクライアントによるネットワーク利用を待機する必要があることを示します。
 *         static readonly IPAddress  IPAddress.IPv6Loopback;    //IPv6 の IP ループバック アドレス
 *         static readonly IPAddress  IPAddress.IPv6None;        //IPv6 の IP アドレスを提供し、ネットワーク インターフェイスを使用しないことを示す。
 *
 *         ＊Property
 *         long       ipAddress.Address { get; set; }
 *         long       ipAddress.ScopeId { get; set; }
 *         bool       ipAddress.IsIPv6SiteLocal { get; }
 *         bool       ipAddress.IsIPv6LinkLocal { get; }
 *         bool       ipAddress.IsIPv6Multicast { get; }
 *         bool       ipAddress.IsIPv6Teredo { get; }
 *         bool       ipAddress.IsIPv4MappedToIPv6 { get; }
 *         
 *         ＊Method
 *         static T    IpAddress.HostToNetworkOrder(T host);    T: 数値型
 *         static bool IpAddress.IsLoopback(IPAddress address);
 *         byte[]      ipAddress.GetAddressBytes();
 *         
 *         AddressFamily  ipAddress.AddressFamily { get; }
 *           └ enum AddressFamily -- System.Net.Sockets
 *             {
 *                Unspecified = 0,
 *                Unix = 1,
 *                InterNetwork = 2,
 *                ImpLink = 3,
 *                Pup = 4,
 *                Chaos = 5,
 *                Ipx = 6,
 *                NS = 6,
 *                Iso = 7,
 *                Osi = 7,
 *                Ecma = 8,
 *                DataKit = 9,
 *                Ccitt = 10,
 *                Sna = 11,
 *                DecNet = 12,
 *                DataLink = 13,
 *                Lat = 14,
 *                HyperChannel = 15,
 *                AppleTalk = 16,
 *                NetBios = 17,
 *                VoiceView = 18,
 *                FireFox = 19,
 *                Banyan = 21,
 *                Atm = 22,
 *                InterNetworkV6 = 23,
 *                Cluster = 24,
 *                Ieee12844 = 25,
 *                Irda = 26,
 *                NetworkDesigners = 28,
 *                Max = 29,
 *                Unknown = -1,
 *            }
 *         
 *@result  Host Name:  LAPTOP-*********
 *         IP Address: 192.168.xxx.xxx  (IPv4)
 *         IP Address: 複数の IPv6      (ipAry)
 */
#endregion
/*
 *@see (No Image for security)  [×] ImageDnsSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-04
 */
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainDnsSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormDnsSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormDnsSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormDnsSample : Form
    {
        private readonly TableLayoutPanel table;
        private readonly Label labelHost;
        private readonly Label labelIp;
        private readonly TextBox textBoxHost;
        private readonly TextBox textBoxIp;
        private readonly Button button;

        public FormDnsSample()
        {
            this.Text = "FormDnsSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 320);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 30f));
            table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 70f));

            for (int i = 0; i < table.RowCount; i++)
            {
                table.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 100f / table.RowCount));
            }//for RowStyles

            //---- Label ----
            labelHost = new Label()
            {
                Text = "Host Name: ",
                TextAlign = ContentAlignment.TopRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelHost, 0, 0);

            labelIp = new Label()
            {
                Text = "IP Address: ",
                TextAlign = ContentAlignment.TopRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(labelIp, 0, 1);

            //---- TextBox ----
            textBoxHost = new TextBox()
            {                
                Multiline = false,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(textBoxHost, 1, 0);

            textBoxIp = new TextBox()
            {
                Multiline = true,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(textBoxIp, 1, 1);

            //---- Button ----
            button = new Button()
            {
                Text = "Get Host, IP",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button, 0, 2);
            table.SetColumnSpan(button, 2);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            string hostName = Dns.GetHostName();
            //IPAddress[] ipAry = Dns.GetHostAddresses(hostName);
            IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);

            //---- Mask hostName ----
            string[] hostNameSplitAry = hostName.Split('-');
            var bld = new StringBuilder(hostName.Length);
            if(hostNameSplitAry.Length >= 2)
            {
                bld.Append(hostNameSplitAry[0]);
                bld.Append("-");
                
                for(int i = 0; i < hostNameSplitAry[1].Length; i++)
                {
                    bld.Append("*");
                }//for
            }//if
            string hostNameMasked = bld.ToString();

            //==== Show ====
            textBoxHost.Text = hostNameMasked;

            //---- ipAry ----
            //for(int i = 0; i < ipAry.Length; i++)
            //{
            //    textBoxIp.Text += $"{ipAry[i]}\n";
            //}

            //---- AddressFamily.InterNetwork / IPv4 ----
            foreach (IPAddress ip in ipHostEntry.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    textBoxIp.Text = ip.ToString();
                }
            }

            this.Refresh();
        }//Button_Click()
    }//class
}
