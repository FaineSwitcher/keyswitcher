using System;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

/// <summary>
/// This class contains only WinAPI functions.
/// </summary>
public static class WinAPI
{
    public const int WM_PAINT = 0x000f;
    [DllImport("Imm32.dll", SetLastError = true)]
    public static extern bool ImmDisableIME(uint param1);
    [DllImport("winmm.dll")]
    public static extern uint mciSendString(
          string lpstrCommand, string lpstrReturnString, uint uReturnLength, uint hWndCallback);
    #region jklXHidServ
    public const int WM_QUIT = 0x0012;
    public const int WM_DESTROY = 0x0002;
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool DestroyWindow(IntPtr hWnd);
    #endregion
    #region RawInputForm/KMHook
    public static uint WM_INPUT = 0x00FF;
    [DllImport("user32.dll")]
    public static extern bool RegisterRawInputDevices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] RAWINPUTDEVICE[] pRawInputDevices, int uiNumDevices, int cbSize);
    [DllImport("user32.dll")]
    public static extern int GetRawInputData(IntPtr hRawInput, RawInputCommand uiCommand, out RAWINPUT pData, ref int pcbSize, int cbSizeHeader);
    /// <summary>Value type for raw input devices.</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTDEVICE
    {
        /// <summary>Top level collection Usage page for the raw input device.</summary>
        public ushort UsagePage;
        /// <summary>Top level collection Usage for the raw input device. </summary>
        public ushort Usage;
        /// <summary>Mode flag that specifies how to interpret the information provided by UsagePage and Usage.</summary>
        public RawInputDeviceFlags Flags;
        /// <summary>Handle to the target device. If NULL, it follows the keyboard focus.</summary>
        public IntPtr WindowHandle;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUT
    {
        public RAWINPUTHEADER Header;
        [StructLayout(LayoutKind.Explicit)]
        public struct data
        {
            [FieldOffset(0)]
            public RAWKEYBOARD Keyboard;
            [FieldOffset(0)]
            public RAWMOUSE Mouse;
            [FieldOffset(0)]
            public RAWHID Hid;
        }
        public data Data;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTHEADER
    {
        /// <summary>Type of device the input is coming from.</summary>
        public RawInputType Type;
        /// <summary>Size of the packet of data.</summary>
        public int Size;
        /// <summary>Handle to the device sending the data.</summary>
        public IntPtr Device;
        /// <summary>wParam from the window message.</summary>
        public IntPtr wParam;
    }
    [Flags()]
    public enum RawInputDeviceFlags
    {
        /// <summary>No flags.</summary>
        None = 0,
        /// <summary>If set, this removes the top level collection from the inclusion list. This tells the operating system to stop reading from a device which matches the top level collection.</summary>
        Remove = 0x00000001,
        /// <summary>If set, this specifies the top level collections to exclude when reading a complete usage page. This flag only affects a TLC whose usage page is already specified with PageOnly.</summary>
        Exclude = 0x00000010,
        /// <summary>If set, this specifies all devices whose top level collection is from the specified usUsagePage. Note that Usage must be zero. To exclude a particular top level collection, use Exclude.</summary>
        PageOnly = 0x00000020,
        /// <summary>If set, this prevents any devices specified by UsagePage or Usage from generating legacy messages. This is only for the mouse and keyboard.</summary>
        NoLegacy = 0x00000030,
        /// <summary>If set, this enables the caller to receive the input even when the caller is not in the foreground. Note that WindowHandle must be specified.</summary>
        InputSink = 0x00000100,
        /// <summary>If set, the mouse button click does not activate the other window.</summary>
        CaptureMouse = 0x00000200,
        /// <summary>If set, the application-defined keyboard device hotkeys are not handled. However, the system hotkeys; for example, ALT+TAB and CTRL+ALT+DEL, are still handled. By default, all keyboard hotkeys are handled. NoHotKeys can be specified even if NoLegacy is not specified and WindowHandle is NULL.</summary>
        NoHotKeys = 0x00000200,
        /// <summary>If set, application keys are handled.  NoLegacy must be specified.  Keyboard only.</summary>
        AppKeys = 0x00000400
    }
    public enum RawInputType
    {
        Mouse = 0,
        Keyboard = 1,
        HID = 2,
        Other = 3
    }
    [Flags()]
    public enum RawMouseFlags : ushort
    {
        /// <summary>Relative to the last position.</summary>
        MoveRelative = 0,
        /// <summary>Absolute positioning.</summary>
        MoveAbsolute = 1,
        /// <summary>Coordinate data is mapped to a virtual desktop.</summary>
        VirtualDesktop = 2,
        /// <summary>Attributes for the mouse have changed.</summary>
        AttributesChanged = 4
    }
    [Flags()]
    public enum RawMouseButtons : ushort
    {
        /// <summary>No button.</summary>
        None = 0,
        /// <summary>Left (button 1) down.</summary>
        LeftDown = 0x0001,
        /// <summary>Left (button 1) up.</summary>
        LeftUp = 0x0002,
        /// <summary>Right (button 2) down.</summary>
        RightDown = 0x0004,
        /// <summary>Right (button 2) up.</summary>
        RightUp = 0x0008,
        /// <summary>Middle (button 3) down.</summary>
        MiddleDown = 0x0010,
        /// <summary>Middle (button 3) up.</summary>
        MiddleUp = 0x0020,
        /// <summary>Button 4 down.</summary>
        Button4Down = 0x0040,
        /// <summary>Button 4 up.</summary>
        Button4Up = 0x0080,
        /// <summary>Button 5 down.</summary>
        Button5Down = 0x0100,
        /// <summary>Button 5 up.</summary>
        Button5Up = 0x0200,
        /// <summary>Mouse wheel moved.</summary>
        MouseWheel = 0x0400
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWMOUSE
    {
        /// <summary>
        /// The mouse state.
        /// </summary>
        public RawMouseFlags Flags;
        [StructLayout(LayoutKind.Explicit)]
        public struct data
        {
            [FieldOffset(0)]
            public uint Buttons;
            /// <summary>
            /// If the mouse wheel is moved, this will contain the delta amount.
            /// </summary>
            [FieldOffset(2)]
            public ushort ButtonData;
            /// <summary>
            /// Flags for the event.
            /// </summary>
            [FieldOffset(0)]
            public RawMouseButtons ButtonFlags;
        }
        public data Data;
        /// <summary>
        /// Raw button data.
        /// </summary>
        public uint RawButtons;
        /// <summary>
        /// The motion in the X direction. This is signed relative motion or
        /// absolute motion, depending on the value of usFlags.
        /// </summary>
        public int LastX;
        /// <summary>
        /// The motion in the Y direction. This is signed relative motion or absolute motion,
        /// depending on the value of usFlags.
        /// </summary>
        public int LastY;
        /// <summary>
        /// The device-specific additional information for the event.
        /// </summary>
        public uint ExtraInformation;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWKEYBOARD
    {
        public short MakeCode;
        public short Flags;
        public short Reserved;
        public ushort VKey;
        public uint Message;
        public UInt32 ExtraInformation;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWHID
    {
        public int SizeHid;
        public int Count;
        public IntPtr RawData;
    }
    public enum RawInputCommand
    {
        /// <summary>
        /// Get input data.
        /// </summary>
        Input = 0x10000003,
        /// <summary>
        /// Get header data.
        /// </summary>
        Header = 0x10000005
    }
    #endregion
    #region Constants
    #region KInputs requirements
    public const int INPUT_KEYBOARD = 1;
    public const uint HKL_PREV = 0;
    public const uint HKL_NEXT = 1;
    public const uint WM_INPUTLANGCHANGEREQUEST = 0x0050;
    public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
    public const uint KEYEVENTF_KEYUP = 0x0002;
    public const uint KEYEVENTF_UNICODE = 0x0004;
    public const uint KEYEVENTF_SCANCODE = 0x0008;
    #endregion
    #region NativeClipboard requirements
    public const int CF_TEXT = 1;
    public const int CF_BITMAP = 2;
    public const int CF_SYLK = 4;
    public const int CF_DIF = 5;
    public const int CF_TIFF = 6;
    public const int CF_OEMTEXT = 7;
    public const int CF_DIB = 8;
    public const int CF_DIBV5 = 17;
    public const int CF_PALETTE = 9;
    public const int CF_PENDATA = 10;
    public const int CF_RIFF = 11;
    public const int CF_WAVE = 12;
    public const int CF_UNICODETEXT = 13;
    public const uint CF_ENHMETAFILE = 14;
    public const uint CF_HDROP = 15;
    public const uint CF_LOCALE = 16;
    public const uint CF_MAX = 17;
    public const uint CF_OWNERDISPLAY = 0x80;
    public const uint CF_DSPTEXT = 0x81;
    public const uint CF_DSPBITMAP = 0x82;
    public const uint CF_DSPMETAFILEPICT = 0x83;
    public const uint CF_DSPENHMETAFILE = 0x8E;
    public const uint CF_HTMLFORMAT = 49315;
    public const uint GMEM_DDESHARE = 0x2000;
    public const uint GMEM_MOVEABLE = 0x0002;
    #endregion
    #region KMHook requirements
    public static uint WM_LBUTTONDOWN = 0x0201;
    public static uint WM_LBUTTONUP = 0x0202;
    public static uint WM_MOUSEMOVE = 0x0200;
    public static uint WM_RBUTTONDOWN = 0x0204;
    public static uint WM_RBUTTONUP = 0x0205;
    public static uint WM_MBUTTONDOWN = 0x0207;
    public static uint WM_MBUTTONUP = 0x0208;
    public static int WH_KEYBOARD_LL = 13;
    public static int WH_MOUSE_LL = 14;
    public static uint WM_KEYDOWN = 0x0100;
    public static uint WM_KEYUP = 0x0101;
    public static uint WM_SYSKEYDOWN = 0x0104;
    public static uint WM_SYSKEYUP = 0x0105;
    #endregion
    #region SwitcherUI requirements
    public static uint MOD_ALT = 0x0001;
    public static uint MOD_CONTROL = 0x0002;
    public static uint MOD_SHIFT = 0x0004;
    public static uint MOD_WIN = 0x0008;
    public static uint MOD_NO_REPEAT = 0x4000;
    public static uint WM_HOTKEY = 0x312;
    [DllImport("Shell32.dll", EntryPoint = "ExtractIconExW", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern int ExtractIconEx(string sFile, int iIndex, out IntPtr piLargeVersion, out IntPtr piSmallVersion, int amountIcons);
    #endregion
    #region LangDisplay requirements
    public const int SW_SHOWNOACTIVATE = 4;
    public const int HWND_TOPMOST = -1;
    public const uint SWP_NOACTIVATE = 0x0010;
    public const int WS_EX_TRANSPARENT = 0x20;
    public const int WS_EX_LAYERED = 0x80000;
    public const int WS_EX_TOOLWINDOW = 0x80;
    public const uint WM_MOUSEWHEEL = 0x020A;
    #endregion
    #region EventHook
    public const uint EVENT_SYSTEM_FOREGROUND = 3;
    public const uint WINEVENT_OUTOFCONTEXT = 0;
    public const uint EVENT_OBJECT_FOCUS = 0x8005;
    #endregion
    #region LangPanel
    public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int HT_CAPTION = 0x2;
    #region From JustUI
    public const int WM_NCPAINT = 0x0085;
    #endregion
    #endregion
    #endregion
    #region DLL Imports
    #region Low-level Hook
    public delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);
    #endregion
    #region ICheckings requires
    [DllImport("user32.dll")]
    public static extern bool GetCursorInfo(out CURSORINFO pci);
    #endregion
    #region KInputs requires
    [DllImport("user32.dll", SetLastError = true)]
    public static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT[] inputs, Int32 sizeOfInputStructure);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern int MapVirtualKey(uint uCode, uint uMapType);
    #endregion
    #region KMHook requires
    [DllImport("user32.dll")]
    public static extern ushort GetAsyncKeyState(int vKey);
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int extraInfo);
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool PostMessage(IntPtr hhwnd, uint msg, uint wparam, uint lparam);
    [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState,
        StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern short VkKeyScanEx(char ch, uint dwhkl);
    #endregion
    #region Locales/CaretPos requires
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetKeyboardLayout(uint WindowsThreadProcessID);
    [DllImport("user32.dll")]
    public static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);
    [DllImport("user32.dll")]
    public static extern bool UnloadKeyboardLayout(IntPtr hkl);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hwnd, IntPtr proccess);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetGUIThreadInfo(uint hTreadID, ref GUITHREADINFO lpgui);
    #endregion
    #region NativeClipboard requires 
    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    public static extern int DragQueryFile(IntPtr hDrop, uint iFile, [Out] StringBuilder lpszFile, int cch);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetClipboardData(uint uFormat);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool EmptyClipboard();
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool OpenClipboard(IntPtr hWndNewOwner);
    [DllImport("user32.dll")]
    public static extern IntPtr GetOpenClipboardWindow();
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool CloseClipboard();
    [DllImport("user32.dll")]
    public static extern uint EnumClipboardFormats(uint format);
    [DllImport("user32.dll")]
    public static extern bool IsClipboardFormatAvailable(uint format);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GlobalLock(IntPtr hMem);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GlobalFree(IntPtr hMem);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GlobalUnlock(IntPtr hMem);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GlobalSize(IntPtr hMem);
    #endregion
    #region SwitcherForm requires
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public extern static bool DestroyIcon(IntPtr handle);
    [DllImport("user32.dll")]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);
    [DllImport("user32.dll")]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd); [
        DllImport("user32.dll")]
    public static extern IntPtr WindowFromPoint(Point p);
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };
    [DllImport("shell32.dll", CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);
    [DllImport("Shell32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern int SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, SHGFI_Flag uFlags);
    [DllImport("Shell32.dll")]
    public static extern int SHGetFileInfo(IntPtr pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, SHGFI_Flag uFlags);
    public enum SHGFI_Flag : uint
    {
        SHGFI_ATTR_SPECIFIED = 0x000020000,
        SHGFI_OPENICON = 0x000000002,
        SHGFI_USEFILEATTRIBUTES = 0x000000010,
        SHGFI_ADDOVERLAYS = 0x000000020,
        SHGFI_DISPLAYNAME = 0x000000200,
        SHGFI_EXETYPE = 0x000002000,
        SHGFI_ICON = 0x000000100,
        SHGFI_ICONLOCATION = 0x000001000,
        SHGFI_LARGEICON = 0x000000000,
        SHGFI_SMALLICON = 0x000000001,
        SHGFI_SHELLICONSIZE = 0x000000004,
        SHGFI_LINKOVERLAY = 0x000008000,
        SHGFI_SYSICONINDEX = 0x000004000,
        SHGFI_TYPENAME = 0x000000400
    }
    #endregion
    #region CaretPos requires
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetCaretPos(out Point lpPoint);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetFocus();
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint GetCurrentThreadId();
    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
    #endregion
    #region LangDisplay requires
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern bool SetWindowPos(
        int hWnd,             // Window handle
        int hWndInsertAfter,  // Placement-order handle
        int X,                // Horizontal position
        int Y,                // Vertical position
        int cx,               // Width
        int cy,               // Height
        uint uFlags);         // Window positioning flags
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    #region EventHook requires
    [DllImport("user32.dll")]
    public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
                                         WinEventDelegate lpfnWinEventProc, uint idProcess,
                                         uint idThread, uint dwFlags);
    [DllImport("user32.dll")]
    public static extern bool UnhookWinEvent(IntPtr hWinEventHook);
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    #endregion
    #region LangPanel requires
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, uint lParam);
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    #region From JustUI
    [DllImport("dwmapi.dll")]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
    [DllImport("dwmapi.dll", EntryPoint = "#127", PreserveSig = false)]
    public static extern void DwmGetColorizationParameters(out DWM_COLORIZATION_PARAMS parameters);
    [DllImport("dwmapi.dll")]
    public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
    [DllImport("dwmapi.dll")]
    public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
    #endregion
    #endregion
    #endregion
    #endregion
    #region Required structs
    #region ICheckings required structs
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public Int32 x;
        public Int32 y;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct CURSORINFO
    {
        public Int32 cbSize;
        public Int32 flags;
        public IntPtr hCursor;
        public POINT ptScreenPos;
    }
    #endregion
    #region KInputs required structs
#pragma warning disable 649
    public struct INPUT
    {
        public UInt32 Type;
        public KEYBOARDMOUSEHARDWARE Data;
    }
    [StructLayout(LayoutKind.Explicit)]
    /// <summary>
    /// This is KEYBOARD-MOUSE-HARDWARE union INPUT won't work if you remove MOUSE or HARDWARE.
    /// </summary>
    public struct KEYBOARDMOUSEHARDWARE
    {
        [FieldOffset(0)]
        public KEYBDINPUT Keyboard;
        [FieldOffset(0)]
        public HARDWAREINPUT Hardware;
        [FieldOffset(0)]
        public MOUSEINPUT Mouse;
    }
    public struct KEYBDINPUT
    {
        public UInt16 Vk;
        public UInt16 Scan;
        public UInt32 Flags;
        public UInt32 Time;
        public IntPtr ExtraInfo;
    }
    public struct MOUSEINPUT
    {
        public Int32 X;
        public Int32 Y;
        public UInt32 MouseData;
        public UInt32 Flags;
        public UInt32 Time;
        public IntPtr ExtraInfo;
    }
    public struct HARDWAREINPUT
    {
        public UInt32 Msg;
        public UInt16 ParamL;
        public UInt16 ParamH;
    }
#pragma warning restore 649
    #endregion
    #region Locales/CaretPos required structs
    /// <summary>
    /// Contains x and y positions of upper-left and lower-right corners.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
        public RECT(int lt, int tp, int rt, int btm)
        {
            Left = lt; Top = tp;
            Right = rt; Bottom = btm;
        }
    }
    public struct GUITHREADINFO
    {
        public int cbSize;
        public int flags;
        public IntPtr hwndActive;
        public IntPtr hwndFocus;
        public IntPtr hwndCapture;
        public IntPtr hwndMenuOwner;
        public IntPtr hwndMoveSize;
        public IntPtr hwndCaret;
        public RECT rectCaret;
    }
    #endregion
    #region EventHook
    public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
                                             IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
    #endregion
    #region From JustUI	
    public struct MARGINS
    {
        public int lW;
        public int rW;
        public int tH;
        public int bH;
    }
    public struct DWM_COLORIZATION_PARAMS
    {
        public uint clrColor;
        public uint clrAfterGlow;
        public uint nIntensity;
        public uint clrAfterGlowBalance;
        public uint clrBlurBalance;
        public uint clrGlassReflectionIntensity;
        public bool fOpaque;
    }
    #endregion
    #endregion

}

