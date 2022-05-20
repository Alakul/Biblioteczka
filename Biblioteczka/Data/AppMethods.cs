using Biblioteczka.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteczka.Data
{
    public static class AppMethods
    {
        public static void Sort(AppDbContext db)
        {
            List<Book> list = db.Book.ToList();
            DbSet<Book> w= db.Book;
        }

        public static void Search(AppDbContext db)
        {

        }
    }
}
