namespace CurtainFireMakerPlugin.Forms
{
    partial class ImportSettingControl
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
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.shotModelText = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.spellScriptText = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.modelDirBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.spellScriptFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.shottypeScriptFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.referenceScriptFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(434, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(49, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "参照";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Click_ShotModel);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Shot Model";
            // 
            // shotModelText
            // 
            this.shotModelText.Location = new System.Drawing.Point(3, 21);
            this.shotModelText.Name = "shotModelText";
            this.shotModelText.ReadOnly = true;
            this.shotModelText.Size = new System.Drawing.Size(425, 19);
            this.shotModelText.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(434, 62);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(49, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "参照";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Click_SpellScript);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 49);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Spell Script";
            // 
            // spellScriptText
            // 
            this.spellScriptText.Location = new System.Drawing.Point(3, 64);
            this.spellScriptText.Name = "spellScriptText";
            this.spellScriptText.ReadOnly = true;
            this.spellScriptText.Size = new System.Drawing.Size(425, 19);
            this.spellScriptText.TabIndex = 6;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(96, 89);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 1;
            this.button6.Text = "OK";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Click_OK);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(341, 89);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 2;
            this.button7.Text = "キャンセル";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Click_Cancel);
            // 
            // spellScriptFileDialog
            // 
            this.spellScriptFileDialog.FileName = "openFileDialog1";
            // 
            // shottypeScriptFileDialog
            // 
            this.shottypeScriptFileDialog.FileName = "openFileDialog1";
            // 
            // referenceScriptFileDialog
            // 
            this.referenceScriptFileDialog.FileName = "openFileDialog1";
            // 
            // ImportSettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.shotModelText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.spellScriptText);
            this.Name = "ImportSettingControl";
            this.Size = new System.Drawing.Size(500, 121);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox spellScriptText;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox shotModelText;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.FolderBrowserDialog modelDirBrowserDialog;
        private System.Windows.Forms.OpenFileDialog spellScriptFileDialog;
        private System.Windows.Forms.OpenFileDialog shottypeScriptFileDialog;
        private System.Windows.Forms.OpenFileDialog referenceScriptFileDialog;
    }
}
