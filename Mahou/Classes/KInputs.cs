using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mahou
{
    static class KInputs
    {
        /// <summary>
        /// Creates INPUT from key and state.
        /// </summary>
        /// <param name="key">Key to be converted to INPUT.</param>
        /// <param name="down">State of key(down=true, up=false)</param>
        /// <returns>WinAPI.INPUT</returns>
        public static WinAPI.INPUT AddKey(Keys key, bool down)
        {
            var vk = (UInt16)key;
            var scan = (ushort)(WinAPI.MapVirtualKey(vk, 0) & 0xFF);
            //System.Diagnostics.Debug.WriteLine("ADDED VK: " +vk + " KEY: " + key + " scan: " + scan);
            var input = new WinAPI.INPUT
            {
                Type = WinAPI.INPUT_KEYBOARD,
                Data = {
                    Keyboard = new WinAPI.KEYBDINPUT {
                        Vk = vk,
                        Flags = IsExtended(key) ? (down ? (WinAPI.KEYEVENTF_EXTENDEDKEY) :
                                                   (WinAPI.KEYEVENTF_KEYUP | WinAPI.KEYEVENTF_EXTENDEDKEY)) :
                                                    (down ? 0 : WinAPI.KEYEVENTF_KEYUP),
                        Scan = scan,
                        ExtraInfo = IntPtr.Zero,
                        Time = 0
                    }
                }
            };
            return input;
        }
        public static WinAPI.INPUT[] AddPress(Keys key, int times = 1)
        {
            var q = new List<WinAPI.INPUT>();
            for (int j = 0; j <= times - 1; j++)
            {
                //System.Diagnostics.Debug.WriteLine("Sending "+j+", key:"+key);
                q.Add(AddKey(key, true));
                q.Add(AddKey(key, false));
            }
            return q.ToArray(); ;
        }
        /// <summary>
        /// Returns true if key is extended, else false.
        /// </summary>
        /// <param name="key">Key to be checked.</param>
        /// <returns>bool</returns>
        public static bool IsExtended(Keys key)
        { //Check for extended keys
            return key == Keys.RMenu ||
                key == Keys.RControlKey ||
                key == Keys.Insert ||
                key == Keys.Delete ||
                key == Keys.Home ||
                key == Keys.End ||
                key == Keys.Prior ||
                key == Keys.Next ||
                key == Keys.Right ||
                key == Keys.Up ||
                key == Keys.Left ||
                key == Keys.Down ||
                key == Keys.NumLock ||
                key == Keys.Cancel ||
                key == Keys.Snapshot ||
                key == Keys.Return ||
                key == Keys.Divide ||
                key == Keys.LMenu ||
                key == Keys.RMenu ||
                key == Keys.Menu ||
                key == Keys.ControlKey;
        }
        public static string GetWordByIndex(string LINE, int index)
        {
            var WORDS = Mahou.KMHook.SplitWords(LINE);
            string word = "";
            var len = 0;
            for (int i = 0; i != WORDS.Length; i++)
            {
                if (index >= len && index <= (len += WORDS[i].Length))
                {
                    word = WORDS[i];
                    //    			System.Diagnostics.Debug.WriteLine("WORD AT INDEX "+index + " IS => ["+word+"]");
                }
            }
            return word;
        }
        /// <summary>
        /// Creates array of INPUT from string.
        /// </summary>
        /// <param name="str">String to be converted into INPUT[].</param>
        /// <returns>INPUT[]</returns>
        public static WinAPI.INPUT[] AddString(string str)
        {
            var result = new List<WinAPI.INPUT>();
            var index = 0;
            int l1_ok = 0, l2_ok = 0;
            foreach (var s in str)
            {
                bool uselt1_vk, uselt2_vk;
                ushort resultvk = 0;
                short lt1_vk = WinAPI.VkKeyScanEx(s, Mahou.MahouUI.MAIN_LAYOUT1);
                uselt1_vk = lt1_vk != -1;
                short lt2_vk = WinAPI.VkKeyScanEx(s, Mahou.MahouUI.MAIN_LAYOUT2);
                uselt2_vk = lt2_vk != -1;
                if (uselt1_vk && uselt2_vk && lt1_vk != lt2_vk)
                {
                    var guess = Mahou.KMHook.WordGuessLayout(GetWordByIndex(str, index));
                    //        		System.Diagnostics.Debug.WriteLine("ST:"+guess.Item2);
                    var lt_guess = guess.Item2;
                    resultvk = (ushort)WinAPI.VkKeyScanEx(s, lt_guess);
                }
                else if (uselt1_vk)
                {
                    resultvk = (ushort)lt1_vk;
                    l1_ok++;
                }
                else if (uselt2_vk)
                {
                    resultvk = (ushort)lt2_vk;
                    l2_ok++;
                }
                bool resultvk_state = ((resultvk >> 8) & 0xff) == 1;
                if (resultvk_state)
                    result.Add(KInputs.AddKey(Keys.RShiftKey, true));
                var down = new WinAPI.INPUT
                {
                    Type = WinAPI.INPUT_KEYBOARD,
                    Data = {
                        Keyboard = new WinAPI.KEYBDINPUT {
                            Vk = MahouUI.GuessKeyCodeFix ? resultvk : (ushort)0,
                            Flags = (UInt32)(WinAPI.KEYEVENTF_UNICODE),
                            Scan = (UInt16)s,
                            ExtraInfo = IntPtr.Zero,
                            Time = 0
                        }
                    }
                };
                var up = new WinAPI.INPUT
                {
                    Type = WinAPI.INPUT_KEYBOARD,
                    Data = {
                        Keyboard = new WinAPI.KEYBDINPUT {
                            Vk = MahouUI.GuessKeyCodeFix ? resultvk : (ushort)0,
                            Flags = (UInt32)(WinAPI.KEYEVENTF_UNICODE | WinAPI.KEYEVENTF_KEYUP),
                            Scan = (UInt16)s,
                            ExtraInfo = IntPtr.Zero,
                            Time = 0
                        }
                    }
                };
                if ((((UInt16)s) & 0xFF00) == 0xE000)
                {
                    down.Data.Keyboard.Flags |= (UInt32)WinAPI.KEYEVENTF_EXTENDEDKEY;
                    up.Data.Keyboard.Flags |= (UInt32)WinAPI.KEYEVENTF_EXTENDEDKEY;
                }
                if (s == '\n')
                {
                    down = AddKey(Keys.Return, true);
                    up = AddKey(Keys.Return, false);
                }
                result.Add(down);
                result.Add(up);
                if (resultvk_state)
                    result.Add(KInputs.AddKey(Keys.RShiftKey, false));
                index++;
            }
            if (MahouUI.GuessKeyCodeFix)
            {
                if (l1_ok != l2_ok)
                {
                    var lll = (l1_ok > l2_ok ? MahouUI.MAIN_LAYOUT1 : MahouUI.MAIN_LAYOUT2);
                    System.Diagnostics.Debug.WriteLine("Changing to layout " + lll + " before GuessKeyCodeFix.");
                    KMHook.ChangeToLayout(Locales.ActiveWindow(), lll);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Don't know what to do...");
                }
            }
            return result.ToArray();
        }
        /// <summary>
        /// Makes input INPUT's in "inputs" variable.
        /// </summary>
        /// <param name="inputs">Array of INPUT to be inputted.</param>
        public static void MakeInput(WinAPI.INPUT[] inputs, int mods = 0)
        { //Simply, sends input
            var rinputs = new List<WinAPI.INPUT>();
            if (Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_SHIFT)) rinputs.Add(AddKey(Keys.LShiftKey, true));
            if (Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_ALT)) rinputs.Add(AddKey(Keys.LMenu, true));
            if (Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_CONTROL)) rinputs.Add(AddKey(Keys.LControlKey, true));
            if (Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_WIN)) rinputs.Add(AddKey(Keys.LWin, true));
            rinputs.AddRange(inputs);
            if (Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_SHIFT)) rinputs.Add(AddKey(Keys.LShiftKey, false));
            if (Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_ALT)) rinputs.Add(AddKey(Keys.LMenu, false));
            if (Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_CONTROL)) rinputs.Add(AddKey(Keys.LControlKey, false));
            if (Hotkey.ContainsModifier(mods, (int)WinAPI.MOD_WIN)) rinputs.Add(AddKey(Keys.LWin, false));
            var sinputs = rinputs.ToArray();
            var done = WinAPI.SendInput((UInt32)sinputs.Length, sinputs, Marshal.SizeOf(typeof(WinAPI.INPUT)));
            //System.Diagnostics.Debug.WriteLine("VK SENDED: " + sinputs[0].Data.Keyboard.Vk);
            if (done != sinputs.Length)
                Mahou.Logging.Log("ERROR during send input, lenght: " + done + ", Win32ERR: " + Marshal.GetLastWin32Error());
        }
    }
}