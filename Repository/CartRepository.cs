using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheMealDBApp.Data;
using TheMealDBApp.DTOs;
using TheMealDBApp.Interface;

namespace TheMealDBApp.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDBContext _context;
        public CartRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public Task<Categories_Temp_DTO> createMealAsync(Categories_Temp_DTO createMealDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<Categories_Temp_DTO>>? getAllMealsAsync()
        {
            throw new NotImplementedException();
        }
    }
}