/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR18_Excel
 *@class MainExcelCellsValueSample.cs
 *@class   └ new FormExcelCellsValueSample() : Form
 *@class       └ new Excel.Application()
 *@using Excel = Microsoft.Office.Interop.Excel
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[486] p820 / Excel / Cells Value
 *@subject【NOTE】index of Excel class start from 1 (not 0).
 *@subject Get Value from Excel WorkSheet
 *         Sheets   workbook.Sheets
 *         dynamic  workbook.Sheets[i]
 *         dynamic  workBook.ActiveSheet
 *         Range    workSheet.Cells[i, j]                   // [row, column] Arguments start from 1 (not 0)
 *         Range    workSheet.Range[object]                 // Argument is Excel CellName
 *         dynamic  workSheet.Range[object from, object to] // Argument is Excel Range of CellNames,
 *                                                          // return 'dynamic', not necessary cast to 'Range', but need do when use Range's Property
 *         string   range.Value
 *         double   range.Value  数値
 *         
 *         [Example]
 *         Excel.Worksheet sheet1 = (Excel.Worksheet) wb.Sheets[1];
 *         textBox.Text = sheet1.Range["A1"].Value;
 *         textBox.Text = sheet1.Cells[1, 1].Value;
 */
#region -> interface Workbook, _Workbook, WorkSheet, _WorkSheet, Sheets, Range
/*
 *@subject ◆interface Workbook : _Workbook, WorkbookEvents_Event
 *                        -- Microsoft.Office.Interop.Excel
 *         public interface Workbook { }
 *         
 *@subject ◆interface Worksheet : _Worksheet, DocEvents_Event
 *                        -- Microsoft.Office.Interop.Excel
 *         public interface Worksheet { }
 *         
 *@subject ◆interface _Workbook -- Microsoft.Office.Interop.Excel
 *         string  _Workbook.Name { get; } 
 *         Names   _Workbook.Names { get; } 
 *         string  _Workbook.FullName { get; } 
 *         string  _Workbook.CodeName { get; } 
 *         string  _Workbook._CodeName { get; set; } 
 *         string  _Workbook.Path { get; } 
 *         string  _Workbook.Title { get; set; } 
 *         string  _Workbook.Subject { get; set; } 
 *         string  _Workbook.Author { get; set; } 
 *         string  _Workbook.Comments { get; set; } 
 *         string  _Workbook.Keywords { get; set; } 
 *         string  _Workbook.Password { get; set; } 
 *         string  _Workbook.WritePassword { get; set; } 
 *         Styles  _Workbook.Styles { get; } 
 *         Connections   _Workbook.Connections { get; } 
 *         SignatureSet  _Workbook.Signatures { get; } 
 *         int     _Workbook.RevisionNumber { get; } 
 *         bool    _Workbook.ReadOnly { get; } 
 *         Application   _Workbook.Application { get; } 
 *         XlCreator     _Workbook.Creator { get; } 
 *         Windows  _Workbook.Windows { get; } 
 *         dynamic  _Workbook.Parent { get; } 
 *         Sheets   _Workbook.Sheets { get; } 
 *         Sheets   _Workbook.Charts { get; } 
 *         Sheets   _Workbook.Worksheets { get; } 
 *         Sheets   _Workbook.DialogSheets { get; } 
 *         dynamic  _Workbook.ActiveSheet { get; } 
 *         Chart    _Workbook.ActiveChart { get; } 
 *         XlFileFormat  _Workbook.FileFormat { get; } 
 *         CommandBars  _Workbook.CommandBars { get; } 
 *         dynamic  _Workbook.Container { get; } 
 *         dynamic  _Workbook.CustomDocumentProperties { get; } 
 *         string  _Workbook.WriteReservedBy { get; } 
 *         string  _Workbook.FullNameURLEncoded { get; } 
 *         string  _Workbook.EncryptionProvider { get; set; } 
 *         string  _Workbook.PasswordEncryptionProvider { get; } 
 *         string  _Workbook.PasswordEncryptionAlgorithm { get; } 
 *         int     _Workbook.PasswordEncryptionKeyLength { get; } 
 *         string  _Workbook.OnSave { get; set; } 
 *         string  _Workbook.OnSheetActivate { get; set; } 
 *         string  _Workbook.OnSheetDeactivate { get; set; } 
 *         
 *         int  _Workbook.AutoUpdateFrequency { get; set; } 
 *         int  _Workbook.CalculationVersion { get; } 
 *         int  _Workbook.ChangeHistoryDuration { get; set; } 
 *         int  _Workbook.AccuracyVersion { get; set; } 
 *         
 *         bool  _Workbook.IsAddin { get; set; } 
 *         bool  _Workbook.HasMailer { get; set; } 
 *         bool  _Workbook.HasPassword { get; } 
 *         bool  _Workbook.ForceFullCalculation { get; set; } 
 *         bool  _Workbook.AutoUpdateSaveChanges { get; set; } 
 *         bool  _Workbook.HasRoutingSlip { get; set; } 
 *         bool  _Workbook.AcceptLabelsInFormulas { get; set; } 
 *         bool  _Workbook.MultiUserEditing { get; } 
 *         bool  _Workbook.PersonalViewListSettings { get; set; } 
 *         bool  _Workbook.DoNotPromptForConvert { get; set; } 
 *         bool  _Workbook.PersonalViewPrintSettings { get; set; } 
 *         bool  _Workbook.PrecisionAsDisplayed { get; set; } 
 *         bool  _Workbook.ProtectStructure { get; } 
 *         bool  _Workbook.ProtectWindows { get; } 
 *         bool  _Workbook._ReadOnlyRecommended { get; } 
 *         bool  _Workbook.Routed { get; } 
 *         bool  _Workbook.CreateBackup { get; } 
 *         bool  _Workbook.Date1904 { get; set; } 
 *         bool  _Workbook.Saved { get; set; } 
 *         bool  _Workbook.ShowConflictHistory { get; set; } 
 *         bool  _Workbook.UpdateRemoteReferences { get; set; } 
 *         bool  _Workbook.UserControl { get; set; } 
 *         bool  _Workbook.WriteReserved { get; } 
 *         bool  _Workbook.SaveLinkValues { get; set; } 
 *         bool  _Workbook.TemplateRemoveExtData { get; set; } 
 *         bool  _Workbook.HighlightChangesOnScreen { get; set; } 
 *         bool  _Workbook.KeepChangeHistory { get; set; } 
 *         bool  _Workbook.ListChangesOnNewSheet { get; set; } 
 *         bool  _Workbook.IsInplace { get; } 
 *         bool  _Workbook.EnvelopeVisible { get; set; } 
 *         bool  _Workbook.VBASigned { get; } 
 *         bool  _Workbook.ShowPivotTableFieldList { get; set; } 
 *         bool  _Workbook.EnableAutoRecover { get; set; } 
 *         bool  _Workbook.RemovePersonalInformation { get; set; } 
 *         bool  _Workbook.PasswordEncryptionFileProperties { get; } 
 *         bool  _Workbook.ReadOnlyRecommended { get; set; } 
 *         bool  _Workbook.InactiveListBorderVisible { get; set; } 
 *         bool  _Workbook.DisplayInkComments { get; set; } 
 *         bool  _Workbook.CheckCompatibility { get; set; } 
 *         bool  _Workbook.HasVBProject { get; } 
 *         bool  _Workbook.Final { get; set; } 
 *         bool  _Workbook.Excel8CompatibilityMode { get; } 
 *         bool  _Workbook.ConnectionsDisabled { get; } 
 *         bool  _Workbook.ShowPivotChartActiveFields { get; set; } 
 *         bool  _Workbook.ChartDataPointTrack { get; set; } 
 *         bool  _Workbook.CaseSensitive { get; } 
 *         bool  _Workbook.UseWholeCellCriteria { get; } 
 *         bool  _Workbook.UseWildcards { get; } 
 *         
 *         dynamic  _Workbook.UserStatus { get; } 
 *         dynamic  _Workbook.DefaultTableStyle { get; set; } 
 *         dynamic  _Workbook.DefaultPivotTableStyle { get; set; } 
 *         dynamic  _Workbook.DefaultTimelineStyle { get; set; } 
 *         dynamic  _Workbook.PivotTables { get; } 
 *         dynamic  _Workbook.BuiltinDocumentProperties { get; } 
 *         
 *         Sheets  _Workbook.Modules { get; } 
 *         Sheets  _Workbook.Excel4IntlMacroSheets { get; } 
 *         Sheets  _Workbook.Excel4MacroSheets { get; } 
 *         Mailer  _Workbook.Mailer { get; } 
 *         TableStyles  _Workbook.TableStyles { get; } 
 *         XlDisplayDrawingObjects  _Workbook.DisplayDrawingObjects { get; set; } 
 *         RoutingSlip  _Workbook.RoutingSlip { get; } 
 *         CustomViews  _Workbook.CustomViews { get; } 
 *         VBProject  _Workbook.VBProject { get; } 
 *         PublishObjects  _Workbook.PublishObjects { get; } 
 *         WebOptions  _Workbook.WebOptions { get; } 
 *         XlSaveConflictResolution  _Workbook.ConflictResolution { get; set; } 
 *         HTMLProject  _Workbook.HTMLProject { get; } 
 *         XlUpdateLinks  _Workbook.UpdateLinks { get; set; } 
 *         SmartTagOptions  _Workbook.SmartTagOptions { get; } 
 *         Permission  _Workbook.Permission { get; } 
 *         SharedWorkspace  _Workbook.SharedWorkspace { get; } 
 *         Sync  _Workbook.Sync { get; } 
 *         XmlNamespaces  _Workbook.XmlNamespaces { get; } 
 *         XmlMaps        _Workbook.XmlMaps { get; } 
 *         SmartDocument  _Workbook.SmartDocument { get; } 
 *         DocumentLibraryVersions  _Workbook.DocumentLibraryVersions { get; } 
 *         MetaProperties  _Workbook.ContentTypeProperties { get; } 
 *         ServerPolicy  _Workbook.ServerPolicy { get; } 
 *         DocumentInspectors  _Workbook.DocumentInspectors { get; } 
 *         ServerViewableItems  _Workbook.ServerViewableItems { get; } 
 *         CustomXMLParts  _Workbook.CustomXMLParts { get; } 
 *         Research  _Workbook.Research { get; } 
 *         OfficeTheme  _Workbook.Theme { get; } 
 *         IconSets  _Workbook.IconSets { get; } 
 *         SlicerCaches  _Workbook.SlicerCaches { get; } 
 *         Slicer  _Workbook.ActiveSlicer { get; } 
 *         dynamic  _Workbook.DefaultSlicerStyle { get; set; } 
 *         Model  _Workbook.Model { get; } 
 *         
 *         ＊Method
 *         void  _Workbook.Activate() 
 *         Window  _Workbook.NewWindow() 
 *         void  _Workbook.OpenLinks(string Name, object ReadOnly, object Type) 
 *         void  _Workbook.ChangeFileAccess(XlFileAccess Mode, object WritePassword, object Notify) 
 *         void  _Workbook.ChangeLink(string Name, string NewName, XlLinkType Type = XlLinkType.xlLinkTypeExcelLinks) 
 *         dynamic  _Workbook.get_Colors(object Index) 
 *         void     _Workbook.set_Colors(object Index, object RHS) 
 *         void  _Workbook.DeleteNumberFormat(string NumberFormat) 
 *         bool  _Workbook.ExclusiveAccess() 
 *         void  _Workbook.ForwardMailer() 
 *         dynamic  _Workbook.LinkInfo(string Name, XlLinkInfo LinkInfo, object Type, object EditionRef) 
 *         dynamic  _Workbook.LinkSources(object Type) 
 *         void  _Workbook.MergeWorkbook(object Filename) 
 *         PivotCaches  _Workbook.PivotCaches() 
 *         void  _Workbook.Post(object DestName) 
 *         void  _Workbook._PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate) 
 *         void  _Workbook.PrintPreview(object EnableChanges) 
 *         void  _Workbook._Protect(object Password, object Structure, object Windows) 
 *         void  _Workbook.ProtectSharing(object Filename, object Password, object WriteResPassword, object ReadOnlyRecommended, object CreateBackup, object SharingPassword) 
 *         void  _Workbook.Reply() 
 *         void  _Workbook.ReplyAll() 
 *         void  _Workbook.ReplyWithChanges(object ShowMessage) 
 *         void  _Workbook.RefreshAll() 
 *         void  _Workbook.RemoveUser(int Index) 
 *         void  _Workbook.Route() 
 *         void  _Workbook.RunAutoMacros(XlRunAutoMacro Which) 
 *         void  _Workbook.Save() 
 *         void  _Workbook.SaveAs(object Filename, object FileFormat, object Password, object WriteResPassword, object ReadOnlyRecommended, object CreateBackup, XlSaveAsAccessMode AccessMode = XlSaveAsAccessMode.xlNoChange, object ConflictResolution = null, object AddToMru = null, object TextCodepage = null, object TextVisualLayout = null, object Local = null) 
 *         void  _Workbook._SaveAs(object Filename, object FileFormat, object Password, object WriteResPassword, object ReadOnlyRecommended, object CreateBackup, XlSaveAsAccessMode AccessMode = XlSaveAsAccessMode.xlNoChange, object ConflictResolution = null, object AddToMru = null, object TextCodepage = null, object TextVisualLayout = null) 
 *         void  _Workbook.SaveCopyAs(object Filename) 
 *         void  _Workbook.ReloadAs(MsoEncoding Encoding) 
 *         void  _Workbook.SendMail(object Recipients, object Subject, object ReturnReceipt) 
 *         void  _Workbook.SendMailer(object FileFormat, XlPriority Priority = XlPriority.xlPriorityNormal) 
 *         void  _Workbook.SetLinkOnData(string Name, object Procedure) 
 *         void  _Workbook.Unprotect(object Password) 
 *         void  _Workbook.UnprotectSharing(object SharingPassword) 
 *         void  _Workbook.UpdateFromFile() 
 *         void  _Workbook.UpdateLink(object Name, object Type) 
 *         void  _Workbook.HighlightChangesOptions(object When, object Who, object Where) 
 *         void  _Workbook.PurgeChangeHistoryNow(int Days, object SharingPassword) 
 *         void  _Workbook.AcceptAllChanges(object When, object Who, object Where) 
 *         void  _Workbook.RejectAllChanges(object When, object Who, object Where) 
 *         void  _Workbook.PivotTableWizard(object SourceType, object SourceData, object TableDestination, object TableName, object RowGrand, object ColumnGrand, object SaveData, object HasAutoFormat, object AutoPage, object Reserved, object BackgroundQuery, object OptimizeCache, object PageFieldOrder, object PageFieldWrapCount, object ReadData, object Connection) 
 *         void  _Workbook.ResetColors() 
 *         void  _Workbook.FollowHyperlink(string Address, object SubAddress, object NewWindow, object AddHistory, object ExtraInfo, object Method, object HeaderInfo) 
 *         void  _Workbook.AddToFavorites() 
 *         void  _Workbook.PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName) 
 *         void  _Workbook.WebPagePreview() 
 *         void  _Workbook.sblt(string s) 
 *         void  _Workbook.BreakLink(string Name, XlLinkType Type) 
 *         void  _Workbook.CheckIn(object SaveChanges, object Comments, object MakePublic) 
 *         bool  _Workbook.CanCheckIn() 
 *         void  _Workbook.SendForReview(object Recipients, object Subject, object ShowMessage, object IncludeAttachment) 
 *         void  _Workbook.EndReview() 
 *         void  _Workbook.SetPasswordEncryptionOptions(object PasswordEncryptionProvider, object PasswordEncryptionAlgorithm, object PasswordEncryptionKeyLength, object PasswordEncryptionFileProperties) 
 *         void  _Workbook.Protect(object Password, object Structure, object Windows) 
 *         void  _Workbook.RecheckSmartTags() 
 *         void  _Workbook.SendFaxOverInternet(object Recipients, object Subject, object ShowMessage) 
 *         XlXmlImportResult  _Workbook.XmlImport(string Url, out XmlMap ImportMap, object Overwrite, object Destination) 
 *         XlXmlImportResult  _Workbook.XmlImportXml(string Data, out XmlMap ImportMap, object Overwrite, object Destination) 
 *         void  _Workbook.SaveAsXMLData(string Filename, XmlMap Map) 
 *         void  _Workbook.ToggleFormsDesign() 
 *         void  _Workbook.RemoveDocumentInformation(XlRemoveDocInfoType RemoveDocInfoType) 
 *         void  _Workbook.CheckInWithVersion(object SaveChanges, object Comments, object MakePublic, object VersionType) 
 *         void  _Workbook.LockServerFile() 
 *         WorkflowTasks  _Workbook.GetWorkflowTasks() 
 *         WorkflowTemplates  _Workbook.GetWorkflowTemplates() 
 *         void  _Workbook.PrintOutEx(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName, object IgnorePrintAreas) 
 *         void  _Workbook.ApplyTheme(string Filename) 
 *         void  _Workbook.EnableConnections() 
 *         void  _Workbook.ExportAsFixedFormat(XlFixedFormatType Type, object Filename, object Quality, object IncludeDocProperties, object IgnorePrintAreas, object From, object To, object OpenAfterPublish, object FixedFormatExtClassPtr) 
 *         void  _Workbook.ProtectSharingEx(object Filename, object Password, object WriteResPassword, object ReadOnlyRecommended, object CreateBackup, object SharingPassword, object FileFormat) 
 *         void  _Workbook.Dummy16() 
 *         void  _Workbook.Dummy17(int calcid) 
 *         void  _Workbook.Dummy26() 
 *         void  _Workbook.Dummy27() 
 *         void  _Workbook.Close(object SaveChanges, object Filename, object RouteWorkbook) 
 *
 *@subject ◆interface _Worksheet -- Microsoft.Office.Interop.Excel
 *         string  _Worksheet.Name { get; set; } 
 *         Names   _Worksheet.Names { get; } 
 *         int     _Worksheet.Index { get; } 
 *         string  _Worksheet.CodeName { get; } 
 *         string  _Worksheet._CodeName { get; set; } 
 *         dynamic _Worksheet.Previous { get; } 
 *         dynamic _Worksheet.Next { get; } 
 *         Range   _Worksheet.Columns { get; } 
 *         Range   _Worksheet.Rows { get; } 
 *         Range   _Worksheet.Cells { get; } 
 *         Application  _Worksheet.Application { get; } 
 *         XlCreator    _Worksheet.Creator { get; } 
 *         XlSheetVisibility  _Worksheet.Visible { get; set; } 
 *         dynamic  _Worksheet.Parent { get; } 
 *         PageSetup  _Worksheet.PageSetup { get; } 
 *         bool  _Worksheet.ProtectContents { get; } 
 *         bool  _Worksheet.ProtectDrawingObjects { get; } 
 *         bool  _Worksheet.ProtectionMode { get; } 
 *         bool  _Worksheet.ProtectScenarios { get; } 
 *         Shapes  _Worksheet.Shapes { get; } 
 *         bool  _Worksheet.TransitionExpEval { get; set; } 
 *         bool  _Worksheet.AutoFilterMode { get; set; } 
 *         bool  _Worksheet.EnableCalculation { get; set; } 
 *         Range  _Worksheet.CircularReference { get; } 
 *         XlConsolidationFunction  _Worksheet.ConsolidationFunction { get; } 
 *         dynamic  _Worksheet.ConsolidationOptions { get; } 
 *         dynamic  _Worksheet.ConsolidationSources { get; } 
 *         bool  _Worksheet.DisplayAutomaticPageBreaks { get; set; } 
 *         bool  _Worksheet.EnableAutoFilter { get; set; } 
 *         XlEnableSelection  _Worksheet.EnableSelection { get; set; } 
 *         bool  _Worksheet.EnableOutlining { get; set; } 
 *         bool  _Worksheet.EnablePivotTable { get; set; } 
 *         bool  _Worksheet.FilterMode { get; } 
 *         Outline  _Worksheet.Outline { get; } 
 *         string  _Worksheet.ScrollArea { get; set; } 
 *         double  _Worksheet.StandardHeight { get; } 
 *         double  _Worksheet.StandardWidth { get; set; } 
 *         bool  _Worksheet.TransitionFormEntry { get; set; } 
 *         XlSheetType  _Worksheet.Type { get; } 
 *         Range  _Worksheet.UsedRange { get; } 
 *         HPageBreaks  _Worksheet.HPageBreaks { get; } 
 *         VPageBreaks  _Worksheet.VPageBreaks { get; } 
 *         QueryTables  _Worksheet.QueryTables { get; } 
 *         bool  _Worksheet.DisplayPageBreaks { get; set; } 
 *         Comments  _Worksheet.Comments { get; } 
 *         Hyperlinks  _Worksheet.Hyperlinks { get; } 
 *         int  _Worksheet._DisplayRightToLeft { get; set; } 
 *         AutoFilter  _Worksheet.AutoFilter { get; } 
 *         bool  _Worksheet.DisplayRightToLeft { get; set; } 
 *         Scripts  _Worksheet.Scripts { get; } 
 *         Tab  _Worksheet.Tab { get; } 
 *         MsoEnvelope  _Worksheet.MailEnvelope { get; } 
 *         CustomProperties  _Worksheet.CustomProperties { get; } 
 *         SmartTags  _Worksheet.SmartTags { get; } 
 *         Protection  _Worksheet.Protection { get; } 
 *         ListObjects  _Worksheet.ListObjects { get; } 
 *         bool  _Worksheet.EnableFormatConditionsCalculation { get; set; } 
 *         Sort  _Worksheet.Sort { get; } 
 *         int  _Worksheet.PrintedCommentPages { get; } 
 *         
 *         void  _Worksheet.Activate() 
 *         void  _Worksheet.Copy(object Before, object After) 
 *         void  _Worksheet.Delete() 
 *         void  _Worksheet.Move(object Before, object After) 
 *         void  _Worksheet._PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate) 
 *         void  _Worksheet.PrintPreview(object EnableChanges) 
 *         void  _Worksheet._Protect(object Password, object DrawingObjects, object Contents, object Scenarios, object UserInterfaceOnly) 
 *         void  _Worksheet._SaveAs(string Filename, object FileFormat, object Password, object WriteResPassword, object ReadOnlyRecommended, object CreateBackup, object AddToMru, object TextCodepage, object TextVisualLayout) 
 *         void  _Worksheet.Select(object Replace) 
 *         void  _Worksheet.Unprotect(object Password) 
 *         dynamic  _Worksheet.Arcs(object Index) 
 *         void  _Worksheet.SetBackgroundPicture(string Filename) 
 *         dynamic  _Worksheet.Buttons(object Index) 
 *         void  _Worksheet.Calculate() 
 *         dynamic  _Worksheet.ChartObjects(object Index) 
 *         dynamic  _Worksheet.CheckBoxes(object Index) 
 *         void  _Worksheet.CheckSpelling(object CustomDictionary, object IgnoreUppercase, object AlwaysSuggest, object SpellLang) 
 *         void  _Worksheet.ClearArrows() 
 *         dynamic  _Worksheet.Drawings(object Index) 
 *         dynamic  _Worksheet.DrawingObjects(object Index) 
 *         dynamic  _Worksheet.DropDowns(object Index) 
 *         dynamic  _Worksheet.Evaluate(object Name) 
 *         dynamic  _Worksheet._Evaluate(object Name) 
 *         void  _Worksheet.ResetAllPageBreaks() 
 *         dynamic  _Worksheet.GroupBoxes(object Index) 
 *         dynamic  _Worksheet.GroupObjects(object Index) 
 *         dynamic  _Worksheet.Labels(object Index) 
 *         dynamic  _Worksheet.Lines(object Index) 
 *         dynamic  _Worksheet.ListBoxes(object Index) 
 *         dynamic  _Worksheet.OLEObjects(object Index) 
 *         dynamic  _Worksheet.OptionButtons(object Index) 
 *         dynamic  _Worksheet.Ovals(object Index) 
 *         void  _Worksheet.Paste(object Destination, object Link) 
 *         void  _Worksheet._PasteSpecial(object Format, object Link, object DisplayAsIcon, object IconFileName, object IconIndex, object IconLabel) 
 *         dynamic  _Worksheet.Pictures(object Index) 
 *         dynamic  _Worksheet.PivotTables(object Index) 
 *         PivotTable  _Worksheet.PivotTableWizard(object SourceType, object SourceData, object TableDestination, object TableName, object RowGrand, object ColumnGrand, object SaveData, object HasAutoFormat, object AutoPage, object Reserved, object BackgroundQuery, object OptimizeCache, object PageFieldOrder, object PageFieldWrapCount, object ReadData, object Connection) 
 *         Range  _Worksheet.get_Range(object Cell1, object Cell2) 
 *         dynamic  _Worksheet.Rectangles(object Index) 
 *         dynamic  _Worksheet.Scenarios(object Index) 
 *         dynamic  _Worksheet.ScrollBars(object Index) 
 *         void  _Worksheet.ShowAllData() 
 *         void  _Worksheet.ShowDataForm() 
 *         dynamic  _Worksheet.Spinners(object Index) 
 *         dynamic  _Worksheet.TextBoxes(object Index) 
 *         void  _Worksheet.ClearCircles() 
 *         void  _Worksheet.CircleInvalid() 
 *         void  _Worksheet.PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName) 
 *         void  _Worksheet._CheckSpelling(object CustomDictionary, object IgnoreUppercase, object AlwaysSuggest, object SpellLang, object IgnoreFinalYaa, object SpellScript) 
 *         void  _Worksheet.SaveAs(string Filename, object FileFormat, object Password, object WriteResPassword, object ReadOnlyRecommended, object CreateBackup, object AddToMru, object TextCodepage, object TextVisualLayout, object Local) 
 *         void  _Worksheet.PasteSpecial(object Format, object Link, object DisplayAsIcon, object IconFileName, object IconIndex, object IconLabel, object NoHTMLFormatting) 
 *         void  _Worksheet.Protect(object Password, object DrawingObjects, object Contents, object Scenarios, object UserInterfaceOnly, object AllowFormattingCells, object AllowFormattingColumns, object AllowFormattingRows, object AllowInsertingColumns, object AllowInsertingRows, object AllowInsertingHyperlinks, object AllowDeletingColumns, object AllowDeletingRows, object AllowSorting, object AllowFiltering, object AllowUsingPivotTables) 
 *         Range  _Worksheet.XmlDataQuery(string XPath, object SelectionNamespaces, object Map) 
 *         Range  _Worksheet.XmlMapQuery(string XPath, object SelectionNamespaces, object Map) 
 *         void  _Worksheet.PrintOutEx(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName, object IgnorePrintAreas) 
 *         void  _Worksheet.ExportAsFixedFormat(XlFixedFormatType Type, object Filename, object Quality, object IncludeDocProperties, object IgnorePrintAreas, object From, object To, object OpenAfterPublish, object FixedFormatExtClassPtr) 
 *
 *         string  _Worksheet.OnDoubleClick { get; set; } 
 *         string  _Worksheet.OnSheetActivate { get; set; } 
 *         string  _Worksheet.OnSheetDeactivate { get; set; } 
 *         string  _Worksheet.OnCalculate { get; set; } 
 *         string  _Worksheet.OnData { get; set; } 
 *         string  _Worksheet.OnEntry { get; set; } 
 *         
 *@subject ◆interface Sheets : IEnumerable
 *                      -- Microsoft.Office.Interop.Excel
 *         dynamic      this[object Index] { get; } 
 *         dynamic      Sheets.Visible { get; set; } 
 *         Application  Sheets.Application { get; } 
 *         XlCreator    Sheets.Creator { get; } 
 *         dynamic      Sheets.Parent { get; } 
 *         int          Sheets.Count { get; } 
 *         HPageBreaks  Sheets.HPageBreaks { get; } 
 *         VPageBreaks  Sheets.VPageBreaks { get; } 
 *         
 *         IEnumerator  Sheets.GetEnumerator() 
 *         dynamic  Sheets.Add(object Before, object After, object Count, object Type) 
 *         dynamic  Sheets.Add2(object Before, object After, object Count, object NewLayout) 
 *         dynamic  Sheets.get_Item(object Index) 
 *         void     Sheets.Select(object Replace) 
 *         void     Sheets.FillAcrossSheets(Range Range, XlFillWith Type = XlFillWith.xlFillWithAll) 
 *         void     Sheets.Move(object Before, object After) 
 *         void     Sheets.Copy(object Before, object After) 
 *         void     Sheets.PrintPreview(object EnableChanges) 
 *         void     Sheets.PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName) 
 *         void     Sheets._PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate) 
 *         void     Sheets.PrintOutEx(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName, object IgnorePrintAreas) 
 *         void     Sheets.Delete() 
 *
 *@subject ◆interface Range : IEnumerable
 *                     -- Microsoft.Office.Interop.Excel
 *         dynamic  this[object RowIndex, object ColumnIndex] { get; set; } 
 *         dynamic  Range.Name { get; set; } 
 *         dynamic  Range.Text { get; } 
 *         string   Range.ID { get; set; } 
 *         int      Range.Count { get; } 
 *         dynamic  Range.CountLarge { get; } 
 *         Worksheet  Range.Worksheet { get; } 
 *         Range    Range.Columns { get; } 
 *         Range    Range.Rows { get; } 
 *         Range    Range.Cells { get; } 
 *         int      Range.Column { get; } 
 *         int      Range.Row { get; } 
 *         Range    Range.MergeArea { get; } 
 *         dynamic  Range.MergeCells { get; set; } 
 *         dynamic  Range.Summary { get; } 
 *         dynamic  Range.Style { get; set; } 
 *         dynamic  Range.Width { get; } 
 *         dynamic  Range.Height { get; } 
 *         dynamic  Range.ColumnWidth { get; set; } 
 *         dynamic  Range.RowHeight { get; set; } 
 *         dynamic  Range.Top { get; } 
 *         dynamic  Range.Left { get; } 
 *         Interior Range.Interior { get; }   セルの内側
 *           └ interface Interior =>〔MainExcelInteriorColorSample.cs〕
 *         dynamic  Range.Value2 { get; set; } 
 *         Areas    Range.Areas { get; } 
 *         Borders  Range.Borders { get; } 
 *         Range    Range.CurrentArray { get; } 
 *         Range    Range.CurrentRegion { get; } 
 *         Range    Range.Previous { get; } 
 *         Range    Range.Next { get; } 
 *         bool     Range.AllowEdit { get; } 
 *         int      Range.ReadingOrder { get; set; } 
 *         Actions  Range.ServerActions { get; } 
 *         Application  Range.Application { get; } 
 *         XlCreator  Range.Creator { get; } 
 *         dynamic  Range.Parent { get; } 
 *         dynamic  Range.AddIndent { get; set; } 
 *         Range    Range.Dependents { get; } 
 *         Range    Range.DirectDependents { get; } 
 *         Range    Range.DirectPrecedents { get; } 
 *         Range    Range.EntireColumn { get; } 
 *         Range    Range.EntireRow { get; } 
 *         int      Range.ListHeaderRows { get; } 
 *         dynamic  Range.Hidden { get; set; } 
 *         dynamic  Range.Locked { get; set; } 
 *         Font     Range.Font { get; } 
 *         dynamic  Range.Orientation { get; set; } 
 *         dynamic  Range.HorizontalAlignment { get; set; } 
 *         dynamic  Range.IndentLevel { get; set; } 
 *         Comment  Range.Comment { get; } 
 *         Phonetic Range.Phonetic { get; } 
 *         dynamic  Range.Formula { get; set; } 
 *         dynamic  Range.FormulaArray { get; set; } 
 *         XlFormulaLabel  Range.FormulaLabel { get; set; } 
 *         dynamic  Range.FormulaHidden { get; set; } 
 *         dynamic  Range.FormulaLocal { get; set; } 
 *         dynamic  Range.FormulaR1C1 { get; set; } 
 *         dynamic  Range.FormulaR1C1Local { get; set; } 
 *         dynamic  Range.HasArray { get; } 
 *         dynamic  Range.HasFormula { get; } 
 *         XlLocationInTable  Range.LocationInTable { get; } 
 *         DisplayFormat  Range.DisplayFormat { get; } 
 *         dynamic  Range.NumberFormat { get; set; } 
 *         dynamic  Range.NumberFormatLocal { get; set; } 
 *         FormatConditions  Range.FormatConditions { get; } 
 *         dynamic     Range.OutlineLevel { get; set; } 
 *         int         Range.PageBreak { get; set; } 
 *         QueryTable  Range.QueryTable { get; } 
 *         PivotTable  Range.PivotTable { get; } 
 *         PivotField  Range.PivotField { get; } 
 *         PivotItem   Range.PivotItem { get; } 
 *         Range    Range.Precedents { get; } 
 *         dynamic  Range.PrefixCharacter { get; } 
 *         dynamic  Range.ShowDetail { get; set; } 
 *         dynamic  Range.ShrinkToFit { get; set; } 
 *         SoundNote  Range.SoundNote { get; } 
 *         dynamic  Range.UseStandardHeight { get; set; } 
 *         dynamic  Range.UseStandardWidth { get; set; } 
 *         Validation  Range.Validation { get; } 
 *         dynamic  Range.VerticalAlignment { get; set; } 
 *         dynamic  Range.WrapText { get; set; } 
 *         Hyperlinks  Range.Hyperlinks { get; } 
 *         Phonetics   Range.Phonetics { get; } 
 *         PivotCell   Range.PivotCell { get; } 
 *         SmartTags   Range.SmartTags { get; } 
 *         ListObject  Range.ListObject { get; } 
 *         XPath       Range.XPath { get; } 
 *         string      Range.MDX { get; } 
 *         SparklineGroups  Range.SparklineGroups { get; } 
 *         Errors      Range.Errors { get; } 
 *         
 *         ＊Method
 *         dynamic  Range.Activate() 
 *         dynamic  Range.Select() 
 *         dynamic  Range.ListNames() 
 *         Range    Range.get_Range(object Cell1, object Cell2) 
 *         Range    Range.SpecialCells(XlCellType Type, object Value) 
 *         string   Range.get_Address(object RowAbsolute, object ColumnAbsolute, XlReferenceStyle ReferenceStyle = XlReferenceStyle.xlA1, object External = null, object RelativeTo = null) 
 *         string   Range.get_AddressLocal(object RowAbsolute, object ColumnAbsolute, XlReferenceStyle ReferenceStyle = XlReferenceStyle.xlA1, object External = null, object RelativeTo = null) 
 *         dynamic  Range.Run(object Arg1, object Arg2, object Arg3, object Arg4, object Arg5, object Arg6, object Arg7, object Arg8, object Arg9, object Arg10, object Arg11, object Arg12, object Arg13, object Arg14, object Arg15, object Arg16, object Arg17, object Arg18, object Arg19, object Arg20, object Arg21, object Arg22, object Arg23, object Arg24, object Arg25, object Arg26, object Arg27, object Arg28, object Arg29, object Arg30) 
 *         dynamic  Range.Calculate() 
 *         dynamic  Range.CalculateRowMajorOrder() 
 *         dynamic  Range.get_Item(object RowIndex, object ColumnIndex) 
 *         void     Range.set_Item(object RowIndex, object ColumnIndex, object value) 
 *         dynamic  Range.get_Value(object RangeValueDataType) 
 *         void     Range.set_Value(object RangeValueDataType, object value) 
 *         dynamic  Range.Table(object RowInput, object ColumnInput) 
 *         dynamic  Range.TextToColumns(object Destination, XlTextParsingType DataType = XlTextParsingType.xlDelimited, XlTextQualifier TextQualifier = XlTextQualifier.xlTextQualifierDoubleQuote, object ConsecutiveDelimiter = null, object Tab = null, object Semicolon = null, object Comma = null, object Space = null, object Other = null, object OtherChar = null, object FieldInfo = null, object DecimalSeparator = null, object ThousandsSeparator = null, object TrailingMinusNumbers = null) 
 *         Comment  Range.AddComment(object Text) 
 *         string   Range.NoteText(object Text, object Start, object Length) 
 *         dynamic  Range.CreateNames(object Top, object Left, object Bottom, object Right) 
 *         dynamic  Range.CreatePublisher(object Edition, XlPictureAppearance Appearance = XlPictureAppearance.xlScreen, object ContainsPICT = null, object ContainsBIFF = null, object ContainsRTF = null, object ContainsVALU = null) 
 *         dynamic  Range.ApplyNames(object Names, object IgnoreRelativeAbsolute, object UseRowColumnNames, object OmitColumn, object OmitRow, XlApplyNamesOrder Order = XlApplyNamesOrder.xlRowThenColumn, object AppendLast = null) 
 *         dynamic  Range.AutoFormat(XlRangeAutoFormat Format = XlRangeAutoFormat.xlRangeAutoFormatClassic1, object Number = null, object Font = null, object Alignment = null, object Border = null, object Pattern = null, object Width = null) 
 *         dynamic  Range.BorderAround(object LineStyle, XlBorderWeight Weight = XlBorderWeight.xlThin, XlColorIndex ColorIndex = XlColorIndex.xlColorIndexAutomatic, object Color = null) 
 *         dynamic  Range.BorderAround2(object LineStyle, XlBorderWeight Weight = XlBorderWeight.xlThin, XlColorIndex ColorIndex = XlColorIndex.xlColorIndexAutomatic, object Color = null, object ThemeColor = null) 
 *         dynamic  Range.CheckSpelling(object CustomDictionary, object IgnoreUppercase, object AlwaysSuggest, object SpellLang) 
 *         Range    Range.ColumnDifferences(object Comparison) 
 *         Range    Range.RowDifferences(object Comparison) 
 *         bool     Range.GoalSeek(object Goal, Range ChangingCell) 
 *         void     Range.AllocateChanges() 
 *         void     Range.DiscardChanges() 
 *         Range    Range.get_Resize(object RowSize, object ColumnSize) 
 *         dynamic  Range.Consolidate(object Sources, object Function, object TopRow, object LeftColumn, object CreateLinks) 
 *         dynamic  Range.Copy(object Destination) 
 *         int      Range.CopyFromRecordset(object Data, object MaxRows, object MaxColumns) 
 *         dynamic  Range.CopyPicture(XlPictureAppearance Appearance = XlPictureAppearance.xlScreen, XlCopyPictureFormat Format = XlCopyPictureFormat.xlPicture) 
 *         dynamic  Range.Cut(object Destination) 
 *         dynamic  Range.PasteSpecial(XlPasteType Paste = XlPasteType.xlPasteAll, XlPasteSpecialOperation Operation = XlPasteSpecialOperation.xlPasteSpecialOperationNone, object SkipBlanks = null, object Transpose = null) 
 *         dynamic  Range._PasteSpecial(XlPasteType Paste = XlPasteType.xlPasteAll, XlPasteSpecialOperation Operation = XlPasteSpecialOperation.xlPasteSpecialOperationNone, object SkipBlanks = null, object Transpose = null) 
 *         Range    Range.Find(object What, object After, object LookIn, object LookAt, object SearchOrder, XlSearchDirection SearchDirection = XlSearchDirection.xlNext, object MatchCase = null, object MatchByte = null, object SearchFormat = null) 
 *         Range    Range.FindNext(object After) 
 *         Range    Range.FindPrevious(object After) 
 *         void     Range.InsertIndent(int InsertAmount) 
 *         dynamic  Range.Insert(object Shift, object CopyOrigin) 
 *         void     Range.Merge(object Across) 
 *         void     Range.UnMerge() 
 *         dynamic  Range.Sort(object Key1, XlSortOrder Order1 = XlSortOrder.xlAscending, object Key2 = null, object Type = null, XlSortOrder Order2 = XlSortOrder.xlAscending, object Key3 = null, XlSortOrder Order3 = XlSortOrder.xlAscending, XlYesNoGuess Header = XlYesNoGuess.xlNo, object OrderCustom = null, object MatchCase = null, XlSortOrientation Orientation = XlSortOrientation.xlSortRows, XlSortMethod SortMethod = XlSortMethod.xlPinYin, XlSortDataOption DataOption1 = XlSortDataOption.xlSortNormal, XlSortDataOption DataOption2 = XlSortDataOption.xlSortNormal, XlSortDataOption DataOption3 = XlSortDataOption.xlSortNormal) 
 *         dynamic  Range.SortSpecial(XlSortMethod SortMethod = XlSortMethod.xlPinYin, object Key1 = null, XlSortOrder Order1 = XlSortOrder.xlAscending, object Type = null, object Key2 = null, XlSortOrder Order2 = XlSortOrder.xlAscending, object Key3 = null, XlSortOrder Order3 = XlSortOrder.xlAscending, XlYesNoGuess Header = XlYesNoGuess.xlNo, object OrderCustom = null, object MatchCase = null, XlSortOrientation Orientation = XlSortOrientation.xlSortRows, XlSortDataOption DataOption1 = XlSortDataOption.xlSortNormal, XlSortDataOption DataOption2 = XlSortDataOption.xlSortNormal, XlSortDataOption DataOption3 = XlSortDataOption.xlSortNormal) 
 *         dynamic  Range.Group(object Start, object End, object By, object Periods) 
 *         dynamic  Range.Ungroup() 
 *         dynamic  Range.Parse(object ParseLine, object Destination) 
 *         bool     Range.Replace(object What, object Replacement, object LookAt, object SearchOrder, object MatchCase, object MatchByte, object SearchFormat, object ReplaceFormat) 
 *         void     Range.RemoveDuplicates(object Columns, XlYesNoGuess Header = XlYesNoGuess.xlNo) 
 *         dynamic  Range.Delete(object Shift) 
 *         dynamic  Range.NavigateArrow(object TowardPrecedent, object ArrowNumber, object LinkNumber) 
 *         dynamic  Range.DataSeries(object Rowcol, XlDataSeriesType Type = XlDataSeriesType.xlDataSeriesLinear, XlDataSeriesDate Date = XlDataSeriesDate.xlDay, object Step = null, object Stop = null, object Trend = null) 
 *         dynamic  Range.AutoFill(Range Destination, XlAutoFillType Type = XlAutoFillType.xlFillDefault) 
 *         dynamic  Range.FillUp() 
 *         dynamic  Range.FillDown() 
 *         dynamic  Range.FillLeft() 
 *         dynamic  Range.FillRight() 
 *         void     Range.FlashFill() 
 *         dynamic  Range.Show() 
 *         dynamic  Range.ShowErrors() 
 *         dynamic  Range.ShowPrecedents(object Remove) 
 *         dynamic  Range.ShowDependents(object Remove) 
 *         dynamic  Range.DialogBox() 
 *         dynamic  Range.FunctionWizard() 
 *         IEnumerator  Range.GetEnumerator() 
 *         dynamic  Range.AutoFit() 
 *         dynamic  Range.EditionOptions(XlEditionType Type, XlEditionOptionsOption Option, object Name, object Reference, XlPictureAppearance Appearance = XlPictureAppearance.xlScreen, XlPictureAppearance ChartSize = XlPictureAppearance.xlScreen, object Format = null) 
 *         dynamic  Range.AutoOutline() 
 *         string   Range.AutoComplete(string String) 
 *         dynamic  Range.ApplyOutlineStyles() 
 *         dynamic  Range.AutoFilter(object Field, object Criteria1, XlAutoFilterOperator Operator = XlAutoFilterOperator.xlAnd, object Criteria2 = null, object VisibleDropDown = null) 
 *         dynamic  Range.AdvancedFilter(XlFilterAction Action, object CriteriaRange, object CopyToRange, object Unique) 
 *         Characters  Range.get_Characters(object Start, object Length) 
 *         Range    Range.get_End(XlDirection Direction) 
 *         Range    Range.get_Offset(object RowOffset, object ColumnOffset) 
 *         dynamic  Range.SubscribeTo(string Edition, XlSubscribeToFormat Format = XlSubscribeToFormat.xlSubscribeToText) 
 *         dynamic  Range.Subtotal(int GroupBy, XlConsolidationFunction Function, object TotalList, object Replace, object PageBreaks, XlSummaryRow SummaryBelowData = XlSummaryRow.xlSummaryBelow) 
 *         dynamic  Range.RemoveSubtotal() 
 *         void     Range.SetPhonetic() 
 *         dynamic  Range.PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName) 
 *         dynamic  Range._PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate) 
 *         dynamic  Range.PrintOutEx(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName) 
 *         dynamic  Range.PrintPreview(object EnableChanges) 
 *         void     Range.ExportAsFixedFormat(XlFixedFormatType Type, object Filename, object Quality, object IncludeDocProperties, object IgnorePrintAreas, object From, object To, object OpenAfterPublish, object FixedFormatExtClassPtr) 
 *         void     Range.Speak(object SpeakDirection, object SpeakFormulas) 
 *         dynamic  Range.Justify() 
 *         void     Range.Dirty() 
 *         dynamic  Range.Clear() 
 *         dynamic  Range.ClearContents() 
 *         void     Range.ClearComments() 
 *         dynamic  Range.ClearFormats() 
 *         dynamic  Range.ClearNotes() 
 *         dynamic  Range.ClearOutline() 
 *         void     Range.ClearHyperlinks() 
 */
