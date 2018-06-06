namespace CurtainFireMakerPlugin.Forms
{
    partial class PresetEditorControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PresetSettingControl = new CurtainFireMakerPlugin.Forms.PresetSettingControl();
            this.PresetSequenceEditorControl = new CurtainFireMakerPlugin.Forms.PresetSequenceEditorControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PresetSettingControl);
            this.groupBox1.Location = new System.Drawing.Point(3, 331);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(636, 85);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "出力設定";
            // 
            // PresetSettingControl
            // 
            this.PresetSettingControl.EndFrame = 0;
            this.PresetSettingControl.Location = new System.Drawing.Point(6, 18);
            this.PresetSettingControl.Name = "PresetSettingControl";
            this.PresetSettingControl.Size = new System.Drawing.Size(160, 62);
            this.PresetSettingControl.StartFrame = 0;
            this.PresetSettingControl.TabIndex = 13;
            // 
            // PresetSequenceEditorControl
            // 
            this.PresetSequenceEditorControl.Location = new System.Drawing.Point(0, 0);
            this.PresetSequenceEditorControl.Margin = new System.Windows.Forms.Padding(0);
            this.PresetSequenceEditorControl.Name = "PresetSequenceEditorControl";
            this.PresetSequenceEditorControl.RecentDirectories = null;
            this.PresetSequenceEditorControl.Size = new System.Drawing.Size(642, 328);
            this.PresetSequenceEditorControl.TabIndex = 0;
            // 
            // PresetEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.PresetSequenceEditorControl);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PresetEditorControl";
            this.Size = new System.Drawing.Size(642, 420);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private PresetSequenceEditorControl PresetSequenceEditorControl;
        private PresetSettingControl PresetSettingControl;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
