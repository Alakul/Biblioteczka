namespace LibraryApp.Models
{
    public class CopyViewModel
    {
        public int BookId { get; set; }
        public string? Title { get; set; }

        public int Number { get; set; }
        public string? Status { get; set; }

        public string? Name { get; set; }
        public string? LastName { get; set; }

        public List<Copy> Copies { get; set; }

    }
}
