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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxRootScript = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxScript
            // 
            this.textBoxScript.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxScript.Location = new System.Drawing.Point(6, 36);
            this.textBoxScript.Margin = new System.Windows.Forms.Padding(5, 5, 5, 8);
            this.textBoxScript.Multiline = true;
            this.textBoxScript.Name = "textBoxScript";
            this.textBoxScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxScript.Size = new System.Drawing.Size(700, 397);
            this.textBoxScript.TabIndex = 2;
            this.textBoxScript.WordWrap = false;
            this.textBoxScript.TextChanged += new System.EventHandler(this.TextChanged_Script);
            // 
            // comboBoxScriptKey
            // 
            this.comboBoxScriptKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxScriptKey.FormattingEnabled = true;
            this.comboBoxScriptKey.Location = new System.Drawing.Point(6, 8);
            this.comboBoxScriptKey.Name = "comboBoxScriptKey";
            this.comboBoxScriptKey.Size = new System.Drawing.Size(449, 20);
            this.comboBoxScriptKey.TabIndex = 3;
            this.comboBoxScriptKey.TextChanged += new System.EventHandler(this.TextChanges_ScriptKey);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(722, 470);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxRootScript);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(714, 444);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "共通スクリプト";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxRootScript
            // 
            this.textBoxRootScript.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxRootScript.Location = new System.Drawing.Point(8, 8);
            this.textBoxRootScript.Margin = new System.Windows.Forms.Padding(5, 5, 5, 8);
            this.textBoxRootScript.Multiline = true;
            this.textBoxRootScript.Name = "textBoxRootScript";
            this.textBoxRootScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxRootScript.Size = new System.Drawing.Size(698, 425);
            this.textBoxRootScript.TabIndex = 0;
            this.textBoxRootScript.WordWrap = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonRemove);
            this.tabPage2.Controls.Add(this.comboBoxScriptKey);
            this.tabPage2.Controls.Add(this.textBoxScript);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(714, 444);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "プレスクリプト";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(461, 8);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 20);
            this.buttonRemove.TabIndex = 4;
            this.buttonRemove.Text = "削除";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.Click_Remove);
            // 
            // ProjectScriptControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ProjectScriptControl";
            this.Size = new System.Drawing.Size(864, 540);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxScript;
        private System.Windows.Forms.ComboBox comboBoxScriptKey;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxRootScript;
        private System.Windows.Forms.Button buttonRemove;
    }
}
