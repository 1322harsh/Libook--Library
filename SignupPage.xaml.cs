using System.Windows;
using System;
using System.Data.SqlClient;
using System.Configuration;

namespace YourNamespace
{
    public partial class SignupPage : Window
    {
        private string userType;

        public SignupPage(string userType)
        {
            InitializeComponent();
            this.userType = userType;
            Title = $"{userType} Sign Up";
        }

        // Event handler for the Sign Up button
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Libook;Integrated Security=True;";

            string name = User_name.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string securityQuestion = SecurityQuestionComboBox.SelectedItem.ToString();
            string answer = SecurityAnswerTextBox.Text;

            // Show message with randomly generated UserID (for display only)
            Random random = new Random();
            int userId = random.Next(1000, 9999);
            string message = $"UserID: {userId}\nName: {name}\nEmail: {email}\nSecurity Question: {securityQuestion}\nAnswer: {answer}\n\n" +
                             "Do you want to proceed with these details?";

            MessageBoxResult result = MessageBox.Show(message, "Confirm Signup", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                string checkUserQuery = "SELECT COUNT(1) FROM Users WHERE Email = @Email";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection);
                    checkUserCommand.Parameters.AddWithValue("@Email", email);

                    try
                    {
                        connection.Open();
                        int userExists = (int)checkUserCommand.ExecuteScalar();

                        if (userExists > 0)
                        {
                            MessageBox.Show("This email is already registered. Please try again.");
                            return;
                        }

                        // Insert query WITHOUT UserID column (let SQL Server handle it)
                        string query = "INSERT INTO Users (Name, Email, Password, SecurityQuestion, SecurityAnswer, Role) " +
                                       "VALUES (@Name, @Email, @Password, @SecurityQuestion, @SecurityAnswer, @Role);" +
                                       "SELECT SCOPE_IDENTITY();"; // Get the generated UserID

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@SecurityQuestion", securityQuestion);
                        command.Parameters.AddWithValue("@SecurityAnswer", answer);
                        command.Parameters.AddWithValue("@Role", "User");  // Default role

                        // Retrieve the generated UserID
                        int newUserId = Convert.ToInt32(command.ExecuteScalar());

                        // Show the UserID
                        MessageBox.Show($"User signup successful!\nYour UserID is: {newUserId}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Signup process canceled.");
            }
        }



        // Event handler for the Login button
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Close the signup page and open the login page
            LoginPage loginPage = new LoginPage(userType);
            loginPage.Show();
            this.Close(); // Close the signup page
        }

        // Dummy method to simulate user registration
        private bool RegisterUser(string username, string email, string password)
        {
            // TODO: Implement actual user registration logic (e.g., save to database)
            return true; // For demonstration, we'll just return true
        }
    }
}