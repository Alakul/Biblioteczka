// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Biblioteczka.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Biblioteczka.Data;
using Biblioteczka.Models;

namespace Biblioteczka.Areas.Identity.Pages.Account
{
    [Authorize(Roles = AppData.Admin + "," + AppData.Librarian)]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext db;

        public RegisterModel(
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            AppDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            db = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [DataType(DataType.Text)]
            [Display(Name = "Login")]
            public string UserName { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [EmailAddress(ErrorMessage = "Pole {0} nie jest prawidłowym adresem email.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Hasło i hasło potwierdzające nie pasują do siebie.")]
            public string ConfirmPassword { get; set; }



            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [DataType(DataType.Text)]
            [Display(Name = "Imię")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [DataType(DataType.Text)]
            [Display(Name = "Nazwisko")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [RegularExpression("^[0-9]{11}$", ErrorMessage = "Wprowadź liczbę skłającą się z 11 cyfr.")]
            [Display(Name = "Pesel")]
            public long Pesel { get; set; }

            [Required(ErrorMessage = "Pole {0} jest wymagane.")]
            [RegularExpression("^[0-9]{8}$", ErrorMessage = "Wprowadź liczbę skłającą się z 8 cyfr.")]
            [Display(Name = "Numer karty bibliotecznej")]
            public long? LibraryCardNumber { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                List<Profile> profileList = db.Profile.Where(x => x.LibraryCardNumber == "LCN" + Input.LibraryCardNumber.ToString()).ToList();
                if (profileList.Count > 0){
                    TempData["Alert"] = "Danger";
                    return RedirectToPage("Register");
                }

                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, "Użytkownik");
                    var userId = await _userManager.GetUserIdAsync(user);


                    Profile profile = new Profile
                    {
                        UserId = userId,
                        Date = DateTime.Now,

                        Name = Input.Name.Trim(),
                        LastName = Input.LastName.Trim(),
                        Pesel = Input.Pesel.ToString(),
                    };
                    
                    if (Input.LibraryCardNumber == null){
                        Random random = new Random();
                        int newNumber = 0;
                        profileList = db.Profile.Where(x => x.LibraryCardNumber == "LCN" + newNumber.ToString()).ToList();

                        do
                        {
                            newNumber = random.Next(10000000, 99999999);
                            profileList = db.Profile.Where(x => x.LibraryCardNumber == "LCN" + newNumber.ToString()).ToList();
                        }
                        while (profileList.Count > 0);

                        profile.LibraryCardNumber = "LCN" + newNumber.ToString();
                    }
                    else {
                        profile.LibraryCardNumber = "LCN" + Input.LibraryCardNumber.ToString();
                    }
                    
                    db.Profile.Add(profile);
                    db.SaveChanges();
                    

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        //return LocalRedirect(returnUrl);

                        TempData["Alert"] = "Success";
                        return RedirectToPage("Register");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            // return Page();

            TempData["Alert"] = "Danger";
            return RedirectToPage("Register");  
        }

        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppUser>)_userStore;
        }
    }
}
