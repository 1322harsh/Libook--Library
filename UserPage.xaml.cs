using System.Windows;
using System.Windows.Controls;

namespace YourNamespace
{
    public partial class UserPage : Window
    {
        public UserPage(string userType, string username)
        {
            InitializeComponent();

            // Setting the title based on the user type
            Title = $"{userType} User Page";

          
        
    }

    // Load all available books
    private void LoadAllBooks()
        {
            // Sample data; replace with actual data retrieval logic
            AddBookToAllBooksList("1234", "Available");
            AddBookToAllBooksList("5678", "Borrowed");
        }

        // Load user's borrowed books
        private void LoadMyBooks()
        {
            // Sample data; replace with actual data retrieval logic
            AddBookToMyBooksList("5678", "Borrowed");
        }

        // Add a book to the All Books list with buttons for each action
        private void AddBookToAllBooksList(string bookId, string status)
        {
            StackPanel bookPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5), Background = System.Windows.Media.Brushes.White };

            // Book information
            TextBlock bookIdText = new TextBlock { Text = $"Book ID: {bookId}", Width = 100, Foreground = System.Windows.Media.Brushes.Blue };
            TextBlock statusText = new TextBlock { Text = $"Status: {status}", Width = 100, Foreground = System.Windows.Media.Brushes.Blue };

            // Buttons
            Button prebookButton = new Button { Content = "Prebook", Width = 80, Margin = new Thickness(5, 0, 0, 0), Background = System.Windows.Media.Brushes.Orange, Foreground = System.Windows.Media.Brushes.White };
            prebookButton.Click += (sender, e) => PrebookBook(bookId);

            Button issueButton = new Button { Content = "Issue", Width = 80, Margin = new Thickness(5, 0, 0, 0), Background = System.Windows.Media.Brushes.Orange, Foreground = System.Windows.Media.Brushes.White };
            issueButton.Click += (sender, e) => IssueBook(bookId);

            Button returnButton = new Button { Content = "Return", Width = 80, Margin = new Thickness(5, 0, 0, 0), Background = System.Windows.Media.Brushes.Orange, Foreground = System.Windows.Media.Brushes.White };
            returnButton.Click += (sender, e) => ReturnBook(bookId);

            // Add components to panel
            bookPanel.Children.Add(bookIdText);
            bookPanel.Children.Add(statusText);
            bookPanel.Children.Add(prebookButton);
            bookPanel.Children.Add(issueButton);
            bookPanel.Children.Add(returnButton);

            // Add panel to All Books list (Assuming an ItemsControl or StackPanel is named AllBooksListPanel in XAML)
            //AllBooksListPanel.Children.Add(bookPanel);
        }

        // Add a book to My Books list with a Return button
        private void AddBookToMyBooksList(string bookId, string status)
        {
            StackPanel bookPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5), Background = System.Windows.Media.Brushes.White };

            TextBlock bookIdText = new TextBlock { Text = $"Book ID: {bookId}", Width = 100, Foreground = System.Windows.Media.Brushes.Blue };
            TextBlock statusText = new TextBlock { Text = $"Status: {status}", Width = 100, Foreground = System.Windows.Media.Brushes.Blue };

            Button returnButton = new Button { Content = "Return", Width = 80, Margin = new Thickness(5, 0, 0, 0), Background = System.Windows.Media.Brushes.Orange, Foreground = System.Windows.Media.Brushes.White };
            returnButton.Click += (sender, e) => ReturnBook(bookId);

            bookPanel.Children.Add(bookIdText);
            bookPanel.Children.Add(statusText);
            bookPanel.Children.Add(returnButton);

           // MyBooksListPanel.Children.Add(bookPanel); // Assuming MyBooksListPanel is a StackPanel in XAML
        }

        // Prebook book event handler
        private void PrebookBook(string bookId)
        {
            MessageBox.Show($"Prebooking book with ID: {bookId}");
            // Add prebooking logic here
        }

        // Issue book event handler
        private void IssueBook(string bookId)
        {
            MessageBox.Show($"Issuing book with ID: {bookId}");
            // Add issuing logic here
        }

        // Return book event handler
        private void ReturnBook(string bookId)
        {
            MessageBox.Show($"Returning book with ID: {bookId}");
            // Add return logic here
        }

        // Logout button event handler
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            WelcomePage welcomepage = new WelcomePage();
           welcomepage.Show();
            this.Hide(); // Hide the login page

            
        }
    }
}
