
namespace JsonConverterWithCSV
{
	partial class Form1
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.BtnLoad = new System.Windows.Forms.Button();
			this.Export = new System.Windows.Forms.Button();
			this.listView = new System.Windows.Forms.ListView();
			this.OpenCsvFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.RemoveBtn = new System.Windows.Forms.Button();
			this.ExportProgressBar = new System.Windows.Forms.ProgressBar();
			this.ProgressLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// BtnLoad
			// 
			this.BtnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnLoad.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.BtnLoad.Location = new System.Drawing.Point(487, 12);
			this.BtnLoad.Name = "BtnLoad";
			this.BtnLoad.Size = new System.Drawing.Size(114, 40);
			this.BtnLoad.TabIndex = 0;
			this.BtnLoad.Text = "Excel 불러오기";
			this.BtnLoad.UseVisualStyleBackColor = true;
			this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
			// 
			// Export
			// 
			this.Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Export.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.Export.Location = new System.Drawing.Point(487, 58);
			this.Export.Name = "Export";
			this.Export.Size = new System.Drawing.Size(114, 40);
			this.Export.TabIndex = 1;
			this.Export.Text = "JSON 내보내기";
			this.Export.UseVisualStyleBackColor = true;
			this.Export.Click += new System.EventHandler(this.Export_Click);
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(12, 12);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(469, 279);
			this.listView.TabIndex = 3;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.List;
			// 
			// OpenCsvFileDialog
			// 
			this.OpenCsvFileDialog.Filter = "Csv files (*.xlsx)|*.xlsx";
			this.OpenCsvFileDialog.Multiselect = true;
			this.OpenCsvFileDialog.RestoreDirectory = true;
			this.OpenCsvFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenCsvFileDialog_FileOk);
			// 
			// RemoveBtn
			// 
			this.RemoveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RemoveBtn.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.RemoveBtn.Location = new System.Drawing.Point(487, 167);
			this.RemoveBtn.Name = "RemoveBtn";
			this.RemoveBtn.Size = new System.Drawing.Size(114, 40);
			this.RemoveBtn.TabIndex = 4;
			this.RemoveBtn.Text = "선택항목 삭제하기";
			this.RemoveBtn.UseVisualStyleBackColor = true;
			this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
			// 
			// ExportProgressBar
			// 
			this.ExportProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ExportProgressBar.Location = new System.Drawing.Point(12, 316);
			this.ExportProgressBar.Name = "ExportProgressBar";
			this.ExportProgressBar.Size = new System.Drawing.Size(589, 23);
			this.ExportProgressBar.TabIndex = 5;
			// 
			// ProgressLabel
			// 
			this.ProgressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ProgressLabel.BackColor = System.Drawing.SystemColors.Control;
			this.ProgressLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ProgressLabel.ForeColor = System.Drawing.Color.Black;
			this.ProgressLabel.Location = new System.Drawing.Point(12, 294);
			this.ProgressLabel.Name = "ProgressLabel";
			this.ProgressLabel.Size = new System.Drawing.Size(589, 19);
			this.ProgressLabel.TabIndex = 6;
			this.ProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(613, 345);
			this.Controls.Add(this.ProgressLabel);
			this.Controls.Add(this.ExportProgressBar);
			this.Controls.Add(this.RemoveBtn);
			this.Controls.Add(this.listView);
			this.Controls.Add(this.Export);
			this.Controls.Add(this.BtnLoad);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Form1";
			this.Text = "ExcelToJson";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnLoad;
		private System.Windows.Forms.Button Export;
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.OpenFileDialog OpenCsvFileDialog;
		private System.Windows.Forms.Button RemoveBtn;
		private System.Windows.Forms.ProgressBar ExportProgressBar;
		private System.Windows.Forms.Label ProgressLabel;
	}
}

