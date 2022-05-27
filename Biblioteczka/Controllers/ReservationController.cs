using Biblioteczka.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace Biblioteczka.Controllers
{
    [Route("Rezerwacje")]
    public class ReservationController : Controller
    {
        private const string role = "Admin";
        private readonly AppDbContext db;
        public ReservationController(AppDbContext context)
        {
            db = context;
        }

        // GET: ReservationController
        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {
            ReservationViewModel reservationViewModel = new ReservationViewModel();
            List<ReservationViewModel> reservations = GetReservationList();

            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var tuple = AppMethods.Search(httpContextAccessor, reservations, "SearchStringReservation", formValue, searchString);
            reservations = tuple.Item1;
            ViewBag.SearchString = tuple.Item2;
            reservations = AppMethods.Sort(httpContextAccessor, reservations, "SortOrderReservation", sortOrder);

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            reservationViewModel.ReservationList = reservations.ToPagedList(pageNumber, pageSize);
            return View(reservationViewModel);
        }

        // GET: ReservationController/Details/5
        [Route("Szczegoly/{id}")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReservationController/Create
        [Route("Dodaj")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Dodaj")]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(Index));
            }
        }

        // GET: ReservationController/Edit/5
        [Route("Edytuj/{id}")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edytuj/{id}")]
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

        // GET: ReservationController/Delete/5
        [Route("Usun/{id}")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReservationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Usun/{id}")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Reservation reservation = db.Reservation.Where(x => x.Id == id).Single();
                Copy copy = db.Copy.Where(x => x.Id == reservation.CopyId).Single();
                copy.Status = "1";
  
                db.Remove(db.Reservation.Where(x => x.Id == id).Single());
                db.Copy.Update(copy);
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

        //GET
        private List<ReservationViewModel> GetReservationList()
        {
            List<ReservationViewModel> reservations = db.Reservation
                .Join(db.Book, a => a.BookId, b => b.Id, (a, b) => new { reservation = a, book = b })
                .Join(db.Author, joined => joined.book.AuthorId, b => b.Id, (joined, b) => new { joined, author = b })
                .Join(db.Copy, joinedTwice => joinedTwice.joined.reservation.CopyId, b => b.Id, (joinedTwice, b) => new { joinedTwice, copy = b })
                .Join(db.Users, joinedThrice => joinedThrice.joinedTwice.joined.reservation.UserBorrowingId, b => b.Id,
                (joinedThrice, user) => new ReservationViewModel
                {
                    Reservation = joinedThrice.joinedTwice.joined.reservation,
                    Book = joinedThrice.joinedTwice.joined.book,
                    Author = joinedThrice.joinedTwice.author,
                    Copy = joinedThrice.copy,
                    User = user,
                })
                .ToList();

            return reservations;
        }
    }
}
