using System;
using System.Data.SqlClient;
using System.Windows;

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
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\Users\\270449050\\OneDrive - UP Education\\c sharp\\Libook-(Library\\Libook.mdf\";Integrated Security=True";

            string name = User_name.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string securityQuestion = SecurityQuestionComboBox.SelectedItem?.ToString();
            string answer = SecurityAnswerTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(securityQuestion) || string.IsNullOrWhiteSpace(answer))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

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

                    // Insert query (UserID will auto-increment)
                    string query = "INSERT INTO Users (Name, Email, Password, SecurityQuestion, SecurityAnswer, Role) " +
                                   "VALUES (@Name, @Email, @Password, @SecurityQuestion, @SecurityAnswer, @Role);" +
                                   "SELECT SCOPE_IDENTITY();"; // Get the generated UserID

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@SecurityQuestion", securityQuestion);
                    command.Parameters.AddWithValue("@SecurityAnswer", answer);
                    command.Parameters.AddWithValue("@Role", "User"); // Default role

                    // Retrieve the generated UserID
                    object result = command.ExecuteScalar();
                    int newUserId = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    if (newUserId == 0)
                    {
                        MessageBox.Show("User signup successful, but the UserID could not be retrieved.");
                    }
                    else
                    {
                        MessageBox.Show($"User signup successful!\nYour UserID is: {newUserId}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
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
    }
}
