using System;
using System.Collections.Generic;
using System.Text;
using Auto_Parts_2019.Models.Parts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auto_Parts_2019.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Part> Parts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cros> Cross { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DefaultDiscount> DefaultDiscounts { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ApplicationDbContext() { }


    }
    
}
