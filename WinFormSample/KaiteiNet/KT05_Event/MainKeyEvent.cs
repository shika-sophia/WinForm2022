/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT05_Event /
 *@class MainKeyEvent.cs
 *@class FormKeyEventSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2010
 *           http://kaitei.net/csforms/events/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm05_Event.txt〕
 *           
 *@content KT 5. Event / KeyEvent
 *@subject ◆KeyEvent
 *         KeyEventHandler      control.KeyDown   keyを押す
 *         KeyEventHandler      control.KeyUp     keyを離す
 *         KeyPressEventHandler control.KeyPress  文字入力
 *         
 *         ＊EventHandler
 *         delegate void KeyEventHandler(object sender, KeyEventArgs e)
 *         delegate void KeyPressEventHandler(object sender, KeyPressEventArgs e)
 *         
 *         ＊KeyEventArgs e 
 *         [×] 文字入力に不適, [〇] 特殊keyの検出に適す
 *         Keys e.KeyCode   keyの種類 enum Keys {  }〔下記〕
 *         bool e.Alt       [Alt]
 *         bool e.Control   [Ctrl]
 *         bool e.Shift     [Shift]
 *         
 *         ＊KeyPressEventArgs e
 *         [〇] 文字入力に適す, [×] 特殊keyの検出に不適
 *         char e.KeyChar
 *         
 *         ※特殊keyの検出には要キャスト
 *         if (e.KeyChar == (char) Keys.Enter)
 *         
 *@NOTE if(e.KeyCode == Keys.Shift)とすると反応せず、
 *      if(e.Shift) を利用すると解決。
 *      
 *@see FormKeyEvent.jpg
 *@author shika
 *@date 2022-06-26
 */
using System;
using System.Text;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT05_Event
{
    class MainKeyEvent
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormKeyEventSample());
        }//Main()
    }//class

    class FormKeyEventSample : Form
    {
        public FormKeyEventSample()
        {
            this.Text = "FormKeyEventSample";
            this.AutoScroll = true;
            Label label = new Label()
            {
                Text = "[Shift] + [Esc]: 終了",
                AutoSize = true
            };
            this.Controls.Add(label);

            this.KeyDown += new KeyEventHandler(form_KeyShiftEsc);
        }//constructor
        
        private void form_KeyShiftEsc(object sender, KeyEventArgs e)
        {
            if(e.Shift && e.KeyCode == Keys.Escape)
            {
                DialogResult result = MessageBox.Show(
                    "Close OK?",
                    "Confirm Close",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                if(result == DialogResult.OK)
                {
                    this.Close();
                }
            }//if Shift + Esc
        }//form_KeyShiftEsc()
    }//class
}

/*
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
    Menu = 18,       //[Alt]
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
    Escape = 27,   //[Esc]
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
    D0 = 48,   //数字 0
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
