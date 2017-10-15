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
            this.scriptText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pmxExportPathTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.scriptFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.keepLogOpenCheckBox = new System.Windows.Forms.CheckBox();
            this.pmxExportDirDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBoxDropPmxFile = new System.Windows.Forms.CheckBox();
            this.checkBoxDropVmdFile = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.vmdExportPathTextBox = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.vmdExportDirDialog = new System.Windows.Forms.FolderBrowserDialog();
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
            this.button5.Location = new System.Drawing.Point(403, 28);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(45, 23);
            this.button5.TabIndex = 20;
            this.button5.Text = "参照";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Click_Script);
            // 
            // scriptText
            // 
            this.scriptText.BackColor = System.Drawing.SystemColors.Window;
            this.scriptText.Location = new System.Drawing.Point(14, 30);
            this.scriptText.Name = "scriptText";
            this.scriptText.ReadOnly = true;
            this.scriptText.Size = new System.Drawing.Size(383, 19);
            this.scriptText.TabIndex = 19;
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
            // pmxExportPathTextBox
            // 
            this.pmxExportPathTextBox.Location = new System.Drawing.Point(14, 73);
            this.pmxExportPathTextBox.Name = "pmxExportPathTextBox";
            this.pmxExportPathTextBox.Size = new System.Drawing.Size(383, 19);
            this.pmxExportPathTextBox.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(403, 71);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "参照";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Click_PmxExportDir);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(304, 236);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 17;
            this.button4.Text = "キャンセル";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Click_Cancel);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(95, 236);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 16;
            this.button3.Text = "出力";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Click_OK);
            // 
            // scriptFileDialog
            // 
            this.scriptFileDialog.FileName = "openFileDialog1";
            this.scriptFileDialog.Filter = "|*.py";
            // 
            // keepLogOpenCheckBox
            // 
            this.keepLogOpenCheckBox.AutoSize = true;
            this.keepLogOpenCheckBox.Location = new System.Drawing.Point(14, 161);
            this.keepLogOpenCheckBox.Name = "keepLogOpenCheckBox";
            this.keepLogOpenCheckBox.Size = new System.Drawing.Size(128, 16);
            this.keepLogOpenCheckBox.TabIndex = 22;
            this.keepLogOpenCheckBox.Text = "ログを開いたままにする";
            this.keepLogOpenCheckBox.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(339, 157);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 23);
            this.button2.TabIndex = 23;
            this.button2.Text = "IronPython初期化";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Click_InitIronPython);
            // 
            // checkBoxDropPmxFile
            // 
            this.checkBoxDropPmxFile.AutoSize = true;
            this.checkBoxDropPmxFile.Location = new System.Drawing.Point(14, 183);
            this.checkBoxDropPmxFile.Name = "checkBoxDropPmxFile";
            this.checkBoxDropPmxFile.Size = new System.Drawing.Size(223, 16);
            this.checkBoxDropPmxFile.TabIndex = 24;
            this.checkBoxDropPmxFile.Text = "出力時にpmxファイルをMMMへドロップする";
            this.checkBoxDropPmxFile.UseVisualStyleBackColor = true;
            // 
            // checkBoxDropVmdFile
            // 
            this.checkBoxDropVmdFile.AutoSize = true;
            this.checkBoxDropVmdFile.Location = new System.Drawing.Point(14, 205);
            this.checkBoxDropVmdFile.Name = "checkBoxDropVmdFile";
            this.checkBoxDropVmdFile.Size = new System.Drawing.Size(223, 16);
            this.checkBoxDropVmdFile.TabIndex = 25;
            this.checkBoxDropVmdFile.Text = "出力時にvmdファイルをMMMへドロップする";
            this.checkBoxDropVmdFile.UseVisualStyleBackColor = true;
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
            // vmdExportPathTextBox
            // 
            this.vmdExportPathTextBox.Location = new System.Drawing.Point(14, 115);
            this.vmdExportPathTextBox.Name = "vmdExportPathTextBox";
            this.vmdExportPathTextBox.Size = new System.Drawing.Size(383, 19);
            this.vmdExportPathTextBox.TabIndex = 27;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(403, 113);
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
            this.ClientSize = new System.Drawing.Size(476, 276);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.vmdExportPathTextBox);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxDropVmdFile);
            this.Controls.Add(this.pmxExportPathTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxDropPmxFile);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.keepLogOpenCheckBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.scriptText);
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
        private System.Windows.Forms.TextBox scriptText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pmxExportPathTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog scriptFileDialog;
        private System.Windows.Forms.CheckBox keepLogOpenCheckBox;
        private System.Windows.Forms.FolderBrowserDialog pmxExportDirDialog;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBoxDropPmxFile;
        private System.Windows.Forms.CheckBox checkBoxDropVmdFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox vmdExportPathTextBox;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.FolderBrowserDialog vmdExportDirDialog;
    }
}