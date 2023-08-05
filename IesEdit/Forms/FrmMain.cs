using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using IesEdit.Ies;
using IesEdit.Properties;
using ScintillaNET;
using static System.Net.Mime.MediaTypeNames;

namespace IesEdit
{
	/// <summary>
	/// Application's main form.
	/// </summary>
	public partial class FrmMain : Form
	{
		private readonly string _windowTitle;
		private string _openedFilePath;
		private bool _fileChanged;

		private FrmSearch _searchForm;

		/// <summary>
		/// Initializes the form.
		/// </summary>
		public FrmMain()
		{
			CultureInfo.CurrentCulture =
			CultureInfo.CurrentUICulture =
			CultureInfo.DefaultThreadCurrentCulture =
			CultureInfo.DefaultThreadCurrentUICulture =
				new CultureInfo("en-US");

			this.InitializeComponent();
			this.SetUpEditor();

			_windowTitle = this.Text;
			this.ToolStrip.Renderer = new ToolStripRendererNL();

			this.UpdateSaveButtons();
		}

		/// <summary>
		/// Called when the form is loaded.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FrmMain_Load(object sender, EventArgs e)
		{
			var args = Environment.GetCommandLineArgs();
			if (args.Length > 1)
			{
				var filePath = args[1];
				this.OpenFile(filePath);
			}

			if (Settings.Default.WindowMaximized)
			{
				this.WindowState = FormWindowState.Maximized;
			}
			else if (Settings.Default.WindowLocation.X != -1 || Settings.Default.WindowLocation.Y != -1)
			{
				this.WindowState = FormWindowState.Normal;
				this.Location = Settings.Default.WindowLocation;
				this.Size = Settings.Default.WindowSize;
			}
		}

		/// <summary>
		/// Called when the form is closing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Default.WindowMaximized = (this.WindowState == FormWindowState.Maximized);
			if (this.WindowState == FormWindowState.Normal)
			{
				Settings.Default.WindowLocation = this.Location;
				Settings.Default.WindowSize = this.Size;
			}

			Settings.Default.Save();
		}

		/// <summary>
		/// Sets up the editor for XML code and subscribes to its events.
		/// </summary>
		public void SetUpEditor()
		{
			this.TxtEditor.Styles[Style.Default].Font = "Courier New";
			this.TxtEditor.Styles[Style.Default].Size = 10;
			this.TxtEditor.Styles[Style.Xml.XmlStart].ForeColor = Color.Blue;
			this.TxtEditor.Styles[Style.Xml.XmlEnd].ForeColor = Color.Blue;
			this.TxtEditor.Styles[Style.Xml.TagEnd].ForeColor = Color.Blue;
			this.TxtEditor.Styles[Style.Xml.Tag].ForeColor = Color.Blue;
			this.TxtEditor.Styles[Style.Xml.TagEnd].ForeColor = Color.Blue;
			this.TxtEditor.Styles[Style.Xml.Attribute].ForeColor = Color.Red;
			this.TxtEditor.Styles[Style.Xml.DoubleString].ForeColor = Color.Blue;
			this.TxtEditor.Styles[Style.Xml.SingleString].ForeColor = Color.Blue;
			this.TxtEditor.Styles[Style.Xml.Comment].ForeColor = Color.Green;
			this.TxtEditor.Styles[Style.IndentGuide].ForeColor = Color.LightGray;
			this.TxtEditor.Margins[0].Width = 40;
			this.TxtEditor.Lexer = Lexer.Xml;
			this.TxtEditor.TextChanged += this.TxtEditor_OnTextChanged;
			this.TxtEditor.UpdateUI += this.TxtEditor_OnUpdateUI;
		}

		/// <summary>
		/// Called if Ctrl+F is pressed in the editor.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TxtEditor_OnCtrlF(object sender, EventArgs e)
		{
			//this.BtnSearch_Click(null, null);
		}

