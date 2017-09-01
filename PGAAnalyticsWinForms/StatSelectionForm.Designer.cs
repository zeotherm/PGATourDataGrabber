namespace PGAAnalyticsWinForms
{
	partial class StatSelectionForm
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
			this.selectedList = new System.Windows.Forms.ListBox();
			this.btnCalculate = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.unselectedList = new System.Windows.Forms.ListBox();
			this.selectButton = new System.Windows.Forms.Button();
			this.deselectButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.selectedList);
			this.groupBox1.Location = new System.Drawing.Point(383, 15);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox1.Size = new System.Drawing.Size(303, 280);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Selected Stats";
			// 
			// selectedList
			// 
			this.selectedList.FormattingEnabled = true;
			this.selectedList.ItemHeight = 16;
			this.selectedList.Location = new System.Drawing.Point(10, 26);
			this.selectedList.Name = "selectedList";
			this.selectedList.Size = new System.Drawing.Size(277, 228);
			this.selectedList.TabIndex = 0;
			this.selectedList.SelectedIndexChanged += new System.EventHandler(this.selectedList_SelectedIndexChanged);
			this.selectedList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.selectedList_MouseDoubleClick);
			// 
			// btnCalculate
			// 
			this.btnCalculate.Location = new System.Drawing.Point(586, 363);
			this.btnCalculate.Margin = new System.Windows.Forms.Padding(4);
			this.btnCalculate.Name = "btnCalculate";
			this.btnCalculate.Size = new System.Drawing.Size(100, 28);
			this.btnCalculate.TabIndex = 2;
			this.btnCalculate.Text = "Calculate";
			this.btnCalculate.UseVisualStyleBackColor = true;
			this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.unselectedList);
			this.groupBox2.Location = new System.Drawing.Point(16, 13);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(303, 282);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Unselected Stats";
			// 
			// unselectedList
			// 
			this.unselectedList.FormattingEnabled = true;
			this.unselectedList.ItemHeight = 16;
			this.unselectedList.Location = new System.Drawing.Point(14, 27);
			this.unselectedList.Name = "unselectedList";
			this.unselectedList.Size = new System.Drawing.Size(269, 228);
			this.unselectedList.TabIndex = 0;
			this.unselectedList.SelectedIndexChanged += new System.EventHandler(this.unselectedList_SelectedIndexChanged);
			// 
			// selectButton
			// 
			this.selectButton.Location = new System.Drawing.Point(325, 105);
			this.selectButton.Name = "selectButton";
			this.selectButton.Size = new System.Drawing.Size(51, 34);
			this.selectButton.TabIndex = 4;
			this.selectButton.Text = ">>>";
			this.selectButton.UseVisualStyleBackColor = true;
			this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
			// 
			// deselectButton
			// 
			this.deselectButton.Location = new System.Drawing.Point(325, 175);
			this.deselectButton.Name = "deselectButton";
			this.deselectButton.Size = new System.Drawing.Size(51, 30);
			this.deselectButton.TabIndex = 5;
			this.deselectButton.Text = "<<<";
			this.deselectButton.UseVisualStyleBackColor = true;
			this.deselectButton.Click += new System.EventHandler(this.deselectButton_Click);
			// 
			// StatSelectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(712, 404);
			this.Controls.Add(this.deselectButton);
			this.Controls.Add(this.selectButton);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnCalculate);
			this.Controls.Add(this.groupBox1);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "StatSelectionForm";
			this.Text = "StatSelectionForm";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnCalculate;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListBox selectedList;
		private System.Windows.Forms.ListBox unselectedList;
		private System.Windows.Forms.Button selectButton;
		private System.Windows.Forms.Button deselectButton;
	}
}