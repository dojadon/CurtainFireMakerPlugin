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
            this.components = new System.ComponentModel.Container();
            this.listBoxSequence = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemNew = new System.Windows.Forms.ToolStripMenuItem();
            this.追加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.エクスプローラーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.atomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxSelectedScript = new System.Windows.Forms.TextBox();
            this.labelPath = new System.Windows.Forms.Label();
            this.saveFileDialogScript = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxSequence
            // 
            this.listBoxSequence.AllowDrop = true;
            this.listBoxSequence.ContextMenuStrip = this.contextMenuStrip1;
            this.listBoxSequence.FormattingEnabled = true;
            this.listBoxSequence.ItemHeight = 12;
            this.listBoxSequence.Location = new System.Drawing.Point(6, 8);
            this.listBoxSequence.Name = "listBoxSequence";
            this.listBoxSequence.Size = new System.Drawing.Size(154, 316);
            this.listBoxSequence.TabIndex = 17;
            this.listBoxSequence.SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChangedSequence);
            this.listBoxSequence.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropSequence);
            this.listBoxSequence.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterSequence);
            this.listBoxSequence.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListBoxMouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemNew,
            this.追加ToolStripMenuItem,
            this.ToolStripMenuItemRemove,
            this.toolStripMenuItem1,
            this.ToolStripMenuItemOpen});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(99, 98);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.OpeningContextMenu);
            // 
            // ToolStripMenuItemNew
            // 
            this.ToolStripMenuItemNew.Name = "ToolStripMenuItemNew";
            this.ToolStripMenuItemNew.Size = new System.Drawing.Size(98, 22);
            this.ToolStripMenuItemNew.Text = "新規";
            this.ToolStripMenuItemNew.Click += new System.EventHandler(this.CreateNewFile);
            // 
            // 追加ToolStripMenuItem
            // 
            this.追加ToolStripMenuItem.Name = "追加ToolStripMenuItem";
            this.追加ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.追加ToolStripMenuItem.Text = "追加";
            this.追加ToolStripMenuItem.Click += new System.EventHandler(this.ClickAdd);
            // 
            // ToolStripMenuItemRemove
            // 
            this.ToolStripMenuItemRemove.Name = "ToolStripMenuItemRemove";
            this.ToolStripMenuItemRemove.Size = new System.Drawing.Size(98, 22);
            this.ToolStripMenuItemRemove.Text = "削除";
            this.ToolStripMenuItemRemove.Click += new System.EventHandler(this.ClickRemove);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(95, 6);
            // 
            // ToolStripMenuItemOpen
            // 
            this.ToolStripMenuItemOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.エクスプローラーToolStripMenuItem,
            this.atomToolStripMenuItem});
            this.ToolStripMenuItemOpen.Name = "ToolStripMenuItemOpen";
            this.ToolStripMenuItemOpen.Size = new System.Drawing.Size(98, 22);
            this.ToolStripMenuItemOpen.Text = "開く";
            // 
            // エクスプローラーToolStripMenuItem
            // 
            this.エクスプローラーToolStripMenuItem.Name = "エクスプローラーToolStripMenuItem";
            this.エクスプローラーToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.エクスプローラーToolStripMenuItem.Text = "エクスプローラー";
            this.エクスプローラーToolStripMenuItem.Click += new System.EventHandler(this.OpenWithExplorer);
            // 
            // atomToolStripMenuItem
            // 
            this.atomToolStripMenuItem.Name = "atomToolStripMenuItem";
            this.atomToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.atomToolStripMenuItem.Text = "Atom";
            this.atomToolStripMenuItem.Click += new System.EventHandler(this.OpenWithAtom);
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
            this.textBoxSelectedScript.TextChanged += new System.EventHandler(this.TextChangedScript);
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
            // saveFileDialogScript
            // 
            this.saveFileDialogScript.Filter = "Python Script|*.py";
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
            this.contextMenuStrip1.ResumeLayout(false);
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
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem エクスプローラーToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem atomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 追加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemNew;
        private System.Windows.Forms.SaveFileDialog saveFileDialogScript;
    }
}
