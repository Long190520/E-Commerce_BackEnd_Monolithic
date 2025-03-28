using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_BackEnd.Models
{
    public class MyDbContext : IdentityDbContext<Users, IdentityRole<Guid>, Guid>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        #region DbSet
        public DbSet<Categories> Categories { get; set; }
        public DbSet<ProductReviews> ProductReviews { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductVariants> ProductVariants { get; set; }
        public DbSet<Carts> Carts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carts>(entity =>
            {
                entity.ToTable(nameof(Carts));

                entity.HasKey(c => c.UserId);

                entity.HasOne(c => c.User)
                      .WithOne(u => u.Cart)
                      .HasForeignKey<Carts>(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.CartItems)
                      .WithOne(ci => ci.Cart)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItems>(entity =>
            {
                entity.ToTable(nameof(CartItems));

                entity.HasKey(ci => ci.Id);

                entity.HasOne(ci => ci.Cart)
                      .WithMany(c => c.CartItems)
                      .HasForeignKey(ci => ci.CartId);

                entity.HasOne(ci => ci.ProductVariant)
                      .WithMany(pv => pv.CartItems)
                      .HasForeignKey(ci => ci.ProductVariantId);
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable(nameof(Categories));

                entity.HasKey(e => e.Id);

                entity.HasMany(c => c.Products)
                      .WithOne(p => p.Category)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable(nameof(Users));

                entity.HasKey(e => e.Id);

                entity.HasOne(u => u.Cart)
                      .WithOne(c => c.User)
                      .HasForeignKey<Carts>(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable(nameof(Orders));

                entity.HasKey(p => p.Id);

                entity.HasMany(o => o.OrderDetails)
                       .WithOne(od => od.Order)
                       .HasForeignKey(od => od.OrderId)
                       .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(o => o.User)
                       .WithMany(u => u.Orders)
                       .HasForeignKey(o => o.UserId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.ToTable(nameof(OrderDetails));

                entity.HasKey(p => p.Id);

                entity.HasOne(od => od.ProductVariant)
                      .WithMany(pv => pv.OrderDetails)
                      .HasForeignKey(od => od.ProductVariantId);

                entity.HasOne(od => od.Order)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(od => od.OrderId);
            });

            modelBuilder.Entity<ProductReviews>(entity =>
            {
                entity.ToTable(nameof(ProductReviews));

                entity.HasKey(p => p.Id);

                modelBuilder.Entity<ProductReviews>()
                      .ToTable(t => t.HasCheckConstraint("CK_ProductReviews_Rating", "[Rating] BETWEEN 0 AND 5"));

                entity.HasOne(pr => pr.User)
                      .WithMany(u => u.ProductReviews)
                      .HasForeignKey(pr => pr.UserId)
                      .OnDelete(DeleteBehavior.Cascade); // Keep this

                entity.HasOne(pr => pr.Product)
                      .WithMany(p => p.ProductReviews)
                      .HasForeignKey(pr => pr.ProductId)
                      .OnDelete(DeleteBehavior.NoAction); // Change to NoAction

                entity.HasOne(pr => pr.ProductVariant)
                      .WithMany(pv => pv.ProductReviews)
                      .HasForeignKey(pr => pr.ProductVariantsId)
                      .OnDelete(DeleteBehavior.NoAction); // Change to NoAction
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.ToTable(nameof(Products));

                entity.HasKey(p => p.Id);

                entity.HasMany(pv => pv.ProductVariants)
                       .WithOne(p => p.Product)
                       .HasForeignKey(p => p.ProductId)
                       .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Category)
                       .WithMany(p => p.Products)
                       .HasForeignKey(p => p.CategoryId);
            });

            modelBuilder.Entity<ProductVariants>(entity =>
            {
                entity.ToTable(nameof(ProductVariants));

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Price)
                      .HasPrecision(18, 2);

                entity.HasOne(pv => pv.Product)
                      .WithMany(p => p.ProductVariants)
                      .HasForeignKey(pv => pv.ProductId);

                entity.HasMany(pr => pr.ProductReviews)
                      .WithOne(pv => pv.ProductVariant)
                      .HasForeignKey(pr => pr.ProductVariantsId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(ci => ci.CartItems)
                        .WithOne(pv => pv.ProductVariant)
                        .HasForeignKey(ci => ci.ProductVariantId);

                entity.HasMany(od => od.OrderDetails)
                        .WithOne(pv => pv.ProductVariant)
                        .HasForeignKey(od => od.ProductVariantId);

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
