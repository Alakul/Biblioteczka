using Biblioteczka.Data;
using Biblioteczka.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Biblioteczka.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        AppDbContext db;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
            IndexViewModel indexViewModel = new IndexViewModel();
            int size = 6;

            if (db.Book.Count() < size){
                indexViewModel.BooksDate = db.Book.OrderBy(x => x.Date).ToList();
                indexViewModel.BooksYear = db.Book.OrderBy(x => x.Year).ToList();
            }
            else {
                indexViewModel.BooksDate = db.Book.OrderBy(x => x.Date).Take(size).ToList();
                indexViewModel.BooksYear = db.Book.OrderBy(x => x.Year).Take(size).ToList();
            }
            indexViewModel.Authors = db.Author.ToList();

            return View(indexViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}