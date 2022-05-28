using Biblioteczka.Areas.Identity.Data;
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
        public async Task<ActionResult> Details(string id)
        {
            AppUser appUser = await UserManager.FindByIdAsync(id);
            return View(appUser);
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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
        public async Task<ActionResult> Role(string id)
        {
            AppUser appUser = await UserManager.FindByIdAsync(id);
            return View(appUser);
        }

        // POST: UserController/Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ZmienRole/{id}")]
        public async Task<ActionResult> Role(string id, IFormCollection collection)
        {
            try
            {
                AppUser appUser = await UserManager.FindByIdAsync(id);

                if (await UserManager.IsInRoleAsync(appUser, collection["Role"]) == false)
                {
                    var currentRoles = await UserManager.GetRolesAsync(appUser);
                    if (currentRoles != null){
                        var resultDelete = await UserManager.RemoveFromRolesAsync(appUser, currentRoles);
                    }

                    var resultAdd = await UserManager.AddToRoleAsync(appUser, collection["Role"]);
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
            List<UserViewModel> users = db.Users.Join(db.UserRoles, x => x.Id, y => y.UserId, (x, y) => new { Users = x, UserRoles = y })
                .Join(db.Roles, joined => joined.UserRoles.RoleId, b => b.Id,
                (joined, roles) => new UserViewModel
                {
                    User = joined.Users,
                    Role = roles,
                })
                .ToList();

            return users;
        }
    }
}
