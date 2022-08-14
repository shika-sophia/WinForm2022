/** 
 *@title WinFormGUI / WinFormSample / KaiteiNet / KT14_Localization
 *@class MainCultureSample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *@reference KT ナガノ  『Windows Form C#』KaiteiNet, 2018
 *           http://kaitei.net/csforms/localization/
 *           =>〔~/Reference/Article_KaiteiNet/WinForm14_Localization.txt〕
 *           
 *@content KT14 Localization | [C#] Culture / [Java] Locale
 *
 *@subject Threadクラス -- System.Threading.
 *         Thread         Thread.CurrentThread     このコードを現在実行している Thread
 *         CultureInfo    Thread.CurrentCulture   〔下記〕
 *         CultureInfo    Thread.CurrentUICulture 〔下記〕
 *         
 *@subject ◆CultureInfo : ICloneable, IFormatProvider
 *                      -- System.Globalization.
 *         CultureInfo    new CultureInfo(string name)
 *         CultureInfo    new CultureInfo(string name, [bool useUserOverride])
 *         
 *         CultureInfo  CultureInfo.CurrentCulture     { get; set; } static カルチャー情報
 *                         ・現在Threadで使用する CultureInfoオブジェクト
 *                         ・日時、数値、通貨
 *         CultureInfo  CultureInfo.CurrentUICulture   { get; set; } static UIカルチャー情報
 *                         ・現在Threadでカルチャー固有のリソースを参照する際に利用
 *                         ・「Resources.ja-JP.resx」「Resources.en-US.resx」などの参照
 *                         ・Exception時の Message言語 (※言語パックが未インストール時は 英語で表示)
 *                         
 *         string    cultureInfo.Name         { get; } "ja-JP", "en-US"など ISO 639-1で定義された「言語 - 国 / 地域」
 *         string    cultureInfo.EnglishName  { get; } 英語名 "Japanese (Japan)"など
 *         string    cultureInfo.NativeName   { get; } カルチャーの表示設定でのカルチャー名
 *         string    cultureInfo.DisplayName  { get; } ローカライズされた(= デフォルトCulture) カルチャー名
 *         bool      cultureInfo.UseUserOverride  { get; }  true: ユーザー定義 / false: システム既定の定義
 *         CultureTypes  cultureInfo.CultureTypes { get; }  カルチャーの種類
 *           └ enum CultureTypes
 *             {
 *                 NeutralCultures = 1,        // 言語に関連付けらているが、国/地域に固有ではないカルチャ
 *                 SpecificCultures = 2,       // 国/地域に固有のカルチャ
 *                 InstalledWin32Cultures = 4, // Windows OSにインストールされている全てのカルチャー
 *                 AllCultures = 7,            // .NET によって認識できる全てのカルチャー
 *                 UserCustomCulture = 8,      // (非推奨) ユーザー定義のカルチャー
 *                 ReplacementCultures = 16,   // (非推奨) .NET Framework のカルチャーを ユーザー定義で置換したカルチャー
 *                 WindowsOnlyCultures = 32,   // (非推奨) 無視される
 *                 FrameworkCultures = 64,     // (非推奨) .NET Framework 2.0 のカルチャー
 *             }
 *         
 *         CultureInfo    CultureInfo.CreateSpecificCulture(string name)  指定した名前で関連付けられている特定のカルチャ
 *         CultureInfo    CultureInfo.GetCultureInfo(string name);        読取専用 CultureInfo
 *         CultureInfo[]  CultureInfo.GetCultures(CultureTypes types)     上記 CutureTypesで利用できる すべての CultureInfo配列
 *          
 *@author shika
 *@date 2022-08-13
 */
using System;
using System.Globalization;
using System.Threading;

namespace WinFormGUI.WinFormSample.KaiteiNet.KT14_Localization
{
    class MainCultureSample
    {
        private readonly Thread currentTh = Thread.CurrentThread;

        //static void Main()
        public void Main()
        {
            var here = new MainCultureSample();

            //---- Default Culture ----          
            here.OutputSample();
            Console.WriteLine();

            //---- Changed Culture ----
            here.currentTh.CurrentCulture = new CultureInfo("de-DE");
            here.currentTh.CurrentUICulture = new CultureInfo("fr-FR");
          
            here.OutputSample();
        }//Main()

        private void OutputSample()
        {
            Console.WriteLine("CurrentCulture: {0}",
                currentTh.CurrentCulture.Name);
            Console.WriteLine("CurrentUICulture: {0}",
                currentTh.CurrentUICulture.Name);
            Console.WriteLine("Today: {0}",
                DateTime.Now.ToLongDateString());

            try
            {
                int zero = 0;
                zero /= zero;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"{exc.GetType()}:\n   {exc.Message}");
            }
        }//OutputSample()
    }//class
}

/*
//==== Result ====
//---- Default Culture ---- 
CurrentCulture: ja-JP
CurrentUICulture: ja-JP
Today: 2022年8月13日                             <-「ja-JP」日本語表記
System.DivideByZeroException:
   0 で除算しようとしました。                      <-「ja-JP」日本語表記

//---- Changed Culture ----
CurrentCulture: en-US
CurrentUICulture: de-DE
Today: Saturday, August 13, 2022                 <-「en-US」英語表記
System.DivideByZeroException:
   Es wurde versucht, durch 0 (null) zu teilen.  <-「de-DE」独語表記

//---- Changed Culture ----
CurrentCulture: fr-FR
CurrentUICulture: de-DE
Today: samedi 13 aout 2022                       <-「fr-FR」仏語表記
System.DivideByZeroException:
   Es wurde versucht, durch 0 (null) zu teilen.

CurrentCulture: de-DE
CurrentUICulture: fr-FR
Today: Samstag, 13. August 2022                  <-「de-DE」独語表記
System.DivideByZeroException:
   Attempted to divide by zero.                  <-「fr-FR」仏語表記を指定したが
                                                    言語パックを未インストールのため、英語表記
*/