using System.ComponentModel.DataAnnotations;

namespace QuanLyHomestay.Api.Models;

public class Customer
{
    public int Id { get; set; }
    [Required, StringLength(120)] public string FullName { get; set; } = "";
    [Required, StringLength(30)] public string Phone { get; set; } = "";
    [EmailAddress, StringLength(120)] public string? Email { get; set; }
}
