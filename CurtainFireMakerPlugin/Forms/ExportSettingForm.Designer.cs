namespace CurtainFireMakerPlugin.Forms
{
    partial class ExportSettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportSettingForm));
            this.label5 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.TextBoxScriptPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxPmxExportPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.OpenFileDialogScript = new System.Windows.Forms.OpenFileDialog();
            this.CheckBoxKeepLog = new System.Windows.Forms.CheckBox();
            this.FolderBrowserDialogPmx = new System.Windows.Forms.FolderBrowserDialog();
            this.CheckBoxDropPmxFile = new System.Windows.Forms.CheckBox();
            this.CheckBoxDropVmdFile = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxVmdExportPath = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.FolderBrowserDialogVmd = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 15);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "スクリプト";
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(469, 28);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(44, 23);
            this.button5.TabIndex = 20;
            this.button5.Text = "参照";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Click_Script);
            // 
            // TextBoxScriptPath
            // 
            this.TextBoxScriptPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxScriptPath.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxScriptPath.Location = new System.Drawing.Point(14, 30);
            this.TextBoxScriptPath.Name = "TextBoxScriptPath";
            this.TextBoxScriptPath.Size = new System.Drawing.Size(448, 19);
            this.TextBoxScriptPath.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 58);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pmx出力先";
            // 
            // TextBoxPmxExportPath
            // 
            this.TextBoxPmxExportPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxPmxExportPath.Location = new System.Drawing.Point(14, 73);
            this.TextBoxPmxExportPath.Name = "TextBoxPmxExportPath";
            this.TextBoxPmxExportPath.Size = new System.Drawing.Size(448, 19);
            this.TextBoxPmxExportPath.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(468, 71);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "参照";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Click_PmxExportDir);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(369, 241);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 17;
            this.button4.Text = "キャンセル";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Click_Cancel);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(95, 241);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 16;
            this.button3.Text = "出力";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Click_OK);
            // 
            // OpenFileDialogScript
            // 
            this.OpenFileDialogScript.FileName = "openFileDialog1";
            this.OpenFileDialogScript.Filter = "|*.py";
            // 
            // CheckBoxKeepLog
            // 
            this.CheckBoxKeepLog.AutoSize = true;
            this.CheckBoxKeepLog.Location = new System.Drawing.Point(14, 161);
            this.CheckBoxKeepLog.Name = "CheckBoxKeepLog";
            this.CheckBoxKeepLog.Size = new System.Drawing.Size(128, 16);
            this.CheckBoxKeepLog.TabIndex = 22;
            this.CheckBoxKeepLog.Text = "ログを開いたままにする";
            this.CheckBoxKeepLog.UseVisualStyleBackColor = true;
            // 
            // CheckBoxDropPmxFile
            // 
            this.CheckBoxDropPmxFile.AutoSize = true;
            this.CheckBoxDropPmxFile.Location = new System.Drawing.Point(14, 183);
            this.CheckBoxDropPmxFile.Name = "CheckBoxDropPmxFile";
            this.CheckBoxDropPmxFile.Size = new System.Drawing.Size(223, 16);
            this.CheckBoxDropPmxFile.TabIndex = 24;
            this.CheckBoxDropPmxFile.Text = "出力時にpmxファイルをMMMへドロップする";
            this.CheckBoxDropPmxFile.UseVisualStyleBackColor = true;
            // 
            // CheckBoxDropVmdFile
            // 
            this.CheckBoxDropVmdFile.AutoSize = true;
            this.CheckBoxDropVmdFile.Location = new System.Drawing.Point(14, 205);
            this.CheckBoxDropVmdFile.Name = "CheckBoxDropVmdFile";
            this.CheckBoxDropVmdFile.Size = new System.Drawing.Size(223, 16);
            this.CheckBoxDropVmdFile.TabIndex = 25;
            this.CheckBoxDropVmdFile.Text = "出力時にvmdファイルをMMMへドロップする";
            this.CheckBoxDropVmdFile.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 100);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "Vmd出力先";
            // 
            // TextBoxVmdExportPath
            // 
            this.TextBoxVmdExportPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxVmdExportPath.Location = new System.Drawing.Point(14, 115);
            this.TextBoxVmdExportPath.Name = "TextBoxVmdExportPath";
            this.TextBoxVmdExportPath.Size = new System.Drawing.Size(448, 19);
            this.TextBoxVmdExportPath.TabIndex = 27;
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.Location = new System.Drawing.Point(468, 113);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(45, 23);
            this.button6.TabIndex = 28;
            this.button6.Text = "参照";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Click_VmdExportDir);
            // 
            // ExportSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 281);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextBoxVmdExportPath);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CheckBoxDropVmdFile);
            this.Controls.Add(this.TextBoxPmxExportPath);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CheckBoxDropPmxFile);
            this.Controls.Add(this.CheckBoxKeepLog);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.TextBoxScriptPath);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExportSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "弾幕生成";
            this.Load += new System.EventHandler(this.LoadForm);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox TextBoxScriptPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxPmxExportPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog OpenFileDialogScript;
        private System.Windows.Forms.CheckBox CheckBoxKeepLog;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialogPmx;
        private System.Windows.Forms.CheckBox CheckBoxDropPmxFile;
        private System.Windows.Forms.CheckBox CheckBoxDropVmdFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxVmdExportPath;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialogVmd;
    }
}