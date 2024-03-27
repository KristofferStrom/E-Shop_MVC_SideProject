using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Shop_MVC.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }
        public DbSet<SubProductCategory> SubCategories { get; set; }
        public DbSet<ProductColor> Colors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductReview> Reviews { get; set; }
    }
}
