using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheMealDBApp.Data;
using TheMealDBApp.Models;

namespace TheMealDBApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDBContext _context;
        public AuthController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(Users model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
                if (user != null)
                {
                    // Kalau password masih plain text
                    if (user.Password == model.Password)
                    {
                        HttpContext.Session.SetString("Username", user.Username);
                        return RedirectToAction("Index", "Home");
                    }

                    // Kalau sudah pakai hash (mis. BCrypt)
                    // if (BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash)) { ... }
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Users model)
        {
            if (ModelState.IsValid)
            {
                // Cek apakah username atau email sudah dipakai
                var existingUser = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username || u.Email == model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username atau Email sudah terdaftar.");
                    return View(model);
                }

                // Simpan user baru ke database
                _context.Users.Add(model);
                _context.SaveChanges();

                TempData["Message"] = "Akun berhasil dibuat. Silakan login!";
                return RedirectToAction("Login");
            }

            return View(model);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}