using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace YourNamespace
{
    public partial class UserPage : Window
    {
        private int currentUserId;  // Store the logged-in user's UserId as an integer

        // Define your connection string here
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Users\270449050\OneDrive - UP Education\c sharp\Libook-(Library\Libook.mdf"";Integrated Security=True";

        public UserPage(string userType, string userId)
        {
            InitializeComponent();
            Title = $"{userType} User Page";
            currentUserId = int.Parse(userId);  // Convert UserId (string) to integer
        }

        // Book class to represent a book record
        public class Book
        {
            public int BookID { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Genre { get; set; }
            public int YearPublished { get; set; }
            public DateTime? DueDate { get; set; }  // Nullable DateTime for DueDate
            public bool IsIssued { get; set; }  // Tracks if the book is issued
        }

        private List<Book> allBooks = new List<Book>();
        private List<Book> myBooks = new List<Book>();

        // Load All Books for the "All Books" button
        private void LoadAllBooksFromDatabase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT b.BookID, b.Title, b.Author, b.Genre, b.YearPublished, t.DueDate, " +
                                   "CASE WHEN t.TransactionType = 'Issue' THEN 1 ELSE 0 END AS IsIssued " +
                                   "FROM Books b LEFT JOIN Transactions t ON b.BookID = t.BookID AND t.TransactionType = 'Issue' " +
                                   "WHERE t.UserId = @UserId OR t.UserId IS NULL";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserId", currentUserId);  // Pass UserId as integer
                    SqlDataReader reader = command.ExecuteReader();

                    allBooks.Clear();

                    while (reader.Read())
                    {
                        allBooks.Add(new Book
                        {
                            BookID = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            Genre = reader.GetString(3),
                            YearPublished = reader.GetInt32(4),
                            DueDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                            IsIssued = reader.GetInt32(6) == 1  // Check if the book is issued
                        });
                    }

                    AllBooksListView.ItemsSource = allBooks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}");
            }
        }

        // Handle "My Books" button click
        private void MyBooksButton_Click(object sender, RoutedEventArgs e)
        {
            LoadMyBooksFromDatabase();  // Load the books borrowed by the user
            MyBooksListView.Visibility = Visibility.Visible;  // Make sure it's visible after loading
        }

        // Load books borrowed by the logged-in user
        private void LoadMyBooksFromDatabase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to select books borrowed by the current user and that have not been returned yet
                    string query = "SELECT b.BookID, b.Title, b.Author, b.Genre, b.YearPublished, t.DueDate " +
                                   "FROM Books b INNER JOIN Transactions t ON b.BookID = t.BookID " +
                                   "WHERE t.UserId = @UserId AND t.TransactionType = 'Issue' AND t.DueDate IS NULL";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserId", currentUserId);  // Pass UserId as integer
                    SqlDataReader reader = command.ExecuteReader();

                    myBooks.Clear();  // Clear the previous list

                    // Read the data from the reader and populate the myBooks list
                    while (reader.Read())
                    {
                        myBooks.Add(new Book
                        {
                            BookID = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            Genre = reader.GetString(3),
                            YearPublished = reader.GetInt32(4),
                            DueDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5)  // Get the DueDate for borrowed books
                        });
                    }

                    // Set the ItemsSource for the ListView to display the books
                    MyBooksListView.ItemsSource = myBooks;
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the database interaction
                MessageBox.Show($"Error loading borrowed books: {ex.Message}");
            }
        }

        // Handle "Prebook" button click
        private void PrebookBookButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedBook = button?.DataContext as Book;

            if (selectedBook != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to prebook the book '{selectedBook.Title}'?",
                                                          "Prebook Book",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // Insert prebooking record into Transactions table
                            string query = "INSERT INTO Transactions (BookID, UserId, TransactionType, TransactionDate) " +
                                           "VALUES (@BookID, @UserId, 'Prebook', GETDATE())";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@BookID", selectedBook.BookID);
                            command.Parameters.AddWithValue("@UserId", currentUserId);  // Pass the logged-in UserId
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Book prebooked successfully!");
                        LoadAllBooksFromDatabase(); // Refresh book list to reflect the change
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error prebooking book: {ex.Message}");
                    }
                }
            }
        }

        // Handle "Issue" button click
        private void IssueBookButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedBook = button?.DataContext as Book;

            if (selectedBook != null && !selectedBook.IsIssued)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to issue the book '{selectedBook.Title}'?",
                                                          "Issue Book",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Proceed with the book issuing logic
                    // (Code to handle issuing the book would go here)
                }
            }
            else
            {
                MessageBox.Show("This book is already issued. You can prebook it instead.");
            }
        }

        // Handle "Return" button click in My Books List
        private void ReturnBookButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedBook = button?.DataContext as Book;

            if (selectedBook != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to return the book '{selectedBook.Title}'?",
                                                          "Return Book",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // Update book status to 'Available' in the Books table
                            string updateBookQuery = "UPDATE Books SET Status = 'Available' WHERE BookID = @BookID";
                            SqlCommand command = new SqlCommand(updateBookQuery, connection);
                            command.Parameters.AddWithValue("@BookID", selectedBook.BookID);
                            command.ExecuteNonQuery();

                            // Update return date in the Transactions table
                            string updateTransactionQuery = "UPDATE Transactions SET DueDate = GETDATE() WHERE BookID = @BookID AND UserId = @UserId";
                            command = new SqlCommand(updateTransactionQuery, connection);
                            command.Parameters.AddWithValue("@BookID", selectedBook.BookID);
                            command.Parameters.AddWithValue("@UserId", currentUserId);
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Book returned successfully!");
                        LoadMyBooksFromDatabase(); // Refresh My Books list
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error returning book: {ex.Message}");
                    }
                }
            }
        }

        // Handle "All Books" button click
        private void AllBooksButton_Click(object sender, RoutedEventArgs e)
        {
            // Load All Books
            LoadAllBooksFromDatabase(); // Refresh the All Books list
            AllBooksListView.Visibility = Visibility.Visible;  // Show All Books list
        }

        // SelectionChanged event handler for "AllBooksListView"
        private void AllBooksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllBooksListView.SelectedItem is Book selectedBook)
            {
                // Handle selection logic here, for example:
                MessageBox.Show($"You selected the book: {selectedBook.Title}");
            }
        }

        // SelectionChanged event handler for "MyBooksListView"
        private void MyBooksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyBooksListView.SelectedItem is Book selectedBook)
            {
                // Handle selection logic here, for example:
                MessageBox.Show($"You selected the book: {selectedBook.Title}");
            }
        }
    }
}
