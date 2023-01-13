using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mahou {
	public class RawInputForm : Form {
		public RawInputForm() {
			this.Visible =false;
			this.FormBorderStyle = FormBorderStyle.None;
			this.Size = new System.Drawing.Size(1,1);
			RegisterRawInputDevices(this.Handle);
		}
		protected override void WndProc(ref Message m) {
			if (m.Msg == WinAPI.WM_INPUT) {
				WinAPI.RAWINPUT input = new WinAPI.RAWINPUT();
	            int outSize = 0;
	            int size = Marshal.SizeOf(typeof(WinAPI.RAWINPUT));
	            outSize = WinAPI.GetRawInputData(m.LParam, WinAPI.RawInputCommand.Input, out input, ref size,
	                                             Marshal.SizeOf(typeof(WinAPI.RAWINPUTHEADER)));
	            if (outSize != -1) {
	                if (input.Header.Type == WinAPI.RawInputType.Mouse)
	            		if (input.Data.Mouse.Data.ButtonFlags != WinAPI.RawMouseButtons.None)
	            	    	KMHook.ListenMouse((ushort)input.Data.Mouse.Data.ButtonFlags);
	                	else 
	            	    	KMHook.ListenMouse((ushort)input.Data.Mouse.Flags);
	                		
	                if (input.Header.Type == WinAPI.RawInputType.Keyboard) {
	                	var kbd = input.Data.Keyboard;
	                	var k = kbd.VKey;
	                	switch (k) {
	                		case (int)Keys.ShiftKey:
	                			if (kbd.MakeCode == 42)
	                				k = (int)Keys.LShiftKey;
	                			else
	                				k = (int)Keys.RShiftKey;
	                			break;
	                		case (int)Keys.ControlKey:
	                			if (kbd.Flags < 2)
	                				k = (int)Keys.LControlKey;
	                			else 
	                				k = (int)Keys.RControlKey;
	                			break;
	                		case (int)Keys.Menu:
	                			if (kbd.Flags < 2)
	                				k = (int)Keys.LMenu;
	                			else 
	                				k = (int)Keys.RMenu;
	                			break;
	                	}
	        	    	KMHook.ListenKeyboard(k, input.Data.Keyboard.Message);
	                }
	            }
			}
            base.WndProc(ref m);
		}
		/// <summary>
		/// Register/Unregister Raw Input devices.
		/// To unregister pass hwnd = IntPtr.Zero, and flag = WinAPI.RawInputDeviceFlags.Remove;
		/// </summary>
		public void RegisterRawInputDevices(IntPtr hwnd, WinAPI.RawInputDeviceFlags flag = WinAPI.RawInputDeviceFlags.InputSink) {
			if (!MahouUI.ENABLED) return;
			WinAPI.RAWINPUTDEVICE[] rids = new WinAPI.RAWINPUTDEVICE[2];
			// Keyboard device
			rids[0].UsagePage = 0x1;
            rids[0].Usage = 0x06;
            rids[0].Flags = flag;
            rids[0].WindowHandle = hwnd;
			// Mouse device
            rids[1].UsagePage = 0x01;
            rids[1].Usage = 0x02;
            rids[1].Flags = flag;
            rids[1].WindowHandle = hwnd;
            // Registering devices
            if(!WinAPI.RegisterRawInputDevices(rids, 2, Marshal.SizeOf(typeof(WinAPI.RAWINPUTDEVICE)))) {
                Logging.Log("Registering raw input devices failed! " + flag);
            }
		}
	}
}