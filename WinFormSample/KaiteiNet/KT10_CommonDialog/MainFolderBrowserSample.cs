/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT10_CommonDialog
 *@class MainFolderBrowserSample.cs
 *@class   └ new FormFolderBrowserSample() : Form
 *@class       └ new FolderBrowserDialog() : CommonDialog
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/common-dialogs/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm10_CommonDialog.txt〕
 *           
 *@content KT10 CommonDialog / FolderBrowserDialog | RR[278] p495
 *@subject ◆FolderBrowserDialog : CommonDialog
 *          FolderBrowserDialog    new FolderBrowserDialog()
 *          string     folderBrowserDialog.SelectedPath         選択フォルダーの path
 *          Environment.SpecialFolder                           参照開始位置のルートフォルダー enum SpecialFoler.Xxxx
 *                     folderBrowserDialog.RootFolder           デスクトップをルートフォルダにしておけば，すべてのファイルおよびフォルダが参照できます。
 *          string     folderBrowserDialog.Description          Dialog上部に表示される説明文字列
 *          bool       folderBrowserDialog.ShowNewFolderButton  [新しいフォルダ]ボタンを表示するか
 *          
 *@subject ◆static Environmentクラス -- System.
 *         ・PC環境に関する情報を扱うクラス
 *         
 *         staticメンバー { get; }
 *         string    Environment.CurrentDirectory  現在のWorkingディレクトリの絶対path { get; set; }
 *         string    Environment.NewLine           このPCのシステム環境での改行文字列
 *         string    Environment.UserName          ユーザー名
 *         string    Environment.UserDomainName    ユーザードメイン名
 *         string    Environment.MachineName       このPCの NetBIOS名
 *         Version   Environment.Version           共通言語ランタイム(CLR)のVersion x.x.x.x
 *         OperatingSystem  Environment.OSVersion  OSのプラットフォームID, Version
 *         bool      Environment.Is64BitProcess    プロセスが 64bitか
 *         bool      Environment.Is64BitOperatingSystem  OSが 64bitか
 *         string    Environment.SystemDirectory   システムディレクトリの絶対パス
 *         int       Environment.ProcessorCount    PCのプロセッサー数
 *         long      Environment.WorkingSet        プロセスに割り当てられる物理メモリ量
 *         int       Environment.CurrentManagedThreadId  現在処理しているThreadのID整数値
 *         
 *         string       Environment.GetFolderPath(                        //特殊フォルダの絶対パス
 *                         SpecialFolder, SpecialFolderOption);  
 *         string       Environment.GetEnvironmentVariable(               //環境変数の取得
 *                         string variable, [EnvironmentVariableTarget]);
 *         IDictionary  Environment.GetEnvironmentVariables(              //環境変数 Dictonary<K, V>
 *                         EnvironmentVariableTarget);
 *         void         Environment.SetEnvironmentVariable(               //環境変数の設定
 *                         string variable, string value, [EnvironmentVariableTarget]);
 *         
 *         enum Environment.SpecialFolder { ... }  特殊フォルダ名〔文末〕
 *         
 *         enum Environment.SpecialFolderOption    特殊フォルダの検索オプション
 *         {
 *            None = 0,             //パス検証して(=パスが存在するか) pathを返す。存在しない場合 ""。デフォルト値
 *            DoNotVerify = 16384,  //パス検証せずに pathを返す。 Web上のフォルダなどで検証時間を節約できる
 *            Create = 32768        //存在しない場合に作成するフォルダーのパス。
 *         }
 *         
 *         enum System.EnvironmentVariableTarget   環境変数の格納場所
 *         {
 *            Process = 0,  //現在プロセスの環境ブロック
 *            User = 1,     //Windows OS のレジストリキー HKEY_CURRENT_USER\Environment
 *            Machine = 2,  //Windows OS のレジストリキー HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session\Manager\Environment
 *         }
 *
 *@see 
 *@author shika
 *@date 2022-08-08
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT10_CommonDialog
{
    class MainFolderBrowserSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormFolderBrowserSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormFolderBrowserSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormFolderBrowserSample : Form
    {
        private Label label;
        private TextBox textBox;
        private Button button;

        public FormFolderBrowserSample()
        {
            this.Text = "FormFolderBrowserSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            label = new Label()
            {
                Text = "Folder / Directory:",
                Location = new Point(20, 20),
                AutoSize = true,
            };

            textBox = new TextBox()
            {
                Text = Environment.GetFolderPath(
                    Environment.SpecialFolder.Desktop),
                Location = new Point(20, 50),
                Width = 300,
                SelectionStart = 0,
                SelectionLength = 0,
            };

            button = new Button()
            {
                Text = "Forlder Open",
                Location = new Point(20, 100),
                UseVisualStyleBackColor = true,
                AutoSize = true,
            };
            button.Click += new EventHandler(button_Click);
            
            this.Controls.AddRange(new Control[]
            {
                label, textBox, button,
            });
        }//constructor

        private void button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog()
            {
                SelectedPath = textBox.Text,
                RootFolder = Environment.SpecialFolder.Desktop,
                Description = "Please select any Folder.",
                ShowNewFolderButton = false,
            };

            DialogResult result = dialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                textBox.Text = dialog.SelectedPath;
            }
        }//button_Click()
    }//class
}


/*
enum SpecialFolder
{
    Desktop = 0,
    Programs = 2,
    MyDocuments = 5,
    Favorites = 6,
    Startup = 7,
    Recent = 8,
    SendTo = 9,
    StartMenu = 11,
    MyMusic = 13,
    MyVideos = 14,
    DesktopDirectory = 16,
    MyComputer = 17,
    NetworkShortcuts = 19,
    Fonts = 20,
    Templates = 21,
    CommonStartMenu = 22,
    CommonPrograms = 23,
    CommonStartup = 24,
    CommonDesktopDirectory = 25,
    ApplicationData = 26,
    PrinterShortcuts = 27,
    LocalApplicationData = 28,
    InternetCache = 32,
    Cookies = 33,
    History = 34,
    CommonApplicationData = 35,
    Windows = 36,  // %windir%, %SYSTEMROOT%, C:\Windows に相当
    System = 37,
    ProgramFiles = 38,
    MyPictures = 39,
    UserProfile = 40,
    SystemX86 = 41,
    ProgramFilesX86 = 42,
    CommonProgramFiles = 43,
    CommonProgramFilesX86 = 44,
    CommonTemplates = 45,
    CommonDocuments = 46,
    CommonAdminTools = 47,
    AdminTools = 48,
    CommonMusic = 53,
    CommonPictures = 54,
    CommonVideos = 55,
    Resources = 56,
    LocalizedResources = 57,
    CommonOemLinks = 58,
    CDBurning = 59,
}
 */