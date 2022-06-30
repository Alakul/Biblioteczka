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

        public static Dictionary<string, string> bookSort = new Dictionary<string, string> {
            {"DateAsc", "po dacie dodania rosnąco"},
            {"DateDesc", "po dacie dodania malejąco"},
            {"TitleAsc", "po tytule rosnąco"},
            {"TitleDesc", "po tytule malejąco"},
            {"LastNameAsc", "po nazwisku autora rosnąco"},
            {"LastNameDesc", "po nazwisku autora malejąco"},
            {"YearAsc", "po roku wydania rosnąco"},
            {"YearDesc", "po roku wydania malejąco"},
        };

        public static Dictionary<string, string> authorSort = new Dictionary<string, string> {
            {"DateAsc", "po dacie dodania rosnąco"},
            {"DateDesc", "po dacie dodania malejąco"},
            {"NameAsc", "po imieniu rosnąco"},
            {"NameDesc", "po imieniu malejąco"},
            {"LastNameAsc", "po nazwisku rosnąco"},
            {"LastNameDesc", "po nazwisku malejąco"},
        };

        public static Dictionary<string, string> copySort = new Dictionary<string, string> {
            {"DateAsc", "po dacie dodania rosnąco"},
            {"DateDesc", "po dacie dodania malejąco"},
            {"NumberAsc", "po numerze rosnąco"},
            {"NumberDesc", "po numerze malejąco"},
            {"TitleAsc", "po tytule rosnąco"},
            {"TitleDesc", "po tytule malejąco"},
            {"LastNameAsc", "po nazwisku autora rosnąco"},
            {"LastNameDesc", "po nazwisku autora malejąco"},
        };

        public static Dictionary<string, string> loanSort = new Dictionary<string, string> {
            {"DateAsc", "po dacie dodania rosnąco"},
            {"DateDesc", "po dacie dodania malejąco"},
            {"UserNameAsc", "po loginie użytkownika rosnąco"},
            {"UserNameDesc", "po loginie użytkownika malejąco"},
            {"TitleAsc", "po tytule rosnąco"},
            {"TitleDesc", "po tytule malejąco"},
            {"LastNameAsc", "po nazwisku autora rosnąco"},
            {"LastNameDesc", "po nazwisku autora malejąco"},
            {"NumberAsc", "po numerze rosnąco"},
            {"NumberDesc", "po numerze malejąco"},
            {"LoanDateAsc", "po dacie wypożyczenia rosnąco"},
            {"LoanDateDesc", "po dacie wypożyczenia malejąco"},
            {"ReturnDateAsc", "po dacie zwrotu rosnąco"},
            {"ReturnDateDesc", "po dacie zwrotu malejąco"},
        };

        public static Dictionary<string, string> reservationSort = new Dictionary<string, string> {
            {"DateAsc", "po dacie dodania rosnąco"},
            {"DateDesc", "po dacie dodania malejąco"},
            {"UserNameAsc", "po loginie użytkownika rosnąco"},
            {"UserNameDesc", "po loginie użytkownika malejąco"},
            {"TitleAsc", "po tytule rosnąco"},
            {"TitleDesc", "po tytule malejąco"},
            {"LastNameAsc", "po nazwisku autora rosnąco"},
            {"LastNameDesc", "po nazwisku autora malejąco"},
            {"NumberAsc", "po numerze rosnąco"},
            {"NumberDesc", "po numerze malejąco"},
        };

        public static Dictionary<string, string> userSort = new Dictionary<string, string> {
            {"NameAsc", "po imieniu rosnąco"},
            {"NameDesc", "po imieniu malejąco"},
            {"LastNameAsc", "po nazwisku rosnąco"},
            {"LastNameDesc", "po nazwisku malejąco"},
            {"UserNameAsc", "po loginie użytkownika rosnąco"},
            {"UserNameDesc", "po loginie użytkownika malejąco"},
            {"EmailAsc", "po adresie email rosnąco"},
            {"EmailDesc", "po adresie email malejąco"},
        };
    }
}
