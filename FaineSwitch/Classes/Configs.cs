using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FaineSwitch
{
    /// <summary>Ini settings writer/reader in memory, not from disk.</summary>
    class INI
    {
        #region Variables
        /// <summary>Raw INI configs file</summary>
        public string Raw;
        /// <summary>Split into lines INI configs file</summary>
        public string[] lines;
        public bool DEBUG;
        #endregion

        public INI(string ini, bool dbg = false)
        {
            this.Raw = ini;
            this.lines = Raw.Replace("\r", "").Split('\n');
            this.DEBUG = dbg;
        }
        #region Debug
        public void log(string str)
        {
            if (!DEBUG) return;
            Logging.Log(str);
        }
        #endregion
        #region Has/Is
        public bool IsCommented(string line)
        {
            if (String.IsNullOrEmpty(line)) return false;
            if (line[0] == '!' || line[0] == ';')
            {
                log("Commented line: " + line);
                return true;
            }
            return false;
        }
        public int HasSection(string Section)
        {
            for (int a = 0; a != lines.Length; a++)
            {
                log("Line => " + lines[a]);
                if (IsCommented(lines[a]))
                    continue;
                if (lines[a] == "[" + Section + "]")
                {
                    return a;
                }
            }
            return -1;
        }
        public int HasValue(int sect, string ValueName)
        {
            if (sect == -1) return -1;
            else
            {
                log("SECT LINE: " + sect);
                for (int i = sect + 1; i != lines.Length; i++)
                {
                    var line = lines[i];
                    if (IsCommented(line))
                        continue;
                    if (line.Length <= 1)
                    {
                        log("--EMPTY LINE!");
                        return -1;
                    }
                    if (line[0] == '[' && line[line.Length - 1] == ']')
                    {
                        log("--NEXT SECT!");
                        return -1;
                    }
                    var valeq = line.Split('=')[0];
                    log(">>Value Line => " + line + " I: " + i);
                    // log("ValEq: " + valeq);
                    if (valeq == ValueName)
                    {
                        log("===Has value: " + ValueName);
                        return i;
                    }
                }
            }
            return -1;
        }
        #endregion
        #region Writing
        string[] AddLine(string NewLine, int pos, string[] source)
        {
            var _source = new string[source.Length + 1];
            if (pos == -1)
            {
                _source[0] = NewLine;
                Array.Copy(source, pos + 1, _source, pos + 2, source.Length - 1);
            }
            else
            {
                if (pos != 0)
                    Array.Copy(source, 0, _source, 0, pos);
                else
                    _source[0] = source[0];
                _source[pos] = source[pos];
                _source[pos + 1] = NewLine;
                Array.Copy(source, pos + 1, _source, pos + 2, source.Length - 1 - pos);
            }
            return _source;
        }
        public void SetValue(string Section, string ValueName, string Value)
        {
            var sect = HasSection(Section);
            var val_line = HasValue(sect, ValueName);
            if (sect == -1)
            {
                log("  NO SUCH SECT! " + Section);
                lines = AddLine("[" + Section + "]", sect, lines);
                sect = 0;
                val_line = -1;
            }
            if (val_line > -1)
            {
                lines[val_line] = ValueName + "=" + Value;
            }
            if (val_line == -1)
            {
                log("   NO SUCH VALUE! " + ValueName);
                lines = AddLine(ValueName + "=" + Value, sect, lines);
            }
            if (val_line == -1 || val_line > -1 || sect == -1)
            {
                Raw = string.Join(Environment.NewLine, lines);
                lines = Raw.Replace("\r", "").Split('\n');
            }
        }
        #endregion
        #region Reading
        public string GetValue(string Section, string ValueName)
        {
            log("Getting value :" + ValueName + ": from section [" + Section + "].");
            var sect = HasSection(Section);
            var val_line = HasValue(sect, ValueName);
            if (val_line < 0) return "";
            return lines[val_line].Split(new[] { '=' }, 2)[1];
        }
        #endregion
    }
    class Configs
    {
        public static bool forceAppData = true;
        public static bool fine = false;
        /// <summary> FaineSwitch.ini file path. </summary>
        public static string filePath = Path.Combine(SwitcherUI.switcher_folder_appd, "FaineSwitch.ini");

        public INI _INI;
        /// <summary> 
        /// Creates if it is not exist and test that config file FaineSwitch.ini is readable
        /// on startup to create dialog about forced AppData config if config file failed to be created/read. 
        /// </summary>
        public static void CreateConfigsFile()
        {
            bool create = true;
            try
            {
                if (!File.Exists(filePath))
                {
                    if (!Directory.Exists(SwitcherUI.switcher_folder_appd))
                    {
                        Directory.CreateDirectory(SwitcherUI.switcher_folder_appd);
                    }

                    // Write configs start
                    File.WriteAllText(filePath, "!Unicode(✔), FaineSwitch settings file", Encoding.Unicode);
                    create = false;
                }
                else
                {
                    using (var sr = new StreamReader(filePath))
                    {
                        sr.Read();
                    }
                }
                fine = true;
            }
            catch (Exception e)
            {
                fine = false;
                if (!SwitchToAppData(create, e))
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
        }
        public static bool SwitchToAppData(bool create, Exception e)
        {
            if (Program.C_SWITCH) { return false; }
            Logging.Log("Configs read/write error, details: " + e.Message + "\n" + e.StackTrace);
            DialogResult swithcToAppDataDialogResult = MessageBox.Show(
                Program.Lang[Languages.Element.ConfigsCannot] + (create ? Program.Lang[Languages.Element.Created] : Program.Lang[Languages.Element.Readen]) + ", " + Program.Lang[Languages.Element.Error].ToLower() + ":\r\n" + e.Message + "\r\n" + Program.Lang[Languages.Element.RetryInAppData],
                Program.Lang[Languages.Element.Error],
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error);
            if (swithcToAppDataDialogResult == DialogResult.Yes)
            {
                if (!Directory.Exists(SwitcherUI.switcher_folder_appd))
                {
                    Directory.CreateDirectory(SwitcherUI.switcher_folder_appd);
                }
                filePath = Path.Combine(SwitcherUI.switcher_folder_appd, "FaineSwitch.ini");
                File.Create(Path.Combine(SwitcherUI.switcher_folder_appd, ".force"));
                Program.MyConfs = new Configs();
                return true;
            }
            return false;
        }
        public string GetRawWithoutGroup(string gr, string OutINI = "")
        {
            var glr = new StringBuilder();
            var inini = _INI.Raw;
            if (!string.IsNullOrEmpty(OutINI))
            {
                inini = OutINI;
            }
            var lines = inini.Split('\n');
            int g = 0;
            for (int i = 0; i != lines.Length; i++)
            {
                var l = lines[i];
                if (l.StartsWith("["))
                {
                    if (l.StartsWith(gr))
                    {
                        g = 1;
                    }
                    else g = 0;
                }
                if (g == 0)
                {
                    var nl = (i == lines.Length) ? "" : Environment.NewLine;
                    glr.Append(l.Replace("\r", "")).Append(nl);
                }
            }
            return glr.ToString();
        }
        /// <summary> Check if configs file readable. </summary>
        /// <returns>Read access.</returns>
        public static bool Readable()
        {
            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    sr.Read();
                }
            }
            catch (Exception e) { Logging.Log("Configs file [" + filePath + "] cannot be readen, error:\r\n" + e.Message); return false; }
            return true;
        }
        /// <summary> Initializes settings, if some of values or settings file, not exists it creates them with default value. </summary>
        public Configs()
        {
            CreateConfigsFile();
            ReadFromDisk();
            #region Hidden
            CheckBool("Hidden", "cmdbackfix", "true");
            CheckBool("Hidden", "DARKTHEME", "false");
            CheckInt("Hidden", "Layout_D_Modifier_Key", "0");
            CheckInt("Hidden", "Layout_2_Modifier_Key", "0");
            CheckInt("Hidden", "Layout_1_Modifier_Key", "0");
            CheckBool("Hidden", "CycleCaseSaveBase", "false");
            CheckBool("Hidden", "SwitcherMMTrayHoverLostFocusClose", "true");
            CheckString("Hidden", "AutoSwitchEndingSymbols", "");
            CheckBool("Hidden", "ClipBackOnlyText", "false");
            CheckString("Hidden", "Redefines", "");
            CheckInt("Hidden", "TrayHoverSwitcherMM", "0");
            CheckString("Hidden", "SymbolClear", "");
            CheckInt("Hidden", "OverlayExcludedInterval", "2500");
            CheckString("Hidden", "OverlayExcluded", "");
            CheckString("Hidden", "NCS", "NCS");
            CheckBool("Hidden", "NCS_tray", "false");
            CheckInt("Hidden", "AutoRestartMins", "0");
            CheckString("Hidden", "ToggleAutoSwitchHK", "");
            CheckBool("Hidden", "DisableMemoryFlush", "false");
            CheckBool("Hidden", "ChangeLayoutOnTrayLMB+DoubleClick", "false");
            CheckBool("Hidden", "ChangeLayoutOnTrayLMB", "false");
            CheckString("Hidden", "AutoCopyTranslation", "");
            CheckString("Hidden", "ReSelectCustoms", "tTRSULCN");
            CheckBool("Hidden", "LibreCtrlAltShiftV", "false");
            CheckString("Hidden", "CycleCaseOrder", "TULSR");
            CheckBool("Hidden", "CycleCaseReset", "false");
            CheckBool("Hidden", "__selection", "false");
            CheckBool("Hidden", "__selection_nomouse", "false");
            CheckString("Hidden", "onlySnippetsExcluded", "");
            CheckString("Hidden", "onlyAutoSwitchExcluded", "");
            CheckBool("Hidden", "__setlayout_FORCED", "false");
            CheckBool("Hidden", "__setlayout_ONLYWM", "false");
            CheckBool("Hidden", "AS_IngoreBack", "false");
            CheckBool("Hidden", "AS_IngoreDel", "false");
            CheckBool("Hidden", "AS_IngoreLS", "false");
            CheckString("Hidden", "AS_IngoreRules", "SWMCLT");
            CheckInt("Hidden", "AS_IngoreLSTimeout", "5000");
            #endregion
            #region Sync
            CheckString("Sync", "BBools", "0|1|0|0|0");
            CheckString("Sync", "RBools", "1|1|1|1|0");
            CheckString("Sync", "RLast", "");
            CheckString("Sync", "BLast", "");
            CheckBool("Sync", "ZxZ", "false");
            #endregion
            #region TranslatePanel
            CheckString("TranslatePanel", "TextFont", "Microsoft Sans Serif; 8.25pt");
            CheckString("TranslatePanel", "TitleFont", "Segoe UI; 12pt");
            CheckBool("TranslatePanel", "Enabled", "False");
            CheckBool("TranslatePanel", "UseGS", "False");
            CheckBool("TranslatePanel", "UseNA", "True");
            CheckBool("TranslatePanel", "OnDoubleClick", "False");
            CheckBool("TranslatePanel", "BorderAero", "False");
            CheckInt("TranslatePanel", "Transparency", "90");
            CheckString("TranslatePanel", "FG", "#8B5FFF");
            CheckString("TranslatePanel", "BorderC", "#F1F100");
            CheckString("TranslatePanel", "BG", "#FFFFFF");
            CheckString("TranslatePanel", "LanguageSets", "set_1/auto/ru|set_2/auto/en");
            CheckBool("TranslatePanel", "Transcription", "false");
            #endregion
            #region Sounds
            CheckBool("Sounds", "Enabled", "False");
            CheckBool("Sounds", "OnAutoSwitch", "True");
            CheckBool("Sounds", "OnSnippets", "False");
            CheckBool("Sounds", "OnConvertLast", "True");
            CheckBool("Sounds", "OnLayoutSwitch", "False");
            CheckBool("Sounds", "UseCustomSound", "False");
            CheckString("Sounds", "CustomSound", "");
            CheckBool("Sounds", "OnAutoSwitch2", "False");
            CheckBool("Sounds", "OnSnippets2", "True");
            CheckBool("Sounds", "OnConvertLast2", "False");
            CheckBool("Sounds", "OnLayoutSwitch2", "True");
            CheckBool("Sounds", "UseCustomSound2", "False");
            CheckString("Sounds", "CustomSound2", "");
            #endregion
            #region Proxy section
            CheckString("Proxy", "Password", "");
            CheckString("Proxy", "UserName", "");
            CheckString("Proxy", "ServerPort", "");
            #endregion
            #region Updates
            CheckString("Updates", "LatestCommit", "");
            CheckString("Updates", "Channel", "Stable");
            CheckString("Updates", "Delay", "5");
            #endregion
            #region Language Panel
            CheckBool("LangPanel", "UpperArrow", "true");
            CheckBool("LangPanel", "BorderAeroColor", "true");
            CheckString("LangPanel", "BorderColor", "#8B5FFF");
            CheckString("LangPanel", "Font", "Microsoft Sans Serif; 8.25pt");
            CheckString("LangPanel", "BackColor", "#FFFFFF");
            CheckString("LangPanel", "ForeColor", "#000000");
            CheckString("LangPanel", "Position", "X0 Y0");
            CheckInt("LangPanel", "RefreshRate", "25");
            CheckInt("LangPanel", "Transparency", "90");
            CheckBool("LangPanel", "Display", "false");
            #endregion
            #region Hotkeys section
            CheckInt("Hotkeys", "ShowCMenuUnderMouse_Key", "0");
            CheckString("Hotkeys", "ShowCMenuUnderMouse_Modifiers", "");
            CheckBool("Hotkeys", "ShowCMenuUnderMouse_Double", "false");
            CheckBool("Hotkeys", "ShowCMenuUnderMouse_Enabled", "false");
            // Cycle Case
            CheckInt("Hotkeys", "CycleCase_Key", "114");
            CheckString("Hotkeys", "CycleCase_Modifiers", "Shift");
            CheckBool("Hotkeys", "CycleCase_Double", "false");
            CheckBool("Hotkeys", "CycleCase_Enabled", "false");
            // Cycle Case
            CheckInt("Hotkeys", "ToggleSwitcher_Key", "112");
            CheckString("Hotkeys", "ToggleSwitcher_Modifiers", "Win Shift");
            CheckBool("Hotkeys", "ToggleSwitcher_Double", "false");
            CheckBool("Hotkeys", "ToggleSwitcher_Enabled", "true");
            // Toggle FaineSwitch
            CheckInt("Hotkeys", "ShowSelectionTranslate_Key", "0");
            CheckString("Hotkeys", "ShowSelectionTranslate_Modifiers", "Alt");
            CheckBool("Hotkeys", "ShowSelectionTranslate_Double", "true");
            CheckBool("Hotkeys", "ShowSelectionTranslate_Enabled", "false");
            // Show Selection Translation
            CheckInt("Hotkeys", "ToggleLangPanel_Key", "120");
            CheckString("Hotkeys", "ToggleLangPanel_Modifiers", "Shift");
            CheckBool("Hotkeys", "ToggleLangPanel_Double", "false");
            CheckBool("Hotkeys", "ToggleLangPanel_Enabled", "true");
            // Toggle Language Panel hotkey
            CheckInt("Hotkeys", "RestartSwitcher_Key", "33");
            CheckString("Hotkeys", "RestartSwitcher_Modifiers", "Win + Shift + Alt");
            CheckBool("Hotkeys", "RestartSwitcher_Enabled", "true");
            // Restart FaineSwitch hotkey
            CheckInt("Hotkeys", "ExitSwitcher_Key", "123");
            CheckString("Hotkeys", "ExitSwitcher_Modifiers", "Win + Control + Shift + Alt");
            CheckBool("Hotkeys", "ExitSwitcher_Double", "false");
            CheckBool("Hotkeys", "ExitSwitcher_Enabled", "true");
            // Exit FaineSwitch hotkey
            CheckInt("Hotkeys", "SelectedToLower_Key", "88");
            CheckString("Hotkeys", "SelectedToLower_Modifiers", "Win");
            CheckBool("Hotkeys", "SelectedToLower_Double", "false");
            CheckBool("Hotkeys", "SelectedToLower_Enabled", "false");
            // Selected text To Lower hotkey
            CheckInt("Hotkeys", "SelectedToUpper_Key", "90");
            CheckString("Hotkeys", "SelectedToUpper_Modifiers", "Win");
            CheckBool("Hotkeys", "SelectedToUpper_Double", "false");
            CheckBool("Hotkeys", "SelectedToUpper_Enabled", "false");
            // Selected text To Upper hotkey
            CheckInt("Hotkeys", "SelectedTextTransliteration_Key", "191");
            CheckString("Hotkeys", "SelectedTextTransliteration_Modifiers", "Win");
            CheckBool("Hotkeys", "SelectedTextTransliteration_Double", "false");
            CheckBool("Hotkeys", "SelectedTextTransliteration_Enabled", "false");
            // Selected text Transliteration hotkey
            CheckInt("Hotkeys", "SelectedTextToSwapCase_Key", "190");
            CheckString("Hotkeys", "SelectedTextToSwapCase_Modifiers", "Win");
            CheckBool("Hotkeys", "SelectedTextToSwapCase_Double", "false");
            CheckBool("Hotkeys", "SelectedTextToSwapCase_Enabled", "false");
            // Selected text to swap case hotkey
            CheckInt("Hotkeys", "SelectedTextToRandomCase_Key", "0");
            CheckString("Hotkeys", "SelectedTextToRandomCase_Modifiers", "Alt");
            CheckBool("Hotkeys", "SelectedTextToRandomCase_Double", "true");
            CheckBool("Hotkeys", "SelectedTextToRandomCase_Enabled", "false");
            // Selected text To random case hotkey
            CheckInt("Hotkeys", "SelectedTextToTitleCase_Key", "0");
            CheckString("Hotkeys", "SelectedTextToTitleCase_Modifiers", "Shift");
            CheckBool("Hotkeys", "SelectedTextToTitleCase_Double", "true");
            CheckBool("Hotkeys", "SelectedTextToTitleCase_Enabled", "false");
            // Selected text To custom converison
            CheckInt("Hotkeys", "SelectedTextToCustomConv_Key", "0");
            CheckString("Hotkeys", "SelectedTextToCustomConv_Modifiers", "");
            CheckBool("Hotkeys", "SelectedTextToCustomConv_Double", "false");
            CheckBool("Hotkeys", "SelectedTextToCustomConv_Enabled", "false");
            // Selected text to title case hotkey
            CheckInt("Hotkeys", "ToggleSymbolIgnoreMode_Key", "122");
            CheckString("Hotkeys", "ToggleSymbolIgnoreMode_Modifiers", "Shift + Control");
            CheckBool("Hotkeys", "ToggleSymbolIgnoreMode_Double", "false");
            CheckBool("Hotkeys", "ToggleSymbolIgnoreMode_Enabled", "true");
            // Toggle symbol ignore mode hotkey
            CheckInt("Hotkeys", "ConvertLastWords_Key", "122");
            CheckString("Hotkeys", "ConvertLastWords_Modifiers", "Shift");
            CheckBool("Hotkeys", "ConvertLastWords_Double", "false");
            CheckBool("Hotkeys", "ConvertLastWords_Enabled", "true");
            // Convert last words hotkey
            CheckInt("Hotkeys", "ConvertLastLine_Key", "19");
            CheckString("Hotkeys", "ConvertLastLine_Modifiers", "Shift");
            CheckBool("Hotkeys", "ConvertLastLine_Double", "false");
            CheckBool("Hotkeys", "ConvertLastLine_Enabled", "true");
            // Convert last line hotkey
            CheckInt("Hotkeys", "ConvertSelectedText_Key", "145");
            CheckString("Hotkeys", "ConvertSelectedText_Modifiers", "");
            CheckBool("Hotkeys", "ConvertSelectedText_Double", "false");
            CheckBool("Hotkeys", "ConvertSelectedText_Enabled", "true");
            // Convert selected text hotkey
            CheckInt("Hotkeys", "ConvertLastWord_Key", "19");
            CheckString("Hotkeys", "ConvertLastWord_Modifiers", "");
            CheckBool("Hotkeys", "ConvertLastWord_Double", "false");
            CheckBool("Hotkeys", "ConvertLastWord_Enabled", "true");
            // Convert last word hotkey
            CheckInt("Hotkeys", "ToggleMainWindow_Key", "45");
            CheckString("Hotkeys", "ToggleMainWindow_Modifiers", "Win + Control + Shift + Alt");
            CheckBool("Hotkeys", "ToggleMainWindow_Double", "false");
            CheckBool("Hotkeys", "ToggleMainWindow_Enabled", "true");
            // Toggle main window hotkey
            #endregion
            #region AutoSwitch section
            CheckBool("AutoSwitch", "DownloadInZip", "true");
            CheckBool("AutoSwitch", "SwitchToGuessLayout", "true");
            CheckBool("AutoSwitch", "SpaceAfter", "true");
            CheckBool("AutoSwitch", "Enabled", "true");
            #endregion
            #region Snippets section
            CheckString("Snippets", "SnippetExpKeyOther", "");
            CheckString("Snippets", "SnippetExpandKey", "Space");
            CheckBool("Snippets", "SwitchToGuessLayout", "false");
            CheckBool("Snippets", "SpaceAfter", "false");
            CheckBool("Snippets", "SnippetsEnabled", "true");
            CheckString("Snippets", "NCRSets", "set_0");
            #endregion
            #region Timings section
            CheckInt("Timings", "LangTooltipForMouseSkipMessages", "5");
            #region Excluded
            CheckBool("Timings", "ConvertSWLinExcl", "false");
            CheckBool("Timings", "ChangeLayoutInExcluded", "true");
            CheckBool("Timings", "ExcludeCaretLD", "false");
            CheckBool("Timings", "UsePasteInCS", "false");
            CheckString("Timings", "ExcludedPrograms", "");
            #endregion
            CheckInt("Timings", "SelectedTextGetMoreTriesCount", "5");
            CheckBool("Timings", "SelectedTextGetMoreTries", "false");
            CheckInt("Timings", "CapsLockDisableRefreshRate", "100");
            CheckInt("Timings", "ScrollLockStateRefreshRate", "100");
            CheckInt("Timings", "FlagsInTrayRefreshRate", "100");
            CheckInt("Timings", "DoubleHotkey2ndPressWait", "350");
            CheckInt("Timings", "LangTooltipForCaretRefreshRate", "25");
            CheckInt("Timings", "LangTooltipForMouseRefreshRate", "25");
            CheckBool("Timings", "UseDelayAfterBackspaces", "false");
            CheckInt("Timings", "DelayAfterBackspaces", "100");

            #endregion
            #region Appearence section
            CheckBool("Appearence", "WindowsMessages", "true");
            // Windows Messages instead of timers
            CheckBool("Appearence", "CaretLTUpperArrow", "false");
            CheckBool("Appearence", "MouseLTUpperArrow", "false");
            // Upper arrows for lang displays
            CheckString("Appearence", "Layout2LTText", "");
            CheckString("Appearence", "Layout1LTText", "");
            // Different text for layouts
            CheckBool("Appearence", "CaretLTUseFlags", "false");
            CheckBool("Appearence", "MouseLTUseFlags", "false");
            // Language tooltips use flags
            CheckInt("Appearence", "MCDS_Bottom", "45");
            CheckInt("Appearence", "MCDS_Top", "60");
            CheckInt("Appearence", "MCDS_Pos_Y", "13");
            CheckInt("Appearence", "MCDS_Pos_X", "58");
            // Language tooltip positions for FaineSwitch Cared Display Server
            CheckInt("Appearence", "CaretLTPositionY", "12");
            CheckInt("Appearence", "CaretLTPositionX", "8");
            CheckInt("Appearence", "CaretLTWidth", "26");
            CheckInt("Appearence", "CaretLTHeight", "14");
            CheckString("Appearence", "CaretLTFont", "Georgia; 8pt");
            CheckBool("Appearence", "CaretLTTransparentBackColor", "false");
            CheckBool("Appearence", "MouseLTTransparentBackColor", "false");
            CheckString("Appearence", "CaretLTBackColor", "#FFFFFF");
            CheckString("Appearence", "CaretLTForeColor", "#000000");
            // Language tooltip appearence for Caret Language Tooltip
            CheckInt("Appearence", "MouseLTPositionY", "0");
            CheckInt("Appearence", "MouseLTPositionX", "8");
            CheckInt("Appearence", "MouseLTWidth", "26");
            CheckInt("Appearence", "MouseLTHeight", "14");
            CheckString("Appearence", "MouseLTFont", "Georgia; 8pt");
            CheckBool("Appearence", "MouseLTTransparentBackColor", "false");
            CheckString("Appearence", "MouseLTBackColor", "#FFFFFF");
            CheckString("Appearence", "MouseLTForeColor", "#000000");
            // Language tooltip appearence for Mouse Language Tooltip
            CheckInt("Appearence", "Layout2PositionY", "0");
            CheckInt("Appearence", "Layout2PositionX", "8");
            CheckInt("Appearence", "Layout2Width", "26");
            CheckInt("Appearence", "Layout2Height", "14");
            CheckString("Appearence", "Layout2Font", "Georgia; 8pt");
            CheckBool("Appearence", "Layout2TransparentBackColor", "false");
            CheckString("Appearence", "Layout2BackColor", "#FFFFFF");
            CheckString("Appearence", "Layout2ForeColor", "#000000");
            // Language tooltip appearence for Layout 2
            CheckInt("Appearence", "Layout1PositionY", "0");
            CheckInt("Appearence", "Layout1PositionX", "8");
            CheckInt("Appearence", "Layout1Width", "26");
            CheckInt("Appearence", "Layout1Height", "14");
            CheckString("Appearence", "Layout1Font", "Georgia; 8pt");
            CheckBool("Appearence", "Layout1TransparentBackColor", "false");
            CheckString("Appearence", "Layout1BackColor", "#FFFFFF");
            CheckString("Appearence", "Layout1ForeColor", "#000000");
            // Language tooltip appearence for Layout 1
            CheckString("Appearence", "Language", "Українська");
            CheckBool("Appearence", "MouseLTAlways", "false");
            CheckBool("Appearence", "DifferentColorsForLayouts", "false");
            CheckBool("Appearence", "DisplayLangTooltipForCaretOnChange", "false");
            CheckBool("Appearence", "DisplayLangTooltipForCaret", "false");
            CheckBool("Appearence", "DisplayLangTooltipForMouseOnChange", "false");
            CheckBool("Appearence", "DisplayLangTooltipForMouse", "false");
            #endregion
            #region Persistent Layout
            CheckString("PersistentLayout", "Layout2Processes", "notepad++.exe winword.exe");
            CheckString("PersistentLayout", "Layout1Processes", "devenv.exe wdexpress.exe");
            CheckInt("PersistentLayout", "Layout2CheckInterval", "50");
            CheckInt("PersistentLayout", "Layout1CheckInterval", "50");
            CheckBool("PersistentLayout", "ActivateForLayout2", "false");
            CheckBool("PersistentLayout", "ActivateForLayout1", "false");
            CheckBool("PersistentLayout", "ChangeOnlyOnce", "false");
            CheckBool("PersistentLayout", "OnlyOnWindowChange", "false");
            #endregion
            #region Layouts section
            CheckString("Layouts", "CTRL_ALT_TemporaryChangeLayout", "0");
            CheckBool("Layouts", "QWERTZfix", "false");
            CheckString("Layouts", "SpecificKeySets", "set_0");
            CheckInt("Layouts", "SpecificKeysType", "0");
            CheckString("Layouts", "SpecificLayout4", "");
            CheckString("Layouts", "SpecificLayout3", "");
            CheckString("Layouts", "SpecificLayout2", "");
            CheckString("Layouts", "SpecificLayout1", Languages.English[Languages.Element.SwitchBetween]);
            CheckInt("Layouts", "SpecificKey4", "0");
            CheckInt("Layouts", "SpecificKey3", "0");
            CheckInt("Layouts", "SpecificKey2", "0");
            CheckInt("Layouts", "SpecificKey1", "1");
            CheckString("Layouts", "MainLayout2", "");
            CheckString("Layouts", "MainLayout1", "");
            CheckBool("Layouts", "ChangeToSpecificLayoutByKey", "true");
            CheckString("Layouts", "EmulateLayoutSwitchType", "Alt+Shift");
            CheckBool("Layouts", "EmulateLayoutSwitch", "false");
            CheckBool("Layouts", "OneLayout", "false");
            CheckBool("Layouts", "SwitchBetweenLayouts", "true");
            #endregion
            #region Functions section
            CheckBool("Functions", "TrayText", "false");
            CheckInt("Functions", "WriteInputHistoryBackSpaceType", "0");
            CheckBool("Functions", "WriteInputHistory", "false");
            CheckBool("Functions", "WriteInputHistoryByDate", "false");
            CheckBool("Functions", "WriteInputHistoryHourly", "false");
            CheckBool("Functions", "ReadOnlyNA", "false");
            CheckBool("Functions", "UseJKL", "true");
            CheckBool("Functions", "RemapCapslockAsF18", "true");
            CheckBool("Functions", "AppDataConfigs", forceAppData.ToString());
            CheckBool("Functions", "GuessKeyCodeFix", "false");
            CheckBool("Functions", "OneLayoutWholeWord", "true");
            CheckBool("Functions", "MCDServerSupport", "false");
            CheckBool("Functions", "SymbolIgnoreModeEnabled", "false");
            CheckBool("Functions", "BlockSwitcherHotkeysWithCtrl", "false");
            CheckBool("Functions", "TrayFlags", "true");
            CheckBool("Functions", "CapsLockTimer", "false");
            CheckBool("Functions", "Logging", "false");
            CheckBool("Functions", "SilentUpdate", "false");
            CheckBool("Functions", "StartupUpdatesCheck", "false");
            CheckBool("Functions", "ScrollTip", "false");
            CheckBool("Functions", "ConvertSelectionLayoutSwitchingPlus", "false");
            CheckBool("Functions", "AddOneEnterToLastWord", "false");
            CheckBool("Functions", "AddOneSpaceToLastWord", "true");
            CheckBool("Functions", "RePress", "false");
            CheckBool("Functions", "ReSelect", "true");
            CheckBool("Functions", "ConvertSelectionLayoutSwitching", "false");
            CheckBool("Functions", "TrayIconVisible", "true");
            CheckBool("Functions", "AutoStartAsAdmin", "false");
            #endregion
            #region FirstStart section
            CheckBool("FirstStart", "First", "true");
            #endregion
            fine = true;
        }
        void CheckBool(string section, string key, string default_value)
        {
            bool bt = false; //bool temp
            if (!Boolean.TryParse(Read(section, key), out bt))
                Write(section, key, default_value);
        }
        void CheckInt(string section, string key, string default_value)
        {
            int it = 0; //int temp
            if (!Int32.TryParse(Read(section, key), out it))
                Write(section, key, default_value);
        }
        void CheckString(string section, string key, string default_value)
        {
            if (String.IsNullOrEmpty(Read(section, key)))
                Write(section, key, default_value);
        }
        /// <summary> Writes "value" to "key" in "section" in INI configuration. </summary>
        public void Write(string section, string key, string value)
        {
            _INI.SetValue(section, key, value);
        }
        /// <summary> Writes "value" to "key" in "section" in INI configuration, and saves to disk. </summary>
        public void WriteSave(string section, string key, string value)
        {
            _INI.SetValue(section, key, value);
            WriteToDisk();
        }
        /// <summary> Reads "value" from "key" in "section" from INI configuration. </summary>
        public string Read(string section, string key)
        {
            return _INI.GetValue(section, key);
        }
        /// <summary>
        /// Reads "value" as int from "key" in "section" from INI configuration.
        /// </summary>
        public int ReadInt(string section, string key)
        {
            return Int32.Parse(_INI.GetValue(section, key));
        }
        /// <summary> Reads "value" as bool from "key" in "section" from INI configuration. </summary>
        public bool ReadBool(string section, string key)
        {
            return Boolean.Parse(_INI.GetValue(section, key).ToLower());
        }
        public void ReadFromDisk()
        {
            _INI = new INI(File.ReadAllText(filePath));
        }
        public void WriteToDisk()
        {
            try
            {
                File.WriteAllText(filePath, _INI.Raw);
            }
            catch (Exception e)
            {
                Logging.Log("Can't write configs file by path: [" + filePath + "].", 1);
                if (!SwitcherUI.ReadOnlyNA)
                {
                    SwitchToAppData(true, e);
                    _INI.SetValue("Functions", "AppDataConfigs", "true");
                    File.WriteAllText(filePath, _INI.Raw);
                }
            }
        }
    }
}
