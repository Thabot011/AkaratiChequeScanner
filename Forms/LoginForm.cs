using SimpleScan;
using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace AkaratiCheckScanner
{
    public partial class LoginForm : Form
    {
        protected override void OnLoad(EventArgs e)
        {
            SetRoundedCorners(loginpanel, 30);
        }
        public LoginForm()
        {
            InitializeComponent();
            this.AcceptButton = loginbtn;
        }
        private void SetRoundedCorners(Control control, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }



        private async void button1_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Text))
            {
                MessageBox.Show("Missing user name or password");
                ResetLoading();
                return;
            }

            var token = await new AuthService().LoginAsync(username.Text, password.Text);
            if (string.IsNullOrEmpty(token))
            {
                MessageBox.Show("Inavlid user name or password");
                ResetLoading();
                return;
            }

            GlobalSetting.AuthToken = token;


            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }


        private void ResetLoading()
        {
            this.Enabled = true;
            this.Visible = true;
        }
    }
}
