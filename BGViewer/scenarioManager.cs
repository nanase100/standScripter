using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;


namespace standScripter
{
	public enum posType
	{
		CENTER		= 0,		//#520	センター
		H2_LEFT		= 1,		//#521	２人立ち用・左
		H2_RIGHT	= 2,		//#522	２人立ち用・右
		H3_LEFT		= 3,		//#523	３人立ち用・左
		H3_RIGHT	= 4,		//#524	３人立ち用・右
		EMPTY		= -1,
	}

	public enum blockType
	{
		OTHER,
		MESSAGE,
		STAND,
		BG,
		CG,
		FACE,
		TRANSITION_BE,	//#transitionで暗転のみ行う場合の対応
		TRANSITION_AF,	//#transitionで背景切り替えも行う場合の背景変更
		RESET_DEF,		//#defマクロでcgも含めてすべて初期化されるため
		DEL_ALL_STAND,	
	}

	public class textStandData
	{
		public bool		isDelete		{ get; set; }			//cg クリア命令かどうか
		public string	toolImgName		{ get; set; }			//画像ファイル名。
		public string	macroName		{ get; set; }			//マクロファイル名。
		public int		bankID			{ get; set; }			//バンク。消すにも立てるにも。ただし、立ち絵クリア時はなし(-1)
		public	string	standSize		{ get; set; }			//立ち絵サイズ
		public	string	faceType		{ get; set; }			//表情差分指定

		//立ち絵データの追加情報
		public posType	standPosType	{ get; set; }
		
		//ツール上での必要経費
		public bool		isContinue		{ get; set; }
		public bool		isPosContinue	{ get; set; }

		public textStandData()
		{
			isDelete		= false;
			bankID			= -1;
			toolImgName		= "";
			faceType		= "";
			standSize		= "m";
			macroName		= string.Empty;
			isContinue		= false;
			standPosType	= posType.EMPTY;
			isPosContinue	= false;
		}

		public textStandData(textStandData rc)
		{
			isDelete		= rc.isDelete;
			bankID			= rc.bankID;
			standSize		= rc.standSize;
			toolImgName		= rc.toolImgName;
			faceType		= rc.faceType;
			macroName		= rc.macroName;
			isContinue		= rc.isContinue;
			standPosType	= rc.standPosType;
			isPosContinue	= rc.isPosContinue;	
		}
	}

	[DebuggerDisplay( "\\{text = {textBlock} type = {blockType}\\}" )]
	public class textBlockData
	{
		//共通情報
		public string					textBlock		{ get; set; }
		public blockType				blockType		{ get; set; }
		
		public string					bgFileName		{ get; set; }
		public bool						isStandClear	{  get; set; }
		public string					faceFileName	{ set; get; }

		//メッセージ表示前に書かれた、立ち絵・顔グラ・背景指定以外のすべての命令、コメント等のテキストをまとめたデータ。
		public string					preProc			{ set; get; }

		public List<textStandData>		standDatas		{ get; set; }
		
		//ツール上での必要経費
		public string					voiceFileName	{ get; set; }
		public bool						isBGContinue	{ get; set; }

		public textBlockData()
		{
			blockType		= blockType.OTHER;
			textBlock		= "";
			voiceFileName	= "";
			standDatas		= new List<textStandData>();	
			bgFileName		= "";
			faceFileName	= "";
			isBGContinue	= false;
		}

		public textBlockData(textBlockData rc)
		{
			blockType		= rc.blockType;
			textBlock		= rc.textBlock;
			voiceFileName	= rc.voiceFileName;
			isStandClear	= rc.isStandClear;
			preProc			= rc.preProc;
			faceFileName	= rc.faceFileName;
			bgFileName		= rc.bgFileName;

			standDatas		= new List<textStandData>();

			foreach( var tmp in rc.standDatas ) standDatas.Add( new textStandData(tmp));
		}
	}


	/// <summary>
	/// 
	/// </summary>
	public class scenarioManager
	{
		
