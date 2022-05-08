using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApp.Controllers
{
    public class BookController : Controller
    {
        AppDbContext db;
        public BookController(AppDbContext context)
        {
            db = context;
        }

        // GET: BookController
        public ActionResult Index()
        {
            List<Book> books = db.Book.ToList();
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            Book book = db.Book.Where(x => x.Id == id).Single();
            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            List<Author> authors = db.Author.ToList();
            ViewBag.Authors = authors;
            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try { 
                var book = new Book();
                book.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                book.Date = DateTime.Now;

                book.Title = collection["Title"];
                book.AuthorId = 3;

                book.Description = collection["Description"];

                //Trzeba przyciąć do nazwy
                book.Image = "img";

                book.Year = int.Parse(collection["Year"]);
                book.City = collection["City"];

                db.Book.Add(book);
                db.SaveChanges();

                return View();
            }
            catch {
                return View();
            }
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            Book book = db.Book.Where(x => x.Id == id).Single();
            return View(book);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Book book = db.Book.Where(x => x.Id == id).Single();
                book.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                book.Date = DateTime.Now;

                book.Title = collection["Title"];
                book.AuthorId = 3;

                book.Description = collection["Description"];

                //Trzeba przyciąć do nazwy
                book.Image = "img";

                book.Year = int.Parse(collection["Year"]);
                book.City = collection["City"];

                db.Book.Update(book);
                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                db.Remove(db.Book.Where(x => x.Id == id).Single());
                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
