using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyHomestay.Api.Data;
using QuanLyHomestay.Api.Models;

namespace QuanLyHomestay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(HomestayDbContext db) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await db.Rooms.OrderBy(x => x.Id).ToListAsync());
    [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id)
    {
        var room = await db.Rooms.FindAsync(id);
        return room is null ? NotFound() : Ok(room);
    }
    [HttpPost] public async Task<IActionResult> Create(Room room)
    {
        db.Rooms.Add(room); await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
    }
    [HttpPut("{id:int}")] public async Task<IActionResult> Update(int id, Room input)
    {
        var room = await db.Rooms.FindAsync(id);
        if (room is null) return NotFound();
        room.Name = input.Name; room.RoomType = input.RoomType;
        room.PricePerNight = input.PricePerNight; room.Status = input.Status;
        await db.SaveChangesAsync(); return Ok(room);
    }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id)
    {
        var room = await db.Rooms.FindAsync(id);
        if (room is null) return NotFound();
        if (await db.Bookings.AnyAsync(b => b.RoomId == id))
            return Conflict("Không thể xóa phòng đã có lịch đặt.");
        db.Rooms.Remove(room); await db.SaveChangesAsync(); return NoContent();
    }
}
