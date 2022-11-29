/** 
 *@title WinFormGUI / WinFormSample / ReverseReference.RR18_Excel
 *@class MainExcelWorkbookOpenSample.cs
 *@class   └ new FormExcelWorkbookOpenSample() : Form
 *@class      └ new Excel.Application() : _Application, AppEvents_Event
 *@using Excel = Microsoft.Office.Interop.Excel
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[484][485][491] p818 / Excel ファイルを Open(), Save()
 *         Microsoft Excel を C#から操作するクラス群
 *         
 *@prepare【Addition Referece Library / 参照追加】
 *         ◆Microsoft Excel 16.0 Object Library
 *         ＊Terms:
 *         Project -> [参照] 右クリック -> [参照の追加]　->
 *         [Refernce Manager] COM -- タイプ ライブラリ -- 
 *         -> [Microsoft Excel 16.0 Object Library] Check -> OK
 *         (16.0: Excel2019 の Edition -- Version 1.9: Object Libraryの version?)
 *
 *@subject Use Alias / 別名の定義
 *         ・何回も「Microsoft.Office.Interop.Excel」と表記する必要はがあるが、
 *           Alias(別名)を定義することでシンプルに表記
 *         
 *         [Example] using欄
 *         using Excel = Microsoft.Office.Interop.Excel;
 *
 *@subject Connect to Excel / Excelへの接続
 *         [Example]
 *         Excel.Application excelApp = new Excel.Application();
 *           └ interface Application : _Application, AppEvents_Event 〔below〕
 *         
 *@subject Relative Path / 相対パス
 *         ・Excel.Application.Workbooks.Open(string path)
 *             「.xmls」への pathは、「excel.exe」からの相対パス？
 *              絶対pathを指定すれば、pathは通る。
 *              
 *         ・「WinFormGUI.exe」(プロジェクト)からの相対パスのままでは path通らないので、
 *            Path.GetFullPath(string path) で 絶対pathを取得。
 *            
 *         [Example]
 *         Excel.Application excelApp = new Excel.Application();
 *         Excel.Workbook wb = excelApp.Workbooks.Open(
 *            Path.GetFullPath(@"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelFileSample.xlsx"));
 *            
 *@NOTE【註】Move File
 *      [×] VSの機能で Excelファイル移動してはいけない
 *      Excelで保存したディレクトリを変更すると、Excelからファイルを開けなくなる。
 *      VSでも利用し、「編集中のためロック」と表示される。
 *      こうなると、Excel, VSとも open, deleteができなくなる。
 *      => PC Shutdownしてファイルを強制終了して解決
 *      
 *      [×] Don't use File Move Operation of Visual Studio.
 *      If you would change the directory of this file where Excel saved, 
 *      Excel cannot open this file.
 *      If VS would use this file, VS said "Locking this file, due to be editing".
 *      On these cases, both Excel and VS cannot open or delete either.
 *      => 【Solve】by PC Shutdown, this file should close enforcely.
 *
 *@NOTE【註】Save changes certainly / 変更は確実に保存する
 *      Excel変更時、「変更を保存しますか」という Dialog Message が出される。
 *      この時に確実に保存すべき。
 *      さもないと、上記のように Excelの状態は「編集中のためロック」状態になってしまう。
 *     
 *      When Excel changed, Excel ask "Do you save this changes to Excel" by Dialog Message.
 *      You should save certainly, 
 *      or Excel State is to "Locking this file, due to be editing" state, as above.
 *
 *@subject excelApp.Quit()
 *         上記の問題回避のために、プログラムで開いたファイルは必ず閉じる
 *         
 *         using() は IDisposable 継承クラスのみ可能。
 *         Microsoft.Office.Interop.Excel は IDisposableを継承していないので、
 *         try - finally で、excelApp.Quit(); を行う
 *         
 *@subject workbook.Save()  ファイルを保存 
 *         -> [Excel] 「すでに同名のファイルが存在します。上書きしますか？」と出る
 *         -> [OK]
 *         
 *         void  workbook.Save()                     上書き保存
 *         void  workbook.SaveAs(string newFileName) 名前を付けて保存
 *         
 *@subject【註】C#プログラム実行する場合は、
 *         プログラムで利用する 対象Excelファイル[.xlsx]を閉じた状態で実行する。
 *         (Excel画面で 対象Excelファイル[.xlsx]を開いている状態で
 *         プログラム実行しても２回目以降の更新を反映しない)
 *         => 〔MainExcelInteriorColorSample.cs〕
 *         
 */
