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
    
    [Route("Uzytkownicy")]
    public class UserController : Controller
    {
        private const string role = AppData.Admin + "," + AppData.Librarian;
        private readonly AppDbContext db;
        private readonly UserManager<AppUser> UserManager;

        // GET: UserController
        public UserController(AppDbContext context,UserManager<AppUser> userManager)
        {
            db = context;
            UserManager = userManager;
        }

        [Authorize(Roles = role)]
        public ActionResult Index(string searchString, string sortOrder, int? page, string formValue)
        {
            UserViewModel userViewModel = new UserViewModel();
            List<UserViewModel> users = GetUserList();

            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            var tuple = AppMethods.Search(httpContextAccessor, users, "SearchStringUser", formValue, searchString);
            users = tuple.Item1;
            ViewBag.SearchString = tuple.Item2;
            users = AppMethods.Sort(httpContextAccessor, users, "SortOrderUser", sortOrder);

            ViewData["Selected"] = AppMethods.SetViewData(httpContextAccessor, sortOrder, "SortOrderUser", "UserNameDesc");
            ViewBag.Values = AppData.userSort;

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            userViewModel.UserList = users.ToPagedList(pageNumber, pageSize);
            return View(userViewModel);
        }

        // GET: UserController/Details/5
        [Authorize(Roles = role)]
        [Route("Szczegoly/{id}")]
        public ActionResult Details(string id)
        {
            List<UserViewModel> allUsers = GetUserList();
            UserViewModel user = allUsers.Where(x => x.User.Id == id).Single();
            return View(user);
        }

        // GET: UserController/Create
        [Authorize(Roles = AppData.Admin)]
        [Route("Dodaj")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [Authorize(Roles = AppData.Admin)]
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
        [Authorize(Roles = AppData.Admin)]
        [Route("Edytuj/{id}")]
        public async Task<ActionResult> Edit(string id)
        {
            UserViewModel user = new UserViewModel();
            user.User = await UserManager.FindByIdAsync(id);
            user.Email = user.User.Email;
            user.UserName = user.User.UserName;
            return View(user);
        }

        // POST: UserController/Edit/5
        [Authorize(Roles = AppData.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edytuj/{id}")]
        public async Task<ActionResult> Edit(string id, IFormCollection collection)
        {
            if (ModelState.IsValid)
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
            else
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Edit));
            }
        }

        // GET: UserController/Delete/5
        [Authorize(Roles = AppData.Admin)]
        [Route("Usun/{id}")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [Authorize(Roles = AppData.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Usun/{id}")]
        public async Task<ActionResult> DeleteAsync(string id, IFormCollection collection)
        {
            try
            {
                AppUser appUser = await UserManager.FindByIdAsync(id);
                await UserManager.DeleteAsync(appUser);

                Profile profile = db.Profile.Where(x=>x.UserId == id).Single();
                db.Profile.Remove(profile);
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

        // GET: UserController/Role/5
        [Authorize(Roles = AppData.Admin)]
        [Route("ZmienRole/{id}")]
        public ActionResult Role(string id)
        {
            ViewBag.Roles = AppData.roleTypes;

            List<UserViewModel> allUsers = GetUserList();
            UserViewModel user = allUsers.Where(x => x.User.Id == id).Single();
            return View(user);
        }

        // POST: UserController/Role
        [Authorize(Roles = AppData.Admin)]
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

        // GET: UserController/Role/5
        [Authorize(Roles = AppData.Admin)]
        [Route("EdytujProfil/{id}")]
        public ActionResult Profile(string id)
        {
            Profile profile = db.Profile.Where(x=>x.UserId == id).Single();
            return View(profile);
        }

        // POST: UserController/Role
        [Authorize(Roles = AppData.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("EdytujProfil/{id}")]
        public ActionResult Profile(string id, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                Profile profile = db.Profile.Where(x => x.UserId == id).Single();

                string pesel = collection["Pesel"].ToString();
                string lcn = "LCN" + collection["LibraryCardNumber"].ToString();

                List<Profile> profileListNumber = db.Profile.Where(x => x.LibraryCardNumber == lcn).ToList();
                List<Profile> profileListPesel = db.Profile.Where(x => x.Pesel == pesel).ToList();

                if (profileListNumber.Count >= 2 || profileListPesel.Count >= 2){
                    TempData["Alert"] = "Danger";
                    return RedirectToAction(nameof(Profile));
                }
                else {
                    profile.Name = collection["Name"];
                    profile.LastName = collection["LastName"];
                    profile.Pesel = pesel;
                    profile.LibraryCardNumber = lcn;
                    profile.Date = DateTime.Now;

                    db.Profile.Update(profile);
                    db.SaveChanges();

                    TempData["Alert"] = "Success";
                    return RedirectToAction(nameof(Profile));
                }
            }
            else
            {
                TempData["Alert"] = "Danger";
                return RedirectToAction(nameof(Profile));
            }
        }



        //GET
        private List<UserViewModel> GetUserList()
        {
            List<UserViewModel> allUsers = (
                from a in db.Users join b in db.UserRoles on a.Id equals b.UserId into ab
                from c in ab.DefaultIfEmpty() join d in db.Roles on c.RoleId equals d.Id into cd
                from e in cd.DefaultIfEmpty() join f in db.Profile on a.Id equals f.UserId into fg
                from f in fg.DefaultIfEmpty()
                select new UserViewModel
                {
                    User = a,
                    Role = e,
                    Profile = f,
                })
                .ToList();

            return allUsers;
        }
    }
}
