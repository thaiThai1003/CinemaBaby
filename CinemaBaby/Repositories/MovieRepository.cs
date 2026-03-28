using CinemaBaby.Models;
using System.Collections.Generic;
using System.Linq;

namespace CinemaBaby.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaBookingDbContext _context;

        public MovieRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie GetById(int id)
        {
            return _context.Movies.FirstOrDefault(m => m.MovieId == id);
        }

        // 🔥 THÊM PHIM
        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        // 🔥 XÓA PHIM + CASCADE
        public void Delete(int id)
        {
            var movie = GetById(id);
            if (movie == null) return;

            var showtimes = _context.Showtimes
                .Where(s => s.MovieId == id)
                .ToList();

            foreach (var showtime in showtimes)
            {
                var seats = _context.Seats
                    .Where(s => s.ShowtimeId == showtime.ShowtimeId)
                    .ToList();

                _context.Seats.RemoveRange(seats);
            }

            _context.Showtimes.RemoveRange(showtimes);
            _context.Movies.Remove(movie);

            _context.SaveChanges();
        }

        // 🔥 PHIM ĐANG CHIẾU
        public List<Movie> GetNowShowing()
        {
            return _context.Movies
                .Where(m => m.ReleaseDate <= DateTime.Now && m.IsPreSale == true)
                .ToList();
        }

        // 🔥 PHIM SẮP CHIẾU
        public List<Movie> GetUpcoming()
        {
            return _context.Movies
                .Where(m => m.ReleaseDate > DateTime.Now)
                .ToList();
        }

        // 🔥 BẬT/TẮT BÁN VÉ
        public void TogglePreSale(int id)
        {
            var movie = GetById(id);
            if (movie == null) return;

            movie.IsPreSale = !movie.IsPreSale;
            _context.SaveChanges();
        }
    }
}