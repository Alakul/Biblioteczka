using Biblioteczka.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Biblioteczka.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Imię")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Pole {0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Nazwisko")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Pole {0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 2)]
        public string LastName { get; set; }
    }
}
