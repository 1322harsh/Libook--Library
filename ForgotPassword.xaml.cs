using System;
using System.Data.SqlClient;
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

            // Populate security questions when the page is loaded
            PopulateSecurityQuestions();
        }

        private void PopulateSecurityQuestions()
        {
            // List of predefined security questions
            var securityQuestions = new string[]
            {
                "What is your mother's maiden name?",
                "What was the name of your first pet?",
                "What is your favorite color?",
                "In what city were you born?",
                "What was the name of your elementary school?"
            };

            // Add questions to ComboBox
            foreach (var question in securityQuestions)
            {
                SecurityQuestionComboBox.Items.Add(question);
            }

            // Optionally select the first question by default
            if (SecurityQuestionComboBox.Items.Count > 0)
            {
                SecurityQuestionComboBox.SelectedIndex = 0;
            }
        }
        // Back button click event
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Show the MainWindow and close the login window
           LoginPage mainWindow = new LoginPage("User");
            mainWindow.Show();
            this.Close(); // Close the login page
        }
          
        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            // Check if required fields are filled
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                SecurityQuestionComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(AnswerTextBox.Text) ||
                string.IsNullOrWhiteSpace(NewPasswordBox.Password))
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string emailOrUserId = EmailTextBox.Text; // Capture email or UserID
            string securityQuestion = SecurityQuestionComboBox.SelectedItem.ToString();
            string answer = AnswerTextBox.Text;
            string newPassword = NewPasswordBox.Password;

            // Call the method to check the user's security question and answer, and reset the password
            bool resetSuccessful = ResetUserPassword(emailOrUserId, securityQuestion, answer, newPassword);

            if (resetSuccessful)
            {
                MessageBox.Show("Password reset successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginPage loginPage = new LoginPage(userType);
                loginPage.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Password reset failed. Please check your email, security question, or answer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ResetUserPassword(string emailOrUserId, string securityQuestion, string answer, string newPassword)
        {
            // Define your connection string
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Libook;Integrated Security=True;";

            // SQL query to check if the email and security question answer match the user's record
            string checkQuery = "SELECT COUNT(1) FROM Users WHERE (Email = @Email OR UserID = @UserID) AND SecurityQuestion = @SecurityQuestion AND SecurityAnswer = @SecurityAnswer";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Email", emailOrUserId);
                checkCommand.Parameters.AddWithValue("@UserID", emailOrUserId);
                checkCommand.Parameters.AddWithValue("@SecurityQuestion", securityQuestion);
                checkCommand.Parameters.AddWithValue("@SecurityAnswer", answer);

                try
                {
                    connection.Open();
                    int userExists = (int)checkCommand.ExecuteScalar();

                    if (userExists == 0)
                    {
                        // No matching user found, return false (reset failed)
                        return false;
                    }

                    // Update the password for the user if the answer is correct
                    string updateQuery = "UPDATE Users SET Password = @NewPassword WHERE (Email = @Email OR UserID = @UserID)";

                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@NewPassword", newPassword); // In a real app, hash this password
                    updateCommand.Parameters.AddWithValue("@Email", emailOrUserId);
                    updateCommand.Parameters.AddWithValue("@UserID", emailOrUserId);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    // If rowsAffected is 1, the password was updated successfully
                    return rowsAffected == 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while resetting the password: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
    }
}
