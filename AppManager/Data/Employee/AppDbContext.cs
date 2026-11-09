using System;
using System.IO;
using EmployeeManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployeeManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connStr = GetConnectionString();
                optionsBuilder.UseSqlServer(connStr);
            }
        }

        private static string GetConnectionString()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                var configuration = builder.Build();
                string? connStr = configuration.GetConnectionString("EmployeeConnection");
                if (!string.IsNullOrEmpty(connStr))
                {
                    return connStr;
                }
            }
            catch
            {
                // Fallback default
            }

            return "Server=.\\SQLEXPRESS;Database=EmployeeDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Department
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Departments");
                entity.HasKey(d => d.DepartmentId);
                entity.Property(d => d.DepartmentId).ValueGeneratedNever();
                entity.Property(d => d.DepartmentName).IsRequired().HasMaxLength(100);
            });

            // Configure Employee
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.EmployeeId).ValueGeneratedNever();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MiddleName).HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Gender).HasMaxLength(20);

                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed initial department data
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "IT" },
                new Department { DepartmentId = 2, DepartmentName = "HR" },
                new Department { DepartmentId = 3, DepartmentName = "Marketing" },
                new Department { DepartmentId = 4, DepartmentName = "Finance" },
                new Department { DepartmentId = 5, DepartmentName = "Sales" }
            );

            // Seed initial employee data
            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeId = 101, FirstName = "John", MiddleName = "William", LastName = "Smith", DepartmentId = 1, Gender = "Male" },
                new Employee { EmployeeId = 102, FirstName = "Sarah", MiddleName = "Jane", LastName = "Connor", DepartmentId = 2, Gender = "Female" },
                new Employee { EmployeeId = 103, FirstName = "Michael", MiddleName = "David", LastName = "Brown", DepartmentId = 1, Gender = "Male" },
                new Employee { EmployeeId = 104, FirstName = "Emma", MiddleName = "Marie", LastName = "Watson", DepartmentId = 3, Gender = "Female" },
                new Employee { EmployeeId = 105, FirstName = "James", MiddleName = "Robert", LastName = "Wilson", DepartmentId = 4, Gender = "Male" }
            );
        }
    }
}