#endregion
/*
 *@see ImageExcelCellsValueSample.jpg
 *@see 
 *@author shika
 *@date 2022-11-23
 */
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinFormGUI.WinFormSample.ReverseReference.RR18_Excel
{
    class MainExcelCellsValueSample
    {
        //[STAThread]
        //static void Main()
        public void Main()
        {
            Console.WriteLine("new FormExcelCellsValueSample()");

            Application.EnableVisualStyles();
            Application.Run(new FormExcelCellsValueSample());

            Console.WriteLine("Close()");
        }//Main()
    }//class

    class FormExcelCellsValueSample : Form
    {
        private readonly Mutex mutex;
        private readonly TableLayoutPanel table;
        private readonly Button button;
        private readonly TextBox textBox;

        public FormExcelCellsValueSample()
        {
            this.Text = "FormExcelCellsValueSample";
            this.Font = new Font("consolas", 12, FontStyle.Regular);
            this.ClientSize = new Size(640, 640);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoSize = true;
            this.BackColor = SystemColors.Window;

            //---- Form Event ----
            mutex = new Mutex(initiallyOwned: false, "FormExcelCellsValueSample");
            this.Load += new EventHandler(FormExcelCellsValueSample_Load);
            this.FormClosed += new FormClosedEventHandler(FormExcelCellsValueSample_FormClosed);

            //---- Controls ----
            table = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };

            button = new Button()
            {
                Text = "Show Excel Value",
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            button.Click += new EventHandler(Button_Click);
            table.Controls.Add(button);

            textBox = new TextBox()
            {
                Multiline = true,
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            table.Controls.Add(textBox);

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
            Excel.Worksheet sheet1 = (Excel.Worksheet) wb.Sheets[1] ;
            //textBox.Text = sheet1.Range["A1"].Value;
            textBox.Text = sheet1.Cells[1, 1].Value;

            excelApp.Quit();
        }//Button_Click()

        //====== Form Event ======
        private void FormExcelCellsValueSample_Load(object sender, EventArgs e)
        {
            if (!mutex.WaitOne(millisecondsTimeout: 0, exitContext: false))
            {
                MessageBox.Show("This Form already has been running.");
                this.Close();
            }
        }//FormExcelCellsValueSample_Load()

        private void FormExcelCellsValueSample_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.Close();
        }//FormExcelCellsValueSample_FormClosed()
    }//class
}
