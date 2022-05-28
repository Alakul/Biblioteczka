using Azure.Core;
using Biblioteczka.Models;
using Microsoft.EntityFrameworkCore;
using static Biblioteczka.Data.SortEnum;

namespace Biblioteczka.Data
{
    public static class AppMethods
    {
        public static Tuple<List<T>,string> Search<T>(IHttpContextAccessor httpContextAccessor, List<T> elements, string cookieName, string formValue, string searchString)
        {
            string cookie = httpContextAccessor.HttpContext.Request.Cookies[cookieName];
            string viewSearchString = "";

            if (formValue == null){
                if (cookie != null){
                    elements = GetElementsSearch(elements, cookie);
                    viewSearchString = cookie;
                }
                else {
                    elements = elements.ToList();
                    viewSearchString = "";
                }
            }
            else if (searchString == null && formValue == "1"){
                elements = elements.ToList();
                httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);
                viewSearchString = "";
            }
            else if (searchString != null && formValue == "1"){
                string newValue = searchString.Trim();
                if (!string.IsNullOrEmpty(newValue)){
                    if (newValue != cookie){
                        httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, newValue); 
                    }
                    elements = GetElementsSearch(elements, newValue);
                    viewSearchString = newValue;
                }
                else {
                    elements = elements.ToList();
                    viewSearchString = "";
                }
            }

