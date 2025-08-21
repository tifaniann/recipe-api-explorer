using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheMealDBApp.DTOs;

namespace TheMealDBApp.Interface
{
    public interface ICartRepository
    {
        Task<CreateCtrMealDto>? createMealAsync(CreateCtrMealDto createMealDto);
    }
}