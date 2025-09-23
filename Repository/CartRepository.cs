using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheMealDBApp.Data;
using TheMealDBApp.DTOs;
using TheMealDBApp.Interface;
using TheMealDBApp.Models;

namespace TheMealDBApp.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDBContext _context;
        public CartRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Categories_Temp> addToCartAsync(Categories_Temp itemAdd)
        {
            var exist = await _context.Categories_Temp.FirstOrDefaultAsync(c => c.IdCust == itemAdd.IdCust && c.IdCategory == itemAdd.IdCategory);
            if (exist != null)
            {
                exist.Jml += itemAdd.Jml;
                _context.Categories_Temp.Update(exist);
            }
            else
            {
                await _context.Categories_Temp.AddAsync(itemAdd);
            }
            await _context.SaveChangesAsync();
            return itemAdd;
        }

        public async Task<Categories_Temp> ClearCarttAsync(int idCust)
        {
            var cartItems = await _context.Categories_Temp.Where(c => c.IdCust == idCust).ToListAsync();

            if (cartItems == null || !cartItems.Any())
                throw new Exception("Keranjang kosong.");
            
            // 1. Buat order baru
            var order = new Orders
            {
                IDcust = idCust,
                OrderDate = DateTime.Now,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // simpan dulu biar dapat OrderID

            // 2. Insert detail
            foreach (var item in cartItems)
            {
                var detail = new OrdersDetail
                {
                    OrderID = order.OrderID,  
                    IdCategory = item.IdCategory,
                    Jml = item.Jml ?? 1
                };
                _context.OrdersDetail.Add(detail);
            }

            // 3. Simpan detail
            await _context.SaveChangesAsync();

            // 4. Clear cart
            _context.Categories_Temp.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return cartItems.Last();
        }

        public Task<Categories_Temp>? createMealAsync(Categories_Temp createMealDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<Categories_Temp>>? getAllMealsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Categories_Temp>> GetCartAsync(int idCust)
        {
            return await _context.Categories_Temp.Where(c => c.IdCust == idCust).ToListAsync();
        }

        public async Task<Categories_Temp> UpdateQtyAsync(int idCust, int idCategory, int qty)
        {
            var cartItem = await _context.Categories_Temp.FirstOrDefaultAsync(c => c.IdCust == idCust && c.IdCategory == idCategory);
            if (cartItem != null)
            {
                cartItem.Jml = qty;
                _context.Categories_Temp.Update(cartItem);
                await _context.SaveChangesAsync();
            }
            return cartItem;
        }
    }
}