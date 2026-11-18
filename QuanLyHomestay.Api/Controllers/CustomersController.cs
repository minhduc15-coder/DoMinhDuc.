using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyHomestay.Api.Data;
using QuanLyHomestay.Api.Models;

namespace QuanLyHomestay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(HomestayDbContext db) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await db.Customers.OrderByDescending(x => x.Id).ToListAsync());
    [HttpPost] public async Task<IActionResult> Create(Customer customer)
    {
        db.Customers.Add(customer); await db.SaveChangesAsync();
        return Ok(customer);
    }
}
