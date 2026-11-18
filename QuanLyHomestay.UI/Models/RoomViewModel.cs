namespace QuanLyHomestay.UI.Models;
public class RoomViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string RoomType { get; set; } = "";
    public int PricePerNight { get; set; }
    public string Status { get; set; } = "Trống";
}
