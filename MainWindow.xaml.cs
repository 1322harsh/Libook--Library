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

      
        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new LoginPage("Admin");
            loginPage.Show();
            this.Hide(); // Hide the welcome page
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new LoginPage("User");
            loginPage.Show();
            this.Hide(); // Hide the welcome page
        }
    }
}