		/// <summary>
		/// Called if the editor's text or styling have changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TxtEditor_OnUpdateUI(object sender, UpdateUIEventArgs e)
		{
			if ((e.Change & UpdateChange.Selection) != 0 || (e.Change & UpdateChange.Content) != 0)
			{
				var pos = this.TxtEditor.CurrentPosition;
				var line = this.TxtEditor.LineFromPosition(pos);
				var col = (pos - this.TxtEditor.Lines[line].Position);

				this.LblCurLine.Text = "Line " + (line + 1);
				this.LblCurCol.Text = "Col " + (col + 1);
			}
		}

		/// <summary>
		/// Called when text in the editor changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TxtEditor_OnTextChanged(object sender, EventArgs e)
		{
			_fileChanged = true;

			this.UpdateUndo();
			this.UpdateSaveButtons();
			this.UpdateTitle();
		}

		/// <summary>
		/// Called when Ctrl+S is pressed in the editor.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TxtEditor_OnCtrlS(object sender, EventArgs e)
		{
			if (this.BtnSave.Enabled)
				this.BtnSave.PerformClick();
		}

		/// <summary>
		/// Updated Undo and Redo buttons, based on the editor's state.
		/// </summary>
		private void UpdateUndo()
		{
			this.BtnUndo.Enabled = this.TxtEditor.CanUndo;
			this.BtnRedo.Enabled = this.TxtEditor.CanRedo;
		}

		/// <summary>
		/// Resets Undo and Redo in the editor and updates the buttons.
		/// </summary>
		private void ResetUndo()
		{
			this.TxtEditor.EmptyUndoBuffer();
			this.UpdateUndo();
		}

		/// <summary>
		/// Toggles save buttons, based on whether saving is possible right
		/// now.
		/// </summary>
		private void UpdateSaveButtons()
		{
			var enabled = (_fileChanged && !string.IsNullOrWhiteSpace(_openedFilePath));
			this.MnuSave.Enabled = this.BtnSave.Enabled = enabled;

			var fileOpen = (_openedFilePath != null);
			this.MnuSaveAsIes.Enabled = fileOpen;
			this.MnuSaveAsXml.Enabled = fileOpen;
		}

		/// <summary>
		/// Called if a file is dragged onto the form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FrmMain_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		/// <summary>
		/// Called if a file is dropped on the form. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FrmMain_DragDrop(object sender, DragEventArgs e)
		{
			var files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files.Length == 0)
				return;

