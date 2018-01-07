namespace CurtainFireMakerPlugin.Forms
{
    partial class IronPythonControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxScript = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "共通スクリプト";
            // 
            // textBoxScript
            // 
            this.textBoxScript.AcceptsTab = true;
            this.textBoxScript.DetectUrls = false;
            this.textBoxScript.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxScript.Location = new System.Drawing.Point(5, 26);
            this.textBoxScript.Margin = new System.Windows.Forms.Padding(3, 3, 80, 60);
            this.textBoxScript.Name = "textBoxScript";
            this.textBoxScript.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.textBoxScript.Size = new System.Drawing.Size(495, 274);
            this.textBoxScript.TabIndex = 3;
            this.textBoxScript.TabStop = false;
            this.textBoxScript.Text = "";
            this.textBoxScript.WordWrap = false;
            // 
            // IronPythonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxScript);
            this.Controls.Add(this.label1);
            this.Name = "IronPythonControl";
            this.Size = new System.Drawing.Size(580, 360);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox textBoxScript;
    }
}
