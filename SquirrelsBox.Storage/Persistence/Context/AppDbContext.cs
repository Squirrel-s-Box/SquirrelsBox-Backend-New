using Microsoft.EntityFrameworkCore;
using Base.Generic.Extensions;
using SquirrelsBox.Storage.Domain.Models;

namespace SquirrelsBox.Storage.Persistence.Context
{
    public partial class AppDbContext : DbContext
    {
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Spec> PersonalizedSpecs { get; set; }
        public DbSet<BoxSectionRelationship> BoxesSectionsList { get; set; }
        public DbSet<SectionItemRelationship> SectionsItemsList { get; set; }

        public DbSet<SharedBox> SharedBoxes { get; set; }
        public DbSet<SharedBoxPermission> SharedBoxPermissions { get; set; }

        public DbSet<Counter> Counter { get; set; }
        public DbSet<ActionLog> ActionLog { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SharedBox>(entity =>
            {
                entity.ToTable("shared_boxes");
                entity.HasKey(sb => sb.Id);
                entity.Property(sb => sb.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(sb => sb.BoxId).IsRequired();
                entity.Property(sb => sb.UserCodeGuest).IsRequired().HasMaxLength(40);
                entity.Property(sb => sb.CreationDate).HasDefaultValueSql("GETDATE()");
                entity.Property(sb => sb.LastUpdateDate);
                entity.Property(sb => sb.State).IsRequired();

                entity.HasOne(sb => sb.Box)
                    .WithMany()
                    .HasForeignKey(sb => sb.BoxId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(sb => sb.SharedBoxPermissions)
                    .WithOne(sbp => sbp.SharedBox)
                    .HasForeignKey(sbp => sbp.SharedBoxId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<SharedBoxPermission>(entity =>
            {
                entity.ToTable("shared_box_permissions");
                entity.HasKey(sbp => sbp.Id);
                entity.Property(sbp => sbp.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(sbp => sbp.SharedBoxId).IsRequired();
                entity.Property(sbp => sbp.PermissionId).IsRequired().HasMaxLength(60);
                entity.Property(sbp => sbp.IsAllowed).IsRequired();

                entity.HasOne(sbp => sbp.SharedBox)
                    .WithMany(sb => sb.SharedBoxPermissions)
                    .HasForeignKey(sbp => sbp.SharedBoxId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Box>(entity =>
            {
                entity.ToTable("boxes");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(p => p.Name).HasMaxLength(60);
                entity.Property(p => p.UserCodeOwner).IsRequired().HasMaxLength(40);
                entity.Property(p => p.Favourite).HasDefaultValue(false);
                entity.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");
                entity.Property(p => p.LastUpdateDate);
                entity.Property(p => p.Active).HasDefaultValue(true);

                entity.HasMany(b => b.BoxSectionsList)
                    .WithOne(bsl => bsl.Box)
                    .HasForeignKey(bsl => bsl.BoxId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(b => b.SharedBoxes)
                    .WithOne(sb => sb.Box)
                    .HasForeignKey(sb => sb.BoxId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Section>(entity =>
            {
                entity.ToTable("sections");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(p => p.Name).HasMaxLength(60);
                entity.Property(p => p.Color).HasMaxLength(60);
                entity.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");
                entity.Property(p => p.LastUpdateDate);
                entity.Property(p => p.Active).HasDefaultValue(true);

                entity.HasMany(s => s.BoxSectionsList)
                    .WithOne(bsl => bsl.Section)
                    .HasForeignKey(bsl => bsl.SectionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.SectionItemsList)
                    .WithOne(sil => sil.Section)
                    .HasForeignKey(sil => sil.SectionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Item>(entity =>
            {
                entity.ToTable("items");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(p => p.Name).HasMaxLength(60);
                entity.Property(p => p.Description).HasMaxLength(60);
                entity.Property(p => p.Amount).HasMaxLength(60);
                entity.Property(p => p.ItemPhoto).HasMaxLength(60);
                entity.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");
                entity.Property(p => p.LastUpdateDate);
                entity.Property(p => p.Active).HasDefaultValue(true);

                entity.HasMany(i => i.SectionItemsList)
                    .WithOne(sil => sil.Item)
                    .HasForeignKey(sil => sil.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(i => i.Specs)
                    .WithOne(ps => ps.Item)
                    .HasForeignKey(ps => ps.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Spec>(entity =>
            {
                entity.ToTable("personalized_specs");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(p => p.HeaderName).HasMaxLength(60);
                entity.Property(p => p.Value).HasMaxLength(60);
                entity.Property(p => p.ValueType).HasMaxLength(60);
                entity.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");
                entity.Property(p => p.LastUpdateDate);
                entity.Property(p => p.Active).HasDefaultValue(true);
            });

            builder.Entity<BoxSectionRelationship>()
                .HasKey(bsl => new { bsl.Id });

            builder.Entity<BoxSectionRelationship>()
                .HasOne(bsl => bsl.Box)
                .WithMany(b => b.BoxSectionsList)
                .HasForeignKey(bsl => bsl.BoxId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BoxSectionRelationship>()
                .HasOne(bsl => bsl.Section)
                .WithMany(s => s.BoxSectionsList)
                .HasForeignKey(bsl => bsl.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SectionItemRelationship>()
                .HasKey(sil => new { sil.Id });

            builder.Entity<SectionItemRelationship>()
                .HasOne(sil => sil.Section)
                .WithMany(s => s.SectionItemsList)
                .HasForeignKey(sil => sil.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SectionItemRelationship>()
                .HasOne(sil => sil.Item)
                .WithMany(i => i.SectionItemsList)
                .HasForeignKey(sil => sil.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.UseSnakeCaseNamingConvention();
        }
    }
}
