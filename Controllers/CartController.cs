using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheMealDBApp.Data;
using TheMealDBApp.Filters;
using TheMealDBApp.Interface;
// using TheMealDBApp.Migrations;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;

using TheMealDBApp.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;
using iText.Layout.Borders;

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
            var now = DateTime.Now;
            var custId = HttpContext.Session.GetInt32("IdCust") ?? 0;
            var orders = await _cartRepo.GetCartAsync(custId);

            var pdfBytes = await GeneratePdf(orders); // sementara blank
            var result = await _cartRepo.ClearCarttAsync(custId);
            return File(pdfBytes, "application/pdf", $"Order{custId}_{now:ddMMyyhhmmss}.pdf");
            // return RedirectToAction("Index", "Home");
        }

        private async Task<byte[]> GeneratePdf(List<Categories_Temp> orders)
        {
            using (var ms = new MemoryStream()) // MemoryStream berguna untuk return PDF langsung tanpa membuat file sementara
            {
                var custId = HttpContext.Session.GetInt32("IdCust") ?? 0;
                var name = await _context.Users.Where(u => u.Id == custId).Select(u => u.Name).FirstOrDefaultAsync(); //select name from users where id = custId
                // Buat dokumen A4
                // Init writer & document
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var doc = new Document(pdf, PageSize.A4);

                // Tambahkan watermark
                var canvas = new PdfCanvas(pdf.AddNewPage()); // declare variable penyimpan kanvas
                // Atur opacity (transparansi)
                var gState = new PdfExtGState().SetFillOpacity(0.2f);
                canvas.SaveState();
                canvas.SetExtGState(gState);

                var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                float angle = (float)(45 * Math.PI / 180); // derajat ke radian
                float cos = (float)Math.Cos(angle);
                float sin = (float)Math.Sin(angle);
                float x = 300;
                float y = 450;
                canvas.BeginText();
                canvas.SetFontAndSize(font, 50);
                canvas.SetTextMatrix(cos, sin, -sin, cos, x, y);
                canvas.ShowText("Glow N Bliss");
                canvas.EndText();
                canvas.RestoreState();

                var isi = await _context.OrdersDetail
                    .Where(od => od.Order.IDcust == custId && od.Order.Status == "Success")
                    .Include(od => od.Order)
                    .ToListAsync();

                var satuOrder = isi.OrderByDescending(od => od.Order.OrderDate).FirstOrDefault();

                if (satuOrder != null)
                {
                    var now = DateTime.Now;
                    var orderDate = satuOrder.Order.OrderDate;
                    doc.Add(new Paragraph($"Hari/Tanggal: {orderDate:dddd, dd-MM-yyyy}"));
                    doc.Add(new Paragraph($"Jam: {orderDate:HH:mm:ss}"));
                    doc.Add(new Paragraph($"Nama Kasir: {name}"));
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("Detail Order:"));
                    // Buat tabel dengan 4 kolom
                    var table = new Table(3, true);
                    table.AddHeaderCell("ID Category");
                    table.AddHeaderCell("Category");
                    table.AddHeaderCell("Quantity");
                    // table.AddHeaderCell("Subtotal");

                    foreach (var item in isi)
                    {
                        table.AddCell(item.IdCategory.ToString());
                        table.AddCell(item.StrCategory ?? "");
                        table.AddCell(item.Jml.ToString());
                    }
                    table.SetBorder(new SolidBorder(1));
                    doc.Add(table);
                }
                Console.WriteLine("isi count: " + isi.Count);
                doc.Close();
                return ms.ToArray();
            }
        }

    }
}