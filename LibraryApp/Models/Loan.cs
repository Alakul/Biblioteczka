using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }


        [ForeignKey("UserBorrowingId")]
        public string? UserBorrowingId { get; set; }
        public int CopyId { get; set; }
    }
}
