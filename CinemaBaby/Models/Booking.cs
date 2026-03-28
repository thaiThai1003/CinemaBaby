using System;
using System.Collections.Generic;

namespace CinemaBaby.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? CustomerId { get; set; }

    public int? SeatId { get; set; }

    public DateTime? BookingDate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Seat? Seat { get; set; }
}
