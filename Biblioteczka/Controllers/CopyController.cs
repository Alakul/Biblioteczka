using Biblioteczka.Areas.Identity.Data;
using Biblioteczka.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace Biblioteczka.Controllers
{
    [Route("Egzemplarze")]
    public class CopyController : Controller
    {
        private const string role = AppData.Admin + "," + AppData.Librarian;
        private readonly AppDbContext db;
        public CopyController(AppDbContext context)
        {
            db = context;
        }

        // GET: CopyController
        [Authorize(Roles = role)]
        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {
            CopyViewModel copyViewModel = new CopyViewModel();
            List<CopyViewModel> copies = GetCopyList();

            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var tuple = AppMethods.Search(httpContextAccessor, copies, "SearchStringCopy", formValue, searchString);
            copies = tuple.Item1;
            ViewBag.SearchString = tuple.Item2;
            copies = AppMethods.Sort(httpContextAccessor, copies, "SortOrderCopy", sortOrder);

            ViewData["Selected"] = AppMethods.SetViewData(httpContextAccessor, sortOrder, "SortOrderCopy", "DateDesc");
            ViewBag.Values = AppData.copySort;

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            copyViewModel.CopyList = copies.ToPagedList(pageNumber, pageSize);
            return View(copyViewModel);
        }

        // GET: CopyController/Details/5
        [Route("Szczegoly/{id}")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CopyController/Create
        [Authorize(Roles = role)]
        [Route("Dodaj/{id}")]
        public ActionResult Create(int id)
        {
            CopyCreateEditViewModel copyCreateEditViewModel = new CopyCreateEditViewModel();
            copyCreateEditViewModel.BookId = id;

            GetStatusList();
            return View(copyCreateEditViewModel);
        }

        // POST: CopyController/Create
        [Authorize(Roles = role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Dodaj/{id}")]
        public ActionResult Create(int id, CopyCreateEditViewModel model)
        {
            GetStatusList();

            if (ModelState.IsValid)
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

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Create));
            }
            else
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: CopyController/Edit/5
        [Authorize(Roles = role)]
        [Route("Edytuj/{id}")]
        public ActionResult Edit(int id)
        {
            Copy copy = db.Copy.Where(x => x.Id == id).Single();

            CopyCreateEditViewModel copyViewModel = new CopyCreateEditViewModel();
            copyViewModel.BookId = copy.BookId;
            copyViewModel.Copy = copy;

            GetStatusList();
            return View(copyViewModel);
        }

        // POST: CopyController/Edit/5
        [Authorize(Roles = role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edytuj/{id}")]
        public ActionResult Edit(int id, CopyCreateEditViewModel model)
        {
            GetStatusList();

            if (ModelState.IsValid)
            {
                Copy copy = db.Copy.Where(x => x.Id == id).Single();
                copy.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                copy.Date = DateTime.Now;
                copy.BookId = copy.BookId;
                copy.Number = model.Copy.Number;
                copy.Status = model.Copy.Status;

                db.Copy.Update(copy);
                db.SaveChanges();

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Edit));
            }
            else
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Edit));
            }
        }

        // GET: CopyController/Delete/5
        [Route("Usun/{id}")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CopyController/Delete/5
        [Authorize(Roles = role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Usun/{id}")]
        public ActionResult Delete(int id, int pageIndex, IFormCollection collection)
        {
            Copy copy = db.Copy.Where(x => x.Id == id).Single();

            try
            {
                db.Remove(db.Copy.Where(x => x.Id == id).Single());
                db.SaveChanges();

                TempData["Alert"] = "Success";

                if (pageIndex == 1){
                    return RedirectToAction(nameof(Index));
                }
                else {
                    return RedirectToAction(nameof(Details), "Book", new { @id = copy.BookId });
                }  
            }
            catch
            {
                TempData["Alert"] = "Danger";
                if (pageIndex == 1){
                    return RedirectToAction(nameof(Index));
                }
                else {
                    return RedirectToAction(nameof(Details), "Book", new { @id = copy.BookId });
                }
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
        private void GetStatusList()
        {
            ViewBag.Status = AppData.copyStatus;
        }
    }
}