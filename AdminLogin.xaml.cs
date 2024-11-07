using System.Windows;
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
            string username = Username.Text;
            string password = Password.Password;

            // Example logic: Validate username and password (you can replace it with actual validation)
            if (username == "admin" && password == "password123")
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                // Navigate to the next page or perform further actions
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         
            this.Close(); // Optionally close the WelcomePage window
        }

        
    }
}
