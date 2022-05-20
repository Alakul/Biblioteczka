namespace Biblioteczka.Models
{
    public class IndexViewModel
    {
        public List<Book>? BooksYear { get; set; }
        public List<Book>? BooksDate{ get; set; }
        public List<Author>? Authors { get; set; }
    }
}
