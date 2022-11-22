/** 
 *@title WinFormGUI / CsharpCode
 *@class MainVisualStudioMetadataViewer.cs
 *@class   └ new FormVisualStudioMetadataViewer() : Form
 *@class       └ new 
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content Visual Studio Metadata Viewer
 *         This Viewer is for 'class' and 'struct'
 *         to Remove comment ,annotation, or others,
 *         to Edit for adjustment from VS-Metadata.
 *         
 *         If Type were 'enum', use 'ShowEnumValue.cs'.
 *         
 *         ---- Flag Parameters ----
 *@params  bool withSubject   if it is for VS top comment.
 *@params  bool isSimpled     if 'buttonReplace' already has been clicked once.
 *@params  bool isGenericOverTwo
 *           case Dictionary: className location need 1 right shift.
 *           case Func:       className location need some right shift, such case as Func<T, T, ...>
 *@params  bool isInherit     (local)  if this class has base class.
 *           case inherit:    className with ":" base class.
 *@params  bool isInnerClass  (local)
 *           case inner class:  Because it is a Member of this class, 
 *                              write with "＊Inner class", as usual member.
 *         
 *@subject Mutex     => 〔MainMutexWaitOneSample.cs〕
 *         二重起動を防止
 *         
 *@subject Clipboard => 〔MainClipboardSample.cs〕
 *         TextBox.Text を Windows OS の Clipboard にコピー
 *
 *@see ImageVisualStudioMetadataViewer.jpg
 *@see 
 *@author shika
 *@date 2022-11-07
 */
