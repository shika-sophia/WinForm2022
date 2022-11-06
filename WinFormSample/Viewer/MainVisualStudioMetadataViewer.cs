/** 
 *@title WinFormGUI / WinFormSample / Viewer
 *@class MainVisualStudioMetadataViewer.cs
 *@class   └ new FormVisualStudioMetadataViewer() : Form
 *@class       └ new 
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content Visual Studio Metadata Viewer
 *         Program to Remove comment and annotation from VS-Metadata.
 *         
 *@subject ◆class Clipboard -- System.Windows.Forms
 *         + static void Clear()
 *         + static bool ContainsAudio()
 *         + static bool ContainsData(string format)
 *         + static bool ContainsFileDropList()
 *         + static bool ContainsImage()
 *         + static bool ContainsText(TextDataFormat format)
 *         + static bool ContainsText()
 *         + static Stream GetAudioStream()
 *         + static object GetData(string format)
 *         + static IDataObject GetDataObject()
 *         + static StringCollection GetFileDropList()
 *         + static Image GetImage()
 *         + static string GetText()
 *         + static string GetText(TextDataFormat format)
 *         + static void SetAudio(Stream audioStream)
 *         + static void SetAudio(byte[] audioBytes)
 *         + static void SetData(string format, object data)
 *         + static void SetDataObject(object data)
 *         + static void SetDataObject(object data, bool copy, int retryTimes, int retryDelay)
 *         + static void SetDataObject(object data, bool copy)
 *         + static void SetFileDropList(StringCollection filePaths)
 *         + static void SetImage(Image image)
 *         + static void SetText(string text)
 *         + static void SetText(string text, TextDataFormat format)
 *
 *@see ImageVisualStudioMetadataViewer.jpg
 *@see 
 *@author shika
 *@date 2022-11-07
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.Viewer
{
    class MainVisualStudioMetadataViewer
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormVisualStudioMetadataViewer()");

            Application.EnableVisualStyles();
            Application.Run(new FormVisualStudioMetadataViewer());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormVisualStudioMetadataViewer : Form
    {
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly RichTextBox textBox;
        private readonly Button buttonReplace;
        private readonly Button buttonCopy;
        private const bool isSubject = true;

        public FormVisualStudioMetadataViewer()
        {
            this.Text = "FormVisualStudioMetadataViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(1024, 600);
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- TableLayoutPanel ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 80f));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));

            //---- Label ---
            label = new Label()
            {
                Text = "Paste Metadata Document:",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(label, 0, 0);
            table.SetColumnSpan(label, 2);

            //---- TextBox ----
            textBox = new RichTextBox()
            {
                Multiline = true,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
            };
            table.Controls.Add(textBox, 0, 1);
            table.SetColumnSpan(textBox, 2);

            //---- Button ----
            buttonReplace = new Button()
            {
                Text = "Replace Comment and Annotation",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonReplace.Click += new EventHandler(ButtonReplace_Click);
            table.Controls.Add(buttonReplace, 0, 2);

            buttonCopy = new Button()
            {
                Text = "Copy to Clip Board",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            buttonCopy.Click += new EventHandler(ButtonCopy_Click);
            table.Controls.Add(buttonCopy, 1, 2);

            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void ButtonReplace_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(textBox.Text)) { return; }

            string[] lineAry = textBox.Text.Split(Environment.NewLine.ToCharArray(),
                //new string[] 
                //    { Environment.NewLine, "\n", "\r", },
                StringSplitOptions.RemoveEmptyEntries);

            var bld = new StringBuilder();
            if (isSubject) { bld.Append(" *@subject ◆"); }

            foreach(string line in lineAry)
            {
                string trimedLine = line.Trim();

                //---- Delete ----
                if(trimedLine.StartsWith("//") 
                    || trimedLine.StartsWith("[")
                    || trimedLine.StartsWith("{")
                    || trimedLine.StartsWith("}")
                    || trimedLine.Contains("~")
                    || trimedLine.Contains("#")
                    || trimedLine.Contains("using")
                ) { continue; }

                //---- Replace ----
                trimedLine = trimedLine.Replace("public", "+");
                trimedLine = trimedLine.Replace("protected", "#");
                trimedLine = trimedLine.Replace("virtual ", "").Trim();
                trimedLine = trimedLine.Replace("override ", "").Trim();

                if(trimedLine.EndsWith(";")) 
                {
                    trimedLine = trimedLine.Replace(";", "");
                }

                //---- Append ----
                if (isSubject && !trimedLine.Contains("class"))
                { bld.Append(" *         "); }
                bld.Append($"{trimedLine}\n");
            }//foreach

            label.Text = "Simpled Metadata:";
            textBox.Text = bld.ToString();
            textBox.Refresh();
            textBox.Focus();   //Scroll を Topに戻す
        }//ButtonReplace_Click()

        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox.Text);
            MessageBox.Show("Copied to Clip Board", "Notation");
        }//ButtonCopy_Click()

    }//class
}
