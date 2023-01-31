using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
namespace FaineSwitcher
{
    /// <summary>
    /// Low level hook.
    /// </summary>
    public static class LLHook
    {
        public static DICT<Keys, Keys> redefines = new DICT<Keys, Keys>(/*
			new []{Keys.CapsLock}, 
			new []{Keys.F18}*/);
        public static List<string> redefines_excl_mods = new List<string>()/*{"all"}*/;
        public static bool _ACTIVE = false;
        public static IntPtr _LLHook_ID = IntPtr.Zero;
        public static WinAPI.LowLevelProc _LLHook_proc = LLHook.Callback;
        static bool alt, alt_r, shift, shift_r, ctrl, ctrl_r, win, win_r;
        static Action dhk_tray_act;
        static string dhk_tray_hk, dhk_tray_hk_real;
        static bool dhk_tray_wait, restarter_running = false;
        static Timer dhk_timer, restarter;
        public static void Set()
        {
            if (!SwitcherUI.ENABLED) return;
            if (_LLHook_ID != IntPtr.Zero)
                UnSet();
            using (Process currProcess = Process.GetCurrentProcess())
            using (ProcessModule currModule = currProcess.MainModule)
                _LLHook_ID = WinAPI.SetWindowsHookEx(WinAPI.WH_KEYBOARD_LL, _LLHook_proc,
                                                     WinAPI.GetModuleHandle(currModule.ModuleName), 0);
            if (_LLHook_ID == IntPtr.Zero)
                Logging.Log("Registering LLHook failed: " + Marshal.GetLastWin32Error(), 1);
            if (restarter != null)
            {
                restarter.Stop();
                restarter.Dispose();
            }
            restarter = new Timer();
            restarter.Interval = 5000;
            restarter.Tick += (_, __) => { Logging.Log("Planed LLHook restart"); restarter_running = true; Set(); };
            if (!restarter_running)
            {
                restarter.Start();
            }
        }
        public static void UnSet()
        {
            var r = WinAPI.UnhookWindowsHookEx(_LLHook_ID);
            if (r)
                _LLHook_ID = IntPtr.Zero;
            else
                Logging.Log("BAD! LLHook unregister failed: " + System.Runtime.InteropServices.Marshal.GetLastWin32Error(), 1);
            if (restarter_running)
            {
                restarter.Stop();
                restarter.Dispose();
                restarter_running = false;
            }
        }
        public static bool[] LMod_act = { false, false, false };
        public static uint[] LMod_layout_pre = { 0, 0, 0 };
        public static bool LMod(Keys k, IntPtr wp)
        {
            var br = false;
            if (SwitcherUI.Layout1ModifierKey == 0 && SwitcherUI.Layout2ModifierKey == 0 && SwitcherUI.LayoutDModifierKey == 0) return br;
            Keys[] x = null;
            try
            {
                x = new[] { (Keys)SwitcherUI.Layout1ModifierKey, (Keys)SwitcherUI.Layout2ModifierKey, (Keys)SwitcherUI.LayoutDModifierKey };
            }
            catch (Exception e) { Logging.Log("LMod error:" + e.Message); return br; }
            for (int i = 0; i != 3; i++)
            {
                if (k == x[i])
                {
                    if ((wp == (IntPtr)WinAPI.WM_KEYDOWN ||
                         wp == (IntPtr)WinAPI.WM_SYSKEYDOWN) && !LMod_act[i])
                    {
                        LMod_act[i] = true;
                        LMod_layout_pre[i] = SwitcherUI.UseJKL ? SwitcherUI.currentLayout : Locales.GetCurrentLocale();
                        Debug.WriteLine("pre layout saved:" + LMod_layout_pre[i] + " key: " + k);
                        KMHook.ChangeToLayout(Locales.ActiveWindow(), i == 0 ? SwitcherUI.MAIN_LAYOUT1 :
                                                                        i == 1 ? SwitcherUI.MAIN_LAYOUT2 :
                          (LMod_layout_pre[i] == SwitcherUI.MAIN_LAYOUT1 ? SwitcherUI.MAIN_LAYOUT2 : SwitcherUI.MAIN_LAYOUT1));
                    }
                    else if ((wp == (IntPtr)WinAPI.WM_KEYUP ||
                                 wp == (IntPtr)WinAPI.WM_SYSKEYUP) && LMod_act[i])
                    {
                        LMod_act[i] = false;
                        KMHook.ChangeToLayout(Locales.ActiveWindow(), LMod_layout_pre[i]);
                        Debug.WriteLine("pre layout restore:" + LMod_layout_pre[i]);
                        LMod_layout_pre[i] = 0;
                    }
                    br = true;
                }
            }
            return br;
        }
        public static IntPtr Callback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (Program.switcher == null || nCode < 0) return WinAPI.CallNextHookEx(_LLHook_ID, nCode, wParam, lParam);
            if (KMHook.ExcludedProgram() && !SwitcherUI.ChangeLayoutInExcluded)
                return WinAPI.CallNextHookEx(_LLHook_ID, nCode, wParam, lParam);
            var vk = Marshal.ReadInt32(lParam);
            var Key = (Keys)vk;
            if (SwitcherUI.BlockAltUpNOW)
            {
                if ((wParam == (IntPtr)WinAPI.WM_SYSKEYUP || wParam == (IntPtr)WinAPI.WM_KEYUP) &&
                    (Key == Keys.LMenu || Key == Keys.RMenu || Key == Keys.Menu))
                {
                    Debug.WriteLine("ihihihihihihihih-hihih-hi blocked alt :)))))");
                    SwitcherUI.BlockAltUpNOW = false;
                    return (IntPtr)1;
                }
            }
            SetModifs(Key, wParam);
            if ((alt || alt) && (shift || shift_r) && Key == Keys.Tab)
            { // mysterious Alt+Shift+Tab, sends Shift Up before Tab down!?
                KMHook.skip_kbd_events++;
            }
            Debug.WriteLine("Alive" + vk + " :: " + wParam);
            #region Redefines
            if (redefines.len > 0)
            {
                var modsstr = KMHook.GetModsStr(ctrl, ctrl_r, shift, shift_r, alt, alt_r, win, win_r).Replace("+", " ").Replace("  ", "").ToLower();
                for (int i = 0; i != redefines.len; i++)
                {
                    if (Key == redefines[i].k)
                    {
                        Logging.Log("[REDEF] > Redefined: " + redefines[i].k + " => " + redefines[i].v);
                        var rli = redefines_excl_mods[i].ToLower();
                        var redy = true;
                        if (rli == "all" && !(!shift && !alt && !ctrl && !win && !shift_r && !alt_r && !ctrl_r && !win_r))
                        {
                            redy = false;
                        }
                        else
                        {
                            var srli = rli.Split(' ');
                            for (int y = 0; y != srli.Length; y++)
                            {
                                if (!String.IsNullOrEmpty(srli[y]) && modsstr.Contains(srli[y]))
                                {
                                    Logging.Log("[REDEF] > Contains the modifier: " + srli[y]);
                                    redy = false;
                                    break;
                                }
                            }
                        }
                        if (redy)
                        {
                            KMHook.KeybdEvent(redefines[i].v, (wParam.ToInt32() == WinAPI.WM_KEYDOWN || wParam.ToInt32() == WinAPI.WM_SYSKEYDOWN) ? 0 : 2);
                            return (IntPtr)1;
                        }
                        else
                        {
                            Logging.Log("[REDEF] > Redefine cancelled: by mods: " + redefines_excl_mods[i] + " (" + modsstr + ")");
                        }
                    }
                }
            }
            #endregion
            #region Test hold-layout
            if (LMod(Key, wParam))
            {
                return (IntPtr)1;
            }
            #endregion
            #region Switcher.mm Tray Hotkeys
            var x = new Tuple<bool, bool, bool, bool, bool, bool, bool, Tuple<bool, int>>(alt, alt_r, shift, shift_r, ctrl, ctrl_r, win, new Tuple<bool, int>(win_r, vk));
            //				Debug.WriteLine("x_hk: " + Hotkey.tray_hk_to_string(x));
            //				Debug.WriteLine("dhk_wait: " +dhk_tray_wait);
            //				Debug.WriteLine("dhk_hk: " +dhk_tray_hk);
            if (dhk_tray_wait)
            {
                var hk = Hotkey.tray_hk_parse(dhk_tray_hk);
                var UpOrDown = OnUpOrDown((Keys)hk.Rest.Item2, wParam);
                if (UpOrDown)
                {
                    var eq = Hotkey.cmp_hotkey(hk, x);
                    //				Debug.WriteLine("dhk_eq: "+eq);
                    if (eq)
                    {
                        Logging.Log("[TR_HK] > Executing action of (double)hotkey: " + dhk_tray_hk_real + " on second hotkey: " + dhk_tray_hk);
                        KMHook.DoSelf(dhk_tray_act, "tray_hotkeys_double");
                        dhk_unset();
                        KMHook.SendModsUp(15, false); // less overkill when whole hotkey is being hold
                        return (IntPtr)1;
                    }
                }
            }
            else
            {
                for (int i = 0; i != SwitcherUI.tray_hotkeys.len; i++)
                {
                    var hk = Hotkey.tray_hk_parse(SwitcherUI.tray_hotkeys[i].k);
                    var UpOrDown = OnUpOrDown((Keys)hk.Rest.Item2, wParam);
                    //					Debug.WriteLine((UpOrDown ? "UP":"DOWN") + " key: " +Key);
                    if (UpOrDown)
                    {
                        if (Hotkey.cmp_hotkey(hk, x))
                        {
                            var d = Hotkey.tray_hk_is_double(SwitcherUI.tray_hotkeys[i].k);
                            if (d.Item1)
                            {
                                dhk_tray_wait = true;
                                dhk_tray_hk = d.Item3;
                                dhk_tray_act = SwitcherUI.tray_hotkeys[i].v.Item1;
                                dhk_tray_hk_real = SwitcherUI.tray_hotkeys[i].k;
                                if (dhk_timer != null)
                                {
                                    dhk_timer.Stop();
                                    dhk_timer.Dispose();
                                }
                                dhk_timer = new Timer();
                                dhk_timer.Interval = d.Item2;
                                dhk_timer.Tick += (_, __) => { Debug.WriteLine("Unset timer dhk! " + dhk_timer.Interval + "ms"); dhk_unset(); dhk_timer.Stop(); dhk_timer.Dispose(); };
                                dhk_timer.Start();
                            }
                            else
                            {
                                Logging.Log("[TR_HK] > Executing action of hotkey: " + SwitcherUI.tray_hotkeys[i].k);
                                dhk_unset();
                                if (SwitcherUI.tray_hotkeys[i].v.Item2.Contains("hk|c") || SwitcherUI.tray_hotkeys[i].v.Item2.Contains("hk|s"))
                                {
                                    var altl = ((hk.Item1 || hk.Rest.Item2 == (int)Keys.LMenu) && !hk.Item2 && // l alt not r alt
                                        !hk.Item3 && !hk.Item4 &&
                                        !hk.Item5 && !hk.Item6 &&
                                        !hk.Item7 && !hk.Rest.Item1);
                                    var altr = (!hk.Item1 && (hk.Item2 || hk.Rest.Item2 == (int)Keys.RMenu) &&
                                        !hk.Item3 && !hk.Item4 &&
                                        !hk.Item5 && !hk.Item6 &&
                                        !hk.Item7 && !hk.Rest.Item1);
                                    if (altl || altr)
                                    {
                                        if (Locales.ActiveWindowClassName(40).Contains("Chrome_WidgetWin"))
                                        {
                                            SwitcherUI.chrome_window_alt_fix();
                                        }
                                    }
                                    else
                                    {
                                        KMHook.SendModsUp(15, false); // less overkill when whole hotkey is being hold
                                    }
                                    if (altl)
                                    {
                                        KMHook.KeybdEvent(Keys.LMenu, 0);
                                        KMHook.KeybdEvent(Keys.LMenu, 2);
                                    }
                                    if (altr)
                                    {
                                        KMHook.KeybdEvent(Keys.RMenu, 0);
                                        KMHook.KeybdEvent(Keys.RMenu, 2);
                                    }
                                    // todo fix, win keys can't be send up
                                    if ((!hk.Item1 && !hk.Item2 &&
                                        !hk.Item3 && !hk.Item4 &&
                                        !hk.Item5 && !hk.Item6 &&
                                        (hk.Item7 || hk.Rest.Item2 == (int)Keys.LWin) && !hk.Rest.Item1))
                                    {
                                        //										KMHook.KeybdEvent(Keys.LWin, 0);
                                        //										KMHook.KeybdEvent(Keys.LWin, 2);
                                    }
                                    if ((!hk.Item1 && !hk.Item2 &&
                                        !hk.Item3 && !hk.Item4 &&
                                        !hk.Item5 && !hk.Item6 &&
                                        !hk.Item7 && (hk.Rest.Item1 || hk.Rest.Item2 == (int)Keys.RWin)))
                                    {
                                        //										KMHook.KeybdEvent(Keys.RWin, 0);
                                        //										KMHook.KeybdEvent(Keys.RWin, 2);
                                    }
                                }
                                KMHook.DoSelf(SwitcherUI.tray_hotkeys[i].v.Item1, "tray_hotkeys");
                                return (IntPtr)1;
                            }
                        }
                    }
                }
            }
            #endregion
            if (SwitcherUI.SnippetsEnabled)
            {
                if (KMHook.c_snip.Count > 0)
                {
                    var t = Program.switcher.SnippetsExpandType == "Tab";
                    if (t && Key == Keys.Tab && !shift && !alt && !win && !ctrl && !shift_r && !alt_r && !ctrl_r && !win_r)
                    {
                        WinAPI.keybd_event((byte)Keys.F14, (byte)Keys.F14, 0, 0);
                        return (IntPtr)1; // Disable event
                    }
                    if (!t && Program.switcher.SnippetsExpandType != "Space" && wParam == (IntPtr)WinAPI.WM_KEYDOWN || wParam == (IntPtr)WinAPI.WM_SYSKEYDOWN)
                    {
                        var ms = KMHook.GetModsStr(ctrl, ctrl_r, shift, shift_r, alt, alt_r, win, win_r);
                        ms += Key;
                        var othmatch = ms == Program.switcher.SnippetsExpKeyOther;
                        Debug.WriteLine("Checking SnippetsExpOther: [" + ms + "] == [" + Program.switcher.SnippetsExpKeyOther + "] => " + othmatch);
                        if (othmatch)
                        {
                            KMHook.ClearModifiers();
                            WinAPI.keybd_event((byte)Keys.F20, (byte)Keys.F20, 0, 0);
                            return (IntPtr)1;
                        }
                    }
                }
            }
            if (SwitcherUI.RemapCapslockAsF18)
            {
                bool _shift = !shift, _alt = !alt, _ctrl = !ctrl, _win = !win, _shift_r = !shift_r, _alt_r = !alt_r, _ctrl_r = !ctrl_r, _win_r = !win_r;
                if (Key == Keys.CapsLock)
                {
                    for (int i = 1; i != 5; i++)
                    {
                        var KeyIndex = (int)typeof(SwitcherUI).GetField("Key" + i).GetValue(Program.switcher);
                        if (KeyIndex == 8)
                        { // Shift+CapsLock
                            _shift = shift;
                            _shift_r = shift_r;
                        }
                    }
                }
                uint mods = 0;
                if (alt || alt_r) mods += WinAPI.MOD_ALT;
                if (ctrl || ctrl_r) mods += WinAPI.MOD_CONTROL;
                if (shift || shift_r) mods += WinAPI.MOD_SHIFT;
                if (win || win_r) mods += WinAPI.MOD_WIN;
                bool has = Program.switcher.HasHotkey(new Hotkey(false, (uint)Keys.F18, mods, 77));
                if (has)
                {
                    if (Hotkey.ContainsModifier((int)mods, (int)WinAPI.MOD_SHIFT))
                        _shift = shift;
                    _shift_r = shift_r;
                    if (Hotkey.ContainsModifier((int)mods, (int)WinAPI.MOD_ALT))
                        _alt = alt;
                    _alt_r = alt_r;
                    if (Hotkey.ContainsModifier((int)mods, (int)WinAPI.MOD_CONTROL))
                        _ctrl = ctrl;
                    _ctrl_r = ctrl_r;
                    if (Hotkey.ContainsModifier((int)mods, (int)WinAPI.MOD_WIN))
                        _win = win;
                    _win_r = win_r;
                }
                var GJIME = false;
                if (vk >= 240 && vk <= 242) // GJ IME's Shift/Alt/Ctrl + CapsLock
                    GJIME = true;
                //			Debug.WriteLine(Key + " " +has + "// " + _shift + " " + _alt + " " + _ctrl + " " + _win + " " + mods + " >> " + (Key == Keys.CapsLock && _shift && _alt && _ctrl && _win));
                if ((Key == Keys.CapsLock || GJIME) && _shift && _alt && _ctrl && _win && _shift_r && _alt_r && _ctrl_r && _win_r)
                {
                    var flags = (int)(KInputs.IsExtended(Key) ? WinAPI.KEYEVENTF_EXTENDEDKEY : 0);
                    if (wParam == (IntPtr)WinAPI.WM_KEYUP)
                        flags |= (int)WinAPI.KEYEVENTF_KEYUP;
                    WinAPI.keybd_event((byte)Keys.F18, (byte)Keys.F18, flags, 0);
                    return (IntPtr)1; // Disable event
                }
                //			Debug.WriteLine(Marshal.GetLastWin32Error());
            }
            return WinAPI.CallNextHookEx(_LLHook_ID, nCode, wParam, lParam);
        }
        static void SetModifs(Keys Key, IntPtr msg)
        {
            switch (Key)
            {
                case Keys.LShiftKey:
                    shift = ((msg == (IntPtr)WinAPI.WM_SYSKEYDOWN) ? true : false) || ((msg == (IntPtr)WinAPI.WM_KEYDOWN) ? true : false);
                    break;
                case Keys.RShiftKey:
                    //				case Keys.ShiftKey:
                    shift_r = ((msg == (IntPtr)WinAPI.WM_SYSKEYDOWN) ? true : false) || ((msg == (IntPtr)WinAPI.WM_KEYDOWN) ? true : false);
                    break;
                case Keys.RControlKey:
                    ctrl_r = ((msg == (IntPtr)WinAPI.WM_SYSKEYDOWN) ? true : false) || ((msg == (IntPtr)WinAPI.WM_KEYDOWN) ? true : false);
                    break;
                case Keys.LControlKey:
                    //				case Keys.ControlKey:
                    ctrl = ((msg == (IntPtr)WinAPI.WM_SYSKEYDOWN) ? true : false) || ((msg == (IntPtr)WinAPI.WM_KEYDOWN) ? true : false);
                    break;
                case Keys.RMenu:
                    alt_r = ((msg == (IntPtr)WinAPI.WM_SYSKEYDOWN) ? true : false) || ((msg == (IntPtr)WinAPI.WM_KEYDOWN) ? true : false);
                    break;
                case Keys.LMenu:
                    //				case Keys.Menu:
                    alt = ((msg == (IntPtr)WinAPI.WM_SYSKEYDOWN) ? true : false) || ((msg == (IntPtr)WinAPI.WM_KEYDOWN) ? true : false);
                    break;
                case Keys.RWin:
                    win_r = ((msg == (IntPtr)WinAPI.WM_SYSKEYDOWN) ? true : false) || ((msg == (IntPtr)WinAPI.WM_KEYDOWN) ? true : false);
                    break;
                case Keys.LWin:
                    win = ((msg == (IntPtr)WinAPI.WM_SYSKEYDOWN) ? true : false) || ((msg == (IntPtr)WinAPI.WM_KEYDOWN) ? true : false);
                    break;
            }
        }
        public static void ClearModifiers()
        {
            alt = shift = ctrl = win = alt_r = shift_r = ctrl_r = win_r = false;
        }
        public static void SetModifier(uint Mod, bool down, bool left = true)
        {
            if (Mod == WinAPI.MOD_WIN)
                if (left)
                    win = down;
                else
                    win_r = down;
            if (Mod == WinAPI.MOD_SHIFT)
                if (left)
                    shift = down;
                else
                    shift_r = down;
            if (Mod == WinAPI.MOD_ALT)
                if (left)
                    alt = down;
                else
                    alt_r = down;
            if (Mod == WinAPI.MOD_CONTROL)
                if (left)
                    ctrl = down;
                else
                    ctrl_r = down;
        }
        static bool OnUpOrDown(Keys k, IntPtr wParam)
        {
            if (Hotkey.KeyIsModifier(k))
                return (wParam == (IntPtr)WinAPI.WM_KEYUP || wParam == (IntPtr)WinAPI.WM_SYSKEYUP);
            return (wParam == (IntPtr)WinAPI.WM_KEYDOWN || wParam == (IntPtr)WinAPI.WM_SYSKEYDOWN);
        }
        static void dhk_unset()
        {
            dhk_tray_wait = false;
            dhk_tray_hk = dhk_tray_hk_real = "";
            dhk_tray_act = null;
        }
    }
}
