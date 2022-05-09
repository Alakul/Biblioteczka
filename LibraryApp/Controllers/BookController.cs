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
                    Date = book.Date,
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
            return View(GetAuthors());
        }

        // POST: BookController/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookViewModel model)
        {
            try {
                string newFileName = UploadFile(model);
                if (newFileName != null)
                {
                    Book book = new Book
                    {
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        Date = DateTime.Now,

                        Title = model.Title,
                        AuthorId = model.AuthorId,
                        Description = model.Description,
                        Image = newFileName,
                        Year = model.Year,
                        City = model.City
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
        public ActionResult Edit(int id, BookViewModel model)
        {
            try
            {
                Book book = db.Book.Where(x => x.Id == id).Single();
                book.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                book.Date = DateTime.Now;

                book.Title = model.Title;
                book.AuthorId = model.AuthorId;
                book.Description = model.Description;
                book.Year = model.Year;
                book.City = model.City;

                if (model.Image != null)
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



        private BookViewModel GetBook(int id)
        {
            Book book = db.Book.Where(x => x.Id == id).Single();

            BookViewModel bookViewModel = new BookViewModel();
            bookViewModel.Date = book.Date;
            bookViewModel.Title = book.Title; 
            bookViewModel.AuthorId = book.AuthorId;
            bookViewModel.Description = book.Description;
            bookViewModel.Image = book.Image;
            bookViewModel.Year = book.Year;
            bookViewModel.City = book.City;
            bookViewModel.Authors = db.Author.ToList();

            return bookViewModel;
        }
        private BookViewModel GetAuthors()
        {
            BookViewModel bookViewModel = new BookViewModel();
            bookViewModel.Authors = db.Author.ToList();

            return bookViewModel;
        }

        private string UploadFile(BookViewModel model)
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
