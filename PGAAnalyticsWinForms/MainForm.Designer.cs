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
			this.nextButton = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.stopButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.progressLabel = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.outputBox);
			this.groupBox1.Location = new System.Drawing.Point(9, 58);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(448, 213);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Output";
			// 
			// outputBox
			// 
			this.outputBox.HideSelection = false;
			this.outputBox.Location = new System.Drawing.Point(11, 23);
			this.outputBox.Margin = new System.Windows.Forms.Padding(2);
			this.outputBox.Name = "outputBox";
			this.outputBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.outputBox.Size = new System.Drawing.Size(428, 176);
			this.outputBox.TabIndex = 0;
			this.outputBox.Text = "";
			// 
			// baseDirTxtBox
			// 
			this.baseDirTxtBox.Location = new System.Drawing.Point(89, 17);
			this.baseDirTxtBox.Margin = new System.Windows.Forms.Padding(2);
			this.baseDirTxtBox.Name = "baseDirTxtBox";
			this.baseDirTxtBox.Size = new System.Drawing.Size(340, 20);
			this.baseDirTxtBox.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 20);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Base Directory";
			// 
			// startBtn
			// 
			this.startBtn.Location = new System.Drawing.Point(274, 279);
			this.startBtn.Margin = new System.Windows.Forms.Padding(2);
			this.startBtn.Name = "startBtn";
			this.startBtn.Size = new System.Drawing.Size(86, 30);
			this.startBtn.TabIndex = 3;
			this.startBtn.Text = "Start";
			this.startBtn.UseVisualStyleBackColor = true;
			this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
			// 
			// dirSelectButton
			// 
			this.dirSelectButton.Location = new System.Drawing.Point(433, 17);
			this.dirSelectButton.Margin = new System.Windows.Forms.Padding(2);
			this.dirSelectButton.Name = "dirSelectButton";
			this.dirSelectButton.Size = new System.Drawing.Size(25, 19);
			this.dirSelectButton.TabIndex = 4;
			this.dirSelectButton.Text = "...";
			this.dirSelectButton.UseVisualStyleBackColor = true;
			// 
			// nextButton
			// 
			this.nextButton.Enabled = false;
			this.nextButton.Location = new System.Drawing.Point(366, 314);
			this.nextButton.Name = "nextButton";
			this.nextButton.Size = new System.Drawing.Size(85, 30);
			this.nextButton.TabIndex = 5;
			this.nextButton.Text = "Next >>";
			this.nextButton.UseVisualStyleBackColor = true;
			this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 314);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(348, 30);
			this.progressBar1.TabIndex = 6;
			// 
			// stopButton
			// 
			this.stopButton.Enabled = false;
			this.stopButton.Location = new System.Drawing.Point(365, 278);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(86, 30);
			this.stopButton.TabIndex = 7;
			this.stopButton.Text = "Stop";
			this.stopButton.UseVisualStyleBackColor = true;
			this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(53, 347);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(97, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "Players Processed:";
			// 
			// progressLabel
			// 
			this.progressLabel.AutoSize = true;
			this.progressLabel.Location = new System.Drawing.Point(156, 347);
			this.progressLabel.Name = "progressLabel";
			this.progressLabel.Size = new System.Drawing.Size(0, 13);
			this.progressLabel.TabIndex = 9;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(470, 397);
			this.Controls.Add(this.progressLabel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.stopButton);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.nextButton);
			this.Controls.Add(this.dirSelectButton);
			this.Controls.Add(this.startBtn);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.baseDirTxtBox);
			this.Controls.Add(this.groupBox1);
			this.Margin = new System.Windows.Forms.Padding(2);
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
		private System.Windows.Forms.Button nextButton;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label progressLabel;
	}
}

