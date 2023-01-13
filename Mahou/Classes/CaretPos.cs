using System;
using System.Drawing;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Mahou {
	public static class CaretPos {
		public static Point _CaretST3 = new Point(0,0);
		public static int SidebarWidth = 0, viewID = 0;
		
		public static int GetMCDSValue(string type, string input) {
			return Int32.Parse(Regex.Match(input, type + @"->(\d+)").Groups[1].Value);
		}
		public static void GetDataFromMCDS() {  
			try {
				TcpClient sock = new TcpClient();  
				sock.Connect("127.0.0.1", 7777);
				var resp = new byte[1024];
				sock.Client.Receive(resp);
				var info = Encoding.UTF8.GetString(resp, 0, resp.Length).TrimEnd(new [] { (char)0 });
				if (resp.Length > 1) {
					_CaretST3 = new Point(GetMCDSValue("C", info) * GetMCDSValue("CW", info), 
					                      GetMCDSValue("L", info) * GetMCDSValue("LH", info));
					SidebarWidth = GetMCDSValue("SBW", info);
					viewID = GetMCDSValue("VID", info);
				}
			} catch (Exception e) {  
				Logging.Log("Error during GetDataFromMCDS, details\r\b:" + e.Message + "\r\n" + e.StackTrace + "\r\n");
			}
		}
		public static bool GetGuiInfo(uint thread_id, ref WinAPI.GUITHREADINFO gui_info) {
			if (!WinAPI.GetGUIThreadInfo(thread_id, ref gui_info)) {
				Logging.Log("Error getting GUI info for thread id: [" + thread_id +
				            "], L-W32Err: [" + Marshal.GetLastWin32Error() + "].", 2);
				return false;
			}
			return true;
		}
		/// <summary>
		/// Gets caret position from focused window or from focused control in window.
		/// </summary>
		/// <returns>Point</returns>
		public static Point GetCaretPointToScreen(out Point caretOnlyPos) {
			var LuckyNone = new Point(77777,77777);
			caretOnlyPos = LuckyNone;
			var _cThr_id = WinAPI.GetCurrentThreadId();
			var _pntCR = new Point(0, 0);
			var _fwFCS_Re = new WinAPI.RECT(0, 0, 0, 0);
			var _fw = WinAPI.GetForegroundWindow();
			uint dummy = 0; 
			var _fwThr_id = WinAPI.GetWindowThreadProcessId(_fw, out dummy);
			IntPtr _fwFCS = IntPtr.Zero;
			var _clsNMb = new StringBuilder(256);
			string _clsNMfw = "";
			Logging.Log("_c HWND: [" +MMain.MAHOU_HANDLE+ "], _c ThrId: ["+_cThr_id+
			            "], "+"_fw HWND: ["+_fw+"]"+", _fw ThrId: "+_fwThr_id+".");
			if (_fwThr_id != _cThr_id) {
				var gti = new WinAPI.GUITHREADINFO();
				gti.cbSize = Marshal.SizeOf(gti);
				if (!GetGuiInfo(_fwThr_id, ref gti))
				    return LuckyNone;
				_fwFCS = gti.hwndFocus;
				WinAPI.GetClassName(_fw, _clsNMb, _clsNMb.Capacity);
				_clsNMfw = _clsNMb.ToString();
					if (_fwFCS != IntPtr.Zero && _fwFCS != _fw) {
					var _fwFCSThr_id = WinAPI.GetWindowThreadProcessId(_fwFCS, out dummy);
					var gtiFCS = new WinAPI.GUITHREADINFO();
					gtiFCS.cbSize = Marshal.SizeOf(gtiFCS);
					if (!GetGuiInfo(_fwFCSThr_id, ref gtiFCS))
					    return LuckyNone;
					Logging.Log("_fcs: ["+_fwFCS+"]."+"_fw classname = ["+_clsNMb+"], " +"_fcs thread_id = ["+_fwFCSThr_id+"].");
					WinAPI.GetClassName(_fwFCS, _clsNMb, _clsNMb.Capacity);
					Logging.Log("_fcs classname = ["+_clsNMb+"].");
					WinAPI.GetWindowRect(_fwFCS, out _fwFCS_Re);
					_pntCR = new Point(gtiFCS.rectCaret.Left, gtiFCS.rectCaret.Top);
				} else {
					_pntCR = new Point(gti.rectCaret.Left, gti.rectCaret.Top);
					WinAPI.GetWindowRect(_fw, out _fwFCS_Re);
				}
				if (_clsNMfw == "PX_WINDOW_CLASS" && MahouUI.MCDSSupport) {
					System.Threading.Tasks.Task.Factory.StartNew(GetDataFromMCDS);
					var CaretToScreen = new Point(_fwFCS_Re.Left, _fwFCS_Re.Top);
					CaretToScreen.X += _CaretST3.X + SidebarWidth + MahouUI.MCDS_Xpos_temp;
					if (viewID == 4) {
						WinAPI.RECT clts = new WinAPI.RECT(0,0,0,0);
						WinAPI.GetWindowRect(WinAPI.GetForegroundWindow(), out clts);
						CaretToScreen.Y = clts.Bottom - MahouUI.MCDS_BottomIndent_temp - 45 + MahouUI.MCDS_Ypos_temp;
						CaretToScreen.X -= 20;
					} else 
						CaretToScreen.Y += _CaretST3.Y + MahouUI.MCDS_TopIndent_temp + MahouUI.MCDS_Ypos_temp;
					caretOnlyPos = _CaretST3;
					return CaretToScreen;
				} else {
					if (_pntCR.Equals(new Point(0,0)))
						return LuckyNone;
					Logging.Log("CaretPos = x["+_pntCR.X+"], y["+_pntCR.Y+"].");	
					var _clsNM = _clsNMb.ToString().ToLower();
					System.Diagnostics.Debug.WriteLine(_clsNM);
					// Do not display caret for these classes:
					if (_clsNM.Contains("listbox") || _clsNM.Contains("button") || 
						_clsNM.Contains("checkbox") || _clsNM.Contains("combobox") || 
						_clsNM.Contains("listview") || _clsNM.Contains("pagecontrol") || 
						(_clsNM.Contains("window") && !_clsNM.Contains("mozilla")) || _clsNM.Contains("syslink") ||
						_clsNM.Contains("tree") || _clsNM.Contains("helpform") || 
						_clsNM.Contains("tmainform") || _clsNM.Contains("btn") ||  
						_clsNM.Contains("удалить") || _clsNM.Contains("delete") ||
					    _clsNM.Contains("afx:") ||
					    _clsNM == "msctls_trackbar32"|| _clsNM.Contains("wxwindow") ||
					    _clsNM == "systabcontrol32" || _clsNM == "directuihwnd" ||
					    _clsNM == "static" ||  _clsNM == "netuihwnd" || _clsNMfw == "mspaintapp" ||
					    _clsNM == "potplayer" || _clsNM == "mdiclient" || 
					    (_clsNMfw == "#32770" && _clsNM != "edit"))
						return LuckyNone;
					if (_clsNM.Contains("sharpdevelop.exe")) {
						_pntCR.Y += 28; _pntCR.X += 3;
					}
					if (_clsNM.Contains("mintty")) { _pntCR.Y += 31; _pntCR.X += 4; }
					Logging.Log("Get caret position finished successfully.", 0);
					caretOnlyPos = _pntCR;
					return new Point(_fwFCS_Re.Left + _pntCR.X, _fwFCS_Re.Top + _pntCR.Y);
				}
			}
			return LuckyNone;
		}
	}
}