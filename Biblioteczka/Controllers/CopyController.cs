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
        private readonly AppDbContext db;
        public CopyController(AppDbContext context)
        {
            db = context;
        }

        // GET: CopyController
        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {
            CopyViewModel copyBookAuthorViewModel = new CopyViewModel();
            List<CopyViewModel> copies = GetCopyList();

            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            AppMethods appMethods = new AppMethods();
            var tuple = appMethods.Search(httpContextAccessor, copies, "SearchStringCopy", formValue, searchString);
            copies = tuple.Item1;
            ViewBag.SearchString = tuple.Item2;
            copies = appMethods.Sort(httpContextAccessor, copies, "SortOrderCopy", sortOrder);

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
            CopyCreateEditViewModel copyCreateEditViewModel = new CopyCreateEditViewModel();
            copyCreateEditViewModel.BookId = id;

            GetList();
            return View(copyCreateEditViewModel);
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

                GetList();
                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Create));
            }
            catch
            {
                GetList();
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Create));
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

                GetList();
                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Edit));
            }
            catch
            {
                GetList();
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Edit));
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
                return RedirectToAction(nameof(Details), "Book", new { @id = copy.BookId });
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Details), "Book", new { @id = copy.BookId });
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

        private void GetList()
        {
            ViewBag.Categories = AppData.copyCategories;
        }
    }
}