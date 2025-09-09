using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheMealDBApp.DTOs;
using TheMealDBApp.Migrations;

namespace TheMealDBApp.Interface
{
    public interface ICartRepository
    {
        Task<Categories_Temp_DTO>? createMealAsync(Categories_Temp_DTO createMealDto);
        Task<List<Categories_Temp_DTO>>? getAllMealsAsync();
    }
}