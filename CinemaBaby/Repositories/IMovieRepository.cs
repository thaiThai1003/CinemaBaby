using CinemaBaby.Models;
using System.Collections.Generic;

namespace CinemaBaby.Repositories
{
    public interface IMovieRepository
    {
        List<Movie> GetAll();
        Movie GetById(int id);

        // 🔥 THÊM CÁC HÀM NÀY
        void Add(Movie movie);
        void Delete(int id);

        List<Movie> GetNowShowing();
        List<Movie> GetUpcoming();

        void TogglePreSale(int id);
    }
}