using System;
using System.Collections.Generic;

namespace CinemaBaby.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public int? ShowtimeId { get; set; }

    public string? SeatNumber { get; set; }

    public bool? IsBooked { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Showtime? Showtime { get; set; }
}
