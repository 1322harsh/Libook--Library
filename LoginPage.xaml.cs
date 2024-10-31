using System.Windows;
using System.Windows.Controls;

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
            // Logic for authentication (simplified for example)
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            // Assume login is successful for demonstration
            var userPage = new UserPage(userType, username);
            userPage.Show();
            this.Hide(); // Hide the login page
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Forgot Password functionality not implemented.");
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sign Up functionality not implemented.");
        }
    }
}
