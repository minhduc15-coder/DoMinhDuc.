using Microsoft.EntityFrameworkCore;
using QuanLyHomestay.Api.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HomestayDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=homestay.db"));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HomestayDbContext>();
    db.Database.EnsureCreated();
    if (!db.Rooms.Any())
    {
        db.Rooms.AddRange(
            new Models.Room { Name = "P101", RoomType = "Phòng đơn", PricePerNight = 350000, Status = "Trống" },
            new Models.Room { Name = "P102", RoomType = "Phòng đôi", PricePerNight = 550000, Status = "Trống" },
            new Models.Room { Name = "Villa A", RoomType = "Villa", PricePerNight = 1500000, Status = "Trống" }
        );
        db.Customers.AddRange(
            new Models.Customer { FullName = "Nguyễn Minh Anh", Phone = "0901234567", Email = "minhanh@example.com" },
            new Models.Customer { FullName = "Trần Hoàng Nam", Phone = "0912345678", Email = "nam@example.com" }
        );
        db.SaveChanges();
    }
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();
