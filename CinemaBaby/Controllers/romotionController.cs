using CinemaBaby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace CinemaBaby.Controllers
{
    public class PromotionController : Controller
    {
        private readonly CinemaBookingDbContext _context;

        public PromotionController(CinemaBookingDbContext context)
        {
            _context = context;
        }

        // ================= USER + ADMIN =================
        public IActionResult Index()
        {
            var data = _context.Promotions.ToList();
            return View(data);
        }

        // ================= CREATE =================
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Promotion p)
        {
            _context.Promotions.Add(p);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // ================= 🔥 EDIT =================

        // GET: Edit
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var promo = _context.Promotions.FirstOrDefault(x => x.PromotionId == id);
            if (promo == null) return NotFound();

            return View(promo);
        }

        // POST: Edit
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Promotion promo)
        {
            var existing = _context.Promotions.FirstOrDefault(x => x.PromotionId == promo.PromotionId);
            if (existing == null) return NotFound();

            existing.Title = promo.Title;
            existing.Description = promo.Description;
            existing.StartDate = promo.StartDate;
            existing.EndDate = promo.EndDate;
            existing.ImageUrl = promo.ImageUrl;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================= DELETE =================
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var p = _context.Promotions.FirstOrDefault(x => x.PromotionId == id);

            if (p != null)
            {
                _context.Promotions.Remove(p);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}