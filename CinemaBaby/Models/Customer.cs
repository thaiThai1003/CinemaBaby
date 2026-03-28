using System;
using System.Collections.Generic;

namespace CinemaBaby.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public int? Points { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
