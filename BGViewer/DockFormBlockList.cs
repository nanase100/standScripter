using Hnx8.ReadJEnc;
using Microsoft.Win32;
using PresentationControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace standScripter
{

	public partial class DockFormBlockList : WeifenLuo.WinFormsUI.Docking.DockContent
	{


		public List<textBlockData>	m_messageBaseData		= new List<textBlockData>();		//内容はシナリオマネージャーが作った、ツール向け加工済データ。
		public List<textBlockData>	m_messageBlockGridList	= new List<textBlockData>();		//内容は同一。ただし、立ち絵や背景の継続表示用

		public MainForm				m_parent;
		private soundPlayer			m_soundPlayer		= null;
		private string				activeScriptName	= "";

		private StatusList			_StatusList;

		private ListSelectionWrapper<Status> StatusSelections;

		//コピペ用データ
		private string			m_copySrcBGname		= "";
		private textStandData	m_copySrcStand		= null;
		private string			m_copySrcFacename	= "";
		private string			m_copySrcComment	= "";

		private int				m_dragSrcRow		= -1;
		private int				m_dragSrcCol		= -1;

		private int				m_dragDestRow		= -1;
		private int				m_dragDestCol		= -1;


		public DockFormBlockList()
		{
			InitializeComponent();
			m_soundPlayer = new soundPlayer();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="scriptName"></param>
		public void SetActiveScript( string scriptName )
		{
			activeScriptName = scriptName;
			button4.Enabled = true;
		}

		/// <summary>
		/// Load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormBlockList_Load(object sender, EventArgs e)
		{
			dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;

			InitDataGridView();

			InitOptionChkCombo();

		}

		/// <summary>
		/// メインのデータグリッド準備
		/// </summary>
		private void InitDataGridView()
		{
			dataGridView1.Columns[0].DefaultCellStyle.BackColor = Color.LightBlue;
			//dataGridView1.Columns[0].DefaultCellStyle.WrapMode		= DataGridViewTriState.True;

			dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

			var tmp = dataGridView1.Rows.Add();

			dataGridView1.RowTemplate.Height = 96;

			//項目が空のときにバツマークを表示させないようにする
			((DataGridViewImageColumn)dataGridView1.Columns[1]).DefaultCellStyle.NullValue = null;
			((DataGridViewImageColumn)dataGridView1.Columns[2]).DefaultCellStyle.NullValue = null;
			((DataGridViewImageColumn)dataGridView1.Columns[3]).DefaultCellStyle.NullValue = null;
			((DataGridViewImageColumn)dataGridView1.Columns[4]).DefaultCellStyle.NullValue = null;
			((DataGridViewImageColumn)dataGridView1.Columns[5]).DefaultCellStyle.NullValue = null;
			((DataGridViewImageColumn)dataGridView1.Columns[6]).DefaultCellStyle.NullValue = null;
			((DataGridViewImageColumn)dataGridView1.Columns[7]).DefaultCellStyle.NullValue = null;
		}

		/// <summary>
		/// チェックボックス付きコンボボックス準備
		/// </summary>
		private void InitOptionChkCombo()
		{
			_StatusList = new StatusList();

			_StatusList.Add(new Status(1, "立絵配置時に立位置自動設定する"));
			_StatusList.Add(new Status(2, "立絵の位置の重複を防止する"));

			StatusSelections = new ListSelectionWrapper<Status>(_StatusList, "Name");

			StatusSelections[1].Selected = false;

			checkBoxComboBox2.DataSource = StatusSelections;
			checkBoxComboBox2.DisplayMemberSingleItem = "Name";
			checkBoxComboBox2.DisplayMember = "NameConcatenated";

			checkBoxComboBox2.ValueMember = "Selected";
			checkBoxComboBox2.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
		}

		/// <summary>
		/// スクリプトブロック情報の立ち絵連続性等を更新・設定する
		/// </summary>
		/// <param name="isInit"></param>
		public 	void UpdateBlockTxtToListS( bool isInit )
		{
			if( isInit )
			{
				dataGridView1.Rows.Clear();
			}

			for( int i = 0;i < dataGridView1.Rows.Count;i++ )
				for( int j = 1; j < dataGridView1.Columns.Count-1;j++)
					dataGridView1.Rows[i].Cells[j].Value = null;
			
			string	buff		= "";
			string	bgPath		= "";
			string	facePath	= "";
			Bitmap	refBitmap	= null;
			int		rowIndex	= 0;

			foreach( var tmp in m_messageBlockGridList )
			{
				if( isInit )
				{
					buff = tmp.textBlock;

					if( tmp.voiceFileName !="")	dataGridView1.Rows.Add("🔊 " +Environment.NewLine +Environment.NewLine+ buff);
					else
					{
						dataGridView1.Rows.Add(Environment.NewLine+ buff);
					}

					int nowRow = dataGridView1.Rows.Add();
					dataGridView1.Rows[nowRow].Height			= 10;
					dataGridView1.Rows[nowRow-1].Cells[8].Value	= tmp.preProc;
				}

				if( tmp.bgFileName != "")
				{
					foreach( var tmpThum in m_parent.m_dataManager.m_dataMaster )
					{
						if( tmpThum.m_fileName.IndexOf(tmp.bgFileName) != -1 )
						{
							tmpThum.m_fileName.Replace(@"/",@"\");

							bgPath = tmpThum.m_fileName.Replace(@"/",@"\");

							if( System.IO.File.Exists(bgPath))
							{
								if( tmp.isBGContinue == false)	refBitmap = m_parent.m_bmpManager.LoadBitmap(bgPath).mainImage;
								else							refBitmap = m_parent.m_bmpManager.LoadBitmap(bgPath).alphaImage;
								dataGridView1.Rows[rowIndex*2].Cells[1].Value = refBitmap;
								dataGridView1.Rows[rowIndex*2].Cells[1].Style.BackColor = (m_messageBaseData[rowIndex].isStandClear?Color.Pink:Color.White);
								break;
							}
						}
					}
				}

				int loopCount = tmp.standDatas.Count;
				string path = "";

				//顔グラ
				if( tmp.faceFileName != "")
				{
					foreach( var tmpThum in m_parent.m_dataManager.m_dataMaster )
					{
						if( tmpThum.m_fileName.IndexOf(tmp.faceFileName) != -1 )
						{
							facePath = tmpThum.m_fileName.Replace(@"/",@"\");

							if(  System.IO.File.Exists(facePath) )
							{
								refBitmap = m_parent.m_bmpManager.LoadBitmap(facePath).mainImage;

								dataGridView1.Rows[rowIndex*2].Cells[7].Value = refBitmap;
							}
							break;
						}
					}
				}

				//データがないセルの色を白に初期化しておく。立ち絵消し命令のピンクが残り、立ち絵情報がなくなって白に更新されない可能性がある

				for( int i = 0; i < 5; i++ )dataGridView1.Rows[rowIndex*2].Cells[i+2].Style.BackColor = Color.White;

				foreach( var tmpStand in tmp.standDatas )
				{
					int bankIndex = tmpStand.bankID+1;

					if( tmpStand.isDelete ) dataGridView1.Rows[rowIndex*2].Cells[bankIndex].Style.BackColor = Color.Pink;

					if( tmpStand.bankID == -1 || tmpStand.bankID == 999 ) continue;

					if( tmpStand.toolImgName == "" ) continue;

					//立ちグラ表示が確定したので画像を探す
					foreach( var tmpThum in m_parent.m_dataManager.m_dataMaster )
					{
						if( tmpThum.m_fileName.IndexOf(tmpStand.toolImgName) != -1 )
						{
							path = tmpThum.m_fileName.Replace(@"/",@"\");
							
							if(  System.IO.File.Exists(path) )
							{
								refBitmap = (tmpStand.isContinue == false?m_parent.m_bmpManager.LoadBitmap(path).mainImage:m_parent.m_bmpManager.LoadBitmap(path).alphaImage);
							}
							if( bankIndex != -1)
							{
								int nowPos = (int)tmpStand.standPosType;
								dataGridView1.Rows[rowIndex*2].Cells[bankIndex].Value = refBitmap;

								if( nowPos == -1 )nowPos = 0;
								
								bool isSame			= false;
								bool isPosContinue	= false;
								foreach( var checkTmp in tmp.standDatas )
								{
									if( checkTmp.toolImgName != "" && checkTmp != tmpStand && tmpStand.standPosType == checkTmp.standPosType)
									{
										isSame = true;
										isPosContinue = false;
										break;
									}
									
									if( bankIndex == checkTmp.bankID+1  && checkTmp.isPosContinue )
									//	if( bankIndex == checkTmp.bankID+1 && checkTmp != tmpStand && checkTmp.isPosContinue )
									{
										isPosContinue = true;
										//break;
									}
								}

								if( isSame )
								{
									dataGridView1.Rows[rowIndex*2+1].Cells[bankIndex].Value = m_parent.m_bmpManager.posImageMiss[nowPos];
								}
								else
								{
									if( isPosContinue )	dataGridView1.Rows[rowIndex*2+1].Cells[bankIndex].Value = m_parent.m_bmpManager.posImageCont[nowPos];
									else				dataGridView1.Rows[rowIndex*2+1].Cells[bankIndex].Value = m_parent.m_bmpManager.posImage[nowPos];
								}
							}
						}
					}
				}

				dataGridView1.Rows[rowIndex*2].Cells[8].Value = tmp.preProc;

				rowIndex++;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="textBlockList"></param>
		/// <param name="isInit"></param>
		public 	void UpdateBlockTxtToList( bool isInit )
		{
			CreateGridList();
			UpdateBlockTxtToListS(isInit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textBlockList"></param>
		public void CopyBlockData(List<textBlockData> textBlockList)
		{
			m_messageBaseData.Clear();
			foreach( var tmp in textBlockList ) m_messageBaseData.Add( new textBlockData(tmp));
		}

		/// <summary>
		/// 
		/// </summary>
		public void CreateGridList()
		{
			m_messageBlockGridList.Clear();

			int				rowIndex		= 0;
			string			keepBGname		= "";

			foreach( var tmp in m_messageBaseData )
			{
				//-------------------------
				//バンク先頭のチェック時にメッセージも追加
				var nowBlock = new textBlockData(tmp);
				m_messageBlockGridList.Add(nowBlock);
				if( nowBlock.bgFileName != "" ) keepBGname = nowBlock.bgFileName;
				if( rowIndex == 0 ){ rowIndex++; continue; }
				var preBlock = m_messageBlockGridList[rowIndex-1];
				//-------------------------
				//背景の継続
				if( keepBGname != "" && nowBlock.bgFileName == "" )
				{
					nowBlock.bgFileName = keepBGname;
					nowBlock.isBGContinue=true;
				}
				//-------------------------
				//立ち絵の継続
				if( nowBlock.isStandClear ) { rowIndex++; continue; }

				foreach( var tmpPreStand in preBlock.standDatas )
				{
					bool isFind = false ;	//前の立ち絵指定があって、今の立ち絵指定がないかフラグ。
					bool isDel	= false;	//立ち絵消去命令があるかフラグ
					textStandData nowStand = null;
					for( int j = 0; j < nowBlock.standDatas.Count; j++ )
					{
						if( tmpPreStand.bankID == nowBlock.standDatas[j].bankID && nowBlock.standDatas[j].isDelete == true) {  isDel = true; break;}
						if( tmpPreStand.bankID == nowBlock.standDatas[j].bankID ) { isFind = true; nowStand = nowBlock.standDatas[j];	break; }
						
					}
					if( isDel == true) continue;
					
					if( isFind == false )
					{
						if( tmpPreStand.toolImgName != "" )
						{
							var newStand = new textStandData(tmpPreStand);

							newStand.isContinue			= true;
							newStand.isPosContinue		= true;
							//newStand.standPosType = posType.EMPTY;
							nowBlock.standDatas.Add( newStand );
						}
					}
					else
					{
						if( nowStand.standPosType == posType.EMPTY ){ nowStand.standPosType = tmpPreStand.standPosType; nowStand.isPosContinue = true; }
					}
				}
	
				//-------------------------
				rowIndex++;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="isSizeUpOrDown"></param>
		private void CellSizeChange( bool isSizeUpOrDown )
		{
			int changeSize = (isSizeUpOrDown?10:-10);

			for( int i = 0; i < dataGridView1.Rows.Count; i+=2 )
			{
				dataGridView1.Rows[i].Height += changeSize;
			}
			for( int i = 0; i < dataGridView1.Columns.Count; i++  )
			{
				dataGridView1.Columns[i].Width += changeSize;
			}
		}



		private void button2_Click(object sender, EventArgs e)
		{
			CellSizeChange(true);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			CellSizeChange(false);
		}

		private void dataGridView1_SelectionChanged(object sender, EventArgs e)
		{
			
		}

		/// <summary>
		/// スクリプト出力
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		
		private void button4_Click(object sender, EventArgs e)
		{
			if( System.Windows.Forms.MessageBox.Show("編集中のスクリプトファイルを上書き保存してもよろしいですか？","確認",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

			//string path = activeScriptName +".txt";
			string path = m_parent.m_dataManager.m_gameDir + "\\scene\\"+activeScriptName +".txt";
			m_parent.m_scenarioManager.Save(path,m_messageBaseData);
		}


		private void button6_Click(object sender, EventArgs e)
		{
			fontSizeChange(true);
		}

		private void button5_Click(object sender, EventArgs e)
		{
			fontSizeChange(false);
		}
		

		/// <summary>
		/// フォントサイズの上下
		/// </summary>
		/// <param name="isSizeUpOrDown"></param>
		private void fontSizeChange( bool isSizeUpOrDown)
		{
			float size = dataGridView1.Font.Size;
			size += (isSizeUpOrDown ?1:-1);

			Font newFont = new Font( dataGridView1.Font.FontFamily, size );
			dataGridView1.Font = newFont;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			StartCellEdit();
		}

		private void StartCellEdit()
		{
			int row = dataGridView1.CurrentCell.RowIndex;
			int col = dataGridView1.CurrentCell.ColumnIndex;

			if( col == 0 ) { PlayVoice(row/2); }

			if( col >= 1 && col != 8 && row%2==0) { m_parent.m_isCallFromBlocklist = true;  m_parent.hotKey_HotKeyPush( false );	}

			if( col == 8) dataGridView1.BeginEdit(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="textID"></param>
		public void PlayVoice(int textID = -1)
		{

			if( textID == -1 ) textID = dataGridView1.CurrentCell.RowIndex/2;

			string voiceName = m_messageBlockGridList[textID].voiceFileName;
			
			if( voiceName != "" )
			{
				string folderPrefix = voiceName.Substring(0,2);
				string path = m_parent.m_dataManager.m_gameDir + "/sound/" + folderPrefix + "/" + voiceName +".ogg";

				if( m_soundPlayer.isPlaying ) m_soundPlayer.StopSound();
				else 		m_soundPlayer.PlaySound(path);
			}
			else
			{
				m_soundPlayer.StopSound();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormBlockList_FormClosing(object sender, FormClosingEventArgs e)
		{
			//m_parent.Close();
			e.Cancel = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
		{

			//bool isGuardPosDup = (bool)checkBoxComboBox2.Items[2].
			bool isGuardPosDup = StatusSelections[1].Selected;
			switch( e.KeyCode)
			{
				//二人立ち絵の左右
				case Keys.Q:		ChangeStandPos(posType.H2_LEFT,isGuardPosDup);			break;
				case Keys.W:		ChangeStandPos(posType.EMPTY,isGuardPosDup);			break;
				case Keys.E:		ChangeStandPos(posType.H2_RIGHT,isGuardPosDup);			break;
				
				//三人立ち絵の左真ん中右
				case Keys.A:		ChangeStandPos(posType.H3_LEFT,isGuardPosDup);			break;
				case Keys.S:		ChangeStandPos(posType.CENTER,isGuardPosDup);			break;
				case Keys.D:		ChangeStandPos(posType.H3_RIGHT,isGuardPosDup);			break;

				//ダブルクリック以外での呼び出し
				case Keys.R:		StartCellEdit();							break;

				case Keys.F:	if(e.Control == true) textBox1.Focus();			break;

				//セルコピペ
				case Keys.X:	if(e.Control == true) CutCell();				break;
				case Keys.C:	if(e.Control == true) CopyCell();				break;
				case Keys.V:	if(e.Control == true) PasteCell();				break;

				case Keys.T:	SelectRangeStandMass(dataGridView1.CurrentCell.RowIndex,dataGridView1.CurrentCell.ColumnIndex);		break;

				//コンパイル・実行
				case Keys.F5:	RunGame();			break;
				case Keys.F7:	CompileNow();		break;

				//立ち絵の削除
				case Keys.Delete:
					if(e.Control == true)
					{
						SwitchStandDel();
					}
					else
					{
						if(e.Alt == true)	SwitchStandClear();
						else				DeleteItem();
					}
					break;

				//バンクチェンジ
				case Keys.D1:		ChangeStandBank(1);			break;
				case Keys.D2:		ChangeStandBank(2);			break;
				case Keys.D3:		ChangeStandBank(3);			break;
				case Keys.D4:		ChangeStandBank(4);			break;
				case Keys.D5:		ChangeStandBank(5);			break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void RunGame()
		{

			string preCurrent = System.IO.Directory.GetCurrentDirectory();

			System.IO.Directory.SetCurrentDirectory(m_parent.m_dataManager.m_gameDir);

			string path = System.IO.Path.Combine( m_parent.m_dataManager.m_gameDir, "cs2.exe" );
			System.Diagnostics.Process.Start("cs2.exe");

			System.IO.Directory.SetCurrentDirectory(preCurrent);
		}

		/// <summary>
		/// 開いているc2ファイルコンパイルする。yu-risはコンパイル=再起動なのでスルー
		/// </summary>
		public void CompileNow()
		{
			
			string filePath = m_parent.m_dataManager.m_gameDir + "\\scene\\"+activeScriptName +".txt";
			string mcPath = m_parent.m_dataManager.m_gameDir + "\\scene\\mc.exe ";

			string preCurrent = System.IO.Directory.GetCurrentDirectory();

			System.IO.Directory.SetCurrentDirectory(m_parent.m_dataManager.m_gameDir+ "\\scene");

			//string CommandLine = "cmd.exe /c " + "mc.exe " + activeScriptName +".txt";
			string CommandLine = "/c " + "mc.exe " + activeScriptName +".txt";

			System.Diagnostics.Process.Start("cmd.exe",CommandLine);
			//System.Diagnostics.Process.Start("cmd.exe");

			System.IO.Directory.SetCurrentDirectory( preCurrent );
		}

		/// <summary>
		/// /
		/// </summary>
		public void SwitchStandDel()
		{
			int loopCount = dataGridView1.SelectedCells.Count;
			int row = 0;
			int col = 0;

			for( int i = 0; i < loopCount; i++ )
			{
				bool isAdd = true;

				row = dataGridView1.SelectedCells[i].RowIndex/2;
				col = dataGridView1.SelectedCells[i].ColumnIndex;
				if( col == 0 || col == 1 ) return;
				col -= 1;

				for( int j = 0; j < m_messageBaseData[row].standDatas.Count; j++)
				{
					if(m_messageBaseData[row].standDatas[j].bankID == col)
					{
						m_messageBaseData[row].standDatas[j].isDelete = !(m_messageBaseData[row].standDatas[j].isDelete);
						isAdd = false;
						break;
					}
				}

				if( isAdd )
				{ 
					textStandData addTmpe	= new textStandData();
					addTmpe.isDelete		= true;
					addTmpe.bankID			= col;
					m_messageBaseData[row].standDatas.Add( addTmpe );
				}
			}

			UpdateBlockTxtToList(false);
		}


		/// <summary>
		/// 
		/// </summary>
		public void SwitchStandClear()
		{
			int row = dataGridView1.CurrentCell.RowIndex/2;

			m_messageBaseData[row].isStandClear = !(m_messageBaseData[row].isStandClear);

			UpdateBlockTxtToList(false);
		}


		/// <summary>
		/// 立ち絵の立ち位置変更命令
		/// </summary>
		/// <param name="newPos"></param>
		public void ChangeStandPos( posType newPos, bool isGuardPosDup = false  )
		{
			int loopCount = dataGridView1.SelectedCells.Count;
			int row = 0;
			int col = 0;
			bool isUpdate = false;
			for( int i = 0; i < loopCount; i++ )
			{
				row = dataGridView1.SelectedCells[i].RowIndex/2;
				col = dataGridView1.SelectedCells[i].ColumnIndex;
				if( col == 0 || col == 1 ) return;
				col -= 1;

				//移動先に別の立ち絵がある場合はスワップするための準備
				int swapBankID = -1;
				int loopCount2 = m_messageBaseData[row].standDatas.Count;

				if( isGuardPosDup )
				{
					for( int j = 0; j < loopCount2; j++ )
					{
						if (m_messageBaseData[row].standDatas[j].bankID != col )
						{
							if( m_messageBaseData[row].standDatas[j].standPosType == newPos)
							{ 
								swapBankID = j;
								break;
							}
						}
					}
				}

				for( int j = 0; j < loopCount2; j++ )
				{
					if (m_messageBaseData[row].standDatas[j].bankID == col )
					{
						if( swapBankID != -1 ) m_messageBaseData[row].standDatas[swapBankID].standPosType = m_messageBaseData[row].standDatas[j].standPosType;
						m_messageBaseData[row].standDatas[j].standPosType = newPos;
						isUpdate = true;
						break;
					}
				}
			}			
			if( isUpdate ) UpdateBlockTxtToList(false);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="newBankID"></param>
		public void ChangeStandBank( int newBankID )
		{
			int loopCount = dataGridView1.SelectedCells.Count;
			int row = 0;
			int col = 0;
			bool isUpdate = false;
			for( int i = 0; i < loopCount; i++ )
			{
				row = dataGridView1.SelectedCells[i].RowIndex/2;
				col = dataGridView1.SelectedCells[i].ColumnIndex;
				if( col == 0 || col == 1 ) return;
				col -= 1;

				//移動先に別の立ち絵がある場合はスワップするための準備

				int swapBankID = -1;
				int loopCount2 = m_messageBaseData[row].standDatas.Count;


				for( int j = 0; j < loopCount2; j++ )
				{
					if (m_messageBaseData[row].standDatas[j].bankID != col )
					{
						if( m_messageBaseData[row].standDatas[j].bankID == newBankID)
						{ 
							swapBankID = j;
							break;
						}
					}
				}
				

				for( int j = 0; j < loopCount2; j++ )
				{
					if (m_messageBaseData[row].standDatas[j].bankID == col )
					{
						if( swapBankID != -1 ) m_messageBaseData[row].standDatas[swapBankID].bankID = m_messageBaseData[row].standDatas[j].bankID;
						m_messageBaseData[row].standDatas[j].bankID = newBankID;
						isUpdate = true;
						break;
					}
				}
			}
			if( isUpdate ) UpdateBlockTxtToList(false);
		}


		/// <summary>
		/// 
		/// </summary>
		public void DeleteItem()
		{
			int		loopCount = dataGridView1.SelectedCells.Count;
			int		row = 0;
			int		col = 0;
			bool	isUpdate = false;

			for( int i = 0; i < loopCount; i++ )
			{
				row = dataGridView1.SelectedCells[i].RowIndex/2;
				col = dataGridView1.SelectedCells[i].ColumnIndex;
				if( col == 0 || col == 1 ) return;
				col -= 1;

				//立ち絵の削除確認
				for( int j = 0; j < m_messageBaseData[row].standDatas.Count; j++ )
				{
					if (m_messageBaseData[row].standDatas[j].bankID == col )
					{
						m_messageBaseData[row].standDatas.Remove(m_messageBaseData[row].standDatas[j]);
						isUpdate = true;
					}
				}

				//顔グラの削除確認
				if( m_messageBaseData[row].faceFileName != "" && col == 6 )
				{
					m_messageBaseData[row].faceFileName = "";
					isUpdate = true;
				}

			}
			if( isUpdate ) UpdateBlockTxtToList(false);
		}




		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			if( e.ColumnIndex != 8) e.Cancel = true;
		}

		public void NextMessage( bool isAdd )
		{
			if( dataGridView1.CurrentCell == null ) return;

			int nowRow = dataGridView1.CurrentCell.RowIndex;
			int nowCol = dataGridView1.CurrentCell.ColumnIndex;

			nowRow += (isAdd?2:-2);

			if( nowRow >= dataGridView1.RowCount ) return;
			if( nowRow < 0 ) return;

			dataGridView1.CurrentCell=dataGridView1.Rows[nowRow].Cells[nowCol];
		}

		private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
		{

			if( dataGridView1.CurrentCell == null)return;

			int nowRow = dataGridView1.CurrentCell.RowIndex;

			bool isChangeRow = (m_parent.m_nowSelectBlockNo == nowRow ?false:true);

			m_parent.m_nowSelectBlockNo = nowRow;
			m_parent.m_nowSelectBankNo =  dataGridView1.CurrentCell.ColumnIndex-1;

			int rowNo = m_parent.m_nowSelectBlockNo/2;

			if( m_parent.formParent.m_blockList.m_messageBlockGridList.Count > rowNo )
			{
				var tmp = m_parent.formParent.m_blockList.m_messageBlockGridList[rowNo];
				if(isChangeRow) m_parent.formParent.m_preview.SetPreviewData( tmp.bgFileName, tmp.faceFileName, tmp.standDatas, tmp.textBlock );
			}
		}

		public void DataGrdiView( bool isEnable = true )
		{
			dataGridView1.Enabled = isEnable;
		}

		public void SearchBlock( string messageText )
		{
			int startRow = dataGridView1.CurrentCell.RowIndex+1;
			if( dataGridView1.Rows[startRow].Cells[0].Value == null )startRow++;

			for( int i = startRow; i < dataGridView1.Rows.Count; i++ )
			{
				if( dataGridView1.Rows[i].Cells[0].Value == null )continue;

				if (dataGridView1.Rows[i].Cells[0].Value.ToString().IndexOf(messageText) != -1)
				{
					dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
					return;
				}

				if (dataGridView1.Rows[i].Cells[8].Value.ToString().IndexOf(messageText) != -1)
				{
					dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[8];
					return;
				}
			}

			//カーソル以降で見つからなかったときのループ検索
			for( int i = 0; i < startRow+1; i++ )
			{
				if( dataGridView1.Rows[i].Cells[0].Value == null )continue;

				if (dataGridView1.Rows[i].Cells[0].Value.ToString().IndexOf(messageText) != -1)
				{
					dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
					return;
				}
				if (dataGridView1.Rows[i].Cells[8].Value.ToString().IndexOf(messageText) != -1)
				{
					dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[8];
					return;
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			SearchBlock( textBox1.Text );
		}

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if( e.KeyChar == (char)Keys.Enter )
			{
				e.Handled = true;
				SearchBlock( textBox1.Text );
			}
		}

		private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if( e.ColumnIndex == 8)
			{
				m_messageBaseData[e.RowIndex/2].preProc = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
			}

		}


		/// <summary>
		/// 選んだ立ち絵情報の前後に続く、一塊の立ち絵グループを選択した状態にする
		/// </summary>
		/// <param name="row"></param>
		/// <param name=""></param>
		public void SelectRangeStandMass( int row, int col)
		{
			if( row%2!=0)row--;
			row /= 2;

			col--;		//列番号は0オリジンながら、バンク番号は1オリジンなので。


			bool isExistStand = false;

			foreach( var tmp in m_messageBlockGridList[row].standDatas ) if( tmp.bankID == col ) { isExistStand = true; break; }

			if( col == 0 || col == 7 || col == 8 || isExistStand == false )
			{
				dataGridView1.ClearSelection();
				return;
			}
			//--------------
			int startRow = -1;
			int endRow = -1;

			//スタート探し
			for( int i = row; i >= 0; i-- )
			{
				
				int toBankID = -1;
				
				for( int j = 0; j < m_messageBlockGridList[i].standDatas.Count; j++ ) if(m_messageBlockGridList[i].standDatas[j].bankID == col ) { toBankID = j; break; }

				if( toBankID == -1 ) break;

				if(m_messageBlockGridList[i].standDatas[toBankID].isDelete == true  )  {  startRow = i;break;}

				if( m_messageBlockGridList[i].isStandClear == true) {  startRow = i;break;}

				startRow = i;

			}

			//エンド探し
			for( int i = row; i < m_messageBlockGridList.Count; i++ )
			{
				int toBankID = -1;

				for( int j = 0; j < m_messageBlockGridList[i].standDatas.Count; j++ ) if(m_messageBlockGridList[i].standDatas[j].bankID == col ) { toBankID = j; break; }

				if( toBankID == -1 ) break;

				if( m_messageBlockGridList[i].standDatas[toBankID].isDelete == true  ){  endRow = i; break; }

				if( m_messageBlockGridList[i].isStandClear == true && i != row) {  /*endRow = i;*/break;}

				endRow = i;
			}

			if( startRow == -1 || endRow == - 1 )return;

			startRow*=2;
			endRow*=2;

			for( int i = startRow; i <= endRow; i++ )
			{
				dataGridView1.Rows[i].Cells[col+1].Selected = true;
			}
		}





		public void SetStand( string thumbName, int bankNo, string sizeType )
		{
			if( bankNo == 0 || dataGridView1.SelectedCells.Count == 0 ) return;

			bool isExist = false;
			textStandData addTmp = null;

			int blockNo = dataGridView1.SelectedCells[0].RowIndex / 2;

			if( blockNo >= m_messageBaseData.Count ) return;
			int loopCount = m_messageBaseData[blockNo].standDatas.Count;
			
			for( int i = 0; i < loopCount; i++ )
			{
				if( m_messageBaseData[blockNo].standDatas[i].bankID == bankNo )
				{
					addTmp = m_messageBaseData[blockNo].standDatas[i];
					isExist = true;
					break;
				}
			}

			if( addTmp == null ) addTmp = new textStandData();
				
			addTmp.bankID		= bankNo;
			addTmp.standSize	= sizeType.Replace("_","");
			addTmp.toolImgName	= thumbName;

			bool isCheck = StatusSelections[0].Selected;

			if( isCheck == true )
			{
				posType newPos = posType.CENTER;
				switch(bankNo)
				{
					case 1: newPos = posType.H3_LEFT; break;
					case 2: newPos = posType.H2_LEFT; break;
					case 4: newPos = posType.H2_RIGHT; break;
					case 5: newPos = posType.H3_RIGHT; break;
				}
				addTmp.standPosType	= newPos;
			}
			

			if( isExist == false ) m_messageBaseData[blockNo].standDatas.Add( addTmp);
		}


		public void SetBG( string bgName )
		{
			if( dataGridView1.SelectedCells.Count == 0) return;
			int blockNo = dataGridView1.SelectedCells[0].RowIndex / 2;
			m_messageBaseData[blockNo].bgFileName = bgName;
		}

		public void SetFace( string bgName )
		{
			if( dataGridView1.SelectedCells.Count == 0) return;
			int blockNo = dataGridView1.SelectedCells[0].RowIndex / 2;
			m_messageBaseData[blockNo].faceFileName = bgName;
		}
		



		protected override string GetPersistString()
		{
			return "DockFormBlockListStr";
		}

		public void CutCell()
		{
			CopyCell( true );
		}

		public void CopyCell( bool isClear = false  )
		{
			if( dataGridView1.CurrentCell == null ) return;
			
			int rowIndex = dataGridView1.CurrentCell.RowIndex/2;
			int colIndex = dataGridView1.CurrentCell.ColumnIndex;

			if( colIndex == 0 ) return;

			int copySrcMode = 0;

			if( colIndex == 1 )			copySrcMode = 1;
			else if( colIndex == 7 )	copySrcMode = 3;
			else if( colIndex == 8 )	copySrcMode = 4;
			else						copySrcMode = 2;

			switch( copySrcMode )
			{
				case 1:
					m_copySrcBGname = m_messageBlockGridList[rowIndex].bgFileName;
					if( isClear ) m_messageBaseData[rowIndex].bgFileName = "";
					break;
				case 2:
					int nowBankID = colIndex-1;
					for( int i = 0; i < m_messageBlockGridList[rowIndex].standDatas.Count; i++ )
					{
						if (m_messageBlockGridList[rowIndex].standDatas[i].bankID == nowBankID ) 
						{
							m_copySrcStand = new textStandData(m_messageBlockGridList[rowIndex].standDatas[i]);
							m_copySrcStand.isContinue = false;
							if( isClear ) m_messageBaseData[rowIndex].standDatas.Remove(m_messageBaseData[rowIndex].standDatas[i]);
							break;
						}
					}
					break;
				case 3:
					m_copySrcFacename = m_messageBlockGridList[rowIndex].faceFileName;
					if( isClear ) m_messageBaseData[rowIndex].faceFileName = "";
					break;
				case 4:
					m_copySrcComment = m_messageBlockGridList[rowIndex].preProc;
					break;
			}

			if( isClear ) UpdateBlockTxtToList(false);
		}


		/// <summary>
		/// 
		/// </summary>
		public void PasteCell()
		{
			if( dataGridView1.CurrentCell == null ) return;

			int rowIndex = dataGridView1.CurrentCell.RowIndex/2;
			int colIndex = dataGridView1.CurrentCell.ColumnIndex;

			if( colIndex == 0 ) return;

			if( colIndex == 1 && m_copySrcBGname == ""  ) return;
			if( colIndex == 7 && m_copySrcFacename == "" ) return;
			if((colIndex >= 2 && colIndex <= 6) && m_copySrcStand == null ) return;

			int copySrcMode = 0;

			if( colIndex == 1 )			copySrcMode = 1;
			else if( colIndex == 7 )	copySrcMode = 3;
			else if( colIndex == 8 )	copySrcMode = 4;
			else						copySrcMode = 2;

			switch( copySrcMode )
			{
				case 1:
					m_messageBaseData[rowIndex].bgFileName = m_copySrcBGname;
					break;
				case 2:
					int nowBankID = colIndex-1;
					
					for ( int i = 0;i < m_messageBaseData[rowIndex].standDatas.Count;i++ )
					{
						if(m_messageBaseData[rowIndex].standDatas[i].bankID == nowBankID )
						{
							m_messageBaseData[rowIndex].standDatas.Remove(m_messageBaseData[rowIndex].standDatas[i]);
						}
					}
					var newItem = new textStandData(m_copySrcStand);
					newItem.bankID = nowBankID;
					m_messageBaseData[rowIndex].standDatas.Add( newItem );

					break;
				case 3:
					m_messageBaseData[rowIndex].faceFileName = m_copySrcFacename;
					break;
				case 4:
					m_messageBaseData[rowIndex].preProc = m_copySrcComment;
					break;
			}

			UpdateBlockTxtToList(false);


		}

		private void SwapStand( int srcRow, int srcCol, int destRow, int destCol )
		{
			
			
				int destID = 0;
				textStandData destBuf = null;
				foreach( var tmp in m_messageBaseData[destRow].standDatas )
				{
					if( tmp.bankID+1 == destCol )
					{
						destBuf = tmp;
						break;
					}
					destID++;
				}
			
				int srcID = 0;
				textStandData srcBuf = null;
				foreach( var tmp in m_messageBaseData[srcRow].standDatas )
				{
					if( tmp.bankID+1 == srcCol )
					{
						srcBuf = tmp;
						break;
					}
					srcID++;
				}

				if( destBuf == null && srcBuf == null ) return;


				if( destBuf != null && srcBuf != null )
				{
					if( srcRow != destRow )
					{
						m_messageBaseData[destRow].standDatas[destCol] = srcBuf;
						m_messageBaseData[srcRow].standDatas[srcCol]   = destBuf;
					}
					int swapBankNo = srcBuf.bankID;
					srcBuf.bankID = destBuf.bankID;
					destBuf.bankID = swapBankNo;
				}

				if( destBuf != null && srcBuf == null )
				{
					destBuf.bankID = srcCol-1;
					m_messageBaseData[destRow].standDatas.Remove(destBuf);
					m_messageBaseData[srcRow].standDatas.Add(destBuf);
				}

				if( destBuf == null && srcBuf != null )
				{
					srcBuf.bankID = destCol-1;
					m_messageBaseData[destRow].standDatas.Add( srcBuf );
					m_messageBaseData[srcRow].standDatas.Remove(srcBuf);
				}

			

			UpdateBlockTxtToList(false);
		}

		private void SwapFace( int srcRow, int srcCol, int dstRow, int dstCol )
		{
			string buf = m_messageBaseData[m_dragDestRow].faceFileName;
			m_messageBaseData[m_dragDestRow].faceFileName = m_messageBaseData[m_dragSrcRow].faceFileName;
			m_messageBaseData[m_dragSrcRow].faceFileName = buf;
			UpdateBlockTxtToList(false);
		}

		private void SwapBG( int srcRow, int srcCol, int dstRow, int dstCol )
		{
			string buf = m_messageBaseData[m_dragDestRow].bgFileName;
			m_messageBaseData[m_dragDestRow].bgFileName = m_messageBaseData[m_dragSrcRow].bgFileName;
			m_messageBaseData[m_dragSrcRow].bgFileName = buf;

			UpdateBlockTxtToList(false);
		}

		private void dataGridView1_DragDrop(object sender, DragEventArgs e)
		{
			Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));

			var destCell	= dataGridView1.HitTest(clientPoint.X, clientPoint.Y);
			m_dragDestRow	= destCell.RowIndex/2;
			m_dragDestCol	= destCell.ColumnIndex;

			if( m_dragDestRow == -1 || m_dragDestCol == -1 ) return;

			if (e.Effect == DragDropEffects.Move)
			{
				DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;

				if( m_dragSrcCol == 1 && m_dragDestCol == 1 ) SwapBG(m_dragSrcRow,m_dragSrcCol,m_dragDestRow,m_dragDestCol);

				if( m_dragSrcCol == 7 && m_dragDestCol == 7 ) SwapFace(m_dragSrcRow,m_dragSrcCol,m_dragDestRow,m_dragDestCol);

				if( 2 <= m_dragSrcCol && m_dragSrcCol <= 6 )
					if( 2 <= m_dragDestCol && m_dragDestCol <= 6  ) 
						SwapStand(m_dragSrcRow,m_dragSrcCol,m_dragDestRow,m_dragDestCol);


				dataGridView1.ClearSelection();
				dataGridView1.Rows[m_dragDestRow].Cells[m_dragDestCol].Selected = true;
			}
		}




		private void dataGridView1_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}



		private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
			{
				DragDropEffects dropEffect = dataGridView1.DoDragDrop(dataGridView1.Rows[m_dragSrcRow],  DragDropEffects.Move);
			}
		}

		private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
		{
			if( e.Button == MouseButtons.Right )
			{

				var clickCell = dataGridView1.HitTest (e.X, e.Y);
				m_dragSrcRow = clickCell.RowIndex/2;
				m_dragSrcCol = clickCell.ColumnIndex;

				if( m_dragSrcRow != -1 && m_dragSrcCol != -1 )
				{

					((DataGridView)sender).ClearSelection();
					((DataGridView)sender).Rows[m_dragSrcRow*2].Cells[m_dragSrcCol].Selected = true;
				}
			}
		}
	}

	/// <summary>
	/// Class used for demo purposes. This could be anything listed in a CheckBoxComboBox.
	/// </summary>
	public class Status
    {
        public Status(int id, string name) { _Id = id; _Name = name; }

        private int _Id;
        private string _Name;

        public int Id { get { return _Id; } set { _Id = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }

        /// <summary>
        /// Now used to return the Name.
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return Name; }
    }
    /// <summary>
    /// Class used for demo purposes. A list of "Status". 
    /// This represents the custom "IList" datasource of anything listed in a CheckBoxComboBox.
    /// </summary>
    public class StatusList : List<Status>
    {
    }
}
