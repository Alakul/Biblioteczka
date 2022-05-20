using Biblioteczka.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using System.Security.Claims;
using System.Web;
using System.Collections.ObjectModel;

namespace Biblioteczka.Controllers
{
    public class BookController : Controller
    {
        private AppDbContext db;
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
            List<BookAuthorViewModel> books = GetBookList();

            books = SearchBooks(formValue, searchString, books);
            books = SortBooks(sortOrder, books);

            int pageSize = 20;
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
        [ValidateAntiForgeryToken]
        [HttpPost]
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
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(int id, BookCreateEditViewModel model)
        {
            try
            {
                Book book = db.Book.Where(x => x.Id == id).Single();
                book.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                book.Date = DateTime.Now;

                book.Title = model.Book.Title.Trim();
                book.AuthorId = model.Book.AuthorId;
                book.Description = model.Book.Description.Trim();
                book.Year = model.Book.Year;
                book.City = model.Book.City.Trim();

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
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction("Index");
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






        private List<BookAuthorViewModel> GetBookList()
        {
            List<BookAuthorViewModel> books = db.Book
                .Join(db.Author, z => z.AuthorId, k => k.Id, (z, k) => new { Book = z, Author = k })
                .Select(s => new BookAuthorViewModel
                {
                    Book = s.Book,
                    Author = s.Author,
                })
                .ToList();

            return books;
        }

        //SEARCH
        private List<BookAuthorViewModel> SearchBooks(string formValue, string searchString, List<BookAuthorViewModel> books)
        {
            CookieOptions options = new CookieOptions();
            string cookie = Request.Cookies["SearchStringBook"];

            if (formValue == null){
                if (cookie != null){
                    books = books.Where(x => x.Book.Title.ToLower().Contains(cookie.ToLower()) || 
                        x.Author.Name.ToLower().Contains(cookie.ToLower()) || 
                        x.Author.LastName.ToLower().Contains(cookie.ToLower())).ToList();
                    ViewBag.SearchString = cookie;
                }
                else {
                    books = GetBookList();
                    ViewBag.SearchString = "";
                }
            }
            else if (searchString == null && formValue == "1"){
                books = GetBookList();
                Response.Cookies.Delete("SearchStringBook");
                ViewBag.SearchString = "";
            }
            else if (searchString != null && formValue == "1"){
                string newValue = searchString.Trim();
                if (!string.IsNullOrEmpty(newValue)){
                    if (newValue != cookie){
                        Response.Cookies.Append("SearchStringBook", newValue, options);
                    }
                    books = books.Where(x => x.Book.Title.ToLower().Contains(newValue.ToLower()) || 
                        x.Author.Name.ToLower().Contains(newValue.ToLower()) || 
                        x.Author.LastName.ToLower().Contains(newValue.ToLower())).ToList();
                    ViewBag.SearchString = newValue;
                }
                else {
                    books = GetBookList();
                    ViewBag.SearchString = "";
                }
            }

            return books;
        }

        //SORT
        private List<BookAuthorViewModel> SortBooks(string sortOrder, List<BookAuthorViewModel> books)
        {
            CookieOptions options = new CookieOptions();

            if (sortOrder == null){
                string cookie = Request.Cookies["SortOrderBook"];
                if (cookie != null){
                    books = GetBooks(cookie, books);
                }
                else {
                    books = GetBookList();
                }
            }
            else if (sortOrder != null){
                books = GetBooks(sortOrder, books);
                if (sortOrder != null){
                    Response.Cookies.Append("SortOrderBook", sortOrder, options);
                }
            }

            return books;
        }

        //SWITCH
        private List<BookAuthorViewModel> GetBooks(string sort, List<BookAuthorViewModel> books)
        {
            switch (sort)
            {
                case "TitleAsc":
                    books = books.OrderBy(x => x.Book.Title).ToList();
                    break;
                case "TitleDesc":
                    books = books.OrderByDescending(x => x.Book.Title).ToList();
                    break;
                case "LastNameAsc":
                    books = books.OrderBy(x => x.Author.LastName).ToList();
                    break;
                case "LastNameDesc":
                    books = books.OrderByDescending(x => x.Author.LastName).ToList();
                    break;
                case "YearAsc":
                    books = books.OrderBy(x => x.Book.Year).ToList();
                    break;
                case "YearDesc":
                    books = books.OrderByDescending(x => x.Book.Year).ToList();
                    break;
                case "DateAsc":
                    books = books.OrderBy(x => x.Book.Date).ToList();
                    break;
                case "DateDesc":
                    books = books.OrderByDescending(x => x.Book.Date).ToList();
                    break;
                default:
                    books = books.OrderByDescending(x => x.Book.Date).ToList();
                    break;
            }
            return books;
        }

        //VIEWMODEL
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

        //FILE
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