using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace WinFormGUI.CsharpCode
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
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly Label label;
        private readonly RichTextBox textBox;
        private readonly Button buttonReplace;
        private readonly Button buttonCopy;
        private const bool withSubject = true;
        private bool isSimpled = false;
        private bool isGenericOverTwo = false;
        private Regex regexConstructor;

        public FormVisualStudioMetadataViewer()
        {
            this.Text = "FormVisualStudioMetadataViewer";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(1024, 600);
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormVisualStudioMetadataViewer");
            this.Load += new EventHandler(FormVisualStudioMetadataViewer_Load);
            this.FormClosed += new FormClosedEventHandler(FormVisualStudioMetadataViewer_FormClosed);

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
            if(isSimpled || String.IsNullOrEmpty(textBox.Text)) { return; }

            string[] lineAry = textBox.Text.Split(Environment.NewLine.ToCharArray(),
                 StringSplitOptions.RemoveEmptyEntries);

            var bld = new StringBuilder();
            if (withSubject) { bld.Append(" *@subject ◆"); }

            string namespaceName = "";
            string className = "";
            bool isInnerClass = false;
            bool isInterface = false;

            foreach (string line in lineAry)
            {
                string trimedLine = line.Trim();

                //---- Delete ----
                if(trimedLine.StartsWith("//") 
                    || trimedLine.StartsWith("[")
                    || trimedLine.StartsWith("{")
                    || trimedLine.StartsWith("}")
                    || trimedLine.StartsWith("*")
                    || trimedLine.Contains("~")
                    || trimedLine.Contains("#")
                    || trimedLine.Contains("using")
                    || trimedLine.Contains("operator")
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

                //---- Get namespaceName ----
                if (trimedLine.StartsWith("namespace "))
                {
                    namespaceName =
                        trimedLine.Substring(startIndex: "namespace ".Length);
                    continue;
                }

                //---- Get className and Append ----
                bool isInherit = trimedLine.Contains(":");

                if (trimedLine.Contains("class ") 
                    || trimedLine.Contains("struct ")
                    || trimedLine.Contains("interface "))
                {
                    string containText = trimedLine.Contains("class ") ? "class " : "struct ";
                    if (trimedLine.Contains("interface "))
                    {
                        containText = "interface ";
                        isInterface = true;
                    }

                    int startIndex = containText.Length + trimedLine.IndexOf(containText);

                    isInnerClass = (className == "") ? false : true;

                    if (isInherit)
                    {
                        int length = trimedLine.IndexOf(":") - startIndex;
                        className = trimedLine.Substring(startIndex, length).Trim();
                    }
                    else
                    {
                        className = trimedLine.Substring(startIndex).Trim();
                    }

                    string pattenConstructor = $@"[ ]+{className}\(+[0-9a-zA-z,<>\[\] ]*\)+";
                    regexConstructor = new Regex(pattenConstructor);

                    //---- case inner class ----
                    if (isInnerClass && withSubject)
                    {
                        bld.Append(" *         \n");
                        bld.Append(" *         ＊Inner class\n");
                        bld.Append(" *         ");
                    }
                    else
                    {
                        trimedLine = trimedLine.Replace("+ ", "");
                    }

                    //---- class line ----
                    bld.Append($"{trimedLine}");

                    //---- case inherit ----
                    if (isInherit)
                    { 
                        bld.Append("\n"); 

                        if (withSubject){ bld.Append(" *         "); }
                    
                        for(int i = 0; i < ("class ".Length + className.Length); i++)
                        {
                            bld.Append(" ");
                        }//for
                    }

                    bld.Append($" -- {namespaceName}\n");

                    //---- case No Constructor Message ----
                    if(trimedLine.Contains("static class "))
                    {
                        if (withSubject) { bld.Append(" *         "); }
                        bld.Append("[×] 'new' is not available, because of static class.\n");
                    }

                    continue;
                }//if class
                
                //---- Append Member with className ----
                if (withSubject) { bld.Append(" *         "); }

                string[] splitedWord = trimedLine.Split(
                    new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
 
                for (int i = 0; i < splitedWord.Length; i++)
                {                    
                    if (regexConstructor.IsMatch(trimedLine))  //case constructor
                    { 
                        if (i == 1)  //index of returnType
                        {
                            if (trimedLine.Contains("# "))     //case protected constructor
                            {
                                bld.Append($"{className}  ");
                            }
                            else    //case public constructor
                            {
                                bld.Append($"{className}  new ");
                            }
                        }//if constructor

                        if(trimedLine.Contains("# ") && i == splitedWord.Length - 1)  //case protected constructor
                        {
                            bld.Append(splitedWord[i]);
                            bld.Append("\n");

                            if (withSubject) { bld.Append(" *         "); }
                            bld.Append("  [×] 'new' is not available, but 'base()' is OK from constructor of inherited class ONLY.\n");
                            if (withSubject) { bld.Append(" *         "); }
                            break;
                        }
                    }
                    else if (trimedLine.Contains("static readonly"))
                    {
                        if (i == 3 && CaseGenericOverTwo(ref i, splitedWord, className, ref bld))
                        {
                            bld.Append($" {className}.");
                            continue;
                        }

                        if (i == 4 && !isGenericOverTwo)
                        {
                            bld.Append($" {className}.");                            
                        }
                    }
                    else if (trimedLine.Contains("static "))
                    {
                        if (i == 2 && CaseGenericOverTwo(ref i, splitedWord, className, ref bld))
                        {
                            bld.Append($" {className}.");
                            continue;
                        }
                            
                        if (i == 3 && !isGenericOverTwo)
                        {
                            bld.Append($" {className}.");
                        }
                    }
                    else if (trimedLine.Contains("internal abstract"))
                    {
                        if (i == 3 && CaseGenericOverTwo(ref i, splitedWord, className, ref bld))
                        {
                            InsertInstanceClassName(bld, className);
                            continue;
                        }

                        if (i == 4 && !isGenericOverTwo)
                        {
                            InsertInstanceClassName(bld, className);
                        }
                    }
                    else if (trimedLine.Contains("abstract ")
                        || trimedLine.Contains("internal ")
                        || trimedLine.Contains("event ")
                        || trimedLine.Contains("const "))
                    { 
                        if (i == 2 && CaseGenericOverTwo(ref i, splitedWord, className, ref bld))
                        {
                            InsertInstanceClassName(bld, className);
                            continue;
                        }
                        
                        if (i == 3 && !isGenericOverTwo)
                        {
                            InsertInstanceClassName(bld, className);
                        }
                    }
                    else if (isInterface)
                    {
                        if (i == 0 && CaseGenericOverTwo(ref i, splitedWord, className, ref bld))
                        {
                            bld.Append($" {className}.");
                            continue;
                        }

                        if (i == 1 && !isGenericOverTwo)
                        {
                            bld.Append($" {className}.");
                        }
                    }
                    else if (i == 1 && CaseGenericOverTwo(ref i, splitedWord, className, ref bld))  //case Dictionary<T, T>
                    {
                        InsertInstanceClassName(bld, className);
                        continue;
                    }
                    else if (i == 2 && !isGenericOverTwo)
                    {
                        InsertInstanceClassName(bld, className);
                    }

                    bld.Append($"{splitedWord[i]} ");
                }//for word

                bld.Append("\n");
            }//foreach lineAry
            bld.Append(" *\n");

            isSimpled = true;
            label.Text = "Simpled Metadata:";
            textBox.Text = bld.ToString();
            textBox.Refresh();
            textBox.Focus();   //Scroll を Topに戻す
        }//ButtonReplace_Click()

        private void InsertInstanceClassName(StringBuilder bld, string className)
        {
            bld.Append(
                $" {className.Substring(0, 1).ToLower()}{className.Substring(1)}.");
        }//InsertInstanceClassName()

        private bool CaseGenericOverTwo(ref int i, string[] splitedWord, string className, ref StringBuilder bld)
        {
            if (splitedWord[i].Contains("<") 
                && splitedWord[i].Contains(","))  //case Dictionary<T, T>, Func<T1, T2, T3, ...>
            {                                     //[Example] class HttpClientHandler
                bld.Append($"{splitedWord[i]} ");

                do
                {
                    bld.Append($"{splitedWord[++i]} ");
                }
                while (!splitedWord[i].Contains(">"));

                isGenericOverTwo = true;
                return true;
            }

            isGenericOverTwo = false;
            return false;
         }//CaseDicType()

        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            if (!isSimpled || String.IsNullOrEmpty(textBox.Text)) { return; }

            Clipboard.SetText(textBox.Text);
            MessageBox.Show("Copied to Clip Board", "Notation");
            
            isSimpled = false;
            label.Text = "Paste Metadata Document:";
            textBox.Text = "";
        }//ButtonCopy_Click()

        private void FormVisualStudioMetadataViewer_Load(object sender, EventArgs e)
        {
            if(!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running here.");
                this.Close();
            }
        }//FormVisualStudioMetadataViewer_Load()

        private void FormVisualStudioMetadataViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.Close();
        }//FormVisualStudioMetadataViewer_FormClosed()
    }//class
}

/*
 //==== Result Copy to Clip Board ====
 *@subject ◆+ sealed class DriveInfo : ISerializable
 *                          -- System.IO
 *         + DriveInfo(string driveName)
 *         + string Name { get; }
 *         + DriveType DriveType { get; }
 *         + string DriveFormat { get; }
 *         + bool IsReady { get; }
 *         + long AvailableFreeSpace { get; }
 *         + long TotalFreeSpace { get; }
 *         + long TotalSize { get; }
 *         + DirectoryInfo RootDirectory { get; }
 *         + string VolumeLabel { get; set; }
 *         + static DriveInfo[] GetDrives()
 *         + string ToString()
 */