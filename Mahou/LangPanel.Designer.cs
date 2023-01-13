// File created by BladeMight in 22.06.2017-17:48
namespace Mahou
{
	partial class LangPanel
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lbl_LayoutName;
		private System.Windows.Forms.PictureBox pct_Flag;
		private System.Windows.Forms.PictureBox pct_Upper;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.lbl_LayoutName = new System.Windows.Forms.Label();
			this.pct_Flag = new System.Windows.Forms.PictureBox();
			this.pct_Upper = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pct_Flag)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pct_Upper)).BeginInit();
			this.SuspendLayout();
			// 
			// lbl_LayoutName
			// 
			this.lbl_LayoutName.AutoSize = true;
			this.lbl_LayoutName.Location = new System.Drawing.Point(24, 4);
			this.lbl_LayoutName.Name = "lbl_LayoutName";
			this.lbl_LayoutName.Size = new System.Drawing.Size(70, 13);
			this.lbl_LayoutName.TabIndex = 0;
			this.lbl_LayoutName.Text = "Layout Name";
			this.lbl_LayoutName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl_LayoutName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LangPanelMouseDown);
			this.lbl_LayoutName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LangPanelMouseMove);
			this.lbl_LayoutName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Lbl_LayoutNameMouseUp);
			// 
			// pct_Flag
			// 
			this.pct_Flag.BackgroundImage = global::Mahou.Properties.Resources.jp;
			this.pct_Flag.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.pct_Flag.InitialImage = null;
			this.pct_Flag.Location = new System.Drawing.Point(4, 4);
			this.pct_Flag.Name = "pct_Flag";
			this.pct_Flag.Size = new System.Drawing.Size(16, 16);
			this.pct_Flag.TabIndex = 1;
			this.pct_Flag.TabStop = false;
			this.pct_Flag.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LangPanelMouseDown);
			this.pct_Flag.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LangPanelMouseMove);
			this.pct_Flag.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Lbl_LayoutNameMouseUp);
			// 
			// pct_Upper
			// 
			this.pct_Upper.BackgroundImage = global::Mahou.Properties.Resources.up;
			this.pct_Upper.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.pct_Upper.InitialImage = null;
			this.pct_Upper.Location = new System.Drawing.Point(20, 4);
			this.pct_Upper.Name = "pct_Upper";
			this.pct_Upper.Size = new System.Drawing.Size(16, 16);
			this.pct_Upper.TabIndex = 2;
			this.pct_Upper.TabStop = false;
			this.pct_Upper.Visible = false;
			this.pct_Upper.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LangPanelMouseDown);
			this.pct_Upper.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LangPanelMouseMove);
			this.pct_Upper.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Lbl_LayoutNameMouseUp);
			// 
			// LangPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(40, 24);
			this.ControlBox = false;
			this.Controls.Add(this.pct_Upper);
			this.Controls.Add(this.pct_Flag);
			this.Controls.Add(this.lbl_LayoutName);
			this.Cursor = System.Windows.Forms.Cursors.SizeAll;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(400, 24);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(40, 24);
			this.Name = "LangPanel";
			this.Opacity = 0.9D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "LangPanel";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.Color.Peru;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LangPanelMouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LangPanelMouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Lbl_LayoutNameMouseUp);
			((System.ComponentModel.ISupportInitialize)(this.pct_Flag)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pct_Upper)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
