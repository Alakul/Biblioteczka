using Biblioteczka.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace Biblioteczka.Controllers
{
    [Route("Autorzy")]
    public class AuthorController : Controller
    {
        private const string role = AppData.Admin + "," + AppData.Librarian;
        private readonly AppDbContext db;
        public AuthorController(AppDbContext context)
        {
            db = context;
        }

        // GET: AuthorController
        [Authorize(Roles = role)]
        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {
            List<Author> authors = db.Author.ToList();

            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var tuple = AppMethods.Search(httpContextAccessor, authors, "SearchStringAuthor", formValue, searchString);
            authors = tuple.Item1;
            ViewBag.SearchString = tuple.Item2;
            authors = AppMethods.Sort(httpContextAccessor, authors, "SortOrderAuthor", sortOrder);

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(authors.ToPagedList(pageNumber, pageSize));
        }

        // GET: AuthorController/Details/5
        [Route("Szczegoly/{id}")]
        public ActionResult Details(int id, int? page)
        {
            BookViewModel bookAuthorViewModel = new BookViewModel();
            List<BookViewModel> books = db.Book
                .Join(db.Author, z => z.AuthorId, k => k.Id, (z, k) => new { Book = z, Author = k })
                .Where(x => x.Book.AuthorId==id)
                .Select(s => new BookViewModel
                {
                    Book = s.Book,
                    Author = s.Author,
                })
                .ToList();

            bookAuthorViewModel.Author = db.Author.Where(x => x.Id == id).Single();
            ViewBag.Id = id;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            bookAuthorViewModel.BookList = books.ToPagedList(pageNumber, pageSize);
            return View(bookAuthorViewModel);
        }

        // GET: AuthorController/Create
        [Authorize(Roles = role)]
        [Route("Dodaj")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [Authorize(Roles = role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Dodaj")]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var author = new Author();
                author.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                author.Date = DateTime.Now;

                author.Name = collection["Name"];
                author.LastName = collection["LastName"];

                db.Author.Add(author);
                db.SaveChanges();

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Create));
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: AuthorController/Edit/5
        [Authorize(Roles = role)]
        [Route("Edytuj/{id}")]
        public ActionResult Edit(int id)
        {
            Author author = db.Author.Where(x => x.Id == id).Single();
            return View(author);
        }

        // POST: AuthorController/Edit/5
        [Authorize(Roles = role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edytuj/{id}")]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Author author = db.Author.Where(x => x.Id == id).Single();

                author.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                author.Date = DateTime.Now;

                author.Name = collection["Name"];
                author.LastName = collection["LastName"];

                db.Author.Update(author);
                db.SaveChanges();

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Edit));
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Edit));
            }
        }

        // GET: AuthorController/Delete/5
        [Route("Usun/{id}")]
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

        // POST: AuthorController/Delete/5
        [Authorize(Roles = role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Usun/{id}")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                db.Remove(db.Author.Where(x => x.Id == id).Single());
                db.SaveChanges();

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
