namespace PRS_v2_0
{
    partial class CyAdvancedControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyAdvancedControl));
            this.groupBoxTimeMultiplexing = new System.Windows.Forms.GroupBox();
            this.labelDisable = new System.Windows.Forms.Label();
            this.labelEnable = new System.Windows.Forms.Label();
            this.radioButtonDisabled = new System.Windows.Forms.RadioButton();
            this.radioButtonEnabled = new System.Windows.Forms.RadioButton();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBoxWakeup = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelStart = new System.Windows.Forms.Label();
            this.labelRestore = new System.Windows.Forms.Label();
            this.radioButtonWakeupStart = new System.Windows.Forms.RadioButton();
            this.radioButtonWakeupResume = new System.Windows.Forms.RadioButton();
            this.groupBoxTimeMultiplexing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupBoxWakeup.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTimeMultiplexing
            // 
            this.groupBoxTimeMultiplexing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTimeMultiplexing.Controls.Add(this.labelDisable);
            this.groupBoxTimeMultiplexing.Controls.Add(this.labelEnable);
            this.groupBoxTimeMultiplexing.Controls.Add(this.radioButtonDisabled);
            this.groupBoxTimeMultiplexing.Controls.Add(this.radioButtonEnabled);
            this.groupBoxTimeMultiplexing.Location = new System.Drawing.Point(3, 3);
            this.groupBoxTimeMultiplexing.Name = "groupBoxTimeMultiplexing";
            this.groupBoxTimeMultiplexing.Size = new System.Drawing.Size(482, 123);
            this.groupBoxTimeMultiplexing.TabIndex = 0;
            this.groupBoxTimeMultiplexing.TabStop = false;
            this.groupBoxTimeMultiplexing.Text = "Implementation:";
            // 
            // labelDisable
            // 
            this.labelDisable.Location = new System.Drawing.Point(235, 39);
            this.labelDisable.Name = "labelDisable";
            this.labelDisable.Size = new System.Drawing.Size(241, 79);
            this.labelDisable.TabIndex = 4;
            this.labelDisable.Text = resources.GetString("labelDisable.Text");
            // 
            // labelEnable
            // 
            this.labelEnable.Location = new System.Drawing.Point(3, 39);
            this.labelEnable.Name = "labelEnable";
            this.labelEnable.Size = new System.Drawing.Size(213, 79);
            this.labelEnable.TabIndex = 3;
            this.labelEnable.Text = resources.GetString("labelEnable.Text");
            // 
            // radioButtonDisabled
            // 
            this.radioButtonDisabled.AutoSize = true;
            this.radioButtonDisabled.ForeColor = System.Drawing.SystemColors.Highlight;
            this.radioButtonDisabled.Location = new System.Drawing.Point(235, 19);
            this.radioButtonDisabled.Name = "radioButtonDisabled";
            this.radioButtonDisabled.Size = new System.Drawing.Size(83, 17);
            this.radioButtonDisabled.TabIndex = 2;
            this.radioButtonDisabled.Text = "Single Cycle";
            this.radioButtonDisabled.UseVisualStyleBackColor = true;
            this.radioButtonDisabled.CheckedChanged += new System.EventHandler(this.radioButtonEnabled_CheckedChanged);
            // 
            // radioButtonEnabled
            // 
            this.radioButtonEnabled.AutoSize = true;
            this.radioButtonEnabled.Checked = true;
            this.radioButtonEnabled.ForeColor = System.Drawing.SystemColors.Highlight;
            this.radioButtonEnabled.Location = new System.Drawing.Point(6, 19);
            this.radioButtonEnabled.Name = "radioButtonEnabled";
            this.radioButtonEnabled.Size = new System.Drawing.Size(132, 17);
            this.radioButtonEnabled.TabIndex = 1;
            this.radioButtonEnabled.TabStop = true;
            this.radioButtonEnabled.Text = "Time Division Multiplex";
            this.radioButtonEnabled.UseVisualStyleBackColor = true;
            this.radioButtonEnabled.CheckedChanged += new System.EventHandler(this.radioButtonEnabled_CheckedChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // groupBoxWakeup
            // 
            this.groupBoxWakeup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxWakeup.Controls.Add(this.panel1);
            this.groupBoxWakeup.Controls.Add(this.radioButtonWakeupStart);
            this.groupBoxWakeup.Controls.Add(this.radioButtonWakeupResume);
            this.groupBoxWakeup.Location = new System.Drawing.Point(3, 132);
            this.groupBoxWakeup.Name = "groupBoxWakeup";
            this.groupBoxWakeup.Size = new System.Drawing.Size(482, 152);
            this.groupBoxWakeup.TabIndex = 3;
            this.groupBoxWakeup.TabStop = false;
            this.groupBoxWakeup.Text = "Low Power Mode Operation:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.labelStart);
            this.panel1.Controls.Add(this.labelRestore);
            this.panel1.Location = new System.Drawing.Point(6, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 113);
            this.panel1.TabIndex = 0;
            // 
            // labelStart
            // 
            this.labelStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStart.Location = new System.Drawing.Point(232, 5);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(224, 106);
            this.labelStart.TabIndex = 4;
            this.labelStart.Text = resources.GetString("labelStart.Text");
            // 
            // labelRestore
            // 
            this.labelRestore.Location = new System.Drawing.Point(0, 5);
            this.labelRestore.Name = "labelRestore";
            this.labelRestore.Size = new System.Drawing.Size(221, 106);
            this.labelRestore.TabIndex = 3;
            this.labelRestore.Text = resources.GetString("labelRestore.Text");
            // 
            // radioButtonWakeupStart
            // 
            this.radioButtonWakeupStart.AutoSize = true;
            this.radioButtonWakeupStart.ForeColor = System.Drawing.SystemColors.Highlight;
            this.radioButtonWakeupStart.Location = new System.Drawing.Point(238, 18);
            this.radioButtonWakeupStart.Name = "radioButtonWakeupStart";
            this.radioButtonWakeupStart.Size = new System.Drawing.Size(124, 17);
            this.radioButtonWakeupStart.TabIndex = 2;
            this.radioButtonWakeupStart.Text = "Restart on Power Up";
            this.radioButtonWakeupStart.UseVisualStyleBackColor = true;
            // 
            // radioButtonWakeupResume
            // 
            this.radioButtonWakeupResume.AutoSize = true;
            this.radioButtonWakeupResume.Checked = true;
            this.radioButtonWakeupResume.ForeColor = System.Drawing.SystemColors.Highlight;
            this.radioButtonWakeupResume.Location = new System.Drawing.Point(9, 18);
            this.radioButtonWakeupResume.Name = "radioButtonWakeupResume";
            this.radioButtonWakeupResume.Size = new System.Drawing.Size(127, 17);
            this.radioButtonWakeupResume.TabIndex = 1;
            this.radioButtonWakeupResume.TabStop = true;
            this.radioButtonWakeupResume.Text = "Restore on Power Up";
            this.radioButtonWakeupResume.UseVisualStyleBackColor = true;
            this.radioButtonWakeupResume.CheckedChanged += new System.EventHandler(this.radioButtonWakeupResume_CheckedChanged);
            // 
            // CyAdvancedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.groupBoxWakeup);
            this.Controls.Add(this.groupBoxTimeMultiplexing);
            this.Name = "CyAdvancedControl";
            this.Size = new System.Drawing.Size(494, 288);
            this.SizeChanged += new System.EventHandler(this.CyAdvancedControl_SizeChanged);
            this.groupBoxTimeMultiplexing.ResumeLayout(false);
            this.groupBoxTimeMultiplexing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.groupBoxWakeup.ResumeLayout(false);
            this.groupBoxWakeup.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTimeMultiplexing;
        private System.Windows.Forms.RadioButton radioButtonDisabled;
        private System.Windows.Forms.RadioButton radioButtonEnabled;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox groupBoxWakeup;
        private System.Windows.Forms.RadioButton radioButtonWakeupStart;
        private System.Windows.Forms.RadioButton radioButtonWakeupResume;
        private System.Windows.Forms.Label labelEnable;
        private System.Windows.Forms.Label labelDisable;
        private System.Windows.Forms.Label labelStart;
        private System.Windows.Forms.Label labelRestore;
        private System.Windows.Forms.Panel panel1;
    }
}
