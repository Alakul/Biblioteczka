using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using System.Security.Claims;
using System.Web;

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
        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {
            BookAuthorViewModel bookAuthorViewModel = new BookAuthorViewModel();
            List<Book> books = db.Book.ToList();

            CookieOptions options = new CookieOptions();

            if (formValue == null){
                if (Request.Cookies["SearchString"] != null){
                    books = db.Book.Where(x => x.Title.Contains(Request.Cookies["SearchString"])).ToList();
                    ViewBag.SearchString = Request.Cookies["SearchString"];
                }
                else {
                    books = db.Book.ToList();
                    ViewBag.SearchString = "";
                }
            }
            else if (searchString == null && formValue=="1"){
                books = db.Book.ToList();
                Response.Cookies.Delete("SearchString");
                ViewBag.SearchString = "";
            }
            else if (searchString != null && formValue == "1")
            {
                string newValue = searchString.Trim();
                if (!string.IsNullOrEmpty(newValue)){
                    if (newValue != Request.Cookies["SearchString"]){
                        books = db.Book.Where(x => x.Title.Contains(newValue)).ToList();
                        Response.Cookies.Append("SearchString", newValue, options);
                        ViewBag.SearchString = newValue;
                    }
                    else {
                        books = db.Book.Where(x => x.Title.Contains(newValue)).ToList();
                        ViewBag.SearchString = newValue;
                    }
                }
                else {
                    books = db.Book.ToList();
                    ViewBag.SearchString = "";
                }
            }



            if (sortOrder == null){
                string sort = Request.Cookies["SortOrder"];
                if (sort != null){
                    switch (sort){
                        case "TitleAsc":
                            books = books.OrderBy(x => x.Title).ToList();
                            break;
                        case "TitleDesc":
                            books = books.OrderByDescending(x => x.Title).ToList();
                            break;
                        case "YearAsc":
                            books = books.OrderBy(x => x.Year).ToList();
                            break;
                        case "YearDesc":
                            books = books.OrderByDescending(x => x.Year).ToList();
                            break;
                        case "DateAsc":
                            books = books.OrderBy(x => x.Date).ToList();
                            break;
                        case "DateDesc":
                            books = books.OrderByDescending(x => x.Date).ToList();
                            break;
                        default:
                            books = books.OrderByDescending(x => x.Date).ToList();
                            break;
                    }
                }
                else {
                    books = books.ToList();
                }
            }
            else if (sortOrder != null){
                switch (sortOrder){
                    case "TitleAsc":
                        books = books.OrderBy(x => x.Title).ToList();
                        break;
                    case "TitleDesc":
                        books = books.OrderByDescending(x => x.Title).ToList();
                        break;
                    case "YearAsc":
                        books = books.OrderBy(x => x.Year).ToList();
                        break;
                    case "YearDesc":
                        books = books.OrderByDescending(x => x.Year).ToList();
                        break;
                    case "DateAsc":
                        books = books.OrderBy(x => x.Date).ToList();
                        break;
                    case "DateDesc":
                        books = books.OrderByDescending(x => x.Date).ToList();
                        break;
                }
                Response.Cookies.Append("SortOrder", sortOrder, options);
            }

            bookAuthorViewModel.Authors = db.Author.ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            bookAuthorViewModel.BookList = books.ToPagedList(pageNumber, pageSize);
            return View(bookAuthorViewModel);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            BookAuthorCopyViewModel bookAuthorCopyViewModel = new BookAuthorCopyViewModel();
            bookAuthorCopyViewModel.Book = db.Book.Where(x => x.Id == id).Single();
            bookAuthorCopyViewModel.Author = db.Author.Where(x => x.Id == bookAuthorCopyViewModel.Book.AuthorId).Single();
            bookAuthorCopyViewModel.Copies = db.Copy.Where(x => x.BookId == id).ToList();

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

                if (model.File != null)
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
                return RedirectToAction("Index", "Book", new { @searchString = "null" });
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction("Index", "Book", new { @searchString = "null" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reservate(int id, IFormCollection collection)
        {
            Copy copy = db.Copy.Where(x => x.Id == id).Single();
            int bookId = copy.BookId;

            try
            {
                Reservation reservation = new Reservation
                {
                    Date = DateTime.Now,
                    UserBorrowingId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    BookId = bookId,
                    CopyId = id,
                };

                copy.Status = "0";
                db.Copy.Update(copy);
                db.Reservation.Add(reservation);
                db.SaveChanges();

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Details), new { @id = bookId });
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Details), new { @id = bookId });
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
