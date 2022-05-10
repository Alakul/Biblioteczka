using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApp.Controllers
{
    public class LoanController : Controller
    {
        AppDbContext db;
        public LoanController(AppDbContext context)
        {
            db = context;
        }

        // GET: LoanController
        public ActionResult Index()
        {
            List<Loan> loans = db.Loan.ToList();
            return View(loans);
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
