using System.ComponentModel.DataAnnotations;

namespace QuanLyHomestay.Api.Models;

public class Booking
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    [Range(1, 30)] public int Guests { get; set; } = 1;
    public int TotalPrice { get; set; }
    [StringLength(30)] public string Status { get; set; } = "Đã đặt";
}
