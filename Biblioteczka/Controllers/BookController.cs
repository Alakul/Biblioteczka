using Biblioteczka.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using System.Security.Claims;
using System.Web;
using System.Collections.ObjectModel;
using Biblioteczka.Areas.Identity.Data;

namespace Biblioteczka.Controllers
{
    [Route("Ksiazki")]
    public class BookController : Controller
    {
        private const string role = AppData.Admin + "," + AppData.Librarian;
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BookController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: BookController
        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {
            BookViewModel bookAuthorViewModel = new BookViewModel();
            List<BookViewModel> books = GetBookList();

            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var tuple = AppMethods.Search(httpContextAccessor, books, "SearchStringBook", formValue, searchString);
            books = tuple.Item1;
            ViewBag.SearchString = tuple.Item2;
            books = AppMethods.Sort(httpContextAccessor, books, "SortOrderBook", sortOrder);

            ViewData["Selected"] = AppMethods.SetViewData(httpContextAccessor, sortOrder, "SortOrderBook", "DateDesc");
            ViewBag.Values = AppData.bookSort;

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            bookAuthorViewModel.BookList = books.ToPagedList(pageNumber, pageSize);
            return View(bookAuthorViewModel);
        }

        // GET: BookController/Details/5
        [Route("Szczegoly/{id}")]
        public ActionResult Details(int id, int? page)
        {
            List<Copy> copies = db.Copy.Where(x => x.BookId == id).ToList();
            BookViewModel bookViewModel = new BookViewModel();
            bookViewModel.Book = db.Book.Where(x => x.Id == id).Single();
            bookViewModel.Author = db.Author.Where(x => x.Id == bookViewModel.Book.AuthorId).Single();
            ViewBag.Id = id;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            bookViewModel.CopyList = copies.ToPagedList(pageNumber, pageSize);         
            return View(bookViewModel);
        }

        // GET: BookController/Create
        [Authorize(Roles = role)]
        [Route("Dodaj")]
        public ActionResult Create()
        {
            return View(GetAuthors());
        }

        // POST: BookController/Create
        
        [Authorize(Roles = role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Dodaj")]
        public ActionResult Create(BookCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string newFileName = AppMethods.UploadFile(webHostEnvironment, model, "images");
                if (newFileName != null)
                {
                    Book book = new Book
                    {
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        Date = DateTime.Now,

                        Title = model.Book.Title.Trim(),
                        AuthorId = model.Book.AuthorId,
                        Description = model.Book.Description.Trim(),
                        Image = newFileName,
                        Year = model.Book.Year,
                        City = model.Book.City.Trim(),

                        ISBN = model.Book.ISBN.Trim(),
                        Series = model.Book.Series,
                        IssueNumber = model.Book.IssueNumber,
                        Publisher = model.Book.Publisher.Trim(),
                        Pages = model.Book.Pages
                    };

                    db.Book.Add(book);
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
            else
            {
                TempData["Alert"] = "Danger";
                return View(nameof(Create), GetAuthors());
            }
        }

        // GET: BookController/Edit/5
        [Authorize(Roles = role)]
        [Route("Edytuj/{id}")]
        public ActionResult Edit(int id)
        {
            return View(nameof(Edit), GetBook(id));
        }

        // POST: BookController/Edit/5
        [Authorize(Roles = role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edytuj/{id}")]
        public ActionResult Edit(int id, BookEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Book book = db.Book.Where(x => x.Id == id).Single();
                book.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                book.Date = DateTime.Now;

                book.Title = model.Book.Title.Trim();
                book.ISBN = model.Book.ISBN.Trim();
                book.Series = model.Book.Series;
                book.IssueNumber = model.Book.IssueNumber;

                book.Description = model.Book.Description.Trim();
                book.Year = model.Book.Year;
                book.City = model.Book.City.Trim();
                book.Publisher = model.Book.Publisher.Trim();
                book.Pages = model.Book.Pages;

                if (model.File != null)
                {
                    AppMethods.DeleteFile(webHostEnvironment, book.Image, "images");
                    string newFileName = AppMethods.UploadFile(webHostEnvironment, model, "images");
                    book.Image = newFileName;
                }

                db.Book.Update(book);
                db.SaveChanges();

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Edit));
                
            }
            else
            {
                TempData["Alert"] = "Danger";
                return View(nameof(Edit), GetBook(id));
            }
        }

        // GET: BookController/Delete/5
        [Route("Usun/{id}")]
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
        [Authorize(Roles = role)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Usun/{id}")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Book book = db.Book.Where(x => x.Id == id).Single();
                string fileName = book.Image;

                AppMethods.DeleteFile(webHostEnvironment, fileName, "images");

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Rezerwuj/{id}")]
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

        //GET
        private List<BookViewModel> GetBookList()
        {
            List<BookViewModel> books = db.Book
                .Join(db.Author, z => z.AuthorId, k => k.Id, (z, k) => new { Book = z, Author = k })
                .Select(s => new BookViewModel
                {
                    Book = s.Book,
                    Author = s.Author,
                })
                .ToList();

            return books;
        }

        private BookCreateViewModel GetAuthors()
        {
            BookCreateViewModel bookCreateViewModel = new BookCreateViewModel();
            bookCreateViewModel.Authors = db.Author.ToList();

            return bookCreateViewModel;
        }

        private BookEditViewModel GetBook(int id)
        {
            Book book = db.Book.Where(x => x.Id == id).Single();

            BookEditViewModel bookEditViewModel = new BookEditViewModel();
            bookEditViewModel.Book = book;

            Author author = db.Author.Where(x => x.Id == book.AuthorId).Single();
            bookEditViewModel.Name = author.Name;
            bookEditViewModel.LastName = author.LastName;

            return bookEditViewModel;
        }
    }
}
