using BooksShop.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShop.Interface
{
    internal interface IBookRepository
    {
        Task GetBooksByFilters(string? title, string? author, string? date, string? orderBy);
        Task BuyBookByID(string? id);
        Task RestockRandomAsync(string? id = null, string? count = null);
        Task AddDefaultBooks();
    }
}
