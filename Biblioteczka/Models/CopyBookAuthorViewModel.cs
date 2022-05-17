﻿using Biblioteczka.Areas.Identity.Data;
using X.PagedList;

namespace Biblioteczka.Models
{
    public class CopyBookAuthorViewModel
    {
        public List<Book>? Books { get; set; }
        public List<Author>? Authors { get; set; }
        public List<Copy>? Copies { get; set; }

        public IPagedList<Copy> CopyList { get; set; }
    }
}