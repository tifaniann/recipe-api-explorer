using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheMealDBApp.Data;
using TheMealDBApp.Interface;
using TheMealDBApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace TheMealDBApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public Task<Users?> LoginUserAsync(Users model)
        {
            // u => u.Username == model.Username filter berdasarkan kolom Username di table yang harus sama dengan model.Username (username yang diinput user).
            // var user = berisi user dari database yang punya Username sama dengan input dari user yang sedang login
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username); // firstOrDefault untuk ambil satu data, select top 1

            if (user != null)
            {
                if (user.Password == model.Password)
                {
                    return Task.FromResult<Users?>(user);
                }
            }
            return Task.FromResult<Users?>(null);
        }

        public async Task<Users?> RegisterUserAsync(Users model)
        {
            // Cek apakah username atau email sudah dipakai
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Username == model.Username || u.Email == model.Email);

            if (existingUser != null)
            {
                return null;
            }

            await _context.Users.AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }
    }
}