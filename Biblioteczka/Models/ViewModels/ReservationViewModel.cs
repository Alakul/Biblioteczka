using Biblioteczka.Areas.Identity.Data;
using X.PagedList;

namespace Biblioteczka.Models
{
    public class ReservationViewModel
    {
        public Reservation Reservation { get; set; }
        public Book Book { get; set; }
        public Author Author { get; set; }
        public Copy Copy { get; set; }
        public AppUser User { get; set; }
        public IPagedList<ReservationViewModel>? ReservationList { get; set; }
    }
}
