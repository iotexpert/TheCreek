namespace CapSense_CSD_v2_0
{
    partial class CyTuningTab
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
            this.cbEnableTuneHelper = new System.Windows.Forms.CheckBox();
            this.tbEZI2CInstaceName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbSubAddressSize = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // cbEnableTuneHelper
            // 
            this.cbEnableTuneHelper.AutoSize = true;
            this.cbEnableTuneHelper.Location = new System.Drawing.Point(14, 24);
            this.cbEnableTuneHelper.Name = "cbEnableTuneHelper";
            this.cbEnableTuneHelper.Size = new System.Drawing.Size(157, 21);
            this.cbEnableTuneHelper.TabIndex = 0;
            this.cbEnableTuneHelper.Text = "Enable Tune Helper";
            this.cbEnableTuneHelper.UseVisualStyleBackColor = true;
            // 
            // tbEZI2CInstaceName
            // 
            this.tbEZI2CInstaceName.Location = new System.Drawing.Point(269, 60);
            this.tbEZI2CInstaceName.Name = "tbEZI2CInstaceName";
            this.tbEZI2CInstaceName.Size = new System.Drawing.Size(147, 22);
            this.tbEZI2CInstaceName.TabIndex = 1;
            this.tbEZI2CInstaceName.TextChanged += new System.EventHandler(this.tbEZI2CInstaceName_TextChanged);
            this.tbEZI2CInstaceName.Validating += new System.ComponentModel.CancelEventHandler(this.tbEZI2CInstaceName_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Instance name for EzI2C component:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(8, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(405, 37);
            this.label2.TabIndex = 3;
            this.label2.Text = "Please open EzI2C component customizer and assign these properties:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(256, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "bit";
            // 
            // cbSubAddressSize
            // 
            this.cbSubAddressSize.Enabled = false;
            this.cbSubAddressSize.FormattingEnabled = true;
            this.cbSubAddressSize.Items.AddRange(new object[] {
            "8",
            "16"});
            this.cbSubAddressSize.Location = new System.Drawing.Point(195, 72);
            this.cbSubAddressSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbSubAddressSize.Name = "cbSubAddressSize";
            this.cbSubAddressSize.Size = new System.Drawing.Size(57, 24);
            this.cbSubAddressSize.TabIndex = 14;
            this.cbSubAddressSize.Text = "16";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "Sub-address size:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cbSubAddressSize);
            this.groupBox1.Location = new System.Drawing.Point(3, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(454, 111);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CyTuningTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbEZI2CInstaceName);
            this.Controls.Add(this.cbEnableTuneHelper);
            this.Name = "CyTuningTab";
            this.Size = new System.Drawing.Size(460, 293);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbEnableTuneHelper;
        private System.Windows.Forms.TextBox tbEZI2CInstaceName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbSubAddressSize;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
