using BooksShop.DbContext;
using BooksShop.Interface;
using System.Text;



var result = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
BookStoreContext bookStoreContext = new BookStoreContext();
bookStoreContext.Database.EnsureCreated();
IEntityAdapter _adapter = new EntityAdapter(bookStoreContext);
IBookRepository bookRepository = new BookRepository(_adapter);
string? date = null;
string? orderBy = null;
string? tittle = null;
string? author = null;

if (GetParams())
{
    switch (args[0])
    {
        case "get":
            {
                orderBy = result.GetValueOrDefault("order-by", null);
                tittle = result.GetValueOrDefault("tittle", null);
                author = result.GetValueOrDefault("author", null);
                date = result.GetValueOrDefault("date", null);
                Console.WriteLine("Ваши фильтры:" + Environment.NewLine + "-----------------" + Environment.NewLine + "tittle:" + tittle + Environment.NewLine + "author:" + author + Environment.NewLine + "date:" + date + Environment.NewLine + "order-by:" + orderBy + Environment.NewLine);
                await bookRepository.GetBooksByFilters(tittle, author, date, orderBy);
                break;
            }
        case "buy":
            {
                await bookRepository.BuyBookByID(result.GetValueOrDefault("Id", null));
                break;
            }
        case "restock":
            {
                await bookRepository.RestockRandomAsync(result.GetValueOrDefault("Id", null), result.GetValueOrDefault("count", null));
                break;
            }
        case "adddata":
            {
                await bookRepository.AddDefaultBooks();
                break;
            }
    }
}




bool GetParams()
{
    try
    {
        for (int i = 1; i < args.Length; i++)
        {
            var w = args[i].Replace("--", "").Split('=');
            result.Add(w[0].ToLower(), w[1]);
        }
        return true;
    }
    catch
    {
        Console.WriteLine("Некорректные значения полей");
        return false;
    }
}














