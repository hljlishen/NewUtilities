namespace Utilities.Forms
{
    partial class SlashForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.info_lab = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // info_lab
            // 
            this.info_lab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.info_lab.AutoSize = true;
            this.info_lab.BackColor = System.Drawing.Color.Transparent;
            this.info_lab.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.info_lab.ForeColor = System.Drawing.Color.White;
            this.info_lab.Location = new System.Drawing.Point(18, 476);
            this.info_lab.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.info_lab.Name = "info_lab";
            this.info_lab.Size = new System.Drawing.Size(216, 31);
            this.info_lab.TabIndex = 0;
            this.info_lab.Text = "正在检测UKey......";
            // 
            // SlashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1082, 522);
            this.Controls.Add(this.info_lab);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SlashForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.SlashForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label info_lab;
    }
}

