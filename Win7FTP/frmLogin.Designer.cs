namespace Win7FTP
{
    partial class frmLogin
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
            this.pnlNonTransparent = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHostName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.pnlNonTransparent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlNonTransparent
            // 
            this.pnlNonTransparent.BackColor = System.Drawing.SystemColors.Control;
            this.pnlNonTransparent.Controls.Add(this.label1);
            this.pnlNonTransparent.Controls.Add(this.txtHostName);
            this.pnlNonTransparent.Controls.Add(this.lblUserName);
            this.pnlNonTransparent.Controls.Add(this.btnLogIn);
            this.pnlNonTransparent.Controls.Add(this.lblPassword);
            this.pnlNonTransparent.Controls.Add(this.txtPassword);
            this.pnlNonTransparent.Controls.Add(this.txtUserName);
            this.pnlNonTransparent.Location = new System.Drawing.Point(0, 0);
            this.pnlNonTransparent.Name = "pnlNonTransparent";
            this.pnlNonTransparent.Size = new System.Drawing.Size(298, 131);
            this.pnlNonTransparent.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(3, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Host Name:";
            // 
            // txtHostName
            // 
            this.txtHostName.Location = new System.Drawing.Point(69, 63);
            this.txtHostName.Name = "txtHostName";
            this.txtHostName.Size = new System.Drawing.Size(211, 20);
            this.txtHostName.TabIndex = 4;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.BackColor = System.Drawing.Color.Transparent;
            this.lblUserName.Location = new System.Drawing.Point(3, 14);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(60, 13);
            this.lblUserName.TabIndex = 0;
            this.lblUserName.Text = "UserName:";
            // 
            // btnLogIn
            // 
            this.btnLogIn.BackColor = System.Drawing.Color.Transparent;
            this.btnLogIn.Location = new System.Drawing.Point(205, 89);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(75, 23);
            this.btnLogIn.TabIndex = 5;
            this.btnLogIn.Text = "Login";
            this.btnLogIn.UseVisualStyleBackColor = false;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.Location = new System.Drawing.Point(3, 40);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(69, 37);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(211, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(69, 11);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(211, 20);
            this.txtUserName.TabIndex = 2;
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 136);
            this.Controls.Add(this.pnlNonTransparent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLogin";
            this.Text = "Login ";
            this.pnlNonTransparent.ResumeLayout(false);
            this.pnlNonTransparent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNonTransparent;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHostName;


    }
}

