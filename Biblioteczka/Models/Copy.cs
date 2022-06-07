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

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Numer inwentarzowy")]
        [Range(1, 99999999, ErrorMessage = "Wprowadź liczbę całkowitą z zakresu od {0} do {1}.")]
        public int Number { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Status")]
        [DataType(DataType.Text)]
        [StringLength(1)]
        public string Status { get; set; }
    }
}
