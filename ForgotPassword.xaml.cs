
using System.Windows;

namespace YourNamespace
{
    public partial class ForgotPasswordPage : Window
    {
        private string userType;

        public ForgotPasswordPage(string userType)
        {
            InitializeComponent();
            this.userType = userType;
            Title = $"{userType} - Forgot Password";
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (SecurityQuestionComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(AnswerTextBox.Text) ||
                string.IsNullOrWhiteSpace(NewPasswordBox.Password))
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Dummy password reset logic; replace with actual implementation
            bool resetSuccessful = ResetUserPassword(AnswerTextBox.Text, NewPasswordBox.Password);

            if (resetSuccessful)
            {
                MessageBox.Show("Password reset successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginPage loginPage = new LoginPage(userType);
                loginPage.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Password reset failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ResetUserPassword(string answer, string newPassword)
        {
            // TODO: Implement actual password reset logic (e.g., validate answer and update password in database)
            return true; // For demonstration, we return true
        }
    }
}
