using X.PagedList;

namespace Biblioteczka.Models
{
    public class CopyViewModel
    {
        public Book Book { get; set; }
        public Author Author { get; set; }
        public Copy Copy { get; set; }
        public IPagedList<CopyViewModel>? CopyList { get; set; }
    }
}
