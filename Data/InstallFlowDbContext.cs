using InstallFlow.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstallFlow.Data
{
    public class InstallFlowDbContext : DbContext
    {
        public InstallFlowDbContext(DbContextOptions<InstallFlowDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Assignment> Assignments => Set<Assignment>();
        public DbSet<Job> Jobs => Set<Job>();
        public DbSet<JobLaborRow> JobLaborRows => Set<JobLaborRow>();
        public DbSet<JobMaterialRow> JobMaterialRows => Set<JobMaterialRow>();
        public DbSet<JobTemplate> JobTemplates => Set<JobTemplate>();
        public DbSet<JobTemplateLaborRow> JobTemplateLaborRows => Set<JobTemplateLaborRow>();
        public DbSet<JobTemplateMaterialRow> JobTemplateMaterialRows => Set<JobTemplateMaterialRow>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== Assignment → User (två relationer) =====
            // EF Core kan inte själv lista ut vilken FK som hör till vilken
            // navigation property, så vi måste vara explicita.

            modelBuilder.Entity<Assignment>(entity =>
            {
                // "Vem skapade uppdraget" — pekar på User.CreatedAssignments
                entity.HasOne(a => a.CreatedByUser)
                      .WithMany(u => u.CreatedAssignments)
                      .HasForeignKey(a => a.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // "Vem uppdaterade senast" — ingen collection tillbaka på User
                entity.HasOne(a => a.UpdatedByUser)
                      .WithMany()
                      .HasForeignKey(a => a.UpdatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                // "Vem skapade uppdraget" — pekar på User.CreatedAssignments
                entity.HasOne(a => a.CreatedByUser)
                      .WithMany(u => u.CreatedCustomers)
                      .HasForeignKey(a => a.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // "Vem uppdaterade senast" — ingen collection tillbaka på User
                entity.HasOne(a => a.UpdatedByUser)
                      .WithMany()
                      .HasForeignKey(a => a.UpdatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Product>(entity =>
            {
                // "Vem skapade uppdraget" — pekar på User.CreatedAssignments
                entity.HasOne(a => a.CreatedByUser)
                      .WithMany(u => u.CreatedProducts)
                      .HasForeignKey(a => a.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // "Vem uppdaterade senast" — ingen collection tillbaka på User
                entity.HasOne(a => a.UpdatedByUser)
                      .WithMany()
                      .HasForeignKey(a => a.UpdatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== Job → User (två relationer) =====
            // Samma mönster som ovan.

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasOne(j => j.CreatedByUser)
                      .WithMany(u => u.CreatedJobs)
                      .HasForeignKey(j => j.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(j => j.UpdatedByUser)
                      .WithMany()
                      .HasForeignKey(j => j.UpdatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== Decimal-precision =====
            // SQL Server kräver att du anger precision för decimal,
            // annars får du varningar vid migration.

            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(j => j.FixedCustomerPrice).HasPrecision(18, 2);
                entity.Property(j => j.FixedInternalCost).HasPrecision(18, 2);
                entity.Property(j => j.LaborMarkupValue).HasPrecision(18, 2);
                entity.Property(j => j.MaterialMarkupValue).HasPrecision(18, 2);
            });

            modelBuilder.Entity<JobLaborRow>(entity =>
            {
                entity.Property(r => r.Hours).HasPrecision(18, 2);
                entity.Property(r => r.HourlyRate).HasPrecision(18, 2);
            });

            modelBuilder.Entity<JobMaterialRow>(entity =>
            {
                entity.Property(r => r.Quantity).HasPrecision(18, 2);
                entity.Property(r => r.UnitPrice).HasPrecision(18, 2);
            });

            modelBuilder.Entity<JobTemplate>(entity =>
            {
                entity.Property(t => t.FixedCustomerPrice).HasPrecision(18, 2);
                entity.Property(t => t.FixedInternalCost).HasPrecision(18, 2);
                entity.Property(t => t.LaborMarkupValue).HasPrecision(18, 2);
                entity.Property(t => t.MaterialMarkupValue).HasPrecision(18, 2);
            });

            modelBuilder.Entity<JobTemplateLaborRow>(entity =>
            {
                entity.Property(r => r.DefaultHours).HasPrecision(18, 2);
                entity.Property(r => r.DefaultHourlyRate).HasPrecision(18, 2);
            });

            modelBuilder.Entity<JobTemplateMaterialRow>(entity =>
            {
                entity.Property(r => r.DefaultQuantity).HasPrecision(18, 2);
                entity.Property(r => r.DefaultUnitPrice).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Product>(entity =>
            {

                entity.Property(r => r.DefaultPrice).HasPrecision(18, 2);
            });
            modelBuilder.Entity<CartItem>(entity =>
            {

                entity.Property(r => r.UnitPriceSnapshot).HasPrecision(18, 2);
            });


        }
    }
}