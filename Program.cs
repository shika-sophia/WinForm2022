/**
 *@title WinFormGUI / Program.cs
 *@content デフォルト Main()
 */

/// <summary>
/// アプリケーションのメイン エントリ ポイントです。
/// </summary>

/// エントリーポイントを自己定義する場合は 
/// static class, [STAThread],static Main()をコメントアウトし、
/// public Main()に変更

using System;
using System.Windows.Forms;

namespace WinFormGUI
{
    //static class Program
    class Program
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }//Main()
    }//class
}
