using System.Windows;
using System.Windows.Controls;

namespace YourNamespace
{
    public partial class EditMembersPage : Page
    {
        public EditMembersPage()
        {
            InitializeComponent();
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            string memberId = MemberIdTextBox.Text;
            string name = NameTextBox.Text;
            string email = EmailTextBox.Text;
            string phone = PhoneTextBox.Text;

            if (string.IsNullOrWhiteSpace(memberId) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Member ID and Name are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show($"Member Updated!\nID: {memberId}\nName: {name}\nEmail: {email}\nPhone: {phone}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            MemberIdTextBox.Clear();
            NameTextBox.Clear();
            EmailTextBox.Clear();
            PhoneTextBox.Clear();
        }
    }
}
