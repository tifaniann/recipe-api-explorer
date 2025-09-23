using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheMealDBApp.Data;
using TheMealDBApp.Filters;
using TheMealDBApp.Models;

namespace TheMealDBApp.Controllers;
[AuthFilter] // filter untuk mengecek apakah user sudah login atau belum
public class HomeController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDBContext _context;
    public HomeController(HttpClient httpClient, ApplicationDBContext context)
    {
        _httpClient = httpClient;
        _context = context;
    }
    public async Task<IActionResult> Index() // Home/Index.cshtml (nama action(fungsi) harus sama dengan nama file .cshtml dan nama folder harus sama dengan nama controller)
    {
        // return View(); // Views/{NamaController}/{NamaAction}.cshtml --> Views/Home/Index.cshtml
        var response = await _httpClient.GetAsync("https://www.themealdb.com/api/json/v1/1/categories.php");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(); // membaca response body sebagai string
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true }; //abaikan case-sensitivity
            var categoryResponse = JsonSerializer.Deserialize<CategoriesResponse>(content, options); //memetakan JSON ke object C# (di models/categories.cs)
            return View(categoryResponse?.Categories); //mengirimkan data ke view, categories adalah model yang akan digunakan di Views/Home/Index.cshtml
        }
        else
        {
            return View("NotFoundError");
        }   
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> Order()
    {
        var idCust = HttpContext.Session.GetInt32("IdCust");
        if (idCust == null)
        {
            return RedirectToAction("Login", "Auth");
        }
        var orderList = await _context.Categories_Temp.Where(c => c.IdCust == idCust).ToListAsync();
        return View(orderList);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
