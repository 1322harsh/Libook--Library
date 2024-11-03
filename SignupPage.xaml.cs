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
            // Validate the input fields
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check if passwords match
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Passwords do not match.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Here you would typically hash the password and store user information in the database
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password; // You should hash this before storing

            // Simulate user registration (you can replace this with actual database logic)
            bool registrationSuccessful = RegisterUser(username, email, password);

            if (registrationSuccessful)
            {
                MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                // Optionally navigate to the login page
                this.Close(); // Close the signup page
            }
            else
            {
                MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
