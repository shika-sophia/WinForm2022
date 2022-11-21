/** 
 *@title WinFormGUI / WinFormSample / ReverseReference / RR17_WindowsSystem
 *@class MainRegistrySample.cs
 *@reference CS 山田祥寛『独習 C＃ [新版] 』 翔泳社, 2017
 *@reference NT 山田祥寛『独習 ASP.NET [第６版] 』 翔泳社, 2019
 *@reference RR 増田智明・国本温子『Visual C＃2019 逆引き大全 500の極意』 秀和システム, 2019
 *          
 *@content RR[481]-[483] p814 / Registry, RegistryKey (No Code)
 *         Registry: 
 *         Windows OS内の ツリー構造の階層的 Databaseで、Windows内の各種設定や
 *         ソフトウェア, ハードウェアの挙動を継続的に同じ設定の元で動作させるために参照される。
 *         Windowsの機能 'Registry Editor' で編集可能だが、Windowsデフォルトの設定変更による動作不良の可能性があるので、
 *         意味を理解した上で編集すべきである。
 *         
 *         ◆MSDN  
 *         ＊Windows registry information for advanced users:
 *         https://learn.microsoft.com/en-us/troubleshoot/windows-server/performance/windows-registry-advanced-users
 *         A central hierarchical database used in Windows 98, Windows CE, Windows NT, and Windows 2000
 *         used to store information that is necessary to configure the system for one or more users, applications, and hardware devices.
 *         The Registry contains information that Windows continually references 
 *         during operation, such as profiles for each user, the applications 
 *         installed on the computer and the types of documents 
 *         that each can create, property sheet settings for folders and application icons,
 *         what hardware exists on the system, and the ports that are being used.
 *         
 *         ◆Computer Hope  
 *         ＊Registry:
 *         https://www.computerhope.com/jargon/r/registry.htm
 *         The registry or Windows registry is a database of information, settings, options, and other values 
 *         for software and hardware installed on all versions of Microsoft Windows operating systems.
 *         When a program is installed, a new subkey is created in the registry.
 *         This subkey contains settings specific to that program,
 *         such as its location, version, and primary executable.
 *         [End of Quotation]
 *         
 *         【Note】
 *         Here is (No Code), because 'Registry' is influenced 
 *         to Windows OS settings or execution, according to above.
 *         API Reference of Visual Studio only is shown here.
 *         
 *@subject ◆static class Registry -- Microsoft.Win32
 *         [×] 'new' is not available, because of static class.
 *         
 *         + static readonly RegistryKey  Registry.Users 
 *         + static readonly RegistryKey  Registry.CurrentUser 
 *         + static readonly RegistryKey  Registry.CurrentConfig 
 *         + static readonly RegistryKey  Registry.LocalMachine 
 *         + static readonly RegistryKey  Registry.ClassesRoot 
 *         + static readonly RegistryKey  Registry.PerformanceData 
 *         + static readonly RegistryKey  Registry.DynData 
 *         + static          object       Registry.GetValue(string keyName, string valueName, object defaultValue) 
 *         + static          void         Registry.SetValue(string keyName, string valueName, object value) 
 *         + static          void         Registry.SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind) 
 *             └ enum RegistryValueKind : System.Enum
 *                            -- Microsoft.Win32
 *               {
 *                   Unknown = 0,
 *                   String = 1,
 *                   ExpandString = 2,
 *                   Binary = 3,
 *                   DWord = 4,
 *                   MultiString = 7,
 *                   QWord = 11,
 *                   None = -1,
 *               }
 *               
 *@subject ◆sealed class RegistryKey : MarshalByRefObject, IDisposable
 *                           -- Microsoft.Win32
 *         [×] 'new' is not available, ONLY from Registry class or this.
 *         
 *         + string              registryKey.Name { get; } 
 *         + int                 registryKey.ValueCount { get; } 
 *         + int                 registryKey.SubKeyCount { get; } 
 *         + SafeRegistryHandle  registryKey.Handle { get; } 
 *         + RegistryView        registryKey.View { get; } 
 *             └ enum RegistryView -- Microsoft.Win32.
 *               {
 *                  Default = 0,
 *                  Registry64 = 256,
 *                  Registry32 = 512
 *               }
 *         
 *         + string[]            registryKey.GetValueNames() 
 *         + string[]            registryKey.GetSubKeyNames() 
 *         + static RegistryKey  RegistryKey.FromHandle(SafeRegistryHandle handle, RegistryView view) 
 *         + static RegistryKey  RegistryKey.FromHandle(SafeRegistryHandle handle) 
 *         + static RegistryKey  RegistryKey.OpenBaseKey(RegistryHive hKey, RegistryView view) 
 *         + static RegistryKey  RegistryKey.OpenRemoteBaseKey(RegistryHive hKey, string machineName) 
 *         + static RegistryKey  RegistryKey.OpenRemoteBaseKey(RegistryHive hKey, string machineName, RegistryView view) 
 *         + RegistryKey  registryKey.CreateSubKey(string subkey) 
 *         + RegistryKey  registryKey.CreateSubKey(string subkey, bool writable) 
 *         + RegistryKey  registryKey.CreateSubKey(string subkey, bool writable, RegistryOptions options) 
 *         + RegistryKey  registryKey.CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck) 
 *         + RegistryKey  registryKey.CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity) 
 *         + RegistryKey  registryKey.CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions, RegistrySecurity registrySecurity) 
 *         + RegistryKey  registryKey.CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions options) 
 *         + void         registryKey.DeleteValue(string name) 
 *         + void         registryKey.DeleteValue(string name, bool throwOnMissingValue) 
 *         + void         registryKey.DeleteSubKey(string subkey) 
 *         + void         registryKey.DeleteSubKey(string subkey, bool throwOnMissingSubKey) 
 *         + void         registryKey.DeleteSubKeyTree(string subkey) 
 *         + void         registryKey.DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey) 
 *         + RegistryValueKind  registryKey.GetValueKind(string name) 
 *         + RegistrySecurity   registryKey.GetAccessControl() 
 *         + RegistrySecurity   registryKey.GetAccessControl(AccessControlSections includeSections) 
 *         + object       registryKey.GetValue(string name) 
 *         + object       registryKey.GetValue(string name, object defaultValue) 
 *         + object       registryKey.GetValue(string name, object defaultValue, RegistryValueOptions options) 
 *         + RegistryKey  registryKey.OpenSubKey(string name) 
 *         + RegistryKey  registryKey.OpenSubKey(string name, bool writable) 
 *         + RegistryKey  registryKey.OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck) 
 *         + RegistryKey  registryKey.OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights) 
 *         + RegistryKey  registryKey.OpenSubKey(string name, RegistryRights rights) 
 *         + void         registryKey.SetValue(string name, object value) 
 *         + void         registryKey.SetValue(string name, object value, RegistryValueKind valueKind) 
 *         + void         registryKey.SetAccessControl(RegistrySecurity registrySecurity) 
 *         + void         registryKey.Flush() 
 *         + void         registryKey.Close() 
 *         + void         registryKey.Dispose() 
 *               
 *@see (No Image)
 *@see 
 *@author shika
 *@date 2022-11-21
 */
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WinFormGUI.WinFormSample.ReverseReference.RR17_WindowsSystem
{
    class MainRegistrySample
    {
        //(No Code)
        // Microsoft.Win32.Registry
        // Microsoft.Win32.RegistryKey
    }
}
