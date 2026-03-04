using Microsoft.EntityFrameworkCore;
using StoreManagementSystem.API.Models;

namespace StoreManagementSystem.API.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseRestock> WarehouseRestocks { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<ShelfRestock> ShelfRestocks { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Rejection> Rejections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships if needed
            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.RestockDetails)
                .WithOne(r => r.Warehouse)
                .HasForeignKey<WarehouseRestock>(r => r.WarehouseId);

            modelBuilder.Entity<Shelf>()
                .HasOne(s => s.RestockDetails)
                .WithOne(r => r.Shelf)
                .HasForeignKey<ShelfRestock>(r => r.ShelfId);

            // Seed Data
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = "Admin" },
                new Role { RoleId = 2, Name = "Shelf Manager" },
                new Role { RoleId = 3, Name = "Warehouse Manager" },
                new Role { RoleId = 4, Name = "Sales Manager" }
            );

            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = 1, Name = "Active" },
                new Status { StatusId = 2, Name = "Inactive" }
            );

            modelBuilder.Entity<ActionType>().HasData(
                new ActionType { ActionId = 1, Name = "Restock" },
                new ActionType { ActionId = 2, Name = "MoveToShelf" },
                new ActionType { ActionId = 3, Name = "Sell" },
                new ActionType { ActionId = 4, Name = "Reject" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Fruit" },
                new Category { CategoryId = 2, Name = "Vegetable" },
                new Category { CategoryId = 3, Name = "Meat" },
                new Category { CategoryId = 4, Name = "Dairy" }
            );

            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { SupplierId = 1, Name = "BrandA" },
                new Supplier { SupplierId = 2, Name = "BrandB" }
            );
        }
    }
}
