/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT11_MenuOld
 *@class MainMenuSample.cs
 *@class FormMenuSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/menus/ 
 *           =>〔~/Reference/Article_KaiteiNet/WinForm11_MenuOld.txt〕
 *           
 *@content KT11 MenuOld
 *@subject ◆MainMenu : Menu  /  Menu : Component
 *         MainMenu  new MainMenu()
 *         MainMenu  new MainMenu(IContainer)
 *         MainMenu  new MainMenu(MenuItem[])
 *         
 *         Form      mainMenu.GetForm()   格納されている上位のFormオブジェクト
 *         MenuItemCollection   menu.MenuItems
 *         int       menu.MenuItems.Add(MenuItem item);
 *         void      menu.MenuItems.AddRange(MenuItem[] items);
 *         int       menu.MenuItems.Add(int index, MenuItem item)
 *         MenuItem  menu.MenuItems.Add(string caption);
 *         MenuItem  menu.MenuItems.Add(string caption, MenuItem[] items);
 *         MenuItem  menu.MenuItems.Add(string caption, EventHandler onClick);
 *         MenuItem[] menu.MenuItems.Find(string key, bool searchAllChildren);
 *         void       menu.MenuItems.Clear() / Remove()他 IList, ICollection系
 *         
 *@subject MenuItem : Menu
 *         MenuItem  new MenuItem()
 *         MenuItem  new MenuItem(string text)
 *         MenuItem  new MenuItem(string text, MenuItem[])
 *         MenuItem  new MenuItem(string text, EventHandler onClick,[ Shortcut ])
 *         
 *         string   menuItem.Text
 *         Shortcut menuItem.Shortcut
 *           └ enum Shortcut { } 〔文末〕
 *         bool     menuItem.ShowShortcut
 *           
 *         MenuItemCollection   menu.MenuItems
 *         ( Add(), AddRange() など  同上)
 *         
 *         EventHandler menuItem.Click
 *         EventHandler menuItem.Select
 *
 *@subject MenuItem("-")   特殊な MenuItem
 *         Menuドロップダウン内に 区切り横線を表示
 *         
 *@subject Shortcut Key (子MenuItemのみ機能する)
 *         ・Menuの Text内に「&A」 -> 表示は「A」のみ
 *           ShortcutKeyとして機能する
 *           
 *         ・menuItem.Shortcut を指定すると
 *           Menu項目の右側に「Ctrl + N」「Alt + Shift + A」などと表示される
 *         
 *@subject Menuの配置
 *         MainMenu  form.Menu                  Formのプロパティ
 *           └ MainMenu                         MainMenuインスタンス
 *               └ mainMenu.MenuItems.Add()     親MenuItem Formに表示される
 *                  └ menuItem.MenuItems.Add()  子MenuItem ドロップダウン
 *                      :
 *         
 *@NOTE    MainMenu の親MenuItemは、Shortcutが機能していない
 *         (ここでは「ファイル(F)」 Formから直接見える部分)
 *         
 *@see FormMenuSample.jpg
 *@author shika
 *@date 2022-07-22
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT11_MenuOld
{
    class MainMenuSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormMenuSample());
        }//Main()
    }//class

    class FormMenuSample : Form
    {
        private MainMenu menu;

        public FormMenuSample()
        {
            this.Text = "FormMenuSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.AutoSize = true;

            menu = new MainMenu();
            //---- Parent ---
            MenuItem menuFile = new MenuItem("ファイル (&F)");

            //---- Children ----
            MenuItem menuNew = new MenuItem("新規 (&N)") 
            { 
                Shortcut = Shortcut.CtrlN,
            };            
            MenuItem menuBar = new MenuItem("-");
            MenuItem menuExit = new MenuItem("終了 (&X)")
            {
                Shortcut = Shortcut.Alt4,
            };

            //---- Event ----
            menuNew.Click += new EventHandler(menuNew_Click);
            menuExit.Click += new EventHandler(menuExit_Click);

            //---- Deployment ----
            menu.MenuItems.Add(menuFile);
            menuFile.MenuItems.AddRange(new MenuItem[]
            {
                menuNew, menuBar, menuExit,
            });

            this.Menu = menu;
        }//constructor

        private void menuNew_Click(object sender, EventArgs e)
        {
            new FormMenuSample().Show();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }//class
}

