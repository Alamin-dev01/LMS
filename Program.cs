using System;
using System.Collections.Generic;
using System.Linq;

class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Category { get; set; }
    public bool IsBorrowed { get; set; }
    public DateTime ReturnDate { get; set; }
}

class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Book> BorrowedBooks { get; set; }

    public User()
    {
        BorrowedBooks = new List<Book>();
    }
}

class Library
{
    private List<Book> books;
    private List<User> users;

    public Library()
    {
        books = new List<Book>();
        users = new List<User>();
    }

    public void AddBook(Book book)
    {
        book.Id = books.Count + 1;
        books.Add(book);
    }

    public List<Book> SearchBooks(string keyword)
    {
        return books.Where(b =>
            b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public void BorrowBook(User user, Book book, DateTime returnDate)
    {
        if (book.IsBorrowed)
        {
            Console.WriteLine("This book is already borrowed.");
            return;
        }

        book.IsBorrowed = true;
        book.ReturnDate = returnDate;
        user.BorrowedBooks.Add(book);
        Console.WriteLine($"Book '{book.Title}' borrowed by {user.Name}.");
    }

    public void ReturnBook(User user, Book book)
    {
        if (!book.IsBorrowed || !user.BorrowedBooks.Contains(book))
        {
            Console.WriteLine("This book was not borrowed by the user.");
            return;
        }

        TimeSpan overdueDays = DateTime.Now - book.ReturnDate;
        if (overdueDays.TotalDays > 0)
        {
            double fineAmount = overdueDays.TotalDays * 0.5; // Assuming fine is 50 cents per day
            Console.WriteLine($"Book returned late! Fine amount: ${fineAmount}");
        }

        book.IsBorrowed = false;
        user.BorrowedBooks.Remove(book);
        Console.WriteLine($"Book '{book.Title}' returned by {user.Name}.");
    }

    public void DisplayBooks()
    {
        Console.WriteLine("\nList of Available Books:");
        foreach (var book in books)
        {
            if (!book.IsBorrowed)
            {
                Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Author: {book.Author}, Category: {book.Category}");
            }
        }
    }

    public void DisplayUserHistory(User user)
    {
        Console.WriteLine($"\nBorrowing History for User: {user.Name}");
        foreach (var book in user.BorrowedBooks)
        {
            Console.WriteLine($"Title: {book.Title}, Borrowed on: {book.ReturnDate.ToShortDateString()}");
        }
    }
}

class Program
{
    static void Main()
    {
        Library library = new Library();

        // Adding sample books to the library
        library.AddBook(new Book { Title = "C# Programming", Author = "John Doe", Category = "Programming" });
        library.AddBook(new Book { Title = "Harry Potter", Author = "J.K. Rowling", Category = "Fantasy" });
        library.AddBook(new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Category = "Fiction" });

        // Creating a sample user
        User user1 = new User { Id = 1, Name = "Alice" };

        // Display available books
        library.DisplayBooks();

        // Searching for books
        List<Book> searchResults = library.SearchBooks("Harry Potter");
        foreach (var result in searchResults)
        {
            Console.WriteLine($"\nFound Book: {result.Title} by {result.Author}");
        }

        // Borrowing a book
        if (searchResults.Count > 0)
        {
            Book bookToBorrow = searchResults[0];
            library.BorrowBook(user1, bookToBorrow, DateTime.Now.AddDays(14)); // Return date after 2 weeks
        }

        // Display user borrowing history
        library.DisplayUserHistory(user1);

        // Returning a book
        if (user1.BorrowedBooks.Count > 0)
        {
            Book bookToReturn = user1.BorrowedBooks[0];
            library.ReturnBook(user1, bookToReturn);
        }
    }
}

