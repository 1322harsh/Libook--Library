﻿using System.Windows;

namespace YourNamespace
{
    public partial class LoginPage : Window
    {
        private string userType;

        public LoginPage(string userType)
        {
            InitializeComponent();
            this.userType = userType;
            Title = $"{userType} Login";
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            // Assume login is successful for demonstration
            string userType = "standard"; // Set or retrieve userType as needed
            UserPage userPage = new UserPage(userType, username);
            userPage.Show();
            this.Hide(); // Hide the login page
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordPage forgotpassword = new ForgotPasswordPage(userType);
            forgotpassword.Show();
            this.Hide(); // Hide the login page
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            SignupPage signupPage = new SignupPage(userType);
            signupPage.Show();
            this.Hide(); // Hide the login page
        }

        // Back button click event
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Show the MainWindow and close the login window
            WelcomePage mainWindow = new WelcomePage();
            mainWindow.Show();
            this.Close(); // Close the login page
        }
    }
}
