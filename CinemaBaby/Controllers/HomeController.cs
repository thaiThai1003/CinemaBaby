using System.Diagnostics;
using CinemaBaby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBaby.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CinemaBookingDbContext _context;


    public HomeController(ILogger<HomeController> logger, CinemaBookingDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            var movies = _context.Movies
                .Include(m => m.Showtimes)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.ToLower().Contains(searchString.ToLower()));
            }

            ViewBag.CurrentFilter = searchString;

            return View(movies.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }


}
