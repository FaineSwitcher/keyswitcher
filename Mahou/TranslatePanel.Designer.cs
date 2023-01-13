using JustUI;
using System.Drawing;
using System.Windows.Forms;

namespace Mahou
{
	partial class TranslatePanel
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private JustUI.JUIButton X;
		private JustUI.JUITitle TITLE;
		private ColorPanel pan_Translations;
		MahouUI.TextBoxCA txt_Source;
		private System.Windows.Forms.Button bull;
		
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
			this.X = new JustUI.JUIButton();
			this.TITLE = new JustUI.JUITitle();
			this.pan_Translations = new Mahou.TranslatePanel.ColorPanel();
			this.txt_Source = new Mahou.MahouUI.TextBoxCA();
			this.bull = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// X
			// 
			this.X.BackColor = System.Drawing.SystemColors.Window;
			this.X.FlatAppearance.BorderSize = 0;
			this.X.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(79)))), ((int)(((byte)(79)))));
			this.X.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
			this.X.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.X.ForeColor = System.Drawing.SystemColors.WindowText;
			this.X.Location = new System.Drawing.Point(140, 0);
			this.X.Name = "X";
			this.X.Size = new System.Drawing.Size(22, 23);
			this.X.TabIndex = 0;
			this.X.TabStop = false;
			this.X.Text = "X";
			this.X.UseVisualStyleBackColor = true;
			this.X.MouseUp += new System.Windows.Forms.MouseEventHandler(this.XClick);
			// 
			// TITLE
			// 
			this.TITLE.BackColor = System.Drawing.SystemColors.Window;
			this.TITLE.ForeColor = System.Drawing.SystemColors.WindowText;
			this.TITLE.Location = new System.Drawing.Point(0, 0);
			this.TITLE.Name = "TITLE";
			this.TITLE.Size = new System.Drawing.Size(100, 23);
			this.TITLE.TabIndex = 1;
			this.TITLE.Text = "Translation";
			this.TITLE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pan_Translations
			// 
			this.pan_Translations.Location = new System.Drawing.Point(13, 67);
			this.pan_Translations.Name = "pan_Translations";
			this.pan_Translations.Size = new System.Drawing.Size(149, 71);
			this.pan_Translations.TabIndex = 3;
			// 
			// txt_Source
			// 
			this.txt_Source.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txt_Source.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txt_Source.Location = new System.Drawing.Point(13, 28);
			this.txt_Source.Name = "txt_Source";
			this.txt_Source.ReadOnly = true;
			this.txt_Source.Size = new System.Drawing.Size(100, 13);
			this.txt_Source.TabIndex = 4;
			this.txt_Source.TabStop = false;
			this.txt_Source.Text = "source text";
			// 
			// bull
			// 
			this.bull.Location = new System.Drawing.Point(0, 0);
			this.bull.Name = "bull";
			this.bull.Size = new System.Drawing.Size(0, 0);
			this.bull.TabIndex = 5;
			this.bull.Text = "bull";
			this.bull.UseVisualStyleBackColor = true;
			// 
			// TranslatePanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(172, 179);
			this.ControlBox = false;
			this.Controls.Add(this.bull);
			this.Controls.Add(this.txt_Source);
			this.Controls.Add(this.pan_Translations);
			this.Controls.Add(this.TITLE);
			this.Controls.Add(this.X);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(800, 400);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(40, 24);
			this.Name = "TranslatePanel";
			this.Opacity = 0.9D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "TranslatePanel";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.Color.Peru;
			this.Deactivate += new System.EventHandler(this.TranslatePanelDeactivate);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
