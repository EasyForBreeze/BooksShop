
using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BooksShop.DbContext;
using BooksShop.Interface;
using BooksShop.Model;

namespace BooksShop.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BooksShop.Enum;
    using Moq;
    using Xunit;

    public class BookRepositoryTests
    {
        private readonly Mock<IEntityAdapter> _mockAdapter;
        private readonly BookRepository _repository;

        public BookRepositoryTests()
        {
            _mockAdapter = new Mock<IEntityAdapter>();
            _repository = new BookRepository(_mockAdapter.Object);
        }

        [Fact]
        public async Task GetBooksByFilters_ValidFilters_BooksFound()
        {
            // Arrange
            var books = new List<Book>
        {
            new Book { Title = "Test Book", Author = "Author", PublishedDate = new DateOnly(2020, 1, 1), Count = 10 }
        };
            _mockAdapter.Setup(a => a.GetBooksByFilterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<OrderBy?>()))
                        .ReturnsAsync(books);

            // Act
            await _repository.GetBooksByFilters("Test Book", "Author", "2020-01-01", null);

            // Assert
            _mockAdapter.Verify(a => a.GetBooksByFilterAsync("Test Book", "Author", new DateOnly(2020, 1, 1), null), Times.Once);
        }

        [Fact]
        public async Task GetBooksByFilters_NoBooksFound_PrintsMessage()
        {
            // Arrange
            _mockAdapter.Setup(a => a.GetBooksByFilterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<OrderBy?>()))
                        .ReturnsAsync(new List<Book>());

            // Act
            await _repository.GetBooksByFilters("NonExistent", "Author", null, null);

            // Assert
            _mockAdapter.Verify(a => a.GetBooksByFilterAsync("NonExistent", "Author", null, null), Times.Once);
        }

        [Fact]
        public async Task AddDefaultBooks_CallsAddDefaultBooksMethod()
        {
            // Act
            await _repository.AddDefaultBooks();

            // Assert
            _mockAdapter.Verify(a => a.AddDefaultBooks(It.IsAny<List<Book>>()), Times.Once);
        }

        [Fact]
        public async Task BuyBookByID_ValidId_DecreasesBookCount()
        {
            // Arrange
            var book = new Book { Count = 5 };
            _mockAdapter.Setup(a => a.GetBookByIdAsync(1)).ReturnsAsync(book);

            // Act
            await _repository.BuyBookByID("1");

            // Assert
            Assert.Equal(4, book.Count);
            _mockAdapter.Verify(a => a.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task BuyBookByID_InvalidId_PrintsErrorMessage()
        {
            // Act
            await _repository.BuyBookByID("invalid");

            // Assert
            _mockAdapter.Verify(a => a.GetBookByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task RestockRandomAsync_SpecificIdAndCount_UpdatesBookCount()
        {
            // Arrange
            var book = new Book { Count = 5 };
            _mockAdapter.Setup(a => a.GetBookByIdAsync(1)).ReturnsAsync(book);

            // Act
            await _repository.RestockRandomAsync("1", "10");

            // Assert
            Assert.Equal(15, book.Count);
            _mockAdapter.Verify(a => a.GetBookByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task RestockRandomAsync_NoId_RestocksAllBooks()
        {
            // Act
            await _repository.RestockRandomAsync();

            // Assert
            _mockAdapter.Verify(a => a.RestockAllAsync(), Times.Once);
        }
    }
}
