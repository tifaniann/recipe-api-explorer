using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Task<Categories_Temp>? addToCartAsync(Categories_Temp itemAdd)
        {
            throw new NotImplementedException();
        }

        public Task<Categories_Temp>? ClearCarttAsync(int idCust)
        {
            throw new NotImplementedException();
        }

        public Task<Categories_Temp>? createMealAsync(Categories_Temp createMealDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<Categories_Temp>>? getAllMealsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Categories_Temp>>? GetCartAsync(int idCust)
        {
            throw new NotImplementedException();
        }

        public Task<Categories_Temp>? UpdateQtyAsync(int idCust, int idCategory, int qty)
        {
            throw new NotImplementedException();
        }
    }
}