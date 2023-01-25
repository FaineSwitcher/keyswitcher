using System.Drawing;
using System.Windows.Forms;

namespace FaineSwitch
{
    public partial class LangDisplay : Form
    {
        /// <summary>
        /// Used to determine if lang display is used for caret display.
        /// </summary>
        public bool lastTransparentBG, transparentBG, caretDisplay, mouseDisplay, DisplayFlag, onInit = true, Empty;
        public string lastText = "NO";
        Size lastsize = new Size(0, 0);
        public LangDisplay()
        {
            InitializeComponent();
            this.FormClosing += (s, e) => { e.Cancel = true; this.Hide(); };
            SetVisInvis();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
        }
        /// <summary>
        /// Change lang display text.
        /// </summary>
        /// <param name="to">Text to be changed to.</param>
        public void ChangeLD(string to)
        {
            if (to == lastText) return;
            lbLang.Text = lastText = to;
            if (DisplayFlag)
            {
                lbLang.Visible = false;
                SwitcherUI.RefreshFLAG(true);
                if (SwitcherUI.MouseTTAlways && mouseDisplay)
                {// fix for tray stuck due to variable "LayoutChanged" which being changed by this mouse tooltip always
                    var fi = Icon.FromHandle((
                            (SwitcherUI.TrayText && SwitcherUI.ITEXT != null) ? SwitcherUI.ITEXT : SwitcherUI.FLAG)
                        .GetHicon());
                    Program.switcher.icon.trIcon.Icon = fi;
                    WinAPI.DestroyIcon(fi.Handle);
                }
                BackgroundImage = new Bitmap(SwitcherUI.FLAG);
                TransparencyKey = BackColor = Color.Pink;
                Invalidate();
                Update();
            }
            if (!transparentBG) return;
            Invalidate();
            Update();
        }
        public void DisplayUpper(bool Upper)
        {
            var lastvis = pct_UpperArrow.Visible;
            if (Upper && !pct_UpperArrow.Visible)
                pct_UpperArrow.Visible = true;
            else if (!Upper && pct_UpperArrow.Visible)
                pct_UpperArrow.Visible = false;
            if (lastvis != pct_UpperArrow.Visible)
            {
                ReSize();
                if (Upper)
                {
                    pct_UpperArrow.Left = Width;
                    pct_UpperArrow.Top = (Height - 16) / 2 + 1;
                    Width = Width + 16;
                }
                lastsize = Size;
            }
        }
        /// <summary>
        /// Toggles lang display label visibility.
        /// </summary>
        public void SetVisInvis()
        {
            lbLang.Visible = !transparentBG;
            Invalidate();
            Update();
        }
        /// <summary>
        /// Refresh language text.
        /// </summary>
        public void RefreshLang()
        {
            uint cLid = Locales.GetCurrentLocale();
            if (DisplayFlag && SwitcherUI.DiffAppearenceForLayouts)
            {
                if (KMHook.CompareLayouts(cLid, SwitcherUI.MAIN_LAYOUT1) && SwitcherUI.Layout1TText.Length < 2)
                    lbLang.Text = SwitcherUI.Layout1TText;
                else if (KMHook.CompareLayouts(cLid, SwitcherUI.MAIN_LAYOUT2) && SwitcherUI.Layout2TText.Length < 2)
                    lbLang.Text = SwitcherUI.Layout2TText;
                Empty = (SwitcherUI.DiffAppearenceForLayouts && lbLang.Text == " ");
            }
            if (!Visible || Empty) return;
            if (cLid == 0)
                cLid = SwitcherUI.currentLayout;
            if (SwitcherUI.UseJKL && !KMHook.JKLERR)
                cLid = SwitcherUI.currentLayout;
            if (SwitcherUI.OneLayout)
                cLid = SwitcherUI.GlobalLayout;
            if (SwitcherUI.LDCaretTransparentBack_temp && caretDisplay)
                transparentBG = true;
            else if (SwitcherUI.LDMouseTransparentBack_temp && mouseDisplay)
                transparentBG = true;
            else transparentBG = false;
            var notTwo = false;
            if (cLid > 0)
            {
                System.Globalization.CultureInfo clangname = null;
                try
                {
                    clangname = new System.Globalization.CultureInfo((int)(cLid >> 16));
                }
                catch
                {
                    clangname = new System.Globalization.CultureInfo((int)(cLid & 0xffff));
                }
                if (clangname == null) return;
                if (SwitcherUI.DiffAppearenceForLayouts && !DisplayFlag)
                {
                    if (KMHook.CompareLayouts(cLid, SwitcherUI.MAIN_LAYOUT1))
                    {
                        ChangeColors(SwitcherUI.Layout1Font_temp, SwitcherUI.Layout1Fore_temp,
                                     SwitcherUI.Layout1Back_temp, SwitcherUI.Layout1TransparentBack_temp);
                        ChangeSize(SwitcherUI.Layout1Height_temp, SwitcherUI.Layout1Width_temp);
                        ChangeLD(SwitcherUI.Layout1TText);
                    }
                    else if (KMHook.CompareLayouts(cLid, SwitcherUI.MAIN_LAYOUT2))
                    {
                        ChangeColors(SwitcherUI.Layout2Font_temp, SwitcherUI.Layout2Fore_temp,
                                     SwitcherUI.Layout2Back_temp, SwitcherUI.Layout2TransparentBack_temp);
                        ChangeSize(SwitcherUI.Layout2Height_temp, SwitcherUI.Layout2Width_temp);
                        ChangeLD(SwitcherUI.Layout2TText);
                    }
                    else notTwo = true;
                }
                else notTwo = true;
                if (notTwo)
                {
                    if (mouseDisplay)
                    {
                        ChangeColors(SwitcherUI.LDMouseFont_temp, SwitcherUI.LDMouseFore_temp,
                                     SwitcherUI.LDMouseBack_temp, SwitcherUI.LDMouseTransparentBack_temp);
                        ChangeSize(SwitcherUI.LDMouseHeight_temp, SwitcherUI.LDMouseWidth_temp);
                    }
                    if (caretDisplay)
                    {
                        ChangeColors(SwitcherUI.LDCaretFont_temp, SwitcherUI.LDCaretFore_temp,
                                     SwitcherUI.LDCaretBack_temp, SwitcherUI.LDCaretTransparentBack_temp);
                        ChangeSize(SwitcherUI.LDCaretHeight_temp, SwitcherUI.LDCaretWidth_temp);
                    }
                    ChangeLD(clangname.ThreeLetterISOLanguageName.Substring(0, 1).ToUpper() + clangname.ThreeLetterISOLanguageName.Substring(1));
                }
                if (!SwitcherUI.DiffAppearenceForLayouts || lbLang.Text == "")
                    ChangeLD(clangname.ThreeLetterISOLanguageName.Substring(0, 1).ToUpper() + clangname.ThreeLetterISOLanguageName.Substring(1));
                if (transparentBG)
                    TransparencyKey = BackColor = lbLang.BackColor = pct_UpperArrow.BackColor = Color.Pink;
                if (lastTransparentBG != transparentBG)
                    SetVisInvis();
                lastTransparentBG = transparentBG;
                if ((caretDisplay && !SwitcherUI.caretLTUpperArrow) || (mouseDisplay && !SwitcherUI.mouseLTUpperArrow) || onInit)
                {
                    ReSize(); onInit = false;
                }
            }
            else
            {
                Logging.Log("Language tooltip text NOT changed, locale id = [" + cLid + "].", 2);
            }
        }
        void ReSize()
        {
            if (DisplayFlag)
            {
                if (BackgroundImage == null) return;
                if (lastsize == BackgroundImage.Size) return;
                Size = BackgroundImage.Size;
            }
            else
            {
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
        public void ChangeColors(Font fnt, Color fore, Color back, bool tBG)
        {
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
        public void ChangeSize(int height, int width)
        {
            lbLang.Height = height;
            lbLang.Width = width;
        }
        /// <summary>
        /// Show lang display form without activation.
        /// </summary>
        public void ShowInactiveTopmost()
        {
            if (Visible) return;
            try
            {
                WinAPI.ShowWindow(Handle, WinAPI.SW_SHOWNOACTIVATE);
                WinAPI.SetWindowPos(Handle.ToInt32(), WinAPI.HWND_TOPMOST,
                    Left, Top, Width, Height,
                    WinAPI.SWP_NOACTIVATE);
            }
            catch (System.Exception e)
            {
                Logging.Log(">> LD - Show error" + e.Message + e.StackTrace, 1);
            }
        }
        /// <summary>
        /// Hide lang display window.
        /// </summary>
        public void HideWnd()
        {
            if (!Visible) return;
            WinAPI.ShowWindow(Handle, 0);
        }
        protected override CreateParams CreateParams
        {
            get
            {
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
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!transparentBG || DisplayFlag) { base.OnPaint(e); return; }
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            e.Graphics.DrawString(lbLang.Text, lbLang.Font, new SolidBrush(lbLang.ForeColor), 0, 0);
            base.OnPaint(e);
        }
    }
}
