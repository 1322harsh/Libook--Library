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
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                SecurityQuestionComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(AnswerTextBox.Text) ||
                string.IsNullOrWhiteSpace(NewPasswordBox.Password))
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

            string emailOrUserId = EmailTextBox.Text;
            bool isValidEmailOrUserId = IsValidEmail(emailOrUserId) || int.TryParse(emailOrUserId, out _);

            if (!isValidEmailOrUserId)
            {
                MessageBox.Show("Please enter a valid Email or User ID.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string securityQuestion = SecurityQuestionComboBox.SelectedItem.ToString();
            string answer = AnswerTextBox.Text;
            string newPassword = NewPasswordBox.Password;

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
                MessageBox.Show("Password reset failed. Please check your Email/UserID, security question, or answer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private bool ResetUserPassword(string emailOrUserId, string securityQuestion, string answer, string newPassword)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\Users\\270449050\\OneDrive - UP Education\\c sharp\\Libook-(Library\\Libook.mdf\";Integrated Security=True";

            bool isEmail = emailOrUserId.Contains("@"); // Determine if input is email

            // SQL query with conditional checks for Email or UserID
            string checkQuery = isEmail
                ? "SELECT COUNT(1) FROM Users WHERE Email = @Email AND SecurityQuestion = @SecurityQuestion AND SecurityAnswer = @SecurityAnswer"
                : "SELECT COUNT(1) FROM Users WHERE UserID = @UserID AND SecurityQuestion = @SecurityQuestion AND SecurityAnswer = @SecurityAnswer";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);

                if (isEmail)
                {
                    checkCommand.Parameters.AddWithValue("@Email", emailOrUserId);
                }
                else
                {
                    checkCommand.Parameters.AddWithValue("@UserID", Convert.ToInt32(emailOrUserId));
                }

                checkCommand.Parameters.AddWithValue("@SecurityQuestion", securityQuestion);
                checkCommand.Parameters.AddWithValue("@SecurityAnswer", answer);

                try
                {
                    connection.Open();
                    int userExists = (int)checkCommand.ExecuteScalar();

                    if (userExists == 0)
                    {
                        // No matching user found
                        return false;
                    }

                    // Query to update password
                    string updateQuery = isEmail
                        ? "UPDATE Users SET Password = @NewPassword WHERE Email = @Email"
                        : "UPDATE Users SET Password = @NewPassword WHERE UserID = @UserID";

                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);

                    if (isEmail)
                    {
                        updateCommand.Parameters.AddWithValue("@Email", emailOrUserId);
                    }
                    else
                    {
                        updateCommand.Parameters.AddWithValue("@UserID", Convert.ToInt32(emailOrUserId));
                    }

                    updateCommand.Parameters.AddWithValue("@NewPassword", newPassword);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    // If one row is updated, password reset is successful
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
