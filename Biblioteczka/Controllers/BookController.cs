﻿using Biblioteczka.Data;
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
            AppMethods appMethods = new AppMethods();
            var tuple = appMethods.Search(httpContextAccessor, books, "SearchStringBook", formValue, searchString);
            books = tuple.Item1;
            ViewBag.SearchString = tuple.Item2;
            books = appMethods.Sort(httpContextAccessor, books, "SortOrderBook", sortOrder);

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            bookAuthorViewModel.BookList = books.ToPagedList(pageNumber, pageSize);
            return View(bookAuthorViewModel);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id, int? page)
        {
            List<Copy> copies = db.Copy.Where(x => x.BookId == id).ToList();
            BookViewModel bookAuthorViewModel = new BookViewModel();
            bookAuthorViewModel.Book = db.Book.Where(x => x.Id == id).Single();
            bookAuthorViewModel.Author = db.Author.Where(x => x.Id == bookAuthorViewModel.Book.AuthorId).Single();
            ViewBag.Id = id;

            int pageSize = 1;
            int pageNumber = (page ?? 1);
            bookAuthorViewModel.CopyList = copies.ToPagedList(pageNumber, pageSize);         
            return View(bookAuthorViewModel);
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
                AppMethods appMethods = new AppMethods();
                string newFileName = appMethods.UploadFile(webHostEnvironment, model, "images");
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
                        City = model.Book.City.Trim()
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
            catch {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: BookController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Book book = db.Book.Where(x => x.Id == id).Single();

            BookCreateEditViewModel bookCreateEditViewModel = new BookCreateEditViewModel();
            bookCreateEditViewModel.Book = book;
            bookCreateEditViewModel.Authors = db.Author.ToList();

            return View(bookCreateEditViewModel);
        }

        // POST: BookController/Edit/5
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
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
                    AppMethods appMethods = new AppMethods();
                    appMethods.DeleteFile(webHostEnvironment, book.Image, "images");
                    string newFileName = appMethods.UploadFile(webHostEnvironment, model, "images");
                    book.Image = newFileName;
                }

                db.Book.Update(book);
                db.SaveChanges();

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Edit));
                
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Edit));
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

                AppMethods appMethods = new AppMethods();
                appMethods.DeleteFile(webHostEnvironment, fileName, "images");

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

        private BookCreateEditViewModel GetAuthors()
        {
            BookCreateEditViewModel bookCreateEditViewModel = new BookCreateEditViewModel();
            bookCreateEditViewModel.Authors = db.Author.ToList();

            return bookCreateEditViewModel;
        }
    }
}
