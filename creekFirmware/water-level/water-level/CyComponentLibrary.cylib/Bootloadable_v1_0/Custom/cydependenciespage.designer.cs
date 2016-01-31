namespace Bootloadable_v1_0
{
    partial class CyDependenciesPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelInfo = new System.Windows.Forms.Label();
            this.labelHexFile = new System.Windows.Forms.Label();
            this.textBoxHexFilePath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelInfo
            // 
            this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInfo.Location = new System.Drawing.Point(3, 9);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(326, 50);
            this.labelInfo.TabIndex = 2;
            this.labelInfo.Text = "Bootloadable projects require a reference to the associated Bootloader project\'s " +
                ".hex and .elf files. These files must have the same name and exist in the same d" +
                "irectory.";
            // 
            // labelHexFile
            // 
            this.labelHexFile.AutoSize = true;
            this.labelHexFile.Location = new System.Drawing.Point(3, 59);
            this.labelHexFile.Name = "labelHexFile";
            this.labelHexFile.Size = new System.Drawing.Size(97, 13);
            this.labelHexFile.TabIndex = 3;
            this.labelHexFile.Text = "Bootloader hex file:";
            // 
            // textBoxHexFilePath
            // 
            this.textBoxHexFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxHexFilePath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBoxHexFilePath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.textBoxHexFilePath.Location = new System.Drawing.Point(6, 75);
            this.textBoxHexFilePath.Name = "textBoxHexFilePath";
            this.textBoxHexFilePath.Size = new System.Drawing.Size(323, 20);
            this.textBoxHexFilePath.TabIndex = 5;
            this.textBoxHexFilePath.TextChanged += new System.EventHandler(this.textBoxHexFilePath_TextChanged);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.Location = new System.Drawing.Point(252, 101);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(77, 23);
            this.buttonBrowse.TabIndex = 7;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // CyDependenciesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxHexFilePath);
            this.Controls.Add(this.labelHexFile);
            this.Controls.Add(this.labelInfo);
            this.Name = "CyDependenciesPage";
            this.Size = new System.Drawing.Size(352, 298);
            this.Load += new System.EventHandler(this.CyDependenciesPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelHexFile;
        private System.Windows.Forms.TextBox textBoxHexFilePath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
