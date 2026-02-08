using HomeChefss.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HomeChefss.Data
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}

		public DbSet<Chef> Chefs
		{ get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<FoodItem> FoodItems { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<Delivery> Deliveries { get; set; }
		public DbSet<DeliveryPartner> DeliveryPartners { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//  UNIQUE EMAIL CONSTRAINT
			modelBuilder.Entity<ApplicationUser>()
				.HasIndex(u => u.NormalizedEmail)
				.IsUnique();

			//ApplicationUser <-> Chef (One-to-One)
			modelBuilder.Entity<ApplicationUser>()
				.HasOne(u => u.Chef)
				.WithOne(c => c.User)
				.HasForeignKey<Chef>(c => c.UserId);

			modelBuilder.Entity<Chef>()
	             .HasIndex(c => c.UserId)
	             .IsUnique();

			modelBuilder.Entity<Chef>()
	             .Property(c => c.VerificationStatus)
	             .HasConversion<string>();



			//Chef <-> FoodItem (One-to-Many)
			modelBuilder.Entity<FoodItem>()
				.HasOne(f => f.Chef)
				.WithMany(c => c.FoodItems)
				.HasForeignKey(f => f.ChefId);

			// Category ↔ FoodItem (One-to-Many)
			modelBuilder.Entity<FoodItem>()
				.HasOne(f => f.Category)
				.WithMany(c => c.FoodItems)
				.HasForeignKey(f => f.CategoryId);

			// Order ↔ OrderItem (One-to-Many)
			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.Order)
				.WithMany(o => o.Items)
				.HasForeignKey(oi => oi.OrderId);

			// FoodItem ↔ OrderItem (One-to-Many)
			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.FoodItem)
				.WithMany()
				.HasForeignKey(oi => oi.FoodItemId);

			// Enum stored as string (Recommended)
			modelBuilder.Entity<Order>()
				.Property(o => o.Status)
				.HasConversion<string>();

			modelBuilder.Entity<Order>()
		        .OwnsOne(o => o.DeliveryLocation);

			modelBuilder.Entity<Chef>()
		        .OwnsOne(c => c.KitchenLocation);

			// Review ↔ Chef
			modelBuilder.Entity<Review>()
				.HasOne(r => r.Chef)
				.WithMany()
				.HasForeignKey(r => r.ChefId);

			// Review ↔ User
			modelBuilder.Entity<Review>()
				.HasOne(r => r.User)
				.WithMany()
				.HasForeignKey(r => r.UserId);

			// Review ↔ Order (One-to-One)
			modelBuilder.Entity<Review>()
				.HasOne(r => r.Order)
				.WithOne()
				.HasForeignKey<Review>(r => r.OrderId);

		}

	}

	
}
