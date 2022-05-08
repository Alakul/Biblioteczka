﻿namespace LibraryApp.Data
{
    public class AppData
    {
        //BOOK
        public static List<string> bookCategories = new List<string> {
            "Thriller", "Horror", "Dramat", "Popularno-naukowa"
        };

        public static List<string> bookSort = new List<string> {
            "po tytule rosnąco", "po tytule malejąco", "po autorze rosnąco", "po autorze malejąco", "po dacie rosnąco", "po dacie malejąco"
        };



        //AUTHOR
        public static List<string> authorSort = new List<string> {
            "po nazwisku rosnąco", "po nazwisku malejąco", "po dacie rosnąco", "po dacie malejąco"
        };



        //USER
        public static List<string> userCategories = new List<string> {
            "Admin", "Zwykły użytkownik"
        };

        public static List<string> userSort = new List<string> {
            "po nazwie rosnąco", "po nazwie malejąco", "po dacie rosnąco", "po dacie malejąco"
        };
    }
}