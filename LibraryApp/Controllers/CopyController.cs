using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApp.Controllers
{
    public class CopyController : Controller
    {
        AppDbContext db;
        public CopyController(AppDbContext context)
        {
            db = context;
        }

        // GET: CopyController
        public ActionResult Index()
        {
            return View();
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
            GetList();
            Copy copy = db.Copy.Where(x => x.Id == id).Single();

            CopyCreateEditViewModel copyViewModel = new CopyCreateEditViewModel();
            copyViewModel.BookId = copy.BookId;
            copyViewModel.Copy = copy;

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
            try
            {
                Copy copy = db.Copy.Where(x => x.Id == id).Single();
                db.Remove(copy);
                db.SaveChanges();

                TempData["Alert"] = "Success";
                return RedirectToAction("Index", new { @id = copy.BookId });
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Index));
            }
        }


        private CopyCreateEditViewModel GetBook(int id)
        {
            CopyCreateEditViewModel copyCreateEditViewModel = new CopyCreateEditViewModel();
            copyCreateEditViewModel.BookId = id;

            return copyCreateEditViewModel;
        }
        private void GetList()
        {
            ViewBag.Sort = AppData.copySort;
            ViewBag.Categories = AppData.copyCategories;
        }
    }
}
