using System.Drawing;
using System.Windows.Forms;

namespace Mahou
{
	public partial class LangDisplay : Form {
		/// <summary>
		/// Used to determine if lang display is used for caret display.
		/// </summary>
		public bool lastTransparentBG, transparentBG, caretDisplay, mouseDisplay, DisplayFlag, onInit = true, Empty;
		public string lastText = "NO";
		Size lastsize = new Size(0,0);
		public LangDisplay() {
			InitializeComponent();
			this.FormClosing += (s, e) => { e.Cancel = true; this.Hide(); };
			SetVisInvis();
			this.SetStyle(ControlStyles.DoubleBuffer, true);
		}
		/// <summary>
		/// Change lang display text.
		/// </summary>
		/// <param name="to">Text to be changed to.</param>
		public void ChangeLD(string to) {
			if (to == lastText) return;
			lbLang.Text = lastText = to;
			if (DisplayFlag) {
				lbLang.Visible = false;
				MahouUI.RefreshFLAG(true);
				if (MahouUI.MouseTTAlways && mouseDisplay) {// fix for tray stuck due to variable "LayoutChanged" which being changed by this mouse tooltip always
					var fi = Icon.FromHandle((
							(MahouUI.TrayText && MahouUI.ITEXT != null) ? MahouUI.ITEXT : MahouUI.FLAG)
						.GetHicon());
					MMain.mahou.icon.trIcon.Icon = fi;
					WinAPI.DestroyIcon(fi.Handle);
				}
				BackgroundImage = new Bitmap(MahouUI.FLAG);
				TransparencyKey = BackColor = Color.Pink;
				Invalidate();
				Update();
			}
			if (!transparentBG) return;
			Invalidate();
			Update();
		}
		public void DisplayUpper(bool Upper) {
			var lastvis = pct_UpperArrow.Visible;
			if (Upper && !pct_UpperArrow.Visible)
				pct_UpperArrow.Visible = true;
			else if (!Upper && pct_UpperArrow.Visible)
				pct_UpperArrow.Visible = false;
			if (lastvis != pct_UpperArrow.Visible) {
				ReSize();
				if (Upper) {
					pct_UpperArrow.Left = Width;
					pct_UpperArrow.Top = (Height - 16)/2+1;
					Width = Width + 16;
				}
				lastsize = Size;
			}
		}
		/// <summary>
		/// Toggles lang display label visibility.
		/// </summary>
		public void SetVisInvis() {
			lbLang.Visible = !transparentBG;
			Invalidate();
			Update();
		}
		/// <summary>
		/// Refresh language text.
		/// </summary>
		public void RefreshLang() {
			uint cLid = Locales.GetCurrentLocale();
			if (DisplayFlag && MahouUI.DiffAppearenceForLayouts) {
				if (KMHook.CompareLayouts(cLid, MahouUI.MAIN_LAYOUT1) && MahouUI.Layout1TText.Length < 2)
					lbLang.Text = MahouUI.Layout1TText;
				else if (KMHook.CompareLayouts(cLid, MahouUI.MAIN_LAYOUT2) && MahouUI.Layout2TText.Length < 2)
					lbLang.Text = MahouUI.Layout2TText;
				Empty = (MahouUI.DiffAppearenceForLayouts && lbLang.Text == " ");
			}
			if (!Visible || Empty) return;
			if (cLid == 0)
				cLid = MahouUI.currentLayout;
			if (MahouUI.UseJKL && !KMHook.JKLERR)
				cLid = MahouUI.currentLayout;
			if (MahouUI.OneLayout)
				cLid = MahouUI.GlobalLayout;
			if (MahouUI.LDCaretTransparentBack_temp && caretDisplay)
				transparentBG = true;
			else if (MahouUI.LDMouseTransparentBack_temp && mouseDisplay)
				transparentBG = true;
			else transparentBG = false;
			var notTwo = false;
			if (cLid > 0) {
				System.Globalization.CultureInfo clangname = null;
				try {
					clangname = new System.Globalization.CultureInfo((int)(cLid >> 16));
				} catch {
					clangname = new System.Globalization.CultureInfo((int)(cLid & 0xffff));
				}
				if (clangname == null) return;
				if (MahouUI.DiffAppearenceForLayouts && !DisplayFlag) {
					if (KMHook.CompareLayouts(cLid, MahouUI.MAIN_LAYOUT1)) {
						ChangeColors(MahouUI.Layout1Font_temp, MahouUI.Layout1Fore_temp, 
						             MahouUI.Layout1Back_temp, MahouUI.Layout1TransparentBack_temp);
						ChangeSize(MahouUI.Layout1Height_temp, MahouUI.Layout1Width_temp);
						ChangeLD(MahouUI.Layout1TText);
					} else if (KMHook.CompareLayouts(cLid, MahouUI.MAIN_LAYOUT2)) {
						ChangeColors(MahouUI.Layout2Font_temp, MahouUI.Layout2Fore_temp, 
						             MahouUI.Layout2Back_temp, MahouUI.Layout2TransparentBack_temp);
						ChangeSize(MahouUI.Layout2Height_temp, MahouUI.Layout2Width_temp);
						ChangeLD(MahouUI.Layout2TText);
					} else notTwo = true;
				} else notTwo = true;
				if (notTwo) {
					if (mouseDisplay) {
						ChangeColors(MahouUI.LDMouseFont_temp, MahouUI.LDMouseFore_temp,
						             MahouUI.LDMouseBack_temp, MahouUI.LDMouseTransparentBack_temp);
						ChangeSize(MahouUI.LDMouseHeight_temp, MahouUI.LDMouseWidth_temp);
					}
					if (caretDisplay) {
						ChangeColors(MahouUI.LDCaretFont_temp, MahouUI.LDCaretFore_temp, 
						             MahouUI.LDCaretBack_temp, MahouUI.LDCaretTransparentBack_temp);
						ChangeSize(MahouUI.LDCaretHeight_temp, MahouUI.LDCaretWidth_temp);
					}
					ChangeLD(clangname.ThreeLetterISOLanguageName.Substring(0, 1).ToUpper() + clangname.ThreeLetterISOLanguageName.Substring(1));
				}
				if (!MahouUI.DiffAppearenceForLayouts || lbLang.Text == "") 
					ChangeLD(clangname.ThreeLetterISOLanguageName.Substring(0, 1).ToUpper() + clangname.ThreeLetterISOLanguageName.Substring(1));
				if (transparentBG)
					TransparencyKey = BackColor = lbLang.BackColor = pct_UpperArrow.BackColor = Color.Pink;
				if (lastTransparentBG != transparentBG)
					SetVisInvis();
				lastTransparentBG = transparentBG;
				if ((caretDisplay && !MahouUI.caretLTUpperArrow) || (mouseDisplay && !MahouUI.mouseLTUpperArrow) || onInit) {
					ReSize(); onInit = false;
				}
			} else {
				Logging.Log("Language tooltip text NOT changed, locale id = [" + cLid + "].", 2);
			}
		}
		void ReSize() {
			if (DisplayFlag) {
				if (BackgroundImage == null) return;
				if (lastsize == BackgroundImage.Size) return;
				Size = BackgroundImage.Size;
			}
			else {
				if (lastsize == lbLang.Size) return;
				Size = lbLang.Size;
			}
			lastsize = Size;
		}
		/// <summary>
		/// Change font and colors of lang display.
		/// </summary>
		/// <param name="fnt">Font to be changed to.</param>
		/// <param name="fore">Text color to be changed to.</param>
		/// <param name="back">Background color to be changed to.</param>
		/// <param name="tBG">Transparent background.</param>
		public void ChangeColors(Font fnt, Color fore, Color back, bool tBG) {
			transparentBG = tBG;
			lbLang.ForeColor = fore;
			pct_UpperArrow.BackColor = lbLang.BackColor = back;
			lbLang.Font = fnt;
		}
		/// <summary>
		/// Change size of lang display.
		/// </summary>
		/// <param name="height">Height to be set to.</param>
		/// <param name="width">Width to be set to.</param>
		public void ChangeSize(int height, int width) {
			lbLang.Height = height;
			lbLang.Width = width;
		}
		/// <summary>
		/// Show lang display form without activation.
		/// </summary>
		public void ShowInactiveTopmost() {
			if (Visible) return;
			try {
				WinAPI.ShowWindow(Handle, WinAPI.SW_SHOWNOACTIVATE);
				WinAPI.SetWindowPos(Handle.ToInt32(), WinAPI.HWND_TOPMOST,
					Left, Top, Width, Height,
					WinAPI.SWP_NOACTIVATE);
			} catch (System.Exception e) {
				Logging.Log(">> LD - Show error" + e.Message + e.StackTrace, 1);
			}
		}
		/// <summary>
		/// Hide lang display window.
		/// </summary>
		public void HideWnd() {
			if (!Visible) return;
			WinAPI.ShowWindow(Handle, 0);
		}
		protected override CreateParams CreateParams {
			get {
				var Params = base.CreateParams;
				// Hides form from everywhere (taskbar/task switcher/etc.).
				Params.ExStyle |= WinAPI.WS_EX_TOOLWINDOW;
				// Add click through window ability.
				Params.ExStyle |= WinAPI.WS_EX_LAYERED | WinAPI.WS_EX_TRANSPARENT;
				return Params;
			}
		}
		/// <summary>
		/// Transparent background text rendering.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e) {
			if (!transparentBG || DisplayFlag) { base.OnPaint(e); return; }
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
			e.Graphics.DrawString(lbLang.Text, lbLang.Font, new SolidBrush(lbLang.ForeColor), 0, 0);
			base.OnPaint(e);
		}
	}
}
