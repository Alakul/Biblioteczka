using Biblioteczka.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteczka.Models
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }


        [ForeignKey("UserBorrowingId")]
        public string UserBorrowingId { get; set; }


        [ForeignKey("BookId")]
        public int BookId { get; set; }


        [ForeignKey("CopyId")]
        public int CopyId { get; set; }
        public string Status { get; set; }
    }
}
