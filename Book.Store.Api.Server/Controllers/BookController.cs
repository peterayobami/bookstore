using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book.Store.Api.Server
{
    [AuthorizeAdmin]
    public class BookController : BaseController<BookController>
    {
        public readonly BookService bookService;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="bookService">The injected <see cref="BookService"/></param>
        /// <param name="logger">The injected <see cref="ILogger{TCategory}"/></param>
        /// <returns></returns>
        public BookController(BookService bookService, ILogger<BookController> logger) : base(logger)
        {
            this.bookService = bookService;
        }
        
        /// <summary>
        /// Handles HTTP request to create book with specified credentials
        /// </summary>
        /// <param name="bookCredentials">The book credentials</param>
        /// <returns></returns>
        [HttpPost(EndpointRoutes.CreateBook)]
        public async Task<ActionResult> CreateBookAsync([FromBody] BookCredentials bookCredentials)
        {
            return await HandleOperationAsync(() => bookService.CreateAsync(bookCredentials));
        }
        
        /// <summary>
        /// Handles HTTP request to retrieve books
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(EndpointRoutes.FetchBooks)]
        public async Task<ActionResult> FetchBooksAsync()
        {
            return await HandleOperationAsync(bookService.FetchAsync);
        }
        
        /// <summary>
        /// Handles HTTP request to retrieve books based on specified book title or author
        /// </summary>
        /// <param name="title">The specified book title</param>
        /// <param name="author">The specified book author</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(EndpointRoutes.SearchBooks)]
        public async Task<ActionResult> FetchBooksAsync([FromQuery] string title, [FromQuery] string author)
        {
            return await HandleOperationAsync(() => bookService.FetchByAuthorOrTitleAsync(title, author));
        }
    }
}
