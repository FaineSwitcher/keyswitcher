using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MLSwitcher;

namespace FaineSwitcher
{
    static class KMHook
    { // Keyboard & Mouse Listeners & Event hook
        #region Variables
        public static string __ANY__ = "***ANY***", REGEXSNIP = "regex/", IGNLAYSNIP = "?~?", last_snip, snip_selection,
                            AS_IGN_RULES;
        public static bool win, alt, ctrl, shift,
            win_r, alt_r, ctrl_r, shift_r,
            shiftRP, ctrlRP, altRP, winRP, //RP = Re-Press
            awas, swas, cwas, wwas, afterEOS, afterEOL, //*was = alt/shift/ctrl was
            keyAfterCTRL, keyAfterALT, keyAfterALTGR, keyAfterSHIFT,
            keyAfterCTRLSHIFT, keyAfterALTSHIFT,
            clickAfterCTRL, clickAfterALT, clickAfterSHIFT,
            hotkeywithmodsfired, csdoing, incapt, waitfornum,
            IsHotkey, ff_chr_wheeled, preSnip, LMB_down, RMB_down, MMB_down,
            dbl_click, click, selfie, aftsingleAS, JKLERR, JKLERRchecking, last_snipANY,
            _selis, _mselis, snipselshiftpressed, snipselwassel,
            AS_IGN_BACK, AS_IGN_DEL, AS_IGN_LS, was_back, was_del, was_ls, __setsnip, L_DOWN,
            CLW_W_SPACE, CLW_W_ENTER, CTRL_ALT_changelayout_temporary, CTRL_ALT_Layout_loaded;
        public static uint CTRL_ALT_prev_layout;
        public static string AS_END_symbols = "";
        public static System.Timers.Timer click_reset = new System.Timers.Timer();
        public static Keys skip_up = Keys.None, prev_up = Keys.None;
        public static System.Timers.Timer JKLERRT = new System.Timers.Timer();
        public static int skip_mouse_events, skip_spec_keys, cursormove = -1, guess_tries, skip_kbd_events, lsnip_noset, AS_IGN_TIMEOUT;
        static char sym = '\0'; static bool sym_upr = false;
        static uint as_lword_layout = 0;
        public static uint last_switch_layout = 0;
        static uint cs_layout_last = 0;
        static string busy_on = "", lastLWClearReason = "";
        public static NativeClipboard.clip lastClip;
        public static string lastClipText;
        public static string symbolclear;
        static List<Keys> tempNumpads = new List<Keys>();
        static Keys preKey = Keys.None, prevKEY; //, seKeyDown = Keys.None, aseKeyDown = Keys.None;
        static Keys altwait = Keys.None;
        public static List<char> c_snip = new List<char>();
        public static System.Windows.Forms.Timer doublekey = new System.Windows.Forms.Timer();
        public static System.Timers.Timer AS_IGN_RESET = null;
        public static List<YuKey> c_word_backup = new List<YuKey>();
        public static List<YuKey> c_word_backup_last = new List<YuKey>();
        public static List<IntPtr> PLC_HWNDs = new List<IntPtr>();
        /// <summary> Created for faster check if program is excluded, when checkin too many times(in hooks, timers etc.). </summary>
        public static List<IntPtr> EXCLUDED_HWNDs = new List<IntPtr>();
        public static Stopwatch pif = new Stopwatch();
        public static List<IntPtr> NOT_EXCLUDED_HWNDs = new List<IntPtr>();
        public static List<IntPtr> AS_NOT_EXCLUDED_HWNDs = new List<IntPtr>();
        public static List<IntPtr> SNI_NOT_EXCLUDED_HWNDs = new List<IntPtr>();
        public static List<IntPtr> AS_EXCLUDED_HWNDs = new List<IntPtr>();
        public static List<IntPtr> SNI_EXCLUDED_HWNDs = new List<IntPtr>();
        public static List<IntPtr> ConHost_HWNDs = new List<IntPtr>();
        public static string[] snipps = new[] { "switcher", "eml" };
        public static string[] exps = new[] {
            "",
            ""
        };
        public static NCR[] NCRules = new NCR[] { };
        public static string[] as_wrongs;
        public static string[] as_corrects;
        static DICT<string, string> DefaultTransliterationDict = new DICT<string, string>(new Dictionary<string, string>() {
                {"Щ", "SCH"}, {"щ", "sch"}, {"Ч", "CH"}, {"Ш", "SH"}, {"Ё", "JO"}, {"ВВ", "W"},
                {"Є", "EH"}, {"ю", "yu"}, {"я", "ya"}, {"є", "eh"}, {"Ж", "ZH"},
                {"ч", "ch"}, {"ш", "sh"}, {"Й", "JJ"}, {"ж", "zh"},
                {"Э", "EH"}, {"Ю", "YU"}, {"Я", "YA"}, {"й", "jj"}, {"ё", "jo"},
                {"э", "eh"}, {"вв", "w"}, {"кь", "q"}, {"КЬ", "Q"},
                {"ь", "j"}, {"№", "#"}, {"А", "A"}, {"Б", "B"},
                {"В", "V"}, {"Г", "G"}, {"Д", "D"}, {"Е", "E"}, {"З", "Z"},
                {"И", "I"}, {"К", "K"}, {"Л", "L"}, {"М", "M"}, {"Н", "N"},
                {"О", "O"}, {"П", "P"}, {"Р", "R"}, {"С", "S"}, {"Т", "T"},
                {"У", "U"}, {"Ф", "F"}, {"Х", "H"}, {"Ц", "C"}, {"Ъ", "'"},
                {"а", "a"}, {"б", "b"}, {"в", "v"}, {"г", "g"}, {"д", "d"},
                {"з", "z"}, {"и", "i"}, {"к", "k"}, {"л", "l"}, {"м", "m"},
                {"н", "n"}, {"о", "o"}, {"п", "p"}, {"р", "r"}, {"с", "s"},
                {"у", "u"}, {"ф", "f"}, {"х", "h"}, {"ц", "c"}, {"ъ", ":"},
                {"Ы", "Y"}, {"Ь", "J"}, {"е", "e"}, {"т", "t"}, {"ы", "y"}
        });
        static DICT<string, string> LayReplDict = new DICT<string, string>(new Dictionary<string, string>() {
            {"ä", "э"},{"э", "ä"},{"ö", "ж"},{"ж", "ö"},
            {"ü", "х"},{"х", "ü"},{"Ä", "Э"},{"Э", "Ä"},
            {"Ö", "Ж"},{"Ж", "Ö"},{"Ü", "Х"},{"Х", "Ü"},
            {"Я", "Y"},{"Y", "Я"},{"Н", "Z"},{"Z", "Н"},
            {"я", "y"},{"y", "я"},{"н", "z"},{"z", "н"},
            {"-", "ß"}
        });
        static DICT<string, string> ASsymDiffDICT = new DICT<string, string>(new Dictionary<string, string>() {
             {"z", "y"}, { "Z", "Y" }
        });
        static DICT<string, string> CustomConversionDICT = new DICT<string, string>(new Dictionary<string, string>() {
            {"abc", "xyz"}, { "s/^hold/bold/", "" }
        });
        static DICT<string, string> transliterationDict = DefaultTransliterationDict;
        static DICT<int, DICT<int, int>> LKDict;
        #endregion
        #region Keyboard, Mouse & Event hooks callbacks
        public static void ListenKeyboard(int vkCode, uint MSG, short Flags = 0)
        {
            if (skip_kbd_events > 0)
            {
                skip_kbd_events--;
                return;
            }
            if (skip_up != Keys.None)
            {
                Debug.WriteLine("Skip up!" + skip_up);
                if (MSG == WinAPI.WM_KEYUP || MSG == WinAPI.WM_SYSKEYUP)
                {
                    if (vkCode == (int)skip_up)
                    {
                        skip_up = Keys.None;
                    }
                }
                return;
            }
            if (altwait != Keys.None)
            {
                if (alt) { alt = IsKDown(Keys.LMenu); }
                if (alt_r) { alt_r = IsKDown(Keys.RMenu); }
                if (shift) { shift = IsKDown(Keys.LShiftKey); }
                if (shift_r) { shift_r = IsKDown(Keys.RShiftKey); }
                if (ctrl) { ctrl = IsKDown(Keys.LControlKey); }
                if (ctrl_r) { ctrl_r = IsKDown(Keys.RControlKey); }
                if (win) { win = IsKDown(Keys.LWin); }
                if (win_r) { win_r = IsKDown(Keys.RWin); }
                if (!alt && !alt_r)
                {
                    altwait = Keys.None;
                }
                else
                {
                    return;
                }
            }
            if (SwitcherUI.CaretLangTooltipEnabled)
                ff_chr_wheeled = false;
            if (vkCode > 254) return;
            var down = (MSG == WinAPI.WM_SYSKEYDOWN) || (MSG == WinAPI.WM_KEYDOWN);
            var Key = (Keys)vkCode; // "Key" will further be used instead of "(Keys)vkCode"
            if ((alt || alt_r) && Key == Keys.Tab)
            {
                altwait = Keys.None;
                if (alt) altwait = Keys.LMenu;
                if (alt_r) altwait = Keys.RMenu;
            }
            if (Program.c_words.Count == 0)
            {
                Program.c_words.Add(new List<YuKey>());
            }
            if ((Key < Keys.D0 || Key > Keys.D9) && waitfornum && (uint)Key != Program.switcher.HKConMorWor.VirtualKeyCode && down)
                Program.switcher.FlushConvertMoreWords();
            #region Checks modifiers that are down
            switch (Key)
            {
                case Keys.LShiftKey: shift = down; break;
                case Keys.LControlKey: ctrl = down; break;
                case Keys.LMenu: alt = down; break;
                case Keys.LWin: win = down; break;
                case Keys.RShiftKey: shift_r = down; break;
                case Keys.RControlKey: ctrl_r = down; break;
                case Keys.RMenu: alt_r = down; break;
                case Keys.RWin: win_r = down; break;
            }
            //			shift = IsKDown(Keys.LShiftKey);
            //			shift_r = IsKDown(Keys.RShiftKey);
            //			ctrl = IsKDown(Keys.LControlKey);
            //			ctrl_r = IsKDown(Keys.RControlKey);
            //			alt = IsKDown(Keys.LMenu);
            //			alt_r = IsKDown(Keys.RMenu);
            //			win = IsKDown(Keys.LWin);
            //			win_r = IsKDown(Keys.RWin);
            // Additional fix for scroll tip.
            if (SwitcherUI.ScrollTip && Key == Keys.Scroll && down)
            {
                DoSelf(() =>
                {
                    KeybdEvent(Keys.Scroll, 0);
                    KeybdEvent(Keys.Scroll, 2);
                }, "scroll_tip_fix");
            }
            uint mods = 0;
            if (alt || alt_r)
                mods += WinAPI.MOD_ALT;
            if (ctrl || ctrl_r)
                mods += WinAPI.MOD_CONTROL;
            if (shift || shift_r)
                mods += WinAPI.MOD_SHIFT;
            if (win || win_r)
                mods += WinAPI.MOD_WIN;
            if (Program.switcher.HasHotkey(new Hotkey(false, (uint)Key, mods, 77)))
            {
                IsHotkey = true;
            }
            else
                IsHotkey = false;
            //			Console.WriteLine("Pressed hotkey?: "+IsHotkey+" => ["+Key+"+"+mods+"] .");
            if ((Key >= Keys.D0 || Key <= Keys.D9) && waitfornum)
                IsHotkey = true;
            if (SwitcherUI.OnceSpecific && !down)
            {
                SwitcherUI.OnceSpecific = false;
            }
            if (SwitcherUI.__selection)
            {
                Debug.WriteLine("SHC: " + snipselshiftpressed + ", SW: " + snipselwassel + ", " + shift + " +" + shift_r);
                if (snipselwassel && snipselshiftpressed && (!shift && !shift_r))
                {
                    if (!down)
                    {
                        switch (Key)
                        {
                            case Keys.LShiftKey:
                            case Keys.RShiftKey:
                                snipsel();
                                break;
                        }
                    }
                    snipselwassel = snipselshiftpressed = false;
                }
                if (shift || shift_r)
                {
                    snipselshiftpressed = true;
                    switch (Key)
                    {
                        case Keys.PageDown:
                        case Keys.PageUp:
                        case Keys.Up:
                        case Keys.Down:
                        case Keys.Left:
                        case Keys.Right:
                        case Keys.Home:
                        case Keys.End:
                            snipselwassel = true;
                            break;
                    }
                }
            }
            var printable = ((Key >= Keys.D0 && Key <= Keys.Z) || // This is 0-9 & A-Z
                             Key >= Keys.Oem1 && Key <= Keys.OemBackslash || // Other printable
                            (Control.IsKeyLocked(Keys.NumLock) && ( // while numlock is on
                             Key >= Keys.NumPad0 && Key <= Keys.NumPad9)) || // Numpad numbers 
                             Key == Keys.Decimal || Key == Keys.Subtract || Key == Keys.Multiply ||
                             Key == Keys.Divide || Key == Keys.Add); // Numpad symbols
            var printable_mod = !win && !win_r && !alt && !alt_r && !ctrl && !ctrl_r; // E.g. only shift is PrintAble
                                                                                      //Key log
            Logging.Log("[KEY] > Catched Key=[" + Key + "] with VKCode=[" + vkCode + "] and message=[" + (int)MSG + "], modifiers=[" +
                        (shift ? "L-Shift" : "") + (shift_r ? "R-Shift" : "") +
                        (alt ? "L-Alt" : "") + (alt_r ? "R-Alt" : "") +
                        (ctrl ? "L-Ctrl" : "") + (ctrl_r ? "R-Ctrl" : "") +
                        (win ? "L-Win" : "") + (win_r ? "R-Win" : "") + "].");
            // Anti C-A-DEL C & A stuck rule
            if (Key == Keys.Delete)
            {
                if (ctrl && alt)
                    ctrl = alt = false;
                if (ctrl && alt_r)
                    ctrl = alt_r = false;
                if (ctrl_r && alt_r)
                    ctrl_r = alt_r = false;
                if (ctrl_r && alt)
                    ctrl_r = alt = false;
            }
            // Anti win-stuck rule
            if (Key == Keys.L)
            {
                L_DOWN = MSG == WinAPI.WM_KEYDOWN || MSG == WinAPI.WM_SYSKEYDOWN;
            }
            if (Key == Keys.LWin)
            {
                win = MSG == WinAPI.WM_KEYDOWN || MSG == WinAPI.WM_SYSKEYDOWN;
            }
            if (Key == Keys.RWin)
            {
                win_r = MSG == WinAPI.WM_KEYDOWN || MSG == WinAPI.WM_SYSKEYDOWN;
            }
            if ((preKey == Keys.LWin && Key != Keys.LWin) || (preKey == Keys.RWin && Key != Keys.RWin))
            {
                preKey = Keys.None;
                Debug.WriteLine("Fix stuck preKey Win");
            }
            if (L_DOWN && (win || win_r))
            {
                preKey = Keys.None;
                L_DOWN = win = win_r = false;
                LLHook.ClearModifiers();
                return;
            }
            // Clear currentLayout in Program.switcher rule
            if (!SwitcherUI.UseJKL || KMHook.JKLERR)
                if (((win || alt || ctrl || win_r || alt_r || ctrl_r) && Key == Keys.Tab) ||
                    win && (Key != Keys.None &&
                            Key != Keys.LWin &&
                            Key != Keys.RWin)) // On any Win+[AnyKey] hotkey
                    SwitcherUI.currentLayout = 0;
            if (!down && (
                ((alt || ctrl || alt_r || ctrl_r) && (Key == Keys.Shift || Key == Keys.LShiftKey || Key == Keys.RShiftKey)) ||
                 shift && (Key == Keys.Menu || Key == Keys.LMenu || Key == Keys.RMenu) ||
                 (IfNW7() && (win || win_r) && Key == Keys.Space)))
            {
                if (!SwitcherUI.UseJKL || KMHook.JKLERR)
                {
                    var time = 200;
                    if (IfNW7())
                        time = 50;
                    SwitcherUI.currentLayout = 0;
                    as_lword_layout = 0;
                    DoLater(() => { SwitcherUI.GlobalLayout = SwitcherUI.currentLayout = Locales.GetCurrentLocale(); AS_IGN_fun(); }, time);
                }
            }
            #endregion
            if (SwitcherUI.CTRL_ALT_TemporaryLayout != 0)
            {
                if (down)
                {
                    if (((Key == Keys.LMenu && ctrl) ||
                        ((Key == Keys.LControlKey) && alt)) && !CTRL_ALT_changelayout_temporary)
                    {
                        CTRL_ALT_prev_layout = (SwitcherUI.UseJKL && !JKLERR) ? SwitcherUI.currentLayout : Locales.GetCurrentLocale();
                        uint sh1 = CTRL_ALT_prev_layout & 0xffff, sh2 = SwitcherUI.CTRL_ALT_TemporaryLayout & 0xffff;
                        uint lo1 = CTRL_ALT_prev_layout >> 16, lo2 = SwitcherUI.CTRL_ALT_TemporaryLayout >> 16;
                        if (!(sh1 == sh2 && lo1 == lo2) && CTRL_ALT_prev_layout != 0)
                        {
                            CTRL_ALT_changelayout_temporary = true;
                            var is_loaded = false;
                            foreach (var l in Program.locales)
                            {
                                if ((l.uId & 0xffff) == sh2 && (l.uId >> 16) == lo2)
                                {
                                    is_loaded = true;
                                    break;
                                }
                            }
                            if (!is_loaded)
                            {
                                var x = SwitcherUI.CTRL_ALT_TemporaryLayout.ToString("X");
                                if (x.Length < 7)
                                {
                                    var zeroes = "";
                                    for (int i = 0; i != 7 - x.Length; i++)
                                    {
                                        zeroes += "0";
                                    }
                                    x = zeroes + x;
                                }
                                Logging.Log("[LCTRLLALT] > Loading layout: " + x);
                                WinAPI.LoadKeyboardLayout(x, 1);
                                Thread.Sleep(15);
                                CTRL_ALT_Layout_loaded = true;
                            }
                            NormalChangeToLayout(Locales.ActiveWindow(), SwitcherUI.CTRL_ALT_TemporaryLayout);
                            Logging.Log("[LCTRLLALT] > SWITCH TO LAYOUT" + SwitcherUI.CTRL_ALT_TemporaryLayout);
                        }
                    }
                }
                if (MSG == WinAPI.WM_KEYUP || MSG == WinAPI.WM_SYSKEYUP)
                {
                    Debug.WriteLine("RELEASE: " + Key + " " + CTRL_ALT_prev_layout);
                    if (CTRL_ALT_changelayout_temporary)
                    {
                        var swtch_back = false;
                        if (Key == Keys.LControlKey)
                        {
                            if (prev_up == Keys.LMenu)
                            {
                                swtch_back = true;
                            }
                            prev_up = Key;
                        }
                        if (Key == Keys.LMenu)
                        {
                            if (prev_up == Keys.LControlKey)
                            {
                                swtch_back = true;
                            }
                            prev_up = Key;
                        }
                        if (swtch_back)
                        {
                            if (CTRL_ALT_prev_layout != 0)
                            {
                                if (CTRL_ALT_Layout_loaded)
                                {
                                    var success = WinAPI.UnloadKeyboardLayout((IntPtr)SwitcherUI.CTRL_ALT_TemporaryLayout);
                                    Logging.Log("[LCTRLLALT] > Unload layout: " + SwitcherUI.CTRL_ALT_TemporaryLayout + " success: " + success);
                                }
                                Logging.Log("[LCTRLLALT] > SWITCH BACK TO LAYOUT" + CTRL_ALT_prev_layout);
                                NormalChangeToLayout(Locales.ActiveWindow(), CTRL_ALT_prev_layout);
                            }
                            prev_up = Keys.None;
                            CTRL_ALT_changelayout_temporary = false;
                            CTRL_ALT_prev_layout = 0;
                        }
                    }
                }
            }
            #region
            if (SwitcherUI.LangPanelDisplay || SwitcherUI.MouseLangTooltipEnabled || SwitcherUI.CaretLangTooltipEnabled)
                if (SwitcherUI.LangPanelUpperArrow || SwitcherUI.mouseLTUpperArrow || SwitcherUI.caretLTUpperArrow)
                {
                    sym = getSym(vkCode);
                }
            var ku = IsUpperInput();
            if (SwitcherUI.LangPanelDisplay)
                if (SwitcherUI.LangPanelUpperArrow)
                    Program.switcher._langPanel.DisplayUpper(ku);
            if (SwitcherUI.MouseLangTooltipEnabled)
                if (SwitcherUI.mouseLTUpperArrow)
                    Program.switcher.mouseLangDisplay.DisplayUpper(ku);
            if (SwitcherUI.CaretLangTooltipEnabled)
                if (SwitcherUI.caretLTUpperArrow)
                    Program.switcher.caretLangDisplay.DisplayUpper(ku);
            #endregion
            #region InputHistory
            if (SwitcherUI.WriteInputHistory)
            {
                if ((printable || Key == Keys.Enter || Key == Keys.Space) && printable_mod && down)
                {
                    if (sym == '\0') sym = getSym(vkCode);
                    WriteToHistory(sym);
                }
                if (Key == Keys.Back && printable_mod && down)
                {
                    if (SwitcherUI.InputHistoryBackSpaceWriteType == 0)
                    {
                        WriteToHistory("<Back>");
                    }
                    else
                        RemLastHistory();
                }
            }
            #endregion
            #region Release Re-Pressed keys
            if (hotkeywithmodsfired && !down &&
               ((Key == Keys.LShiftKey || Key == Keys.LMenu || Key == Keys.LControlKey || Key == Keys.LWin) ||
                 (Key == Keys.RShiftKey || Key == Keys.RMenu || Key == Keys.RControlKey || Key == Keys.RWin)))
            {
                hotkeywithmodsfired = false;
                mods = 0;
                if (cwas)
                {
                    cwas = false;
                    mods += WinAPI.MOD_CONTROL;
                }
                if (swas)
                {
                    swas = false;
                    mods += WinAPI.MOD_SHIFT;
                }
                if (awas)
                {
                    awas = false;
                    mods += WinAPI.MOD_ALT;
                }
                if (wwas)
                {
                    wwas = false;
                    mods += WinAPI.MOD_WIN;
                }
                SendModsUp((int)mods, false);
            }
            #endregion
            #region One key layout switch
            if (!down)
                if (Key == Keys.LControlKey || Key == Keys.RControlKey)
                    clickAfterCTRL = false;
            if (Key != Keys.LMenu && Key != Keys.RMenu)
                clickAfterALT = false;
            if (Key != Keys.LShiftKey && Key != Keys.RShiftKey)
                clickAfterSHIFT = false;
            if (SwitcherUI.ChangeLayouByKey)
            {
                if (((Key == Keys.LControlKey || Key == Keys.RControlKey) && !SwitcherUI.CtrlInHotkey) ||
                    ((Key == Keys.LShiftKey || Key == Keys.RShiftKey) && !SwitcherUI.ShiftInHotkey) ||
                    ((Key == Keys.LMenu || Key == Keys.RMenu) && !SwitcherUI.AltInHotkey) ||
                    ((Key == Keys.LWin || Key == Keys.RWin) && !SwitcherUI.WinInHotkey) ||
                    Key == Keys.CapsLock || Key == Keys.F18 || vkCode == 240 || Key == Keys.Tab)
                {
                    SpecificKey(Key, MSG, vkCode);
                }
                if ((ctrl || ctrl_r) && (Key != Keys.LControlKey && Key != Keys.RControlKey && Key != Keys.ControlKey || clickAfterCTRL))
                    keyAfterCTRL = true;
                else
                    keyAfterCTRL = false;
                if ((ctrl || ctrl_r) && (shift || shift_r) &&
                    (Key != Keys.LControlKey && Key != Keys.RControlKey && Key != Keys.ControlKey)
                    && (Key != Keys.LShiftKey && Key != Keys.RShiftKey && Key != Keys.Shift))
                    keyAfterCTRLSHIFT = true;
                else
                    keyAfterCTRLSHIFT = false;
                if ((alt || alt_r) && (shift || shift_r) &&
                    (Key != Keys.LMenu && Key != Keys.RMenu && Key != Keys.Menu)
                    && (Key != Keys.LShiftKey && Key != Keys.RShiftKey && Key != Keys.Shift))
                    keyAfterALTSHIFT = true;
                else
                    keyAfterALTSHIFT = false;
                if ((alt || alt_r) && (Key != Keys.LMenu && Key != Keys.RMenu && Key != Keys.Menu || clickAfterALT))
                    keyAfterALT = true;
                else
                    keyAfterALT = false;
                if (((alt || alt_r) && (ctrl || ctrl_r)) &&
                    (Key != Keys.LMenu && Key != Keys.RMenu && Key != Keys.Menu || clickAfterALT) &&
                    (Key != Keys.LControlKey && Key != Keys.RControlKey && Key != Keys.Control || clickAfterCTRL))
                    keyAfterALTGR = true;
                else
                    keyAfterALTGR = false;
                if ((shift || shift_r) && (Key != Keys.LShiftKey && Key != Keys.RShiftKey && Key != Keys.Shift || clickAfterSHIFT))
                    keyAfterSHIFT = true;
                else
                    keyAfterSHIFT = false;
            }
            if (MSG == WinAPI.WM_KEYDOWN || MSG == WinAPI.WM_SYSKEYDOWN)
            {
                if (preKey == Keys.None)
                {
                    preKey = Key;
                    //Debug.WriteLine("PREKEY: " +preKey);
                }
            }
            if (MSG == WinAPI.WM_KEYUP || MSG == WinAPI.WM_SYSKEYUP)
            {
                if ((int)Key == (int)preKey)
                {
                    //Debug.WriteLine("PREKEY-OFF: " +preKey);
                    preKey = Keys.None;
                }
            }
            #endregion
            if ((ctrl || win || alt || ctrl_r || win_r || alt_r) && Key == Keys.Tab)
            {
                ClearWord(true, true, true, "Any modifier + Tab", true, AS_IGN_RULES.Contains("W"));
                SwitcherUI.CCReset("mod+tab");
            }
            #region Other, when KeyDown
            if (MSG == WinAPI.WM_KEYDOWN && !waitfornum && !IsHotkey)
            {
                if (Key == Keys.Back)
                { //Removes last item from current word when user press Backspace
                    SwitcherUI.CCReset("back");
                    if (Program.c_word.Count != 0)
                    {
                        Program.c_word.RemoveAt(Program.c_word.Count - 1);
                    }
                    if (Program.c_words.Count > 0)
                    {
                        if (Program.c_words[Program.c_words.Count - 1].Count - 1 > 0)
                        {
                            Logging.Log("[WORD] > Removed key [" + Program.c_words[Program.c_words.Count - 1][Program.c_words[Program.c_words.Count - 1].Count - 1].key + "] from last word in words.");
                            Program.c_words[Program.c_words.Count - 1].RemoveAt(Program.c_words[Program.c_words.Count - 1].Count - 1);
                        }
                        else
                        {
                            Logging.Log("[WORD] > Removed one empty word from current words.");
                            Program.c_words.RemoveAt(Program.c_words.Count - 1);
                        }
                    }
                    if (SwitcherUI.SnippetsEnabled)
                    {
                        if (c_snip.Count != 0)
                        {
                            c_snip.RemoveAt(c_snip.Count - 1);
                            Logging.Log("[SNI] >Removed one character from current snippet.");
                        }
                    }
                }
                //Pressing any of these Keys will empty current word, and snippet
                if (Key == Keys.Home || Key == Keys.End || Key == Keys.Escape ||
                    (Key == Keys.Tab && Program.switcher.SnippetsExpandType != "Tab" && snipps.Length > 0) || Key == Keys.PageDown || Key == Keys.PageUp ||
                   Key == Keys.Left || Key == Keys.Right || Key == Keys.Down || Key == Keys.Up ||
                   Key == Keys.BrowserSearch || ((win || win_r) && (Key >= Keys.D1 && Key <= Keys.D9)) ||
                   ((ctrl || win || alt || ctrl_r || win_r || alt_r) && (Key != Keys.Menu && //Ctrl modifier and key which is not modifier
                            Key != Keys.LMenu &&
                            Key != Keys.RMenu &&
                            Key != Keys.LWin &&
                            Key != Keys.ShiftKey &&
                            Key != Keys.RShiftKey &&
                            Key != Keys.LShiftKey &&
                            Key != Keys.RWin &&
                            Key != Keys.ControlKey &&
                            Key != Keys.LControlKey &&
                            Key != Keys.RControlKey)))
                {
                    SwitcherUI.CCReset("cc/noShift.Hkey");
                    ClearWord(true, true, true, "Pressed combination of key and modifiers(not shift) or key that changes caret position.", true, AS_IGN_RULES.Contains("C"));
                }
                if (Key == Keys.Space)
                {
                    if (prevKEY != Keys.Space)
                    {
                        Logging.Log("[FUN] > Adding one new empty word to words, and adding to it [Space] key.");
                        Program.c_words.Add(new List<YuKey>());
                        Program.c_words[Program.c_words.Count - 1].Add(new YuKey() { key = Keys.Space });
                        if (SwitcherUI.AddOneSpace && Program.c_word.Count != 0 &&
                            Program.c_word[Program.c_word.Count - 1].key != Keys.Space)
                        {
                            Logging.Log("[FUN] > Eat one space passed, next space will clear last word.");
                            Program.c_word.Add(new YuKey() { key = Keys.Space });
                            afterEOS = true;
                        }
                        else
                        {
                            ClearWord(true, false, false, "Pressed space");
                            afterEOS = false;
                        }
                        SwitcherUI.CCReset("space");
                    }
                    else
                    {
                        if (CLW_W_SPACE)
                        {
                            ClearWord(true, false, false, "Pressed space");
                            CLW_W_SPACE = false;
                        }
                    }
                }
                if (Key == Keys.Enter)
                {
                    if (prevKEY != Keys.Enter)
                    {
                        if (SwitcherUI.Add1NL && Program.c_word.Count != 0 &&
                            Program.c_word[Program.c_word.Count - 1].key != Keys.Enter)
                        {
                            Logging.Log("[FUN] > Eat one New Line passed, next Enter will clear last word.");
                            Program.c_word.Add(new YuKey() { key = Keys.Enter });
                            Program.c_words[Program.c_words.Count - 1].Add(new YuKey() { key = Keys.Enter });
                            afterEOL = true;
                            ClearWord(false, false, true, "Pressed 1st enter", true, AS_IGN_RULES.Contains("C"));
                        }
                        else
                        {
                            ClearWord(true, true, true, "Pressed enter", true, AS_IGN_RULES.Contains("C"));
                            afterEOL = false;
                        }
                        as_lword_layout = 0;
                        SwitcherUI.CCReset("enter");
                    }
                    else
                    {
                        if (CLW_W_ENTER)
                        {
                            ClearWord(true, true, true, "Pressed enter", true, AS_IGN_RULES.Contains("C"));
                            CLW_W_ENTER = false;
                        }
                    }
                }
                if (printable && printable_mod)
                {
                    if (afterEOS)
                    { //Clears word after Eat ONE space
                        ClearWord(true, false, false, "Clear last word after 1 space");
                        afterEOS = false;
                    }
                    if (afterEOL)
                    { //Clears word after Eat ONE enter
                        ClearWord(true, false, false, "Clear last word after 1 enter");
                        afterEOL = false;
                    }
                    if (sym == '\0') { sym = getSym(vkCode); }
                    Program.c_word.Add(new YuKey() { key = Key, upper = sym_upr });
                    Program.c_words[Program.c_words.Count - 1].Add(new YuKey() { key = Key, upper = sym_upr });
                    Logging.Log("[WORD] > Added [" + Key + "]^" + sym_upr);
                    SwitcherUI.CCReset("key:" + Key);
                    #region Symbol Clear Mode
                    if (!String.IsNullOrEmpty(symbolclear))
                    {
                        if (sym == '\0') { sym = getSym(vkCode); }
                        if (symbolclear.Contains(sym))
                        {
                            ClearWord(true, true, true, "SymbolClear", true);
                        }
                    }
                    #endregion
                }
            }
            #endregion
            #region TranslatePanel HotKeys
            IntPtr hwnd = IntPtr.Zero;
            if (SwitcherUI.TrEnabled)
            {
                if (Program.switcher._TranslatePanel != null)
                {
                    hwnd = WinAPI.GetForegroundWindow();
                    if (hwnd == Program.switcher._TranslatePanel.Handle)
                    {
                        if (MSG == WinAPI.WM_KEYUP)
                        {
                            if (Key == Keys.Escape || Key == Keys.Enter || Key == Keys.Space)
                            {
                                Program.switcher._TranslatePanel.HideWnd();
                            }
                        }
                    }
                }
            }
            #endregion
            #region Snippets
            if (SwitcherUI.SnippetsEnabled && !ExcludedProgram(true, hwnd))
            {
                if (printable && printable_mod && down)
                {
                    if (sym == '\0') sym = getSym(vkCode);
                    c_snip.Add(sym);
                    Logging.Log("[SNI] > Added [" + sym + "] to current snippet.");
                    Debug.WriteLine("added " + sym);
                }
                var seKey = Keys.Space;
                bool asls = false;
                if (Program.switcher.SnippetsExpandType == "Tab")
                    seKey = Keys.F14;
                else if (Program.switcher.SnippetsExpandType != "Space")
                {
                    seKey = Keys.F20;
                }
                if (Key == seKey || seKey == Keys.F14 || seKey == Keys.F20)
                    preSnip = true;
                //				if (MSG == WinAPI.WM_KEYUP) {
                //					if (Key == seKeyDown)
                //						seKeyDown = Keys.None;
                //					if (Key == Keys.Space)
                //						aseKeyDown = Keys.None;
                //				}
                if (MSG == WinAPI.WM_KEYDOWN)
                {
                    var ssb = new StringBuilder(); foreach (var c in c_snip) { ssb.Append(c); };
                    var snip = ssb.ToString();
                    var matched = false;
                    Debug.WriteLine("Snip " + snip + ", last: " + last_snip);
                    var NCRule = CheckNCS(snip);
                    if (Key == seKey)
                    {
                        if (NCRule.rule == "\0" || (NCRule.rule != "\0" && !NCRule.isnip))
                        {
                            //						if (seKeyDown == Keys.None) {
                            matched = CheckSnippet(snip);
                            if (!matched && !last_snipANY)
                                matched = CheckSnippet(last_snip + " " + snip, true);
                            if (SwitcherUI.__selection)
                                snip_selection = "";
                            //							seKeyDown = seKey;
                            //						}
                            if (matched || preSnip)
                            {
                                if (__setsnip)
                                    __setsnip = false;
                                else
                                    c_snip.Clear();
                            }
                        }
                        else
                        {
                            Logging.Log("[NCR] > Rule: " + NCRule.rule + " for snippets ignored expansion of the snippet: " + snip);
                        }
                        if (!matched && seKey == Keys.F14)
                        {
                            Debug.WriteLine("No snippet match, restore Tab original action.");
                            DoSelf(() => KInputs.MakeInput(KInputs.AddPress(Keys.Tab)), "Tab-restore-snippet-no-match");
                        }
                    }
                    bool IGN = false;
                    if (SwitcherUI.AutoSwitchEnabled && !ExcludedProgram(false, hwnd, true))
                    {
                        IGN = ((AS_IGN_BACK && was_back) || (AS_IGN_DEL && was_del) || (AS_IGN_LS && was_ls));
                        if (IGN) { Logging.Log("[AS] > Ignore AutoSwitch by: B/D/LS: " + was_back + "/" + was_del + "/" + was_ls); }
                        Debug.WriteLine("Ignore AutoSwitch by: B/D/LS: " + was_back + "/" + was_del + "/" + was_ls);
                        Debug.WriteLine("IGN:" + IGN + "EVT" + MSG);
                        if (!matched /*&& as_wrongs != null*/ && Key == Keys.Space && !IGN /*&& aseKeyDown == Keys.None*/)
                        {
                            if (NCRule.rule == "\0" || (NCRule.rule != "\0" && !NCRule.iauto))
                            {
                                var CW = c_word_backup;
                                var CLW = c_word_backup_last;
                                if (SwitcherUI.AddOneSpace)
                                {
                                    CW = Program.c_word;
                                    CLW = c_word_backup;
                                }
                                if (SwitcherUI.QWERTZ_fix)
                                {
                                    var ASsymDR = ASsymDiffReplace(snip);
                                    Debug.WriteLine("[ASsymDiff] > [" + snip + "] => [" + ASsymDR + "].");
                                    snip = ASsymDR;
                                }
                                asls = matched = CheckAutoSwitch(snip, CW);
                                //if (!matched)
                                //{
                                //    var snip2x = last_snip + " " + snip;
                                //    //Debug.WriteLine("SNIp2x! " + snip2x);
                                //    var SPace = new List<YuKey>() { new YuKey() { key = Keys.Space, altnum = false, upper = false } };
                                //    var dash = new List<YuKey>() { new YuKey() { key = Keys.OemMinus, altnum = false, upper = false } };
                                //    var last2words = CLW.Concat(dash).Concat(CW).ToList();
                                //    asls = matched = CheckAutoSwitch(snip2x, last2words);
                                //    if (!matched)
                                //    {
                                //        last2words = CLW.Concat(SwitcherUI.AddOneSpace ? new List<YuKey>() : SPace).Concat(CW).ToList();
                                //        asls = matched = CheckAutoSwitch(snip2x, last2words);
                                //    }
                                //}
                                if (!matched)
                                {
                                    var snl = WordGuessLayout(snip).Item2;
                                    as_lword_layout = snl;
                                    Logging.Log("[AS] > Last AS word layout: " + snl);
                                }
                            }
                            else
                            {
                                Logging.Log("[NCR] > Rule: " + NCRule.rule + " for autoswitch ignored conversion of the word: " + snip);
                            }
                            //							aseKeyDown = Key;
                        }
                    }
                    if (Key == seKey && !asls)
                    {
                        if (lsnip_noset <= 0)
                            last_snip = snip;
                        else
                            lsnip_noset--;
                    }
                    if (Key == Keys.Space && (seKey == Keys.F14 || seKey == Keys.F20))
                        c_snip.Clear();
                }
                if (MSG == WinAPI.WM_KEYUP)
                {
                    if (Key == Keys.Back) { was_back = true; }
                    if (Key == Keys.Delete) { was_del = true; }
                    if (Key == Keys.Space && AS_IGN_RULES.Contains("S"))
                    {
                        was_back = was_del = false;
                        if (!AS_IGN_RULES.Contains("L")) { was_ls = false; }
                    }
                }
            }
            #endregion
            #region Alt+Numpad (fully workable)
            if (incapt &&
               (Key == Keys.RMenu || Key == Keys.LMenu || Key == Keys.Menu) && !down)
            {
                Logging.Log("[NUM] > Capture of numpads ended, captured [" + tempNumpads.Count + "] numpads.");
                if (tempNumpads.Count > 0)
                { // Prevents zero numpads(alt only) keys
                    Program.c_word.Add(new YuKey()
                    {
                        altnum = true,
                        numpads = new List<Keys>(tempNumpads)//new List => VERY important here!!!
                    });                                      //It prevents pointer to tempNumpads, which is cleared.
                    Program.c_words[Program.c_words.Count - 1].Add(new YuKey()
                    {
                        altnum = true,
                        numpads = new List<Keys>(tempNumpads)
                    });
                }
                tempNumpads.Clear();
                incapt = false;
            }
            if (!incapt && (alt || alt_r) && down)
            {
                Logging.Log("[NUM] > Alt is down, starting capture of Numpads...");
                incapt = true;
            }
            if ((alt || alt_r) && incapt)
            {
                if (Key >= Keys.NumPad0 && Key <= Keys.NumPad9 && !down)
                {
                    tempNumpads.Add(Key);
                }
            }
            #endregion
            #region Reset Modifiers in Hotkeys
            SwitcherUI.ShiftInHotkey = SwitcherUI.AltInHotkey = SwitcherUI.WinInHotkey = SwitcherUI.CtrlInHotkey = false;
            #endregion
            preSnip = false;
            #region Update LD
            Program.switcher.UpdateLDs();
            #endregion
            sym = '\0';
            prevKEY = Key;
            sym_upr = false;
        }
        public static void ListenMouse(ushort MSG)
        {
            if (SwitcherUI.__selection)
            {
                if (MSG == (ushort)WinAPI.RawMouseButtons.LeftUp && ICheckings.IsICursor() && !SwitcherUI.__selection_nomouse)
                {
                    snipsel();

                }
            }
            if ((MSG == (ushort)WinAPI.RawMouseButtons.MouseWheel))
            {
                if (Program.switcher.caretLangDisplay.Visible && SwitcherUI.CaretLangTooltipEnabled)
                {
                    var _fw = WinAPI.GetForegroundWindow();
                    var _clsNMb = new StringBuilder(40);
                    WinAPI.GetClassName(_fw, _clsNMb, _clsNMb.Capacity);
                    var clsNM = _clsNMb.ToString();
                    if (clsNM == "MozillaWindowClass" || clsNM.Contains("mozilla") || clsNM.Contains("Chrome_WidgetWin"))
                        ff_chr_wheeled = true;
                }
            }
            if (MSG == (ushort)WinAPI.RawMouseButtons.LeftDown || MSG == (ushort)WinAPI.RawMouseButtons.RightDown)
            {
                SwitcherUI.CCReset("mouse");
                if (ctrl || ctrl_r)
                    clickAfterCTRL = true;
                if (shift || shift_r)
                    clickAfterSHIFT = true;
                if (alt || alt_r)
                    clickAfterALT = true;
                if (!SwitcherUI.UseJKL || KMHook.JKLERR)
                    SwitcherUI.currentLayout = 0;
                ClearWord(true, true, true, "Mouse click", true, AS_IGN_RULES.Contains("M"));
            }
            if (Program.switcher != null)
            {
                if (WinAPI.GetForegroundWindow() == Program.switcher.Handle)
                {
                    if (MSG == (ushort)WinAPI.RawMouseButtons.MiddleUp)
                    {
                        try
                        {
                            var c = WinAPI.WindowFromPoint(Cursor.Position);
                            var x = Control.FromHandle(c);
                            RestoreClipBoard(x.Text);
                            var was = x.ForeColor;
                            x.ForeColor = System.Drawing.Color.YellowGreen;
                            var z = new System.Windows.Forms.Timer();
                            int v = 0;
                            System.Drawing.Color[] colors = { x.ForeColor, was };
                            z.Tick += (_, __) =>
                            {
                                x.ForeColor = colors[v % 2 == 0 ? 1 : 0];
                                v++;
                                if (v == 5)
                                {
                                    z.Dispose();
                                    x.ForeColor = was;
                                }
                            };
                            z.Interval = 200;
                            z.Start();
                        }
                        catch (Exception e)
                        {
                            Logging.Log("Could not get control: " + e.Message + " " + e.StackTrace);
                        }
                    }
                }
            }
            #region Double click show translate
            if (SwitcherUI.TrEnabled)
                if (SwitcherUI.TrOnDoubleClick)
                {
                    if (MSG == (ushort)WinAPI.RawMouseButtons.LeftUp || MSG == (ushort)WinAPI.RawMouseButtons.RightUp)
                    {
                        if (dbl_click)
                        {
                            Debug.WriteLine("DBL");
                            SwitcherUI.ShowSelectionTranslation(true);
                            dbl_click = click = false;
                        }
                    }
                    if (MSG == (ushort)WinAPI.RawMouseButtons.LeftDown || MSG == (ushort)WinAPI.RawMouseButtons.RightDown)
                    {
                        if (!click)
                        {
                            pif.Start();
                            click = true;
                            click_reset.Interval = SystemInformation.DoubleClickTime;
                            click_reset.Elapsed += (_, __) =>
                            {
                                click = false;
                                Debug.WriteLine("Slow second click!");
                                click_reset.Stop();
                                click_reset.Dispose();
                                click_reset = new System.Timers.Timer();
                            };
                            click_reset.Start();
                            Debug.WriteLine("First click, reset after: " + SystemInformation.DoubleClickTime);
                        }
                        else
                        {
                            var el = pif.ElapsedMilliseconds;
                            pif.Reset();
                            if (el <= 5)
                            {
                                Debug.WriteLine("Too fast [" + el + "ms], probably buggy...");
                                click_reset.Stop();
                                click_reset.Dispose();
                                click_reset = new System.Timers.Timer();
                                click = false;
                            }
                            else
                            {
                                Debug.WriteLine("Second click, after: [" + el + "ms] + kill reset + waiting to Up button");
                                click_reset.Stop();
                                click_reset.Dispose();
                                click_reset = new System.Timers.Timer();
                                dbl_click = true;
                                click = false;
                            }
                        }
                    }
                }
            #endregion
            if (SwitcherUI.LDUseWindowsMessages)
            {
                if (MSG == (ushort)WinAPI.RawMouseButtons.LeftDown)
                    LMB_down = true;
                else if (MSG == (ushort)WinAPI.RawMouseButtons.LeftUp)
                    LMB_down = false;
                if (MSG == (ushort)WinAPI.RawMouseButtons.RightDown)
                    RMB_down = true;
                else if (MSG == (ushort)WinAPI.RawMouseButtons.RightUp)
                    RMB_down = false;
                if (MSG == (ushort)WinAPI.RawMouseButtons.MiddleDown)
                    MMB_down = true;
                else if (MSG == (ushort)WinAPI.RawMouseButtons.MiddleUp)
                    MMB_down = false;
                if (MSG == (ushort)WinAPI.RawMouseButtons.MouseWheel ||
                    MSG == (ushort)WinAPI.RawMouseButtons.LeftUp ||
                    MSG == (ushort)WinAPI.RawMouseButtons.RightUp ||
                    MSG == (ushort)WinAPI.RawMouseButtons.MiddleUp)
                {
                    if (SwitcherUI.LDForCaret)
                    {
                        Program.switcher.UpdateCaredLD();
                    }
                }
                if (MSG == (ushort)WinAPI.RawMouseButtons.LeftUp ||
                    MSG == (ushort)WinAPI.RawMouseButtons.RightUp ||
                    MSG == (ushort)WinAPI.RawMouseButtons.MiddleUp)
                    if (SwitcherUI.CaretLangTooltipEnabled)
                        ff_chr_wheeled = false;
                if (skip_mouse_events-- == 0 || skip_mouse_events == 0)
                {
                    skip_mouse_events = SwitcherUI.LD_MouseSkipMessagesCount;
                    if (MSG == (ushort)WinAPI.RawMouseFlags.MoveRelative)
                    {
                        if (SwitcherUI.LDForMouse)
                        {
                            Program.switcher.UpdateMouseLD();
                        }
                        if ((LMB_down || RMB_down || MMB_down))
                        {
                            if (SwitcherUI.LDForCaret)
                            {
                                Program.switcher.UpdateCaredLD();
                            }
                        }
                    }
                }
            }
        }
        public static void LDEventHook(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject,
                                             int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (SwitcherUI.LDUseWindowsMessages)
            {
                if (eventType == WinAPI.EVENT_OBJECT_FOCUS)
                {
                    if (Program.switcher != null)
                        Program.switcher.UpdateLDs();
                    //SwitcherUI.CCReset("object-focus");
                }
            }
        }
        public static void EventHookCallback(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject,
                                               int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            SwitcherUI.CCReset("fg-window-change");
            if (SwitcherUI.PersistentLayoutOnWindowChange)
            {
                var proc = Locales.ActiveWindowProcess();
                var cont = PLC_HWNDs.Contains(hwnd);
                if (!cont || !SwitcherUI.PersistentLayoutOnlyOnce)
                {
                    if (SwitcherUI.PersistentLayoutForLayout1)
                        Program.switcher.PersistentLayoutCheck(Program.switcher.PersistentLayout1Processes, SwitcherUI.MAIN_LAYOUT1, proc.ProcessName);
                    if (SwitcherUI.PersistentLayoutForLayout2)
                        Program.switcher.PersistentLayoutCheck(Program.switcher.PersistentLayout2Processes, SwitcherUI.MAIN_LAYOUT2, proc.ProcessName);
                }
                if (SwitcherUI.PersistentLayoutOnlyOnce && !cont)
                    PLC_HWNDs.Add(hwnd);
            }
            uint hwndLayout = Locales.GetCurrentLocale(hwnd);
            as_lword_layout = 0;
            bool conhost = false;
            if (SwitcherUI.UseJKL && !KMHook.JKLERR)
            {
                if (ConHost_HWNDs.Contains(hwnd))
                {
                    conhost = true;
                    Logging.Log("[JKL] > Known ConHost window: " + hwnd);
                    jklXHidServ.CycleAllLayouts(hwnd);
                }
                else
                {
                    var strb = new StringBuilder(350);
                    WinAPI.GetClassName(hwnd, strb, strb.Capacity);
                    if (strb.ToString() == "Shell_InputSwitchTopLevelWindow")
                    {
                        Logging.Log("[WND_CHANGE] > Ignore layout-select window " + hwnd);
                        return;
                    }
                    if (strb.ToString() == "ConsoleWindowClass"
                       //|| strb.ToString() == "Chrome_WidgetWin_1"
                       )
                    {
                        conhost = true;
                        Logging.Log("[JKL] > [" + hwnd + "] = ConHost window, remembering...");
                        ConHost_HWNDs.Add(hwnd);
                        jklXHidServ.CycleAllLayouts(hwnd);
                    }
                }
            }
            if (!SwitcherUI.UseJKL || KMHook.JKLERR)
            {
                // Only if JKL is not enabled/working, also for console apps use getconbkl.dll in ↓ which works only in x86
                SwitcherUI.currentLayout = /*SwitcherUI.GlobalLayout =*/ conhost ? Locales.GetCurrentLocale() : hwndLayout;
                Logging.Log("[FOCUS] > Updating currentLayout on window activate to [" + SwitcherUI.currentLayout + "]...");
            }
            Logging.Log("Hwnd " + hwnd + ", layout: " + hwndLayout + ", FaineSwitcher layout: " + SwitcherUI.GlobalLayout);
            if (SwitcherUI.OneLayout)
                if (hwndLayout != SwitcherUI.GlobalLayout)
                {
                    var title = new StringBuilder(128);
                    WinAPI.GetWindowText(hwnd, title, 127);
                    DoLater(() =>
                    {
                        Logging.Log("[ONEL] > Layout in this window [" + title + "] was different, changing layout to FaineSwitcher global layout.");
                        ChangeToLayout(hwnd, SwitcherUI.GlobalLayout);
                    }, 100);
                }
            if (ExcludedProgram(false, hwnd))
            {
                Logging.Log("[WinCh-HK] Disabled win-hotkeys.");
                Program.switcher.UnregisterHotkeys(0, true);
            }
            else if (SwitcherUI.ENABLED)
            {
                Logging.Log("[WinCh-HK] Enabled win-hotkeys.");
                Program.switcher.RegisterHotkeys();
            }
        }
        #endregion
        #region Functions/Struct
        static void snipsel()
        {
            //			var clipr = GetClipboard(4,10);
            skip_kbd_events += 2;
            snip_selection = GetClipStr();
            Debug.WriteLine("SEL>> " + snip_selection);
            RestoreClipBoard();
        }
        static bool _hasKey(string[] ar, string key)
        {
            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i] == null) continue;
                if (key.Length == ar[i].Length)
                {
                    if (ar[i].ToLowerInvariant() == key.ToLowerInvariant()) return true;
                }
            }
            return false;
        }

        // слова української мови які не потрібно перевіряти через AI
        static List<string> ukrainianWords = new List<string>()
        {
            "та", "і", "а", "але", "бо", "над", "у", "до", "по", "перед", "не", "ні", "тільки", "ледве", "мов",
            "ти", "на", "не", "ні", "як", "ще"
        };

        // список слів які потрібно переписати навіть якщо MLResul == false
        // тут можуть бути неправельно написані слова для любої мови
        static List<string> needSwitch = new List<string>() { "іффіоуе", "гш" };

        static bool CheckAutoSwitch(string snip, List<YuKey> word, bool single = true)
        {
            var matched = false;

            // Перевірка, чи текст забагато короткий або вже міститься у списку українських слів
            if (snip.Length < 2 || ukrainianWords.Contains(snip))
                return matched;

            var corr = "";
            var snil = snip.ToLowerInvariant();

            // Перевірка, чи текст є пустим або складається тільки з пробілів
            if (String.IsNullOrWhiteSpace(snip))
                return matched;

            // Застосування моделі машинного навчання для передбачення мови
            var res = MLModel1.Predict(new MLModel1.ModelInput() { Col0 = snip });
            if (needSwitch.Contains(snip) || res.PredictedLabel > 0.5)
            {
                if (SwitcherUI.SoundOnAutoSwitch)
                    SwitcherUI.SoundPlay();
                if (SwitcherUI.SoundOnAutoSwitch2)
                    SwitcherUI.SoundPlay(true);

                // Парсинг слова для подальшого визначення мови
                corr = ParseWord(snip);

                // Визначення мови (використовується функція WordGuessLayout)
                var asl = WordGuessLayout(corr, 0, false).Item2;

                // Зміна мови на визначену
                ChangeToLayout(Locales.ActiveWindow(), asl);

                // видалення останнього спейсу якщо необхідно
                if (!SwitcherUI.AddOneSpace)
                    DoSelf(() => KInputs.MakeInput(KInputs.AddPress(Keys.Back)), "autoswitch_back");
                else if (!SwitcherUI.AutoSwitchSpaceAfter)
                {
                    DoSelf(() => KInputs.MakeInput(KInputs.AddPress(Keys.Back)), "autoswitch_back2");
                    word.RemoveAt(word.Count - 1);
                }
                word = QWERTZ_wordFIX(word);
                StartConvertWord(word.ToArray(), Locales.GetCurrentLocale(), true, true);

                matched = true;
            }
            
            if (matched)
            {
                aftsingleAS = single;
                last_snip = corr;
            }
            return matched;
        }


        static Dictionary<string, string> replacmentTemplate = new Dictionary<string, string>()
{
    {"q", "й"},
    {"й", "q"},
    {"w", "ц"},
    {"ц", "w"},
    {"e", "у"},
    {"у", "e"},
    {"r", "к"},
    {"к", "r"},
    {"t", "е"},
    {"е", "t"},
    {"y", "н"},
    {"н", "y"},
    {"u", "г"},
    {"г", "u"},
    {"i", "ш"},
    {"ш", "i"},
    {"o", "щ"},
    {"щ", "o"},
    {"p", "з"},
    {"з", "p"},
    {"[", "х"},
    {"х", "["},
    {"]", "ї"},
    {"ї", "]"},
    {"a", "ф"},
    {"ф", "a"},
    {"s", "і"},
    {"і", "s"},
    {"d", "в"},
    {"в", "d"},
    {"f", "а"},
    {"а", "f"},
    {"g", "п"},
    {"п", "g"},
    {"h", "р"},
    {"р", "h"},
    {"j", "о"},
    {"о", "j"},
    {"k", "л"},
    {"л", "k"},
    {"l", "д"},
    {"д", "l"},
    {";", "ж"},
    {"ж", ";"},
    {"'", "є"},
    {"є", "'"},
    {"z", "я"},
    {"я", "z"},
    {"x", "ч"},
    {"ч", "x"},
    {"c", "с"},
    {"с", "c"},
    {"v", "м"},
    {"м", "v"},
    {"b", "и"},
    {"и", "b"},
    {"n", "т"},
    {"т", "n"},
    {"m", "ь"},
    {"ь", "m"},
    {",", "б"},
    {"б", ","},
    {".", "ю"},
    {"ю", "."},
    {"-", "-"}
};


        static string ParseWord(string word)
        {
            var result = string.Empty;

            foreach (var ch in word)
            {
                if (ch != ' ' && replacmentTemplate.ContainsKey(ch.ToString()))
                    result += replacmentTemplate[ch.ToString()];
            }

            return result.Trim();
        }
        static NCR CheckNCS(string snip)
        {
            for (int i = 0; i != NCRules.Length; i++)
            {
                if (Regex.IsMatch(snip, NCRules[i].rule))
                {
                    return NCRules[i];
                }
            }
            return new NCR() { rule = "\0" };
        }
        static bool CheckSnippet(string snip, bool xx2 = false)
        {
            var matched = false;
            var x2 = xx2; //&& aftsingleAS && !SwitcherUI.AutoSwitchSpaceAfter;
            Logging.Log("[SNI] > Current snippet is [" + snip + "].");
            var doublefirst = false;
            if (String.IsNullOrEmpty(snip)) return matched;
            for (int i = 0; i < snipps.Length; i++)
            {
                var snipi = snipps[i];
                if (snipi == null) break;
                if (snipi.StartsWith("D*", StringComparison.InvariantCulture))
                {
                    Debug.WriteLine("snip ori: " + snipi);
                    snipi = snipi.Substring(2);
                    if (!String.IsNullOrEmpty(last_snip))
                    {
                        snip = last_snip + " " + snip;
                        doublefirst = true;
                    }
                    Debug.WriteLine("snip cut: " + snipi);
                }
                if (snipi.StartsWith(IGNLAYSNIP, StringComparison.InvariantCulture))
                {
                    var ignlaysnip = snipi.Replace(IGNLAYSNIP, "");
                    if (ignlaysnip.Length != snip.Length)
                    {
                        Debug.WriteLine("length mismatch, it would never match");
                        continue;
                    }
                    bool allok = true;
                    if (ignlaysnip != snip)
                    {
                        var _ = WordGuessLayout(snip);
                        var __ = WordGuessLayout(ignlaysnip);
                        Debug.WriteLine(_.Item2 + "/" + __.Item2);
                        for (int q = 0; q != snip.Length; q++)
                        {
                            char c = snip[q], cq = ignlaysnip[q];
                            var kk = WinAPI.VkKeyScanEx(c, _.Item2);
                            if (kk == -1)
                            {
                                foreach (var l in Program.locales)
                                {
                                    kk = WinAPI.VkKeyScanEx(c, l.uId);
                                    if (kk != -1) break;
                                }
                            }
                            //						var l = Locales.GetCurrentLocale(Locales.ActiveWindow());
                            //						Debug.WriteLine("Scan "+cq+" + " +l);
                            var kq = WinAPI.VkKeyScanEx(cq, __.Item2);
                            Debug.WriteLine("kk = " + kk + ", kq = " + kq);
                            if (kk != kq)
                            {
                                allok = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("input: [" + snip + "] actually equals snippet by characters exactly!: [" + ignlaysnip + "], no need to check key-equality.");
                    }
                    if (allok)
                    {
                        Debug.WriteLine("All chars from [" + snip + "] are key-equally to snippet: [" + snipi + "].");
                        ExpandSnippet(snip, exps[i], SwitcherUI.SnippetSpaceAfter, SwitcherUI.SnippetsSwitchToGuessLayout, false, x2);
                        aftsingleAS = false;
                        break;
                    }
                }
                var igncase = snipi.EndsWith("/i", StringComparison.InvariantCulture);
                if (snipi.StartsWith(REGEXSNIP, StringComparison.InvariantCulture) &&
                    (snipi.EndsWith("/", StringComparison.InvariantCulture) || igncase))
                {
                    var regex_r = snipi.Substring(6, snipi.Length - 7 + (igncase ? -1 : 0));
                    var repl = RegexREPLACEP(snip, regex_r, exps[i], igncase);
                    if (!String.IsNullOrEmpty(repl))
                    {
                        Logging.Log("[REEX] > Replaced: " + repl);
                    }
                    if (!String.IsNullOrEmpty(repl))
                    {
                        ExpandSnippet(snip, repl, SwitcherUI.SnippetSpaceAfter, SwitcherUI.SnippetsSwitchToGuessLayout, false, x2);
                        aftsingleAS = false;
                        matched = true;
                        break;
                    }
                }
                if (snipi.Contains(__ANY__))
                {
                    var any = "";
                    var pins = snipi;
                    var len = pins.Length;
                    var at = pins.IndexOf(__ANY__, StringComparison.InvariantCulture);
                    var aft = at + __ANY__.Length;
                    //					Debug.WriteLine("aftst:"+pins[aft]);
                    var laf = len - aft;
                    if (snip.Length < laf + at)
                    {
                        Logging.Log("[SNI] > Too small snip, to use with " + __ANY__);
                        continue;
                    }
                    //					Debug.WriteLine("at:"+at+",aft:"+aft+",laf:"+laf);
                    bool yay = true;
                    if (at <= snip.Length)
                        for (int f = 0; f != at; f++)
                        {
                            if (snip[f] != pins[f]) yay = false;
                        }
                    for (int f = 0; f != laf; f++)
                    {
                        var t = f + (pins.Length - laf);
                        var g = f + (snip.Length - laf);
                        //						Debug.WriteLine("Calc: " + g + ", " + t +  ", " + at + ", " + laf);
                        if (g > snip.Length || g < 0) continue;
                        //						Debug.WriteLine("Cht: " + snip[g] + ", " + pins[t]);
                        if (snip[g] != pins[t]) yay = false;
                    }
                    if (yay)
                    {
                        last_snipANY = true;
                        if (SwitcherUI.SoundOnSnippets)
                            SwitcherUI.SoundPlay();
                        if (SwitcherUI.SoundOnSnippets2)
                            SwitcherUI.SoundPlay(true);
                        any = snip.Substring(at, (snip.Length - laf - at));
                        //						Debug.WriteLine("Yay!" + any);
                        Logging.Log("[SNI] > Current snippet [" + snip + "] matched with " + __ANY__ + " existing snippet [" + exps[i] + "].");
                        var exp = exps[i].Replace(__ANY__, any);
                        //						Debug.WriteLine("exp: " + exp);
                        ExpandSnippet(snip, exp, SwitcherUI.SnippetSpaceAfter, SwitcherUI.SnippetsSwitchToGuessLayout, false, x2);
                        aftsingleAS = false;
                        break;
                    }
                    //		    		Debug.WriteLine("ANY " + yay);
                }
                if (snip.Length == snipi.Length)
                {
                    if (snip == snipi)
                    {
                        last_snipANY = false;
                        if (exps.Length > i)
                        {
                            if (SwitcherUI.SoundOnSnippets)
                                SwitcherUI.SoundPlay();
                            if (SwitcherUI.SoundOnSnippets2)
                                SwitcherUI.SoundPlay(true);
                            Logging.Log("[SNI] > Current snippet [" + snip + "] matched existing snippet [" + exps[i] + "].");
                            ExpandSnippet(snip, exps[i], SwitcherUI.SnippetSpaceAfter, SwitcherUI.SnippetsSwitchToGuessLayout, false, x2);
                            matched = true;
                        }
                        else
                        {
                            Logging.Log("[SNI] > Snippet [" + snip + "] has no expansion, snippet is not finished or its expansion commented.", 1);
                        }
                        aftsingleAS = false;
                        break;
                    }
                }
                doublefirst = false;
            }
            if (matched && doublefirst)
            {
                last_snip = "";
            }
            return matched;
        }
        static string CreateHFDir()
        {
            var dir = System.IO.Path.Combine(SwitcherUI.nPath, "histories");
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            return dir;
        }
        static void ReCreateHF(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                Debug.WriteLine("Creating history file: " + path);
                var fs = System.IO.File.Create(path);
                fs.Close();
            }
        }
        static void RemLastHistory()
        {
            try
            {
                var path = System.IO.Path.Combine(SwitcherUI.nPath, "history.txt");
                if (SwitcherUI.WriteInputHistoryByDate)
                {
                    path = GetHistoryByDatePath();
                }
                ReCreateHF(path);
                var txt = System.IO.File.ReadAllText(path);
                if (txt.Length < 1) return;
                txt = txt.Substring(0, txt.Length - 1);
                System.IO.File.WriteAllText(System.IO.Path.Combine(SwitcherUI.nPath, "history.txt"), txt);
            }
            catch (Exception e)
            {
                if (!Configs.SwitchToAppData(true, e))
                    SwitcherUI.WriteInputHistory = false;
                Logging.Log("Write history(r) error: " + e.Message, 1);
            }
        }
        public static string GetHistoryByDatePath()
        {
            var date = DateTime.Now;
            var ymd = date.ToString("yy-MM-dd");
            var h = date.ToString("HH");
            return System.IO.Path.Combine(CreateHFDir(), ymd + (SwitcherUI.WriteInputHistoryHourly ? ("-[" + h + "]") : "") + ".txt");
        }
        static void WriteToHistory(char c)
        {
            try
            {
                var path = System.IO.Path.Combine(SwitcherUI.nPath, "history.txt");
                if (SwitcherUI.WriteInputHistoryByDate)
                {
                    path = GetHistoryByDatePath();
                }
                ReCreateHF(path);
                var sw = System.IO.File.AppendText(path);
                sw.Write(c);
                sw.Close();
            }
            catch (Exception e)
            {
                if (!Configs.SwitchToAppData(true, e))
                    SwitcherUI.WriteInputHistory = false;
                Logging.Log("Write history(c) error: " + e.Message, 1);
            }
        }
        static void WriteToHistory(string s)
        {
            try
            {
                var path = System.IO.Path.Combine(SwitcherUI.nPath, "history.txt");
                if (SwitcherUI.WriteInputHistoryByDate)
                {
                    path = GetHistoryByDatePath();
                }
                ReCreateHF(path);
                var sw = System.IO.File.AppendText(path);
                sw.Write(s);
                sw.Close();
            }
            catch (Exception e)
            {
                if (!Configs.SwitchToAppData(true, e))
                    SwitcherUI.WriteInputHistory = false;
                Logging.Log("Write history(s) error: " + e.Message, 1);
            }
        }
        static char getSym(int vkCode, bool ignore = false)
        {
            // Fix for AltGr+Shift+<some-umlaut> #271
            if (((ctrl || ctrl_r) && (alt || alt_r) && (shift || shift_r)) || (alt_r && (shift || shift_r))) { return '\0'; }
            // e.g. return nothing when ctrl+alt+shift+something is pressed
            // or else the umlaut input will be *eaten*
            //			var stb = new StringBuilder(10);
            //			var byt = new byte[256];
            if (!ignore)
            {
                if (sym == '\0')
                    sym = getSym(vkCode, true);
                if (IsUpperInput(!Char.IsLetterOrDigit(sym)))
                {
                    sym_upr = true;
                    //					byt[(int)Keys.ShiftKey] = 0xFF;
                }
                else { sym_upr = false; }
            }
            uint layout = Locales.GetCurrentLocale();
            if (SwitcherUI.UseJKL && !KMHook.JKLERR)
            {
                if (layout != (SwitcherUI.currentLayout))
                {
                    if (IsConhost())
                        layout = SwitcherUI.currentLayout;
                }
            }
            //                                                                     0=alt, 1=noalt, 1<<2=?
            // it eats umlaut characters with 0, so:
            var c = ToUnicodeExMulti((uint)vkCode, (IntPtr)((int)layout), sym_upr);
            //			WinAPI.ToUnicodeEx((uint)vkCode, (uint)vkCode, byt, stb, stb.Capacity, 1<<2, (IntPtr)layout);
            if (c != '\0')
            {
                Logging.Log("[GETSYM] > " + (ignore ? "fake;" : "true;") + " ToUnEx() => [" + c + "].");
                return c;
            }
            return '\0';
        }
        public static DICT<string, string> ParseDictionary(string[] raw_dict, bool tsdict = false)
        {
            var dict = new DICT<string, string>();
            for (int i = 0; i != raw_dict.Length; i++)
            {
                var line = raw_dict[i];
                if (line.Contains("|"))
                {
                    var lr = line.Split('|');
                    var cc = lr[0];
                    var rr = lr[1];
                    if (tsdict)
                    {
                        if (line.Length > 3 && lr.Length > 2)
                        {
                            if (line[0] == 's' && line[1] == '/')
                            {
                                lr = SplitNoEsc(line, '|', '\\', '/', 3);
                                foreach (var e in lr)
                                {
                                    Debug.WriteLine("LR Noesc split: " + e);
                                }
                                cc = lr[0];
                                rr = lr[1];
                            }
                        }
                    }
                    //			    	Debug.WriteLine("Noth: "+cc+" " +rr);
                    if (cc != "")
                        dict[cc] = rr;
                    else if (rr != "")
                        dict[rr] = cc;
                    else
                        Logging.Log("[DICT] Empty entry, just | : " + line, 2);
                }
                else
                {
                    Logging.Log("[DICT] > Wrong Dictionary, line #" + i + ", => " + line);
                    dict = null;
                    break;
                }
            }
            return dict;
        }
        public static string DictToRaw(DICT<string, string> dict)
        {
            var raw = new StringBuilder();
            for (int i = 0; i != dict.len; i++)
            {
                raw.Append(dict[i].k).Append("|").Append(dict[i].v).Append(Environment.NewLine);
            }
            return raw.ToString();
        }
        public static void __RELOADDict(string PATH, ref DICT<string, string> OUTD, string type, bool tsdict = false, bool writedef = false, DICT<string, string> def = null)
        {
            DICT<string, string> __dict = null;
            var load = false;
            if (System.IO.File.Exists(PATH))
            {
                var lines = System.IO.File.ReadAllLines(PATH);
                __dict = ParseDictionary(lines, tsdict);
                load = true;
            }
            else if (writedef)
            {
                if (def != null)
                    System.IO.File.WriteAllText(PATH, DictToRaw(def));
            }
            if (load)
            {
                if (__dict != null && __dict.len != 0)
                {
                    Logging.Log("[" + type + "] > Succesfully initialized " + type + " DICT from [" + PATH + "].");
                    OUTD = __dict;
                }
                else
                {
                    Logging.Log("[" + type + "] > " + PATH + " wrong syntax, DICT not updated.", 1);
                }
            }
        }
        public static void ReloadCusRepDict()
        {
            __RELOADDict(System.IO.Path.Combine(SwitcherUI.nPath, "CustomConversion.txt"), ref CustomConversionDICT,
                         "CustomConversion", true, SwitcherUI.HKSelCustConv_tempEnabled, CustomConversionDICT);
        }
        public static void ReloadASsymDiffDict()
        {
            __RELOADDict(System.IO.Path.Combine(SwitcherUI.nPath, "ASsymDiff.txt"), ref ASsymDiffDICT,
                         "ASsymDiff", false, SwitcherUI.QWERTZ_fix, ASsymDiffDICT);
        }
        public static void ReloadTSDict()
        {
            __RELOADDict(System.IO.Path.Combine(SwitcherUI.nPath, "TSDict.txt"), ref transliterationDict,
                         "TRANSLTRT", true, true, DefaultTransliterationDict);
        }
        public static void ReloadLayReplDict()
        {
            __RELOADDict(System.IO.Path.Combine(SwitcherUI.nPath, "LayoutReplaces.txt"), ref LayReplDict,
                         "LayoutReplace", false, SwitcherUI.QWERTZ_fix, LayReplDict);
        }
        public static string ul_str(string s, int st, int x, int act)
        {
            var left = s.Substring(0, st);
            var center = s.Substring(st, (x == -1 ? s.Length : x) - st);
            var right = x == -1 ? "" : s.Substring(x, s.Length - x);
            var ul = " [" + (act == 0 ? "l" : act == 1 ? "U" : "?") + "] ";
            Logging.Log("[Ul_str] > pre:" + ul + center);
            center = act == 0 ? center.ToLowerInvariant() : act == 1 ? center.ToUpperInvariant() : center;
            Logging.Log("[Ul_str] > aft:" + ul + center);
            return string.Join("", new[] { left, center, right });
        }
        public static string UL_no_e12(string input)
        {
            int start = -1;
            int ul = -1;
            int act = -1;
            var result = new StringBuilder(input);
            for (int i = 0; i != input.Length; i++)
            {
                if (input[i] == '\\')
                {
                    if (i > expressions[12].Length)
                    { // __convert
                        var e12 = input.Substring(i - expressions[12].Length - 1, expressions[12].Length);
                        if (e12.ToLowerInvariant() == expressions[12])
                        {
                            Debug.WriteLine("EXPR_IGNORE " + e12);
                            if (start != -1)
                            {
                                input = ul_str(input, start, i - expressions[12].Length - 1, act);
                                act = start = -1;
                            }
                            continue;
                        }
                    }
                    if (i + 1 < input.Length)
                    {
                        var i1 = input[i + 1].ToString().ToLowerInvariant();
                        ul = (i1 == "l" ? 0 : i1 == "u" ? 1 : -1);
                        if (i1 == "e" && start != -1)
                        {
                            input = ul_str(input, start, i, act);
                            act = start = -1;
                        }
                        if (ul != -1)
                        {
                            if (i + 2 < input.Length)
                            {
                                start = i + 2;
                                act = ul;
                            }
                        }
                        i++;
                    }
                }
            }
            if (start != -1)
            {
                input = ul_str(input, start, -1, act);
            }
            input = Regex.Replace(input, @"(?<!__convert\()\\[uUlLeE](?!\))", "");
            return input;
        }
        public static string RegexREPLACEP(string input, string regex_raw, string replacement, bool ignorecase = false)
        {
            bool ism = false;
            RegexOptions ics = (ignorecase ? RegexOptions.IgnoreCase : RegexOptions.None);
            try
            {
                ism = Regex.IsMatch(input, regex_raw, ics);
            }
            catch (Exception e)
            {
                Logging.Log("[RegexRP] > Regex replace FAILED, error in regex: " + regex_raw + " error message: " + e.Message, 1);
                return input;
            }
            if (ism)
            {
                Logging.Log("[REEX] > regex: /" + regex_raw + "/" + (ignorecase ? "i" : "") + ", snip [" + input + "]");
                input = Regex.Replace(input, regex_raw, replacement, ics);
                Debug.WriteLine("PRE UL : " + input);
                input = UL_no_e12(input);
            }
            else { return ""; }
            return input;
        }
        static void ExpandSnippet(string snip, string expand, bool spaceAft, bool switchLayout, bool ignoreExpand = false, bool x2 = false, uint guessl = 0)
        {
            DoSelf(() =>
            {
                try
                {
                    Debug.WriteLine("Snippet: " + snip);
                    var exsni = expand;
                    if (!ignoreExpand)
                    {
                        var backs = snip.Length + 1;
                        Debug.WriteLine("X2" + x2);
                        if ( /*x2||*/ Program.switcher.SnippetsExpandType != "Space") backs--;
                        KInputs.MakeInput(KInputs.AddPress(Keys.Back, backs));
                        Logging.Log("[SNI] > Expanding snippet [" + snip + "] to [" + expand + "].");
                        exsni = ExpandSnippetWithExpressions(expand);
                        var snipclear = !__setsnip;
                        ClearWord(true, true, snipclear, "Cleared due to snippet expansion" + (snipclear ? "" : " (snippet clear skipped by __setsnip!)"));
                        Debug.WriteLine("OK");
                        //						KInputs.MakeInput(KInputs.AddString(expand));
                    }
                    Debug.WriteLine("EXSNI: " + exsni);
                    if (switchLayout)
                    {
                        bool skp = false;
                        if (SwitcherUI.__setlayoutForce)
                            if (expand.Contains("__setlayout"))
                            {
                                var i = expand.IndexOf("__setlayout", StringComparison.InvariantCulture);
                                Debug.WriteLine("__setlayout(x" + i);
                                if ((i >= 1 && expand[i - 1] != '\\') || i == 0)
                                {
                                    skp = true;
                                    Debug.WriteLine("__setlayout forced! " + expand);
                                }
                            }
                        if (!skp)
                        {
                            var guess = guessl;
                            if (guess == 0)
                                guess = WordGuessLayout(exsni).Item2;
                            else
                                Debug.WriteLine("Skip Guess for snippet expand, layout suplied: " + guessl);
                            if (guess == 0)
                            {
                                Logging.Log("Layout can't be guessed for: [" + snip + "].", 2);
                            }
                            else
                            {
                                var gn = Program.locales.ToList().Find(l => l.uId == guess).Lang;
                                Logging.Log("[SNI] > Changing to guess layout [" + guess + "] after snippet [" + gn + "].");
                                ChangeToLayout(Locales.ActiveWindow(), guess);
                            }
                        }
                        else
                        {
                            Logging.Log("[SNI] > Switch layout skip due to __setlayout_FORCED");
                        }
                    }
                    if (spaceAft && !expand.Contains("__cursorhere"))
                        KInputs.MakeInput(KInputs.AddString(" "));
                    DoLater(() => Program.switcher.Invoke((MethodInvoker)delegate
                    {
                        Program.switcher.UpdateLDs();
                    }), snip.Length * 2);
                }
                catch (Exception e)
                {
                    Logging.Log("[SNI] > Some snippets configured wrong, check them, error:\r\n" + e.Message + "\r\n" + e.StackTrace + "\r\n", 1);
                    // If not use TASK, form(MessageBox) won't accept the keys(Enter/Escape/Alt+F4).
                    var msg = new[] { "", "" };
                    msg[0] = Program.Lang[Languages.Element.MSG_SnippetsError];
                    msg[1] = Program.Lang[Languages.Element.Error];
                    var tsk = new System.Threading.Tasks.Task(() => MessageBox.Show(msg[0], msg[1], MessageBoxButtons.OK, MessageBoxIcon.Error));
                    tsk.Start();
                    KInputs.MakeInput(KInputs.AddString(snip));
                }
            }, "expand_snippet");
        }
        #region in Snippets expressions 
        //                                                0         1          2             3         4             5          6                7            8            9            10         11               12           13           14              15             16             17           18        19       20    
        public static readonly string[] expressions = new[] { "__date", "__time", "__version", "__system", "__title", "__keyboard", "__execute", "__cursorhere", "__paste", "__Switcherhome", "__delay", "__uppercase", "__convert", "__setlayout", "__selection", "__clearlsnip", "__replace", "__setsnip", "__setlsnip", "__if", "__nif" };
        static string ExpandSnippetWithExpressions(string expand)
        {
            StringBuilder ex, args, raw, err, allraw;
            ex = new StringBuilder(); args = new StringBuilder(); raw = new StringBuilder(); err = new StringBuilder(); allraw = new StringBuilder();
            bool args_getting = false, is_expr = false, escaped = false;
            int expr_start = -1;
            bool contains_expr = false;
            foreach (var expr in expressions)
            {
                if (expand.Contains(expr))
                {
                    contains_expr = true;
                    break;
                }
            }
            if (!contains_expr)
            {
                KInputs.MakeInput(KInputs.AddString(expand));
                return expand;
            }
            bool just_escaped = false;
            EXSN_result = new StringBuilder();
            for (int i = 0; i != expand.Length; i++)
            {
                var args_get = false;
                var e = expand[i];
                if (i > 0 && e == '\\' && expand[i - 1] == '\\' && !just_escaped)
                {
                    Logging.Log("[EXPR] Escape \"\\\".");
                    just_escaped = true;
                    continue;
                }
                //				Debug.WriteLine("i:"+i+", e:"+e+ "just" + just_escaped);
                if (!is_expr)
                {
                    if (ex.ToString() == "__" && e == '_')
                    { // Fix for multiple "_" repeats before __expr
                        raw.Append(e);
                    }
                    else
                    {
                        ex.Append(e);
                    }
                }
                else err.Append(e);
                if (is_expr && e == ')')
                { // Escape closing
                    if (expand[i - 1] == '\\' && !just_escaped)
                    {
                        Logging.Log("[EXPR] > Escaped \")\" at position: " + i);
                        if (args.Length > 2)
                            args = new StringBuilder(args.ToString().Substring(0, args.Length - 1));
                    }
                    else
                    {
                        if (args_getting)
                        {
                            args_getting = false;
                            args_get = true;
                            //						Debug.WriteLine("end of args of: " + fun + " -> " +i);
                        }
                        else
                        {
                            Logging.Log("[EXPR] > Expression \"(\" missing, but \")\" were there, in [" + ex + "], at position: " + expr_start + " in [" + expand + "]");
                            KInputs.MakeInput(KInputs.AddString(new StringBuilder(ex.ToString()).Append(err).ToString()));
                            is_expr = false;
                            args_get = false;
                            escaped = false;
                            args.Clear(); ex.Clear(); raw.Clear();
                        }
                    }
                }
                if (args_getting)
                    args.Append(e);
                if (is_expr && e == '(' && !args_getting)
                {
                    args_getting = true;
                    //					Debug.WriteLine("start of args of: " + fun + " -> " +i);
                }
                var maybe_fun = false;
                if (!args_getting && !string.IsNullOrEmpty(ex.ToString()) && !is_expr)
                {
                    foreach (var expr in expressions)
                    {
                        if (expr.StartsWith(ex.ToString(), StringComparison.InvariantCulture))
                        {
                            maybe_fun = true;
                            if (expr == ex.ToString())
                            {
                                expr_start = i - (ex.Length - 1);
                                escaped = false;
                                if (expr_start - 1 < 0)
                                    escaped = false;
                                else if (expand[expr_start - 1] == '\\')
                                    escaped = true;
                                is_expr = !escaped;
                                //								Debug.WriteLine("expr: " +expr+" equals " + ex + ", expr_start: " + expr_start + " is_expr: " + is_expr);
                                err.Clear();
                                break;
                            }
                        }
                        else
                            maybe_fun = false;
                        //						Debug.WriteLine("Try: " +fun+" > " + expr + (maybe_fun ? " OK" : " NO"));
                        if (maybe_fun) break;
                    }
                }
                if (is_expr && i == expand.Length - 1 && !args_get)
                {
                    Logging.Log("[EXPR] > Expression [" + ex + "] missing its end \")\", at positon: " + expr_start + " in: [" + expand + "].", 2);
                    KInputs.MakeInput(KInputs.AddString(new StringBuilder(ex.ToString()).Append(err.ToString()).Append(args.ToString()).ToString()));
                    err.Clear();
                }
                if (args_get && !escaped)
                {
                    Logging.Log("[EXPR] > Executing expression: " + ex + " with args: [" + args + "]");
                    var curlefts = expand.Length - i - 1;
                    ExecExpression(ex.ToString(), args.ToString(), curlefts, allraw.ToString());
                    is_expr = false;
                    args_get = false;
                    args.Clear(); ex.Clear();
                }
                if (!args_getting && !maybe_fun && !is_expr)
                {
                    if (!escaped)
                    {
                        //						Debug.WriteLine("Not even start of any expression: " + ex);
                        raw.Append(ex.ToString());
                    }
                    ex.Clear();
                    maybe_fun = false;
                    is_expr = false;
                    expr_start = -1;
                }
                if (!string.IsNullOrEmpty(raw.ToString()))
                {
                    //					Debug.WriteLine("Inputting raw: ["+raw+"]");
                    KInputs.MakeInput(KInputs.AddString(raw.ToString()));
                    allraw.Append(raw.ToString());
                    raw.Clear();
                }
                if (escaped)
                {
                    Logging.Log("[EXPR] > Ignored espaced expression: " + ex);
                    KInputs.MakeInput(KInputs.AddPress(Keys.Back));
                    KInputs.MakeInput(KInputs.AddString(ex.ToString()));
                    is_expr = false;
                    args_get = false;
                    escaped = false;
                    args.Clear(); ex.Clear(); raw.Clear();
                }
                just_escaped = false;
            }
            if (cursormove != -1)
            {
                KInputs.MakeInput(KInputs.AddPress(Keys.Left, cursormove));
            }
            cursormove = -1;
            return allraw.ToString();

        }
        static void ExprAgainTestOrSend(string estr, ref StringBuilder result)
        {
            var contains = false;
            foreach (var e in expressions)
            {
                if (estr.Contains(e))
                {
                    contains = true;
                    break;
                }
            }
            if (contains)
            {
                Debug.WriteLine("Contains EXPR again " + estr);
                ExpandSnippetWithExpressions(estr);
            }
            else
            {
                result.Append(estr);
                KInputs.MakeInput(KInputs.AddString(estr));
            }
        }
        static StringBuilder EXSN_result;
        public static void ExecExpression(string expr, string args, int curlefts = -1, string plaintext_pre = "")
        {
            if (EXSN_result == null)
            {
                EXSN_result = new StringBuilder();
            }
            if (!string.IsNullOrEmpty(plaintext_pre))
            {
                EXSN_result.Append(plaintext_pre);
            }
            switch (expr)
            {
                case "__paste":
                    Logging.Log("[EXPR] > Pasting text from snippet.");
                    Debug.WriteLine("Paste: " + args);
                    EXSN_result.Append(args);
                    GetClipStr();
                    RestoreClipBoard(Regex.Replace(args, "\r?\n|\r", Environment.NewLine));
                    KInputs.MakeInput(KInputs.AddPress(Keys.V), (int)WinAPI.MOD_CONTROL);
                    DoLater(() => RestoreClipBoard(), 300);
                    break;
                case "__date":
                case "__time":
                    var now = DateTime.Now;
                    var format = args;
                    if (string.IsNullOrEmpty(args))
                    {
                        if (expr == "__date")
                            format = "dd/MM/yyyy";
                        else
                            format = "HH:mm:ss";
                    }
                    var ndt = now.ToString(format);
                    EXSN_result.Append(ndt);
                    KInputs.MakeInput(KInputs.AddString(ndt));
                    break;
                case "__version":
                    var v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    EXSN_result.Append(v);
                    KInputs.MakeInput(KInputs.AddString(v));
                    break;
                case "__title":
                    EXSN_result.Append(Program.switcher.Text);
                    KInputs.MakeInput(KInputs.AddString(Program.switcher.Text));
                    break;
                case "__system":
                    var os = Environment.OSVersion.ToString();
                    EXSN_result.Append(os);
                    KInputs.MakeInput(KInputs.AddString(os));
                    break;
                case "__keyboard":
                    SimKeyboard(args);
                    break;
                case "__replace":
                    var argv = SplitEsc(args, ',');
                    var replace = argv[0];
                    for (int i = 1; i < argv.Length - 1; i += 2)
                    {
                        Debug.WriteLine("Replacing: " + argv[i] + " => ", argv[i + 1]);
                        replace = replace.Replace(argv[i], argv[i + 1]);
                    }
                    Debug.WriteLine("===REPLACED INPUT: " + replace);
                    EXSN_result.Append(replace);
                    KInputs.MakeInput(KInputs.AddString(replace));
                    break;
                case "__if":
                    var sep = args[0];
                    argv = SplitEsc(args.Substring(1, args.Length - 1), sep);
                    if (argv[0].Length >= 1 && argv.Length >= 2)
                    {
                        ExprAgainTestOrSend(argv[1], ref EXSN_result);
                    }
                    break;
                case "__nif":
                    sep = args[0];
                    argv = SplitEsc(args.Substring(1, args.Length - 1), sep);
                    if (argv[0].Length == 0 && argv.Length >= 2)
                    {
                        ExprAgainTestOrSend(argv[1], ref EXSN_result);
                    }
                    break;
                case "__execute":
                    Execute(args);
                    break;
                case "__delay":
                    int d = 0;
                    if (Int32.TryParse(args, out d))
                        Thread.Sleep(d);
                    break;
                case "__Switcherhome":
                    EXSN_result.Append(SwitcherUI.nPath);
                    KInputs.MakeInput(KInputs.AddString(SwitcherUI.nPath));
                    break;
                case "__cursorhere":
                    Debug.WriteLine("Curlefts: " + curlefts);
                    cursormove = curlefts;
                    break;
                case "__uppercase":
                    var upc = 1;
                    var t = args;
                    if (args == "")
                    {
                        Debug.WriteLine("sorry nothing here?");
                        ClearWord(false, false, true, "__uppercase NullArgs");
                        break;
                    }
                    if (args.Contains("|"))
                    {
                        var A = args.Split('|');
                        t = A[0];
                        Int32.TryParse(A[1], out upc);
                        if (A[1] == "*")
                            upc = t.Length;
                    }
                    var subst = 0;
                    var res = "";
                    for (int i = 0; i != upc; i++)
                    {
                        if (upc > t.Length)
                        {
                            Debug.WriteLine("Can't go on, no more chars left...");
                            break;
                        }
                        subst++;
                        res += char.ToUpper(t[i]);
                    }
                    if (subst != t.Length)
                        res += t.Substring(subst, t.Length - subst);
                    Debug.WriteLine("Uppercase conversion: " + res);
                    if (res != "")
                    {
                        EXSN_result.Append(res);
                        KInputs.MakeInput(KInputs.AddString(res));
                    }
                    break;
                case "__convert":
                    var ct = ConvertText(args);
                    var argsl = args.ToLowerInvariant();
                    if (argsl.Contains("\\l") || argsl.Contains("\\e") || argsl.Contains("\\u"))
                    {
                        var matches = new Dictionary<int, string>();
                        for (int i = 0; i != args.Length; i++)
                        {
                            if (i + 1 < args.Length)
                            {
                                var ail = args[i + 1].ToString().ToLowerInvariant();
                                if (args[i] == '\\' && Regex.IsMatch(ail, @"[eul]"))
                                {
                                    matches[i] = ail;
                                }
                            }
                        }
                        foreach (var kv in matches)
                        {
                            var left = ct.Substring(0, kv.Key);
                            var right = ct.Substring(kv.Key + 2, ct.Length - kv.Key - 2);
                            Debug.WriteLine("Restore: " + kv.Key + " => " + ct);
                            ct = left + "\\" + kv.Value + right;
                            Debug.WriteLine(ct);
                        }
                        Debug.WriteLine("Post \\U / \\L in __convert: " + ct);
                        ct = UL_no_e12(ct);
                        Debug.WriteLine(ct);
                    }
                    EXSN_result.Append(ct);
                    KInputs.MakeInput(KInputs.AddString(ct));
                    break;
                case "__setlayout":
                    //					bool err = false;
                    uint l = 0;
                    try
                    {
                        UInt32.TryParse(args, out l);
                        //						var i = new System.Globalization.CultureInfo((int)(l>>16));
                    }
                    catch (Exception e)
                    {
                        //			         	err = true;	
                        Logging.Log("__setlayout: ERR: " + e.Message);
                    }
                    var l1 = l == 1;
                    var l2 = l == 2;
                    if (l1)
                        l = SwitcherUI.MAIN_LAYOUT1;
                    if (l2)
                        l = SwitcherUI.MAIN_LAYOUT2;
                    if (l > 2)
                    {
                        ;
                        Logging.Log("[SELAE] Changing to " + l + " ONLYWM: " + SwitcherUI.__setlayoutOnlyWM);
                        if (SwitcherUI.__setlayoutOnlyWM)
                            NormalChangeToLayout(Locales.ActiveWindow(), l);
                        else
                            ChangeToLayout(Locales.ActiveWindow(), l);
                    }
                    break;
                case "__selection":
                    if (!string.IsNullOrEmpty(snip_selection))
                    {
                        EXSN_result.Append(snip_selection);
                        KInputs.MakeInput(KInputs.AddString(snip_selection));
                    }
                    break;
                case "__clearlsnip": // acts as __setlsnip()
                    last_snip = "";
                    lsnip_noset++;
                    Logging.Log("[__clearlsnip] Cleared last snippet.");
                    break;
                case "__setsnip":
                    args = args.Replace(">.<", EXSN_result.ToString());
                    Logging.Log("[__setsnip] Set snip to [" + args + "]");
                    c_snip = args.ToCharArray().ToList();
                    __setsnip = true;
                    break;
                case "__setlsnip":
                    args = args.Replace(">.<", EXSN_result.ToString());
                    Logging.Log("[__setlsnip] Set last snip to [" + args + "]");
                    last_snip = args;
                    lsnip_noset++;
                    break;
            }
        }
        static void Execute(string args)
        {
            string fil = "", arg = "";
            bool fil_get = false;
            for (int i = 0; i < args.Length; i++)
            {
                var c = args[i];
                Debug.WriteLine("c: " + c);
                if (!fil_get)
                {
                    if (c == '|')
                    {
                        fil_get = true;
                    }
                    else fil += c;
                }
                else
                {
                    arg += c;
                }
            }
            Logging.Log("[EXPR] > Executing: executable: [" + fil + "] with args: [" + arg + "].");
            var p = new ProcessStartInfo();
            p.Arguments = arg;
            p.UseShellExecute = true;
            p.FileName = fil;
            try
            {
                Process.Start(p);
            }
            catch (Exception e)
            {
                Logging.Log("[EXPR] > Execute error: " + e.Message);
            }
        }
        public static List<Keys> strparsekey(string key, int times = 1)
        {
            key = key.ToLower().Replace("capslock", "capital");
            List<Keys> keys = new List<Keys>();
            foreach (Keys k in Enum.GetValues(typeof(Keys)))
            {
                var _n = k.ToString().ToLower()
                    .Replace("menu", "alt").Replace("control", "ctrl")
                    .Replace("d0", "0").Replace("d1", "1")
                    .Replace("d2", "2").Replace("d3", "3")
                    .Replace("d4", "4").Replace("d5", "5")
                    .Replace("d6", "6").Replace("d7", "7")
                    .Replace("d8", "8").Replace("d9", "9")
                    .Replace("return", "enter").Replace("numpa", "numpad");
                if (_n == key + "key")
                { // controlkey, shiftkey
                  //					Logging.Log("Added the " + _n);
                    for (int x = 0; x != times; x++)
                    {
                        keys.Add(k);
                    }
                    break;
                }
                if (key.Length > 1)
                {
                    if (key[0] == '[' && key[key.Length - 1] == ']')
                    {
                        var scode = key.Substring(1, key.Length - 2).ToLower();
                        int code = -1;
                        bool ok = false;
                        if (scode.Contains("x"))
                        {
                            scode = scode.Replace("x", "");
                            ok = Int32.TryParse(scode, System.Globalization.NumberStyles.HexNumber, null, out code);
                        }
                        else
                        {
                            ok = Int32.TryParse(scode, out code);
                        }
                        if (ok)
                            if (code == (int)k)
                            {
                                Logging.Log("[EXPR] > Added the key by code: " + code + ", key: " + k);
                                for (int x = 0; x != times; x++)
                                {
                                    keys.Add(k);
                                }
                                break;
                            }
                    }
                }
                if (key == "esc")
                {
                    Logging.Log("[EXPR] > Added the short escape: " + key);
                    for (int x = 0; x != times; x++)
                    {
                        keys.Add(k);
                    }
                    break;
                }
                if (key == "win")
                {
                    Logging.Log("[EXPR] > Added the lwin as base of: " + _n);
                    for (int x = 0; x != times; x++)
                    {
                        keys.Add(k);
                    }
                    break;
                }
                if (_n == key)
                {
                    Logging.Log("[EXPR] > Added the " + _n);
                    for (int x = 0; x != times; x++)
                    {
                        keys.Add(k);
                    }
                    break;
                }
            }
            return keys;
        }
        public static void SimKeyboard(string args)
        {
            string[] multi_args;
            var all_keys = new List<List<Keys>>();
            var delay = 0;
            var tt = args.Contains("!!");
            //			Debug.WriteLine("tt?"+tt+" " +args);
            if (args.Contains("|") || tt)
            {
                var axy = args.Split(new[] { tt ? "!!" : "|" }, 2, StringSplitOptions.None);
                args = axy[0];
                Int32.TryParse(axy[1], out delay);
                Debug.WriteLine("SimKeyboard set delay:" + delay);
            }
            if (args.Contains(" "))
                multi_args = args.Split(' ');
            else
                multi_args = new[] { args };
            for (int i = 0; i != multi_args.Length; i++)
            {
                var keys = new List<Keys>();
                var _args = multi_args[i];
                string[] multi_keys;
                if (_args.Contains("+"))
                    multi_keys = _args.Split('+');
                else
                    multi_keys = new[] { _args };
                var multim = "(.*?)\\*(\\d+)$";
                for (int j = 0; j != multi_keys.Length; j++)
                {
                    var key = multi_keys[j].ToLower();
                    var rma = Regex.Matches(key, multim);
                    var times = 1;
                    if (rma.Count > 0)
                    {
                        key = rma[0].Groups[1].Value;
                        Int32.TryParse(rma[0].Groups[2].Value, out times);
                    }
                    Debug.WriteLine("SimKey: " + key + " " + times + " times");
                    keys.AddRange(strparsekey(key, times));
                }
                all_keys.Add(keys);
            }
            foreach (var keys in all_keys)
            {
                var q = new List<WinAPI.INPUT>();
                foreach (var key in keys)
                {
                    Logging.Log("[EXPR] > Pressing: " + key);
                    if (delay > 0)
                    {
                        KInputs.MakeInput(new[] { KInputs.AddKey(key, true) });
                        Thread.Sleep(delay);
                    }
                    else
                        q.Add(KInputs.AddKey(key, true));
                }
                foreach (var key in keys)
                {
                    Logging.Log("[EXPR] > Releasing: " + key);
                    if (delay > 0)
                    {
                        KInputs.MakeInput(new[] { KInputs.AddKey(key, false) });
                        Thread.Sleep(delay);
                    }
                    else
                        q.Add(KInputs.AddKey(key, false));
                }
                if (delay <= 0)
                {
                    KInputs.MakeInput(q.ToArray());
                }
                Thread.Sleep(5);
            }
            Thread.Sleep(30);
        }
        #endregion
        public static bool IfNW7()
        {
            //			Logging.Log("OS: " +Environment.OSVersion.Version);
            return Environment.OSVersion.Version.Major == 10 || (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor > 1);
        }
        public static void DoLater(Action act, int timeout)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Thread.Sleep(timeout);
                act();
            });
        }
        public static string GetModsStr(bool c, bool cr, bool s, bool sr, bool a, bool ar, bool w, bool wr)
        {
            var modstr = new StringBuilder();
            if (c) { modstr.Append("LCtrl + "); }
            if (cr) { modstr.Append("RCtrl + "); }
            if (s) { modstr.Append("LShift + "); }
            if (sr) { modstr.Append("RShift + "); }
            if (a) { modstr.Append("LAlt + "); }
            if (ar) { modstr.Append("RAlt + "); }
            if (w) { modstr.Append("LWin + "); }
            if (wr) { modstr.Append("RWin + "); }
            return modstr.ToString();
        }
        //		public static void SetNextLayout() {
        //			var CUR = Locales.GetCurrentLocale();
        //			var CUR_IND = Program.locales.ToList().FindIndex(lid => lid.uId == CUR);
        //			CUR_IND++;
        //			if (CUR_IND >= Program.locales.Length)
        //				CUR_IND = 0;
        //			Debug.WriteLine("NEXT LAYOUT: " + Program.locales[CUR_IND].Lang + " IND " + CUR_IND  + " LEN " + Program.locales.Length + " CUR " + CUR) ;
        //		}
        static bool IsUpperInput(bool symbolic = false)
        {
            bool caps = Control.IsKeyLocked(Keys.CapsLock), shishift = (shift || shift_r);
            if (SwitcherUI.CapsLockDisablerTimer)
                caps = false;
            if (symbolic)
                return shishift;
            if ((shishift && !caps) || (!shishift && caps))
                return true;
            if ((shishift && caps) || (!shishift && !caps))
                return false;
            return false;
        }
        public static bool OverlayExcluded(string excluded)
        {
            var processes = Process.GetProcesses();
            string _checked = "";
            foreach (var p in processes)
            {
                _checked += p.ProcessName;
                var excl = excluded.ToLower().Split('|');
                foreach (var e in excl)
                {
                    if (e.Replace(".exe", "") == p.ProcessName.ToLower())
                    {
                        Debug.WriteLine("Checked processes" + _checked);
                        return true;
                    }
                }
            }
            Debug.WriteLine("Checked processes" + _checked);
            return false;
        }
        public static bool ExcludedProgram(bool onlysnip = false, IntPtr hwnd = default(IntPtr), bool onlyas = false)
        {
            if (Program.switcher == null) return false;
            if (String.IsNullOrEmpty(SwitcherUI.ExcludedPrograms))
            {
                if (onlysnip && String.IsNullOrEmpty(SwitcherUI.onlySnippetsExcluded)) { return false; }
                if (onlyas && String.IsNullOrEmpty(SwitcherUI.onlyAutoSwitchExcluded)) { return false; }
                if (!onlysnip && !onlyas) { return false; }
            }
            if (hwnd == IntPtr.Zero || hwnd == default(IntPtr))
                hwnd = WinAPI.GetForegroundWindow();
            if (NOT_EXCLUDED_HWNDs.Contains(hwnd) && (!onlysnip && !onlyas))
            {
                Logging.Log("[EXCL] > This program was been checked already, it is not excluded hwnd: " + hwnd);
                return false;
            }
            if (onlysnip && !onlyas)
            {
                if (SNI_EXCLUDED_HWNDs.Contains(hwnd))
                {
                    Logging.Log("[EXCL] > Excluded program by snippets excluded program saved hwnd: " + hwnd);
                    return true;
                }
                if (SNI_NOT_EXCLUDED_HWNDs.Contains(hwnd))
                {
                    Logging.Log("[EXCL] > This program was been checked already, it is snippets not excluded hwnd: " + hwnd);
                    return false;
                }
            }
            if (!onlysnip && onlyas)
            {
                if (AS_EXCLUDED_HWNDs.Contains(hwnd))
                {
                    Logging.Log("[EXCL] > Excluded program by autoswitch excluded program saved hwnd: " + hwnd);
                    return true;
                }
                if (AS_NOT_EXCLUDED_HWNDs.Contains(hwnd))
                {
                    Logging.Log("[EXCL] > This program was been checked already, it is autoswitch not excluded hwnd: " + hwnd);
                    return false;
                }
            }
            if (!EXCLUDED_HWNDs.Contains(hwnd))
            {
                uint pid;
                WinAPI.GetWindowThreadProcessId(hwnd, out pid);
                Process prc = null;
                try
                {
                    prc = Process.GetProcessById((int)pid);
                    if (prc == null) return false;
                    Logging.Log("Active Window Process NAME: " + prc.ProcessName);
                    var onlys = false;
                    if (onlysnip)
                    {
                        Debug.WriteLine("ONLY SNIP CHECK...");
                        if (!String.IsNullOrEmpty(SwitcherUI.onlySnippetsExcluded))
                        {
                            onlys = SwitcherUI.onlySnippetsExcluded.Split('|').Contains(prc.ProcessName.ToLower() + ".exe");
                            Debug.WriteLine("ONLYSNIP EXCLUDE?: " + onlys);
                            if (onlys)
                                SNI_EXCLUDED_HWNDs.Add(hwnd);
                        }
                        else
                        {
                            SNI_NOT_EXCLUDED_HWNDs.Add(hwnd);
                        }
                    }
                    if (onlyas)
                    {
                        if (!String.IsNullOrEmpty(SwitcherUI.onlyAutoSwitchExcluded))
                        {
                            onlys = SwitcherUI.onlyAutoSwitchExcluded.Split('|').Contains(prc.ProcessName.ToLower() + ".exe");
                            Debug.WriteLine("ONLYAS EXCLUDE?: " + onlys);
                            if (onlys)
                                AS_EXCLUDED_HWNDs.Add(hwnd);
                        }
                        else
                        {
                            AS_NOT_EXCLUDED_HWNDs.Add(hwnd);
                        }
                    }
                    if (SwitcherUI.ExcludedPrograms.Replace(Environment.NewLine, " ").ToLower().Contains(prc.ProcessName.ToLower().Replace(" ", "_")) || onlys)
                    {
                        Logging.Log(prc.ProcessName + "->excluded" + (onlys ? " ONLY SNIPPETS or AS" : ""));
                        if (!onlys)
                            EXCLUDED_HWNDs.Add(hwnd);
                        return true;
                    }
                }
                catch { Logging.Log("[EXCL] > Process with id [" + pid + "] not exist...", 1); }
            }
            else
            {
                Logging.Log("[EXCL] > Excluded program by excluded program saved hwnd: " + hwnd);
                return true;
            }
            NOT_EXCLUDED_HWNDs.Add(hwnd);
            return false;
        }
        public static void ResetPersistentTimer()
        {
            if (SwitcherUI.PersistentLayoutForLayout1)
            {
                Logging.Log("Reset persistent layout 1 timer.");
                Program.switcher.persistentLayout1Check.Stop();
                Program.switcher.persistentLayout1Check.Start();
            }
            if (SwitcherUI.PersistentLayoutForLayout2)
            {
                Logging.Log("Reset persistent layout 2 timer.");
                Program.switcher.persistentLayout2Check.Stop();
                Program.switcher.persistentLayout2Check.Start();
            }
        }
        public static void AS_IGN_fun()
        {
            ResetPersistentTimer(); // not related to AS directly, but uses same triggers of switching layout.
            if (AS_IGN_LS)
            {
                if (AS_IGN_RULES.Contains("L"))
                {
                    Debug.WriteLine("[HEY] > " + was_ls);
                    if (was_ls)
                    {
                        was_ls = was_back = was_del = false;
                    }
                    else
                    {
                        was_ls = true;
                        if (AS_IGN_RULES.Contains("T"))
                        {
                            if (AS_IGN_RESET != null)
                            {
                                AS_IGN_RESET.Stop();
                                AS_IGN_RESET.Dispose();
                                return;
                            }
                            AS_IGN_RESET = new System.Timers.Timer();
                            AS_IGN_RESET.Interval = AS_IGN_TIMEOUT;
                            AS_IGN_RESET.Elapsed += (_, __) =>
                            {
                                Debug.WriteLine("+++++++++++ TIMER STOP" + AS_IGN_TIMEOUT);
                                was_ls = was_back = was_del = false;
                                AS_IGN_RESET.Stop();
                                AS_IGN_RESET.Dispose();
                                AS_IGN_RESET = null;
                            };
                            AS_IGN_RESET.Start();
                            Debug.WriteLine("+++++++++++ TIMER START" + AS_IGN_TIMEOUT);
                        }
                    }
                }
                else
                    was_ls = true;
            }
        }
        static void SpecificKey(Keys Key, uint MSG, int vkCode = 0)
        {
            Logging.Log("[SPKEY] > Check on key: [" + Key + "]" + " MSG: " + MSG.ToString());
            //			Debug.WriteLine("SPK:" + skip_spec_keys);
            if (skip_spec_keys > 0)
            {
                skip_spec_keys--;
                if (skip_spec_keys < 0)
                    skip_spec_keys = 0;
                return;
            }
            //			Debug.WriteLine("Speekky->" + Key);
            for (int i = 1; i != 5; i++)
            {
                if ((MSG == WinAPI.WM_KEYUP || MSG == WinAPI.WM_SYSKEYUP || vkCode == 240))
                {
                    var specificKey = (int)typeof(SwitcherUI).GetField("Key" + i).GetValue(Program.switcher);
                    if (SwitcherUI.ChangeLayoutInExcluded || !ExcludedProgram())
                    {
                        #region Switch between layouts with one key
                        bool F18 = Key == Keys.F18;
                        bool GJIME = false;
                        var npre = ((int)preKey == (int)Keys.None || (int)preKey == (int)Key);
                        var altgr = (Key == Keys.RMenu && Key == Keys.LControlKey) ||
                            (Key == Keys.RMenu && Key == Keys.RControlKey) ||
                            (Key == Keys.LMenu && Key == Keys.LControlKey) ||
                            ((ctrl && Key == Keys.RMenu) || (alt_r && Key == Keys.LControlKey)) ||
                            ((ctrl && Key == Keys.LMenu) || (alt && Key == Keys.LControlKey)) ||
                            ((ctrl_r && Key == Keys.RMenu) || (alt_r && Key == Keys.RControlKey)) ||
                            ((ctrl_r && Key == Keys.LMenu) || (alt && Key == Keys.RControlKey));
                        if (specificKey == 8) // Shift+CapsLock
                            if (vkCode == 240)
                            { // Google Japanese IME's  Shift+CapsLock repam fix
                                skip_spec_keys++; // Skip next CapsLock up event
                                GJIME = true;
                            }
                        if ((Key == Keys.CapsLock && !shift && !shift_r && !alt && !alt_r && !ctrl && !ctrl_r && !win && !win_r && specificKey == 1) ||
                            (Key == Keys.CapsLock && (shift || shift_r) && !alt && !alt_r && !ctrl && !ctrl_r && !win && !win_r && specificKey == 8))
                            if (Control.IsKeyLocked(Keys.CapsLock))
                                DoSelf(() => { KeybdEvent(Keys.CapsLock, 0); KeybdEvent(Keys.CapsLock, 2); }, "mod_and_caps_onoff");
                        var speclayout = (string)typeof(SwitcherUI).GetField("Layout" + i).GetValue(Program.switcher);
                        if (String.IsNullOrEmpty(speclayout))
                        {
                            Logging.Log("[SPKEY] > No layout for Layout" + i + " variable.");
                            continue;
                        }
                        if (speclayout == Program.Lang[Languages.Element.SwitchBetween])
                        {
                            if (specificKey == 12 && Key == Keys.Tab && !ctrl && !ctrl_r && !shift_r && !shift && !win && !win_r && !alt && !alt_r)
                            {
                                Logging.Log("[SPKEY] > Changing layout by Tab key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 11 && (
                                (Key == Keys.LShiftKey && ctrl) || (Key == Keys.RShiftKey && ctrl_r) ||
                                (Key == Keys.LControlKey && shift) || (Key == Keys.RControlKey && shift_r)) && !keyAfterCTRLSHIFT && !win && !win_r && !alt && !alt_r)
                            {
                                Logging.Log("[SPKEY] > Changing layout by Ctrl+Shift key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 10 && (
                                (Key == Keys.LShiftKey && alt) || (Key == Keys.RShiftKey && alt_r) ||
                                (Key == Keys.LMenu && shift) || (Key == Keys.RMenu && shift_r)) && !keyAfterALTSHIFT && !win && !win_r && !ctrl && !ctrl_r)
                            {
                                Logging.Log("[SPKEY] > Changing layout by Alt+Shift key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 8 && (Key == Keys.CapsLock || F18 || GJIME) && (shift || shift_r) && !alt && !alt_r && !ctrl && !ctrl_r)
                            {
                                Logging.Log("[SPKEY] > Changing layout by Shift+CapsLock" + (GJIME ? "(KeyCode: 240, Google Japanese IME's Shift+CapsLock remap)" : "") + (F18 ? "(F18)" : "") + " key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            else
                            if (!shift && !shift_r && !alt && !alt_r && !ctrl && !ctrl_r && !win && !win_r && specificKey == 1 &&
                                    (Key == Keys.CapsLock || F18))
                            {
                                ChangeLayout();
                                AS_IGN_fun();
                                Logging.Log("[SPKEY] > Changing layout by CapsLock" + (F18 ? "(F18)" : "") + " key.");
                                return;
                            }
                            if (specificKey == 2 && Key == Keys.LControlKey && !keyAfterCTRL && npre)
                            {
                                Logging.Log("[SPKEY] > Changing layout by L-Ctrl key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 3 && Key == Keys.RControlKey && !keyAfterCTRL && npre)
                            {
                                Logging.Log("[SPKEY] > Changing layout by R-Ctrl key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 4 && Key == Keys.LShiftKey && !keyAfterSHIFT && npre)
                            {
                                Logging.Log("[SPKEY] > Changing layout by L-Shift key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 5 && Key == Keys.RShiftKey && !keyAfterSHIFT && npre)
                            {
                                Logging.Log("[SPKEY] > Changing layout by R-Shift key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 6 && Key == Keys.LMenu && !keyAfterALT && npre)
                            {
                                Logging.Log("[SPKEY] > Changing layout by L-Alt key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 7 && Key == Keys.RMenu && !keyAfterALT && npre)
                            {
                                Logging.Log("[SPKEY] > Changing layout by R-Alt key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 9 && altgr && !keyAfterALTGR)
                            {
                                Logging.Log("[SPKEY] > Changing layout by AltGr key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 13 && ((Key == Keys.RShiftKey && shift) || (Key == Keys.LShiftKey && shift_r)) &&
                               !alt && !alt_r && !ctrl_r && !ctrl && !win && !win_r)
                            {
                                Logging.Log("[SPKEY] > Changing layout by LShift+RShift key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 14 && ((Key == Keys.LMenu && ctrl) || (Key == Keys.LControlKey && alt)) &&
                               !alt_r && !ctrl_r && !win && !win_r && !(keyAfterALT && keyAfterCTRL))
                            {
                                Logging.Log("[SPKEY] > Changing layout by LCtrl+LAlt key.");
                                ChangeLayout();
                                AS_IGN_fun();
                                return;
                            }
                            //							if (catched) {
                            //			       			    if (Key == Keys.LMenu)
                            //									DoSelf(()=>{ Thread.Sleep(150); KeybdEvent(Keys.LMenu, 0); KeybdEvent(Keys.LMenu, 2); });
                            //			       			    if (Key == Keys.RMenu)
                            //									DoSelf(()=>{ Thread.Sleep(150); KeybdEvent(Keys.RMenu, 0); KeybdEvent(Keys.RMenu, 2); });
                            //							}
                            #endregion
                        }
                        else
                        {
                            #region By layout switch
                            var matched = false;
                            if (specificKey == 12 && Key == Keys.Tab && !ctrl && !ctrl_r && !shift_r && !shift && !win && !win_r && !alt && !alt_r)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by Tab key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 11 && (
                                (Key == Keys.LShiftKey && ctrl) || (Key == Keys.RShiftKey && ctrl_r) ||
                                (Key == Keys.LControlKey && shift) || (Key == Keys.RControlKey && shift_r)) && !keyAfterCTRLSHIFT && !win && !win_r && !alt && !alt_r)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by Ctrl+Shift key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 10 && (
                                (Key == Keys.LShiftKey && alt) || (Key == Keys.RShiftKey && alt_r) ||
                                (Key == Keys.LMenu && shift) || (Key == Keys.RMenu && shift_r)) && !keyAfterALTSHIFT && !win && !win_r && !ctrl && !ctrl_r)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by Alt+Shift key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 8 && (Key == Keys.CapsLock || F18 || GJIME) && (shift || shift_r) && !alt && !alt_r && !ctrl && !ctrl_r)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by Shift+CapsLock" + (GJIME ? "(KeyCode: 240, Google Japanese IME's Shift+CapsLock remap)" : "") + (F18 ? "(F18)" : "") + " key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                return;
                            }
                            else
                            if (specificKey == 1 && (Key == Keys.CapsLock || F18))
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by Caps Lock" + (F18 ? "(F18)" : "") + " key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 2 && Key == Keys.LControlKey && !keyAfterCTRL && npre)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by  L-Ctrl key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 3 && Key == Keys.RControlKey && !keyAfterCTRL && npre)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by R-Ctrl key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 4 && Key == Keys.LShiftKey && !keyAfterSHIFT && npre)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by L-Shift key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 5 && Key == Keys.RShiftKey && !keyAfterSHIFT && npre)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by R-Shift key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 6 && Key == Keys.LMenu && !keyAfterALT && npre)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by L-Alt key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                DoSelf(() => { KeybdEvent(Keys.LMenu, 0); KeybdEvent(Keys.LMenu, 2); }, "lmenu_spkey");
                                return;
                            }
                            if (specificKey == 7 && Key == Keys.RMenu && !keyAfterALT && npre)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by R-Alt key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                DoSelf(() => { KeybdEvent(Keys.RMenu, 0); KeybdEvent(Keys.RMenu, 2); }, "rmenu_spkey");
                                return;
                            }
                            if (specificKey == 9 && altgr && !keyAfterALTGR)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by AltGr key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                DoSelf(() => { KeybdEvent(Keys.RMenu, 0); KeybdEvent(Keys.RMenu, 2); }, "altgr_spkey");
                                return;
                            }
                            if (specificKey == 13 && ((Key == Keys.RShiftKey && shift) || (Key == Keys.LShiftKey && shift_r)) &&
                               !alt && !alt_r && !ctrl_r && !ctrl && !win && !win_r)
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by LShift+RShift key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                return;
                            }
                            if (specificKey == 14 && ((Key == Keys.LMenu && ctrl) || (Key == Keys.LControlKey && alt)) &&
                                !alt_r && !ctrl_r && !win && !win_r && !(keyAfterALT && keyAfterCTRL))
                            {
                                Logging.Log("[SPKEY] > Switching to specific layout by LCtrl+LAlt key.");
                                ChangeToLayout(Locales.ActiveWindow(), Locales.GetLocaleFromString(speclayout).uId);
                                matched = true;
                                AS_IGN_fun();
                                return;
                            }
                            try
                            {
                                if (matched)
                                {
                                    Logging.Log("[SPKEY] > Available layout from string [" + speclayout + "] & id [" + i + "].");
                                    //Fix for alt-show-menu in programs
                                    //				       			    if (Key == Keys.LMenu)
                                    //										DoSelf(()=>{ KeybdEvent(Keys.LMenu, 0); KeybdEvent(Keys.LMenu, 2); });
                                    //				       			    if (Key == Keys.RMenu)
                                    //										DoSelf(()=>{ KeybdEvent(Keys.RMenu, 0); KeybdEvent(Keys.RMenu, 2); });
                                }
                            }
                            catch
                            {
                                Logging.Log("[SPKEY] > No layout available from string [" + speclayout + "] & id [" + i + "].");
                            }
                        }
                        #endregion
                    }
                }
            }
            ctrl = IsKDown(Keys.LControlKey);
            ctrl_r = IsKDown(Keys.RControlKey);
            alt = IsKDown(Keys.LMenu);
            alt_r = IsKDown(Keys.RMenu);
            win = IsKDown(Keys.LWin);
            win_r = IsKDown(Keys.RWin);
            shift = IsKDown(Keys.LShiftKey);
            shift_r = IsKDown(Keys.RShiftKey);
        }
        public static void ClearModifiers()
        {
            win = alt = ctrl = shift = win_r = alt_r = ctrl_r = shift_r = false;
            LLHook.ClearModifiers();
            SendModsUp((int)(WinAPI.MOD_ALT + WinAPI.MOD_CONTROL + WinAPI.MOD_SHIFT + WinAPI.MOD_WIN), false);
        }
        static void ClearWord(bool LastWord = false, bool LastLine = false, bool Snippet = false, string ClearReason = "", bool lastSnippet = false, bool wass = false)
        {
            string ReasonEnding = ".";
            Debug.WriteLine("CLEAR: " + ClearReason);
            if (SwitcherUI.LoggingEnabled && !String.IsNullOrEmpty(ClearReason))
                ReasonEnding = ", reason: [" + ClearReason + "].";
            if (LastWord)
            {
                c_word_backup_last = new List<YuKey>(c_word_backup);
                c_word_backup = new List<YuKey>(Program.c_word);
                if (Program.c_word.Count > 0)
                {
                    Program.c_word.Clear();
                    lastLWClearReason = ReasonEnding;
                    Logging.Log("[CLWORD] > Cleared last word" + ReasonEnding);
                }
            }
            if (LastLine)
            {
                if (Program.c_words.Count > 0)
                {
                    Program.c_words.Clear();
                    Logging.Log("[CLWORD] > Cleared last line" + ReasonEnding);
                }
            }
            if (Snippet)
            {
                if (c_snip.Count > 0)
                {
                    if (SwitcherUI.SnippetsEnabled)
                    {
                        c_snip.Clear();
                        Logging.Log("[CLWORD] > Cleared current snippet" + ReasonEnding);
                    }
                }
                if (lastSnippet)
                {
                    last_snip = "";
                    Debug.WriteLine("CL LASTSNIP");
                    Logging.Log("[CLWORD] > Cleared last snippet" + ReasonEnding);
                }
            }
            if (wass) { was_back = was_del = was_ls = false; }
            SwitcherUI.RefreshFLAG();
            Program.switcher.RefreshAllIcons();
            Program.switcher.UpdateLDs();
        }
        public static string[] SplitWords(string LINE)
        {
            if (string.IsNullOrEmpty(LINE)) { return new string[] { "" }; }
            var LIST = new List<string>();
            string left = LINE;
            int ind = left.IndexOf(' ');
            while ((ind = left.IndexOf(' ')) != -1)
            {
                ind = left.IndexOf(' ');
                if (ind == 0)
                    ind = 1;
                var word = left.Substring(0, ind);
                left = left.Substring(ind, left.Length - ind);
                //				Debug.WriteLine(word + "] " + ind + " [" + left);
                LIST.Add(word);
            }
            if (ind == -1 && !String.IsNullOrEmpty(left))
            {
                LIST.Add(left);
                //				Debug.WriteLine(left);
            }
            return LIST.ToArray();
        }
        public static string ConvertText(string ClipStr, uint l1 = 0, uint l2 = 0)
        {
            if (String.IsNullOrEmpty(ClipStr))
            {
                Logging.Log("Empty or null ClipStr.", 2);
                return "";
            }
            if (l1 == 0) l1 = cs_layout_last;
            if (l2 == 0) l2 = GetNextLayout(l1).uId;
            var result = new StringBuilder();
            var index = 0;
            if (SwitcherUI.OneLayoutWholeWord)
            {
                Logging.Log("[CT] > Using one layout whole word convert text mode.");
                var lines = Regex.Split(ClipStr, @"\r?\n");
                int lcnt = 0;
                foreach (var line in lines)
                {
                    var allWords = SplitWords(line);
                    var word_index = 0;
                    foreach (var w in allWords)
                    {
                        if (w == " ")
                        {
                            result.Append(w);
                        }
                        else
                        {
                            var wx = WordGuessLayout(w, l2).Item1;
                            if (!String.IsNullOrEmpty(wx))
                                result.Append(wx);
                            else result.Append(w);
                        }
                        word_index += 1;
                        //						Debug.WriteLine("(" + w + ") ["+ result +"]");
                        index++;
                    }
                    lcnt++;
                    if (lcnt != lines.Count())
                        result.Append('\n');
                }
            }
            else
            {
                Logging.Log("[CT] > Using default convert text mode.");
                for (int I = 0; I != ClipStr.Length; I++)
                {
                    var sm = false;
                    var c = ClipStr[I];
                    if (c == 'ո' || c == 'Ո')
                    {
                        if (c == 'ո') sm = true;
                        if (ClipStr.Length > I + 1)
                        {
                            if (ClipStr[I + 1] == 'ւ')
                            {
                                var shrt = l2 >> 16;
                                var _shrt = l1 >> 16;
                                if (shrt == 1033 || shrt == 1041)
                                {
                                    result.Append(sm ? "u" : "U");
                                    I++; continue;
                                }
                                if (_shrt == 1033 || _shrt == 1041)
                                {
                                    result.Append(sm ? "u" : "U");
                                    I++; continue;
                                }
                                if (shrt == 1049)
                                {
                                    result.Append(sm ? "г" : "Г");
                                    I++; continue;
                                }
                                if (_shrt == 1049)
                                {
                                    result.Append(sm ? "г" : "Г");
                                    I++; continue;
                                }
                            }
                        }
                    }
                    var T = InAnother(c, l1, l2);
                    for (int i = 0; i != Program.locales.Length; i++)
                    {
                        var l = Program.locales[i].uId;
                        if (c == '\n')
                            T = "\n";
                        T = GermanLayoutFix(c);
                        T = InAnother(c, l, l2);
                        if (T != "")
                            break;
                        index++;
                    }
                    if (T == "")
                        T = ClipStr[index].ToString();
                    result.Append(T);
                }
            }
            return result.ToString();
        }
        /// <summary>
        /// Converts selected text.
        /// </summary>
        public static void ConvertSelection()
        {
            Debug.WriteLine("Start CS");
            try
            { //Used to catch errors
                DoSelf(() =>
                {
                    Logging.Log("[CS] > Starting Convert selection.");
                    string ClipStr = GetClipStr();
                    if (!String.IsNullOrEmpty(ClipStr))
                    {
                        csdoing = true;
                        Logging.Log("[CS] > Starting conversion of [" + ClipStr + "].");
                        KInputs.MakeInput(KInputs.AddPress(Keys.Back));
                        var result = "";
                        int items = 0;
                        if (SwitcherUI.ConvertSelectionLS && !SwitcherUI.OneLayoutWholeWord)
                        {
                            Logging.Log("[CS] > Using CS-Switch mode.");
                            var wasLocale = Locales.GetCurrentLocale();
                            if (SwitcherUI.UseJKL && !KMHook.JKLERR)
                                wasLocale = SwitcherUI.currentLayout;
                            var wawasLocale = wasLocale;
                            uint nowLocale = 0;
                            if (SwitcherUI.SwitchBetweenLayouts)
                            {
                                nowLocale = CompareLayouts(wasLocale, SwitcherUI.MAIN_LAYOUT1)
                                    ? SwitcherUI.MAIN_LAYOUT2
                                    : SwitcherUI.MAIN_LAYOUT1;
                                if (nowLocale == wasLocale &&
                                    (CompareLayouts(SwitcherUI.currentLayout, SwitcherUI.MAIN_LAYOUT1) ||
                                     CompareLayouts(SwitcherUI.currentLayout, SwitcherUI.MAIN_LAYOUT2)))
                                {
                                    if (!CompareLayouts(wasLocale, SwitcherUI.currentLayout))
                                        nowLocale = SwitcherUI.currentLayout;
                                }
                            }
                            else
                            {
                                Thread.Sleep(10); nowLocale = GetNextLayout().uId;
                            }
                            ChangeLayout(true);
                            var index = 0;
                            var q = new List<WinAPI.INPUT>();
                            foreach (char c in ClipStr)
                            {
                                items++;
                                wasLocale = wawasLocale;
                                var s = "";
                                var sb = "";
                                var yk = new YuKey();
                                var scan = WinAPI.VkKeyScanEx(c, wasLocale);
                                var state = ((scan >> 8) & 0xff);
                                //								var bytes = new byte[255];
                                //								if (state == 1)
                                //									bytes[(int)Keys.ShiftKey] = 0xFF;
                                var scan2 = WinAPI.VkKeyScanEx(c, nowLocale);
                                var state2 = ((scan2 >> 8) & 0xff);
                                //								var bytes2 = new byte[255];
                                //								if (state2 == 1)
                                //									bytes2[(int)Keys.ShiftKey] = 0xFF;
                                if (SwitcherUI.ConvertSelectionLSPlus)
                                {
                                    Logging.Log("[CS] > Using Experimental CS-Switch mode.");
                                    var y = ToUnicodeExMulti((uint)scan, (IntPtr)wasLocale, state == 1);
                                    if (y != '\0') s += y;
                                    Logging.Log("[CS] > Char 1 is [" + s + "] in locale +[" + wasLocale + "].");
                                    if (ClipStr[index].ToString() == s)
                                    {
                                        if (!SymbolIgnoreRules((Keys)(scan & 0xff), state == 1, wasLocale, ref q))
                                        {
                                            Logging.Log("Making input of [" + scan + "] in locale +[" + nowLocale + "].");
                                            q.Add(KInputs.AddString(InAnother(c, wasLocale, nowLocale))[0]);
                                        }
                                        index++;
                                        continue;
                                    }
                                    y = ToUnicodeExMulti((uint)scan2, (IntPtr)nowLocale, state2 == 1);
                                    if (y != '\0') sb += y;
                                    Logging.Log("[CS] > Char 2 is [" + sb + "] in locale +[" + nowLocale + "].");
                                    if (ClipStr[index].ToString() == sb)
                                    {
                                        Logging.Log("[CS] > Char 1, 2 and original are equivalent.");
                                        ChangeToLayout(Locales.ActiveWindow(), wasLocale);
                                        wasLocale = nowLocale;
                                        scan = scan2;
                                        state = state2;
                                    }
                                }
                                if (c == '\n')
                                {
                                    yk.key = Keys.Enter;
                                    yk.upper = false;
                                }
                                else
                                {
                                    if (scan != -1)
                                    {
                                        var key = (Keys)(scan & 0xff);
                                        bool upper = state == 1;
                                        yk = new YuKey() { key = key, upper = upper };
                                        Logging.Log("[CS] > Key of char [" + c + "] = {" + key + "}, upper = +[" + state + "].");
                                    }
                                    else
                                    {
                                        yk = new YuKey() { key = Keys.None };
                                    }
                                }
                                if (yk.key == Keys.None)
                                { // retype unrecognized as unicode
                                    var unrecognized = ClipStr[items - 1].ToString();
                                    WinAPI.INPUT unr = KInputs.AddString(unrecognized)[0];
                                    Debug.WriteLine("[CS] > Key of char [" + c + "] = not exist, using input as string.");
                                    q.Add(unr);
                                }
                                else
                                {
                                    if (!SymbolIgnoreRules(yk.key, yk.upper, wasLocale, ref q))
                                    {
                                        Logging.Log("[CS] > Making input of [" + yk.key + "] key with upper = [" + yk.upper + "].");
                                        if (yk.upper)
                                            q.Add(KInputs.AddKey(Keys.LShiftKey, true));
                                        q.AddRange(KInputs.AddPress(yk.key));
                                        if (yk.upper)
                                            q.Add(KInputs.AddKey(Keys.LShiftKey, false));
                                    }
                                }
                                index++;
                            }
                            KInputs.MakeInput(q.ToArray());
                        }
                        else
                        {
                            var l1 = cs_layout_last;
                            if (SwitcherUI.ConvertSelectionLS)
                            {
                                Logging.Log("[CS] > Using Layout Switch in Convert Selection.");
                                l1 = Locales.GetCurrentLocale();
                                if (SwitcherUI.UseJKL && !KMHook.JKLERR)
                                    l1 = SwitcherUI.currentLayout;
                                ChangeLayout();
                            }
                            var l2 = GetNextLayout(l1).uId;
                            Debug.WriteLine("next: " + l2);
                            result = ConvertText(ClipStr, l1, l2);
                            cs_layout_last = l2;
                            Logging.Log("[CS] > Conversion of string [" + ClipStr + "] from locale [" + l1 + "] into locale [" + l2 + "] became [" + result + "].");
                            //Inputs converted text
                            result = Regex.Replace(result, @"(\d+)[,.?бю/](\d+)[,.?бю/](\d+)[,.?бю/](\d+)", "$1.$2.$3.$4");
                            if (SwitcherUI.UsePaste)
                            {
                                PasteText(result, "Selection");
                            }
                            else
                            {
                                Logging.Log("[CS] > Making input of [" + result + "] as string");
                                KInputs.MakeInput(KInputs.AddString(result));
                            }
                            items = result.Length;
                        }
                        ReSelect(items, "N");
                        SwitcherUI.hk_result = true;
                    }
                    NativeClipboard.Clear();
                    RestoreClipBoard();
                }, "convert_selection");
            }
            catch (Exception e)
            {
                Logging.Log("[CS] > Convert Selection encountered error, details:\r\n" + e.Message + "\r\n" + e.StackTrace, 1);
            }
            Memory.Flush();
        }
        public enum ConvT
        {
            Transliteration,
            Random,
            Title,
            Swap,
            Upper,
            Lower,
            Custom
        }
        public static void SelectionConversion(ConvT t = 0)
        {
            var tn = Enum.GetName(typeof(ConvT), t);
            try
            { //Used to catch errors
                Locales.IfLessThan2();
                DoSelf(() =>
                {
                    Logging.Log("[" + tn + "] > Starting " + tn + " selection.");
                    string ClipStr = GetClipStr();
                    var cT = "";
                    if (!String.IsNullOrEmpty(ClipStr))
                    {
                        if (SwitcherUI.CycleCaseSaveBase)
                        {
                            if (String.IsNullOrEmpty(SwitcherUI.CycleCaseBase))
                            {
                                SwitcherUI.CycleCaseBase = ClipStr;
                                Debug.WriteLine("CC [B]ase saved: " + SwitcherUI.CycleCaseBase);
                            }
                        }
                        var output = "";
                        switch (t)
                        {
                            case ConvT.Custom:
                                output = CustomReplaceText(ClipStr); cT = "C"; break;
                            case ConvT.Transliteration:
                                output = TransliterateText(ClipStr); cT = "t"; break;
                            case ConvT.Random:
                                output = ToSTULRSelection(ClipStr, false, false, false, true); cT = "R"; break;
                            case ConvT.Title:
                                output = ToSTULRSelection(ClipStr, false, true); cT = "T"; break;
                            case ConvT.Swap:
                                output = ToSTULRSelection(ClipStr, true); cT = "S"; break;
                            case ConvT.Upper:
                                output = ToSTULRSelection(ClipStr); cT = "U"; break;
                            case ConvT.Lower:
                                output = ToSTULRSelection(ClipStr, false, false, true); cT = "L"; break;
                        }
                        if (SwitcherUI.UsePaste)
                        {
                            PasteText(output, tn);
                        }
                        else
                        {
                            Logging.Log("Inputting [" + output + "] as " + tn);
                            if (output[output.Length - 1] == '\n')
                            {
                                var ac = Locales.ActiveWindowProcess().ProcessName.ToLower();
                                Debug.WriteLine("AC: " + ac);
                                if (ac == "winword")
                                {
                                    Debug.WriteLine("Last char is line, and active is word");
                                    var n = 0;
                                    while (output[output.Length - 1] == '\n')
                                    {
                                        output = output.Substring(0, output.Length - 1);
                                        n++;
                                    }
                                    var x = new StringBuilder();
                                    n--;
                                    int real = n - 1;
                                    Debug.WriteLine("EXTRA empty lines:" + real);
                                    if (real > 0)
                                    {
                                        for (int i = 0; i != real; i++)
                                        {
                                            x.Append("\n");
                                        }
                                    }
                                    output = output + x;
                                }
                            }
                            KInputs.MakeInput(KInputs.AddString(output));
                        }
                        ReSelect(output.Length, cT);
                        SwitcherUI.hk_result = true;
                    }
                    NativeClipboard.Clear();
                    RestoreClipBoard();
                }, "selection_convert");
            }
            catch (Exception e)
            {
                Logging.Log("[" + tn + "] > Selection encountered error, details:\r\n" + e.Message + "\r\n" + e.StackTrace, 1);
            }
            Memory.Flush();
        }
        public static void PasteText(string text, string info = "")
        {
            Logging.Log("Pasting [" + text + "]  as " + info);
            RestoreClipBoard(text);
            List<WinAPI.INPUT> a = new List<WinAPI.INPUT>();
            a.Add(KInputs.AddKey(Keys.LControlKey, true));
            var v = SwitcherUI.LibreCtrlAltShiftV && Locales.ActiveWindowProcess().ProcessName.ToLower().Contains("soffice.");
            if (v)
            {
                Logging.Log("Using Libre paste fix.");
                a.Add(KInputs.AddKey(Keys.LShiftKey, true));
                a.Add(KInputs.AddKey(Keys.LMenu, true));
            }
            a.Add(KInputs.AddKey(Keys.V, true));
            KInputs.MakeInput(a.ToArray());
            Thread.Sleep(50);
            a.Clear();
            a.Add(KInputs.AddKey(Keys.LControlKey, false));
            if (v)
            {
                a.Add(KInputs.AddKey(Keys.LShiftKey, false));
                a.Add(KInputs.AddKey(Keys.LMenu, false));
            }
            a.Add(KInputs.AddKey(Keys.V, false));
            KInputs.MakeInput(a.ToArray());
        }
        public static bool LooksLikeRegex(string regex)
        {
            if (regex.Length > 3)
            {
                if (regex[0] == 's' && regex[1] == '/' && regex[regex.Length - 1] == '/')
                    return true;
            }
            return false;
        }
        public static string[] SplitEsc(string input, char sep, char esc = '\\')
        {
            bool esca = false;
            var result = new List<string>();
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i != input.Length; i++)
            {
                var c = input[i];
                if (esca && c == sep)
                {
                    if (buf.Length >= 1)
                        buf.Remove(buf.Length - 1, 1);
                }
                if (!esca && c == sep)
                {
                    result.Add(buf.ToString());
                    buf.Clear();
                    continue;
                }
                esca = false;
                if (c == esc) { esca = true; }
                buf.Append(c);
            }
            if (buf.Length >= 1)
            {
                result.Add(buf.ToString());
            }
            return result.ToArray();
        }
        public static string[] SplitNoEsc(string input, char sep, char esc = '\\', char reCh = '\0', int reC = -1)
        {
            bool esca = false;
            var result = new List<string>();
            StringBuilder buf = new StringBuilder();
            var REs = reCh != '\0';
            for (int i = 0; i != input.Length; i++)
            {
                var c = input[i];
                if ((!REs && !esca && c == sep) || (reCh != '\0' && reC == 0))
                {
                    result.Add(buf.ToString());
                    buf.Clear();
                    if (reC == 0) { result.Add(input.Substring(i + 1, input.Length - 1 - i)); break; }
                    continue;
                }
                if (REs) { if (!esca && c == reCh) { reC--; } }
                if (esca && c != sep) { esca = false; }
                if (c == esc) { esca = true; }
                buf.Append(c);
            }
            return result.ToArray();
        }
        public static bool __TSDictContainsOnlyRegex()
        {
            for (int z = 0; z != transliterationDict.len; z++)
            {
                if (!LooksLikeRegex(transliterationDict[z].k))
                {
                    Debug.WriteLine("TSDict actually not all regex, the: " + transliterationDict[z].k + " is not regex");
                    return false;
                }
            }
            return true;
        }
        public static string __dictReplace(DICT<string, string> d, string input, ref bool only_regex, bool reverse = false, bool donttouchagain = false)
        {
            var ir = input.Replace("\r", "");
            var lines = ir.Split('\n');
            var result = new StringBuilder();
            List<string> listagain = null;
            if (donttouchagain)
                listagain = new List<string>();
            for (int o = 0; o != lines.Length; o++)
            {
                var line = lines[o];
                for (int z = 0; z != d.len; z++)
                {
                    var repl = d[z].k;
                    var tore = d[z].v;
                    if (reverse) { tore = repl; repl = d[z].v; }
                    var isRegex = LooksLikeRegex(repl);
                    Debug.WriteLine("CHECK: " + repl + " " + isRegex);
                    if (String.IsNullOrEmpty(repl))
                    {
                        if (LooksLikeRegex(tore)) { isRegex = true; repl = tore; }
                        else { continue; }
                    }
                    if (!isRegex)
                    {
                        if (line.Contains(repl))
                        {
                            var replace_ok = true;
                            if (donttouchagain)
                            {
                                if (listagain.Contains(repl))
                                {
                                    Logging.Log("[CUSTOM] > Stopping, that one already replaced: " + repl);
                                    replace_ok = false;
                                }
                            }
                            if (replace_ok)
                            {
                                line = line.Replace(repl, tore);
                                if (donttouchagain)
                                {
                                    if (!listagain.Contains(tore))
                                        listagain.Add(tore);
                                }
                                Debug.WriteLine("Replace: " + repl + " => " + tore + " [" + line + "]");
                                only_regex = false;
                            }
                        }
                    }
                    else
                    {
                        var spl = SplitNoEsc(repl, '/');
                        Debug.WriteLine("SplitNoEsc: " + spl.Length);
                        if (spl.Length < 3) { Debug.WriteLine("Wrong regex, unerminated /, etc."); continue; }
                        var regex = spl[1];
                        var regex_r = spl[2];
                        //Debug.WriteLine("REGEX_REPLACE: "+repl);
                        Debug.WriteLine(line + " => s/" + regex + "/" + regex_r);
                        //if (!rir) { Debug.WriteLine("NOT REGEX
                        //					if ((!rir && rr)) {
                        //						Debug.WriteLine("SWAP REGEX.");
                        //						var rx = regex;
                        //						regex = regex_r; regex = rx;
                        //					}
                        //					if (rir&&rr) { Debug.WriteLine("You can't replace regex to regex"); continue; }
                        var repi = RegexREPLACEP(line, regex, regex_r);
                        if (!String.IsNullOrEmpty(repi))
                        {
                            Debug.WriteLine("Regex replace success: " + repi + ", s/" + regex + "/" + regex_r + "/g & " + line);
                            line = repi;
                        }
                    }
                }
                result.Append(line).Append((o == lines.Length - 1 ? "" : "\n"));
            }
            return result.ToString();
        }
        public static string __TSDictReplace(string input, bool reverse = false, bool noloop = false)
        {
            bool only_regex = true;
            var orig = input;
            input = __dictReplace(transliterationDict, input, ref only_regex, reverse);
            Debug.WriteLine("Replaced: " + input + " only regex? " + only_regex);
            if (only_regex && !__TSDictContainsOnlyRegex() && !noloop)
            {
                var test = __TSDictReplace(input, true, true);
                Debug.WriteLine("One more time with reverse: " + input + " & " + test);
                if (test != orig)
                    input = test;
            }
            return input;
        }
        public static string CustomReplaceText(string ClipStr)
        {
            bool onre = false;
            if (CustomConversionDICT == null) return ClipStr;
            var ret = __dictReplace(CustomConversionDICT, ClipStr, ref onre, false, true);
            if (ret.Equals(ClipStr))
            {
                ret = __dictReplace(CustomConversionDICT, ClipStr, ref onre, true, true);
            }
            return ret;
        }
        public static string TransliterateText(string ClipStr)
        {
            if (String.IsNullOrEmpty(ClipStr))
            {
                Logging.Log("Empty or null ClipStr.", 2);
                return "";
            }
            Logging.Log("[TRANSLTRT] > Starting Transliterate text.");
            string output = __TSDictReplace(ClipStr);
            Debug.WriteLine("1st TR: " + output);
            if (ClipStr == output)
            {
                output = __TSDictReplace(ClipStr, true);
                //if (ClipStr == output)
            }
            return output;
        }
        public static string ToSTULRSelection(string ClipStr, bool swap = false, bool title = false, bool lower = false, bool random = false)
        {
            string[] ClipStrLines = ClipStr.Split('\n');
            int lines = 0;
            var output = new StringBuilder();
            foreach (var line in ClipStrLines)
            {
                lines++;
                string[] ClipStrWords = SplitWords(line);
                int words = 0;
                foreach (var word in ClipStrWords)
                {
                    words++;
                    var STULR = new StringBuilder();
                    if (title)
                    {
                        if (word.Length > 0)
                            STULR.Append(word[0].ToString().ToUpper());
                        if (word.Length > 1)
                            foreach (char ch in word.Substring(1, word.Length - 1))
                            {
                                STULR.Append(char.ToLower(ch));
                            }
                    }
                    else
                    {
                        foreach (char ch in word)
                        {
                            if (random)
                            {
                                if (SwitcherUI.rand.NextDouble() >= 0.5)
                                {
                                    STULR.Append(char.ToLower(ch));
                                }
                                else
                                {
                                    STULR.Append(char.ToUpper(ch));
                                }
                            }
                            else if (swap)
                            {
                                if (char.IsUpper(ch))
                                    STULR.Append(char.ToLower(ch));
                                else if (char.IsLower(ch))
                                    STULR.Append(char.ToUpper(ch));
                                else
                                    STULR.Append(ch);
                            }
                            else
                            {
                                if (lower)
                                    STULR.Append(char.ToLower(ch));
                                else
                                    STULR.Append(char.ToUpper(ch));
                            }
                        }
                    }
                    output.Append(STULR.ToString());
                }
                if (lines != ClipStrLines.Length)
                    output.Append("\n");
            }
            return output.ToString();
        }
        public static void ReSelect(int count, string cT = "")
        {
            if (SwitcherUI.ReSelect)
            {
                if (!SwitcherUI.ReselectCustoms.Contains(cT))
                    return;
                //reselects text
                Logging.Log("Reselecting text.");
                var f = new List<WinAPI.INPUT>();
                f.Add(KInputs.AddKey(Keys.RShiftKey, true));
                f.AddRange(KInputs.AddPress(Keys.Left, count));
                f.Add(KInputs.AddKey(Keys.RShiftKey, false));
                KInputs.MakeInput(f.ToArray());
            }
        }
        static string ArmenianSignleCharFix(string word, uint next_layout, uint this_layout = 0)
        {
            var shrt = next_layout >> 16;
            var _shrt = this_layout >> 16;
            //			if (shrt == 1033 || shrt == 1041) // English/Japanese
            var repl = word;
            //			Debug.WriteLine("Next: " + next_layout + ", word: " +word);
            if (shrt == 1067) // Armenian
                repl = word.Replace("u", "w6").Replace("U", "W6").Replace("г", "ц6").Replace("Г", "Ц6");
            if (_shrt == 1067)
            {
                if (shrt == 1033 || shrt == 1041) // English/Japanese
                    repl = word.Replace("ու", "u").Replace("Ու", "U");
                if (shrt == 1049) //  Russian
                    repl = word.Replace("ու", "Г").Replace("Ու", "Г");
            }
            //			else if (shrt == 1049) // Russian
            //				word.Replace("Ու", "Г").Replace("ու", "г");
            Debug.WriteLine("RELT: " + repl);
            return repl;
        }
        static string ASsymDiffReplace(string input)
        {
            if (ASsymDiffDICT.len < 1) return input;
            var buf = new StringBuilder();
            for (int i = 0; i != input.Length; i++)
            {
                var c = input[i];
                string T = c.ToString();
                for (int z = 0; z != ASsymDiffDICT.len; z++)
                {
                    if (c == ASsymDiffDICT[z].k[0])
                        T = ASsymDiffDICT[z].v;
                }
                buf.Append(T);
            }
            return buf.ToString();
        }
        static List<YuKey> QWERTZ_wordFIX(List<YuKey> word)
        {
            if (SwitcherUI.QWERTZ_fix)
            {
                for (int a = 0; a != word.Count; a++)
                {
                    if (word[a].key == Keys.Z)
                    {
                        Debug.WriteLine("AS: QWERTZ REPLACE Z=>Y :" + a);
                        word[a] = new YuKey() { key = Keys.Y, upper = word[a].upper, altnum = word[a].altnum };
                    }
                    else if (word[a].key == Keys.Y)
                    {
                        Debug.WriteLine("AS: QWERTZ REPLACE Y=>Z :" + a);
                        word[a] = new YuKey() { key = Keys.Z, upper = word[a].upper, altnum = word[a].altnum };
                    }
                }
            }
            return word;
        }
        static string GermanLayoutFix(char c)
        {
            if (!SwitcherUI.QWERTZ_fix)
                return "";
            var T = "";
            for (int z = 0; z != LayReplDict.len; z++)
            {
                if (c == LayReplDict[z].k[0])
                    T = LayReplDict[z].v;
            }
            Logging.Log("German fix T:" + T + "/ c: " + c);
            return T;
        }
        static bool WaitForClip2BeFree()
        {
            Debug.WriteLine(">> WFC2F");
            IntPtr CB_Blocker = IntPtr.Zero;
            int tries = 0;
            do
            {
                CB_Blocker = WinAPI.GetOpenClipboardWindow();
                if (CB_Blocker == IntPtr.Zero) break;
                Logging.Log("Clipboard blocked by process id [" + WinAPI.GetWindowThreadProcessId(CB_Blocker, IntPtr.Zero) + "].", 2);
                tries++;
                if (tries > 3000)
                {
                    Logging.Log("3000 Tries to wait for clipboard blocker ended, blocker didn't free'd clipboard |_|.", 2); return false;
                }
            } while (CB_Blocker != IntPtr.Zero);
            Debug.WriteLine(">> WFC2F t: " + tries);
            return true;
        }
        public static bool RestoreClipBoard(string special = "")
        {
            Debug.WriteLine(">> RC");
            var restore = special;
            bool spc = true;
            if (String.IsNullOrEmpty(restore))
            {
                if (SwitcherUI.ClipBackOnlyText)
                {
                    restore = lastClipText;
                    spc = false;
                }
                else
                {
                    NativeClipboard.clip_set(lastClip);
                    return true;
                }
            }
            Logging.Log((spc ? "Force-Text " : "") + "Restoring clipboard text: [" + restore + "].");
            if (WaitForClip2BeFree())
            {
                try { Clipboard.SetDataObject(restore, true, 5, 120); return true; }
                catch { Logging.Log("Error during clipboard " + (spc ? "Special " : "") + "text restore after 5 tries.", 2); return false; }
            }
            return false;
        }
        public static string GetClipboard(int tries = 1, int timeout = 5)
        {
            var txt = NativeClipboard.GetText();
            for (int i = 1; i < tries; i++)
            {
                if (!String.IsNullOrEmpty(txt)) break;
                txt = NativeClipboard.GetText();
                Thread.Sleep(timeout);
            }
            return txt;
        }
        /// <summary>
        /// Sends RCtrl + Insert to selected get text, and returns that text by using WinAPI.GetText().
        /// </summary>
        /// <returns>string</returns>
        static string MakeCopy()
        {
            Debug.WriteLine(">> MC");
            ClearModifiers();
            KInputs.MakeInput(KInputs.AddPress(Keys.Insert), (int)WinAPI.MOD_CONTROL);
            Thread.Sleep(30);
            var txt = NativeClipboard.GetText();
            if (string.IsNullOrEmpty(txt))
            {
                KInputs.MakeInput(KInputs.AddPress(Keys.C), (int)WinAPI.MOD_CONTROL);
                Thread.Sleep(30);
                txt = NativeClipboard.GetText();
            }
            return txt;
        }
        public static string GetClipStr()
        {
            Debug.WriteLine(">> GCS");
            Locales.IfLessThan2();
            string ClipStr = "";
            // Backup & Restore feature, now only text supported...
            if (Program.SwitcherActive() && Program.switcher.ActiveControl is TextBox)
                return (Program.switcher.ActiveControl as TextBox).SelectedText;
            //Logging.Log("Taking backup of clipboard text if possible.");
            if (SwitcherUI.ClipBackOnlyText)
            {
                lastClipText = NativeClipboard.GetText();
            }
            else
            {
                lastClip = NativeClipboard.clip_get();
            }

            //			Thread.Sleep(50);
            //			if (!String.IsNullOrEmpty(lastClipText))
            //				lastClipText = Clipboard.GetText();
            //			This prevents from converting text that already exist in Clipboard
            //			by pressing "Convert Selection hotkey" without selected text.
            NativeClipboard.Clear();
            Logging.Log("Getting selected text.");
            if (SwitcherUI.SelectedTextGetMoreTries)
                for (int i = 0; i != Program.switcher.SelectedTextGetMoreTriesCount; i++)
                {
                    if (WaitForClip2BeFree())
                    {
                        ClipStr = MakeCopy();
                        if (!String.IsNullOrEmpty(ClipStr))
                            break;
                    }
                }
            else
            {
                if (WaitForClip2BeFree())
                {
                    ClipStr = MakeCopy();
                    if (String.IsNullOrEmpty(ClipStr))
                        ClipStr = MakeCopy();
                }
            }
            if (String.IsNullOrEmpty(ClipStr))
                return "";
            return Regex.Replace(ClipStr, "\r?\n|\r", "\n");
        }
        /// <summary>
        /// Re-presses modifiers you hold when hotkey fired(due to SendModsUp()).
        /// </summary>
        public static void RePress()
        {
            DoSelf(() =>
            {
                //Repress's modifiers by RePress variables
                if (shiftRP)
                {
                    skip_kbd_events++;
                    KeybdEvent(Keys.LShiftKey, 0);
                    swas = true;
                    shiftRP = false;
                }
                if (altRP)
                {
                    skip_kbd_events++;
                    KeybdEvent(Keys.LMenu, 0);
                    awas = true;
                    altRP = false;
                }
                if (ctrlRP)
                {
                    skip_kbd_events++;
                    KeybdEvent(Keys.LControlKey, 0);
                    cwas = true;
                    ctrlRP = false;
                }
                if (winRP)
                {
                    skip_kbd_events++;
                    KeybdEvent(Keys.LWin, 0);
                    wwas = true;
                    winRP = false;
                }
            }, "repress");
        }
        /// <summary>
        /// Do action without RawInput listeners(e.g. not catch).
        /// Useful with SendInput or keybd_event functions.
        /// </summary>
        /// <param name="self_action">Action that will be done without RawInput listeners, Hotkeys and low-level hook.</param>
        public static void DoSelf(Action self_action, string caller = "unknown")
        {
            var pt = ">> DoSelf() ";
            if (self_action == null) { Logging.Log(pt + "null() action: from +" + caller); return; }
            var mn = "?()";
            if (self_action.Method != null)
                mn = self_action.Method.Name;
            mn += "+" + caller;
            if (selfie)
            {
                Logging.Log(pt + "Inside " + busy_on + " called: " + mn);
                self_action();
            }
            else
            {
                Debug.WriteLine(pt + mn);
                //				Program.switcher.Invoke((MethodInvoker)delegate {
                if (LLHook._ACTIVE) { LLHook.UnSet(); }
                if (Program.switcher != null) { Program.switcher.UnregisterHotkeys(); }
                //			});
                if (Program.rif != null)
                    Program.rif.RegisterRawInputDevices(IntPtr.Zero, WinAPI.RawInputDeviceFlags.Remove);
                selfie = true;
                busy_on = mn;
                self_action();
                //				Program.switcher.Invoke((MethodInvoker)delegate {
                if (LLHook._ACTIVE) { LLHook.Set(); }
                if (Program.switcher != null) { Program.switcher.RegisterHotkeys(); }
                //				                   });
                if (Program.rif != null)
                    Program.rif.RegisterRawInputDevices(Program.rif.Handle);
                selfie = false;
                Debug.WriteLine(pt + "end " + mn);
            }
        }
        public static void StartConvertWord(YuKey[] YuKeys, uint wasLocale, bool skipsnip = false, bool last = false)
        {
            if (YuKeys.Length == 0)
            {
                Logging.Log("Convert Last failed: EMPTY WORD.");
                return;
            }
            Logging.Log("Start Convert Word len: [" + YuKeys.Length + "], wl:" + wasLocale + ", ss:" + skipsnip);
            DoSelf(() =>
            {
                Debug.WriteLine(">> ST CLW");
                //ASKME:comments -> get count backs from inputed string
                var backs = YuKeys.Length;
                // Fix for cmd exe pause hotkey leaving one char.
                //if (!skipsnip) { // E.g. not from AutoSwitch
                var clsNM = Locales.ActiveWindowClassName(40, WinAPI.GetForegroundWindow());
                if (IfNW7() &&
                    clsNM == "ConsoleWindowClass" && (
                    Program.switcher.HKCLast.VirtualKeyCode == (int)Keys.Pause)
                       && SwitcherUI.cmdbackfix)
                    backs++;
                
                if (clsNM.StartsWith("Qt5"))
                { // Qt5 keyboard message handling seems slow, so wait for it before starting 
                    Thread.Sleep(250);
                }
                
                Debug.WriteLine(">> LC Aft. " + (Program.locales.Length * 20));
                var rewr = new StringBuilder();
                if (!skipsnip)
                {
                    foreach (var c in c_snip) { rewr.Append(c); }
                    c_snip.Clear();
                }
                Logging.Log("Deleting old word, with lenght of [" + YuKeys.Length + "].");
                //ASKME:comments -> Make backspace input
                KInputs.MakeInput(KInputs.AddPress(Keys.Back, backs));
                if (SwitcherUI.UseDelayAfterBackspaces)
                    Thread.Sleep(Program.switcher.DelayAfterBackspaces);
                //ASKME:comments -> create new input array
                var q = new List<WinAPI.INPUT>();
                for (int i = 0; i < YuKeys.Length; i++)
                {
                    if (YuKeys[i].altnum)
                    {
                        Logging.Log("An YuKey with [" + YuKeys[i].numpads.Count + "] numpad(s) passed.");
                        q.Add(KInputs.AddKey(Keys.LMenu, true));
                        foreach (var numpad in YuKeys[i].numpads)
                        {
                            Logging.Log(numpad + " is being inputted.");
                            q.AddRange(KInputs.AddPress(numpad));
                        }
                        q.Add(KInputs.AddKey(Keys.LMenu, false));
                    }
                    else
                    {
                        //ASKME:comments -> add key
                        var k = YuKeys[i].key; var u = YuKeys[i].upper;
                        Logging.Log("An YuKey with state passed, key = {" + k + "}, upper = [" + u + "].");
                        var upp = u && !Control.IsKeyLocked(Keys.CapsLock);
                        if (upp)
                            q.Add(KInputs.AddKey(Keys.LShiftKey, true));
                        if (!SymbolIgnoreRules(k, u, wasLocale, ref q))
                            q.AddRange(KInputs.AddPress(k));
                        //ASKME:comments -> add press shift if word is Upper
                        if (upp)
                            q.Add(KInputs.AddKey(Keys.LShiftKey, false));
                        if (!skipsnip)
                        {
                            var loc = (Locales.GetCurrentLocale());
                            if (SwitcherUI.UseJKL && !KMHook.JKLERR)
                                loc = SwitcherUI.currentLayout;
                            var c = ToUnicodeExMulti((uint)k, (IntPtr)((int)loc), u);
                            if (c != '\0')
                            {
                                c_snip.Add(c);
                            }
                            else
                            {
                                Logging.Log("Snip rewrite failed: k:" + k + ", loc:" + loc);
                            }
                        }
                    }
                }
                if (!skipsnip)
                {
                    Logging.Log("[SNI] Snip rewrite: " + rewr + " => " + new string(c_snip.ToArray()));
                }
                //ASKME:comments -> Make correct word input
                KInputs.MakeInput(q.ToArray());
                SwitcherUI.hk_result = true;
                if (YuKeys.Length > 0 && last)
                {
                    if (afterEOS && YuKeys[YuKeys.Length - 1].key == Keys.Space)
                    {
                        CLW_W_SPACE = true;
                    }
                    if (afterEOL && YuKeys[YuKeys.Length - 1].key == Keys.Enter)
                    {
                        CLW_W_ENTER = true;
                    }
                }
                Debug.WriteLine("XX CLW_END");
            }, "st_conv_word");
        }
        public static void LayoutKeyReplaceInit()
        {
            var p = System.IO.Path.Combine(SwitcherUI.nPath, "LayoutKeyCodeReplace.txt");
            if (System.IO.File.Exists(p))
            {
                var t = System.IO.File.ReadAllText(p);
                var ll = Regex.Split(t, @"\r?\n");
                LKDict = new DICT<int, DICT<int, int>>();
                var tmp = new DICT<int, int>();
                var thisid = 0;
                foreach (string l in ll)
                {
                    if (String.IsNullOrEmpty(l)) continue;
                    if (thisid != 0)
                    {
                        var kcs = new List<string>();
                        if (l.Contains(','))
                        {
                            kcs.AddRange(l.Split(','));
                        }
                        else { kcs.Add(l); }
                        for (int i = 0; i < kcs.Count; i++)
                        {
                            if (!kcs[i].Contains('=')) continue;
                            var eqs = kcs[i].Split('=');
                            if ((eqs.Length > 2) && (eqs.Length % 2 == 0))
                            {
                                for (int x = 0; x < eqs.Length; x += 2)
                                {
                                    int x1 = 0, x2 = 0;
                                    if (eqs[x][0] == '[' && eqs[x][eqs[x].Length] == ']')
                                    {
                                        if (eqs[x + 1][0] == '[' && eqs[x + 1][eqs[x + 1].Length] == ']')
                                        {
                                            Int32.TryParse(eqs[x].Substring(1, eqs[x].Length - 2), out x1);
                                            Int32.TryParse(eqs[x + 1].Substring(1, eqs[x + 1].Length - 2), out x2);
                                            if (x1 != 0 && x2 != 0)
                                            {
                                                tmp.Add(x1, x2);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int x1 = 0, x2 = 0;
                                string xx1 = eqs[0].Substring(1, eqs[0].Length - 2),
                                        xx2 = eqs[1].Substring(1, eqs[1].Length - 2);
                                Int32.TryParse(xx1, out x1);
                                Int32.TryParse(xx2, out x2);
                                if (x1 != 0 && x2 != 0)
                                {
                                    tmp.Add(x1, x2);
                                }
                            }
                        }
                    }
                    if (l[0] == '<')
                    {
                        var id = l.Substring(1, l.Length - 1);
                        var tid = -1;
                        Int32.TryParse(id, out tid);
                        if (thisid == tid)
                        {
                            LKDict.Set(thisid, tmp);
                            thisid = 0;
                            tmp = new DICT<int, int>();
                        }
                    }
                    if (l[0] == '>')
                    {
                        var id = l.Substring(1, l.Length - 1);
                        Debug.WriteLine("id" + id);
                        Int32.TryParse(id, out thisid);
                    }
                }
            }
        }
        public static List<YuKey> LayoutKeyReplace(List<YuKey> yks, int layout, int next)
        {
            Debug.WriteLine("LK start!");
            if (LKDict == null) return yks;
            bool reverse = false;
            int st = 0;
            DICT<int, int> l = null;
            for (int i = 0; i < LKDict.len; i++)
            {
                if (LKDict[i].k == next)
                {
                    l = LKDict.GetByKey(next);
                    reverse = true;
                    st = l.len - 1;
                    Debug.WriteLine("REVERSE to layout " + next);
                    break;
                }
                if (LKDict[i].k == layout)
                {
                    l = LKDict.GetByKey(layout);
                    break;
                }
            }
            Debug.WriteLine("LK? Dict OK" + l == null);
            if (l == null) return yks;
            Debug.WriteLine("LK Dict OK" + l.len);
            for (int z = 0; z != yks.Count; z++)
            {
                if (yks[z].altnum) continue;
                for (int i = st; ;)
                {
                    if (reverse ? (i < 0) : (i == l.len)) break;
                    int kk = l[i].k, vv = l[i].v;
                    if (reverse)
                    {
                        kk = l[i].v; vv = l[i].k;
                    }
                    Debug.WriteLine(">>TEST " + ((int)yks[z].key) + "==" + kk + " => " + vv);
                    if ((int)yks[z].key == kk)
                    {
                        Keys kn = yks[z].key;
                        Debug.WriteLine("GOT: " + vv);
                        try
                        {
                            kn = (Keys)vv;
                            yks[z] = new YuKey() { key = kn, altnum = yks[z].altnum, numpads = yks[z].numpads, upper = yks[z].upper };
                            break;
                        }
                        catch
                        {
                            Logging.Log(vv + " is not a valid key code.");
                        }
                    }
                    if (reverse)
                    {
                        i--;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return yks;
        }
        public static bool JKL_Restart_1only = false;
        /// <summary>
        /// Converts last word/line/words.
        /// </summary>
        /// <param name="c_">List of YuKeys to be converted.</param>
        public static void ConvertLast(List<YuKey> c_, bool line = false)
        {
            try
            { //Used to catch errors, since it called as Task
                Debug.WriteLine("Start CL");
                Debug.WriteLine(c_.Count + " LL");
                Logging.Log("[CLAST] > Starting to convert word, count:" + c_.Count + ", LW: " + Program.c_word.Count + " Last CR:" + lastLWClearReason);
                if (c_.Count <= 0)
                    return;
                Locales.IfLessThan2();
                if (SwitcherUI.SoundOnConvLast)
                    SwitcherUI.SoundPlay();
                if (SwitcherUI.SoundOnConvLast2)
                    SwitcherUI.SoundPlay(true);
                var wasLocale = Locales.GetCurrentLocale();
                if (SwitcherUI.UseJKL && !KMHook.JKLERR)
                    wasLocale = SwitcherUI.currentLayout;
                var desl = GetNextLayout(wasLocale).uId;
                YuKey[] YuKeys = line ? c_.ToArray() : LayoutKeyReplace(c_, (int)(wasLocale & 0xffff), (int)(desl & 0xffff)).ToArray();
                if (SwitcherUI.UseJKL && SwitcherUI.EmulateLS && !JKLERR)
                {
                    Debug.WriteLine("JKL-ed CLW");
                    Logging.Log("[CLAST] > On JKL layout: " + desl);
                    if (!JKLERRchecking)
                    {
                        Debug.WriteLine("JKL-ed CLW JKLERRNCH");
                        jklXHidServ.actionOnLayoutExecuted = false;
                        jklXHidServ.ActionOnLayout = () => StartConvertWord(YuKeys, wasLocale, false, true);
                        jklXHidServ.OnLayoutAction = desl;
                        ChangeLayout(true);
                        JKLERRchecking = true;
                        var t = 0;
                        JKLERRT.Interval = 50;
                        JKLERRT.Elapsed += (_, __) =>
                        {
                            if (!jklXHidServ.actionOnLayoutExecuted)
                            {
                                Logging.Log("JKL convert word failed, JKL didn't monitor the layout or didn't send it, fallback to default...", 1);
                                Logging.Log("JKL seems BAD.");
                                JKLERR = true;
                                JKLERRchecking = false;
                                JKLERRT.Stop();
                                JKLERRT.Dispose();
                                JKLERRT = new System.Timers.Timer();
                                if (!JKL_Restart_1only)
                                {
                                    Logging.Log("One-time per error JKL restart.", 2);
                                    JKL_Restart_1only = true;
                                    jklXHidServ.Destroy();
                                    Thread.Sleep(10);
                                    jklXHidServ.Init();
                                    Thread.Sleep(100);
                                    ConvertLast(c_, line);
                                }
                                else
                                {
                                    Logging.Log("JKL restart didn't help...", 1);
                                    StartConvertWord(YuKeys, wasLocale, false, true);
                                    JKL_Restart_1only = false;
                                }
                            }
                            else
                            {
                                Logging.Log("JKL seems OK.");
                                Debug.WriteLine("JKL seems OK...");
                                JKLERRchecking = false;
                                JKLERRT.Stop();
                                JKLERRT.Dispose();
                                JKLERRT = new System.Timers.Timer();
                            }
                            Debug.WriteLine("JKL CHECK...");
                            if (t > 50)
                                JKLERRchecking = false;
                            t++;
                        };
                        JKLERRT.Start();
                    }
                }
                else
                {
                    ChangeLayout(true);
                    StartConvertWord(YuKeys, wasLocale, false, true);
                }
            }
            catch (Exception e)
            {
                Logging.Log("Convert Last encountered error, details:\r\n" + e.Message + "\r\n" + e.StackTrace, 1);
            }
            Debug.WriteLine("===============> Fin");
            Program.switcher.UpdateLDs();
            Memory.Flush();
        }
        static bool SymbolIgnoreRules(char c)
        {
            var ign = "<,>./?'\";:[]{}\\|";
            return ign.Contains(c);
        }
        /// <summary>
        /// Rules to ignore symbols in ConvertLast() function.
        /// </summary>
        /// <param name="key">Key to be checked.</param>
        /// <param name="upper">State of key to be checked.</param>
        /// <param name="wasLocale">Last layout id.</param>
        /// <returns></returns>
        static bool SymbolIgnoreRules(Keys key, bool upper, uint wasLocale, ref List<WinAPI.INPUT> q)
        {
            Logging.Log("Passing Key = [" + key + "]+[" + (upper ? "UPPER" : "lower") + "] with WasLayoutID = [" + wasLocale + "] through symbol ignore rules.");
            wasLocale = wasLocale >> 16;
            if (Program.switcher.HKSymIgn.Enabled &&
                SwitcherUI.SymIgnEnabled &&
                (wasLocale == 1033 || wasLocale == 1041) &&
                ((Locales.AllList().Length < 3 && !SwitcherUI.SwitchBetweenLayouts) ||
                SwitcherUI.SwitchBetweenLayouts) && (
                    key == Keys.Oem5 ||
                    key == Keys.OemOpenBrackets ||
                    key == Keys.Oem6 ||
                    key == Keys.Oem1 ||
                    key == Keys.Oem7 ||
                    key == Keys.Oemcomma ||
                    key == Keys.OemPeriod ||
                    key == Keys.OemQuestion))
            {
                if (upper && key == Keys.OemOpenBrackets)
                    q.AddRange(KInputs.AddString("{"));
                if (!upper && key == Keys.OemOpenBrackets)
                    q.AddRange(KInputs.AddString("["));

                if (upper && key == Keys.Oem5)
                    q.AddRange(KInputs.AddString("|"));
                if (!upper && key == Keys.Oem5)
                    q.AddRange(KInputs.AddString("\\"));

                if (upper && key == Keys.Oem6)
                    q.AddRange(KInputs.AddString("}"));
                if (!upper && key == Keys.Oem6)
                    q.AddRange(KInputs.AddString("]"));

                if (upper && key == Keys.Oem1)
                    q.AddRange(KInputs.AddString(":"));
                if (!upper && key == Keys.Oem1)
                    q.AddRange(KInputs.AddString(";"));

                if (upper && key == Keys.Oem7)
                    q.AddRange(KInputs.AddString("\""));
                if (!upper && key == Keys.Oem7)
                    q.AddRange(KInputs.AddString("'"));

                if (upper && key == Keys.Oemcomma)
                    q.AddRange(KInputs.AddString("<"));
                if (!upper && key == Keys.Oemcomma)
                    q.AddRange(KInputs.AddString(","));

                if (upper && key == Keys.OemPeriod)
                    q.AddRange(KInputs.AddString(">"));
                if (!upper && key == Keys.OemPeriod)
                    q.AddRange(KInputs.AddString("."));

                if (upper && key == Keys.OemQuestion)
                    q.AddRange(KInputs.AddString("?"));
                if (!upper && key == Keys.OemQuestion)
                    q.AddRange(KInputs.AddString("/"));
                Memory.Flush();
                return true;
            }
            else
                return false;
        }
        public static bool IsConhost()
        {
            return Locales.ActiveWindowClassName(100).Contains("ConsoleWindowClass");
        }
        /// <summary>
        /// Changes current layout.
        /// </summary>
        public static uint ChangeLayout(bool quiet = false)
        {
            uint desired = 0;
            as_lword_layout = 0;
            Debug.WriteLine(">> LC + SELF");
            DoSelf(() =>
            {
                if (!quiet)
                {
                    if (SwitcherUI.SoundOnLayoutSwitch)
                        SwitcherUI.SoundPlay();
                    if (SwitcherUI.SoundOnLayoutSwitch2)
                        SwitcherUI.SoundPlay(true);
                }
                if (Locales.ActiveWindowProcess().ProcessName.ToLower() == "HD-Frontend".ToLower())
                {
                    KInputs.MakeInput(KInputs.AddPress(Keys.Space), (int)WinAPI.MOD_CONTROL);
                    Thread.Sleep(13);
                }
                else
                {
                    if (SwitcherUI.SwitchBetweenLayouts)
                    {
                        uint last = 0;
                        bool conhost = false;
                        if (SwitcherUI.UseJKL && !KMHook.JKLERR)
                        {
                            conhost = IsConhost();
                        }
                        for (int i = Program.locales.Length; i != 0; i--)
                        {
                            var nowLocale = Locales.GetCurrentLocale();
                            if (SwitcherUI.UseJKL)
                            {
                                if (nowLocale == 0 || conhost)
                                    nowLocale = SwitcherUI.currentLayout;
                                if (last == nowLocale && nowLocale != 0)
                                {
                                    nowLocale = SwitcherUI.currentLayout;
                                    desired = 0;
                                }
                            }
                            if (CompareLayouts(nowLocale, desired))
                                break;
                            uint notnowLocale = CompareLayouts(nowLocale, SwitcherUI.MAIN_LAYOUT1)
                                ? SwitcherUI.MAIN_LAYOUT2
                                : SwitcherUI.MAIN_LAYOUT1;
                            last = nowLocale;
                            if (!CompareLayouts(nowLocale, SwitcherUI.MAIN_LAYOUT1) &&
                                !CompareLayouts(nowLocale, SwitcherUI.MAIN_LAYOUT2) &&
                                !CompareLayouts(notnowLocale, last_switch_layout))
                            {
                                Debug.WriteLine("Not 2 layouts! " + nowLocale + " last:" + last_switch_layout + " not:" + notnowLocale);
                                Logging.Log("> [ChangeLaouyt] Change layout, wanted: " + notnowLocale + " changed mind to " + last_switch_layout);
                                if (last_switch_layout != 0)
                                    notnowLocale = last_switch_layout;
                            }
                            ChangeToLayout(Locales.ActiveWindow(), notnowLocale, conhost);
                            desired = notnowLocale;
                            if (SwitcherUI.EmulateLS)
                                break;
                        }
                    }
                    else
                    {
                        if (SwitcherUI.EmulateLS)
                        {
                            CycleEmulateLayoutSwitch();
                        }
                        else
                        {
                            CycleLayoutSwitch();
                        }
                    }
                }
            }, "change_layout");
            return desired;
        }
        public static void evt_layoutchanged(uint to, uint fr, string eve, int t = 0)
        {
            Logging.Log("[EVT] Check event bindings" + SwitcherUI.event_bindings.len);
            if (SwitcherUI.event_bindings.len != 0)
            {
                for (var i = 0; i != SwitcherUI.event_bindings.len; i++)
                {
                    var evt = SwitcherUI.event_bindings[i];
                    //					for (var j = 0; j!= SwitcherUI.bindable_events.Length; j++) {
                    //						var l = SwitcherUI.bindable_events[j];
                    //						if (!l.StartsWith(lc)) { continue; }
                    var xxl = eve == SwitcherUI.bindable_events[0] ? to : fr;
                    if (Regex.Replace(evt.k, "\\d+", "") == eve)
                    {
                        var sus = Regex.Replace(evt.k, "^[a-z]+", "");
                        Debug.WriteLine("SUS: " + sus + "evk.k " + evt.k + " " + eve);
                        var kt = UInt32.Parse(sus);
                        Logging.Log("[EVT] Starting event #" + i + " on " + evt.k + " | (" + (xxl >> 16) + " == " + (kt >> 16) + ") => " + evt.v.Method.Name);
                        if (kt == xxl || (kt >> 16) == (xxl >> 16))
                        {
                            if (t > 0) { DoLater(evt.v, t); }
                            else { evt.v(); }
                        }
                    }
                    //					}
                }
            }
        }
        /// <summary>
        /// Compares layouts, if any of them is short, e.g. 1041, comparison will be done only on short part.
        /// </summary>
        /// <param name="L1">The first layout which will be compared to the second</param>
        /// <param name="L2">The second layout</param>
        /// <returns></returns>
        public static bool CompareLayouts(uint L1, uint L2)
        {
            return (L1 == L2) || (
                (L1 & 0xffff) == (L2 & 0xffff) &&
                (
                    (L1 >> 16) == 0 ? true : (L2 >> 16) == (L1 >> 16) ||
                    (L2 >> 16) == 0 ? true : (L1 >> 16) == (L2 >> 16)
                )
            );
        }
        /// <summary>
        /// Calls functions to change layout based on EmulateLS variable.
        /// </summary>
        /// <param name="hwnd">Target window to change its layout.</param>
        /// <param name="LayoutId">Desired layout to switch to.</param>
        public static void ChangeToLayout(IntPtr hwnd, uint LayoutId, bool conhost = false)
        {
            Debug.WriteLine(">> CTL");
            if (SwitcherUI.EmulateLS)
                EmulateChangeToLayout(LayoutId, conhost);
            else
                NormalChangeToLayout(hwnd, LayoutId, conhost);
        }
        /// <summary>
        /// Changing layout to LayoutId in hwnd with PostMessage and WM_INPUTLANGCHANGEREQUEST.
        /// </summary>
        /// <param name="hwnd">Target window to change its layout.</param>
        /// <param name="LayoutId">Desired layout to switch to.</param>
        static void NormalChangeToLayout(IntPtr hwnd, uint LayoutId, bool conhost = false)
        {
            Debug.WriteLine(">> N-CTL");
            Logging.Log("Changing layout using normal mode, WinAPI.SendMessage [WinAPI.WM_INPUTLANGCHANGEREQUEST] with LParam [" + LayoutId + "].");
            int tries = 0;
            uint last = 0;
            var loc = SwitcherUI.currentLayout;
            if (!SwitcherUI.UseJKL || JKLERR)
            {
                loc = Locales.GetCurrentLocale();
            }
            //Cycles while layout not changed
            evt_layoutchanged(0, loc, SwitcherUI.bindable_events[1]);
            do
            {
                if (SwitcherUI.UseJKL && !KMHook.JKLERR)
                {
                    if (loc != 0 && (CompareLayouts(loc, last)) || conhost)
                        loc = SwitcherUI.currentLayout;
                }
                else
                {
                    loc = Locales.GetCurrentLocale();
                }
                if (LayoutId == 0)
                {
                    Logging.Log("Layout change skipped, 0 is not layout.", 1);
                }
                else
                    WinAPI.PostMessage(hwnd, (int)WinAPI.WM_INPUTLANGCHANGEREQUEST, 0, LayoutId);
                Thread.Sleep(10);//Give some time to switch layout
                tries++;
                if (tries >= Program.locales.Length * 2)
                {
                    Logging.Log("Tries break, probably failed layout changing...", 1);
                    break;
                }
                last = loc;
            } while (!CompareLayouts(loc, LayoutId));
            evt_layoutchanged(LayoutId, 0, SwitcherUI.bindable_events[0]);
            if (SwitcherUI.MAIN_LAYOUT1 == loc || SwitcherUI.MAIN_LAYOUT2 == loc)
            {
                last_switch_layout = loc;
            }
            //			if (!SwitcherUI.UseJKL) // Wow, gives no sense!!
            SwitcherUI.currentLayout = SwitcherUI.GlobalLayout = LayoutId;
        }
        static bool failed = true;
        /// <summary>
        /// Changing layout to LayoutId by emulating windows layout switch hotkey. 
        /// </summary>
        /// <param name="LayoutId">Desired layout to switch to.</param>
        static void EmulateChangeToLayout(uint LayoutId, bool conhost = false)
        {
            Debug.WriteLine(">> E-CTL");
            var last = SwitcherUI.currentLayout;
            //			var lash = last;
            if (CompareLayouts(last, LayoutId))
            {
                if (!conhost && last == Locales.GetCurrentLocale())
                {
                    Debug.WriteLine("Layout already " + LayoutId);
                    return;
                }
                Debug.WriteLine("False, layout isn't actually #" + last);
            }
            Logging.Log("Changing to specific layout [" + LayoutId + "] by emulating layout switch.");
            for (int i = Program.locales.Length; i != 0; i--)
            {
                uint loc = Locales.GetCurrentLocale();
                //				uint locsh = loc;
                //				Debug.WriteLine(loc + " " + last);
                if (SwitcherUI.UseJKL && !KMHook.JKLERR && ((loc == 0 || CompareLayouts(loc, last) /*|| locsh == lash*/) || conhost))
                {
                    jklXHidServ.start_cyclEmuSwitch = true;
                    jklXHidServ.cycleEmuDesiredLayout = LayoutId;
                    Debug.WriteLine("LI: " + LayoutId);
                    KMHook.evt_layoutchanged(0, loc, SwitcherUI.bindable_events[1]);
                    CycleEmulateLayoutSwitch();
                    break;
                }
                else
                {
                    //					Debug.WriteLine(i+".LayoutID: " + LayoutId + ", loc: " +loc);
                    if (CompareLayouts(loc, LayoutId) /*|| locsh == LayoutId*/)
                    {
                        failed = false;
                        break;
                    }
                    CycleEmulateLayoutSwitch();
                    Thread.Sleep(10);
                }
                last = loc;
                //				lash = locsh;
                if (!failed)
                    break;
            }
            if (!SwitcherUI.UseJKL || KMHook.JKLERR)
            {
                if (!failed)
                {
                    if (CompareLayouts(SwitcherUI.MAIN_LAYOUT1, LayoutId) || CompareLayouts(SwitcherUI.MAIN_LAYOUT2, LayoutId))
                    {
                        last_switch_layout = LayoutId;
                    }
                    SwitcherUI.currentLayout = SwitcherUI.GlobalLayout = LayoutId;
                }
                else
                {
                    Logging.Log("Changing to layout [" + LayoutId + "] using emulation failed after # of layouts tries,\r\nmaybe you have more that 16 layouts, disabled change layout hotkey in windows, or working in console window(use getconkbl.dll)?", 1);
                }
            }
            failed = true;
        }
        /// <summary>
        /// Changing layout by emulating windows layout switch hotkey
        /// </summary>
        public static void CycleEmulateLayoutSwitch()
        {
            Debug.WriteLine(">> CELS");
            DoSelf(() =>
            {
                if (SwitcherUI.EmulateLSType == "Alt+Shift")
                {
                    Logging.Log("Changing layout using cycle mode by simulating key press [Alt+Shift].");
                    //Emulate Alt+Shift
                    KInputs.MakeInput(KInputs.AddPress(Keys.LShiftKey), (int)WinAPI.MOD_ALT);
                }
                else if (SwitcherUI.EmulateLSType == "Ctrl+Shift")
                {
                    Logging.Log("Changing layout using cycle mode by simulating key press [Ctrl+Shift].");
                    //Emulate Ctrl+Shift
                    KInputs.MakeInput(KInputs.AddPress(Keys.LShiftKey), (int)WinAPI.MOD_CONTROL);
                }
                else
                {
                    Logging.Log("Changing layout using cycle mode by simulating key press [Win+Space].");
                    //Emulate Win+Space
                    KInputs.MakeInput(KInputs.AddPress(Keys.Space), (int)WinAPI.MOD_WIN);
                    Thread.Sleep(50); //Important!
                }
                if (!SwitcherUI.UseJKL || KMHook.JKLERR)
                    DoLater(() => { SwitcherUI.currentLayout = SwitcherUI.GlobalLayout = Locales.GetCurrentLocale(); }, 25);
            }, "cycle emulate layout switch");
        }
        public static Locales.Locale GetNextLayout(uint before = 0)
        {
            Debug.WriteLine(">> GNL");
            var loc = new Locales.Locale();
            uint last = 0;
            var cur = before;
            if (SwitcherUI.UseJKL && !KMHook.JKLERR)
            {
                if (cur == 0 || cur == last)
                    cur = SwitcherUI.currentLayout;
            }
            else if (cur == 0)
                cur = Locales.GetCurrentLocale();
            Debug.WriteLine("Current: " + cur);
            for (int i = 0; i != Program.locales.Length; i++)
            {
                if (last != 0 && !CompareLayouts(cur, last))
                    break;
                if (SwitcherUI.SwitchBetweenLayouts)
                {
                    if (CompareLayouts(cur, SwitcherUI.MAIN_LAYOUT1))
                        loc.uId = SwitcherUI.MAIN_LAYOUT2;
                    else if (CompareLayouts(cur, SwitcherUI.MAIN_LAYOUT2))
                    {
                        loc.uId = SwitcherUI.MAIN_LAYOUT1;
                    }
                    else
                        loc.uId = SwitcherUI.MAIN_LAYOUT1;
                    break;
                }
                //				Thread.Sleep(15);
                var curind = Program.locales.ToList().FindIndex(lid => lid.uId == cur);
                if (curind == Program.locales.Length - 1)
                {
                    loc = Program.locales[0];
                }
                else
                {
                    loc = Program.locales[curind + 1];
                    if (loc.Lang.Contains("Microsoft Office IME")) // fake layout
                        if (curind + 2 < Program.locales.Length)
                            loc = Program.locales[curind + 2];
                        else
                            loc = Program.locales[0];
                    //					for (int g=curind+1; g != Program.locales.Length; g++) {
                    //						var l = Program.locales[g];
                    //						Debug.WriteLine("Checking: " + l.Lang + ", with "+cur);
                    //						Logging.Log("LIDC = "+g +" curid = "+curind + " Lidle = " +(Program.locales.Length - 1));
                    ////						if (g >= curind)
                    //							if (l.uId != cur) {
                    //								Logging.Log("Locales +1 Next BREAK on " + l.uId);
                    //								loc = l;
                    //	//							if (last !=0) // ensure its checked at least twice
                    //									br = true;
                    //								break;
                    //						}
                    //					}
                }
                last = cur;
            }
            Logging.Log("[GNL] > Get Next layout return: " + loc.uId + ", layout before: " + cur);
            return loc;
        }
        /// <summary>
        /// Changing layout to next with PostMessage and WM_INPUTLANGCHANGEREQUEST and LParam HKL_NEXT.
        /// </summary>
        public static void CycleLayoutSwitch()
        {
            Debug.WriteLine(">> CLS");
            Logging.Log("Changing layout using cycle mode by sending Message [WinAPI.WM_INPUTLANGCHANGEREQUEST] with LParam [HKL_NEXT] using WinAPI.PostMessage to ActiveWindow");
            //Use WinAPI.PostMessage to switch to next layout
            ChangeToLayout(Locales.ActiveWindow(), GetNextLayout().uId);
        }
        static char ToUnicodeExMulti(uint vk, IntPtr layout, bool upper = false)
        {
            var flags = new[] { 1 << 2, 2, 0 };
            var s = new StringBuilder(10);
            var byt = new byte[256];
            if (upper)
            {
                byt[(int)Keys.ShiftKey] = 0xFF;
            }
            var vsc = (uint)WinAPI.MapVirtualKey(vk, 0);
            int tr = 0;
            foreach (uint f in flags)
            {
                tr++;
                WinAPI.ToUnicodeEx(vk, vsc, byt, s, s.Capacity, f, layout);
                if (s.Length > 0)
                {
                    Logging.Log("[ToUniEx] Try:" + tr + " char: " + s + " at flag:" + f);
                    return s.ToString()[0];
                }
            }
            return '\0';
        }
        /// <summary>
        /// Converts character(c) from layout(uID1) to another layout(uID2) by using WinAPI.ToUnicodeEx().
        /// </summary>
        /// <param name="c">Character to be converted.</param>
        /// <param name="uID1">Layout id 1(from).</param>
        /// <param name="uID2">Layout id 2(to)</param>
        /// <returns></returns>
        static string InAnother(char c, uint uID1, uint uID2)
        { //Remakes c from uID1  to uID2
            var cc = c;
            var s = "";
            var chsc = WinAPI.VkKeyScanEx(cc, uID1);
            if (chsc == -1) return s;
            var state = (chsc >> 8) & 0xff;

            //			var byt = new byte[256];
            //			//it needs just 1 but,anyway let it be 10, i think that's better
            var y = ToUnicodeExMulti((uint)chsc, (IntPtr)((int)uID2), state == 1);
            if (y != '\0') s += y;
            //			//Checks if 'chsc' have upper state
            //			if (state == 1) {
            //				byt[(int)Keys.ShiftKey] = 0xFF;
            //			}
            //			//"Convert magic✩" is the string below
            //			var ant = WinAPI.ToUnicodeEx((uint)chsc, (uint)chsc, byt, s, s.Capacity, 1<<2, (IntPtr)uID2);
            return s;
        }
        /// <summary>
        /// Simplified WinAPI.keybd_event() with extended recognize feature.
        /// </summary>
        /// <param name="key">Key to be inputted.</param>
        /// <param name="flags">Flags(state) of key.</param>
        public static void KeybdEvent(Keys key, int flags)
        { // 
          //Do not remove this line, it needed for "Left Control Switch Layout" to work properly
          //			Thread.Sleep(15);
            WinAPI.keybd_event((byte)key, 0, flags | (KInputs.IsExtended(key) ? 1 : 0), 0);
        }
        public static void RePressAfter(int mods)
        {
            ctrlRP = Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_CONTROL);
            shiftRP = Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_SHIFT);
            altRP = Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_ALT);
            winRP = Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_WIN);
        }
        public static bool IsKDown(Keys k)
        {
            return (WinAPI.GetAsyncKeyState((int)k) & 0x8000) != 0;
        }
        public static void WaitKey2Breleased(Keys key)
        {
            bool k = true;
            while (k)
            {
                k = IsKDown(key);
                Thread.Sleep(15);
                Debug.WriteLine("k" + k);
            }
        }
        /// <summary>
        /// Sends modifiers up by modstoup array. 
        /// </summary>
        /// <param name="modstoup">Array of modifiers which will be send up. 0 = ctrl, 1 = shift, 2 = alt.</param>
        public static void SendModsUp(int modstoup, bool waitwin = true)
        { //
          //These three below are needed to release all modifiers, so even if you will still hold any of it
          //it will skip them and do as it must.
            if (modstoup <= 0) return;
            Debug.WriteLine(">> SMU: " + Hotkey.GetMods(modstoup));
            DoSelf(() =>
            {
                var modsUP = "";
                if (Hotkey.ContainsModifier(modstoup, (int)WinAPI.MOD_WIN))
                {
                    if (waitwin)
                    {
                        WaitKey2Breleased(Keys.LWin);
                        WaitKey2Breleased(Keys.RWin);
                        win = win_r = false;
                        LLHook.SetModifier(WinAPI.MOD_WIN, false);
                        LLHook.SetModifier(WinAPI.MOD_WIN, false, false);
                        modsUP += "LWin,RWin,";
                    }
                    else
                    {
                        if (IsKDown(Keys.LWin))
                        {
                            KMHook.KeybdEvent(Keys.LWin, 2); // Left Win Up
                            win = false;
                            LLHook.SetModifier(WinAPI.MOD_WIN, false);
                            modsUP += "LWin,";
                        }
                        if (IsKDown(Keys.RWin))
                        {
                            KMHook.KeybdEvent(Keys.RWin, 2); // Right Win Up
                            win_r = false;
                            LLHook.SetModifier(WinAPI.MOD_WIN, false, false);
                            modsUP += "RWin,";
                        }
                    }
                }
                if (Hotkey.ContainsModifier(modstoup, (int)WinAPI.MOD_SHIFT))
                {
                    if (IsKDown(Keys.RShiftKey))
                    {
                        KMHook.KeybdEvent(Keys.RShiftKey, 2); // Right Shift Up
                        shift_r = false;
                        LLHook.SetModifier(WinAPI.MOD_SHIFT, false, false);
                        modsUP += "RShift,";
                    }
                    if (IsKDown(Keys.LShiftKey))
                    {
                        KMHook.KeybdEvent(Keys.LShiftKey, 2); // Left Shift Up
                        LLHook.SetModifier(WinAPI.MOD_SHIFT, false);
                        shift = false;
                        modsUP += "LShift,";
                    }
                }
                if (Hotkey.ContainsModifier(modstoup, (int)WinAPI.MOD_CONTROL))
                {
                    if (IsKDown(Keys.RControlKey))
                    {
                        KMHook.KeybdEvent(Keys.RControlKey, 2); // Right Control Up
                        ctrl_r = false;
                        LLHook.SetModifier(WinAPI.MOD_CONTROL, false);
                        modsUP += "RCtrl,";
                    }
                    if (IsKDown(Keys.LControlKey))
                    {
                        KMHook.KeybdEvent(Keys.LControlKey, 2); // Left Control Up
                        ctrl = false;
                        LLHook.SetModifier(WinAPI.MOD_CONTROL, false, false);
                        modsUP += "LCtrl,";
                    }
                }
                if (Hotkey.ContainsModifier(modstoup, (int)WinAPI.MOD_ALT))
                {
                    if (IsKDown(Keys.RMenu))
                    {
                        KMHook.KeybdEvent(Keys.RMenu, 2); // Right Alt Up
                        alt_r = false;
                        LLHook.SetModifier(WinAPI.MOD_ALT, false, false);
                        modsUP += "RAlt,";
                    }
                    if (IsKDown(Keys.LMenu))
                    {
                        KMHook.KeybdEvent(Keys.LMenu, 2); // Left Alt Up
                        alt = false;
                        LLHook.SetModifier(WinAPI.MOD_ALT, false);
                        modsUP += "LAlt,";
                    }
                }
                Logging.Log("Modifiers [" + ((modsUP.Length > 2) ? modsUP.Substring(0, modsUP.Length - 1) : "") + "] sent up.");
            }, "sendmodsup");
        }
        /// <summary>
        /// Checks if key is modifier, and calls SendModsUp() if it is.
        /// </summary>
        /// <param name="key">Key to be checked.</param>
        public static void IfKeyIsMod(Keys key)
        {
            uint mods = 0;
            switch (key)
            {
                case Keys.LControlKey:
                case Keys.RControlKey:
                    mods += WinAPI.MOD_CONTROL;
                    break;
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    mods += WinAPI.MOD_SHIFT;
                    break;
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.Alt:
                    mods += WinAPI.MOD_ALT;
                    break;
                case Keys.LWin:
                case Keys.RWin:
                    mods += WinAPI.MOD_WIN;
                    break;
            }
            if (mods > 0)
                SendModsUp((int)mods, false);
        }
        public static Tuple<string, uint> WordGuessLayout(string word, uint _target = 0, bool glfix = true)
        {
            uint layout = 0;
            string guess = "";
            uint target = 0;
            if (_target == 0)
            {
                if (SwitcherUI.SwitchBetweenLayouts)
                {
                    var cur = (SwitcherUI.UseJKL && !KMHook.JKLERR) ? SwitcherUI.currentLayout : Locales.GetCurrentLocale();
                    target = cur == SwitcherUI.MAIN_LAYOUT1 ? SwitcherUI.MAIN_LAYOUT2 : SwitcherUI.MAIN_LAYOUT1;
                }
                else
                    target = GetNextLayout().uId;
            }
            else target = _target;
            for (int i = 0; i != Program.locales.Length; i++)
            {
                if (Program.locales[i].Lang.Contains("Microsoft Office IME")) // fake layout
                    continue;
                var l = Program.locales[i].uId;
                if (SwitcherUI.SwitchBetweenLayouts)
                {
                    if (l == target || (l != SwitcherUI.MAIN_LAYOUT1 && l != SwitcherUI.MAIN_LAYOUT2))
                    {
                        continue;
                    }
                }
                var l2 = target;
                if (l == target) continue;
                int wordLMinuses = 0;
                int wordL2Minuses = 0;
                int minmin = 0;
                int thismin = 0;
                int wordLFMinIndex = -1;
                int wordL2FMinIndex = -1;
                uint lay = 0;
                var wordL = new StringBuilder();
                var wordL2 = new StringBuilder();
                var mux1 = new StringBuilder();
                var mux2 = new StringBuilder();
                var result = new StringBuilder();
                //				var nany = false;
                Debug.WriteLine("Testing " + word + " against: " + l + " and " + l2);
                for (int I = 0; I != word.Length; I++)
                {
                    var c = word[I];
                    if (Char.IsNumber(c))
                    {
                        wordL.Append(c);
                        wordL2.Append(c);
                        mux1.Append(c);
                        mux2.Append(c);
                        Debug.WriteLine("N-skip: " + c);
                        continue;
                    }
                    var sm = false;
                    if (c == 'ո' || c == 'Ո')
                    {
                        if (c == 'ո') sm = true;
                        if (word.Length > I + 1)
                        {
                            if (word[I + 1] == 'ւ')
                            {
                                var shrt = l2 >> 16;
                                var _shrt = l >> 16;
                                if (shrt == 1033 || shrt == 1041)
                                {
                                    wordL.Append(sm ? "u" : "U");
                                    I++; continue;
                                }
                                if (_shrt == 1033 || _shrt == 1041)
                                {
                                    wordL2.Append(sm ? "u" : "U");
                                    I++; continue;
                                }
                                if (shrt == 1049)
                                {
                                    wordL.Append(sm ? "г" : "Г");
                                    I++; continue;
                                }
                                if (_shrt == 1049)
                                {
                                    wordL2.Append(sm ? "г" : "Г");
                                    I++; continue;
                                }
                            }
                        }
                    }
                    if (glfix)
                    {
                        var T3 = GermanLayoutFix(c);
                        Debug.WriteLine("GEFIX: " + c + " T3" + T3);
                        if (T3 != "")
                        {
                            wordL.Append(T3);
                            wordL2.Append(T3);
                            continue;
                        }
                    }
                    if (SwitcherUI.SymIgnEnabled)
                    {
                        if (SymbolIgnoreRules(c))
                        {
                            wordL.Append(c);
                            wordL2.Append(c);
                            Debug.WriteLine("Symbol Ignored: " + c);
                            continue;
                        }
                    }
                    if (c == '\n')
                    {
                        wordL.Append("\n");
                        wordL2.Append("\n");
                        mux1.Append("\n");
                        mux2.Append("\n");
                        continue;
                    }
                    var T1 = InAnother(c, l, l2);
                    wordL.Append(T1);
                    mux1.Append(T1);
                    if (T1 == "") { wordLMinuses++; mux1.Append(c); if (wordLFMinIndex == -1) { wordLFMinIndex = I; } }
                    var T2 = InAnother(c, l2, l);
                    wordL2.Append(T2);
                    mux2.Append(T2);
                    if (T2 == "") { wordL2Minuses++; mux2.Append(c); if (wordL2FMinIndex == -1) { wordL2FMinIndex = I; } }
                    Debug.WriteLine("T1: " + T1 + ", T2: " + T2 + ", C: " + c);
                    if (T2 == "" && T1 == "")
                    {
                        //						nany = true;
                        Debug.WriteLine("Char [" + c + "] is not in any of two layouts [" + l + "], [" + l2 + "] just rewriting.");
                        wordL.Append(word[I]);
                        wordL2.Append(word[I]);
                    }
                }
                if (wordLMinuses > wordL2Minuses)
                {
                    thismin = wordL2Minuses;
                    lay = l2;
                    result = wordL2;
                    if (result.Length < word.Length)
                    {
                        Debug.WriteLine("Mult-layout word Muxed 2!");
                        result = mux2;
                    }
                }
                else
                {
                    thismin = wordLMinuses;
                    lay = l;
                    result = wordL;
                    if (result.Length < word.Length)
                    {
                        Debug.WriteLine("Mult-layout word Muxed 1!");
                        result = mux1;
                    }
                }
                Debug.WriteLine("End, " + lay + "|" + wordL + ", " + wordL2 + "|" + wordLMinuses + ", " + wordL2Minuses + " mux1: " + mux1 + ", mux2: " + mux2);
                if (wordLMinuses == wordL2Minuses)
                {
                    if (wordLMinuses == 0 && wordL2Minuses == 0)
                    {
                        lay = l;
                        result = wordL;
                    }
                    else
                    {
                        thismin = wordLMinuses;
                        lay = 0;
                        result.Clear().Append(word);
                        bool one = wordLFMinIndex > wordL2FMinIndex;
                        if (one)
                        {
                            if (mux1.Length == word.Length)
                            {
                                result = mux1;
                            }
                        }
                        else
                            if (mux2.Length == word.Length)
                        {
                            result = mux2;
                        }
                        Debug.WriteLine("LMin" + wordLFMinIndex + " & L2Min: " + wordL2FMinIndex + " mux" + (one ? "1" : "2") + " => " + result);
                    }
                }
                if (result.Length > guess.Length || (lay != 0 && thismin <= minmin))
                {
                    guess = result.ToString();
                    layout = lay;
                }
                if (thismin < minmin)
                    minmin = thismin;
                if (lay == target) break;
            }
            if (target == layout)
                guess = word;
            if (layout == target)
            {
                guess_tries++;
                Debug.WriteLine("WARNING! Guess Try [#" + guess_tries + "], target layout and word layout are same!, taking next layout as target!");
                if (guess_tries < Program.locales.Length + 1)
                {
                    target = GetNextLayout(target).uId;
                    Debug.WriteLine("Retry with: layout: " + layout + ", target: " + target);
                    return WordGuessLayout(word, target);
                }
                else
                {
                    guess_tries = 0;
                }
            }
            else
            {
                guess_tries = 0;
            }
            Debug.WriteLine("Word " + word + " layout is " + layout + " targeting: " + target + " guess: " + guess);
            return Tuple.Create(guess, layout);
        }
        public static Tuple<bool, int> SnippetsLineCommented(string snippets, int k)
        {
            if (k == 0 || (k - 1 >= 0 && snippets[k - 1].Equals('\n')))
            { // only at every new line
                var end = snippets.IndexOf('\n', k);
                if (end == -1)
                    end = snippets.Length;
                var l = end - k - 1;
                if (end == -1)
                    l = end - k;
                if (end == k)
                    l = 0;
                var line = snippets.Substring(k, l);
                if (line.Length > 0) // Ingore empty lines
                    if (line[0] == '#' || (line[0] == '/' && (line.Length > 1 && line[1] == '/')))
                    {
                        //						Logging.Log("Ignored commented line in snippets:[" + line + "].");
                        return new Tuple<bool, int>(true, line.Length - 1);
                    }
            }
            return new Tuple<bool, int>(false, 0);
        }
        public static void GetSnippetsData(string snippets, bool isSnip = true)
        {
            var leng = 0;
            if (isSnip)
                leng = SwitcherUI.SnippetsCount;
            else
                leng = SwitcherUI.AutoSwitchCount;
            string[] smalls = new string[leng + 1024];
            string[] bigs = new string[leng + 1024];
            if (String.IsNullOrEmpty(snippets)) return;
            snippets = snippets.Replace("\r", "");
            int ids = 0, idb = 0, add_alias = 0;
            for (int k = 0; k < snippets.Length - 6; k++)
            {
                var com = SnippetsLineCommented(snippets, k);
                if (com.Item1)
                {
                    k += com.Item2; // skip commented line, speedup!
                    continue;
                }
                if (snippets[k].Equals('-') && snippets[k + 1].Equals('>'))
                {
                    var len = -1;
                    var endl = snippets.IndexOf('\n', k + 2);
                    if (endl == -1)
                        endl = snippets.Length;
                    //					Debug.WriteLine((k+2) + " X " +endl);
                    string cool = snippets.Substring(k + 2, endl - (k + 2));
                    if (cool.Length > 4)
                        for (int i = 0; i != cool.Length - 5; i++)
                        {
                            if (cool[i].Equals('=') && cool[i + 1].Equals('=') && cool[i + 2].Equals('=') && cool[i + 3].Equals('=') && cool[i + 4].Equals('>'))
                            {
                                len = i;
                            }
                        }
                    else
                        len = cool.Length;
                    if (len == -1)
                        len = endl - (k + 2);
                    var sm = snippets.Substring(k + 2, len).Replace("\r", "");
                    if (sm.Contains("|") && !((sm.StartsWith(REGEXSNIP, StringComparison.InvariantCulture) ||
                                               sm.StartsWith("D*" + REGEXSNIP, StringComparison.InvariantCulture)) &&
                                              (sm.EndsWith("/", StringComparison.InvariantCulture) ||
                                               sm.EndsWith("/i", StringComparison.InvariantCulture))))
                    {
                        var esm = sm.Replace("||", pipe_esc);
                        foreach (var n in esm.Split('|'))
                        {
                            smalls[ids] = n.Replace(pipe_esc, "|");
                            //							Debug.WriteLine("ADded sm alias: " +ids + ", ++ " + smalls[ids]);
                            ids++;
                            add_alias++;
                        }
                    }
                    else
                    {
                        smalls[ids] = sm;
                        ids++;
                    }
                }
                if (snippets[k].Equals('=') && snippets[k + 1].Equals('=') && snippets[k + 2].Equals('=') && snippets[k + 3].Equals('=') && snippets[k + 4].Equals('>'))
                {
                    var endl = snippets.IndexOf('\n', k + 2);
                    if (endl == -1)
                        endl = snippets.Length;
                    var pool = snippets.Substring(k + 5, endl - (k + 5));
                    if (isSnip)
                        pool = snippets.Substring(k + 5);
                    StringBuilder pyust = new StringBuilder(); // Should be faster than string +=
                    for (int g = 0; g != pool.Length - 5; g++)
                    {
                        if (pool[g].Equals('<') && pool[g + 1].Equals('=') && pool[g + 2].Equals('=') && pool[g + 3].Equals('=') && pool[g + 4].Equals('='))
                            break;
                        pyust.Append(pool[g]);
                    }
                    if (add_alias != 0)
                    {
                        while (add_alias != 0)
                        {
                            //							Debug.WriteLine("ADded exp alias: " +idb + ", ++ " + pyust);
                            bigs[idb] = (pyust.ToString());
                            idb++;
                            add_alias--;
                        }
                    }
                    else
                    {
                        bigs[idb] = (pyust.ToString());
                        idb++;
                    }
                    k += 4 + pyust.Length;
                }
            }
            if (isSnip)
            {
                //				snipps = exps = null;
                //				Memory.Flush();
                snipps = smalls;
                exps = bigs;
            }
            else
            {
                //				as_wrongs = as_corrects = null;
                //				Memory.Flush();

                //Auto switch data;
                as_wrongs = smalls;
                as_corrects = bigs;

            }
        }
        /// <summary>
        /// Re-Initializes snippets.
        /// </summary>
        public static void ReInitSnippets()
        {
            if (System.IO.File.Exists(SwitcherUI.snipfile))
            {
                var snippets = System.IO.File.ReadAllText(SwitcherUI.snipfile);
                Stopwatch watch = null;
                if (SwitcherUI.LoggingEnabled)
                {
                    watch = new Stopwatch();
                    watch.Start();
                }
                GetSnippetsData(snippets);
                if (SwitcherUI.LoggingEnabled)
                {
                    watch.Stop();
                    Logging.Log("Snippets init finished, elapsed [" + watch.Elapsed.TotalMilliseconds + "] ms.");
                    watch.Reset();
                    watch.Start();
                }
                if (SwitcherUI.AutoSwitchEnabled)
                {
                    GetSnippetsData(SwitcherUI.AutoSwitchDictionaryRaw, false);
                    if (SwitcherUI.AutoSwitchDictionaryTooBig)
                    {
                        SwitcherUI.AutoSwitchDictionaryRaw = null;
                        Memory.Flush();
                    }
                }
                else
                {
                    as_wrongs = as_corrects = null;
                    SwitcherUI.AutoSwitchDictionaryTooBig = false;
                    Memory.Flush();
                }
                if (SwitcherUI.LoggingEnabled && SwitcherUI.AutoSwitchEnabled)
                {
                    watch.Stop();
                    Logging.Log("AutoSwitch dictionary init finished, elapsed [" + watch.Elapsed.TotalMilliseconds + "] ms.");
                }
            }
            Memory.Flush();
        }
        #region Snippets Aliases
        static readonly string pipe_esc = "__pipeEscape::";
        #endregion
        /// <summary>
        ///  Contains key(Keys key), it state(bool upper), if it is Alt+[NumPad](bool altnum) and array of numpads(list of numpad keys).
        /// </summary>
        public struct YuKey
        {
            public Keys key;
            public bool upper;
            public bool altnum;
            public List<Keys> numpads;
        }
        public struct NCR
        {
            public string rule;
            public bool isnip;
            public bool iauto;
        }
        #endregion
    }
}