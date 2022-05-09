namespace LibraryApp.Models
{
    public class BookAuthorCopyViewModel
    {
        public Book Book { get; set; }
        public Author Author { get; set; }
        public List<Copy> Copies { get; set; }
    }
}
