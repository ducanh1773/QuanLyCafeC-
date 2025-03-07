using Microsoft.EntityFrameworkCore;

namespace QuanLyCafe.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<ProductCoffee> ProductCoffee { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<TableCoffe> Tables { get; set; }

        public DbSet<Supply> Supplies { get; set; }

        public DbSet<PaymentForm> paymentForms { get; set; }

        public DbSet<DetailSupplyStock> detailSupplyStocks { get; set; }

        public DbSet<Fund> funds { get; set; }

        public DbSet<DetailTableOrder> detailTableOrders { get; set; }

        public DbSet<OrderCoffe> orderCoffes { get; set; }

        public DbSet<OrderDetailProduct> orderDetailProducts { get; set; }

        public DbSet<DeatailStockProduct> deatailStockProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>().ToTable("Account"); // Đảm bảo trùng tên bảng
            modelBuilder.Entity<ProductCoffee>().ToTable("ProductCoffee");
            modelBuilder.Entity<Stock>().ToTable("Stock");
            modelBuilder.Entity<TableCoffe>().ToTable("TableCoffee");
            modelBuilder.Entity<Supply>().ToTable("Supply");
            modelBuilder.Entity<PaymentForm>().ToTable("Payment_Form");
            modelBuilder.Entity<DetailSupplyStock>().ToTable("DetailSupplyStock");
            modelBuilder.Entity<DetailTableOrder>().ToTable("DetailTableOrder");
            modelBuilder.Entity<OrderCoffe>().ToTable("OrderCoffe");
            modelBuilder.Entity<OrderDetailProduct>().ToTable("OrderDeatilProduct");
            modelBuilder.Entity<DeatailStockProduct>().ToTable("DeatailStockProduct");
            modelBuilder.Entity<Fund>().ToTable("Fund");





            // modelBuilder.Entity<DetailSupplyStock>(entity =>
            //  {
            // entity.HasKey(e => e.Id);
            // entity.Property(e => e.ID_Supply).HasColumnName("Id_Supply"); // Đảm bảo tên cột đúng
            // entity.ToTable("DetailSupplyStock");
            //  });

            // modelBuilder.Entity<PaymentForm>(entity =>
            // {
            //     entity.HasKey(e => e.Id);
            //     entity.Property(e => e.ID_Supply).HasColumnName("Id_Supply"); // Đảm bảo tên cột đúng
            //     entity.ToTable("Payment_Form");
            // });

        }
    }

}