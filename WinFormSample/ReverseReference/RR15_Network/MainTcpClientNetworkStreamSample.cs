/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR15_Network
 *@class MainTcpClientNetworkStreamSample.cs
 *@class   └ new FormTcpClientNetworkStreamSample() : Form
 *@class       └ new TcpClient()
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[422][423] p714 | TcpClient / NetworkStream
 *         【Required To Excuse】実行前に必要なこと
 *          ・WebServer -- Apache を起動 「Apache24/bin/httpd.exe」
 *          ・「startTcpSample.html」 を　Apache24内に作成
 *          ・「httpd.exe」->「startTcpSample.html」の相対パス: 「../startTcpSample.html」
 *
 *@subject ◆class NetworkStream : Stream -- System.Net.Sockets
 *         + NetworkStream  new NetworkStream(Socket socket)
 *         + NetworkStream  new NetworkStream(Socket socket, bool ownsSocket)
 *         + NetworkStream  new NetworkStream(Socket socket, FileAccess access)
 *         + NetworkStream  new NetworkStream(Socket socket, FileAccess access, bool ownsSocket)
 *         
 *         + long Length { get; }
 *         + long Position { get; set; }
 *         + int ReadTimeout { get; set; }
 *         + int WriteTimeout { get; set; }
 *         + bool CanRead { get; }
 *         + bool CanWrite { get; }
 *         + bool CanTimeout { get; }
 *         + bool CanSeek { get; }
 *         + bool DataAvailable { get; }
 *         # Socket Socket { get; }
 *         # bool Readable { get; set; }
 *         # bool Writeable { get; set; }
 *         
 *         + void SetLength(long value)
 *         + int Read(byte[] buffer, int offset, int size)
 *         + void Write(byte[] buffer, int offset, int size)
 *         + long Seek(long offset, SeekOrigin origin)
 *         + IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
 *         + IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
 *         + void Flush()
 *         + Task FlushAsync(CancellationToken cancellationToken)
 *         + void Close(int timeout)
 *         + int EndRead(IAsyncResult asyncResult)
 *         + void EndWrite(IAsyncResult asyncResult)
 *         # void Dispose(bool disposing)
 *         
 *@NOTE【Problem】Result
 *      Send: output "Successed to send data", when 'Apache' was running.
 *            But 'startTcpSample.html' had no data.
 *            
 *      Recieve: output "400 Bad Request" from Web Server.
 *               I don't find the reason.
 *               
 *               output "404 Not Found" when 'startTcpSample.html' was not.
 *               So I made the Html file in Directory 'Apache24',
 *               wrote "../startTcpSample.html" in 'GetBytes()' argument.
 *               "../" means the relative path from 'httpd.exe' as Apache execute file.
 *               Then output changed "400 Bad Request" as above.
 *               
 *@see ImageTcpClientNetworkStreamSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-07
 */
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR15_Network
{
    class MainTcpClientNetworkStreamSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormTcpClientNetworkStreamSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormTcpClientNetworkStreamSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormTcpClientNetworkStreamSample : Form
    {
        private readonly TcpClient client;
        private readonly TableLayoutPanel table;
        private readonly Button buttonSend;
        private readonly Button buttonRecieve;
        private readonly TextBox textBox;

        public FormTcpClientNetworkStreamSample()
        {
            this.Text = "FormTcpClientNetworkStreamSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 480);
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- TcpClient ----
            client = new TcpClient();

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 3,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 80f));

            //---- Button ----
            buttonSend = new Button()
            {
                Text = "Send Data by NetworkStream",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonSend.Click += new EventHandler(ButtonSend_Click);
            table.Controls.Add(buttonSend);

            buttonRecieve = new Button()
            {
                Text = "Recieve Data by NetworkStream",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonRecieve.Click += new EventHandler(ButtonRecieve_Click);
            table.Controls.Add(buttonRecieve);

            //---- TextBox ----
            textBox = new TextBox()
            {
                Multiline = true,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBox);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect("localhost", 80);
                NetworkStream stream = client.GetStream();
                byte[] buffer = Encoding.ASCII.GetBytes(
                    "GET /startTcpSample.html HTTP/1.0\r\n\r\n");
                stream.Write(buffer, offset: 0, size: buffer.Length);

                textBox.Text = "Successed to send data.";
            }
            catch (Exception ex)
            {
                textBox.Text = $"{ex.GetType()}: \n{ex.Message}";
            }
            finally
            {
                client.Close();
            }
        }//ButtonSend_Click()

        private void ButtonRecieve_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect("localhost", 80);
                NetworkStream stream = client.GetStream();
                byte[] buffer = Encoding.ASCII.GetBytes(
                    "GET ../startTcpSample.html HTTP/1.0\r\n\r\n");
                stream.Write(buffer, offset: 0, size: buffer.Length);

                byte[] data = new byte[1011];
                stream.Read(data, offset: 0, size: data.Length);

                textBox.Text = Encoding.ASCII.GetString(data);
            }
            catch (Exception ex)
            {
                textBox.Text = $"{ex.GetType()}: \n{ex.Message}";
            }
            finally
            {
                client.Close();
            }
        }//ButtonRecieve_Click()
    }//class
}
