using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Authorization;
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
            var bookJoinAuthor = (from book in db.Book
                                  join author in db.Author on book.AuthorId equals author.Id
                                  select new
                                  {
                                      Id = book.Id,
                                      Title = book.Title,
                                      Name = author.Name,
                                      LastName = author.LastName,
                                  }).ToList();

            ViewBag.Categories = AppData.bookCategories;
            ViewBag.Sort = AppData.bookSort;

            return View(bookJoinAuthor);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var bookJointAuthor = db.Book.Join(db.Author, book => book.AuthorId, author => author.Id,
                (book, author) => new {
                    Id = book.Id,
                    Title = book.Title,
                    Descriptiion = book.Description,
                    Image = book.Image,
                    Year = book.Year,
                    City = book.City,
                    Name = author.Name,
                    LastName = author.LastName }).Where(x => x.Id == id).Single();

            return View(bookJointAuthor);
        }


        // GET: BookController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            List<Author> authors = db.Author.ToList();
            return View(authors);
        }

        // POST: BookController/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            List<Author> authors = db.Author.ToList();

            try {
                Book book = new Book
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Date = DateTime.Now,

                    Title = collection["Title"],
                    AuthorId = int.Parse(collection["Author"]),
                    Description = collection["Description"],

                    //Trzeba przyciąć do nazwy
                    Image = "img",

                    Year = int.Parse(collection["Year"]),
                    City = collection["City"]
                };

                db.Book.Add(book);
                db.SaveChanges();

                return View(authors);
            }
            catch {

                return View(authors);
            }
        }

        // GET: BookController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Book book = db.Book.Where(x => x.Id == id).Single();
            return View(book);
        }

        // POST: BookController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            List<Author> authors = db.Author.ToList();

            try
            {
                Book book = db.Book.Where(x => x.Id == id).Single();
                book.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                book.Date = DateTime.Now;

                book.Title = collection["Title"];
                book.AuthorId = int.Parse(collection["Author"]);

                book.Description = collection["Description"];

                //Trzeba przyciąć do nazwy
                book.Image = "img";

                book.Year = int.Parse(collection["Year"]);
                book.City = collection["City"];

                db.Book.Update(book);
                db.SaveChanges();

                return View(authors);
            }
            catch
            {
                return View(authors);
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
        [Authorize(Roles = "Admin")]
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
