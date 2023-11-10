namespace standScripter
{
	partial class DockFormBlockList
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockFormBlockList));
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.message = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.BG = new System.Windows.Forms.DataGridViewImageColumn();
			this.Bunk1 = new System.Windows.Forms.DataGridViewImageColumn();
			this.Bunk2 = new System.Windows.Forms.DataGridViewImageColumn();
			this.Bunk3 = new System.Windows.Forms.DataGridViewImageColumn();
			this.Bunk4 = new System.Windows.Forms.DataGridViewImageColumn();
			this.Bunk5 = new System.Windows.Forms.DataGridViewImageColumn();
			this.colFace = new System.Windows.Forms.DataGridViewImageColumn();
			this.comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.message,
            this.BG,
            this.Bunk1,
            this.Bunk2,
            this.Bunk3,
            this.Bunk4,
            this.Bunk5,
            this.colFace,
            this.comment});
			this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dataGridView1.Enabled = false;
			this.dataGridView1.Location = new System.Drawing.Point(12, 64);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.RowTemplate.Height = 21;
			this.dataGridView1.Size = new System.Drawing.Size(1031, 418);
			this.dataGridView1.TabIndex = 1;
			this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
			this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
			this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
			this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
			this.dataGridView1.CurrentCellChanged += new System.EventHandler(this.dataGridView1_CurrentCellChanged);
			this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
			this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
			// 
			// message
			// 
			this.message.HeaderText = "メッセージ";
			this.message.Name = "message";
			this.message.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.message.Width = 250;
			// 
			// BG
			// 
			this.BG.HeaderText = "背景";
			this.BG.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
			this.BG.Name = "BG";
			this.BG.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// Bunk1
			// 
			this.Bunk1.HeaderText = "立絵バンク1";
			this.Bunk1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
			this.Bunk1.Name = "Bunk1";
			this.Bunk1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// Bunk2
			// 
			this.Bunk2.HeaderText = "立絵バンク2";
			this.Bunk2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
			this.Bunk2.Name = "Bunk2";
			this.Bunk2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// Bunk3
			// 
			this.Bunk3.HeaderText = "立絵バンク3";
			this.Bunk3.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
			this.Bunk3.Name = "Bunk3";
			this.Bunk3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// Bunk4
			// 
			this.Bunk4.HeaderText = "立絵バンク4";
			this.Bunk4.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
			this.Bunk4.Name = "Bunk4";
			this.Bunk4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// Bunk5
			// 
			this.Bunk5.HeaderText = "立絵バンク5";
			this.Bunk5.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
			this.Bunk5.Name = "Bunk5";
			// 
			// colFace
			// 
			this.colFace.HeaderText = "顔グラ";
			this.colFace.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
			this.colFace.Name = "colFace";
			this.colFace.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// comment
			// 
			this.comment.HeaderText = "コメント(+命令)";
			this.comment.Name = "comment";
			this.comment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.comment.Width = 200;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Location = new System.Drawing.Point(12, 9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(127, 49);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "セルサイズの拡大縮小";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(62, 18);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(50, 25);
			this.button3.TabIndex = 1;
			this.button3.Text = "縮小－";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(6, 18);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(50, 25);
			this.button2.TabIndex = 0;
			this.button2.Text = "拡大＋";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button4
			// 
			this.button4.Enabled = false;
			this.button4.Location = new System.Drawing.Point(6, 15);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(130, 24);
			this.button4.TabIndex = 5;
			this.button4.Text = "保存";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.button5);
			this.groupBox2.Controls.Add(this.button6);
			this.groupBox2.Location = new System.Drawing.Point(145, 9);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(124, 49);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "フォントの拡大縮小";
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(62, 18);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(50, 25);
			this.button5.TabIndex = 1;
			this.button5.Text = "縮小－";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(6, 18);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(50, 25);
			this.button6.TabIndex = 0;
			this.button6.Text = "拡大＋";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.button4);
			this.groupBox3.Location = new System.Drawing.Point(511, 13);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(144, 45);
			this.groupBox3.TabIndex = 9;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "編集中のスクリプトの保存";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.button1);
			this.groupBox4.Controls.Add(this.textBox1);
			this.groupBox4.Location = new System.Drawing.Point(275, 9);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(230, 49);
			this.groupBox4.TabIndex = 10;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "検索";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(150, 21);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(58, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "検索";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(6, 21);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(138, 19);
			this.textBox1.TabIndex = 0;
			this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
			// 
			// fileSystemWatcher1
			// 
			this.fileSystemWatcher1.EnableRaisingEvents = true;
			this.fileSystemWatcher1.SynchronizingObject = this;
			// 
			// DockFormBlockList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1055, 494);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.dataGridView1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DockFormBlockList";
			this.Text = "スクリプトブロック一覧";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBlockList_FormClosing);
			this.Load += new System.EventHandler(this.FormBlockList_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGridViewTextBoxColumn comment;
		private System.Windows.Forms.DataGridViewImageColumn Bunk5;
		private System.Windows.Forms.DataGridViewImageColumn Bunk4;
		private System.Windows.Forms.DataGridViewImageColumn Bunk3;
		private System.Windows.Forms.DataGridViewImageColumn Bunk2;
		private System.Windows.Forms.DataGridViewImageColumn Bunk1;
		private System.Windows.Forms.DataGridViewImageColumn BG;
		private System.Windows.Forms.DataGridViewTextBoxColumn message;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewImageColumn colFace;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.IO.FileSystemWatcher fileSystemWatcher1;
	}
}
