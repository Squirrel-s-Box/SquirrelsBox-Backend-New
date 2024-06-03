using Microsoft.EntityFrameworkCore;
using Base.Generic.Extensions;
using SquirrelsBox.Authentication.Domain.Models;

namespace SquirrelsBox.Authentication.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<AssignedPermission> AssignedPermissions { get; set; }


        public DbSet<DeviceSession> DevicesSessions { get; set; }
        public DbSet<UserSession> UsersSessions { get; set; }
        public DbSet<AccessSession> AccessSessions { get; set; }


        public DbSet<UserData> UsersData { get; set; }


        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions");
                entity.Property(e => e.Collection).HasMaxLength(60);
                entity.Property(e => e.Name).HasMaxLength(60);
                entity.Property(e => e.Value).HasMaxLength(60);
                entity.HasKey(e => e.Id);
            });

            builder.Entity<AssignedPermission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
                entity.ToTable("assigned_permissions");
                entity.Property(e => e.UserCode).HasMaxLength(40);
                //entity.Property(e => e.ElementId);
                entity.Property(e => e.PermissionId);

                entity.HasOne(e => e.Permission)
                      .WithMany()
                      .HasForeignKey(e => e.PermissionId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            ///////////
            ///
            builder.Entity<AccessSession>(entity =>
            {
                entity.ToTable("access_sessions");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(p => p.Code).IsRequired().HasMaxLength(40);
                entity.Property(p => p.Attempt).HasDefaultValue(0);
                entity.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");
                entity.Property(p => p.LastUpdateDate);

                entity.HasMany(u => u.SessionsTokens) // Correct the property name here
                   .WithOne(st => st.User)
                   .HasForeignKey(st => st.UserId)
                   .OnDelete(DeleteBehavior.Restrict); // Adjust the deletion behavior as per your needs

                entity.HasMany(u => u.DevicesTokens) // Correct the property name here
                    .WithOne(dt => dt.User)
                    .HasForeignKey(dt => dt.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<UserSession>(entity =>
            {
                entity.ToTable("users_sessions");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(p => p.Token).HasMaxLength(255);
                entity.Property(p => p.OldToken).HasMaxLength(255);
                entity.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()").IsRequired();
                entity.Property(p => p.LastUpdateDate);
                entity.Property(p => p.UserId).IsRequired();
            });

            builder.Entity<DeviceSession>(entity =>
            {
                entity.ToTable("devices_sessions");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(p => p.Token).HasMaxLength(255);
                entity.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()").IsRequired();
                entity.Property(p => p.LastUpdateDate);
                entity.Property(p => p.UserId).IsRequired();
            });

            ///////////
            ///

            builder.Entity<UserData>(entity =>
            {
                entity.ToTable("users_data");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(p => p.Username).IsRequired();
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.Lastname).IsRequired();
                entity.Property(p => p.Email).IsRequired();
                entity.Property(p => p.UserPhoto).IsRequired();
                entity.Property(p => p.UserCode).IsRequired();

            });

            builder.UseSnakeCaseNamingConvention();
        }

    }
}
