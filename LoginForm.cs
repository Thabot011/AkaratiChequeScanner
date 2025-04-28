using Newtonsoft.Json;
using SimpleScan;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AkaratiCheckScanner
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }



        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Missing user name or password");
                return;
            }

            var token = await LoginApiAsync(textBox1.Text, textBox2.Text);

            if (string.IsNullOrEmpty(token))
            {
                MessageBox.Show("Inavlid user name or password");
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
                var baseUrl = ConfigurationManager.AppSettings["LoginBaseUrl"];
                string apiUrl = $"{baseUrl}v1/token";

                var data = new
                {
                    username,
                    password,
                    identifier = "mL5WislMnggrmNaw4/h3pg=="
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
                // Handle API errors (e.g., network issues, invalid response, etc.)
                MessageBox.Show($"Error fetching suggestions: {ex.Message}");
            }
            return token;
        }
    }
}
