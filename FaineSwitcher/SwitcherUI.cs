using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Web;

namespace FaineSwitcher
{
    public partial class SwitcherUI : Form
    {
        #region Variables
        // Hotkeys, HKC => HotKey Convert
        public Hotkey Mainhk, ExitHk, HKCLast, HKCSelection, HKCLine, HKSymIgn, HKConMorWor,
                        HKTitleCase, HKRandomCase, HKSwapCase, HKTransliteration, HKRestart,
                        HKToggleLP, HKShowST, HKToggleSwitcher, HKUpperCase, HKLowerCase, HKCycleCase,
                        HKSelCustConv, HKShCMenuUM;
        public List<Hotkey> SpecificSwitchHotkeys = new List<Hotkey>();
        /// <summary>
        /// Hotkey OK to fire action bools.
        /// </summary>
        public bool hksTTCOK, hksTRCOK, hksTSCOK, hksTrslOK, hkShWndOK, hkcwdsOK, hklOK,
                    hksOK, hklineOK, hkSIOK, hkExitOK, hkToglLPOK, hkShowTSOK, hkToggleSwitcherOK, hkUcOK, hklcOK, hkccOK,
                    hkSCCok, hkSCMUM;
        public static string nPath = AppDomain.CurrentDomain.BaseDirectory, CustomSound, CustomSound2, Redefines;
        public static int ACT_Match = 0, TrayHoverSwitcherMM = 0, explorer_pid;
        public static bool LoggingEnabled, dummy, CapsLockDisablerTimer, LangPanelUpperArrow, mouseLTUpperArrow, caretLTUpperArrow,
                           ShiftInHotkey, AltInHotkey, CtrlInHotkey, WinInHotkey, AutoStartAsAdmin, UseJKL, AutoSwitchEnabled, ReadOnlyNA,
                           SoundEnabled, UseCustomSound, SoundOnAutoSwitch, SoundOnConvLast, SoundOnSnippets, SoundOnLayoutSwitch,
                           UseCustomSound2, SoundOnAutoSwitch2, SoundOnConvLast2, SoundOnSnippets2, SoundOnLayoutSwitch2, TrOnDoubleClick,
                           TrEnabled, TrBorderAero, OnceSpecific, WriteInputHistory, ExcludeCaretLD, UsePaste,
                           WriteInputHistoryByDate, WriteInputHistoryHourly, SwitcherMM = false,
                           hk_result, multi_continue = true, ZxZ = true, configs_loading;
        static string[] UpdInfo;
        public static List<int> HKBlockAlt = new List<int>();
        public static bool BlockAltUpNOW = false;
        static bool updating, was, isold = true, checking, snip_checking, as_checking, check_ASD_size = true;
        public static bool ENABLED = true, reload_snip = false;
        #region Timers
        static Timer overlay_excluder;
        static Timer tmr = new Timer();
        static Timer old = new Timer();
        static Timer stimer = new Timer();
        static Timer animate = new Timer();
        static Timer showUpdWnd = new Timer();
        public Timer ICheck = new Timer();
        public Timer ScrlCheck = new Timer();
        public Timer crtCheck = new Timer();
        public Timer capsCheck = new Timer();
        public Timer flagsCheck = new Timer();
        public Timer persistentLayout1Check = new Timer();
        public Timer persistentLayout2Check = new Timer();
        public Timer langPanelRefresh = new Timer();
        public Timer res = new Timer();
        public Timer resC = new Timer();
        #endregion
        #region [Hidden]
        public static bool __setlayoutForce, __setlayoutOnlyWM, nomemoryflush, LibreCtrlAltShiftV, __selection, __selection_nomouse, CycleCaseReset,
                            OVEXDisabled, ClipBackOnlyText, SwitcherMMTrayHoverLostFocusClose, CycleCaseSaveBase, cmdbackfix;
        public static string ReselectCustoms, AutoCopyTranslation = "", onlySnippetsExcluded = "", onlyAutoSwitchExcluded = "", CycleCaseBase;
        static string CycleCaseOrder = "TULSR", OverlayExcluded, tas, ncs;
        static int OverlayExcludedInerval, arm;
        static Timer armt;
        #endregion
        static uint lastTrayFlagLayout = 0;
        public static Bitmap FLAG, ITEXT;
        static int progress = 0, _progress = 0;
        public string SnippetsExpandType = "", SnippetsExpKeyOther = "";
        int titlebar = 12;
        public static int AtUpdateShow, SpecKeySetCount, SnippetsCount, AutoSwitchCount, TrSetCount, InputHistoryBackSpaceWriteType;
        public int DoubleHKInterval = 200, SelectedTextGetMoreTriesCount, DelayAfterBackspaces, NCRSetsCount;
        #region Temporary variables
        /// <summary> Translate Panel Colors</summary>
        public static Color TrFore, TrBack, TrBorder;
        public static Font TrText, TrTitle;
        public static int TrTransparency, Layout1ModifierKey, Layout2ModifierKey, LayoutDModifierKey;
        /// <summary> In memory settings, for timers/hooks.</summary>
        public static bool DiffAppearenceForLayouts, LDForCaretOnChange, LDForMouseOnChange, ScrollTip, AddOneSpace,
                    TrayFlags, TrayText, SymIgnEnabled, TrayIconVisible, SnippetsEnabled, ChangeLayouByKey, EmulateLS,
                    RePress, BlockHKWithCtrl, blueIcon, SwitchBetweenLayouts, SelectedTextGetMoreTries, ReSelect,
                    ConvertSelectionLS, ConvertSelectionLSPlus, MCDSSupport, OneLayoutWholeWord,
                    MouseTTAlways, OneLayout, MouseLangTooltipEnabled, CaretLangTooltipEnabled, QWERTZ_fix,
                    ChangeLayoutInExcluded, SnippetSpaceAfter, SnippetsSwitchToGuessLayout,
                    AutoSwitchSpaceAfter, AutoSwitchSwitchToGuessLayout, GuessKeyCodeFix, Dowload_ASD_InZip,
                    LDForCaret, LDForMouse, LDUseWindowsMessages, RemapCapslockAsF18, Add1NL, PersistentLayoutOnWindowChange, PersistentLayoutOnlyOnce,
                    PersistentLayoutForLayout1, PersistentLayoutForLayout2, UseDelayAfterBackspaces,
                    ConvertSWLinExcl;
        /// <summary> Temporary modifiers of hotkeys. </summary>
        string Mainhk_tempMods, ExitHk_tempMods, HKCLast_tempMods, HKCSelection_tempMods,
                HKCLine_tempMods, HKSymIgn_tempMods, HKConMorWor_tempMods, HKTitleCase_tempMods,
                 HKRandomCase_tempMods, HKSwapCase_tempMods, HKTransliteration_tempMods, HKRestart_tempMods,
                 HKToggleLangPanel_tempMods, HKShowSelectionTranslate_tempMods, HKToggleSwitcher_tempMods, HKToUpper_tempMods,
                 HKToLower_tempMods, HKCycleCase_tempMods, HKSelCustConv_tempMods, HKShCMenuUM_tempMods;
        /// <summary> Temporary key of hotkeys. </summary>
        int Mainhk_tempKey, ExitHk_tempKey, HKCLast_tempKey, HKCSelection_tempKey,
                HKCLine_tempKey, HKSymIgn_tempKey, HKConMorWor_tempKey, HKTitleCase_tempKey,
                 HKRandomCase_tempKey, HKSwapCase_tempKey, HKTransliteration_tempKey, HKRestart_tempKey,
                 HKToggleLangPanel_tempKey, HKShowSelectionTranslate_tempKey, HKToggleSwitcher_tempKey, HKToUpper_tempKey,
                 HKToLower_tempKey, HKCycleCase_tempKey, HKSelCustConv_tempKey, HKShCMenuUM_tempKey;
        /// <summary> Temporary Enabled of hotkeys. </summary>
        bool Mainhk_tempEnabled, ExitHk_tempEnabled, HKCLast_tempEnabled, HKCSelection_tempEnabled,
                HKCLine_tempEnabled, HKSymIgn_tempEnabled, HKConMorWor_tempEnabled, HKTitleCase_tempEnabled,
                 HKRandomCase_tempEnabled, HKSwapCase_tempEnabled, HKTransliteration_tempEnabled, HKRestart_tempEnabled,
                 HKToggleLangPanel_tempEnabled, HKShowSelectionTranslate_tempEnabled, HKToggleSwitcher_tempEnabled,
                 HKToUpper_tempEnabled, HKToLower_tempEnabled, HKCycleCase_tempEnabled, HKShCMenuUM_tempEnabled;
        public static bool HKSelCustConv_tempEnabled;
        /// <summary> Temporary Double of hotkeys. </summary>
        bool Mainhk_tempDouble, ExitHk_tempDouble, HKCLast_tempDouble, HKCSelection_tempDouble,
                HKCLine_tempDouble, HKSymIgn_tempDouble, HKConMorWor_tempDouble, HKTitleCase_tempDouble,
                 HKRandomCase_tempDouble, HKSwapCase_tempDouble, HKTransliteration_tempDouble,
                 HKToggleLangPanel_tempDouble, HKShowSelectionTranslate_tempDouble, HKToggleSwitcher_tempDouble,
                 HKToUpper_tempDouble, HKToLower_tempDouble, HKCycleCase_tempDouble, HKSelCustConv_tempDouble, HKShCMenuUM_tempDouble;
        /// <summary> Temporary colors of LangDisplays appearece. </summary>
        public static Color LDMouseFore_temp, LDCaretFore_temp, LDMouseBack_temp, LDCaretBack_temp,
               Layout1Fore_temp, Layout2Fore_temp, Layout1Back_temp, Layout2Back_temp;
        /// <summary> Temporary fonts of LangDisplays appearece. </summary>
        public static Font LDMouseFont_temp, LDCaretFont_temp, Layout1Font_temp, Layout2Font_temp;
        /// <summary> Temporary use flags of LangDisplays appearece. </summary>
        public static bool LDMouseUseFlags_temp, LDCaretUseFlags_temp;
        /// <summary> Temporary transparent backgrounds of LangDisplays appearece. </summary>
        public static bool LDMouseTransparentBack_temp, LDCaretTransparentBack_temp,
              Layout1TransparentBack_temp, Layout2TransparentBack_temp;
        /// <summary> Temporary positions of LangDisplays appearece. </summary>
        public static int LDMouseY_Pos_temp, LDCaretY_Pos_temp, LDMouseX_Pos_temp, LDCaretX_Pos_temp,
               Layout1Y_Pos_temp, Layout2Y_Pos_temp, Layout1X_Pos_temp, Layout2X_Pos_temp,
               MCDS_Xpos_temp, MCDS_Ypos_temp, MCDS_TopIndent_temp, MCDS_BottomIndent_temp;

        private void chk_AppDataConfigs_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary> Temporary sizes of LangDisplays appearece. </summary>
        public static int LDMouseHeight_temp, LDCaretHeight_temp, LDMouseWidth_temp, LDCaretWidth_temp,
               Layout1Height_temp, Layout2Height_temp, Layout1Width_temp, Layout2Width_temp;
        /// <summary>
        /// Temporary list boxes indexes before and after settings loaded.
        /// </summary>
        public int tmpHotkeysIndex, tmpLangTTAppearenceIndex;
        /// <summary> Temporary hotkey key of hotkey in txt_Hotkey. </summary>
        int txt_Hotkey_tempKey;
        /// <summary> Temporary hotkey modifiers of hotkey in txt_Hotkey. </summary>
        string txt_Hotkey_tempModifiers;
        /// <summary> Temporary persistent layout's processes. </summary>
        public string PersistentLayout1Processes, PersistentLayout2Processes;
        /// <summary> Temporary layouts, etc.. </summary>
        public static string Layout1, Layout2, Layout3, Layout4,
            MainLayout1, MainLayout2, EmulateLSType, ExcludedPrograms, Layout1TText, Layout2TText;
        /// <summary> Temporary specific keys. </summary>
        public int Key1, Key2, Key3, Key4;
        /// <summary> LangPanel temporary bool variables. </summary>
        public static bool LangPanelDisplay, LangPanelBorderAero;
        /// <summary> LangPanel temporary int variables. </summary>
        public int LangPanelRefreshRate, LangPanelTransparency;
        /// <summary> LangPanel temporary color variables. </summary>
        public Color LangPanelForeColor, LangPanelBackColor, LangPanelBorderColor;
        /// <summary> LangPanel temporary position variable. </summary>
        public Point LangPanelPosition;
        /// <summary> LangPanel temporary font variable. </summary>
        public Font LangPanelFont;
        /// <summary> Static last layout for LangPanel. </summary>
        public static uint lastLayoutLangPanel = 0;
        ToolTip HelpMeUnderstand;
        #endregion
        public TrayIcon icon;
        public LangDisplay mouseLangDisplay = new LangDisplay();
        public LangDisplay caretLangDisplay = new LangDisplay();
        public LangPanel _langPanel;
        public TranslatePanel _TranslatePanel;
        uint latestL = 0, latestCL = 0;
        static string decim = ",";
        public static uint currentLayout, GlobalLayout;
        public static uint MAIN_LAYOUT1, MAIN_LAYOUT2, CTRL_ALT_TemporaryLayout;
        bool onepass = true, onepassC = true;
        /// <summary>
        /// Has a lot of values/keys taken from dynamic controls:<br/>
        /// txt_keyN - HotkeyBox,<br/> 
        /// ^---> ADDONS: _mods - to get modifiers, _key - to get keyCode.<br/>
        /// chk_winN - Use Win modifier in hotkey, <br/>
        /// lbl_arrN - Arrow label, [has no values]<br/>
        /// cbb_typN - Switch type(To specific layout or switch between).
        /// </summary>
        public Dictionary<string, string> SpecKeySetsValues = new Dictionary<string, string>();
        public static Dictionary<string, string> TrSetsValues = new Dictionary<string, string>();
        static string latestSwitch = "null";
        const string SYNC_HOST = "https://hastebin.com";
        const string SYNC_HOST2 = "https://0x0.st";
        const string SYNC_SEP = "#------>";
        readonly string[] SYNC_NAMES = { "FaineSwitcher.ini", "snippets.txt", "history.txt", "TSDict.txt", "FaineSwitcher.mm" };
        readonly string[] SYNC_TYPES = { "ini", "sni", "his", "tdi", "mm" };
        // From more configs
        ColorDialog clrd = new ColorDialog();
        FontDialog fntd = new FontDialog();
        public static FontConverter fcv = new FontConverter();
        public static string snipfile = Path.Combine(SwitcherUI.nPath, "snippets.txt");
        public static string AS_dictfile = Path.Combine(SwitcherUI.nPath, "AS_dict.txt");
        public static string switcher_folder_appd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FaineSwitcher");
        public static string latest_save_dir = "";
        public static string AutoSwitchDictionaryRaw = "";
        public static bool AutoSwitchDictionaryTooBig = false;
        public static Point LDC_lp = new Point(0, 0);
        public static int LD_MouseSkipMessagesCount = 0;
        System.Threading.Thread uche;
        public static List<IntPtr> PERSISTENT_LAYOUT1_HWNDs = new List<IntPtr>();
        public static List<IntPtr> NOT_PERSISTENT_LAYOUT1_HWNDs = new List<IntPtr>();
        public static List<IntPtr> PERSISTENT_LAYOUT2_HWNDs = new List<IntPtr>();
        public static List<IntPtr> NOT_PERSISTENT_LAYOUT2_HWNDs = new List<IntPtr>();
        #endregion
        public SwitcherUI()
        {
            DeleteTrash();
            Program.SWITCHER_HANDLE = Handle;
            InitializeComponent();
            HelpMeUnderstand = new ToolTip();
            HelpMeUnderstand.AutoPopDelay = 20000;
            HelpMeUnderstand.InitialDelay = 500;
            HelpMeUnderstand.ReshowDelay = 100;
            HelpMeUnderstand.ShowAlways = true;
            HelpMeUnderstand.ToolTipIcon = ToolTipIcon.Info;
            HelpMeUnderstand.Popup += HelpMeUnderstandPopup;
            if (Program.C_SWITCH)
            {
                chk_AppDataConfigs.Enabled = false;
            }
            // Visual designer always wants to put that string into resources, blast it!
            txt_Snippets.Text = "-><" + KMHook.__ANY__ + ">====><" + KMHook.__ANY__ + ">__cursorhere()</" + KMHook.__ANY__ + "><====" +
    "\r\n->nowtime====>__date(HH:mm:ss)<====\r\n->nowdate====>__date(dd/MM/yyyy)<====\r\n->datepretty====>__date(dd, ddd MMM)<====" +
    "\r\n->env_system====>__system()<====\r\n->date_esc====>\\__date(HH:mm:ss)<====";
            // Switch to more secure connection.
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            nud_LangTTPositionX.Minimum = nud_LangTTPositionY.Minimum = -100;
            LoadConfigs();
            InitializeListBoxes();
            // Set minnimum values because they're ALWAYS restores to 0 after Form Editor is used.
            nud_CapsLockRefreshRate.Minimum = nud_DoubleHK2ndPressWaitTime.Minimum =
                nud_LangTTCaretRefreshRate.Minimum = nud_LangTTMouseRefreshRate.Minimum =
                nud_ScrollLockRefreshRate.Minimum = nud_TrayFlagRefreshRate.Minimum =
                nud_PersistentLayout1Interval.Minimum = nud_PersistentLayout2Interval.Minimum = 1;
            // Disable horizontal scroll
            //pan_TrSets.AutoScroll = pan_KeySets.AutoScroll = false;
            //pan_TrSets.HorizontalScroll.Maximum = pan_KeySets.HorizontalScroll.Maximum = 0;
            //pan_TrSets.AutoScroll = pan_KeySets.AutoScroll = true;
            Text = "FaineSwitcher ";
            //if (____.commit != "") {
            //	Text += " <"+____.commit+">";
            //	Program.MyConfs.Write("Updates", "LatestCommit", ____.commit);
            //	Program.MyConfs.WriteToDisk();
            //}
            //else {
            //	var commit = Program.MyConfs.Read("Updates", "LatestCommit");
            //	if (Program.MyConfs.Read("Updates", "LatestCommit").Length == 7)
            //		Text += " <"+commit+">";
            //}
            var mult = 4;
            if (KMHook.IfNW7())
                mult = 6;
            if (tabs.RowCount >= 2)
                mult++;
            if (tabs.RowCount >= 3)
                mult += 2;
            var pty = tabs.RowCount * mult;
            var lsy = btn_OK.Location.Y + pty;
            btn_Apply.Location = new Point(btn_Apply.Location.X, lsy);
            btn_Cancel.Location = new Point(btn_Cancel.Location.X, lsy);
            btn_OK.Location = new Point(btn_OK.Location.X, lsy);
            tabs.Height += pty;
            Height += pty;

            RegisterHotkeys();
            RefreshAllIcons();
            //Background startup check for updates (disable for now)
            if (Program.MyConfs.ReadBool("Functions", "StartupUpdatesCheck") && false)
            {
                uche = new System.Threading.Thread(StartupCheck);
                uche.Name = "Startup Check";
                uche.Start();
                showUpdWnd.Tick += (_, __) =>
                {
                    Logging.Log("Checking: " + AtUpdateShow);
                    if (AtUpdateShow == 1)
                    {
                        if (Program.MyConfs.ReadBool("Functions", "SilentUpdate"))
                        {
                            Btn_DownloadUpdateClick((object)0, new EventArgs());
                            Logging.Log("Silent UPDATE!");
                        }
                        else
                        {
                            tabs.SelectedIndex = tabs.TabPages.IndexOf(tab_updates);
                            SetUInfo();
                            Visible = TopMost = true;
                            grb_DownloadUpdate.Enabled = true;
                            btn_DownloadUpdate.PerformClick();
                        }
                        showUpdWnd.Stop();
                        showUpdWnd.Dispose();
                    }
                    if (AtUpdateShow == 2)
                    {
                        Logging.Log("No new version updates found.");
                        showUpdWnd.Stop();
                        showUpdWnd.Dispose();
                    }
                    if (AtUpdateShow == 3)
                    {
                        Logging.Log("Network error.", 1);
                        showUpdWnd.Stop();
                        showUpdWnd.Dispose();
                    }
                };
                showUpdWnd.Interval = 1000;
                showUpdWnd.Start();
            }
            else { showUpdWnd.Dispose(); }

            cbb_Language.SelectedItem = Program.MyConfs.Read("Appearence", "Language");

            DPISCALE(this);
            Memory.Flush();
        }
        static NotifyIcon[] Ticons;
        static Timer ttmr;
        static bool[] nvisible;
        public static void NCS_tray()
        {
            var icons = new[] { Properties.Resources.num, Properties.Resources.caps, Properties.Resources.scr };
            var icons_on = new[] { Properties.Resources.num_on, Properties.Resources.caps_on, Properties.Resources.scr_on };
            if (Ticons == null)
                Ticons = new NotifyIcon[3];
            if (nvisible != null)
            {
                if (String.IsNullOrEmpty(ncs) && (nvisible[0] || nvisible[1] || nvisible[2]))
                {
                    NCS_destroy(); return;
                }
                else
                {
                    var visible = new[] { ncs.Contains("N"), ncs.Contains("C"), ncs.Contains("S") };
                    for (int v = 0; v < 3; v++)
                    {
                        if (Ticons[v] == null) continue;
                        if (!visible[v] || !nvisible[v])
                        { // if one of them off, remove 
                            Ticons[v].Visible = false;
                        }
                    }
                }
            }
            nvisible = new[] { ncs.Contains("N"), ncs.Contains("C"), ncs.Contains("S") };
            var Tstates = new bool[3] { Control.IsKeyLocked(Keys.NumLock), Control.IsKeyLocked(Keys.CapsLock), Control.IsKeyLocked(Keys.Scroll) };
            for (int v = 0; v < 3; v++)
            {
                if (!nvisible[v]) continue;
                NotifyIcon t;
                if (Ticons[v] == null) t = new NotifyIcon(); else t = Ticons[v];
                if (Tstates[v])
                    t.Icon = Icon.FromHandle(icons_on[v].GetHicon());
                else
                    t.Icon = Icon.FromHandle(icons[v].GetHicon());
                Ticons[v] = t;
            }
            for (int v = 2; v >= 0; v--)
            {
                if (!nvisible[v]) continue;
                Ticons[v].Visible = true;
            }
            if (ttmr == null)
            {
                ttmr = new Timer();
                ttmr.Tick += (_, __) =>
                {
                    var tTstates = new bool[3] { Control.IsKeyLocked(Keys.NumLock), Control.IsKeyLocked(Keys.CapsLock), Control.IsKeyLocked(Keys.Scroll) };
                    int diff = 0;
                    for (int v = 0; v < 3; v++)
                    {
                        if (!nvisible[v]) continue;
                        var t = Ticons[v];
                        if (Tstates[v] == tTstates[v]) continue;
                        diff++;
                        if (tTstates[v])
                            t.Icon = Icon.FromHandle(icons_on[v].GetHicon());
                        else
                            t.Icon = Icon.FromHandle(icons[v].GetHicon());
                    }
                    if (diff > 0)
                        Tstates = tTstates;
                };
                ttmr.Interval = 50;
                ttmr.Start();
            }
        }
        public static void NCS_destroy()
        {
            if (ttmr != null)
            {
                ttmr.Stop();
                ttmr.Dispose();
                ttmr = null;
            }
            if (Ticons != null)
            {
                for (int v = 0; v < 3; v++)
                {
                    if (Ticons[v] == null) continue;
                    if (Ticons[v].Visible) Ticons[v].Visible = false;
                    Ticons[v].Dispose();
                }
                Ticons = null;
            }
        }
        public static double xr = 1, yr = 1;
        public static void DPISCALE_CONTROL(Control c)
        {
            int ww = Convert.ToInt32(Convert.ToDouble(c.Width) * xr);
            int hh = Convert.ToInt32(Convert.ToDouble(c.Height) * yr);
            c.Width = ww;
            c.Height = hh;
            //			Debug.WriteLine(ww + " x " + hh);
        }
        public static void DPIPOS_CONTROL(Control c)
        {
            int xx = Convert.ToInt32(Convert.ToDouble(c.Location.X) * xr);
            int yy = Convert.ToInt32(Convert.ToDouble(c.Location.Y) * yr);
            c.Location = new Point(xx, yy);
            //			Debug.WriteLine(xx + " p " + yy);
        }
        public static Control[] ALLCONTROLS(Control c)
        {
            if (c == null) { return new Control[] { }; }
            List<Control> ctrls = new List<Control>();
            foreach (Control con in c.Controls)
            {
                ctrls.Add(con);
                ctrls.AddRange(ALLCONTROLS(con));
            }
            return ctrls.ToArray();
        }
        public static void DPISCALE(Control cxx, bool nox = false)
        {
            float dx, dy;
            Graphics g = cxx.CreateGraphics();
            try { dx = g.DpiX; dy = g.DpiY; }
            finally { g.Dispose(); }
            var es = Convert.ToSingle(96);
            if (dx.Equals(es) && dy.Equals(es)) { return; }
            xr = dx / 96;
            yr = dx / 96;
            Control[] Cons = new Control[] { cxx /*, _TranslatePanel, _langPanel, caretLangDisplay, mouseLangDisplay*/};
            foreach (Control x in Cons)
            {
                var cs = ALLCONTROLS(x);
                if (!nox)
                {
                    DPIPOS_CONTROL(x);
                    DPISCALE_CONTROL(x);
                }
                foreach (var c in cs)
                {
                    DPIPOS_CONTROL(c);
                    DPISCALE_CONTROL(c);
                }
            }
        }
        public static void chrome_window_alt_fix()
        {
            var last = Locales.ActiveWindow();
            var f = new Form();
            f.FormBorderStyle = FormBorderStyle.None;
            f.MaximizeBox = f.MinimizeBox = false;
            f.TopMost = true;
            f.Width = f.Height = 1;
            f.Location = new Point(0, 0);
            f.Show();
            WinAPI.SetForegroundWindow(f.Handle);
            KInputs.MakeInput(new[] {
                              KInputs.AddKey(Keys.LMenu, false),
                              KInputs.AddKey(Keys.RMenu, false)});
            System.Threading.Thread.Sleep(1);
            f.Dispose();
            WinAPI.SetForegroundWindow(last);
        }
        #region WndProc(Hotkeys) & Functions
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Program.ao)
            { // ao = Already Opened
                ToggleVisibility();
                Logging.Log("Another instance detected, closing it.");
            }
            if (m.Msg == Program.re)
            { // Restart FaineSwitcher
                Logging.Log("Restarting FaineSwitcher from command line...");
                Restart();
            }
            if (m.Msg == WinAPI.WM_MOUSEWHEEL)
            {
                if (WinAPI.WindowFromPoint(Cursor.Position) == tabs.Handle)
                {
                    try
                    {
                        if (((uint)m.WParam >> 16) == 120)
                        {
                            if (tabs.SelectedIndex + 1 > tabs.TabPages.Count - 1)
                                tabs.SelectedIndex = 0;
                            else
                                tabs.SelectedIndex += 1;
                        }
                        else
                        {
                            if (tabs.SelectedIndex - 1 < 0)
                                tabs.SelectedIndex = tabs.TabPages.Count - 1;
                            else
                                tabs.SelectedIndex -= 1;
                        }
                    }
                    catch (Exception e) { Logging.Log("Error in tabs wheel scroll, details: " + e.Message + "\r\n" + e.StackTrace + "\r\n"); }
                    tabs.Focus();
                }
            }
            //			Logging.Log("MSG: "+m.Msg+", LP: "+m.LParam+", WP: "+m.WParam+", KMS: "+KMHook.self+" 0x312");
            if (m.Msg == WinAPI.WM_HOTKEY)
            {
                if (HKBlockAlt.Contains(m.WParam.ToInt32()))
                {
                    BlockAltUpNOW = true;
                }
                var id = (Hotkey.HKID)m.WParam.ToInt32();
                var mods = (int)m.LParam & 0xFFFF;
                if (mods == WinAPI.MOD_ALT)
                { // Experimental fix for [only Alt] + something.
                    if (Locales.ActiveWindowClassName(40).Contains("Chrome_WidgetWin"))
                    {
                        chrome_window_alt_fix();
                    }
                    else
                        KInputs.MakeInput(new[] {
                                          KInputs.AddKey(Keys.LMenu, false),
                                          KInputs.AddKey(Keys.RMenu, false),
                                          KInputs.AddKey(Keys.LMenu, true)});

                }
                if (m.WParam.ToInt32() == 774)
                {
                    AutoSwitchEnabled = !AutoSwitchEnabled;
                    ShowTooltip(Program.Lang[Languages.Element.tab_AutoSwitch] + ": " + (AutoSwitchEnabled ? "ON" : "OFF"), 1000);
                    if (AutoSwitchEnabled && (KMHook.as_corrects == null))
                    {
                        if (File.Exists(AS_dictfile))
                        {
                            AutoSwitchDictionaryRaw = File.ReadAllText(AS_dictfile);
                            AutoSwitchDictionaryTooBig = AutoSwitchDictionaryRaw.Length > 710000;
                            ChangeAutoSwitchDictionaryTextBox();
                            UpdateSnippetCountLabel(AutoSwitchDictionaryRaw, lbl_AutoSwitchWordsCount, false);
                        }
                        KMHook.ReInitSnippets();
                        Debug.WriteLine("Reinit AutoSwitch Dictionary");
                    }
                    Debug.WriteLine("ToggleAutoSwitch..$" + AutoSwitchEnabled);
                }
                #region Convert multiple words 
                if (m.WParam.ToInt32() >= 100 && m.WParam.ToInt32() <= 109 && KMHook.waitfornum)
                {
                    int wordnum = m.WParam.ToInt32() - 100;
                    if (wordnum == 0) wordnum = 10;
                    Logging.Log("Attempt to convert " + wordnum + " word(s).");
                    var words = new List<KMHook.YuKey>();
                    try
                    {
                        var wasLocale = Locales.GetCurrentLocale();
                        if (SwitcherUI.UseJKL && !KMHook.JKLERR)
                            wasLocale = SwitcherUI.currentLayout;
                        var desl = KMHook.GetNextLayout(wasLocale).uId;
                        for (int w = Program.c_words.Count - wordnum; w != Program.c_words.Count; w++)
                        {
                            var mt = KMHook.LayoutKeyReplace(Program.c_words[w], (int)(wasLocale >> 16), (int)(desl >> 16)).ToArray();
                            words.AddRange(mt);
                        }
                        Logging.Log("Full character count in all " + wordnum + " last word(s) is " + words.Count + ".");
                    }
                    catch
                    {
                        Logging.Log("Converting " + wordnum + " word(s) impossible it is bigger that entered words.");
                    }
                    FlushConvertMoreWords();
                    KMHook.ConvertLast(words, true);
                }
                else if (KMHook.waitfornum) { FlushConvertMoreWords(); }
                #endregion
                var key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                #region Redefines
                if (LLHook.redefines.len > 0)
                {
                    for (int i = 0; i < LLHook.redefines.len; i++)
                    {
                        if (key == LLHook.redefines[i].v)
                        {
                            KMHook.skip_up = LLHook.redefines[i].v;
                        }
                    }
                }
                #endregion
                #region SpecificKeys			
                var specific = false;
                if (m.WParam.ToInt32() >= 201 && m.WParam.ToInt32() <= 299 &&
                    (SwitcherUI.ChangeLayoutInExcluded || !KMHook.ExcludedProgram()))
                {
                    specific = true;
                    if (!OnceSpecific)
                    {
                        OnceSpecific = true;
                        var si = m.WParam.ToInt32() - 200;
                        var type = SpecKeySetsValues["cbb_typ" + si];
                        try
                        {
                            if (mods != 0 && EmulateLS)
                            {
                                KMHook.SendModsUp(mods);
                            }
                            if (type == Program.Lang[Languages.Element.SwitchBetween])
                            {
                                KMHook.ChangeLayout();
                            }
                            else KMHook.ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(type).uId);
                            if (key == Keys.CapsLock)
                            {
                                KMHook.DoSelf(() =>
                                {
                                    if (Control.IsKeyLocked(Keys.CapsLock))
                                    {
                                        KMHook.KeybdEvent(Keys.CapsLock, 0);
                                        KMHook.KeybdEvent(Keys.CapsLock, 2);
                                    }
                                }, "specific_keys_capslock");
                            }
                        }
                        catch (Exception e)
                        {
                            Logging.Log("Possibly layout switch type was not selected for " + OemReadable((SpecKeySetsValues["txt_key" + si + "_mods"].Replace(",", " +") + " + " +
                                                                                                           Remake(key)).Replace("None + ", "")) + ". Layout string: [" + type + "]. Exception: " + e.Message + "\r\n" + e.StackTrace, 2);
                        }
                    }
                }
                #endregion
                if ((!KMHook.ExcludedProgram() || ConvertSWLinExcl) && !specific)
                {
                    var clcs = Hotkey.GetMods(HKCSelection_tempMods) == Hotkey.GetMods(HKCLast_tempMods) &&
                                HKCSelection_tempKey == HKCLast_tempKey &&
                                HKCLast_tempEnabled && HKCSelection_tempEnabled;
                    if (clcs && HKCSelection_tempDouble == HKCLast_tempDouble)
                        Hotkey.CallHotkey(HKCLast, id, ref hksOK, KMHook.ConvertSelection); // Use HKCLast id for cs if hotkeys are the same
                    else
                        Hotkey.CallHotkey(HKCSelection, id, ref hksOK, KMHook.ConvertSelection);
                    var clcl = false; // Convert Line + Convert Last
                    var conv = false;
                    if (Hotkey.GetMods(HKCLine_tempMods) == Hotkey.GetMods(HKCLast_tempMods) &&
                        HKCLine_tempKey == HKCLast_tempKey && HKCLine_tempDouble != HKCLast_tempDouble)
                    {
                        clcl = true;
                        var lastcl = hklineOK;
                        Hotkey.CallHotkey(HKCLine, Hotkey.HKID.ConvertLastLine, ref hklineOK, () =>
                        {
                            ConvertLastLine();
                            Debug.WriteLine("DISPOSING STIMER");
                            stimer.Dispose();
                            conv = true;
                        });
                        if (!lastcl && !conv)
                        {
                            stimer = new Timer();
                            stimer.Interval = DoubleHKInterval + 50;
                            stimer.Tick += (_, __) =>
                            {
                                if (!hklineOK && !conv) // Even here !conv because of time delay!
                                    Hotkey.CallHotkey(HKCLast, id, ref hklOK, () => KMHook.ConvertLast(Program.c_word));
                                Debug.WriteLine("STOPING STIMER");
                                stimer.Stop();
                                Debug.WriteLine("DISPOSING STIMER");
                                stimer.Dispose();
                            };
                            Debug.WriteLine("STARTIN STIMER");
                            stimer.Start();
                        }
                    }
                    if (!clcl)
                    {
                        if (clcs && HKCSelection_tempDouble && !HKCLast_tempDouble)
                        {
                            if (!hklOK)
                            {
                                hklOK = true;
                                //								Debug.WriteLine("hklOK NOT");
                                KMHook.doublekey.Interval = Program.switcher.DoubleHKInterval;
                                KMHook.doublekey.Start();
                                var clcst = new Timer();
                                clcst.Interval = Program.switcher.DoubleHKInterval + 25;
                                clcst.Tick += (_, __) =>
                                {
                                    if (!hklOK)
                                    {
                                        Hotkey.CallHotkey(HKCLast, id, ref hklOK, () => KMHook.ConvertLast(Program.c_word));
                                    }
                                    clcst.Stop(); clcst.Dispose();
                                };
                                clcst.Start();
                            }
                            else
                                Hotkey.CallHotkey(HKCLast, id, ref hklOK, KMHook.ConvertSelection);
                        }
                        else
                            Hotkey.CallHotkey(HKCLast, id, ref hklOK, () => KMHook.ConvertLast(Program.c_word));
                    }
                    Hotkey.CallHotkey(HKCLine, id, ref hklineOK, ConvertLastLine);
                }
                if (!KMHook.ExcludedProgram() && !specific)
                {
                    Hotkey.CallHotkey(HKCycleCase, id, ref hkccOK, CycleCase);
                    Hotkey.CallHotkey(HKTitleCase, id, ref hksTTCOK, () => KMHook.SelectionConversion(KMHook.ConvT.Title));
                    Hotkey.CallHotkey(HKSwapCase, id, ref hksTSCOK, () => KMHook.SelectionConversion(KMHook.ConvT.Swap));
                    Hotkey.CallHotkey(HKUpperCase, id, ref hkUcOK, () => KMHook.SelectionConversion(KMHook.ConvT.Upper));
                    Hotkey.CallHotkey(HKLowerCase, id, ref hklcOK, () => KMHook.SelectionConversion(KMHook.ConvT.Lower));
                    Hotkey.CallHotkey(HKRandomCase, id, ref hksTRCOK, () => KMHook.SelectionConversion(KMHook.ConvT.Random));
                    Hotkey.CallHotkey(HKConMorWor, id, ref hkcwdsOK, PrepareConvertMoreWords);
                    Hotkey.CallHotkey(HKTransliteration, id, ref hksTrslOK, () => KMHook.SelectionConversion(KMHook.ConvT.Transliteration));
                    Hotkey.CallHotkey(HKSelCustConv, id, ref hkSCCok, () => KMHook.SelectionConversion(KMHook.ConvT.Custom));
                    Hotkey.CallHotkey(HKShCMenuUM, id, ref hkSCMUM, ShowContextMenuUnderMouse);
                    ShiftInHotkey = Hotkey.ContainsModifier(((int)m.LParam & 0xFFFF), (int)WinAPI.MOD_SHIFT) ? true : false;
                    AltInHotkey = Hotkey.ContainsModifier(((int)m.LParam & 0xFFFF), (int)WinAPI.MOD_ALT) ? true : false;
                    CtrlInHotkey = Hotkey.ContainsModifier(((int)m.LParam & 0xFFFF), (int)WinAPI.MOD_CONTROL) ? true : false;
                    WinInHotkey = Hotkey.ContainsModifier(((int)m.LParam & 0xFFFF), (int)WinAPI.MOD_WIN) ? true : false;
                    KMHook.csdoing = false;
                }
                if (HKSymIgn.Enabled)
                {
                    Hotkey.CallHotkey(HKSymIgn, id, ref hkSIOK, ToggleSymIgn);
                }
                Hotkey.CallHotkey(HKRestart, id, ref dummy, Restart);
                Hotkey.CallHotkey(Mainhk, id, ref hkShWndOK, ToggleVisibility);
                Hotkey.CallHotkey(HKToggleLP, id, ref hkToglLPOK, ToggleLangPanel);
                Hotkey.CallHotkey(ExitHk, id, ref hkExitOK, ExitProgram);
                Hotkey.CallHotkey(HKShowST, id, ref hkShowTSOK, () => ShowSelectionTranslation());
                Hotkey.CallHotkey(HKToggleSwitcher, id, ref hkToggleSwitcherOK, ToggleSwitcher);
                //				if (m.WParam.ToInt32() <= (int)Hotkey.HKID.TransliterateSelection)
                //					KMHook.ClearModifiers();
                UpdateLDs();
                // Fix for experimental alt-only + something;
                KInputs.MakeInput(new[] { KInputs.AddKey(Keys.LMenu, false) });
            }
            base.WndProc(ref m);
        }
        static void ToggleSymIgn()
        {
            if (SymIgnEnabled)
            {
                SymIgnEnabled = false;
                Program.MyConfs.WriteSave("Functions", "SymbolIgnoreModeEnabled", "false");
                Program.switcher.Icon = Program.switcher.icon.trIcon.Icon = Properties.Resources.FaineSwitcher;
            }
            else
            {
                Program.MyConfs.WriteSave("Functions", "SymbolIgnoreModeEnabled", "true");
                SymIgnEnabled = true;
                Program.switcher.Icon = Program.switcher.icon.trIcon.Icon = Properties.Resources.FaineSwitcher;
            }
        }
        static void ConvertLastLine()
        {
            var line = new List<KMHook.YuKey>();
            var wasLocale = Locales.GetCurrentLocale();
            if (SwitcherUI.UseJKL && !KMHook.JKLERR)
                wasLocale = SwitcherUI.currentLayout;
            var desl = KMHook.GetNextLayout(wasLocale).uId;
            for (int w = 0; w != Program.c_words.Count; w++)
            {
                var mt = KMHook.LayoutKeyReplace(Program.c_words[w], (int)(wasLocale >> 16), (int)(desl >> 16)).ToArray();
                line.AddRange(mt);
                foreach (var x in mt)
                {
                    Debug.WriteLine("KK: " + x.key);
                }
            }
            KMHook.ConvertLast(line, true);
        }
        static bool tooltip = true;
        public static void ShowTooltip(string text, int time)
        {
            if (tooltip)
            {
                tooltip = false;
                var s = new Form();
                s.FormBorderStyle = FormBorderStyle.None;
                s.MinimumSize = new Size(2, 2);
                s.TopMost = true;
                var i = new TextBox();
                i.ReadOnly = true;
                i.TabStop = false;
                i.Text = "FaineSwitcher: " + text;
                i.Size = TextRenderer.MeasureText(i.Text, i.Font);
                if (text.Contains("\r\n"))
                {
                    i.Multiline = true;
                }
                i.AutoSize = true;
                i.Height = i.Font.Height * (i.Multiline ? 2 : 1) + 10;
                i.BorderStyle = BorderStyle.None;
                s.BackColor = i.BackColor = Color.Black;
                i.ForeColor = Color.White;
                i.TextAlign = HorizontalAlignment.Left;
                s.Size = new Size(i.Size.Width + 1, i.Size.Height - 6);
                s.Controls.Add(i);
                i.Location = new Point(0, 0);
                s.Show();
                s.Activate();
                s.Location = new Point(Cursor.Position.X, Cursor.Position.Y - 20);
                var t = new Timer();
                t.Interval = time;
                t.Tick += (_, __) =>
                {
                    tooltip = true;
                    s.Close();
                    s.Dispose();
                    t.Stop();
                    t.Dispose();
                };
                t.Start();
            }
        }
        public static void CCReset(string info = "")
        {
            if (CycleCaseSaveBase)
            {
                Debug.WriteLine("CC [B]ase reset by: " + info);
                CycleCaseBase = "";
            }
            if (CycleCaseReset)
            {
                CCPos = 0;
                if (!string.IsNullOrEmpty(info))
                {
                    Logging.Log("CCPos reset, " + info);
                    Debug.WriteLine("CCPos reset, " + info);
                }
            }
        }
        public static int CCPos = 0;
        public void CycleCase()
        {
            if (CCPos >= CycleCaseOrder.Length)
                CCPos = 0;
            var a = char.ToUpper(CycleCaseOrder[CCPos]);
            switch (a)
            {
                case 'T':
                    KMHook.SelectionConversion(KMHook.ConvT.Title); break;
                case 'U':
                    KMHook.SelectionConversion(KMHook.ConvT.Upper); break;
                case 'L':
                    KMHook.SelectionConversion(KMHook.ConvT.Lower); break;
                case 'S':
                    KMHook.SelectionConversion(KMHook.ConvT.Swap); break;
                case 'R':
                    KMHook.SelectionConversion(KMHook.ConvT.Random); break;
                case 'B':
                    var cbe = String.IsNullOrEmpty(CycleCaseBase);
                    var r = CycleCaseSaveBase ? (cbe ? "Emtpy, needs saving first" : "???") : "not enabled";
                    if (!CycleCaseSaveBase || cbe)
                    {
                        Debug.WriteLine("Skip CC [B]: " + r);
                        CCPos++;
                        CycleCase();
                        break;
                    }
                    if (!cbe)
                    {
                        Debug.WriteLine("CC [B]: Inputting: " + CycleCaseBase);
                        KMHook.DoSelf(() =>
                        {
                            KMHook.ClearModifiers();
                            if (SwitcherUI.UsePaste)
                            {
                                KMHook.PasteText(CycleCaseBase, "Base");
                            }
                            else
                            {
                                KInputs.MakeInput(KInputs.AddString(CycleCaseBase));
                            }
                            SwitcherUI.hk_result = true;
                            KMHook.ReSelect(CycleCaseBase.Length, "B");
                        }, "Cycle Case [B]ase restore");
                    }
                    break;
                default:
                    Logging.Log("I have no idea what: " + a + " means..."); break;
            }
            Debug.WriteLine("Hail to the [" + a + "]");
            CCPos++;
        }
        public static Point last_CR = new Point(0, 0);
        public void ToggleSwitcher()
        {
            if (ENABLED)
            {
                PreExit(false, 2);
                Program.c_word.Clear();
                Program.c_words.Clear();
                KMHook.c_snip.Clear();
                InitLangDisplays(true);
                Text = Text.Replace(" [" + Program.Lang[Languages.Element.Disabled] + "]", "");
                Text += " [" + Program.Lang[Languages.Element.Disabled] + "]";
                icon.trIcon.Text = icon.trIcon.Text.Replace(" [" + Program.Lang[Languages.Element.Disabled] + "]", "");
                icon.trIcon.Text += " [" + Program.Lang[Languages.Element.Disabled] + "]";
                icon.trIcon.Icon = Properties.Resources.FaineSwitcher;
                ENABLED = false;
            }
            else
            {
                ENABLED = true;
                RegisterHotkeys();
                Program.rif.RegisterRawInputDevices(Program.rif.Handle);
                if (LLHook._ACTIVE)
                {
                    LLHook.Set();
                }
                InitLangDisplays();
                ToggleTimers();
                if (UseJKL)
                    jklXHidServ.Init();
                Text = Text.Replace(" [" + Program.Lang[Languages.Element.Disabled] + "]", "");
                icon.trIcon.Text = icon.trIcon.Text.Replace(" [" + Program.Lang[Languages.Element.Disabled] + "]", "");
                ChangeTrayIconToFlag(true);
            }
            icon.CheckEnDis(ENABLED);
            Logging.Log("Switched FaineSwitcher enabled state to: [" + ENABLED + "].");
        }
        public static void ShowSelectionTranslation(bool mouse = false)
        {
            if (!TrEnabled) return;
            //			var dum = new Point(0,0);
            //			var pos = CaretPos.GetCaretPointToScreen(out dum);
            //			Debug.WriteLine(pos.X);
            //			if (mouse || pos.Equals(new Point(77777,77777)) || pos == last_CR)
            //				pos = Cursor.Position;
            var pos = Cursor.Position;
            pos.Y += 10;
            var str = KMHook.GetClipStr().Replace('\n', ' ');
            Debug.WriteLine(str);
            if (!string.IsNullOrEmpty(str))
            {
                if (!TranslatePanel.running)
                {
                    Program.switcher._TranslatePanel.ShowTranslation(str, pos);
                    SwitcherUI.hk_result = true;
                }
            }
            if (ACT_Match < 1)
                KMHook.RestoreClipBoard();
            else
                ACT_Match--;
            if (!mouse) last_CR = pos;
        }
        /// <summary>
        /// Restores temporary variables from settings.
        /// </summary>
        void LoadTemps()
        {
            //This creates(silently) new config file if existed one disappeared o_O
            // Restores temps
            #region Hotkey enableds
            Mainhk_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "ToggleMainWindow_Enabled");
            HKCLast_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "ConvertLastWord_Enabled");
            HKCSelection_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "ConvertSelectedText_Enabled");
            HKCLine_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "ConvertLastLine_Enabled");
            HKConMorWor_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "ConvertLastWords_Enabled");
            HKSymIgn_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "ToggleSymbolIgnoreMode_Enabled");
            HKTitleCase_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "SelectedTextToTitleCase_Enabled");
            HKRandomCase_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "SelectedTextToRandomCase_Enabled");
            HKSwapCase_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "SelectedTextToSwapCase_Enabled");
            HKToUpper_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "SelectedToUpper_Enabled");
            HKToLower_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "SelectedToLower_Enabled");
            HKTransliteration_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "SelectedTextTransliteration_Enabled");
            ExitHk_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "ExitSwitcher_Enabled");
            HKRestart_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "RestartSwitcher_Enabled");
            HKToggleLangPanel_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "ToggleLangPanel_Enabled");
            HKShowSelectionTranslate_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "ShowSelectionTranslate_Enabled");
            HKToggleSwitcher_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "ToggleSwitcher_Enabled");
            HKCycleCase_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "CycleCase_Enabled");
            HKSelCustConv_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "SelectedTextToCustomConv_Enabled");
            HKShCMenuUM_tempEnabled = Program.MyConfs.ReadBool("Hotkeys", "ShowCMenuUnderMouse_Enabled");
            #endregion
            #region Hotkey doubles
            Mainhk_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "ToggleMainWindow_Double");
            HKCLast_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "ConvertLastWord_Double");
            HKCSelection_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "ConvertSelectedText_Double");
            HKCLine_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "ConvertLastLine_Double");
            HKConMorWor_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "ConvertLastWords_Double");
            HKSymIgn_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "ToggleSymbolIgnoreMode_Double");
            HKTitleCase_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "SelectedTextToTitleCase_Double");
            HKRandomCase_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "SelectedTextToRandomCase_Double");
            HKSwapCase_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "SelectedTextToSwapCase_Double");
            HKToUpper_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "SelectedToUpper_Double");
            HKToLower_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "SelectedToLower_Double");
            HKTransliteration_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "SelectedTextTransliteration_Double");
            ExitHk_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "ExitSwitcher_Double");
            HKToggleLangPanel_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "ToggleLangPanel_Double");
            HKShowSelectionTranslate_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "ShowSelectionTranslate_Double");
            HKToggleSwitcher_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "ToggleSwitcher_Double");
            HKCycleCase_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "CycleCase_Double");
            HKSelCustConv_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "SelectedTextToCustomConv_Double");
            HKShCMenuUM_tempDouble = Program.MyConfs.ReadBool("Hotkeys", "ShowCMenuUnderMouse_Double");
            #endregion
            #region Hotkey modifiers
            Mainhk_tempMods = Program.MyConfs.Read("Hotkeys", "ToggleMainWindow_Modifiers");
            HKCLast_tempMods = Program.MyConfs.Read("Hotkeys", "ConvertLastWord_Modifiers");
            HKCSelection_tempMods = Program.MyConfs.Read("Hotkeys", "ConvertSelectedText_Modifiers");
            HKCLine_tempMods = Program.MyConfs.Read("Hotkeys", "ConvertLastLine_Modifiers");
            HKConMorWor_tempMods = Program.MyConfs.Read("Hotkeys", "ConvertLastWords_Modifiers");
            HKSymIgn_tempMods = Program.MyConfs.Read("Hotkeys", "ToggleSymbolIgnoreMode_Modifiers");
            HKTitleCase_tempMods = Program.MyConfs.Read("Hotkeys", "SelectedTextToTitleCase_Modifiers");
            HKRandomCase_tempMods = Program.MyConfs.Read("Hotkeys", "SelectedTextToRandomCase_Modifiers");
            HKSwapCase_tempMods = Program.MyConfs.Read("Hotkeys", "SelectedTextToSwapCase_Modifiers");
            HKToUpper_tempMods = Program.MyConfs.Read("Hotkeys", "SelectedToUpper_Modifiers");
            HKToLower_tempMods = Program.MyConfs.Read("Hotkeys", "SelectedToLower_Modifiers");
            HKTransliteration_tempMods = Program.MyConfs.Read("Hotkeys", "SelectedTextTransliteration_Modifiers");
            ExitHk_tempMods = Program.MyConfs.Read("Hotkeys", "ExitSwitcher_Modifiers");
            HKRestart_tempMods = Program.MyConfs.Read("Hotkeys", "RestartSwitcher_Modifiers");
            HKToggleLangPanel_tempMods = Program.MyConfs.Read("Hotkeys", "ToggleLangPanel_Modifiers");
            HKShowSelectionTranslate_tempMods = Program.MyConfs.Read("Hotkeys", "ShowSelectionTranslate_Modifiers");
            HKToggleSwitcher_tempMods = Program.MyConfs.Read("Hotkeys", "ToggleSwitcher_Modifiers");
            HKCycleCase_tempMods = Program.MyConfs.Read("Hotkeys", "CycleCase_Modifiers");
            HKSelCustConv_tempMods = Program.MyConfs.Read("Hotkeys", "SelectedTextToCustomConv_Mods");
            HKShCMenuUM_tempMods = Program.MyConfs.Read("Hotkeys", "ShowCMenuUnderMouse_Mods");
            #endregion
            #region Hotkey keys
            Mainhk_tempKey = Program.MyConfs.ReadInt("Hotkeys", "ToggleMainWindow_Key");
            HKCLast_tempKey = Program.MyConfs.ReadInt("Hotkeys", "ConvertLastWord_Key");
            HKCSelection_tempKey = Program.MyConfs.ReadInt("Hotkeys", "ConvertSelectedText_Key");
            HKCLine_tempKey = Program.MyConfs.ReadInt("Hotkeys", "ConvertLastLine_Key");
            HKConMorWor_tempKey = Program.MyConfs.ReadInt("Hotkeys", "ConvertLastWords_Key");
            HKSymIgn_tempKey = Program.MyConfs.ReadInt("Hotkeys", "ToggleSymbolIgnoreMode_Key");
            HKTitleCase_tempKey = Program.MyConfs.ReadInt("Hotkeys", "SelectedTextToTitleCase_Key");
            HKRandomCase_tempKey = Program.MyConfs.ReadInt("Hotkeys", "SelectedTextToRandomCase_Key");
            HKSwapCase_tempKey = Program.MyConfs.ReadInt("Hotkeys", "SelectedTextToSwapCase_Key");
            HKToUpper_tempKey = Program.MyConfs.ReadInt("Hotkeys", "SelectedToUpper_Key");
            HKToLower_tempKey = Program.MyConfs.ReadInt("Hotkeys", "SelectedToLower_Key");
            HKTransliteration_tempKey = Program.MyConfs.ReadInt("Hotkeys", "SelectedTextTransliteration_Key");
            ExitHk_tempKey = Program.MyConfs.ReadInt("Hotkeys", "ExitSwitcher_Key");
            HKRestart_tempKey = Program.MyConfs.ReadInt("Hotkeys", "RestartSwitcher_Key");
            HKToggleLangPanel_tempKey = Program.MyConfs.ReadInt("Hotkeys", "ToggleLangPanel_Key");
            HKShowSelectionTranslate_tempKey = Program.MyConfs.ReadInt("Hotkeys", "ShowSelectionTranslate_Key");
            HKToggleSwitcher_tempKey = Program.MyConfs.ReadInt("Hotkeys", "ToggleSwitcher_Key");
            HKCycleCase_tempKey = Program.MyConfs.ReadInt("Hotkeys", "CycleCase_Key");
            HKSelCustConv_tempKey = Program.MyConfs.ReadInt("Hotkeys", "SelectedTextToCustomConv_Key");
            HKShCMenuUM_tempKey = Program.MyConfs.ReadInt("Hotkeys", "ShowCMenuUnderMouse_Key");
            #endregion
            #region Lang Display colors
            LDMouseFore_temp = GetColor(Program.MyConfs.Read("Appearence", "MouseLTForeColor"));
            LDCaretFore_temp = GetColor(Program.MyConfs.Read("Appearence", "CaretLTForeColor"));
            LDMouseBack_temp = GetColor(Program.MyConfs.Read("Appearence", "MouseLTBackColor"));
            LDCaretBack_temp = GetColor(Program.MyConfs.Read("Appearence", "CaretLTBackColor"));
            Layout1Fore_temp = GetColor(Program.MyConfs.Read("Appearence", "Layout1ForeColor"));
            Layout2Fore_temp = GetColor(Program.MyConfs.Read("Appearence", "Layout2ForeColor"));
            Layout1Back_temp = GetColor(Program.MyConfs.Read("Appearence", "Layout1BackColor"));
            Layout2Back_temp = GetColor(Program.MyConfs.Read("Appearence", "Layout2BackColor"));
            LDMouseFont_temp = GetFont(Program.MyConfs.Read("Appearence", "MouseLTFont"));
            LDCaretFont_temp = GetFont(Program.MyConfs.Read("Appearence", "CaretLTFont"));
            Layout1Font_temp = GetFont(Program.MyConfs.Read("Appearence", "Layout1Font"));
            Layout2Font_temp = GetFont(Program.MyConfs.Read("Appearence", "Layout2Font"));
            // Transparent background colors
            LDMouseTransparentBack_temp = Program.MyConfs.ReadBool("Appearence", "MouseLTTransparentBackColor");
            LDCaretTransparentBack_temp = Program.MyConfs.ReadBool("Appearence", "CaretLTTransparentBackColor");
            Layout1TransparentBack_temp = Program.MyConfs.ReadBool("Appearence", "Layout1TransparentBackColor");
            Layout2TransparentBack_temp = Program.MyConfs.ReadBool("Appearence", "Layout2TransparentBackColor");
            #endregion
            #region Lang Display poisitions & sizes
            LDMouseY_Pos_temp = Program.MyConfs.ReadInt("Appearence", "MouseLTPositionY");
            LDCaretY_Pos_temp = Program.MyConfs.ReadInt("Appearence", "CaretLTPositionY");
            LDMouseX_Pos_temp = Program.MyConfs.ReadInt("Appearence", "MouseLTPositionX");
            LDCaretX_Pos_temp = Program.MyConfs.ReadInt("Appearence", "CaretLTPositionX");
            Layout1Y_Pos_temp = Program.MyConfs.ReadInt("Appearence", "Layout1PositionY");
            Layout2Y_Pos_temp = Program.MyConfs.ReadInt("Appearence", "Layout2PositionY");
            Layout1X_Pos_temp = Program.MyConfs.ReadInt("Appearence", "Layout1PositionX");
            Layout2X_Pos_temp = Program.MyConfs.ReadInt("Appearence", "Layout2PositionX");

            LDMouseHeight_temp = Program.MyConfs.ReadInt("Appearence", "MouseLTHeight");
            LDCaretHeight_temp = Program.MyConfs.ReadInt("Appearence", "CaretLTHeight");
            LDMouseWidth_temp = Program.MyConfs.ReadInt("Appearence", "MouseLTWidth");
            LDCaretWidth_temp = Program.MyConfs.ReadInt("Appearence", "CaretLTWidth");
            Layout1Height_temp = Program.MyConfs.ReadInt("Appearence", "Layout1Height");
            Layout2Height_temp = Program.MyConfs.ReadInt("Appearence", "Layout2Height");
            Layout1Width_temp = Program.MyConfs.ReadInt("Appearence", "Layout1Width");
            Layout2Width_temp = Program.MyConfs.ReadInt("Appearence", "Layout2Width");
            // MCDS
            MCDS_Xpos_temp = Program.MyConfs.ReadInt("Appearence", "MCDS_Pos_X");
            MCDS_Ypos_temp = Program.MyConfs.ReadInt("Appearence", "MCDS_Pos_Y");
            MCDS_TopIndent_temp = Program.MyConfs.ReadInt("Appearence", "MCDS_Top");
            MCDS_BottomIndent_temp = Program.MyConfs.ReadInt("Appearence", "MCDS_Bottom");
            // Use Flags
            LDMouseUseFlags_temp = Program.MyConfs.ReadBool("Appearence", "MouseLTUseFlags");
            LDCaretUseFlags_temp = Program.MyConfs.ReadBool("Appearence", "CaretLTUseFlags");
            // Diff text for layouts
            Layout1TText = Program.MyConfs.Read("Appearence", "Layout1LTText");
            Layout2TText = Program.MyConfs.Read("Appearence", "Layout2LTText");
            #endregion
        }
        void SaveFromTemps()
        {
            UpdateHotkeyTemps();
            #region Hotkey enableds
            Program.MyConfs.Write("Hotkeys", "ToggleMainWindow_Enabled", Mainhk_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertLastWord_Enabled", HKCLast_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertSelectedText_Enabled", HKCSelection_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertLastLine_Enabled", HKCLine_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertLastWords_Enabled", HKConMorWor_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "ToggleSymbolIgnoreMode_Enabled", HKSymIgn_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToTitleCase_Enabled", HKTitleCase_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToRandomCase_Enabled", HKRandomCase_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToSwapCase_Enabled", HKSwapCase_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedToUpper_Enabled", HKToUpper_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedToLower_Enabled", HKToLower_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextTransliteration_Enabled", HKTransliteration_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "ExitSwitcher_Enabled", ExitHk_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "RestartSwitcher_Enabled", HKRestart_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "ToggleLangPanel_Enabled", HKToggleLangPanel_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "ShowSelectionTranslate_Enabled", HKShowSelectionTranslate_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "ToggleSwitcher_Enabled", HKToggleSwitcher_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "CycleCase_Enabled", HKCycleCase_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToCustomConv_Enabled", HKSelCustConv_tempEnabled.ToString());
            Program.MyConfs.Write("Hotkeys", "ShowCMenuUnderMouse_Enabled", HKShCMenuUM_tempEnabled.ToString());
            #endregion
            #region Hotkey doubles
            Program.MyConfs.Write("Hotkeys", "ToggleMainWindow_Double", Mainhk_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertLastWord_Double", HKCLast_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertSelectedText_Double", HKCSelection_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertLastLine_Double", HKCLine_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertLastWords_Double", HKConMorWor_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "ToggleSymbolIgnoreMode_Double", HKSymIgn_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToTitleCase_Double", HKTitleCase_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToRandomCase_Double", HKRandomCase_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToSwapCase_Double", HKSwapCase_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedToUpper_Double", HKToUpper_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedToLower_Double", HKToLower_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextTransliteration_Double", HKTransliteration_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "ExitSwitcher_Double", ExitHk_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "ToggleLangPanel_Double", HKToggleLangPanel_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "ShowSelectionTranslate_Double", HKShowSelectionTranslate_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "ToggleSwitcher_Double", HKToggleSwitcher_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "CycleCase_Double", HKCycleCase_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToCustomConv_Double", HKSelCustConv_tempDouble.ToString());
            Program.MyConfs.Write("Hotkeys", "ShowCMenuUnderMouse_Double", HKShCMenuUM_tempDouble.ToString());
            #endregion
            #region Hotkey modifiers
            Program.MyConfs.Write("Hotkeys", "ToggleMainWindow_Modifiers", Mainhk_tempMods);
            Program.MyConfs.Write("Hotkeys", "ConvertLastWord_Modifiers", HKCLast_tempMods);
            Program.MyConfs.Write("Hotkeys", "ConvertSelectedText_Modifiers", HKCSelection_tempMods);
            Program.MyConfs.Write("Hotkeys", "ConvertLastLine_Modifiers", HKCLine_tempMods);
            Program.MyConfs.Write("Hotkeys", "ConvertLastWords_Modifiers", HKConMorWor_tempMods);
            Program.MyConfs.Write("Hotkeys", "ToggleSymbolIgnoreMode_Modifiers", HKSymIgn_tempMods);
            Program.MyConfs.Write("Hotkeys", "SelectedTextToTitleCase_Modifiers", HKTitleCase_tempMods);
            Program.MyConfs.Write("Hotkeys", "SelectedTextToRandomCase_Modifiers", HKRandomCase_tempMods);
            Program.MyConfs.Write("Hotkeys", "SelectedTextToSwapCase_Modifiers", HKSwapCase_tempMods);
            Program.MyConfs.Write("Hotkeys", "SelectedToUpper_Modifiers", HKToUpper_tempMods);
            Program.MyConfs.Write("Hotkeys", "SelectedToLower_Modifiers", HKToLower_tempMods);
            Program.MyConfs.Write("Hotkeys", "SelectedTextTransliteration_Modifiers", HKTransliteration_tempMods);
            Program.MyConfs.Write("Hotkeys", "ExitSwitcher_Modifiers", ExitHk_tempMods);
            Program.MyConfs.Write("Hotkeys", "RestartSwitcher_Modifiers", HKRestart_tempMods);
            Program.MyConfs.Write("Hotkeys", "ToggleLangPanel_Modifiers", HKToggleLangPanel_tempMods);
            Program.MyConfs.Write("Hotkeys", "ShowSelectionTranslate_Modifiers", HKShowSelectionTranslate_tempMods);
            Program.MyConfs.Write("Hotkeys", "ToggleSwitcher_Modifiers", HKToggleSwitcher_tempMods);
            Program.MyConfs.Write("Hotkeys", "CycleCase_Modifiers", HKCycleCase_tempMods);
            Program.MyConfs.Write("Hotkeys", "SelectedTextToCustomConv_Mods", HKSelCustConv_tempMods);
            Program.MyConfs.Write("Hotkeys", "ShowCMenuUnderMouse_Mods", HKShCMenuUM_tempMods);
            #endregion
            #region Hotkey keys
            Program.MyConfs.Write("Hotkeys", "ToggleMainWindow_Key", Mainhk_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertLastWord_Key", HKCLast_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertSelectedText_Key", HKCSelection_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertLastLine_Key", HKCLine_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "ConvertLastWords_Key", HKConMorWor_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "ToggleSymbolIgnoreMode_Key", HKSymIgn_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToTitleCase_Key", HKTitleCase_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToRandomCase_Key", HKRandomCase_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToSwapCase_Key", HKSwapCase_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedToUpper_Key", HKToUpper_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedToLower_Key", HKToLower_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextTransliteration_Key", HKTransliteration_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "ExitSwitcher_Key", ExitHk_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "RestartSwitcher_Key", HKRestart_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "ToggleLangPanel_Key", HKToggleLangPanel_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "ShowSelectionTranslate_Key", HKShowSelectionTranslate_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "ToggleSwitcher_Key", HKToggleSwitcher_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "CycleCase_Key", HKCycleCase_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "SelectedTextToCustomConv_Key", HKSelCustConv_tempKey.ToString());
            Program.MyConfs.Write("Hotkeys", "ShowCMenuUnderMouse_Key", HKShCMenuUM_tempKey.ToString());
            #endregion
            UpdateLangDisplayTemps();
            #region Lang Display colors
            Program.MyConfs.Write("Appearence", "MouseLTForeColor", ColorTranslator.ToHtml(LDMouseFore_temp));
            Program.MyConfs.Write("Appearence", "CaretLTForeColor", ColorTranslator.ToHtml(LDCaretFore_temp));
            Program.MyConfs.Write("Appearence", "MouseLTBackColor", ColorTranslator.ToHtml(LDMouseBack_temp));
            Program.MyConfs.Write("Appearence", "CaretLTBackColor", ColorTranslator.ToHtml(LDCaretBack_temp));
            Program.MyConfs.Write("Appearence", "Layout1ForeColor", ColorTranslator.ToHtml(Layout1Fore_temp));
            Program.MyConfs.Write("Appearence", "Layout2ForeColor", ColorTranslator.ToHtml(Layout2Fore_temp));
            Program.MyConfs.Write("Appearence", "Layout1BackColor", ColorTranslator.ToHtml(Layout1Back_temp));
            Program.MyConfs.Write("Appearence", "Layout2BackColor", ColorTranslator.ToHtml(Layout2Back_temp));
            Program.MyConfs.Write("Appearence", "MouseLTFont", fcv.ConvertToString(LDMouseFont_temp));
            Program.MyConfs.Write("Appearence", "CaretLTFont", fcv.ConvertToString(LDCaretFont_temp));
            Program.MyConfs.Write("Appearence", "Layout1Font", fcv.ConvertToString(Layout1Font_temp));
            Program.MyConfs.Write("Appearence", "Layout2Font", fcv.ConvertToString(Layout2Font_temp));
            // Transparent background colors
            Program.MyConfs.Write("Appearence", "MouseLTTransparentBackColor", LDMouseTransparentBack_temp.ToString());
            Program.MyConfs.Write("Appearence", "CaretLTTransparentBackColor", LDCaretTransparentBack_temp.ToString());
            Program.MyConfs.Write("Appearence", "Layout1TransparentBackColor", Layout1TransparentBack_temp.ToString());
            Program.MyConfs.Write("Appearence", "Layout2TransparentBackColor", Layout2TransparentBack_temp.ToString());
            #endregion
            #region Lang Display poisitions & sizes
            Program.MyConfs.Write("Appearence", "MouseLTPositionY", LDMouseY_Pos_temp.ToString());
            Program.MyConfs.Write("Appearence", "CaretLTPositionY", LDCaretY_Pos_temp.ToString());
            Program.MyConfs.Write("Appearence", "MouseLTPositionX", LDMouseX_Pos_temp.ToString());
            Program.MyConfs.Write("Appearence", "CaretLTPositionX", LDCaretX_Pos_temp.ToString());
            Program.MyConfs.Write("Appearence", "Layout1PositionY", Layout1Y_Pos_temp.ToString());
            Program.MyConfs.Write("Appearence", "Layout2PositionY", Layout2Y_Pos_temp.ToString());
            Program.MyConfs.Write("Appearence", "Layout1PositionX", Layout1X_Pos_temp.ToString());
            Program.MyConfs.Write("Appearence", "Layout2PositionX", Layout2X_Pos_temp.ToString());

            Program.MyConfs.Write("Appearence", "MouseLTHeight", LDMouseHeight_temp.ToString());
            Program.MyConfs.Write("Appearence", "CaretLTHeight", LDCaretHeight_temp.ToString());
            Program.MyConfs.Write("Appearence", "MouseLTWidth", LDMouseWidth_temp.ToString());
            Program.MyConfs.Write("Appearence", "CaretLTWidth", LDCaretWidth_temp.ToString());
            Program.MyConfs.Write("Appearence", "Layout1Height", Layout1Height_temp.ToString());
            Program.MyConfs.Write("Appearence", "Layout2Height", Layout2Height_temp.ToString());
            Program.MyConfs.Write("Appearence", "Layout1Width", Layout1Width_temp.ToString());
            Program.MyConfs.Write("Appearence", "Layout2Width", Layout2Width_temp.ToString());
            // MCDS
            Program.MyConfs.Write("Appearence", "MCDS_Pos_X", MCDS_Xpos_temp.ToString());
            Program.MyConfs.Write("Appearence", "MCDS_Pos_Y", MCDS_Ypos_temp.ToString());
            Program.MyConfs.Write("Appearence", "MCDS_Top", MCDS_TopIndent_temp.ToString());
            Program.MyConfs.Write("Appearence", "MCDS_Bottom", MCDS_BottomIndent_temp.ToString());
            // Use Flags
            Program.MyConfs.Write("Appearence", "MouseLTUseFlags", LDMouseUseFlags_temp.ToString());
            Program.MyConfs.Write("Appearence", "CaretLTUseFlags", LDCaretUseFlags_temp.ToString());
            // Diff text for layouts
            Program.MyConfs.Write("Appearence", "Layout1LTText", Layout1TText);
            Program.MyConfs.Write("Appearence", "Layout2LTText", Layout2TText);
            #endregion
            Logging.Log("Saved from temps.");
        }
        /// <summary>
        /// Update save paths for logs, snippets, autoswitch dictionary, configs.
        /// </summary>
        void UpdateSaveLoadPaths(bool appdata = false)
        {
            if (!Program.C_SWITCH)
            {
                if (Configs.forceAppData || appdata)
                    nPath = switcher_folder_appd;
            }
            snipfile = Path.Combine(nPath, "snippets.txt");
            AS_dictfile = Path.Combine(nPath, "AS_dict.txt");
            Logging.logdir = Path.Combine(nPath, "Logs");
            Logging.log = Path.Combine(Logging.logdir, DateTime.Today.ToString("yyyy.MM.dd") + ".txt");
            Configs.filePath = Path.Combine(nPath, "FaineSwitcher.ini");
            Program.MyConfs = new Configs();
        }
        /// <summary>
        /// Saves current settings to INI.
        /// </summary>
        void SaveConfigs()
        {
            if (Configs.forceAppData && !chk_AppDataConfigs.Checked)
            {
                try
                {
                    File.Delete(Path.Combine(switcher_folder_appd, ".force"));
                    Configs.forceAppData = false;
                }
                catch { Logging.Log("Force AppData file was missing...", 2); }
            }
            bool only_load = false;
            if (!Program.C_SWITCH)
            {
                if (chk_AppDataConfigs.Checked)
                {
                    if (!Directory.Exists(switcher_folder_appd))
                        Directory.CreateDirectory(switcher_folder_appd);
                    nPath = switcher_folder_appd;
                }
                else
                {
                    nPath = AppDomain.CurrentDomain.BaseDirectory;
                }
            }
            Logging.Log("Base path: " + nPath);
            AutoStartAsAdmin = (cbb_AutostartType.SelectedIndex != 0);
            if (chk_AutoStart.Checked)
            {
                if (!AutoStartExist(AutoStartAsAdmin))
                    CreateAutoStart();
            }
            else
            {
                if (AutoStartExist(AutoStartAsAdmin))
                    AutoStartRemove(AutoStartAsAdmin);
            }
            var exist = File.Exists(Path.Combine(nPath, "FaineSwitcher.ini"));
            if (latest_save_dir != nPath && exist) only_load = true;
            if (!exist)
            {
                Logging.Log("Creating new configs file [" + Configs.filePath + "].");
                Configs.CreateConfigsFile();
            }
            DoInMainConfigs(() => { Program.MyConfs.WriteSave("Functions", "AppDataConfigs", chk_AppDataConfigs.Checked.ToString()); return (object)0; });
            if (!only_load)
            {
                tmpLangTTAppearenceIndex = lsb_LangTTAppearenceForList.SelectedIndex;
                tmpHotkeysIndex = lsb_Hotkeys.SelectedIndex;
                #region Functions
                Program.MyConfs.Write("Functions", "AutoStartAsAdmin", AutoStartAsAdmin.ToString());
                Program.MyConfs.Write("Functions", "TrayIconVisible", chk_TrayIcon.Checked.ToString());
                Program.MyConfs.Write("Functions", "ConvertSelectionLayoutSwitching", chk_CSLayoutSwitching.Checked.ToString());
                Program.MyConfs.Write("Functions", "ReSelect", chk_ReSelect.Checked.ToString());
                Program.MyConfs.Write("Functions", "RePress", chk_RePress.Checked.ToString());
                Program.MyConfs.Write("Functions", "AddOneSpaceToLastWord", chk_AddOneSpace.Checked.ToString());
                Program.MyConfs.Write("Functions", "AddOneEnterToLastWord", chk_Add1NL.Checked.ToString());
                Program.MyConfs.Write("Functions", "ConvertSelectionLayoutSwitchingPlus", chk_CSLayoutSwitchingPlus.Checked.ToString());
                Program.MyConfs.Write("Functions", "ScrollTip", chk_HighlightScroll.Checked.ToString());
                Program.MyConfs.Write("Functions", "StartupUpdatesCheck", chk_StartupUpdatesCheck.Checked.ToString());
                Program.MyConfs.Write("Functions", "SilentUpdate", chk_SilentUpdate.Checked.ToString());
                Program.MyConfs.Write("Functions", "Logging", chk_Logging.Checked.ToString());
                Program.MyConfs.Write("Functions", "TrayFlags", (cbb_TrayDislpayType.SelectedIndex == 1).ToString());
                Program.MyConfs.Write("Functions", "TrayText", (cbb_TrayDislpayType.SelectedIndex == 2).ToString());
                Program.MyConfs.Write("Functions", "CapsLockTimer", chk_CapsLockDTimer.Checked.ToString());
                Program.MyConfs.Write("Functions", "BlockSwitcherHotkeysWithCtrl", chk_BlockHKWithCtrl.Checked.ToString());
                Program.MyConfs.Write("Functions", "MCDServerSupport", chk_MCDS_support.Checked.ToString());
                Program.MyConfs.Write("Functions", "OneLayoutWholeWord", chk_OneLayoutWholeWord.Checked.ToString());
                Program.MyConfs.Write("Appearence", "MouseLTAlways", chk_MouseTTAlways.Checked.ToString());
                Program.MyConfs.Write("Functions", "GuessKeyCodeFix", chk_GuessKeyCodeFix.Checked.ToString());
                Program.MyConfs.Write("Functions", "RemapCapslockAsF18", chk_RemapCapsLockAsF18.Checked.ToString());
                Program.MyConfs.Write("Functions", "UseJKL", chk_GetLayoutFromJKL.Checked.ToString());
                Program.MyConfs.Write("Functions", "ReadOnlyNA", chk_ReadOnlyNA.Checked.ToString());
                Program.MyConfs.Write("Functions", "WriteInputHistory", chk_WriteInputHistory.Checked.ToString());
                try { Program.MyConfs.Write("Functions", "WriteInputHistoryBackSpaceType", cbb_BackSpaceType.SelectedIndex.ToString()); } catch { }
                #endregion
                #region Layouts
                //Program.MyConfs.Write("Layouts", "SwitchBetweenLayouts", chk_SwitchBetweenLayouts.Checked.ToString());
                //Program.MyConfs.Write("Layouts", "EmulateLayoutSwitch", chk_EmulateLS.Checked.ToString());
                //Program.MyConfs.Write("Layouts", "ChangeToSpecificLayoutByKey", chk_SpecificLS.Checked.ToString());
                //// Specific keys sets
                //SaveSpecificKeySets();
                //// Specific keys type
                //Program.MyConfs.Write("Layouts", "SpecificKeysType", cbb_SpecKeysType.SelectedIndex.ToString());
                //// Keys 
                //Program.MyConfs.Write("Layouts", "SpecificKey1", cbb_Key1.SelectedIndex.ToString());
                //Program.MyConfs.Write("Layouts", "SpecificKey2", cbb_Key2.SelectedIndex.ToString());
                //Program.MyConfs.Write("Layouts", "SpecificKey3", cbb_Key3.SelectedIndex.ToString());
                //Program.MyConfs.Write("Layouts", "SpecificKey4", cbb_Key4.SelectedIndex.ToString());
                //try {
                //	try { Program.MyConfs.Write("Layouts", "EmulateLayoutSwitchType", cbb_EmulateType.SelectedItem.ToString()); } catch { }
                //	// Main Layouts
                //	try { Program.MyConfs.Write("Layouts", "MainLayout1", cbb_MainLayout1.SelectedItem.ToString()); } catch {  }
                //	try { Program.MyConfs.Write("Layouts", "MainLayout2", cbb_MainLayout2.SelectedItem.ToString()); } catch { }
                //	// Layouts
                //	//try { Program.MyConfs.Write("Layouts", "SpecificLayout1", cbb_Layout1.SelectedItem.ToString()); } catch { }
                //	//try { Program.MyConfs.Write("Layouts", "SpecificLayout2", cbb_Layout2.SelectedItem.ToString()); } catch { }
                //	//try { Program.MyConfs.Write("Layouts", "SpecificLayout3", cbb_Layout3.SelectedItem.ToString()); } catch { }
                //	//try { Program.MyConfs.Write("Layouts", "SpecificLayout4", cbb_Layout4.SelectedItem.ToString()); } catch { }
                //} catch { Logging.Log("Some settings in layouts tab failed to save, they are skipped."); }
                //Program.MyConfs.Write("Layouts", "OneLayout", chk_OneLayout.Checked.ToString());
                //Program.MyConfs.Write("Layouts", "QWERTZfix", chk_qwertz.Checked.ToString());
                //Program.MyConfs.Write("Layouts", "CTRL_ALT_TemporaryChangeLayout", txt_LCTRLLALTTempLayout.Text);
                #endregion
                #region Persistent Layout
                Program.MyConfs.Write("PersistentLayout", "OnlyOnWindowChange", chk_OnlyOnWindowChange.Checked.ToString());
                Program.MyConfs.Write("PersistentLayout", "ChangeOnlyOnce", chk_ChangeLayoutOnlyOnce.Checked.ToString());
                Program.MyConfs.Write("PersistentLayout", "ActivateForLayout1", chk_PersistentLayout1Active.Checked.ToString());
                Program.MyConfs.Write("PersistentLayout", "ActivateForLayout2", chk_PersistentLayout2Active.Checked.ToString());
                Program.MyConfs.Write("PersistentLayout", "Layout1CheckInterval", nud_PersistentLayout1Interval.Value.ToString());
                Program.MyConfs.Write("PersistentLayout", "Layout2CheckInterval", nud_PersistentLayout2Interval.Value.ToString());
                Program.MyConfs.Write("PersistentLayout", "Layout1Processes", txt_PersistentLayout1Processes.Text.Replace(Environment.NewLine, "^cr^lf"));
                Program.MyConfs.Write("PersistentLayout", "Layout2Processes", txt_PersistentLayout2Processes.Text.Replace(Environment.NewLine, "^cr^lf"));
                #endregion
                #region Appearence
                Program.MyConfs.Write("Appearence", "DisplayLangTooltipForMouse", chk_LangTooltipMouse.Checked.ToString());
                Program.MyConfs.Write("Appearence", "DisplayLangTooltipForCaret", chk_LangTooltipCaret.Checked.ToString());
                Program.MyConfs.Write("Appearence", "DisplayLangTooltipForMouseOnChange", chk_LangTTMouseOnChange.Checked.ToString());
                Program.MyConfs.Write("Appearence", "DisplayLangTooltipForCaretOnChange", chk_LangTTCaretOnChange.Checked.ToString());
                Program.MyConfs.Write("Appearence", "DifferentColorsForLayouts", chk_LangTTDiffLayoutColors.Checked.ToString());
                try
                {
                    Program.MyConfs.Write("Appearence", "Language", cbb_Language.SelectedItem.ToString());
                }
                catch
                {
                    Logging.Log("Language saving failed, restored to English.");
                    Program.MyConfs.Write("Appearence", "Language", "Українська");
                }
                Program.MyConfs.Write("Appearence", "MouseLTUpperArrow", mouseLTUpperArrow.ToString());
                Program.MyConfs.Write("Appearence", "CaretLTUpperArrow", caretLTUpperArrow.ToString());
                Program.MyConfs.Write("Appearence", "WindowsMessages", chk_LDMessages.Checked.ToString());
                #endregion
                #region Timings
                if (LDUseWindowsMessages)
                    Program.MyConfs.Write("Timings", "LangTooltipForMouseSkipMessages", nud_LangTTMouseRefreshRate.Value.ToString());
                else
                    Program.MyConfs.Write("Timings", "LangTooltipForMouseRefreshRate", nud_LangTTMouseRefreshRate.Value.ToString());
                Program.MyConfs.Write("Timings", "UsePasteInCS", chk_CSUsePaste.Checked.ToString());
                Program.MyConfs.Write("Timings", "LangTooltipForCaretRefreshRate", nud_LangTTCaretRefreshRate.Value.ToString());
                Program.MyConfs.Write("Timings", "DoubleHotkey2ndPressWait", nud_DoubleHK2ndPressWaitTime.Value.ToString());
                Program.MyConfs.Write("Timings", "FlagsInTrayRefreshRate", nud_TrayFlagRefreshRate.Value.ToString());
                Program.MyConfs.Write("Timings", "ScrollLockStateRefreshRate", nud_ScrollLockRefreshRate.Value.ToString());
                Program.MyConfs.Write("Timings", "CapsLockDisableRefreshRate", nud_CapsLockRefreshRate.Value.ToString());
                Program.MyConfs.Write("Timings", "ScrollLockStateRefreshRate", nud_ScrollLockRefreshRate.Value.ToString());
                Program.MyConfs.Write("Timings", "SelectedTextGetMoreTries", chk_SelectedTextGetMoreTries.Checked.ToString());
                Program.MyConfs.Write("Timings", "SelectedTextGetMoreTriesCount", nud_SelectedTextGetTriesCount.Value.ToString());
                Program.MyConfs.Write("Timings", "DelayAfterBackspaces", nud_DelayAfterBackspaces.Value.ToString());
                Program.MyConfs.Write("Timings", "UseDelayAfterBackspaces", chk_UseDelayAfterBackspaces.Checked.ToString());
                #region Excluded
                Program.MyConfs.Write("Timings", "ExcludedPrograms", txt_ExcludedPrograms.Text.Replace(Environment.NewLine, "^cr^lf"));
                Program.MyConfs.Write("Timings", "ChangeLayoutInExcluded", chk_Change1KeyL.Checked.ToString());
                Program.MyConfs.Write("Timings", "ConvertSWLinExcl", chk_ConvSWL.Checked.ToString());
                #endregion
                #endregion
                #region Snippets
                Program.MyConfs.Write("Snippets", "SnippetsEnabled", chk_Snippets.Checked.ToString());
                Program.MyConfs.Write("Snippets", "SpaceAfter", chk_SnippetsSpaceAfter.Checked.ToString());
                Program.MyConfs.Write("Snippets", "SwitchToGuessLayout", chk_SnippetsSwitchToGuessLayout.Checked.ToString());
                if (SnippetsEnabled)
                    File.WriteAllText(snipfile, txt_Snippets.Text, Encoding.UTF8);
                Program.MyConfs.Write("Snippets", "SnippetExpandKey", cbb_SnippetExpandKeys.SelectedItem == null ? "null" : cbb_SnippetExpandKeys.SelectedItem.ToString());
                Program.MyConfs.Write("Snippets", "SnippetsExpKeyOther", SnippetsExpKeyOther);
                SaveNCRSets();
                #endregion
                #region AutoSwitch
                Program.MyConfs.Write("AutoSwitch", "Enabled", chk_AutoSwitch.Checked.ToString());
                Program.MyConfs.Write("AutoSwitch", "SpaceAfter", chk_AutoSwitchSpaceAfter.Checked.ToString());
                Program.MyConfs.Write("AutoSwitch", "SwitchToGuessLayout", chk_AutoSwitchSwitchToGuessLayout.Checked.ToString());
                Program.MyConfs.Write("AutoSwitch", "DownloadInZip", chk_DownloadASD_InZip.Checked.ToString());
                if (AutoSwitchEnabled && !string.IsNullOrEmpty(AutoSwitchDictionaryRaw) && !AutoSwitchDictionaryTooBig)
                    File.WriteAllText(AS_dictfile, AutoSwitchDictionaryRaw, Encoding.UTF8);
                #endregion
                #region Appearence & Hotkeys
                SaveFromTemps();
                #endregion
                #region LangPanel
                Program.MyConfs.Write("LangPanel", "Display", chk_DisplayLangPanel.Checked.ToString());
                Program.MyConfs.Write("LangPanel", "RefreshRate", nud_LPRefreshRate.Value.ToString());
                Program.MyConfs.Write("LangPanel", "Transparency", nud_LPTransparency.Value.ToString());
                Program.MyConfs.Write("LangPanel", "ForeColor", ColorTranslator.ToHtml(btn_LPFore.BackColor));
                Program.MyConfs.Write("LangPanel", "BackColor", ColorTranslator.ToHtml(btn_LPBack.BackColor));
                Program.MyConfs.Write("LangPanel", "BorderColor", ColorTranslator.ToHtml(btn_LPBorderColor.BackColor));
                Program.MyConfs.Write("LangPanel", "BorderAeroColor", chk_LPAeroColor.Checked.ToString());
                Program.MyConfs.Write("LangPanel", "Font", fcv.ConvertToString(btn_LPFont.Font));
                Program.MyConfs.Write("LangPanel", "UpperArrow", chk_LPUpperArrow.Checked.ToString());
                #endregion
                #region Translate Panel
                Program.MyConfs.Write("TranslatePanel", "Enabled", chk_TrEnable.Checked.ToString());
                Program.MyConfs.Write("TranslatePanel", "UseGS", TranslatePanel.useGS.ToString());
                Program.MyConfs.Write("TranslatePanel", "UseNA", TranslatePanel.useNA.ToString());
                Program.MyConfs.Write("TranslatePanel", "Transparency", nud_TrTransparency.Value.ToString());
                Program.MyConfs.Write("TranslatePanel", "OnDoubleClick", chk_TrOnDoubleClick.Checked.ToString());
                Program.MyConfs.Write("TranslatePanel", "FG", ColorTranslator.ToHtml(btn_TrFG.BackColor));
                Program.MyConfs.Write("TranslatePanel", "BG", ColorTranslator.ToHtml(btn_TrBG.BackColor));
                Program.MyConfs.Write("TranslatePanel", "BorderC", ColorTranslator.ToHtml(btn_TrBorderC.BackColor));
                Program.MyConfs.Write("TranslatePanel", "BorderAero", chk_TrUseAccent.Checked.ToString());
                Program.MyConfs.Write("TranslatePanel", "TextFont", fcv.ConvertToString(btn_TrTextFont.Font));
                Program.MyConfs.Write("TranslatePanel", "TitleFont", fcv.ConvertToString(btn_TrTitleFont.Font));
                Program.MyConfs.Write("TranslatePanel", "Transcription", chk_TrTranscription.Checked.ToString());
                SaveTrSets();
                #endregion
                #region Sync
                Program.MyConfs.Write("Sync", "BBools", string.Join("|", bin(chk_Mini.Checked), bin(chk_Stxt.Checked), bin(chk_Htxt.Checked), bin(chk_Ttxt.Checked), bin(chk_andPROXY.Checked), bin(chk_Mmm.Checked)));
                Program.MyConfs.Write("Sync", "RBools", string.Join("|", bin(chk_rMini.Checked), bin(chk_rStxt.Checked), bin(chk_rHtxt.Checked), bin(chk_rTtxt.Checked), bin(chk_andPROXY2.Checked), bin(chk_rMmm.Checked)));
                Program.MyConfs.Write("Sync", "BLast", txt_backupId.Text);
                Program.MyConfs.Write("Sync", "RLast", txt_restoreId.Text);
                Program.MyConfs.Write("Sync", "ZxZ", ZxZ.ToString());
                #endregion
                #region Proxy
                Program.MyConfs.Write("Proxy", "ServerPort", txt_ProxyServerPort.Text);
                Program.MyConfs.Write("Proxy", "UserName", txt_ProxyLogin.Text);
                Program.MyConfs.Write("Proxy", "Password", Convert.ToBase64String(Encoding.Unicode.GetBytes(txt_ProxyPassword.Text)));
                #endregion
                #region Sounds
                Program.MyConfs.Write("Sounds", "Enabled", chk_EnableSnd.Checked.ToString());
                Program.MyConfs.Write("Sounds", "OnAutoSwitch", chk_SndAutoSwitch.Checked.ToString());
                Program.MyConfs.Write("Sounds", "OnSnippets", chk_SndSnippets.Checked.ToString());
                Program.MyConfs.Write("Sounds", "OnConvertLast", chk_SndLast.Checked.ToString());
                Program.MyConfs.Write("Sounds", "OnLayoutSwitch", chk_SndLayoutSwitch.Checked.ToString());
                Program.MyConfs.Write("Sounds", "UseCustomSound", chk_UseCustomSnd.Checked.ToString());
                Program.MyConfs.Write("Sounds", "CustomSound", lbl_CustomSound.Text);
                Program.MyConfs.Write("Sounds", "OnAutoSwitch2", chk_SndAutoSwitch2.Checked.ToString());
                Program.MyConfs.Write("Sounds", "OnSnippets2", chk_SndSnippets2.Checked.ToString());
                Program.MyConfs.Write("Sounds", "OnConvertLast2", chk_SndLast2.Checked.ToString());
                Program.MyConfs.Write("Sounds", "OnLayoutSwitch2", chk_SndLayoutSwitch2.Checked.ToString());
                Program.MyConfs.Write("Sounds", "UseCustomSound2", chk_UseCustomSnd2.Checked.ToString());
                Program.MyConfs.Write("Sounds", "CustomSound2", lbl_CustomSound2.Text);
                #endregion
                saveHidden();
                Program.MyConfs.WriteToDisk();
                Logging.Log("All configurations saved.");
            }
            LoadConfigs();
        }
        void SaveNCRSets()
        {
            var sets = new StringBuilder();
            for (int i = 1; i <= NCRSetsCount; i++)
            {
                var _set = pan_NoConvertRules.Controls["set_" + i];
                sets.Append("set_").Append(i).Append("\0")
                    .Append(_set.Controls["rule" + i].Text).Append("\0")
                    .Append((_set.Controls["isnip" + i] as CheckBox).Checked).Append("\0")
                    .Append((_set.Controls["iauto" + i] as CheckBox).Checked);
                if (i != NCRSetsCount)
                    sets.Append("\0");
            }
            if (String.IsNullOrEmpty(sets.ToString()))
            {
                sets.Clear().Append("set_0");
            }
            Program.MyConfs.Write("Snippets", "NCRSets", sets.ToString());
        }
        void LoadNCRSets()
        {
            var sets_raw = Program.MyConfs.Read("Snippets", "NCRSets");
            if (sets_raw.Contains("set_0")) { return; }
            var SETS = sets_raw.Split(new string[] { "\0set_" }, StringSplitOptions.None);
            if (SETS.Length == 0) return;
            var NOTR = NCRSetsCount == 0;
            if (NOTR)
                pan_NoConvertRules.Controls.Clear();
            KMHook.NCRules = null;
            KMHook.NCRules = new KMHook.NCR[SETS.Length];
            for (int i = 1; i <= SETS.Length; i++)
            {
                if (NOTR)
                    Btn_NCR_AddClick((object)1, new EventArgs());
                var _set = pan_NoConvertRules.Controls["set_" + i];
                var values = SETS[i - 1].Split('\0');
                _set.Controls["rule" + i].Text = values[1];
                bool b1 = false, b2 = false;
                bool.TryParse(values[2], out b1);
                var c1 = (_set.Controls["isnip" + i] as CheckBox);
                c1.Checked = b1;
                c1.Text = Program.Lang[Languages.Element.tab_Snippets];
                bool.TryParse(values[3], out b2);
                var c2 = (_set.Controls["iauto" + i] as CheckBox);
                c2.Text = Program.Lang[Languages.Element.tab_AutoSwitch];
                c2.Checked = b2;
                KMHook.NCRules[i - 1] = new KMHook.NCR() { rule = values[1], isnip = b1, iauto = b2 };
            }
        }
        void SaveTrSets()
        {
            var sets = new StringBuilder();
            for (int i = 1; i <= TrSetCount; i++)
            {
                sets.Append("set_").Append(i).Append("/")
                    .Append(TrSetsValues[new StringBuilder("cbb_fr").Append(i).ToString()]).Append("/")
                    .Append(TrSetsValues[new StringBuilder("cbb_to").Append(i).ToString()]);
                if (i != TrSetCount)
                    sets.Append("|");
            }
            if (String.IsNullOrEmpty(sets.ToString()))
                sets.Clear().Append("set_0");
            Program.MyConfs.Write("TranslatePanel", "LanguageSets", sets.ToString());
        }
        void SaveSpecificKeySets(bool change1set = false, int setId = 0, string typ = "")
        {
            //var sets = new StringBuilder();;
            //for (int i = 1; i <= SpecKeySetCount; i++) {
            //	sets.Append("set_").Append(i).Append("/")
            //	    .Append(SpecKeySetsValues[new StringBuilder("txt_key").Append(i).Append("_key").ToString()]).Append("/")
            //		.Append(SpecKeySetsValues[new StringBuilder("txt_key").Append(i).Append("_mods").ToString()]);
            //	if ((pan_KeySets.Controls["set_"+i].Controls["chk_win"+i] as CheckBox).Checked &&
            //	    !SpecKeySetsValues["txt_key"+i+"_mods"].Contains("Win"))
            //		sets.Append(" + Win");
            //	sets.Append("/");
            //	if (setId == i && change1set)
            //		sets.Append(typ);
            //	else 
            //		sets.Append(SpecKeySetsValues["cbb_typ"+i]);
            //	if (i != SpecKeySetCount)
            //		sets.Append("|");
            //}
            //if (String.IsNullOrEmpty(sets.ToString()))
            //	sets.Clear().Append("set_0");
            //Program.MyConfs.Write("Layouts", "SpecificKeySets", sets.ToString());
        }
        object DoInMainConfigs(Func<object> act)
        {
            if (Configs.forceAppData || Program.C_SWITCH) return (object)true;
            var last = Configs.filePath; // Last configs file
            Configs.filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FaineSwitcher.ini");
            if (!Configs.Readable())
            {
                Configs.filePath = last;
                return (object)true;
            }
            Program.MyConfs = new Configs();
            object rsl = act();
            if (chk_AppDataConfigs.Checked)
            {
                ;
                if (!Directory.Exists(switcher_folder_appd))
                    Directory.CreateDirectory(switcher_folder_appd);
                Configs.filePath = Path.Combine(switcher_folder_appd, "FaineSwitcher.ini");
                Program.MyConfs = new Configs();
            }
            return rsl;
        }
        void ChangeAutoSwitchDictionaryTextBox()
        {
            if (AutoSwitchDictionaryTooBig)
            {
                txt_AutoSwitchDictionary.ReadOnly = true;
                txt_AutoSwitchDictionary.Text = Program.Lang[Languages.Element.AutoSwitchDictionaryTooBigToDisplay];
            }
            else
            {
                txt_AutoSwitchDictionary.ReadOnly = false;
                txt_AutoSwitchDictionary.Text = AutoSwitchDictionaryRaw;
            }
        }
        Color GetColor(string color_html)
        {
            Color color = SystemColors.WindowText;
            try
            {
                color = ColorTranslator.FromHtml(color_html);
            }
            catch (Exception e)
            {
                WrongColorLog(color_html, e.Message + "\r\n" + e.StackTrace);
            }
            return color;
        }
        Font GetFont(string font_raw, bool remstyle = false)
        {
            Font font = SystemFonts.DefaultFont;
            font_raw = FontDecimReplace(font_raw);
            try
            {
                if (remstyle)
                {
                    if (!font_raw.Contains("style=")) font_raw = font_raw.Replace("; style", "");
                }
                font = (Font)fcv.ConvertFromString(font_raw);
            }
            catch (Exception e)
            {
                WrongFontLog(font_raw, e.Message + "\r\n" + e.StackTrace);
            }
            return font;
        }
        string FontDecimReplace(string font_raw)
        {
            var pattern = @"(?:([.,]))([0-9]+pt)";
            var repl = font_raw;
            var mt = new Regex(pattern).Match(repl);
            //			Debug.WriteLine("DECIM REPL FONT :" +mt.Groups[1] + " == " + decim);
            if (mt.Groups[1].Value != decim && !string.IsNullOrEmpty(mt.Groups[1].Value))
            {
                repl = Regex.Replace(font_raw, pattern, decim + "$2");
                Logging.Log("Replaced decimal in font " + font_raw + ", with: " + decim + " => " + repl);
            }
            return repl;
        }
        void parseRedefines()
        {
            LLHook.redefines.Clear();
            LLHook.redefines_excl_mods.Clear();
            var pon = Redefines.Split('|');
            for (int i = 0; i != pon.Length; i++)
            {
                var pyon = pon[i].Split(new[] { '<' }, 2);
                var kin = pyon[0].Split(new[] { '>' }, 2);
                if (String.IsNullOrEmpty(pon[i]))
                {
                    Logging.Log("[REDEF] > Ignore empty: #" + i);
                    continue;
                }
                if (kin.Length < 2)
                {
                    Debug.WriteLine("Syntax error, expected: \">\"");
                    continue;
                }
                Keys k, k2;
                try
                {
                    k = KMHook.strparsekey(kin[0])[0];
                }
                catch { Logging.Log("[REDEF] > Can't parse key: " + kin[0]); continue; }
                try
                {
                    k2 = KMHook.strparsekey(kin[1])[0];
                }
                catch { Logging.Log("[REDEF] > Can't parse key: " + kin[1]); continue; }
                var sn = (pyon.Length < 2 ? "" : pyon[1]);
                Logging.Log("[REDEF] > +Redefine: " + k + " => " + k2 + " <= " + sn);
                LLHook.redefines.Add(k, k2);
                LLHook.redefines_excl_mods.Add(sn);
            }
        }
        void saveHidden()
        {
            Program.MyConfs.Write("Hidden", "cmdbackfix", Hchk_cmdbackfix.Checked.ToString());
            Program.MyConfs.Write("Hidden", "DARKTHEME", Hchk_DARK.Checked.ToString());
            Program.MyConfs.Write("Hidden", "ChangeLayoutOnTrayLMB", Hchk_LMBTrayLayoutChange.Checked.ToString());
            Program.MyConfs.Write("Hidden", "DisableMemoryFlush", Hchk_DisableMemFlush.Checked.ToString());
            Program.MyConfs.Write("Hidden", "SymbolClear", Htxt_SymbolClear.Text);
            Program.MyConfs.Write("Hidden", "LibreCtrlAltShiftV", Hchk_LibrePasteFixCASV.Checked.ToString());
            Program.MyConfs.Write("Hidden", "CycleCaseOrder", Htxt_CycleCaseOrder.Text);
            Program.MyConfs.Write("Hidden", "CycleCaseReset", Hchk_CycleCaseReset.Checked.ToString());
            Program.MyConfs.Write("Hidden", "__selection", Hchk___selection.Checked.ToString());
            Program.MyConfs.Write("Hidden", "__selection_nomouse", Hchk___selection_nomouse.Checked.ToString());
            Program.MyConfs.Write("Hidden", "onlySnippetsExcluded", Htxt_OSnippetsExcluded.Text);
            Program.MyConfs.Write("Hidden", "onlyAutoSwitchExcluded", Htxt_OAutoSwitchExcluded.Text);
            Program.MyConfs.Write("Hidden", "OverlayExcluded", Htxt_OverlayExcluded.Text);
            Program.MyConfs.Write("Hidden", "OverlayExcludedInterval", Hnud_OverlayExcludedInterval.Value.ToString());
            Program.MyConfs.Write("Hidden", "AutoCopyTranslation", Htxt_AutoCopyTranslation.Text);
            Program.MyConfs.Write("Hidden", "AS_IngoreBack", Hchk_ASIgnoreBack.Checked.ToString());
            Program.MyConfs.Write("Hidden", "AS_IngoreDel", Hchk_ASIgnoreDel.Checked.ToString());
            Program.MyConfs.Write("Hidden", "AS_IngoreLS", Hchk_ASIgnoreLS.Checked.ToString());
            Program.MyConfs.Write("Hidden", "AS_IngoreRules", Htxt_AutoSwitchIngoreRules.Text.ToUpper());
            Program.MyConfs.Write("Hidden", "AS_IngoreLSTimeout", Hnud_ASIgnoreTimeout.Value.ToString());
            Program.MyConfs.Write("Hidden", "NCS_tray", Hchk_NCStray.Checked.ToString());
            Program.MyConfs.Write("Hidden", "NCS", Htxt_NCS.Text.ToUpper());
            Program.MyConfs.Write("Hidden", "ToggleAutoSwitchHK", Htxt_AutoSwitchHotkeyStr.Text);
            Program.MyConfs.Write("Hidden", "AutoRestartMins", Hnud_AutoRestartMins.Value.ToString());
            Program.MyConfs.Write("Hidden", "__setlayout_FORCED", Hchk___setlayoutForce.Checked.ToString());
            Program.MyConfs.Write("Hidden", "__setlayout_ONLYWM", Hchk___setlayoutOnlyWM.Checked.ToString());
            Program.MyConfs.Write("Hidden", "ReSelectCustoms", Htxt_ReselectCustoms.Text);
            Program.MyConfs.Write("Hidden", "ChangeLayoutOnTrayLMB+DoubleClick", Hchk_LMBTrayLayoutChangeDC.Checked.ToString());
            Program.MyConfs.Write("Hidden", "TrayHoverSwitcherMM", Hnud_TrayHoverMM.Value.ToString());
            Program.MyConfs.Write("Hidden", "Redefines", Htxt_Redefines.Text);
            Program.MyConfs.Write("Hidden", "ClipBackOnlyText", Hchk_ClipBackOnlyText.Checked.ToString());
            Program.MyConfs.Write("Hidden", "AutoSwitchEndingSymbols", Htxt_ASEndSymbols.Text);
            Program.MyConfs.Write("Hidden", "CycleCaseSaveBase", Hchk_SaveBase.Checked.ToString());
            try
            {
                Program.MyConfs.Write("Hidden", "Layout_1_Modifier_Key", (
                    String.IsNullOrEmpty(Htxt_LayoutModifier_1.Text) ? 0 :
                    ((int)KMHook.strparsekey(Htxt_LayoutModifier_1.Text)[0])
                ).ToString());
            }
            catch { Logging.Log("Layout modifier 1 parse error, can't recognize that key:" + Htxt_LayoutModifier_1.Text, 1); }
            try
            {
                Program.MyConfs.Write("Hidden", "Layout_2_Modifier_Key", (
                    String.IsNullOrEmpty(Htxt_LayoutModifier_2.Text) ? 0 :
                    ((int)KMHook.strparsekey(Htxt_LayoutModifier_2.Text)[0])
                ).ToString());
            }
            catch { Logging.Log("Layout modifier 2 parse error, can't recognize that key:" + Htxt_LayoutModifier_2.Text, 1); }
            try
            {
                Program.MyConfs.Write("Hidden", "Layout_D_Modifier_Key", (
                    String.IsNullOrEmpty(Htxt_LayoutModifier_D.Text) ? 0 :
                    ((int)KMHook.strparsekey(Htxt_LayoutModifier_D.Text)[0])
                ).ToString());
            }
            catch { Logging.Log("Layout modifier D parse error, can't recognize that key:" + Htxt_LayoutModifier_D.Text, 1); }
            //			NCS_destroy();
        }
        string KeynameReplace(string input)
        {
            var x = input.ToLower().Replace("menu", "alt").Replace("control", "ctrl")
                .Replace("d0", "0").Replace("d1", "1")
                .Replace("d2", "2").Replace("d3", "3")
                .Replace("d4", "4").Replace("d5", "5")
                .Replace("d6", "6").Replace("d7", "7")
                .Replace("d8", "8").Replace("d9", "9")
                .Replace("capital", "capsLock")
                .Replace("return", "enter");
            x = x[0].ToString().ToUpperInvariant() + x.Substring(1);
            return x;
        }
        void loadHidden()
        {
            Hchk_LMBTrayLayoutChange.Checked = Program.MyConfs.ReadBool("Hidden", "ChangeLayoutOnTrayLMB");
            Hchk_DisableMemFlush.Checked = nomemoryflush = Program.MyConfs.ReadBool("Hidden", "DisableMemoryFlush");
            Htxt_SymbolClear.Text = KMHook.symbolclear = Program.MyConfs.Read("Hidden", "SymbolClear");
            Hchk_LibrePasteFixCASV.Checked = LibreCtrlAltShiftV = Program.MyConfs.ReadBool("Hidden", "LibreCtrlAltShiftV");
            Htxt_CycleCaseOrder.Text = CycleCaseOrder = Program.MyConfs.Read("Hidden", "CycleCaseOrder");
            Hchk_CycleCaseReset.Checked = CycleCaseReset = Program.MyConfs.ReadBool("Hidden", "CycleCaseReset");
            Hchk___selection.Checked = __selection = Program.MyConfs.ReadBool("Hidden", "__selection");
            Hchk___selection_nomouse.Checked = __selection_nomouse = Program.MyConfs.ReadBool("Hidden", "__selection_nomouse");
            Htxt_OSnippetsExcluded.Text = onlySnippetsExcluded = Program.MyConfs.Read("Hidden", "onlySnippetsExcluded");
            Htxt_OAutoSwitchExcluded.Text = onlyAutoSwitchExcluded = Program.MyConfs.Read("Hidden", "onlyAutoSwitchExcluded");
            Htxt_OverlayExcluded.Text = OverlayExcluded = Program.MyConfs.Read("Hidden", "OverlayExcluded");
            Hnud_OverlayExcludedInterval.Value = OverlayExcludedInerval = Program.MyConfs.ReadInt("Hidden", "OverlayExcludedInterval");
            Htxt_AutoCopyTranslation.Text = AutoCopyTranslation = Program.MyConfs.Read("Hidden", "AutoCopyTranslation");
            Hchk_ASIgnoreBack.Checked = KMHook.AS_IGN_BACK = Program.MyConfs.ReadBool("Hidden", "AS_IngoreBack");
            Hchk_ASIgnoreDel.Checked = KMHook.AS_IGN_DEL = Program.MyConfs.ReadBool("Hidden", "AS_IngoreDel");
            Hchk_ASIgnoreLS.Checked = KMHook.AS_IGN_LS = Program.MyConfs.ReadBool("Hidden", "AS_IngoreLS");
            Htxt_AutoSwitchIngoreRules.Text = KMHook.AS_IGN_RULES = Program.MyConfs.Read("Hidden", "AS_IngoreRules").ToUpper();
            Hnud_ASIgnoreTimeout.Value = KMHook.AS_IGN_TIMEOUT = Program.MyConfs.ReadInt("Hidden", "AS_IngoreLSTimeout");
            Hchk_NCStray.Checked = Program.MyConfs.ReadBool("Hidden", "NCS_tray");
            Htxt_NCS.Text = ncs = Program.MyConfs.Read("Hidden", "NCS").ToUpper();
            Htxt_AutoSwitchHotkeyStr.Text = tas = Program.MyConfs.Read("Hidden", "ToggleAutoSwitchHK");
            Hnud_AutoRestartMins.Value = arm = Program.MyConfs.ReadInt("Hidden", "AutoRestartMins");
            Hchk___setlayoutForce.Checked = __setlayoutForce = Program.MyConfs.ReadBool("Hidden", "__setlayout_FORCED");
            Hchk___setlayoutOnlyWM.Checked = __setlayoutOnlyWM = Program.MyConfs.ReadBool("Hidden", "__setlayout_ONLYWM");
            Htxt_ReselectCustoms.Text = ReselectCustoms = Program.MyConfs.Read("Hidden", "ReSelectCustoms");
            Hchk_LMBTrayLayoutChangeDC.Checked = Program.MyConfs.ReadBool("Hidden", "ChangeLayoutOnTrayLMB+DoubleClick");
            TrayHoverSwitcherMM = Program.MyConfs.ReadInt("Hidden", "TrayHoverSwitcherMM");
            Htxt_Redefines.Text = Redefines = Program.MyConfs.Read("Hidden", "Redefines");
            ClipBackOnlyText = Hchk_ClipBackOnlyText.Checked = Program.MyConfs.ReadBool("Hidden", "ClipBackOnlyText");
            KMHook.AS_END_symbols = Htxt_ASEndSymbols.Text = Program.MyConfs.Read("Hidden", "AutoSwitchEndingSymbols");
            SwitcherMMTrayHoverLostFocusClose = Program.MyConfs.ReadBool("Hidden", "SwitcherMMTrayHoverLostFocusClose");
            CycleCaseSaveBase = Hchk_SaveBase.Checked = Program.MyConfs.ReadBool("Hidden", "CycleCaseSaveBase");
            Layout1ModifierKey = Program.MyConfs.ReadInt("Hidden", "Layout_1_Modifier_Key");
            Layout2ModifierKey = Program.MyConfs.ReadInt("Hidden", "Layout_2_Modifier_Key");
            LayoutDModifierKey = Program.MyConfs.ReadInt("Hidden", "Layout_D_Modifier_Key");
            if (Program.MyConfs.ReadBool("Hidden", "DARKTHEME"))
            {
                Hchk_DARK.Checked = true;
            }
            Hchk_cmdbackfix.Checked = cmdbackfix = Program.MyConfs.ReadBool("Hidden", "cmdbackfix");
            try { var k = (Keys)Layout1ModifierKey; Htxt_LayoutModifier_1.Text = k == Keys.None ? "" : KeynameReplace(k.ToString()); } catch { Logging.Log("Layout modifier 1 key code is not valid key."); }
            try { var k = (Keys)Layout2ModifierKey; Htxt_LayoutModifier_2.Text = k == Keys.None ? "" : KeynameReplace(k.ToString()); } catch { Logging.Log("Layout modifier 2 key code is not valid key."); }
            try { var k = (Keys)LayoutDModifierKey; Htxt_LayoutModifier_D.Text = k == Keys.None ? "" : KeynameReplace(k.ToString()); } catch { Logging.Log("Layout modifier D key code is not valid key."); }
            parseRedefines();
            Hnud_TrayHoverMM.Value = TrayHoverSwitcherMM;
            if (!String.IsNullOrEmpty(OverlayExcluded))
            {
                Debug.WriteLine("Starting overlay excluded");
                if (overlay_excluder != null)
                {
                    overlay_excluder.Stop();
                    overlay_excluder.Dispose();
                }
                overlay_excluder = new Timer();
                overlay_excluder.Interval = OverlayExcludedInerval;
                overlay_excluder.Tick += (_, __) =>
                {
                    OVEXDisabled = ENABLED = KMHook.OverlayExcluded(OverlayExcluded);
                    //					Debug.WriteLine("Toggle FaineSwitcher to " +(!ENABLED));
                    ToggleSwitcher();
                };
                overlay_excluder.Start();
            }
            else
            {
                if (overlay_excluder != null)
                {
                    if (OVEXDisabled)
                    {
                        ToggleSwitcher();
                    }
                    overlay_excluder.Stop();
                    overlay_excluder.Dispose();
                }
            }
            if (arm > 0 && armt != null)
            {
                armt = new Timer();
                armt.Interval = 1000 * arm * 60;
                armt.Tick += (_, __) => Restart();
                armt.Start();
            }
        }
        /// <summary>
        /// Refresh all controls state from configs.
        /// </summary>
        void LoadConfigs()
        {
            configs_loading = true;
            loadHidden();
            decim = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sDecimal", null);
            TrSetsValues = new Dictionary<string, string>();
            chk_AppDataConfigs.Checked = (bool)DoInMainConfigs(() => Program.MyConfs.ReadBool("Functions", "AppDataConfigs"));
            UpdateSaveLoadPaths(chk_AppDataConfigs.Checked);
            InitializeTrayIcon();
            InitLanguage();
            SnippetsExpKeyOther = Program.MyConfs.Read("Snippets", "SnippetsExpKeyOther");
            RefreshLanguage();
            #region Functions
            Program.MyConfs = new Configs();
            AutoStartAsAdmin = Program.MyConfs.ReadBool("Functions", "AutoStartAsAdmin");
            if(chk_AutoStart.Checked && !AutoStartExist(AutoStartAsAdmin))
                CreateAutoStart();
            chk_AutoStart.Checked = AutoStartExist(AutoStartAsAdmin);
            lbl_TaskExist.Visible = AutoStartExist(true);
            lbl_LinkExist.Visible = AutoStartExist(false);
            TrayIconVisible = chk_TrayIcon.Checked = Program.MyConfs.ReadBool("Functions", "TrayIconVisible");
            ConvertSelectionLS = chk_CSLayoutSwitching.Checked = Program.MyConfs.ReadBool("Functions", "ConvertSelectionLayoutSwitching");
            ReSelect = chk_ReSelect.Checked = Program.MyConfs.ReadBool("Functions", "ReSelect");
            RePress = chk_RePress.Checked = Program.MyConfs.ReadBool("Functions", "RePress");
            AddOneSpace = chk_AddOneSpace.Checked = Program.MyConfs.ReadBool("Functions", "AddOneSpaceToLastWord");
            Add1NL = chk_Add1NL.Checked = Program.MyConfs.ReadBool("Functions", "AddOneEnterToLastWord");
            ConvertSelectionLSPlus = chk_CSLayoutSwitchingPlus.Checked = Program.MyConfs.ReadBool("Functions", "ConvertSelectionLayoutSwitchingPlus");
            ScrollTip = chk_HighlightScroll.Checked = Program.MyConfs.ReadBool("Functions", "ScrollTip");
            chk_StartupUpdatesCheck.Checked = Program.MyConfs.ReadBool("Functions", "StartupUpdatesCheck");
            chk_SilentUpdate.Checked = Program.MyConfs.ReadBool("Functions", "SilentUpdate");
            LoggingEnabled = chk_Logging.Checked = Program.MyConfs.ReadBool("Functions", "Logging");
            latest_save_dir = nPath;
            if (LoggingEnabled)
                Program._logTimer.Change(300, 0);
            else
                Program._logTimer.Change(0, 0);
            TrayFlags = Program.MyConfs.ReadBool("Functions", "TrayFlags");
            TrayText = Program.MyConfs.ReadBool("Functions", "TrayText");
            CapsLockDisablerTimer = chk_CapsLockDTimer.Checked = Program.MyConfs.ReadBool("Functions", "CapsLockTimer");
            BlockHKWithCtrl = chk_BlockHKWithCtrl.Checked = Program.MyConfs.ReadBool("Functions", "BlockSwitcherHotkeysWithCtrl");
            SymIgnEnabled = Program.MyConfs.ReadBool("Functions", "SymbolIgnoreModeEnabled");
            MCDSSupport = chk_MCDS_support.Checked = Program.MyConfs.ReadBool("Functions", "MCDServerSupport");
            OneLayoutWholeWord = chk_OneLayoutWholeWord.Checked = Program.MyConfs.ReadBool("Functions", "OneLayoutWholeWord");
            MouseLangTooltipEnabled = Program.MyConfs.ReadBool("Appearence", "DisplayLangTooltipForMouse");
            CaretLangTooltipEnabled = Program.MyConfs.ReadBool("Appearence", "DisplayLangTooltipForCaret");
            GuessKeyCodeFix = chk_GuessKeyCodeFix.Checked = Program.MyConfs.ReadBool("Functions", "GuessKeyCodeFix");
            RemapCapslockAsF18 = Program.MyConfs.ReadBool("Functions", "RemapCapslockAsF18");
            UseJKL = chk_GetLayoutFromJKL.Checked = Program.MyConfs.ReadBool("Functions", "UseJKL");
            ReadOnlyNA = chk_ReadOnlyNA.Checked = Program.MyConfs.ReadBool("Functions", "ReadOnlyNA");
            WriteInputHistory = chk_WriteInputHistory.Checked = Program.MyConfs.ReadBool("Functions", "WriteInputHistory");
            WriteInputHistoryByDate = Program.MyConfs.ReadBool("Functions", "WriteInputHistoryByDate");
            WriteInputHistoryHourly = Program.MyConfs.ReadBool("Functions", "WriteInputHistoryHourly");
            #endregion
            #region Layouts
            //SwitchBetweenLayouts = chk_SwitchBetweenLayouts.Checked = Program.MyConfs.ReadBool("Layouts", "SwitchBetweenLayouts");
            //EmulateLS = chk_EmulateLS.Checked = Program.MyConfs.ReadBool("Layouts", "EmulateLayoutSwitch");
            //ChangeLayouByKey = chk_SpecificLS.Checked = Program.MyConfs.ReadBool("Layouts", "ChangeToSpecificLayoutByKey");
            //MainLayout1 = Program.MyConfs.Read("Layouts", "MainLayout1");
            //MainLayout2 = Program.MyConfs.Read("Layouts", "MainLayout2");
            //MAIN_LAYOUT1 = Locales.GetLocaleFromString(MainLayout1).uId;
            //MAIN_LAYOUT2 = Locales.GetLocaleFromString(MainLayout2).uId;
            //Layout1 = Program.MyConfs.Read("Layouts", "SpecificLayout1");
            //Layout2 = Program.MyConfs.Read("Layouts", "SpecificLayout2");
            //Layout3 = Program.MyConfs.Read("Layouts", "SpecificLayout3");
            //Layout4 = Program.MyConfs.Read("Layouts", "SpecificLayout4");
            //TestLayout(Layout1, 1);
            //TestLayout(Layout2, 2);
            //TestLayout(Layout3, 3);
            //TestLayout(Layout4, 4);
            //Layout1 = Program.MyConfs.Read("Layouts", "SpecificLayout1");
            //Layout2 = Program.MyConfs.Read("Layouts", "SpecificLayout2");
            //Layout3 = Program.MyConfs.Read("Layouts", "SpecificLayout3");
            //Layout4 = Program.MyConfs.Read("Layouts", "SpecificLayout4");
            //Key1 = Program.MyConfs.ReadInt("Layouts", "SpecificKey1");
            //Key2 = Program.MyConfs.ReadInt("Layouts", "SpecificKey2");
            //Key3 = Program.MyConfs.ReadInt("Layouts", "SpecificKey3");
            //Key4 = Program.MyConfs.ReadInt("Layouts", "SpecificKey4");
            //OneLayout = chk_OneLayout.Checked = Program.MyConfs.ReadBool("Layouts", "OneLayout");
            //QWERTZ_fix = chk_qwertz.Checked = Program.MyConfs.ReadBool("Layouts", "QWERTZfix");
            //txt_LCTRLLALTTempLayout.Text = Program.MyConfs.Read("Layouts", "CTRL_ALT_TemporaryChangeLayout");
            //UInt32.TryParse(txt_LCTRLLALTTempLayout.Text, out CTRL_ALT_TemporaryLayout);
            //LoadSpecKeySetsValues();
            #endregion
            #region Persistent Layout
            PersistentLayoutOnWindowChange = chk_OnlyOnWindowChange.Checked = Program.MyConfs.ReadBool("PersistentLayout", "OnlyOnWindowChange");
            PersistentLayoutOnlyOnce = chk_ChangeLayoutOnlyOnce.Checked = Program.MyConfs.ReadBool("PersistentLayout", "ChangeOnlyOnce");
            KMHook.PLC_HWNDs.Clear();
            KMHook.ConHost_HWNDs.Clear();
            PERSISTENT_LAYOUT1_HWNDs.Clear();
            NOT_PERSISTENT_LAYOUT1_HWNDs.Clear();
            PERSISTENT_LAYOUT2_HWNDs.Clear();
            NOT_PERSISTENT_LAYOUT2_HWNDs.Clear();
            PersistentLayoutForLayout1 = chk_PersistentLayout1Active.Checked = Program.MyConfs.ReadBool("PersistentLayout", "ActivateForLayout1");
            PersistentLayoutForLayout2 = chk_PersistentLayout2Active.Checked = Program.MyConfs.ReadBool("PersistentLayout", "ActivateForLayout2");
            nud_PersistentLayout1Interval.Value = Program.MyConfs.ReadInt("PersistentLayout", "Layout1CheckInterval");
            nud_PersistentLayout2Interval.Value = Program.MyConfs.ReadInt("PersistentLayout", "Layout2CheckInterval");
            PersistentLayout1Processes = txt_PersistentLayout1Processes.Text = Program.MyConfs.Read("PersistentLayout", "Layout1Processes").Replace("^cr^lf", Environment.NewLine);
            PersistentLayout2Processes = txt_PersistentLayout2Processes.Text = Program.MyConfs.Read("PersistentLayout", "Layout2Processes").Replace("^cr^lf", Environment.NewLine);
            #endregion
            #region Appearence
            LDForMouse = chk_LangTooltipMouse.Checked = Program.MyConfs.ReadBool("Appearence", "DisplayLangTooltipForMouse");
            LDForCaret = chk_LangTooltipCaret.Checked = Program.MyConfs.ReadBool("Appearence", "DisplayLangTooltipForCaret");
            LDForMouseOnChange = chk_LangTTMouseOnChange.Checked = Program.MyConfs.ReadBool("Appearence", "DisplayLangTooltipForMouseOnChange");
            LDForCaretOnChange = chk_LangTTCaretOnChange.Checked = Program.MyConfs.ReadBool("Appearence", "DisplayLangTooltipForCaretOnChange");
            DiffAppearenceForLayouts = chk_LangTTDiffLayoutColors.Checked = Program.MyConfs.ReadBool("Appearence", "DifferentColorsForLayouts");
            MouseTTAlways = chk_MouseTTAlways.Checked = Program.MyConfs.ReadBool("Appearence", "MouseLTAlways");
            mouseLTUpperArrow = Program.MyConfs.ReadBool("Appearence", "MouseLTUpperArrow");
            caretLTUpperArrow = Program.MyConfs.ReadBool("Appearence", "CaretLTUpperArrow");
            LDUseWindowsMessages = chk_LDMessages.Checked = Program.MyConfs.ReadBool("Appearence", "WindowsMessages");
            #endregion
            #region Timings
            LD_MouseSkipMessagesCount = Program.MyConfs.ReadInt("Timings", "LangTooltipForMouseSkipMessages");
            if (LDUseWindowsMessages)
            {
                nud_LangTTMouseRefreshRate.Maximum = 100;
                nud_LangTTMouseRefreshRate.Minimum = 0;
                nud_LangTTMouseRefreshRate.Increment = 1;
                nud_LangTTMouseRefreshRate.Value = LD_MouseSkipMessagesCount;
                lbl_LangTTMouseRefreshRate.Text = Program.Lang[Languages.Element.LD_MouseSkipMessages];
            }
            else
            {
                nud_LangTTMouseRefreshRate.Maximum = 2500;
                nud_LangTTMouseRefreshRate.Minimum = 1;
                nud_LangTTMouseRefreshRate.Increment = 25;
                nud_LangTTMouseRefreshRate.Value = Program.MyConfs.ReadInt("Timings", "LangTooltipForMouseRefreshRate");
            }
            nud_LangTTCaretRefreshRate.Value = Program.MyConfs.ReadInt("Timings", "LangTooltipForCaretRefreshRate");
            nud_DoubleHK2ndPressWaitTime.Value = DoubleHKInterval = Program.MyConfs.ReadInt("Timings", "DoubleHotkey2ndPressWait");
            nud_TrayFlagRefreshRate.Value = Program.MyConfs.ReadInt("Timings", "FlagsInTrayRefreshRate");
            nud_ScrollLockRefreshRate.Value = Program.MyConfs.ReadInt("Timings", "ScrollLockStateRefreshRate");
            nud_CapsLockRefreshRate.Value = Program.MyConfs.ReadInt("Timings", "CapsLockDisableRefreshRate");
            nud_ScrollLockRefreshRate.Value = Program.MyConfs.ReadInt("Timings", "ScrollLockStateRefreshRate");
            SelectedTextGetMoreTries = chk_SelectedTextGetMoreTries.Checked = Program.MyConfs.ReadBool("Timings", "SelectedTextGetMoreTries");
            nud_SelectedTextGetTriesCount.Value = Program.MyConfs.ReadInt("Timings", "SelectedTextGetMoreTriesCount");
            nud_DelayAfterBackspaces.Value = DelayAfterBackspaces = Program.MyConfs.ReadInt("Timings", "DelayAfterBackspaces");
            UseDelayAfterBackspaces = chk_UseDelayAfterBackspaces.Checked = Program.MyConfs.ReadBool("Timings", "UseDelayAfterBackspaces");
            #region Excluded
            ExcludeCaretLD = Program.MyConfs.ReadBool("Timings", "ExcludeCaretLD");
            UsePaste = chk_CSUsePaste.Checked = Program.MyConfs.ReadBool("Timings", "UsePasteInCS");
            ExcludedPrograms = txt_ExcludedPrograms.Text = Program.MyConfs.Read("Timings", "ExcludedPrograms").Replace("^cr^lf", Environment.NewLine);
            KMHook.EXCLUDED_HWNDs.Clear();
            KMHook.NOT_EXCLUDED_HWNDs.Clear();
            KMHook.AS_EXCLUDED_HWNDs.Clear();
            KMHook.AS_NOT_EXCLUDED_HWNDs.Clear();
            KMHook.SNI_EXCLUDED_HWNDs.Clear();
            KMHook.SNI_NOT_EXCLUDED_HWNDs.Clear();
            ChangeLayoutInExcluded = chk_Change1KeyL.Checked = Program.MyConfs.ReadBool("Timings", "ChangeLayoutInExcluded");
            ConvertSWLinExcl = chk_ConvSWL.Checked = Program.MyConfs.ReadBool("Timings", "ConvertSWLinExcl");
            #endregion
            SelectedTextGetMoreTriesCount = (int)nud_SelectedTextGetTriesCount.Value;
            #endregion
            #region LangPanel
            LangPanelDisplay = chk_DisplayLangPanel.Checked = Program.MyConfs.ReadBool("LangPanel", "Display");
            nud_LPRefreshRate.Value = LangPanelRefreshRate = Program.MyConfs.ReadInt("LangPanel", "RefreshRate");
            nud_LPTransparency.Value = LangPanelTransparency = Program.MyConfs.ReadInt("LangPanel", "Transparency");
            btn_LPFore.BackColor = LangPanelForeColor = GetColor(Program.MyConfs.Read("LangPanel", "ForeColor"));
            btn_LPBack.BackColor = LangPanelBackColor = GetColor(Program.MyConfs.Read("LangPanel", "BackColor"));
            btn_LPBorderColor.BackColor = LangPanelBorderColor = GetColor(Program.MyConfs.Read("LangPanel", "BorderColor"));
            LangPanelBorderAero = chk_LPAeroColor.Checked = Program.MyConfs.ReadBool("LangPanel", "BorderAeroColor");
            btn_LPFont.Font = LangPanelFont = GetFont(Program.MyConfs.Read("LangPanel", "Font"));
            LangPanelUpperArrow = chk_LPUpperArrow.Checked = Program.MyConfs.ReadBool("LangPanel", "UpperArrow");
            #endregion
            #region Translate Panel
            //TrEnabled = chk_TrEnable.Checked = Program.MyConfs.ReadBool("TranslatePanel", "Enabled");
            chk_TrEnable.Enabled = false;
            TrEnabled = chk_TrEnable.Checked = false;
            TranslatePanel.useGS = Program.MyConfs.ReadBool("TranslatePanel", "UseGS");
            TranslatePanel.useNA = Program.MyConfs.ReadBool("TranslatePanel", "UseNA");
            TrOnDoubleClick = chk_TrOnDoubleClick.Checked = Program.MyConfs.ReadBool("TranslatePanel", "OnDoubleClick");
            nud_TrTransparency.Value = TrTransparency = Program.MyConfs.ReadInt("TranslatePanel", "Transparency");
            btn_TrFG.BackColor = TrFore = GetColor(Program.MyConfs.Read("TranslatePanel", "FG"));
            btn_TrBG.BackColor = TrBack = GetColor(Program.MyConfs.Read("TranslatePanel", "BG"));
            btn_TrBorderC.BackColor = TrBorder = GetColor(Program.MyConfs.Read("TranslatePanel", "BorderC"));
            TrBorderAero = chk_TrUseAccent.Checked = Program.MyConfs.ReadBool("TranslatePanel", "BorderAero");
            LoadTrSetsValues();
            RefreshComboboxes();
            if (TrEnabled)
            {
                if (_TranslatePanel == null)
                    _TranslatePanel = new TranslatePanel();
                _TranslatePanel.SetTitle(Program.Lang[Languages.Element.Translation]);
                chk_TrTranscription.Checked = TranslatePanel.TRANSCRIPTION = Program.MyConfs.ReadBool("TranslatePanel", "Transcription");
            }
            else
            {
                if (_TranslatePanel != null)
                    _TranslatePanel.Dispose();
            }
            TrText = btn_TrTextFont.Font = GetFont(Program.MyConfs.Read("TranslatePanel", "TextFont"));
            TrTitle = btn_TrTitleFont.Font = GetFont(Program.MyConfs.Read("TranslatePanel", "TitleFont"));
            #endregion
            #region Snippets
            SnippetsEnabled = chk_Snippets.Checked = Program.MyConfs.ReadBool("Snippets", "SnippetsEnabled");
            SnippetSpaceAfter = chk_SnippetsSpaceAfter.Checked = Program.MyConfs.ReadBool("Snippets", "SpaceAfter");
            SnippetsSwitchToGuessLayout = chk_SnippetsSwitchToGuessLayout.Checked = Program.MyConfs.ReadBool("Snippets", "SwitchToGuessLayout");
            SnippetsExpandType = Program.MyConfs.Read("Snippets", "SnippetExpandKey");
            cbb_SnippetExpandKeys.SelectedIndex = cbb_SnippetExpandKeys.Items.IndexOf(SnippetsExpandType);
            LoadNCRSets();
            #endregion
            #region AutoSwitch
            AutoSwitchEnabled = chk_AutoSwitch.Checked = Program.MyConfs.ReadBool("AutoSwitch", "Enabled");
            AutoSwitchSpaceAfter = chk_AutoSwitchSpaceAfter.Checked = Program.MyConfs.ReadBool("AutoSwitch", "SpaceAfter");
            AutoSwitchSwitchToGuessLayout = chk_AutoSwitchSwitchToGuessLayout.Checked = Program.MyConfs.ReadBool("AutoSwitch", "SwitchToGuessLayout");
            Dowload_ASD_InZip = chk_DownloadASD_InZip.Checked = Program.MyConfs.ReadBool("AutoSwitch", "DownloadInZip");
            check_ASD_size = true;
            if (AutoSwitchEnabled && SnippetsEnabled)
                if (File.Exists(AS_dictfile) && !AutoSwitchDictionaryTooBig)
                {
                    AutoSwitchDictionaryRaw = File.ReadAllText(AS_dictfile);
                    AutoSwitchDictionaryTooBig = AutoSwitchDictionaryRaw.Length > 710000;
                    ChangeAutoSwitchDictionaryTextBox();
                    UpdateSnippetCountLabel(AutoSwitchDictionaryRaw, lbl_AutoSwitchWordsCount, false);
                }
                else if(!File.Exists(AS_dictfile))
                {
                    AutoSwitchDictionaryRaw = Properties.Resources.AS_dict;
                    File.WriteAllText(AS_dictfile, AutoSwitchDictionaryRaw);
                    AutoSwitchDictionaryTooBig = AutoSwitchDictionaryRaw.Length > 710000;
                    ChangeAutoSwitchDictionaryTextBox();
                    UpdateSnippetCountLabel(AutoSwitchDictionaryRaw, lbl_AutoSwitchWordsCount, false);
                }

            SwitcherUIActivated((object)1, new EventArgs());
            if (SnippetsEnabled)
            {
                if (!File.Exists(snipfile) || String.IsNullOrWhiteSpace(File.ReadAllText(snipfile)))
                    File.WriteAllText(snipfile, txt_Snippets.Text, Encoding.UTF8);
                if (File.Exists(snipfile))
                {
                    txt_Snippets.Text = File.ReadAllText(snipfile);
                    UpdateSnippetCountLabel(txt_Snippets.Text, lbl_SnippetsCount);
                    KMHook.ReInitSnippets();
                    KMHook.DoLater(() =>
                    {
                        if (KMHook.snipps.Length != SnippetsCount || KMHook.as_corrects.Length != AutoSwitchCount)
                            KMHook.ReInitSnippets();
                    }, 650);
                }
            }
            #endregion
            LoadTemps();
            #region DICT reload
            KMHook.ReloadTSDict();
            if (QWERTZ_fix)
            {
                KMHook.ReloadLayReplDict();
                KMHook.ReloadASsymDiffDict();
            }
            if (HKSelCustConv_tempEnabled)
            {
                KMHook.ReloadCusRepDict();
            }
            KMHook.LayoutKeyReplaceInit();
            #endregion
            #region Appearence & Hotkeys
            UpdateLangDisplayControlsSwitch();
            UpdateHotkeyControlsSwitch();
            #endregion
            #region Proxy
            txt_ProxyServerPort.Text = Program.MyConfs.Read("Proxy", "ServerPort");
            txt_ProxyLogin.Text = Program.MyConfs.Read("Proxy", "UserName");
            try
            {
                txt_ProxyPassword.Text = Encoding.Unicode.GetString(Convert.FromBase64String(Program.MyConfs.Read("Proxy", "Password")));
            }
            catch { Logging.Log("Password invalidly encoded, reset to none.", 2); }
            #endregion
            #region Sounds
            SoundEnabled = chk_EnableSnd.Checked = Program.MyConfs.ReadBool("Sounds", "Enabled");
            SoundOnAutoSwitch = chk_SndAutoSwitch.Checked = Program.MyConfs.ReadBool("Sounds", "OnAutoSwitch");
            SoundOnSnippets = chk_SndSnippets.Checked = Program.MyConfs.ReadBool("Sounds", "OnSnippets");
            SoundOnConvLast = chk_SndLast.Checked = Program.MyConfs.ReadBool("Sounds", "OnConvertLast");
            SoundOnLayoutSwitch = chk_SndLayoutSwitch.Checked = Program.MyConfs.ReadBool("Sounds", "OnLayoutSwitch");
            UseCustomSound = chk_UseCustomSnd.Checked = Program.MyConfs.ReadBool("Sounds", "UseCustomSound");
            CustomSound = lbl_CustomSound.Text = Program.MyConfs.Read("Sounds", "CustomSound");
            SoundOnAutoSwitch2 = chk_SndAutoSwitch2.Checked = Program.MyConfs.ReadBool("Sounds", "OnAutoSwitch2");
            SoundOnSnippets2 = chk_SndSnippets2.Checked = Program.MyConfs.ReadBool("Sounds", "OnSnippets2");
            SoundOnConvLast2 = chk_SndLast2.Checked = Program.MyConfs.ReadBool("Sounds", "OnConvertLast2");
            SoundOnLayoutSwitch2 = chk_SndLayoutSwitch2.Checked = Program.MyConfs.ReadBool("Sounds", "OnLayoutSwitch2");
            UseCustomSound2 = chk_UseCustomSnd2.Checked = Program.MyConfs.ReadBool("Sounds", "UseCustomSound2");
            CustomSound2 = lbl_CustomSound2.Text = Program.MyConfs.Read("Sounds", "CustomSound2");
            var lbCSh = lbl_CustomSound.Text;
            var lbCSh2 = lbl_CustomSound2.Text;
            if (!File.Exists(replaceenv(CustomSound, "%Switcher_dir%", () => nPath)))
            {
                lbl_CustomSound.ForeColor = Color.Red;
                lbCSh = Program.Lang[Languages.Element.Not] + " " + Program.Lang[Languages.Element.Exist] + ":\r\n[" + lbl_CustomSound.Text + "]";
            }
            else
                lbl_CustomSound.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
            if (!File.Exists(replaceenv(CustomSound2, "%Switcher_dir%", () => nPath)))
            {
                lbl_CustomSound2.ForeColor = Color.Red;
                lbCSh2 = Program.Lang[Languages.Element.Not] + " " + Program.Lang[Languages.Element.Exist] + ":\r\n[" + lbl_CustomSound2.Text + "]";
            }
            else
                lbl_CustomSound2.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
            HelpMeUnderstand.SetToolTip(lbl_CustomSound, lbCSh);
            HelpMeUnderstand.SetToolTip(lbl_CustomSound2, lbCSh2);
            #endregion
            #region Sync
            var bbools = Program.MyConfs.Read("Sync", "BBools");
            bool m, s, h, t, p, mm;
            SetBools(bbools, '|', out m, out s, out h, out t, out p, out mm);
            chk_Mini.Checked = m;
            chk_Stxt.Checked = s;
            chk_Htxt.Checked = h;
            chk_Ttxt.Checked = t;
            chk_andPROXY.Checked = p;
            chk_Mmm.Checked = mm;
            var rbools = Program.MyConfs.Read("Sync", "RBools");
            SetBools(rbools, '|', out m, out s, out h, out t, out p, out mm);
            chk_rMini.Checked = m;
            chk_rStxt.Checked = s;
            chk_rHtxt.Checked = h;
            chk_rTtxt.Checked = t;
            chk_andPROXY2.Checked = p;
            chk_rMmm.Checked = mm;
            var blast = Program.MyConfs.Read("Sync", "BLast");
            if (!string.IsNullOrEmpty(blast))
            {
                txt_backupId.Text = blast;
                txt_backupId.Enabled = true;
            }
            var rlast = Program.MyConfs.Read("Sync", "RLast");
            if (!string.IsNullOrEmpty(rlast))
                txt_restoreId.Text = rlast;
            chk_ZxZ.Checked = ZxZ = Program.MyConfs.ReadBool("Sync", "ZxZ");
            #endregion
            LLHook._ACTIVE = (RemapCapslockAsF18 || SnippetsExpandType != "Space" || SwitcherMM || LLHook.redefines.len > 0);
            if (LLHook._ACTIVE)
                LLHook.Set();
            else
                LLHook.UnSet();
            InitializeHotkeys();
            InitializeTimers();
            InitializeLangPanel();
            ToggleDependentControlsEnabledState();
            RefreshAllIcons(true);
            if (_langPanel != null)
            {
                _langPanel.UpdateApperence(LangPanelBackColor, LangPanelForeColor, LangPanelTransparency, LangPanelFont);
                if (LangPanelDisplay)
                    _langPanel.ShowInactiveTopmost();
                else
                    _langPanel.HideWnd();
            }
            // Restore last positon
            lsb_LangTTAppearenceForList.SelectedIndex = tmpLangTTAppearenceIndex;
            lsb_Hotkeys.SelectedIndex = tmpHotkeysIndex;
            if (UseJKL)
            {
                if (!jklXHidServ.jklExist())
                {
                    chk_GetLayoutFromJKL.ForeColor = Color.Red;
                    HelpMeUnderstand.SetToolTip(chk_GetLayoutFromJKL, jklXHidServ.jklInfoStr);
                }
                else
                {
                    chk_GetLayoutFromJKL.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                    HelpMeUnderstand.SetToolTip(chk_GetLayoutFromJKL, Program.Lang[Languages.Element.TT_UseJKL]);
                }
                jklXHidServ.Init();
            }
            else
            {
                jklXHidServ.Destroy();
                chk_GetLayoutFromJKL.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                HelpMeUnderstand.SetToolTip(chk_GetLayoutFromJKL, Program.Lang[Languages.Element.TT_UseJKL]);
            }
            UnregisterHotkeys();
            RegisterHotkeys();
            if (Hchk_NCStray.Checked)
            {
                NCS_tray();
            }
            else
            {
                NCS_destroy();
            }
            Memory.Flush();
            configs_loading = false;
            Logging.Log("All configurations loaded.");
        }
        List<string[]> ParseSets(string raw_sets, char sep = '|', char sep2 = '/')
        {
            if (raw_sets.Contains("set_0") || raw_sets.Contains("set0")) return new List<string[]>();
            var sets = raw_sets.Split(sep);
            var last_set = sets[sets.Length - 1];
            //			Debug.WriteLine(last_set);
            //			var set_count = Int32.Parse(last_set.Split('/')[0].Replace("set_",""));
            var SETS = new List<string[]>();
            foreach (var _set in sets)
            {
                SETS.Add(_set.Split(sep2));
            }
            return SETS;
        }
        void LoadTrSetsValues()
        {
            var sets_raw = Program.MyConfs.Read("TranslatePanel", "LanguageSets");
            var SETS = ParseSets(sets_raw);
            if (SETS.Count == 0) return;
            var NOTR = TrSetCount == 0;
            if (NOTR)
                pan_TrSets.Controls.Clear();
            for (int i = 1; i != SETS.Count + 1; i++)
            {
                if (NOTR)
                    Btn_TrAddSetClick((object)1, new EventArgs());
                var values = SETS[i - 1];
                TrSetsValues["cbb_fr" + i] = values[1];
                TrSetsValues["cbb_to" + i] = values[2];
                //				var key = 0;
                //				if (!String.IsNullOrEmpty(values[1]))
                //					key = Int32.Parse(values[1]);
                //				UpdateSetControls(i, key, values[2]);
            }
        }
        void LoadSpecKeySetsValues()
        {
            var sets_raw = Program.MyConfs.Read("Layouts", "SpecificKeySets");
            var SETS = ParseSets(sets_raw);
            if (SETS.Count == 0) return;
            var NOSPEC = SpecKeySetCount == 0;
            //if (NOSPEC)
            //	pan_KeySets.Controls.Clear();
            // Initilize sets
            for (int i = 1; i != SETS.Count + 1; i++)
            {
                if (NOSPEC)
                    Btn_AddSetClick((object)1, new EventArgs());
                var values = SETS[i - 1];
                SpecKeySetsValues["txt_key" + i + "_key"] = values[1];
                SpecKeySetsValues["txt_key" + i + "_mods"] = values[2];
                SpecKeySetsValues["cbb_typ" + i] = values[3];
                if (!String.IsNullOrEmpty(values[3]))
                {
                    if ((values[3] == Languages.English[Languages.Element.SwitchBetween] && Program.Lang == Languages.Russian) ||
                        (values[3] == Languages.Russian[Languages.Element.SwitchBetween] && Program.Lang == Languages.English))
                    {
                        SaveSpecificKeySets(true, i, Program.Lang[Languages.Element.SwitchBetween]);
                        SpecKeySetsValues["cbb_typ" + i] = Program.Lang[Languages.Element.SwitchBetween];
                    }
                }
                var key = 0;
                if (!String.IsNullOrEmpty(values[1]))
                    key = Int32.Parse(values[1]);
                UpdateSetControls(i, key, values[2]);
            }
        }
        Tuple<int, Color, int, string> GetSnippetsCount(string snippets)
        {
            if (String.IsNullOrEmpty(snippets)) return new Tuple<int, Color, int, string>(0, Color.Black, 0, "");
            Logging.Log("Starting counting snippets...");
            Stopwatch watch = null;
            if (SwitcherUI.LoggingEnabled)
            {
                watch = new Stopwatch();
                watch.Start();
            }
            // This regex is ~x8 slower than the way above. 
            //			var matches = Regex.Matches(snippets, "(->)|(====>)|(<====)", RegexOptions.Compiled);
            var com = 0;
            var ci = 0;
            var cia = 0;
            var cic = 0;
            var cir = new bool[snippets.Length];
            var ciar = new bool[snippets.Length];
            var cicr = new bool[snippets.Length];
            bool in_exp = false, ci_st = false;
            var cil = -1;
            var l = 0;
            for (int k = 0; k < snippets.Length - 1; k++)
            {
                // Do not try to store snippets[k] & snippets[k+n] to string variable, that will be significally slower.
                // with string.Concat() ~x15 slower, with string.Format() ~x45 slower.			
                var cml = KMHook.SnippetsLineCommented(snippets, k);
                if (cml.Item1)
                {
                    com++;
                    k += cml.Item2;
                    continue;
                }
                if (!in_exp && !ci_st && cil == -1 && (snippets[k].Equals('-') && snippets[k + 1].Equals('>')))
                {
                    ci_st = true;
                    ci++;
                    cil = l;
                    cir[cil] = true;
                }
                if (k + 4 < snippets.Length)
                {
                    if (ci_st && !in_exp && snippets[k].Equals('=') && snippets[k + 1].Equals('=') &&
                       snippets[k + 2].Equals('=') && snippets[k + 3].Equals('=') &&
                       snippets[k + 4].Equals('>'))
                    {
                        cia++;
                        ciar[cil] = true;
                        in_exp = true;
                        ci_st = false;
                    }
                    if (in_exp && snippets[k].Equals('<') && snippets[k + 1].Equals('=') &&
                       snippets[k + 2].Equals('=') && snippets[k + 3].Equals('=') &&
                       snippets[k + 4].Equals('='))
                    {
                        cic++;
                        cicr[cil] = true;
                        in_exp = false;
                        cil = -1;
                    }
                }
                if (snippets[k] == '\n')
                {
                    l++;
                }
            }
            var err = new StringBuilder();
            Logging.Log("Snippets word count details: " + cic + ", " + cia + ", " + ci + "<com> " + com);
            for (int k = 0; k != cir.Length; k++)
            {
                if (cir[k] != ciar[k] || cir[k] != cicr[k] || ciar[k] != cicr[k])
                {
                    err.Append((k + 1)).Append(" ");
                }
            }
            Debug.WriteLine("err " + err);
            var result = ci + cia + cic;
            if (SwitcherUI.LoggingEnabled)
            {
                watch.Stop();
                Logging.Log("Snippets with length [" + snippets.Length + "], snippets count [" + result / 3 + "], errors [" + (result % 3 != 0) + "], elapsed [" + watch.Elapsed.TotalMilliseconds + "] ms.");
            }
            Memory.Flush();
            if (result % 3 == 0)
                return new Tuple<int, Color, int, string>(result / 3, Color.Orange, com, "");
            return new Tuple<int, Color, int, string>(ci, Color.Red, com, err.ToString());
        }
        void TestLayout(string layout, int id)
        {
            if ((layout == Languages.English[Languages.Element.SwitchBetween] && Program.Lang == Languages.Russian) ||
                (layout == Languages.Russian[Languages.Element.SwitchBetween] && Program.Lang == Languages.English))
            {
                Program.MyConfs.WriteSave("Layouts", "SpecificLayout" + id, Program.Lang[Languages.Element.SwitchBetween]);
            }
        }
        /// <summary>
        /// Refreshes comboboxes items.
        /// </summary>
        void RefreshComboboxes()
        {
            cbb_AutostartType.SelectedIndex = AutoStartAsAdmin ? 1 : 0;
            cbb_UpdatesChannel.SelectedIndex = cbb_UpdatesChannel.Items.IndexOf(Program.MyConfs.Read("Updates", "Channel"));
            Program.locales = Locales.AllList();
            Program.RefreshLCnMID();
            cbb_BackSpaceType.Items.Clear();
            cbb_BackSpaceType.Items.Add(Program.Lang[Languages.Element.InputHistoryBackSpaceWriteType1]);
            cbb_BackSpaceType.Items.Add(Program.Lang[Languages.Element.InputHistoryBackSpaceWriteType2]);
            cbb_TrayDislpayType.Items.Clear();
            cbb_TrayDislpayType.Items.Add(Program.Lang[Languages.Element.JustIcon]);
            cbb_TrayDislpayType.Items.Add(Program.Lang[Languages.Element.ContryFlags]);
            cbb_TrayDislpayType.Items.Add(Program.Lang[Languages.Element.TextLayout]);
            cbb_TrMethod.Items.Clear();
            cbb_TrMethod.Items.Add(Program.Lang[Languages.Element.Direct]);
            cbb_TrMethod.Items.Add(Program.Lang[Languages.Element.WebScript]);
            cbb_TrMethod.Items.Add(Program.Lang[Languages.Element.DirectV2]);
            if (TranslatePanel.useGS)
                cbb_TrMethod.SelectedIndex = 1;
            else if (TranslatePanel.useNA)
                cbb_TrMethod.SelectedIndex = 2;
            else
                cbb_TrMethod.SelectedIndex = 0;
            if (TrayFlags)
                cbb_TrayDislpayType.SelectedIndex = 1;
            else if (TrayText)
                cbb_TrayDislpayType.SelectedIndex = 2;
            else
                cbb_TrayDislpayType.SelectedIndex = 0;
            InputHistoryBackSpaceWriteType = cbb_BackSpaceType.SelectedIndex = Program.MyConfs.ReadInt("Functions", "WriteInputHistoryBackSpaceType");
            if (SpecKeySetCount > 0)
                for (int i = 1; i <= SpecKeySetCount; i++)
                {
                    Logging.Log("Refreshing Specific Hotkey Set #" + i);
                    //var cbb = (pan_KeySets.Controls["set_"+i].Controls["cbb_typ"+i] as ComboBox);
                    //cbb.Items.Clear();
                    //cbb.Items.Add(Program.Lang[Languages.Element.SwitchBetween]);
                    //cbb.Items.AddRange(Program.lcnmid.ToArray());
                    //cbb.SelectedIndex = cbb.Items.IndexOf(SpecKeySetsValues["cbb_typ"+i]);
                }
            if (TrSetCount > 0)
                for (int i = 1; i <= TrSetCount; i++)
                {
                    Logging.Log("Refreshing Tr Set #" + i);
                    var cbb = (pan_TrSets.Controls["set_" + i].Controls["cbb_fr" + i] as ComboBox);
                    cbb.Items.Clear();
                    cbb.Items.AddRange(TranslatePanel.GTLangs);
                    cbb.SelectedIndex = Array.IndexOf(TranslatePanel.GTLangsSh, TrSetsValues["cbb_fr" + i]);
                    cbb = (pan_TrSets.Controls["set_" + i].Controls["cbb_to" + i] as ComboBox);
                    cbb.Items.Clear();
                    cbb.Items.AddRange(TranslatePanel.GTLangs);
                    cbb.SelectedIndex = Array.IndexOf(TranslatePanel.GTLangsSh, TrSetsValues["cbb_to" + i]);
                }
            //cbb_Layout1.Items.Clear();
            //cbb_Layout2.Items.Clear();
            //cbb_Layout3.Items.Clear();
            //cbb_Layout4.Items.Clear();
            //cbb_MainLayout1.Items.Clear();
            //cbb_MainLayout2.Items.Clear();
            //cbb_Layout1.Items.Add(Program.Lang[Languages.Element.SwitchBetween]);
            //cbb_Layout2.Items.Add(Program.Lang[Languages.Element.SwitchBetween]);
            //cbb_Layout3.Items.Add(Program.Lang[Languages.Element.SwitchBetween]);
            //cbb_Layout4.Items.Add(Program.Lang[Languages.Element.SwitchBetween]);
            //cbb_Layout1.Items.AddRange(Program.lcnmid.ToArray());
            //cbb_Layout2.Items.AddRange(Program.lcnmid.ToArray());
            //cbb_Layout3.Items.AddRange(Program.lcnmid.ToArray());
            //cbb_Layout4.Items.AddRange(Program.lcnmid.ToArray());
            //cbb_MainLayout1.Items.AddRange(Program.lcnmid.ToArray());
            //cbb_MainLayout2.Items.AddRange(Program.lcnmid.ToArray());
            //cbb_SpecKeysType.SelectedIndex = Program.MyConfs.ReadInt("Layouts", "SpecificKeysType");
            //			try {
            //				cbb_Language.SelectedIndex = cbb_Language.Items.IndexOf(Program._language);
            //				EmulateLSType = Program.MyConfs.Read("Layouts", "EmulateLayoutSwitchType");
            //				cbb_Layout1.SelectedIndex = cbb_Layout1.Items.IndexOf(Layout1);
            //				cbb_Layout2.SelectedIndex = cbb_Layout2.Items.IndexOf(Layout2);
            //				cbb_Layout3.SelectedIndex = cbb_Layout3.Items.IndexOf(Layout3);
            //				cbb_Layout4.SelectedIndex = cbb_Layout4.Items.IndexOf(Layout4);
            //				cbb_Key1.SelectedIndex = Key1;
            //				cbb_Key2.SelectedIndex = Key2;
            //				cbb_Key3.SelectedIndex = Key3;
            //				cbb_Key4.SelectedIndex = Key4;
            //				cbb_EmulateType.SelectedIndex = cbb_EmulateType.Items.IndexOf(EmulateLSType);
            //				cbb_MainLayout1.SelectedIndex = Program.lcnmid.IndexOf(MainLayout1);
            //				cbb_MainLayout2.SelectedIndex = Program.lcnmid.IndexOf(MainLayout2);
            //			} catch (Exception e){
            ////				MessageBox.Show(Program.Msgs[9], Program.Msgs[5], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //				RefreshComboboxes();
            //				cbb_MainLayout1.SelectedIndex = 0;
            //				cbb_MainLayout2.SelectedIndex = 1;
            //				Logging.Log("Locales indexes select failed, error message:\n" + e.Message +"\n"+e.StackTrace+"\n", 1);
            //			}
            Logging.Log("Locales for ALL comboboxes refreshed.");
        }
        /// <summary>
        /// Toggles some controls enabled state based on some checkboxes checked state. 
        /// </summary>
        void ToggleDependentControlsEnabledState()
        {
            // Functions tab
            chk_CSLayoutSwitchingPlus.Enabled = chk_CSLayoutSwitching.Checked;
            //			chk_OneLayoutWholeWord.Enabled = !chk_CSLayoutSwitching.Checked;
            if (chk_CSLayoutSwitching.Checked && chk_OneLayoutWholeWord.Checked)
                chk_CSLayoutSwitchingPlus.Enabled = false;
            if (!chk_OneLayoutWholeWord.Checked)
                chk_CSLayoutSwitching.ForeColor = chk_CSLayoutSwitchingPlus.ForeColor = Color.Red;
            else
                chk_CSLayoutSwitching.ForeColor = chk_CSLayoutSwitchingPlus.ForeColor = chk_OneLayoutWholeWord.ForeColor;
            lbl_TrayDislpayType.Enabled = cbb_TrayDislpayType.Enabled = chk_TrayIcon.Checked;
            chk_SilentUpdate.Enabled = chk_StartupUpdatesCheck.Checked;
            lnk_OpenHistory.Enabled = lbl_BackSpaceType.Enabled = cbb_BackSpaceType.Enabled = chk_WriteInputHistory.Checked;
            lnk_OpenLogs.Enabled = chk_Logging.Checked;
            // Sync tab
            chk_andPROXY.Enabled = chk_Mini.Checked;
            chk_andPROXY2.Enabled = chk_rMini.Checked;
            // Layouts tab
            //lbl_SetsCount.Enabled = pan_KeySets.Enabled = btn_AddSet.Enabled = btn_SubSet.Enabled = 
            //	lbl_KeysType.Enabled = cbb_SpecKeysType.Enabled = chk_SpecificLS.Checked;
            //cbb_MainLayout1.Enabled = cbb_MainLayout2.Enabled = 
            //	lbl_LayoutNum1.Enabled = lbl_LayoutNum2.Enabled = chk_SwitchBetweenLayouts.Checked;
            //lbl_EmuType.Enabled = cbb_EmulateType.Enabled = chk_EmulateLS.Checked;
            //grb_Keys.Enabled = grb_Layouts.Enabled = chk_SpecificLS.Checked;
            //			if (chk_EmulateLS.Checked) {
            //				chk_SwitchBetweenLayouts.Enabled = chk_SwitchBetweenLayouts.Checked = false;
            //			} else { chk_SwitchBetweenLayouts.Enabled = true; }
            // Appearence tab
            chk_LangTTCaretOnChange.Enabled = chk_LangTooltipCaret.Checked;
            lbl_LangTTBackgroundColor.Enabled = btn_LangTTBackgroundColor.Enabled =
                !chk_LangTTTransparentColor.Checked;
            lbl_LangTTBackgroundColor.Enabled = btn_LangTTBackgroundColor.Enabled =
                chk_LangTTTransparentColor.Enabled = lbl_LangTTForegroundColor.Enabled = btn_LangTTForegroundColor.Enabled =
                btn_LangTTFont.Enabled = !chk_LangTTUseFlags.Checked;
            if (!chk_LangTooltipMouse.Checked)
                chk_MouseTTAlways.Enabled = chk_LangTTMouseOnChange.Enabled = false;
            else
            {
                chk_MouseTTAlways.Enabled = !chk_LangTTMouseOnChange.Checked;
                chk_LangTTMouseOnChange.Enabled = !chk_MouseTTAlways.Checked;
            }
            // Snippets tab
            lbl_NCR.Enabled = lbl_NCRCount.Enabled = pan_NoConvertRules.Enabled = btn_NCRAdd.Enabled = btn_NCR_Sub.Enabled = lbl_SnippetsCount.Enabled = lbl_SnippetExpandKey.Enabled = cbb_SnippetExpandKeys.Enabled = txt_Snippets.Enabled = chk_SnippetsSwitchToGuessLayout.Enabled = chk_SnippetsSpaceAfter.Enabled = lnk_SnipOpen.Enabled = chk_Snippets.Checked;
            // Auto Switch tab
            lbl_AutoSwitchWordsCount.Enabled = btn_UpdateAutoSwitchDictionary.Enabled = txt_AutoSwitchDictionary.Enabled = chk_AutoSwitchSwitchToGuessLayout.Enabled = chk_AutoSwitchSpaceAfter.Enabled = chk_DownloadASD_InZip.Enabled = chk_AutoSwitch.Checked;
            // Persistent Layout tab
            chk_ChangeLayoutOnlyOnce.Enabled = chk_OnlyOnWindowChange.Checked;
            txt_PersistentLayout1Processes.Enabled = chk_PersistentLayout1Active.Checked;
            txt_PersistentLayout2Processes.Enabled = chk_PersistentLayout2Active.Checked;
            if (chk_OnlyOnWindowChange.Checked || !chk_PersistentLayout1Active.Checked)
                lbl_PersistentLayout1Interval.Enabled = nud_PersistentLayout1Interval.Enabled = false;
            else
                lbl_PersistentLayout1Interval.Enabled = nud_PersistentLayout1Interval.Enabled = true;
            if (chk_OnlyOnWindowChange.Checked || !chk_PersistentLayout2Active.Checked)
                lbl_PersistentLayout2Interval.Enabled = nud_PersistentLayout2Interval.Enabled = false;
            else
                lbl_PersistentLayout2Interval.Enabled = nud_PersistentLayout2Interval.Enabled = true;
            // Language Panel tab
            grb_LPConfig.Enabled = chk_DisplayLangPanel.Checked;
            btn_LPBorderColor.Enabled = !chk_LPAeroColor.Checked;
            // Hotkeys tab
            chk_DoubleHotkey.Enabled = chk_WinInHotKey.Enabled = txt_Hotkey.Enabled = chk_HotKeyEnabled.Checked;
            chk_DoubleHotkey.Enabled = lsb_Hotkeys.SelectedIndex != 13;
            // Timings tab
            nud_DelayAfterBackspaces.Enabled = chk_UseDelayAfterBackspaces.Checked;
            nud_SelectedTextGetTriesCount.Enabled = chk_SelectedTextGetMoreTries.Checked;
            lbl_ScrollLockRefreshRate.Enabled = nud_ScrollLockRefreshRate.Enabled = chk_HighlightScroll.Checked;
            lbl_CapsLockRefreshRate.Enabled = nud_CapsLockRefreshRate.Enabled = chk_CapsLockDTimer.Checked;
            lbl_FlagTrayRefreshRate.Enabled = nud_TrayFlagRefreshRate.Enabled = cbb_TrayDislpayType.SelectedIndex == 1;
            lbl_LangTTCaretRefreshRate.Enabled = nud_LangTTCaretRefreshRate.Enabled = chk_LangTooltipCaret.Checked;
            lbl_LangTTMouseRefreshRate.Enabled = nud_LangTTMouseRefreshRate.Enabled = LDUseWindowsMessages || chk_LangTooltipMouse.Checked;
            lbl_LangTTCaretRefreshRate.Enabled = !chk_LDMessages.Checked;
            // Sounds tab
            lbl_CustomSound.Enabled = btn_SelectSnd.Enabled = chk_UseCustomSnd.Checked;
            lbl_CustomSound2.Enabled = btn_SelectSnd2.Enabled = chk_UseCustomSnd2.Checked;
            grb_Sound1.Enabled = grb_Sound2.Enabled = chk_EnableSnd.Checked;
            // Translation tab
            btn_TrBorderC.Enabled = !chk_TrUseAccent.Checked;
            grb_TrConfs.Enabled = chk_TrEnable.Checked;
        }
        /// <summary>
        /// Toggles visibility of main window.
        /// </summary>
        public void ToggleVisibility()
        {
            Logging.Log("FaineSwitcher Main window visibility changed to [" + !Visible + "].");
            if (Visible)
            {
                Visible = false;
            }
            else
            {
                TopMost = Visible = true;
                TopMost = false;
                WinAPI.SetForegroundWindow(Handle);
            }
            if (Program.switcher != null)
                KMHook.ClearModifiers();
            icon.CheckShHi(Visible);
            Memory.Flush();
        }
        public void ToggleLangPanel()
        {
            if (_langPanel.Visible)
            {
                chk_DisplayLangPanel.Checked = LangPanelDisplay = _langPanel.Visible = false;
                Program.MyConfs.WriteSave("LangPanel", "Display", "false");
                langPanelRefresh.Stop();
            }
            else
            {
                chk_DisplayLangPanel.Checked = LangPanelDisplay = _langPanel.Visible = true;
                Program.MyConfs.WriteSave("LangPanel", "Display", "true");
                langPanelRefresh.Start();
            }
        }
        /// <summary>
        /// Restarts FaineSwitcher.
        /// </summary>
        public void Restart()
        {
            int SwitcherPID = Process.GetCurrentProcess().Id;
            PreExit();
            var restartSwitcherPath = Path.Combine(new string[] {
                nPath,
                "RestartSwitcher.cmd"
            });
            //Batch script to restart FaineSwitcher.
            var restartSwitcher =
                @"@ECHO OFF
REM You should never see this file, if you are it means during restarting FaineSwitcher something went wrong. 
chcp 65001
SET SWITCHERDIR=" + AppDomain.CurrentDomain.BaseDirectory + @"
TASKKILL /PID " + SwitcherPID + @" /F
TASKKILL /IM FaineSwitcher.exe /F
START """" ""%SWITCHERDIR%FaineSwitcher.exe""
DEL " + restartSwitcherPath;
            Logging.Log("Writing restart script.");
            File.WriteAllText(restartSwitcherPath, restartSwitcher);
            var piRestartSwitcher = new ProcessStartInfo() { FileName = restartSwitcherPath, WindowStyle = ProcessWindowStyle.Hidden };
            Logging.Log("Starting restart script.");
            Process.Start(piRestartSwitcher);
        }
        /// <summary>
        /// Refreshes all icon's images and tray icon visibility.
        /// </summary>
        public void RefreshAllIcons(bool force = false)
        {
            var fong = false;
            try
            {
                var p = Process.GetProcessesByName("explorer");
                if (explorer_pid != p[0].Id)
                {
                    fong = true;
                }
                //				Debug.WriteLine(p[0].Id + " " + force);
                explorer_pid = p[0].Id;
            }
            catch (Exception e)
            {
                fong = true;
            }
            if (fong)
            {
                force = true;
                icon.trIcon.Visible = false;
                System.Threading.Thread.Sleep(1000);
                icon.trIcon.Visible = true;
            }
            if (TrayFlags || TrayText)
            {
                ChangeTrayIconToFlag(force);
            }
            else
            {
                if (HKSymIgn_tempEnabled && SymIgnEnabled && icon.trIcon.Icon != Properties.Resources.FaineSwitcher)
                    icon.trIcon.Icon = Properties.Resources.FaineSwitcher;
                else if (!TrayFlags && !TrayText && icon.trIcon.Icon != Properties.Resources.FaineSwitcher)
                    icon.trIcon.Icon = Properties.Resources.FaineSwitcher;
            }
            if (!blueIcon && HKSymIgn_tempEnabled && SymIgnEnabled)
            {
                blueIcon = true;
                Icon = Properties.Resources.FaineSwitcher;
            }
            else if (blueIcon && HKSymIgn_tempEnabled && !SymIgnEnabled)
            {
                Icon = Properties.Resources.FaineSwitcher;
                blueIcon = false;
            }
            if (TrayIconVisible && !icon.trIcon.Visible)
            {
                icon.Show();
            }
            else if (!TrayIconVisible && icon.trIcon.Visible)
            {
                icon.Hide();
            }
        }
        public static void IfDispose(ref Bitmap disposable)
        {
            if (disposable != null) disposable.Dispose();
            disposable = null;
        }
        public static void RefreshFLAG(bool force = false)
        {
            Debug.WriteLine("aLIVe");
            // No need for update when no display wrapper
            if (!TrayIconVisible && !LDCaretUseFlags_temp && !LDMouseUseFlags_temp && !LangPanelDisplay && !force) return;
            IfDispose(ref FLAG);
            if (!ENABLED)
            {
                Debug.WriteLine("NOT ENABLED");
                FLAG = new Bitmap(Properties.Resources.FaineSwitcher.ToBitmap());
                return;
            }
            if (force)
            {
                IfDispose(ref FLAG);
                IfDispose(ref ITEXT);
            }
            Debug.WriteLine("STIlL");
            uint lcraw = 0;
            if (!UseJKL || KMHook.JKLERR)
                lcraw = Locales.GetCurrentLocale();
            else
                lcraw = SwitcherUI.currentLayout;
            var ol = false;
            int lcid = (int)(lcraw >> 16);
            if (Program.switcher != null)
                ol = SwitcherUI.OneLayout;
            else
                ol = Program.MyConfs.ReadBool("Layouts", "OneLayout");
            if (ol)
                lcid = (int)(SwitcherUI.GlobalLayout >> 16);
            if (lcid > 0)
            {
                var flagname = "jp";
                CultureInfo clangname;
                try
                {
                    clangname = new CultureInfo(lcid);
                }
                catch
                {
                    clangname = new CultureInfo((int)(lcraw & 0xffff));
                }
                flagname = clangname.ThreeLetterISOLanguageName.Substring(0, 2).ToLower();
                var flagpth = Path.Combine(SwitcherUI.nPath, "Flags\\" + flagname + ".png");
                Debug.WriteLine("UpDATe?" + flagname + ", " + (flagname != latestSwitch || (TrayText && ITEXT == null) || (TrayFlags && FLAG == null)));
                if (flagname != latestSwitch || (TrayText && ITEXT == null) || (TrayFlags && FLAG == null) || force)
                {
                    Logging.Log("Changed flag to " + flagname + " lcid " + lcid);
                    Debug.WriteLine("Changed flag to " + flagname + " lcid " + lcid);
                    if (File.Exists(flagpth))
                    {
                        FLAG = new Bitmap(Image.FromFile(flagpth));
                    }
                    else
                        switch (flagname)
                        {
                            case "ru":
                                FLAG = new Bitmap(Properties.Resources.ru);
                                break;
                            case "en":
                                FLAG = new Bitmap(Properties.Resources.en);
                                break;
                            case "es":
                                FLAG = new Bitmap(Properties.Resources.es);
                                break;
                            case "jp":
                                FLAG = new Bitmap(Properties.Resources.jp);
                                break;
                            case "bu":
                                FLAG = new Bitmap(Properties.Resources.bu);
                                break;
                            case "uk":
                                FLAG = new Bitmap(Properties.Resources.uk);
                                break;
                            case "po":
                                FLAG = new Bitmap(Properties.Resources.po);
                                break;
                            case "sw":
                                FLAG = new Bitmap(Properties.Resources.sw);
                                break;
                            case "zh":
                                FLAG = new Bitmap(Properties.Resources.zh);
                                break;
                            case "be":
                                FLAG = new Bitmap(Properties.Resources.be);
                                break;
                            case "de":
                                FLAG = new Bitmap(Properties.Resources.de);
                                break;
                            case "sp":
                                FLAG = new Bitmap(Properties.Resources.sp);
                                break;
                            case "it":
                                FLAG = new Bitmap(Properties.Resources.it);
                                break;
                            case "fr":
                                FLAG = new Bitmap(Properties.Resources.fr);
                                break;
                            case "la":
                                FLAG = new Bitmap(Properties.Resources.la);
                                break;
                            case "hy":
                                FLAG = new Bitmap(Properties.Resources.hy);
                                break;
                            case "ka":
                                FLAG = new Bitmap(Properties.Resources.ka);
                                break;
                            case "el":
                                FLAG = new Bitmap(Properties.Resources.el);
                                break;
                            default:
                                FLAG = new Bitmap(Properties.Resources.FaineSwitcher.ToBitmap());
                                Logging.Log("Missing flag for language [" + flagname + " / " + lcid + "].", 2);
                                break;
                        }
                    if (TrayText)
                    {
                        Debug.WriteLine("Drawing the text layout *icon* in tray.");
                        var n2 = true;
                        var t = char.ToUpper(flagname[0]) + flagname.Substring(1);
                        var bg = LDCaretBack_temp;
                        var fg = LDCaretFore_temp;
                        var fn = LDCaretFont_temp;
                        if (lcid == (MAIN_LAYOUT2 >> 16))
                        {
                            bg = Layout2Back_temp;
                            fg = Layout2Fore_temp;
                            fn = Layout2Font_temp;
                            if (!String.IsNullOrEmpty(Layout2TText)) t = Layout2TText;
                            n2 = false;
                        }
                        else if (lcid == (MAIN_LAYOUT1 >> 16))
                        {
                            bg = Layout1Back_temp;
                            fg = Layout1Fore_temp;
                            fn = Layout1Font_temp;
                            if (!String.IsNullOrEmpty(Layout1TText)) t = Layout1TText;
                            n2 = false;
                        }
                        Debug.WriteLine("D" + n2);
                        var b = new Bitmap(16, 16);
                        var g = Graphics.FromImage(b);
                        var sf = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
                        var sb = new SolidBrush(fg);
                        g.Clear(bg);
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                        g.DrawString(t, fn, sb, new PointF(8, 8), sf);
                        g.Dispose();
                        sf.Dispose();
                        sb.Dispose();
                        IfDispose(ref ITEXT);
                        ITEXT = new Bitmap(b);
                        if (n2 && LDCaretUseFlags_temp)
                        {
                            IfDispose(ref ITEXT);
                            ITEXT = new Bitmap(FLAG);
                        }
                        b.Dispose();
                    }
                    latestSwitch = flagname;
                }
            }
            else
                Logging.Log("Layout id was [" + lcid + "].", 2);
        }
        /// <summary>
        /// Changes tray icon image to country flag based on current layout.
        /// </summary>
        void ChangeTrayIconToFlag(bool force = false)
        {
            try
            {
                uint lcid = 0;
                if (OneLayout)
                    lcid = GlobalLayout;
                else if (!UseJKL || KMHook.JKLERR)
                    lcid = Locales.GetCurrentLocale();
                else
                    lcid = SwitcherUI.currentLayout;
                //			Debug.WriteLine("refresh?"+ (lastTrayFlagLayout != lcid || force));
                if (lastTrayFlagLayout != lcid || force)
                {
                    RefreshFLAG(force);
                    Bitmap b = null;
                    if (FLAG != null) b = new Bitmap(FLAG);
                    if (TrayText && ITEXT != null) b = new Bitmap(ITEXT);
                    Icon flagicon;
                    if (b != null)
                        flagicon = Icon.FromHandle(b.GetHicon());
                    else
                        flagicon = FaineSwitcher.Properties.Resources.FaineSwitcher;
                    icon.trIcon.Icon = flagicon;
                    WinAPI.DestroyIcon(flagicon.Handle);
                    lastTrayFlagLayout = lcid;
                }
            }
            catch (Exception e)
            {
                Logging.Log("[TrICON] > Can't change tray icon, error: " + e.Message + "\r\n" + e.StackTrace, 1);
            }
        }
        /// <summary>
        /// Initializes UI language.
        /// </summary>
        public static void InitLanguage()
        {
            Program._language = Program.MyConfs.Read("Appearence", "Language");
            if (Program._language == "English")
                Program.Lang = Languages.English;
            else if (Program._language == "Українська")
                Program.Lang = Languages.Ukrainian;
        }
        /// <summary>
        /// Initializes language tooltips.
        /// </summary>
        public void InitLangDisplays(bool destroyonly = false, bool lc = false)
        {
            if (mouseLangDisplay == null)
                mouseLangDisplay = new LangDisplay();
            if (caretLangDisplay == null)
                caretLangDisplay = new LangDisplay();
            if (destroyonly || !ENABLED)
            {
                mouseLangDisplay.Dispose();
                caretLangDisplay.Dispose();
                return;
            }
            if (LDForMouse)
            {
                mouseLangDisplay.mouseDisplay = true;
                mouseLangDisplay.DisplayFlag = LDMouseUseFlags_temp;
                mouseLangDisplay.Visible = true;
            }
            else if (lc)
            {
                mouseLangDisplay.Visible = false;
            }
            if (LDForCaret)
            {
                caretLangDisplay.caretDisplay = true;
                caretLangDisplay.DisplayFlag = LDCaretUseFlags_temp;
                if (mouseLangDisplay != null)
                {
                    caretLangDisplay.AddOwnedForm(mouseLangDisplay); //Prevents flickering when tooltips are one on another
                }
                caretLangDisplay.Visible = true;
            }
            else if (lc)
            {
                caretLangDisplay.Visible = false;
            }
        }
        void lastAltTabChangeLayout()
        {
            KInputs.MakeInput(new[] { KInputs.AddKey(Keys.LMenu, true), KInputs.AddKey(Keys.Tab, true) });
            System.Threading.Thread.Sleep(1);
            KInputs.MakeInput(new[] { KInputs.AddKey(Keys.LMenu, false), KInputs.AddKey(Keys.Tab, false) });
            var t = new Timer();
            t.Tick += (x, xx) =>
            {
                KMHook.ChangeLayout(true);
                t.Stop();
                t.Dispose();
            };
            t.Interval = 300;
            t.Start();
        }
        static Timer thmm;
        static bool thmmr, thmme, start_skip = true;
        /// <summary>
        /// Initializes tray icon.
        /// </summary>
        void InitializeTrayIcon()
        {
            if (icon != null)
            {
                icon.Hide();
                icon.trIcon.Dispose();
            }
            icon = new TrayIcon(Program.MyConfs.ReadBool("Functions", "TrayIconVisible"));
            icon.Exit += (_, __) => ExitProgram();
            if (Hchk_LMBTrayLayoutChange.Checked)
            {
                if (Hchk_LMBTrayLayoutChangeDC.Checked)
                {
                    var cc = 0; bool tx = false; Timer t = null;
                    icon.MLBAct += (_, __) =>
                    {
                        cc++;
                        Debug.WriteLine("CC" + cc);
                        if (cc > 1)
                        {
                            ToggleVisibility();
                            cc = 0;
                            if (t != null)
                            {
                                t.Stop();
                                t.Dispose();
                                tx = false;
                            }
                        }
                        else if (!tx)
                        {
                            tx = true;
                            t = new Timer(); bool fign = false;
                            t.Tick += (x, xx) =>
                            {
                                if (!fign) { fign = true; return; }
                                if (cc == 1) { lastAltTabChangeLayout(); }
                                cc = 0;
                                t.Stop(); t.Dispose(); tx = false;
                            };
                            t.Interval = SystemInformation.DoubleClickTime;
                            t.Start();
                        }
                    };
                }
                else
                    icon.MLBAct += (_, __) => lastAltTabChangeLayout();
            }
            else
                icon.MLBAct += (_, __) => ToggleVisibility();
            icon.ShowHide += (_, __) => ToggleVisibility();
            icon.EnaDisable += (_, __) => ToggleSwitcher();
            icon.Restart += (_, __) => Restart();
            icon.ChangeLt += (_, __) => lastAltTabChangeLayout();
            icon.ConvertClip += (_, __) =>
            {
                var t = KMHook.ConvertText(KMHook.GetClipboard(2));
                KMHook.RestoreClipBoard(t);
            };
            icon.TransliClip += (_, __) =>
            {
                var t = KMHook.TransliterateText(KMHook.GetClipboard(2));
                KMHook.RestoreClipBoard(t);
            };
            var mm = Path.Combine(nPath, "FaineSwitcher.mm");
            if (File.Exists(mm))
            {
                SwitcherMM = true;
                makemenu(File.ReadAllText(mm));
            }
            else SwitcherMM = false;
            if (SwitcherMM && TrayHoverSwitcherMM > 0)
            {
                var now_p = new Point(7777, 7777);
                if (thmm != null)
                {
                    thmm.Stop();
                    thmm.Dispose();
                }
                thmm = new Timer();
                thmm.Interval = TrayHoverSwitcherMM;
                thmm.Tick += (_, __) =>
                {
                    thmm.Stop(); // doesn't work..
                    thmmr = false;  // either
                    if (now_p.Equals(Cursor.Position) && !icon.trIcon.ContextMenuStrip.Visible && !now_p.Equals(new Point(7777, 7777)))
                    {
                        Debug.WriteLine("You haven't moved from: " + now_p.X + "/" + now_p.Y + " for " + TrayHoverSwitcherMM + "ms.");
                        ShowSwitcherMMMenuUnderMouse();
                        thmme = true; // prevents timer tick to go on and on
                        var t = new Timer() { Interval = 60 }; // reset after 60ms
                        t.Tick += (z, zz) => { thmme = false; };
                        t.Start();
                    }
                    now_p = new Point(7777, 7777);
                };
                icon.trIcon.MouseMove += (_, __) =>
                {
                    if (thmme) { return; }
                    if (start_skip) { start_skip = false; return; }
                    Debug.WriteLine("MOMO:" + Cursor.Position.X);
                    now_p = Cursor.Position;
                    if (thmmr)
                    {
                        thmmr = false;
                        thmm.Stop();
                    }
                    if (!thmmr)
                    {
                        thmm.Start();
                        thmmr = true;
                    }
                };
                icon.trIcon.MouseClick += (_, __) =>
                {
                    Debug.WriteLine("CLI");
                    thmm.Stop(); thmmr = false;
                };
            }
        }
        /// <summary>
        /// Initializes list boxes.
        /// </summary>
        void InitializeListBoxes()
        {
            lsb_Hotkeys.SelectedIndex = 0;
            lsb_LangTTAppearenceForList.SelectedIndex = 0;
        }
        void InitializeLangPanel()
        {
            if (_langPanel == null)
                _langPanel = new LangPanel();
            int x = -7, y = -7;
            try
            {
                var getXY = new Regex(@"(X|Y)(\d+)");
                var xy = Program.MyConfs.Read("LangPanel", "Position");
                var _xy = getXY.Matches(xy);
                Logging.Log("XY: " + _xy[0].Groups[2].Value + " / " + _xy[1].Groups[2].Value);
                x = Convert.ToInt32(_xy[0].Groups[2].Value);
                y = Convert.ToInt32(_xy[1].Groups[2].Value);
            }
            catch (Exception e) { Logging.Log("Erro during latest x/y position get, details:\r\n" + e.Message + "\r\n" + e.StackTrace, 1); }
            _langPanel.Location = new Point(x, y);
            _langPanel.UpdateApperence(LangPanelBackColor, LangPanelForeColor, LangPanelTransparency, LangPanelFont);
            if (LangPanelDisplay)
            {
                _langPanel.ShowInactiveTopmost();
                langPanelRefresh.Start();
            }
        }
        /// <summary>
        /// Initializes all hotkeys.
        /// </summary>
        public void InitializeHotkeys()
        {
            Mainhk = new Hotkey(Mainhk_tempEnabled, (uint)Mainhk_tempKey,
                                Hotkey.GetMods(Mainhk_tempMods), (int)Hotkey.HKID.ToggleVisibility, Mainhk_tempDouble);
            HKCLast = new Hotkey(HKCLast_tempEnabled, (uint)HKCLast_tempKey,
                Hotkey.GetMods(HKCLast_tempMods), (int)Hotkey.HKID.ConvertLastWord, HKCLast_tempDouble);
            HKCSelection = new Hotkey(HKCSelection_tempEnabled, (uint)HKCSelection_tempKey,
                Hotkey.GetMods(HKCSelection_tempMods), (int)Hotkey.HKID.ConvertSelection, HKCSelection_tempDouble);
            HKCLine = new Hotkey(HKCLine_tempEnabled, (uint)HKCLine_tempKey,
                Hotkey.GetMods(HKCLine_tempMods), (int)Hotkey.HKID.ConvertLastLine, HKCLine_tempDouble);
            HKSymIgn = new Hotkey(HKSymIgn_tempEnabled, (uint)HKSymIgn_tempKey,
                Hotkey.GetMods(HKSymIgn_tempMods), (int)Hotkey.HKID.ToggleSymbolIgnoreMode, HKSymIgn_tempDouble);
            HKConMorWor = new Hotkey(HKConMorWor_tempEnabled, (uint)HKConMorWor_tempKey,
                Hotkey.GetMods(HKConMorWor_tempMods), (int)Hotkey.HKID.ConvertMultipleWords, HKConMorWor_tempDouble);
            HKTitleCase = new Hotkey(HKTitleCase_tempEnabled, (uint)HKTitleCase_tempKey,
                Hotkey.GetMods(HKTitleCase_tempMods), (int)Hotkey.HKID.ToTitleSelection, HKTitleCase_tempDouble);
            HKRandomCase = new Hotkey(HKRandomCase_tempEnabled, (uint)HKRandomCase_tempKey,
                Hotkey.GetMods(HKRandomCase_tempMods), (int)Hotkey.HKID.ToRandomSelection, HKRandomCase_tempDouble);
            HKSwapCase = new Hotkey(HKSwapCase_tempEnabled, (uint)HKSwapCase_tempKey,
                Hotkey.GetMods(HKSwapCase_tempMods), (int)Hotkey.HKID.ToSwapSelection, HKSwapCase_tempDouble);
            HKUpperCase = new Hotkey(HKToUpper_tempEnabled, (uint)HKToUpper_tempKey,
                Hotkey.GetMods(HKToUpper_tempMods), (int)Hotkey.HKID.ToUpperSelection, HKToUpper_tempDouble);
            HKLowerCase = new Hotkey(HKToLower_tempEnabled, (uint)HKToLower_tempKey,
                Hotkey.GetMods(HKToLower_tempMods), (int)Hotkey.HKID.ToLowerSelection, HKToLower_tempDouble);
            HKTransliteration = new Hotkey(HKTransliteration_tempEnabled, (uint)HKTransliteration_tempKey,
                Hotkey.GetMods(HKTransliteration_tempMods), (int)Hotkey.HKID.TransliterateSelection, HKTransliteration_tempDouble);
            ExitHk = new Hotkey(ExitHk_tempEnabled, (uint)ExitHk_tempKey,
                Hotkey.GetMods(ExitHk_tempMods), (int)Hotkey.HKID.Exit, ExitHk_tempDouble);
            HKRestart = new Hotkey(HKRestart_tempEnabled, (uint)HKRestart_tempKey,
                Hotkey.GetMods(HKRestart_tempMods), (int)Hotkey.HKID.Restart, false);
            HKToggleLP = new Hotkey(HKToggleLangPanel_tempEnabled, (uint)HKToggleLangPanel_tempKey,
                Hotkey.GetMods(HKToggleLangPanel_tempMods), (int)Hotkey.HKID.ToggleLangPanel, HKToggleLangPanel_tempDouble);
            HKShowST = new Hotkey(HKShowSelectionTranslate_tempEnabled, (uint)HKShowSelectionTranslate_tempKey,
                Hotkey.GetMods(HKShowSelectionTranslate_tempMods), (int)Hotkey.HKID.ShowSelectionTranslation, HKShowSelectionTranslate_tempDouble);
            HKToggleSwitcher = new Hotkey(HKToggleSwitcher_tempEnabled, (uint)HKToggleSwitcher_tempKey,
                Hotkey.GetMods(HKToggleSwitcher_tempMods), (int)Hotkey.HKID.ToggleSwitcher, HKToggleSwitcher_tempDouble);
            HKCycleCase = new Hotkey(HKCycleCase_tempEnabled, (uint)HKCycleCase_tempKey,
                Hotkey.GetMods(HKCycleCase_tempMods), (int)Hotkey.HKID.CycleCase, HKCycleCase_tempDouble);
            HKSelCustConv = new Hotkey(HKSelCustConv_tempEnabled, (uint)HKSelCustConv_tempKey,
                Hotkey.GetMods(HKSelCustConv_tempMods), (int)Hotkey.HKID.CustomConversion, HKSelCustConv_tempDouble);
            HKShCMenuUM = new Hotkey(HKShCMenuUM_tempEnabled, (uint)HKShCMenuUM_tempKey,
                Hotkey.GetMods(HKShCMenuUM_tempMods), (int)Hotkey.HKID.ShowCMenuUnderMouse, HKShCMenuUM_tempDouble);
            Logging.Log("Hotkeys initialized.");
        }
        public bool HasHotkey(Hotkey thishk)
        {
            if (thishk == Mainhk ||
                thishk == HKCLast ||
                thishk == HKCSelection ||
                thishk == HKCLine ||
                thishk == HKSymIgn ||
                thishk == HKConMorWor ||
                thishk == HKTitleCase ||
                thishk == HKRandomCase ||
                thishk == HKSwapCase ||
                thishk == HKUpperCase ||
                thishk == HKLowerCase ||
                thishk == HKTransliteration ||
                thishk == ExitHk ||
                thishk == HKRestart ||
                thishk == HKToggleLP ||
                thishk == HKShowST)
                return true;
            foreach (Hotkey hk in SpecificSwitchHotkeys)
            {
                if (thishk == hk)
                    return true;

            }
            return false;
        }
        void WrongColorLog(string color, string err = "")
        {
            Logging.Log("[" + color + "]is not color, it is skipped." + (!string.IsNullOrEmpty(err) ? ("\r\nError: " + err) : ""), 2);
        }
        void WrongFontLog(string font, string err = "")
        {
            Logging.Log("[" + font + "]is not font, or its missing from system, it is skipped." + (!string.IsNullOrEmpty(err) ? ("\r\nError: " + err) : ""), 2);
        }
        /// <summary>
        /// Initializes timers.
        /// </summary>
        void InitializeTimers()
        {
            #region Reset Timers
            crtCheck.Stop();
            ICheck.Stop();
            ScrlCheck.Stop();
            res.Stop();
            resC.Stop();
            old.Stop();
            capsCheck.Stop();
            flagsCheck.Stop();
            persistentLayout1Check.Stop();
            persistentLayout2Check.Stop();
            langPanelRefresh.Stop();
            ICheck = new Timer();
            crtCheck = new Timer();
            ScrlCheck = new Timer();
            res = new Timer();
            resC = new Timer();
            capsCheck = new Timer();
            flagsCheck = new Timer();
            persistentLayout1Check = new Timer();
            persistentLayout2Check = new Timer();
            langPanelRefresh = new Timer();
            old = new Timer();
            KMHook.doublekey = new Timer();
            #endregion
            crtCheck.Interval = Program.MyConfs.ReadInt("Timings", "LangTooltipForCaretRefreshRate");
            crtCheck.Tick += (_, __) => UpdateCaredLD();
            ICheck.Interval = Program.MyConfs.ReadInt("Timings", "LangTooltipForMouseRefreshRate");
            ICheck.Tick += (_, __) => UpdateMouseLD();
            res.Interval = (ICheck.Interval + crtCheck.Interval) * 2;
            resC.Interval = (ICheck.Interval + crtCheck.Interval) * 2;
            res.Tick += (_, __) =>
            {
                onepass = true;
                mouseLangDisplay.HideWnd();
                if (LDUseWindowsMessages)
                    UpdateMouseLD();
                res.Stop();
            };
            resC.Tick += (_, __) =>
            {
                onepassC = true;
                caretLangDisplay.HideWnd();
                if (LDUseWindowsMessages)
                    UpdateCaredLD();
                resC.Stop();
            };
            ScrlCheck.Interval = Program.MyConfs.ReadInt("Timings", "ScrollLockStateRefreshRate");
            ScrlCheck.Tick += (_, __) =>
            {
                if (ScrollTip && !KMHook.alt)
                {
                    KMHook.DoSelf(() =>
                    {
                        var l = currentLayout;
                        if (!UseJKL || KMHook.JKLERR)
                            l = Locales.GetCurrentLocale();
                        if (l == SwitcherUI.MAIN_LAYOUT1)
                        {
                            if (!Control.IsKeyLocked(Keys.Scroll))
                            { // Turn on 
                                KMHook.KeybdEvent(Keys.Scroll, 0);
                                KMHook.KeybdEvent(Keys.Scroll, 2);
                            }
                        }
                        else
                        {
                            if (Control.IsKeyLocked(Keys.Scroll))
                            {
                                KMHook.KeybdEvent(Keys.Scroll, 0);
                                KMHook.KeybdEvent(Keys.Scroll, 2);
                            }
                        }
                    }, "scroll_check_timer");
                }
            };
            capsCheck.Tick += (_, __) => KMHook.DoSelf(() =>
            {
                if (Control.IsKeyLocked(Keys.CapsLock))
                {
                    KMHook.KeybdEvent(Keys.CapsLock, 0);
                    KMHook.KeybdEvent(Keys.CapsLock, 2);
                }
            }, "caps_check_timer");
            capsCheck.Interval = Program.MyConfs.ReadInt("Timings", "CapsLockDisableRefreshRate");
            KMHook.doublekey.Tick += (_, __) =>
            {
                if (hklOK)
                    hklOK = false;
                if (hksOK)
                    hksOK = false;
                if (hklineOK)
                    hklineOK = false;
                if (hkSIOK)
                    hkSIOK = false;
                if (hkShWndOK)
                    hkShWndOK = false;
                if (hkExitOK)
                    hkExitOK = false;
                if (hkcwdsOK)
                    hkcwdsOK = false;
                if (hksTRCOK)
                    hksTRCOK = false;
                if (hksTrslOK)
                    hksTrslOK = false;
                if (hksTTCOK)
                    hksTTCOK = false;
                if (hksTSCOK)
                    hksTSCOK = false;
                if (hkUcOK)
                    hkUcOK = false;
                if (hklcOK)
                    hklcOK = false;
                if (hkccOK)
                    hkccOK = false;
                if (hkShowTSOK)
                    hkShowTSOK = false;
                if (hkToggleSwitcherOK)
                    hkToggleSwitcherOK = false;
                KMHook.doublekey.Stop();
            };
            flagsCheck.Interval = Program.MyConfs.ReadInt("Timings", "FlagsInTrayRefreshRate");
            flagsCheck.Tick += (_, __) => RefreshAllIcons();
            titlebar = RectangleToScreen(ClientRectangle).Top - Top;
            animate.Interval = 2500;
            tmr.Interval = 3000;
            old.Interval = 7500;
            old.Tick += (_, __) => { isold = !isold; };
            persistentLayout1Check.Interval = Program.MyConfs.ReadInt("PersistentLayout", "Layout1CheckInterval");
            persistentLayout2Check.Interval = Program.MyConfs.ReadInt("PersistentLayout", "Layout2CheckInterval");
            persistentLayout1Check.Tick += (_, __) => PersistentLayoutCheck(PersistentLayout1Processes, MAIN_LAYOUT1);
            persistentLayout2Check.Tick += (_, __) => PersistentLayoutCheck(PersistentLayout2Processes, MAIN_LAYOUT2);
            langPanelRefresh.Interval = LangPanelRefreshRate;
            langPanelRefresh.Tick += (_, __) =>
            {
                uint loc = 0;
                try
                {
                    if (!OneLayout)
                        loc = currentLayout == 0 ? Locales.GetCurrentLocale() : currentLayout;
                    else
                        loc = GlobalLayout;
                    if (loc > 0 && loc != lastLayoutLangPanel)
                    {
                        RefreshFLAG();
                        _langPanel.ChangeLayout(FLAG, Program.locales[Array.FindIndex(Program.locales, l => l.uId == loc)].Lang);
                        lastLayoutLangPanel = loc;
                    }
                }
                catch (Exception e) { Logging.Log("Error in LangPanel Refresh, loc: " + loc + ",  details:\r\n" + e.Message + "\r\n" + e.StackTrace); }
            };
            InitLangDisplays(false, true);
            ToggleTimers();
        }
        public void UpdateMouseLD()
        {
            if (mouseLangDisplay == null)
            {
                Logging.Log("mouseLangDisplay was null when trying to update it.", 2);
                InitLangDisplays();
                return;
            }
            if (LDForMouseOnChange)
            {
                var cLuid = Locales.GetCurrentLocale();
                if (UseJKL && !KMHook.JKLERR)
                    cLuid = currentLayout;
                if (onepass)
                {
                    latestL = cLuid;
                    onepass = false;
                }
                if (latestL != cLuid)
                {
                    latestL = cLuid;
                    mouseLangDisplay.ShowInactiveTopmost();
                    res.Start();
                }
            }
            else
            {
                if ((ICheckings.IsICursor() || MouseTTAlways) && !mouseLangDisplay.Empty)
                    mouseLangDisplay.ShowInactiveTopmost();
                else
                    mouseLangDisplay.HideWnd();
            }
            if (mouseLangDisplay.Visible)
            {
                mouseLangDisplay.Location = new Point(Cursor.Position.X + LDMouseX_Pos_temp, Cursor.Position.Y + LDMouseY_Pos_temp);
                mouseLangDisplay.RefreshLang();
            }
        }
        public void UpdateCaredLD()
        {
            if (caretLangDisplay == null)
            {
                Logging.Log("caretLangDisplay was null when trying to update it.", 2);
                InitLangDisplays();
                return;
            }
            var crtOnly = new Point(0, 0);
            var curCrtPos = CaretPos.GetCaretPointToScreen(out crtOnly);
            uint cLuid = 0;
            var notTwo = false;
            if (LDForCaretOnChange || DiffAppearenceForLayouts)
            {
                cLuid = Locales.GetCurrentLocale();
                if (UseJKL && !KMHook.JKLERR)
                    cLuid = currentLayout;
            }
            if (LDForCaretOnChange && cLuid != 0)
            {
                if (onepassC)
                {
                    //					Debug.WriteLine("OPC!" + cLuid);
                    latestCL = cLuid;
                    onepassC = false;
                }
                //				Debug.WriteLine("L"+latestCL+", CL"+cLuid);
                if (latestCL != cLuid)
                {
                    latestCL = cLuid;
                    caretLangDisplay.ShowInactiveTopmost();
                    resC.Start();
                }
            }
            else
            {
                var x = false;
                if (ExcludeCaretLD)
                    x = KMHook.ExcludedProgram();
                if (KMHook.ff_chr_wheeled || caretLangDisplay.Empty || x)
                    caretLangDisplay.HideWnd();
                else if (crtOnly.X != 77777 && crtOnly.Y != 77777) // 77777x77777 is null/none point
                    caretLangDisplay.ShowInactiveTopmost();
            }
            if (caretLangDisplay.Visible)
            {
                var LDC_np = new Point(0, 0);
                if (DiffAppearenceForLayouts && cLuid != 0)
                {
                    if (cLuid == SwitcherUI.MAIN_LAYOUT1)
                    {
                        LDC_np = new Point(curCrtPos.X + Layout1X_Pos_temp,
                                                              curCrtPos.Y + Layout1Y_Pos_temp);
                    }
                    else if (cLuid == SwitcherUI.MAIN_LAYOUT2)
                    {
                        LDC_np = new Point(curCrtPos.X + Layout2X_Pos_temp,
                                                              curCrtPos.Y + Layout2Y_Pos_temp);
                    }
                    else notTwo = true;
                }
                else notTwo = true;
                if (notTwo)
                    LDC_np = new Point(curCrtPos.X + LDCaretX_Pos_temp,
                                                          curCrtPos.Y + LDCaretY_Pos_temp);
                caretLangDisplay.RefreshLang();
                if (LDC_lp != LDC_np)
                    caretLangDisplay.Location = LDC_np;
                LDC_lp = LDC_np;
            }
        }
        public void UpdateLDs()
        {
            if (LDUseWindowsMessages)
            {
                if (LDForCaret)
                    UpdateCaredLD();
                if (LDForMouse)
                    UpdateMouseLD();
            }
        }
        public void PersistentLayoutCheck(string ProcessNames, uint Layout, string ProcName = "")
        {
            try
            {
                var actProcName = "";
                var plh = PERSISTENT_LAYOUT1_HWNDs;
                var nplh = NOT_PERSISTENT_LAYOUT1_HWNDs;
                if (Layout == MAIN_LAYOUT2)
                {
                    plh = PERSISTENT_LAYOUT2_HWNDs;
                    nplh = NOT_PERSISTENT_LAYOUT2_HWNDs;
                }
                var hwnd = WinAPI.GetForegroundWindow();
                if (nplh.Contains(hwnd))
                {
                    Logging.Log("Already known hwnd which shouldn't have persistent layout.");
                    return;
                }
                if (!plh.Contains(hwnd))
                {
                    if (!String.IsNullOrEmpty(ProcName))
                        actProcName = ProcName;
                    else
                        actProcName = Locales.ActiveWindowProcess().ProcessName;
                    actProcName = actProcName.ToLower().Replace(" ", "_") + ".exe";
                    Logging.Log("Checking active window's process name: [" + actProcName + "] with processes: [" + ProcessNames + "], for layout: [" + Layout + "].");
                    if (ProcessNames.ToLower().Replace(Environment.NewLine, " ").Contains(actProcName))
                    {
                        SetPersistentLayout(Layout);
                        plh.Add(hwnd);
                    }
                    else
                    {
                        nplh.Add(hwnd);
                    }
                }
                else
                {
                    Logging.Log("Already known hwnd which needs to have persistent layout, setting layout " + Layout);
                    SetPersistentLayout(Layout);
                }
            }
            catch (Exception e) { Logging.Log("Exception in Persistent layout(" + Layout + ") check, error messages & stack:\r\n" + e.Message + "+\r\n" + e.StackTrace, 1); }
        }
        void SetPersistentLayout(uint layout)
        {
            uint CurrentLayout = Locales.GetCurrentLocale();
            Logging.Log("Checking current layout: [" + CurrentLayout + "] with selected persistent layout: [" + layout + "].");
            if (CurrentLayout != layout)
            {
                KMHook.ChangeToLayout(Locales.ActiveWindow(), layout);
                Logging.Log("Layout was different, changing to: [" + layout + "].");
            }
        }
        /// <summary>
        /// Toggles timers state.
        /// </summary>
        public void ToggleTimers()
        {
            if (!Configs.fine || !ENABLED) return;
            if (!LDUseWindowsMessages)
            {
                if (LDForMouse)
                    ICheck.Start();
                else
                    ICheck.Dispose();
                if (LDForCaret)
                    crtCheck.Start();
                else
                    crtCheck.Dispose();
            }
            if (Program.MyConfs.ReadBool("Functions", "ScrollTip"))
                ScrlCheck.Start();
            else
                ScrlCheck.Dispose();
            if (Program.MyConfs.ReadBool("Functions", "CapsLockTimer"))
                capsCheck.Start();
            else
                capsCheck.Dispose();
            if ((Program.MyConfs.ReadBool("Functions", "TrayFlags") || Program.MyConfs.ReadBool("Functions", "TrayText")) && TrayIconVisible)
                flagsCheck.Start();
            else
                flagsCheck.Dispose();
            if (!PersistentLayoutOnWindowChange)
            {
                if (PersistentLayoutForLayout1)
                    persistentLayout1Check.Start();
                else
                    persistentLayout1Check.Dispose();
                if (PersistentLayoutForLayout2)
                    persistentLayout2Check.Start();
                else
                    persistentLayout2Check.Dispose();
            }
            if (LangPanelDisplay && !langPanelRefresh.Enabled)
                langPanelRefresh.Start();
            else
                langPanelRefresh.Dispose();
        }
        void AutoStartTask(bool deleteonly = false)
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<Task version=""1.2"" xmlns=""http://schemas.microsoft.com/windows/2004/02/mit/task"">
  <RegistrationInfo>
    <Date>2023-01-16T15:11:10.596</Date>
    <Author>SaaSJet</Author>
    <Description>Starts FaineSwitcher with highest priveleges.</Description>
  </RegistrationInfo>
  <Triggers>
    <LogonTrigger>
      <Enabled>true</Enabled>
    </LogonTrigger>
    <BootTrigger>
      <Enabled>true</Enabled>
    </BootTrigger>
  </Triggers>
  <Principals>
    <Principal id=""Author"">
      <UserId>" + Environment.UserDomainName + "\\" + Environment.UserName + @"</UserId>
      <LogonType>InteractiveToken</LogonType>
      <RunLevel>HighestAvailable</RunLevel>
    </Principal>
  </Principals>
  <Settings>
    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>
    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
    <StopIfGoingOnBatteries>true</StopIfGoingOnBatteries>
    <AllowHardTerminate>true</AllowHardTerminate>
    <StartWhenAvailable>true</StartWhenAvailable>
    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>
    <IdleSettings>
      <StopOnIdleEnd>true</StopOnIdleEnd>
      <RestartOnIdle>false</RestartOnIdle>
    </IdleSettings>
    <AllowStartOnDemand>true</AllowStartOnDemand>
    <Enabled>true</Enabled>
    <Hidden>false</Hidden>
    <RunOnlyIfIdle>false</RunOnlyIfIdle>
    <WakeToRun>false</WakeToRun>
    <ExecutionTimeLimit>PT0S</ExecutionTimeLimit>
    <Priority>7</Priority>
  </Settings>
  <Actions Context=""Author"">
    <Exec>
      <Command>" + "\"" + Assembly.GetExecutingAssembly().Location + "\"" + @"</Command>
    </Exec>
  </Actions>
</Task>";
            var xml_path = Path.Combine(Path.GetTempPath(), "SwitcherStartup+.xml");
            System.Threading.Tasks.Task.Factory.StartNew(() => File.WriteAllText(xml_path, xml)).Wait();
            var pif = new ProcessStartInfo
            {
                FileName = "schtasks.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = "/delete /TN SwitcherAutostart+ /f",
                CreateNoWindow = true,
                Verb = "runas"
            };
            Process.Start(pif).WaitForExit();
            if (!deleteonly)
            {
                pif.Arguments = "/create /xml \"" + xml_path + "\" /TN SwitcherAutostart+";
                Process.Start(pif).WaitForExit();
            }
            File.Delete(xml_path);
        }
        public static void SoundPlay(bool second = false)
        {
            if (SoundEnabled)
            {
                byte[] snd = second ? Properties.Resources.snd2 : Properties.Resources.snd;
                bool ucs = second ? UseCustomSound2 : UseCustomSound;
                string csf = second ? CustomSound2 : CustomSound;
                var sms = new MemoryStream(snd);
                var sp = new System.Media.SoundPlayer(sms);
                try
                {
                    csf = replaceenv(csf, "%Switcher_dir%", () => nPath);
                    if (ucs) if (File.Exists(csf))
                            sp = new System.Media.SoundPlayer(csf);
                }
                catch (Exception e)
                {
                    Logging.Log("Error during loading of the custom sound file: " + e.Message + "\n" + e.StackTrace, 1);
                    Logging.Log("Fallback to default sound...");
                }
                sp.Play();
                sp.Dispose();
                sms.Dispose();
            }
        }
        public string SelectGetWavFile()
        {
            var fp = "";
            var ofd = new OpenFileDialog();
            ofd.DefaultExt = ".wav";
            ofd.Filter = "Wave sound|*.wav";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fp = ofd.FileName;
            }
            ofd.Dispose();
            return fp;
        }
        /// <summary>
        /// Creates startup shortcut/task v3.0+v2.0.
        /// </summary>
        void CreateAutoStart()
        {
            if (AutoStartAsAdmin)
            {
                AutoStartRemove(true);
                AutoStartTask();
                //				if (AutoStartExist(false))
                //					AutoStartRemove(false);
                Logging.Log("Startup task created.");
            }
            else
            {
                AutoStartRemove(false);
                var exelocation = Assembly.GetExecutingAssembly().Location;
                var shortcutLocation = Path.Combine(
                                           Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                                           "FaineSwitcher.lnk");
                if (File.Exists(shortcutLocation))
                    return;
                Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
                dynamic shell = Activator.CreateInstance(t);
                try
                {
                    var lnk = shell.CreateShortcut(shortcutLocation);
                    try
                    {
                        lnk.TargetPath = exelocation;
                        lnk.WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        lnk.IconLocation = exelocation + ", 0";
                        lnk.Description = "FaineSwitcher - Magic layout switcher";
                        lnk.Save();
                    }
                    finally
                    {
                        Marshal.FinalReleaseComObject(lnk);
                    }
                }
                finally
                {
                    Marshal.FinalReleaseComObject(shell);
                }
                //				if (AutoStartExist(true))
                //					AutoStartRemove(true);
                Logging.Log("Startup shortcut created.");
            }
        }
        bool AutoStartExist(bool admin)
        {
            if (admin)
            {
                var pif = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c schtasks.exe /query /TN SwitcherAutoStart+ >NUL 2>&1 && echo Y",
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }
                };
                pif.Start();
                while (!pif.StandardOutput.EndOfStream)
                {
                    var l = pif.StandardOutput.ReadLine();
                    if (l.Contains("Y"))
                    {
                        Debug.WriteLine("Task exist!");
                        pif.StartInfo.Arguments = "/c schtasks.exe /query /TN SwitcherAutoStart+ /fo LIST /v";
                        pif.Start();
                        Debug.WriteLine("Checking task path...");
                        while (!pif.StandardOutput.EndOfStream)
                        {
                            l = pif.StandardOutput.ReadLine();
                            if (l.Contains(Assembly.GetExecutingAssembly().Location))
                            {
                                Debug.WriteLine("Task path OK! in: " + l);
                                pif.Dispose();
                                return true;
                            }
                        }
                    }
                }
                Debug.WriteLine("Task path wrong!");
                pif.Dispose();
                return false;
            }
            var lnk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "FaineSwitcher.lnk");
            bool actual = false;
            if (File.Exists(lnk))
            {
                Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
                dynamic shell = Activator.CreateInstance(t);
                try
                {
                    var slnk = shell.CreateShortcut(lnk);
                    try
                    {
                        if (slnk.TargetPath == Assembly.GetExecutingAssembly().Location)
                            actual = true;
                    }
                    finally
                    {
                        Marshal.FinalReleaseComObject(slnk);
                    }
                }
                finally
                {
                    Marshal.FinalReleaseComObject(shell);
                }
            }
            Debug.WriteLine("Actual: " + actual);
            return actual;
        }
        /// <summary>
        /// Remove startup with Windows.
        /// </summary>
        void AutoStartRemove(bool admin)
        {
            if (admin)
            {
                AutoStartTask(true);
                Logging.Log("Startup task removed.");
            }
            else
            {
                if (File.Exists(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                        "FaineSwitcher.lnk")))
                {
                    File.Delete(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                        "FaineSwitcher.lnk"));
                }
                Logging.Log("Startup shortcut removed.");
            }
        }
        void PreExit(bool hideicon = true, int noglobal = 0)
        {
            if (UseJKL && !KMHook.JKLERR)
                jklXHidServ.Destroy();
            if (hideicon)
                icon.Hide();
            if (RemapCapslockAsF18)
                LLHook.UnSet();
            Program.switcher.UnregisterHotkeys(noglobal);
            Program.rif.RegisterRawInputDevices(IntPtr.Zero, WinAPI.RawInputDeviceFlags.Remove);
            if (uche != null)
                uche.Abort();
            if (tmr != null) { tmr.Stop(); tmr.Dispose(); }
            if (old != null) { old.Stop(); old.Dispose(); }
            if (res != null) { res.Stop(); res.Dispose(); }
            if (resC != null) { resC.Stop(); resC.Dispose(); }
            if (stimer != null) { stimer.Stop(); stimer.Dispose(); }
            if (ICheck != null) { ICheck.Stop(); ICheck.Dispose(); }
            if (animate != null) { animate.Stop(); animate.Dispose(); }
            if (crtCheck != null) { crtCheck.Stop(); crtCheck.Dispose(); }
            if (ScrlCheck != null) { ScrlCheck.Stop(); ScrlCheck.Dispose(); }
            if (capsCheck != null) { capsCheck.Stop(); capsCheck.Dispose(); }
            if (showUpdWnd != null) { showUpdWnd.Stop(); showUpdWnd.Dispose(); }
            if (flagsCheck != null) { flagsCheck.Stop(); flagsCheck.Dispose(); }
            if (langPanelRefresh != null) { langPanelRefresh.Stop(); langPanelRefresh.Dispose(); }
            if (persistentLayout1Check != null) { persistentLayout1Check.Stop(); persistentLayout1Check.Dispose(); }
            if (persistentLayout2Check != null) { persistentLayout2Check.Stop(); persistentLayout2Check.Dispose(); }
            NCS_destroy();
        }
        /// <summary>Exits FaineSwitcher.</summary>
        public void ExitProgram()
        {
            Logging.Log("Exit by user demand.");
            PreExit();
            if (!KMHook.IfNW7())
                System.Threading.Thread.Sleep(100);
            try
            {
                var piKill = new ProcessStartInfo()
                {
                    FileName = "taskkill",
                    Arguments = "/IM FaineSwitcher.exe /F",
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process.Start(piKill);
                piKill.Arguments = "/PID " + Process.GetCurrentProcess().Id + " /F";
                Process.Start(piKill);
            }
            catch (Exception e)
            {
                Logging.Log("Taskkill error, try exit normally...", 1);
                Application.Exit();
            }
        }
        /// <summary>
        /// Registers keys 1->9 & 0 on keyboard as hotkey to be used as word count selector for Convert Multiple Words Count.
        /// </summary>
        void PrepareConvertMoreWords()
        {
            for (int i = 0; i <= 9; i++)
            {
                //				Debug.WriteLine("Registering +"+(Keys)(((int)Keys.D0)+i) + " i " +(i+100));
                WinAPI.RegisterHotKey(Handle, 100 + i, WinAPI.MOD_NO_REPEAT, ((int)Keys.D0) + i);
            }
            KMHook.waitfornum = true;
        }
        /// <summary>
        /// Unregisters keys 1->9 & 0 on keyboard that were used for Convert Multiple Words Count function.
        /// </summary>
        public void FlushConvertMoreWords()
        {
            for (int i = 100; i <= 109; i++)
            {
                //				Debug.WriteLine("Unregistering +"+i);
                WinAPI.UnregisterHotKey(Handle, i);
            }
            KMHook.waitfornum = false;
        }
        /// <summary>
        /// Unregisters FaineSwitcher hotkeys.
        /// </summary>
        /// <param name="noglobal">Keeps *global hotkeys*(the one's that goes after TransliterateSelection in HKID enum) alive if true.</param>
        public void UnregisterHotkeys(int noglobal = 0, bool excluded_related = false)
        {
            foreach (int id in Enum.GetValues(typeof(Hotkey.HKID)))
            {
                if (excluded_related)
                {
                    if (ConvertSWLinExcl)
                    {
                        if (id == (int)Hotkey.HKID.ConvertLastWord ||
                            id == (int)Hotkey.HKID.ConvertSelection ||
                            id == (int)Hotkey.HKID.ConvertLastLine)
                        {
                            continue;
                        }
                    }
                }
                if (noglobal == 1 && (id > (int)Hotkey.HKID.TransliterateSelection)) break;
                if (noglobal == 2 && (id > (int)Hotkey.HKID.ShowSelectionTranslation)) break;
                WinAPI.UnregisterHotKey(Handle, id);
            }
            for (int id = 201; id <= 300; id++)
            {
                SpecificSwitchHotkeys.Clear();
                WinAPI.UnregisterHotKey(Handle, id);
            }
        }
        public void _regHK(IntPtr h, int id, uint mod, int key)
        {
            int rk = key;
            if (LLHook.redefines.len > 0)
            {
                for (int i = 0; i != LLHook.redefines.len; i++)
                {
                    if (key == (int)LLHook.redefines[i].k)
                    {
                        rk = (int)LLHook.redefines[i].v;
                    }
                }
            }
            if (RemapCapslockAsF18)
            {
                if (key == (int)Keys.Capital)
                {
                    rk = (int)Keys.F18;
                }
                //				Debug.WriteLine("WCHI " + key + " rk:" +rk);
            }
            WinAPI.RegisterHotKey(h, id, mod, rk);
        }
        public void RegisterHotkeys()
        {
            if (HKToggleSwitcher_tempEnabled)
                WinAPI.RegisterHotKey(Handle, (int)Hotkey.HKID.ToggleSwitcher,
                                      WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKToggleSwitcher_tempMods), HKToggleSwitcher_tempKey);
            if (ENABLED)
            {
                if (!String.IsNullOrEmpty(tas))
                {
                    try
                    {
                        var tasq = tas.Split('|');
                        var mods = Hotkey.GetMods(tasq[0]);
                        var kk = (int)KMHook.strparsekey(tasq[1])[0];
                        Debug.WriteLine("TT: Mod" + mods + " " + kk);
                        _regHK(Handle, 774, WinAPI.MOD_NO_REPEAT + mods, kk);
                    }
                    catch (Exception e)
                    {
                        Logging.Log("Error syntax: modifiers|keycode/key toggle AutoSwitch hotkey:" + e.Message);
                    }
                }
                if (HKCLast_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ConvertLastWord,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKCLast_tempMods), HKCLast_tempKey);
                if (HKCSelection_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ConvertSelection,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKCSelection_tempMods), HKCSelection_tempKey);
                if (HKCLine_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ConvertLastLine,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKCLine_tempMods), HKCLine_tempKey);
                if (HKConMorWor_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ConvertMultipleWords,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKConMorWor_tempMods), HKConMorWor_tempKey);
                if (HKTitleCase_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ToTitleSelection,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKTitleCase_tempMods), HKTitleCase_tempKey);
                if (HKSwapCase_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ToSwapSelection,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKSwapCase_tempMods), HKSwapCase_tempKey);
                if (HKToUpper_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ToUpperSelection,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKToUpper_tempMods), HKToUpper_tempKey);
                if (HKToLower_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ToLowerSelection,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKToLower_tempMods), HKToLower_tempKey);
                if (HKRandomCase_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ToRandomSelection,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKRandomCase_tempMods), HKRandomCase_tempKey);
                if (HKTransliteration_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.TransliterateSelection,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKTransliteration_tempMods), HKTransliteration_tempKey);
                if (HKSymIgn_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ToggleSymbolIgnoreMode,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKSymIgn_tempMods), HKSymIgn_tempKey);
                if (Mainhk_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ToggleVisibility,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(Mainhk_tempMods), Mainhk_tempKey);
                if (ExitHk_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.Exit,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(ExitHk_tempMods), ExitHk_tempKey);
                if (HKRestart_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.Restart,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKRestart_tempMods), HKRestart_tempKey);
                if (HKToggleLangPanel_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ToggleLangPanel,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKToggleLangPanel_tempMods), HKToggleLangPanel_tempKey);
                if (HKShowSelectionTranslate_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ShowSelectionTranslation,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKShowSelectionTranslate_tempMods), HKShowSelectionTranslate_tempKey);
                if (HKCycleCase_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.CycleCase,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKCycleCase_tempMods), HKCycleCase_tempKey);
                if (HKSelCustConv_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.CustomConversion,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKSelCustConv_tempMods), HKSelCustConv_tempKey);
                if (HKShCMenuUM_tempEnabled)
                    _regHK(Handle, (int)Hotkey.HKID.ShowCMenuUnderMouse,
                                          WinAPI.MOD_NO_REPEAT + Hotkey.GetMods(HKShCMenuUM_tempMods), HKShCMenuUM_tempKey);
                if (!ChangeLayouByKey) return;
                for (int i = 1; i != SpecKeySetCount + 1; i++)
                {
                    var key = 0;
                    if (!String.IsNullOrEmpty(SpecKeySetsValues["txt_key" + i + "_key"]))
                    {
                        key = Int32.Parse(SpecKeySetsValues["txt_key" + i + "_key"]);
                        var mods = Hotkey.GetMods(SpecKeySetsValues["txt_key" + i + "_mods"]);
                        if (key == (int)Keys.CapsLock && RemapCapslockAsF18)
                            key = (int)Keys.F18;
                        if ((key == (int)Keys.LControlKey || key == (int)Keys.RControlKey || key == (int)Keys.ControlKey ||
                            key == (int)Keys.LShiftKey || key == (int)Keys.ShiftKey || key == (int)Keys.RShiftKey) &&
                            Hotkey.ContainsModifier((int)mods, (int)WinAPI.MOD_ALT))
                        {
                            HKBlockAlt.Add(200 + i);
                            Debug.WriteLine("hiHI: " + (200 + i));
                        }
                        Debug.WriteLine("Key:" + key);
                        var hk = new Hotkey(true, (uint)key, mods, 200 + i);
                        SpecificSwitchHotkeys.Add(hk);
                        WinAPI.RegisterHotKey(Handle, 200 + i, mods, key);
                    }
                }
            }
        }
        /// <summary>
        /// Converts some special keys to readable string.
        /// </summary>
        /// <param name="k">Key to be converted.</param>
        /// <param name="oninit">On initialize.</param>
        /// <returns>string</returns>
        public string Remake(Keys k, bool oninit = false, bool Double = false)
        {
            if (Double || oninit)
            {
                switch (k)
                {
                    case Keys.ShiftKey:
                        return "Shift";
                    case Keys.Menu:
                        return "Alt";
                    case Keys.ControlKey:
                        return "Control";
                }
            }
            switch (k)
            {
                case Keys.Cancel:
                    return k.ToString().Replace("Cancel", "Pause");
                case Keys.Scroll:
                    return k.ToString().Replace("Cancel", "Scroll");
                case Keys.ShiftKey:
                case Keys.Menu:
                case Keys.ControlKey:
                case Keys.LWin:
                case Keys.RWin:
                    return "";
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    return k.ToString().Replace("D", "");
                case Keys.Capital:
                    return "Caps Lock";
                default:
                    return k.ToString();
            }
        }
        /// <summary>
        /// Converts Oem Keys string to readable string.
        /// </summary>
        /// <param name="inpt">String with oem keys.</param>
        /// <returns>string</returns>
        public string OemReadable(string inpt)
        {
            return inpt
                  .Replace("Oemtilde", "`")
                  .Replace("OemMinus", "-")
                  .Replace("Oemplus", "+")
                  .Replace("OemBackslash", "\\")
                  .Replace("Oem5", "\\")
                  .Replace("OemOpenBrackets", "{")
                  .Replace("OemCloseBrackets", "}")
                  .Replace("Oem6", "}")
                  .Replace("OemSemicolon", ";")
                  .Replace("Oem1", ";")
                  .Replace("OemQuotes", "\"")
                  .Replace("Oem7", "\"")
                  .Replace("OemPeriod", ".")
                  .Replace("Oemcomma", ",")
                  .Replace("OemQuestion", "/");
        }
        /// <summary>
        /// Calls UpdateLangDisplayControls() which updates lang display controls based on selected [layout appearence]. 
        /// </summary>
        void UpdateLangDisplayControlsSwitch()
        {
            if (lsb_LangTTAppearenceForList.SelectedIndex < 4)
            {
                if (lsb_LangTTAppearenceForList.SelectedIndex > 1)
                    txt_LangTTText.Enabled = lbl_LangTTText.Enabled = false;
                else
                    txt_LangTTText.Enabled = lbl_LangTTText.Enabled = true;
                chk_LangTTTransparentColor.Enabled = btn_LangTTFont.Enabled = btn_LangTTForegroundColor.Enabled =
                    btn_LangTTBackgroundColor.Enabled = lbl_LangTTBackgroundColor.Enabled = lbl_LangTTForegroundColor.Enabled = true;
                grb_LangTTSize.Text = Program.Lang[Languages.Element.LDSize];
                lbl_LangTTWidth.Text = Program.Lang[Languages.Element.LDWidth];
                lbl_LangTTHeight.Text = Program.Lang[Languages.Element.LDHeight];
            }
            else
            {
                chk_LangTTTransparentColor.Enabled = btn_LangTTFont.Enabled = btn_LangTTForegroundColor.Enabled =
                    btn_LangTTBackgroundColor.Enabled = lbl_LangTTBackgroundColor.Enabled = lbl_LangTTForegroundColor.Enabled = false;
                grb_LangTTSize.Text = Program.Lang[Languages.Element.LDPosition];
                lbl_LangTTWidth.Text = Program.Lang[Languages.Element.MCDSTopIndent];
                lbl_LangTTHeight.Text = Program.Lang[Languages.Element.MCDSBottomIndent];
            }
            lbl_LangTTWidth.Text += ":";
            lbl_LangTTHeight.Text += ":";
            switch (lsb_LangTTAppearenceForList.SelectedIndex)
            {
                case 0:
                    UpdateLangDisplayControls(Layout1Fore_temp, Layout1Back_temp, Layout1TransparentBack_temp,
                                              Layout1Font_temp, Layout1X_Pos_temp, Layout1Y_Pos_temp, Layout1Width_temp,
                                              Layout1Height_temp, Layout1TText);
                    chk_LangTTUpperArrow.Visible = chk_LangTTUseFlags.Visible = false;
                    lbl_LangTTText.Visible = txt_LangTTText.Visible = true;
                    break;
                case 1:
                    UpdateLangDisplayControls(Layout2Fore_temp, Layout2Back_temp, Layout2TransparentBack_temp,
                                              Layout2Font_temp, Layout2X_Pos_temp, Layout2Y_Pos_temp, Layout2Width_temp,
                                              Layout2Height_temp, Layout2TText);
                    chk_LangTTUpperArrow.Visible = chk_LangTTUseFlags.Visible = false;
                    lbl_LangTTText.Visible = txt_LangTTText.Visible = true;
                    break;
                case 2:
                    UpdateLangDisplayControls(LDMouseFore_temp, LDMouseBack_temp, LDMouseTransparentBack_temp,
                                              LDMouseFont_temp, LDMouseX_Pos_temp, LDMouseY_Pos_temp, LDMouseWidth_temp,
                                              LDMouseHeight_temp, "", LDMouseUseFlags_temp, mouseLTUpperArrow);
                    chk_LangTTUpperArrow.Visible = chk_LangTTUseFlags.Visible = true;
                    lbl_LangTTText.Visible = txt_LangTTText.Visible = false;
                    break;
                case 3:
                    UpdateLangDisplayControls(LDCaretFore_temp, LDCaretBack_temp, LDCaretTransparentBack_temp,
                                              LDCaretFont_temp, LDCaretX_Pos_temp, LDCaretY_Pos_temp, LDCaretWidth_temp,
                                              LDCaretHeight_temp, "", LDCaretUseFlags_temp, caretLTUpperArrow);
                    chk_LangTTUpperArrow.Visible = chk_LangTTUseFlags.Visible = true;
                    lbl_LangTTText.Visible = txt_LangTTText.Visible = false;
                    break;
                case 4:
                    UpdateLangDisplayControls(LDCaretFore_temp, LDCaretBack_temp, LDCaretTransparentBack_temp,
                                              LDCaretFont_temp, MCDS_Xpos_temp, MCDS_Ypos_temp, MCDS_TopIndent_temp,
                                              MCDS_BottomIndent_temp);
                    chk_LangTTUpperArrow.Visible = chk_LangTTUseFlags.Visible = false;
                    lbl_LangTTText.Visible = txt_LangTTText.Visible = false;
                    break;
            }
        }
        /// <summary>
        /// Updates lang display controls.
        /// </summary>
        /// <param name="FGcolor">Foreground color.</param>
        /// <param name="BGColor">Background color.</param>
        /// <param name="TransparentBG">Transparent background color.</param>
        /// <param name="font">Font.</param>
        /// <param name="posX">Position x.</param>
        /// <param name="posY">Position y.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        void UpdateLangDisplayControls(Color FGcolor, Color BGColor, bool TransparentBG, Font font,
                                       int posX, int posY, int width, int height, string TTText = "", bool UseFlags = false, bool arrow = false)
        {
            btn_LangTTForegroundColor.BackColor = FGcolor;
            btn_LangTTBackgroundColor.BackColor = BGColor;
            chk_LangTTTransparentColor.Checked = TransparentBG;
            btn_LangTTFont.Font = font;
            nud_LangTTPositionX.Value = posX;
            nud_LangTTPositionY.Value = posY;
            nud_LangTTWidth.Value = width;
            nud_LangTTHeight.Value = height;
            txt_LangTTText.Text = TTText;
            chk_LangTTUseFlags.Checked = UseFlags;
            chk_LangTTUpperArrow.Checked = arrow;
        }
        /// <summary>
        /// Updates Lang Display temporary variables based on selected [layout appearence]. 
        /// </summary>
        void UpdateLangDisplayTemps()
        {
            switch (lsb_LangTTAppearenceForList.SelectedIndex)
            {
                case 0:
                    Layout1Fore_temp = btn_LangTTForegroundColor.BackColor;
                    Layout1Back_temp = btn_LangTTBackgroundColor.BackColor;
                    Layout1Font_temp = btn_LangTTFont.Font;
                    Layout1X_Pos_temp = (int)nud_LangTTPositionX.Value;
                    Layout1Y_Pos_temp = (int)nud_LangTTPositionY.Value;
                    Layout1Width_temp = (int)nud_LangTTWidth.Value;
                    Layout1Height_temp = (int)nud_LangTTHeight.Value;
                    Layout1TransparentBack_temp = chk_LangTTTransparentColor.Checked;
                    Layout1TText = txt_LangTTText.Text;
                    break;
                case 1:
                    Layout2Fore_temp = btn_LangTTForegroundColor.BackColor;
                    Layout2Back_temp = btn_LangTTBackgroundColor.BackColor;
                    Layout2Font_temp = btn_LangTTFont.Font;
                    Layout2X_Pos_temp = (int)nud_LangTTPositionX.Value;
                    Layout2Y_Pos_temp = (int)nud_LangTTPositionY.Value;
                    Layout2Width_temp = (int)nud_LangTTWidth.Value;
                    Layout2Height_temp = (int)nud_LangTTHeight.Value;
                    Layout2TransparentBack_temp = chk_LangTTTransparentColor.Checked;
                    Layout2TText = txt_LangTTText.Text;
                    break;
                case 2:
                    LDMouseFore_temp = btn_LangTTForegroundColor.BackColor;
                    LDMouseBack_temp = btn_LangTTBackgroundColor.BackColor;
                    LDMouseFont_temp = btn_LangTTFont.Font;
                    LDMouseX_Pos_temp = (int)nud_LangTTPositionX.Value;
                    LDMouseY_Pos_temp = (int)nud_LangTTPositionY.Value;
                    LDMouseWidth_temp = (int)nud_LangTTWidth.Value;
                    LDMouseHeight_temp = (int)nud_LangTTHeight.Value;
                    LDMouseUseFlags_temp = chk_LangTTUseFlags.Checked;
                    mouseLTUpperArrow = chk_LangTTUpperArrow.Checked;
                    LDMouseTransparentBack_temp = chk_LangTTTransparentColor.Checked;
                    break;
                case 3:
                    LDCaretFore_temp = btn_LangTTForegroundColor.BackColor;
                    LDCaretBack_temp = btn_LangTTBackgroundColor.BackColor;
                    LDCaretFont_temp = btn_LangTTFont.Font;
                    LDCaretX_Pos_temp = (int)nud_LangTTPositionX.Value;
                    LDCaretY_Pos_temp = (int)nud_LangTTPositionY.Value;
                    LDCaretWidth_temp = (int)nud_LangTTWidth.Value;
                    LDCaretHeight_temp = (int)nud_LangTTHeight.Value;
                    LDCaretUseFlags_temp = chk_LangTTUseFlags.Checked;
                    caretLTUpperArrow = chk_LangTTUpperArrow.Checked;
                    LDCaretTransparentBack_temp = chk_LangTTTransparentColor.Checked;
                    break;
                case 4:
                    MCDS_Xpos_temp = (int)nud_LangTTPositionX.Value;
                    MCDS_Ypos_temp = (int)nud_LangTTPositionY.Value;
                    MCDS_TopIndent_temp = (int)nud_LangTTWidth.Value;
                    MCDS_BottomIndent_temp = (int)nud_LangTTHeight.Value;
                    break;
            }
        }
        /// <summary>
        /// Calls UpdateHotkeyControls() which updates hotkey controls based on selected [layout appearence]. 
        /// </summary>
        void UpdateHotkeyControlsSwitch()
        {
            chk_DoubleHotkey.Enabled = lsb_Hotkeys.SelectedIndex != 13;
            switch (lsb_Hotkeys.SelectedIndex)
            {
                case 0:
                    UpdateHotkeyControls(Mainhk_tempEnabled, Mainhk_tempDouble, Mainhk_tempMods, Mainhk_tempKey);
                    break;
                case 1:
                    UpdateHotkeyControls(HKCLast_tempEnabled, HKCLast_tempDouble, HKCLast_tempMods, HKCLast_tempKey);
                    break;
                case 2:
                    UpdateHotkeyControls(HKCSelection_tempEnabled, HKCSelection_tempDouble, HKCSelection_tempMods, HKCSelection_tempKey);
                    break;
                case 3:
                    UpdateHotkeyControls(HKCLine_tempEnabled, HKCLine_tempDouble, HKCLine_tempMods, HKCLine_tempKey);
                    break;
                case 4:
                    UpdateHotkeyControls(HKConMorWor_tempEnabled, HKConMorWor_tempDouble, HKConMorWor_tempMods, HKConMorWor_tempKey);
                    break;
                case 5:
                    UpdateHotkeyControls(HKSymIgn_tempEnabled, HKSymIgn_tempDouble, HKSymIgn_tempMods, HKSymIgn_tempKey);
                    break;
                case 6:
                    UpdateHotkeyControls(HKTitleCase_tempEnabled, HKTitleCase_tempDouble, HKTitleCase_tempMods, HKTitleCase_tempKey);
                    break;
                case 7:
                    UpdateHotkeyControls(HKRandomCase_tempEnabled, HKRandomCase_tempDouble, HKRandomCase_tempMods, HKRandomCase_tempKey);
                    break;
                case 8:
                    UpdateHotkeyControls(HKSwapCase_tempEnabled, HKSwapCase_tempDouble, HKSwapCase_tempMods, HKSwapCase_tempKey);
                    break;
                case 9:
                    UpdateHotkeyControls(HKToUpper_tempEnabled, HKToUpper_tempDouble, HKToUpper_tempMods, HKToUpper_tempKey);
                    break;
                case 10:
                    UpdateHotkeyControls(HKToLower_tempEnabled, HKToLower_tempDouble, HKToLower_tempMods, HKToLower_tempKey);
                    break;
                case 11:
                    UpdateHotkeyControls(HKTransliteration_tempEnabled, HKTransliteration_tempDouble, HKTransliteration_tempMods, HKTransliteration_tempKey);
                    break;
                case 12:
                    UpdateHotkeyControls(ExitHk_tempEnabled, ExitHk_tempDouble, ExitHk_tempMods, ExitHk_tempKey);
                    break;
                case 13:
                    UpdateHotkeyControls(HKRestart_tempEnabled, false, HKRestart_tempMods, HKRestart_tempKey);
                    break;
                case 14:
                    UpdateHotkeyControls(HKToggleLangPanel_tempEnabled, HKToggleLangPanel_tempDouble, HKToggleLangPanel_tempMods, HKToggleLangPanel_tempKey);
                    break;
                case 15:
                    UpdateHotkeyControls(HKShowSelectionTranslate_tempEnabled, HKShowSelectionTranslate_tempDouble, HKShowSelectionTranslate_tempMods, HKShowSelectionTranslate_tempKey);
                    break;
                case 16:
                    UpdateHotkeyControls(HKToggleSwitcher_tempEnabled, HKToggleSwitcher_tempDouble, HKToggleSwitcher_tempMods, HKToggleSwitcher_tempKey);
                    break;
                case 17:
                    UpdateHotkeyControls(HKCycleCase_tempEnabled, HKCycleCase_tempDouble, HKCycleCase_tempMods, HKCycleCase_tempKey);
                    break;
                case 18:
                    UpdateHotkeyControls(HKSelCustConv_tempEnabled, HKSelCustConv_tempDouble, HKSelCustConv_tempMods, HKSelCustConv_tempKey);
                    break;
                case 19:
                    UpdateHotkeyControls(HKShCMenuUM_tempEnabled, HKShCMenuUM_tempDouble, HKShCMenuUM_tempMods, HKShCMenuUM_tempKey);
                    break;
            }
        }
        /// <summary>
        /// Updates hotkey controls.
        /// </summary>
        void UpdateHotkeyControls(bool enabled, bool Double, string modifiers, int key)
        {
            chk_HotKeyEnabled.Checked = enabled;
            chk_DoubleHotkey.Checked = Double;
            txt_Hotkey.Text = Regex.Replace(OemReadable(modifiers.Replace(",", " +") +
                                                        " + " + Remake((Keys)key, true, Double)),
                                                        @"Win\s?\+?\s?|\s?\+?\s?None\s?\+?\s?|^[ +]+|\s?\+\s?$", "", RegexOptions.Multiline);
            chk_WinInHotKey.Checked = modifiers.Contains("Win");
            txt_Hotkey_tempKey = key;
            txt_Hotkey_tempModifiers = Regex.Replace(modifiers.Replace("Win", ""), @"^[ +]+", "", RegexOptions.Multiline);
            // Debug.WriteLine(txt_Hotkey_tempModifiers);
        }
        /// <summary>
        /// Updates Hotkey temporary variables based on selected [layout appearence]. 
        /// </summary>
        void UpdateHotkeyTemps()
        {
            switch (lsb_Hotkeys.SelectedIndex)
            {
                case 0:
                    Mainhk_tempEnabled = chk_HotKeyEnabled.Checked;
                    Mainhk_tempDouble = chk_DoubleHotkey.Checked;
                    Mainhk_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    Mainhk_tempKey = txt_Hotkey_tempKey;
                    break;
                case 1:
                    HKCLast_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKCLast_tempDouble = chk_DoubleHotkey.Checked;
                    HKCLast_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKCLast_tempKey = txt_Hotkey_tempKey;
                    break;
                case 2:
                    HKCSelection_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKCSelection_tempDouble = chk_DoubleHotkey.Checked;
                    HKCSelection_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKCSelection_tempKey = txt_Hotkey_tempKey;
                    break;
                case 3:
                    HKCLine_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKCLine_tempDouble = chk_DoubleHotkey.Checked;
                    HKCLine_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKCLine_tempKey = txt_Hotkey_tempKey;
                    break;
                case 4:
                    HKConMorWor_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKConMorWor_tempDouble = chk_DoubleHotkey.Checked;
                    HKConMorWor_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKConMorWor_tempKey = txt_Hotkey_tempKey;
                    break;
                case 5:
                    HKSymIgn_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKSymIgn_tempDouble = chk_DoubleHotkey.Checked;
                    HKSymIgn_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKSymIgn_tempKey = txt_Hotkey_tempKey;
                    break;
                case 6:
                    HKTitleCase_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKTitleCase_tempDouble = chk_DoubleHotkey.Checked;
                    HKTitleCase_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKTitleCase_tempKey = txt_Hotkey_tempKey;
                    break;
                case 7:
                    HKRandomCase_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKRandomCase_tempDouble = chk_DoubleHotkey.Checked;
                    HKRandomCase_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKRandomCase_tempKey = txt_Hotkey_tempKey;
                    break;
                case 8:
                    HKSwapCase_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKSwapCase_tempDouble = chk_DoubleHotkey.Checked;
                    HKSwapCase_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKSwapCase_tempKey = txt_Hotkey_tempKey;
                    break;
                case 9:
                    HKToUpper_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKToUpper_tempDouble = chk_DoubleHotkey.Checked;
                    HKToUpper_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKToUpper_tempKey = txt_Hotkey_tempKey;
                    break;
                case 10:
                    HKToLower_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKToLower_tempDouble = chk_DoubleHotkey.Checked;
                    HKToLower_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKToLower_tempKey = txt_Hotkey_tempKey;
                    break;
                case 11:
                    HKTransliteration_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKTransliteration_tempDouble = chk_DoubleHotkey.Checked;
                    HKTransliteration_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKTransliteration_tempKey = txt_Hotkey_tempKey;
                    break;
                case 12:
                    ExitHk_tempEnabled = chk_HotKeyEnabled.Checked;
                    ExitHk_tempDouble = chk_DoubleHotkey.Checked;
                    ExitHk_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    ExitHk_tempKey = txt_Hotkey_tempKey;
                    break;
                case 13:
                    HKRestart_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKRestart_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKRestart_tempKey = txt_Hotkey_tempKey;
                    break;
                case 14:
                    HKToggleLangPanel_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKToggleLangPanel_tempDouble = chk_DoubleHotkey.Checked;
                    HKToggleLangPanel_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKToggleLangPanel_tempKey = txt_Hotkey_tempKey;
                    break;
                case 15:
                    HKShowSelectionTranslate_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKShowSelectionTranslate_tempDouble = chk_DoubleHotkey.Checked;
                    HKShowSelectionTranslate_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKShowSelectionTranslate_tempKey = txt_Hotkey_tempKey;
                    break;
                case 16:
                    HKToggleSwitcher_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKToggleSwitcher_tempDouble = chk_DoubleHotkey.Checked;
                    HKToggleSwitcher_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKToggleSwitcher_tempKey = txt_Hotkey_tempKey;
                    break;
                case 17:
                    HKCycleCase_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKCycleCase_tempDouble = chk_DoubleHotkey.Checked;
                    HKCycleCase_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKCycleCase_tempKey = txt_Hotkey_tempKey;
                    break;
                case 18:
                    HKSelCustConv_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKSelCustConv_tempDouble = chk_DoubleHotkey.Checked;
                    HKSelCustConv_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKSelCustConv_tempKey = txt_Hotkey_tempKey;
                    break;
                case 19:
                    HKShCMenuUM_tempEnabled = chk_HotKeyEnabled.Checked;
                    HKShCMenuUM_tempDouble = chk_DoubleHotkey.Checked;
                    HKShCMenuUM_tempMods = (chk_WinInHotKey.Checked ? "Win + " : "") + txt_Hotkey_tempModifiers;
                    HKShCMenuUM_tempKey = txt_Hotkey_tempKey;
                    break;
            }
        }
        /// <summary>
        /// Returns selected hotkey's Double bool.
        /// </summary>
        bool GetSelectedHotkeyDoubleTemp()
        {
            switch (lsb_Hotkeys.SelectedIndex)
            {
                case 0:
                    return Mainhk_tempDouble;
                case 1:
                    return HKCLast_tempDouble;
                case 2:
                    return HKCSelection_tempDouble;
                case 3:
                    return HKCLine_tempDouble;
                case 4:
                    return HKConMorWor_tempDouble;
                case 5:
                    return HKSymIgn_tempDouble;
                case 6:
                    return HKTitleCase_tempDouble;
                case 7:
                    return HKRandomCase_tempDouble;
                case 8:
                    return HKSwapCase_tempDouble;
                case 9:
                    return HKToUpper_tempDouble;
                case 10:
                    return HKToLower_tempDouble;
                case 11:
                    return HKTransliteration_tempDouble;
                case 12:
                    return ExitHk_tempDouble;
                case 13:
                    return false;
            }
            return false;
        }
        void UpdateSetControls(int setIndex, int keyCode, string modifiers)
        {
            //var _set = pan_KeySets.Controls["set_"+setIndex];
            //_set.Controls["txt_key"+setIndex].Text = Regex.Replace(OemReadable(modifiers.Replace(",", " +") +
            //                                            " + " + Remake((Keys)keyCode, true, false)), 
            //                                            @"Win\s?\+?\s?|\s?\+?\s?None\s?\+?\s?|^[ +]+|\s?\+\s?$", "", RegexOptions.Multiline);
            //(_set.Controls["chk_win"+setIndex] as CheckBox).Checked = modifiers.Contains("Win");
        }
        void DeleteOrMove(string file)
        {
            try
            {
                File.Delete(file);
                Logging.Log("Deleting file [" + file + "] succeeded.");
            }
            catch
            {
                Logging.Log("Deleting file [" + file + "] not succeeded, trying to move.", 2);
                try
                {
                    var name = Guid.NewGuid().ToString("n").Substring(0, 8);
                    var d = Path.GetDirectoryName(file);
                    var trash = Path.Combine(d, "trash");
                    if (!Directory.Exists(trash))
                        Directory.CreateDirectory(trash);
                    var f = Path.Combine(trash, name);
                    File.Move(file, f);
                }
                catch (Exception e)
                {
                    Logging.Log("Unexpected error happened when trying to move file, details:\r\n" + e.Message + "\r\n" + e.StackTrace, 1);
                }
            }
        }
        void DeleteTrash()
        {
            var trash = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "trash");
            try
            {
                if (Directory.Exists(trash))
                {
                    Directory.Delete(trash, true);
                    Logging.Log("Deleting [" + trash + "] directory succeeded.");
                }
                else
                {
                    Logging.Log("No trash found. (" + trash + ")");
                }
            }
            catch (Exception e)
            {
                Logging.Log("Error deleting trash directory, details:\r\n" + e.Message + "\r\n" + e.StackTrace, 2);
            }
        }
        void DeleteOldJKL()
        {
            if (jklXHidServ.jklExist())
            {
                var jkl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "jkl");
                if (jklXHidServ.jklFEX[0])
                    DeleteOrMove(jkl + ".exe");
                if (jklXHidServ.jklFEX[1])
                    DeleteOrMove(jkl + ".dll");
                if (jklXHidServ.jklFEX[2])
                    DeleteOrMove(jkl + "x86.exe");
                if (jklXHidServ.jklFEX[3])
                    DeleteOrMove(jkl + "x86.dll");
            }
        }
        void ShowContextMenuUnderMouse()
        {
            icon.trIcon.ContextMenuStrip.Show(Cursor.Position);
            icon.trIcon.ContextMenuStrip.Focus();
        }
        void ShowSwitcherMMMenuUnderMouse()
        {
            if (!SwitcherMM) return;
            //			List<ToolStripMenuItem> mmdd = new List<ToolStripMenuItem>();
            //			var mmddi = Program.switcher.icon.trIcon.ContextMenuStrip.Items[0] as ToolStripMenuItem;
            //			for(var i = 0; i != mmddi.DropDownItems.Count; i++) {
            //				var z = mmddi.DropDownItems[i];
            //				var events = typeof(System.ComponentModel.Component).
            //							GetField("events", BindingFlags.NonPublic | BindingFlags.Instance);
            //				var all_events = events.GetValue(z);
            //				var y = new ToolStripMenuItem(z.Text);
            //				y.Name = z.Name;
            //				events.SetValue(y, all_events);
            //				mmdd.Add(y);
            //			}
            //			menu.Items.AddRange(mmdd.ToArray());
            MMmenu.BackColor = SystemColors.Control;
            MMmenu.RenderMode = ToolStripRenderMode.System;
            MMmenu.Show(Cursor.Position);
            MMmenu.Focus();
            if (MMmenu.Items.Count > 0)
                MMmenu.Items[0].Select();
            Timer t = null;
            if (SwitcherMMTrayHoverLostFocusClose)
            {
                MMmenu.LostFocus += (_, __) => { MMmenu.Hide(); };
                //				MMmenu.VisibleChanged += (_, __) => {
                //					if (!menu.Visible) {
                //						menu.Close();
                //					}
                //				};
                t = new Timer();
                t.Interval = 50;
                var con = 0;
                t.Tick += (_, __) =>
                {
                    var magick = MMmenu.PointToClient(Cursor.Position);
                    for (int i = 0; i != MMmenu.Items.Count; i++)
                    {
                        var x = MMmenu.Items[i] as ToolStripMenuItem;
                        if (x.HasDropDownItems)
                        {
                            if (magick.X < 0 || magick.Y < 0)
                            {
                                magick = x.DropDown.PointToClient(Cursor.Position);
                                //								while (x.HasDropDownItems) {
                                //									if (magick.X < 0 || magick.Y < 0) {
                                //										for (int z = 0; z < x.DropDownItems.Count; z++) {
                                //											var y = x.DropDownItems[z] as ToolStripMenuItem;
                                //											if (y.HasDropDownItems) {
                                //												magick = y.DropDown.PointToClient(Cursor.Position);
                                //												x = y;
                                //											}
                                //										}
                                //									}
                                //								}
                            }
                        }
                    }
                    Debug.WriteLine(magick);
                    if (magick.X < 0 || magick.Y < 0)
                    {
                        con += 50;
                    }
                    else
                    {
                        con = 0;
                    }
                    if (con >= TrayHoverSwitcherMM * 1.5)
                    {
                        Debug.WriteLine("Out of menu for " + con + "ms!, autohide!");
                        //						if (!menu.IsDisposed || !menu.Disposing)
                        MMmenu.Hide();
                        t.Stop();
                        t.Dispose();
                    }
                };
                t.Start();
            }
            MMmenu.PreviewKeyDown += (_, __) =>
            {
                if (__.KeyCode == Keys.Escape)
                {
                    MMmenu.Hide();
                }
                if (t != null)
                {
                    Debug.WriteLine("Autohide disabled by keyboard.");
                    t.Stop();
                    t.Dispose();
                    t = null;
                }
            };
            WinAPI.SetForegroundWindow(MMmenu.Handle);
        }
        #region Updates functions
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (isold)
                _progress = e.ProgressPercentage;
            prb_UpdateDownloadProgress.Value = progress = e.ProgressPercentage;
            //Below in "if" is AUTO-UPDATE feature ;)
            if (e.ProgressPercentage == 100 && !was)
            {
                Logging.Log("Download of FaineSwitcher update [" + UpdInfo[3] + "] finished.");
                int SwitcherPID = Process.GetCurrentProcess().Id;
                //Downloaded archive
                var arch = Regex.Match(UpdInfo[3], @"[^\\\/]+$").Groups[0].Value;
                PreExit();
                DeleteOldJKL();
                //Batch script to create other script o.0,
                //which shutdown running FaineSwitcher,
                //delete old version,
                //unzip downloaded one, and start it.
                var silent = Program.MyConfs.ReadBool("Functions", "SilentUpdate");
                var UpdateSwitcher =
                    @"@ECHO OFF
chcp 65001
SET SwitcherDIR=" + AppDomain.CurrentDomain.BaseDirectory + @"
SET FaineSwitcher=" + AppDomain.CurrentDomain.FriendlyName + @"
TASKKILL /PID " + SwitcherPID + @" /F
TASKKILL /IM %FaineSwitcher% /F
set i=1
:loop
DEL /Q /F /A ""%SwitcherDIR%%FaineSwitcher%""
set /a i=%i%+1
if %i% == 333 goto continue
if not exist ""%SwitcherDIR%%FaineSwitcher%"" goto continue
goto loop
:continue
echo %i%
ECHO x0 = replace(Wscript.Arguments(0), ""\\"", ""\"") > ""%TEMP%\unzip.vbs""
ECHO x1 = replace(Wscript.Arguments(1), ""\\"", ""\"") >> ""%TEMP%\unzip.vbs""
ECHO With CreateObject(""Shell.Application"") >> ""%TEMP%\unzip.vbs""
ECHO    .NameSpace(x1).CopyHere .NameSpace(x0).items, 20 >> ""%TEMP%\unzip.vbs""
ECHO End With >> ""%TEMP%\unzip.vbs""

CSCRIPT ""%TEMP%\unzip.vbs"" ""%TEMP%\" + arch + @""" ""%SwitcherDIR%""

START """" ""%SwitcherDIR%FaineSwitcher.exe"" " + (!silent ? "\"_!_updated_!_\"" : "\"_!_silent_updated_!_\"") + @"
DEL /Q /F /A ""%TEMP%\" + arch + @"""
DEL /Q /F /A ""%TEMP%\unzip.vbs""
DEL /Q /F /A ""%TEMP%\UpdateSwitcher.cmd""";
                //Save Batch script
                Logging.Log("Writing update script.");
                var fn = Path.Combine(Path.GetTempPath(), "UpdateSwitcher.cmd");
                File.WriteAllText(fn, UpdateSwitcher);
                var piUpdateSwitcher = new ProcessStartInfo();
                piUpdateSwitcher.FileName = fn;
                //Make UpdateSwitcher.cmd's startup hidden
                piUpdateSwitcher.WindowStyle = ProcessWindowStyle.Hidden;
                //Start updating(unzipping)
                Logging.Log("Starting update script.");
                Process.Start(piUpdateSwitcher);
                was = true;
                ExitProgram();
            }
        }
        string getASD_RemoteSize(bool InZip = false)
        {
            try
            {
                Logging.Log("getASD_RemoteSize(): Not Allow");
                return "0.00";

                if (InZip)
                {
                    var data = getResponce("https://github.com/BladeMight/FaineSwitcher/releases/latest-commit");
                    if (!String.IsNullOrEmpty(data))
                    {
                        var siz = Regex.Match(data, "<small class=\"text-gray float-right\">(.+)</small>").Groups[1].Value;
                        Logging.Log("Remote size of AS_dict: " + siz);
                        return siz;
                    }
                    else throw new Exception(Program.Lang[Languages.Element.NetError]);
                }
                var request = (HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/BladeMight/FaineSwitcher/master/AS_dict.txt");
                if (!String.IsNullOrEmpty(txt_ProxyServerPort.Text))
                {
                    request.Proxy = MakeProxy();
                }
                request.Method = "HEAD";
                request.AllowAutoRedirect = false;
                using (var r = (HttpWebResponse)request.GetResponse())
                {
                    var type = " B";
                    var D = Convert.ToDouble(r.ContentLength);
                    if (D / 1024 != 0 && D / 1024 >= 1)
                    {
                        D /= 1024;
                        type = " KB";
                        if (D / 1024 != 0 && D / 1024 >= 1)
                        {
                            D /= 1024;
                            type = " MB";
                        }
                    }
                    return D.ToString("0.00") + type;
                }
            }
            catch (Exception e)
            {
                Logging.Log("Getting remote size of AS_dict failed, details: " + e.Message, 1);
                return Program.Lang[Languages.Element.Error];
            }
        }
        string getResponce(string url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "request";
                // For proxy
                if (!String.IsNullOrEmpty(txt_ProxyServerPort.Text))
                {
                    request.Proxy = MakeProxy();
                }
                request.ServicePoint.SetTcpKeepAlive(true, 5000, 1000);
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var sr = new StreamReader(response.GetResponseStream(), true);
                    var data = sr.ReadToEnd();
                    sr.Dispose();
                    response.Close();
                    Logging.Log("Responce of url [" + url + "] succeded.");
                    return data;
                }
                response.Close();
            }
            catch (Exception e)
            {
                Logging.Log("Responce of url [" + url + "] done with error, message:\r\n" + e.Message + e.StackTrace, 1);
            }
            return null;

        }
        void btn_UpdateAutoSwitchDictionary_Click(object sender, EventArgs e)
        {

            Logging.Log("btn_UpdateAutoSwitchDictionary_Click(): Not Allow");

            return;
            var resp = "";
            if (check_ASD_size)
            {
                var size = getASD_RemoteSize(Dowload_ASD_InZip);
                if (size == Program.Lang[Languages.Element.Error])
                {
                    btn_UpdateAutoSwitchDictionary.ForeColor = Color.OrangeRed;
                    btn_UpdateAutoSwitchDictionary.Text = Program.Lang[Languages.Element.Error];
                    tmr.Tick += (o, oo) =>
                    {
                        btn_UpdateAutoSwitchDictionary.Text = Program.Lang[Languages.Element.AutoSwitchUpdateDictionary];
                        btn_UpdateAutoSwitchDictionary.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                        tmr.Stop();
                    };
                    tmr.Interval = 350;
                    tmr.Start();
                    return;
                }
                check_ASD_size = false;
                var name = "AS_dict.txt";
                if (Dowload_ASD_InZip)
                    name = "AS_dict.zip";
                btn_UpdateAutoSwitchDictionary.Text = name + " " + size + " " + Program.Lang[Languages.Element.Download] + "?";
                return;
            }
            check_ASD_size = true;
            if (Dowload_ASD_InZip)
            {
                var zip = Path.Combine(Path.GetTempPath(), "AS_dict.zip");
                using (var wc = new WebClient())
                {
                    // For proxy
                    if (!String.IsNullOrEmpty(txt_ProxyServerPort.Text))
                    {
                        wc.Proxy = MakeProxy();
                    }
                    wc.DownloadFile(new Uri("https://github.com/BladeMight/FaineSwitcher/releases/download/latest-commit/AS_dict.zip"), zip);
                    var ExtractASD = @"@ECHO OFF
chcp 65001
ECHO With CreateObject(""Shell.Application"") > ""unzip.vbs""
ECHO    .NameSpace(WScript.Arguments(1)).CopyHere .NameSpace(WScript.Arguments(0)).items, 16 >> ""unzip.vbs""
ECHO End With >> ""unzip.vbs""

CSCRIPT ""unzip.vbs"" """ + zip + @""" """ + Path.GetTempPath() + @"""
DEL """ + zip + @"""
DEL ""unzip.vbs""
DEL ""ExtractASD.cmd""";
                    Logging.Log("Writing extract script.");
                    File.WriteAllText(Path.Combine(Path.GetTempPath(), "ExtractASD.cmd"), ExtractASD);
                    var piExtractASD = new ProcessStartInfo() { FileName = "ExtractASD.cmd", WorkingDirectory = Path.GetTempPath(), WindowStyle = ProcessWindowStyle.Hidden };
                    Logging.Log("Starting extract script.");
                    Process.Start(piExtractASD).WaitForExit();
                    resp = File.ReadAllText(Path.Combine(Path.GetTempPath(), "AS_dict.txt"));
                    File.Delete(Path.Combine(Path.GetTempPath(), "AS_dict.txt"));
                }
            }
            else
                resp = getResponce("https://raw.githubusercontent.com/BladeMight/FaineSwitcher/master/AS_dict.txt");
            btn_UpdateAutoSwitchDictionary.Text = Program.Lang[Languages.Element.Checking];
            var dict = Regex.Replace(resp, "\r?\n", Environment.NewLine);
            tmr.Interval = 300;
            if (dict != null)
            {
                btn_UpdateAutoSwitchDictionary.ForeColor = Color.BlueViolet;
                btn_UpdateAutoSwitchDictionary.Text = "OK";
                tmr.Tick += (o, oo) =>
                {
                    btn_UpdateAutoSwitchDictionary.Text = Program.Lang[Languages.Element.AutoSwitchUpdateDictionary];
                    btn_UpdateAutoSwitchDictionary.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                    tmr.Stop();
                };
                tmr.Interval = 350;
                tmr.Start();
                AutoSwitchDictionaryRaw = dict;
                this.txt_AutoSwitchDictionary.Invoke((MethodInvoker)delegate
                {
                    ChangeAutoSwitchDictionaryTextBox();
                    UpdateSnippetCountLabel(AutoSwitchDictionaryRaw, lbl_AutoSwitchWordsCount, false);
                });
                File.WriteAllText(AS_dictfile, dict, Encoding.UTF8);
            }
            else
            {
                btn_UpdateAutoSwitchDictionary.ForeColor = Color.OrangeRed;
                btn_UpdateAutoSwitchDictionary.Text = Program.Lang[Languages.Element.Error];
                tmr.Tick += (o, oo) =>
                {
                    btn_UpdateAutoSwitchDictionary.Text = Program.Lang[Languages.Element.AutoSwitchUpdateDictionary];
                    btn_UpdateAutoSwitchDictionary.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                    tmr.Stop();
                };
                tmr.Interval = 350;
                tmr.Start();
            }
        }
        static Regex rx = new Regex(@"\\u([a-fA-f0-9]{4})", RegexOptions.Compiled);
        public static string UnescapeUnicode(string x)
        {
            if (!x.Contains("\\u")) return x;
            var rep = rx.Replace(x, a => ((char)int.Parse(a.Groups[1].Value, System.Globalization.NumberStyles.HexNumber)).ToString()
            );
            //			Debug.WriteLine("REP:X: " + rep);
            return rep;
        }
        static string trimlr(string t, int c = 1)
        {
            if (t.Length <= c) return "";
            return t.Substring(c, t.Length - (c + 1));
        }
        /// <summary>
        /// Gets update info, and sets it to static [UpdInfo] string.
        /// </summary>
        void GetUpdateInfo()
        {

            Logging.Log("GetUpdateInfo(): Not Allow");
            UpdInfo = new string[5] { "Not Allow", "Not Allow By Developer", "Not Allow", "Not Allow", "Not Allow" };
            return;

            var Info = new string[5] { "", "", "", "", "" }; // Update info
            var api = "https://api.github.com/repos/BladeMight/FaineSwitcher/releases";
            var url = api + "/latest";
            var beta = Program.MyConfs.Read("Updates", "Channel") != "Stable";
            if (beta)
            {
                url = api + "/tags/latest-commit";
            }
            var data = getResponce(url);
            if (!String.IsNullOrEmpty(data))
            {
                //				Debug.WriteLine(data);
                var a = new Auri(data);
                var Title = trimlr(a["name"]);
                var Description = trimlr(a["body"]);
                // cosmetics
                Description = Description.Replace(":memo:", "📝").Replace(":gem:", "💎").Replace(":bug:", "🐛")
                                            .Replace(":speech_balloon:", "💬").Replace(":rocket:", "🚀");
                Description = Regex.Unescape(Description);
                var Version = trimlr(a["tag_name"]);
                var aa = new Auri(a["assets"]);
                var Lindex = "0";
                var Commit = "";
                if (beta)
                {
                    Lindex = "7";
                    Commit = Regex.Match(Title, @".*\[([a-fA-F0-9]{7})\]").Groups[1].Value;
                }
                var Link = trimlr(new Auri(aa["^" + Lindex])["browser_download_url"]);
                Debug.WriteLine(Title);
                Debug.WriteLine(Description);
                Debug.WriteLine(Version);
                Debug.WriteLine(Commit);
                Debug.WriteLine(Link);
                Info[0] = Title;
                Info[1] = UnescapeUnicode(Description);
                Info[2] = Version;
                Info[3] = Link;
                if (!String.IsNullOrEmpty(Commit))
                    Info[4] = Commit;
                Logging.Log("Check for updates succeded, GitHub " + (beta ? ("commit:" + Commit) : ("version: " + Version)) + ".");
            }
            else
            {
                Logging.Log("Check for updates failed, error above.", 1);
                Info = new string[]{
                        Program.Lang[Languages.Element.Error],
                        Program.Lang[Languages.Element.NetError],
                        Program.Lang[Languages.Element.Error],
                        Program.Lang[Languages.Element.Error],
                        Program.Lang[Languages.Element.Error]
                };
            }
            UpdInfo = Info;
        }
        /// <summary>
        /// Creates proxy from proxy controls(server/name/pass) text.
        /// </summary>
        /// <returns>WebProxy</returns>
        WebProxy MakeProxy()
        {
            Logging.Log("Creating proxy...");
            var myProxy = new WebProxy();
            try
            {
                var newUri = new Uri("http://" + txt_ProxyServerPort.Text);
                Logging.Log("Proxy is " + newUri + ", port is " + newUri.Port + ".");
                myProxy.Address = newUri;
            }
            catch
            {
                //				grb_ProxyConfig.Text = Program.UI[51];
                tmr.Interval = 3000;
                tmr.Tick += (___, ____) =>
                {
                    grb_ProxyConfig.Text = "Proxy configuration";
                    tmr.Stop();
                };
                tmr.Start();
            }
            if (!String.IsNullOrEmpty(txt_ProxyLogin.Text) || !String.IsNullOrEmpty(txt_ProxyPassword.Text))
                myProxy.Credentials = new NetworkCredential(txt_ProxyLogin.Text, txt_ProxyPassword.Text);
            return myProxy;
        }
        /// <summary>
        /// Check for updates at FaineSwitcher startup.
        /// </summary>
        public void StartupCheck()
        {
            Logging.Log("Startup check for updates.");
            var update_delay = Program.MyConfs.ReadInt("Updates", "Delay");
            if (update_delay <= 0) update_delay = 1;
            Logging.Log("[UPD] > Delaying updates by " + update_delay + "s.");
            System.Threading.Thread.Sleep(update_delay * 1000);
            System.Threading.Tasks.Task.Factory.StartNew(GetUpdateInfo).Wait();
            SetUInfo();
            bool silent = Program.MyConfs.ReadBool("Functions", "SilentUpdate");
            Debug.WriteLine(UpdInfo[2]);
            try
            {
                if ((UpdInfo[2] == "latest-commit" || Program.MyConfs.Read("Updates", "Channel") != "Stable") ?
                    Program.MyConfs.Read("Updates", "LatestCommit") != UpdInfo[4] :
                    flVersion("v" + Application.ProductVersion) < flVersion(UpdInfo[2]))
                {
                    Logging.Log("New version available, " + (!silent ? "showing dialog..." : "silent updating..."));
                    if (silent)
                        AtUpdateShow = 1;
                    else
                    {
                        if (UpdInfo[0] != Program.Lang[Languages.Element.Error])
                        {
                            var fx = new Form() { TopMost = false, Visible = false };
                            if (MessageBox.Show(fx, UpdInfo[1].Substring(0, ((UpdInfo[1].Length > 640) ? 640 : UpdInfo[1].Length)) + "...\n" + UpdInfo[3], UpdInfo[0],
                                     MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                            {
                                AtUpdateShow = 1;
                            }
                            else { AtUpdateShow = 2; }
                            fx.Dispose();
                        }
                        else
                        {
                            AtUpdateShow = 3;
                        }
                    }
                }
                else
                    AtUpdateShow = 2; // No Updates
            }
            catch (Exception e)
            {
                Logging.Log("Unexpected error: \n" + e.Message + "\n" + e.StackTrace);
                AtUpdateShow = 3;
            }
        }
        /// <summary>
        /// Sets UI info controls(version/title/description) text.
        /// </summary>
        void SetUInfo()
        {
            if (Program.switcher == null) return;
            //			this.grb_SwitcherReleaseTitle.Invoke((MethodInvoker)delegate {
            Program.switcher.grb_SwitcherReleaseTitle.Text = UpdInfo[0];
            //			});
            //			this.txt_UpdateDetails.Invoke((MethodInvoker)delegate {
            Program.switcher.txt_UpdateDetails.Text = UpdInfo[1];
            //			});
            //			this.btn_DownloadUpdate.Invoke((MethodInvoker)delegate {
            Program.switcher.btn_DownloadUpdate.Text = Program.Lang[Languages.Element.DownloadUpdate]; // Restore download button text
            Program.switcher.btn_DownloadUpdate.Text = Regex.Replace(Program.switcher.btn_DownloadUpdate.Text, @"\<.+?\>", UpdInfo[2]);
            //			});
        }
        void UpdateSnippetCountLabel(string snippets, Label target, bool isSnip = true)
        {
            if (!isSnip && string.IsNullOrEmpty(snippets)) { return; }
            var snipc = GetSnippetsCount(snippets);
            target.Text = target.Text.Split(' ')[0] + " " + snipc.Item1 + ((snipc.Item2 == Color.Red) ? "?" : "") + "(#" + snipc.Item3 + ")";
            var t = HelpMeUnderstand.GetToolTip(target);
            if (t.StartsWith("ERR: "))
            {
                var fl = t.IndexOf('\n') + 1;
                var tn = t.Substring(fl, t.Length - fl);
                //				Debug.WriteLine("tn" + tn);
                HelpMeUnderstand.SetToolTip(target, tn);
            }
            if (snipc.Item4 != "")
            {
                t = HelpMeUnderstand.GetToolTip(target);
                HelpMeUnderstand.SetToolTip(target, "ERR: " + snipc.Item4 + Environment.NewLine + t);
            }
            target.ForeColor = snipc.Item2;
            if (isSnip)
                SnippetsCount = snipc.Item1;
            else
                AutoSwitchCount = snipc.Item1;
        }
        #endregion
        /// <summary>
        /// Refreshes language.
        /// </summary>
        void RefreshLanguage()
        {
            #region Tabs
            tab_functions.Text = Program.Lang[Languages.Element.tab_Functions];
            //tab_layouts.Text = Program.Lang[Languages.Element.tab_Layouts];
            tab_appearence.Text = Program.Lang[Languages.Element.tab_Appearence];
            tab_timings.Text = Program.Lang[Languages.Element.tab_Timings];
            tab_excluded.Text = Program.Lang[Languages.Element.tab_Excluded];
            tab_snippets.Text = Program.Lang[Languages.Element.tab_Snippets];
            tab_autoswitch.Text = Program.Lang[Languages.Element.tab_AutoSwitch];
            tab_hotkeys.Text = Program.Lang[Languages.Element.tab_Hotkeys];
            tab_updates.Text = Program.Lang[Languages.Element.tab_Updates];
            tab_LangPanel.Text = Program.Lang[Languages.Element.tab_LangPanel];
            tab_about.Text = Program.Lang[Languages.Element.tab_About];
            tab_sounds.Text = Program.Lang[Languages.Element.tab_Sounds];
            tab_translator.Text = Program.Lang[Languages.Element.tab_Translator];
            tab_sync.Text = Program.Lang[Languages.Element.tab_Sync];
            #endregion
            #region Functions
            lnk_plugin.Text = "ST3 " + Program.Lang[Languages.Element.Plugin];
            chk_OneLayoutWholeWord.Text = Program.Lang[Languages.Element.OneLayoutWholeWord];
            chk_AutoStart.Text = Program.Lang[Languages.Element.AutoStart];
            cbb_AutostartType.Items.Clear();
            cbb_AutostartType.Items.AddRange(new[] {
                                                 Program.Lang[Languages.Element.CreateShortcut],
                                                 Program.Lang[Languages.Element.CreateTask]
                                             });
            chk_TrayIcon.Text = Program.Lang[Languages.Element.TrayIcon];
            chk_CSLayoutSwitching.Text = Program.Lang[Languages.Element.ConvertSelectionLS];
            chk_ReSelect.Text = Program.Lang[Languages.Element.ReSelect];
            chk_RePress.Text = Program.Lang[Languages.Element.RePress];
            chk_AddOneSpace.Text = Program.Lang[Languages.Element.Add1Space];
            chk_Add1NL.Text = Program.Lang[Languages.Element.Add1NL];
            chk_CSLayoutSwitchingPlus.Text = Program.Lang[Languages.Element.ConvertSelectionLSPlus];
            chk_HighlightScroll.Text = Program.Lang[Languages.Element.HighlightScroll];
            chk_StartupUpdatesCheck.Text = Program.Lang[Languages.Element.UpdatesCheck];
            chk_SilentUpdate.Text = Program.Lang[Languages.Element.SilentUpdate];
            chk_Logging.Text = Program.Lang[Languages.Element.Logging];
            chk_CapsLockDTimer.Text = Program.Lang[Languages.Element.CapsTimer];
            lbl_TrayDislpayType.Text = Program.Lang[Languages.Element.DisplayInTray];
            chk_BlockHKWithCtrl.Text = Program.Lang[Languages.Element.BlockCtrlHKs];
            chk_MCDS_support.Text = Program.Lang[Languages.Element.MCDSSupport];
            chk_GuessKeyCodeFix.Text = Program.Lang[Languages.Element.GuessKeyCodeFix];
            chk_AppDataConfigs.Text = Program.Lang[Languages.Element.ConfigsInAppData];
            chk_RemapCapsLockAsF18.Text = Program.Lang[Languages.Element.RemapCapslockAsF18];
            chk_GetLayoutFromJKL.Text = Program.Lang[Languages.Element.UseJKL];
            chk_ReadOnlyNA.Text = Program.Lang[Languages.Element.ReadOnlyNA];
            chk_WriteInputHistory.Text = Program.Lang[Languages.Element.WriteInputHistory];
            lbl_BackSpaceType.Text = Program.Lang[Languages.Element.BackSpaceType];
            lnk_OpenLogs.Text = lnk_OpenConfig.Text = lnk_OpenHistory.Text = lnk_SnipOpen.Text = Program.Lang[Languages.Element.Open];
            #endregion
            #region Layouts
            //chk_SwitchBetweenLayouts.Text = Program.Lang[Languages.Element.SwitchBetween]+":";
            //chk_EmulateLS.Text = Program.Lang[Languages.Element.EmulateLS];
            //lbl_EmuType.Text = Program.Lang[Languages.Element.EmulateType];
            //chk_SpecificLS.Text = Program.Lang[Languages.Element.ChangeLayoutBy1Key];
            //grb_Layouts.Text = Program.Lang[Languages.Element.Layouts];
            //grb_Keys.Text = Program.Lang[Languages.Element.Keys];
            //chk_OneLayout.Text = Program.Lang[Languages.Element.OneLayout];
            //chk_qwertz.Text = Program.Lang[Languages.Element.QWERTZ];
            //lbl_KeysType.Text = Program.Lang[Languages.Element.KeysType];
            //cbb_SpecKeysType.Items.Clear();
            //cbb_SpecKeysType.Items.AddRange(new [] { Program.Lang[Languages.Element.SelectKeyType], Program.Lang[Languages.Element.SetHotkeyType]});
            //lbl_LCTRLLALTTempLayout.Text = Program.Lang[Languages.Element.LCTRLLALTTempLayout];
            #endregion
            #region Persistent Layout
            chk_ChangeLayoutOnlyOnce.Text = Program.Lang[Languages.Element.SwitchOnlyOnce];
            chk_OnlyOnWindowChange.Text = Program.Lang[Languages.Element.SwitchOnlyOnWindowChange];
            tab_persistent.Text = Program.Lang[Languages.Element.PersistentLayout];
            grb_PersistentLayout1.Text = Program.Lang[Languages.Element.Layout] + " 1";
            grb_PersistentLayout2.Text = Program.Lang[Languages.Element.Layout] + " 2";
            chk_PersistentLayout1Active.Text = chk_PersistentLayout2Active.Text = Program.Lang[Languages.Element.ActivatePLFP];
            lbl_PersistentLayout1Interval.Text = lbl_PersistentLayout2Interval.Text = Program.Lang[Languages.Element.CheckInterval];
            #endregion
            #region Appearence
            chk_LangTooltipMouse.Text = Program.Lang[Languages.Element.LDMouseDisplay];
            chk_LangTooltipCaret.Text = Program.Lang[Languages.Element.LDCaretDisplay];
            chk_MouseTTAlways.Text = Program.Lang[Languages.Element.Always];
            chk_LangTTCaretOnChange.Text = chk_LangTTMouseOnChange.Text = Program.Lang[Languages.Element.LDOnlyOnChange];
            lbl_Language.Text = Program.Lang[Languages.Element.Language];
            chk_LangTTDiffLayoutColors.Text = Program.Lang[Languages.Element.LDDifferentAppearence];
            grb_LangTTAppearence.Text = Program.Lang[Languages.Element.LDAppearence];
            btn_LangTTFont.Text = Program.Lang[Languages.Element.LDFont];
            lbl_LangTTForegroundColor.Text = Program.Lang[Languages.Element.LDFore];
            lbl_LangTTBackgroundColor.Text = Program.Lang[Languages.Element.LDBack];
            lbl_LangTTText.Text = Program.Lang[Languages.Element.LDText];
            grb_LangTTSize.Text = Program.Lang[Languages.Element.LDSize];
            grb_LangTTPositon.Text = Program.Lang[Languages.Element.LDPosition];
            lbl_LangTTHeight.Text = Program.Lang[Languages.Element.LDHeight];
            lbl_LangTTWidth.Text = Program.Lang[Languages.Element.LDWidth];
            chk_LangTTTransparentColor.Text = Program.Lang[Languages.Element.LDTransparentBG];
            lsb_LangTTAppearenceForList.Items.Clear();
            lsb_LangTTAppearenceForList.Items.AddRange(new[] {
                                                        Program.Lang[Languages.Element.Layout] + " 1",
                                                        Program.Lang[Languages.Element.Layout] + " 2",
                                                        Program.Lang[Languages.Element.LDAroundMouse],
                                                        Program.Lang[Languages.Element.LDAroundCaret],
                                                        "MCDS"
                                                        });
            chk_LangTTUseFlags.Text = Program.Lang[Languages.Element.UseFlags];
            chk_LangTTUpperArrow.Text = Program.Lang[Languages.Element.LDUpperArrow];
            chk_LDMessages.Text = Program.Lang[Languages.Element.LDUseWinMessages];
            #endregion
            #region Timings
            chk_CSUsePaste.Text = Program.Lang[Languages.Element.UsePasteInCS];
            lbl_LangTTMouseRefreshRate.Text = Program.Lang[Languages.Element.LDForMouseRefreshRate];
            lbl_LangTTCaretRefreshRate.Text = Program.Lang[Languages.Element.LDForCaretRefreshRate];
            lbl_DoubleHK2ndPressWaitTime.Text = Program.Lang[Languages.Element.DoubleHKDelay];
            lbl_FlagTrayRefreshRate.Text = Program.Lang[Languages.Element.TrayFlagsRefreshRate];
            lbl_ScrollLockRefreshRate.Text = Program.Lang[Languages.Element.ScrollLockRefreshRate];
            lbl_CapsLockRefreshRate.Text = Program.Lang[Languages.Element.CapsLockRefreshRate];
            chk_SelectedTextGetMoreTries.Text = Program.Lang[Languages.Element.MoreTriesToGetSelectedText];
            chk_UseDelayAfterBackspaces.Text = Program.Lang[Languages.Element.UseDelayAfterBackspaces];
            #endregion
            #region Excluded
            lbl_ExcludedPrograms.Text = Program.Lang[Languages.Element.ExcludedPrograms];
            chk_Change1KeyL.Text = Program.Lang[Languages.Element.Change1KeyLayoutInExcluded];
            chk_ConvSWL.Text = Program.Lang[Languages.Element.AllowConvertSWL];
            #endregion
            #region Snippets
            chk_Snippets.Text = Program.Lang[Languages.Element.SnippetsEnabled];
            chk_SnippetsSpaceAfter.Text = Program.Lang[Languages.Element.SnippetSpaceAfter];
            chk_SnippetsSwitchToGuessLayout.Text = Program.Lang[Languages.Element.SnippetSwitchToGuessLayout];
            lbl_SnippetsCount.Text = Program.Lang[Languages.Element.SnippetsCount];
            lbl_SnippetExpandKey.Text = Program.Lang[Languages.Element.SnippetsExpandKey];
            var sko = (SnippetsExpKeyOther == "" ? Program.Lang[Languages.Element.SnippetsExpKeyOther] : "*[" + SnippetsExpKeyOther + "]");
            if (cbb_SnippetExpandKeys.Items.Count < 3)
            {
                cbb_SnippetExpandKeys.Items.Add(sko);
            }
            else
            {
                cbb_SnippetExpandKeys.Items[2] = sko;
            }
            lbl_NCR.Text = Program.Lang[Languages.Element.SnippetsNCRules];
            #endregion
            #region AutoSwitch
            chk_AutoSwitch.Text = Program.Lang[Languages.Element.AutoSwitchEnabled];
            chk_AutoSwitchSpaceAfter.Text = Program.Lang[Languages.Element.AutoSwitchSpaceAfter];
            chk_AutoSwitchSwitchToGuessLayout.Text = Program.Lang[Languages.Element.AutoSwitchSwitchToGuessLayout];
            btn_UpdateAutoSwitchDictionary.Text = Program.Lang[Languages.Element.AutoSwitchUpdateDictionary];
            lbl_AutoSwitchDependsOnSnippets.Text = Program.Lang[Languages.Element.AutoSwitchDependsOnSnippets];
            if (lbl_AutoSwitchWordsCount.Text.Contains(" "))
            {
                var t = lbl_AutoSwitchWordsCount.Text.Split(new[] { ' ' }, 2);
                lbl_AutoSwitchWordsCount.Text = Program.Lang[Languages.Element.AutoSwitchDictionaryWordsCount] + t[1];
            }
            else
                lbl_AutoSwitchWordsCount.Text = Program.Lang[Languages.Element.AutoSwitchDictionaryWordsCount];
            chk_DownloadASD_InZip.Text = Program.Lang[Languages.Element.DownloadAutoSwitchDictionaryInZip];
            #endregion
            #region Hotkeys
            grb_Hotkey.Text = Program.Lang[Languages.Element.Hotkey];
            chk_HotKeyEnabled.Text = Program.Lang[Languages.Element.Enabled];
            chk_DoubleHotkey.Text = Program.Lang[Languages.Element.DoubleHK];
            lsb_Hotkeys.Items.Clear();
            lsb_Hotkeys.Items.AddRange(new[]{
                                        Program.Lang[Languages.Element.ToggleMainWnd],
                                        Program.Lang[Languages.Element.ConvertLast],
                                        Program.Lang[Languages.Element.ConvertSelected],
                                        Program.Lang[Languages.Element.ConvertLine],
                                        Program.Lang[Languages.Element.ConvertWords],
                                        Program.Lang[Languages.Element.ToggleSymbolIgnore],
                                        Program.Lang[Languages.Element.SelectedToTitleCase],
                                        Program.Lang[Languages.Element.SelectedToRandomCase],
                                        Program.Lang[Languages.Element.SelectedToSwapCase],
                                        Program.Lang[Languages.Element.SelectedToUpperCase],
                                        Program.Lang[Languages.Element.SelectedToLowerCase],
                                        Program.Lang[Languages.Element.SelectedTransliteration],
                                        Program.Lang[Languages.Element.ExitSwitcher],
                                        Program.Lang[Languages.Element.RestartSwitcher],
                                        Program.Lang[Languages.Element.ToggleLangPanel],
                                        Program.Lang[Languages.Element.TranslateSelection],
                                        Program.Lang[Languages.Element.ToggleSwitcher],
                                        Program.Lang[Languages.Element.CycleCase],
                                        Program.Lang[Languages.Element.CustomConversion],
                                        Program.Lang[Languages.Element.ShowCMenuUnderMouse]
                                        });
            #endregion
            #region LangPanel/TranslatePanel
            chk_DisplayLangPanel.Text = Program.Lang[Languages.Element.DisplayLangPanel];
            lbl_LPRefreshRate.Text = Program.Lang[Languages.Element.RefreshRate];
            lbl_TrTransparency.Text = lbl_LPTrasparency.Text = Program.Lang[Languages.Element.Transparency];
            lbl_TrBorderC.Text = lbl_LPBorderColor.Text = Program.Lang[Languages.Element.BorderColor];
            lbl_TrFG.Text = lbl_LPFore.Text = Program.Lang[Languages.Element.LDFore];
            lbl_TrBG.Text = lbl_LPBack.Text = Program.Lang[Languages.Element.LDBack];
            chk_TrUseAccent.Text = chk_LPAeroColor.Text = Program.Lang[Languages.Element.UseAeroColor];
            lbl_LPFont.Text = Program.Lang[Languages.Element.LDFont] + ":";
            btn_LPFont.Text = Program.Lang[Languages.Element.LDFont];
            chk_LPUpperArrow.Text = Program.Lang[Languages.Element.DisplayUpperArrow];
            lbl_TrMethod.Text = Program.Lang[Languages.Element.Method] + ":";
            #endregion
            #region TranslatePanel
            chk_TrEnable.Text = "Disable by developer"; //Program.Lang[Languages.Element.EnableTranslatePanel];
            chk_TrOnDoubleClick.Text = Program.Lang[Languages.Element.ShowTranslationOnDoubleClick];
            lbl_TrLanguages.Text = Program.Lang[Languages.Element.TranslateLanguages];
            lbl_TrTextFont.Text = Program.Lang[Languages.Element.TextFont];
            lbl_TrTitleFont.Text = Program.Lang[Languages.Element.TitleFont];
            btn_TrTitleFont.Text = btn_TrTextFont.Text = Program.Lang[Languages.Element.LDFont];
            chk_TrTranscription.Text = Program.Lang[Languages.Element.Transcription];
            #endregion
            #region Updtaes
            btn_CheckForUpdates.Text = Program.Lang[Languages.Element.CheckForUpdates];
            btn_DownloadUpdate.Text = Program.Lang[Languages.Element.UpdateSwitcher];
            grb_DownloadUpdate.Text = Program.Lang[Languages.Element.DownloadUpdate];
            grb_ProxyConfig.Text = Program.Lang[Languages.Element.ProxyConfig];
            lbl_ProxyServerPort.Text = Program.Lang[Languages.Element.ProxyServer];
            lbl_ProxyLogin.Text = Program.Lang[Languages.Element.ProxyLogin];
            lbl_ProxyPassword.Text = Program.Lang[Languages.Element.ProxyPass];
            lbl_UpdateChannel.Text = Program.Lang[Languages.Element.UpdatesChannel];
            #endregion
            #region About
            btn_DebugInfo.Text = Program.Lang[Languages.Element.DbgInf];
            lnk_Site.Text = Program.Lang[Languages.Element.Site];
            lnk_Releases.Text = Program.Lang[Languages.Element.Releases];
            txt_Help.Text = Program.Lang[Languages.Element.Switcher] + "\r\n" + Program.Lang[Languages.Element.About];
            #endregion
            #region Sync
            grb_backup.Text = btn_backup.Text = Program.Lang[Languages.Element.Backup];
            grb_restore.Text = btn_restore.Text = Program.Lang[Languages.Element.Restore];
            #endregion
            #region Sounds
            chk_EnableSnd.Text = Program.Lang[Languages.Element.EnableSounds];
            grb_Sound1.Text = Program.Lang[Languages.Element.Sound] + " #1";
            grb_Sound2.Text = Program.Lang[Languages.Element.Sound] + " #2";
            grb_SoundOn2.Text = grb_SoundOn.Text = Program.Lang[Languages.Element.PlaySoundWhen];
            chk_SndAutoSwitch2.Text = chk_SndAutoSwitch.Text = Program.Lang[Languages.Element.SoundOnAutoSwitch];
            chk_SndSnippets2.Text = chk_SndSnippets.Text = Program.Lang[Languages.Element.SoundOnSnippets];
            chk_SndLast2.Text = chk_SndLast.Text = Program.Lang[Languages.Element.SoundOnConvertLast];
            chk_SndLayoutSwitch2.Text = chk_SndLayoutSwitch.Text = Program.Lang[Languages.Element.SoundOnLayoutSwitching];
            chk_UseCustomSnd2.Text = chk_UseCustomSnd.Text = Program.Lang[Languages.Element.UseCustomSound];
            btn_SelectSnd2.Text = btn_SelectSnd.Text = Program.Lang[Languages.Element.Select];
            #endregion
            #region Buttons
            btn_Apply.Text = Program.Lang[Languages.Element.ButtonApply];
            btn_Cancel.Text = Program.Lang[Languages.Element.ButtonCancel];
            btn_OK.Text = Program.Lang[Languages.Element.ButtonOK];
            #endregion
            #region Misc
            icon.RefreshText(Program.Lang[Languages.Element.Switcher], Program.Lang[Languages.Element.ShowHide],
                             Program.Lang[Languages.Element.ExitSwitcher], Program.Lang[Languages.Element.Enable],
                             Program.Lang[Languages.Element.RestartSwitcher], Program.Lang[Languages.Element.Convert],
                             Program.Lang[Languages.Element.Transliterate], Program.Lang[Languages.Element.Clipboard],
                             Program.Lang[Languages.Element.Latest], Program.Lang[Languages.Element.ChangeLayout]);
            #endregion
            Logging.Log("Language changed.");
            SetTooltips();
        }
        class MTheme
        {
            public Color BG;
            public Color FG;
            public Color TAB_BORDERS;
            public Color TAB_FOCUS_BG;
        }
        void ToggleDark(bool yes)
        {
            var BGDARK = Color.FromArgb(51, 54, 58);
            var FGDARK = Color.FromArgb(181, 181, 181);
            if (yes)
            {
                for (int ii = 0; ii != this.Controls.Count; ii++)
                {
                    this.Controls[ii].BackColor = BGDARK;
                    this.Controls[ii].ForeColor = FGDARK;
                }
                this.BackColor = tabs.BG = lsb_Hotkeys.BackColor = lsb_LangTTAppearenceForList.BackColor = BGDARK;
                this.ForeColor = tabs.FG = lsb_Hotkeys.ForeColor = lsb_LangTTAppearenceForList.ForeColor = FGDARK;
                tabs.TAB_BORDERS = Color.DarkSlateGray;
                tabs.TAB_FOCUS_BG = Color.Black;
                for (int i = 0; i != tabs.TabPages.Count; i++)
                {
                    if (tabs.TabPages[i].Text == "[Hidden]") continue;
                    tabs.TabPages[i].BackColor = BGDARK;
                    tabs.TabPages[i].ForeColor = FGDARK;
                    for (int ii = 0; ii != tabs.TabPages[i].Controls.Count; ii++)
                    {
                        if (tabs.TabPages[i].Controls[ii] is TextBoxCA || tabs.TabPages[i].Controls[ii] is TextBox)
                        {
                            tabs.TabPages[i].Controls[ii].BackColor = Color.Black;

                        }
                        else
                        {
                            tabs.TabPages[i].Controls[ii].BackColor = BGDARK;
                        }
                        tabs.TabPages[i].Controls[ii].ForeColor = FGDARK;
                    }
                }
            }
            else
            {
                for (int ii = 0; ii != this.Controls.Count; ii++)
                {
                    this.Controls[ii].BackColor = SystemColors.Control;
                    this.Controls[ii].ForeColor = SystemColors.WindowText;
                }
                this.BackColor = tabs.BG = lsb_Hotkeys.BackColor = lsb_LangTTAppearenceForList.BackColor = SystemColors.Control;
                this.ForeColor = tabs.FG = lsb_Hotkeys.ForeColor = lsb_LangTTAppearenceForList.ForeColor = SystemColors.WindowText;
                tabs.TAB_BORDERS = SystemColors.ControlLight;
                tabs.TAB_FOCUS_BG = SystemColors.Window;
                for (int i = 0; i != tabs.TabPages.Count; i++)
                {
                    if (tabs.TabPages[i].Text == "[Hidden]") continue;
                    tabs.TabPages[i].BackColor = SystemColors.Control;
                    tabs.TabPages[i].ForeColor = SystemColors.WindowText;
                    for (int ii = 0; ii != tabs.TabPages[i].Controls.Count; ii++)
                    {
                        if (tabs.TabPages[i].Controls[ii] is TextBoxCA || tabs.TabPages[i].Controls[ii] is TextBox)
                        {
                            tabs.TabPages[i].Controls[ii].BackColor = SystemColors.Window;

                        }
                        else
                        {
                            tabs.TabPages[i].Controls[ii].BackColor = SystemColors.Control;
                        }
                        tabs.TabPages[i].Controls[ii].ForeColor = SystemColors.WindowText;
                    }
                }
            }
        }
        #region Colored TabControl
        public class TabControlC : TabControl
        {
            public Color BG = SystemColors.Control;
            public Color FG = SystemColors.WindowText;
            public Color TAB_BORDERS = SystemColors.ControlLight;
            public Color TAB_FOCUS_BG = SystemColors.Window;
            public TabControlC()
            {
                this.DrawItem += DrawItemHandler;
            }
            Dictionary<int, DrawItemEventArgs> ItemArgs = new Dictionary<int, DrawItemEventArgs>();
            Dictionary<int, string> ItemTexts = new Dictionary<int, string>();
            void DrawItemHandler(object sender, DrawItemEventArgs e)
            {
                if (!ItemArgs.ContainsKey(e.Index))
                    ItemArgs.Add(e.Index, e);
                else
                    ItemArgs[e.Index] = e;
                if (!ItemTexts.ContainsKey(e.Index))
                    ItemTexts.Add(e.Index, (sender as TabControlC).TabPages[e.Index].Text);
                else
                    ItemTexts[e.Index] = (sender as TabControlC).TabPages[e.Index].Text;
            }
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == (int)WinAPI.WM_PAINT)
                {
                    using (var g = this.CreateGraphics())
                    {
                        //Double buffering stuff...
                        BufferedGraphicsContext currentContext;
                        BufferedGraphics myBuffer;
                        currentContext = BufferedGraphicsManager.Current;
                        myBuffer = currentContext.Allocate(g,
                           this.ClientRectangle);
                        Rectangle r = ClientRectangle;

                        //Painting background
                        if (Enabled)
                            myBuffer.Graphics.FillRectangle(new SolidBrush(BG), r);
                        else
                            myBuffer.Graphics.FillRectangle(Brushes.LightGray, r);

                        //Painting border
                        r.Height = this.DisplayRectangle.Height + 1; //Using display rectangle hight because it excludes the tab headers already
                        r.Y = this.DisplayRectangle.Y - 1; //Same for Y coordinate
                        r.Width -= 5;
                        r.X += 1;

                        if (Enabled)
                            myBuffer.Graphics.DrawRectangle(new Pen(Color.FromArgb(255, 133, 158, 191), 1), r);
                        else
                            myBuffer.Graphics.DrawRectangle(Pens.DarkGray, r);

                        for (int ii = 0; ii != ItemArgs.Count; ii++)
                        {
                            var i = ItemArgs[ii];
                            var t = ItemTexts[ii];
                            Debug.WriteLine(i.Bounds);
                            //		                	CustomDrawItem(ItemArgs[i], ItemTexts[i]);
                            myBuffer.Graphics.DrawRectangle(new Pen(TAB_BORDERS), i.Bounds.X, i.Bounds.Y, i.Bounds.Width, i.Bounds.Height);
                            var yal = i.Bounds.Y;
                            var xal = i.Bounds.X;
                            if (i.Bounds.Height == 24)
                            { // Assume that is focused tab
                                yal += 4;
                                xal += 6;
                                myBuffer.Graphics.FillRectangle(new SolidBrush(TAB_FOCUS_BG), i.Bounds.X + 1, i.Bounds.Y + 1, i.Bounds.Width - 2, i.Bounds.Height - 2);
                            }
                            myBuffer.Graphics.DrawString(t, i.Font, new SolidBrush(FG), xal, yal);
                        }

                        myBuffer.Render();
                        myBuffer.Dispose();
                    }
                }
            }
        }
        #endregion
        #region Textbox + Ctrl+A
        public class TextBoxCA : TextBox
        {
            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
            {
                var keyCode = (Keys)(msg.WParam.ToInt32() & Convert.ToInt32(Keys.KeyCode));
                if ((msg.Msg == WinAPI.WM_KEYDOWN && keyCode == Keys.A) &&
                    (ModifierKeys == Keys.Control) && this.Focused)
                {
                    this.SelectAll();
                    return true;
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }
        #endregion
        #region Tooltips
        void SetTooltips()
        {
            HelpMeUnderstand.SetToolTip(chk_CSLayoutSwitching, Program.Lang[Languages.Element.TT_ConvertSelectionSwitch]);
            HelpMeUnderstand.SetToolTip(chk_ReSelect, Program.Lang[Languages.Element.TT_ReSelect]);
            HelpMeUnderstand.SetToolTip(chk_RePress, Program.Lang[Languages.Element.TT_RePress]);
            HelpMeUnderstand.SetToolTip(chk_AddOneSpace, Program.Lang[Languages.Element.TT_Add1Space]);
            HelpMeUnderstand.SetToolTip(chk_Add1NL, Program.Lang[Languages.Element.TT_Add1NL]);
            HelpMeUnderstand.SetToolTip(chk_CSLayoutSwitchingPlus, Program.Lang[Languages.Element.TT_ConvertSelectionSwitchPlus]);
            HelpMeUnderstand.SetToolTip(chk_HighlightScroll, Program.Lang[Languages.Element.TT_ScrollTip]);
            HelpMeUnderstand.SetToolTip(chk_Logging, Program.Lang[Languages.Element.TT_Logging]);
            HelpMeUnderstand.SetToolTip(chk_CapsLockDTimer, Program.Lang[Languages.Element.TT_CapsDis]);
            HelpMeUnderstand.SetToolTip(cbb_TrayDislpayType, Program.Lang[Languages.Element.TT_TrayDisplayType]);
            HelpMeUnderstand.SetToolTip(lbl_TrayDislpayType, Program.Lang[Languages.Element.TT_TrayDisplayType]);
            HelpMeUnderstand.SetToolTip(chk_BlockHKWithCtrl, Program.Lang[Languages.Element.TT_BlockCtrl]);
            HelpMeUnderstand.SetToolTip(chk_MCDS_support, Program.Lang[Languages.Element.TT_MCDSSupport]);
            HelpMeUnderstand.SetToolTip(chk_OneLayoutWholeWord, Program.Lang[Languages.Element.TT_OneLayoutWholeWordCS]);
            //HelpMeUnderstand.SetToolTip(chk_SwitchBetweenLayouts, Program.Lang[Languages.Element.TT_SwitchBetween]);
            //HelpMeUnderstand.SetToolTip(chk_EmulateLS, Program.Lang[Languages.Element.TT_EmulateLS]);
            HelpMeUnderstand.SetToolTip(chk_LangTooltipCaret, Program.Lang[Languages.Element.TT_LDForCaret]);
            HelpMeUnderstand.SetToolTip(chk_LangTooltipMouse, Program.Lang[Languages.Element.TT_LDForMouse]);
            HelpMeUnderstand.SetToolTip(chk_LangTTCaretOnChange, Program.Lang[Languages.Element.TT_LDOnlyOnChange]);
            HelpMeUnderstand.SetToolTip(chk_LangTTMouseOnChange, Program.Lang[Languages.Element.TT_LDOnlyOnChange]);
            HelpMeUnderstand.SetToolTip(txt_LangTTText, Program.Lang[Languages.Element.TT_LDText]);
            HelpMeUnderstand.SetToolTip(chk_LangTTDiffLayoutColors, Program.Lang[Languages.Element.TT_LDDifferentAppearence]);
            HelpMeUnderstand.SetToolTip(chk_Snippets, Program.Lang[Languages.Element.TT_Snippets]);
            HelpMeUnderstand.SetToolTip(lbl_ExcludedPrograms, Program.Lang[Languages.Element.TT_ExcludedPrograms]);
            HelpMeUnderstand.SetToolTip(txt_ExcludedPrograms, Program.Lang[Languages.Element.TT_ExcludedPrograms]);
            HelpMeUnderstand.SetToolTip(txt_PersistentLayout1Processes, Program.Lang[Languages.Element.TT_PersistentLayout]);
            HelpMeUnderstand.SetToolTip(txt_PersistentLayout2Processes, Program.Lang[Languages.Element.TT_PersistentLayout]);
            //HelpMeUnderstand.SetToolTip(chk_OneLayout, Program.Lang[Languages.Element.TT_OneLayout]);
            //HelpMeUnderstand.SetToolTip(chk_qwertz, Program.Lang[Languages.Element.TT_QWERTZ]);
            HelpMeUnderstand.SetToolTip(chk_Change1KeyL, Program.Lang[Languages.Element.TT_Change1KeyLayoutInExcluded]);
            HelpMeUnderstand.SetToolTip(chk_ConvSWL, Program.Lang[Languages.Element.TT_AllowConvertSWL]);
            HelpMeUnderstand.SetToolTip(chk_SnippetsSwitchToGuessLayout, Program.Lang[Languages.Element.TT_SnippetsSwitchToGuessLayout]);
            HelpMeUnderstand.SetToolTip(lbl_SnippetsCount, Program.Lang[Languages.Element.TT_SnippetsCount]);
            HelpMeUnderstand.SetToolTip(lbl_AutoSwitchWordsCount, Program.Lang[Languages.Element.TT_SnippetsCount]);
            HelpMeUnderstand.SetToolTip(chk_GuessKeyCodeFix, Program.Lang[Languages.Element.TT_GuessKeyCodeFix]);
            HelpMeUnderstand.SetToolTip(chk_AppDataConfigs, Program.Lang[Languages.Element.TT_ConfigsInAppData]);
            //HelpMeUnderstand.SetToolTip(lbl_KeysType, Program.Lang[Languages.Element.TT_KeysType]);
            //HelpMeUnderstand.SetToolTip(cbb_SpecKeysType, Program.Lang[Languages.Element.TT_KeysType]);
            HelpMeUnderstand.SetToolTip(lbl_SnippetExpandKey, Program.Lang[Languages.Element.TT_SnippetExpandKey]);
            HelpMeUnderstand.SetToolTip(cbb_SnippetExpandKeys, Program.Lang[Languages.Element.TT_SnippetExpandKey]);
            HelpMeUnderstand.SetToolTip(chk_LDMessages, Program.Lang[Languages.Element.TT_LDUseWinMessages]);
            HelpMeUnderstand.SetToolTip(chk_RemapCapsLockAsF18, Program.Lang[Languages.Element.TT_RemapCapslockAsF18]);
            HelpMeUnderstand.SetToolTip(chk_OnlyOnWindowChange, Program.Lang[Languages.Element.TT_SwitchOnlyOnWindowChange]);
            HelpMeUnderstand.SetToolTip(chk_ChangeLayoutOnlyOnce, Program.Lang[Languages.Element.TT_SwitchOnlyOnce]);
            HelpMeUnderstand.SetToolTip(chk_UseDelayAfterBackspaces, Program.Lang[Languages.Element.TT_UseDelayAfterBackspaces]);
            HelpMeUnderstand.SetToolTip(chk_GetLayoutFromJKL, Program.Lang[Languages.Element.TT_UseJKL]);
            HelpMeUnderstand.SetToolTip(chk_ReadOnlyNA, Program.Lang[Languages.Element.TT_ReadOnlyNA]);
            HelpMeUnderstand.SetToolTip(chk_WriteInputHistory, Program.Lang[Languages.Element.TT_WriteInputHistory]);
            HelpMeUnderstand.SetToolTip(lnk_OpenLogs, Program.Lang[Languages.Element.TT_LeftRightMB] + "\n" + Logging.log);
            HelpMeUnderstand.SetToolTip(lnk_OpenHistory, Program.Lang[Languages.Element.TT_LeftRightMB] + "\n" + Path.Combine(nPath, "history.txt"));
            HelpMeUnderstand.SetToolTip(lnk_OpenConfig, Program.Lang[Languages.Element.TT_LeftRightMB] + "\n" + Configs.filePath);
            HelpMeUnderstand.SetToolTip(chk_TrTranscription, Program.Lang[Languages.Element.TT_Transcription_1] +
                                        Program.Lang[Languages.Element.DirectV2] + Program.Lang[Languages.Element.TT_Transcription_2]);
            HelpMeUnderstand.SetToolTip(txt_Snippets, Program.Lang[Languages.Element.TT_SnippetsEditHotkeys]);
            //HelpMeUnderstand.SetToolTip(lbl_LCTRLLALTTempLayout, Program.Lang[Languages.Element.TT_LCTRLLALTTempLayout]);
        }
        void HelpMeUnderstandPopup(object sender, PopupEventArgs e)
        {
            HelpMeUnderstand.ToolTipTitle = e.AssociatedControl.Text;
        }
        #endregion
        /// <summary>
        /// Converts FaineSwitcher version string to float.
        /// </summary>
        /// <param name="ver">FaineSwitcher version string.</param>
        /// <returns>float</returns>
        public static float flVersion(string ver)
        {
            var justdigs = Regex.Replace(ver, "\\D", "");
            float fl = 0.0f;
            if (justdigs.Length > 2)
            {
                var strfl = justdigs[0] + "." + justdigs.Substring(1);
                float.TryParse(strfl, out fl);
            }
            return fl;
        }
        #endregion
        #region Links
        static void __lopen(string file, string type, bool dir = false, bool copy = false)
        {
            if (copy)
            {
                KMHook.RestoreClipBoard(file);
                ShowTooltip(Program.Lang[Languages.Element.DbgInf_Copied] + "\r\n" + file, 800);
                return;
            }
            string fORd = dir ? Path.GetDirectoryName(file) : file;
            try
            {
                Process.Start(fORd);
            }
            catch (Exception ex) { Logging.Log("No program to open " + type + ", opening skiped. Details:\r\n" + ex.Message + "\r\n" + ex.StackTrace, 2); }
        }
        void Lnk_OpenHistoryClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var path = Path.Combine(nPath, "history.txt");
            if (WriteInputHistoryByDate)
                path = KMHook.GetHistoryByDatePath();
            __lopen(path, "txt", e.Button == MouseButtons.Right);
        }
        void Lnk_OpenConfigClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            __lopen(Path.Combine(nPath, "FaineSwitcher.ini"), "ini", e.Button == MouseButtons.Right);
        }
        void Lnk_OpenLogsClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            __lopen(Logging.log, "txt", e.Button == MouseButtons.Right);
        }
        void Lnk_RepositoryLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          //  __lopen("http://github.com/BladeMight/FaineSwitcher", "http", false, e.Button == MouseButtons.Right);
        }
        void Lnk_SiteLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           // __lopen("http://blademight.github.io/FaineSwitcher/", "http", false, e.Button == MouseButtons.Right);
        }
        void Lnk_WikiLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          //  __lopen("http://github.com/BladeMight/FaineSwitcher/wiki", "http", false, e.Button == MouseButtons.Right);
        }
        void Lnk_ReleasesLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          //  __lopen("http://github.com/BladeMight/FaineSwitcher/releases", "http", false, e.Button == MouseButtons.Right);
        }
        void Lnk_EmailLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           // __lopen("mailto:BladeMight@gmail.com", "mailto", false, e.Button == MouseButtons.Right);
        }
        void Lnk_pluginLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           // __lopen("http://github.com/BladeMight/SwitcherCaretDisplayServer", "http", false, e.Button == MouseButtons.Right);
        }
        void Lnk_SnipOpenLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            __lopen(snipfile, "txt");
            reload_snip = true;
        }
        #endregion
        #region Custom Context Menu
        //		static string getargtype(string arg) {
        //			arg = arg.ToLower();
        //			if (arg.StartsWith("http"))
        //				return "http";
        //			if (arg.StartsWith("mailto"))
        //				return "mailto";
        //			if (File.Exists(arg)) {
        //				var m = Regex.Match(arg, @".*\.(.*)");
        //				if (m.Groups.Count >0) {
        //					return m.Groups[1].Value;
        //				}
        //			}
        //			return "unknown";
        //		}
        static string replaceenv(string arg, string env, Func<string> getvalue)
        {
            if (arg.Contains(env))
                arg = arg.Replace(env, getvalue());
            return arg;
        }
        static string expandmenuarg(string arg)
        {
            arg = replaceenv(arg, "%clipboard%", () => KMHook.GetClipboard());
            arg = replaceenv(arg, "%clipboard_url%", () => HttpUtility.UrlEncode(KMHook.GetClipboard()));
            arg = replaceenv(arg, "%ini%", () => Configs.filePath);
            arg = replaceenv(arg, "%Switcher_dir%", () => nPath);
            return arg;
        }
        /// <summary><br/>
        /// 0 = layoutchange<br/>
        /// 1 = layoutchangedfrom
        /// </summary>
        public static string[] bindable_events = { "layoutchange", "layoutchangedfrom" };
        static void menuhandle(string act, string arg)
        {
            act = act.ToLower();
            arg = expandmenuarg(arg);
            if (act == "url")
            {
                //				var type = getargtype(arg);
                string args = "", prog = arg;
                if (arg.Contains(" "))
                {
                    var m = arg.Split(new[] { ' ' }, 2);
                    prog = m[0]; args = m[1];
                    if (arg[0] == '"')
                    {
                        var quot = arg.Substring(1, arg.Length - 1).IndexOf('"');
                        if (quot != -1)
                        {
                            prog = arg.Substring(1, quot);
                            args = arg.Substring(quot + 2, arg.Length - 2 - quot);
                            Debug.WriteLine("Quoted program: " + prog + " args: " + args);
                        }
                    }
                }
                try
                {
                    var re = new Regex(@"^([A-Za-z]:\\.*)\\");
                    var pi = new ProcessStartInfo();
                    pi.FileName = prog;
                    pi.Arguments = args;
                    var m = re.Matches(prog);
                    if (m.Count > 0)
                    {
                        pi.WorkingDirectory = m[0].Groups[0].Value;
                        Debug.WriteLine("SerWorkingDirectory: " + pi.WorkingDirectory);
                    }
                    Process.Start(pi);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "\n" + arg, "FaineSwitcher.mm => " + Program.Lang[Languages.Element.Error], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (act == "multi")
            {
                var s = arg.Split('|');
                //				var by2 = (s.Length-1)%2 == 0;
                //				if (by2) {
                for (int i = 0; i < s.Length - 1; i += 2)
                {
                    if (s[i].ToLower() == "multi") continue;
                    if (!multi_continue)
                    {
                        Debug.WriteLine("Multi_continue break.");
                        multi_continue = true;
                        hk_result = false;
                        break;
                    }
                    //						MessageBox.Show("multi"+i+" " + s[i] + " => " +s[i+1]);
                    menuhandle(s[i], s[i + 1]);
                }
                //				}
            }
            else if (act == "kbd")
            {
                //				MessageBox.Show("KBD: " + arg);
                KMHook.SendModsUp(15);
                KMHook.SimKeyboard(arg);
            }
            else if (act == "hk")
            {
                if (arg.Length > 1)
                {
                    arg = arg.ToLower();
                    int x = -1;
                    Int32.TryParse(arg[1].ToString(), out x);
                    if (x >= 0)
                    {
                        if (arg[0] == 's')
                        {
                            SwitcherUI.hk_result = false;
                            KMHook.SendModsUp(15);
                            if (x <= 6)
                                KMHook.SelectionConversion((KMHook.ConvT)x);
                            else
                            {
                                if (x == 7) KMHook.ConvertSelection();
                                if (x == 8) Program.switcher.CycleCase();
                                if (x == 9) ShowSelectionTranslation();
                            }
                        }
                        if (arg[0] == 'c')
                        {
                            SwitcherUI.hk_result = false;
                            KMHook.SendModsUp(15);
                            if (x == 0) KMHook.ConvertLast(Program.c_word);
                            if (x == 1) ConvertLastLine();
                            if (x == 2) Program.switcher.PrepareConvertMoreWords();
                        }
                        if (arg[0] == 'h')
                        {
                            if (x == 0) Program.switcher.ToggleVisibility();
                            if (x == 1) Program.switcher.Restart();
                            if (x == 2) Program.switcher.ExitProgram();
                            if (x == 3) ToggleSymIgn();
                            if (x == 4) Program.switcher.ToggleLangPanel();
                            if (x == 5) Program.switcher.ToggleSwitcher();
                            if (x == 6) Program.switcher.ShowContextMenuUnderMouse();
                        }
                    }
                }
            }
            else if (act == "ifhk")
            {
                if (arg.ToLower().StartsWith("y"))
                {
                    multi_continue = hk_result;
                }
                else
                {
                    Debug.WriteLine("Not IF!");
                    multi_continue = !hk_result;
                }
                Debug.WriteLine("Multi_continue set: " + multi_continue);
            }
            else if (act == "evt")
            {
                Debug.WriteLine("Event-bind: " + arg);
                var argx = arg.Split(new[] { '_' }, 3);
                argx[0] = argx[0].ToLower();
                foreach (var evt in bindable_events)
                {
                    if (Regex.Replace(argx[0], "\\d+", "") == evt)
                    {
                        event_bindings.Add(argx[0], () => menuhandle(argx[1], argx[2]));
                    }
                }
            }
            else if (act == "paste")
            {
                var cl = NativeClipboard.GetText();
                if (string.IsNullOrEmpty(cl))
                {
                    cl = NativeClipboard.GetText(WinAPI.CF_HTMLFORMAT, false);
                    var st = "<!--StartFragment-->";
                    var s = cl.IndexOf(st) + st.Length;
                    var e = cl.IndexOf("<!--EndFragment-->");
                    cl = cl.Substring(s, e - s);
                }
                if (!string.IsNullOrEmpty(cl))
                {
                    if (SwitcherUI.ClipBackOnlyText)
                    {
                        KMHook.lastClipText = NativeClipboard.GetText();
                    }
                    else
                    {
                        KMHook.lastClip = NativeClipboard.clip_get();
                    }
                    KMHook.SendModsUp(15);
                    KMHook.PasteText(cl);
                    KMHook.RestoreClipBoard();
                }
            }
            else if (act == "snipex")
            {
                var expr = "";
                foreach (var snex in KMHook.expressions)
                {
                    if (arg.Contains(snex.ToLower()))
                    {
                        expr = snex.ToLower();
                        break;
                    }
                }
                if (expr != "")
                {
                    if (arg.EndsWith(")", StringComparison.InvariantCulture) &&
                        arg.StartsWith(expr + "(", StringComparison.InvariantCulture))
                    {
                        var argn = arg.Substring(expr.Length + 1, arg.Length - expr.Length - 2);
                        KMHook.ExecExpression(expr, argn);
                    }
                    else
                    {
                        Logging.Log("Expression: " + arg + " missing ( or )", 2);
                    }
                }
                else
                {
                    Logging.Log("Unknown expression called: " + arg, 2);
                }
            }
            else
            {
                MessageBox.Show("Unknown action: " + act, "No such action", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        public static DICT<string, Tuple<Action, string>> tray_hotkeys = new DICT<string, Tuple<Action, string>>();
        public static DICT<string, Action> event_bindings = new DICT<string, Action>();
        static ContextMenuStrip MMmenu;
        static void makemenu(string switchermenu)
        {
            tray_hotkeys.Clear();
            event_bindings.Clear();
            if (MMmenu == null)
            {
                MMmenu = new ContextMenuStrip();
            }
            else
            {
                MMmenu.Dispose(); MMmenu = null;
                MMmenu = new ContextMenuStrip();
            }
            var mm = Regex.Replace(switchermenu, "\r?\n", "\n");
            var mmls = mm.Split('\n');
            var mms = new ToolStripMenuItem();
            mms.Text = "FaineSwitcher Custom Menu";
            foreach (var me in mmls)
            {
                if (me.StartsWith("||||")) { continue; }
                var mct = Regex.Match(me, "^##(.*)##$", RegexOptions.Multiline);
                if (mct.Groups.Count > 1)
                {
                    mms.Text = mct.Groups[1].Value;
                    continue;
                }
                var lc = '\0';
                var type = 0;
                StringBuilder buf, text, act, arg, hotk;
                buf = new StringBuilder();
                text = new StringBuilder();
                act = new StringBuilder();
                arg = new StringBuilder();
                hotk = new StringBuilder();
                int last = 2;
                for (int i = 0; i != me.Length; i++)
                {
                    var c = me[i];
                    if ((lc != '\\' && c == '|') || i == me.Length - 1)
                    {
                        if (type == 0) text = new StringBuilder(buf.ToString());
                        if (type == 2 && (!string.IsNullOrEmpty(hotk.ToString()) || string.IsNullOrEmpty(act.ToString())))
                        {
                            act = new StringBuilder(buf.ToString());
                            last++;
                        }
                        if (type == 1)
                        {
                            if (buf.ToString().StartsWith("^^"))
                                hotk = new StringBuilder(buf.ToString());
                            else
                                act = new StringBuilder(buf.ToString());
                        }
                        if (type == last) { arg = new StringBuilder(buf.ToString()).Append(c); }
                        if (act.ToString().ToLower() == "multi")
                        {
                            arg.Clear().Append(me.Substring(i + 1, me.Length - i - 1));
                            break;
                        }
                        type++;
                        buf.Clear();
                        lc = c;
                        continue;
                    }
                    buf.Append(c);
                    lc = c;
                }
                arg = arg.Replace("\\|", "|");
                if (!string.IsNullOrEmpty(hotk.ToString()))
                {
                    tray_hotkeys.Add(hotk.ToString(), new Tuple<Action, string>(() =>
                        menuhandle(act.ToString(), arg.ToString()),
                            new StringBuilder().Append(act).Append("|").Append(arg).ToString()));
                    var d = Hotkey.tray_hk_is_double(hotk.ToString());
                    Debug.WriteLine("hotk: " + hotk + d.Item1);
                    if (d.Item1)
                    {
                        if (hotk.ToString().Contains("(("))
                        {
                            hotk = hotk.Replace("((", "~")
                                .Replace("))", "ms [")
                                .Replace("&&", "] => ");
                        }
                        else
                        {
                            hotk = hotk.Replace("&&", "] => ~250ms [");
                        }
                        if (hotk.ToString().EndsWith("["))
                        {
                            hotk.Append(d.Item3);
                        }
                    }
                    text.Append("    [")
                        .Append(Regex.Replace(hotk.Replace("^^", "").ToString(), "((^|\\+|\\[)[lr]?.)", m => m.ToString().ToUpper()))
                        .Append("]");
                }
                var acts = act.ToString();
                if (acts == "evt")
                {
                    menuhandle(acts, arg.ToString());
                    continue;
                }
                if (acts.ToLower() == "dir")
                {
                    try
                    {
                        var argts = arg.ToString();
                        string dir = argts, allow_types = ".*";
                        int maxd = 10, maxentries = 25;
                        if (argts.Contains(">"))
                        {
                            var spl = argts.Split(new[] { '>' });
                            if (spl.Length >= 1)
                            {
                                dir = spl[0];
                            }
                            if (spl.Length >= 2)
                            {
                                var t = spl[1];
                                if (t.Contains("&"))
                                {
                                    var splspl = t.Split(new[] { '&' });
                                    t = splspl[0];
                                    Int32.TryParse(splspl[1], out maxentries);
                                }
                                Int32.TryParse(t, out maxd);
                            }
                            if (spl.Length >= 3)
                            {
                                allow_types = spl[2];
                            }
                        }
                        if (Directory.Exists(dir))
                        {
                            var mmd = new ToolStripMenuItem(text.ToString(), null);
                            dirparser(ref mmd, dir, maxd, allow_types, maxentries);
                            mms.DropDownItems.Add(mmd);
                            var mmd2 = new ToolStripMenuItem(text.ToString(), null);
                            dirparser(ref mmd2, dir, maxd, allow_types, maxentries);
                            MMmenu.Items.Add(mmd2);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + e.StackTrace);
                        Logging.Log("FaineSwitcher.mm > DIR error: " + e.Message + e.StackTrace, 1);
                    }
                    continue;
                }
                mms.DropDownItems.Add(new ToolStripMenuItem(text.ToString(), null, (_, __) =>
                                                            menuhandle(act.ToString(), arg.ToString())));
                MMmenu.Items.Add(new ToolStripMenuItem(text.ToString(), null, (_, __) =>
                                                            menuhandle(act.ToString(), arg.ToString())));
            }
            var wfm = new Timer();
            wfm.Interval = 1000;
            wfm.Tick += (x, xx) =>
            {
                if (Program.switcher != null)
                {
                    List<ToolStripMenuItem> lastitems = new List<ToolStripMenuItem>();
                    for (var i = 0; i != Program.switcher.icon.trIcon.ContextMenuStrip.Items.Count; i++)
                    {
                        lastitems.Add((ToolStripMenuItem)Program.switcher.icon.trIcon.ContextMenuStrip.Items[i]);
                    }
                    Program.switcher.icon.trIcon.ContextMenuStrip.Items.Clear();
                    Program.switcher.icon.trIcon.ContextMenuStrip.Items.Add(mms);
                    Program.switcher.icon.trIcon.ContextMenuStrip.Items.AddRange(lastitems.ToArray());
                    wfm.Stop();
                    wfm.Dispose();
                }
            };
            wfm.Start();
        }
        static string GetShortcutTarget(string file)
        {
            try
            {
                if (Path.GetExtension(file).ToLower() != ".lnk")
                {
                    throw new Exception("Supplied file must be a .LNK file");
                }
                FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read);
                using (BinaryReader fileReader = new BinaryReader(fileStream))
                {
                    fileStream.Seek(0x14, SeekOrigin.Begin);     // Seek to flags
                    uint flags = fileReader.ReadUInt32();        // Read flags
                    if ((flags & 1) == 1)
                    {                      // Bit 1 set means we have to
                                           // skip the shell item ID list
                        fileStream.Seek(0x4c, SeekOrigin.Begin); // Seek to the end of the header
                        uint offset = fileReader.ReadUInt16();   // Read the length of the Shell item ID list
                        fileStream.Seek(offset, SeekOrigin.Current); // Seek past it (to the file locator info)
                    }

                    long fileInfoStartsAt = fileStream.Position; // Store the offset where the file info
                                                                 // structure begins
                    uint totalStructLength = fileReader.ReadUInt32(); // read the length of the whole struct
                    fileStream.Seek(0xc, SeekOrigin.Current); // seek to offset to base pathname
                    uint fileOffset = fileReader.ReadUInt32(); // read offset to base pathname
                                                               // the offset is from the beginning of the file info struct (fileInfoStartsAt)
                    fileStream.Seek((fileInfoStartsAt + fileOffset), SeekOrigin.Begin); // Seek to beginning of
                                                                                        // base pathname (target)
                    long pathLength = (totalStructLength + fileInfoStartsAt) - fileStream.Position - 2; // read
                                                                                                        // the base pathname. I don't need the 2 terminating nulls.
                    char[] linkTarget = fileReader.ReadChars((int)pathLength); // should be unicode safe
                    var link = new string(linkTarget);

                    int begin = link.IndexOf("\0\0");
                    if (begin > -1)
                    {
                        int end = link.IndexOf("\\\\", begin + 2) + 2;
                        end = link.IndexOf('\0', end) + 1;

                        string firstPart = link.Substring(0, begin);
                        string secondPart = link.Substring(end);

                        return firstPart + secondPart;
                    }
                    else
                    {
                        return link;
                    }
                }
            }
            catch
            {
                return "";
            }
        }
        public static string GetLnkTarget(string lnkpath)
        {
            Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
            dynamic shell = Activator.CreateInstance(t);
            string outpath = lnkpath;
            try
            {
                var lnk = shell.CreateShortcut(lnkpath);
                try
                {
                    outpath = lnk.TargetPath;
                }
                finally
                {
                    Marshal.FinalReleaseComObject(lnk);
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(shell);
            }
            return outpath;
        }
        public static Icon GetPathIcon(string filepath, bool small = true)
        {
            Icon clone;
            WinAPI.SHGFI_Flag flags;
            WinAPI.SHFILEINFO shinfo = new WinAPI.SHFILEINFO();
            if (small)
            {
                flags = WinAPI.SHGFI_Flag.SHGFI_ICON | WinAPI.SHGFI_Flag.SHGFI_SMALLICON;
            }
            else
            {
                flags = WinAPI.SHGFI_Flag.SHGFI_ICON | WinAPI.SHGFI_Flag.SHGFI_LARGEICON;
            }
            if (WinAPI.SHGetFileInfo(filepath, 0, ref shinfo, Marshal.SizeOf(shinfo), flags) == 0)
            {
                throw (new FileNotFoundException());
            }
            Icon tmp = Icon.FromHandle(shinfo.hIcon);
            clone = (Icon)tmp.Clone();
            tmp.Dispose();
            if (!WinAPI.DestroyIcon(shinfo.hIcon))
            {
                return clone;
            }
            return clone;
        }
        static Dictionary<string, Image> file_icons_cache = new Dictionary<string, Image>();
        static void dirparser(ref ToolStripMenuItem root, string dir, int max_depth, string allow_types, int maxentries, int this_depth = -1)
        {
            Debug.WriteLine("parsing: " + dir);
            if (this_depth == -1) { this_depth = 0; }
            if (this_depth > max_depth) { return; }
            var dirs = Directory.EnumerateDirectories(dir);
            var fils = Directory.EnumerateFiles(dir);
            var fidis = new List<string>();
            foreach (var d in dirs)
            {
                fidis.Add("D^" + d);
            }
            foreach (var f in fils)
            {
                fidis.Add("F^" + f);
            }
            fidis.Sort();
            int e = 0;
            List<ToolStripMenuItem> _files = new List<ToolStripMenuItem>();
            foreach (var fidi in fidis)
            {
                if (e >= maxentries) { break; }
                FileAttributes attr;
                string t = fidi.Substring(0, 2);
                var fd = fidi.Substring(2);
                bool directory = t == "D^";
                string ext = "", n = "";
                if (t == "F^")
                {
                    var inf = new FileInfo(fd);
                    attr = inf.Attributes;
                    ext = inf.Extension;
                    n = inf.Name;
                }
                else if (directory)
                {
                    var inf = new DirectoryInfo(fd);
                    attr = inf.Attributes;
                    directory = true;
                    n = inf.Name;
                }
                else { continue; }
                if ((attr & FileAttributes.Hidden) == FileAttributes.Hidden) { continue; }
                if (!directory)
                {
                    if (!string.IsNullOrEmpty(allow_types))
                    {
                        var allowed = allow_types.ToLower().Split(',');
                        var lex = ext.ToLower();
                        var allow_that = false;
                        foreach (var a in allowed)
                        {
                            if (a.EndsWith("*") && lex.StartsWith(a.Substring(0, a.Length - 2)) ||
                                a.StartsWith("*") && lex.EndsWith(a.Substring(1)) ||
                                   a == lex)
                            {
                                allow_that = true;
                                break;
                            }
                        }
                        if (!allow_that)
                            continue;
                    }
                }
                Image img = null;
                if (!directory)
                {
                    var eex = ext;
                    var ffd = fd;
                    if (ext.ToLower() == ".lnk")
                    {
                        ffd = GetLnkTarget(fd); //GetShortcutTarget(fd);
                        if (File.Exists(ffd))
                        {
                            eex = new FileInfo(ffd).Extension;
                        }
                        else
                        {
                            ffd = fd;
                        }
                        Debug.WriteLine("LNK-Real: " + ffd);
                    }
                    if (file_icons_cache.ContainsKey(eex))
                    {
                        img = file_icons_cache[eex];
                    }
                    else
                    {
                        var b = GetPathIcon(ffd).ToBitmap(); //Icon.ExtractAssociatedIcon(ffd).ToBitmap();
                        img = Image.FromHbitmap(b.GetHbitmap());
                        b.Dispose();
                        if (eex != ".exe")
                            file_icons_cache[eex] = img;
                    }
                }
                else
                {
                    if (file_icons_cache.ContainsKey("<DIRECTORY>"))
                    {
                        img = file_icons_cache["<DIRECTORY>"];
                    }
                    else
                    {
                        IntPtr large;
                        IntPtr small;
                        WinAPI.ExtractIconEx("shell32.dll", 3, out large, out small, 1);
                        try
                        {
                            var b = Icon.FromHandle(large != IntPtr.Zero ? large : small).ToBitmap();
                            img = Image.FromHbitmap(b.GetHbitmap());
                            b.Dispose();
                        }
                        catch (Exception ee)
                        {
                            Logging.Log("Can't extract icon..." + ee.Message + ee.StackTrace, 1);
                        }
                        file_icons_cache["<DIRECTORY>"] = img;
                    }
                }
                var new_root = new ToolStripMenuItem(n, img);
                new_root.MouseDown += (_, __) => __lopen(fd, directory ? "DIR" : ext, __.Button == MouseButtons.Right);
                if (directory)
                {
                    dirparser(ref new_root, fd, max_depth, allow_types, maxentries, this_depth + 1);
                    root.DropDownItems.Add(new_root);
                }
                else
                {
                    _files.Add(new_root);
                }
                e++;
            }
            if (_files.Count > 0)
            {
                root.DropDownItems.AddRange(_files.ToArray());
            }
        }
        #endregion
        #region Switcher UI controls events
        void Hchk_DARKCheckedChanged(object sender, EventArgs e)
        {
            ToggleDark(Hchk_DARK.Checked);
        }
        void Txt_LCTRLLALTTempLayoutTextChanged(object sender, EventArgs e)
        {
            var txt = (sender as TextBox);
            if (Regex.IsMatch(txt.Text, @"[^0-9]"))
            {
                txt.Text = Regex.Replace(txt.Text, @"[^0-9]", "");
                if (txt.Text.Length > 0)
                {
                    txt.SelectionStart = txt.Text.Length;
                }
            }
        }
        void Chk_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDependentControlsEnabledState();
        }
        void Chk_HKCheckedChanged(object sender, EventArgs e)
        {
            UpdateHKTemps(sender, e);
            ToggleDependentControlsEnabledState();
        }
        void Chk_AutoStartCheckedChanged(object sender, EventArgs e)
        {
        }
        void Btn_DebugInfoClick(object sender, EventArgs e)
        {
            try
            {
                var debuginfo = new StringBuilder().Append("<details><summary>Switcher DEBUG INFO</summary>\r\n\r\n")
                    .Append("<details><summary>Environment info</summary>\r\n\r\n")
                    .Append("\r\n- ").Append(Text)
                    .Append("\r\n- OS = [").Append(Environment.OSVersion).Append("]")
                    .Append("\r\n- x64 = [").Append(Environment.Is64BitOperatingSystem).Append("]")
                    .Append("\r\n- .Net = [").Append(Environment.Version).Append("]")
                    .Append("\r\n</details>")
                    .Append("\r\n" + "<details><summary>All installed layouts</summary>\r\n\r\n");
                foreach (var l in Program.lcnmid)
                {
                    debuginfo.Append(l).Append("\r\n");
                }
                debuginfo.Append("\r\n</details>")
                .Append("<details><summary>FaineSwitcher.ini</summary>\r\n\r\n```ini\r\n")
                    .Append(Program.MyConfs.GetRawWithoutGroup("[Proxy]"))
                    .Append("\r\n</details>");
                if (File.Exists(Path.Combine(nPath, "snippets.txt")))
                    debuginfo.Append("\r\n" + "<details><summary>Snippets</summary>\r\n\r\n```\r\n")
                        .Append(File.ReadAllText(Path.Combine(nPath, "snippets.txt"))).Append("\r\n```");
                debuginfo.Append("\r\n</details>");
                if (Directory.Exists(Path.Combine(nPath, "Flags")))
                {
                    debuginfo.Append("\r\n").Append("<details><summary>Additional flags in Flags directory</summary>\r\n\r\n");
                    foreach (var flg in Directory.GetFiles(Path.Combine(nPath, "Flags")))
                    {
                        debuginfo.Append("- ").Append(Path.GetFileName(flg)).Append("\r\n");
                    }
                    debuginfo.Append("\r\n")
                        .Append("\r\n</details>");
                }
                debuginfo.Append("\r\n</details>");
                Clipboard.SetText(debuginfo.ToString());
                var btDgtTxtWas = btn_DebugInfo.Text;
                btn_DebugInfo.Text = Program.Lang[Languages.Element.DbgInf_Copied];
                tmr.Tick += (_, __) =>
                {
                    btn_DebugInfo.Text = btDgtTxtWas;
                    tmr.Stop();
                };
                tmr.Interval = 2000;
                tmr.Start();
                Logging.Log("Debug info copied.");
            }
            catch (Exception er)
            {
                MessageBox.Show("Error during dgbcopy" + er.StackTrace);
                Logging.Log("Error during DEBUG INFO copy, details:\r\n" + er.Message + "\r\n" + er.StackTrace);
            }
        }
        void Btn_OKClick(object sender, EventArgs e)
        {
            ToggleVisibility();
            SaveConfigs();
        }
        void Btn_ApplyClick(object sender, EventArgs e)
        {
            SaveConfigs();
        }
        void Btn_CancelClick(object sender, EventArgs e)
        {
            ToggleVisibility();
            LoadConfigs();
        }
        void Cbb_KeySelectedIndexChanged(object sender, EventArgs e)
        {
            //cbb_Layout1.Enabled = cbb_Key1.SelectedIndex != 0;
            //cbb_Layout2.Enabled = cbb_Key2.SelectedIndex != 0;
            //cbb_Layout3.Enabled = cbb_Key3.SelectedIndex != 0;
            //cbb_Layout4.Enabled = cbb_Key4.SelectedIndex != 0;
        }
        void SwitcherUIFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                ToggleVisibility();
                LoadConfigs();
            }
        }
        void Lsb_HotkeysSelectedIndexChanged(object sender, EventArgs e)
        {
            UnregisterHotkeys(1);
            UpdateHotkeyControlsSwitch();
            UpdateHotkeyTemps();
            switch (lsb_Hotkeys.SelectedIndex)
            {
                case 4:
                    lbl_HotkeyHelp.Text = Program.Lang[Languages.Element.TT_ConvertWords];
                    break;
                case 5:
                    lbl_HotkeyHelp.Text = Program.Lang[Languages.Element.TT_SymbolIgnore];
                    break;
                case 15:
                    lbl_HotkeyHelp.Text = Program.Lang[Languages.Element.TT_ShowSelectionTranslationHotkey];
                    break;
                case 17:
                    lbl_HotkeyHelp.Text = Program.Lang[Languages.Element.TT_CycleCase];
                    break;
                case 18:
                    lbl_HotkeyHelp.Text = Program.Lang[Languages.Element.TT_CustomConversion];
                    break;
                default:
                    lbl_HotkeyHelp.Text = "";
                    break;
            }
        }
        void Txt_HotkeyKeyDown(object sender, KeyEventArgs e)
        {
            switch (lsb_Hotkeys.SelectedIndex)
            {
                case 0:
                    WinAPI.UnregisterHotKey(Handle, (int)Hotkey.HKID.ToggleVisibility);
                    break;
                case 5:
                    WinAPI.UnregisterHotKey(Handle, (int)Hotkey.HKID.ToggleSymbolIgnoreMode);
                    break;
                case 12:
                    WinAPI.UnregisterHotKey(Handle, (int)Hotkey.HKID.Exit);
                    break;
                case 13:
                    WinAPI.UnregisterHotKey(Handle, (int)Hotkey.HKID.Restart);
                    break;
            }
            txt_Hotkey.Text = OemReadable((e.Modifiers.ToString().Replace(",", " +") + " + " +
                                          Remake(e.KeyCode)).Replace("None + ", ""));
            txt_Hotkey_tempModifiers = e.Modifiers.ToString().Replace(",", " +");
            switch ((int)e.KeyCode)
            {
                case 16:
                case 17:
                case 18:
                    txt_Hotkey_tempKey = 0;
                    break;
                default:
                    txt_Hotkey_tempKey = (int)e.KeyCode;
                    break;
            }
            UpdateHotkeyTemps();
        }
        void Lsb_LangTTAppearenceForListSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLangDisplayControlsSwitch();
            UpdateLangDisplayTemps();
        }
        void Btn_ColorSelectionClick(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (clrd.ShowDialog() == DialogResult.OK)
                btn.BackColor = clrd.Color;
            UpdateLangDisplayTemps();
        }
        void UpdateLDTemps(object sender, EventArgs e)
        {
            UpdateLangDisplayTemps();
        }
        void UpdateHKTemps(object sender, EventArgs e)
        {
            UpdateHotkeyTemps();
        }
        void Btn_LangTTFontClick(object sender, EventArgs e)
        {
            Btn_FontSelection(sender, e);
            UpdateLangDisplayTemps();
        }
        void Btn_FontSelection(object sender, EventArgs e)
        {
            var btn = sender as Button;
            fntd.Font = btn.Font;
            if (fntd.ShowDialog() == DialogResult.OK)
                btn.Font = fntd.Font;
        }
        void Btn_CheckForUpdatesClick(object sender, EventArgs e)
        {
            if (!checking)
            {
                checking = true;
                var btChkTextWas = btn_CheckForUpdates.Text;
                btn_CheckForUpdates.Text = Program.Lang[Languages.Element.Checking];
                UpdInfo = null;
                System.Threading.Tasks.Task.Factory.StartNew(GetUpdateInfo).Wait();
                tmr.Tick += (_, __) =>
                {
                    ;
                    btn_CheckForUpdates.Text = btChkTextWas;
                    SetUInfo();
                    checking = false;
                    tmr.Interval = 3000;
                    tmr.Stop();
                };
                tmr.Interval = 1900;
                if (UpdInfo[2] == Program.Lang[Languages.Element.Error])
                {
                    tmr.Interval = 1000;
                    tmr.Start();
                }
                else
                {
                    if (cbb_UpdatesChannel.SelectedIndex != 0)
                    {
                        if (Program.MyConfs.Read("Updates", "LatestCommit") != UpdInfo[4])
                        {
                            btn_CheckForUpdates.Text = Program.Lang[Languages.Element.TimeToUpdate];
                            tmr.Start();
                            SetUInfo();
                            grb_DownloadUpdate.Enabled = true;
                        }
                        else
                        {
                            btn_CheckForUpdates.Text = Program.Lang[Languages.Element.YouHaveLatest];
                            tmr.Start();
                            grb_DownloadUpdate.Enabled = false;
                            SetUInfo();
                        }
                    }
                    else if (flVersion("v" + Application.ProductVersion) <
                       flVersion(UpdInfo[2]) || this.Text.Contains("dev"))
                    {
                        btn_CheckForUpdates.Text = Program.Lang[Languages.Element.TimeToUpdate];
                        tmr.Start();
                        SetUInfo();
                        grb_DownloadUpdate.Enabled = true;
                    }
                    else
                    {
                        btn_CheckForUpdates.Text = Program.Lang[Languages.Element.YouHaveLatest];
                        tmr.Start();
                        grb_DownloadUpdate.Enabled = false;
                        SetUInfo();
                    }
                }
            }
        }
        void Btn_DownloadUpdateClick(object sender, EventArgs e)
        {
            if (!updating && UpdInfo != null)
            {
                updating = true;
                //Downloads latest FaineSwitcher
                using (var wc = new WebClient())
                {
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    // Gets filename from url
                    var BDMText = btn_DownloadUpdate.Text;
                    var fn = Regex.Match(UpdInfo[3], @"[^\\\/]+$").Groups[0].Value;
                    if (!String.IsNullOrEmpty(txt_ProxyServerPort.Text))
                    {
                        wc.Proxy = MakeProxy();
                    }
                    if (UpdInfo.Length > 4)
                        Program.MyConfs.WriteSave("Updates", "LatestCommit", UpdInfo[4]);
                    else Program.MyConfs.WriteSave("Updates", "LatestCommit", "Downgraded to Stable");
                    Logging.Log("Downloading FaineSwitcher update: " + UpdInfo[3]);
                    try
                    {
                        wc.DownloadFileAsync(new Uri(UpdInfo[3]), Path.Combine(Path.GetTempPath(), fn));
                        btn_DownloadUpdate.Text = "Downloading " + fn;
                        animate.Tick += (_, __) => { btn_DownloadUpdate.Text += "."; };
                        animate.Start();
                        btn_DownloadUpdate.Enabled = false;
                        tmr.Tick += (_, __) =>
                        {
                            // Checks if progress changed?
                            if (progress == _progress)
                            {
                                old.Stop();
                                isold = true;
                                btn_DownloadUpdate.Enabled = true;
                                animate.Stop();
                                prb_UpdateDownloadProgress.Value = progress = _progress = 0;
                                wc.CancelAsync();
                                updating = false;
                                btn_DownloadUpdate.Text = "Error...";
                                tmr.Tick += (o, oo) =>
                                {
                                    btn_DownloadUpdate.Text = BDMText;
                                    tmr.Stop();
                                };
                                tmr.Interval = 3000;
                                tmr.Start();
                            }
                            else
                            {
                                tmr.Stop();
                            }
                        };
                        old.Start();
                        tmr.Interval = 15000;
                        tmr.Start();
                    }
                    catch (Exception ex)
                    {
                        Logging.Log("Download update error: " + ex.Message + Environment.NewLine + ex.StackTrace, 1);
                    }
                }
            }
        }
        void SwitcherUIDeactivate(object sender, EventArgs e)
        {
            RegisterHotkeys();
        }
        void SwitcherUIActivated(object sender, EventArgs e)
        {
            if (reload_snip)
            {
                txt_Snippets.Text = File.ReadAllText(snipfile);
                reload_snip = false;
            }
            if (tabs.SelectedIndex == tabs.TabPages.IndexOf(tab_hotkeys))
            {
                UnregisterHotkeys(1);
                ScrlCheck.Stop();
                capsCheck.Stop();
            }
            else
            {
                RegisterHotkeys();
                ToggleTimers();
            }
            if (tabs.SelectedIndex == tabs.TabPages.IndexOf(tab_autoswitch))
            {
                if (!SnippetsEnabled)
                {
                    chk_AutoSwitchSpaceAfter.Visible = chk_AutoSwitch.Visible = chk_AutoSwitchSwitchToGuessLayout.Enabled =
                        btn_UpdateAutoSwitchDictionary.Enabled = txt_AutoSwitchDictionary.Enabled = chk_DownloadASD_InZip.Enabled = false;
                    lbl_AutoSwitchDependsOnSnippets.Visible = true;
                }
                else
                {
                    chk_AutoSwitchSpaceAfter.Visible = chk_AutoSwitch.Visible = chk_AutoSwitchSwitchToGuessLayout.Enabled =
                        btn_UpdateAutoSwitchDictionary.Enabled = txt_AutoSwitchDictionary.Enabled = chk_DownloadASD_InZip.Enabled = true;
                    lbl_AutoSwitchDependsOnSnippets.Visible = false;
                    ToggleDependentControlsEnabledState();
                }
            }
        }
        void Cbb_UpdatesChannelSelectedIndexChanged(object sender, EventArgs e)
        {
            Program.MyConfs.WriteSave("Updates", "Channel", (sender as ComboBox).SelectedItem.ToString());
        }
        void Txt_AutoSwitchDictionaryTextChanged(object sender, EventArgs e)
        {
            if (!AutoSwitchDictionaryTooBig && !txt_AutoSwitchDictionary.ReadOnly)
                AutoSwitchDictionaryRaw = txt_AutoSwitchDictionary.Text;
            if (!as_checking)
            {
                as_checking = true;
                tmr.Tick += (_, __) =>
                {
                    UpdateSnippetCountLabel(AutoSwitchDictionaryRaw, lbl_AutoSwitchWordsCount, false);
                    as_checking = false;
                    tmr.Dispose(); tmr = new Timer();
                };
                tmr.Interval = 1500;
                tmr.Start();
            }
        }
        void Txt_SnippetsTextChanged(object sender, EventArgs e)
        {
            if (!snip_checking)
            {
                snip_checking = true;
                tmr.Tick += (_, __) =>
                {
                    UpdateSnippetCountLabel(txt_Snippets.Text, lbl_SnippetsCount);
                    snip_checking = false;
                    tmr.Dispose(); tmr = new Timer();
                };
                tmr.Interval = 1000;
                tmr.Start();
            }
        }
        void Chk_DownloadASD_InZipCheckedChanged(object sender, EventArgs e)
        {
            Dowload_ASD_InZip = chk_DownloadASD_InZip.Checked;
            check_ASD_size = true;
        }
        void Btn_NCR_AddClick(object sender, EventArgs e)
        {
            var _set = new Panel();
            _set.Width = pan_NoConvertRules.Width * 95 / 100 - 2;
            NCRSetsCount++;
            _set.Name = "set_" + NCRSetsCount;
            var top = 1;
            if (NCRSetsCount > 1)
                top = pan_NoConvertRules.Controls["set_" + (NCRSetsCount - 1)].Top + 25;
            _set.Height = 27;
            _set.Top = top;
            _set.Left = 1;
            var _baseLeft = (int)(pan_NoConvertRules.Width * 2 / 100);
            var rule = new TextBoxCA() { Left = _baseLeft, Name = "rule" + NCRSetsCount, Width = pan_NoConvertRules.Width / 3, Text = "^[A-Z]+$", Top = 2 };
            var isnip = new CheckBox() { Left = _baseLeft + rule.Width + 5, Name = "isnip" + NCRSetsCount, Width = pan_NoConvertRules.Width / 5, Text = Program.Lang[Languages.Element.tab_Snippets], Top = 2 };
            var iauto = new CheckBox() { Left = _baseLeft + rule.Width + 5 + isnip.Width, Name = "iauto" + NCRSetsCount, Width = pan_NoConvertRules.Width / 5, Text = Program.Lang[Languages.Element.tab_AutoSwitch], Top = 2 };
            _set.Controls.Add(rule);
            _set.Controls.Add(isnip);
            _set.Controls.Add(iauto);
            pan_NoConvertRules.Controls.Add(_set);
            lbl_NCRCount.Text = "#" + NCRSetsCount;
        }
        void Btn_NCR_SubClick(object sender, EventArgs e)
        {
            if (NCRSetsCount < 1) return;
            pan_NoConvertRules.Controls["set_" + NCRSetsCount].Dispose();
            NCRSetsCount--;
            lbl_NCRCount.Text = "#" + NCRSetsCount;
        }
        void Btn_TrAddSetClick(object sender, EventArgs e)
        {
            if (TrSetCount > 98) return;
            var _set = new Panel();
            _set.Width = (pan_TrSets.Width * 98 / 100) - 2;
            TrSetCount++;
            _set.Name = "set_" + TrSetCount;
            var top = 1;
            if (TrSetCount > 1)
                top = pan_TrSets.Controls["set_" + (TrSetCount - 1)].Top + 25;
            _set.Height = 23;
            _set.Top = top;
            _set.Left = 1;
            var _baseLeft = (int)(pan_TrSets.Width * 2 / 100);
            var lbl_width = 25;
            var lbl_frto_width = 40;
            var cbb_width = 160;
            _set.Controls.Add(new Label() { Left = _baseLeft, Name = "lbl_num" + TrSetCount, Width = lbl_width, Text = TrSetCount + ":", Top = 2 });
            var fr_lbl = new Label() { Left = _baseLeft + lbl_width, Name = "lbl_fr" + TrSetCount, Width = lbl_frto_width, Text = "From:", Top = 2 };
            var fr_cbb = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Left = _baseLeft + lbl_width + lbl_frto_width + 9, Name = "cbb_fr" + TrSetCount, Width = cbb_width };
            var to_lbl = new Label() { Left = _baseLeft + lbl_width + lbl_frto_width + 49 + cbb_width, Name = "lbl_to" + TrSetCount, Width = lbl_frto_width, Text = "To:", Top = 2 };
            var to_cbb = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Left = _baseLeft + lbl_width + lbl_frto_width + 49 + cbb_width + lbl_frto_width + 9, Name = "cbb_to" + TrSetCount, Width = cbb_width };
            fr_cbb.SelectedIndexChanged += new EventHandler(Cbb_FrToSelectedIndexChanged);
            to_cbb.SelectedIndexChanged += new EventHandler(Cbb_FrToSelectedIndexChanged);
            //			cbb.Items.Add(Program.Lang[Languages.Element.SwitchBetween]);
            fr_cbb.Items.AddRange(TranslatePanel.GTLangs);
            to_cbb.Items.AddRange(TranslatePanel.GTLangs);
            fr_cbb.SelectedIndex = to_cbb.SelectedIndex = 0;
            _set.Controls.Add(fr_lbl);
            _set.Controls.Add(fr_cbb);
            _set.Controls.Add(new Label() { Left = _baseLeft + lbl_width + lbl_frto_width + 20 + cbb_width, Name = "lbl_arr" + TrSetCount, Width = lbl_width, Text = "->", Top = 2 });
            _set.Controls.Add(to_lbl);
            _set.Controls.Add(to_cbb);
            //			SpecKeySetsValues["cbb_fr"+TrSetCount+"_key"] = SpecKeySetsValues["txt_key"+TrSetCount+"_mods"] = SpecKeySetsValues["cbb_typ"+TrSetCount] = "";
            pan_TrSets.Controls.Add(_set);
            lbl_TrSetsCount.ForeColor = Color.Black;
            lbl_TrSetsCount.Text = "#" + TrSetCount;
            if (TrSetCount > 98) lbl_TrSetsCount.ForeColor = Color.Red;
        }
        void Btn_TrSubSetClick(object sender, EventArgs e)
        {
            if (TrSetCount < 1) return;
            pan_TrSets.Controls["set_" + TrSetCount].Dispose();
            TrSetCount--;
            lbl_TrSetsCount.ForeColor = Color.Black;
            lbl_TrSetsCount.Text = "#" + TrSetCount;
            if (TrSetCount < 1)
                lbl_TrSetsCount.ForeColor = Color.LightGray;

        }
        void Btn_AddSetClick(object sender, EventArgs e)
        {
            //if (SpecKeySetCount>98) return;
            //var _set = new Panel();
            //_set.Width = (pan_KeySets.Width*98/100)-2;
            //SpecKeySetCount++;
            //_set.Name = "set_"+SpecKeySetCount;
            //var top = 1;
            //if (SpecKeySetCount>1)
            //	top = pan_KeySets.Controls["set_"+(SpecKeySetCount-1)].Top+25;
            //_set.Height = 23;
            //_set.Top = top;
            //_set.Left = 1;
            //var _baseLeft = (int)(pan_KeySets.Width*2/100);
            //var txt_width = 190;
            //var chk_width = 45;
            //var lbl_width = 25;
            //var cbb_width = 190;
            //_set.Controls.Add(new Label(){Left = _baseLeft, Name="lbl_num"+SpecKeySetCount, Width=lbl_width, Text=SpecKeySetCount+":", Top=2});
            //var txt = new TextBox(){Left = _baseLeft+lbl_width, Name="txt_key"+SpecKeySetCount, Width=txt_width, BackColor=SystemColors.Window, ReadOnly=true};
            //txt.KeyDown += new KeyEventHandler(Txt_SpecHotkeyDown);
            //var chk = new CheckBox(){Left = _baseLeft+lbl_width+txt_width+3, Name="chk_win"+SpecKeySetCount, Width=chk_width, Text="Win"};
            //chk.CheckedChanged += new EventHandler(Chk_SpecWinCheckedChanged);
            //var cbb = new ComboBox(){DropDownStyle = ComboBoxStyle.DropDownList, Left = _baseLeft+lbl_width+txt_width+chk_width+lbl_width+9, Name="cbb_typ"+SpecKeySetCount, Width=cbb_width};
            //cbb.SelectedIndexChanged += new EventHandler(Cbb_SpecTypeSelectedIndexChanged);
            //cbb.Items.Add(Program.Lang[Languages.Element.SwitchBetween]);
            //cbb.Items.AddRange(Program.lcnmid.ToArray());
            //_set.Controls.Add(txt);
            //_set.Controls.Add(chk);
            //_set.Controls.Add(new Label(){Left = _baseLeft+lbl_width+txt_width+chk_width+6, Name="lbl_arr"+SpecKeySetCount, Width=lbl_width, Text="->", Top=2});
            //_set.Controls.Add(cbb);
            //SpecKeySetsValues["txt_key"+SpecKeySetCount+"_key"] = SpecKeySetsValues["txt_key"+SpecKeySetCount+"_mods"] = SpecKeySetsValues["cbb_typ"+SpecKeySetCount] = "";
            //pan_KeySets.Controls.Add(_set);
            //lbl_SetsCount.ForeColor = Color.Black;
            //lbl_SetsCount.Text = "#"+SpecKeySetCount;
            //if (SpecKeySetCount>98) lbl_SetsCount.ForeColor = Color.Red;
        }
        void Btn_SubSetClick(object sender, EventArgs e)
        {
            //if (SpecKeySetCount < 1) return;
            //pan_KeySets.Controls["set_"+SpecKeySetCount].Dispose();
            //SpecKeySetCount--;
            //lbl_SetsCount.ForeColor = Color.Black;
            //lbl_SetsCount.Text = "#"+SpecKeySetCount;
            //if (SpecKeySetCount < 1)
            //	lbl_SetsCount.ForeColor = Color.LightGray;

        }
        void Txt_SpecHotkeyDown(object sender, KeyEventArgs e)
        {
            var t = sender as TextBox;
            if (e.KeyCode == Keys.Back && e.Modifiers == Keys.None)
            {
                SpecKeySetsValues[t.Name + "_key"] = SpecKeySetsValues[t.Name + "_mods"] = t.Text = "";
                return;
            }
            Debug.WriteLine(e.KeyCode + " E");
            t.Text = OemReadable((e.Modifiers.ToString().Replace(",", " +") + " + " +
                                          Remake(e.KeyCode)).Replace("None + ", ""));
            SpecKeySetsValues[t.Name + "_key"] = ((int)e.KeyCode).ToString();
            SpecKeySetsValues[t.Name + "_mods"] = e.Modifiers.ToString().Replace(",", " +");
        }
        void Chk_SpecWinCheckedChanged(object sender, EventArgs e)
        {
            var c = sender as CheckBox;
            var key = SpecKeySetsValues["txt_key" + c.Name.Replace("chk_win", "") + "_mods"];
            var hasWin = key.Contains("Win");
            if (hasWin && !c.Checked)
                SpecKeySetsValues["txt_key" + c.Name.Replace("chk_win", "") + "_mods"] = key.Replace("Win", "");
            if (!hasWin && c.Checked)
                SpecKeySetsValues["txt_key" + c.Name.Replace("chk_win", "") + "_mods"] = key + " + Win";
        }
        void Cbb_SpecTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            SpecKeySetsValues[cb.Name] = cb.SelectedItem.ToString();
        }
        void Cbb_TrMethodSelectedIndexChanged(object sender, EventArgs e)
        {
            TranslatePanel.useGS = (cbb_TrMethod.SelectedIndex == 1) ? true : false;
            TranslatePanel.useNA = (cbb_TrMethod.SelectedIndex == 2) ? true : false;
        }
        void Cbb_FrToSelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            TrSetsValues[cb.Name] = TranslatePanel.GTLangsSh[cb.SelectedIndex];
            //			Debug.WriteLine(TrSetsValues[cb.Name]);
        }
        void Cbb_SpecKeysTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            //var old = cbb_SpecKeysType.SelectedIndex == 0;
            //lbl_Arrow1.Visible = lbl_Arrow2.Visible = lbl_Arrow3.Visible = lbl_Arrow4.Visible = grb_Layouts.Visible = grb_Keys.Visible = old;
            //lbl_SetsCount.Visible = pan_KeySets.Visible = btn_SubSet.Visible = btn_AddSet.Visible = !old;
        }
        void Btn_SelectSndClick(object sender, EventArgs e)
        {
            lbl_CustomSound.Text = SelectGetWavFile();
            HelpMeUnderstand.SetToolTip(lbl_CustomSound, lbl_CustomSound.Text);
        }
        void Btn_SelectSnd2Click(object sender, EventArgs e)
        {
            lbl_CustomSound2.Text = SelectGetWavFile();
            HelpMeUnderstand.SetToolTip(lbl_CustomSound2, lbl_CustomSound2.Text);
        }
        void Btn_backupClick(object sender, EventArgs e)
        {
            SyncBackup();
        }
        void Btn_restoreClick(object sender, EventArgs e)
        {
            SyncRestore();
        }
        bool pctres, pctbkp;
        void PctBkpCopyClick(object sender, EventArgs e)
        {
            if (!pctbkp)
            {
                pctbkp = true;
                var t = new Timer();
                t.Tick += (_, ___) =>
                {
                    pctBkpCopy.BackgroundImage = Properties.Resources.clip;
                    pctbkp = false;
                    t.Stop();
                    t.Dispose();
                };
                t.Interval = 1800;
                if (!string.IsNullOrEmpty(txt_backupId.Text))
                {
                    KMHook.RestoreClipBoard(txt_backupId.Text);
                    pctBkpCopy.BackgroundImage = Properties.Resources.clipok;
                    t.Start();
                }
                else
                {
                    pctBkpCopy.BackgroundImage = Properties.Resources.cliperr;
                    t.Start();
                }
            }
        }
        void PctResPasteClick(object sender, EventArgs e)
        {
            if (!pctres)
            {
                pctres = true;
                var t = new Timer();
                t.Tick += (_, ___) =>
                {
                    pctResPaste.BackgroundImage = Properties.Resources.clip;
                    pctres = false;
                    t.Stop();
                    t.Dispose();
                };
                t.Interval = 1800;
                var stri = KMHook.GetClipboard(3);
                if (!string.IsNullOrEmpty(stri))
                {
                    txt_restoreId.Text = stri;
                    pctResPaste.BackgroundImage = Properties.Resources.clipok;
                    t.Start();
                }
                else
                {
                    pctResPaste.BackgroundImage = Properties.Resources.cliperr;
                    t.Start();
                }
            }
        }
        void Chk_ZxZCheckedChanged(object sender, EventArgs e)
        {
            ZxZ = (sender as CheckBox).Checked;
        }
        static string HotkeyForm_hotkey = "";
        class HotkeyForm : Form
        {
            Label hk;
            public HotkeyForm()
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.ShowInTaskbar = false;
                this.BackColor = Color.ForestGreen;
                this.hk = new Label();
                this.hk.AutoSize = false;
                this.hk.Width = 238;
                this.hk.Font = new Font(this.hk.Font.FontFamily, 14);
                this.hk.Height = 26;
                this.hk.BackColor = Color.White;
                this.hk.ForeColor = Color.Orange;
                this.hk.Location = new Point(1, 1);
                this.Controls.Add(hk);
                this.MinimumSize = new Size(50, 10);
                this.Width = 240;
                this.Height = 28;
                this.CenterToScreen();
                this.TopMost = true;
            }
            bool aok; int lmsg = 0;
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == (int)WinAPI.WM_KEYUP ||
                        m.Msg == (int)WinAPI.WM_SYSKEYUP ||
                        m.Msg == (int)WinAPI.WM_SYSKEYDOWN ||
                        m.Msg == (int)WinAPI.WM_KEYDOWN)
                {
                    var mods = KMHook.GetModsStr(KMHook.ctrl, KMHook.ctrl_r, KMHook.shift, KMHook.shift_r, KMHook.alt, KMHook.alt_r, KMHook.win, KMHook.win_r);
                    var k = (Keys)m.WParam.ToInt32();
                    var ok = false;
                    if (k != Keys.LControlKey &&
                            k != Keys.RControlKey &&
                            k != Keys.LShiftKey &&
                            k != Keys.RShiftKey &&
                            k != Keys.LMenu &&
                            k != Keys.RMenu &&
                            k != Keys.ShiftKey &&
                            k != Keys.Menu &&
                            k != Keys.ControlKey &&
                            k != Keys.LWin &&
                            k != Keys.RWin)
                    {
                        mods += k;
                        ok = true;
                    }
                    this.hk.Text = mods;
                    if (lmsg == 0) { lmsg = m.Msg; }
                    if (ok)
                    {
                        aok = true;
                        HotkeyForm_hotkey = this.hk.Text;
                    }
                    if (aok && lmsg != m.Msg)
                    {
                        this.Close();
                    }
                    lmsg = m.Msg;
                }
                base.WndProc(ref m);
            }
        }
        void Cbb_SnippetExpandKeysSelectedIndexChanged(object sender, EventArgs e)
        {
            if (configs_loading) return;
            if (cbb_SnippetExpandKeys.SelectedIndex == 2)
            {
                Debug.WriteLine("Other");
                var hkf = new HotkeyForm();
                hkf.ShowDialog();
                SnippetsExpKeyOther = HotkeyForm_hotkey;
                configs_loading = true;
                cbb_SnippetExpandKeys.Items[2] = "*[" + SnippetsExpKeyOther + "]";
                configs_loading = false;
                Debug.WriteLine("Snippets-other-hotkey" + SnippetsExpKeyOther);
            }
        }
        void Htxt_RedefinesEnter(object sender, EventArgs e)
        {
            var t = (TextBox)sender;
            t.Multiline = true;
            var i = t.SelectionStart;
            t.Text = t.Text.Replace("|", Environment.NewLine);
            t.SelectionStart = i;
            if (t.Lines.Length > 1)
            {
                t.Height = (int)(5 * 24);
                t.ScrollBars = ScrollBars.Vertical;
            }
        }
        void Htxt_RedefinesLeave(object sender, EventArgs e)
        {
            var t = (TextBox)sender;
            var i = t.SelectionStart;
            t.Text = t.Text.Replace(Environment.NewLine, "|");
            t.SelectionStart = i;
            t.Height = 24;
            //			t.Multiline = false;
            t.ScrollBars = ScrollBars.None;
        }
        void Htxt_RedefinesTextChanged(object sender, EventArgs e)
        {
            var t = (TextBox)sender;
            if (t.Focused)
            {
                var i = t.SelectionStart;
                if (t.Text.Length > 0)
                {
                    if (t.Text[t.Text.Length - 1] == '|') i++;
                    t.Text = t.Text.Replace("|", Environment.NewLine);
                }
                t.SelectionStart = i;
                t.ScrollToCaret();
            }
        }
        #endregion
        #region Sync
        string[] ReadToBackup(string id, string name, bool chk, bool proxyg = true)
        {
            var r = "";
            var stat = "";
            var f = Path.Combine(nPath, name);
            if (chk)
            {
                if (!File.Exists(f))
                {
                    stat += name + " " + Program.Lang[Languages.Element.Not].ToLower() + " " + Program.Lang[Languages.Element.Exist].ToLower();
                    return new[] { "", stat };
                }
                var fi = new FileInfo(f);
                if (fi.Length >= 400000)
                    stat += name + Program.Lang[Languages.Element.TooBig];
                try
                {
                    r += "#------>" + id + Environment.NewLine;
                    if (f.Contains("FaineSwitcher.ini") && !proxyg)
                        r += Program.MyConfs.GetRawWithoutGroup("[Proxy]");
                    else
                        r += File.ReadAllText(f);
                    r += Environment.NewLine + "#------>" + id + Environment.NewLine;
                }
                catch (Exception e)
                {
                    stat += name + ": " + Program.Lang[Languages.Element.CannotBe].ToLower() + " " + Program.Lang[Languages.Element.Readen].ToLower() + e.Message;
                }
            }
            return new[] { r, stat };
        }
        string WriteRestoreFiles(string raw, bool mini, bool stxt, bool htxt, bool ttxt, bool proxyg = true, bool mm = false)
        {
            var stat = "";
            var t = raw.Replace("\r", "");
            var ll = t.Split('\n');
            var bb = new[] { mini, stxt, htxt, ttxt, mm };
            var tn = "dummy";
            var st = false;
            var d = new Dictionary<string, string>();
            for (var i = 0; i != ll.Length - 1; i++)
            {
                var l = ll[i];
                var cont = false;
                var end = false;
                if (st)
                {
                    if (i + 1 <= ll.Length - 1)
                    {
                        var lz = ll[i + 1];
                        if (lz.StartsWith(SYNC_SEP, StringComparison.InvariantCulture))
                        {
                            if (lz == SYNC_SEP + tn)
                            {
                                end = true;
                            }
                        }
                    }
                }
                if (l.StartsWith(SYNC_SEP, StringComparison.InvariantCulture))
                {
                    foreach (var type in SYNC_TYPES)
                    {
                        if (l == SYNC_SEP + type)
                        {
                            if (tn == type)
                                tn = "dummy";
                            else
                            {
                                tn = type;
                                st = true;
                                cont = true;
                            }
                        }
                    }
                }
                if (cont) continue;
                var ln = l + ((i == ll.Length - 2 || end) ? "" : Environment.NewLine);
                if (d.ContainsKey(tn))
                {
                    var va = d[tn];
                    d[tn] = va + ln;
                }
                else
                    d.Add(tn, ln);
            }
            var OK = "";
            var ERR = "";
            for (var i = 0; i != bb.Length; i++)
            {
                var b = bb[i];
                var ty = SYNC_TYPES[i];
                if (b)
                {
                    if (d.ContainsKey(ty))
                    {
                        try
                        {
                            if (ty == "ini")
                            {
                                if (!proxyg)
                                    Program.MyConfs._INI.Raw = Program.MyConfs.GetRawWithoutGroup("[Proxy]", d[ty]);
                                else
                                    Program.MyConfs._INI.Raw = d[ty];
                            }
                            var f = Path.Combine(nPath, SYNC_NAMES[i]);
                            Debug.WriteLine("Writing: " + f);
                            File.WriteAllText(f, d[ty]);
                            OK += " " + SYNC_NAMES[i];
                        }
                        catch (Exception e)
                        {
                            stat += ty + ": " + e.Message + Environment.NewLine;
                        }
                    }
                    else
                    {
                        ERR += " " + SYNC_NAMES[i];
                    }
                }
            }
            stat += "OK:" + OK + (ERR != "" ? (Environment.NewLine + "ERR:" + ERR) : "");
            return stat;
        }
        public static Random rand = new Random();
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var s = "";
            for (int i = 0; i < length; i++)
                s += chars[rand.Next(chars.Length)];
            return s;
        }
        string SyncUploadZxZ(string content)
        {
            try
            {
                var fname = "FaineSwitcher-Sync." + GetRandomString(8) + ".txt";
                Console.WriteLine("fname:" + fname);
                string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(SYNC_HOST2);
                if (!String.IsNullOrEmpty(txt_ProxyServerPort.Text))
                {
                    req.Proxy = MakeProxy();
                }
                req.ContentType = "multipart/form-data; boundary=" + boundary;
                req.Method = "POST";
                req.KeepAlive = true;
                var form = Encoding.UTF8.GetBytes("\n--" + boundary + "\n" +
                                                  "Content-Disposition: form-data; name=\"file\"; filename=" + fname + "\n" +
                                                  "Content-Type: application/octet-stream\n\n" +
                                                  content + "\n--" + boundary + "--");
                req.ContentLength = form.Length;
                using (var rs = req.GetRequestStream())
                {
                    rs.Write(form, 0, form.Length);
                }
                using (var r = req.GetResponse())
                {
                    var rs = r.GetResponseStream();
                    var sr = new StreamReader(rs);
                    var str = sr.ReadToEnd();
                    sr.Dispose();
                    return str;
                }
            }
            catch (WebException ex)
            {
                using (WebResponse r = ex.Response)
                {
                    using (var sr = new StreamReader(r.GetResponseStream()))
                        return sr.ReadToEnd();

                }
            }
        }
        string SyncUploadHB(byte[] data, ref StringBuilder stat)
        {

            return "";

            using (var wc = new WebClient())
            {
                if (!String.IsNullOrEmpty(txt_ProxyServerPort.Text))
                {
                    wc.Proxy = MakeProxy();
                }
                wc.Encoding = Encoding.UTF8;
                try
                {
                    var r = wc.UploadData(new Uri(SYNC_HOST + "/documents"), "POST", data);
                    return Regex.Match(Encoding.UTF8.GetString(r), "^[{].key.:.(.+).[}]$").Groups[1].Value;
                }
                catch (Exception e)
                {
                    stat.Clear().Append(e.Message);
                    if (data.Length >= 400000)
                        stat.Append(Program.Lang[Languages.Element.TooBig]);
                }
            }
            return "";
        }
        void SyncBackup()
        {

            txt_backupId.Text = "Not allow";
            return;
            string id = "";
            var rawtext = new StringBuilder();
            var bb = new[] { chk_Mini.Checked, chk_Stxt.Checked, chk_Htxt.Checked, chk_Ttxt.Checked, chk_Mmm.Checked };
            var stat = new StringBuilder("OK");
            for (int i = 0; i != SYNC_NAMES.Length; i++)
            {
                var r = ReadToBackup(SYNC_TYPES[i], SYNC_NAMES[i], bb[i], chk_andPROXY.Checked);
                rawtext.Append(r[0]);
                if (r[1] != "")
                {
                    stat.Append(Environment.NewLine).Append(r[1]);
                }
            }
            Debug.WriteLine("Rawtext: " + rawtext);
            if (!ZxZ)
                id = SyncUploadHB(Encoding.UTF8.GetBytes(rawtext.ToString()), ref stat);
            else
                id = SyncUploadZxZ(rawtext.ToString());
            Debug.WriteLine("id:" + id);
            txt_backupId.Text = (ZxZ ? "" : (SYNC_HOST + "/")) + id;
            Program.MyConfs.Write("Sync", "BLast", txt_backupId.Text);
            txt_backupId.Enabled = true;
            txt_backupStatus.Text = stat.ToString();
            txt_backupStatus.Visible = true;
        }
        void SyncRestore()
        {
            txt_restoreId.Text = "Not Allow";
            return;
            var id = txt_restoreId.Text;
            var stat = "";
            if (!string.IsNullOrEmpty(id))
            {
                if (!ZxZ)
                {
                    var raw = SYNC_HOST + "/raw";
                    if (id.StartsWith("http", StringComparison.InvariantCulture))
                    {
                        if (!id.StartsWith(raw, StringComparison.InvariantCulture) || id.Contains("hastebin.com"))
                        {
                            var p = id.Split('/');
                            var l = p[p.Length - 1];
                            if (string.IsNullOrEmpty(l))
                                l = p[p.Length - 2];
                            id = raw + "/" + l;
                        }
                    }
                    else
                    {
                        if (id.Length >= 32)
                        {
                            stat = Program.Lang[Languages.Element.UnknownID];
                        }
                        else
                            id = raw + "/" + id;
                    }
                }
                Debug.WriteLine("id:" + id);
                var d = "";
                if (!string.IsNullOrEmpty(id))
                {
                    using (var wc = new WebClient())
                    {
                        if (!String.IsNullOrEmpty(txt_ProxyServerPort.Text))
                        {
                            wc.Proxy = MakeProxy();
                        }
                        wc.Encoding = Encoding.UTF8;
                        try
                        {
                            d = Encoding.UTF8.GetString(wc.DownloadData(new Uri(id)));
                        }
                        catch (Exception e)
                        {
                            stat = e.Message;
                        }
                    }
                }
                Debug.WriteLine(d);
                if (!string.IsNullOrEmpty(d))
                {
                    stat += WriteRestoreFiles(d, chk_rMini.Checked, chk_rStxt.Checked, chk_rHtxt.Checked, chk_rTtxt.Checked, chk_andPROXY2.Checked, chk_rMmm.Checked); Program.MyConfs.Write("Sync", "BLast", txt_backupId.Text);
                    Program.MyConfs.Write("Sync", "RLast", txt_restoreId.Text);
                }
                LoadConfigs();
            }
            else
            {
                stat = Program.Lang[Languages.Element.EnterID];
            }
            txt_restoreStatus.Text = stat;
            txt_restoreStatus.Visible = true;
        }
        void SetBools(string bools, char sep, out bool mini, out bool stxt, out bool htxt, out bool ttxt, out bool ptxt, out bool mmm)
        {
            var s = bools.Split(sep);
            mini = boo(s[0]);
            stxt = boo(s[1]);
            htxt = boo(s[2]);
            ttxt = boo(s[3]);
            try
            {
                ptxt = boo(s[4]);
            }
            catch
            {
                ptxt = false;
            }
            try
            {
                mmm = boo(s[5]);
            }
            catch
            {
                mmm = false;
            }
        }
        bool boo(string s)
        {
            int i = 0;
            bool b;
            bool.TryParse(s, out b);
            int.TryParse(s, out i);
            if (i > 0)
                return true;
            return b;
        }
        int bin(bool b)
        {
            if (b)
                return 1;
            return 0;
        }
        void Txt_SnippetsKeyDown(object sender, KeyEventArgs e)
        {
            var t = (sender as TextBox);
            Debug.WriteLine("C" + e.Control + " K" + e.KeyCode);
            if (e.Control && (e.KeyCode == Keys.OemQuestion || e.KeyCode == Keys.K))
            {
                var cs = t.SelectionStart;
                var l = t.GetLineFromCharIndex(t.SelectionStart);
                var sl = t.SelectionLength;
                var ll = l;
                if (sl != 0)
                {
                    ll = t.GetLineFromCharIndex(t.SelectionStart + sl);
                }
                var tl = t.Lines;
                Debug.WriteLine("U: Ss: " + cs + " Sl: " + sl);
                for (int i = l; i <= ll; i++)
                {
                    var lv = tl.GetValue(i) as string;
                    Debug.WriteLine(i + ": Cur: " + lv);
                    if (lv.StartsWith("#"))
                    {
                        lv = lv.Substring(1, lv.Length - 1);
                        if (sl > 0) sl--; else cs--;
                    }
                    else
                    {
                        lv = "#" + lv;
                        if (sl > 0) sl++; else cs++;
                    }
                    tl.SetValue(lv, i);
                }
                t.Lines = tl;
                if (cs < 0) { cs = 0; }
                Debug.WriteLine("Ss: " + cs + " Sl: " + sl);
                t.SelectionStart = cs;
                t.ScrollToCaret();
                e.SuppressKeyPress = true;
                t.Select(cs, sl);
            }
        }
        #endregion
    }
}
