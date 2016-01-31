namespace FanController_v2_20
{
    partial class CyBasicTab
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyBasicTab));
            this.ep_Errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_nudDampingFactor = new System.Windows.Forms.NumericUpDown();
            this.m_cmbFanTolerance = new System.Windows.Forms.ComboBox();
            this.m_lblFanTolerance = new System.Windows.Forms.Label();
            this.m_cbANR = new System.Windows.Forms.CheckBox();
            this.m_lblDampingFactor = new System.Windows.Forms.Label();
            this.m_rbModeOpenLoop = new System.Windows.Forms.RadioButton();
            this.m_rbModeClosedLoop = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_cbAltSpeedFailure = new System.Windows.Forms.CheckBox();
            this.m_cbAltFanStall = new System.Windows.Forms.CheckBox();
            this.m_connection = new System.Windows.Forms.CheckBox();
            this.gbTerminals = new System.Windows.Forms.GroupBox();
            this.m_externalClock = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDampingFactor)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.gbTerminals.SuspendLayout();
            this.SuspendLayout();
            // 
            // ep_Errors
            // 
            this.ep_Errors.ContainerControl = this;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.m_nudDampingFactor);
            this.groupBox2.Controls.Add(this.m_cmbFanTolerance);
            this.groupBox2.Controls.Add(this.m_lblFanTolerance);
            this.groupBox2.Controls.Add(this.m_cbANR);
            this.groupBox2.Controls.Add(this.m_lblDampingFactor);
            this.groupBox2.Controls.Add(this.m_rbModeOpenLoop);
            this.groupBox2.Controls.Add(this.m_rbModeClosedLoop);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_nudDampingFactor
            // 
            this.m_nudDampingFactor.DecimalPlaces = 2;
            this.m_nudDampingFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.m_nudDampingFactor, "m_nudDampingFactor");
            this.m_nudDampingFactor.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            131072});
            this.m_nudDampingFactor.Name = "m_nudDampingFactor";
            this.m_nudDampingFactor.ValueChanged += new System.EventHandler(this.m_nudDampingFactor_ValueChanged);
            // 
            // m_cmbFanTolerance
            // 
            this.m_cmbFanTolerance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cmbFanTolerance.FormattingEnabled = true;
            resources.ApplyResources(this.m_cmbFanTolerance, "m_cmbFanTolerance");
            this.m_cmbFanTolerance.Name = "m_cmbFanTolerance";
            // 
            // m_lblFanTolerance
            // 
            resources.ApplyResources(this.m_lblFanTolerance, "m_lblFanTolerance");
            this.m_lblFanTolerance.Name = "m_lblFanTolerance";
            // 
            // m_cbANR
            // 
            resources.ApplyResources(this.m_cbANR, "m_cbANR");
            this.m_cbANR.Name = "m_cbANR";
            this.m_cbANR.UseVisualStyleBackColor = true;
            // 
            // m_lblDampingFactor
            // 
            resources.ApplyResources(this.m_lblDampingFactor, "m_lblDampingFactor");
            this.m_lblDampingFactor.Name = "m_lblDampingFactor";
            // 
            // m_rbModeOpenLoop
            // 
            this.m_rbModeOpenLoop.AutoCheck = false;
            resources.ApplyResources(this.m_rbModeOpenLoop, "m_rbModeOpenLoop");
            this.m_rbModeOpenLoop.Name = "m_rbModeOpenLoop";
            this.m_rbModeOpenLoop.TabStop = true;
            this.m_rbModeOpenLoop.UseVisualStyleBackColor = true;
            this.m_rbModeOpenLoop.Click += new System.EventHandler(this.m_rbModeOpenLoop_Click);
            // 
            // m_rbModeClosedLoop
            // 
            this.m_rbModeClosedLoop.AutoCheck = false;
            resources.ApplyResources(this.m_rbModeClosedLoop, "m_rbModeClosedLoop");
            this.m_rbModeClosedLoop.Checked = true;
            this.m_rbModeClosedLoop.Name = "m_rbModeClosedLoop";
            this.m_rbModeClosedLoop.TabStop = true;
            this.m_rbModeClosedLoop.UseVisualStyleBackColor = true;
            this.m_rbModeClosedLoop.Click += new System.EventHandler(this.m_rbModeClosedLoop_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.m_cbAltSpeedFailure);
            this.groupBox3.Controls.Add(this.m_cbAltFanStall);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // m_cbAltSpeedFailure
            // 
            resources.ApplyResources(this.m_cbAltSpeedFailure, "m_cbAltSpeedFailure");
            this.m_cbAltSpeedFailure.Name = "m_cbAltSpeedFailure";
            this.m_cbAltSpeedFailure.UseVisualStyleBackColor = true;
            this.m_cbAltSpeedFailure.CheckedChanged += new System.EventHandler(this.m_cbAltSpeedFailure_CheckedChanged);
            // 
            // m_cbAltFanStall
            // 
            resources.ApplyResources(this.m_cbAltFanStall, "m_cbAltFanStall");
            this.m_cbAltFanStall.Name = "m_cbAltFanStall";
            this.m_cbAltFanStall.UseVisualStyleBackColor = true;
            this.m_cbAltFanStall.CheckedChanged += new System.EventHandler(this.m_cbAltFanStall_CheckedChanged);
            // 
            // m_connection
            // 
            resources.ApplyResources(this.m_connection, "m_connection");
            this.m_connection.Name = "m_connection";
            this.m_connection.UseVisualStyleBackColor = true;
            // 
            // gbTerminals
            // 
            this.gbTerminals.Controls.Add(this.m_externalClock);
            this.gbTerminals.Controls.Add(this.m_connection);
            resources.ApplyResources(this.gbTerminals, "gbTerminals");
            this.gbTerminals.Name = "gbTerminals";
            this.gbTerminals.TabStop = false;
            // 
            // m_externalClock
            // 
            resources.ApplyResources(this.m_externalClock, "m_externalClock");
            this.m_externalClock.Name = "m_externalClock";
            this.m_externalClock.UseVisualStyleBackColor = true;
            // 
            // CyBasicTab
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTerminals);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "CyBasicTab";
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDampingFactor)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbTerminals.ResumeLayout(false);
            this.gbTerminals.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider ep_Errors;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton m_rbModeOpenLoop;
        private System.Windows.Forms.RadioButton m_rbModeClosedLoop;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox m_cbAltSpeedFailure;
        private System.Windows.Forms.CheckBox m_cbAltFanStall;
        private System.Windows.Forms.Label m_lblDampingFactor;
        private System.Windows.Forms.CheckBox m_cbANR;
        private System.Windows.Forms.Label m_lblFanTolerance;
        private System.Windows.Forms.ComboBox m_cmbFanTolerance;
        private System.Windows.Forms.GroupBox gbTerminals;
        private System.Windows.Forms.CheckBox m_connection;
        private System.Windows.Forms.CheckBox m_externalClock;
        private System.Windows.Forms.NumericUpDown m_nudDampingFactor;


    }
}
