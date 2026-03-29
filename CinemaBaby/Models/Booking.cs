using System;

namespace CinemaBaby.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        // 🔥 lưu danh sách ghế (A1,A2,B3...)
        public string Seats { get; set; }

        // 🔥 tổng tiền
        public decimal TotalPrice { get; set; }

        // 🔥 trạng thái thanh toán
        public string Status { get; set; } = "Pending";

        // 🔥 ngày đặt
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}