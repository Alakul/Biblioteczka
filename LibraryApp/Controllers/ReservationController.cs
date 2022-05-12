﻿using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApp.Controllers
{
    public class ReservationController : Controller
    {

        AppDbContext db;
        public ReservationController(AppDbContext context)
        {
            db = context;
        }

        // GET: ReservationController
        public ActionResult Index(string searchString)
        {
            List<Reservation> reservations = db.Reservation.ToList();

            if (!string.IsNullOrEmpty(searchString)){
                searchString = searchString.Trim();
                reservations = db.Reservation.Where(x => x.UserBorrowingId.Contains(searchString) || x.BookId.ToString().Contains(searchString)).ToList();
            }
            else {
                reservations = db.Reservation.ToList();
            }

            ViewBag.Sort = AppData.reservationSort;

            return View(reservations);
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
    }
}
