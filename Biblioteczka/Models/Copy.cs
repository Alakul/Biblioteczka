using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteczka.Models
{
    public class Copy
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }
        public DateTime Date { get; set; }


        [ForeignKey("BookId")]
        public int BookId { get; set; }
        

        public int Number { get; set; }
        public string Status { get; set; }
    }
}
