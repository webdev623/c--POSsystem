namespace Ovan_P1
{
    partial class MenuReading
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
            this.headerPanel = new System.Windows.Forms.Panel();
            this.menuReadingTitle = new System.Windows.Forms.Label();
            this.bodyPanel = new System.Windows.Forms.Panel();
            this.cancelButton = new Ovan_P1.RoundedButton();
            this.readButton = new Ovan_P1.RoundedButton();
            this.subContent2 = new System.Windows.Forms.Label();
            this.subContent1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.headerPanel.SuspendLayout();
            this.bodyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.Controls.Add(this.menuReadingTitle);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(800, 80);
            this.headerPanel.TabIndex = 0;
            // 
            // menuReadingTitle
            // 
            this.menuReadingTitle.AutoSize = true;
            this.menuReadingTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuReadingTitle.ForeColor = System.Drawing.Color.Red;
            this.menuReadingTitle.Location = new System.Drawing.Point(50, 30);
            this.menuReadingTitle.Name = "menuReadingTitle";
            this.menuReadingTitle.Size = new System.Drawing.Size(138, 51);
            this.menuReadingTitle.TabIndex = 0;
            this.menuReadingTitle.Text = "label1";
            // 
            // bodyPanel
            // 
            this.bodyPanel.Controls.Add(this.cancelButton);
            this.bodyPanel.Controls.Add(this.readButton);
            this.bodyPanel.Controls.Add(this.subContent2);
            this.bodyPanel.Controls.Add(this.subContent1);
            this.bodyPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bodyPanel.Location = new System.Drawing.Point(0, 77);
            this.bodyPanel.Name = "bodyPanel";
            this.bodyPanel.Size = new System.Drawing.Size(800, 373);
            this.bodyPanel.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(645, 301);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(126, 48);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "roundedButton1";
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // readButton
            // 
            this.readButton.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.readButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.readButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.readButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.readButton.ForeColor = System.Drawing.Color.White;
            this.readButton.Location = new System.Drawing.Point(352, 185);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(198, 56);
            this.readButton.TabIndex = 1;
            this.readButton.Text = "roundedButton1";
            this.readButton.UseVisualStyleBackColor = false;
            // 
            // subContent2
            // 
            this.subContent2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subContent2.Location = new System.Drawing.Point(62, 92);
            this.subContent2.Name = "subContent2";
            this.subContent2.Size = new System.Drawing.Size(85, 29);
            this.subContent2.TabIndex = 0;
            this.subContent2.Text = "label1";
            this.subContent2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // subContent1
            // 
            this.subContent1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subContent1.Location = new System.Drawing.Point(62, 50);
            this.subContent1.Name = "subContent1";
            this.subContent1.Size = new System.Drawing.Size(85, 29);
            this.subContent1.TabIndex = 0;
            this.subContent1.Text = "label1";
            this.subContent1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // MenuReading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bodyPanel);
            this.Controls.Add(this.headerPanel);
            this.Name = "MenuReading";
            this.Text = "MenuReading";
            this.Load += new System.EventHandler(this.MenuReading_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.bodyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Panel bodyPanel;
        private System.Windows.Forms.Label menuReadingTitle;
        private RoundedButton readButton;
        private System.Windows.Forms.Label subContent2;
        private System.Windows.Forms.Label subContent1;
        private RoundedButton cancelButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}