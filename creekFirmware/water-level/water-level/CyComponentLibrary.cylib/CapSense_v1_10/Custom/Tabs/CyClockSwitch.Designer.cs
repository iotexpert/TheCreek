namespace  CapSense_v1_10
{
    partial class CyClockSwitch
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lLeftHalf = new System.Windows.Forms.Label();
            this.lRightHalf = new System.Windows.Forms.Label();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lLeftHalf
            // 
            this.lLeftHalf.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.lLeftHalf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lLeftHalf.Dock = System.Windows.Forms.DockStyle.Top;
            this.lLeftHalf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lLeftHalf.Location = new System.Drawing.Point(0, 0);
            this.lLeftHalf.Margin = new System.Windows.Forms.Padding(0);
            this.lLeftHalf.Name = "lLeftHalf";
            this.lLeftHalf.Size = new System.Drawing.Size(188, 19);
            this.lLeftHalf.TabIndex = 0;
            this.lLeftHalf.Text = "Left Half";
            this.lLeftHalf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lRightHalf
            // 
            this.lRightHalf.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.lRightHalf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lRightHalf.Dock = System.Windows.Forms.DockStyle.Top;
            this.lRightHalf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lRightHalf.Location = new System.Drawing.Point(188, 0);
            this.lRightHalf.Margin = new System.Windows.Forms.Padding(0);
            this.lRightHalf.Name = "lRightHalf";
            this.lRightHalf.Size = new System.Drawing.Size(188, 19);
            this.lRightHalf.TabIndex = 0;
            this.lRightHalf.Text = "Right Half";
            this.lRightHalf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 2;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.Controls.Add(this.lLeftHalf, 0, 0);
            this.tableLayoutMain.Controls.Add(this.lRightHalf, 1, 0);
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 3);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 2;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutMain.Size = new System.Drawing.Size(376, 208);
            this.tableLayoutMain.TabIndex = 2;
            // 
            // CyClockSwitch
            // 
            this.Controls.Add(this.tableLayoutMain);
            this.Name = "CyClockSwitch";
            this.Size = new System.Drawing.Size(379, 214);            
            this.tableLayoutMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lLeftHalf;
        private System.Windows.Forms.Label lRightHalf;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
    }
}
