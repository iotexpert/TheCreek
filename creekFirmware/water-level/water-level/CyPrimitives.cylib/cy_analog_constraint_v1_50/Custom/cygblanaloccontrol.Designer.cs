namespace Cypress.Components.Primitives.cy_analog_constraint_v1_50
{
    partial class CyGlobalAnalogLocationControl
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
            this.m_gblAnaLocComboBox = new global::System.Windows.Forms.ComboBox();
            this.label1 = new global::System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_gblAnaLocComboBox
            // 
            this.m_gblAnaLocComboBox.Anchor = ((global::System.Windows.Forms.AnchorStyles)(((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left)
                        | global::System.Windows.Forms.AnchorStyles.Right)));
            this.m_gblAnaLocComboBox.FormattingEnabled = true;
            this.m_gblAnaLocComboBox.Location = new global::System.Drawing.Point(171, 3);
            this.m_gblAnaLocComboBox.Name = "m_gblAnaLocComboBox";
            this.m_gblAnaLocComboBox.Size = new global::System.Drawing.Size(129, 24);
            this.m_gblAnaLocComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new global::System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new global::System.Drawing.Size(162, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Analog Resource Name:";
            // 
            // CyGlobalAnalogLocationControl
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_gblAnaLocComboBox);
            this.Name = "CyGlobalAnalogLocationControl";
            this.Size = new global::System.Drawing.Size(307, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private global::System.Windows.Forms.ComboBox m_gblAnaLocComboBox;
        private global::System.Windows.Forms.Label label1;
    }
}
