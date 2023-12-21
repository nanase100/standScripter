using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace standScripter
{
	public partial class FormParent : Form
	{
		public MainForm				m_parent		= null;
		public DockFormScriptList		m_scriptList	= null;
		public DockFormPreview			m_preview		= null;
		public DockFormBlockList		m_blockList		= null;
			
		public FormParent(MainForm parent)
		{
			InitializeComponent();

			m_parent = parent;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormParent_Load(object sender, EventArgs e)
		{
			m_scriptList			= new DockFormScriptList();
			m_scriptList.m_parent	= m_parent;
			m_scriptList.Text		= "スクリプト一覧";
			//m_scriptList.Show(dockPanel1);

			m_preview			= new DockFormPreview();
			m_preview.m_parent	= m_parent;
			m_preview.Text		= "プレビュー";
			//m_preview.Show(dockPanel1);

			m_blockList				= new DockFormBlockList();
			m_blockList.m_parent	= m_parent;
			m_blockList.Text		= "スクリプト内容";
			//m_blockList.Show(dockPanel1);

			LayoutLoad(); // レイアウト読み込み

			this.Left	= m_parent.m_dataManager.dockingBasePos.Left;
			this.Top	= m_parent.m_dataManager.dockingBasePos.Top;
			this.Width	= m_parent.m_dataManager.dockingBasePos.Width;
			this.Height	= m_parent.m_dataManager.dockingBasePos.Height;
		}

		/// <summary>
		/// 
		/// </summary>
		public void StockPos()
		{

			if (this.WindowState == FormWindowState.Normal)
			{
				
				m_parent.m_dataManager.dockingBasePos = new Rectangle( this.Left,this.Top, this.Width, this.Height );
			}
			else
			{
				m_parent.m_dataManager.dockingBasePos = new Rectangle( this.RestoreBounds.Left,this.RestoreBounds.Top, this.RestoreBounds.Width, this.RestoreBounds.Height );
			}

			LayoutSave();
		}

		private void FormParent_FormClosed(object sender, FormClosedEventArgs e)
		{
			//StockPos();
		}

		private void FormParent_FormClosing(object sender, FormClosingEventArgs e)
		{
			if( m_parent.m_closeFrom == 0 )
			{
				m_parent.m_closeFrom = 1;
				m_parent.Close();
			}

			if( m_parent.m_isCancelCloseFromDock == true )
			{
				e.Cancel = true;
			}
			//m_scriptList?.Dispose();
		}

		private string LayoutFilePath
		{
			get
			{
				//カレントディレクトリではなく、exeの固定位置
				//string exePath = Process.GetCurrentProcess().MainModule.FileName;
				//return System.IO.Path.ChangeExtension(exePath, "layout.xml");

				string exePath = System.IO.Directory.GetCurrentDirectory().ToString();
				return System.IO.Path.Combine(exePath, "layout.xml");
				
			}
		}

		/// <summary>
		/// レイアウト読み込み
		/// </summary>
		private void LayoutLoad()
		{
			try
			{
				DeserializeDockContent deserializeDockContent 	= new DeserializeDockContent(GetDockContent);
				dockPanel1.LoadFromXml(LayoutFilePath, deserializeDockContent);

				m_parent.Show(dockPanel1);
				m_scriptList.Show(dockPanel1);
				m_preview.Show(dockPanel1);
				m_blockList.Show(dockPanel1);

				
			}
			catch(Exception ee)
			{
				// 初回起動など保存ファイルがない場合などならレイアウト無視で全部表示開始
				m_parent.Show(dockPanel1);
				m_scriptList.Show(dockPanel1);
				m_preview.Show(dockPanel1);
				m_blockList.Show(dockPanel1);

				
			}
		}
 
		// レイアウト保存
		private void LayoutSave()
		{
			dockPanel1.SaveAsXml(LayoutFilePath);
		}
 
		// レイアウト読み込み時の保存名からどのフォームかを判定する処理
		private IDockContent GetDockContent(string persistString)
		{
			
			switch(persistString)
			{
				case "DockFormBlockListStr":	return m_blockList;		break;
				case "DockFormScriptListStr":	return m_scriptList;	break;
				case "DockFormPreviewStr":		return m_preview;		break;
				case "MainForm":				return m_parent;		break;
			}

			return null;
		}
 

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			LayoutSave(); // レイアウト保存
 
			// 掃除
			m_blockList?.Dispose();
			m_scriptList?.Dispose();
			m_preview?.Dispose();

			m_parent?.Dispose();
		}
 
		private void windowToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
		}
 


		private void プレビューの表示非表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(m_preview.IsHidden) { m_preview.Show(); } else { m_preview.Hide(); } // Form2 の再表、非表示切り替え
		}

		private void スクリプトの表示非表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(m_blockList.IsHidden) { m_blockList.Show(); } else { m_blockList.Hide(); } // Form2 の再表、非表示切り替え
		}

		private void ファイル一覧の表示非表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(m_scriptList.IsHidden) { m_scriptList.Show(); } else { m_scriptList.Hide(); } // Form2 の再表、非表示切り替え
		}


		private void FormParent_KeyDown(object sender, KeyEventArgs e)
		{
			if( e.KeyCode == Keys.S && e.Modifiers == Keys.Control) m_blockList.Save();
		}

		private void dockPanel1_DragDrop(object sender, DragEventArgs e)
		{	

		}
	}
}
