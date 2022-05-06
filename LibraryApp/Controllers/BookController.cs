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
            return View();
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
           
                var book = new Book();
                book.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                book.Date = DateTime.Now;

                book.Title = collection["Title"];
                book.AuthorId = 2;

                book.Description = collection["Description"];

                //Trzeba przyciąć do nazwy
                book.Image = "img";

                book.Year = int.Parse(collection["Year"]);
                book.City = collection["City"];

                db.Book.Add(book);
                db.SaveChanges();

                return RedirectToAction(nameof(Index));
        
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                db.Remove(db.Book.Where(x => x.Id == id).Single());
                db.SaveChanges();

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
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
