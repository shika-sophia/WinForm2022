/** 
 *@title WinFormGUI / WinFormSample / ReverseReference.RR18_Excel
 *@class MainExcelWorkbookOpenSample.cs
 *@class   └ new FormExcelWorkbookOpenSample() : Form
 *@interface  └ new Microsoft.Office.Interop.Excel.Application() : _Application, AppEvents_Event
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[484][485] p818 / Excel ファイルを Open
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
 *         XlCreator    _Application.Creator { get; } 
 *         Application  _Application.Application { get; } 
 *         Application  _Application.Parent { get; } 
 *         dynamic      _Application.Selection { get; } 
 *         Names        _Application.Names { get; } 
 *         Windows      _Application.Windows { get; } 
 *         Window       _Application.ActiveWindow { get; } 
 *         Workbooks    _Application.Workbooks { get; } 
 *         Workbook     _Application.ThisWorkbook { get; } 
 *         Workbook     _Application.ActiveWorkbook { get; } 
 *         Sheets       _Application.Sheets { get; } 
 *         Sheets       _Application.Worksheets { get; } 
 *         Sheets       _Application.DialogSheets { get; } 
 *         dynamic      _Application.ActiveSheet { get; } 
 *         Range        _Application.Columns { get; } 
 *         Range        _Application.Rows { get; } 
 *         Range        _Application.Cells { get; } 
 *         Range        _Application.ActiveCell { get; } 
 *         Sheets       _Application.Charts { get; } 
 *         Chart        _Application.ActiveChart { get; } 
 *         DialogSheet  _Application.ActiveDialog { get; } 
 *         WorksheetFunction  _Application.WorksheetFunction { get; } 
 *         string       _Application.ActivePrinter { get; set; } 
 *         AddIns       _Application.AddIns { get; } 
 *         Assistant    _Application.Assistant { get; } 
 *         MenuBars     _Application.MenuBars { get; } 
 *         MenuBar      _Application.ActiveMenuBar { get; } 
 *         CommandBars  _Application.CommandBars { get; } 
 *         Toolbars     _Application.Toolbars { get; } 
 *         Modules      _Application.Modules { get; } 
 *         Sheets       _Application.Excel4IntlMacroSheets { get; } 
 *         Sheets       _Application.Excel4MacroSheets { get; } 
 *         
 *         string       _Application._Default { get; } 
 *         string       _Application.Name { get; } 
 *         string       _Application.OrganizationName { get; } 
 *         string       _Application.OperatingSystem { get; } 
 *         string       _Application.Path { get; } 
 *         string       _Application.DefaultFilePath { get; set; } 
 *         string       _Application.LibraryPath { get; } 
 *         string       _Application.NetworkTemplatesPath { get; } 
 *         string       _Application.PathSeparator { get; } 
 *         string       _Application.AltStartupPath { get; set; } 
 *         string       _Application.Caption { get; set; } 
 *         string       _Application.OnWindow { get; set; } 
 *         string       _Application.OnSheetActivate { get; set; } 
 *         string       _Application.OnSheetDeactivate { get; set; } 
 *         string       _Application.OnEntry { get; set; } 
 *         string       _Application.OnCalculate { get; set; } 
 *         string       _Application.OnData { get; set; } 
 *         string       _Application.OnDoubleClick { get; set; } 
 *         
 *         int          _Application.Build { get; } 
 *         int          _Application.DDEAppReturnCode { get; } 
 *         int          _Application.CustomListCount { get; } 
 *         int          _Application.DataEntryMode { get; set; } 
 *         int          _Application.FixedDecimalPlaces { get; set; } 
 *         double       _Application.Height { get; set; } 
 *         double       _Application.Left { get; set; } 
 *         double       _Application.MaxChange { get; set; } 
 *         int  _Application.MaxIterations { get; set; } 
 *         int  _Application.MemoryFree { get; } 
 *         int  _Application.MemoryTotal { get; } 
 *         int  _Application.MemoryUsed { get; } 
 *         int  _Application.ODBCTimeout { get; set; } 
 *         
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
 *         bool  _Application.DisplayFormulaBar { get; set; } 
 *         bool  _Application.DisplayFullScreen { get; set; } 
 *         bool  _Application.DisplayNoteIndicator { get; set; } 
 *         bool  _Application.DisplayExcel4Menus { get; set; } 
 *         bool  _Application.DisplayRecentFiles { get; set; } 
 *         bool  _Application.DisplayScrollBars { get; set; } 
 *         bool  _Application.DisplayStatusBar { get; set; } 
 *         bool  _Application.EditDirectlyInCell { get; set; } 
 *         bool  _Application.EnableAutoComplete { get; set; } 
 *         bool  _Application.EnableSound { get; set; } 
 *         bool  _Application.EnableTipWizard { get; set; } 
 *         bool  _Application.FixedDecimal { get; set; } 
 *         bool  _Application.IgnoreRemoteRequests { get; set; } 
 *         bool  _Application.Interactive { get; set; } 
 *         bool  _Application.Iteration { get; set; } 
 *         bool  _Application.LargeButtons { get; set; } 
 *         bool  _Application.MathCoprocessorAvailable { get; } 
 *         bool  _Application.MouseAvailable { get; } 
 *         bool  _Application.MoveAfterReturn { get; set; } 
 *         
 *         XlCalculation  _Application.Calculation { get; set; } 
 *         AutoCorrect    _Application.AutoCorrect { get; } 
 *         XlCommandUnderlines  _Application.CommandUnderlines { get; set; } 
 *         XlMousePointer  _Application.Cursor { get; set; } 
 *         XlCutCopyMode  _Application.CutCopyMode { get; set; } 
 *         Dialogs  _Application.Dialogs { get; } 
 *         XlCommentDisplayMode  _Application.DisplayCommentIndicator { get; set; } 
 *         XlEnableCancelKey  _Application.EnableCancelKey { get; set; } 
 *         FileSearch  _Application.FileSearch { get; } 
 *         IFind  _Application.FileFind { get; } 
 *         dynamic  _Application.MailSession { get; } 
 *         XlMailSystem  _Application.MailSystem { get; } 
 *         XlDirection  _Application.MoveAfterReturnDirection { get; set; } 
 *         RecentFiles  _Application.RecentFiles { get; } 
 *         ODBCErrors   _Application.ODBCErrors { get; } 
 *         XlReferenceStyle  _Application.ReferenceStyle { get; set; } 
 *         bool  _Application.PivotTableSelection { get; set; } 
 *         bool  _Application.PromptForSummaryInfo { get; set; } 
 *         bool  _Application.RecordRelative { get; } 
 *         bool  _Application.RollZoom { get; set; } 
 *         bool  _Application.ScreenUpdating { get; set; } 
 *         int  _Application.SheetsInNewWorkbook { get; set; } 
 *         bool  _Application.ShowChartTipNames { get; set; } 
 *         bool  _Application.ShowChartTipValues { get; set; } 
 *         string  _Application.StandardFont { get; set; } 
 *         double  _Application.StandardFontSize { get; set; } 
 *         int  _Application.TransitionMenuKeyAction { get; set; } 
 *         string  _Application.StartupPath { get; } 
 *         dynamic  _Application.StatusBar { get; set; } 
 *         string  _Application.TemplatesPath { get; } 
 *         bool  _Application.ShowToolTips { get; set; } 
 *         double  _Application.Top { get; set; } 
 *         string  _Application.TransitionMenuKey { get; set; } 
 *         XlFileFormat  _Application.DefaultSaveFormat { get; set; } 
 *         bool  _Application.TransitionNavigKeys { get; set; } 
 *         double  _Application.UsableHeight { get; } 
 *         double  _Application.UsableWidth { get; } 
 *         bool  _Application.UserControl { get; set; } 
 *         string  _Application.UserName { get; set; } 
 *         string  _Application.Value { get; } 
 *         VBE  _Application.VBE { get; } 
 *         string  _Application.Version { get; } 
 *         bool  _Application.Visible { get; set; } 
 *         double  _Application.Width { get; set; } 
 *         bool  _Application.WindowsForPens { get; } 
 *         XlWindowState  _Application.WindowState { get; set; } 
 *         int  _Application.UILanguage { get; set; } 
 *         int  _Application.DefaultSheetDirection { get; set; } 
 *         int  _Application.CursorMovement { get; set; } 
 *         bool  _Application.ControlCharacters { get; set; } 
 *         bool  _Application.EnableEvents { get; set; } 
 *         bool  _Application.DisplayInfoWindow { get; set; } 
 *         bool  _Application.ExtendList { get; set; } 
 *         OLEDBErrors  _Application.OLEDBErrors { get; } 
 *         COMAddIns  _Application.COMAddIns { get; } 
 *         DefaultWebOptions  _Application.DefaultWebOptions { get; } 
 *         string  _Application.ProductCode { get; } 
 *         string  _Application.UserLibraryPath { get; } 
 *         bool  _Application.AutoPercentEntry { get; set; } 
 *         LanguageSettings  _Application.LanguageSettings { get; } 
 *         dynamic  _Application.Dummy101 { get; } 
 *         AnswerWizard  _Application.AnswerWizard { get; } 
 *         int  _Application.CalculationVersion { get; } 
 *         bool  _Application.ShowWindowsInTaskbar { get; set; } 
 *         MsoFeatureInstall  _Application.FeatureInstall { get; set; } 
 *         bool  _Application.Ready { get; } 
 *         CellFormat  _Application.FindFormat { get; set; } 
 *         CellFormat  _Application.ReplaceFormat { get; set; } 
 *         UsedObjects  _Application.UsedObjects { get; } 
 *         XlCalculationState  _Application.CalculationState { get; } 
 *         XlCalculationInterruptKey  _Application.CalculationInterruptKey { get; set; } 
 *         Watches  _Application.Watches { get; } 
 *         bool  _Application.DisplayFunctionToolTips { get; set; } 
 *         MsoAutomationSecurity  _Application.AutomationSecurity { get; set; } 
 *         bool  _Application.DisplayPasteOptions { get; set; } 
 *         bool  _Application.DisplayInsertOptions { get; set; } 
 *         bool  _Application.GenerateGetPivotData { get; set; } 
 *         AutoRecover  _Application.AutoRecover { get; } 
 *         int  _Application.Hwnd { get; } 
 *         int  _Application.Hinstance { get; } 
 *         ErrorCheckingOptions  _Application.ErrorCheckingOptions { get; } 
 *         bool  _Application.AutoFormatAsYouTypeReplaceHyperlinks { get; set; } 
 *         SmartTagRecognizers  _Application.SmartTagRecognizers { get; } 
 *         NewFile  _Application.NewWorkbook { get; } 
 *         SpellingOptions  _Application.SpellingOptions { get; } 
 *         Speech  _Application.Speech { get; } 
 *         bool  _Application.MapPaperSize { get; set; } 
 *         bool  _Application.ShowStartupDialog { get; set; } 
 *         string  _Application.DecimalSeparator { get; set; } 
 *         string  _Application.ThousandsSeparator { get; set; } 
 *         bool  _Application.UseSystemSeparators { get; set; } 
 *         Range  _Application.ThisCell { get; } 
 *         RTD  _Application.RTD { get; } 
 *         bool  _Application.DisplayDocumentActionTaskPane { get; set; } 
 *         bool  _Application.ArbitraryXMLSupportAvailable { get; } 
 *         int  _Application.MeasurementUnit { get; set; } 
 *         bool  _Application.ShowSelectionFloaties { get; set; } 
 *         bool  _Application.ShowMenuFloaties { get; set; } 
 *         bool  _Application.ShowDevTools { get; set; } 
 *         bool  _Application.EnableLivePreview { get; set; } 
 *         bool  _Application.DisplayDocumentInformationPanel { get; set; } 
 *         bool  _Application.AlwaysUseClearType { get; set; } 
 *         bool  _Application.WarnOnFunctionNameConflict { get; set; } 
 *         int  _Application.FormulaBarHeight { get; set; } 
 *         bool  _Application.DisplayFormulaAutoComplete { get; set; } 
 *         XlGenerateTableRefs  _Application.GenerateTableRefs { get; set; } 
 *         IAssistance  _Application.Assistance { get; } 
 *         bool  _Application.EnableLargeOperationAlert { get; set; } 
 *         int  _Application.LargeOperationCellThousandCount { get; set; } 
 *         bool  _Application.DeferAsyncQueries { get; set; } 
 *         MultiThreadedCalculation  _Application.MultiThreadedCalculation { get; } 
 *         int  _Application.ActiveEncryptionSession { get; } 
 *         bool  _Application.HighQualityModeForGraphics { get; set; } 
 *         FileExportConverters  _Application.FileExportConverters { get; } 
 *         SmartArtLayouts  _Application.SmartArtLayouts { get; } 
 *         SmartArtQuickStyles  _Application.SmartArtQuickStyles { get; } 
 *         SmartArtColors  _Application.SmartArtColors { get; } 
 *         AddIns2  _Application.AddIns2 { get; } 
 *         bool  _Application.PrintCommunication { get; set; } 
 *         bool  _Application.UseClusterConnector { get; set; } 
 *         string  _Application.ClusterConnector { get; set; } 
 *         bool  _Application.Quitting { get; } 
 *         bool  _Application.Dummy22 { get; set; } 
 *         bool  _Application.Dummy23 { get; set; } 
 *         ProtectedViewWindows  _Application.ProtectedViewWindows { get; } 
 *         ProtectedViewWindow  _Application.ActiveProtectedViewWindow { get; } 
 *         bool  _Application.IsSandboxed { get; } 
 *         bool  _Application.SaveISO8601Dates { get; set; } 
 *         dynamic  _Application.HinstancePtr { get; } 
 *         MsoFileValidationMode  _Application.FileValidation { get; set; } 
 *         XlFileValidationPivotMode  _Application.FileValidationPivot { get; set; } 
 *         bool  _Application.ShowQuickAnalysis { get; set; } 
 *         QuickAnalysis  _Application.QuickAnalysis { get; } 
 *         bool  _Application.FlashFill { get; set; } 
 *         bool  _Application.EnableMacroAnimations { get; set; } 
 *         bool  _Application.ChartDataPointTrack { get; set; } 
 *         bool  _Application.FlashFillMode { get; set; } 
 *         bool  _Application.MergeInstances { get; set; } 
 *         bool  _Application.EnableCheckFileExtensions { get; set; } 
 *         
 *         ＊abstract Method
 *         void  _Application.Calculate() 
 *         void  _Application.DDEExecute(int Channel, string String) 
 *         int  _Application.DDEInitiate(string App, string Topic) 
 *         void  _Application.DDEPoke(int Channel, object Item, object Data) 
 *         dynamic  _Application.DDERequest(int Channel, string Item) 
 *         void  _Application.DDETerminate(int Channel) 
 *         dynamic  _Application.Evaluate(object Name) 
 *         dynamic  _Application._Evaluate(object Name) 
 *         dynamic  _Application.ExecuteExcel4Macro(string String) 
 *         Range  _Application.Intersect(Range Arg1, Range Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         Range  _Application.get_Range(object Cell1, object Cell2) 
 *         dynamic  _Application.Run(object Macro, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         dynamic  _Application._Run2(object Macro, object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         void  _Application.SendKeys(object Keys, object Wait) 
 *         Menu  _Application.get_ShortcutMenus(int Index) 
 *         Range  _Application.Union(Range Arg1, Range Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         void  _Application.ActivateMicrosoftApp(XlMSApplication Index) 
 *         void  _Application.AddChartAutoFormat(object Chart, string Name, object Description) 
 *         void  _Application.AddCustomList(object ListArray, object ByRow) 
 *         dynamic  _Application.get_Caller(object Index) 
 *         double  _Application.CentimetersToPoints(double Centimeters) 
 *         bool  _Application.CheckSpelling(string Word, object CustomDictionary, object IgnoreUppercase) 
 *         dynamic  _Application.get_ClipboardFormats(object Index) 
 *         dynamic  _Application.ConvertFormula(object Formula, XlReferenceStyle FromReferenceStyle, object ToReferenceStyle, object ToAbsolute, object RelativeTo) 
 *         dynamic  _Application.Dummy1(object Arg1, object Arg2, object Arg3, object Arg4) 
 *         dynamic  _Application.Dummy2(object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8) 
 *         dynamic  _Application.Dummy3() 
 *         dynamic  _Application.Dummy4(object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15) 
 *         dynamic  _Application.Dummy5(object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13) 
 *         dynamic  _Application.Dummy6() 
 *         dynamic  _Application.Dummy7() 
 *         dynamic  _Application.Dummy8(object Arg1) 
 *         dynamic  _Application.Dummy9() 
 *         bool  _Application.Dummy10(object arg) 
 *         void  _Application.Dummy11() 
 *         void  _Application.DeleteChartAutoFormat(string Name) 
 *         void  _Application.DeleteCustomList(int ListNum) 
 *         void  _Application.DoubleClick() 
 *         dynamic  _Application.get_FileConverters(object Index1, object Index2) 
 *         void  _Application._FindFile() 
 *         dynamic  _Application.GetCustomListContents(int ListNum) 
 *         int  _Application.GetCustomListNum(object ListArray) 
 *         dynamic  _Application.GetOpenFilename(object FileFilter, object FilterIndex, object Title, object ButtonText, object MultiSelect) 
 *         dynamic  _Application.GetSaveAsFilename(object InitialFilename, object FileFilter, object FilterIndex, object Title, object ButtonText) 
 *         void  _Application.Goto(object Reference, object Scroll) 
 *         void  _Application.Help(object HelpFile, object HelpContextID) 
 *         double  _Application.InchesToPoints(double Inches) 
 *         dynamic  _Application.InputBox(string Prompt, object Title, object Default, object Left, object Top, object HelpFile, object HelpContextID, object Type) 
 *         dynamic  _Application.get_International(object Index) 
 *         void  _Application.MacroOptions(object Macro, object Description, object HasMenu, object MenuText, object HasShortcutKey, object ShortcutKey, object Category, object StatusBar, object HelpContextID, object HelpFile) 
 *         void  _Application.MailLogoff() 
 *         void  _Application.MailLogon(object Name, object Password, object DownloadNewMail) 
 *         Workbook  _Application.NextLetter() 
 *         void  _Application.OnKey(string Key, object Procedure) 
 *         void  _Application.OnRepeat(string Text, string Procedure) 
 *         void  _Application.OnTime(object EarliestTime, string Procedure, object LatestTime, object Schedule) 
 *         void  _Application.OnUndo(string Text, string Procedure) 
 *         dynamic  _Application.get_PreviousSelections(object Index) 
 *         void  _Application.Quit() 
 *         void  _Application.RecordMacro(object BasicCode, object XlmCode) 
 *         dynamic  _Application.get_RegisteredFunctions(object Index1, object Index2) 
 *         bool  _Application.RegisterXLL(string Filename) 
 *         void  _Application.Repeat() 
 *         void  _Application.ResetTipWizard() 
 *         void  _Application.Save(object Filename) 
 *         void  _Application.SaveWorkspace(object Filename) 
 *         void  _Application.SetDefaultChart(object FormatName, object Gallery) 
 *         void  _Application.Undo() 
 *         void  _Application.Volatile(object Volatile) 
 *         void  _Application._Wait(object Time) 
 *         dynamic  _Application._WSFunction(object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         bool  _Application.Wait(object Time) 
 *         string  _Application.GetPhonetic(object Text) 
 *         void  _Application.Dummy12(PivotTable p1, PivotTable p2) 
 *         void  _Application.CalculateFull() 
 *         bool  _Application.FindFile() 
 *         dynamic  _Application.Dummy13(object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         FileDialog  _Application.get_FileDialog(MsoFileDialogType fileDialogType) 
 *         void  _Application.Dummy14() 
 *         void  _Application.CalculateFullRebuild() 
 *         void  _Application.CheckAbort(object KeepAbort) 
 *         void  _Application.DisplayXMLSourcePane(object XmlMap) 
 *         dynamic  _Application.Support(object Object, int ID, object arg) 
 *         dynamic  _Application.Dummy20(int grfCompareFunctions) 
 *         void  _Application.CalculateUntilAsyncQueriesDone() 
 *         int  _Application.SharePointVersion(string bstrUrl) 
 *         void  _Application.MacroOptions2(object Macro, object Description, object HasMenu, object MenuText, object HasShortcutKey, object ShortcutKey, object Category, object StatusBar, object HelpContextID, object HelpFile, object ArgumentDescriptions) 
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
            Excel.Workbook wb = excelApp.Workbooks.Open(
                Path.GetFullPath(@"..\..\WinFormSample\ReverseReference\RR18_Excel\RR18_ExcelFileSample.xlsx"));
            textBox.Text = wb.Name;

            excelApp.Quit();
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
