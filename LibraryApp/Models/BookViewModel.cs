namespace LibraryApp.Models
{
    public class BookViewModel
    {
        public DateTime Date { get; set; }

        public string? Title { get; set; }

        public int Author { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }

        public int Year { get; set; }
        public string? City { get; set; }

        public List<Author> Authors { get; set; }
    }
}
