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

            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.RestockDetails)
                .WithOne(r => r.Warehouse)
                .HasForeignKey<WarehouseRestock>(r => r.WarehouseId);

            modelBuilder.Entity<Shelf>()
                .HasOne(s => s.RestockDetails)
                .WithOne(r => r.Shelf)
                .HasForeignKey<ShelfRestock>(r => r.ShelfId);
        }
    }
}
