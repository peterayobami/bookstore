using System.ComponentModel.DataAnnotations;

namespace Book.Store.Api.Server
{
    /// <summary>
    /// The credentials to create a book
    /// </summary>
    public class BookCredentials
    {
        /// <summary>
        /// The title of the book as named by the author
        /// </summary>
        [Required(ErrorMessage = "The title of the book is required")]
        public string Title { get; set; }

        /// <summary>
        /// The name of the author as specified on the book
        /// </summary>
        [Required(ErrorMessage = "The author's name is required")]
        public string Author { get; set; }
        
        /// <summary>
        /// The price of the book
        /// </summary>
        [Required(ErrorMessage = "The price of the book is required")]
        public double Price { get; set; }

        /// <summary>
        /// The date when the book was published
        /// </summary>
        [Required(ErrorMessage = "The published date is required")]
        public DateTime PublishedDate { get; set; }
    }
}