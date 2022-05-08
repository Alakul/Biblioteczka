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
            var bookJoinAuthor = (from book in db.Book
                                  join author in db.Author on book.AuthorId equals author.Id
                                  select new
                                  {
                                      Id = book.Id,
                                      Title = book.Title,
                                      Name = author.Name,
                                      LastName = author.LastName,
                                  }).ToList();

            ViewBag.Categories = AppData.bookCategories;
            ViewBag.Sort = AppData.bookSort;

            return View(bookJoinAuthor);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var bookJointAuthor = db.Book.Join(db.Author, book => book.AuthorId, author => author.Id,
                (book, author) => new {
                    Id = book.Id,
                    Title = book.Title,
                    Descriptiion = book.Description,
                    Image = book.Image,
                    Year = book.Year,
                    City = book.City,
                    Name = author.Name,
                    LastName = author.LastName }).Where(x => x.Id == id).Single();

            return View(bookJointAuthor);
        }


        // GET: BookController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(GetBookViewModel());
        }

        // POST: BookController/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookViewModel model)
        {
            try {
                string fileName = UploadedFile(model);
                if (fileName != null)
                {
                    Book book = new Book
                    {
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        Date = DateTime.Now,

                        Title = model.Title,
                        AuthorId = model.Author,
                        Description = model.Description,
                        Image = fileName,
                        Year = model.Year,
                        City = model.City
                    };

                    db.Book.Add(book);
                    db.SaveChanges();

                    ViewData["Alert"] = "Success";
                    return View(GetBookViewModel());
                }
                else
                {
                    ViewData["Alert"] = "Danger";
                    return View(GetBookViewModel());
                }
            }
            catch {
                ViewData["Alert"] = "Danger";
                return View(GetBookViewModel());
            }
        }

        // GET: BookController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Book book = db.Book.Where(x => x.Id == id).Single();

            BookViewModel bookViewModel = new BookViewModel();
            bookViewModel.Title = book.Title;
            bookViewModel.Description = book.Description;
            bookViewModel.Year = book.Year;
            bookViewModel.City = book.City;
            bookViewModel.Authors = db.Author.ToList();

            return View(bookViewModel);
        }

        // POST: BookController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookViewModel model)
        {
            try
            {
                Book book = db.Book.Where(x => x.Id == id).Single();
                book.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                book.Date = DateTime.Now;

                book.Title = model.Title;
                book.AuthorId = model.Author;

                book.Description = model.Description;

                //Trzeba przyciąć do nazwy
                book.Image = "img";

                book.Year = model.Year;
                book.City = model.City;

                db.Book.Update(book);
                db.SaveChanges();

                ViewData["Alert"] = "Success";
                return View(GetBookViewModel());
            }
            catch
            {
                ViewData["Alert"] = "Danger";
                return View(GetBookViewModel());
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


        private BookViewModel GetBookViewModel()
        {
            BookViewModel bookViewModel = new BookViewModel();
            bookViewModel.Authors = db.Author.ToList();

            return bookViewModel;
        }

        //NIE DZIAŁA
        private string UploadedFile(BookViewModel model)
        {
            string fileName=null;

            if (model.Image != null)
            {
                string destinationFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(destinationFolder);

                fileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(destinationFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            return fileName;
        }
    }
}