#region -> interface Microsoft.Office.Interop.Excel.Application
/*
 *@subject ◆Application interface : _Application, AppEvents_Event
 *             -- 
 *         ・空のインターフェイス empty interface
 *         ・Prorerty, abstract Method -> interface _Application    〔below〕
 *         ・Event                     -> interface AppEvents_Event 〔below〕
 *         
 *         public interface Application : _Application, AppEvents_Event
 *         {  }
 *         
 *@subject ◆interface  _Application -- Microsoft.Office.Interop.Excel
 *         ＊Property
 *         string       _Application.Name { get; } 
 *         Names        _Application.Names { get; } 
 *         string       _Application.UserName { get; set; } 
 *         string       _Application.Value { get; } 
 *         string       _Application.Version { get; } 
 *         string       _Application.StandardFont { get; set; } 
 *         double       _Application.StandardFontSize { get; set; } 
 *         bool         _Application.Visible { get; set; } 
 *         bool         _Application.EnableEvents { get; set; } 
 *         double       _Application.Width { get; set; } 
 *         double       _Application.Height { get; set; } 
 *         double       _Application.UsableWidth { get; } 
 *         double       _Application.UsableHeight { get; } 
 *         int          _Application.FormulaBarHeight { get; set; } 
 *         double       _Application.Top { get; set; } 
 *         double       _Application.Left { get; set; } 
 *         double       _Application.MaxChange { get; set; }
 *         Range        _Application.Columns { get; } 
 *         Range        _Application.Rows { get; } 
 *         Range        _Application.Cells { get; } 
 *         XlCreator    _Application.Creator { get; } 
 *         Application  _Application.Application { get; } 
 *         Application  _Application.Parent { get; } 
 *         dynamic      _Application.Selection { get; } 
 *         Windows      _Application.Windows { get; } 
 *         Workbooks    _Application.Workbooks { get; } 
 *         Workbook     _Application.ThisWorkbook { get; } 
 *         Sheets       _Application.Sheets { get; } 
 *         Sheets       _Application.Worksheets { get; } 
 *         Sheets       _Application.DialogSheets { get; } 
 *         Sheets       _Application.Charts { get; } 
 *         Window       _Application.ActiveWindow { get; } 
 *         Workbook     _Application.ActiveWorkbook { get; } 
 *         dynamic      _Application.ActiveSheet { get; } 
 *         Range        _Application.ActiveCell { get; } 
 *         Chart        _Application.ActiveChart { get; } 
 *         DialogSheet  _Application.ActiveDialog { get; } 
 *         string       _Application.ActivePrinter { get; set; } 
 *         WorksheetFunction  _Application.WorksheetFunction { get; } 
 *         Assistant    _Application.Assistant { get; } 
 *         MenuBars     _Application.MenuBars { get; } 
 *         MenuBar      _Application.ActiveMenuBar { get; } 
 *         CommandBars  _Application.CommandBars { get; } 
 *         Toolbars     _Application.Toolbars { get; } 
 *         dynamic      _Application.StatusBar { get; set; } 
 *         Modules      _Application.Modules { get; } 
 *         Sheets       _Application.Excel4MacroSheets { get; } 
 *         Sheets       _Application.Excel4IntlMacroSheets { get; } 
 *         
 *         string       _Application._Default { get; } 
 *         string       _Application.OrganizationName { get; } 
 *         string       _Application.OperatingSystem { get; } 
 *         string       _Application.Path { get; } 
 *         string       _Application.DefaultFilePath { get; set; } 
 *         string       _Application.StartupPath { get; } 
 *         string       _Application.TemplatesPath { get; } 
 *         string       _Application.LibraryPath { get; } 
 *         string       _Application.UserLibraryPath { get; } 
 *         string       _Application.NetworkTemplatesPath { get; } 
 *         string       _Application.PathSeparator { get; } 
 *         string       _Application.AltStartupPath { get; set; } 
 *         string       _Application.Caption { get; set; } 
 *         string       _Application.TransitionMenuKey { get; set; } 
 *         string       _Application.ProductCode { get; } 
 *         string       _Application.DecimalSeparator { get; set; } 
 *         string       _Application.ThousandsSeparator { get; set; } 
 *         string       _Application.ClusterConnector { get; set; } 
 *         string       _Application.OnWindow { get; set; } 
 *         string       _Application.OnSheetActivate { get; set; } 
 *         string       _Application.OnSheetDeactivate { get; set; } 
 *         string       _Application.OnEntry { get; set; } 
 *         string       _Application.OnCalculate { get; set; } 
 *         string       _Application.OnData { get; set; } 
 *         string       _Application.OnDoubleClick { get; set; } 
 *         
 *         int          _Application.Build { get; } 
 *         int          _Application.CustomListCount { get; } 
 *         int          _Application.DataEntryMode { get; set; } 
 *         int          _Application.FixedDecimalPlaces { get; set; } 
 *         int          _Application.MaxIterations { get; set; } 
 *         int          _Application.MemoryFree { get; } 
 *         int          _Application.MemoryTotal { get; } 
 *         int          _Application.MemoryUsed { get; } 
 *         int          _Application.ODBCTimeout { get; set; } 
 *         int          _Application.DDEAppReturnCode { get; } 
 *         int          _Application.SheetsInNewWorkbook { get; set; } 
 *         int          _Application.TransitionMenuKeyAction { get; set; } 
 *         int          _Application.Hwnd { get; } 
 *         int          _Application.Hinstance { get; } 
 *         int          _Application.UILanguage { get; set; } 
 *         int          _Application.DefaultSheetDirection { get; set; } 
 *         int          _Application.CursorMovement { get; set; } 
 *         int          _Application.CalculationVersion { get; } 
 *         int          _Application.LargeOperationCellThousandCount { get; set; } 
 *         int          _Application.ActiveEncryptionSession { get; } 
 *         int          _Application.MeasurementUnit { get; set; }  
 *         
 *         bool         _Application.WindowsForPens { get; } 
 *         bool         _Application.DisplayInfoWindow { get; set; } 
 *         bool         _Application.PivotTableSelection { get; set; } 
 *         bool         _Application.PromptForSummaryInfo { get; set; } 
 *         bool         _Application.RecordRelative { get; } 
 *         bool         _Application.RollZoom { get; set; } 
 *         bool         _Application.ScreenUpdating { get; set; } 
 *         bool         _Application.ShowToolTips { get; set; } 
 *         bool         _Application.ShowChartTipNames { get; set; } 
 *         bool         _Application.ShowChartTipValues { get; set; } 
 *         bool         _Application.DisplayFunctionToolTips { get; set; } 
 *         bool         _Application.TransitionNavigKeys { get; set; } 
 *         bool         _Application.UserControl { get; set; } 
 *         bool         _Application.ControlCharacters { get; set; } 
 *         bool         _Application.ExtendList { get; set; } 
 *         bool         _Application.AutoPercentEntry { get; set; } 
 *         bool         _Application.Ready { get; } 
 *         bool         _Application.ShowWindowsInTaskbar { get; set; } 
 *         bool         _Application.AlertBeforeOverwriting { get; set; } 
 *         bool         _Application.AskToUpdateLinks { get; set; } 
 *         bool         _Application.EnableAnimations { get; set; } 
 *         bool         _Application.CalculateBeforeSave { get; set; } 
 *         bool         _Application.CanPlaySounds { get; } 
 *         bool         _Application.CanRecordSounds { get; } 
 *         bool         _Application.CellDragAndDrop { get; set; } 
 *         bool         _Application.DisplayClipboardWindow { get; set; } 
 *         bool         _Application.ColorButtons { get; set; } 
 *         bool         _Application.ConstrainNumeric { get; set; } 
 *         bool         _Application.CopyObjectsWithCells { get; set; } 
 *         bool         _Application.DisplayAlerts { get; set; } 
 *         bool         _Application.DisplayFullScreen { get; set; } 
 *         bool         _Application.DisplayNoteIndicator { get; set; } 
 *         bool         _Application.DisplayExcel4Menus { get; set; } 
 *         bool         _Application.DisplayRecentFiles { get; set; } 
 *         bool         _Application.DisplayStatusBar { get; set; } 
 *         bool         _Application.DisplayScrollBars { get; set; } 
 *         bool         _Application.DisplayFormulaBar { get; set; } 
 *         bool         _Application.EditDirectlyInCell { get; set; } 
 *         bool         _Application.EnableAutoComplete { get; set; } 
 *         bool         _Application.EnableSound { get; set; } 
 *         bool         _Application.EnableTipWizard { get; set; } 
 *         bool         _Application.FixedDecimal { get; set; } 
 *         bool         _Application.IgnoreRemoteRequests { get; set; } 
 *         bool         _Application.Interactive { get; set; } 
 *         bool         _Application.Iteration { get; set; } 
 *         bool         _Application.LargeButtons { get; set; } 
 *         bool         _Application.MathCoprocessorAvailable { get; } 
 *         bool         _Application.MouseAvailable { get; } 
 *         bool         _Application.MoveAfterReturn { get; set; } 
 *         bool         _Application.DisplayPasteOptions { get; set; } 
 *         bool         _Application.DisplayInsertOptions { get; set; } 
 *         bool         _Application.GenerateGetPivotData { get; set; } 
 *         bool         _Application.AutoFormatAsYouTypeReplaceHyperlinks { get; set; } 
 *         bool         _Application.MapPaperSize { get; set; } 
 *         bool         _Application.ShowStartupDialog { get; set; } 
 *         bool         _Application.UseSystemSeparators { get; set; } 
 *         bool         _Application.DisplayDocumentActionTaskPane { get; set; } 
 *         bool         _Application.ArbitraryXMLSupportAvailable { get; } 
 *         bool         _Application.ShowSelectionFloaties { get; set; } 
 *         bool         _Application.ShowMenuFloaties { get; set; } 
 *         bool         _Application.ShowDevTools { get; set; } 
 *         bool         _Application.EnableLivePreview { get; set; } 
 *         bool         _Application.DisplayDocumentInformationPanel { get; set; } 
 *         bool         _Application.AlwaysUseClearType { get; set; } 
 *         bool         _Application.WarnOnFunctionNameConflict { get; set; } 
 *         bool         _Application.EnableLargeOperationAlert { get; set; } 
 *         bool         _Application.DeferAsyncQueries { get; set; } 
 *         bool         _Application.HighQualityModeForGraphics { get; set; } 
 *         bool         _Application.PrintCommunication { get; set; } 
 *         bool         _Application.UseClusterConnector { get; set; } 
 *         bool         _Application.Quitting { get; } 
 *         bool         _Application.Dummy22 { get; set; } 
 *         bool         _Application.Dummy23 { get; set; } 
 *         bool         _Application.IsSandboxed { get; } 
 *         bool         _Application.SaveISO8601Dates { get; set; } 
 *         bool         _Application.DisplayFormulaAutoComplete { get; set; } 
 *         bool         _Application.ShowQuickAnalysis { get; set; } 
 *         bool         _Application.FlashFill { get; set; } 
 *         bool         _Application.EnableMacroAnimations { get; set; } 
 *         bool         _Application.ChartDataPointTrack { get; set; } 
 *         bool         _Application.FlashFillMode { get; set; } 
 *         bool         _Application.MergeInstances { get; set; } 
 *         bool         _Application.EnableCheckFileExtensions { get; set; } 
 *         
 *         XlWindowState   _Application.WindowState { get; set; } 
 *         XlFileFormat    _Application.DefaultSaveFormat { get; set; } 
 *         DefaultWebOptions  _Application.DefaultWebOptions { get; } 
 *         LanguageSettings   _Application.LanguageSettings { get; } 
 *         SpellingOptions    _Application.SpellingOptions { get; } 
 *         NewFile         _Application.NewWorkbook { get; } 
 *         Speech          _Application.Speech { get; } 
 *         Range           _Application.ThisCell { get; } 
 *         Watches         _Application.Watches { get; } 
 *         IAssistance     _Application.Assistance { get; } 
 *         XlMousePointer  _Application.Cursor { get; set; } 
 *         AutoCorrect     _Application.AutoCorrect { get; } 
 *         AutoRecover     _Application.AutoRecover { get; } 
 *         MsoAutomationSecurity  _Application.AutomationSecurity { get; set; } 
 *         Dialogs         _Application.Dialogs { get; } 
 *         XlCalculation         _Application.Calculation { get; set; } 
 *         XlCommandUnderlines   _Application.CommandUnderlines { get; set; } 
 *         XlCommentDisplayMode  _Application.DisplayCommentIndicator { get; set; } 
 *         XlEnableCancelKey     _Application.EnableCancelKey { get; set; } 
 *         IFind                 _Application.FileFind { get; } 
 *         FileSearch            _Application.FileSearch { get; } 
 *         RecentFiles           _Application.RecentFiles { get; } 
 *         XlMailSystem          _Application.MailSystem { get; } 
 *         dynamic               _Application.MailSession { get; } 
 *         XlGenerateTableRefs   _Application.GenerateTableRefs { get; set; } 
 *         ProtectedViewWindows  _Application.ProtectedViewWindows { get; } 
 *         ProtectedViewWindow   _Application.ActiveProtectedViewWindow { get; } 
 *         QuickAnalysis         _Application.QuickAnalysis { get; } 
 *         XlDirection           _Application.MoveAfterReturnDirection { get; set; } 
 *         XlCutCopyMode         _Application.CutCopyMode { get; set; } 
 *         MultiThreadedCalculation   _Application.MultiThreadedCalculation { get; } 
 *         VBE  _Application.VBE { get; } 
 *         RTD  _Application.RTD { get; } 
 *         AnswerWizard  _Application.AnswerWizard { get; } 
 *         MsoFeatureInstall  _Application.FeatureInstall { get; set; } 
 *         CellFormat  _Application.FindFormat { get; set; } 
 *         CellFormat  _Application.ReplaceFormat { get; set; } 
 *         UsedObjects  _Application.UsedObjects { get; } 
 *         XlCalculationState  _Application.CalculationState { get; } 
 *         XlCalculationInterruptKey  _Application.CalculationInterruptKey { get; set; } 
 *         SmartTagRecognizers        _Application.SmartTagRecognizers { get; } 
 *         FileExportConverters       _Application.FileExportConverters { get; } 
 *         MsoFileValidationMode      _Application.FileValidation { get; set; } 
 *         XlFileValidationPivotMode  _Application.FileValidationPivot { get; set; } 
 *         ODBCErrors   _Application.ODBCErrors { get; } 
 *         OLEDBErrors  _Application.OLEDBErrors { get; } 
 *         ErrorCheckingOptions   _Application.ErrorCheckingOptions { get; } 
 *         XlReferenceStyle  _Application.ReferenceStyle { get; set; } 
 *         SmartArtLayouts        _Application.SmartArtLayouts { get; } 
 *         SmartArtQuickStyles    _Application.SmartArtQuickStyles { get; } 
 *         SmartArtColors         _Application.SmartArtColors { get; } 
 *         AddIns   _Application.AddIns { get; } 
 *         AddIns2  _Application.AddIns2 { get; } 
 *         COMAddIns  _Application.COMAddIns { get; } 
 *         dynamic  _Application.HinstancePtr { get; } 
 *         dynamic  _Application.Dummy101 { get; } 
 *         
 *         ＊abstract Method
 *         dynamic  _Application.Run(object Macro, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         dynamic  _Application._Run2(object Macro, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         dynamic  _Application.InputBox(string Prompt, object Title, object Default, object Left, object Top, object HelpFile, object HelpContextID, object Type) 
 *         Range    _Application.get_Range(object Cell1, object Cell2) 
 *         Range    _Application.Intersect(Range Arg1, Range Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         void     _Application.CheckAbort(object KeepAbort) 
 *         void     _Application.AddChartAutoFormat(object Chart, string Name, object Description) 
 *         void     _Application.AddCustomList(object ListArray, object ByRow) 
 *         Range    _Application.Union(Range Arg1, Range Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         int      _Application.SharePointVersion(string bstrUrl) 
 *         double   _Application.InchesToPoints(double Inches) 
 *         double   _Application.CentimetersToPoints(double Centimeters) 
 *         bool     _Application.CheckSpelling(string Word, object CustomDictionary, object IgnoreUppercase) 
 *         dynamic  _Application.ConvertFormula(object Formula, XlReferenceStyle FromReferenceStyle, object ToReferenceStyle, object ToAbsolute, object RelativeTo) 
 *         Workbook _Application.NextLetter() 
 *         void     _Application.DoubleClick() 
 *         void     _Application.Undo() 
 *         void     _Application.Repeat() 
 *         bool     _Application.FindFile() 
 *         void     _Application._FindFile() 
 *         dynamic  _Application.GetOpenFilename(object FileFilter, object FilterIndex, object Title, object ButtonText, object MultiSelect) 
 *         void     _Application.Calculate() 
 *         void     _Application.CalculateFull() 
 *         void     _Application.CalculateFullRebuild() 
 *         void     _Application.CalculateUntilAsyncQueriesDone() 
 *         dynamic  _Application.Evaluate(object Name) 
 *         dynamic  _Application._Evaluate(object Name) 
 *         void     _Application.Save(object Filename) 
 *         void     _Application.SaveWorkspace(object Filename) 
 *         dynamic  _Application.GetSaveAsFilename(object InitialFilename, object FileFilter, object FilterIndex, object Title, object ButtonText) 
 *         void     _Application.Volatile(object Volatile) 
 *         bool     _Application.Wait(object Time) 
 *         void     _Application._Wait(object Time) 
 *         dynamic  _Application.Support(object Object, int ID, object arg) 
 *         bool     _Application.RegisterXLL(string Filename) 
 *         void     _Application.ResetTipWizard() 
 *         void     _Application.Goto(object Reference, object Scroll) 
 *         void     _Application.Help(object HelpFile, object HelpContextID) 
 *         void     _Application.MailLogon(object Name, object Password, object DownloadNewMail) 
 *         void     _Application.MailLogoff() 
 *         void     _Application.DeleteChartAutoFormat(string Name) 
 *         void     _Application.DeleteCustomList(int ListNum) 
 *         dynamic  _Application.get_Caller(object Index) 
 *         Menu     _Application.get_ShortcutMenus(int Index) 
 *         dynamic  _Application.get_FileConverters(object Index1, object Index2) 
 *         dynamic  _Application.GetCustomListContents(int ListNum) 
 *         FileDialog  _Application.get_FileDialog(MsoFileDialogType fileDialogType) 
 *         int      _Application.GetCustomListNum(object ListArray) 
 *         string   _Application.GetPhonetic(object Text) 
 *         dynamic  _Application.get_International(object Index) 
 *         dynamic  _Application.get_RegisteredFunctions(object Index1, object Index2) 
 *         dynamic  _Application.get_PreviousSelections(object Index) 
 *         dynamic  _Application.get_ClipboardFormats(object Index) 
 *         void     _Application.SetDefaultChart(object FormatName, object Gallery) 
 *         void     _Application.DisplayXMLSourcePane(object XmlMap) 
 *         void     _Application.SendKeys(object Keys, object Wait) 
 *         dynamic  _Application._WSFunction(object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         void     _Application.ActivateMicrosoftApp(XlMSApplication Index) 
 *         dynamic  _Application.ExecuteExcel4Macro(string String) 
 *         void     _Application.RecordMacro(object BasicCode, object XlmCode) 
 *         void     _Application.MacroOptions(object Macro, object Description, object HasMenu, object MenuText, object HasShortcutKey, object ShortcutKey, object Category, object StatusBar, object HelpContextID, object HelpFile) 
 *         void     _Application.MacroOptions2(object Macro, object Description, object HasMenu, object MenuText, object HasShortcutKey, object ShortcutKey, object Category, object StatusBar, object HelpContextID, object HelpFile, object ArgumentDescriptions) 
 *         void     _Application.DDEExecute(int Channel, string String) 
 *         dynamic  _Application.DDERequest(int Channel, string Item) 
 *         int      _Application.DDEInitiate(string App, string Topic) 
 *         void     _Application.DDEPoke(int Channel, object Item, object Data) 
 *         void     _Application.DDETerminate(int Channel) 
 *         dynamic  _Application.Dummy1(object Arg1, object Arg2, object Arg3, object Arg4) 
 *         dynamic  _Application.Dummy2(object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8) 
 *         dynamic  _Application.Dummy3() 
 *         dynamic  _Application.Dummy4(object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15) 
 *         dynamic  _Application.Dummy5(object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13) 
 *         dynamic  _Application.Dummy6() 
 *         dynamic  _Application.Dummy7() 
 *         dynamic  _Application.Dummy8(object Arg1) 
 *         dynamic  _Application.Dummy9() 
 *         bool     _Application.Dummy10(object arg) 
 *         void     _Application.Dummy11() 
 *         void     _Application.Dummy12(PivotTable p1, PivotTable p2) 
 *         dynamic  _Application.Dummy13(object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         void     _Application.Dummy14() 
 *         dynamic  _Application.Dummy20(int grfCompareFunctions) 
 *         void     _Application.Quit() 
 *          
 *         void  _Application.OnKey(string Key, object Procedure) 
 *         void  _Application.OnRepeat(string Text, string Procedure) 
 *         void  _Application.OnTime(object EarliestTime, string Procedure, object LatestTime, object Schedule) 
 *         void  _Application.OnUndo(string Text, string Procedure) 
 *
 *@subject ◆interface AppEvents_Event -- Microsoft.Office.Interop.Excel
 *         event AppEvents_NewWorkbookEventHandler NewWorkbook 
 *         event AppEvents_WorkbookBeforeXmlImportEventHandler WorkbookBeforeXmlImport 
 *         event AppEvents_WorkbookAfterXmlImportEventHandler WorkbookAfterXmlImport 
 *         event AppEvents_WorkbookBeforeXmlExportEventHandler WorkbookBeforeXmlExport 
 *         event AppEvents_WorkbookAfterXmlExportEventHandler WorkbookAfterXmlExport 
 *         event AppEvents_WorkbookRowsetCompleteEventHandler WorkbookRowsetComplete 
 *         event AppEvents_AfterCalculateEventHandler AfterCalculate 
 *         event AppEvents_SheetPivotTableAfterValueChangeEventHandler SheetPivotTableAfterValueChange 
 *         event AppEvents_SheetPivotTableBeforeAllocateChangesEventHandler SheetPivotTableBeforeAllocateChanges 
 *         event AppEvents_SheetPivotTableBeforeCommitChangesEventHandler SheetPivotTableBeforeCommitChanges 
 *         event AppEvents_WorkbookSyncEventHandler WorkbookSync 
 *         event AppEvents_SheetPivotTableBeforeDiscardChangesEventHandler SheetPivotTableBeforeDiscardChanges 
 *         event AppEvents_ProtectedViewWindowBeforeEditEventHandler ProtectedViewWindowBeforeEdit 
 *         event AppEvents_ProtectedViewWindowBeforeCloseEventHandler ProtectedViewWindowBeforeClose 
 *         event AppEvents_ProtectedViewWindowResizeEventHandler ProtectedViewWindowResize 
 *         event AppEvents_ProtectedViewWindowActivateEventHandler ProtectedViewWindowActivate 
 *         event AppEvents_ProtectedViewWindowDeactivateEventHandler ProtectedViewWindowDeactivate 
 *         event AppEvents_WorkbookAfterSaveEventHandler WorkbookAfterSave 
 *         event AppEvents_WorkbookNewChartEventHandler WorkbookNewChart 
 *         event AppEvents_SheetLensGalleryRenderCompleteEventHandler SheetLensGalleryRenderComplete 
 *         event AppEvents_SheetTableUpdateEventHandler SheetTableUpdate 
 *         event AppEvents_ProtectedViewWindowOpenEventHandler ProtectedViewWindowOpen 
 *         event AppEvents_WorkbookModelChangeEventHandler WorkbookModelChange 
 *         event AppEvents_WorkbookPivotTableOpenConnectionEventHandler WorkbookPivotTableOpenConnection 
 *         event AppEvents_SheetPivotTableUpdateEventHandler SheetPivotTableUpdate 
 *         event AppEvents_SheetSelectionChangeEventHandler SheetSelectionChange 
 *         event AppEvents_SheetBeforeDoubleClickEventHandler SheetBeforeDoubleClick 
 *         event AppEvents_SheetBeforeRightClickEventHandler SheetBeforeRightClick 
 *         event AppEvents_SheetActivateEventHandler SheetActivate 
 *         event AppEvents_SheetDeactivateEventHandler SheetDeactivate 
 *         event AppEvents_SheetCalculateEventHandler SheetCalculate 
 *         event AppEvents_SheetChangeEventHandler SheetChange 
 *         event AppEvents_WorkbookOpenEventHandler WorkbookOpen 
 *         event AppEvents_WorkbookActivateEventHandler WorkbookActivate 
 *         event AppEvents_WorkbookPivotTableCloseConnectionEventHandler WorkbookPivotTableCloseConnection 
 *         event AppEvents_WorkbookDeactivateEventHandler WorkbookDeactivate 
 *         event AppEvents_WorkbookBeforeSaveEventHandler WorkbookBeforeSave 
 *         event AppEvents_WorkbookBeforePrintEventHandler WorkbookBeforePrint 
 *         event AppEvents_WorkbookNewSheetEventHandler WorkbookNewSheet 
 *         event AppEvents_WorkbookAddinInstallEventHandler WorkbookAddinInstall 
 *         event AppEvents_WorkbookAddinUninstallEventHandler WorkbookAddinUninstall 
 *         event AppEvents_WindowResizeEventHandler WindowResize 
 *         event AppEvents_WindowActivateEventHandler WindowActivate 
 *         event AppEvents_WindowDeactivateEventHandler WindowDeactivate 
 *         event AppEvents_SheetFollowHyperlinkEventHandler SheetFollowHyperlink 
 *         event AppEvents_WorkbookBeforeCloseEventHandler WorkbookBeforeClose 
 *         event AppEvents_SheetBeforeDeleteEventHandler SheetBeforeDelete 
 */
