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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
            
        public async Task<Categories_Temp> addToCartAsync(Categories_Temp itemAdd)
        {
            var custId = _httpContextAccessor.HttpContext?.Session.GetInt32("IdCust") ?? 0;
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
            // 1. Buat order baru
            var order = new Orders
            {
                IDcust = custId,
                OrderDate = DateTime.Now,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // simpan dulu biar dapat OrderID
            return itemAdd;

            
        }

        public async Task<Categories_Temp> ClearCarttAsync(int idCust)
        {
            var cartItems = await _context.Categories_Temp.Where(c => c.IdCust == idCust).ToListAsync();

            if (cartItems == null || !cartItems.Any())
                throw new Exception("Keranjang kosong.");

            var orderblm = await _context.Orders
                .Where(o => o.IDcust == idCust && o.Status == "Pending")
                .OrderByDescending(or => or.OrderID)
                .FirstOrDefaultAsync();

            if (orderblm != null)
            {
                orderblm.Status = "Success";
                _context.Update(orderblm);
                await _context.SaveChangesAsync();
            }

            var order = await _context.Orders
                .Where(o => o.IDcust == idCust && o.Status == "Success")
                .OrderByDescending(or => or.OrderID)
                .FirstOrDefaultAsync();

            // 2. Insert detail
            foreach (var item in cartItems)
            {
                var detail = new OrdersDetail
                {
                    OrderID = order.OrderID,
                    IdCategory = item.IdCategory,
                    StrCategory = item.StrCategory,
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
                if (qty <= 0)
                {
                    _context.Categories_Temp.Remove(cartItem);
                }
                else
                {
                    cartItem.Jml = qty;
                    _context.Categories_Temp.Update(cartItem);
                }
                await _context.SaveChangesAsync();
            }
            return cartItem;
        }
    }
}