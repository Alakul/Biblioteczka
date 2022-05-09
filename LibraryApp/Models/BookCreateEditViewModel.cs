namespace LibraryApp.Models
{
    public class BookCreateEditViewModel
    {
        public Book? Book { get; set; }
        public List<Author>? Authors { get; set; }

        public IFormFile? File { get; set; }
    }
}