		protected List<textBlockData>	m_rowTextBlockList	= new List<textBlockData>();		////スクリプトから読み込んだテキストデータそのままの形。のはず
		public List<textBlockData>		m_toolBlockList		= new List<textBlockData>();		////スクリプトから読み込んだデータをツール用に整形・加工した操作・セーブする実際のデータ。

		public string allScenario;

		public bool Load( string filePath )
		{
			m_rowTextBlockList.Clear();

			bool ret = false;

			using( var fp = new System.IO.StreamReader(filePath,System.Text.Encoding.GetEncoding("shift_jis")) )
			{
				allScenario = fp.ReadToEnd();

				Parse();

				ConvertRowToTool();

				ret = true;
			}
			return ret;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="data"></param>
		public void Save( string filePath, List<textBlockData> data)
		{
			string[] standPosStr = { "#520", "#521", "#522", "#523", "#524" };
			using( var fp = new System.IO.StreamWriter(filePath,false,System.Text.Encoding.GetEncoding("shift_jis")) )
			{
				foreach( var item in data )
				{
					//コメント・その他命令等
					if( item.preProc != "" ) fp.WriteLine( item.preProc + Environment.NewLine );

					//cg 全立ち絵削除命令の場合
					if( item.bgFileName == "" && item.isStandClear == true )fp.WriteLine( "	cg" + Environment.NewLine + "	rdraw 20" + Environment.NewLine );

					//背景トランジション切り替え
					if( item.bgFileName != "" )
					{
						fp.WriteLine("	%MsgBoxOff" + Environment.NewLine  );
						fp.WriteLine("	wait 15" + Environment.NewLine  );
						fp.WriteLine("	%Transition_BE bgblack 0" + Environment.NewLine  );
						fp.WriteLine("	wait 60" + Environment.NewLine  );
					}

					//立ち絵
					if( item.standDatas.Count > 0 )
					{
						string outName = "";
						foreach( var tmp in item.standDatas)
						{
							if( tmp.isDelete )
							{
								outName += "	cg " + tmp.bankID.ToString() + Environment.NewLine;
							}

							if( tmp.toolImgName != "" )
							{ 
								outName += "	%st"+tmp.toolImgName;
								outName = outName.Replace( "(2)", "_" + tmp.standSize );
								outName = outName.Replace( "(1)", tmp.bankID.ToString() );
								if( tmp.standPosType != posType.EMPTY ) outName += " " + standPosStr[(int)tmp.standPosType];
							}
							
							fp.WriteLine( outName );

							outName = "";
						}

						fp.WriteLine( "	rdraw 20" + Environment.NewLine );
					}

					//背景トランジション切り替え
					if( item.bgFileName != "" )
					{
						fp.WriteLine("	%Transition_AF bgwipe06 " + item.bgFileName + Environment.NewLine );
						fp.WriteLine("	wait 15"	+ Environment.NewLine );
						fp.WriteLine("	%MsgBoxOn"	+ Environment.NewLine  );
					}

					//顔グラがある場合
					if( item.faceFileName != "")
					{
						string outName = "	%fw" + item.faceFileName + Environment.NewLine;
						outName = outName.Replace("(2) ", " 1" );
						outName = outName.Replace("(1)", "" );
						fp.WriteLine( outName );
					}

					//ボイスがある場合
					if( item.voiceFileName !="") fp.WriteLine( "	pcm " + item.voiceFileName );

					//メッセージ
					fp.WriteLine( item.textBlock + Environment.NewLine  );

					//fp.Write( item.textBlock );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Parse()
		{
			int				nowLineNo		= 0;

			blockType		preType			= blockType.OTHER;
			blockType		nowType			= blockType.OTHER;

			Regex			standReg		= new Regex("^\t[^/]*(%st.*?) (.) (.) 0 0 0 0",	RegexOptions.IgnoreCase);
			Regex			rdrawReg		= new Regex("^\trdraw.*",						RegexOptions.IgnoreCase);
			Regex			deltandReg		= new Regex("^\tcg( )(\\d)?",					RegexOptions.IgnoreCase);
			Regex			delAllStandReg	= new Regex("^\tcg$",							RegexOptions.IgnoreCase);
			Regex			nullLineReg		= new Regex("^[ \t]*$",							RegexOptions.IgnoreCase);
			Regex			pcmReg			= new Regex("^\tpcm.*",							RegexOptions.IgnoreCase);
			Regex			zenkakuReg		= new Regex("^\t?[^\x01-\x7E/\t]+");
			Regex			bgReg			= new Regex(@"^\t[^/]*bg\d\d.*$");
			Regex			cgReg			= new Regex(@"^\t[^/]*%Ev_sb (.*)( .*)?$");
			Regex			transitionBEReg	= new Regex(@"^\t[^/]*%Transition_BE.*");
			Regex			transitionAFReg	= new Regex(@"^\t[^/]*%Transition_AF [^ ]+ ([^ ]+) *.*$.*");
			Regex			bgResetDefReg	= new Regex(@"^\t[^/]*%def.*");
			Regex			faceReg			= new Regex(@"^\t[^/]*%fw.*? ");

			string[]		scenarioByLine	= allScenario.Replace("\r","").Split('\n');

			string buff			= "";
			string nowStr		= "";
			
			while( nowLineNo < scenarioByLine.Length )
			{
				nowStr =scenarioByLine[nowLineNo];
				
				if( nullLineReg.IsMatch(nowStr) )														nowType = blockType.OTHER;
				if( pcmReg.IsMatch(nowStr)	 || zenkakuReg.IsMatch(nowStr) )							nowType = blockType.MESSAGE;
				if( deltandReg.IsMatch(nowStr)|| standReg.IsMatch(nowStr) || rdrawReg.IsMatch(nowStr) )	nowType = blockType.STAND;
				if( bgReg.IsMatch(nowStr) )																nowType = blockType.BG;
				if( cgReg.IsMatch(nowStr) )																nowType = blockType.CG;
				if( faceReg.IsMatch(nowStr) )															nowType = blockType.FACE;
				if( transitionBEReg.IsMatch(nowStr) )													nowType = blockType.TRANSITION_BE;
				if( transitionAFReg.IsMatch(nowStr) )													nowType = blockType.TRANSITION_AF;
				if( delAllStandReg.IsMatch(nowStr) )													nowType = blockType.DEL_ALL_STAND;

				if( nowType == preType && nowType != blockType.BG && nowType != blockType.TRANSITION_AF )
				{
					buff += nowStr + Environment.NewLine;
				}
				else
				{
					//ブロック種類の切り替え時に前のブロックをクローズしてリストに追加
					LoadAddBlock(buff,preType);
				
					buff				= nowStr + Environment.NewLine;
					preType				= nowType;
				}

				nowLineNo++;

				if( nowLineNo >= scenarioByLine.Length )
				{
					LoadAddBlock(buff,preType);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="type"></param>
		private void LoadAddBlock( string buff, blockType type )
		{
			textBlockData addObj = new textBlockData
			{
				textBlock = buff,
				blockType = type
			};

			//メッセージの場合・ボイス番号の抜き出し
			if ( type == blockType.MESSAGE )
			{
				Regex voiceReg = new Regex( ".*pcm (.*)\n.+");
				if( voiceReg.IsMatch(buff) )
				{
					addObj.voiceFileName = voiceReg.Match(buff).Groups[1].Value.Replace("\r","");;
				}
			}

			//立ち絵の場合・立ち絵の場合の情報、cgクリア命令の場合の情報、rdrawは一旦クリアしてセーブ時に戻す
			if( type == blockType.STAND )
			{
				string[] allText = buff.Replace("\r","").Replace("\t","").Split('\n');
				
				Regex standPosReg	= new Regex( @"^[^/]*%st(.*)_(.).(\d) (.) 0 0 0 0 (.+)$");
				Regex standReg		= new Regex( @"^[^/]*%st(.*)_(.).(\d) (.) 0 0 0 0$");
				Regex cgDelReg		= new Regex( @"^[^/]*cg +(\d)$");

				foreach( var tmp in allText )
				{
					if( tmp.IndexOf("rdraw") != -1 ) continue;
					if( tmp == "" ) continue;

					textStandData standData = new textStandData
					{
						macroName = tmp
					};
					//-----------------------
					//立ち絵全削除
					if ( tmp == "cg" )
					{
						standData.isDelete = true;
						standData.bankID = 999;
					}
					//-----------------------
					//立ち絵削除・バンク指定
					if( cgDelReg.IsMatch( tmp ) )
					{
						standData.isDelete = true;
						standData.bankID = int.Parse(cgDelReg.Match(tmp).Groups[1].Value);
					}
					//-----------------------
					//立ち絵指定・位置指定有り
					if( standPosReg.IsMatch( tmp ) )
					{
						var result = standPosReg.Match(tmp);
						standData.faceType		= result.Groups[4].Value;
						standData.bankID		= int.Parse( result.Groups[3].Value );
						standData.standSize		= result.Groups[2].Value;
						standData.toolImgName	= result.Groups[1].Value+"(2) (1) "+standData.faceType+" 0 0 0 0";
						string posStr			= result.Groups[5].Value;
						if( posStr.IndexOf("#") != -1 )
						{
							int outResult = 0;
							if( int.TryParse(posStr.Replace("#",""),out outResult))	standData.standPosType = posType.CENTER + (int.Parse(posStr.Replace("#",""))-520);
						}
						else
						{
							//standData.isPosContinue = true;
//							int outResult=0;
//							if( int.TryParse(posStr, out outResult) )standData.standPos = int.Parse( posStr );
						}
					}

					//-----------------------
					//立ち絵指定・位置指定なし
					if( standReg.IsMatch( tmp ) )
					{
						var result = standReg.Match(tmp);
						standData.faceType		= result.Groups[4].Value;
						standData.bankID		= int.Parse( result.Groups[3].Value );
						standData.standSize		= result.Groups[2].Value;
						standData.toolImgName	= result.Groups[1].Value+"(2) (1) "+standData.faceType+" 0 0 0 0";
					}

					addObj.standDatas.Add(standData);
				}
			}

			//背景・CGの場合、画像の名前を抜き出す
			if( type == blockType.BG )
			{
				Regex reg = new Regex(@".*(bg\d\d[^ ]*) ?.*$");

				addObj.bgFileName = reg.Match(buff).Groups[1].Value;
			}
			if( type == blockType.CG )
			{
				Regex reg = new Regex(@".*Ev_sb ([^ ]*)( .*)?\r",  RegexOptions.Multiline);

				addObj.bgFileName = reg.Match(buff).Groups[1].Value;
			}

			if( type == blockType.FACE )
			{
				Regex reg = new Regex( @"%fw(.*?) \d (\D).*");
				var result = reg.Match(buff);
				addObj.faceFileName = result.Groups[1].Value+"(2) (1) "+result.Groups[2].Value+" 0 0 0 0";
			}

			if( type == blockType.TRANSITION_AF )
			{
				Regex reg = new Regex(@"^\t[^/]*%Transition_AF [^ ]+ ([^ ]+) *.*$.*");

				addObj.bgFileName = reg.Match(buff).Groups[1].Value;
			}

			addObj.bgFileName = addObj.bgFileName.Replace("\r\n","");

			m_rowTextBlockList.Add(addObj);
		}

		/// <summary>
		/// 
		/// </summary>
		private void ConvertRowToTool()
		{
			string					bgName			= "";
			string					faceName		= "";
			string					stackBuff		= "";
			bool					isChangeClear	= false;
			textBlockData			addData			= null;
			List<textStandData>		tmpStandData	= null;
			Regex					pcmReg			= new Regex( "^\tpcm ([^\n]*)\r\n(.*)",RegexOptions.Singleline);

			m_toolBlockList.Clear();

			foreach( var tmp in m_rowTextBlockList )
			{
				if( tmp.blockType == blockType.OTHER )			stackBuff += tmp.textBlock;

				if( tmp.blockType == blockType.TRANSITION_BE )	{ stackBuff += tmp.textBlock;	isChangeClear				= true;	}
				if( tmp.blockType == blockType.RESET_DEF )		{ stackBuff += tmp.textBlock;	isChangeClear				= true;	}

				if( tmp.blockType == blockType.TRANSITION_AF )	{ bgName = tmp.bgFileName;		isChangeClear				= true;	}
				if( tmp.blockType == blockType.BG )				{ bgName = tmp.bgFileName;		isChangeClear				= true;	}
				if( tmp.blockType == blockType.CG )				{ bgName = tmp.bgFileName;		isChangeClear				= true;	}

				if( tmp.blockType == blockType.FACE )			faceName = tmp.faceFileName;

				if( tmp.blockType == blockType.DEL_ALL_STAND )	isChangeClear = true;

				if( tmp.blockType == blockType.STAND )
				{
					tmpStandData = new List<textStandData>();
					foreach( var tmpStand in tmp.standDatas) tmpStandData.Add(new textStandData(tmpStand));
				}

				if( tmp.blockType == blockType.MESSAGE )
				{
					addData = new textBlockData(tmp);
					addData.standDatas = tmpStandData;

					if( addData.standDatas == null ) addData.standDatas = new List<textStandData>();
					
					//末尾の空白行削除
					Regex regNullDel = new Regex(@"^(\r\n)*(.*?)(\r\n)+$",RegexOptions.Singleline);
					Match result;

					if( addData.textBlock != null )
					{
						result = regNullDel.Match(addData.textBlock);
						if( result.Success ) { addData.textBlock = result.Groups[2].Value; }
					}

					m_toolBlockList.Add(addData);

					var tmp2			= m_toolBlockList.Last();
					tmp2.isStandClear	= isChangeClear;
					tmp2.bgFileName		= bgName;
					tmp2.faceFileName	= faceName;
					tmp2.preProc		= stackBuff;

					if(tmp2.preProc != null)
					{
						result = regNullDel.Match(tmp2.preProc);
						if( result.Success ) { tmp2.preProc = result.Groups[2].Value; }
					}

					//-------------------------
					//最初以外の背景切り替えは「複数の命令行のフルセット+必要なら立ち絵追加」なので、preProcをコメント以外をクリアする
					//バンクスタートの「#」を見つけたらキャンセル
					if( stackBuff.IndexOf("#") == -1 && tmp2.bgFileName != "" )
					{
						var line = stackBuff.Replace("\r","").Split('\n');
						string correctStr = "";

						foreach( var tmpStr in line )
						{
							if( tmpStr.IndexOf("//") != -1) correctStr += (tmpStr +Environment.NewLine);
						}

						tmp2.preProc		= correctStr;
					}

					//-------------------------

					var matchR			= pcmReg.Match(tmp2.textBlock);

					if( matchR.Success )
					{
						tmp2.voiceFileName	= matchR.Groups[1].Value;
						tmp2.textBlock		= matchR.Groups[2].Value;
					}
					else
					{
						tmp2.textBlock = tmp2.textBlock;
					}

					addData				= new textBlockData();

					//新規データ用にクリア
					bgName				= "";
					stackBuff			= "";
					faceName			= "";
					tmpStandData		= null;
					isChangeClear		= false;
				}

				//末尾の終了処理はメッセージ無しでpreprocコメントを保持する
				if(tmp == m_rowTextBlockList.Last())
				{
					addData = new textBlockData(tmp);
					addData.standDatas = tmpStandData;

					if( addData.standDatas == null ) addData.standDatas = new List<textStandData>();
					
					//末尾の空白行削除
					Regex regNullDel = new Regex(@"^(\r\n)*(.*?)(\r\n)+$",RegexOptions.Singleline);
					Match result;

					m_toolBlockList.Add(addData);
					addData.textBlock = "";
					addData.preProc		= stackBuff;
				}
			}
		}
	

	}
}
