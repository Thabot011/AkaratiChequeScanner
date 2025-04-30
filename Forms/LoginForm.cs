using Newtonsoft.Json;
using SimpleScan;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AkaratiCheckScanner
{
    public partial class LoginForm : Form
    {
        protected override void OnLoad(EventArgs e)
        {
            SetRoundedCorners(loginpanel1, 30);
        }
        public LoginForm()
        {
            InitializeComponent();
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
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Missing user name or password");
                ResetLoading();
                return;
            }

            var token = await LoginApiAsync(textBox1.Text, textBox2.Text);
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

        private async Task<string> LoginApiAsync(string username, string password)
        {
            string token = string.Empty;
            try
            {
                // Replace with your actual API endpoint
                var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
                string apiUrl = $"{baseUrl}/v1/token";

                var data = new
                {
                    username,
                    password,
                    identifier = "KS6YL+e8wNw3VFhRXx7ssQ=="
                };

                string jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the API request and get the response
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();  // Throws an exception if the status code is not successful

                    // Read the API response
                    token = await response.Content.ReadAsStringAsync();
                }

            }

            catch (Exception ex)
            {
                ResetLoading();
            }
            return token;
        }


        private void ResetLoading()
        {
            this.Enabled = true;
            this.Visible = true;
        }
    }
}
