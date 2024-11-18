using System.Data.SqlClient;
using System;
using System.Windows;

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
            string usernameOrEmail = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(usernameOrEmail) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your UserID/Email and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Database connection string
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Users\270449050\OneDrive - UP Education\c sharp\Libook-(Library\Libook.mdf;Integrated Security=True";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT COUNT(1) 
                FROM Users 
                WHERE (Email = @Input OR CAST(UserID AS NVARCHAR) = @Input) 
                  AND Password = @Password";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Input", usernameOrEmail);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count == 1)
                    {
                        MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Navigate to the main page or dashboard
                       UserPage mainPage = new UserPage("User",usernameOrEmail);
                        mainPage.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid UserID/Email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("A database error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
