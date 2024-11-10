using System;
using System.Windows;


namespace YourNamespace
{
    public partial class WelcomePage : Window
    {
        public WelcomePage()
        {
            InitializeComponent();
        }


        // Admin button click handler
        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the AdminLogin window
            AdminLogin adminLoginWindow = new AdminLogin();
            adminLoginWindow.Show();
            this.Close(); // Optionally close the WelcomePage window
        }


        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new LoginPage("User");
            loginPage.Show();
            this.Hide(); // Hide the welcome page
        }
    }
}
