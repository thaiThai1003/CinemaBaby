using CinemaBaby.Models;
using CinemaBaby.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CinemaBaby.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _repository;

        // ❌ BỎ _context
        public MoviesController(IMovieRepository repository)
        {
            _repository = repository;
        }

        // ================= ADMIN =================

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var movies = _repository.GetAll();
            return View(movies);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(movie); // ✅ dùng repository
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // 🔥 DELETE
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id); // ✅ chuyển hết vào repo
            return RedirectToAction("Index");
        }

        // ================= USER + ADMIN =================

        public IActionResult Details(int id)
        {
            var movie = _repository.GetById(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        // 🔥 PHIM ĐANG CHIẾU
        public IActionResult NowShowing()
        {
            var movies = _repository.GetNowShowing(); // ✅ chuyển vào repo
            return View(movies);
        }

        // 🔥 PHIM SẮP CHIẾU
        public IActionResult Upcoming()
        {
            var movies = _repository.GetUpcoming(); // ✅ chuyển vào repo
            return View(movies);
        }

        // ================= 🔥 BẬT / TẮT BÁN VÉ =================

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult TogglePreSale(int id)
        {
            _repository.TogglePreSale(id); // ✅ repo xử lý
            return RedirectToAction("Index");
        }

        public IActionResult Trailer()
        {
            var movies = _repository.GetAll(); // ✅ không dùng context
            return View(movies);
        }
    }
}