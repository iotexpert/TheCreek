namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_10
{
    partial class CyMappingControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyMappingControl));
            this.m_displayAsBusCheckBox = new System.Windows.Forms.CheckBox();
            this.m_contCheckBox = new System.Windows.Forms.CheckBox();
            this.m_spanningCheckBox = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // m_displayAsBusCheckBox
            // 
            this.m_displayAsBusCheckBox.AutoSize = true;
            this.m_displayAsBusCheckBox.Location = new System.Drawing.Point(3, 3);
            this.m_displayAsBusCheckBox.Name = "m_displayAsBusCheckBox";
            this.m_displayAsBusCheckBox.Size = new System.Drawing.Size(123, 21);
            this.m_displayAsBusCheckBox.TabIndex = 0;
            this.m_displayAsBusCheckBox.Text = "Display as Bus";
            this.m_displayAsBusCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_contCheckBox
            // 
            this.m_contCheckBox.AutoSize = true;
            this.m_contCheckBox.Location = new System.Drawing.Point(3, 122);
            this.m_contCheckBox.Name = "m_contCheckBox";
            this.m_contCheckBox.Size = new System.Drawing.Size(101, 21);
            this.m_contCheckBox.TabIndex = 1;
            this.m_contCheckBox.Text = "Contiguous";
            this.m_contCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_spanningCheckBox
            // 
            this.m_spanningCheckBox.AutoSize = true;
            this.m_spanningCheckBox.Location = new System.Drawing.Point(3, 214);
            this.m_spanningCheckBox.Name = "m_spanningCheckBox";
            this.m_spanningCheckBox.Size = new System.Drawing.Size(90, 21);
            this.m_spanningCheckBox.TabIndex = 2;
            this.m_spanningCheckBox.Text = "Spanning";
            this.m_spanningCheckBox.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(25, 30);
            this.textBox1.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(534, 86);
            this.textBox1.TabIndex = 3;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(25, 149);
            this.textBox2.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(534, 59);
            this.textBox2.TabIndex = 4;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Forces placement in adjacent physical pins.\r\nPort-level APIs will only be generat" +
                "ed for this instance if it is set to be Contiguous. Per-pin APIs will be globall" +
                "y available either way.";
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(25, 241);
            this.textBox3.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(534, 69);
            this.textBox3.TabIndex = 5;
            this.textBox3.TabStop = false;
            this.textBox3.Text = "Allows placement in multiple physical ports.\r\nNote: Spanning is currently configu" +
                "red based on the Contiguous setting. This will not be the case in the future.";
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.m_errorProvider.ContainerControl = this;
            // 
            // CyMappingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.m_displayAsBusCheckBox);
            this.Controls.Add(this.m_spanningCheckBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.m_contCheckBox);
            this.Name = "CyMappingControl";
            this.Size = new System.Drawing.Size(562, 508);
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox m_displayAsBusCheckBox;
        private System.Windows.Forms.CheckBox m_contCheckBox;
        private System.Windows.Forms.CheckBox m_spanningCheckBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ErrorProvider m_errorProvider;
    }
}
