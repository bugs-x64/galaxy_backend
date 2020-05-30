using System;
using GalaxyRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace GalaxyRepository
{
    public partial class GalaxyContext : DbContext
    {
        public GalaxyContext()
        {
        }

        public GalaxyContext(DbContextOptions<GalaxyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Password> Password { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("galaxyContext, отсутствует конфигурация для подклчюения.");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Password>(entity =>
            {
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("PasswordHash");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Password)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Password_user");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .HasName("index_user_username");

                entity.Property(e => e.Birthdate).HasColumnType("datetime");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modified)
                    .HasColumnName("modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);
                
                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("money");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
