/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR04_Control
 *@class GuiAboutBoxSample.cs
 *@class   └ ◆Main() { new GuiAboutBoxSample() : Form }
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *           
 *@content RR[51] AboutBox 情報Box / p102
 *         ・Form, Form部品を利用した Template
 *         ・VS -> [追加] -> [新しい項目] -> [情報Box](WindowsForm)
 *           -> クラス名「xxxxx.cs」-> [OK]
 *         ・追加した時点で Assembly情報とのDataBind済
 *           ([デザイン]表示では、データの値は表示されない。Form実行すると表示)
 *         ・PictureBoxの画像選定: [>]クリック -> [イメージの選択]
 *         ・Button[OK] のイベントハンドラー処理を記述 this.Close()など
 *         
 *@subject Assemblyクラス
 *         =>〔KT09_CustomDialog/MainAssemblyNameSample.cs〕
 *
 *@see ImageGuiAboutBoxSample.jpg
 *@see 
 *@author shika
 *@date 2022-08-05
 */

using System;
using System.Reflection;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR06_Template
{    
    partial class GuiAboutBoxSample : Form
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("GuiAboutBoxSample()");

            Application.EnableVisualStyles();
            Application.Run(new GuiAboutBoxSample());

            Console.WriteLine("Close()");
        }//Main()

        public GuiAboutBoxSample()
        {
            InitializeComponent();
            this.Text = String.Format("{0} のバージョン情報", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("バージョン {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #region アセンブリ属性アクセサー

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
