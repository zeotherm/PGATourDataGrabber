namespace PGAAnalyticsWinForms {
	partial class AvailablePlayersDisplayForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.PlayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Salary = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Points = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Multi = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Tournaments = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CutsMade = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Top10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Earnings = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CupPoints = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.button1 = new System.Windows.Forms.Button();
			this.keeperBtn = new System.Windows.Forms.Button();
			this.exclude_btn = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PlayerName,
            this.Salary,
            this.Points,
            this.Multi,
            this.Tournaments,
            this.CutsMade,
            this.Top10,
            this.Earnings,
            this.CupPoints});
			this.dataGridView1.Location = new System.Drawing.Point(12, 12);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowTemplate.Height = 24;
			this.dataGridView1.Size = new System.Drawing.Size(997, 369);
			this.dataGridView1.TabIndex = 0;
			// 
			// PlayerName
			// 
			this.PlayerName.HeaderText = "Player Name";
			this.PlayerName.Name = "PlayerName";
			this.PlayerName.ReadOnly = true;
			// 
			// Salary
			// 
			this.Salary.HeaderText = "Salary";
			this.Salary.Name = "Salary";
			this.Salary.ReadOnly = true;
			// 
			// Points
			// 
			this.Points.HeaderText = "Points";
			this.Points.Name = "Points";
			this.Points.ReadOnly = true;
			// 
			// Multi
			// 
			this.Multi.HeaderText = "# of Lists";
			this.Multi.Name = "Multi";
			this.Multi.ReadOnly = true;
			// 
			// Tournaments
			// 
			this.Tournaments.HeaderText = "Tournaments Played";
			this.Tournaments.Name = "Tournaments";
			this.Tournaments.ReadOnly = true;
			// 
			// CutsMade
			// 
			this.CutsMade.HeaderText = "Cuts Made";
			this.CutsMade.Name = "CutsMade";
			this.CutsMade.ReadOnly = true;
			// 
			// Top10
			// 
			this.Top10.HeaderText = "Top 10 Finishes";
			this.Top10.Name = "Top10";
			this.Top10.ReadOnly = true;
			// 
			// Earnings
			// 
			this.Earnings.HeaderText = "Earnings";
			this.Earnings.Name = "Earnings";
			this.Earnings.ReadOnly = true;
			// 
			// CupPoints
			// 
			this.CupPoints.HeaderText = "FedEx Cup Points";
			this.CupPoints.Name = "CupPoints";
			this.CupPoints.ReadOnly = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(893, 396);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(116, 34);
			this.button1.TabIndex = 1;
			this.button1.Text = "Optimize Team";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// keeperBtn
			// 
			this.keeperBtn.Location = new System.Drawing.Point(760, 396);
			this.keeperBtn.Name = "keeperBtn";
			this.keeperBtn.Size = new System.Drawing.Size(118, 34);
			this.keeperBtn.TabIndex = 2;
			this.keeperBtn.Text = "Keepers";
			this.keeperBtn.UseVisualStyleBackColor = true;
			this.keeperBtn.Click += new System.EventHandler(this.keeperBtn_Click);
			// 
			// exclude_btn
			// 
			this.exclude_btn.Location = new System.Drawing.Point(639, 396);
			this.exclude_btn.Name = "exclude_btn";
			this.exclude_btn.Size = new System.Drawing.Size(104, 34);
			this.exclude_btn.TabIndex = 3;
			this.exclude_btn.Text = "Exclude";
			this.exclude_btn.UseVisualStyleBackColor = true;
			this.exclude_btn.Click += new System.EventHandler(this.exclude_btn_Click);
			// 
			// AvailablePlayersDisplayForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1021, 442);
			this.Controls.Add(this.exclude_btn);
			this.Controls.Add(this.keeperBtn);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.dataGridView1);
			this.Name = "AvailablePlayersDisplayForm";
			this.Text = "AvailablePlayersDisplayForm";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button keeperBtn;
		private System.Windows.Forms.DataGridViewTextBoxColumn PlayerName;
		private System.Windows.Forms.DataGridViewTextBoxColumn Salary;
		private System.Windows.Forms.DataGridViewTextBoxColumn Points;
		private System.Windows.Forms.DataGridViewTextBoxColumn Multi;
		private System.Windows.Forms.DataGridViewTextBoxColumn Tournaments;
		private System.Windows.Forms.DataGridViewTextBoxColumn CutsMade;
		private System.Windows.Forms.DataGridViewTextBoxColumn Top10;
		private System.Windows.Forms.DataGridViewTextBoxColumn Earnings;
		private System.Windows.Forms.DataGridViewTextBoxColumn CupPoints;
		private System.Windows.Forms.Button exclude_btn;
	}
}