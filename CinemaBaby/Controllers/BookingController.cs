using CinemaBaby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CinemaBaby.Controllers
{
    public class BookingController : Controller
    {
        private readonly CinemaBookingDbContext _context;

        public BookingController(CinemaBookingDbContext context)
        {
            _context = context;
        }

        // 🔹 Chọn suất chiếu
        public IActionResult SelectShowtime(int movieId)
        {
            

            var showtimes = _context.Showtimes
                .Include(s => s.Cinema) 
                .Where(s => s.MovieId == movieId)
                .ToList();
            {
                TempData["Message"] = "Phim này chưa có suất chiếu!";
            }

            return View(showtimes);
        }

        // 🔹 Chọn ghế
        public IActionResult SelectSeat(int showtimeId)
        {
            var seats = _context.Seats
                .Where(s => s.ShowtimeId == showtimeId)
                .ToList();

            // 🔥 Nếu chưa có ghế → tạo ghế
            if (!seats.Any())
            {
                for (int i = 1; i <= 5; i++)
                {
                    _context.Seats.Add(new Seat
                    {
                        SeatNumber = "A" + i,
                        IsBooked = false,
                        ShowtimeId = showtimeId
                    });
                }

                for (int i = 1; i <= 5; i++)
                {
                    _context.Seats.Add(new Seat
                    {
                        SeatNumber = "B" + i,
                        IsBooked = false,
                        ShowtimeId = showtimeId
                    });
                }

                _context.SaveChanges();

                seats = _context.Seats
                    .Where(s => s.ShowtimeId == showtimeId)
                    .ToList();
            }

            ViewBag.ShowtimeId = showtimeId;
            return View(seats);
        }

        // 🔹 Đặt 1 ghế
        [HttpPost]
        public IActionResult BookSeat(int seatId)
        {
            var seat = _context.Seats.FirstOrDefault(s => s.SeatId == seatId);

            // ❗ FIX lỗi null + tránh crash
            if (seat == null)
            {
                TempData["Message"] = "Không tìm thấy ghế!";
                return RedirectToAction("Index", "Home");
            }

            if (seat.IsBooked == true)
            {
                TempData["Message"] = "Ghế đã được đặt!";
                return RedirectToAction("SelectSeat", new { showtimeId = seat.ShowtimeId });
            }

            seat.IsBooked = true;
            _context.SaveChanges();

            TempData["Message"] = "Đặt vé thành công 🎉";
            return RedirectToAction("SelectSeat", new { showtimeId = seat.ShowtimeId });
        }

        // 🔹 Đặt nhiều ghế
        [HttpPost]
        public IActionResult BookMultipleSeats(string selectedSeats)
        {
            if (string.IsNullOrEmpty(selectedSeats))
            {
                TempData["Message"] = "Bạn chưa chọn ghế!";
                return RedirectToAction("Index", "Home");
            }

            var seatIds = selectedSeats.Split(',')
                                       .Select(int.Parse)
                                       .ToList();

            var seats = _context.Seats
                                .Where(s => seatIds.Contains(s.SeatId))
                                .ToList();

            if (!seats.Any())
            {
                TempData["Message"] = "Không tìm thấy ghế!";
                return RedirectToAction("Index", "Home");
            }

            foreach (var seat in seats)
            {
                if (seat.IsBooked != true)
                {
                    seat.IsBooked = true;
                }
            }

            _context.SaveChanges();

            TempData["Message"] = "Đặt nhiều ghế thành công 🎉";

            int showtimeId = seats.First().ShowtimeId ?? 0;

            return RedirectToAction("SelectSeat", new { showtimeId });
        }
    }
}