using Biblioteczka.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteczka.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public DateTime Date { get; set; }

        [Display(Name = "Autor")]
        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Tytuł")]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "Pole {0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 2)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Opis")]
        [DataType(DataType.Text)]
        [StringLength(3000, ErrorMessage = "Pole {0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 3)]
        public string Description { get; set; }
        public string? Image { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Rok wydania")]
        [Range(1, 9999, ErrorMessage = "Wprowadź liczbę całkowitą z zakresu od {1} do {2}.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Miejsce wydania")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Pole {0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 2)]
        public string City { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "ISBN")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Pole {0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 10)]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Wydawnictwo")]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "Pole {0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 2)]
        public string Publisher { get; set; }

        [Display(Name = "Liczba stron")]
        [Range(1, 9999, ErrorMessage = "Wprowadź liczbę całkowitą z zakresu od {1} do {2}.")]
        public int? Pages { get; set; }

        [Display(Name = "Wydanie")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Pole {0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 1)]
        public string? IssueNumber { get; set; }

        [Display(Name = "Seria")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Pole {0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 2)]
        public string? Series { get; set; }
    }
}
