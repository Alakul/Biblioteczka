using Biblioteczka.Areas.Identity.Data;
using Biblioteczka.Data;
using Biblioteczka.Models;
using Biblioteczka.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace Biblioteczka.Controllers
{
    public class LoanController : Controller
    {
        AppDbContext db;
        readonly UserManager<AppUser> UserManager;
        public LoanController(AppDbContext context, UserManager<AppUser> userManager)
        {
            db = context;
            UserManager = userManager;
        }

        // GET: LoanController
        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {

            LoanViewModel loanViewModel = new LoanViewModel();
            List<LoanViewModel> loans = GetLoanList();

            loans = SearchCopies(formValue, searchString, loans);
            loans = SortCopies(sortOrder, loans);

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            loanViewModel.LoanList = loans.ToPagedList(pageNumber, pageSize);
            return View(loanViewModel);
        }

        // GET: LoanController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoanController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoanController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, IFormCollection collection)
        {
            try
            {
                Reservation reservation = db.Reservation.Where(x => x.Id == id).Single();
                Loan loan = new Loan
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    LoanDate = DateTime.Now,

                    UserBorrowingId = reservation.UserBorrowingId,
                    BookId = reservation.BookId,
                    CopyId = reservation.CopyId,
                    Status = "0",
                };

                db.Loan.Add(loan);
                db.Remove(reservation);
                db.SaveChanges();

                TempData["Alert"] = "Success";
                return RedirectToAction("Index","Reservation");
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction("Index", "Reservation");
            }
        }

        // GET: LoanController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoanController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: LoanController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoanController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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



        // POST: LoanController/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(int id, IFormCollection collection)
        {
            try
            {
                Loan loan = db.Loan.Where(x => x.Id == id).Single();
                loan.ReturnDate = DateTime.Now;
                loan.Status = "1";

                db.Loan.Update(loan);
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









        private List<LoanViewModel> GetLoanList()
        {
            List<LoanViewModel> loans = db.Loan
                .Join(db.Book, a => a.BookId, b => b.Id, (a, b) => new { loan = a, book = b })
                .Join(db.Author, joined => joined.book.AuthorId, b => b.Id, (joined, b) => new { joined, author = b })
                .Join(db.Copy, joinedTwice => joinedTwice.joined.loan.CopyId, b => b.Id, (joinedTwice, b) => new { joinedTwice, copy = b })
                .Join(db.Users, joinedThrice => joinedThrice.joinedTwice.joined.loan.UserBorrowingId, b => b.Id,
                (joinedThrice, user) => new LoanViewModel
                {
                    Loan = joinedThrice.joinedTwice.joined.loan,
                    Book = joinedThrice.joinedTwice.joined.book, 
                    Author = joinedThrice.joinedTwice.author,
                    Copy = joinedThrice.copy,
                    User = user,
                })
                .ToList();

            return loans;
        }

        //SEARCH
        private List<LoanViewModel> SearchCopies(string formValue, string searchString, List<LoanViewModel> loans)
        {
            CookieOptions options = new CookieOptions();
            string cookie = Request.Cookies["SearchStringLoan"];

            if (formValue == null){
                if (cookie != null){
                    loans = loans.Where(x => x.Copy.Number.ToString().ToLower().Contains(cookie.ToLower()) ||
                        x.User.UserName.ToLower().Contains(cookie.ToLower()) ||
                        x.Book.Title.ToLower().Contains(cookie.ToLower()) ||
                        x.Author.LastName.ToLower().Contains(cookie.ToLower())).ToList();
                    ViewBag.SearchString = cookie;
                }
                else {
                    loans = GetLoanList();
                    ViewBag.SearchString = "";
                }
            }
            else if (searchString == null && formValue == "1"){
                loans = GetLoanList();
                Response.Cookies.Delete("SearchStringLoan");
                ViewBag.SearchString = "";
            }
            else if (searchString != null && formValue == "1"){
                string newValue = searchString.Trim();
                if (!string.IsNullOrEmpty(newValue)){
                    if (newValue != cookie){
                        Response.Cookies.Append("SearchStringLoan", newValue, options);
                    }
                    loans = loans.Where(x => x.Copy.Number.ToString().ToLower().Contains(newValue.ToLower()) ||
                        x.User.UserName.ToLower().Contains(newValue.ToLower()) || 
                        x.Book.Title.ToLower().Contains(newValue.ToLower()) ||
                        x.Author.LastName.ToLower().Contains(newValue.ToLower())).ToList();
                    ViewBag.SearchString = newValue;
                }
                else {
                    loans = GetLoanList();
                    ViewBag.SearchString = "";
                }
            }

            return loans;
        }

        //SORT
        private List<LoanViewModel> SortCopies(string sortOrder, List<LoanViewModel> loans)
        {
            CookieOptions options = new CookieOptions();

            if (sortOrder == null){
                string cookie = Request.Cookies["SortOrderLoan"];
                if (cookie != null){
                    loans = GetLoans(cookie, loans);
                }
                else {
                    loans = GetLoanList();
                }
            }
            else if (sortOrder != null){
                loans = GetLoans(sortOrder, loans);
                if (sortOrder != null){
                    Response.Cookies.Append("SortOrderLoan", sortOrder, options);
                }
            }

            return loans;
        }

        //SWITCH
        private List<LoanViewModel> GetLoans(string sort, List<LoanViewModel> loans)
        {
            switch (sort)
            {
                case "UserNameAsc":
                    loans = loans.OrderBy(x => x.User.UserName).ToList();
                    break;
                case "UserNameDesc":
                    loans = loans.OrderByDescending(x => x.User.UserName).ToList();
                    break;
                case "TitleAsc":
                    loans = loans.OrderBy(x => x.Book.Title).ToList();
                    break;
                case "TitleDesc":
                    loans = loans.OrderByDescending(x => x.Book.Title).ToList();
                    break;
                case "LastNameAsc":
                    loans = loans.OrderBy(x => x.Author.LastName).ToList();
                    break;
                case "LastNameDesc":
                    loans = loans.OrderByDescending(x => x.Author.LastName).ToList();
                    break;
                case "NumberAsc":
                    loans = loans.OrderBy(x => x.Copy.Number).ToList();
                    break;
                case "NumberDesc":
                    loans = loans.OrderByDescending(x => x.Copy.Number).ToList();
                    break;
                case "DateAsc":
                    loans = loans.OrderBy(x => x.Copy.Date).ToList();
                    break;
                case "DateDesc":
                    loans = loans.OrderByDescending(x => x.Copy.Date).ToList();
                    break;
                default:
                    loans = loans.OrderByDescending(x => x.Copy.Date).ToList();
                    break;
            }
            return loans;
        }



    }
}
