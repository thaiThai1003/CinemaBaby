using System.ComponentModel.DataAnnotations;

namespace CinemaBaby.Models
{
    public class Cinema
    {
        [Key]
        public int CinemaId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}