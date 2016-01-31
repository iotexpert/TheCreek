namespace TIA_v1_70
{
    partial class TIAcontrol
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
            this.Capacitive_Feedback = new System.Windows.Forms.Label();
            this.Minimum_Vdda = new System.Windows.Forms.Label();
            this.Power = new System.Windows.Forms.Label();
            this.Resistive_Feedback = new System.Windows.Forms.Label();
            this.mCapFb = new System.Windows.Forms.ComboBox();
            this.mMinVdda = new System.Windows.Forms.ComboBox();
            this.mPower = new System.Windows.Forms.ComboBox();
            this.mResFb = new System.Windows.Forms.ComboBox();
            this.mResFbk = new System.Windows.Forms.ComboBox();
            this.mPowr = new System.Windows.Forms.ComboBox();
            this.mCapFbk = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.db = new System.Windows.Forms.Label();
            this.DbFreq = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Capacitive_Feedback
            // 
            this.Capacitive_Feedback.Location = new System.Drawing.Point(0, 0);
            this.Capacitive_Feedback.Name = "Capacitive_Feedback";
            this.Capacitive_Feedback.Size = new System.Drawing.Size(100, 23);
            this.Capacitive_Feedback.TabIndex = 0;
            // 
            // Minimum_Vdda
            // 
            this.Minimum_Vdda.Location = new System.Drawing.Point(0, 0);
            this.Minimum_Vdda.Name = "Minimum_Vdda";
            this.Minimum_Vdda.Size = new System.Drawing.Size(100, 23);
            this.Minimum_Vdda.TabIndex = 0;
            // 
            // Power
            // 
            this.Power.Location = new System.Drawing.Point(0, 0);
            this.Power.Name = "Power";
            this.Power.Size = new System.Drawing.Size(100, 23);
            this.Power.TabIndex = 0;
            // 
            // Resistive_Feedback
            // 
            this.Resistive_Feedback.Location = new System.Drawing.Point(0, 0);
            this.Resistive_Feedback.Name = "Resistive_Feedback";
            this.Resistive_Feedback.Size = new System.Drawing.Size(100, 23);
            this.Resistive_Feedback.TabIndex = 0;
            // 
            // mCapFb
            // 
            this.mCapFb.Location = new System.Drawing.Point(0, 0);
            this.mCapFb.Name = "mCapFb";
            this.mCapFb.Size = new System.Drawing.Size(121, 21);
            this.mCapFb.TabIndex = 0;
            // 
            // mMinVdda
            // 
            this.mMinVdda.Location = new System.Drawing.Point(0, 0);
            this.mMinVdda.Name = "mMinVdda";
            this.mMinVdda.Size = new System.Drawing.Size(121, 21);
            this.mMinVdda.TabIndex = 0;
            // 
            // mPower
            // 
            this.mPower.Location = new System.Drawing.Point(0, 0);
            this.mPower.Name = "mPower";
            this.mPower.Size = new System.Drawing.Size(121, 21);
            this.mPower.TabIndex = 0;
            // 
            // mResFb
            // 
            this.mResFb.Location = new System.Drawing.Point(0, 0);
            this.mResFb.Name = "mResFb";
            this.mResFb.Size = new System.Drawing.Size(121, 21);
            this.mResFb.TabIndex = 0;
            // 
            // mResFbk
            // 
            this.mResFbk.FormattingEnabled = true;
            this.mResFbk.Location = new System.Drawing.Point(130, 88);
            this.mResFbk.Name = "mResFbk";
            this.mResFbk.Size = new System.Drawing.Size(208, 21);
            this.mResFbk.TabIndex = 15;
            this.mResFbk.SelectedIndexChanged += new System.EventHandler(this.mResFbk_SelectedIndexChanged);
            // 
            // mPowr
            // 
            this.mPowr.FormattingEnabled = true;
            this.mPowr.Location = new System.Drawing.Point(130, 51);
            this.mPowr.Name = "mPowr";
            this.mPowr.Size = new System.Drawing.Size(208, 21);
            this.mPowr.TabIndex = 14;
            this.mPowr.SelectedIndexChanged += new System.EventHandler(this.mPowr_SelectedIndexChanged);
            // 
            // mCapFbk
            // 
            this.mCapFbk.FormattingEnabled = true;
            this.mCapFbk.Location = new System.Drawing.Point(130, 17);
            this.mCapFbk.Name = "mCapFbk";
            this.mCapFbk.Size = new System.Drawing.Size(208, 21);
            this.mCapFbk.TabIndex = 12;
            this.mCapFbk.SelectedIndexChanged += new System.EventHandler(this.mCapFbk_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Resistive_Feedback";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Power";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Capacitive_Feedback";
            // 
            // db
            // 
            this.db.AutoSize = true;
            this.db.Location = new System.Drawing.Point(14, 166);
            this.db.Name = "db";
            this.db.Size = new System.Drawing.Size(85, 13);
            this.db.TabIndex = 16;
            this.db.Text = "-3 dB Frequency";
            // 
            // DbFreq
            // 
            this.DbFreq.Location = new System.Drawing.Point(137, 167);
            this.DbFreq.Name = "DbFreq";
            this.DbFreq.ReadOnly = true;
            this.DbFreq.Size = new System.Drawing.Size(88, 20);
            this.DbFreq.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mResFbk);
            this.groupBox1.Controls.Add(this.mPowr);
            this.groupBox1.Controls.Add(this.mCapFbk);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(8, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 124);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Options";
            // 
            // TIAcontrol
            // 
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.DbFreq);
            this.Controls.Add(this.db);
            this.Name = "TIAcontrol";
            this.Size = new System.Drawing.Size(428, 200);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Capacitive_Feedback;
        private System.Windows.Forms.Label Minimum_Vdda;
        private System.Windows.Forms.Label Power;
        private System.Windows.Forms.Label Resistive_Feedback;
        private System.Windows.Forms.ComboBox mCapFb;
        private System.Windows.Forms.ComboBox mMinVdda;
        private System.Windows.Forms.ComboBox mPower;
        private System.Windows.Forms.ComboBox mResFb;
        private System.Windows.Forms.ComboBox mResFbk;
        private System.Windows.Forms.ComboBox mPowr;
        private System.Windows.Forms.ComboBox mCapFbk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label db;
        private System.Windows.Forms.TextBox DbFreq;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