/*
//---- enum Shortcut ----
enum Shortcut
{
    None = 0,
    Ins = 45,
    Del = 46,
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
    ShiftIns = 65581,
    ShiftDel = 65582,
    ShiftF1 = 65648,
    ShiftF2 = 65649,
    ShiftF3 = 65650,
    ShiftF4 = 65651,
    ShiftF5 = 65652,
    ShiftF6 = 65653,
    ShiftF7 = 65654,
    ShiftF8 = 65655,
    ShiftF9 = 65656,
    ShiftF10 = 65657,
    ShiftF11 = 65658,
    ShiftF12 = 65659,
    CtrlIns = 131117,
    CtrlDel = 131118,
    Ctrl0 = 131120,
    Ctrl1 = 131121,
    Ctrl2 = 131122,
    Ctrl3 = 131123,
    Ctrl4 = 131124,
    Ctrl5 = 131125,
    Ctrl6 = 131126,
    Ctrl7 = 131127,
    Ctrl8 = 131128,
    Ctrl9 = 131129,
    CtrlA = 131137,
    CtrlB = 131138,
    CtrlC = 131139,
    CtrlD = 131140,
    CtrlE = 131141,
    CtrlF = 131142,
    CtrlG = 131143,
    CtrlH = 131144,
    CtrlI = 131145,
    CtrlJ = 131146,
    CtrlK = 131147,
    CtrlL = 131148,
    CtrlM = 131149,
    CtrlN = 131150,
    CtrlO = 131151,
    CtrlP = 131152,
    CtrlQ = 131153,
    CtrlR = 131154,
    CtrlS = 131155,
    CtrlT = 131156,
    CtrlU = 131157,
    CtrlV = 131158,
    CtrlW = 131159,
    CtrlX = 131160,
    CtrlY = 131161,
    CtrlZ = 131162,
    CtrlF1 = 131184,
    CtrlF2 = 131185,
    CtrlF3 = 131186,
    CtrlF4 = 131187,
    CtrlF5 = 131188,
    CtrlF6 = 131189,
    CtrlF7 = 131190,
    CtrlF8 = 131191,
    CtrlF9 = 131192,
    CtrlF10 = 131193,
    CtrlF11 = 131194,
    CtrlF12 = 131195,
    CtrlShift0 = 196656,
    CtrlShift1 = 196657,
    CtrlShift2 = 196658,
    CtrlShift3 = 196659,
    CtrlShift4 = 196660,
    CtrlShift5 = 196661,
    CtrlShift6 = 196662,
    CtrlShift7 = 196663,
    CtrlShift8 = 196664,
    CtrlShift9 = 196665,
    CtrlShiftA = 196673,
    CtrlShiftB = 196674,
    CtrlShiftC = 196675,
    CtrlShiftD = 196676,
    CtrlShiftE = 196677,
    CtrlShiftF = 196678,
    CtrlShiftG = 196679,
    CtrlShiftH = 196680,
    CtrlShiftI = 196681,
    CtrlShiftJ = 196682,
    CtrlShiftK = 196683,
    CtrlShiftL = 196684,
    CtrlShiftM = 196685,
    CtrlShiftN = 196686,
    CtrlShiftO = 196687,
    CtrlShiftP = 196688,
    CtrlShiftQ = 196689,
    CtrlShiftR = 196690,
    CtrlShiftS = 196691,
    CtrlShiftT = 196692,
    CtrlShiftU = 196693,
    CtrlShiftV = 196694,
    CtrlShiftW = 196695,
    CtrlShiftX = 196696,
    CtrlShiftY = 196697,
    CtrlShiftZ = 196698,
    CtrlShiftF1 = 196720,
    CtrlShiftF2 = 196721,
    CtrlShiftF3 = 196722,
    CtrlShiftF4 = 196723,
    CtrlShiftF5 = 196724,
    CtrlShiftF6 = 196725,
    CtrlShiftF7 = 196726,
    CtrlShiftF8 = 196727,
    CtrlShiftF9 = 196728,
    CtrlShiftF10 = 196729,
    CtrlShiftF11 = 196730,
    CtrlShiftF12 = 196731,
    AltBksp = 262152,
    AltLeftArrow = 262181,
    AltUpArrow = 262182,
    AltRightArrow = 262183,
    AltDownArrow = 262184,
    Alt0 = 262192,
    Alt1 = 262193,
    Alt2 = 262194,
    Alt3 = 262195,
    Alt4 = 262196,
    Alt5 = 262197,
    Alt6 = 262198,
    Alt7 = 262199,
    Alt8 = 262200,
    Alt9 = 262201,
    AltF1 = 262256,
    AltF2 = 262257,
    AltF3 = 262258,
    AltF4 = 262259,
    AltF5 = 262260,
    AltF6 = 262261,
    AltF7 = 262262,
    AltF8 = 262263,
    AltF9 = 262264,
    AltF10 = 262265,
    AltF11 = 262266,
    AltF12 = 262267,
}

 */