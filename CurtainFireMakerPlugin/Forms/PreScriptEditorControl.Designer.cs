namespace CurtainFireMakerPlugin.Forms
{
    partial class PreScriptEditorControl
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
            this.button1 = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.ComboBoxScript = new System.Windows.Forms.ComboBox();
            this.TextBoxPreScript = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button1.Location = new System.Drawing.Point(543, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 9;
            this.button1.Text = "弾幕生成";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ClickGenerate);
            // 
            // buttonRemove
            // 
            this.buttonRemove.ForeColor = System.Drawing.SystemColors.WindowText;
            this.buttonRemove.Location = new System.Drawing.Point(624, 10);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 20);
            this.buttonRemove.TabIndex = 8;
            this.buttonRemove.Text = "削除";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.ClickRemoveScript);
            // 
            // ComboBoxScript
            // 
            this.ComboBoxScript.DisplayMember = "FineName";
            this.ComboBoxScript.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxScript.FormattingEnabled = true;
            this.ComboBoxScript.Location = new System.Drawing.Point(7, 10);
            this.ComboBoxScript.Name = "ComboBoxScript";
            this.ComboBoxScript.Size = new System.Drawing.Size(530, 20);
            this.ComboBoxScript.TabIndex = 7;
            this.ComboBoxScript.SelectedIndexChanged += new System.EventHandler(this.SelectedScriptIndexChanged);
            // 
            // TextBoxPreScript
            // 
            this.TextBoxPreScript.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TextBoxPreScript.Location = new System.Drawing.Point(9, 38);
            this.TextBoxPreScript.Margin = new System.Windows.Forms.Padding(5, 5, 5, 8);
            this.TextBoxPreScript.Multiline = true;
            this.TextBoxPreScript.Name = "TextBoxPreScript";
            this.TextBoxPreScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxPreScript.Size = new System.Drawing.Size(698, 397);
            this.TextBoxPreScript.TabIndex = 6;
            this.TextBoxPreScript.WordWrap = false;
            this.TextBoxPreScript.Leave += new System.EventHandler(this.LeaveTextBoxPreScript);
            // 
            // PreScriptEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.ComboBoxScript);
            this.Controls.Add(this.TextBoxPreScript);
            this.Name = "PreScriptEditorControl";
            this.Size = new System.Drawing.Size(714, 444);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.ComboBox ComboBoxScript;
        private System.Windows.Forms.TextBox TextBoxPreScript;
    }
}
