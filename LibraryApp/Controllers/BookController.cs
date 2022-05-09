using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApp.Controllers
{
    public class BookController : Controller
    {
        AppDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BookController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: BookController
        public ActionResult Index()
        {
            BookAuthorViewModel bookAuthorViewModel = new BookAuthorViewModel();
            bookAuthorViewModel.Books = db.Book.ToList();
            bookAuthorViewModel.Authors = db.Author.ToList();

            ViewBag.Categories = AppData.bookCategories;
            ViewBag.Sort = AppData.bookSort;

            return View(bookAuthorViewModel);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            BookAuthorCopyViewModel bookAuthorCopyViewModel = new BookAuthorCopyViewModel();
            bookAuthorCopyViewModel.Book = db.Book.Where(x => x.Id == id).Single();
            bookAuthorCopyViewModel.Author = db.Author.Where(x => x.Id == bookAuthorCopyViewModel.Book.AuthorId).Single();
            bookAuthorCopyViewModel.Copies = db.Copy.Where(x => x.BookId == id).ToList();

            ViewBag.Categories = AppData.copyCategories;
            ViewBag.Sort = AppData.copySort;

            return View(bookAuthorCopyViewModel);
        }

        // GET: BookController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(GetAuthors());
        }

        // POST: BookController/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookCreateEditViewModel model)
        {
            try {
                string newFileName = UploadFile(model);
                if (newFileName != null)
                {
                    Book book = new Book
                    {
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        Date = DateTime.Now,

                        Title = model.Book.Title,
                        AuthorId = model.Book.AuthorId,
                        Description = model.Book.Description,
                        Image = newFileName,
                        Year = model.Book.Year,
                        City = model.Book.City
                    };

                    db.Book.Add(book);
                    db.SaveChanges();

                    ViewData["Alert"] = "Success";
                    return View(GetAuthors());
                }
                else
                {
                    ViewData["Alert"] = "Danger";
                    return View(GetAuthors());
                }
            }
            catch {
                ViewData["Alert"] = "Danger";
                return View(GetAuthors());
            }
        }

        // GET: BookController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View(GetBook(id));
        }

        // POST: BookController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookCreateEditViewModel model)
        {
            try
            {
                Book book = db.Book.Where(x => x.Id == id).Single();
                book.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                book.Date = DateTime.Now;

                book.Title = model.Book.Title;
                book.AuthorId = model.Book.AuthorId;
                book.Description = model.Book.Description;
                book.Year = model.Book.Year;
                book.City = model.Book.City;

                if (model.Book.Image != null)
                {
                    DeleteFile(book.Image);
                    string newFileName = UploadFile(model);
                    book.Image = newFileName;
                }

                db.Book.Update(book);
                db.SaveChanges();

                ViewData["Alert"] = "Success";
                return View(GetBook(id));
            }
            catch
            {
                ViewData["Alert"] = "Danger";
                return View(GetBook(id));
            }
        }

        // GET: BookController/Delete/5
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

        // POST: BookController/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Book book = db.Book.Where(x => x.Id == id).Single();
                string fileName = book.Image;
                DeleteFile(fileName);

                db.Remove(db.Book.Where(x => x.Id == id).Single());
                db.SaveChanges();

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Index));
            }
        }



        private BookCreateEditViewModel GetBook(int id)
        {
            Book book = db.Book.Where(x => x.Id == id).Single();

            BookCreateEditViewModel bookCreateEditViewModel = new BookCreateEditViewModel();
            bookCreateEditViewModel.Book = book;
            bookCreateEditViewModel.Authors = db.Author.ToList();

            return bookCreateEditViewModel;
        }
        private BookCreateEditViewModel GetAuthors()
        {
            BookCreateEditViewModel bookCreateEditViewModel = new BookCreateEditViewModel();
            bookCreateEditViewModel.Authors = db.Author.ToList();

            return bookCreateEditViewModel;
        }

        private string UploadFile(BookCreateEditViewModel model)
        {
            string fileName = null;

            if (model.File != null)
            {
                string destinationFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(destinationFolder);

                string fileExtension = model.File.FileName;
                fileName = Guid.NewGuid().ToString() + fileExtension.Substring(fileExtension.LastIndexOf('.'));
                string filePath = Path.Combine(destinationFolder, fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    model.File.CopyTo(stream);
                }
            }
            return fileName;
        }

        private void DeleteFile(string newFileName)
        {
            string destinationFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
            string filePath = Path.Combine(destinationFolder, newFileName);

            if (System.IO.File.Exists(filePath)){
                System.IO.File.Delete(filePath);
            }
        }
    }
}
