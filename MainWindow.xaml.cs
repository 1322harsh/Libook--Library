using System.Windows;
using System.Windows.Controls;

namespace YourNamespace
{
    public partial class AdminScreen : Window
    {
        public AdminScreen()
        {
            InitializeComponent();
        }
        private void NavigateToAddBooksPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AddBooksPage());
        }

        private void NavigateToAddMembersPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AddMembersPage());
        }

        private void NavigateToEditMembersPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EditMembersPage());
        }

        private void NavigateToEditBooksPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EditBooksPage());
        }
    }
}
