using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace standScripter
{
    public partial class FormConfig : Form
    {

		DataManger m_rDataManager = null;

		public FormConfig(DataManger refDataManager) { 
			InitializeComponent();
			m_rDataManager = refDataManager;
		}

        private void button1_Click(object sender, EventArgs e)
        {
			GetUI();
			this.Close();
		}

        private void button2_Click(object sender, EventArgs e)
        {
			this.Close();
		}

        private void button3_Click(object sender, EventArgs e)
        {
			ColorDialogButton((Button)sender);
		}

        private void button4_Click(object sender, EventArgs e)
        {
			ColorDialogButton((Button)sender);

		}


		private void ColorDialogButton(Button btn)
		{
			ColorDialog cd = new ColorDialog();

			//はじめに選択されている色を設定
			cd.Color = btn.BackColor;

			//[作成した色]に指定した色（RGB値）を表示する
			cd.CustomColors = new int[] {
				0x33, 0x66, 0x99, 0xCC, 0x3300, 0x3333,
				0x3366, 0x3399, 0x33CC, 0x6600, 0x6633,
				0x6666, 0x6699, 0x66CC, 0x9900, 0x9933};


			//ダイアログを表示する
			if (cd.ShowDialog() == DialogResult.OK)
			{
				//選択された色の取得
				//			m_tabInfo[itemIndex].m_color = cd.Color;

				btn.BackColor = cd.Color;

			}
		}

		private void SetUI()
		{
			button3.BackColor = m_rDataManager.m_previewWindowColor;
			button4.BackColor = m_rDataManager.m_previewTextColor;
			numericUpDown1.Value = m_rDataManager.m_previewWindowalpha;
		}


		private void GetUI()
		{
			m_rDataManager.m_previewWindowColor = button3.BackColor;
			m_rDataManager.m_previewTextColor = button4.BackColor;
			m_rDataManager.m_previewWindowalpha = (int)numericUpDown1.Value;
		}

        private void FormConfig_Load(object sender, EventArgs e)
        {
			SetUI();
		}
    }
}
