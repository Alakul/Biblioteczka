using Biblioteczka.Areas.Identity.Data;
using Biblioteczka.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace Biblioteczka.Controllers
{
    public class LoanController : Controller
    {
        private readonly AppDbContext db;
        private readonly UserManager<AppUser> UserManager;
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

            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            AppMethods appMethods = new AppMethods();
            var tuple = appMethods.Search(httpContextAccessor, loans, "SearchStringLoan", formValue, searchString);
            loans = tuple.Item1;
            ViewBag.SearchString = tuple.Item2;
            loans = appMethods.Sort(httpContextAccessor, loans, "SortOrderLoan", sortOrder);

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
                return RedirectToAction(nameof(Index),"Reservation");
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Index), "Reservation");
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
    }
}
