using System.Windows;

namespace YourNamespace
{
    public partial class UserPage : Window
    {
        public UserPage(string userType, string username)
        {
            InitializeComponent();
            WelcomeTextBlock.Text = $"Welcome {userType}: {username}!";
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            var welcomePage = new WelcomePage();
            welcomePage.Show();
            this.Close(); // Close the user page
        }
    }
}
