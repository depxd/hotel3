namespace hotel3
{
    partial class HistoryForm
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
            this.dataGridViewHistory = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewHistory
            // 
            this.dataGridViewHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHistory.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewHistory.Name = "dataGridViewHistory";
            this.dataGridViewHistory.RowHeadersWidth = 51;
            this.dataGridViewHistory.RowTemplate.Height = 24;
            this.dataGridViewHistory.Size = new System.Drawing.Size(776, 351);
            this.dataGridViewHistory.TabIndex = 0;
            // 
            // HistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewHistory);
            this.Name = "HistoryForm";
            this.Text = "HistoryForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewHistory;
    }
}