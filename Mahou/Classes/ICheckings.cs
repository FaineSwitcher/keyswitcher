using System.Runtime.InteropServices;
using System.Windows.Forms;

public static class ICheckings
{
	/// <summary>
	/// Checks if current cursor is IBeam.
	/// </summary>
	public static bool IsICursor() {
	    WinAPI.CURSORINFO cInfo;
	    cInfo.cbSize = Marshal.SizeOf(typeof(WinAPI.CURSORINFO));
	    WinAPI.GetCursorInfo(out cInfo);
	    return cInfo.hCursor == Cursors.IBeam.Handle;
	}	
}
