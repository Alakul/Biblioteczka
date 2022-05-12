using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public ActionResult Index(string searchString)
        {
            List<Author> authors = db.Author.ToList();

            if (!string.IsNullOrEmpty(searchString)){
                searchString = searchString.Trim();
                authors = db.Author.Where(x => x.Name.Contains(searchString) || x.LastName.Contains(searchString)).ToList();
            }

            ViewBag.Sort = AppData.authorSort;

            return View(authors);
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

                return RedirectToAction(nameof(Index));
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
