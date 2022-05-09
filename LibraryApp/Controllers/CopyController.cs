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
            GetCategories();

            CopyCreateEditViewModel copyCreateEditView = new CopyCreateEditViewModel();
            copyCreateEditView.BookId = id;

            return View(copyCreateEditView);
        }

        // POST: CopyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CopyCreateEditViewModel model)
        {
            try
            {///błąd
                Copy copy = new Copy
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Date = DateTime.Now,

                    BookId = model.BookId,
                    Number = model.Copy.Number,
                    Status = model.Copy.Status,
                };

                db.Copy.Add(copy);
                db.SaveChanges();

                ViewData["Alert"] = "Success";
                GetCategories();
                return View(model);
            }
            catch
            {
                ViewData["Alert"] = "Danger";
                GetCategories();
                return View(model);
            }
        }

        // GET: CopyController/Edit/5
        public ActionResult Edit(int id)
        {
            GetCategories();
            Copy copy = db.Copy.Where(x => x.Id == id).Single();

            CopyCreateEditViewModel copyViewModel = new CopyCreateEditViewModel();
            copyViewModel.BookId = copy.BookId;
            copyViewModel.Copy = copy;

            return View(copyViewModel);
        }

        // POST: CopyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CopyCreateEditViewModel model)
        {
            try
            {///błąd
                Copy copy = db.Copy.Where(x => x.Id == model.BookId).Single();
                copy.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                copy.Date = DateTime.Now;
                copy.Number = model.Copy.Number;
                copy.Status = model.Copy.Status;

                db.Copy.Update(copy);
                db.SaveChanges();

                ViewData["Alert"] = "Success";
                GetCategories();
                return View(model);
            }
            catch
            {
                ViewData["Alert"] = "Danger";
                GetCategories();
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

        private void GetCategories()
        {
            ViewBag.Categories = AppData.copyCategories;
        }
    }
}
