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
            List<AppUser> users = db.Users.ToList();
            users = SearchUsers(formValue, searchString, users);
            users = SortUsers(sortOrder, users);

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

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Alert"] = "Danger";
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

                TempData["Alert"] = "Success";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Index));
            }
        }





        //SEARCH
        private List<AppUser> SearchUsers(string formValue, string searchString, List<AppUser> users)
        {
            CookieOptions options = new CookieOptions();
            string cookieName = "SearchStringUser";
            string cookie = Request.Cookies[cookieName];

            if (formValue == null){
                if (cookie != null){
                    users = users.Where(x => x.UserName.ToLower().Contains(cookie.ToLower()) ||
                        x.Email.ToLower().Contains(cookie.ToLower())).ToList();
                    ViewBag.SearchString = cookie;
                }
                else {
                    users = UserManager.Users.ToList();
                    ViewBag.SearchString = "";
                }
            }
            else if (searchString == null && formValue == "1"){
                users = UserManager.Users.ToList();
                Response.Cookies.Delete(cookieName);
                ViewBag.SearchString = "";
            }
            else if (searchString != null && formValue == "1"){
                string newValue = searchString.Trim();
                if (!string.IsNullOrEmpty(newValue)){
                    if (newValue != cookie){
                        Response.Cookies.Append(cookieName, newValue, options);
                    }
                    users = users.Where(x => x.UserName.ToLower().Contains(newValue.ToLower()) ||
                        x.Email.ToLower().Contains(newValue.ToLower())).ToList();
                    ViewBag.SearchString = newValue;
                }
                else {
                    users = UserManager.Users.ToList();
                    ViewBag.SearchString = "";
                }
            }

            return users;
        }

        //SORT
        private List<AppUser> SortUsers(string sortOrder, List<AppUser> users)
        {
            CookieOptions options = new CookieOptions();
            string cookieName = "SortOrderUser";

            if (sortOrder == null){
                string cookie = Request.Cookies[cookieName];
                if (cookie != null){
                    users = GetUsers(cookie, users);
                }
            }
            else if (sortOrder != null){
                users = GetUsers(sortOrder, users);
                if (sortOrder != null){
                    Response.Cookies.Append(cookieName, sortOrder, options);
                }
            }

            return users;
        }

        //SWITCH
        private List<AppUser> GetUsers(string sort, List<AppUser> users)
        {
            switch (sort)
            {
                case "UserNameAsc":
                    users = UserManager.Users.OrderBy(x => x.UserName).ToList();
                    break;
                case "UserNameDesc":
                    users = UserManager.Users.OrderByDescending(x => x.UserName).ToList();
                    break;
                case "EmailAsc":
                    users = UserManager.Users.OrderBy(x => x.Email).ToList();
                    break;
                case "EmailDesc":
                    users = UserManager.Users.OrderByDescending(x => x.Email).ToList();
                    break;
                default:
                    users = UserManager.Users.OrderBy(x => x.UserName).ToList();
                    break;
            }
            return users;
        }
    }
}
