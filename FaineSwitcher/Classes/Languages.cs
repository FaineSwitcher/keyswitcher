﻿using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
public class Languages
{
    public enum Element
    {
        #region Tabs
        tab_Functions,
        tab_Layouts,
        tab_Appearence,
        tab_Timings,
        tab_Excluded,
        tab_Snippets,
        tab_AutoSwitch,
        tab_Hotkeys,
        tab_Updates,
        tab_About,
        tab_LangPanel,
        tab_Sounds,
        tab_Translator,
        tab_Sync,
        tab_Share_The_Settings,
        #endregion
        #region Functions
        AutoStart,
        CreateTask,
        CreateShortcut,
        TrayIcon,
        ConvertSelectionLS,
        ReSelect,
        RePress,
        Add1Space,
        Add1NL,
        ConvertSelectionLSPlus,
        HighlightScroll,
        UpdatesCheck,
        SilentUpdate,
        Logging,
        CapsTimer,
        DisplayInTray,
        JustIcon,
        ContryFlags,
        TextLayout,
        BlockCtrlHKs,
        MCDSSupport,
        OneLayoutWholeWord,
        GuessKeyCodeFix,
        ConfigsInAppData,
        RemapCapslockAsF18,
        UseJKL,
        ReadOnlyNA,
        WriteInputHistory,
        BackSpaceType,
        #endregion
        #region Layouts
        SwitchBetween,
        EmulateLS,
        EmulateType,
        ChangeLayoutBy1Key,
        OneLayout,
        QWERTZ,
        KeysType,
        SelectKeyType,
        SetHotkeyType,
        LCTRLLALTTempLayout,
        #endregion
        #region Persistent Layout
        SwitchOnlyOnWindowChange,
        SwitchOnlyOnce,
        PersistentLayout,
        ActivatePLFP,
        CheckInterval,
        #endregion
        #region Appearence
        LDMouseDisplay,
        LDCaretDisplay,
        LDOnlyOnChange,
        LDDifferentAppearence,
        Language,
        LDAppearence,
        LDAroundMouse,
        LDAroundCaret,
        LDTransparentBG,
        LDFont,
        LDFore,
        LDBack,
        LDText,
        LDSize,
        LDPosition,
        LDWidth,
        LDHeight,
        MCDSTopIndent,
        MCDSBottomIndent,
        UseFlags,
        Always,
        LDUpperArrow,
        LDUseWinMessages,
        #endregion
        #region Timings
        LDForMouseRefreshRate,
        LDForCaretRefreshRate,
        DoubleHKDelay,
        TrayFlagsRefreshRate,
        ScrollLockRefreshRate,
        CapsLockRefreshRate,
        MoreTriesToGetSelectedText,
        LD_MouseSkipMessages,
        UseDelayAfterBackspaces,
        UsePasteInCS,
        #endregion
        #region Excluded
        ExcludedPrograms,
        Change1KeyLayoutInExcluded,
        AllowConvertSWL,
        #endregion
        #region Snippets
        SnippetsEnabled,
        SnippetSpaceAfter,
        SnippetSwitchToGuessLayout,
        SnippetsCount,
        SnippetsExpandKey,
        SnippetsExpKeyOther,
        SnippetsNCRules,
        #endregion
        #region AutoSwitch
        AutoSwitchEnabled,
        AutoSwitchSpaceAfter,
        AutoSwitchSwitchToGuessLayout,
        AutoSwitchUpdateDictionary,
        AutoSwitchDependsOnSnippets,
        AutoSwitchDictionaryWordsCount,
        DownloadAutoSwitchDictionaryInZip,
        AutoSwitchDictionaryTooBigToDisplay,
        #endregion
        #region Hotkeys
        ToggleMainWnd,
        ConvertLast,
        ConvertSelected,
        ConvertLine,
        ConvertWords,
        ToggleSymbolIgnore,
        SelectedToTitleCase,
        SelectedToRandomCase,
        SelectedToSwapCase,
        SelectedToUpperCase,
        SelectedToLowerCase,
        SelectedTransliteration,
        ExitSwitcher,
        RestartSwitcher,
        Enabled,
        DoubleHK,
        ToggleLangPanel,
        TranslateSelection,
        ToggleSwitcher,
        CycleCase,
        CustomConversion,
        ShowCMenuUnderMouse,
        #endregion
        #region LangPanel
        DisplayLangPanel,
        RefreshRate,
        Transparency,
        BorderColor,
        UseAeroColor,
        DisplayUpperArrow,
        Transcription,
        #endregion
        #region TranslatePanel
        EnableTranslatePanel,
        ShowTranslationOnDoubleClick,
        TranslateLanguages,
        Translation,
        TextFont,
        TitleFont,
        Direct,
        WebScript,
        DirectV2,
        Method,
        #endregion
        #region Updates
        CheckForUpdates,
        Checking,
        YouHaveLatest,
        TimeToUpdate,
        UpdateSwitcher,
        DownloadUpdate,
        ProxyConfig,
        ProxyServer,
        ProxyLogin,
        ProxyPass,
        Error,
        NetError,
        UpdatesChannel,
        #endregion
        #region Sounds
        EnableSounds,
        PlaySoundWhen,
        SoundOnAutoSwitch,
        SoundOnSnippets,
        SoundOnConvertLast,
        SoundOnLayoutSwitching,
        UseCustomSound,
        Select,
        #endregion
        #region About
        DbgInf,
        DbgInf_Copied,
        Site,
        Releases,
        About,
        #endregion
        #region Sync
        Backup,
        Restore,
        TooBig,
        CannotBe,
        UnknownID,
        EnterID,
        #endregion
        #region Misc
        Not,
        Transliterate,
        Convert,
        Exist,
        Latest,
        Clipboard,
        ChangeLayout,
        Keys,
        Key_Left,
        Key_Right,
        Layouts,
        Plugin,
        Layout,
        Hotkey,
        UpdateFound,
        UpdateComplete,
        ShowHide,
        Switcher,
        Download,
        ConfigsCannot,
        Created,
        Readen,
        RetryInAppData,
        Sound,
        InputHistoryBackSpaceWriteType1,
        InputHistoryBackSpaceWriteType2,
        Disabled,
        Enable,
        Open,
        #endregion
        #region Buttons
        ButtonOK,
        ButtonApply,
        ButtonCancel,
        #endregion
        #region Tooltips
        TT_SwitchBetween,
        TT_ConvertSelectionSwitch,
        TT_BlockCtrl,
        TT_CapsDis,
        TT_EmulateLS,
        TT_RePress,
        TT_Add1Space,
        TT_Add1NL,
        TT_ReSelect,
        TT_ScrollTip,
        TT_LDOnlyOnChange,
        TT_ConvertSelectionSwitchPlus,
        TT_LDForMouse,
        TT_LDForCaret,
        TT_Snippets,
        TT_Logging,
        TT_LDDifferentAppearence,
        TT_TrayDisplayType,
        TT_SymbolIgnore,
        TT_ConvertWords,
        TT_ExcludedPrograms,
        TT_MCDSSupport,
        TT_LDText,
        TT_OneLayoutWholeWordCS,
        TT_PersistentLayout,
        TT_SwitchOnlyOnWindowChange,
        TT_SwitchOnlyOnce,
        TT_OneLayout,
        TT_QWERTZ,
        TT_Change1KeyLayoutInExcluded,
        TT_AllowConvertSWL,
        TT_SnippetsSwitchToGuessLayout,
        TT_SnippetsCount,
        TT_GuessKeyCodeFix,
        TT_ConfigsInAppData,
        TT_KeysType,
        TT_SnippetExpandKey,
        TT_LDUseWinMessages,
        TT_RemapCapslockAsF18,
        TT_UseDelayAfterBackspaces,
        TT_UseJKL,
        TT_ReadOnlyNA,
        TT_WriteInputHistory,
        TT_ShowSelectionTranslationHotkey,
        TT_LeftRightMB,
        TT_CycleCase,
        TT_CustomConversion,
        TT_Transcription_1,
        TT_Transcription_2,
        TT_SnippetsEditHotkeys,
        TT_LCTRLLALTTempLayout,
        #endregion
        #region Messages
        MSG_SnippetsError,
        NotNeedSwitch,
        NeedSwitch,
        #endregion
        #region Share_The_Settings
        Share_The_Settings_Export,
        Share_The_Settings_Import,
        Share_The_Setting_Info
        #endregion
    }
    public static Dictionary<Element, string> English = new Dictionary<Element, string>() { 
		#region Tabs
		{ Element.tab_Functions, "Functions" },
        { Element.tab_Layouts, "Layouts" },
        { Element.tab_Appearence, "Appearance" },
        { Element.tab_Timings, "Timings" },
        { Element.tab_Excluded, "Excluded" },
        { Element.tab_Snippets, "Snippets" },
        { Element.tab_AutoSwitch, "Auto switch" },
        { Element.tab_Hotkeys, "Hotkeys" },
        { Element.tab_LangPanel, "Language panel" },
        { Element.tab_Updates, "Updates" },
        { Element.tab_About, "About" },
        { Element.tab_Sounds, "Sounds" },
        { Element.tab_Translator, "Translator" },
        { Element.tab_Sync, "Sync" }, 
        { Element.tab_Share_The_Settings, "Share The Settings" }, 
		#endregion
		#region Functions
		{ Element.AutoStart, "Start with Windows." },
        { Element.CreateTask, "Create task (will run as Administrator)."},
        { Element.CreateShortcut, "Create shortcut in Startup folder."},
        { Element.TrayIcon, "Show tray icon." },
        { Element.ConvertSelectionLS, "Convert selection layout switching." },
        { Element.ReSelect, "Re-select text after conversion." },
        { Element.RePress, "Re-press modifiers after hotkey action." },
        { Element.Add1Space, "Consider 1 space as part of last word." },
        { Element.Add1NL, "Consider Enter as part of last word." },
        { Element.ConvertSelectionLSPlus, "Convert selection layout switching+." },
        { Element.HighlightScroll, "Highlight Scroll-Lock when layout 1 is active." },
        { Element.UpdatesCheck, "Check for updates at startup." },
        { Element.SilentUpdate, "Silent update at startup." },
        { Element.Logging, "Enable logging for debugging." },
        { Element.CapsTimer, "Activate Caps Lock disabler timer." },
        { Element.DisplayInTray, "Display in tray:" },
        { Element.JustIcon, "Faine Switcher's icon" },
        { Element.ContryFlags, "Country flags" },
        { Element.TextLayout, "Text of layout" },
        { Element.BlockCtrlHKs, "Block Faine Switcher hotkeys with Ctrl." },
        { Element.MCDSSupport, "Enable MCDS support." },
        { Element.OneLayoutWholeWord, "Use layout for whole word in CS." },
        { Element.GuessKeyCodeFix, "Use guess key code fix." },
        { Element.ConfigsInAppData, "Configs in AppData." },
        { Element.RemapCapslockAsF18, "Remap Caps Lock as F18." },
        { Element.UseJKL, "Retrieve layout from JKL." },
        { Element.ReadOnlyNA, "Read only if no access." },
        { Element.WriteInputHistory, "Write input history." },
        { Element.BackSpaceType, "Backspace type:" },
		#endregion
		#region Layouts
		{ Element.SwitchBetween, "Switch between layouts" },
        { Element.EmulateLS, "Emulate layout switching." },
        { Element.EmulateType, "Emulation type:" },
        { Element.ChangeLayoutBy1Key, "Change to specific layout by keys:" },
        { Element.OneLayout, "One layout for all programs." },
        { Element.QWERTZ, "Fix for QWERTZ keyboard." },
        { Element.KeysType, "Keys type:" },
        { Element.SelectKeyType, "Select key." },
        { Element.SetHotkeyType, "Set hotkey." },
        { Element.LCTRLLALTTempLayout, "Temporary change layout on LCtrl+LAlt combination:" },
		#endregion
		#region Persistent Layout
		{ Element.PersistentLayout, "Persistent layout" },
        { Element.SwitchOnlyOnWindowChange, "Change only when active window changes." },
        { Element.SwitchOnlyOnce, "Change layout only once per window." },
        { Element.ActivatePLFP, "Activate persistent layout for processes:" },
        { Element.CheckInterval, "Check interval (ms):" }, 
		#endregion
		#region Appearence
		{ Element.LDMouseDisplay, "Display current language tooltip around mouse." },
        { Element.LDCaretDisplay, "Display current language tooltip around caret." },
        { Element.LDOnlyOnChange, "Only on change." },
        { Element.LDDifferentAppearence, "Use different appearance for layouts." },
        { Element.Language, "Language:" },
        { Element.LDAppearence, "Language tooltip appearance:" },
        { Element.LDAroundMouse, "Around mouse" },
        { Element.LDAroundCaret, "Around caret" },
        { Element.LDTransparentBG, "Transparent color." },
        { Element.LDFont, "Font" },
        { Element.LDFore, "Foreground color:" },
        { Element.LDBack, "Background color:" },
        { Element.LDText, "Tooltip text:" },
        { Element.LDSize, "Size" },
        { Element.LDPosition, "Position" },
        { Element.LDWidth, "Width" },
        { Element.LDHeight, "Height" },
        { Element.MCDSTopIndent, "Top" },
        { Element.MCDSBottomIndent, "Bottom" },
        { Element.UseFlags, "Use flags." },
        { Element.Always, "Always." },
        { Element.LDUpperArrow, "Arrow when upper case." },
        { Element.LDUseWinMessages, "Use Windows Messages instead of timers." },
		#endregion
		#region Timings
		{ Element.LDForMouseRefreshRate, "Language tooltip around mouse refresh rate(ms):" },
        { Element.LDForCaretRefreshRate, "Language tooltip around caret refresh rate(ms):" },
        { Element.DoubleHKDelay, "Double hotkey wait time for second press(ms):" },
        { Element.TrayFlagsRefreshRate, "Flags in tray icon refresh rate(ms):" },
        { Element.ScrollLockRefreshRate, "Scroll Lock refresh rate(ms):" },
        { Element.CapsLockRefreshRate, "Caps Lock update rate(ms):" },
        { Element.MoreTriesToGetSelectedText, "Use more tries to get selected text:" },
        { Element.LD_MouseSkipMessages, "Mouse movement Messages to skip before updating language tooltips:" },
        { Element.UseDelayAfterBackspaces, "Use delay after backspaces in convert last word(ms):" },
        { Element.UsePasteInCS, "Use Paste in Selection Conversions." },
		#endregion
		#region Excluded
		{ Element.ExcludedPrograms, "Excluded programs:" },
        { Element.Change1KeyLayoutInExcluded, "Change layout by 1 key even in excluded." },
        { Element.AllowConvertSWL, "Allow convert selection/word/line conversion." },
        { Element.NeedSwitch, "Words that need to be switched" },
        { Element.NotNeedSwitch, "Words that do not need to be switched" },
		#endregion
		#region Snippets
		{ Element.SnippetsEnabled, "Enable snippets." },
        { Element.SnippetSpaceAfter, "Add 1 space after snippets." },
        { Element.SnippetSwitchToGuessLayout, "Switch to guess layout after snippet." },
        { Element.SnippetsCount, "Snippets: " },
        { Element.SnippetsExpandKey, "Snippet expand key: " },
        { Element.SnippetsExpKeyOther, "Other" },
        { Element.SnippetsNCRules, "Disable snippets/autoswitch rules (Regex)" }, 
		#endregion
		#region AutoSwitch
		{ Element.AutoSwitchEnabled, "Enable auto-switch." },
        { Element.AutoSwitchSpaceAfter, "Add 1 space after auto-switch." },
        { Element.AutoSwitchSwitchToGuessLayout, "Switch to guess layout after auto-switch." },
        { Element.AutoSwitchUpdateDictionary, "Update auto-switch dictionary." },
        { Element.AutoSwitchDependsOnSnippets, "To use this feature enable Snippets feature!" },
        { Element.AutoSwitchDictionaryWordsCount, "Words: " },
        { Element.DownloadAutoSwitchDictionaryInZip, "Download auto-switch dictionary in zip." },
        { Element.AutoSwitchDictionaryTooBigToDisplay, "Too big dictionary, it will take a lot time to display, dictionary display disabled.\r\nDictionary WON'T be updated every time you press [Apply] button, it will be updated once only after restart or after disabling=>Apply=>enabling=>Apply AutoSwitch feature or through \"[Hidden] Toggle AutoSwitch Hotkey\" also only once." }, 
		#endregion
		#region Hotkeys
		{ Element.ToggleMainWnd, "Toggle settings window" },
        { Element.ConvertLast, "Convert last word" },
        { Element.ConvertSelected, "Convert selected text" },
        { Element.ConvertLine, "Convert last line" },
        { Element.ConvertWords, "Convert specific last words count" },
        { Element.ToggleSymbolIgnore, "Toggle symbol ignore mode" },
        { Element.SelectedToTitleCase, "Selected text words to Title Case" },
        { Element.SelectedToRandomCase, "Selected text words to RanDoM cASe" },
        { Element.SelectedToSwapCase, "Selected text words to sWAP cASE" },
        { Element.SelectedToUpperCase, "Selected text to UPPER CASE" },
        { Element.SelectedToLowerCase, "Selected text to lower case" },
        { Element.SelectedTransliteration, "Selected text transliteration" },
        { Element.ExitSwitcher, "Exit" },
        { Element.RestartSwitcher, "Restart" },
        { Element.Enabled, "Enabled" },
        { Element.DoubleHK, "Double hotkey" },
        { Element.ToggleLangPanel, "Toggle language panel" },
        { Element.TranslateSelection, "Show selected text translation" },
        { Element.ToggleSwitcher, "Toogle Pause" },
        { Element.CycleCase, "Cycle selected text case" },
        { Element.CustomConversion, "Custom text conversion" },
        { Element.ShowCMenuUnderMouse, "Show tray context menu under mouse" }, 
		#endregion
		#region LangPanel
		{ Element.DisplayLangPanel, "Display language panel." },
        { Element.RefreshRate, "Refresh rate(ms):" },
        { Element.Transparency, "Transparency,%:" },
        { Element.BorderColor, "Border color:" },
        { Element.UseAeroColor, "Use Aero/Accent color." },
        { Element.DisplayUpperArrow, "Display up arrow icon when input is upper case." },
        { Element.Transcription, "Transcription" },
		#endregion
		#region TranslatePanel
		{ Element.EnableTranslatePanel, "Enable Translator." },
        { Element.ShowTranslationOnDoubleClick, "Show on Double Click." },
        { Element.TranslateLanguages, "Translate languages:" },
        { Element.Translation, "Translation" },
        { Element.TextFont, "Text Font:" },
        { Element.TitleFont, "Title Font:" },
        { Element.Direct, "Direct(faster/can: 429 Too Many)" },
        { Element.WebScript, "Web Script(slower/stable)" },
        { Element.DirectV2, "Direct-v2(faster/more stable)" },
        { Element.Method, "Method" }, 
		#endregion
		#region Updates
		{ Element.CheckForUpdates, "Check for updates" },
        { Element.YouHaveLatest, "You have latest version." },
        { Element.TimeToUpdate, "I think it is time to update." },
        { Element.UpdateSwitcher, "<- Update to new version" },
        { Element.DownloadUpdate, "Download update" },
        { Element.ProxyConfig, "Proxy configuration" },
        { Element.ProxyServer, "Server:Port" },
        { Element.ProxyLogin, "Login:" },
        { Element.ProxyPass, "Password:" },
        { Element.Error, "Error..." },
        { Element.NetError, "Connection to github.com can't be established, check your network connection or proxy settings..." },
        { Element.UpdatesChannel, "Updates channel:" },
		#endregion
		#region About
		{ Element.DbgInf, "Debug info" },
        { Element.DbgInf_Copied, "Copied!" },
        { Element.Site, "Site" },
        { Element.Releases, "Releases" },
        { Element.About, "" },
		#endregion
		#region Sync
		{ Element.Backup, "Backup" },
        { Element.Restore, "Restore" },
        { Element.TooBig, " probably too big..." },
        { Element.CannotBe, "cannot be" },
        { Element.UnknownID, "Unknown ID." },
        { Element.EnterID, "Please enter ID." },
		#endregion
		#region Sounds
		{ Element.EnableSounds, "Enable sounds." },
        { Element.PlaySoundWhen, "Play sound when:" },
        { Element.SoundOnAutoSwitch, "On Auto-Switch conversion." },
        { Element.SoundOnSnippets, "On Snippets expansion." },
        { Element.SoundOnConvertLast, "On Last Word conversion." },
        { Element.SoundOnLayoutSwitching, "Layout switching." },
        { Element.UseCustomSound, "Use custom sound:" },
        { Element.Select, "Select" }, 
		#endregion
		#region Misc
		{ Element.Not, "Not" },
        { Element.Transliterate, "Transliterate" },
        { Element.Convert, "Convert" },
        { Element.Latest, "Latest" },
        { Element.Clipboard, "Clipboard" },
        { Element.ChangeLayout, "Change layout" },
        { Element.Exist, "exist" },
        { Element.Checking, "Checking..." },
        { Element.Keys, "Keys" },
        { Element.Key_Left, "Left" },
        { Element.Key_Right, "Right" },
        { Element.Layouts, "Layouts" },
        { Element.Plugin, "plugin" },
        { Element.Layout, "Layout" },
        { Element.Hotkey, "Hotkey" },
        { Element.UpdateFound, "New version available!" },
        { Element.UpdateComplete, "Successfully updated!" },
        { Element.ShowHide, "Show" },
        { Element.Switcher, "Faine Switch " + Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version },
        { Element.Download, "Download" },
        { Element.ConfigsCannot, "Configs file *.ini cannot be " },
        { Element.Created, "created" },
        { Element.Readen, "readen" },
        { Element.RetryInAppData, "Retry creating/switching to configs in %AppData%?" },
        { Element.Sound, "Sound" },
        { Element.InputHistoryBackSpaceWriteType1, "Write <Back>(faster)" },
        { Element.InputHistoryBackSpaceWriteType2, "Delete last" },
        { Element.Disabled, "Disabled" },
        { Element.Enable, "Enable" },
        { Element.Open, "Open" },
		#endregion
		#region Buttons
		{ Element.ButtonOK, "OK" },
        { Element.ButtonApply, "Apply" },
        { Element.ButtonCancel, "Cancel" }, 
		#endregion
		#region Tooltips
		{ Element.TT_SwitchBetween, "While this option is disabled, [Convert word], [Convert line] and [Convert selection with \"Convert selection layout switching\" enabled]\n" +
                                          "will just cycle between all locales instead of switching between the selected ones in settings."+
                                          "If there is a program in which [Convert word], [Convert line] or [Convert selection with \"CS-Switch\" enabled] don't work,try with this option enabled.\nThere is also possible now to enable this function with Emulate layout switching, that will fix problems with apps like MSOffice2016.\nThis function is not working in console apps without getconkbl.dll." },
        { Element.TT_ConvertSelectionSwitch, "This function can add layout switching to convert selection,\nnow it can be used together with [Use layout for whole word in CS],\nin that case it will cause layout switching before conversion.\nConversion mode will be used from [Use layout for whole word in CS] function.\n+ Mode can't be enabled while function [Use layout for whole word in CS] enabled.\n\nCan cause bugs!!\r\nIf enabled, Convert selection will use layout switching.\nAll symbols will be written as the must(if layout before switching was the one where they are written it).\nThere also a plus version of that function." },
        { Element.TT_BlockCtrl, "Blocks hotkeys that use Control,\nwhen [Switch layout by key] is set to Left/Right Ctrl." },
        { Element.TT_CapsDis, "If enabled, timer which disables CapsLock(led) will work.\r\nDelay can be configured in [Timings] tab." },
        { Element.TT_EmulateLS, "If enabled, layout switching will emulate press of keys selected on right.\nNow it is possible to enable this function with [Switch between layouts] function." },
        { Element.TT_RePress, "If enabled, modifiers(Ctrl/Alt/Shift/Win) will be pressed again conversion(NOT recommended),\r\n"+
                "although if you release modifiers before conversion action finishes - modifiers may stuck...))." },
        { Element.TT_Add1Space, "If enabled, space will be adding to last word." },
        { Element.TT_Add1NL, "If enabled, ONE Enter(line break) will be adding to last word,\r\n\r\nso the convert last word will work if you accidentally press Enter after the word." },
        { Element.TT_ReSelect, "If enabled, any [Convert selected] will select text again after conversion.\nYou can also specify which hotkeys are being affected in [Hidden] tab, default value is: tTRSULCNB for all hotkeys that use selection" },
        { Element.TT_ScrollTip, "Highlight Scroll Lock when active language 1, selected in Layouts tab.\nUnnesesary to keep enabled [Switch between layouts] function enabled for this function to work, just select layout #1 below it and then disable it if you need to." },
        { Element.TT_LDOnlyOnChange, "Display language tooltip only on layout change.\nDisplay time - 2×[Refresh rate for mouse + for caret]." },
        { Element.TT_ConvertSelectionSwitchPlus, "Can cause bugs!!\r\nCombines some abilities of Convert selection with enabled [Convert selection layout switching] and when it's disabled." +
                                        "\nIt can:"+
                                        "\n1.Convert text from different layouts to different layouts at once."+
                                        "\n2.Ignore symbols feature work in it."+
                                        "\n3.Auto get layout of text (symbols, that exist in both layouts are not supported)."+
                                        "\n4.Convert unsupported symbols differently, if you change layout before conversion." },
        { Element.TT_LDForMouse, "If enabled, when hovering text form with, a language tooltip will be displayed around the mouse." },
        { Element.TT_LDForCaret, "If enabled, a language tooltip will be displayed around the caret." },
        { Element.TT_Snippets, "If enabled, pressing SPACE will expand small (which starts with \"->\") word, to big (which is between \"====>\" and \"<====\") word/text fragment." },
        { Element.TT_Logging, "Designed ONLY to search for errors, BIG PERFORMANCE IMPACT, logs are saved in folder, in folder Logs." },
        { Element.TT_LDDifferentAppearence, "If enabled, you can select different appearance for main layouts(1&2), for others will be used from [around mouse] or [around caret]." },
        { Element.TT_TrayDisplayType, "Allows to choose what to display in tray icon.\nIf [Layout text] is selected, its appearance will be same as in Appearance tab's [Layout 1] and [Layout 2] appearance, and [around caret]/[around mouse] for any other layout.\nAlso it is possible to use flags for any other layout, just enable [Use flags] in [around caret]/[around mouse]." },
        { Element.TT_SymbolIgnore, "If enabled, symbols []{};':\"./<>? will be ignored.\nWorks in Convert last word, line, selection.\n" +
                                        "WON'T WORK IF YOU HAVE MORE THAN 2 LAYOUTS AND FUNCTION \"Switch between layouts\" disabled!" },
        { Element.TT_ConvertWords, "Allow to convert specific last word count by pressing hotkey and then 0-9 (0 = 10) on keyboard." },
        { Element.TT_ExcludedPrograms, "Programs(excluded) in which convert hotkeys won't work.\nSeparators - spaces and new lines.\r\nIf process name has spaces in it replace it with _, if process name has the _ just write it so.\r\nExample: Process Name: foo_bar 2000.exe\r\nIn : foo_bar_2000.exe." },
        { Element.TT_MCDSSupport, "Add the ability to display language tooltip around caret in Sublime Text 3.\nFor it to work you need to install a plugin, link on right.\nSettings available in appearance tab:\nTop: Your ST3 titlebar + tab bar height,\nBottom: Your y pixels to ST3 console edit box(ctrl+`).\nFor different windows/themes settings will be different!" },
        { Element.TT_LDText, "Leave empty for auto-detect.\r\nEnter Alt+255(Numpad) to disable displaying of this layout, when display flags feature active." },
        { Element.TT_OneLayoutWholeWordCS, "Use one layout for whole word in Convert Selection,\r\n"+
                "this feature uses quantity of rightly recognized chars IN ALL layouts to indicate layout of whole word,"+
                "\r\nthis feature works PERFECTLY with words that have symbols around them, but word length must be greater that 1 char for this feature to work properly." },
        { Element.TT_PersistentLayout, "Write here process names in which you want to have persistent layout, separators are spaces and new lines.\r\nIf process name has spaces in it replace it with _, if process name has the _ just write it so.\r\nExample: Process Name: foo_bar 2000.exe\r\nIn Faine Switcher: foo_bar_2000.exe."},
        { Element.TT_SwitchOnlyOnWindowChange, "Enabling this will disable timers and persistent layout will update only when active window changes(Windows message).\r\nConsumes CPU only when changing windows, i.e. way less CPU usage than timers."},
        { Element.TT_SwitchOnlyOnce, "If enabled switching layout will be only once per window,\r\ne.g. it will switch window's layout on activation if its process name matches selected only once.\r\nList of windows for which layout already been changed clears when clicked on Apply or OK."},
        { Element.TT_OneLayout, "Allows to store global layout in Faine Switcher, instead of layout per window/program.\r\n(if You have Windows 8 or greater this feature is built in Windows, so you don't need to use enable it in Faine Switcher)"},
        { Element.TT_QWERTZ, "Makes right substitutes in QWERTZ keyboards for chars: ß, ä, ö, ü, Ä, Ö, Ü, Y, Z in Convert Selection\r\n(!! but convert selection layout switching(or +) not supported)." },
        { Element.TT_Change1KeyLayoutInExcluded, "Function is in Layouts tab -> [Change to specific layout by keys]." },
        { Element.TT_AllowConvertSWL, "Hotkeys affected: -> \r\n[Convert Selection]\r\n[Convert Last Word]\r\n[Convert Last Line]." },
        { Element.TT_SnippetsSwitchToGuessLayout, "Switches to *guessed* layout after snippet expanded.\r\nGuess works like in whole [One Layout for whole word in Convert Selection] function."},
        { Element.TT_SnippetsCount, "If ORANGE snippets are OK.\r\nIf RED snippets has errors, maybe its unfinished etc.\r\nIn brackets are displayed count of commented lines(they are ignored by Faine Switcher),\r\nvalid comment characters: # and // and only at start of line." },
        { Element.TT_GuessKeyCodeFix, "Enabling this will make snippets, convert selection, auto-switch to send real virtual key codes instead of unicode chars,\r\nbut that will cause that all characters will be in your current layout.\r\nUseful in programs virtual machines.(BlueStacks, VirtualBox etc.)" },
        { Element.TT_ConfigsInAppData, "If enabled Faine Switcher will copy current configs to AppData, and will use them.\r\nAlso logs and snippets will be stored in %AppData%\\Faine Switcher.\r\nAfter this checkbox state changed other checkboxes/datas configurations from user interface will not be saved, because they will be loaded from another configs(if switched, from Faine Switcher's directory or from %AppData%\\Faine Switcher).\r\nUseful if you need to run Faine Switcher from Program Files directory by multiple users, and while some of them have no write access to it,\r\nalso it makes possible to have different configurations for each user." },
        { Element.TT_KeysType, "Select which keys type to display in Faine Switcher user interface, they are both working at same time,\r\nso try not to set same keys/hotkeys to avoid double layout switching."},
        { Element.TT_SnippetExpandKey, "Select custom snippet expand key,\r\nworks only for snippets, auto-switch will still expand only on space." },
        { Element.TT_LDUseWinMessages, "If enabled, timers will not be used to update language tooltips,\r\ninstead they will be updated on appropriate Windows Messages.\r\nLess CPU hungry than timers.\r\nMost CPU hungry is mouse tooltip with always enabled,\r\nconsumes CPU only on mouse move/clicks,\r\nto decrease its CPU usage, there will be 1 new config in Timings tab.\r\nSkip x Windows Messages(mouse movement) before updating tooltip." },
        { Element.TT_RemapCapslockAsF18, "Remaps Caps Lock as F18, after this CapsLock will be disabled.\r\nTo toggle its state use Ctrl/Alt/Shift/Win + Caps Lock key.\r\n! Faine Switcher window excluded from remap!\r\nIn Faine Switcher you should set hotkeys as Caps Lock key, in other programs they will be remapped as F18.\r\nAfter changing hotkey don't forget to press Apply or OK." },
        { Element.TT_UseJKL, "Allows Faine Switcher to retrieve layout from jkl.exe.\r\nAllows always to get right keyboard layout.(Highly recommended)\r\n" },
        { Element.TT_ReadOnlyNA, "Switches to read-only mode when no write access to Faine Switcher.ini.\r\nUseful for Administrators." },
        { Element.TT_WriteInputHistory, "Writes input to history.txt(in Faine Switcher folder or in AppData, depends on configuration),\nwith support of <Backspace> key!, though it may be slow if history too big." },
        { Element.TT_UseDelayAfterBackspaces, "If enabled Faine Switcher will wait some time after deleting old word and before inputting converted word.\r\nUseful if in some apps Faine Switcher's function to convert last word doesn't work properly." },
        { Element.TT_ShowSelectionTranslationHotkey, "Enable the translator in [Translator] tab in order this hotkey to work." },
        { Element.TT_LeftRightMB, "Left button - open file.\nRight button - open directory." },
        { Element.TT_CycleCase, "Cycle the selected text words case by:\r\nTitle => UPPER => lower => RanDoM => sWAP => Title => ..." },
        { Element.TT_CustomConversion, "Converts selected text according to rules in CustomConversion.txt" },
        { Element.TT_Transcription_1, "Works only when [" },
        { Element.TT_Transcription_2, "] method is active. Not all languages have it." },
        { Element.TT_SnippetsEditHotkeys, "  Hotkeys:\r\n[Ctrl]+[K] or [Ctrl]+[/] = comment/uncomment line.\r\n     - If more than 1 line is selected, all selected lines will be commented/uncommented." },
        { Element.TT_LCTRLLALTTempLayout, @"Temporarily changes layout while you hold combination of keys containing: LCtrl+LAlt, until release of both.
Fixes problem when instead of firing hotkeys containing Ctrl+Alt in apps Windows prints special character, which usually typed via AltGr(right Alt, in Windows AltGr = Ctrl+Alt).
Relevant for extended and typographic layouts: US-International, UK-Extended, European languages layouts etc.
You need to specify in text box to the right code of layout in which Faine Switcher will be switching, choose layout that doesn't have AltGr combinations: for example English(US) 67699721.
You can even specify layout which is not loaded in system, for example: English(US) 67699721, Faine Switcher will load/unload it when pressing/releasing LCtrl+LAlt.
Specify layout code 0 to disable this feature." },
		#endregion
		#region Messages
		{ Element.MSG_SnippetsError, "Snippets contains error in syntax, check if there are errors, details on snippets syntax you can find on Wiki." },
		#endregion
        #region Share_The_Settings
        { Element.Share_The_Settings_Export, "Export" },
        { Element.Share_The_Settings_Import, "Import" },
        { Element.Share_The_Setting_Info, "This page is designed for transferring program settings" +
            "\n from one user to another." +
"\nBy pressing the Export key, you will be able to save the program settings" +
            "\n file in the computer's directory." +
"\nPressing the Import key will provide the option to read the settings file" +
            "\n and apply it as the program's configuration." }
        #endregion
	};

    /// <summary>
    /// Ukrainian language for SwitcherUI.
    /// </summary>
    public static Dictionary<Element, string> Ukrainian = new Dictionary<Element, string>() {
		#region Tabs
		{ Element.tab_Functions, "Функції" },
        { Element.tab_Layouts, "Розкладки" },
        { Element.tab_Appearence, "Вигляд" },
        { Element.tab_Timings, "Таймінги" },
        { Element.tab_Excluded, "Виключення" },
        { Element.tab_Snippets, "Сніппети" },
        { Element.tab_AutoSwitch, "Автозаміна" },
        { Element.tab_Hotkeys, "Гарячі клавіші" },
        { Element.tab_LangPanel, "Мовна панель" },
        { Element.tab_Updates, "Оновлення" },
        { Element.tab_About, "Про..." },
        { Element.tab_Sounds, "Звуки" },
        { Element.tab_Translator, "Перекладач" },
        { Element.tab_Sync, "Синхронізація" },
        { Element.tab_Share_The_Settings, "Поділіться налаштуваннями" },
		#endregion
		#region Functions
		{ Element.AutoStart, "Запускати з Windows" },
        { Element.CreateTask, "Створити завдання (від Адміністратора)."},
        { Element.CreateShortcut, "Створити ярлик в папці «Автозавантаження»."},
        { Element.TrayIcon, "Показувати значок у треї." },
        { Element.ConvertSelectionLS, "Зміна розкладки у Конвертації виділення." },
        { Element.ReSelect, "Виділяти знову текст після конвертації." },
        { Element.RePress, "Натискати знову модифікатори гарячих клавіш." },
        { Element.Add1Space, "Вважати Пробіл частиною останнього слова." },
        { Element.Add1NL, "Вважати Enter частиною останнього слова." },
        { Element.ConvertSelectionLSPlus, "Зміна розкладки у Конвертації виділення+." },
        { Element.HighlightScroll, "Увімкнути Scroll Lock, коли активна розкладка #1." },
        { Element.UpdatesCheck, "Перевіряти оновлення при запуску." },
        { Element.SilentUpdate, "Тихо оновлювати при запуску." },
        { Element.Logging, "Увімкнути журналювання дій." },
        { Element.CapsTimer, "Увімкнути таймер-вимикач Caps Lock." },
        { Element.DisplayInTray, "Показувати у треї:" },
        { Element.JustIcon, "Значок Faine Switcher" },
        { Element.ContryFlags, "Прапори країн" },
        { Element.TextLayout, "Текст розкладки" },
        { Element.BlockCtrlHKs, "Блокувати гарячі клавіші, що містять Ctrl." },
        { Element.MCDSSupport, "Увімкнути підтримку MCDS." },
        { Element.OneLayoutWholeWord, "Вважати розкладку для всього слова в КВ." },
        { Element.GuessKeyCodeFix, "Виправлення кодів клавіш." },
        { Element.ConfigsInAppData, "Конфігурація в AppData." },
        { Element.RemapCapslockAsF18, "Перевизначити Caps Lock як F18." },
        { Element.UseJKL, "Отримувати розкладку з JKL." },
        { Element.ReadOnlyNA, "Тільки читання, якщо немає доступу." },
        { Element.WriteInputHistory, "Записувати історію вводу." },
        { Element.BackSpaceType, "Тип Backspace:" },
		#endregion
		#region Layouts
        { Element.SwitchBetween, "Перемикатися між розкладками" },
        { Element.EmulateLS, "Емулювати перемикання розкладки." },
        { Element.EmulateType, "Тип емуляції:" },
        { Element.ChangeLayoutBy1Key, "Перемикати розкладки за клавішами:" },
        { Element.QWERTZ, "Виправлення для QWERTZ клавіатур." },
        { Element.KeysType, "Тип клавіш:" },
        { Element.SelectKeyType, "Вибрати зі списку." },
        { Element.SetHotkeyType, "Вказати свою клавішу." },
        { Element.LCTRLLALTTempLayout, "Тимчасово змінювати розкладку при комбінації LCtrl+LAlt:" },
		#endregion
		#region Persistent Layout
		{ Element.PersistentLayout, "Постійна розкладка" },
        { Element.SwitchOnlyOnWindowChange, "Змінювати тільки коли змінюються вікна." },
        { Element.SwitchOnlyOnce, "Змінити 1 раз для вікна." },
        { Element.ActivatePLFP, "Постійна розкладка для процесів:" },
        { Element.CheckInterval, "Інтервал перевірки (мс):" },
        { Element.OneLayout, "Одна розкладка для всіх програм." },  
		#endregion
		#region Appearence
		{ Element.LDMouseDisplay, "Відображати підказку поточної мови поруч з мишкою." },
        { Element.LDCaretDisplay, "Відображати підказку поточної мови поруч з кареткою." },
        { Element.LDOnlyOnChange, "Тільки при зміні." },
        { Element.LDDifferentAppearence, "Використовувати різний вигляд для розкладок." },
        { Element.Language, "Мова:" },
        { Element.LDAppearence, "Вигляд підказки мови:" },
        { Element.LDAroundMouse, "Поруч з мишкою" },
        { Element.LDAroundCaret, "Поруч з кареткою" },
        { Element.LDTransparentBG, "Прозорий фон." },
        { Element.LDFont, "Шрифт" },
        { Element.LDFore, "Колір тексту:" },
        { Element.LDBack, "Колір фону:" },
        { Element.LDText, "Текст підказки:" },
        { Element.LDSize, "Розмір" },
        { Element.LDPosition, "Позиція" },
        { Element.LDWidth, "Ширина" },
        { Element.LDHeight, "Висота" },
        { Element.MCDSTopIndent, "Згори" },
        { Element.MCDSBottomIndent, "Знизу" },
        { Element.UseFlags, "Використовувати прапори." },
        { Element.Always, "Завжди." },
        { Element.LDUpperArrow, "Стрілка при верхньому регістрі." },
        { Element.LDUseWinMessages, "Використовувати Повідомлення Windows замість таймерів." },
		#endregion
		#region Timings
		{ Element.LDForMouseRefreshRate, "Швидкість оновлення підказки мови біля миші (мс):" },
        { Element.LDForCaretRefreshRate, "Швидкість оновлення підказки мови біля каретки (мс)" },
        { Element.DoubleHKDelay, "Час очікування наступного натиснення подвійних гарячих клавіш (мс):" },
        { Element.TrayFlagsRefreshRate, "Швидкість оновлення флагів в треї (мс):" },
        { Element.ScrollLockRefreshRate, "Швидкість оновлення Scroll Lock (мс):" },
        { Element.CapsLockRefreshRate, "Швидкість оновлення Caps Lock (мс):" },
        { Element.MoreTriesToGetSelectedText, "Використовувати більше спроб для взяття тексту:" },
        { Element.LD_MouseSkipMessages, "Повідомлень руху миші пропущено перед оновленням підказок:" },
        { Element.UseDelayAfterBackspaces, "Вносити затримку після видалення слова в конвертації слов (мс):" },
        { Element.UsePasteInCS, "Використовувати вставку для конвертацій виділеного." },
		#endregion
		#region Excluded
        { Element.ExcludedPrograms, "Программи-виключення:" },
        { Element.Change1KeyLayoutInExcluded, "Змінювати розкладку однієї клавіші навіть у виключеннях." },
        { Element.AllowConvertSWL, "Дозволити конвертування виділення/слова/рядка." },
        { Element.NeedSwitch, "Слова, що потрібно перемикати" },
        { Element.NotNeedSwitch, "Слова, що не потрібно перемикати" },
		#endregion
		#region Snippets
		{ Element.SnippetsEnabled, "Включить сніпети." },
        { Element.SnippetSpaceAfter, "Додати 1 пробіл після сніпетів." },
        { Element.SnippetSwitchToGuessLayout, "Перемикати на передбачувану розкладку сніпетів." },
        { Element.SnippetsCount, "Сніпетів: " },
        { Element.SnippetsExpandKey, "Клавіша розгортання:" },
        { Element.SnippetsExpKeyOther, "Інша" },
        { Element.SnippetsNCRules, "Правила скасування сніпетів/автозаміни (Regex)" },
		#region AutoSwitch
		{ Element.AutoSwitchEnabled, "Включити автозаміну." },
        { Element.AutoSwitchSpaceAfter, "Додавати 1 пробіл після автозаміни." },
        { Element.AutoSwitchSwitchToGuessLayout, "Перемикати на передбачувану розкладку автозаміни." },
        { Element.AutoSwitchUpdateDictionary, "Оновити словник." },
        { Element.AutoSwitchDependsOnSnippets, "Щоб використовувати цю функцію, ввімкніть функцію Сніпетів!" },
        { Element.AutoSwitchDictionaryWordsCount, "Слів: " },
        { Element.DownloadAutoSwitchDictionaryInZip, "Завантажити словник автозаміни у zip-архіві." },
        { Element.AutoSwitchDictionaryTooBigToDisplay, "Словник занадто великий, відведе багато часу на його відображення, тому він вимкнений.\r\nСловник НЕ буде оновлюватися кожного разу при натисканні кнопки «Застосувати», тільки один раз при запуску або при вимкненні функції Автозаміни=>Застосувати=>ввімкнення=>Застосувати, або через гарячу клавішу „[Hidden] Toggle AutoSwitch Hotkey“, також тільки один раз." },
		#endregion
		#endregion
		#region Hotkeys
		{ Element.ToggleMainWnd, "Перемкнути видимість головного вікна" },
        { Element.ConvertLast, "Конвертація останнього слова" },
        { Element.ConvertSelected, "Конвертація виділеного тексту" },
        { Element.ConvertLine, "Конвертація останньої лінії" },
        { Element.ConvertWords, "Конвертація декількох слів" },
        { Element.ToggleSymbolIgnore, "Перемкнути ігнорування символів" },
        { Element.SelectedToTitleCase, "Виділені слова в Заголовний регістр" },
        { Element.SelectedToRandomCase, "Виділені слова в Випадковий регістр" },
        { Element.SelectedToSwapCase, "Виділені слова в зворотний регістр" },
        { Element.SelectedToUpperCase, "Виділені слова в ВЕРХНІЙ РЕГІСТР" },
        { Element.SelectedToLowerCase, "Виділені слова в нижній регістр" },
        { Element.SelectedTransliteration, "Транслітерація виділеного тексту" },
        { Element.ExitSwitcher, "Вихід" },
        { Element.RestartSwitcher, "Перезапустити" },
        { Element.Enabled, "Включена" },
        { Element.DoubleHK, "Подвійна гаряча клавіша" },
        { Element.ToggleLangPanel, "Перемкнути видимість панелі мови" },
        { Element.TranslateSelection, "Показати переклад виділеного тексту" },
        { Element.ToggleSwitcher, "Перемкнути паузу Faine Switcher" },
        { Element.CycleCase, "Циклічне перемикання регістру" },
        { Element.CustomConversion, "Особливе змінення тексту" },
        { Element.ShowCMenuUnderMouse, "Показати меню трея поруч з мишкою" },
		#endregion
		#region LangPanel
		{ Element.DisplayLangPanel, "Показати мовну панель." },
        { Element.RefreshRate, "Швидкість оновлення (мс):" },
        { Element.Transparency, "Прозорість,%:" },
        { Element.BorderColor, "Колір рамки:" },
        { Element.UseAeroColor, "Використовувати Aero/Головний колір." },
        { Element.DisplayUpperArrow, "Показувати стрілку при наборі в верхньому регістрі." },
        { Element.Transcription, "Транскрипція" },
		#endregion
		#region TranslatePanel
		{ Element.EnableTranslatePanel, "Включити перекладач." },
        { Element.ShowTranslationOnDoubleClick, "Показувати при подвійному кліку." },
        { Element.TranslateLanguages, "Мови перекладу:" },
        { Element.Translation, "Переклад" },
        { Element.TextFont, "Шрифт тексту:" },
        { Element.TitleFont, "Шрифт заголовку:" },
        { Element.Direct, "Напряму (швидше|може:429 Too Many)" },
        { Element.WebScript, "Веб-скрипт (повільніше|без помилок)" },
        { Element.DirectV2, "Напряму-v2(швидше|стабільніше)" },
        { Element.Method, "Метод" },
		#endregion
		#region Updates
		{ Element.CheckForUpdates, "Перевірити наявність оновлень" },
        { Element.Checking, "Перевіряю..." },
        { Element.YouHaveLatest, "У вас остання версія." },
        { Element.TimeToUpdate, "Думаю, пора оновитися." },
        { Element.UpdateSwitcher, "<- Оновити Faine Switcher" },
        { Element.DownloadUpdate, "Завантажити оновлення" },
        { Element.ProxyConfig, "Конфігурація проксі" },
        { Element.ProxyServer, "Сервер:порт" },
        { Element.ProxyLogin, "Логін:" },
        { Element.ProxyPass, "Пароль:" },
        { Element.Error, "Помилка..." },
        { Element.NetError, "З'єднання з github.com не може бути встановлено, " +
            "перевірте підключення до Інтернету або ваші налаштування проксі..."},
        { Element.UpdatesChannel, "Канал оновлень:" },
		#endregion
		#region About
		 { Element.DbgInf, "Дебагінгова інформація" },
        { Element.DbgInf_Copied, "Скопійовано!" },
        { Element.Site, "Сайт" },
        { Element.Releases, "Релізи" },
        { Element.About, "Горячі клавіші: Ви можете подивитися їх в вкладці гарячі клавіші.\r\n"+
            "\r\n*Зауважте: якщо ви вводите текст не з обраних розкладках у налаштуваннях, Конвертація конвертує текст в Мову 1 (Неактуально, якщо увімкнено Цикліч. режим).\r\n\r\n"+
            "**Якщо у вас проблеми з символами при Конвертації виділення, увімкніть функцію «Зчитувати розкладку для всього слова в КВ» (рекомендується), ще можете спробувати перемкнути мови місцями (1=>2 і 2=>1) або увімкнути «Зміна розкладки в конвертації виділення/+».\r\n"+
            "***Якщо у вас проблеми при Конвертації виділення, спробуйте збільшити кількість спроб взяття тексту в вкладці «Таймінги»." +
            "\r\nПочитайте wiki або запитайте мене, якщо у вас є питання щодо Faine Switcher (ел. пошта і посилання на wiki нижче).\r\nУдачі."},
		#endregion
		#region Sync
		{ Element.Backup, "Резервне копіювання" },
        { Element.Restore, "Відновлення" },
        { Element.TooBig, " можливо занадто великий..." },
        { Element.CannotBe, "не може бути" },
        { Element.UnknownID, "Невідомий ID." },
        { Element.EnterID, "Будь ласка, введіть ID." },
		#endregion
		#region Sounds
        { Element.EnableSounds, "Увімкнути звуки." },
        { Element.PlaySoundWhen, "Відтворювати при:" },
        { Element.SoundOnAutoSwitch, "конвертації автозаміни." },
        { Element.SoundOnSnippets, "розкритті сніппетів." },
        { Element.SoundOnConvertLast, "конвертації останнього слова." },
        { Element.SoundOnLayoutSwitching, "зміні розкладки." },
        { Element.UseCustomSound, "Свій звук:" },
        { Element.Select, "Вибрати" },
		#endregion
		#region Misc
		{ Element.Not, "Ні"},
        { Element.Transliterate, "Транслітерація" },
        { Element.Convert, "Конвертація" },
        { Element.Latest, "Останні" },
        { Element.Clipboard, "Буфер обміну" },
        { Element.ChangeLayout, "Перемкнути розкладку" },
        { Element.Exist, "існує" },
        { Element.Keys, "Клавіші" },
        { Element.Key_Left, "Лівий" },
        { Element.Key_Right, "Правий" },
        { Element.Layouts, "Розкладки" },
        { Element.Layout, "Розкладка" },
        { Element.Plugin, "плагін" },
        { Element.Hotkey, "Гаряча клавіша" },
        { Element.UpdateFound, "Доступна нова версія!" },
        { Element.UpdateComplete, "Faine Switcher успішно оновлено!" },
        { Element.ShowHide, "Показати" },
        { Element.Switcher, $"Faine Switcher {Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version} – вбудований перемикач розкладок." },
        { Element.Download, "Завантажити" },
        { Element.ConfigsCannot, "Файл налаштувань Faine Switcher.ini не може бути " },
        { Element.Created, "створений" },
        { Element.Readen, "прочитаний" },
        { Element.RetryInAppData, "Спробувати створити/перемкнутися на налаштування в %AppData%?" },
        { Element.Sound, "Звук" },
        { Element.InputHistoryBackSpaceWriteType1, "Писати <Back>(швидше)" },
        { Element.InputHistoryBackSpaceWriteType2, "Видаляти символ" },
        { Element.Disabled, "Відключено" },
        { Element.Enable, "Увімкнути" },
        { Element.Open, "Відкрити" },
		#endregion
		#region Buttons
		{ Element.ButtonOK, "ОК" },
        { Element.ButtonApply, "Застосувати" },
        { Element.ButtonCancel, "Скасувати" }, 
		#endregion
		#region Tooltips
		{ Element.TT_SwitchBetween, "Поки ввімкнено, і «Конвертація слова», і «Конвертація лінії», і «Конвертація виділення з «Зміна розкладки в Конвертації виділення» включеною]\n" +
                 "буде перемикати розкладку циклічно замість перемикання між вибраними в налаштуваннях.\n" +
                 "Якщо є програма, з якою «Конвертація слова», «Конвертація лінії» або «Конвертація виділення з включеною «Зміна розкладки в Конвертації виділення» не працює, то спробуйте включити цю функцію.\nТакож тепер можна включати цю функцію *разом* з емуляцією перемикання розкладки,\nце вирішує проблеми в програмах типу MS Office 2016, функція не працює в консольних додатках без getconbl.dll." },
        { Element.TT_ConvertSelectionSwitch, "Цю функцію можна ввімкнути, щоб відбувалася зміна розкладки при Конвертації виділення.\nТепер можна ввімкнути разом з функцією «Враховувати розкладку для всього слова у КВ»,\nу цьому випадку буде проводитися зміна розкладки перед конвертацією.\nСам режим конвертації буде використовуватися з «Враховувати розкладку для всього слова у КВ».\n+Режим неможливо ввімкнути, якщо активна функція «Враховувати розкладку для всього слова у КВ»\n\nМоже викликати проблеми, якщо використовується без «Враховувати розкладку для всього слова у КВ»!!\r\nЯкщо ввімкнено, Конвертація виділення буде змінювати розкладку після конвертації.\nВсі символи будуть надруковані з Faine Switcher правильно, якщо перед перемиканням стояла розкладка, в якій вони були написані користувачем.\nТакож є поліпшення функції – «Зміна розкладки в Конвертації виділення+»." },
        { Element.TT_BlockCtrl, "Блокує гарячі клавіші, які містять Control,\nможе знадобитися, якщо «Перемикати мову клавішею» встановлено на ліву/праву клавішу Ctrl." },
        { Element.TT_CapsDis, "Якщо включено, то буде працювати таймер, який вимикає індикатор Caps Lock (світлодіод).\r\nЗатримку можна налаштувати в вкладці «Таймінги»." },
        { Element.TT_EmulateLS, "Якщо включено, Faine Switcher буде емулювати вибрані клавіші (в списку «Тип емуляції» справа) для зміни розкладки.\nСлід вибирати таке комбінацію клавіш, яка використовується в системі для зміни мови введення.\nЦю функцію тепер можна включати разом із функцією «Перемикатися між розкладками»." },
        { Element.TT_RePress, "Якщо включено, то модифікатори (Ctrl/Alt/Shift/Win) будуть натиснуті знову після дії гарячої клавіші (НЕ рекомендується),\r\n"+
                              "хоча якщо ви відпустите модифікатори до того, як завершиться дія конвертації, вони можуть залипнути...)." },
        { Element.TT_Add1Space, "Якщо увімкнено, то ОДИН пробіл буде додаватися до останнього слова." },
        { Element.TT_Add1NL, "Якщо увімкнено, то до останнього введеного слова буде додаватися ОДИН перехід на новий рядок,\r\nщо дозволяє конвертувати останнє слово, якщо ви натиснули Enter після слова не навмисно." },
        { Element.TT_ReSelect, "Якщо увімкнено, будь-які «Конвертації виділення» будуть виділяти текст знову.\nТакож можна вказати для яких гарячих клавіш спрацьовує ефект у вкладці [Прихований], за замовчуванням для всіх гарячих клавіш, що використовують виділення: tTRSULCNB." },
        { Element.TT_ScrollTip, "Підсвічувати лампочку Scroll Lock, коли активна розкладка #1, вибрана во вкладці «Розкладки».\nНеобов'язково залишати увімкненою функцію «Перемикатися між розкладками», достатньо просто вибрати розкладку #1 нижче її." },
        { Element.TT_LDOnlyOnChange, "Відображати підказку мови тільки при зміні розкладки.\nЧас відображення - 2×[Швидкість оновлення біля каретки + біля миші]." },
        { Element.TT_ConvertSelectionSwitchPlus,"Може викликати проблеми!!\r\nОб'єднує можливості Конвертація виділення з увімкненим «Зміна розкладки при Конвертації виділення» і коли вона вимкнена." +
                                        "\nМожливості:"+
                                        "\n1.Конвертувати текст з різних мов на різні мови за одну конвертацію."+
                                        "\n2.Ігнорування символів тут працює."+
                                        "\n3.Автоматичне розпізнавання розкладки тексту (символи, що є в обох розкладках, не підтримуються)"+
                                        "\n4.Конвертувати не підтримувані символи по-різному, якщо змінювати розкладку перед конвертацією." },
                { Element.TT_LDForMouse, "Якщо включено, то при наведенні курсора миші на текстове поле буде відображатися підказка мови." },
        { Element.TT_LDForCaret, "Якщо включено, то поруч активного (текстового) вказівника буде відображатися підказка мови." },
        { Element.TT_Snippets, "Якщо включено, натиснення ПРОБІЛА збільшить маленьке слово (яке має суфікс «->») у великий кусок тексту (який між «====>» і «<====»)." },
        { Element.TT_Logging, "Призначено ТІЛЬКИ для пошуку помилок, впливає БІЛЬШЕ НІЖ НА ШВИДКІСТЬ РОБОТИ, журнали зберігаються у папці [Logs] поруч з Faine Switcher.exe." },
        { Element.TT_LDDifferentAppearence, "Якщо включено, то ви зможете вибрати різний вигляд для двох розкладок (1&2), для інших будуть використовуватися стандартні з „поруч з мишею“ або „поруч з кареткою“." },
        { Element.TT_TrayDisplayType, "Дозволяє вибирати, що відображати в значку трею (в області повідомлень).\nЯкщо вибрано «Текст розкладки», його вигляд буде різним для кожної з двох розкладок: «Розкладка 1» і «Розкладка 2» в вкладці Вигляд, для інших розкладок буде використовуватися вигляд з „поруч з курсором“ або „поруч з мишею“.\nТакож можна налаштувати використання *прапорців* для інших розкладок, якщо увімкнути «Використовувати прапорці» в вкладці Вигляд -> «поруч з курсором»/«поруч з мишею»." },
        { Element.TT_SymbolIgnore, "Якщо увімкнено, символи []{};':\"./<>? будуть проігноровані.\nПрацює в Конвертації слів, ліній, виділень.\n" +
                                        "НЕ БУДЕ ПРАЦЮВАТИ, якщо у вас більше 2 розкладок і функція «Перемикатися між розкладками» вимкнена!" },
        { Element.TT_ConvertWords, "Дає можливість конвертувати специфічну кількість останніх слів, після гарячої клавіші натисніть 0-9 (0 = 10) на клавіатурі." },
        { Element.TT_ExcludedPrograms, "Програми (виключення), в яких гарячі клавіші Конвертації/Зміни розкладки не будуть працювати.\nРозділювачі – пробіли і нові рядки.\r\nЯкщо в іменах процесів є Пробіл, замінюйте його на _ , сам _ також можна замінювати на _ .\r\nНаприклад: Ім'я процесу: mon_hun online.exe\r\nУ Faine Switcher: mon_hun_online.exe." },
        { Element.TT_MCDSSupport, "Дає можливість відображення підказки поточної мови поруч каретки в Sublime Text 3.\nДля його роботи потрібно встановити плагін, посилання справа.\nНалаштування в вкладці Вид.\nЗверху: Висота заголовка вікна + висота панелі вкладок ST3,\nЗнизу: Ваші Y пікселів від кінця вікна до форми введення консолі ST3(ctrl+`).\nДля різних Windows/тем потрібні різні налаштування!" },
        { Element.TT_LDText, "Залиште порожнім для автовизначення.\r\nВведіть Alt+255 (на цифровому блоці) для відключення показу підказки в цій розкладці при активній функції відображення флагів." },
        { Element.TT_OneLayoutWholeWordCS, "Визначити та використовувати одну розкладку для всіх символів кожного цілого слова (пробіл розділяє) в Конвертації виділення,\r\n"+
                "ця функція аналізує кількість правильно розпізнаних літер у ВСІХ розкладках, щоб визначити розкладку слова,\r\n"+
                "ця функція ПРЕКРАСНО працює зі словами, які мають поруч символи, але довжина слова має бути більше 1 (не рахуючи символи), щоб вона нормально працювала." },
        { Element.TT_PersistentLayout, "Напишіть тут назви процесів, в яких ви б бажали мати постійну розкладку, роздільник – пробіл або нова стрічка.\r\nЯкщо в іменах процесів є Пробіл, замінюйте його на _ , сам _ теж можна замінювати на _ .\r\nНаприклад: Ім'я процесу: mon_hun online.exe\r\nУ Faine Switcher: mon_hun_online.exe."},
        { Element.TT_SwitchOnlyOnWindowChange, "Якщо увімкнено, то функція постійної розкладки буде оновлюватися не за допомогою таймерів, а лише при зміні вікон (Повідомлення Windows).\r\nВикористовує ресурси ЦП лише при зміні вікон, у результаті навантаження НАЗАВЖДИ менше, ніж з таймерами."},
        { Element.TT_SwitchOnlyOnce, "Якщо включено, перемикання розкладки буде відбуватися тільки ОДИН раз для кожного вікна обраних процесів.\r\nСписок вікон, для яких розкладка вже була змінена, очищається при натисканні на «Застосувати» або «ОК»."},
        { Element.TT_OneLayout, "Дозволяє зберігати розкладку в Faine Switcher замість розкладки для кожного вікна/програми.\r\n(якщо у вас Windows 8 і вище, то там дана функція вже стоїть за замовчуванням, немає необхідності включати її ще і в Faine Switcher)"},
        { Element.TT_QWERTZ, "Робить правильні заміни на німецьких (QWERTZ) клавіатурах для літер: ß, ä, ö, ü, Ä, Ö, Ü, Y, Z в Конвертації виділення\r\n(!! але не сумісно з «Cміною розкладки в Конвертації виділення/+»)." },
        { Element.TT_Change1KeyLayoutInExcluded, "Функція знаходиться в вкладці Розкладки -> «Перемикати розкладки за клавішами»." },
        { Element.TT_AllowConvertSWL, "Впливає на гарячі клавіші: -> \r\n«Конвертація виділеного тексту»\r\n«Конвертація останнього слова»\r\n«Конвертація останньої лінії»." },
        { Element.TT_SnippetsSwitchToGuessLayout, "Змінює розкладку на *угадану* після того, як сніппет сконвертувався.\r\nВгадування працює так само, як у функції «Одна розкладка для всього слова в Конвертації виділення»."},
        { Element.TT_SnippetsCount, "Якщо колір ПОМАРАНЧЕВИЙ, то зі сніппетами все ОК.\r\nЯкщо ЧЕРВОНИЙ, то в сніппетах є помилка, вони можуть не запрацювати і т.д.\r\nУ дужках відображається кількість закоментованих рядків (вони ігноруються Faine Switcher),\r\ndопустимі символи для коментування рядків: # або // – і тільки в початку рядка." },
        { Element.TT_GuessKeyCodeFix, "Включивши це, Сніппети, Конвертація виділення, Автозаміна будуть надсилати реальні коди клавіш замість символів Юнікода,\r\nале це зробить так, що всі символи будуть в вашій поточній розкладці.\r\nКорисно в додатках-віртуальних машинах (BlueStacks, VirtualBox і інших)." },
        { Element.TT_ConfigsInAppData, "Якщо включено, Faine Switcher скопіює поточну конфігурацію (Faine Switcher.ini) в AppData, і буде використовувати звідти.\r\nТакож логи і сніппети будуть зберігатися в %AppData%\\Faine Switcher.\r\nКоли стан цієї галочки змінено, всі інші галочки/дані конфігурації, змінені в користувальницькому інтерфейсі, не будуть збережені,\r\nт.к. відбудеться зміна конфігурації і завантаження обраної (із папки Faine Switcher або з %AppData%\\Faine Switcher).\r\nКорисно, якщо потрібно запускати Faine Switcher в папці Program Files, а прав на запис у всіх користувачів - немає,\r\nі ще це дає можливість мати різні налаштування для кожного користувача." },
        { Element.TT_KeysType, "Виберіть, який тип клавіш відображати в Faine Switcher, ОБИДВА типи працюють ОДНОЧАСНО,\r\nтак що краще не призначати однакові клавіші/гор. клавіші для уникнення подвійного перемикання розкладки."},
        { Element.TT_SnippetExpandKey, "Виберіть, якою клавішею розгортати (збільшувати/перетворювати) сніппети,\r\nпрацює лише для сніппетів, автозаміна, як і раніше, буде працювати лише при Пробілі." },
        { Element.TT_LDUseWinMessages, "Якщо включено, підказки будуть оновлюватися не через таймери,\r\nзамість цього вони задіяні відповідні Повідомлення Windows.\r\nВитрачає менше ресурсів ЦП.\r\nНайбільш требована до ЦП функція - підказка поруч з мишею, включена «Завжди»,\r\nнавантажує ЦП лише при русі/кліках миші,\r\nдля зменшення витрат нею ресурсів ЦП є нова конфігурація в вкладці Тайминги.\r\nПропуск х Повідомлень Windows (руху миші) перед оновленням підказки." },
        { Element.TT_RemapCapslockAsF18, "Перезаписує клавішу Caps Lock як F18, після чого функція клавіши Caps Lock буде вимкнена.\r\nЩоб перемкнути стан Caps Lock, натисніть її разом з однією з клавіш-модифікаторів: Ctrl/Alt/Shift/Win.\r\n! Вікно Faine Switcher виключено з перезапису!\r\nУ Faine Switcher призначайте гор. клавіші на Caps Lock, в інших програмах вона буде перезаписана як F18.\r\nПісля зміни гор. клавіш не забудьте натиснути на «Застосувати» чи «ОК»." },
        { Element.TT_UseJKL, "Отримує розкладку для Faine Switcher за допомогою jkl.exe.\r\nДозволяє завжди відображати правильну розкладку (Строго рекомендується включити).\r\n" },
        { Element.TT_ReadOnlyNA, "Перемикається в режим «тільки читання», коли немає прав на запис Faine Switcher.ini.\r\nКорисно для Адміністраторів." },
        { Element.TT_WriteInputHistory, "Буде писати історію введення в history.txt (в папці Faine Switcher або в AppData – в залежності від налаштувань),\nз підтримкою клавіші <Backspace>!, хоча може працювати повільно, якщо історія занадто велика." },
        { Element.TT_UseDelayAfterBackspaces, "Якщо включено, Faine Switcher буде чекати деякий час після видалення старого слова і перед введенням конвертованого слова.\r\nКорисно, якщо в деяких програмах функція Faine Switcher «Конвертація останнього слова» не працює нормально." },
        { Element.TT_ShowSelectionTranslationHotkey, "Увімкніть перекладача на вкладці «Перекладач», щоб ця гаряча клавіша працювала." },
        { Element.TT_LeftRightMB, "Ліва кнопка миші – відкрити файл.\nПрава кнопка – відкрити папку." },
        { Element.TT_CycleCase, "По циклу перемикає регістр виділених слів, у порядку:\r\nЗаголовний => ВЕРХНІЙ => нижній => випАДковий => ОБРАТНИЙ => Заголовний => ..." },
        { Element.TT_CustomConversion, "Змінює виділений текст за заданими в CustomConversion.txt правилами" },
        { Element.TT_Transcription_1, "Працює лише при [" },
        { Element.TT_Transcription_2, "] активному методі. Підтримується не всіми мовами." },
        { Element.TT_SnippetsEditHotkeys, "  Гарячі клавіші:\r\n[Ctrl]+[K](Л) або [Ctrl]+[/](.) = закоментувати/розкоментувати лінію.\r\n     - Якщо виділено більше однієї лінії, всі виділені лінії будуть закоментовані/розкоментовані." },
        { Element.TT_LCTRLLALTTempLayout, @"Замінює розкладку у момент набору комбінацій клавіш, що починається з LCtrl+LAlt, до моменту відпускання обох.
Розв'язує проблему, коли замість комбінацій Ctrl+Alt у програмах друкується спецсимвол, набирається з AltGr(правий Alt, в Windows AltGr = Ctrl+Alt) та аналогічною клавішею.
Актуально для розширених та типографських розкладок: US-International, UK-Extended, розкладки європейських мов та ін.
У текстовому полі праворуч вводиться код розкладки без виділеної клавіші AltGr, на яку слід перемикатися: наприклад, English (US) = 67699721.
Можна вказати навіть ту розкладку, яка не завантажена в системі, наприклад English (US) = 67699721; Faine Switcher завантажить/вивантажить її при натисканні/відпусканні LCtrl+LAlt.
Вкажіть код розкладки 0, щоб вимкнути цю функцію." },
		#endregion
		#region Messages
		{ Element.MSG_SnippetsError, "Сніппети містять помилки у синтаксисі, перевірте правильність написання, деталі синтаксису можна знайти на Wiki." },
		#endregion
        #region Share_The_Settings
        { Element.Share_The_Settings_Export, "Експорт" },
        { Element.Share_The_Settings_Import, "Імпорт" },
       { Element.Share_The_Setting_Info, "Ця сторінка призначення для передачі налаштувань програми " +
            "\nвід одного користувача до іншого. " +
            "\nПри натискання клавіші Експорту буде надана можливість зберегти файл налаштувань " +
            "\nпрограми в дерикторії на компютері. " +
            "\nПри натисканні клавіші Імпорту буде надана можливість зчитати файл налаштувань і " +
            "\nзадіяти його як налаштування для програми. " }
        #endregion
	};

    /// <summary>
    /// Russian language for SwitcherUI.
    /// </summary>
    public static Dictionary<Element, string> Russian = new Dictionary<Element, string>() {
		#region Tabs
		{ Element.tab_Functions, "Функции" },
        { Element.tab_Layouts, "Раскладки" },
        { Element.tab_Appearence, "Вид" },
        { Element.tab_Timings, "Тайминги" },
        { Element.tab_Excluded, "Исключения" },
        { Element.tab_Snippets, "Сниппеты" },
        { Element.tab_AutoSwitch, "Автозамена" },
        { Element.tab_Hotkeys, "Горячие клавиши" },
        { Element.tab_LangPanel, "Языковая панель" },
        { Element.tab_Updates, "Обновления" },
        { Element.tab_About, "О..." },
        { Element.tab_Sounds, "Звуки" },
        { Element.tab_Translator, "Переводчик" },
        { Element.tab_Sync, "Синхронизация" }, 
		#endregion
		#region Functions
		{ Element.AutoStart, "Запускать с Windows" },
        { Element.CreateTask, "Создать задачу (от Администратора)."},
        { Element.CreateShortcut, "Создать ярлык в папке «Автозагрузка»."},
        { Element.TrayIcon, "Показывать значок в трее." },
        { Element.ConvertSelectionLS, "Смена раскладки в Конвертации выделения." },
        { Element.ReSelect, "Выделять заново текст после конвертации." },
        { Element.RePress, "Нажимать снова модификаторы горячих клавиш." },
        { Element.Add1Space, "Считать Пробел частью последнего слова." },
        { Element.Add1NL, "Считать Enter частью последнего слова." },
        { Element.ConvertSelectionLSPlus, "Смена раскладки в Конвертации выделения+." },
        { Element.HighlightScroll, "Включать Scroll Lock, когда активна раскладка #1." },
        { Element.UpdatesCheck, "Проверять обновления при запуске." },
        { Element.SilentUpdate, "Тихо обновлять при запуске." },
        { Element.Logging, "Включить журналирование действий." },
        { Element.CapsTimer, "Включить таймер-выключатель Caps Lock." },
        { Element.DisplayInTray, "Показывать в трее:" },
        { Element.JustIcon, "Значок Faine Switcher" },
        { Element.ContryFlags, "Флаги стран" },
        { Element.TextLayout, "Текст раскладки" },
        { Element.BlockCtrlHKs, "Блокировать горячие клавиши, содержащие Ctrl." },
        { Element.MCDSSupport, "Включить поддержку MCDS." },
        { Element.OneLayoutWholeWord, "Считать раскладку для всего слова в КВ." },
        { Element.GuessKeyCodeFix, "Исправление кодов клавиш." },
        { Element.ConfigsInAppData, "Конфигурация в AppData." },
        { Element.RemapCapslockAsF18, "Переопределить Caps Lock как F18." },
        { Element.UseJKL, "Получать раскладку с JKL." },
        { Element.ReadOnlyNA, "Только чтение, если нет доступа." },
        { Element.WriteInputHistory, "Записывать историю ввода." },
        { Element.BackSpaceType, "Тип Backspace:" },
		#endregion
		#region Layouts
		{ Element.SwitchBetween, "Переключаться между раскладками" },
        { Element.EmulateLS, "Эмулировать переключение раскладки." },
        { Element.EmulateType, "Тип эмуляции:" },
        { Element.ChangeLayoutBy1Key, "Переключать раскладки по клавишам:" },
        { Element.QWERTZ, "Исправление для QWERTZ клавиатур." },
        { Element.KeysType, "Тип клавиш:" },
        { Element.SelectKeyType, "Выбрать из списка." },
        { Element.SetHotkeyType, "Указать свою клавишу." },
        { Element.LCTRLLALTTempLayout, "Временно изменять раскладку при комбинации LCtrl+LAlt:" },
		#endregion
		#region Persistent Layout
		{ Element.PersistentLayout, "Постоянная раскладка" },
        { Element.SwitchOnlyOnWindowChange, "Менять только когда меняются окна." },
        { Element.SwitchOnlyOnce, "Менять 1 раз для окна." },
        { Element.ActivatePLFP, "Постоянная раскладка для процессов:" },
        { Element.CheckInterval, "Интервал проверки (мс):" },
        { Element.OneLayout, "Единая раскладка для всех программ." }, 
		#endregion
		#region Appearence
		{ Element.LDMouseDisplay, "Отображать подсказку текущего языка рядом с мышью." },
        { Element.LDCaretDisplay, "Отображать подсказку текущего языка рядом с кареткой." },
        { Element.LDOnlyOnChange, "Только при смене." },
        { Element.LDDifferentAppearence, "Использовать разный вид для раскладок." },
        { Element.Language, "Язык:" },
        { Element.LDAppearence, "Вид подсказки языка:" },
        { Element.LDAroundMouse, "Возле мыши" },
        { Element.LDAroundCaret, "Возле каретки" },
        { Element.LDTransparentBG, "Прозрачный фон." },
        { Element.LDFont, "Шрифт" },
        { Element.LDFore, "Цвет текста:" },
        { Element.LDBack, "Цвет фона:" },
        { Element.LDText, "Текст подсказки:" },
        { Element.LDSize, "Размер" },
        { Element.LDPosition, "Позиция" },
        { Element.LDWidth, "Ширина" },
        { Element.LDHeight, "Высота" },
        { Element.MCDSTopIndent, "Сверху" },
        { Element.MCDSBottomIndent, "Снизу" },
        { Element.UseFlags, "Использовать флаги." },
        { Element.Always, "Всегда." },
        { Element.LDUpperArrow, "Стрелка при верхнем регистре." },
        { Element.LDUseWinMessages, "Использовать Сообщения Windows вместо таймеров." },
		#endregion
		#region Timings
		{ Element.LDForMouseRefreshRate, "Скорость обновления подсказки языка возле мыши (мс):" },
        { Element.LDForCaretRefreshRate, "Скорость обновления подсказки языка возле каретки (мс)" },
        { Element.DoubleHKDelay, "Время ожидания следующего нажатия двойных горячих клавиш (мс):" },
        { Element.TrayFlagsRefreshRate, "Скорость обновления флагов в трее (мс):" },
        { Element.ScrollLockRefreshRate, "Скорость обновления Scroll Lock (мс):" },
        { Element.CapsLockRefreshRate, "Скорость обновления Caps Lock (мс):" },
        { Element.MoreTriesToGetSelectedText, "Использовать больше попыток взятия текста:" },
        { Element.LD_MouseSkipMessages, "Сообщений движения мыши пропускается перед обновлением подсказок:" },
        { Element.UseDelayAfterBackspaces, "Вносить задержку после удаления слова в конвертации слов (мс):" },
        { Element.UsePasteInCS, "Использовать вставку для конвертаций выделенного." },
		#endregion
		#region Excluded
		{ Element.ExcludedPrograms, "Программы-исключения:" },
        { Element.Change1KeyLayoutInExcluded, "Менять раскладку одной клавишей даже в исключениях." },
        { Element.AllowConvertSWL, "Разрешить конвертацию выделения/слова/линии." }, 
		#endregion
		#region Snippets
		{ Element.SnippetsEnabled, "Включить сниппеты." },
        { Element.SnippetSpaceAfter, "Добавлять 1 пробел после сниппетов." },
        { Element.SnippetSwitchToGuessLayout, "Переключать на предполагаемую раскладку сниппетов." },
        { Element.SnippetsCount, "Сниппетов: " },
        { Element.SnippetsExpandKey, "Клавиша развертывания:" },
        { Element.SnippetsExpKeyOther, "Другая" },
        { Element.SnippetsNCRules, "Правила отмены сниппетов/автозамены (Regex)" }, 
		#region AutoSwitch
		{ Element.AutoSwitchEnabled, "Включить автозамену." },
        { Element.AutoSwitchSpaceAfter, "Добавлять 1 пробел после автозамены." },
        { Element.AutoSwitchSwitchToGuessLayout, "Переключать на предполагаемую раскладку автозамены." },
        { Element.AutoSwitchUpdateDictionary, "Обновить словарь." },
        { Element.AutoSwitchDependsOnSnippets, "Чтобы использовать эту функцию, включите функцию Сниппетов!" },
        { Element.AutoSwitchDictionaryWordsCount, "Слов: " },
        { Element.DownloadAutoSwitchDictionaryInZip, "Скачивать словарь автозамены в zip-архиве." },
        { Element.AutoSwitchDictionaryTooBigToDisplay, "Словарь слишком большой, уйдет много времени на его отображение, поэтому оно отключено.\r\nСловарь НЕ будет обновляться каждый раз по нажатию кнопки «Применить», только один раз при запуске или при выключении функции Автозамены=>Применить=>включении=>Применить, или через горячую клавишу „[Hidden] Toggle AutoSwitch Hotkey“, тоже только один раз." }, 
		#endregion
		#endregion
		#region Hotkeys
		{ Element.ToggleMainWnd, "Переключить видимость главного окна" },
        { Element.ConvertLast, "Конвертация последнего слова" },
        { Element.ConvertSelected, "Конвертация выделенного текста" },
        { Element.ConvertLine, "Конвертация последней линии" },
        { Element.ConvertWords, "Конвертация нескольких слов" },
        { Element.ToggleSymbolIgnore, "Переключить игнорирование символов" },
        { Element.SelectedToTitleCase, "Выделенные слова в Заглавный регистр" },
        { Element.SelectedToRandomCase, "Выделенные слова в СЛУчАйнЫй регистр" },
        { Element.SelectedToSwapCase, "Выделенные слова в оБРАТНЫЙ регистр" },
        { Element.SelectedToUpperCase, "Выделенные слова в ВЕРХНИЙ РЕГИСТР" },
        { Element.SelectedToLowerCase, "Выделенные слова в нижний регистр" },
        { Element.SelectedTransliteration, "Транслитерация выделенного текста" },
        { Element.ExitSwitcher, "Выход" },
        { Element.RestartSwitcher, "Перезапустить" },
        { Element.Enabled, "Включена" },
        { Element.DoubleHK, "Двойная горячая клавиша" },
        { Element.ToggleLangPanel, "Переключить видимость панели языка" },
        { Element.TranslateSelection, "Показать перевод выделенного текста" },
        { Element.ToggleSwitcher, "Переключать паузу Faine Switcher" },
        { Element.CycleCase, "Цикличное переключение регистра" },
        { Element.CustomConversion, "Особое изменение текста" },
        { Element.ShowCMenuUnderMouse, "Показать меню трея рядом с мышью" }, 
		#endregion
		#region LangPanel
		{ Element.DisplayLangPanel, "Отображать языковую панель." },
        { Element.RefreshRate, "Скорость обновления (мс):" },
        { Element.Transparency, "Прозрачность,%:" },
        { Element.BorderColor, "Цвет рамки:" },
        { Element.UseAeroColor, "Использовать Aero/Главный цвет." },
        { Element.DisplayUpperArrow, "Отображать стрелку при наборе в верхнем регистре." },
        { Element.Transcription, "Транскрипция" },
		#endregion
		#region TranslatePanel
		{ Element.EnableTranslatePanel, "Включить переводчик." },
        { Element.ShowTranslationOnDoubleClick, "Показывать при двойном клике." },
        { Element.TranslateLanguages, "Языки перевода:" },
        { Element.Translation, "Перевод" },
        { Element.TextFont, "Шрифт текста:" },
        { Element.TitleFont, "Шрифт заголовка:" },
        { Element.Direct, "Напрямую (быстрее|может:429 Too Many)" },
        { Element.WebScript, "Веб-скрипт (медленнее|без ошибок)" },
        { Element.DirectV2, "Напрямую-v2(быстрее|стабильнее)" },
        { Element.Method, "Метод" }, 
		#endregion
		#region Updates
		{ Element.CheckForUpdates, "Проверить наличие обновлений" },
        { Element.Checking, "Проверяю..." },
        { Element.YouHaveLatest, "У вас последняя версия." },
        { Element.TimeToUpdate, "Думаю, пора обновиться." },
        { Element.UpdateSwitcher, "<- Обновить Faine Switcher" },
        { Element.DownloadUpdate, "Скачать обновление" },
        { Element.ProxyConfig, "Конфигурация прокси" },
        { Element.ProxyServer, "Сервер:порт" },
        { Element.ProxyLogin, "Логин:" },
        { Element.ProxyPass, "Пароль:" },
        { Element.Error, "Ошибка..." },
        { Element.NetError, "Соединение с github.com не может быть установлено, " +
            "проверьте подключение к Интернету или ваши настройки прокси..."},
        { Element.UpdatesChannel, "Канал обновлений:" },
		#endregion
		#region About
		{ Element.DbgInf, "Отладочная информация" },
        { Element.DbgInf_Copied, "Скопировано!" },
        { Element.Site, "Сайт" },
        { Element.Releases, "Релизы" },
        { Element.About, "Горячие клавиши: Вы можете посмотреть их во вкладке горячие клавиши.\r\n"+
            "\r\n*Заметьте: если вы вводите текст не из выбранных раскладок в настройках, Конвертация конвертирует текст в Язык 1 (Неактуально, если включён Циклич. режим).\r\n\r\n"+
            "**Если у вас проблемы с символами при Конвертации выделения, включите функцию «Считать раскладку для всего слова в КВ» (рекомендуется), еще можете попробовать переключить языки местами (1=>2 и 2=>1) или включить «Смена раскладки в конвертации выделения/+».\r\n"+
            "***Если у вас проблемы при Конвертации выделения, попробуйте увеличить количество попыток взятия текста во вкладке «Тайминги»." +
            "\r\nПочитайте wiki или спросите меня, если у вас есть возникли вопросы по поводу Faine Switcher (эл. почта и ссылка на wiki ниже).\r\nУдачи."}, 
		#endregion
		#region Sync
		{ Element.Backup, "Копирование" },
        { Element.Restore, "Восстановление" },
        { Element.TooBig, " возможно слишком большой..." },
        { Element.CannotBe, "не может быть" },
        { Element.UnknownID, "Неизвестный ID." },
        { Element.EnterID, "Пожалуйста, введите ID." },
		#endregion
		#region Sounds
		{ Element.EnableSounds, "Включить звуки." },
        { Element.PlaySoundWhen, "Воспроизводить при:" },
        { Element.SoundOnAutoSwitch, "конвертации автозамены." },
        { Element.SoundOnSnippets, "развертывании сниппетов." },
        { Element.SoundOnConvertLast, "конвертации последнего слова." },
        { Element.SoundOnLayoutSwitching, "смене раскладки." },
        { Element.UseCustomSound, "Свой звук:" },
        { Element.Select, "Выбрать" }, 
		#endregion
		#region Misc
		{ Element.Not, "Не"},
        { Element.Transliterate, "Транслитерация" },
        { Element.Convert, "Конвертация" },
        { Element.Latest, "Последние" },
        { Element.Clipboard, "Буфер обмена" },
        { Element.ChangeLayout, "Переключить раскладку" },
        { Element.Exist, "существует" },
        { Element.Keys, "Клавиши" },
        { Element.Key_Left, "Левый" },
        { Element.Key_Right, "Правый" },
        { Element.Layouts, "Раскладки" },
        { Element.Layout, "Раскладка" },
        { Element.Plugin, "плагин" },
        { Element.Hotkey, "Горячая клавиша" },
        { Element.UpdateFound, "Доступна новая версия!" },
        { Element.UpdateComplete, "Faine Switcher успешно обновлен!" },
        { Element.ShowHide, "Показать" },
        { Element.Switcher, $"Faine Switcher {Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version} – волшебный переключатель раскладок." },
        { Element.Download, "Скачать" },
        { Element.ConfigsCannot, "Файл настроек Faine Switcher.ini не может быть " },
        { Element.Created, "создан" },
        { Element.Readen, "прочитан" },
        { Element.RetryInAppData, "Попробовать создать/переключиться на настройки в %AppData%?" },
        { Element.Sound, "Звук" },
        { Element.InputHistoryBackSpaceWriteType1, "Писать <Back>(быстрее)" },
        { Element.InputHistoryBackSpaceWriteType2, "Удалять символ" },
        { Element.Disabled, "Отключен" },
        { Element.Enable, "Включить" },
        { Element.Open, "Открыть" },
		#endregion
		#region Buttons
		{ Element.ButtonOK, "ОК" },
        { Element.ButtonApply, "Применить" },
        { Element.ButtonCancel, "Отмена" }, 
		#endregion
		#region Tooltips
		{ Element.TT_SwitchBetween, "Пока включена, и «Конвертация слова», и «Конвертация линии», и «Конвертация выделения с „Смена раскладки в Конвертации выделения“ включённой]\n" +
                                          "будет переключать раскладку циклично вместо переключения между выбранными в настройках." +
                                          "Если есть программа, с которой «Конвертация слова», «Конвертация линии» или «Конвертация выделения с включённой „Смена раскладки в Конвертации выделения“» не работают, то попробуйте включить эту функцию.\nТакже теперь можно включать эту функцию *вместе* с эмуляцией переключения раскладки,\nэто решает проблемы в программах типа MS Office 2016, функция не работает в консольных приложениях без getconbl.dll." },
        { Element.TT_ConvertSelectionSwitch, "Эту функцию можно включать, чтобы происходила смена раскладки при Конвертации выделения.\nТеперь можно включать вместе с функцией «Считать раскладку для всего слова в КВ»,\nв этом случае будет производиться смена раскладки перед конвертацией.\nСам режим конвертации будет использоваться из «Считать раскладку для всего слова в КВ».\n+Режим невозможно включить, если активна функция «Считать раскладку для всего слова в КВ»\n\nМожет вызвать проблемы, если используется без «Считать раскладку для всего слова в КВ»!!\r\nЕсли включена, Конвертация выделения будет менять раскладку после конвертации.\nВсе символы будут напечатаны с Faine Switcher правильно, если перед переключением стояла раскладка, в которой они были написаны пользователем.\nТакже есть улучшение функции – «Смена раскладки в Конвертации выделения+»." },
        { Element.TT_BlockCtrl, "Блокирует горячие клавиши, содержащие Control,\nможет пригодиться, если «Переключать язык клавишей» установлен на левую/правую клавишу Ctrl." },
        { Element.TT_CapsDis, "Если включено, то будет работать таймер, который выключает индикатор Caps Lock (светодиод).\r\nЗадержку можно настроить во вкладке «Тайминги»." },
        { Element.TT_EmulateLS, "Если включено, Faine Switcher будет эмулировать выбранные клавиши (в списке «Тип эмуляции» справа) для смены раскладки.\nСледует выбирать то сочетание клавиш, которое используется в системе для смены языка ввода.\nЭту функцию теперь можно включать вместе с функцией «Переключаться между раскладками»." },
        { Element.TT_RePress, "Если включено, то модификаторы (Ctrl/Alt/Shift/Win) будут нажаты заново после действия горячей клавиши (НЕ рекомендуется),\r\n"+
                              "хотя если вы отпустите модификаторы до того, как завершится действие конвертации, они могут залипнуть...)." },
        { Element.TT_Add1Space, "Если включено, то ОДИН пробел будет добавляться в последнее слово." },
        { Element.TT_Add1NL, "Если включено, то к последнему введенному слову будет добавляться ОДИН перевод строки,\r\nблагодаря чему конвертация последнего слова будет работать, если вы случайно нажали Enter после слова." },
        { Element.TT_ReSelect, "Если включено, любые «Конвертации выделения» будут выделять текст заново.\nМожно также указать для каких горячих клавиш распространяется эффект во вкладке [Hidden], по-умолчанию для всех гор. клавиш использующих выделение: tTRSULCNB." },
        { Element.TT_ScrollTip, "Подсвечивать лампочку Scroll Lock, когда активна раскладка #1, выбранная во вкладке «Раскладки».\nНеобязательно оставлять включенным функцию «Переключаться между раскладками», достаточно просто выбрать раскладку #1 ниже неё." },
        { Element.TT_LDOnlyOnChange, "Отображать подсказку языка только при смене раскладки.\nВремя отображения - 2×[Скорость обновления возле каретки + возле мыши]." },
        { Element.TT_ConvertSelectionSwitchPlus, "Может вызвать проблемы!!\r\nСовмещает способности Конвертация выделения с включенным «Смена раскладки в Конвертации выделения» и когда она выключена." +
                                        "\nВозможности:"+
                                        "\n1.Конвертировать текст с разных языков на разные языки за одну конвертацию."+
                                        "\n2.Игнорирование символов здесь работает."+
                                        "\n3.Автораспознавание раскладки текста (символы, которые есть в обеих раскладках, не поддерживаются)"+
                                        "\n4.Конвертировать не поддерживаемые символы по-разному, если менять раскладку перед конвертацией." },
        { Element.TT_LDForMouse, "Если включена, то при наведении курсора мыши на текстовое поле будет отображаться подсказка языка." },
        { Element.TT_LDForCaret, "Если включена, то возле мигающего (текстового) указателя будет отображаться подсказка языка." },
        { Element.TT_Snippets, "Если включено, нажатие ПРОБЕЛА увеличит маленькое слово (которое имеет суффикс «->») в большой кусок текста (который между «====>» и «<====»)." },
        { Element.TT_Logging, "Предназначено ТОЛЬКО для поиска ошибок, оказывает БОЛЬШОЕ ВЛИЯНИЕ НА СКОРОСТЬ РАБОТЫ, журналы сохраняются в папке [Logs] рядом с Faine Switcher.exe." },
        { Element.TT_LDDifferentAppearence, "Если включено, то вы сможете выбрать разный вид для двух раскладок (1&2), для других будут использоваться стандартные из „возле мыши“ или „возле каретки“." },
        { Element.TT_TrayDisplayType, "Позволяет выбирать, что отображать в значке трея (в области уведомлений).\nЕсли выбран «Текст раскладки», его вид будет разным для каждой из двух раскладок: „Раскладка 1“ и „Раскладка 2“ во вкладке Вид, для других раскладок будет использоваться вид из „возле каретки“ или „возле мыши“.\nТакже можно настроить использование *флагов* для остальных раскладок, если включить «Использовать флаги» во вкладке Вид -> „возле каретки“/„возле мыши“." },
        { Element.TT_SymbolIgnore, "Если включено, символы []{};':\"./<>? будут проигнорированы.\nРаботает в Конвертации слова, линии, выделения.\n" +
                                        "НЕ БУДЕТ РАБОТАТЬ, если у вас больше 2 раскладок и функция «Переключаться между раскладками» выключена!" },
        { Element.TT_ConvertWords, "Дает возможность конвертировать специфическое количество последних слов, после горячей клавиши нажмите 0-9(0 = 10) на клавиатуре." },
        { Element.TT_ExcludedPrograms, "Программы (исключения), в которых горячие клавиши Конвертирования/Смены раскладки не будут работать.\nРазделители – пробелы и новые строки.\r\nЕсли в именах процессов есть Пробел, заменяйте его на _ , сам _ тоже можно заменять на _ .\r\nНапример: Имя процесса: mon_hun online.exe\r\nВ Faine Switcher: mon_hun_online.exe." },
        { Element.TT_MCDSSupport, "Дает возможность отображения подсказки текущего языка возле каретки в Sublime Text 3.\nДля его работы нужно установить плагин, ссылка справа.\nНастройки во вкладке Вид.\nСверху: Высота заголовка окна + высота панели вкладок ST3,\nСнизу: Ваши Y пиксели от конца окна до формы ввода консоли ST3(ctrl+`).\nДля разных Windows/тем потребуются разные настройки!" },
        { Element.TT_LDText, "Оставьте пустым для автоопределения.\r\nВведите Alt+255 (на цифровом блоке) для отключения показа подсказки в этой раскладке при активной функции отображении флагов." },
        { Element.TT_OneLayoutWholeWordCS, "Определять и использовать одну раскладку для всех символов каждого целого слова (пробел разделяет) в Конвертации выделения,\r\n"+
                "эта функция анализирует количество правильно распознанных букв ВО ВСЕХ раскладках, чтобы определить раскладку слова,\r\n"+
                "эта функция ПРЕКРАСНО работает с словами, которые имеют рядом символы, но длина слова должна быть больше 1 (не считая символы), чтобы она нормально работала." },
        { Element.TT_PersistentLayout, "Напишите здесь названия процессов, в которых вы бы хотели иметь постоянную раскладку, разделитель – пробел или новая строка.\r\nЕсли в именах процессах есть Пробел, заменяйте его на _ , сам _ тоже можно заменять на _ .\r\nНапример: Имя процесса: mon_hun online.exe\r\nВ Faine Switcher: mon_hun_online.exe."},
        { Element.TT_SwitchOnlyOnWindowChange, "Если включено, то функция постоянной раскладки будет обновляться не через таймеры, а только при смене окон (Сообщения Windows).\r\nПотребляет ресурсы ЦП только при смене окон, в итоге нагрузка НАМНОГО меньше, нежели с таймерами."},
        { Element.TT_SwitchOnlyOnce, "Если включено, смена раскладки будет происходить только ОДИН раз для каждого окна выбранных процессов.\r\nСписок окон, для которых раскладка уже менялась, очищается при нажатии на «Применить» или «ОК»."},
        { Element.TT_OneLayout, "Позволяет хранить раскладку в Faine Switcher вместо раскладки для каждого окна/программы.\r\n(если у вас Windows 8 и выше, то там данная функция уже стоит по умолчанию, нет необходимости включать ее ещё и в Faine Switcher)"},
        { Element.TT_QWERTZ, "Делает правильные замены на немецких (QWERTZ) клавиатурах для букв: ß, ä, ö, ü, Ä, Ö, Ü, Y, Z в Конвертации выделения\r\n(!! но не совместимо со «Cменой раскладки в Конвертации выделения/+»)." },
        { Element.TT_Change1KeyLayoutInExcluded, "Функция находится во вкладке Раскладки -> «Переключать раскладки по клавишам»." },
        { Element.TT_AllowConvertSWL, "Влияет на горячие клавиши: -> \r\n«Конвертация выделенного текста»\r\n«Конвертация последнего слова»\r\n«Конвертация последней линии»." },
        { Element.TT_SnippetsSwitchToGuessLayout, "Меняет раскладку на *угаданную* после того, как сниппет сконвертировался.\r\nУгадывание работает так же, как в функции «Одна раскладка для целого слова в Конвертации выделения»."},
        { Element.TT_SnippetsCount, "Если цвет ОРАНЖЕВЫЙ, то со сниппетами всё ОК.\r\nЕсли КРАСНЫЙ, то в сниппетах есть ошибка, они могут не сработать и т.д.\r\nВ скобках отображается количество закомментированных линий (они игнорируются Faine Switcher),\r\nдопустимые символы для комментирования линий: # или // – и только в начале строки." },
        { Element.TT_GuessKeyCodeFix, "Включив это, Сниппеты, Конвертация выделения, Автозамена будут отправлять реальные коды клавиш вместо символов Юникода,\r\nно это сделает так, что все символы будут в вашей текущей раскладке.\r\nПолезно в приложениях–виртуальных машинах (BlueStacks, VirtualBox и других)." },
        { Element.TT_ConfigsInAppData, "Если включено, Faine Switcher скопирует текущую конфигурацию (Faine Switcher.ini) в AppData, и будет использовать оттуда.\r\nТакже логи и сниппеты будут храниться в %AppData%\\Faine Switcher.\r\nКогда состояние этой галочки изменено, все другие галочки/данные конфигурации, измененные в пользовательском интерфейсе, не будут сохранены,\r\nт.к. произойдет смена конфигурации и загрузка выбранной (из папки Faine Switcher или из %AppData%\\Faine Switcher).\r\nПолезно, если нужно запускать Faine Switcher в папке Program Files, а прав на запись у всех пользователей - нет,\r\nи еще это даёт возможность иметь разные настройки для каждого пользователя." },
        { Element.TT_KeysType, "Выберите, какой тип клавиш отображать в Faine Switcher, ОБА типа работают ОДНОВРЕМЕННО,\r\nтак что лучше не назначайте одинаковые клавиши/гор. клавиши во избежание двойного переключения раскладки."},
        { Element.TT_SnippetExpandKey, "Выберите, какой клавишей развертывать (увеличивать/превращать) сниппеты,\r\nработает только для сниппетов, автозамена, как и раньше, будет работать только при Пробеле." },
        { Element.TT_LDUseWinMessages, "Если включено, подсказки будут обновляться не через таймеры,\r\nвместо этого они задействуют соответствующие Сообщения Windows.\r\nПотребляет меньше ресуров ЦП.\r\nСамая требовательная к ЦП функция – подсказка возле мыши, включенная «Всегда»,\r\nнагружает ЦП только при движении/кликах мыши,\r\nдля снижения потребления ею ресурсов ЦП есть новая конфигурация во вкладке Тайминги.\r\nПропуск х Сообщений Windows (движения мыши) перед обновлением подсказки." },
        { Element.TT_RemapCapslockAsF18, "Переопределяет клавишу Caps Lock как F18, после чего функция клавиши Caps Lock будет отключена.\r\nЧтобы переключить состояние Caps Lock, нажмите её вместе с одной из клавиш-модификаторов: Ctrl/Alt/Shift/Win.\r\n! Окно Faine Switcher исключено из переопределения!\r\nВ Faine Switcher назначайте гор. клавиши на Caps Lock, в других программах она будет переопределена как F18.\r\nПосле смены гор. клавиш не забудьте нажать на «Применить» или «ОК»." },
        { Element.TT_UseJKL, "Получает раскладку для Faine Switcher с помощью jkl.exe.\r\nПозволяет всегда отображать правильную раскладку (Настоятельно рекомендуется включить).\r\n" },
        { Element.TT_ReadOnlyNA, "Переключается в режим «только чтение», когда нет прав на запись Faine Switcher.ini.\r\nПолезно для Администраторов." },
        { Element.TT_WriteInputHistory, "Будет писать историю ввода в history.txt (в папке Faine Switcher или в AppData – в зависимости от настроек),\nс поддержкой клавиши <Backspace>!, хотя может работать медленно, если история слишком большая." },
        { Element.TT_UseDelayAfterBackspaces, "Если включено, Faine Switcher будет ждать некоторое время после удаления старого слова и перед вводом конвертированного слова.\r\nПолезно, если в некоторых программах функция Faine Switcher «Конвертация последнего слова» не работает нормально." },
        { Element.TT_ShowSelectionTranslationHotkey, "Включите переводчик во вкладке «Переводчик», чтобы эта горячая клавиша работала." },
        { Element.TT_LeftRightMB, "Левая кнопка мыши – открыть файл.\nПравая кнопка – открыть папку." },
        { Element.TT_CycleCase, "По циклу переключает регистр выделенных слов, в порядке:\r\nЗаглавный => ВЕРХНИЙ => нижний => слУчАЙный => оБРАТНЫЙ => Заглавный => ..." },
        { Element.TT_CustomConversion, "Меняет выделенный текст по заданным в CustomConversion.txt правилам" },
        { Element.TT_Transcription_1, "Работает только при [" },
        { Element.TT_Transcription_2, "] активном методе. Поддерживается не всеми языками." },
        { Element.TT_SnippetsEditHotkeys, "  Горячие клавиши:\r\n[Ctrl]+[K](Л) or [Ctrl]+[/](.) = закомментировать/раскомментировать линию.\r\n     - Если выделено больше одной линии, все выделенные линии будут закомментированы/раскомментированы." },
        { Element.TT_LCTRLLALTTempLayout, @"Подменяет раскладку в момент набора комбинации клавиш, начинающейся с LCtrl+LAlt, до момента отпуска обеих.
Решает проблему, когда заместо комбинаций Ctrl+Alt в приложениях печатается спецсимвол, набираемый с AltGr(правый Alt, в Windows AltGr = Ctrl+Alt) и аналогичной клавишей.
Актуально для расширенных и типографских раскладок: US-International, UK-Extended, раскладки европейских языков и пр.
В текстовом поле справа вводится код раскладки без выделенной клавиши AltGr, на которую следует переключаться: например, English (US) = 67699721.
Можно указать даже ту раскладку, которая не загружена в системе, например English (US) = 67699721; Faine Switcher загрузит/выгрузит её при нажатии/отпускании LCtrl+LAlt.
Укажите код раскладки 0, чтобы отключить эту функцию." },
		#endregion
		#region Messages
		{ Element.MSG_SnippetsError, "Сниппеты содержат ошибки в синтаксисе, проверьте правильность написания, детали синтаксиса можете найти на Wiki." },
		#endregion
        #region Share_The_Settings
        { Element.Share_The_Settings_Export, "Експорт" },
        { Element.Share_The_Settings_Import, "Імпорт" },
         { Element.Share_The_Setting_Info, "Ця сторінка призначення для передачі налаштувань програми " +
            "\nвід одного користувача до іншого. " +
            "\nПри натискання клавіші Експорту буде надана можливість зберегти файл налаштувань " +
            "\nпрограми в дерикторії на компютері. " +
            "\nПри натисканні клавіші Імпорту буде надана можливість зчитати файл налаштувань і " +
            "\nзадіяти його як налаштування для програми. " }
        #endregion
	};
}
