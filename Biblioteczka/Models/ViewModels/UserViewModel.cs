using Biblioteczka.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace Biblioteczka.Models
{
    public class UserViewModel
    {
        public AppUser User { get; set; }
        public IdentityRole? Role { get; set; }
        public Profile Profile { get; set; }
        public IPagedList<UserViewModel>? UserList { get; set; }



        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [EmailAddress(ErrorMessage = "Pole {0} nie jest prawidłowym adresem email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
