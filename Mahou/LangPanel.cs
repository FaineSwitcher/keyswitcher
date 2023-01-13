using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Mahou {
	/// <summary> Language panel with display of flag, name, id of current layout. </summary>
	public partial class LangPanel : Form {
		int l = 4;
		public LangPanel() {
			InitializeComponent();
			this.FormClosing += (s, e) => { e.Cancel = true; this.Hide(); };
			Height = 24;
			AeroCheck();
			MahouUI.DPISCALE(this);
			l = Convert.ToInt32(MahouUI.xr*l);
			outsidefix(pct_Flag);
			outsidefix(pct_Upper);
		}
		void outsidefix(PictureBox p) {
			var outside = p.Height;
			if (outside >= this.Height) {
				p.Height -= outside-this.Height+1+p.Location.Y;
			}
		}
		#region Screen-Snap
		const int SnapDist = 15;
		public Point mouseLocation;
		public bool snap_l, snap_r, snap_t, snap_b;
		public void ChangeLayout(Bitmap flag, string layoutName) {
			lbl_LayoutName.Text = layoutName;
			pct_Flag.BackgroundImage = new Bitmap(flag);
			Width = lbl_LayoutName.Left + lbl_LayoutName.Width + l;
			ReSnap();
		}
		public void DisplayUpper(bool Upper) {
			pct_Upper.Visible = Upper;
			var side = Upper ? l+pct_Flag.Width+pct_Upper.Width : l+pct_Flag.Width;
			Width = side + lbl_LayoutName.Width + l;
			lbl_LayoutName.Left = side;
			ReSnap();
        }
		void ReSnap() {
			var scr = Screen.FromPoint(Location);
			if (Location.X + Width > scr.Bounds.Width)
				Left = scr.Bounds.Width - Width;
			if (snap_r && Left != scr.Bounds.Width - Width)
				Left = scr.Bounds.Width - Width;
			Invalidate();
		}
		void LangPanelMouseDown(object sender, MouseEventArgs e) {
			int top = 0, left = 0;
			if (!(sender is Form)) {
				var ctrl = ((Control)sender);
				top = ctrl.Top;
				left = ctrl.Left;
			}
			mouseLocation = new Point(-e.X - left, -e.Y - top);
		}
		void LangPanelMouseMove(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
			    var mousePos = MousePosition;
			    var mousePosR = MousePosition;
			    mousePos.Offset(mouseLocation.X, mouseLocation.Y);
			    mousePosR.Offset(Width + mouseLocation.X, Height + mouseLocation.Y);
				var scr = Screen.FromPoint(Location);
//			    Debug.WriteLine(mouseLocation.X + " " + mouseLocation.Y + " / " +
//			                    MousePosition.X + " " + MousePosition.Y + " cr " + 
//			                    mousePosR.X + " " + mousePosR.Y + " sl " + 
//			                    snap_l + " sr " + snap_r);
				snap_l = mousePos.X < SnapDist ? true : false;
				snap_r = mousePosR.X > scr.Bounds.Width - SnapDist ? true : false;
				snap_t = mousePos.Y < SnapDist ? true : false;
				snap_b = mousePosR.Y > scr.Bounds.Height - SnapDist ? true : false; 
			    if (snap_l || snap_r || snap_t || snap_b) {
					if ((snap_l || snap_r) && (!snap_t && !snap_b))
						Top = mousePos.Y;
				    if ((snap_t || snap_b) && (!snap_l && !snap_r))
						Left = mousePos.X;
			    	if (snap_l)
						Left = scr.WorkingArea.Left;
			    	if (snap_r)
						Left = scr.WorkingArea.Right - Width;
			    	if (snap_t)
						Top = scr.WorkingArea.Top;
			    	if (snap_b)
						Top = scr.WorkingArea.Bottom - Height;
		    	} else 
			    	Location = mousePos;
			}
		}
		
		void Lbl_LayoutNameMouseUp(object sender, MouseEventArgs e) {
			Logging.Log("Saved position of LangPanel");
			MMain.MyConfs.WriteSave("LangPanel", "Position", "X" + Location.X + " Y" + Location.Y);
		}
		public void UpdateApperence(Color back, Color fore, int opacity, Font font) {
			BackColor = back;
			lbl_LayoutName.ForeColor = fore;
			lbl_LayoutName.Font = font;
			Opacity = (double)opacity / 100;
			Invalidate();
		}
		#endregion
		#region Derived from LangDisplay
		//Comments removed
		public void ShowInactiveTopmost(int left = -7, int top = -7) {
			if (Visible) return;
			int LEFT = Left, TOP = Top;
			if (left != -7)
				LEFT = left;
			if (top != -7)
				TOP = top;
			WinAPI.ShowWindow(Handle, WinAPI.SW_SHOWNOACTIVATE);
			WinAPI.SetWindowPos(Handle.ToInt32(), WinAPI.HWND_TOPMOST,
				LEFT, TOP, Width, Height,
				WinAPI.SWP_NOACTIVATE);
		}
		public void HideWnd() {
			if (!Visible) return;
			WinAPI.ShowWindow(Handle, 0);
		}
		protected override CreateParams CreateParams {
			get {
				var Params = base.CreateParams;
				Params.ExStyle |= WinAPI.WS_EX_TOOLWINDOW;
//				Params.ExStyle |=  // WinAPI.WS_EX_LAYERED |
//					WinAPI.WS_EX_TRANSPARENT;
				return Params;
			}
		}
		#endregion
		#region Derived from JustUI
		public bool AeroEnabled;
		public void AeroCheck() {
			if (KMHook.IfNW7()) {
				int enabled = 0;
				WinAPI.DwmIsCompositionEnabled(ref enabled);
				AeroEnabled = (enabled == 1);
			}
		}
		Color CurrentAeroColor() {
			WinAPI.DWM_COLORIZATION_PARAMS parameters;
			WinAPI.DwmGetColorizationParameters(out parameters);
			return Color.FromArgb(Int32.Parse(parameters.clrColor.ToString("X"), System.Globalization.NumberStyles.HexNumber));;
		}
		protected override void WndProc(ref Message m) {
			if (m.Msg == WinAPI.WM_NCPAINT && AeroEnabled) {
				var v = 2;
				WinAPI.DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
				var margins = new WinAPI.MARGINS(){ bH = 1, lW = 1, rW = 1, tH = 1 };
				WinAPI.DwmExtendFrameIntoClientArea(this.Handle, ref margins);
			}
			base.WndProc(ref m);
		}
		protected override void OnPaint(PaintEventArgs e) {
			if (MMain.mahou == null) { base.OnPaint(e); return; }
			Graphics g = CreateGraphics();
			var pn = new Pen(Color.Black);
			if (AeroEnabled && MahouUI.LangPanelBorderAero)
				pn = new Pen(CurrentAeroColor());
			else
				pn.Color = MMain.mahou.LangPanelBorderColor;
			g.DrawRectangle(pn, new Rectangle(0, 0, Size.Width - 1, Size.Height - 1));
			g.Dispose();
			pn.Dispose();
			base.OnPaint(e);
		}
		#endregion
	}
}
