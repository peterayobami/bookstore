using Swashbuckle.AspNetCore.Filters;

namespace Book.Store.Api.Server
{
    /// <summary>
    /// Create book credential examples
    /// </summary>
    public class BookCredentialsModelExample : IExamplesProvider<BookCredentials>
    {
        /// <summary>
        /// Gets the sample value for object properties
        /// </summary>
        /// <returns></returns>
        public BookCredentials GetExamples()
        {
            return new BookCredentials
            {
                Title = "Understanding Hybrid Technology",
                Author = "Emma Davis",
                Price = 4500,
                PublishedDate = DateTime.Parse("2023-12-01T00:00:00")
            };
        }
    }
}