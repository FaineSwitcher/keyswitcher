using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mahou
{
    public static class NativeClipboard {
    	/// <summary>
    	/// Clears clipboard.
    	/// </summary>
        public static void Clear() {
            WinAPI.OpenClipboard(IntPtr.Zero);
            WinAPI.EmptyClipboard();
            WinAPI.CloseClipboard();
        }
        /// <summary>
        /// Gets clipboard text if clipboard data contains text(CF_UNICODETEXT).
        /// </summary>
        /// <returns>string</returns>
        public static string GetText(uint format=WinAPI.CF_UNICODETEXT, bool wide=true)  { // Gets text data from clipboard
            if (!WinAPI.IsClipboardFormatAvailable(format))
                return null;
            int Tries = 0;
            var opened = false;
            string data = null;
            while (true) {
                ++Tries;
                opened = WinAPI.OpenClipboard(IntPtr.Zero);
                var hGlobal = WinAPI.GetClipboardData(format);
                var lpwcstr = WinAPI.GlobalLock(hGlobal);
                if (wide)
                	data = Marshal.PtrToStringUni(lpwcstr);
                else
                	data = Marshal.PtrToStringAnsi(lpwcstr);
                if (opened) {
                    WinAPI.GlobalUnlock(hGlobal);
                    break;
                }
                System.Threading.Thread.Sleep(1);
            }
            WinAPI.CloseClipboard();
            Logging.Log("Clipboard text was get.");
            return data;
        }
        /// <summary> Clipboard data/formats in two lists.</summary>
		public class clip {
			public int Count;
			public List<uint> f;
			public List<byte[]> d;
			public clip() {
				f = new List<uint>();
				d = new List<byte[]>();
				Count = 0;
			}
			public byte[] this[uint f] {
				get { return get(f); }
				set { set(f, value); }
			}
			public int findex(uint format) {
				for (int i = 0; i < Count; i++) { if (f[i] == format) { return i; } }
				return -1;
			}
			public byte[] get(uint format) {
				int fi = findex(format);
				if (fi != -1) { return d[fi]; }
				//return null;
				throw new Exception("No such format:" +format);
			}
			public void set(uint format, byte[] data, bool add_missing = false) {
				int fi = findex(format);
				if (fi == -1 /*&& add_missing*/) {
					f.Add(format);
					d.Add(data);
				} else {
					//formats[fi] = format;
					d[fi] = data;
				}
				Count = f.Count;
			}
//			public void rem(uint format) {
//				int fi = findex(format);
//				if (fi != -1) {
//					f.RemoveAt(fi);
//					d.RemoveAt(fi);
//				}
//			}
		}
		public static void clip_set(clip c) {
        	if (c == null) return;
        	if (c.Count == 0) return;
			WinAPI.OpenClipboard(IntPtr.Zero);
			WinAPI.EmptyClipboard();
			for (int i = 0; i < c.Count; i++) {
				Logging.Log("[clip] Setting: "+ c.f[i] + " format.");
//				IntPtr hglob = Marshal.AllocHGlobal(c.d[i].Length);
				IntPtr alloc = WinAPI.GlobalAlloc(WinAPI.GMEM_MOVEABLE | WinAPI.GMEM_DDESHARE, new UIntPtr(Convert.ToUInt32(c.d[i].GetLength(0))));
				IntPtr glock = WinAPI.GlobalLock(alloc);
				Marshal.Copy(c.d[i], 0, glock, c.d[i].Length);
				WinAPI.GlobalUnlock(glock);
				WinAPI.SetClipboardData(c.f[i], alloc);
				WinAPI.GlobalFree(alloc);
//				Marshal.FreeHGlobal(hglob);
			}
			WinAPI.CloseClipboard();
		}
		public static clip clip_get() {
        	clip c;
    		var open = WinAPI.OpenClipboard(IntPtr.Zero);
			if (!open) {
    			WinAPI.CloseClipboard();
				Logging.Log("[clip] Error can't open clipboard.");
				return null;
			}
			int size = 0, all_size = 0;
			c = new clip();
			IntPtr hglob = IntPtr.Zero;//, all;
			IntPtr glock = IntPtr.Zero;
			uint format, dib_skip = 0;
//			all = new IntPtr((uint)Marshal.SizeOf(typeof(uint)));
			for (format = 0; (format = WinAPI.EnumClipboardFormats(format)) != 0;) {
				switch (format) {
					case WinAPI.CF_BITMAP:
					case WinAPI.CF_ENHMETAFILE:
					case WinAPI.CF_DSPENHMETAFILE:
						continue; // unsafe formats for GlobalSize
				}
				if (format == WinAPI.CF_TEXT || format == WinAPI.CF_OEMTEXT // calculate only CF_UNICODETEXT instead
					|| format == dib_skip) // also only one of dib/dibv5 formats should be calculated
					continue;
				hglob = WinAPI.GetClipboardData(format);
				if (hglob == IntPtr.Zero) { Logging.Log("[clip] hglob Fail: " +format);continue;  } // GetClipboardData() failed: skip this format.
//				if (format == WinAPI.CF_HDROP) {
//					System.Diagnostics.Debug.WriteLine("HGLOGADDR:" +hglob.ToInt32());
//					glock = WinAPI.GlobalLock(hglob);
//					if (glock != IntPtr.Zero) {
////						System.Diagnostics.Debug.WriteLine("HGLOGADDR:" +glock.ToInt32() + " " +Marshal.GetLastWin32Error());
////						int fc = WinAPI.DragQueryFile(glock, 0xFFFFFFFF, null, 0);
////						System.Diagnostics.Debug.WriteLine("[clip] FC:" + fc);
////						if (fc != 0) {
////							size = ((fc - 1) * 2);  // Init; -1 if don't want a newline after last file.
////							for (uint i = 0; i < fc; ++i) {
////								var tsize = WinAPI.DragQueryFile(glock, i, null, 0);
////								Logging.Log("[clip] File:" + i+ ", size:" +tsize);
////								size+=tsize;
////							}
////						}
////						else
////							size = 0;
//						size = WinAPI.GlobalSize(glock).ToInt32();
//						if (size != 0) {
//							byte[] bin = new byte[size];
//							System.Diagnostics.Debug.WriteLine("[clip] CF_HDROP Marshal copy: size:" + size +", bin-len: " + bin.Length + " glock:" + glock);
//							Marshal.Copy(glock, bin, 0, Convert.ToInt32(size));
//							c[format] = bin;
//						}
////						fc = WinAPI.DragQueryFile(glock, 0xFFFFFFFF, new System.Text.StringBuilder(""), 0);
////						System.Diagnostics.Debug.WriteLine("[clip] FC:" + fc);
////						if (fc != 0) {
////							size = ((fc - 1) * 2);  // Init; -1 if don't want a newline after last file.
////							var fnb = new System.Text.StringBuilder(999);
////							for (uint i = 0; i < fc; ++i) {
////								var tsize = WinAPI.DragQueryFile(glock, i, fnb, 999);
////							System.Diagnostics.Debug.WriteLine("[clip] File:" + i+ ", size:" +tsize);
////								size+=tsize;
////							}
////							System.Diagnostics.Debug.WriteLine("[clip] Fnb:" +fnb);
////						}
////					}
//					WinAPI.GlobalUnlock(glock);
//					continue;
//				}
				glock = WinAPI.GlobalLock(hglob);
				if (glock != IntPtr.Zero) {
					size = WinAPI.GlobalSize(glock).ToInt32();
					all_size += size;
					byte[] bin = new byte[size];
					Logging.Log("[clip] Marshal copy: size:" + size +", bin-len: " + bin.Length + " glock:" + glock);
					Marshal.Copy(glock, bin, 0, (int)size);
					c[format] = bin;
				} else { Logging.Log("[clip] glock Fail: " +format); }
				WinAPI.GlobalUnlock(glock);
				Logging.Log("[clip] hglob:" + hglob + " x fmt: " + format);
				if (dib_skip == 0) {
					if (format == WinAPI.CF_DIB)
						dib_skip = WinAPI.CF_DIBV5;
					else if (format == WinAPI.CF_DIBV5)
						dib_skip = WinAPI.CF_DIB;
				}
			}
			Logging.Log("[clip] formats_count:," + c.Count);
			WinAPI.CloseClipboard();
			return c;
    	}
    }
}
