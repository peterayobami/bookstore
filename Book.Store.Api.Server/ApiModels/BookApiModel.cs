namespace Book.Store.Api.Server
{
    public class BookApiModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}