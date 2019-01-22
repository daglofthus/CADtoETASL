namespace SWapp
{
    partial class Form1
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.swProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.swTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.swFileName = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.swBrowse = new System.Windows.Forms.Button();
            this.swCancel = new System.Windows.Forms.Button();
            this.swProcess = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.swProgressBar,
            this.swTime,
            this.swFileName});
            this.statusStrip1.Location = new System.Drawing.Point(0, 421);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(827, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // swProgressBar
            // 
            this.swProgressBar.Name = "swProgressBar";
            this.swProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // swTime
            // 
            this.swTime.Name = "swTime";
            this.swTime.Size = new System.Drawing.Size(49, 17);
            this.swTime.Text = "00:00:00";
            // 
            // swFileName
            // 
            this.swFileName.Name = "swFileName";
            this.swFileName.Size = new System.Drawing.Size(42, 17);
            this.swFileName.Text = "Ready!";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridView1, 3);
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(821, 375);
            this.dataGridView1.TabIndex = 1;
            // 
            // swBrowse
            // 
            this.swBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.swBrowse.Location = new System.Drawing.Point(3, 384);
            this.swBrowse.Name = "swBrowse";
            this.swBrowse.Size = new System.Drawing.Size(269, 34);
            this.swBrowse.TabIndex = 2;
            this.swBrowse.Text = "Browse...";
            this.swBrowse.UseVisualStyleBackColor = true;
            this.swBrowse.Click += new System.EventHandler(this.swBrowse_Click);
            // 
            // swCancel
            // 
            this.swCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.swCancel.Enabled = false;
            this.swCancel.Location = new System.Drawing.Point(553, 384);
            this.swCancel.Name = "swCancel";
            this.swCancel.Size = new System.Drawing.Size(271, 34);
            this.swCancel.TabIndex = 3;
            this.swCancel.Text = "Cancel";
            this.swCancel.UseVisualStyleBackColor = true;
            // 
            // swProcess
            // 
            this.swProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.swProcess.Location = new System.Drawing.Point(278, 384);
            this.swProcess.Name = "swProcess";
            this.swProcess.Size = new System.Drawing.Size(269, 34);
            this.swProcess.TabIndex = 4;
            this.swProcess.Text = "Process";
            this.swProcess.UseVisualStyleBackColor = true;
            this.swProcess.Click += new System.EventHandler(this.swProcess_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.swBrowse, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.swProcess, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.swCancel, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(827, 421);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(827, 443);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar swProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel swTime;
        private System.Windows.Forms.ToolStripStatusLabel swFileName;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button swBrowse;
        private System.Windows.Forms.Button swCancel;
        private System.Windows.Forms.Button swProcess;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

