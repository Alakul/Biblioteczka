using Biblioteczka.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace Biblioteczka.Controllers
{
    public class CopyController : Controller
    {
        AppDbContext db;
        public CopyController(AppDbContext context)
        {
            db = context;
        }

        // GET: CopyController
        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {
            CopyViewModel copyBookAuthorViewModel = new CopyViewModel();
            List<CopyViewModel> copies = GetCopyList();

            copies = SearchCopies(formValue, searchString, copies);
            copies = SortCopies(sortOrder, copies);

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            copyBookAuthorViewModel.CopyList = copies.ToPagedList(pageNumber, pageSize);
            return View(copyBookAuthorViewModel);
        }

        // GET: CopyController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CopyController/Create
        public ActionResult Create(int id)
        {
            GetList();
            return View(GetBook(id));
        }

        // POST: CopyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, CopyCreateEditViewModel model)
        {
            try
            {
                Copy copy = new Copy
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Date = DateTime.Now,

                    BookId = id,
                    Number = model.Copy.Number,
                    Status = model.Copy.Status,
                };

                db.Copy.Add(copy);
                db.SaveChanges();

                ViewData["Alert"] = "Success";
                GetList();
                return View(GetBook(model.BookId));
            }
            catch
            {
                ViewData["Alert"] = "Danger";
                GetList();
                return View(GetBook(model.BookId));
            }
        }

        // GET: CopyController/Edit/5
        public ActionResult Edit(int id)
        {
            Copy copy = db.Copy.Where(x => x.Id == id).Single();

            CopyCreateEditViewModel copyViewModel = new CopyCreateEditViewModel();
            copyViewModel.BookId = copy.BookId;
            copyViewModel.Copy = copy;

            GetList();
            return View(copyViewModel);
        }

        // POST: CopyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CopyCreateEditViewModel model)
        {
            try
            {
                Copy copy = db.Copy.Where(x => x.Id == id).Single();
                copy.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                copy.Date = DateTime.Now;
                copy.BookId = copy.BookId;
                copy.Number = model.Copy.Number;
                copy.Status = model.Copy.Status;

                db.Copy.Update(copy);
                db.SaveChanges();

                ViewData["Alert"] = "Success";
                GetList();
                return View(model);
            }
            catch
            {
                ViewData["Alert"] = "Danger";
                GetList();
                return View(model);
            }
        }

        // GET: CopyController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CopyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            Copy copy = db.Copy.Where(x => x.Id == id).Single();

            try
            {
                db.Remove(copy);
                db.SaveChanges();

                TempData["Alert"] = "Success";
                return RedirectToAction("Details", "Book", new { @id = copy.BookId });
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction("Details", "Book", new { @id = copy.BookId });
            }
        }



        private List<CopyViewModel> GetCopyList()
        {
            List<CopyViewModel> copies = db.Book
                .Join(db.Copy, a => a.Id, b => b.BookId, (a, b) => new { book = a, copy = b })
                .Join(db.Author, joined => joined.book.AuthorId, author => author.Id,
                (joined, author) => new CopyViewModel
                {
                    Book = joined.book,
                    Copy = joined.copy,
                    Author = author,
                })
                .ToList();

            return copies;
        }

        //SEARCH
        private List<CopyViewModel> SearchCopies(string formValue, string searchString, List<CopyViewModel> copies)
        {
            CookieOptions options = new CookieOptions();
            string cookie = Request.Cookies["SearchStringCopy"];

            if (formValue == null)
            {
                if (cookie != null)
                {
                    copies = copies.Where(x => x.Copy.Number.ToString().ToLower().Contains(cookie.ToLower()) || x.Book.Title.ToLower().Contains(cookie.ToLower()) ||
                        x.Author.Name.ToLower().Contains(cookie.ToLower()) ||
                        x.Author.LastName.ToLower().Contains(cookie.ToLower())).ToList();
                    ViewBag.SearchString = cookie;
                }
                else
                {
                    copies = GetCopyList();
                    ViewBag.SearchString = "";
                }
            }
            else if (searchString == null && formValue == "1")
            {
                copies = GetCopyList();
                Response.Cookies.Delete("SearchStringCopy");
                ViewBag.SearchString = "";
            }
            else if (searchString != null && formValue == "1")
            {
                string newValue = searchString.Trim();
                if (!string.IsNullOrEmpty(newValue))
                {
                    if (newValue != cookie)
                    {
                        Response.Cookies.Append("SearchStringCopy", newValue, options);
                    }
                    copies = copies.Where(x => x.Copy.Number.ToString().ToLower().Contains(newValue.ToLower()) || x.Book.Title.ToLower().Contains(newValue.ToLower()) ||
                        x.Author.Name.ToLower().Contains(newValue.ToLower()) ||
                        x.Author.LastName.ToLower().Contains(newValue.ToLower())).ToList();
                    ViewBag.SearchString = newValue;
                }
                else
                {
                    copies = GetCopyList();
                    ViewBag.SearchString = "";
                }
            }

            return copies;
        }

        //SORT
        private List<CopyViewModel> SortCopies(string sortOrder, List<CopyViewModel> copies)
        {
            CookieOptions options = new CookieOptions();

            if (sortOrder == null){
                string cookie = Request.Cookies["SortOrderCopy"];
                if (cookie != null){
                    copies = GetCopies(cookie, copies);
                }
                else {
                    copies = GetCopyList();
                }
            }
            else if (sortOrder != null){
                copies = GetCopies(sortOrder, copies);
                if (sortOrder != null){
                    Response.Cookies.Append("SortOrderCopy", sortOrder, options);
                }
            }

            return copies;
        }

        //SWITCH
        private List<CopyViewModel> GetCopies(string sort, List<CopyViewModel> copies)
        {
            switch (sort)
            {
                case "NumberAsc":
                    copies = copies.OrderBy(x => x.Copy.Number).ToList();
                    break;
                case "NumberDesc":
                    copies = copies.OrderByDescending(x => x.Copy.Number).ToList();
                    break;
                case "TitleAsc":
                    copies = copies.OrderBy(x => x.Book.Title).ToList();
                    break;
                case "TitleDesc":
                    copies = copies.OrderByDescending(x => x.Book.Title).ToList();
                    break;
                case "LastNameAsc":
                    copies = copies.OrderBy(x => x.Author.LastName).ToList();
                    break;
                case "LastNameDesc":
                    copies = copies.OrderByDescending(x => x.Author.LastName).ToList();
                    break;
                case "DateAsc":
                    copies = copies.OrderBy(x => x.Copy.Date).ToList();
                    break;
                case "DateDesc":
                    copies = copies.OrderByDescending(x => x.Copy.Date).ToList();
                    break;
                default:
                    copies = copies.OrderByDescending(x => x.Copy.Date).ToList();
                    break;
            }
            return copies;
        }

        private CopyCreateEditViewModel GetBook(int id)
        {
            CopyCreateEditViewModel copyCreateEditViewModel = new CopyCreateEditViewModel();
            copyCreateEditViewModel.BookId = id;

            return copyCreateEditViewModel;
        }
        private void GetList()
        {
            ViewBag.Categories = AppData.copyCategories;
        }
    }
}