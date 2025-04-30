using AkaratiCheckScanner;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace SimpleScan
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            GlobalSetting.BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}