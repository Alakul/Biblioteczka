using LibraryApp.Areas.Identity.Data;

namespace LibraryApp.Models
{
    public class CopyBookAuthorViewModel
    {
        public List<Book>? Books { get; set; }
        public List<Author>? Authors { get; set; }
        public List<Copy>? Copies { get; set; }
    }
}
