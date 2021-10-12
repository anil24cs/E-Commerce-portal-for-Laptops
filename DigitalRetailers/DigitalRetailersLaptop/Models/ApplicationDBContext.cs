using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DigitalRetailersLaptop.Models
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=H5CG125CX1Q;Database=DigitalRetailersLaptop;Integrated Security=true;");
        }
    }
}
