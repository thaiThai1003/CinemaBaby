using CinemaBaby.Models;
using System.Collections.Generic;

namespace CinemaBaby.Repositories
{
    public interface IShowtimeRepository
    {
        List<Showtime> GetAll();
        List<Showtime> GetByCinema(int cinemaId);
    }
}