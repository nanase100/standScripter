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
		public DockStandList			m_standList		= null;
		public DockFormScriptList		m_scriptList	= null;
		public DockFormPreview			m_preview		= null;
		public DockFormBlockList		m_blockList		= null;

		public string					m_nowEditScriptName = "";
			

		public int						m_nowSelectBlockNo	= 0;
		public int						m_nowSelectBankNo	= 0;


		public bool		m_isEdit = false;			//開いたファイルを編集したかフラグ。保存や閉じる時の確認に使用

		public DataManger		m_dataManager		= new DataManger();
		
		public scenarioManager m_scenarioManager = new scenarioManager();


		public FormParent()
		{
			InitializeComponent();

		}


		public void LoadScriptFile( string scriptFileName )
		{

			if( m_isEdit )
			{
				if(  MessageBox.Show("編集されたファイルが保存されていませんが、\n別のスクリプトを開いてよろしいですか","確認",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No )
				{
					return;
				}
			}

			m_isEdit = false;

			m_nowEditScriptName = scriptFileName;

			string path = m_dataManager.m_gameDir + "/scene/" + scriptFileName+".txt";
			m_scenarioManager.Load(path);
			SetBlockTxtToList();

			m_blockList.SetActiveScript( scriptFileName );
			m_blockList.DataGrdiView(true);


			this.Text =  "立絵仮打ツール：" + scriptFileName;

		}

		public void SetBlockTxtToList()
		{
			m_blockList.CopyBlockData(m_scenarioManager.m_toolBlockList);
			m_blockList.UpdateBlockTxtToList(true);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormParent_Load(object sender, EventArgs e)
		{

			if( m_dataManager.SettingLoad("option.txt") == false )
			{
				m_dataManager.SettingLoad("_option.txt", true);
			}

			m_scriptList			= new DockFormScriptList();
			m_scriptList.m_parent	= this;
			m_scriptList.Text		= "スクリプト一覧";

			m_preview			= new DockFormPreview();
			m_preview.m_parent	= this;
			m_preview.Text		= "プレビュー";

			m_blockList				= new DockFormBlockList();
			m_blockList.m_parent	= this;
			m_blockList.Text		= "スクリプト内容";

			m_standList				= new DockStandList();
			m_standList.m_parent	= this;

			LayoutLoad(); // レイアウト読み込み

			this.Left	= m_dataManager.dockingBasePos.Left;
			this.Top	= m_dataManager.dockingBasePos.Top;
			this.Width	= m_dataManager.dockingBasePos.Width;
			this.Height	= m_dataManager.dockingBasePos.Height;
		}

		public void SetPanelBound( int ID, Rectangle rect)
		{
			dockPanel1.Panes[ID].SetBounds(rect.Left,rect.Top,rect.Width,rect.Height);
		}

		/// <summary>
		/// 
		/// </summary>
		public void StockPos()
		{

			if (this.WindowState == FormWindowState.Normal)
			{
				
				m_dataManager.dockingBasePos = new Rectangle( this.Left,this.Top, this.Width, this.Height );
			}
			else
			{
				m_dataManager.dockingBasePos = new Rectangle( this.RestoreBounds.Left,this.RestoreBounds.Top, this.RestoreBounds.Width, this.RestoreBounds.Height );
			}

			LayoutSave();
		}

		private void FormParent_FormClosed(object sender, FormClosedEventArgs e)
		{
			//StockPos();
		}

		private void FormParent_FormClosing(object sender, FormClosingEventArgs e)
		{

			if( m_standList.m_isCancelCloseFromDock == true )
			{
				e.Cancel = true;
			}
			else
			{
				if(  MessageBox.Show("ツールを終了してもよろしいですか？","確認",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No )
				{
					e.Cancel = true;

				}
				else if( m_isEdit )
				{
					if(  MessageBox.Show("編集中のファイルが保存されていない状態ですか、本当に終了してよろしいですか？","確認",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No )
					{
						e.Cancel = true;
					}
				}

			}
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

				m_standList.Show(dockPanel1);
				m_scriptList.Show(dockPanel1);
				m_preview.Show(dockPanel1);
				m_blockList.Show(dockPanel1);

			}
			catch(Exception ee)
			{
				// 初回起動など保存ファイルがない場合などならレイアウト無視で全部表示開始
				m_standList.Show(dockPanel1);
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
				case "DockFormBlockListStr":	return m_blockList;	
				case "DockFormScriptListStr":	return m_scriptList;
				case "DockFormPreviewStr":		return m_preview;	
				case "MainForm":				return m_standList;	
			}

			return null;
		}
 

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			LayoutSave(); // レイアウト保存
 
			m_blockList?.Dispose();
			m_scriptList?.Dispose();
			m_preview?.Dispose();
			m_standList?.Dispose();
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

		private void ビューワーの表示非表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(m_standList.IsHidden) {	m_standList.m_isCallFromBlocklist = false;	m_standList.Show(); } else { m_standList.Hide(); } // Form2 の再表、非表示切り替え
		}



		//-----------------------------------------------------------------------------------
		//立ち絵仮設定ツール追加機能。
		//-----------------------------------------------------------------------------------
		public void SetStand( string thumbName, int bankNo, string sizeType )
		{
			m_blockList.SetStand( thumbName, bankNo, sizeType );
			SetEditFlg();
		}

		public void SetBG( string bgName )
		{
			m_blockList.SetBG(bgName);
			SetEditFlg();
		}

		public void SetFace( string faceName )
		{
			m_blockList.SetFace(faceName);
			SetEditFlg();
		}

		public void SetEditFlg( bool flg =true)
		{
			m_isEdit = flg;

			this.Text =  "立絵仮打ツール：" + m_nowEditScriptName + (m_isEdit?"※":"");
				
		}
		
	}
}
