using BooksShop.Enum;
using BooksShop.Interface;
using BooksShop.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace BooksShop.DbContext
{
    internal class EntityAdapter : IEntityAdapter
    {
        private readonly BookStoreContext _context;

        public EntityAdapter(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<List<Book>> GetBooksByFilterAsync(string? title, string? author, DateOnly? date, OrderBy? orderBy)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(b => b.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(author))
                query = query.Where(b => b.Author.Contains(author));

            if (date.HasValue)
                query = query.Where(b => b.PublishedDate == date.Value);

            query = orderBy switch
            {
                OrderBy.tittle => query.OrderBy(b => b.Title),
                OrderBy.author => query.OrderBy(b => b.Author),
                OrderBy.date => query.OrderBy(b => b.PublishedDate),
                OrderBy.count => query.OrderBy(b => b.Count),
                _ => query.OrderBy(b => b.Id)
            };

            return await query.ToListAsync();
        }
        public async Task AddDefaultBooks(List<Book> list)
        {
            await _context.AddRangeAsync(list);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Book>> GetAllBooksAsync()
        {
            var query = _context.Books.AsQueryable().OrderBy(x => x.Id);
            return await query.ToListAsync();
        }

        public async Task RestockAllAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("UPDATE Books SET Count = Count + (ABS(RANDOM() % 15) + 1)");
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
