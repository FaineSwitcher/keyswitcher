using System;
using System.Windows.Forms;

namespace Mahou {
	public class Hotkey {
		public readonly int ID;
		public readonly uint VirtualKeyCode;
		public readonly uint Modifiers;
		public readonly bool Enabled, Double;
		/// <summary>
		/// Initializes an hotkey, modifs are ctrl(2), shift(4), alt(1), win(8).
		/// </summary>
		public Hotkey(bool enabled, uint VKCode, uint modifiers, int id, bool dble = false) {
			this.ID = id;
			this.Enabled = enabled;
			this.VirtualKeyCode = VKCode;
			this.Modifiers = modifiers;
			this.Double = dble;
		}
		/// <summary>
		/// All Mahou hotkey names and IDs.
		/// </summary>
		public enum HKID {
			ConvertLastWord,
			ConvertSelection,
			ConvertLastLine,
			ConvertMultipleWords,
			ToTitleSelection,
			ToSwapSelection,
			ToUpperSelection,
			ToLowerSelection,
			ToRandomSelection,
			TransliterateSelection,
			ToggleSymbolIgnoreMode,
			ToggleVisibility,
			Exit,
			Restart,
			ToggleLangPanel,
			ShowSelectionTranslation,
			ToggleMahou,
			CycleCase,
			CustomConversion,
			ShowCMenuUnderMouse
		}
		/// <summary>
		/// Gets all modifiers in hotkey.
		/// </summary>
		/// <param name="hkmods">Hotkey modifiers string.</param>
		public static uint GetMods(string hkmods) {
			uint MOD = 0;
			hkmods = hkmods.ToLower();
			if (hkmods.Contains("alt"))
				MOD += WinAPI.MOD_ALT;
			if (hkmods.Contains("control") || hkmods.Contains("ctrl"))
				MOD += WinAPI.MOD_CONTROL;
			if (hkmods.Contains("shift"))
				MOD += WinAPI.MOD_SHIFT;
			if( hkmods.Contains("win"))
				MOD += WinAPI.MOD_WIN;
			return MOD;
		}
		/// <summary>
		/// Get all modifiers in hotkey as string.
		/// </summary>
		/// <param name="hkmods">Hotkey modifiers hex unit.</param>
		/// <returns></returns>
		public static string GetMods(int hkmods) {
			string MOD = "";
			if (ContainsModifier(hkmods, (int)WinAPI.MOD_ALT))
				MOD += "Alt";
			if (ContainsModifier(hkmods, (int)WinAPI.MOD_CONTROL))
				MOD += "Control";
			if (ContainsModifier(hkmods, (int)WinAPI.MOD_SHIFT))
				MOD += "Shift";
			if (ContainsModifier(hkmods, (int)WinAPI.MOD_WIN))
				MOD += "Win";
			return MOD;
		}
		/// <summary>
		/// Checks if modifiers "mods" contains modifier "mod".
		/// </summary>
		/// <returns>True if "mods" contains "mod".</returns>
		public static bool ContainsModifier(int mods, int mod) {
			if (mod == WinAPI.MOD_WIN && mods >= WinAPI.MOD_WIN) {
				return true;
			}
			if (mod == WinAPI.MOD_SHIFT && (mods == WinAPI.MOD_SHIFT || 
			    mods == WinAPI.MOD_SHIFT + WinAPI.MOD_ALT ||
			    mods == WinAPI.MOD_SHIFT + WinAPI.MOD_CONTROL ||
			    mods == WinAPI.MOD_SHIFT + WinAPI.MOD_WIN || 
			    mods == WinAPI.MOD_SHIFT + WinAPI.MOD_ALT + WinAPI.MOD_CONTROL ||
			    mods == WinAPI.MOD_SHIFT + WinAPI.MOD_WIN + WinAPI.MOD_CONTROL ||
			   	mods == WinAPI.MOD_SHIFT + WinAPI.MOD_WIN + WinAPI.MOD_ALT ||
			   	mods == WinAPI.MOD_SHIFT + WinAPI.MOD_WIN + WinAPI.MOD_CONTROL + WinAPI.MOD_ALT)) {
				return true;
			}
			if (mod == WinAPI.MOD_CONTROL && (mods == WinAPI.MOD_CONTROL || 
			    mods == WinAPI.MOD_CONTROL + WinAPI.MOD_SHIFT ||
			    mods == WinAPI.MOD_CONTROL + WinAPI.MOD_ALT ||
			    mods == WinAPI.MOD_CONTROL + WinAPI.MOD_WIN || 
			    mods == WinAPI.MOD_CONTROL + WinAPI.MOD_ALT + WinAPI.MOD_SHIFT ||
			    mods == WinAPI.MOD_CONTROL + WinAPI.MOD_WIN + WinAPI.MOD_SHIFT ||
			   	mods == WinAPI.MOD_CONTROL + WinAPI.MOD_WIN + WinAPI.MOD_ALT ||
			   	mods == WinAPI.MOD_CONTROL + WinAPI.MOD_WIN + WinAPI.MOD_SHIFT + WinAPI.MOD_ALT)) {
				return true;
			}
			if (mod == WinAPI.MOD_ALT && mods % 2 != 0) {
				return true;
			}
			return false;
		}
		#region GetHash code, Equals etc.
		public override int GetHashCode() {
			return (VirtualKeyCode.GetHashCode() * Modifiers.GetHashCode()) ^ 7;
		}
		public override bool Equals(object obj) {
			var other = obj as Hotkey;
			if (other == null)
				return false;
			if (((this.VirtualKeyCode == 16 || this.VirtualKeyCode == 160 || this.VirtualKeyCode == 161) && this.Modifiers == 0 && other.Modifiers == WinAPI.MOD_SHIFT && other.VirtualKeyCode == 0) 
			 || ((other.VirtualKeyCode == 16 || other.VirtualKeyCode == 160 || other.VirtualKeyCode == 161) && other.Modifiers == 0 && this.Modifiers == WinAPI.MOD_SHIFT && this.VirtualKeyCode == 0))
				return true;
			if (((this.VirtualKeyCode == 18 || this.VirtualKeyCode == 164 || this.VirtualKeyCode == 165) && this.Modifiers == 0 && other.Modifiers == WinAPI.MOD_ALT && other.VirtualKeyCode == 0) 
			 || ((other.VirtualKeyCode == 18 || other.VirtualKeyCode == 164 || other.VirtualKeyCode == 165) && other.Modifiers == 0 && this.Modifiers == WinAPI.MOD_ALT && this.VirtualKeyCode == 0))
				return true;
			if (((this.VirtualKeyCode == 17 || this.VirtualKeyCode == 162 || this.VirtualKeyCode == 163) && this.Modifiers == 0 && other.Modifiers == WinAPI.MOD_CONTROL && other.VirtualKeyCode == 0) 
			 || ((other.VirtualKeyCode == 17 || other.VirtualKeyCode == 162 || other.VirtualKeyCode == 163) && other.Modifiers == 0 && this.Modifiers == WinAPI.MOD_CONTROL && this.VirtualKeyCode == 0))
				return true;
			return this.VirtualKeyCode == other.VirtualKeyCode && 
				   this.Modifiers == other.Modifiers;
		}
		public static bool operator ==(Hotkey lhs, Hotkey rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		public static bool operator !=(Hotkey lhs, Hotkey rhs) {
			return !(lhs == rhs);
		}
		#endregion
		public static void CallHotkey(Hotkey hotkey, HKID hkID, ref bool hkOK, Action hkAction) {
			bool once = false;
			if (!hotkey.Double) once = true;
			if (hotkey.ID == (int)hkID && hotkey.Enabled) {
				if (hkOK || once) {
					Logging.Log("Hotkey [" + Enum.GetName(typeof(HKID), hkID) + "] fired.");
					if (MahouUI.BlockHKWithCtrl && ContainsModifier((int)hotkey.Modifiers, (int)WinAPI.MOD_CONTROL)) { } else {
						if (hotkey.Modifiers > 0 && MahouUI.RePress) {
							KMHook.hotkeywithmodsfired = true;
							KMHook.RePressAfter(Convert.ToInt32(hotkey.Modifiers));
						}
					}
					if (hotkey.ID <= (int)Hotkey.HKID.TransliterateSelection || hotkey.ID == (int)Hotkey.HKID.ShowSelectionTranslation ||
					    hotkey.ID == (int)Hotkey.HKID.CycleCase
					   ) {
						KMHook.SendModsUp(Convert.ToInt32(hotkey.Modifiers));
					}
					KMHook.IfKeyIsMod((Keys)hotkey.VirtualKeyCode);
					hkAction();
					if (MahouUI.RePress)
						KMHook.RePress();
				} else if (hotkey.Double) {
					Logging.Log("Waiting for second hotkey ["+Enum.GetName(typeof(HKID), hkID) +"] press.");
					hkOK = true;
					KMHook.doublekey.Interval = MMain.mahou.DoubleHKInterval;
					KMHook.doublekey.Start();
				}
			}
		}
// for debugging
//		public static string tray_hk_to_string(Tuple<bool, bool, bool, bool, bool, bool, bool, Tuple<bool, int>> trhk) {
//			var strhk = "";
//			if(trhk.Item1) { strhk += "lalt+"; }
//			if(trhk.Item2) { strhk += "ralt+"; }
//			if(trhk.Item3) { strhk += "lshift+"; }
//			if(trhk.Item4) { strhk += "rshift+"; }
//			if(trhk.Item5) { strhk += "lctrl"; }
//			if(trhk.Item6) { strhk += "rctrl+"; }
//			if(trhk.Item7) { strhk += "lwin+"; }
//			if(trhk.Rest.Item1) { strhk += "rwin+"; }
//			strhk += ((Keys)trhk.Rest.Item2).ToString();
//			return strhk;
//		}
		/// <summary>
		/// values: 
		/// [LAlt],[RAlt],[LShift],[RShift],[LCtrl],[RCtrl],[LWin],[RWin],[Key_Code]
		/// </summary>
		/// <param name="trhk"> raw hotkey from tray menu Mahou.mm</param>
		/// <returns></returns>
		public static Tuple<bool, bool, bool, bool, bool, bool, bool, Tuple<bool, int>> tray_hk_parse(string trhk) {
			bool la,ra,ls,rs,lc,rc,lw,rw; int kc = 0;
			la=ra=ls=rs=lc=rc=lw=rw=false;
			if (trhk.Contains("&&")) {
				trhk = trhk.Split(new []{"&&"}, StringSplitOptions.None)[0];
			}
			var p = trhk.ToLower().Substring(2,trhk.Length-2).Split('+');
			foreach (var x in p) {
//				System.Diagnostics.Debug.WriteLine("X: " +x);
				switch (x) {
					case "lalt": la = true; break;
					case "ralt": ra = true; break;
					case "lshift": ls = true; break;
					case "rshift": rs = true; break;
					case "lctrl": lc = true; break;
					case "rctrl": rc = true; break;
					case "lwin": lw = true; break;
					case "rwin": rw = true; break;
					default:
//					System.Diagnostics.Debug.WriteLine("x = " +x);
					if (!string.IsNullOrEmpty(x)) {
						if (x.ToLower() == "laltkey")
							kc = (int)Keys.LMenu;
						else if (x.ToLower() == "raltkey")
							kc = (int)Keys.LMenu;
						else if (x.ToLower() == "lwinkey")
							kc = (int)Keys.LWin;
						else if (x.ToLower() == "rwinkey")
							kc = (int)Keys.RWin;
						else {
							var l = KMHook.strparsekey(x);
							if (l.Count > 0) 
								kc = (int)l[0];
						}
					}
					break;
				}
			}
			return new Tuple<bool, bool, bool, bool, bool, bool, bool, Tuple<bool, int>>(la,ra,ls,rs,lc,rc,lw, new Tuple<bool, int>(rw,kc));
		}
		/// <summary>
		/// returns true if a and b equal
		/// </summary>
		/// <param name="a">hotkey data 1</param>
		/// <param name="b">hotkey data 2</param>
		public static bool cmp_hotkey(Tuple<bool, bool, bool, bool, bool, bool, bool, Tuple<bool, int>> a, Tuple<bool, bool, bool, bool, bool, bool, bool, Tuple<bool, int>> b) {
			return (a.Item1 == b.Item1 && 
			        a.Item2 == b.Item2 &&
			        a.Item3 == b.Item3 && 
			        a.Item4 == b.Item4 &&
			        a.Item5 == b.Item5 &&
			        a.Item6 == b.Item6 && 
			        a.Item7 == b.Item7 &&
			        a.Rest.Item1 == b.Rest.Item1 && 
			        a.Rest.Item2 == b.Rest.Item2);
		}
		public static Tuple<bool, int, string> tray_hk_is_double(string hk) {
			var a = hk.Contains("&&"); 
			var b = 250; // default 250 ms for "double hotkeys"
			var c = "";
			hk = hk.ToLower();
			if (a) {
				var x = hk.Split(new []{"&&"}, StringSplitOptions.None);
//				MessageBox.Show(x[0]+"\n"+x[1]);
				c = x[0];
				if (x[1].StartsWith("((") && x[1].Contains("))")) {
					var end = x[1].IndexOf("))");
					var f = x[1].Substring(2, end-2);
//					MessageBox.Show(f +  " => " +hk);
					Int32.TryParse(f, out b);
					var hk2 = x[1].Substring(end+2,x[1].Length-end-2);
//					MessageBox.Show("hk2"+hk2+" b" + b);
					if (!string.IsNullOrEmpty(hk2)) {
						c = "^^"+hk2;
					}
				}
			}
			return new Tuple<bool, int, string>(a, b, c);
		}
		public static bool KeyIsModifier(Keys key) {
			var r = false;
			switch (key) {
				case Keys.LMenu:
				case Keys.RMenu:
				case Keys.LShiftKey:
				case Keys.RShiftKey:
				case Keys.LControlKey:
				case Keys.RControlKey:
				case Keys.LWin:
				case Keys.RWin:
					r = true; break;
			}
			return r;
		}
	}
}
