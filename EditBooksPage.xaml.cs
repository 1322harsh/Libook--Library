using System.Windows;
using System.Windows.Controls;

namespace YourNamespace
{
    public partial class EditBooksPage : Page
    {
        public EditBooksPage()
        {
            InitializeComponent();
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            string bookId = BookIdTextBox.Text;
            string title = TitleTextBox.Text;
            string author = AuthorTextBox.Text;
            string isbn = ISBNTextBox.Text;

            if (string.IsNullOrWhiteSpace(bookId) || string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Book ID and Title are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show($"Book Updated!\nID: {bookId}\nTitle: {title}\nAuthor: {author}\nISBN: {isbn}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            BookIdTextBox.Clear();
            TitleTextBox.Clear();
            AuthorTextBox.Clear();
            ISBNTextBox.Clear();
        }
    }
}
