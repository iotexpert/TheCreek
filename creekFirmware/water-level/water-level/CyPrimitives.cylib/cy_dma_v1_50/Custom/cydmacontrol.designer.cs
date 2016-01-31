namespace Cypress.Components.System.cy_dma_v1_50
{
    partial class CyDMAControl
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
            this.components = new global::System.ComponentModel.Container();
            this.label1 = new global::System.Windows.Forms.Label();
            this.m_requestComboBox = new global::System.Windows.Forms.ComboBox();
            this.label2 = new global::System.Windows.Forms.Label();
            this.m_terminationComboBox = new global::System.Windows.Forms.ComboBox();
            this.m_errorProvider = new global::System.Windows.Forms.ErrorProvider(this.components);
            ((global::System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new global::System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new global::System.Drawing.Size(130, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hardware Request:";
            // 
            // m_requestComboBox
            // 
            this.m_requestComboBox.Anchor = ((global::System.Windows.Forms.AnchorStyles)(((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left)
                        | global::System.Windows.Forms.AnchorStyles.Right)));
            this.m_requestComboBox.FormattingEnabled = true;
            this.m_requestComboBox.Location = new global::System.Drawing.Point(173, 3);
            this.m_requestComboBox.Name = "m_requestComboBox";
            this.m_requestComboBox.Size = new global::System.Drawing.Size(188, 24);
            this.m_requestComboBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new global::System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new global::System.Drawing.Size(152, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Hardware Termination:";
            // 
            // m_terminationComboBox
            // 
            this.m_terminationComboBox.Anchor = ((global::System.Windows.Forms.AnchorStyles)(((global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left)
                        | global::System.Windows.Forms.AnchorStyles.Right)));
            this.m_terminationComboBox.FormattingEnabled = true;
            this.m_terminationComboBox.Location = new global::System.Drawing.Point(173, 33);
            this.m_terminationComboBox.Name = "m_terminationComboBox";
            this.m_terminationComboBox.Size = new global::System.Drawing.Size(188, 24);
            this.m_terminationComboBox.TabIndex = 3;
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.ContainerControl = this;
            // 
            // CyDMAControl
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_terminationComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_requestComboBox);
            this.Controls.Add(this.label1);
            this.Name = "CyDMAControl";
            this.Size = new global::System.Drawing.Size(364, 90);
            ((global::System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private global::System.Windows.Forms.Label label1;
        private global::System.Windows.Forms.ComboBox m_requestComboBox;
        private global::System.Windows.Forms.Label label2;
        private global::System.Windows.Forms.ComboBox m_terminationComboBox;
        private global::System.Windows.Forms.ErrorProvider m_errorProvider;
    }
}
