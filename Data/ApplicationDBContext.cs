using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TheMealDBApp.Models;
using TheMealDBApp.DTOs;

namespace TheMealDBApp.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext() { }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        
        public DbSet<CreateCtrMealDto> Categories_Temp { get; set; }
    }
}