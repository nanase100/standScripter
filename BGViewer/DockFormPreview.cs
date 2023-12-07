using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace standScripter
{
	public partial class DockFormPreview : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		public MainForm m_parent = null;


		public DockFormPreview()
		{
			InitializeComponent();
		}

		private void DockFormPreview_Load(object sender, EventArgs e)
		{
			this.pictureBox1.MouseWheel		+= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
		}


		public void SetPreviewData( string bgName, string faceName, List<textStandData> standList, string message = "" )
		{
			//Graphics gs = pictureBox1.CreateGraphics();

			Bitmap bmp = new Bitmap(1280,720);
			Graphics gs = Graphics.FromImage(bmp);

			//gs.Clear(BackColor);

			//立ち絵座標
			int[] standPos = {640,370,900,240,1040 };

			//背景の描画
			if( bgName != "" )
			{
				bgName = bgName.Replace("\r\n","");

				var tmpBmp = m_parent.m_bmpManager.LoadPreviewBitmap(bgName);
				//if( tmpBmp != null )	gs.DrawImage( tmpBmp,0,0,tmpBmp.Width,tmpBmp.Height);
				if( tmpBmp != null )	gs.DrawImage( tmpBmp,0,0,1280,720);
			}

			

			//立ち絵の描画
			foreach( var standData in standList )
			{
				var tmpBmp = m_parent.m_bmpManager.LoadPreviewBitmap(standData.toolImgName);

				if( tmpBmp == null ) continue;

				int x = standPos[0];

				if( standData.standPosType != posType.EMPTY) x = standPos[(int)standData.standPosType];

				x -= tmpBmp.Width/2;
				gs.DrawImage( tmpBmp,x,0,tmpBmp.Width,tmpBmp.Height);
			}

			//顔描画
			if( faceName!="")
			{
				Bitmap tmpBmp = null;
				foreach( var tmpDic in m_parent.m_bmpManager.m_bitmapDictionary )
				{
					if( tmpDic.Key.IndexOf(faceName) != -1 ) tmpBmp = tmpDic.Value.mainImage;
				}
					
				if( tmpBmp != null )
				{
					gs.DrawImage( tmpBmp,20,500,(float)tmpBmp.Width,(float)tmpBmp.Height);
				}
			}

			//フォントオブジェクトの作成
			Font fnt = new Font("MS UI Gothic", 22);
			//文字列を表示
			gs.DrawString(message, fnt, System.Drawing.Brushes.Black, 200+2, 550+2);
			gs.DrawString(message, fnt, System.Drawing.Brushes.White, 200, 550);

			Graphics gsMain = pictureBox1.CreateGraphics();
			gsMain.DrawImage(bmp,0,0,pictureBox1.Width,pictureBox1.Height);

			gs.Dispose();
			gsMain.Dispose();

		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			
		}

		private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)  
		{
			if( e.Delta < 0 )
			{
				m_parent.formParent.m_blockList.NextMessage(true);
			}
			else
			{
				m_parent.formParent.m_blockList.NextMessage(false);
			}
		}

		private void DockFormPreview_KeyDown(object sender, KeyEventArgs e)
		{
			if( e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Enter|| e.KeyCode == Keys.Down ) m_parent.formParent.m_blockList.NextMessage(true);

			if( e.KeyCode == Keys.Up ) m_parent.formParent.m_blockList.NextMessage(false);

			e.Handled = true;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
		  switch (keyData)
		  {
			case Keys.Down:
			case Keys.Up:
//			case Keys.Left:
//			case Keys.Right:
				return false;
			default:
			  return base.ProcessDialogKey(keyData);
			  
		  }
		  return true;
		} 

		private void DockFormPreview_SizeChanged(object sender, EventArgs e)
		{
			int w = this.Width;
			int h = this.Height;

			if( w > h ) { h = h * 9  / 9 ; w = h  *16 /9;}
			if( w < h ) { w = w * 16 / 16;  h = w *9/16; }

			if (w>this.Width)
			{
				w = this.Width;
				h = w *9/16;
			}

			if (h>this.Height)
			{
				h = this.Height;
				w = h *16/9;
			}

			pictureBox1.Width  = w;
			pictureBox1.Height = h;

		}

		protected override string GetPersistString()
		{
			return "DockFormPreviewStr";
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			if( e.Button == MouseButtons.Middle ) m_parent.formParent.m_blockList.PlayVoice();
			if( e.Button == MouseButtons.Left ) m_parent.formParent.m_blockList.NextMessage(true);
			if( e.Button == MouseButtons.Right ) m_parent.formParent.m_blockList.NextMessage(false);
		}
	}
}
