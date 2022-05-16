using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace LibraryApp.Controllers
{
    public class AuthorController : Controller
    {
        AppDbContext db;
        public AuthorController(AppDbContext context)
        {
            db = context;
        }

        // GET: AuthorController
        public ActionResult Index(string searchString, string sortOrder, int? page)
        {
            var authors = db.Author.ToList();

            if (!string.IsNullOrEmpty(searchString)){
                searchString = searchString.Trim();
                authors = db.Author.Where(x => x.Name.Contains(searchString) || x.LastName.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "NameAsc":
                    authors = authors.OrderBy(x => x.Name).ToList();
                    break;
                case "NameDesc":
                    authors = authors.OrderByDescending(x => x.Name).ToList();
                    break;
                case "LastNameAsc":
                    authors = authors.OrderBy(x => x.LastName).ToList();
                    break;
                case "LastNameDesc":
                    authors = authors.OrderByDescending(x => x.LastName).ToList();
                    break;
                case "DateAsc":
                    authors = authors.OrderBy(x => x.Date).ToList();
                    break;
                case "DateDesc":
                    authors = authors.OrderByDescending(x => x.Date).ToList();
                    break;
                default:
                    authors = authors.OrderByDescending(x => x.Date).ToList();
                    break;
            }
            ViewBag.SearchString = searchString;
            ViewBag.SortOrder = sortOrder;

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(authors.ToPagedList(pageNumber, pageSize));
        }

        // GET: AuthorController/Details/5
        public ActionResult Details(int id)
        {
            BookAuthorViewModel bookAuthorViewModel = new BookAuthorViewModel();
            bookAuthorViewModel.Author = db.Author.Where(x => x.Id == id).Single();
            bookAuthorViewModel.Books = db.Book.Where(x => x.AuthorId == id).ToList();

            return View(bookAuthorViewModel);
        }

        // GET: AuthorController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Author author = db.Author.Where(x => x.Id == id).Single();
            return View(author);
        }

        // POST: AuthorController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorController/Delete/5
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                db.Remove(db.Author.Where(x => x.Id == id).Single());
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
