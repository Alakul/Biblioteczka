﻿using Biblioteczka.Areas.Identity.Data;
using Biblioteczka.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Biblioteczka.Controllers
{
    [Authorize(Roles = AppData.Admin)]
    [Route("Uzytkownicy")]
    public class UserController : Controller
    {
        private readonly AppDbContext db;
        private readonly UserManager<AppUser> UserManager;

        // GET: UserController
        public UserController(AppDbContext context,UserManager<AppUser> userManager)
        {
            db = context;
            UserManager = userManager;
        }

        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {
            UserViewModel userViewModel = new UserViewModel();
            List<UserViewModel> users = GetUserList();

            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var tuple = AppMethods.Search(httpContextAccessor, users, "SearchStringUser", formValue, searchString);
            users = tuple.Item1;
            ViewBag.SearchString = tuple.Item2;
            users = AppMethods.Sort(httpContextAccessor, users, "SortOrderUser", sortOrder);

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            userViewModel.UserList = users.ToPagedList(pageNumber, pageSize);
            return View(userViewModel);
        }

        // GET: UserController/Details/5
        [Route("Szczegoly/{id}")]
        public ActionResult Details(string id)
        {
            List<UserViewModel> allUsers = GetUserList();
            UserViewModel user = allUsers.Where(x => x.User.Id == id).Single();
            return View(user);
        }

        // GET: UserController/Create
        [Route("Dodaj")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
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
                return View();
            }
        }

        // GET: UserController/Edit/5
        [Route("Edytuj/{id}")]
        public async Task<ActionResult> Edit(string id)
        {
            AppUser appUser = await UserManager.FindByIdAsync(id);
            return View(appUser);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edytuj/{id}")]
        public async Task<ActionResult> Edit(string id, IFormCollection collection)
        {
            try
            {
                string emailNormalized = collection["Email"].ToString().ToUpper();
                string userNameNormalized = collection["UserName"].ToString().ToUpper();

                AppUser appUser = await UserManager.FindByIdAsync(id);
                List<AppUser> users = db.Users.Where(x => x.NormalizedUserName == userNameNormalized).ToList();

                if (users.Count == 0)
                {
                    appUser.Email = collection["Email"];
                    appUser.NormalizedEmail = emailNormalized;

                    appUser.UserName = collection["UserName"];
                    appUser.NormalizedUserName = userNameNormalized;

                    db.Users.Update(appUser);
                    db.SaveChanges();

                    TempData["Alert"] = "Success";
                    return RedirectToAction(nameof(Edit));
                }
                else
                {
                    TempData["Alert"] = "Danger";
                    return RedirectToAction(nameof(Edit));
                }
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Edit));
            }
        }

        // GET: UserController/Delete/5
        [Route("Usun/{id}")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Usun/{id}")]
        public async Task<ActionResult> DeleteAsync(string id, IFormCollection collection)
        {
            try
            {
                AppUser appUser = await UserManager.FindByIdAsync(id);
                await UserManager.DeleteAsync(appUser);

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: UserController/Role/5
        [Route("ZmienRole/{id}")]
        public ActionResult Role(string id)
        {
            ViewBag.Roles = AppData.roleTypes;

            List<UserViewModel> allUsers = GetUserList();
            UserViewModel user = allUsers.Where(x => x.User.Id == id).Single();
            return View(user);
        }

        // POST: UserController/Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ZmienRole/{id}")]
        public async Task<ActionResult> Role(string id, IFormCollection collection)
        {
            ViewBag.Roles = AppData.roleTypes;

            try
            {
                AppUser appUser = await UserManager.FindByIdAsync(id);

                if (await UserManager.IsInRoleAsync(appUser, collection["Role"]) == false)
                {
                    var currentRoles = await UserManager.GetRolesAsync(appUser);
                    if (currentRoles != null){
                        var resultDelete = await UserManager.RemoveFromRolesAsync(appUser, currentRoles);
                    }

                    var resultRole = "Użytkownik";
                    if (collection["Role"].ToString() != ""){
                        resultRole = collection["Role"];
                    }

                    var resultAdd = await UserManager.AddToRoleAsync(appUser, resultRole);
                    if (resultAdd.Succeeded){
                        TempData["Alert"] = "Success";
                        return RedirectToAction(nameof(Role));
                    }
                    else {
                        TempData["Alert"] = "Danger";
                        return RedirectToAction(nameof(Role));
                    }
                }
                else
                {
                    TempData["Alert"] = "Success";
                    return RedirectToAction(nameof(Role));
                }    
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Role));
            }
        }


        //GET
        private List<UserViewModel> GetUserList()
        {
            List<UserViewModel> allUsers = (
                from a in db.Users join b in db.UserRoles on a.Id equals b.UserId into ab
                from c in ab.DefaultIfEmpty() join d in db.Roles on c.RoleId equals d.Id into cd
                from e in cd.DefaultIfEmpty()
                select new UserViewModel
                {
                    User = a,
                    Role = e,
                }).ToList();

            return allUsers;
        }
    }
}
