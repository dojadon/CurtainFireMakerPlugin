namespace CurtainFireMakerPlugin.Forms
{
    partial class ProjectScriptControl
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
            this.textBoxScript = new System.Windows.Forms.TextBox();
            this.comboBoxScriptKey = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // textBoxScript
            // 
            this.textBoxScript.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxScript.Location = new System.Drawing.Point(5, 42);
            this.textBoxScript.Multiline = true;
            this.textBoxScript.Name = "textBoxScript";
            this.textBoxScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxScript.Size = new System.Drawing.Size(700, 400);
            this.textBoxScript.TabIndex = 2;
            this.textBoxScript.WordWrap = false;
            this.textBoxScript.TextChanged += new System.EventHandler(this.TextChanged_Script);
            // 
            // comboBoxScriptKey
            // 
            this.comboBoxScriptKey.FormattingEnabled = true;
            this.comboBoxScriptKey.Location = new System.Drawing.Point(5, 16);
            this.comboBoxScriptKey.Name = "comboBoxScriptKey";
            this.comboBoxScriptKey.Size = new System.Drawing.Size(530, 20);
            this.comboBoxScriptKey.TabIndex = 3;
            this.comboBoxScriptKey.TextChanged += new System.EventHandler(this.TextChanges_ScriptKey);
            // 
            // ProjectScriptControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxScriptKey);
            this.Controls.Add(this.textBoxScript);
            this.Name = "ProjectScriptControl";
            this.Size = new System.Drawing.Size(960, 540);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxScript;
        private System.Windows.Forms.ComboBox comboBoxScriptKey;
    }
}
