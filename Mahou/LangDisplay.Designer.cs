namespace Mahou
{
	partial class LangDisplay
	{
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lbLang;
		private System.Windows.Forms.PictureBox pct_UpperArrow;
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.lbLang = new System.Windows.Forms.Label();
			this.pct_UpperArrow = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pct_UpperArrow)).BeginInit();
			this.SuspendLayout();
			// 
			// lbLang
			// 
			this.lbLang.BackColor = System.Drawing.Color.Black;
			this.lbLang.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.lbLang.Font = new System.Drawing.Font("Segoe UI", 7F);
			this.lbLang.ForeColor = System.Drawing.Color.White;
			this.lbLang.Location = new System.Drawing.Point(0, 0);
			this.lbLang.Name = "lbLang";
			this.lbLang.Size = new System.Drawing.Size(15, 14);
			this.lbLang.TabIndex = 0;
			this.lbLang.Text = "Ru";
			this.lbLang.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pct_UpperArrow
			// 
			this.pct_UpperArrow.BackgroundImage = global::Mahou.Properties.Resources.up;
			this.pct_UpperArrow.Location = new System.Drawing.Point(16, 0);
			this.pct_UpperArrow.Name = "pct_UpperArrow";
			this.pct_UpperArrow.Size = new System.Drawing.Size(16, 16);
			this.pct_UpperArrow.TabIndex = 1;
			this.pct_UpperArrow.TabStop = false;
			this.pct_UpperArrow.Visible = false;
			// 
			// LangDisplay
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Black;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(32, 16);
			this.ControlBox = false;
			this.Controls.Add(this.pct_UpperArrow);
			this.Controls.Add(this.lbLang);
			this.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LangDisplay";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "this";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.Color.Pink;
			((System.ComponentModel.ISupportInitialize)(this.pct_UpperArrow)).EndInit();
			this.ResumeLayout(false);

		}
	}
}
