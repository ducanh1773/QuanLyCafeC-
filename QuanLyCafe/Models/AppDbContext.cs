using Microsoft.EntityFrameworkCore;

namespace QuanLyCafe.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<ProductCoffee> ProductCoffee { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<Supply> Supplies { get; set; }

        public DbSet<PaymentForm> paymentForms { get; set; }

        public DbSet<DetailSupplyStock> detailSupplyStocks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Account"); // Đảm bảo trùng tên bảng
            modelBuilder.Entity<ProductCoffee>().ToTable("ProductCoffee");
            modelBuilder.Entity<Stock>().ToTable("Stock");
            modelBuilder.Entity<Table>().ToTable("TableCoffee");
            modelBuilder.Entity<Supply>().ToTable("Supply");
            // modelBuilder.Entity<PaymentForm>().ToTable("Payment_Form");
            // modelBuilder.Entity<DetailSupplyStock>().ToTable("DetailSupplyStock");




            modelBuilder.Entity<DetailSupplyStock>(entity =>
             {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ID_Supply).HasColumnName("Id_Supply"); // Đảm bảo tên cột đúng
            entity.ToTable("DetailSupplyStock");
             });

            modelBuilder.Entity<PaymentForm>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ID_Supply).HasColumnName("Id_Supply"); // Đảm bảo tên cột đúng
                entity.ToTable("Payment_Form");
            });

        }
    }

}