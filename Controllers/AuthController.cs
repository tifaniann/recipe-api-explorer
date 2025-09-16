using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheMealDBApp.Data;
using TheMealDBApp.Interface;
using TheMealDBApp.Models;

namespace TheMealDBApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IUserRepository _userRepo;
        public AuthController(ApplicationDBContext context, IUserRepository userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task <IActionResult> Login(Users model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userRepo.LoginUserAsync(model);
            if (user != null)
            {
                HttpContext.Session.SetInt32("IdCust", user.Id); 
                HttpContext.Session.SetString("Username", user.Username); // HttpContext.Session.SetString(key, value) untuk menyimpan data di session
                return RedirectToAction("Index", "Home"); // RedirectToAction("Action", "Controller")
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Users model)
        {
            if (model == null)
            {
                return BadRequest("Data wajib diisi!");
            }

            var result = await _userRepo.RegisterUserAsync(model);

            if (result == null) // null bukan berarti data kosong, tapi berarti service kasih tanda gagal (karena sudah ada user sebelumnya
            {
                ModelState.AddModelError("", "Username atau Email sudah terdaftar."); 
                return View(model); 
            }
            TempData["Message"] = "Akun berhasil dibuat. Silakan login!";
            return RedirectToAction("Login");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}