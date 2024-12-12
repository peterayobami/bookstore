using Microsoft.EntityFrameworkCore;

namespace Book.Store.Api.Server
{
    public class BookService
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<BookService> logger;

        public BookService(ApplicationDbContext context, ILogger<BookService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<OperationResult> CreateAsync(BookCredentials bookCredentials)
        {
            try
            {
                var book = new BookDataModel
                {
                    Title = bookCredentials.Title,
                    Author = bookCredentials.Author,
                    Price = bookCredentials.Price,
                    PublishedDate = bookCredentials.PublishedDate
                };

                // Create the book
                await context.Books.AddAsync(book);

                await context.SaveChangesAsync();

                return new OperationResult
                {
                    StatusCode = StatusCodes.Status201Created,
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    ErrorMessage = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<OperationResult> FetchAsync()
        {
            try
            {
                var books = await context.Books.Select(book => new BookApiModel
                {
                    Title = book.Title,
                    Author = book.Author,
                    Price = book.Price,
                    PublishedDate = book.PublishedDate
                }).ToListAsync();

                return new OperationResult
                {
                    Result = books,
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    ErrorMessage = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<OperationResult> FetchByAuthorOrTitleAsync(string title, string author)
        {
            try
            {
                var books = await context.Books.Select(book => new BookApiModel
                {
                    Title = book.Title,
                    Author = book.Author,
                    Price = book.Price,
                    PublishedDate = book.PublishedDate
                })
                .Where(book => book.Title == title || book.Author == author)
                .ToListAsync();

                return new OperationResult
                {
                    Result = books,
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                return new OperationResult
                {
                    ErrorTitle = "SYSTEM ERROR",
                    ErrorMessage = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}