using System.ComponentModel.DataAnnotations;

namespace QuanLyHomestay.Api.Models;

public class Room
{
    public int Id { get; set; }
    [Required, StringLength(80)] public string Name { get; set; } = "";
    [Required, StringLength(80)] public string RoomType { get; set; } = "";
    [Range(0, int.MaxValue)] public int PricePerNight { get; set; }
    [StringLength(30)] public string Status { get; set; } = "Trống";
}
