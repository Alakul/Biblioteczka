using X.PagedList;

namespace Biblioteczka.Models
{
    public class BookAuthorViewModel
    {
        //DO PRZYSZŁEGO USUNIĘCIA
        public List<Book>? Books { get; set; }




        public Book Book { get; set; }
        public Author Author { get; set; }
        public IPagedList<BookAuthorViewModel>? BookList { get; set; }
    }
}
