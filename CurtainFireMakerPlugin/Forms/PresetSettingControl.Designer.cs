namespace CurtainFireMakerPlugin.Forms
{
    partial class PresetSettingControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.saveFileDialogExportPath = new System.Windows.Forms.SaveFileDialog();
            this.numericUpDownStartFrame = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownEndFrame = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // saveFileDialogExportPath
            // 
            this.saveFileDialogExportPath.Filter = "Pmx File|*.pmx";
            // 
            // numericUpDownStartFrame
            // 
            this.numericUpDownStartFrame.Location = new System.Drawing.Point(75, 9);
            this.numericUpDownStartFrame.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownStartFrame.Minimum = new decimal(new int[] {
            65535,
            0,
            0,
            -2147483648});
            this.numericUpDownStartFrame.Name = "numericUpDownStartFrame";
            this.numericUpDownStartFrame.Size = new System.Drawing.Size(80, 19);
            this.numericUpDownStartFrame.TabIndex = 9;
            this.numericUpDownStartFrame.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "開始フレーム";
            // 
            // numericUpDownEndFrame
            // 
            this.numericUpDownEndFrame.Location = new System.Drawing.Point(75, 37);
            this.numericUpDownEndFrame.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownEndFrame.Name = "numericUpDownEndFrame";
            this.numericUpDownEndFrame.Size = new System.Drawing.Size(80, 19);
            this.numericUpDownEndFrame.TabIndex = 11;
            this.numericUpDownEndFrame.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label3.Location = new System.Drawing.Point(3, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "終了フレーム";
            // 
            // PresetSettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownEndFrame);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownStartFrame);
            this.Name = "PresetSettingControl";
            this.Size = new System.Drawing.Size(160, 62);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndFrame)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog saveFileDialogExportPath;
        private System.Windows.Forms.NumericUpDown numericUpDownStartFrame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownEndFrame;
        private System.Windows.Forms.Label label3;
    }
}
