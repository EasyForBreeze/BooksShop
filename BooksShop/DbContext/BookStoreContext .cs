using BooksShop.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


public class BookStoreContext : DbContext
{
    public DbSet<Book> Books { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasKey(p => p.Id);
    }
}
