namespace LibraryApp.Models
{
    public class BookViewModel
    {
        public DateTime Date { get; set; }

        public string? Title { get; set; }
        public int AuthorId { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int Year { get; set; }
        public string? City { get; set; }

        public List<Author> Authors { get; set; }
        public IFormFile? File { get; set; }
    }
}
