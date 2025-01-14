using BooksShop.Enum;
using BooksShop.Model;

namespace BooksShop.Interface
{
    public interface IEntityAdapter
    {
        Task<List<Book>> GetBooksByFilterAsync(string? title, string? author, DateOnly? date, OrderBy? orderBy);
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task SaveChangesAsync();
        Task AddDefaultBooks(List<Book> list);
        Task RestockAllAsync();
    }
}
