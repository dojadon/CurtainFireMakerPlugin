namespace CurtainFireMakerPlugin.Forms
{
    partial class ExportSettingControl
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
            this.exportPmxText = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.exportVmdText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.modelNameText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.modelDescriptionText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.pmxFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.vmdFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.scriptText = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "出力先(pmx)";
            // 
            // exportPmxText
            // 
            this.exportPmxText.Location = new System.Drawing.Point(8, 36);
            this.exportPmxText.Name = "exportPmxText";
            this.exportPmxText.Size = new System.Drawing.Size(383, 19);
            this.exportPmxText.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(397, 77);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "参照";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Click_ExportVmd);
            // 
            // exportVmdText
            // 
            this.exportVmdText.Location = new System.Drawing.Point(8, 79);
            this.exportVmdText.Name = "exportVmdText";
            this.exportVmdText.Size = new System.Drawing.Size(383, 19);
            this.exportVmdText.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "出力先(vmd)";
            // 
            // modelNameText
            // 
            this.modelNameText.Location = new System.Drawing.Point(8, 122);
            this.modelNameText.Name = "modelNameText";
            this.modelNameText.Size = new System.Drawing.Size(413, 19);
            this.modelNameText.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 107);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "モデル名";
            // 
            // modelDescriptionText
            // 
            this.modelDescriptionText.Location = new System.Drawing.Point(8, 165);
            this.modelDescriptionText.Multiline = true;
            this.modelDescriptionText.Name = "modelDescriptionText";
            this.modelDescriptionText.Size = new System.Drawing.Size(413, 85);
            this.modelDescriptionText.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 150);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "モデル説明";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(91, 354);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "OK";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Click_OK);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(300, 354);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "Cansel";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Click_Cancel);
            // 
            // pmxFileDialog
            // 
            this.pmxFileDialog.FileName = "openFileDialog1";
            // 
            // vmdFileDialog
            // 
            this.vmdFileDialog.FileName = "openFileDialog1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.exportPmxText);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.modelDescriptionText);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.exportVmdText);
            this.groupBox1.Controls.Add(this.modelNameText);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(6, 88);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(461, 257);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "出力設定";
            // 
            // scriptText
            // 
            this.scriptText.Location = new System.Drawing.Point(14, 40);
            this.scriptText.Name = "scriptText";
            this.scriptText.Size = new System.Drawing.Size(383, 19);
            this.scriptText.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(397, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "参照";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Click_ExportPmx);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(403, 38);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(45, 23);
            this.button5.TabIndex = 14;
            this.button5.Text = "参照";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 25);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "スクリプト";
            // 
            // ExportSettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.scriptText);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Name = "ExportSettingControl";
            this.Size = new System.Drawing.Size(473, 402);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox exportPmxText;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox exportVmdText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox modelNameText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox modelDescriptionText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.OpenFileDialog pmxFileDialog;
        private System.Windows.Forms.OpenFileDialog vmdFileDialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox scriptText;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label5;
    }
}
