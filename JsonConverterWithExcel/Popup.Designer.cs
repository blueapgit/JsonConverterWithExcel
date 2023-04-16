
namespace JsonConverterWithCSV
{
	partial class Popup
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
			this.close = new System.Windows.Forms.Button();
			this.label = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// close
			// 
			this.close.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.close.Location = new System.Drawing.Point(88, 84);
			this.close.Name = "close";
			this.close.Size = new System.Drawing.Size(100, 30);
			this.close.TabIndex = 0;
			this.close.Text = "닫기";
			this.close.UseVisualStyleBackColor = true;
			this.close.Click += new System.EventHandler(this.close_Click);
			// 
			// label
			// 
			this.label.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label.Location = new System.Drawing.Point(12, 9);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(260, 72);
			this.label.TabIndex = 1;
			this.label.Text = "label";
			this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Popup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 120);
			this.Controls.Add(this.label);
			this.Controls.Add(this.close);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Popup";
			this.Text = "알림";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button close;
		private System.Windows.Forms.Label label;
	}
}