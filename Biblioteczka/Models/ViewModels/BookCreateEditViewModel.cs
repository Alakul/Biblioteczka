using System.ComponentModel.DataAnnotations;

namespace Biblioteczka.Models
{
    public class BookCreateEditViewModel
    {
        public Book Book { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        
        [Display(Name = "Okładka")]
        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public List<Author>? Authors { get; set; }
    }
}
