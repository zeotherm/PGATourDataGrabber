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
			this.statsCheckBox = new System.Windows.Forms.CheckedListBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.btnCalculate = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statsCheckBox
			// 
			this.statsCheckBox.FormattingEnabled = true;
			this.statsCheckBox.Location = new System.Drawing.Point(12, 12);
			this.statsCheckBox.Name = "statsCheckBox";
			this.statsCheckBox.Size = new System.Drawing.Size(254, 229);
			this.statsCheckBox.TabIndex = 0;
			this.statsCheckBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.statsCheckBox_ItemCheck);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.listView1);
			this.groupBox1.Location = new System.Drawing.Point(287, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(227, 200);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Selected Stats";
			// 
			// listView1
			// 
			this.listView1.Location = new System.Drawing.Point(14, 25);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(202, 160);
			this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.List;
			// 
			// btnCalculate
			// 
			this.btnCalculate.Location = new System.Drawing.Point(439, 218);
			this.btnCalculate.Name = "btnCalculate";
			this.btnCalculate.Size = new System.Drawing.Size(75, 23);
			this.btnCalculate.TabIndex = 2;
			this.btnCalculate.Text = "Calculate";
			this.btnCalculate.UseVisualStyleBackColor = true;
			// 
			// StatSelectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(534, 261);
			this.Controls.Add(this.btnCalculate);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.statsCheckBox);
			this.Name = "StatSelectionForm";
			this.Text = "StatSelectionForm";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox statsCheckBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Button btnCalculate;
	}
}