using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheMealDBApp.Data;
using TheMealDBApp.Filters;

namespace TheMealDBApp.Controllers
{
    [AuthFilter]

    [Route("Meal/[controller]")]
    public class CartController : Controller
    {
        private readonly ApplicationDBContext _context;
        public CartController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] string categoryId)
        {
            // Proses tambah ke cart
            TempData["Success"] = true;
            return View("SuccessPage");  // kembali ke halaman index
        }
    }
}