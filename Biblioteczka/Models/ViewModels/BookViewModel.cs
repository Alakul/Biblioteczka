using X.PagedList;

namespace Biblioteczka.Models
{
    public class BookViewModel
    {
        public Book Book { get; set; }
        public Author Author { get; set; }
        public IPagedList<BookViewModel>? BookList { get; set; }


        public IPagedList<Copy>? CopyList { get; set; }
    }
}
