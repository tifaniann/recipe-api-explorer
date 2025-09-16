using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheMealDBApp.DTOs;
// using TheMealDBApp.Migrations;
using TheMealDBApp.Models;

namespace TheMealDBApp.Interface
{
    public interface ICartRepository
    {
        Task<Categories_Temp>? createMealAsync(Categories_Temp createMealDto);
        Task<List<Categories_Temp>>? getAllMealsAsync();
        Task<List<Categories_Temp>>? GetCartAsync(int idCust);
        Task<Categories_Temp>? addToCartAsync(Categories_Temp itemAdd);
        Task<Categories_Temp>? UpdateQtyAsync(int idCust, int idCategory, int qty);
        Task<Categories_Temp>? ClearCarttAsync(int idCust);
    }
}