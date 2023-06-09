namespace WinAgent
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lstvApps = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbComputerName = new System.Windows.Forms.Label();
            this.lbOSinfo = new System.Windows.Forms.Label();
            this.btnPost = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lstvApps, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lstvApps
            // 
            this.lstvApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvApps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstvApps.FullRowSelect = true;
            this.lstvApps.GridLines = true;
            this.lstvApps.HideSelection = false;
            this.lstvApps.Location = new System.Drawing.Point(3, 103);
            this.lstvApps.MultiSelect = false;
            this.lstvApps.Name = "lstvApps";
            this.lstvApps.Size = new System.Drawing.Size(794, 344);
            this.lstvApps.TabIndex = 0;
            this.lstvApps.UseCompatibleStateImageBehavior = false;
            this.lstvApps.View = System.Windows.Forms.View.Details;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnPost);
            this.panel1.Controls.Add(this.lbComputerName);
            this.panel1.Controls.Add(this.lbOSinfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 94);
            this.panel1.TabIndex = 1;
            // 
            // lbComputerName
            // 
            this.lbComputerName.AutoSize = true;
            this.lbComputerName.Location = new System.Drawing.Point(18, 49);
            this.lbComputerName.Name = "lbComputerName";
            this.lbComputerName.Size = new System.Drawing.Size(52, 13);
            this.lbComputerName.TabIndex = 1;
            this.lbComputerName.Text = "PC Name";
            // 
            // lbOSinfo
            // 
            this.lbOSinfo.AutoSize = true;
            this.lbOSinfo.Location = new System.Drawing.Point(18, 18);
            this.lbOSinfo.Name = "lbOSinfo";
            this.lbOSinfo.Size = new System.Drawing.Size(42, 13);
            this.lbOSinfo.TabIndex = 0;
            this.lbOSinfo.Text = "OS info";
            // 
            // btnPost
            // 
            this.btnPost.Location = new System.Drawing.Point(694, 58);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(75, 23);
            this.btnPost.TabIndex = 2;
            this.btnPost.Text = "Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Windows Agent";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView lstvApps;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbOSinfo;
        private System.Windows.Forms.Label lbComputerName;
        private System.Windows.Forms.Button btnPost;
    }
}

