using Biblioteczka.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteczka.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }
        public DateTime Date { get; set; }


        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }


        public int Year { get; set; }
        public string City { get; set; }
    }
}
