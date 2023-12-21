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
	public partial class helpForm : Form
	{
		private int posX = 0;
		private int posY = 0;

		public helpForm(int x,int y)
		{
			InitializeComponent();

			posX = x - this.Width/2;
			posY = y - this.Height/2;
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void helpForm_Load(object sender, EventArgs e)
		{
			this.Left = posX;
			this.Top = posY;
		}
	}
}
