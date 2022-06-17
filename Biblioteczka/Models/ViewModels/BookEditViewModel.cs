using Biblioteczka.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Biblioteczka.Models
{
    public class BookEditViewModel
    {
        public Book Book { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }

        [Display(Name = "Okładka")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", "jpeg", ".png" }, ErrorMessage = "Nieprawidłowe rozszerzenie pliku! Dostępne rozszerzenia: .jpg, .jpeg, .png.")]
        [MaxFileSize(2 * 1024 * 1024, ErrorMessage = "Rozmiar pliku nie może przekraczać 2 MB.")]
        public IFormFile? File { get; set; }
    }
}