#endregion
/*
 *@see ImageExcelWorkbookOpenSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-22
 */
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelWorkbookOpenSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelWorkbookOpenSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelWorkbookOpenSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelWorkbookOpenSample : Form
    {
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly TextBox textBox;
        private readonly Button button;

        public FormExcelWorkbookOpenSample()
        {
            this.Text = "FormExcelWorkbookOpenSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(480, 120);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelWorkbookOpenSample");
            this.Load += new EventHandler(FormExcelWorkbookOpenSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormExcelWorkbookOpenSample_FormClosed);

            //---- Controls ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            textBox = new TextBox()
            {
                Text = "",
                Multiline = false,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBox);

            button = new Button()
            {
                Text = "Open Excel Application",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button);

            //---- Deployment ----
            this.Controls.AddRange(new Control[]
            {
                table,
            });
        }//constructor

        private void Button_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            try
            {
                Excel.Workbook wb = excelApp.Workbooks.Open(
                    Path.GetFullPath(@"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelFileSample.xlsx"));
                Excel.Worksheet sheet1 = (Excel.Worksheet)wb.Sheets[1];

                textBox.Text = wb.Name;
                wb.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
            finally
            {
                excelApp.Quit();
            }
        }//Button_Click()

        //====== Form Event ======
        private void FormExcelWorkbookOpenSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//Form1_Load()

        private void FormExcelWorkbookOpenSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.Close();
        }//Form1_FormClosed()
    }//class
}
