using CinemaBaby.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CinemaBaby.Repositories
{
    public class ShowtimeRepository : IShowtimeRepository
    {
        private readonly CinemaBookingDbContext _context;

        public ShowtimeRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public List<Showtime> GetAll()
        {
            return _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .ToList();
        }

        public List<Showtime> GetByCinema(int cinemaId)
        {
            return _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Cinema)
                .Where(s => s.CinemaId == cinemaId)
                .ToList();
        }
    }
}