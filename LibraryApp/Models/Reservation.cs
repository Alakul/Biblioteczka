using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }


        [ForeignKey("UserBorrowingId")]
        public string? UserBorrowingId { get; set; }
        public int CopyId { get; set; }
    }
}
