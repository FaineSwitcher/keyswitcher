using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace FaineSwitch
{
    class Program
    {
        #region DLLs
        [DllImport("user32.dll")]
        public static extern uint RegisterWindowMessage(string message);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetProcessDPIAware();
        #endregion
        #region Prevent another instance variables
        /// GGPU = Global GUID PC User
        public static string GGPU_Mutex = "Global\\" + "ec511418-1d57-4dbe-a0c3-c6022b33735b_" + Environment.UserDomainName + "_" + Environment.UserName;
        public static uint ao = RegisterWindowMessage("AlreadyOpenedSwitcher!");
        public static uint re = RegisterWindowMessage("RestartSwitcher!");
        #endregion
        #region All Main variables, arrays etc.
        public static List<KMHook.YuKey> c_word = new List<KMHook.YuKey>();
        public static List<List<KMHook.YuKey>> c_words = new List<List<KMHook.YuKey>>();
        public static IntPtr _evt_hookID = IntPtr.Zero;
        public static IntPtr _LDevt_hookID = IntPtr.Zero;
        public static WinAPI.WinEventDelegate _evt_proc = KMHook.EventHookCallback;
        public static WinAPI.WinEventDelegate _LDevt_proc = KMHook.LDEventHook;
        public static Locales.Locale[] locales = Locales.AllList();
        public static int PHLayouts = 0;
        public static string _language = "";
        public static Dictionary<Languages.Element, string> Lang = Languages.English;
        public static Configs MyConfs;
        public static SwitcherUI switcher;
        public static bool C_SWITCH = false;
        public static IntPtr SWITCHER_HANDLE;
        public static RawInputForm rif;
        public static System.Threading.Timer _logTimer = new System.Threading.Timer((_) => { try { Logging.UpdateLog(); } catch (Exception e) { Logging.Log("Error updating log, details:\r\n" + e.Message); } }, null, 20, 300);
        public static List<string> lcnmid = new List<string>();
        #endregion
        [STAThread] //DO NOT REMOVE THIS
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles(); // at first enable styles.
            SetProcessDPIAware();
            //Catch any error during program runtime
            AppDomain.CurrentDomain.UnhandledException += (obj, arg) =>
            {
                var e = (Exception)arg.ExceptionObject;
                Logging.Log("Unexpected error occurred, FaineSwitch exited, error details:\r\n" + e.Message + "\r\n" + e.StackTrace, 1);
            };
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            if (System.Globalization.CultureInfo.InstalledUICulture.TwoLetterISOLanguageName == "ua")
            {
                Lang = Languages.Ukrainian;
            }
            MyConfs = new Configs();
            if (Configs.forceAppData && Configs.fine)
            {
                MyConfs.Write("Functions", "AppDataConfigs", "true");
            }
            Logging.Log("FaineSwitch started.");
            var ind = 0;
            using (var mutex = new Mutex(false, GGPU_Mutex))
            {
                if (!mutex.WaitOne(0, false))
                {
                    if (args.Length > 0)
                    {
                        var arg1 = args[ind].ToUpper();
                        if (arg1.StartsWith("/R") || arg1.StartsWith("-R") || arg1.StartsWith("R"))
                        {
                            ind = 1;
                            WinAPI.PostMessage((IntPtr)0xffff, re, 0, 0);
                            return;
                        }
                    }
                    WinAPI.PostMessage((IntPtr)0xffff, ao, 0, 0);
                    return;
                }
                if (args.Length > ind)
                {
                    var arg1 = args[ind].ToUpper();
                    if (arg1.StartsWith("/C") || arg1.StartsWith("-C") || arg1.StartsWith("C"))
                    {
                        if (args.Length > ind + 1)
                        {
                            var ok = false;
                            if (Directory.Exists(args[ind + 1]))
                            {
                                ok = true;
                            }
                            else
                            {
                                try
                                {
                                    Directory.CreateDirectory(args[ind + 1]);
                                    ok = true;
                                }
                                catch (Exception e)
                                {
                                    Logging.Log("Can't create directory: " + args[ind + 1]);
                                }
                            }
                            if (ok)
                            {
                                Logging.Log("Switching config directory to : " + args[ind + 1]);
                                SwitcherUI.nPath = args[ind + 1];
                                C_SWITCH = true;
                            }
                        }
                    }
                }
                if (Program.MyConfs.ReadBool("Functions", "AppDataConfigs"))
                {
                    var Switcher_folder_appd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FaineSwitch");
                    if (!Directory.Exists(Switcher_folder_appd))
                        Directory.CreateDirectory(Switcher_folder_appd);
                    if (!File.Exists(Path.Combine(Switcher_folder_appd, "FaineSwitch.ini"))) // Copy main configs to appdata
                        File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FaineSwitch.ini"),
                                  Path.Combine(Switcher_folder_appd, "FaineSwitch.ini"), true);
                    Configs.filePath = Path.Combine(Switcher_folder_appd, "FaineSwitch.ini");
                    MyConfs = new Configs();
                }
                else
                    Configs.filePath = Path.Combine(SwitcherUI.nPath, "FaineSwitch.ini");
                SwitcherUI.latest_save_dir = Configs.filePath;
                SwitcherUI.InitLanguage();
                RefreshLCnMID();
                //for first run, add your locale 1 & locale 2 to settings
                if (MyConfs.Read("Layouts", "MainLayout1") == "" && MyConfs.Read("Layouts", "MainLayout2") == "")
                {
                    Logging.Log("Initializing locales.");
                    MyConfs.Write("Layouts", "MainLayout1", lcnmid[0]);
                    if (lcnmid.Count > 1)
                        MyConfs.Write("Layouts", "MainLayout2", lcnmid[1]);
                    MyConfs.WriteToDisk();
                }
                switcher = new SwitcherUI();
                rif = new RawInputForm();
                Locales.IfLessThan2();
                //if (MyConfs.Read("Layouts", "MainLayout1") == "" && MyConfs.Read("Layouts", "MainLayout2") == "") {
                //	switcher.cbb_MainLayout1.SelectedIndex = 0;
                //	if (lcnmid.Count > 1)
                //		switcher.cbb_MainLayout2.SelectedIndex = 1;
                //}
                _evt_hookID = WinAPI.SetWinEventHook(WinAPI.EVENT_SYSTEM_FOREGROUND, WinAPI.EVENT_SYSTEM_FOREGROUND,
                                                     IntPtr.Zero, _evt_proc, 0, 0, WinAPI.WINEVENT_OUTOFCONTEXT);
                _LDevt_hookID = WinAPI.SetWinEventHook(WinAPI.EVENT_OBJECT_FOCUS, WinAPI.EVENT_OBJECT_FOCUS,
                                                     IntPtr.Zero, _LDevt_proc, 0, 0, WinAPI.WINEVENT_OUTOFCONTEXT);
                if (args.Length != 0)
                {
                    if (args[0] == "_!_updated_!_")
                    {
                        Logging.Log("FaineSwitch updated.");
                        switcher.ToggleVisibility();
                        MessageBox.Show(Lang[Languages.Element.UpdateComplete], Lang[Languages.Element.UpdateComplete],
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    if (args[0] == "_!_silent_updated_!_")
                    {
                        Logging.Log("FaineSwitch silently updated.");
                        switcher.icon.trIcon.Visible = true;
                        switcher.icon.trIcon.ShowBalloonTip(1000, Lang[Languages.Element.UpdateComplete], "FaineSwitch -> " + switcher.Text, ToolTipIcon.Info);
                        switcher.icon.trIcon.BalloonTipClicked += (_, __) => switcher.ToggleVisibility();
                        if (!SwitcherUI.TrayIconVisible)
                            KMHook.DoLater(() => switcher.Invoke((MethodInvoker)delegate { switcher.icon.trIcon.Visible = false; }), 1005);
                    }
                }
                MyConfs.WriteToDisk();
                if (!string.IsNullOrEmpty(SwitcherUI.MainLayout1))
                    SwitcherUI.GlobalLayout = SwitcherUI.currentLayout = Locales.GetLocaleFromString(SwitcherUI.MainLayout1).uId;
                Application.Run();
            }
        }
        public static void RefreshLCnMID()
        {
            Program.locales = Locales.AllList();
            lcnmid.Clear();
            foreach (var lc in locales)
            {
                lcnmid.Add(lc.Lang + "(" + lc.uId + ")");
            }
        }
        public static bool SwitcherActive()
        {
            var ActHandle = WinAPI.GetForegroundWindow();
            if (ActHandle == IntPtr.Zero)
            {
                return false;
            }
            var active = switcher.Handle == ActHandle;
            Logging.Log("FaineSwitch is active = [" + active + "]" + ", FaineSwitch handle [" + switcher.Handle + "], fore win handle [" + ActHandle + "]");
            return active;
        }
    }
}