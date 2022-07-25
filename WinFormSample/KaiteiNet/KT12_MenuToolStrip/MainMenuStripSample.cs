/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT12_MenuToolStrip
 *@class MainMenuStripSample.cs
 *@class FormMenuStripSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menu-strips/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm12_MenuToolStrip.txt〕
 *           
 *@content KT12 MenuToolStrip / MenuStrip | RR[81] -[84] p157-161
 *
 *@subject Inherit 継承関係
 *
 *         ScrollableControl
 *           └ ToolStrip                     =>〔MainToolStripSample.cs〕
 *               └ MenuStrip                 =>〔this〕
 *               └ ToolStripDropDown
 *               |   └ ToolStripDropDownMenu
 *               |       └ ContextMenuStrip  =>〔MainContextMenuStripSample.cs〕
 *               └ StatusStrip               =>〔MainStatusStripSample.cs〕
 *               └ ToolStripSeparator        
 *           
 *         Component
 *           └ ToolStripItem
 *              └ ToolStripItemDropDown
 *              |   └ ToolStripMenuItem
 *              |   └ ToolStripDropDownButton =>〔MainStatusStripSample.cs〕
 *              |   └ ToolStripSplitButton    =>〔MainStatusStripSample.cs〕
 *              |
 *              └ ToolStripLabel
 *              |   └ ToolStripStatusLabel    =>〔MainStatusStripSample.cs〕
 *              |
 *              └ ToolStripControlHost
 *                  └ ToolStripProgressBar    =>〔MainStatusStripSample.cs〕
 *                  └ ToolStripTextBox
 *                  └ ToolStripComboBox
 */
