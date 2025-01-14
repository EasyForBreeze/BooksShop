namespace BooksShop.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateOnly PublishedDate { get; set; }
        public int Count { get; set; }
    }
}