            return Tuple.Create(elements, viewSearchString);
        }

        private static List<T> GetElementsSearch<T>(List<T> elements, string cookie)
        {
            List<T> genericList = new List<T>();

            var type = typeof(T);
            if (type == typeof(BookViewModel)){

                List<BookViewModel> bookList = elements.Cast<BookViewModel>().ToList();
                bookList = bookList.Where(x => x.Book.Title.ToLower().Contains(cookie.ToLower()) ||
                        x.Author.Name.ToLower().Contains(cookie.ToLower()) ||
                        x.Author.LastName.ToLower().Contains(cookie.ToLower())).ToList();
                genericList = bookList.Cast<T>().ToList();

            }
            else if (type == typeof(Author))
            {
                List<Author> authorList = elements.Cast<Author>().ToList();
                authorList = authorList.Where(x => x.Name.ToLower().Contains(cookie.ToLower()) ||
                        x.LastName.ToLower().Contains(cookie.ToLower())).ToList();
                genericList = authorList.Cast<T>().ToList();
            }
            else if (type == typeof(CopyViewModel))
            {
                List<CopyViewModel> copyList = elements.Cast<CopyViewModel>().ToList();
                copyList = copyList.Where(x => x.Copy.Number.ToString().ToLower().Contains(cookie.ToLower()) ||
                        x.Book.Title.ToLower().Contains(cookie.ToLower()) ||
                        x.Author.Name.ToLower().Contains(cookie.ToLower()) ||
                        x.Author.LastName.ToLower().Contains(cookie.ToLower())).ToList();
                genericList = copyList.Cast<T>().ToList();
            }
            else if (type == typeof(LoanViewModel))
            {
                List<LoanViewModel> loanList = elements.Cast<LoanViewModel>().ToList();
                loanList = loanList.Where(x => x.Copy.Number.ToString().ToLower().Contains(cookie.ToLower()) ||
                        x.User.UserName.ToLower().Contains(cookie.ToLower()) ||
                        x.Book.Title.ToLower().Contains(cookie.ToLower()) ||
                        x.Author.LastName.ToLower().Contains(cookie.ToLower())).ToList();
                genericList = loanList.Cast<T>().ToList();
            }
            else if (type == typeof(ReservationViewModel))
            {
                List<ReservationViewModel> reservationList = elements.Cast<ReservationViewModel>().ToList();
                reservationList = reservationList.Where(x => x.Copy.Number.ToString().ToLower().Contains(cookie.ToLower()) ||
                        x.User.UserName.ToLower().Contains(cookie.ToLower()) ||
                        x.Book.Title.ToLower().Contains(cookie.ToLower()) ||
                        x.Author.LastName.ToLower().Contains(cookie.ToLower())).ToList();
                genericList = reservationList.Cast<T>().ToList();
            }
            else if (type == typeof(UserViewModel))
            {
                List<UserViewModel> userList = elements.Cast<UserViewModel>().ToList();
                userList = userList.Where(x => x.User.UserName.ToLower().Contains(cookie.ToLower()) ||
                        x.User.Email.ToLower().Contains(cookie.ToLower()) ||
                        x.Role.Name.ToLower().Contains(cookie.ToLower())).ToList();
                genericList = userList.Cast<T>().ToList();
            }

            return genericList;
        }

        public static List<T> Sort<T>(IHttpContextAccessor httpContextAccessor, List<T> elements, string cookieName, string sortOrder)
        {
            string cookie = httpContextAccessor.HttpContext.Request.Cookies[cookieName];

            if (sortOrder == null){
                if (cookie != null){
                    SortValues sortValue = (SortValues)Enum.Parse(typeof(SortValues), cookie, true);
                    elements = GetElementsSort(elements, sortValue);
                }
            }
            else if (sortOrder != null){
                httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, sortOrder);
                SortValues sortValue = (SortValues)Enum.Parse(typeof(SortValues), sortOrder, true);
                elements = GetElementsSort(elements, sortValue);
            }

            return elements;
        }
        private static List<T> GetElementsSort<T>(List<T> elements, SortValues sort)
        {
            List<T> genericList = new List<T>();

            var type = typeof(T);
            if (type == typeof(BookViewModel)){
                List<BookViewModel> bookList = elements.Cast<BookViewModel>().ToList();
                bookList = SortElementsBook(bookList, sort);
                genericList = bookList.Cast<T>().ToList();
            }
            else if (type == typeof(Author)){
                List<Author> authorList = elements.Cast<Author>().ToList();
                authorList = SortElementsAuthor(authorList, sort);
                genericList = authorList.Cast<T>().ToList();
            }
            else if (type == typeof(CopyViewModel)){
                List<CopyViewModel> copyList = elements.Cast<CopyViewModel>().ToList();
                copyList = SortElementsCopy(copyList, sort);
                genericList = copyList.Cast<T>().ToList();
            }
            else if (type == typeof(LoanViewModel)){
                List<LoanViewModel> loanList = elements.Cast<LoanViewModel>().ToList();
                loanList = SortElementsLoan(loanList, sort);
                genericList = loanList.Cast<T>().ToList();
            }
            else if (type == typeof(ReservationViewModel)){
                List<ReservationViewModel> reservationList = elements.Cast<ReservationViewModel>().ToList();
                reservationList = SortElementsReservation(reservationList, sort);
                genericList = reservationList.Cast<T>().ToList();
            }
            else if (type == typeof(UserViewModel))
            {
                List<UserViewModel> userList = elements.Cast<UserViewModel>().ToList();
                userList = SortElementsUser(userList, sort);
                genericList = userList.Cast<T>().ToList();
            }

            return genericList;
        }

        public static List<BookViewModel> SortElementsBook(List<BookViewModel> elements, SortValues sortValues) =>
        sortValues switch
        {
            SortValues.TitleAsc => elements = elements.OrderBy(x => x.Book.Title).ToList(),
            SortValues.TitleDesc => elements = elements.OrderByDescending(x => x.Book.Title).ToList(),
            SortValues.LastNameAsc => elements = elements.OrderBy(x => x.Author.LastName).ToList(),
            SortValues.LastNameDesc => elements = elements.OrderByDescending(x => x.Author.LastName).ToList(),
            SortValues.YearAsc => elements = elements.OrderBy(x => x.Book.Year).ToList(),
            SortValues.YearDesc => elements = elements.OrderByDescending(x => x.Book.Year).ToList(),
            SortValues.DateAsc => elements = elements.OrderBy(x => x.Book.Date).ToList(),
            SortValues.DateDesc => elements = elements.OrderByDescending(x => x.Book.Date).ToList(),
            _ => elements = elements.OrderByDescending(x => x.Book.Date).ToList(),
        };

        public static List<Author> SortElementsAuthor(List<Author> elements, SortValues sortValues) =>
        sortValues switch
        {
            SortValues.NameAsc => elements = elements.OrderBy(x => x.LastName).ToList(),
            SortValues.NameDesc => elements = elements.OrderByDescending(x => x.LastName).ToList(),
            SortValues.LastNameAsc => elements = elements.OrderBy(x => x.LastName).ToList(),
            SortValues.LastNameDesc => elements = elements.OrderByDescending(x => x.LastName).ToList(),
            SortValues.DateAsc => elements = elements.OrderBy(x => x.Date).ToList(),
            SortValues.DateDesc => elements = elements.OrderByDescending(x => x.Date).ToList(),
            _ => elements = elements.OrderByDescending(x => x.Date).ToList(),
        };

        public static List<CopyViewModel> SortElementsCopy(List<CopyViewModel> elements, SortValues sortValues) =>
        sortValues switch
        {
            SortValues.NumberAsc => elements = elements.OrderBy(x => x.Copy.Number).ToList(),
            SortValues.NumberDesc => elements = elements.OrderByDescending(x => x.Copy.Number).ToList(),
            SortValues.TitleAsc => elements = elements.OrderBy(x => x.Book.Title).ToList(),
            SortValues.TitleDesc => elements = elements.OrderByDescending(x => x.Book.Title).ToList(),
            SortValues.LastNameAsc => elements = elements.OrderBy(x => x.Author.LastName).ToList(),
            SortValues.LastNameDesc => elements = elements.OrderByDescending(x => x.Author.LastName).ToList(),
            SortValues.DateAsc => elements = elements.OrderBy(x => x.Copy.Date).ToList(),
            SortValues.DateDesc => elements = elements.OrderByDescending(x => x.Copy.Date).ToList(),
            _ => elements = elements.OrderByDescending(x => x.Copy.Date).ToList(),
        };

        public static List<LoanViewModel> SortElementsLoan(List<LoanViewModel> elements, SortValues sortValues) =>
        sortValues switch
        {
            SortValues.UserNameAsc => elements = elements.OrderBy(x => x.User.UserName).ToList(),
            SortValues.UserNameDesc => elements = elements.OrderByDescending(x => x.User.UserName).ToList(),
            SortValues.TitleAsc => elements = elements.OrderBy(x => x.Book.Title).ToList(),
            SortValues.TitleDesc => elements = elements.OrderByDescending(x => x.Book.Title).ToList(),
            SortValues.LastNameAsc => elements = elements.OrderBy(x => x.Author.LastName).ToList(),
            SortValues.LastNameDesc => elements = elements.OrderByDescending(x => x.Author.LastName).ToList(),
            SortValues.NumberAsc => elements = elements.OrderBy(x => x.Copy.Number).ToList(),
            SortValues.NumberDesc => elements = elements.OrderByDescending(x => x.Copy.Number).ToList(),
            SortValues.LoanDateAsc => elements = elements.OrderBy(x => x.Loan.LoanDate).ToList(),
            SortValues.LoanDateDesc => elements = elements.OrderByDescending(x => x.Loan.LoanDate).ToList(),
            SortValues.ReturnDateAsc => elements = elements.OrderBy(x => x.Loan.ReturnDate).ToList(),
            SortValues.ReturnDateDesc => elements = elements.OrderByDescending(x => x.Loan.ReturnDate).ToList(),
            _ => elements = elements.OrderByDescending(x => x.Loan.LoanDate).ToList(),
        };

        public static List<ReservationViewModel> SortElementsReservation(List<ReservationViewModel> elements, SortValues sortValues) =>
        sortValues switch
        {
            SortValues.UserNameAsc => elements = elements.OrderBy(x => x.User.UserName).ToList(),
            SortValues.UserNameDesc => elements = elements.OrderByDescending(x => x.User.UserName).ToList(),
            SortValues.TitleAsc => elements = elements.OrderBy(x => x.Book.Title).ToList(),
            SortValues.TitleDesc => elements = elements.OrderByDescending(x => x.Book.Title).ToList(),
            SortValues.LastNameAsc => elements = elements.OrderBy(x => x.Author.LastName).ToList(),
            SortValues.LastNameDesc => elements = elements.OrderByDescending(x => x.Author.LastName).ToList(),
            SortValues.NumberAsc => elements = elements.OrderBy(x => x.Copy.Number).ToList(),
            SortValues.NumberDesc => elements = elements.OrderByDescending(x => x.Copy.Number).ToList(),
            SortValues.DateAsc => elements = elements.OrderBy(x => x.Copy.Date).ToList(),
            SortValues.DateDesc => elements = elements.OrderByDescending(x => x.Copy.Date).ToList(),
            _ => elements = elements.OrderByDescending(x => x.Copy.Date).ToList(),
        };

        public static List<UserViewModel> SortElementsUser(List<UserViewModel> elements, SortValues sortValues) =>
        sortValues switch
        {
            SortValues.UserNameAsc => elements = elements.OrderBy(x => x.User.UserName).ToList(),
            SortValues.UserNameDesc => elements = elements.OrderByDescending(x => x.User.UserName).ToList(),
            SortValues.EmailAsc => elements = elements.OrderBy(x => x.User.Email).ToList(),
            SortValues.EmailDesc => elements = elements.OrderByDescending(x => x.User.Email).ToList(),
            SortValues.RoleAsc => elements = elements.OrderBy(x => x.Role.Name).ToList(),
            SortValues.RoleDesc => elements = elements.OrderByDescending(x => x.Role.Name).ToList(),
            _ => elements = elements.OrderByDescending(x => x.User.UserName).ToList(),
        };

        //FILE
        public static string UploadFile(IWebHostEnvironment webHostEnvironment, BookCreateEditViewModel model, string folderName)
        {
            string fileName = null;
            if (model.File != null){
                string destinationFolder = Path.Combine(webHostEnvironment.WebRootPath, folderName);
                Directory.CreateDirectory(destinationFolder);

                string fileExtension = model.File.FileName;
                fileName = Guid.NewGuid().ToString() + fileExtension.Substring(fileExtension.LastIndexOf('.'));
                string filePath = Path.Combine(destinationFolder, fileName);
                using (var stream = File.Create(filePath))
                {
                    model.File.CopyTo(stream);
                }
            }
            return fileName;
        }

        public static void DeleteFile(IWebHostEnvironment webHostEnvironment, string newFileName, string folderName)
        {
            string destinationFolder = Path.Combine(webHostEnvironment.WebRootPath, folderName);
            string filePath = Path.Combine(destinationFolder, newFileName);

            if (File.Exists(filePath)){
                File.Delete(filePath);
            }
        }
    }
}
