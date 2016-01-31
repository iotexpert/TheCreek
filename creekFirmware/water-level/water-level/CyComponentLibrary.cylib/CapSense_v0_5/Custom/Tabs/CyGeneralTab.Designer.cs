namespace  CapSense_v0_5
{
    partial class CyGeneralTab
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
            this.gbConfiguration = new System.Windows.Forms.GroupBox();
            this.rbParallelAsynch = new System.Windows.Forms.RadioButton();
            this.rbParallel = new System.Windows.Forms.RadioButton();
            this.rbSerial = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pBottom = new System.Windows.Forms.Panel();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.gbLeft = new System.Windows.Forms.Panel();
            this.cntCCSPropsL = new  CapSense_v0_5.cntGenCSProps();
            this.lLeftHalf = new System.Windows.Forms.Label();
            this.gbRight = new System.Windows.Forms.Panel();
            this.cntCCSPropsR = new  CapSense_v0_5.cntGenCSProps();
            this.lRightHalf = new System.Windows.Forms.Label();
            this.gbConfiguration.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pBottom.SuspendLayout();
            this.tableLayoutMain.SuspendLayout();
            this.gbLeft.SuspendLayout();
            this.gbRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConfiguration
            // 
            this.gbConfiguration.Controls.Add(this.rbParallelAsynch);
            this.gbConfiguration.Controls.Add(this.rbParallel);
            this.gbConfiguration.Controls.Add(this.rbSerial);
            this.gbConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbConfiguration.Location = new System.Drawing.Point(0, 0);
            this.gbConfiguration.Name = "gbConfiguration";
            this.gbConfiguration.Size = new System.Drawing.Size(617, 31);
            this.gbConfiguration.TabIndex = 3;
            this.gbConfiguration.TabStop = false;
            this.gbConfiguration.Text = "Configuration";
            // 
            // rbParallelAsynch
            // 
            this.rbParallelAsynch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rbParallelAsynch.AutoSize = true;
            this.rbParallelAsynch.Location = new System.Drawing.Point(356, 10);
            this.rbParallelAsynch.Name = "rbParallelAsynch";
            this.rbParallelAsynch.Size = new System.Drawing.Size(129, 17);
            this.rbParallelAsynch.TabIndex = 0;
            this.rbParallelAsynch.Text = "Parallel Asynchronous";
            this.rbParallelAsynch.UseVisualStyleBackColor = true;
            this.rbParallelAsynch.CheckedChanged += new System.EventHandler(this.rbConfiguration_CheckedChanged);
            // 
            // rbParallel
            // 
            this.rbParallel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rbParallel.AutoSize = true;
            this.rbParallel.Location = new System.Drawing.Point(224, 10);
            this.rbParallel.Name = "rbParallel";
            this.rbParallel.Size = new System.Drawing.Size(126, 17);
            this.rbParallel.TabIndex = 0;
            this.rbParallel.Text = "Parallel Synchronized";
            this.rbParallel.UseVisualStyleBackColor = true;
            this.rbParallel.CheckedChanged += new System.EventHandler(this.rbConfiguration_CheckedChanged);
            // 
            // rbSerial
            // 
            this.rbSerial.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rbSerial.AutoSize = true;
            this.rbSerial.Checked = true;
            this.rbSerial.Location = new System.Drawing.Point(140, 10);
            this.rbSerial.Name = "rbSerial";
            this.rbSerial.Size = new System.Drawing.Size(51, 17);
            this.rbSerial.TabIndex = 1;
            this.rbSerial.TabStop = true;
            this.rbSerial.Text = "Serial";
            this.rbSerial.UseVisualStyleBackColor = true;
            this.rbSerial.CheckedChanged += new System.EventHandler(this.rbConfiguration_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbConfiguration);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(617, 33);
            this.panel1.TabIndex = 0;
            // 
            // pBottom
            // 
            this.pBottom.AutoScroll = true;
            this.pBottom.Controls.Add(this.tableLayoutMain);
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBottom.Location = new System.Drawing.Point(0, 33);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(617, 290);
            this.pBottom.TabIndex = 4;
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 2;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.Controls.Add(this.gbLeft, 0, 0);
            this.tableLayoutMain.Controls.Add(this.gbRight, 1, 0);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 1;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.Size = new System.Drawing.Size(617, 290);
            this.tableLayoutMain.TabIndex = 6;
            // 
            // gbLeft
            // 
            this.gbLeft.Controls.Add(this.cntCCSPropsL);
            this.gbLeft.Controls.Add(this.lLeftHalf);
            this.gbLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLeft.Location = new System.Drawing.Point(0, 0);
            this.gbLeft.Margin = new System.Windows.Forms.Padding(0);
            this.gbLeft.Name = "gbLeft";
            this.gbLeft.Size = new System.Drawing.Size(308, 290);
            this.gbLeft.TabIndex = 5;
            this.gbLeft.Text = "Left  AMUX Bus Scan Slot Assignment";
            // 
            // cntCCSPropsL
            // 
            this.cntCCSPropsL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cntCCSPropsL.Location = new System.Drawing.Point(0, 19);
            this.cntCCSPropsL.Name = "cntCCSPropsL";
            this.cntCCSPropsL.SelectedIndex = 0;
            this.cntCCSPropsL.Size = new System.Drawing.Size(308, 271);
            this.cntCCSPropsL.TabIndex = 40;
            // 
            // lLeftHalf
            // 
            this.lLeftHalf.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.lLeftHalf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lLeftHalf.Dock = System.Windows.Forms.DockStyle.Top;
            this.lLeftHalf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLeftHalf.Location = new System.Drawing.Point(0, 0);
            this.lLeftHalf.Name = "lLeftHalf";
            this.lLeftHalf.Size = new System.Drawing.Size(308, 19);
            this.lLeftHalf.TabIndex = 39;
            this.lLeftHalf.Text = "Left AMUX Bus Scan Slot Assignment";
            this.lLeftHalf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbRight
            // 
            this.gbRight.Controls.Add(this.cntCCSPropsR);
            this.gbRight.Controls.Add(this.lRightHalf);
            this.gbRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRight.Location = new System.Drawing.Point(308, 0);
            this.gbRight.Margin = new System.Windows.Forms.Padding(0);
            this.gbRight.Name = "gbRight";
            this.gbRight.Size = new System.Drawing.Size(309, 290);
            this.gbRight.TabIndex = 4;
            this.gbRight.Text = "Right  AMUX Bus Scan Slot Assignment";
            // 
            // cntCCSPropsR
            // 
            this.cntCCSPropsR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cntCCSPropsR.Location = new System.Drawing.Point(0, 19);
            this.cntCCSPropsR.Name = "cntCCSPropsR";
            this.cntCCSPropsR.SelectedIndex = 0;
            this.cntCCSPropsR.Size = new System.Drawing.Size(309, 271);
            this.cntCCSPropsR.TabIndex = 40;
            // 
            // lRightHalf
            // 
            this.lRightHalf.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.lRightHalf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lRightHalf.Dock = System.Windows.Forms.DockStyle.Top;
            this.lRightHalf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lRightHalf.Location = new System.Drawing.Point(0, 0);
            this.lRightHalf.Name = "lRightHalf";
            this.lRightHalf.Size = new System.Drawing.Size(309, 19);
            this.lRightHalf.TabIndex = 39;
            this.lRightHalf.Text = "Right AMUX Bus Scan Slot Assignment";
            this.lRightHalf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CyGeneralTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.pBottom);
            this.Controls.Add(this.panel1);
            this.Name = "CyGeneralTab";
            this.Size = new System.Drawing.Size(617, 323);
            this.Leave += new System.EventHandler(this.cyGeneralTab_Leave);
            this.gbConfiguration.ResumeLayout(false);
            this.gbConfiguration.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pBottom.ResumeLayout(false);
            this.tableLayoutMain.ResumeLayout(false);
            this.gbLeft.ResumeLayout(false);
            this.gbRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbConfiguration;
        private System.Windows.Forms.RadioButton rbSerial;
        private System.Windows.Forms.Panel pBottom;
        private System.Windows.Forms.Panel gbRight;
        private System.Windows.Forms.RadioButton rbParallelAsynch;
        private System.Windows.Forms.Panel gbLeft;
        private System.Windows.Forms.Label lLeftHalf;
        private System.Windows.Forms.Label lRightHalf;
        public System.Windows.Forms.RadioButton rbParallel;
        private cntGenCSProps cntCCSPropsL;
        private cntGenCSProps cntCCSPropsR;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
    }
}
