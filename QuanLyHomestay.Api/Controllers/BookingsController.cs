using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyHomestay.Api.Data;
using QuanLyHomestay.Api.Models;

namespace QuanLyHomestay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController(HomestayDbContext db) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> GetAll()
    {
        var list = await db.Bookings.Include(x => x.Customer).Include(x => x.Room)
            .OrderByDescending(x => x.Id).ToListAsync();
        return Ok(list);
    }

    [HttpPost] public async Task<IActionResult> Create(Booking input)
    {
        if (input.CheckOut.Date <= input.CheckIn.Date)
            return BadRequest("Ngày trả phòng phải sau ngày nhận phòng.");
        var room = await db.Rooms.FindAsync(input.RoomId);
        var customer = await db.Customers.FindAsync(input.CustomerId);
        if (room is null || customer is null) return BadRequest("Phòng hoặc khách hàng không tồn tại.");
        if (room.Status != "Trống") return Conflict("Phòng hiện không trống.");
        input.TotalPrice = (input.CheckOut.Date - input.CheckIn.Date).Days * room.PricePerNight;
        input.Status = "Đã đặt";
        room.Status = "Đã đặt";
        db.Bookings.Add(input);
        await db.SaveChangesAsync();
        return Ok(input);
    }

    [HttpPatch("{id:int}/status")] public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        var booking = await db.Bookings.Include(x => x.Room).FirstOrDefaultAsync(x => x.Id == id);
        if (booking is null) return NotFound();
        if (!new[] { "Đã đặt", "Đang ở", "Đã trả phòng", "Đã hủy" }.Contains(status))
            return BadRequest("Trạng thái không hợp lệ.");
        booking.Status = status;
        if (booking.Room is not null)
            booking.Room.Status = status == "Đang ở" ? "Đang sử dụng" :
                status is "Đã trả phòng" or "Đã hủy" ? "Trống" : "Đã đặt";
        await db.SaveChangesAsync();
        return Ok(booking);
    }
}
