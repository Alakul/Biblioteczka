using Biblioteczka.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Biblioteczka.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public DateTime Date { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }

        public List<Book>? Books { get; set; }
    }
}
