/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR17_WindowsSystem
 *@class MainClipboardSample.cs
 *@class   └ new FormClipboardSample() : Form
 *@class       └ Clipboard.SetXxxx()  static Method
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[476]-[480] p809 / Clipboard
 *         Windows Clipboard を操作し、取得/保存するプログラム
 *         
 *@subject ◆class Clipboard -- System.Windows.Forms
 *         [×] 'new' is not avaliable.
 *         
 *         + static bool    Clipboard.ContainsText() 
 *         + static bool    Clipboard.ContainsText(TextDataFormat format) 
 *         + static bool    Clipboard.ContainsImage()  
 *         + static bool    Clipboard.ContainsAudio() 
 *         + static bool    Clipboard.ContainsData(string format) 
 *             └ [Argument] string -> class DataFormats 〔below〕
 *         + static bool    Clipboard.ContainsFileDropList() 
 *         
 *         + static string  Clipboard.GetText() 
 *         + static string  Clipboard.GetText(TextDataFormat format) 
 *         + static Image   Clipboard.GetImage()
 *         + static Stream  Clipboard.GetAudioStream() 
 *         + static object  Clipboard.GetData(string format) 
 *             └ [Argument] string -> class DataFormats 〔below〕
 *         + static IDataObject  Clipboard.GetDataObject() 
 *         + static StringCollection  Clipboard.GetFileDropList() 
 *         
 *         + static void  Clipboard.SetText(string text) 
 *         + static void  Clipboard.SetText(string text, TextDataFormat format) 
 *         + static void  Clipboard.SetImage(Image image)           
 *         + static void  Clipboard.SetAudio(Stream audioStream) 
 *         + static void  Clipboard.SetAudio(byte[] audioBytes) 
 *         + static void  Clipboard.SetData(string format, object data)
 *             └ [Argument] string -> class DataFormats 〔below〕
 *         + static void  Clipboard.SetDataObject(object data) 
 *         + static void  Clipboard.SetDataObject(object data, bool copy, int retryTimes, int retryDelay) 
 *         + static void  Clipboard.SetDataObject(object data, bool copy) 
 *         + static void  Clipboard.SetFileDropList(StringCollection filePaths) 
 *         + static void  Clipboard.Clear()  
 *         
 *@subject ◆class DataFormats -- System.Windows.Forms
 *         + static readonly string  DataFormats.Text 
 *         + static readonly string  DataFormats.Serializable 
 *         + static readonly string  DataFormats.StringFormat 
 *         + static readonly string  DataFormats.CommaSeparatedValue 
 *         + static readonly string  DataFormats.Rtf 
 *         + static readonly string  DataFormats.Html 
 *         + static readonly string  DataFormats.Locale 
 *         + static readonly string  DataFormats.FileDrop 
 *         + static readonly string  DataFormats.WaveAudio 
 *         + static readonly string  DataFormats.Riff 
 *         + static readonly string  DataFormats.PenData 
 *         + static readonly string  DataFormats.OemText 
 *         + static readonly string  DataFormats.Tiff 
 *         + static readonly string  DataFormats.Dif 
 *         + static readonly string  DataFormats.SymbolicLink 
 *         + static readonly string  DataFormats.MetafilePict 
 *         + static readonly string  DataFormats.EnhancedMetafile 
 *         + static readonly string  DataFormats.Bitmap 
 *         + static readonly string  DataFormats.Dib 
 *         + static readonly string  DataFormats.UnicodeText 
 *         + static readonly string  DataFormats.Palette 
 *         + static          Format  DataFormats.GetFormat(int id) 
 *         + static          Format  DataFormats.GetFormat(string format) 
 *         
 *         ＊Inner class
 *         + class Format -- System.Windows.Forms
 *         + Format  new Format(string name, int id) 
 *         + string  format.Name { get; } 
 *         + int     format.Id { get; } 
 *

 *@NOTE【Problem】Clipboad.ContainsXxxx(),  SetXxxx()
 *      Clipboard上に複数保存できるように Windows設定を変更していても、
 *      直近の１つ目のみ、検索・取得可能。
 *      
 *@NOTE【Problem】Clipboad.ContainsImage(), SetImage()
 *      VSの説明には Bitmap形式となっているが、「.jpg」も可能。
 *      Clipboard 上にあっても、false, null となる場合がある。
 *      ・Windows機能の[切取 & スケッチ]で保存した場合は「.jpg」でも OK
 *      ・上記機能を利用していない Windowsコピーは、
 *        Bitmap形式「.bmp」でも、JPEG形式「.JPG」「.jpg」でも NG
 *      ・pic.Image = new Bitmap(Clipboard.SetImage()); としても、
 *        上記で null となる場合は、ArgumentNullReferenceException
 *      
 *@see ImageClipboardSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-20
 */
