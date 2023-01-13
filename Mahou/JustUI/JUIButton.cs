using System;
using System.Drawing;
using System.Windows.Forms;

namespace JustUI
{
	public partial class JUIButton : Button {
		public static uint OK_HoverBGColor = 0xFFADD8E6;
		public static uint OK_DownBGColor = 0xFF00BFFF;
		public static uint NO_HoverBGColor = 0xFFFF8080;
		public static uint NO_DownBGColor = 0xFFFF0000;
		public static uint BTN_HoverBGColor = 0xFFD3D3D3;
		public static uint BTN_DownBGColor = 0xFF2F4F4F;
		public Color _original_color;
		public JUIButton() {
			Text = "Just UI Button";
			FlatStyle = FlatStyle.Flat;
			FlatAppearance.BorderSize = 0;
			TabStop = false;
//			BackColor = Color.FromArgb(12,0,0,0);
			ForeColor = Color.FromKnownColor(KnownColor.WindowText);
			FlatStyle = FlatStyle.Flat;
			SetColors(0);
		}
		public void SetColors(int buttonType = 0, uint HoverBGColor = 0, uint DownBGColor = 0) {
			if  (buttonType == 1) {
				FlatAppearance.MouseOverBackColor = UIntToColor(OK_HoverBGColor);
				FlatAppearance.MouseDownBackColor = UIntToColor(OK_DownBGColor);
			} 
			else if (buttonType == 2) {
				FlatAppearance.MouseOverBackColor = UIntToColor(NO_HoverBGColor);
				FlatAppearance.MouseDownBackColor = UIntToColor(NO_DownBGColor);
			} 
			else if (HoverBGColor != 0 && HoverBGColor != 0) {
				FlatAppearance.MouseOverBackColor = UIntToColor(HoverBGColor);
				FlatAppearance.MouseDownBackColor = UIntToColor(DownBGColor);
			} 
			else {
				FlatAppearance.MouseOverBackColor = UIntToColor(BTN_HoverBGColor);
				FlatAppearance.MouseDownBackColor = UIntToColor(BTN_DownBGColor);
			}
		}
		Color UIntToColor(uint color) {
		     byte a = (byte)(color >> 24 & 0xFF);
		     byte r = (byte)(color >> 16 & 0xFF);
		     byte g = (byte)(color >> 8 & 0xFF);
		     byte b = (byte)(color >> 0 & 0xFF);
		     return Color.FromArgb(a, r, g, b);
		}
		bool once=false;
		protected override void OnMouseMove(MouseEventArgs e) {
			if (!once) {
				ForeColor = Color.White;
				once = true;
			}
			base.OnMouseMove(e);
		}
		protected override void OnMouseLeave(EventArgs e) {
			ForeColor = _original_color;
			once = false;
			base.OnMouseLeave(e);
		}
	}
}
