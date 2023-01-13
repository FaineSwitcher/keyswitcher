using System;
using System.Windows.Forms;

namespace Mahou {
    public class TrayIcon {
        public event EventHandler<EventArgs> Exit, EnaDisable,
       		ShowHide, Restart, ConvertClip, TransliClip, ChangeLt, MLBAct;
        public NotifyIcon trIcon;
        ContextMenuStrip cMenu;
        ToolStripMenuItem Exi, ShHi, EnDis, Resta, ChLa;
        /// <summary> Clipboard menu item. </summary>
		ToolStripMenuItem Clip, CConvert, CTransli, CLast;
        /// <summary>Initializes new tray icon.</summary>
        /// <param name="visible">State of tray icon's visibility on initialize.</param>
        public TrayIcon(bool? visible = true) {
            trIcon = new NotifyIcon();
            cMenu = new ContextMenuStrip();
            trIcon.Icon = Properties.Resources.MahouTrayHD;
            trIcon.Visible = visible == true;
            CConvert = new ToolStripMenuItem("Convert",null,ConvertClipHandler);
            CTransli = new ToolStripMenuItem("Transliterate",null,TransliClipHandler);
            CLast = new ToolStripMenuItem("Latest: (not implemented)"); // Dynamically add the last later
            Clip = new ToolStripMenuItem("Clipboard"); Clip.DropDownItems.AddRange(new []{CConvert, CTransli, CLast});
            Exi = new ToolStripMenuItem("Exit",null,ExitHandler);
            ShHi = new ToolStripMenuItem("Show",null,ShowHideHandler);
            EnDis = new ToolStripMenuItem("Enable",null,EnaDisableHandler);
            Resta = new ToolStripMenuItem("Restart",null,RestartHandler);
            ChLa = new ToolStripMenuItem("Change layout",null,ChangeLayout);
            EnDis.Checked = true;
            cMenu.Items.Add(ShHi);
            cMenu.Items.Add(Clip);
            cMenu.Items.Add(EnDis);
            cMenu.Items.Add(ChLa);
            cMenu.Items.Add(Resta);
            cMenu.Items.Add(Exi);
            cMenu.BackColor = System.Drawing.SystemColors.Control;
            cMenu.RenderMode = ToolStripRenderMode.System;
            trIcon.Text = "Mahou (魔法)\nA magical layout switcher.";
            trIcon.ContextMenuStrip = cMenu;
            trIcon.MouseClick += (_,__) => { if (__.Button == MouseButtons.Left) MLBAction(_,__); };
            trIcon.MouseDoubleClick += (_,__) => { if (__.Button == MouseButtons.Left) MLBAction(_,__); };
        }
        public void CheckShHi(bool shhi) {
        	ShHi.Checked = shhi;
        }
        public void CheckEnDis(bool endis) {
        	EnDis.Checked = endis;
        }
        /// <summary>Transliterate the text in clipboard.</summary>
        void TransliClipHandler(object sender, EventArgs e) {
            if (TransliClip != null) TransliClip(this, null);
        }
        /// <summary>Convert the text in clipboard.</summary>
        void ConvertClipHandler(object sender, EventArgs e) {
            if (ConvertClip != null) ConvertClip(this, null);
        }
        /// <summary>Toggle Mahou, enable/disable event handler..</summary>
        void EnaDisableHandler(object sender, EventArgs e) {
            if (EnaDisable != null) EnaDisable(this, null);
        }
        /// <summary>Restart event handler..</summary>
        void ChangeLayout(object sender, EventArgs e) {
            if (ChangeLt != null) ChangeLt(this, null);
        }
        /// <summary>Restart event handler..</summary>
        void RestartHandler(object sender, EventArgs e) {
            if (Restart != null) Restart(this, null);
        }
        /// <summary>Exit event handler..</summary>
        void ExitHandler(object sender, EventArgs e) {
            if (Exit != null) Exit(this, null);
        }
        /// <summary>ShowHide event handler..</summary>
        void ShowHideHandler(object sender, EventArgs e) {
            if (ShowHide != null) ShowHide(this, null);
        }
        /// <summary>ShowHide event handler..</summary>
        void MLBAction(object sender, EventArgs e) {
            if (MLBAct != null) MLBAct(sender, e);
        }
        /// <summary>Hides tray icon.</summary>
        public void Hide() {
        	if ((MahouUI.TrayFlags || MahouUI.TrayText) && MMain.mahou != null && MMain.mahou.flagsCheck != null)
        		MMain.mahou.flagsCheck.Stop();
            trIcon.Visible = false;
        }
        /// <summary>Shows tray icon.</summary>
        public void Show() {
        	if (MahouUI.TrayFlags || MahouUI.TrayText)
        		MMain.mahou.flagsCheck.Start();
            trIcon.Visible = true;
        }
        /// <summary>Refreshes tray icon various text.</summary>
        public void RefreshText(string TrText, string ShHiText, string ExiText, string EnaDisText,
                                string RestartText, string ConvClipText, string TransliClipText, string ClipbText, string LatestText, string ChLaText) {
            trIcon.Text = TrText;
            if (!String.IsNullOrEmpty(ShHiText))
            	ShHi.Text = ShHiText;
            if (!String.IsNullOrEmpty(ExiText))
            	Exi.Text = ExiText;
            if (!String.IsNullOrEmpty(EnaDisText))
            	EnDis.Text = EnaDisText;
            if (!String.IsNullOrEmpty(ChLaText))
            	ChLa.Text = ChLaText;
            if (!String.IsNullOrEmpty(RestartText))
            	Resta.Text = RestartText;
            if (!String.IsNullOrEmpty(ConvClipText))
            	CConvert.Text = ConvClipText;
            if (!String.IsNullOrEmpty(TransliClipText))
            	CTransli.Text = TransliClipText;
            if (!String.IsNullOrEmpty(ClipbText))
            	Clip.Text = ClipbText;
            if (!String.IsNullOrEmpty(LatestText))
            	CLast.Text = LatestText;
        }
    }
}
