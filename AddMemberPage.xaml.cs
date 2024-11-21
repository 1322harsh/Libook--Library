using System.Windows;
using System.Windows.Controls;

namespace YourNamespace
{
    public partial class AddMembersPage : Page
    {
        public AddMembersPage()
        {
            InitializeComponent();
        }

        private void SaveMember(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string email = EmailTextBox.Text;
            string phone = PhoneTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

        }
    }
}
