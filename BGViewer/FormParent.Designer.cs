namespace standScripter
{
	partial class FormParent
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormParent));
			this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.dockParentMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.プレビューの表示非表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.スクリプトの表示非表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ファイル一覧の表示非表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dockPanel1
			// 
			this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel1.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
			this.dockPanel1.Location = new System.Drawing.Point(0, 24);
			this.dockPanel1.Name = "dockPanel1";
			this.dockPanel1.Size = new System.Drawing.Size(800, 426);
			this.dockPanel1.TabIndex = 0;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.dockParentMenu});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(800, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// dockParentMenu
			// 
			this.dockParentMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.プレビューの表示非表示ToolStripMenuItem,
			this.スクリプトの表示非表示ToolStripMenuItem,
			this.ファイル一覧の表示非表示ToolStripMenuItem});
			this.dockParentMenu.Name = "dockParentMenu";
			this.dockParentMenu.Size = new System.Drawing.Size(150, 20);
			this.dockParentMenu.Text = "各ウインドウの表示切り替え";
			// 
			// プレビューの表示非表示ToolStripMenuItem
			// 
			this.プレビューの表示非表示ToolStripMenuItem.Name = "プレビューの表示非表示ToolStripMenuItem";
			this.プレビューの表示非表示ToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.プレビューの表示非表示ToolStripMenuItem.Text = "プレビューの表示/非表示";
			this.プレビューの表示非表示ToolStripMenuItem.Click += new System.EventHandler(this.プレビューの表示非表示ToolStripMenuItem_Click);
			// 
			// スクリプトの表示非表示ToolStripMenuItem
			// 
			this.スクリプトの表示非表示ToolStripMenuItem.Name = "スクリプトの表示非表示ToolStripMenuItem";
			this.スクリプトの表示非表示ToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.スクリプトの表示非表示ToolStripMenuItem.Text = "スクリプトの表示/非表示";
			this.スクリプトの表示非表示ToolStripMenuItem.Click += new System.EventHandler(this.スクリプトの表示非表示ToolStripMenuItem_Click);
			// 
			// ファイル一覧の表示非表示ToolStripMenuItem
			// 
			this.ファイル一覧の表示非表示ToolStripMenuItem.Name = "ファイル一覧の表示非表示ToolStripMenuItem";
			this.ファイル一覧の表示非表示ToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.ファイル一覧の表示非表示ToolStripMenuItem.Text = "ファイル一覧の表示/非表示";
			this.ファイル一覧の表示非表示ToolStripMenuItem.Click += new System.EventHandler(this.ファイル一覧の表示非表示ToolStripMenuItem_Click);
			// 
			// FormParent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.dockPanel1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "FormParent";
			this.Text = "立絵仮打ツール：スクリプト編集";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormParent_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormParent_FormClosed);
			this.Load += new System.EventHandler(this.FormParent_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem dockParentMenu;
		private System.Windows.Forms.ToolStripMenuItem プレビューの表示非表示ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem スクリプトの表示非表示ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ファイル一覧の表示非表示ToolStripMenuItem;
	}
}