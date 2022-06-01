using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteczka.Models
{
    public class Profile
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }

        public string LibraryCardNumber { get; set; }
        public string Pesel { get; set; }

        public DateTime Date { get; set; }
    }
}
