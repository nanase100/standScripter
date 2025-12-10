namespace standScripter
{
    partial class DockFormScriptText
	{
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockFormScriptText));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tbLineList = new System.Windows.Forms.RichTextBox();
			this.tbMainText = new System.Windows.Forms.RichTextBox();
			this.tbLineList2 = new System.Windows.Forms.RichTextBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.tbMainText2 = new System.Windows.Forms.RichTextBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(16, 60);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(275, 573);
			this.tabControl1.TabIndex = 2;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.tbLineList);
			this.tabPage1.Controls.Add(this.tbMainText);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(282, 588);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "元スクリプト";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.tbLineList2);
			this.tabPage2.Controls.Add(this.tbMainText2);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(267, 547);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "編集中のスクリプト";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tbLineList
			// 
			this.tbLineList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tbLineList.BackColor = System.Drawing.SystemColors.Window;
			this.tbLineList.Location = new System.Drawing.Point(6, 6);
			this.tbLineList.Name = "tbLineList";
			this.tbLineList.ReadOnly = true;
			this.tbLineList.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.tbLineList.Size = new System.Drawing.Size(52, 576);
			this.tbLineList.TabIndex = 3;
			this.tbLineList.Text = "";
			this.tbLineList.WordWrap = false;
			// 
			// tbMainText
			// 
			this.tbMainText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbMainText.BackColor = System.Drawing.SystemColors.Window;
			this.tbMainText.Location = new System.Drawing.Point(54, 6);
			this.tbMainText.Name = "tbMainText";
			this.tbMainText.ReadOnly = true;
			this.tbMainText.Size = new System.Drawing.Size(222, 576);
			this.tbMainText.TabIndex = 2;
			this.tbMainText.Text = "";
			this.tbMainText.WordWrap = false;
			this.tbMainText.VScroll += new System.EventHandler(this.text_VScroll);
			// 
			// tbLineList2
			// 
			this.tbLineList2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tbLineList2.BackColor = System.Drawing.SystemColors.Window;
			this.tbLineList2.Location = new System.Drawing.Point(6, 6);
			this.tbLineList2.Name = "tbLineList2";
			this.tbLineList2.ReadOnly = true;
			this.tbLineList2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.tbLineList2.Size = new System.Drawing.Size(52, 535);
			this.tbLineList2.TabIndex = 5;
			this.tbLineList2.Text = "";
			this.tbLineList2.WordWrap = false;
			// 
			// checkBox1
			// 
			this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(43, 639);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(244, 16);
			this.checkBox1.TabIndex = 3;
			this.checkBox1.Text = "プレビューのメッセージ位置と同期スクロールする";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// tbMainText2
			// 
			this.tbMainText2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbMainText2.BackColor = System.Drawing.SystemColors.Window;
			this.tbMainText2.Location = new System.Drawing.Point(54, 6);
			this.tbMainText2.Name = "tbMainText2";
			this.tbMainText2.ReadOnly = true;
			this.tbMainText2.Size = new System.Drawing.Size(207, 535);
			this.tbMainText2.TabIndex = 6;
			this.tbMainText2.Text = "";
			this.tbMainText2.WordWrap = false;
			this.tbMainText2.VScroll += new System.EventHandler(this.text_VScroll);
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.button1);
			this.groupBox4.Controls.Add(this.textBox1);
			this.groupBox4.Location = new System.Drawing.Point(16, 12);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(267, 42);
			this.groupBox4.TabIndex = 11;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "検索";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(203, 11);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(58, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "検索";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(6, 13);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(191, 19);
			this.textBox1.TabIndex = 0;
			this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
			// 
			// DockFormScriptText
			// 
			this.ClientSize = new System.Drawing.Size(299, 667);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.tabControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DockFormScriptText";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DockFormScriptText_FormClosing);
			this.Load += new System.EventHandler(this.FormScriptList2_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox tbLineList;
        private System.Windows.Forms.RichTextBox tbMainText;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox tbLineList2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RichTextBox tbMainText2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
    }
}
