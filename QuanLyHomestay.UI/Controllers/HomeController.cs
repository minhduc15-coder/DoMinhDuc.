using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using QuanLyHomestay.UI.Models;

namespace QuanLyHomestay.UI.Controllers;

public class HomeController(IHttpClientFactory factory) : Controller
{
    private HttpClient Api => factory.CreateClient("HomestayApi");

    public async Task<IActionResult> Index()
    {
        var rooms = await Api.GetFromJsonAsync<List<RoomViewModel>>("api/rooms") ?? [];
        var customers = await Api.GetFromJsonAsync<List<CustomerViewModel>>("api/customers") ?? [];
        var bookings = await Api.GetFromJsonAsync<List<BookingViewModel>>("api/bookings") ?? [];
        ViewBag.RoomCount = rooms.Count;
        ViewBag.Available = rooms.Count(x => x.Status == "Trống");
        ViewBag.CustomerCount = customers.Count;
        ViewBag.BookingCount = bookings.Count;
        ViewBag.Revenue = bookings.Where(x => x.Status == "Đã trả phòng").Sum(x => x.TotalPrice);
        ViewBag.Latest = bookings.Take(5).ToList();
        return View();
    }

    public async Task<IActionResult> Rooms()
    {
        ViewBag.Rooms = await Api.GetFromJsonAsync<List<RoomViewModel>>("api/rooms") ?? [];
        return View();
    }

    [HttpPost] public async Task<IActionResult> AddRoom(RoomViewModel room)
    {
        var response = await Api.PostAsJsonAsync("api/rooms", room);
        TempData["Message"] = response.IsSuccessStatusCode ? "Đã thêm phòng." : "Không thể thêm phòng.";
        return RedirectToAction(nameof(Rooms));
    }

    [HttpPost] public async Task<IActionResult> DeleteRoom(int id)
    {
        var response = await Api.DeleteAsync($"api/rooms/{id}");
        TempData["Message"] = response.IsSuccessStatusCode ? "Đã xóa phòng." : "Không thể xóa phòng đã có lịch đặt.";
        return RedirectToAction(nameof(Rooms));
    }

    public async Task<IActionResult> Customers()
    {
        ViewBag.Customers = await Api.GetFromJsonAsync<List<CustomerViewModel>>("api/customers") ?? [];
        return View();
    }

    [HttpPost] public async Task<IActionResult> AddCustomer(CustomerViewModel customer)
    {
        var response = await Api.PostAsJsonAsync("api/customers", customer);
        TempData["Message"] = response.IsSuccessStatusCode ? "Đã thêm khách hàng." : "Không thể thêm khách hàng.";
        return RedirectToAction(nameof(Customers));
    }

    public async Task<IActionResult> Bookings()
    {
        ViewBag.Bookings = await Api.GetFromJsonAsync<List<BookingViewModel>>("api/bookings") ?? [];
        ViewBag.Rooms = (await Api.GetFromJsonAsync<List<RoomViewModel>>("api/rooms") ?? []).Where(x => x.Status == "Trống").ToList();
        ViewBag.Customers = await Api.GetFromJsonAsync<List<CustomerViewModel>>("api/customers") ?? [];
        return View();
    }

    [HttpPost] public async Task<IActionResult> AddBooking(BookingViewModel booking)
    {
        var response = await Api.PostAsJsonAsync("api/bookings", booking);
        TempData["Message"] = response.IsSuccessStatusCode ? "Đã tạo đặt phòng." : await response.Content.ReadAsStringAsync();
        return RedirectToAction(nameof(Bookings));
    }

    [HttpPost] public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var response = await Api.PatchAsJsonAsync($"api/bookings/{id}/status", status);
        TempData["Message"] = response.IsSuccessStatusCode ? "Đã cập nhật trạng thái." : "Không thể cập nhật trạng thái.";
        return RedirectToAction(nameof(Bookings));
    }
}
