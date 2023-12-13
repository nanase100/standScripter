using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace standScripter
{
	public partial class DockFormScriptList : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		public MainForm m_parent = null;

		public DockFormScriptList()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormScriptList2_Load(object sender, EventArgs e)
		{
			listView1.Items.Clear();
			string scrPath = m_parent.m_dataManager.m_gameDir + "\\scene";
			string[] scrList = System.IO.Directory.GetFiles( scrPath, "*.txt ");
			int row = 0;

			foreach( var tmp in scrList)
			{
				if( tmp.IndexOf("macro") != -1 ) continue;
				var item = listView1.Items.Add( System.IO.Path.GetFileNameWithoutExtension( tmp) );
				if( row%2==1) item.BackColor = Color.Azure;
				row++;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			string path = m_parent.m_dataManager.m_gameDir + "/scene/" + listView1.SelectedItems[0].Text +".txt";
			m_parent.m_scenarioManager.Load(path);//
			m_parent.SetBlockTxtToList();

			m_parent.formParent.m_blockList.SetActiveScript( listView1.SelectedItems[0].Text );
			m_parent.formParent.m_blockList.DataGrdiView(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override string GetPersistString()
		{
			return "DockFormScriptListStr";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView1_MouseDown(object sender, MouseEventArgs e)
		{
			if( listView1.SelectedItems.Count == 0 ) return;

			string path = m_parent.m_dataManager.m_gameDir + "/scene/";
			string filePath = m_parent.m_dataManager.m_gameDir + "/scene/" + listView1.SelectedItems[0].Text +".txt";
			filePath = filePath.Replace( "/", "\\" );
			string selectPath = "/select,\""+System.IO.Directory.GetCurrentDirectory() +"\\" +filePath+"\"";
	
			if( e.Button == MouseButtons.Right )		System.Diagnostics.Process.Start(filePath);
			if( e.Button == MouseButtons.Middle )		System.Diagnostics.Process.Start("EXPLORER.EXE",selectPath );
		}

		private void DockFormScriptList_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(this.IsHidden) { this.Show(); } else { this.Hide(); }
			e.Cancel = true;
		}
	}
}
