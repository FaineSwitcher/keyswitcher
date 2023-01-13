using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace JustUI {
	public partial class JUITitle : Label {
		public JUITitle() {
			InitializeComponent();
		}
		protected override void OnMouseDown(MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				const int WM_NCLBUTTONDOWN = 0xA1;
				const int HT_CAPTION = 0x2;
				ReleaseCapture();
				SendMessage(this.Parent.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
			base.OnMouseDown(e);
		}
		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();
	}
}
