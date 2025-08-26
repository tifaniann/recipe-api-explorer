using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheMealDBApp.Models;

namespace TheMealDBApp.Interface
{
    public interface IUserRepository
    {
        Task<Users?> LoginUserAsync(Users user);
        Task<Users?> RegisterUserAsync(Users user);
    }
}