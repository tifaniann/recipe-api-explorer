using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheMealDBApp.Data;
using TheMealDBApp.Filters;
using TheMealDBApp.Interface;
// using TheMealDBApp.Migrations;
using TheMealDBApp.Models;

namespace TheMealDBApp.Controllers
{
    [AuthFilter]

    [Route("Meal/[controller]")]
    public class CartController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly ICartRepository _cartRepo;
        public CartController(ApplicationDBContext context, ICartRepository cartRepo)
        {
            _context = context;
            _cartRepo = cartRepo;
        }

        public async Task<IActionResult> Index()
        {
            var idCust = HttpContext.Session.GetInt32("IdCust");
            if (!idCust.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cart = await _cartRepo.GetCartAsync(idCust.Value);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int idCategory, string strCategory, string StrCategoryThumb, int qty = 1)
        {
            var idCust = HttpContext.Session.GetInt32("IdCust");
            if (idCust == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            await _cartRepo.addToCartAsync(new Categories_Temp
            {
                IdCust = idCust.Value,
                IdCategory = idCategory,
                StrCategory = strCategory,
                StrCategoryThumb = StrCategoryThumb,
                Jml = qty
            });

            return RedirectToAction("Order", "Home");
        }

        // [HttpPost]
        // public async Task<IActionResult> UpdateQty(int idCategory, int qty)
        // {
        //     var idCust = HttpContext.Session.GetInt32("IdCust");
        //     if (idCust == null)
        //     {
        //         return RedirectToAction("Login", "Auth");
        //     }

        //     await _cartRepo.UpdateQtyAsync(idCust.Value, idCategory, qty);

        //     return RedirectToAction("Index");
        // }

        // [HttpPost]
        // public async Task<IActionResult> Checkout()
        // {
        //     var idCust = HttpContext.Session.GetInt32("IdCust");
        //     if (idCust == null)
        //     {
        //         return RedirectToAction("Login", "Auth");
        //     }

        //     // panggil ClearCarttAsync
        //     var result = await _cartRepo.ClearCarttAsync(idCust.Value);

        //     return RedirectToAction("Index", "Home");
        // }

    }
}