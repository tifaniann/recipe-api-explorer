using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TheMealDBApp.Filters;
using TheMealDBApp.Models;

namespace TheMealDBApp.Controllers;
[AuthFilter]
public class HomeController : Controller
{
    private readonly HttpClient _httpClient;
    public HomeController(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
            return View(categoryResponse?.Categories);
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
