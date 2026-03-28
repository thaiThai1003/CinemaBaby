using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaBaby.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Genre { get; set; }

        public int Duration { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; } // 👈 QUAN TRỌNG

        public virtual ICollection<Showtime>? Showtimes { get; set; }

        public bool IsPreSale { get; set; } = false;

        public string? TrailerUrl { get; set; }

       
    }
}