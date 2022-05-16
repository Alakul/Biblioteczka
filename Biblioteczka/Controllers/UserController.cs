using Biblioteczka.Areas.Identity.Data;
using Biblioteczka.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Biblioteczka.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        readonly UserManager<AppUser> UserManager;
        // GET: UserController

        public UserController(UserManager<AppUser> userManager)
        {
            UserManager = userManager;
        }

        public ActionResult Index(string searchString, string sortOrder, int? page)
        {
            var users = UserManager.Users.ToList();

            if (!string.IsNullOrEmpty(searchString)){
                searchString = searchString.Trim();
                users = UserManager.Users.Where(x => x.UserName.Contains(searchString) || x.Email.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "LoginAsc":
                    users = UserManager.Users.OrderBy(x => x.UserName).ToList();
                    break;
                case "LoginDesc":
                    users = UserManager.Users.OrderByDescending(x => x.UserName).ToList();
                    break;
                default:
                    users = UserManager.Users.OrderByDescending(x => x.UserName).ToList();
                    break;
            }
            ViewBag.SearchString = searchString;
            ViewBag.SortOrder = sortOrder;

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            AppUser appUser = await UserManager.FindByIdAsync(id);
            return View(appUser);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
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
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(string id, IFormCollection collection)
        {
            try
            {
                AppUser appUser = await UserManager.FindByIdAsync(id);
                await UserManager.DeleteAsync(appUser);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }


        
        // GET: UserController/Role
        public async Task<ActionResult> Role(string id)
        {
            AppUser appUser = await UserManager.FindByIdAsync(id);
            return View(appUser);
        }

        // POST: UserController/Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Role(string id, IFormCollection collection)
        {
            try
            {
                AppUser appUser = await UserManager.FindByIdAsync(id);
                var result = await UserManager.IsInRoleAsync(appUser, "Admin");

                if (collection["Role"] == "Admin" && result==false){
                    await UserManager.AddToRoleAsync(appUser, "Admin");

                }
                else if (collection["Role"] == "User" && result==true){
                    await UserManager.RemoveFromRoleAsync(appUser, "Admin");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