#region ◆ToolStrip Reference
/*
 *@subject ◆ToolStrip : ScrollableControl, IArrangedElement, IComponent, IDisposable
 *         ToolStrip   new ToolStrip();
 *         ToolStrip   new ToolStrip(params ToolStripItem[] items);
 *         
 *         ImageList                  control.ImageList
 *         Size                       toolStrip.ImageScalingSize  イメージサイズ px単位 /デフォルト値 16×16
 *         ToolStripItemCollection    toolStrip.Items
 *         ToolStripItemCollection    toolStrip.DisplayedItems  現在表示されている項目のコレクション
 *                                      :  Add(), AddRange() 他 IList, ICollection系 メソッド
 *         
 *         ToolStripDropDownDirection toolStrip.DefaultDropDownDirection
 *           └ enum ToolStripDropDownDirection
 *             {
 *                 AboveLeft = 0,  // マウスの位置を使用して、親コントロールの左上
 *                 AboveRight = 1, // マウスの位置を使用して、親コントロールの右上
 *                 BelowLeft = 2,  // マウスの位置を使用して、親コントロールの左下
 *                 BelowRight = 3, // マウスの位置を使用して、親コントロールの右下
 *                 Left = 4,       //親コントロールの左側
 *                 Right = 5,      //親コントロールの右側
 *                 Default = 7     //RightToLeft の設定に応じて、Left | Right
 *             }
 *             
 *         ToolStripTextDirection  toolStrip.TextDirection  テキストの向き
 *           └ enum ToolStripTextDirection
 *             {
 *                 Inherit = 0,     //親コントロールから継承
 *                 Horizontal = 1,  //水平方向
 *                 Vertical90 = 2,  //テキストを 90 度回転
 *                 Vertical270 = 3  //テキストを 270 度回転
 *             }
 *             
 *         ToolStripLayoutStyle    toolStrip.LayoutStyle  
 *           └ enum ToolStripLayoutStyle
 *             {
 *                 StackWithOverflow = 0,           //自動的にレイアウト
 *                 HorizontalStackWithOverflow = 1, //水平方向にレイアウトし、必要に応じてオーバーフロー(=折り返し)
 *                 VerticalStackWithOverflow = 2,   //垂直方向にレイアウトし、コントロール内で中央揃えで表示し、必要に応じてオーバーフロー
 *                 Flow = 3,                        //必要に応じて水平方向または垂直方向にフロー
 *                 Table = 4                        //項目を左寄せでレイアウト
 *             }
 *   
 *         ToolStripGripStyle      toolStrip.GripStyle
 *           └ enum ToolStripGripStyle
 *             {
 *                 Hidden = 0,  //移動ハンドル (グリップ) を表示しない
 *                 Visible = 1  //移動ハンドル (グリップ) を表示する / デフォルト値
 *             }
 *             
 *         ToolStripGripDisplayStyle toolStrip.GripDisplayStyle 移動ハンドル(グリップ)の方向 / getのみ
 *           └ enum ToolStripGripDisplayStyle
 *             {
 *                 Horizontal = 0,
 *                 Vertical = 1
 *             }
 *             
 *         ＊Event
 *         ToolStripItemClickedEventHandler
 *                       toolStrip.ItemClicked   ToolStripクラスのクリック時イベント
 *                                               (全項目を一括で１つのイベントハンドラーで処理できる)
 *         ToolStripItemClickedEventArgs e
 *         ToolStripItem     e.ClickedItem
 *         
 *@subject ◆MenuStrip : ToolStrip
 *         MenuStrip   new MenuStrip()
 *         (主なメンバーは ToolStripで定義済)
 *         
 *@subject ◆ToolStripItem : Component, IDropTarget, IArrangedElement, IComponent, IDisposable
 *         ToolStripItem   new ToolStripItem();
 *         ToolStripItem   new ToolStripItem(string text, Image image, EventHandler onClick);
 *         
 *         bool      toolStrip.Enabled    グレーで表示。選択はできない。
 *         string    toolStripItem.Text
 *         Image     toolStripItem.Image       アイコン
 *         ImageList control.ImageList
 *         int       toolStripItem.ImageIndex  ImageList.Images[i]のindex
 *         string    toolStripItem.ImageKey
 *         
 *         ToolStripItem toolStripItem.OwnerItem
 *         ToolStrip     toolStripItem.Owner     = ToolStrip toolStripItem.Parent
 *         
 *         ToolStripItemAlignment  toolStripItem.Alignment
 *           └ enum ToolStripItemAlignment
 *           {
 *               Left = 0,
 *               Right = 1,
 *           }
 *           
 *         TextImageRelation toolStripItem.TextImageRelation
 *           └ enum TextImageRelation
 *             {
 *                  Overlay = 0,        //イメージとテキストがコントロール上で同じスペースを共有
 *                  ImageAboveText = 1, //イメージがコントロールのテキストの上部に表示
 *                  TextAboveImage = 2, //テキストがコントロールのイメージの上部に表示
 *                  ImageBeforeText = 4,//イメージがコントロールのテキストの左側に表示
 *                  TextBeforeImage = 8 //テキストがコントロールのイメージの左側に表示
 *              }
 *              
 *         ToolStripItemPlacement Placement
 *           └ enum ToolStripItemPlacement
 *             {
 *                 Main = 0,      //メインToolStrip 上にレイアウト
 *                 Overflow = 1,  //オーバーフローToolStrip にレイアウト
 *                 None = 2       //画面上にレイアウトしない
 *             }
 *             
 *         ToolStripTextDirection TextDirection
 *           └〔上記 ToolStrip〕
 *             
 *         ＊Event
 *         EventHandler  toolStripItem.Click     ToolStripItemクラスのクリック時イベント
 *                                               (項目ごとにイベントハンドラーを追加する必要がある)
 *         ToolStripItem     (ToolStripItem) sender
 *         
 *@subject ◆ToolStripDropDownItem : ToolStripItem
 *         ToolStripDropDownItem  new ToolStripDropDownItem()
 *         ToolStripDropDownItem  new ToolStripDropDownItem(string text, Image, EventHandler onClick);
 *         ToolStripDropDownItem  new ToolStripDropDownItem(string text, Image, params ToolStripItem[]);
 *         
 *         ToolStripItemCollection toolStripDropDownItem.DropDownItems
 *                                    :  Add(), AddRange(), 他 IList, ICollection系 メソッド
 *                                    
 *         ToolStripDropDownDirection  toolStripDropDownItem.DropDownDirection
 *           └〔上記 ToolStrip〕
 *         
 *         void   toolStripDropDownItem.HideDropDown();
 *         void   toolStripDropDownItem.ShowDropDown();
 *         
 *         EventHandler toolStripDropDownItem.DropDownOpening;
 *         EventHandler toolStripDropDownItem.DropDownOpened;
 *         EventHandler toolStripDropDownItem.DropDownClosed;
 *         
 *@subject ◆ToolStripMenuItem : ToolStripDropDownItem
 *         ToolStripMenuItem   new ToolStripMenuItem()
 *         ToolStripMenuItem   new ToolStripMenuItem(string text);
 *         ToolStripMenuItem   new ToolStripMenuItem(Image image);
 *         ToolStripMenuItem   new ToolStripMenuItem(string text, Image, EventHandler onClick);
 *         ToolStripMenuItem   new ToolStripMenuItem(
 *                                   string text,
 *                                   Image image,
 *                                   EventHandler onClick,
 *                                   Keys shortcutKeys);
 *
 *         bool   toolStripMenuItem.ShowShortcutKeys
 *         Keys   toolStripMenuItem.ShortcutKeys
 *           └ enum Keys { }   〔文末〕
 *         string toolStripMenuItem.ShortcutKeyDisplayString
 *         
 *         bool   toolStripMenuItem.Checked         チェックされているか。中間は true / デフォルト false
 *         bool   toolStripMenuItem.CheckOnClick    自動的にチェック状態で表示、クリックでチェック解除されるか / デフォルト false
 *         CheckState toolStripMenuItem.CheckState  既定値は、Unchecked
 *           └ enum CheckState
 *             {
 *                 Unchecked = 0,
 *                 Checked = 1,
 *                 Indeterminate = 2
 *             }
 *         
 *@subject ToolStripSeparator : ToolStripItem
 *         ToolStripSeparator   new ToolStripSeparator()
 *
 *@subject Shotcut Key
 *         ・Textに「&A」-> 表示「A」-> Shortcut Keyを登録
 *         ・親ToolStripMenuItemは 「&F」-> 表示「F」 -> [Alt] + [F]で機能する
 *         ・子ToolStripMenuItemは 「&A」-> 表示「A」と
 *           Keys   menuItem.ShortcutKeys  メニュー項目の後ろに表示。Shortcut Keyを登録。
 *           
 *@NOTE 親ToolStripMenuItemに Keysを利用すると例外発生
 *      System.ComponentModel.InvalidEnumArgumentException:
 *      引数値 'value' は列挙型 'Keys' に対して無効です。
 *      パラメーター名: value
 *      場所 System.Windows.Forms.ToolStripMenuItem.set_ShortcutKeys(Keys value)
 *      
 *      => menuFile.ShortcutKeys = Keys.F; を削除すると解決
 *               
 *@subject Deployment 配置
 *        (MainStrip Form.MainStripMenu  記述しなくても機能する)
 *         Control.ControlCollection
 *                   Form.Controls.Add(mainStrip)
 *           └ MainStrip
 *              └ mainStrip.Items.Add()                   親ToolStripMenuItem
 *                └ toolStripMenuItem.DropDownItems.Add() 子ToolStripMenuItem
 */
