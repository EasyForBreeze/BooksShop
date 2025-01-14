using BooksShop.Enum;
using BooksShop.Interface;
using BooksShop.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace BooksShop.DbContext
{
    public class BookRepository : IBookRepository
    {
        private readonly IEntityAdapter _adapter;
        private PropertyInfo[] properties;
        public BookRepository()
        {
            BookStoreContext bookStoreContext = new BookStoreContext();
            bookStoreContext.Database.EnsureCreated();
            _adapter = new EntityAdapter(bookStoreContext);
            properties = typeof(Book).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public async Task GetBooksByFilters(string? title, string? author, string? date, string? orderBy)
        {
            DateOnly? xdate = null;
            OrderBy? xorderBy = null;
            if (date != null)
            {
                if (!DateOnly.TryParse(date, out DateOnly dt))
                {
                    Console.WriteLine(EnumMessages.UserMessage.ErrorDate.GetMessage());
                }
                else
                {
                    xdate = dt;
                }
            }
            if (orderBy != null)
            {
                if (!System.Enum.TryParse(orderBy, out OrderBy oBy))
                {
                    Console.WriteLine(EnumMessages.UserMessage.ErrorOrderyBy.GetMessage());
                }
                else
                {
                    xorderBy = oBy;
                }
            }

            var ListOfBooks = await _adapter.GetBooksByFilterAsync(title, author, xdate, xorderBy);
            if (ListOfBooks.Count > 0)
            {
                foreach (var book in ListOfBooks)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        Console.WriteLine($"{property.Name}: {property.GetValue(book)}");
                    }
                }
            }
            else
            {
                Console.WriteLine(EnumMessages.UserMessage.BooksNotFound.GetMessage());
            }
        }

        public async Task AddDefaultBooks()
        {
            List<Book> books = new List<Book>()
            {
                new Book { Author = "J.K. Rowling", Title = "Harry Potter and the Philosopher's Stone", PublishedDate = new DateOnly(1997, 6, 26), Count = 10 },
                new Book { Author = "George Orwell", Title = "1984", PublishedDate = new DateOnly(1949, 6, 8), Count = 15 },
                new Book { Author = "J.R.R. Tolkien", Title = "The Hobbit", PublishedDate = new DateOnly(1937, 9, 21), Count = 20 },
                new Book { Author = "Harper Lee", Title = "To Kill a Mockingbird", PublishedDate = new DateOnly(1960, 7, 11), Count = 12 },
                new Book { Author = "F. Scott Fitzgerald", Title = "The Great Gatsby", PublishedDate = new DateOnly(1925, 4, 10), Count = 8 },
                new Book { Author = "Mary Shelley", Title = "Frankenstein", PublishedDate = new DateOnly(1818, 1, 1), Count = 5 },
                new Book { Author = "Leo Tolstoy", Title = "War and Peace", PublishedDate = new DateOnly(1869, 1, 1), Count = 6 },
                new Book { Author = "Herman Melville", Title = "Moby-Dick", PublishedDate = new DateOnly(1851, 10, 18), Count = 7 },
                new Book { Author = "Jane Austen", Title = "Pride and Prejudice", PublishedDate = new DateOnly(1813, 1, 28), Count = 11 },
                new Book { Author = "Mark Twain", Title = "Adventures of Huckleberry Finn", PublishedDate = new DateOnly(1884, 12, 10), Count = 9 },
                new Book { Author = "Isaac Asimov", Title = "Foundation", PublishedDate = new DateOnly(1951, 6, 1), Count = 14 },
                new Book { Author = "Arthur C. Clarke", Title = "2001: A Space Odyssey", PublishedDate = new DateOnly(1968, 7, 11), Count = 13 },
                new Book { Author = "Douglas Adams", Title = "The Hitchhiker's Guide to the Galaxy", PublishedDate = new DateOnly(1979, 10, 12), Count = 18 },
                new Book { Author = "Frank Herbert", Title = "Dune", PublishedDate = new DateOnly(1965, 8, 1), Count = 16 },
                new Book { Author = "Philip K. Dick", Title = "Do Androids Dream of Electric Sheep?", PublishedDate = new DateOnly(1968, 3, 1), Count = 10 },
                new Book { Author = "Ray Bradbury", Title = "Fahrenheit 451", PublishedDate = new DateOnly(1953, 10, 19), Count = 11 },
                new Book { Author = "H.G. Wells", Title = "The War of the Worlds", PublishedDate = new DateOnly(1898, 1, 1), Count = 8 },
                new Book { Author = "Jules Verne", Title = "Twenty Thousand Leagues Under the Sea", PublishedDate = new DateOnly(1870, 1, 1), Count = 9 },
                new Book { Author = "Orson Scott Card", Title = "Ender's Game", PublishedDate = new DateOnly(1985, 1, 15), Count = 12 },
                new Book { Author = "C.S. Lewis", Title = "The Lion, the Witch and the Wardrobe", PublishedDate = new DateOnly(1950, 10, 16), Count = 15 }
            };
            await _adapter.AddDefaultBooks(books);
            Console.WriteLine(EnumMessages.UserMessage.Success.GetMessage());
        }
        public async Task BuyBookByID(string? id)
        {
            if (id != null)
            {
                if (int.TryParse(id, out int Id))
                {
                    var book = await _adapter.GetBookByIdAsync(Id);
                    if (book == null)
                    {
                        Console.WriteLine(EnumMessages.UserMessage.BookNotFound.GetMessage());
                    }
                    else
                    {
                        if (book.Count > 0)
                        {
                            book.Count -= 1;
                            await _adapter.SaveChangesAsync();
                            Console.WriteLine(EnumMessages.UserMessage.Success.GetMessage());
                        }
                        else
                        {
                            Console.WriteLine(EnumMessages.UserMessage.BookIsFinished.GetMessage());
                        }
                    }
                }
                else
                {
                    Console.WriteLine(EnumMessages.UserMessage.ErrorId.GetMessage());
                }
            }
            else
            {
                Console.WriteLine(EnumMessages.UserMessage.IdNotFound.GetMessage());
            }
        }

        public async Task RestockRandomAsync(string? id = null, string? count = null)
        {
            if (id != null && count != null)
            {
                if (int.TryParse(id, out int Id))
                {
                    if (int.TryParse(count, out int cnt))
                    {
                        var book = await _adapter.GetBookByIdAsync(Id);
                        if (book == null)
                        {
                            Console.WriteLine(EnumMessages.UserMessage.BookNotFound.GetMessage());
                        }
                        else
                        {
                            book.Count += cnt;
                            Console.WriteLine(EnumMessages.UserMessage.Success.GetMessage());
                        }
                    }
                    else
                    {
                        Console.WriteLine(EnumMessages.UserMessage.ErrorCnt.GetMessage());
                    }
                }
                else
                {
                    Console.WriteLine(EnumMessages.UserMessage.ErrorId.GetMessage());
                }
            }
            else
            {
                //var books = await _adapter.GetAllBooksAsync();
                //var random = new Random();
                //foreach (var book in books)
                //{
                //    if (random.Next(0, 1) == 0)
                //    {
                //        book.Count += random.Next(0, 15);
                //    }
                //}

                await _adapter.RestockAllAsync();
                Console.WriteLine(EnumMessages.UserMessage.Success.GetMessage());
            }
            //await _adapter.SaveChangesAsync();
        }
    }
}
