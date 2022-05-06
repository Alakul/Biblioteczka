using LibraryApp.Data;
using LibraryApp.Models;
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
        public ActionResult Index()
        {
            List<Author> authors = db.Author.ToList();
            return View(authors);
        }

        // GET: AuthorController/Details/5
        public ActionResult Details(int id)
        {
            Author author = db.Author.Where(x => x.Id == id).Single();
            return View(author);
        }

        // GET: AuthorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
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
        public ActionResult Edit(int id)
        {
            Author author = db.Author.Where(x => x.Id == id).Single();
            return View(author);
        }

        // POST: AuthorController/Edit/5
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
                db.Remove(db.Author.Where(x => x.Id == id).Single());
                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

            
        }

        // POST: AuthorController/Delete/5
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
