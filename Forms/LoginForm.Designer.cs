using System.Windows.Forms;

namespace AkaratiCheckScanner
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.usernamelabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.passwordlabel = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.loginbtn = new System.Windows.Forms.Button();
            this.loginpanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.loginpanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(101, 53);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(175, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // usernamelabel
            // 
            this.usernamelabel.AutoSize = true;
            this.usernamelabel.Location = new System.Drawing.Point(16, 193);
            this.usernamelabel.Name = "usernamelabel";
            this.usernamelabel.Size = new System.Drawing.Size(76, 16);
            this.usernamelabel.TabIndex = 3;
            this.usernamelabel.Text = "User Name";
            // 
            // label3
            // 
            this.label3.AccessibleDescription = "";
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label3.Location = new System.Drawing.Point(144, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "Account Login";
            // 
            // username
            // 
            this.username.BackColor = System.Drawing.SystemColors.HighlightText;
            this.username.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.username.Location = new System.Drawing.Point(19, 212);
            this.username.Multiline = true;
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(338, 30);
            this.username.TabIndex = 0;
            // 
            // passwordlabel
            // 
            this.passwordlabel.AutoSize = true;
            this.passwordlabel.Location = new System.Drawing.Point(16, 252);
            this.passwordlabel.Name = "passwordlabel";
            this.passwordlabel.Size = new System.Drawing.Size(67, 16);
            this.passwordlabel.TabIndex = 4;
            this.passwordlabel.Text = "Password";
            // 
            // password
            // 
            this.password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.password.Location = new System.Drawing.Point(19, 271);
            this.password.Multiline = true;
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(338, 30);
            this.password.TabIndex = 1;
            // 
            // loginbtn
            // 
            this.loginbtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.loginbtn.Location = new System.Drawing.Point(101, 327);
            this.loginbtn.Name = "loginbtn";
            this.loginbtn.Size = new System.Drawing.Size(175, 63);
            this.loginbtn.TabIndex = 2;
            this.loginbtn.Text = "Login";
            this.loginbtn.UseVisualStyleBackColor = false;
            this.loginbtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // loginpanel
            // 
            this.loginpanel.BackColor = System.Drawing.Color.White;
            this.loginpanel.Controls.Add(this.loginbtn);
            this.loginpanel.Controls.Add(this.password);
            this.loginpanel.Controls.Add(this.passwordlabel);
            this.loginpanel.Controls.Add(this.username);
            this.loginpanel.Controls.Add(this.label3);
            this.loginpanel.Controls.Add(this.usernamelabel);
            this.loginpanel.Controls.Add(this.pictureBox1);
            this.loginpanel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.loginpanel.Location = new System.Drawing.Point(1100, 171);
            this.loginpanel.Name = "loginpanel";
            this.loginpanel.Size = new System.Drawing.Size(371, 422);
            this.loginpanel.TabIndex = 7;
            // 
            // LoginForm
            // 
            this.AutoSize = true;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1586, 705);
            this.Controls.Add(this.loginpanel);
            this.Name = "LoginForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.loginpanel.ResumeLayout(false);
            this.loginpanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private Label usernamelabel;
        private Label label3;
        private TextBox username;
        private Label passwordlabel;
        private TextBox password;
        private Button loginbtn;
        private Panel loginpanel;
    }
}
