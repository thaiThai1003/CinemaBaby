using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CinemaBaby.Models   // 🔥 THÊM DÒNG NÀY
{
    public class Showtime
    {
        public int ShowtimeId { get; set; }

        public DateTime ShowDate { get; set; }

        public decimal Price { get; set; }

        public int MovieId { get; set; }

        [ValidateNever]
        public virtual Movie Movie { get; set; }

        [ValidateNever]
        public virtual ICollection<Seat> Seats { get; set; }

        public int? CinemaId { get; set; }

        [ValidateNever]
        public Cinema Cinema { get; set; }
    }
}