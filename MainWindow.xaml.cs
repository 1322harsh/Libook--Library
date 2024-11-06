using LibookApp;
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


        private void AdminLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the AdminLogin window
            AdminLogin adminLoginWindow = new AdminLogin();
            adminLoginWindow.Show();
        } // Show the AdminLogin window

            private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new LoginPage("User");
            loginPage.Show();
            this.Hide(); // Hide the welcome page
        }
    }
}
