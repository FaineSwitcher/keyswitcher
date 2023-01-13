using System;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Mahou {
	static class jklXHidServ {
		public static uint cycleEmuDesiredLayout = 0;
		public static bool start_cyclEmuSwitch = false;
		public static Action ActionOnLayout;
		public static uint OnLayoutAction = 0;
		public static int[] jkluMSG = new int[]{-1, -1};
		public static int jklMaxChangeTries = 20, jklChangeTries = 0;
		const string HWND_SERVER_TITLE = "777_MAHOU_777_HIDDEN_HWND_SERVER";
		public static bool running = false, self_change, actionOnLayoutExecuted;
		/// <summary>0=exe, 1=dll, 2=x86.exe, 3=x86.dll</summary>
		public static bool[] jklFEX = new bool[5];
		public static string jklInfoStr = "";
		static HServer s;
	    static public void Destroy() {
			if (s != null) {
				var serv = WinAPI.FindWindow("_HIDDEN_HWND_SERVER", "_HIDDEN_HWND_SERVER");
				var x86help = WinAPI.FindWindow("_HIDDEN_X86_HELPER", "_HIDDEN_X86_HELPER");
				if (serv != IntPtr.Zero)
					WinAPI.PostMessage(serv, WinAPI.WM_QUIT, 0, 0);
				if (x86help != IntPtr.Zero)
					WinAPI.PostMessage(x86help, WinAPI.WM_QUIT, 0, 0);
				running = false;
				s.Dispose();
				s = null;
				// Multiple CreateWindowEx & WM_DESTROY causes NullReference exception in NATIVE CODE!!
				// So its disabled for now... Create window 1 time and not destroy it.
//				WinAPI.PostMessage(HWND, WinAPI.WM_DESTROY, 0, 0); 
//		        HWND = IntPtr.Zero;
			}
	    }
		public static bool jklExist() {
			var pth = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "jkl");
			jklFEX[0] = File.Exists(pth+".exe");
			jklFEX[1] = File.Exists(pth+".dll");
			jklFEX[2] = File.Exists(pth+"x86.exe");
			jklFEX[3] = File.Exists(pth+"x86.dll");
			if (!Environment.Is64BitOperatingSystem) {
				if (jklFEX[2] && jklFEX[3])
					return true;
			}
			if (jklFEX[0] && jklFEX[1] && jklFEX[2] && jklFEX[3])
				return true;
			jklInfoStr = "jkl.exe " + (jklFEX[0] ? "" : MMain.Lang[Languages.Element.Not] + " ") + MMain.Lang[Languages.Element.Exist] + "\r\n";
			jklInfoStr += "jkl.dll " + (jklFEX[1] ? "" : MMain.Lang[Languages.Element.Not] + " ") + MMain.Lang[Languages.Element.Exist] + "\r\n";
			jklInfoStr += "jklx86.exe " + (jklFEX[2] ? "" : MMain.Lang[Languages.Element.Not] + " ") + MMain.Lang[Languages.Element.Exist] + "\r\n";
			jklInfoStr += "jklx86.dll " + (jklFEX[3] ? "" : MMain.Lang[Languages.Element.Not] + " ") + MMain.Lang[Languages.Element.Exist] + "\r\n";
			return false;
		}
	    public static void Init() {
			if (!MahouUI.ENABLED) return;
			if (running) {
				bool exist = true;
				if (Environment.Is64BitOperatingSystem) {
					if (Process.GetProcessesByName("jkl").Length > 0) {
						Logging.Log("[JKL] > JKL already running.");
					} else 
						exist = false;
				} else 	{
					if (Process.GetProcessesByName("jklx86").Length > 0) {
						Logging.Log("[JKL] > JKLx86 already running.");
					} else 
						exist = false;
				}
				if (!exist) {
					Logging.Log("[JKL] > JKL seems closed, restarting...");
					running = false;
				}
			}
			if (s == null) {
				Logging.Log("[JKL] > Initializing JKL HWND server...");
				s = new HServer();
		        Logging.Log("[JKL] > SERVER HWND: " +s.Handle + " WinTitle: " +s.Text);
			}
			if (!running) {
				if (jklExist()) {
					int ii = 0;
					var jkl = new ProcessStartInfo();
					jkl.UseShellExecute = true;
					jkl.WorkingDirectory = Path.Combine(Path.GetTempPath());
					if (Environment.Is64BitOperatingSystem) {
						Logging.Log("[JKL] > Starting jkl.exe...");
						jkl.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "jkl.exe");
			        	Process.Start(jkl);
					} else { ii = 1; }
					Logging.Log("[JKL] > Starting \"jklx86.exe\"...");
					jkl.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "jklx86.exe");
		        	Process.Start(jkl);
					var umsgIDs = new []{Path.Combine(Path.GetTempPath(), "umsg.id"), Path.Combine(Path.GetTempPath(), "umsg32.id")};
					for (; ii != umsgIDs.Length; ii++) {
						var umsgID = umsgIDs[ii];
						var tries = 0;
						while (!File.Exists(umsgID)) {
							Thread.Sleep(350);
							tries++;
							if (tries > 20) {
								Logging.Log("[JKL] > Error, "+umsgID+" not found after 20 tries by 350 ms timeout.", 1);
								Destroy();
								break;
							}
						}
						if (tries <= 20) {
							Logging.Log("[JKL] > "+umsgID+" found, after " + tries + " tries * 350ms timeout.");
							Logging.Log("[JKL] > Retrieving umsg.id...");
							int max_tries = 5;
							while (max_tries > 0) {
								try {
									jkluMSG[ii] = Int32.Parse(File.ReadAllText(umsgID));
									break;
								} catch(Exception e) {
									max_tries--;
									Thread.Sleep(50);
								}
							}
//							File.Delete(umsgID);
	//						KMHook.DoLater(() => CycleAllLayouts(Locales.ActiveWindow()), 350);
							KMHook.DoLater(() => { MahouUI.GlobalLayout = MahouUI.currentLayout = Locales.GetCurrentLocale(); }, 200);
							running = true;
						}
					}
				} else {
					Logging.Log("[JKL] > " + jklInfoStr, 1);
				}
				if ((jkluMSG[0] == -1 && Environment.Is64BitOperatingSystem) || jkluMSG[1] == -1)
					KMHook.JKLERR = true;
				else {
					KMHook.JKLERR = false;
					Logging.Log("[JKL] > Init done, umsg: ["+jkluMSG[0]+" "+jkluMSG[1]+"], JKLXServ: ["+s.Handle+"].");
				}
			}
	    }
		public static void CycleAllLayouts(IntPtr hwnd) {
			self_change = true;
//			for (int i = 0; i!=MMain.PHLayouts; i++) {
//				if (MMain.MahouActive()) return; // Else creates invalid culture 0 exception.
				WinAPI.SendMessage(hwnd, (int)WinAPI.WM_INPUTLANGCHANGEREQUEST, 0, WinAPI.HKL_NEXT);
//				Logging.Log("[JKL] > Cycle all: "+i+"/"+MMain.PHLayouts);
//				Thread.Sleep(5);
				WinAPI.SendMessage(hwnd, (int)WinAPI.WM_INPUTLANGCHANGEREQUEST, 0, WinAPI.HKL_PREV);
//			}/
			self_change = false;
		}
		static uint last_change_layout = 0;
		static int repeat = 0;
		class HServer : Form {
			public HServer() {
				this.Visible = false;
				this.Size = new Size(1,1);
				this.Location = new Point(0,0);
				this.Text = HWND_SERVER_TITLE;
			}
			protected override void WndProc(ref Message m) {
				if (m.Msg == jkluMSG[0] || m.Msg == jkluMSG[1]) {
					uint layout = (uint)m.LParam;
					if (layout == last_change_layout) {
						repeat++;
					} else {
						if (repeat != 0) {
							Logging.Log("[JKL] > Last layout change to ["+last_change_layout+"] was repeated "+repeat+"x times.");
							repeat = 0;
						}
						MahouUI.GlobalLayout = MahouUI.currentLayout = layout;
						last_change_layout = layout;
						Logging.Log("[JKL]("+(m.Msg==jkluMSG[0]?"64":"32")+") > Layout changed to [" + layout + "] / [0x"+layout.ToString("X") +"].");
						Debug.WriteLine(">> JKL LC: " + layout);
						Logging.Log("[JKL] > On layout act:" +OnLayoutAction);
						var anull = ActionOnLayout==null;
						Logging.Log("[JKL] > ACtion: " +(anull?"null":ActionOnLayout.Method.Name));
						if (KMHook.CompareLayouts(layout, OnLayoutAction)) {
							actionOnLayoutExecuted = true;
							OnLayoutAction = 0;
							if (anull) {
								Logging.Log("[JKL] > Action is null, something terribly went wrong... Please try to disable JKL, if layout changing went wild.",1);
							} else {
								Debug.WriteLine("Executing action: " + ActionOnLayout.Method.Name + " on layout: " +layout);
							    ActionOnLayout();
							    ActionOnLayout = null;
							}
						}
						if (start_cyclEmuSwitch) {
							jklChangeTries++;
							if (jklChangeTries>=jklMaxChangeTries) {
								Logging.Log("Emulate layout switch to "+cycleEmuDesiredLayout+" failed, could not reach that layout after " + jklMaxChangeTries +" tries.",1);
								jklChangeTries = 0;
								start_cyclEmuSwitch = false;
								Logging.Log("Fallback to NormalChangeToLayout");
								MahouUI.EmulateLS = false;
								KMHook.ChangeToLayout(Locales.ActiveWindow(), cycleEmuDesiredLayout);
								if (!anull && KMHook.CompareLayouts(OnLayoutAction, cycleEmuDesiredLayout)) {
									ActionOnLayout();
									ActionOnLayout = null;
								}
								MahouUI.EmulateLS = true;
							}
							Debug.WriteLine("Cycling out from: "+ layout + " to " + cycleEmuDesiredLayout +"..." + jklChangeTries);
							if (!KMHook.CompareLayouts(layout, cycleEmuDesiredLayout)) {
								KMHook.CycleEmulateLayoutSwitch();
							}
							else {
								jklChangeTries = 0;
								start_cyclEmuSwitch = false;
								if (KMHook.CompareLayouts(MahouUI.MAIN_LAYOUT1, layout) || 
								    KMHook.CompareLayouts(MahouUI.MAIN_LAYOUT2, layout)) {
									KMHook.last_switch_layout = layout;
									KMHook.evt_layoutchanged(layout, 0, MahouUI.bindable_events[0]);
								}
							}
						}
						if (!self_change && !start_cyclEmuSwitch) {
							MahouUI.RefreshFLAG();
							MMain.mahou.RefreshAllIcons();
							MMain.mahou.UpdateLDs();
							if (anull && !KMHook.selfie) {
								KMHook.AS_IGN_fun();
							}
						}
					}
				}
				base.WndProc(ref m);
			}
		}
	}
}