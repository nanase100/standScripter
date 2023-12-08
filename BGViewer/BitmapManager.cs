using garu.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace standScripter
{
	public class BitmapSet
	{
		public Bitmap mainImage			{ get; set; }
		public Bitmap alphaImage		{ get; set; }

		public BitmapSet()
		{
			mainImage		= null;
			alphaImage		= null;
		}
	}
	
	
	public class BitmapManager
	{
		public Dictionary<string, BitmapSet>	m_bitmapDictionary{get;set;}			= new Dictionary<string, BitmapSet>();
		public Dictionary<string, Bitmap>		m_previewBitmapDictionary{get;set;}		= new Dictionary<string, Bitmap>();

		public Bitmap[] posImage		= new Bitmap[5];
		public Bitmap[] posImageMiss	= new Bitmap[5];		//立ち位置していが被っている場合
		public Bitmap[] posImageCont	= new Bitmap[5];		//立ち位置指定なしで、前のを引き継いだ上での位置の場合

		//-----------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------
		public BitmapManager()
		{
			CreatePosImage();
		}

		/// <summary>
		/// 立ち位置を表すブロックの並び画像を作成する。
		/// </summary>
		private void CreatePosImage()
		{
			for( int i = 0; i < 5; i++)
			{
				posImage[i]		= new Bitmap(100,10);
				posImageMiss[i]	= new Bitmap(100,10);
				posImageCont[i]	= new Bitmap(100,10);

				Graphics g1 = Graphics.FromImage(posImage[i]);
				Graphics g2 = Graphics.FromImage(posImageMiss[i]);
				Graphics g3 = Graphics.FromImage(posImageCont[i]);

				for( int l = 0; l < 5; l++)
				{
					g1.DrawRectangle(Pens.Black, l*20,1,18,8);
					g2.DrawRectangle(Pens.Black, l*20,1,18,8);
					g3.DrawRectangle(Pens.Black, l*20,1,18,8);
				}

				int xPos = 0;
				switch(i)
				{
					case 0: xPos = 2*20; break;
					case 1: xPos = 1*20; break;
					case 2: xPos = 3*20; break;
					case 3: xPos = 0*20; break;
					case 4: xPos = 4*20; break;
				}

				g1.FillRectangle(Brushes.Black, xPos,1,18,8);
				g2.FillRectangle(Brushes.Red, xPos,1,18,8);
				g3.FillRectangle(Brushes.MediumBlue, xPos,1,18,8);
				
				g1.Dispose();
				g2.Dispose();
				g3.Dispose();
			}
		}

		//-----------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------
		~BitmapManager()
		{
			foreach ( KeyValuePair<string,BitmapSet> imgSet in m_bitmapDictionary )
			{
				imgSet.Value.mainImage.Dispose();
				imgSet.Value.alphaImage.Dispose();
			}
		}


		//-----------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------
		public BitmapSet LoadBitmap( string fileName )
		{
			if( m_bitmapDictionary.ContainsKey( fileName )) return m_bitmapDictionary[fileName];

			if( fileName.IndexOf(".png") == -1 && fileName.IndexOf(".jpg") == -1  )
			{
				string filePath = fileName + ".jpg";
				if( System.IO.File.Exists(filePath) == true )
				{
					fileName = filePath;
				}
				else
				{
					filePath = fileName + ".png";
					if( System.IO.File.Exists(filePath) == true )
					{
						fileName = filePath;
					}
					else
					{
						return null;
					}
				}
			}


			//--------------------------
			m_bitmapDictionary[fileName] = new BitmapSet();
			m_bitmapDictionary[fileName].mainImage	= new Bitmap(fileName);
			m_bitmapDictionary[fileName].alphaImage	= new Bitmap(m_bitmapDictionary[fileName].mainImage.Width, m_bitmapDictionary[fileName].mainImage.Height);
			
			//ColorMatrixの行列の値を変更して、アルファ値が変更されるようにする
			System.Drawing.Imaging.ColorMatrix cm =	new System.Drawing.Imaging.ColorMatrix();
			cm.Matrix00 = 1;
			cm.Matrix11 = 1;
			cm.Matrix22 = 1;
			cm.Matrix33 = 0.5F;
			cm.Matrix44 = 1;
			System.Drawing.Imaging.ImageAttributes ia =	new System.Drawing.Imaging.ImageAttributes();
			ia.SetColorMatrix(cm);				//ColorMatrixを設定する

			Graphics g = Graphics.FromImage(m_bitmapDictionary[fileName].alphaImage);

			int width	= m_bitmapDictionary[fileName].mainImage.Width;
			int height	= m_bitmapDictionary[fileName].mainImage.Height;
			var rect	= new Rectangle( 0,0,width,height);
			g.DrawImage(m_bitmapDictionary[fileName].mainImage,rect, 0,0,width,height	, GraphicsUnit.Pixel,ia);
			g.Dispose();
			return m_bitmapDictionary[fileName];
		}


		//-----------------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------------
		public Bitmap LoadPreviewBitmap( string fileName )
		{
			fileName = fileName.Replace("\r\n","");
			string path = "プレビュー\\" + fileName+".png";

			if( !System.IO.File.Exists(path) )
			{
				 path = "プレビュー\\" + fileName+".jpg";
				if( !System.IO.File.Exists(path) )
				{
					return null;
				}
			}

			if( m_previewBitmapDictionary.ContainsKey( fileName )) return m_previewBitmapDictionary[fileName];

			m_previewBitmapDictionary[fileName] = new Bitmap(path);

			return m_previewBitmapDictionary[fileName];
		}
	}





}
