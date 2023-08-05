using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace IesEdit
{
	/// <summary>
	/// About form, giving information about the program.
	/// </summary>
	public partial class FrmAbout : Form
	{
		/// <summary>
		/// Initializes the form.
		/// </summary>
		public FrmAbout()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Closes the form when the OK button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Opens the clicked image's link in its Tag.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LinkImage_Click(object sender, EventArgs e)
		{
			var tag = ((sender as Control)?.Tag as string);
			if (tag != null)
				Process.Start(tag);
		}
	}
}
