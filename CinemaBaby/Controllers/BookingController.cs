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

            if (!showtimes.Any())
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

        // 🔹 Đặt nhiều ghế + chuyển thanh toán
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

            // ✅ đánh dấu ghế đã đặt
            foreach (var seat in seats)
            {
                if (seat.IsBooked != true)
                {
                    seat.IsBooked = true;
                }
            }

            // ✅ tính tiền
            decimal pricePerSeat = 50000;
            decimal total = seats.Count * pricePerSeat;

            // ✅ lưu booking
            var booking = new Booking
            {
                Seats = string.Join(",", seats.Select(s => s.SeatNumber)),
                TotalPrice = total,
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            // 🚀 chuyển sang thanh toán
            return RedirectToAction("Payment", new { id = booking.BookingId });
        }

        // 🔹 Trang thanh toán
        public IActionResult Payment(int id)
        {
            var booking = _context.Bookings.Find(id);

            if (booking == null)
                return RedirectToAction("Index", "Home");

            return View(booking);
        }
    }
}