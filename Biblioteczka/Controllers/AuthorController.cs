using Biblioteczka.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace Biblioteczka.Controllers
{
    public class AuthorController : Controller
    {
        private AppDbContext db;
        public AuthorController(AppDbContext context)
        {
            db = context;
        }

        // GET: AuthorController
        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {
            List<Author> authors = db.Author.ToList();
            authors = SearchAuthors(formValue, searchString, authors);
            authors = SortAuthors(sortOrder, authors);

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(authors.ToPagedList(pageNumber, pageSize));
        }

        // GET: AuthorController/Details/5
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










        private List<Author> SearchAuthors(string formValue, string searchString, List<Author> authors)
        {
            CookieOptions options = new CookieOptions();
            string cookie = Request.Cookies["SearchStringAuthor"];

            if (formValue == null){
                if (cookie != null){
                    authors = authors.Where(x => x.Name.ToLower().Contains(cookie.ToLower()) ||
                        x.LastName.ToLower().Contains(cookie.ToLower())).ToList();
                    ViewBag.SearchString = cookie;
                }
                else {
                    authors = db.Author.ToList();
                    ViewBag.SearchString = "";
                }
            }
            else if (searchString == null && formValue == "1"){
                authors = db.Author.ToList();
                Response.Cookies.Delete("SearchStringAuthor");
                ViewBag.SearchString = "";
            }
            else if (searchString != null && formValue == "1"){
                string newValue = searchString.Trim();
                if (!string.IsNullOrEmpty(newValue)){
                    if (newValue != cookie){
                        Response.Cookies.Append("SearchStringAuthor", newValue, options);
                    }
                    authors = authors.Where(x => x.Name.ToLower().Contains(newValue.ToLower()) ||
                        x.LastName.ToLower().Contains(newValue.ToLower())).ToList();
                    ViewBag.SearchString = newValue;
                }
                else
                {
                    authors = db.Author.ToList();
                    ViewBag.SearchString = "";
                }
            }

            return authors;
        }

        //SORT
        private List<Author> SortAuthors(string sortOrder, List<Author> authors)
        {
            CookieOptions options = new CookieOptions();

            if (sortOrder == null){
                string cookie = Request.Cookies["SortOrderAuthor"];
                if (cookie != null){
                    authors = GetAuthors(cookie, authors);
                }
                else{
                    authors = db.Author.ToList();
                }
            }
            else if (sortOrder != null){
                authors = GetAuthors(sortOrder, authors);
                if (sortOrder != null)
                {
                    Response.Cookies.Append("SortOrderAuthor", sortOrder, options);
                }
            }

            return authors;
        }

        private List<Author> GetAuthors(string sort, List<Author> authors)
        {
            switch (sort)
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
            return authors;
        }
    }
}
