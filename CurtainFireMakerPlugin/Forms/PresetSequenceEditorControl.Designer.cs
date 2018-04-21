namespace CurtainFireMakerPlugin.Forms
{
    partial class PresetSequenceEditorControl
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
            this.listBoxSequence = new System.Windows.Forms.ListBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxSelectedScript = new System.Windows.Forms.TextBox();
            this.openFileDialogScript = new System.Windows.Forms.OpenFileDialog();
            this.labelPath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxSequence
            // 
            this.listBoxSequence.AllowDrop = true;
            this.listBoxSequence.FormattingEnabled = true;
            this.listBoxSequence.ItemHeight = 12;
            this.listBoxSequence.Location = new System.Drawing.Point(6, 8);
            this.listBoxSequence.Name = "listBoxSequence";
            this.listBoxSequence.Size = new System.Drawing.Size(154, 316);
            this.listBoxSequence.TabIndex = 17;
            this.listBoxSequence.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChangedSequence);
            this.listBoxSequence.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropSequence);
            this.listBoxSequence.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterSequence);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button6.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button6.Location = new System.Drawing.Point(137, 330);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(23, 23);
            this.button6.TabIndex = 16;
            this.button6.Text = "↓";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.ClickDown);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button5.Location = new System.Drawing.Point(108, 330);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(23, 23);
            this.button5.TabIndex = 15;
            this.button5.Text = "↑";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.ClickUp);
            // 
            // button4
            // 
            this.button4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button4.Location = new System.Drawing.Point(57, 330);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(45, 23);
            this.button4.TabIndex = 14;
            this.button4.Text = "削除";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.ClickRemove);
            // 
            // button3
            // 
            this.button3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button3.Location = new System.Drawing.Point(6, 330);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(45, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "追加";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.ClickAdd);
            // 
            // textBoxSelectedScript
            // 
            this.textBoxSelectedScript.Location = new System.Drawing.Point(166, 23);
            this.textBoxSelectedScript.Multiline = true;
            this.textBoxSelectedScript.Name = "textBoxSelectedScript";
            this.textBoxSelectedScript.ReadOnly = true;
            this.textBoxSelectedScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSelectedScript.Size = new System.Drawing.Size(473, 328);
            this.textBoxSelectedScript.TabIndex = 12;
            this.textBoxSelectedScript.WordWrap = false;
            // 
            // openFileDialogScript
            // 
            this.openFileDialogScript.FileName = "openFileDialog1";
            this.openFileDialogScript.Filter = "Python Script|*.py";
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labelPath.Location = new System.Drawing.Point(166, 8);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(28, 12);
            this.labelPath.TabIndex = 18;
            this.labelPath.Text = "Path";
            // 
            // PresetSequenceEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelPath);
            this.Controls.Add(this.listBoxSequence);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBoxSelectedScript);
            this.Name = "PresetSequenceEditorControl";
            this.Size = new System.Drawing.Size(642, 354);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listBoxSequence;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxSelectedScript;
        private System.Windows.Forms.OpenFileDialog openFileDialogScript;
        private System.Windows.Forms.Label labelPath;
    }
}
