namespace QuanLyHomestay.UI.Models;
public class BookingViewModel
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public CustomerViewModel? Customer { get; set; }
    public int RoomId { get; set; }
    public RoomViewModel? Room { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Guests { get; set; }
    public int TotalPrice { get; set; }
    public string Status { get; set; } = "Đã đặt";
}
