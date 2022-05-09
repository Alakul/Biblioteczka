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
        public ActionResult Index(int id)
        {/*
            var copyJoinBookJoinAuthor = (from copy in db.Copy
                                  join book in db.Book on copy.BookId equals book.Id
                                  join author in db.Author on book.AuthorId equals author.Id
                                  select new
                                  {
                                      Id = book.Id,
                                      Title = book.Title,
                                      Description = book.Description,
                                      Image = book.Image,
                                      Copies = book.Copies,
                                      Name = author.Name,
                                      LastName = author.LastName,
                                  }).ToList();
            */

            Book book = db.Book.Where(x => x.Id == id).Single();
            List<Copy> copies = db.Copy.Where(x => x.BookId == id).ToList();
            Author author = db.Author.Where(x => x.Id == book.AuthorId).Single();

            CopyViewModel copyViewModel = new CopyViewModel();
            copyViewModel.BookId = book.Id;
            copyViewModel.Title = book.Title;
            copyViewModel.Name = author.Name;
            copyViewModel.LastName = author.LastName;
            copyViewModel.Copies = copies;

            ViewBag.Categories = AppData.copyCategories;
            ViewBag.Sort = AppData.copySort;

            return View(copyViewModel);
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

            CopyViewModel copyViewModel = new CopyViewModel();
            copyViewModel.BookId = id;

            return View(copyViewModel);
        }

        // POST: CopyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, CopyViewModel model)
        {
            try
            {
                Copy copy = new Copy
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Date = DateTime.Now,

                    BookId = id,
                    Number = model.Number,
                    Status = model.Status,
                };

                db.Copy.Add(copy);
                db.SaveChanges();

                ViewData["Alert"] = "Success";
                GetCategories();
                return View();
            }
            catch
            {
                ViewData["Alert"] = "Danger";
                GetCategories();
                return View();
            }
        }

        // GET: CopyController/Edit/5
        public ActionResult Edit(int id)
        {
            GetCategories();
            Copy copy = db.Copy.Where(x => x.Id == id).Single();

            CopyViewModel copyViewModel = new CopyViewModel();
            copyViewModel.BookId = id;
            copyViewModel.Number = copy.Number;
            copyViewModel.Status = copy.Status;

            return View(copyViewModel);
        }

        // POST: CopyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CopyViewModel model)
        {
            try
            {
                Copy copy = db.Copy.Where(x => x.Id == id).Single();
                copy.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                copy.Date = DateTime.Now;
                copy.Number = model.Number;
                copy.Status = model.Status;

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
