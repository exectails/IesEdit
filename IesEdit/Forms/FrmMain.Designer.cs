namespace IesEdit
{
	partial class FrmMain
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
			this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.LblCurLine = new System.Windows.Forms.ToolStripStatusLabel();
			this.LblCurCol = new System.Windows.Forms.ToolStripStatusLabel();
			this.ToolStrip = new System.Windows.Forms.ToolStrip();
			this.BtnOpen = new System.Windows.Forms.ToolStripButton();
			this.BtnSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.BtnUndo = new System.Windows.Forms.ToolStripButton();
			this.BtnRedo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.BtnSearch = new System.Windows.Forms.ToolStripButton();
			this.SplMain = new System.Windows.Forms.SplitContainer();
			this.TxtEditor = new ScintillaNET.Scintilla();
			this.StatusStrip = new System.Windows.Forms.StatusStrip();
			this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.MainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.MnuOpen = new System.Windows.Forms.MenuItem();
			this.MnuSave = new System.Windows.Forms.MenuItem();
			this.MnuSaveIes = new System.Windows.Forms.MenuItem();
			this.MnuSaveAsXml = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.MnuExit = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.MnuUndo = new System.Windows.Forms.MenuItem();
			this.MnuRedo = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.MnuSearch = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.MnuAbout = new System.Windows.Forms.MenuItem();
			this.ToolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SplMain)).BeginInit();
			this.SplMain.Panel1.SuspendLayout();
			this.SplMain.SuspendLayout();
			this.StatusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// SaveFileDialog
			// 
			this.SaveFileDialog.Filter = "XML file|*.xml";
			// 
			// LblCurLine
			// 
			this.LblCurLine.AutoSize = false;
			this.LblCurLine.Name = "LblCurLine";
			this.LblCurLine.Padding = new System.Windows.Forms.Padding(0, 0, 25, 0);
			this.LblCurLine.Size = new System.Drawing.Size(60, 19);
			this.LblCurLine.Text = "Line 0";
			this.LblCurLine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblCurCol
			// 
			this.LblCurCol.AutoSize = false;
			this.LblCurCol.Name = "LblCurCol";
			this.LblCurCol.Padding = new System.Windows.Forms.Padding(0, 0, 25, 0);
			this.LblCurCol.Size = new System.Drawing.Size(60, 19);
			this.LblCurCol.Text = "Col 0";
			this.LblCurCol.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ToolStrip
			// 
			this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.BtnOpen,
			this.BtnSave,
			this.toolStripSeparator1,
			this.BtnUndo,
			this.BtnRedo,
			this.toolStripSeparator2,
			this.BtnSearch});
			this.ToolStrip.Location = new System.Drawing.Point(0, 0);
			this.ToolStrip.Name = "ToolStrip";
			this.ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.ToolStrip.Size = new System.Drawing.Size(1052, 25);
			this.ToolStrip.TabIndex = 5;
			this.ToolStrip.Text = "toolStrip1";
			// 
			// BtnOpen
			// 
			this.BtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.BtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("BtnOpen.Image")));
			this.BtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.BtnOpen.Name = "BtnOpen";
			this.BtnOpen.Size = new System.Drawing.Size(23, 22);
			this.BtnOpen.Text = "Open...";
			this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
			// 
			// BtnSave
			// 
			this.BtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.BtnSave.Enabled = false;
			this.BtnSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnSave.Image")));
			this.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new System.Drawing.Size(23, 22);
			this.BtnSave.Text = "Save";
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// BtnUndo
			// 
			this.BtnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.BtnUndo.Enabled = false;
			this.BtnUndo.Image = ((System.Drawing.Image)(resources.GetObject("BtnUndo.Image")));
			this.BtnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.BtnUndo.Name = "BtnUndo";
			this.BtnUndo.Size = new System.Drawing.Size(23, 22);
			this.BtnUndo.Text = "Undo";
			this.BtnUndo.Click += new System.EventHandler(this.BtnUndo_Click);
			// 
			// BtnRedo
			// 
			this.BtnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.BtnRedo.Enabled = false;
			this.BtnRedo.Image = ((System.Drawing.Image)(resources.GetObject("BtnRedo.Image")));
			this.BtnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.BtnRedo.Name = "BtnRedo";
			this.BtnRedo.Size = new System.Drawing.Size(23, 22);
			this.BtnRedo.Text = "Redo";
			this.BtnRedo.Click += new System.EventHandler(this.BtnRedo_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// BtnSearch
			// 
			this.BtnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.BtnSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnSearch.Image")));
			this.BtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.BtnSearch.Name = "BtnSearch";
			this.BtnSearch.Size = new System.Drawing.Size(23, 22);
			this.BtnSearch.Text = "Search";
			this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
			// 
			// SplMain
			// 
			this.SplMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SplMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.SplMain.IsSplitterFixed = true;
			this.SplMain.Location = new System.Drawing.Point(0, 25);
			this.SplMain.Name = "SplMain";
			// 
			// SplMain.Panel1
			// 
			this.SplMain.Panel1.Controls.Add(this.TxtEditor);
			this.SplMain.Panel2Collapsed = true;
			this.SplMain.Size = new System.Drawing.Size(1052, 643);
			this.SplMain.SplitterDistance = 774;
			this.SplMain.TabIndex = 7;
			// 
			// TxtEditor
			// 
			this.TxtEditor.AllowDrop = true;
			this.TxtEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TxtEditor.CaretLineBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
			this.TxtEditor.CaretLineVisible = true;
			this.TxtEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TxtEditor.HighlightGuide = 0;
			this.TxtEditor.IndentationGuides = ScintillaNET.IndentView.Real;
			this.TxtEditor.Location = new System.Drawing.Point(0, 0);
			this.TxtEditor.Name = "TxtEditor";
			this.TxtEditor.RectangularSelectionAnchor = 0;
			this.TxtEditor.RectangularSelectionAnchorVirtualSpace = 0;
			this.TxtEditor.RectangularSelectionCaret = 0;
			this.TxtEditor.RectangularSelectionCaretVirtualSpace = 0;
			this.TxtEditor.Size = new System.Drawing.Size(1052, 643);
			this.TxtEditor.TabIndex = 0;
			// 
			// StatusStrip
			// 
			this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.LblCurLine,
			this.LblCurCol});
			this.StatusStrip.Location = new System.Drawing.Point(0, 668);
			this.StatusStrip.Name = "StatusStrip";
			this.StatusStrip.Size = new System.Drawing.Size(1052, 24);
			this.StatusStrip.TabIndex = 6;
			this.StatusStrip.Text = "statusStrip1";
			// 
			// OpenFileDialog
			// 
			this.OpenFileDialog.Filter = "IES File|*.ies";
			// 
			// MainMenu
			// 
			this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.menuItem1,
			this.menuItem3,
			this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.MnuOpen,
			this.MnuSave,
			this.MnuSaveIes,
			this.MnuSaveAsXml,
			this.menuItem5,
			this.MnuExit});
			this.menuItem1.Text = "File";
			// 
			// MnuOpen
			// 
			this.MnuOpen.Index = 0;
			this.MnuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.MnuOpen.Text = "Open...";
			this.MnuOpen.Click += new System.EventHandler(this.BtnOpen_Click);
			// 
			// MnuSave
			// 
			this.MnuSave.Enabled = false;
			this.MnuSave.Index = 1;
			this.MnuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.MnuSave.Text = "Save";
			this.MnuSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// MnuSaveIes
			// 
			this.MnuSaveIes.Enabled = false;
			this.MnuSaveIes.Index = 2;
			this.MnuSaveIes.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
			this.MnuSaveIes.Text = "Save as IES...";
			this.MnuSaveIes.Click += new System.EventHandler(this.MnuSaveIes_Click);
			// 
			// MnuSaveAsXml
			// 
			this.MnuSaveAsXml.Enabled = false;
			this.MnuSaveAsXml.Index = 3;
			this.MnuSaveAsXml.Text = "Save as XML...";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 4;
			this.menuItem5.Text = "-";
			// 
			// MnuExit
			// 
			this.MnuExit.Index = 5;
			this.MnuExit.Text = "Exit";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.MnuUndo,
			this.MnuRedo,
			this.menuItem9,
			this.MnuSearch});
			this.menuItem3.Text = "Edit";
			// 
			// MnuUndo
			// 
			this.MnuUndo.Index = 0;
			this.MnuUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.MnuUndo.Text = "Undo";
			this.MnuUndo.Click += new System.EventHandler(this.BtnUndo_Click);
			// 
			// MnuRedo
			// 
			this.MnuRedo.Index = 1;
			this.MnuRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
			this.MnuRedo.Text = "Redo";
			this.MnuRedo.Click += new System.EventHandler(this.BtnRedo_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 2;
			this.menuItem9.Text = "-";
			// 
			// MnuSearch
			// 
			this.MnuSearch.Index = 3;
			this.MnuSearch.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
			this.MnuSearch.Text = "Search";
			this.MnuSearch.Click += new System.EventHandler(this.BtnSearch_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 2;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.MnuAbout});
			this.menuItem2.Text = "?";
			// 
			// MnuAbout
			// 
			this.MnuAbout.Index = 0;
			this.MnuAbout.Text = "About";
			this.MnuAbout.Click += new System.EventHandler(this.MnuAbout_Click);
			// 
			// FrmMain
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1052, 692);
			this.Controls.Add(this.SplMain);
			this.Controls.Add(this.StatusStrip);
			this.Controls.Add(this.ToolStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.MainMenu;
			this.Name = "FrmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "IesEdit";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
			this.Load += new System.EventHandler(this.FrmMain_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrmMain_DragEnter);
			this.ToolStrip.ResumeLayout(false);
			this.ToolStrip.PerformLayout();
			this.SplMain.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.SplMain)).EndInit();
			this.SplMain.ResumeLayout(false);
			this.StatusStrip.ResumeLayout(false);
			this.StatusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SaveFileDialog SaveFileDialog;
		private System.Windows.Forms.ToolStripStatusLabel LblCurLine;
		private System.Windows.Forms.ToolStripStatusLabel LblCurCol;
		private System.Windows.Forms.ToolStrip ToolStrip;
		private System.Windows.Forms.ToolStripButton BtnOpen;
		private System.Windows.Forms.ToolStripButton BtnSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton BtnUndo;
		private System.Windows.Forms.ToolStripButton BtnRedo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton BtnSearch;
		private System.Windows.Forms.SplitContainer SplMain;
		private System.Windows.Forms.StatusStrip StatusStrip;
		private System.Windows.Forms.OpenFileDialog OpenFileDialog;
		private System.Windows.Forms.MainMenu MainMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem MnuOpen;
		private System.Windows.Forms.MenuItem MnuSave;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem MnuExit;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem MnuUndo;
		private System.Windows.Forms.MenuItem MnuRedo;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem MnuSearch;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem MnuAbout;
		private ScintillaNET.Scintilla TxtEditor;
		private System.Windows.Forms.MenuItem MnuSaveIes;
		private System.Windows.Forms.MenuItem MnuSaveAsXml;
	}
}

