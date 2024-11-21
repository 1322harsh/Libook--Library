using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace YourNamespace
{
    public partial class UserPage : Window
    {
        private string currentUserId = "CurrentLoggedUser"; // This should be set during the login process.
  
        public UserPage(string userType, string UserId)
        {
            InitializeComponent();
            Title = $"{userType} User Page";
            currentUserId = UserId;  // Store UserId for transactions.

        }
        // Assuming CurrentUserId is set after the user logs in
        public static int CurrentUserId { get; set; }

       

        // Book class to represent the book data
        public class Book
        {
            public int BookID { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Genre { get; set; }
            public int YearPublished { get; set; }
            public string Status { get; set; }
        }

        private List<Book> allBooks = new List<Book>();

        // Load all books from the database
        private void LoadAllBooksFromDatabase()
        {
            try
            {
                // Clear the list to avoid duplicates
                allBooks.Clear();

                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\Users\\270449050\\OneDrive - UP Education\\c sharp\\Libook-(Library\\Libook.mdf\";Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT BookID, Title, Author, Genre, YearPublished, Status FROM Books";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        allBooks.Add(new Book
                        {
                            BookID = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.IsDBNull(2) ? "Unknown" : reader.GetString(2),
                            Genre = reader.IsDBNull(3) ? "Unknown" : reader.GetString(3),
                            YearPublished = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            Status = reader.IsDBNull(5) ? "Available" : reader.GetString(5)
                        });
                    }

                    // Bind the updated list to the ListView
                    AllBooksListView.ItemsSource = null; // Reset the binding
                    AllBooksListView.ItemsSource = allBooks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}");
            }
        }

        // Show the list of books when the "All Books" button is clicked
        private void AllBooksButton_Click(object sender, RoutedEventArgs e)
        {
            // Make the ListView visible and load the books
            AllBooksListView.Visibility = Visibility.Visible;
            LoadAllBooksFromDatabase();
            // Show only the AllBooksListView
            ShowOnly(AllBooksListView);
        }

        // Handle when a user selects a book
        private void AllBooksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllBooksListView.SelectedItem is Book selectedBook)
            {
                BookDetailsBox.Visibility = Visibility.Visible;
                BookInfo.Text = $"Title: {selectedBook.Title}\nAuthor: {selectedBook.Author}\nGenre: {selectedBook.Genre}\nYear: {selectedBook.YearPublished}\nStatus: {selectedBook.Status}";
            }
        }
        private void MyBooksButton_Click(object sender, RoutedEventArgs e)
        {
            ShowOnly(MyBooksListView);
            try
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Users\270449050\OneDrive - UP Education\c sharp\Libook-(Library\Libook.mdf;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT b.BookID, b.Title, b.Author, b.Genre, b.YearPublished, t.DueDate
                FROM Books b
                JOIN Transactions t ON b.BookID = t.BookID
                WHERE t.UserID = @UserID AND t.Status = 'Issued'";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", currentUserId); // Use the logged-in user's ID
                    SqlDataReader reader = command.ExecuteReader();

                    List<Book> myBooks = new List<Book>();
                    while (reader.Read())
                    {
                        myBooks.Add(new Book
                        {
                            BookID = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.IsDBNull(2) ? "Unknown" : reader.GetString(2),
                            Genre = reader.IsDBNull(3) ? "Unknown" : reader.GetString(3),
                            YearPublished = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            Status = "Issued"
                        });
                    }

                    MyBooksListView.ItemsSource = myBooks; // Use a ListView for displaying the user's books
                    MyBooksListView.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading your books: {ex.Message}");
            }
        }

        // Handle when the "Issue Book" button is clicked
        private void IssueBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (AllBooksListView.SelectedItem is Book selectedBook)
            {
                if (selectedBook.Status == "Available")
                {
                    DateTime? selectedDate = IssueDatePicker.SelectedDate;
                    if (selectedDate.HasValue && (selectedDate.Value - DateTime.Now).Days <= 30)
                    {
                        try
                        {
                            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Users\270449050\OneDrive - UP Education\c sharp\Libook-(Library\Libook.mdf;Integrated Security=True";

                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();

                                // Insert transaction record
                                string query = @"
                            INSERT INTO Transactions (BookID, UserID, TransactionType, TransactionDate, DueDate, Status)
                            VALUES (@BookID, @UserID, 'Issue', GETDATE(), @DueDate, 'Issued')";
                                SqlCommand command = new SqlCommand(query, connection);
                                command.Parameters.AddWithValue("@BookID", selectedBook.BookID);
                                command.Parameters.AddWithValue("@UserID", currentUserId); // Use the logged-in user's ID
                                command.Parameters.AddWithValue("@DueDate", selectedDate.Value);
                                command.ExecuteNonQuery();

                                // Update book status
                                query = "UPDATE Books SET Status = 'Issued' WHERE BookID = @BookID";
                                command = new SqlCommand(query, connection);
                                command.Parameters.AddWithValue("@BookID", selectedBook.BookID);
                                command.ExecuteNonQuery();
                            }

                            MessageBox.Show("Book issued successfully!");

                            // Update UI
                            selectedBook.Status = "Issued";
                            AllBooksListView.Items.Refresh();

                            // Show only the MyBooksListView after issuing the book
                           
                            ShowOnly(MyBooksListView);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error issuing book: {ex.Message}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a valid return date (no more than 30 days from today).");
                    }
                }
                else
                {
                    MessageBox.Show("This book is not available for issuing.");
                }
            }
            else
            {
                MessageBox.Show("Please select a book to issue.");
            }
        }

        private void ShowOnly(UIElement elementToShow)
        {
            // Hide all interactive elements
            AllBooksListView.Visibility = Visibility.Collapsed;
            MyBooksListView.Visibility = Visibility.Collapsed;
            BookDetailsBox.Visibility = Visibility.Collapsed;

            // Show the requested element
            if (elementToShow != null)
            {
                elementToShow.Visibility = Visibility.Visible;
            }
        
        }
