using CinemaBaby.Models;
using CinemaBaby.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace CinemaBaby.Controllers
{
    public class ShowtimeController : Controller
    {
        private readonly IShowtimeRepository _repository;
        private readonly CinemaBookingDbContext _context;

        public ShowtimeController(IShowtimeRepository repository, CinemaBookingDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        // ================= DANH SÁCH =================
        public IActionResult Index()
        {
            var showtimes = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .ToList();

            return View(showtimes);
        }

        // ================= CREATE =================
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            LoadDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Showtime showtime)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdown();
                return View(showtime);
            }

            _context.Showtimes.Add(showtime);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT =================
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var showtime = _context.Showtimes.Find(id);
            if (showtime == null) return NotFound();

            LoadDropdown(showtime.MovieId, showtime.CinemaId);

            return View(showtime);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, Showtime showtime)
        {
            if (id != showtime.ShowtimeId) return NotFound();

            if (!ModelState.IsValid)
            {
                LoadDropdown(showtime.MovieId, showtime.CinemaId);
                return View(showtime);
            }

            var existing = _context.Showtimes.Find(id);
            if (existing == null) return NotFound();

            existing.MovieId = showtime.MovieId;
            existing.ShowDate = showtime.ShowDate;
            existing.Price = showtime.Price;
            existing.CinemaId = showtime.CinemaId;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE =================
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var showtime = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .FirstOrDefault(s => s.ShowtimeId == id);

            if (showtime == null) return NotFound();

            return View(showtime);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var showtime = _context.Showtimes.Find(id);

            if (showtime != null)
            {
                _context.Showtimes.Remove(showtime);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= 🔥 LOAD DROPDOWN (QUAN TRỌNG) =================
        private void LoadDropdown(int? selectedMovie = null, int? selectedCinema = null)
        {
            ViewBag.Movies = new SelectList(
                _context.Movies.Where(m => m.IsPreSale == true),
                "MovieId",
                "Title",
                selectedMovie
            );

            ViewBag.Cinemas = new SelectList(
                _context.Cinemas,
                "CinemaId",
                "Name",
                selectedCinema
            );
        }

        // ================= 🔥 RẠP GẦN BẠN =================
        public IActionResult Nearby(double? lat, double? lng)
        {
            var cinemas = _context.Cinemas.ToList();

            var result = cinemas.Select(c => new
            {
                CinemaName = c.Name,
                Address = c.Address,
                Distance = (lat.HasValue && lng.HasValue)
                    ? GetDistance(lat.Value, lng.Value, c.Latitude, c.Longitude)
                    : 0
            })
            .OrderBy(x => x.Distance)
            .Take(5)
            .ToList();

            return View(result);
        }

        // ================= 📏 TÍNH KHOẢNG CÁCH =================
        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371;
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) *
                Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        // ================= 🏢 HỆ THỐNG RẠP =================
        public IActionResult CinemaList()
        {
            var cinemas = _context.Cinemas.ToList();

            var grouped = cinemas.GroupBy(c =>
            {
                if (c.Name.Contains("CGV")) return "CGV";
                if (c.Name.Contains("Lotte")) return "Lotte";
                if (c.Name.Contains("Galaxy")) return "Galaxy";
                if (c.Name.Contains("BHD")) return "BHD";
                if (c.Name.Contains("Beta")) return "Beta";
                if (c.Name.Contains("Mega")) return "Mega GS";
                return "Khác";
            }).ToList();

            return View(grouped);
        }

        // ================= 🎬 PHIM THEO RẠP =================
        public IActionResult ByCinema(int id)
        {
            var showtimes = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .Where(s => s.CinemaId == id)
                .ToList();

            return View(showtimes);
        }
    }
}