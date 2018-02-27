namespace MetaGraffiti.App.GpxGraffiti
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.toolbar = new System.Windows.Forms.ToolStrip();
			this.lblPath = new System.Windows.Forms.ToolStripLabel();
			this.txtPath = new System.Windows.Forms.ToolStripTextBox();
			this.cmdPath = new System.Windows.Forms.ToolStripButton();
			this.separator1 = new System.Windows.Forms.ToolStripSeparator();
			this.cmdFix = new System.Windows.Forms.ToolStripButton();
			this.separator2 = new System.Windows.Forms.ToolStripSeparator();
			this.cmdSaveGpx = new System.Windows.Forms.ToolStripButton();
			this.cmdSaveKml = new System.Windows.Forms.ToolStripButton();
			this.listFiles = new System.Windows.Forms.CheckedListBox();
			this.listInfo = new System.Windows.Forms.ListBox();
			this.toolbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolbar
			// 
			this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblPath,
            this.txtPath,
            this.cmdPath,
            this.separator1,
            this.cmdFix,
            this.separator2,
            this.cmdSaveGpx,
            this.cmdSaveKml});
			this.toolbar.Location = new System.Drawing.Point(0, 0);
			this.toolbar.Name = "toolbar";
			this.toolbar.Size = new System.Drawing.Size(702, 25);
			this.toolbar.TabIndex = 0;
			this.toolbar.Text = "toolStrip1";
			// 
			// lblPath
			// 
			this.lblPath.Name = "lblPath";
			this.lblPath.Size = new System.Drawing.Size(40, 22);
			this.lblPath.Text = "Path: ";
			// 
			// txtPath
			// 
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(200, 25);
			// 
			// cmdPath
			// 
			this.cmdPath.Image = ((System.Drawing.Image)(resources.GetObject("cmdPath.Image")));
			this.cmdPath.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdPath.Name = "cmdPath";
			this.cmdPath.Size = new System.Drawing.Size(57, 22);
			this.cmdPath.Text = "Load";
			this.cmdPath.Click += new System.EventHandler(this.cmdPath_Click);
			// 
			// separator1
			// 
			this.separator1.Name = "separator1";
			this.separator1.Size = new System.Drawing.Size(6, 25);
			// 
			// cmdFix
			// 
			this.cmdFix.Checked = true;
			this.cmdFix.CheckOnClick = true;
			this.cmdFix.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cmdFix.Image = ((System.Drawing.Image)(resources.GetObject("cmdFix.Image")));
			this.cmdFix.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdFix.Name = "cmdFix";
			this.cmdFix.Size = new System.Drawing.Size(43, 22);
			this.cmdFix.Text = "Fix";
			// 
			// separator2
			// 
			this.separator2.Name = "separator2";
			this.separator2.Size = new System.Drawing.Size(6, 25);
			// 
			// cmdSaveGpx
			// 
			this.cmdSaveGpx.Image = ((System.Drawing.Image)(resources.GetObject("cmdSaveGpx.Image")));
			this.cmdSaveGpx.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdSaveGpx.Name = "cmdSaveGpx";
			this.cmdSaveGpx.Size = new System.Drawing.Size(83, 22);
			this.cmdSaveGpx.Text = "Save GPX";
			// 
			// cmdSaveKml
			// 
			this.cmdSaveKml.Image = ((System.Drawing.Image)(resources.GetObject("cmdSaveKml.Image")));
			this.cmdSaveKml.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdSaveKml.Name = "cmdSaveKml";
			this.cmdSaveKml.Size = new System.Drawing.Size(85, 22);
			this.cmdSaveKml.Text = "Save KML";
			// 
			// listFiles
			// 
			this.listFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listFiles.FormattingEnabled = true;
			this.listFiles.Location = new System.Drawing.Point(0, 28);
			this.listFiles.Name = "listFiles";
			this.listFiles.Size = new System.Drawing.Size(350, 394);
			this.listFiles.TabIndex = 1;
			this.listFiles.SelectedIndexChanged += new System.EventHandler(this.listFiles_SelectedIndexChanged);
			// 
			// listInfo
			// 
			this.listInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listInfo.FormattingEnabled = true;
			this.listInfo.Location = new System.Drawing.Point(352, 28);
			this.listInfo.Name = "listInfo";
			this.listInfo.Size = new System.Drawing.Size(350, 394);
			this.listInfo.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(702, 427);
			this.Controls.Add(this.listInfo);
			this.Controls.Add(this.listFiles);
			this.Controls.Add(this.toolbar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "GpxGraffiti";
			this.toolbar.ResumeLayout(false);
			this.toolbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolbar;
		private System.Windows.Forms.ToolStripLabel lblPath;
		private System.Windows.Forms.ToolStripTextBox txtPath;
		private System.Windows.Forms.ToolStripButton cmdPath;
		private System.Windows.Forms.ToolStripSeparator separator1;
		private System.Windows.Forms.CheckedListBox listFiles;
		private System.Windows.Forms.ListBox listInfo;
		private System.Windows.Forms.ToolStripButton cmdSaveGpx;
		private System.Windows.Forms.ToolStripButton cmdFix;
		private System.Windows.Forms.ToolStripButton cmdSaveKml;
		private System.Windows.Forms.ToolStripSeparator separator2;
	}
}