using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR17_WindowsSystem
{
    class MainClipboardSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormClipboardSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormClipboardSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormClipboardSample : Form
    {
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly TextBox textBox;
        private readonly PictureBox pic;
        private readonly Button[] buttonAry;
        private readonly string[] buttonTextAry = new string[]
        {
            "Load Text", "Load Image", "Save Text", "Save Image",
        };

        public FormClipboardSample()
        {
            this.Text = "FormClipboardSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormClipboardSample");
            this.Load += new EventHandler(FormClipboardSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormClipboardSample_FormClosed);

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = buttonTextAry.Length,
                RowCount = 3,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            for (int i = 0; i < table.ColumnCount; i++)
            {
                table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100f / table.ColumnCount));
            }//for

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 70f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));

            //---- TextBox ----
            textBox = new TextBox()
            {
                Text = "",
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBox, 0, 0);
            table.SetColumnSpan(textBox, table.ColumnCount);

            //---- PictureBox ----
            pic = new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(pic, 0, 1);
            table.SetColumnSpan(pic, table.ColumnCount);

            //---- Button ----
            buttonAry = new Button[buttonTextAry.Length];
            for (int i = 0; i < buttonTextAry.Length; i++)
            {
                buttonAry[i] = new Button()
                {
                    Text = buttonTextAry[i],
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                buttonAry[i].Click += new EventHandler(Button_Click);
                table.Controls.Add(buttonAry[i], i, 2);
            }//for

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            string buttonText = (sender as Button).Text;

            switch (buttonText)
            {
                case "Load Text":
                    if (Clipboard.ContainsText())
                    {
                        textBox.Text = Clipboard.GetText();
                    }
                    else
                    {
                        ShowMessage("No Text in Clipboard.");
                    }
                    break;

                case "Load Image":
                    if (Clipboard.ContainsImage())
                    {
                        pic.Image = Clipboard.GetImage();
                    }
                    else
                    {
                        ShowMessage("No Image in Clipboard.");
                    }
                    break;

                case "Save Text":
                    if (ValidateInput(textBox.Text))
                    {
                        Clipboard.SetText(textBox.Text);
                        ShowMessage("Saved Text to Clipboard.");
                        textBox.Text = "";
                    }
                    
                    break;
                case "Save Image":
                    if (pic.Image != null)
                    {
                        Clipboard.SetImage(pic.Image);
                        ShowMessage("Saved Image to Clipboard.");
                        pic.Image = null;
                    }
                    else
                    {
                        ShowMessage("No Image in PictureBox.");
                    }
                    break;
            }//switch
        }//Button_Click()

        private bool ValidateInput(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                ShowMessage("No Text in TextBox.");
                return false; 
            }
            
            var errorMessageBld = new StringBuilder();

            //---- Anti Script ----
            Regex regexAntiScript = new Regex(@"[<>&;]+");

            if (regexAntiScript.IsMatch(input))
            {
                errorMessageBld.Append(
                    $"<！> Invalid Input ! {Environment.NewLine}");
            }

            if (errorMessageBld.Length > 0)
            {
                ShowMessage(errorMessageBld.ToString());
                return false;
            }

            return true; // bool canInput
        }//ValidateInput()

        private void ShowMessage(string errorMessage)
        {
            MessageBox.Show(
                errorMessage,
                "Notation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }//ShowErrorMessage()

        //====== Form Event ======
        private void FormClipboardSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                ShowMessage("This Form already has been running.");
                this.Close();
            }
        }//FormClipboardSample_Load()

        private void FormClipboardSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.Close();
        }//FormClipboardSample_FormClosed()
    }//class
}
