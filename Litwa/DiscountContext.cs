using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Litwa
{
    internal class DiscountContext : DbContext
    {
        public DbSet<Discount> Discounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=MACIEJ\\SQLEXPRESS;Database=myDataBase;Trusted_Connection=True;TrustServerCertificate=true;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Discount>()
                .HasIndex(x => x.DiscountCode)
                .IsUnique();
        }
    }
}
