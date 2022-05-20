using Biblioteczka.Areas.Identity.Data;
using X.PagedList;

namespace Biblioteczka.Models.ViewModels
{
    public class LoanViewModel
    {
        public Loan Loan { get; set; }
        public Book Book { get; set; }
        public Author Author { get; set; }
        public Copy Copy { get; set; }
        public AppUser User { get; set; }
        public IPagedList<LoanViewModel>? LoanList { get; set; }
    }
}
