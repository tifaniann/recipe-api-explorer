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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

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

        [HttpPost("UpdateQty/{idCategory}")]
        public async Task<IActionResult> UpdateQty(int idCategory, [FromForm] int qty)
        {
            var idCust = HttpContext.Session.GetInt32("IdCust");
            if (idCust == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            await _cartRepo.UpdateQtyAsync(idCust.Value, idCategory, qty);

            return RedirectToAction("Order", "Home");
        }

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

        [HttpPost("Cart/CheckoutPdf")]
        public async Task<IActionResult> CheckoutPdf()
        {
            var custId = HttpContext.Session.GetInt32("IdCust") ?? 0;
            var orders = _cartRepo.GetCartAsync(custId);

            var pdfBytes = GeneratePdf(orders); // sementara blank
            var result = await _cartRepo.ClearCarttAsync(custId);
            return File(pdfBytes, "application/pdf", "Order.pdf");
            // return RedirectToAction("Index", "Home");
        }

        private byte[] GeneratePdf(Task<List<Categories_Temp>>? orders)
        {
            using (var ms = new MemoryStream()) // MemoryStream berguna untuk return PDF langsung tanpa membuat file sementara
            {
                // Buat dokumen A4
                var doc = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Tambahkan watermark
                PdfContentByte cb = writer.DirectContentUnder; // declare variable penyimpan kanvas
                // Atur opacity (transparansi)
                PdfGState gState = new PdfGState();
                gState.FillOpacity = 0.2f; // 0.0 = transparan total, 1.0 = solid
                cb.SetGState(gState);
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.BeginText();
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.SetFontAndSize(bf, 50);
                cb.ShowTextAligned(Element.ALIGN_CENTER, "Glow N Bliss", 300, 450, 45); // x,y dan rotasi(atur kemiringan)
                cb.EndText();

                // Tambahkan tanggal dan jam
                var now = DateTime.Now;
                doc.Add(new Paragraph($"Hari/Tanggal: {now:dd-MM-yyyy}"));
                doc.Add(new Paragraph($"Jam: {now:HH:mm}"));

                doc.Close();
                return ms.ToArray();
            }
        }

    }
}