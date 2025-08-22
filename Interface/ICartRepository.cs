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
        Task<Categories_Temp>? createMealAsync(Categories_Temp createMealDto);
    }
}