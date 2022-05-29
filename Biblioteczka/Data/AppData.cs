namespace Biblioteczka.Data
{
    public static class AppData
    {
        public const string Admin = "Administrator";
        public const string Librarian = "Bibliotekarz";
        public const string User = "Użytkownik";

        public static List<string> roleTypes = new List<string> {
           User, Librarian, Admin
        };

        public static Dictionary<string, string> copyStatus = new Dictionary<string, string> {
            {"1", "Dostępna"},
            {"0", "Niedostępna"},
        };
    }
}
