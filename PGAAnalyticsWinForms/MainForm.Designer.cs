namespace PGAAnalyticsWinForms
{
	partial class MainForm
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.outputBox = new System.Windows.Forms.RichTextBox();
			this.baseDirTxtBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.startBtn = new System.Windows.Forms.Button();
			this.dirSelectButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.outputBox);
			this.groupBox1.Location = new System.Drawing.Point(12, 71);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(598, 262);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Output";
			// 
			// outputBox
			// 
			this.outputBox.Location = new System.Drawing.Point(15, 28);
			this.outputBox.Name = "outputBox";
			this.outputBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.outputBox.Size = new System.Drawing.Size(569, 216);
			this.outputBox.TabIndex = 0;
			this.outputBox.Text = "";
			// 
			// baseDirTxtBox
			// 
			this.baseDirTxtBox.Location = new System.Drawing.Point(119, 21);
			this.baseDirTxtBox.Name = "baseDirTxtBox";
			this.baseDirTxtBox.Size = new System.Drawing.Size(452, 22);
			this.baseDirTxtBox.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(101, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Base Directory";
			// 
			// startBtn
			// 
			this.startBtn.Location = new System.Drawing.Point(496, 339);
			this.startBtn.Name = "startBtn";
			this.startBtn.Size = new System.Drawing.Size(114, 37);
			this.startBtn.TabIndex = 3;
			this.startBtn.Text = "Start";
			this.startBtn.UseVisualStyleBackColor = true;
			// 
			// dirSelectButton
			// 
			this.dirSelectButton.Location = new System.Drawing.Point(577, 21);
			this.dirSelectButton.Name = "dirSelectButton";
			this.dirSelectButton.Size = new System.Drawing.Size(33, 23);
			this.dirSelectButton.TabIndex = 4;
			this.dirSelectButton.Text = "...";
			this.dirSelectButton.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(626, 394);
			this.Controls.Add(this.dirSelectButton);
			this.Controls.Add(this.startBtn);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.baseDirTxtBox);
			this.Controls.Add(this.groupBox1);
			this.Name = "MainForm";
			this.Text = "PGA Data Analyzer";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RichTextBox outputBox;
		private System.Windows.Forms.TextBox baseDirTxtBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button startBtn;
		private System.Windows.Forms.Button dirSelectButton;
	}
}

