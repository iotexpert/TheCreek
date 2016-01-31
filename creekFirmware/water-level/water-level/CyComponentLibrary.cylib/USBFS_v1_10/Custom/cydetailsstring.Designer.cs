namespace USBFS_v1_10
{
    partial class CyDetailsString
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
            this.groupBoxParams = new System.Windows.Forms.GroupBox();
            this.textBoxString = new System.Windows.Forms.TextBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.groupBoxOption = new System.Windows.Forms.GroupBox();
            this.radioButtonSilicon = new System.Windows.Forms.RadioButton();
            this.radioButtonCallback = new System.Windows.Forms.RadioButton();
            this.radioButtonUser = new System.Windows.Forms.RadioButton();
            this.groupBoxParams.SuspendLayout();
            this.groupBoxOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxParams
            // 
            this.groupBoxParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxParams.Controls.Add(this.textBoxString);
            this.groupBoxParams.Location = new System.Drawing.Point(0, 9);
            this.groupBoxParams.Name = "groupBoxParams";
            this.groupBoxParams.Size = new System.Drawing.Size(332, 63);
            this.groupBoxParams.TabIndex = 3;
            this.groupBoxParams.TabStop = false;
            this.groupBoxParams.Text = "Value";
            // 
            // textBoxString
            // 
            this.textBoxString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxString.Location = new System.Drawing.Point(15, 25);
            this.textBoxString.Name = "textBoxString";
            this.textBoxString.Size = new System.Drawing.Size(308, 20);
            this.textBoxString.TabIndex = 12;
            this.textBoxString.Validated += new System.EventHandler(this.textBoxString_Validated);
            this.textBoxString.EnabledChanged += new System.EventHandler(this.textBoxString_EnabledChanged);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(129, 184);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 5;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Visible = false;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // groupBoxOption
            // 
            this.groupBoxOption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOption.Controls.Add(this.radioButtonSilicon);
            this.groupBoxOption.Controls.Add(this.radioButtonCallback);
            this.groupBoxOption.Controls.Add(this.radioButtonUser);
            this.groupBoxOption.Location = new System.Drawing.Point(0, 78);
            this.groupBoxOption.Name = "groupBoxOption";
            this.groupBoxOption.Size = new System.Drawing.Size(332, 100);
            this.groupBoxOption.TabIndex = 6;
            this.groupBoxOption.TabStop = false;
            this.groupBoxOption.Visible = false;
            // 
            // radioButtonSilicon
            // 
            this.radioButtonSilicon.AutoSize = true;
            this.radioButtonSilicon.Location = new System.Drawing.Point(6, 65);
            this.radioButtonSilicon.Name = "radioButtonSilicon";
            this.radioButtonSilicon.Size = new System.Drawing.Size(178, 17);
            this.radioButtonSilicon.TabIndex = 2;
            this.radioButtonSilicon.Text = "Silicon Generated Serial Number";
            this.radioButtonSilicon.UseVisualStyleBackColor = true;
            this.radioButtonSilicon.CheckedChanged += new System.EventHandler(this.radioButtonUser_CheckedChanged);
            // 
            // radioButtonCallback
            // 
            this.radioButtonCallback.AutoSize = true;
            this.radioButtonCallback.Location = new System.Drawing.Point(6, 42);
            this.radioButtonCallback.Name = "radioButtonCallback";
            this.radioButtonCallback.Size = new System.Drawing.Size(95, 17);
            this.radioButtonCallback.TabIndex = 1;
            this.radioButtonCallback.Text = "User Call Back";
            this.radioButtonCallback.UseVisualStyleBackColor = true;
            this.radioButtonCallback.CheckedChanged += new System.EventHandler(this.radioButtonUser_CheckedChanged);
            // 
            // radioButtonUser
            // 
            this.radioButtonUser.AutoSize = true;
            this.radioButtonUser.Checked = true;
            this.radioButtonUser.Location = new System.Drawing.Point(6, 19);
            this.radioButtonUser.Name = "radioButtonUser";
            this.radioButtonUser.Size = new System.Drawing.Size(108, 17);
            this.radioButtonUser.TabIndex = 0;
            this.radioButtonUser.TabStop = true;
            this.radioButtonUser.Text = "User EnteredText";
            this.radioButtonUser.UseVisualStyleBackColor = true;
            this.radioButtonUser.CheckedChanged += new System.EventHandler(this.radioButtonUser_CheckedChanged);
            // 
            // CyDetailsString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBoxOption);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.groupBoxParams);
            this.Name = "CyDetailsString";
            this.Size = new System.Drawing.Size(335, 217);
            this.Load += new System.EventHandler(this.CyDetailsString_Load);
            this.SizeChanged += new System.EventHandler(this.CyDetailsString_SizeChanged);
            this.groupBoxParams.ResumeLayout(false);
            this.groupBoxParams.PerformLayout();
            this.groupBoxOption.ResumeLayout(false);
            this.groupBoxOption.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxParams;
        private System.Windows.Forms.TextBox textBoxString;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.GroupBox groupBoxOption;
        private System.Windows.Forms.RadioButton radioButtonSilicon;
        private System.Windows.Forms.RadioButton radioButtonCallback;
        private System.Windows.Forms.RadioButton radioButtonUser;
    }
}