#endregion
/*
 *@see FormMenuStripSample.jpg
 *@author shika
 *@date 2022-07-24
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT12_MenuToolStrip
{
    class MainMenuStripSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormMenuStripSample());
        }//Main()
    }//class

    class FormMenuStripSample : Form
    {
        private MenuStrip menu;
        
        public FormMenuStripSample()
        {
            this.Text = "FormMenuStripSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            //---- Menu Children ----
            var menuNew = new ToolStripMenuItem()
            {
                Text = "New File (&N)",
                ShortcutKeys = Keys.Control | Keys.F,
            };
            menuNew.Click += new EventHandler(menuNew_Click);

            var menuExit = new ToolStripMenuItem()
            {
                Text = "Close (&X)",
                ShortcutKeys = Keys.Alt | Keys.F4,
            };
            menuExit.Click += new EventHandler(menuExit_Click);

            //---- Menu Parent ----
            var menuFile = new ToolStripMenuItem("File (&F)");
            menuFile.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuNew, new ToolStripSeparator(), menuExit,
            });

            //---- Menu ----
            menu = new MenuStrip();            
            menu.Items.Add(menuFile);

            //---- Form ----
            this.Controls.Add(menu);
        }//constructor

        private void menuNew_Click(object sender, EventArgs e)
        {
            new FormMenuStripSample().Show();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }//class
}

/*
//---- enum Keys ----
enum Keys
{
    None = 0,
    LButton = 1,
    RButton = 2,
    Cancel = 3,
    MButton = 4,
    XButton1 = 5,
    XButton2 = 6,
    Back = 8,
    Tab = 9,
    LineFeed = 10,
    Clear = 12,
    Return = 13,
    Return = 13,
    ShiftKey = 16,
    ControlKey = 17,
    Menu = 18,
    Pause = 19,
    Capital = 20,
    Capital = 20,
    KanaMode = 21,
    KanaMode = 21,
    KanaMode = 21,
    JunjaMode = 23,
    FinalMode = 24,
    HanjaMode = 25,
    HanjaMode = 25,
    Escape = 27,
    IMEConvert = 28,
    IMENonconvert = 29,
    IMEAceept = 30,
    IMEAceept = 30,
    IMEModeChange = 31,
    Space = 32,
    PageUp = 33,
    PageUp = 33,
    Next = 34,
    Next = 34,
    End = 35,
    Home = 36,
    Left = 37,
    Up = 38,
    Right = 39,
    Down = 40,
    Select = 41,
    Print = 42,
    Execute = 43,
    PrintScreen = 44,
    PrintScreen = 44,
    Insert = 45,
    Delete = 46,
    Help = 47,
    D0 = 48,
    D1 = 49,
    D2 = 50,
    D3 = 51,
    D4 = 52,
    D5 = 53,
    D6 = 54,
    D7 = 55,
    D8 = 56,
    D9 = 57,
    A = 65,
    B = 66,
    C = 67,
    D = 68,
    E = 69,
    F = 70,
    G = 71,
    H = 72,
    I = 73,
    J = 74,
    K = 75,
    L = 76,
    M = 77,
    N = 78,
    O = 79,
    P = 80,
    Q = 81,
    R = 82,
    S = 83,
    T = 84,
    U = 85,
    V = 86,
    W = 87,
    X = 88,
    Y = 89,
    Z = 90,
    LWin = 91,
    RWin = 92,
    Apps = 93,
    Sleep = 95,
    NumPad0 = 96,
    NumPad1 = 97,
    NumPad2 = 98,
    NumPad3 = 99,
    NumPad4 = 100,
    NumPad5 = 101,
    NumPad6 = 102,
    NumPad7 = 103,
    NumPad8 = 104,
    NumPad9 = 105,
    Multiply = 106,
    Add = 107,
    Separator = 108,
    Subtract = 109,
    Decimal = 110,
    Divide = 111,
    F1 = 112,
    F2 = 113,
    F3 = 114,
    F4 = 115,
    F5 = 116,
    F6 = 117,
    F7 = 118,
    F8 = 119,
    F9 = 120,
    F10 = 121,
    F11 = 122,
    F12 = 123,
    F13 = 124,
    F14 = 125,
    F15 = 126,
    F16 = 127,
    F17 = 128,
    F18 = 129,
    F19 = 130,
    F20 = 131,
    F21 = 132,
    F22 = 133,
    F23 = 134,
    F24 = 135,
    NumLock = 144,
    Scroll = 145,
    LShiftKey = 160,
    RShiftKey = 161,
    LControlKey = 162,
    RControlKey = 163,
    LMenu = 164,
    RMenu = 165,
    BrowserBack = 166,
    BrowserForward = 167,
    BrowserRefresh = 168,
    BrowserStop = 169,
    BrowserSearch = 170,
    BrowserFavorites = 171,
    BrowserHome = 172,
    VolumeMute = 173,
    VolumeDown = 174,
    VolumeUp = 175,
    MediaNextTrack = 176,
    MediaPreviousTrack = 177,
    MediaStop = 178,
    MediaPlayPause = 179,
    LaunchMail = 180,
    SelectMedia = 181,
    LaunchApplication1 = 182,
    LaunchApplication2 = 183,
    Oem1 = 186,
    Oem1 = 186,
    Oemplus = 187,
    Oemcomma = 188,
    OemMinus = 189,
    OemPeriod = 190,
    OemQuestion = 191,
    OemQuestion = 191,
    Oemtilde = 192,
    Oemtilde = 192,
    OemOpenBrackets = 219,
    OemOpenBrackets = 219,
    Oem5 = 220,
    Oem5 = 220,
    Oem6 = 221,
    Oem6 = 221,
    Oem7 = 222,
    Oem7 = 222,
    Oem8 = 223,
    OemBackslash = 226,
    OemBackslash = 226,
    ProcessKey = 229,
    Packet = 231,
    Attn = 246,
    Crsel = 247,
    Exsel = 248,
    EraseEof = 249,
    Play = 250,
    Zoom = 251,
    NoName = 252,
    Pa1 = 253,
    OemClear = 254,
    KeyCode = 65535,
    Shift = 65536,
    Control = 131072,
    Alt = 262144,
    Modifiers = -65536,
}
 */