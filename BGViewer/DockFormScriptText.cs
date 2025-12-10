using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Forms;

namespace standScripter
{
	public partial class DockFormScriptText : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		[DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
		internal static extern int SendMessage(IntPtr hwnd, int msg, int wParam, IntPtr lParam);

		[DllImport("user32")]
		public static extern int GetScrollPos(IntPtr hWnd, int nBar);

		const int
			 SB_THUMBPOSITION = 4,
			 SB_VERT = 1,
			 WM_VSCROLL = 0x0115;

		public FormParent m_parent = null;

		public DockFormScriptText()
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

		}


		public void SearchScroll( string str, int startPos = 0 )
		{
			if( checkBox1.Checked == false ) return;

			
			str = str.Replace("\r\n", "\n");
			str = str.Replace("\t", "	");
			var rt = (tabControl1.SelectedIndex == 0) ? tbMainText : tbMainText2;

			if (startPos == -1) startPos = rt.SelectionStart+rt.SelectionLength;
			rt.Select(startPos, str.Length);

			if ( rt.Lines.Length == 0 ) return;

			int index =rt.Text.IndexOf(str, startPos);

			if ( index != -1 )
			{
				rt.Select( index, str.Length);
				
				rt.ScrollToCaret();
				rt.Select(index, str.Length);
			}
		}

		public void SetScriptText(string text)
		{
			this.tbMainText.Text = text;
			int lineCount = tbMainText.Lines.Length;
			SetLineCount(tbLineList, lineCount);
		}

		public void SetScriptText2(string text)
		{
			this.tbMainText2.Text = text;
			int lineCount = tbMainText2.Lines.Length;
			SetLineCount(tbLineList2, lineCount);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override string GetPersistString()
		{
			return "DockFormScriptTextStr";
		}


		private void DockFormScriptText_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(this.IsHidden) { this.Show(); } else { this.Hide(); }
			e.Cancel = true;
		}


		private void text_VScroll(object sender, EventArgs e)
        {
			int pos = GetScrollPos(((System.Windows.Forms.Control)sender).Handle, SB_VERT);
			LinkScroll(pos,0);
		}

        private void button1_Click(object sender, EventArgs e)
        {
			SearchScroll(textBox1.Text, -1);
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter)
			{
				SearchScroll(textBox1.Text,-1);
			}
		}


		private void LinkScroll( int pos, int target )
		{
			if( target == 0 )	SendMessage(tbLineList.Handle, WM_VSCROLL, (pos << 16) | SB_THUMBPOSITION, IntPtr.Zero);
			if( target == 1 )	SendMessage(tbLineList2.Handle, WM_VSCROLL, (pos << 16) | SB_THUMBPOSITION, IntPtr.Zero);

			tbLineList.Show();
		}

		private void SetLineCount( System.Windows.Forms.RichTextBox rTb, int lineCount )
		{
			string lineText = "";
			for (int i = 1; i <= lineCount; i++)
			{
				lineText += (i).ToString() + "\r\n";
			}
			rTb.Text = lineText;
		}

    }
}
