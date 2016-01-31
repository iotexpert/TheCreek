namespace Cypress.Components.System.cy_gsref_v1_0
{
    partial class CyGlobalSignalRefIDControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private global::System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PerformDispose();
            }

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
            this.m_gsRefComboBox = new global::System.Windows.Forms.ComboBox();
            this.label1 = new global::System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_gsRefComboBox
            // 
            this.m_gsRefComboBox.Anchor = ((global::System.Windows.Forms.AnchorStyles)(((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left)
                        | global::System.Windows.Forms.AnchorStyles.Right)));
            this.m_gsRefComboBox.FormattingEnabled = true;
            this.m_gsRefComboBox.Location = new global::System.Drawing.Point(146, 3);
            this.m_gsRefComboBox.Name = "m_gsRefComboBox";
            this.m_gsRefComboBox.Size = new global::System.Drawing.Size(114, 24);
            this.m_gsRefComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new global::System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new global::System.Drawing.Size(137, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Global Signal Name:";
            // 
            // CyGlobalSignalRefIDControl
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_gsRefComboBox);
            this.Name = "CyGlobalSignalRefIDControl";
            this.Size = new global::System.Drawing.Size(267, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private global::System.Windows.Forms.ComboBox m_gsRefComboBox;
        private global::System.Windows.Forms.Label label1;
    }
}
