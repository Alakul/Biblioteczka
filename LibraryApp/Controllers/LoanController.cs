using LibraryApp.Areas.Identity.Data;
using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace LibraryApp.Controllers
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
        public ActionResult Index(string searchString, string sortOrder, int? page)
        {
            var loans = db.Loan.ToList();

            if (!string.IsNullOrEmpty(searchString)){
                searchString = searchString.Trim();
                loans = db.Loan.Where(x => x.UserBorrowingId.Contains(searchString) || x.BookId.ToString().Contains(searchString) || x.CopyId.ToString().Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "LoanDateAsc":
                    loans = loans.OrderBy(x => x.LoanDate).ToList();
                    break;
                case "LoanDateDesc":
                    loans = loans.OrderByDescending(x => x.LoanDate).ToList();
                    break;
                case "ReturnDateAsc":
                    loans = loans.OrderBy(x => x.ReturnDate).ToList();
                    break;
                case "ReturnDateDesc":
                    loans = loans.OrderByDescending(x => x.ReturnDate).ToList();
                    break;
                default:
                    loans = loans.OrderByDescending(x => x.LoanDate).ToList();
                    break;
            }
            ViewBag.SearchString = searchString;
            ViewBag.SortOrder = sortOrder;

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(loans.ToPagedList(pageNumber, pageSize));
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
    }
}
