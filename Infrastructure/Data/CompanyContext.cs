using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace Infrastructure.Data
{
    public class CompanyContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This ensures the Identity schema is included

            // Define relationships
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Address)
                .WithOne(a => a.Employee)
                .HasForeignKey<Employee>(e => e.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Account)
                .WithOne(a => a.Employee)
                .HasForeignKey<Employee>(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
