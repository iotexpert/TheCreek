namespace GlitchFilter_v2_0
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
            this.components = new System.ComponentModel.Container();
            this.m_lblSignalWidth = new System.Windows.Forms.Label();
            this.m_numSignalWidth = new System.Windows.Forms.NumericUpDown();
            this.m_numGlitchLength = new System.Windows.Forms.NumericUpDown();
            this.m_lblGlitchLength = new System.Windows.Forms.Label();
            this.m_rbNone = new System.Windows.Forms.RadioButton();
            this.m_rbLogicZero = new System.Windows.Forms.RadioButton();
            this.m_rbLogicOne = new System.Windows.Forms.RadioButton();
            this.m_grpBypassFilter = new System.Windows.Forms.GroupBox();
            this.m_toolTipHint = new System.Windows.Forms.ToolTip(this.components);
            this.m_lblGlitchPulseLength = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_numSignalWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numGlitchLength)).BeginInit();
            this.m_grpBypassFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_lblSignalWidth
            // 
            this.m_lblSignalWidth.AutoSize = true;
            this.m_lblSignalWidth.Location = new System.Drawing.Point(3, 5);
            this.m_lblSignalWidth.Name = "m_lblSignalWidth";
            this.m_lblSignalWidth.Size = new System.Drawing.Size(92, 13);
            this.m_lblSignalWidth.TabIndex = 1;
            this.m_lblSignalWidth.Text = "Signal width (bits):";
            // 
            // m_numSignalWidth
            // 
            this.m_numSignalWidth.Location = new System.Drawing.Point(125, 3);
            this.m_numSignalWidth.Maximum = new decimal(new int[] {
            -1486618624,
            232830643,
            0,
            0});
            this.m_numSignalWidth.Name = "m_numSignalWidth";
            this.m_numSignalWidth.Size = new System.Drawing.Size(41, 20);
            this.m_numSignalWidth.TabIndex = 2;
            this.m_toolTipHint.SetToolTip(this.m_numSignalWidth, "Determines the bus width of d and q terminals.");
            // 
            // m_numGlitchLength
            // 
            this.m_numGlitchLength.Location = new System.Drawing.Point(125, 30);
            this.m_numGlitchLength.Maximum = new decimal(new int[] {
            -1486618624,
            232830643,
            0,
            0});
            this.m_numGlitchLength.Name = "m_numGlitchLength";
            this.m_numGlitchLength.Size = new System.Drawing.Size(41, 20);
            this.m_numGlitchLength.TabIndex = 3;
            this.m_toolTipHint.SetToolTip(this.m_numGlitchLength, "Defines the number of samples for which input has to be stable before being propa" +
                    "gated to the output.");
            // 
            // m_lblGlitchLength
            // 
            this.m_lblGlitchLength.AutoSize = true;
            this.m_lblGlitchLength.Location = new System.Drawing.Point(3, 32);
            this.m_lblGlitchLength.Name = "m_lblGlitchLength";
            this.m_lblGlitchLength.Size = new System.Drawing.Size(116, 13);
            this.m_lblGlitchLength.TabIndex = 4;
            this.m_lblGlitchLength.Text = "Glitch length (samples):";
            // 
            // m_rbNone
            // 
            this.m_rbNone.AutoSize = true;
            this.m_rbNone.Location = new System.Drawing.Point(6, 19);
            this.m_rbNone.Name = "m_rbNone";
            this.m_rbNone.Size = new System.Drawing.Size(51, 17);
            this.m_rbNone.TabIndex = 6;
            this.m_rbNone.TabStop = true;
            this.m_rbNone.Text = "None";
            this.m_toolTipHint.SetToolTip(this.m_rbNone, "Specifies whether the corresponding logic level is filtered or directly propagate" +
                    "d to the output.");
            this.m_rbNone.UseVisualStyleBackColor = true;
            // 
            // m_rbLogicZero
            // 
            this.m_rbLogicZero.AutoSize = true;
            this.m_rbLogicZero.Location = new System.Drawing.Point(6, 42);
            this.m_rbLogicZero.Name = "m_rbLogicZero";
            this.m_rbLogicZero.Size = new System.Drawing.Size(74, 17);
            this.m_rbLogicZero.TabIndex = 7;
            this.m_rbLogicZero.TabStop = true;
            this.m_rbLogicZero.Text = "Logic zero";
            this.m_toolTipHint.SetToolTip(this.m_rbLogicZero, "Specifies whether the corresponding logic level is filtered or directly propagate" +
                    "d to the output.");
            this.m_rbLogicZero.UseVisualStyleBackColor = true;
            // 
            // m_rbLogicOne
            // 
            this.m_rbLogicOne.AutoSize = true;
            this.m_rbLogicOne.Location = new System.Drawing.Point(6, 65);
            this.m_rbLogicOne.Name = "m_rbLogicOne";
            this.m_rbLogicOne.Size = new System.Drawing.Size(72, 17);
            this.m_rbLogicOne.TabIndex = 8;
            this.m_rbLogicOne.TabStop = true;
            this.m_rbLogicOne.Text = "Logic one";
            this.m_toolTipHint.SetToolTip(this.m_rbLogicOne, "Specifies whether the corresponding logic level is filtered or directly propagate" +
                    "d to the output.");
            this.m_rbLogicOne.UseVisualStyleBackColor = true;
            // 
            // m_grpBypassFilter
            // 
            this.m_grpBypassFilter.Controls.Add(this.m_rbNone);
            this.m_grpBypassFilter.Controls.Add(this.m_rbLogicOne);
            this.m_grpBypassFilter.Controls.Add(this.m_rbLogicZero);
            this.m_grpBypassFilter.Location = new System.Drawing.Point(6, 56);
            this.m_grpBypassFilter.Name = "m_grpBypassFilter";
            this.m_grpBypassFilter.Size = new System.Drawing.Size(136, 91);
            this.m_grpBypassFilter.TabIndex = 9;
            this.m_grpBypassFilter.TabStop = false;
            this.m_grpBypassFilter.Text = "Bypass filter";
            this.m_toolTipHint.SetToolTip(this.m_grpBypassFilter, "Specifies whether the corresponding logic level is filtered or directly propagate" +
                    "d to the output.");
            // 
            // m_lblGlitchPulseLength
            // 
            this.m_lblGlitchPulseLength.AutoSize = true;
            this.m_lblGlitchPulseLength.Location = new System.Drawing.Point(181, 32);
            this.m_lblGlitchPulseLength.Name = "m_lblGlitchPulseLength";
            this.m_lblGlitchPulseLength.Size = new System.Drawing.Size(27, 13);
            this.m_lblGlitchPulseLength.TabIndex = 10;
            this.m_lblGlitchPulseLength.Text = "N/A";
            // 
            // CyGeneralTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_lblGlitchPulseLength);
            this.Controls.Add(this.m_grpBypassFilter);
            this.Controls.Add(this.m_lblGlitchLength);
            this.Controls.Add(this.m_numGlitchLength);
            this.Controls.Add(this.m_numSignalWidth);
            this.Controls.Add(this.m_lblSignalWidth);
            this.Name = "CyGeneralTab";
            this.Size = new System.Drawing.Size(210, 153);
            ((System.ComponentModel.ISupportInitialize)(this.m_numSignalWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numGlitchLength)).EndInit();
            this.m_grpBypassFilter.ResumeLayout(false);
            this.m_grpBypassFilter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lblSignalWidth;
        private System.Windows.Forms.NumericUpDown m_numSignalWidth;
        private System.Windows.Forms.NumericUpDown m_numGlitchLength;
        private System.Windows.Forms.Label m_lblGlitchLength;
        private System.Windows.Forms.RadioButton m_rbNone;
        private System.Windows.Forms.RadioButton m_rbLogicZero;
        private System.Windows.Forms.RadioButton m_rbLogicOne;
        private System.Windows.Forms.GroupBox m_grpBypassFilter;
        private System.Windows.Forms.ToolTip m_toolTipHint;
        private System.Windows.Forms.Label m_lblGlitchPulseLength;
    }
}
