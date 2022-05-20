using Biblioteczka.Data;
using Biblioteczka.Models;
using Biblioteczka.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace Biblioteczka.Controllers
{
    public class ReservationController : Controller
    {

        AppDbContext db;
        public ReservationController(AppDbContext context)
        {
            db = context;
        }

        // GET: ReservationController
        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {
            ReservationViewModel reservationViewModel = new ReservationViewModel();
            List<ReservationViewModel> reservations = GetReservationList();

            reservations = SearchReservations(formValue, searchString, reservations);
            reservations = SortReservations(sortOrder, reservations);

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            reservationViewModel.ReservationList = reservations.ToPagedList(pageNumber, pageSize);
            return View(reservationViewModel);
        }

        // GET: ReservationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReservationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservationController/Edit/5
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

        // GET: ReservationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReservationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        //SEARCH
        private List<ReservationViewModel> SearchReservations(string formValue, string searchString, List<ReservationViewModel> reservations)
        {
            CookieOptions options = new CookieOptions();
            string cookieName = "SearchStringReservation";
            string cookie = Request.Cookies[cookieName];

            if (formValue == null)
            {
                if (cookie != null){
                    reservations = reservations.Where(x => x.Copy.Number.ToString().ToLower().Contains(cookie.ToLower()) ||
                        x.User.UserName.ToLower().Contains(cookie.ToLower()) ||
                        x.Book.Title.ToLower().Contains(cookie.ToLower()) ||
                        x.Author.LastName.ToLower().Contains(cookie.ToLower())).ToList();
                    ViewBag.SearchString = cookie;
                }
                else {
                    reservations = GetReservationList();
                    ViewBag.SearchString = "";
                }
            }
            else if (searchString == null && formValue == "1"){
                reservations = GetReservationList();
                Response.Cookies.Delete(cookieName);
                ViewBag.SearchString = "";
            }
            else if (searchString != null && formValue == "1"){
                string newValue = searchString.Trim();
                if (!string.IsNullOrEmpty(newValue)){
                    if (newValue != cookie){
                        Response.Cookies.Append(cookieName, newValue, options);
                    }
                    reservations = reservations.Where(x => x.Copy.Number.ToString().ToLower().Contains(newValue.ToLower()) ||
                        x.User.UserName.ToLower().Contains(newValue.ToLower()) ||
                        x.Book.Title.ToLower().Contains(newValue.ToLower()) ||
                        x.Author.LastName.ToLower().Contains(newValue.ToLower())).ToList();
                    ViewBag.SearchString = newValue;
                }
                else
                {
                    reservations = GetReservationList();
                    ViewBag.SearchString = "";
                }
            }

            return reservations;
        }

        //SORT
        private List<ReservationViewModel> SortReservations(string sortOrder, List<ReservationViewModel> reservations)
        {
            CookieOptions options = new CookieOptions();
            string cookieName = "SortOrderReservation";

            if (sortOrder == null){
                string cookie = Request.Cookies[cookieName];
                if (cookie != null){
                    reservations = GetReservations(cookie, reservations);
                }
                else {
                    reservations = GetReservationList();
                }
            }
            else if (sortOrder != null){
                reservations = GetReservations(sortOrder, reservations);
                if (sortOrder != null){
                    Response.Cookies.Append(cookieName, sortOrder, options);
                }
            }

            return reservations;
        }

        //SWITCH
        private List<ReservationViewModel> GetReservations(string sort, List<ReservationViewModel> reservations)
        {
            switch (sort)
            {
                case "UserNameAsc":
                    reservations = reservations.OrderBy(x => x.User.UserName).ToList();
                    break;
                case "UserNameDesc":
                    reservations = reservations.OrderByDescending(x => x.User.UserName).ToList();
                    break;
                case "TitleAsc":
                    reservations = reservations.OrderBy(x => x.Book.Title).ToList();
                    break;
                case "TitleDesc":
                    reservations = reservations.OrderByDescending(x => x.Book.Title).ToList();
                    break;
                case "LastNameAsc":
                    reservations = reservations.OrderBy(x => x.Author.LastName).ToList();
                    break;
                case "LastNameDesc":
                    reservations = reservations.OrderByDescending(x => x.Author.LastName).ToList();
                    break;
                case "NumberAsc":
                    reservations = reservations.OrderBy(x => x.Copy.Number).ToList();
                    break;
                case "NumberDesc":
                    reservations = reservations.OrderByDescending(x => x.Copy.Number).ToList();
                    break;
                case "DateAsc":
                    reservations = reservations.OrderBy(x => x.Copy.Date).ToList();
                    break;
                case "DateDesc":
                    reservations = reservations.OrderByDescending(x => x.Copy.Date).ToList();
                    break;
                default:
                    reservations = reservations.OrderByDescending(x => x.Copy.Date).ToList();
                    break;
            }
            return reservations;
        }
    }
}
