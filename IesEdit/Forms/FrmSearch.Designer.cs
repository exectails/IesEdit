namespace IesEdit
{
	partial class FrmSearch
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.BtnClose = new System.Windows.Forms.Button();
			this.BtnSearch = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.TxtSearchText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// BtnClose
			// 
			this.BtnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnClose.Location = new System.Drawing.Point(288, 37);
			this.BtnClose.Name = "BtnClose";
			this.BtnClose.Size = new System.Drawing.Size(75, 23);
			this.BtnClose.TabIndex = 7;
			this.BtnClose.Text = "Close";
			this.BtnClose.UseVisualStyleBackColor = true;
			this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
			// 
			// BtnSearch
			// 
			this.BtnSearch.Location = new System.Drawing.Point(207, 37);
			this.BtnSearch.Name = "BtnSearch";
			this.BtnSearch.Size = new System.Drawing.Size(75, 23);
			this.BtnSearch.TabIndex = 6;
			this.BtnSearch.Text = "Search";
			this.BtnSearch.UseVisualStyleBackColor = true;
			this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Search for";
			// 
			// TxtSearchText
			// 
			this.TxtSearchText.Location = new System.Drawing.Point(74, 11);
			this.TxtSearchText.Name = "TxtSearchText";
			this.TxtSearchText.Size = new System.Drawing.Size(289, 20);
			this.TxtSearchText.TabIndex = 4;
			// 
			// FrmSearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(374, 71);
			this.Controls.Add(this.BtnClose);
			this.Controls.Add(this.BtnSearch);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.TxtSearchText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmSearch";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Search";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BtnClose;
		private System.Windows.Forms.Button BtnSearch;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TxtSearchText;
	}
}