// Handle when the "Logout" button is clicked
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Redirect to the Welcome Page
            var WelcomePage = new WelcomePage(); // Assuming WelcomePage is another page
            WelcomePage.Show();
            this.Close(); // Close the current page
        }
        private void ReturnBook(int bookId, int currentUserId)
        {
            try
            {
                // Update the transaction status to "Returned" and set the return date (if needed)
                string query = @"
            UPDATE [dbo].[Transactions]
            SET [Status] = 'Returned', [TransactionDate] = GETDATE()
            WHERE [BookID] = @BookID 
              AND [UserID] = @UserID
              AND [Status] = 'Issued';";

                // Update the book status to "Available"
                string updateBookStatusQuery = @"
            UPDATE [dbo].[Books]
            SET [Status] = 'Available'
            WHERE [BookID] = @BookID;";

                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\Users\\270449050\\OneDrive - UP Education\\c sharp\\Libook-(Library\\Libook.mdf\";Integrated Security=True"))
                {
                    conn.Open();

                    // Begin transaction to ensure both queries succeed together
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        // First query: Update the transaction record
                        SqlCommand cmd1 = new SqlCommand(query, conn, transaction);
                        cmd1.Parameters.AddWithValue("@BookID", bookId);
                        cmd1.Parameters.AddWithValue("@UserID", currentUserId);  // Use the correct UserID here
                        int rowsAffected = cmd1.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("No matching borrowed record found. Please check.");
                            transaction.Rollback();
                            return;
                        }

                        // Second query: Update the book's status to "Available"
                        SqlCommand cmd2 = new SqlCommand(updateBookStatusQuery, conn, transaction);
                        cmd2.Parameters.AddWithValue("@BookID", bookId);
                        cmd2.ExecuteNonQuery();

                        // Commit the transaction
                        transaction.Commit();
                    }

                    MessageBox.Show("Book returned successfully!");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null)
            {
                int bookId = int.Parse(button.Tag.ToString());

                // Make sure the current user ID is properly assigned
                int currentUserId = CurrentUserId;

                // Call the method to update the database and mark the book as returned
                ReturnBook(bookId, currentUserId);  // Pass the current user ID here
            }
        }



    }
}

