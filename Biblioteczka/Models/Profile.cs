using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteczka.Models
{
    public class Profile
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Wprowadź liczbę skłającą się z 11 cyfr.")]
        [Display(Name = "Pesel")]
        public string Pesel { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "Wprowadź liczbę skłającą się z 8 cyfr.")]
        [Display(Name = "Numer karty bibliotecznej")]
        public string LibraryCardNumber { get; set; }
        public DateTime Date { get; set; }
    }
}