			this.OpenFile(files[0]);
		}

		/// <summary>
		/// Called when one of the Open buttons is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnOpen_Click(object sender, EventArgs e)
		{
			var result = this.OpenFileDialog.ShowDialog();
			if (result != DialogResult.OK)
				return;

			this.OpenFile(OpenFileDialog.FileName);
		}

		/// <summary>
		/// Called if one of the Save buttons is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnSave_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(_openedFilePath))
				return;

			this.SaveFile(_openedFilePath);
		}


		/// <summary>
		/// Called if the Save as IES menu item is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MnuSaveAsIes_Click(object sender, EventArgs e)
		{
			var fileName = Path.GetFileNameWithoutExtension(_openedFilePath) + ".ies";

			this.SaveFileDialog.InitialDirectory = Path.GetDirectoryName(_openedFilePath);
			this.SaveFileDialog.FileName = fileName;
			this.SaveFileDialog.Filter = "IES Files (*.ies)|*.ies";

			if (this.SaveFileDialog.ShowDialog() != DialogResult.OK)
				return;

			this.SaveFile(this.SaveFileDialog.FileName);
		}

		/// <summary>
		/// Called if the Save as XML menu item is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MnuSaveAsXml_Click(object sender, EventArgs e)
		{
			var fileName = Path.GetFileNameWithoutExtension(_openedFilePath) + ".xml";

			this.SaveFileDialog.InitialDirectory = Path.GetDirectoryName(_openedFilePath);
			this.SaveFileDialog.FileName = fileName;
			this.SaveFileDialog.Filter = "XML Files (*.xml)|*.xml";

			if (this.SaveFileDialog.ShowDialog() != DialogResult.OK)
				return;

			this.SaveFile(this.SaveFileDialog.FileName);
		}

		/// <summary>
		/// Called if the Undo button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnUndo_Click(object sender, EventArgs e)
		{
			this.TxtEditor.Undo();

			this.UpdateUndo();
			this.UpdateSaveButtons();
		}

		/// <summary>
		/// Called if the Redo button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnRedo_Click(object sender, EventArgs e)
		{
			this.TxtEditor.Redo();

			this.UpdateUndo();
			this.UpdateSaveButtons();
		}

		/// <summary>
		/// Called if the Exit menu item is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Called if Search button is clicked, opens Search form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnSearch_Click(object sender, EventArgs e)
		{
			var firstTime = false;

			if (_searchForm == null)
			{
				_searchForm = new FrmSearch(this);
				firstTime = true;
			}

			if (!_searchForm.Visible)
				_searchForm.Show(this);
			else
				_searchForm.Focus();

			if (firstTime)
			{
				var x = this.Location.X + this.Width / 2 - _searchForm.Width / 2;
				var y = this.Location.Y + this.Height / 2 - _searchForm.Height / 2;
				_searchForm.Location = new Point(x, y);
			}

			if (this.TxtEditor.SelectedText.Length > 0)
				_searchForm.SetSearchText(this.TxtEditor.SelectedText);

			_searchForm.SelectSearchText();
		}

		/// <summary>
		/// Called if the About menu item is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MnuAbout_Click(object sender, EventArgs e)
		{
			var form = new FrmAbout();
			form.ShowDialog();
		}

		/// <summary>
		/// Updates the window title based on the opened file and whether
		/// it was modified.
		/// </summary>
		private void UpdateTitle()
		{
			var title = _windowTitle;

			if (_openedFilePath != null)
			{
				title = string.Format("{0} - {1}", _openedFilePath, _windowTitle);

				if (_fileChanged)
					title = "*" + title;
			}

			this.Text = title;
		}

		/// <summary>
		/// Opens the given file.
		/// </summary>
		/// <param name="filePath"></param>
		private void OpenFile(string filePath)
		{
			try
			{
				var iesFile = IesFile.LoadIesFile(filePath);
				var text = iesFile.GetXml();

				this.TxtEditor.Text = text;

				_openedFilePath = filePath;
				_fileChanged = false;

				this.ResetUndo();
				this.UpdateSaveButtons();
				this.UpdateTitle();
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Failed to open '{0}'. Error: {1}", filePath, ex), _windowTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Saves the current file.
		/// </summary>
		/// <param name="filePath"></param>
		private void SaveFile(string filePath)
		{
			try
			{
				if (filePath.EndsWith(".xml"))
				{
					var text = this.TxtEditor.Text;
					File.WriteAllText(filePath, text);
				}
				else
				{
					var text = this.TxtEditor.Text;
					var fileName = Path.GetFileNameWithoutExtension(_openedFilePath);

					var iesFile = IesFile.LoadXmlString(fileName, text);
					iesFile.SaveIes(filePath);
				}

				_fileChanged = false;

				this.UpdateSaveButtons();
				this.UpdateTitle();
			}
			catch (XmlException ex)
			{
				MessageBox.Show("There's an error in the XML code. " + ex.Message, _windowTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Failed to save '{0}'. Error: {1}", _openedFilePath, ex), _windowTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Jumps to the next occurance of given text in editor.
		/// </summary>
		/// <param name="searchText">Text to search for, starting at current position.</param>
		/// <returns>True if text was found, false if not.</returns>
		public bool SearchFor(string searchText)
		{
			var text = this.TxtEditor.Text;
			var currentPos = this.TxtEditor.CurrentPosition;
			var selectLength = searchText.Length;

			if (string.IsNullOrEmpty(text))
				return false;

			currentPos++;
			if (currentPos > text.Length - 1)
				currentPos = 0;

			var nextIndex = text.IndexOf(searchText, currentPos, StringComparison.CurrentCultureIgnoreCase);
			if (nextIndex == -1)
			{
				nextIndex = text.IndexOf(searchText, 0, StringComparison.CurrentCultureIgnoreCase);
				if (nextIndex == -1)
					return false;
			}

			this.TxtEditor.SetSelection(nextIndex + selectLength, nextIndex);
			this.TxtEditor.ScrollRange(nextIndex + selectLength, nextIndex);

			return true;
		}
	}
}
