using System.Windows;
using System;
using System.Data.SqlClient;
using System.Configuration;
using YourNamespace;

namespace YourNamespace // Make sure this matches the namespace in your project
{
    public partial class AdminLogin : Window  // The class name should match the XAML file's class name
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        // Event handler for Login button click
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Directly use your connection string here
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Libook;Integrated Security=True;";

            // Parse UserID input and validate it
            if (!int.TryParse(Username.Text, out int userId))
            {
                MessageBox.Show("Please enter a valid UserID.");
                return;
            }

            // Capture the password
            string password = Password.Password;

            // Define the SQL query
            string query = "SELECT COUNT(1) FROM Users WHERE UserID = @UserID AND Password = @Password AND Role = 'Admin'";

            // Execute the query
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    if (count == 1) // if there's exactly one matching admin user
                    {
                        MessageBox.Show("Admin login successful!");
                        // Redirect to the admin dashboard or main window here
                    }
                    else
                    {
                        MessageBox.Show("Invalid admin credentials. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            WelcomePage welcomepage = new WelcomePage();
            welcomepage.Show();
            this.Hide(); // Hide the login page
        }

        
    }
}
