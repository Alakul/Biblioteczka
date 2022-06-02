using Biblioteczka.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using X.PagedList;

namespace Biblioteczka.Models
{
    public class UserViewModel
    {
        public AppUser User { get; set; }
        public IdentityRole? Role { get; set; }
        public Profile Profile { get; set; }
        public IPagedList<UserViewModel>? UserList { get; set; }
    }
}
