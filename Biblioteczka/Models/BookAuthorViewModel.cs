using X.PagedList;

namespace Biblioteczka.Models
{
    public class BookAuthorViewModel
    {
        public List<Book>? Books { get; set; }
        public List<Author>? Authors { get; set; }

        public Book Book { get; set; }
        public Author Author { get; set; }


        public IPagedList<Book> BookList { get; set; }
    }
}
