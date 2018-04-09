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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxPresetName = new System.Windows.Forms.TextBox();
            this.ButtonReferenceVmd = new System.Windows.Forms.Button();
            this.ButtonReferencePmx = new System.Windows.Forms.Button();
            this.TextBoxExportDirPmx = new System.Windows.Forms.TextBox();
            this.TextBoxExportDirVmd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.TextBoxRootScript = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ComboBoxPresets = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.PreScriptEditor = new CurtainFireMakerPlugin.Forms.PreScriptEditorControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(8, 65);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(722, 430);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.TextBoxPresetName);
            this.tabPage1.Controls.Add(this.ButtonReferenceVmd);
            this.tabPage1.Controls.Add(this.ButtonReferencePmx);
            this.tabPage1.Controls.Add(this.TextBoxExportDirPmx);
            this.tabPage1.Controls.Add(this.TextBoxExportDirVmd);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(714, 404);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "プリセット";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "プリセット名";
            // 
            // TextBoxPresetName
            // 
            this.TextBoxPresetName.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxPresetName.Location = new System.Drawing.Point(6, 26);
            this.TextBoxPresetName.Name = "TextBoxPresetName";
            this.TextBoxPresetName.Size = new System.Drawing.Size(218, 19);
            this.TextBoxPresetName.TabIndex = 17;
            this.TextBoxPresetName.TextChanged += new System.EventHandler(this.TextChangedPresetName);
            this.TextBoxPresetName.Leave += new System.EventHandler(this.LeaveTextBoxPresetName);
            // 
            // ButtonReferenceVmd
            // 
            this.ButtonReferenceVmd.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ButtonReferenceVmd.Location = new System.Drawing.Point(474, 123);
            this.ButtonReferenceVmd.Name = "ButtonReferenceVmd";
            this.ButtonReferenceVmd.Size = new System.Drawing.Size(48, 21);
            this.ButtonReferenceVmd.TabIndex = 26;
            this.ButtonReferenceVmd.Text = "参照";
            this.ButtonReferenceVmd.UseVisualStyleBackColor = true;
            this.ButtonReferenceVmd.Click += new System.EventHandler(this.ReferenceExportDirVmd);
            // 
            // ButtonReferencePmx
            // 
            this.ButtonReferencePmx.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ButtonReferencePmx.Location = new System.Drawing.Point(474, 76);
            this.ButtonReferencePmx.Name = "ButtonReferencePmx";
            this.ButtonReferencePmx.Size = new System.Drawing.Size(48, 21);
            this.ButtonReferencePmx.TabIndex = 23;
            this.ButtonReferencePmx.Text = "参照";
            this.ButtonReferencePmx.UseVisualStyleBackColor = true;
            this.ButtonReferencePmx.Click += new System.EventHandler(this.ReferenceExportDirPmx);
            // 
            // TextBoxExportDirPmx
            // 
            this.TextBoxExportDirPmx.Location = new System.Drawing.Point(6, 77);
            this.TextBoxExportDirPmx.Name = "TextBoxExportDirPmx";
            this.TextBoxExportDirPmx.Size = new System.Drawing.Size(462, 19);
            this.TextBoxExportDirPmx.TabIndex = 21;
            this.TextBoxExportDirPmx.TextChanged += new System.EventHandler(this.TextChangedExportDirPmx);
            // 
            // TextBoxExportDirVmd
            // 
            this.TextBoxExportDirVmd.Location = new System.Drawing.Point(6, 124);
            this.TextBoxExportDirVmd.Name = "TextBoxExportDirVmd";
            this.TextBoxExportDirVmd.Size = new System.Drawing.Size(462, 19);
            this.TextBoxExportDirVmd.TabIndex = 24;
            this.TextBoxExportDirVmd.TextChanged += new System.EventHandler(this.TextChangedExportDirVmd);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label3.Location = new System.Drawing.Point(4, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "Vmd出力フォルダ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label2.Location = new System.Drawing.Point(4, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 12);
            this.label2.TabIndex = 22;
            this.label2.Text = "Pmx出力フォルダ";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TextBoxRootScript);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(714, 404);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "共通スクリプト";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TextBoxRootScript
            // 
            this.TextBoxRootScript.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TextBoxRootScript.Location = new System.Drawing.Point(8, 8);
            this.TextBoxRootScript.Margin = new System.Windows.Forms.Padding(5, 5, 5, 8);
            this.TextBoxRootScript.Multiline = true;
            this.TextBoxRootScript.Name = "TextBoxRootScript";
            this.TextBoxRootScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxRootScript.Size = new System.Drawing.Size(698, 425);
            this.TextBoxRootScript.TabIndex = 0;
            this.TextBoxRootScript.WordWrap = false;
            this.TextBoxRootScript.TextChanged += new System.EventHandler(this.TextChangedRootScript);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.PreScriptEditor);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(714, 404);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "プレスクリプト";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ComboBoxPresets
            // 
            this.ComboBoxPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxPresets.FormattingEnabled = true;
            this.ComboBoxPresets.Location = new System.Drawing.Point(8, 25);
            this.ComboBoxPresets.Margin = new System.Windows.Forms.Padding(8, 3, 8, 8);
            this.ComboBoxPresets.Name = "ComboBoxPresets";
            this.ComboBoxPresets.Size = new System.Drawing.Size(218, 20);
            this.ComboBoxPresets.TabIndex = 27;
            this.ComboBoxPresets.SelectedIndexChanged += new System.EventHandler(this.SelectedPresetIndexChanged);
            // 
            // button3
            // 
            this.button3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button3.Location = new System.Drawing.Point(317, 24);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 21);
            this.button3.TabIndex = 19;
            this.button3.Text = "削除";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button2.Location = new System.Drawing.Point(232, 24);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 21);
            this.button2.TabIndex = 18;
            this.button2.Text = "新規作成";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.AddPreset);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 12);
            this.label4.TabIndex = 28;
            this.label4.Text = "プリセット";
            // 
            // PreScriptEditor
            // 
            this.PreScriptEditor.Location = new System.Drawing.Point(0, 0);
            this.PreScriptEditor.Name = "PreScriptEditor";
            this.PreScriptEditor.PresetEditor = null;
            this.PreScriptEditor.Size = new System.Drawing.Size(714, 444);
            this.PreScriptEditor.TabIndex = 0;
            // 
            // PresetEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ComboBoxPresets);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Name = "PresetEditorControl";
            this.Size = new System.Drawing.Size(864, 566);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox TextBoxRootScript;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button ButtonReferenceVmd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxExportDirVmd;
        private System.Windows.Forms.Button ButtonReferencePmx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxExportDirPmx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox TextBoxPresetName;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
        private System.Windows.Forms.ComboBox ComboBoxPresets;
        private PreScriptEditorControl PreScriptEditor;
        private System.Windows.Forms.Label label4;
    }
}
