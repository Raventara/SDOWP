namespace SDOW_P
{
	partial class Form1
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
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.pbScreensPreview = new System.Windows.Forms.PictureBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.tclWallpapers = new System.Windows.Forms.TabControl();
			this.tpSDO = new System.Windows.Forms.TabPage();
			this.btnSDOPreview = new System.Windows.Forms.Button();
			this.rbSliced = new System.Windows.Forms.RadioButton();
			this.rbConsecutive = new System.Windows.Forms.RadioButton();
			this.pbSDO2 = new SDOW_P.ToggleImageButton();
			this.pbSDO3 = new SDOW_P.ToggleImageButton();
			this.pbSDO4 = new SDOW_P.ToggleImageButton();
			this.pbSDO5 = new SDOW_P.ToggleImageButton();
			this.pbSDO6 = new SDOW_P.ToggleImageButton();
			this.pbSDO7 = new SDOW_P.ToggleImageButton();
			this.pbSDO1 = new SDOW_P.ToggleImageButton();
			this.tpStatic = new System.Windows.Forms.TabPage();
			this.pbStatic = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pbScreensPreview)).BeginInit();
			this.tclWallpapers.SuspendLayout();
			this.tpSDO.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO1)).BeginInit();
			this.tpStatic.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbStatic)).BeginInit();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(282, 315);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(184, 134);
			this.listBox1.TabIndex = 1;
			// 
			// pbScreensPreview
			// 
			this.pbScreensPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbScreensPreview.Location = new System.Drawing.Point(12, 12);
			this.pbScreensPreview.Name = "pbScreensPreview";
			this.pbScreensPreview.Size = new System.Drawing.Size(454, 297);
			this.pbScreensPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbScreensPreview.TabIndex = 3;
			this.pbScreensPreview.TabStop = false;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(472, 12);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(175, 21);
			this.comboBox1.TabIndex = 4;
			// 
			// tclWallpapers
			// 
			this.tclWallpapers.Controls.Add(this.tpSDO);
			this.tclWallpapers.Controls.Add(this.tpStatic);
			this.tclWallpapers.Location = new System.Drawing.Point(473, 40);
			this.tclWallpapers.Name = "tclWallpapers";
			this.tclWallpapers.SelectedIndex = 0;
			this.tclWallpapers.Size = new System.Drawing.Size(399, 269);
			this.tclWallpapers.TabIndex = 5;
			// 
			// tpSDO
			// 
			this.tpSDO.Controls.Add(this.btnSDOPreview);
			this.tpSDO.Controls.Add(this.rbSliced);
			this.tpSDO.Controls.Add(this.rbConsecutive);
			this.tpSDO.Controls.Add(this.pbSDO2);
			this.tpSDO.Controls.Add(this.pbSDO3);
			this.tpSDO.Controls.Add(this.pbSDO4);
			this.tpSDO.Controls.Add(this.pbSDO5);
			this.tpSDO.Controls.Add(this.pbSDO6);
			this.tpSDO.Controls.Add(this.pbSDO7);
			this.tpSDO.Controls.Add(this.pbSDO1);
			this.tpSDO.Location = new System.Drawing.Point(4, 22);
			this.tpSDO.Name = "tpSDO";
			this.tpSDO.Padding = new System.Windows.Forms.Padding(3);
			this.tpSDO.Size = new System.Drawing.Size(391, 243);
			this.tpSDO.TabIndex = 1;
			this.tpSDO.Text = "SDO Images";
			this.tpSDO.UseVisualStyleBackColor = true;
			// 
			// btnSDOPreview
			// 
			this.btnSDOPreview.Location = new System.Drawing.Point(6, 214);
			this.btnSDOPreview.Name = "btnSDOPreview";
			this.btnSDOPreview.Size = new System.Drawing.Size(75, 23);
			this.btnSDOPreview.TabIndex = 10;
			this.btnSDOPreview.Text = "Pre&view";
			this.btnSDOPreview.UseVisualStyleBackColor = true;
			this.btnSDOPreview.Click += new System.EventHandler(this.btnSDOPreview_Click);
			// 
			// rbSliced
			// 
			this.rbSliced.AutoSize = true;
			this.rbSliced.Location = new System.Drawing.Point(133, 220);
			this.rbSliced.Name = "rbSliced";
			this.rbSliced.Size = new System.Drawing.Size(54, 17);
			this.rbSliced.TabIndex = 9;
			this.rbSliced.TabStop = true;
			this.rbSliced.Text = "Sliced";
			this.rbSliced.UseVisualStyleBackColor = true;
			// 
			// rbConsecutive
			// 
			this.rbConsecutive.AutoSize = true;
			this.rbConsecutive.Location = new System.Drawing.Point(198, 220);
			this.rbConsecutive.Name = "rbConsecutive";
			this.rbConsecutive.Size = new System.Drawing.Size(84, 17);
			this.rbConsecutive.TabIndex = 8;
			this.rbConsecutive.TabStop = true;
			this.rbConsecutive.Text = "Consecutive";
			this.rbConsecutive.UseVisualStyleBackColor = true;
			// 
			// pbSDO2
			// 
			this.pbSDO2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbSDO2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbSDO2.Location = new System.Drawing.Point(102, 6);
			this.pbSDO2.Name = "pbSDO2";
			this.pbSDO2.Selected = true;
			this.pbSDO2.Size = new System.Drawing.Size(90, 90);
			this.pbSDO2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbSDO2.TabIndex = 7;
			this.pbSDO2.TabStop = false;
			// 
			// pbSDO3
			// 
			this.pbSDO3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbSDO3.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbSDO3.Location = new System.Drawing.Point(198, 6);
			this.pbSDO3.Name = "pbSDO3";
			this.pbSDO3.Selected = true;
			this.pbSDO3.Size = new System.Drawing.Size(90, 90);
			this.pbSDO3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbSDO3.TabIndex = 6;
			this.pbSDO3.TabStop = false;
			// 
			// pbSDO4
			// 
			this.pbSDO4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbSDO4.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbSDO4.Location = new System.Drawing.Point(294, 6);
			this.pbSDO4.Name = "pbSDO4";
			this.pbSDO4.Selected = true;
			this.pbSDO4.Size = new System.Drawing.Size(90, 90);
			this.pbSDO4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbSDO4.TabIndex = 5;
			this.pbSDO4.TabStop = false;
			// 
			// pbSDO5
			// 
			this.pbSDO5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbSDO5.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbSDO5.Location = new System.Drawing.Point(52, 102);
			this.pbSDO5.Name = "pbSDO5";
			this.pbSDO5.Selected = true;
			this.pbSDO5.Size = new System.Drawing.Size(90, 90);
			this.pbSDO5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbSDO5.TabIndex = 4;
			this.pbSDO5.TabStop = false;
			// 
			// pbSDO6
			// 
			this.pbSDO6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbSDO6.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbSDO6.Location = new System.Drawing.Point(148, 102);
			this.pbSDO6.Name = "pbSDO6";
			this.pbSDO6.Selected = true;
			this.pbSDO6.Size = new System.Drawing.Size(90, 90);
			this.pbSDO6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbSDO6.TabIndex = 3;
			this.pbSDO6.TabStop = false;
			// 
			// pbSDO7
			// 
			this.pbSDO7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbSDO7.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbSDO7.Location = new System.Drawing.Point(244, 102);
			this.pbSDO7.Name = "pbSDO7";
			this.pbSDO7.Selected = true;
			this.pbSDO7.Size = new System.Drawing.Size(90, 90);
			this.pbSDO7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbSDO7.TabIndex = 2;
			this.pbSDO7.TabStop = false;
			// 
			// pbSDO1
			// 
			this.pbSDO1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbSDO1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbSDO1.Location = new System.Drawing.Point(6, 6);
			this.pbSDO1.Name = "pbSDO1";
			this.pbSDO1.Selected = true;
			this.pbSDO1.Size = new System.Drawing.Size(90, 90);
			this.pbSDO1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbSDO1.TabIndex = 1;
			this.pbSDO1.TabStop = false;
			// 
			// tpStatic
			// 
			this.tpStatic.Controls.Add(this.pbStatic);
			this.tpStatic.Location = new System.Drawing.Point(4, 22);
			this.tpStatic.Name = "tpStatic";
			this.tpStatic.Padding = new System.Windows.Forms.Padding(3);
			this.tpStatic.Size = new System.Drawing.Size(391, 243);
			this.tpStatic.TabIndex = 0;
			this.tpStatic.Text = "Static Wallpaper";
			this.tpStatic.UseVisualStyleBackColor = true;
			// 
			// pbStatic
			// 
			this.pbStatic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbStatic.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pbStatic.Location = new System.Drawing.Point(6, 6);
			this.pbStatic.Name = "pbStatic";
			this.pbStatic.Size = new System.Drawing.Size(379, 231);
			this.pbStatic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbStatic.TabIndex = 0;
			this.pbStatic.TabStop = false;
			this.pbStatic.Click += new System.EventHandler(this.pbStatic_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 461);
			this.Controls.Add(this.tclWallpapers);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.pbScreensPreview);
			this.Controls.Add(this.listBox1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SDO WP";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.pbScreensPreview)).EndInit();
			this.tclWallpapers.ResumeLayout(false);
			this.tpSDO.ResumeLayout(false);
			this.tpSDO.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbSDO1)).EndInit();
			this.tpStatic.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbStatic)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.PictureBox pbScreensPreview;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TabControl tclWallpapers;
		private System.Windows.Forms.TabPage tpStatic;
		private System.Windows.Forms.PictureBox pbStatic;
		private System.Windows.Forms.TabPage tpSDO;
		private System.Windows.Forms.RadioButton rbSliced;
		private System.Windows.Forms.RadioButton rbConsecutive;
		private System.Windows.Forms.Button btnSDOPreview;
		private ToggleImageButton pbSDO1;
		private ToggleImageButton pbSDO2;
		private ToggleImageButton pbSDO3;
		private ToggleImageButton pbSDO4;
		private ToggleImageButton pbSDO5;
		private ToggleImageButton pbSDO6;
		private ToggleImageButton pbSDO7;
	}
}

