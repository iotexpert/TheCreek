namespace  CapSense_v1_30
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
            this.cntCCSPropsL = new CapSense_v1_30.CyGenPropertyUnit();
            this.lLeftHalf = new System.Windows.Forms.Label();
            this.gbRight = new System.Windows.Forms.Panel();
            this.cntCCSPropsR = new CapSense_v1_30.CyGenPropertyUnit();
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
            this.gbConfiguration.Margin = new System.Windows.Forms.Padding(4);
            this.gbConfiguration.Name = "gbConfiguration";
            this.gbConfiguration.Padding = new System.Windows.Forms.Padding(4);
            this.gbConfiguration.Size = new System.Drawing.Size(562, 41);
            this.gbConfiguration.TabIndex = 5;
            this.gbConfiguration.TabStop = false;
            this.gbConfiguration.Text = "Configuration";
            //
            // rbParallelAsynch
            //
            this.rbParallelAsynch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rbParallelAsynch.AutoSize = true;
            this.rbParallelAsynch.Location = new System.Drawing.Point(348, 18);
            this.rbParallelAsynch.Margin = new System.Windows.Forms.Padding(4);
            this.rbParallelAsynch.Name = "rbParallelAsynch";
            this.rbParallelAsynch.Size = new System.Drawing.Size(170, 21);
            this.rbParallelAsynch.TabIndex = 3;
            this.rbParallelAsynch.Text = "Parallel Asynchronous";
            this.rbParallelAsynch.UseVisualStyleBackColor = true;
            this.rbParallelAsynch.CheckedChanged += new System.EventHandler(this.rbConfiguration_CheckedChanged);
            //
            // rbParallel
            //
            this.rbParallel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rbParallel.AutoSize = true;
            this.rbParallel.Location = new System.Drawing.Point(154, 18);
            this.rbParallel.Margin = new System.Windows.Forms.Padding(4);
            this.rbParallel.Name = "rbParallel";
            this.rbParallel.Size = new System.Drawing.Size(166, 21);
            this.rbParallel.TabIndex = 2;
            this.rbParallel.Text = "Parallel Synchronized";
            this.rbParallel.UseVisualStyleBackColor = true;
            this.rbParallel.CheckedChanged += new System.EventHandler(this.rbConfiguration_CheckedChanged);
            //
            // rbSerial
            //
            this.rbSerial.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rbSerial.AutoSize = true;
            this.rbSerial.Checked = true;
            this.rbSerial.Location = new System.Drawing.Point(57, 18);
            this.rbSerial.Margin = new System.Windows.Forms.Padding(4);
            this.rbSerial.Name = "rbSerial";
            this.rbSerial.Size = new System.Drawing.Size(65, 21);
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
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(562, 42);
            this.panel1.TabIndex = 0;
            //
            // pBottom
            //
            this.pBottom.AutoScroll = true;
            this.pBottom.Controls.Add(this.tableLayoutMain);
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBottom.Location = new System.Drawing.Point(0, 42);
            this.pBottom.Margin = new System.Windows.Forms.Padding(4);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(562, 354);
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
            this.tableLayoutMain.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 1;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.Size = new System.Drawing.Size(562, 354);
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
            this.gbLeft.Size = new System.Drawing.Size(281, 354);
            this.gbLeft.TabIndex = 10;
            this.gbLeft.Text = "Left  AMUX Bus Scan Slot Assignment";
            //
            // cntCCSPropsL
            //
            this.cntCCSPropsL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cntCCSPropsL.Location = new System.Drawing.Point(0, 23);
            this.cntCCSPropsL.Margin = new System.Windows.Forms.Padding(5);
            this.cntCCSPropsL.Name = "cntCCSPropsL";
            this.cntCCSPropsL.SelectedIndex = 0;
            this.cntCCSPropsL.Size = new System.Drawing.Size(281, 331);
            this.cntCCSPropsL.TabIndex = 10;
            //
            // lLeftHalf
            //
            this.lLeftHalf.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.lLeftHalf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lLeftHalf.Dock = System.Windows.Forms.DockStyle.Top;
            this.lLeftHalf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLeftHalf.Location = new System.Drawing.Point(0, 0);
            this.lLeftHalf.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lLeftHalf.Name = "lLeftHalf";
            this.lLeftHalf.Size = new System.Drawing.Size(281, 23);
            this.lLeftHalf.TabIndex = 39;
            this.lLeftHalf.Text = "Left AMUX Bus Scan Slot Assignment";
            this.lLeftHalf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // gbRight
            //
            this.gbRight.Controls.Add(this.cntCCSPropsR);
            this.gbRight.Controls.Add(this.lRightHalf);
            this.gbRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRight.Location = new System.Drawing.Point(281, 0);
            this.gbRight.Margin = new System.Windows.Forms.Padding(0);
            this.gbRight.Name = "gbRight";
            this.gbRight.Size = new System.Drawing.Size(281, 354);
            this.gbRight.TabIndex = 20;
            this.gbRight.Text = "Right  AMUX Bus Scan Slot Assignment";
            //
            // cntCCSPropsR
            //
            this.cntCCSPropsR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cntCCSPropsR.Location = new System.Drawing.Point(0, 23);
            this.cntCCSPropsR.Margin = new System.Windows.Forms.Padding(5);
            this.cntCCSPropsR.Name = "cntCCSPropsR";
            this.cntCCSPropsR.SelectedIndex = 0;
            this.cntCCSPropsR.Size = new System.Drawing.Size(281, 331);
            this.cntCCSPropsR.TabIndex = 20;
            //
            // lRightHalf
            //
            this.lRightHalf.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.lRightHalf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lRightHalf.Dock = System.Windows.Forms.DockStyle.Top;
            this.lRightHalf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lRightHalf.Location = new System.Drawing.Point(0, 0);
            this.lRightHalf.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lRightHalf.Name = "lRightHalf";
            this.lRightHalf.Size = new System.Drawing.Size(281, 23);
            this.lRightHalf.TabIndex = 39;
            this.lRightHalf.Text = "Right AMUX Bus Scan Slot Assignment";
            this.lRightHalf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // CyGeneralTab
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.pBottom);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CyGeneralTab";
            this.Size = new System.Drawing.Size(562, 396);
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
        private CyGenPropertyUnit cntCCSPropsL;
        private CyGenPropertyUnit cntCCSPropsR;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
    }
}
