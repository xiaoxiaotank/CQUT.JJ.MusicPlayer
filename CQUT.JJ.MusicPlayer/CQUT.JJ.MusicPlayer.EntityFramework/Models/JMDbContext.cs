using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    public partial class JMDbContext : DbContext
    {
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<User> User { get; set; }

        public JMDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Header)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.ParentId).HasDefaultValueSql("((0))");

                entity.Property(e => e.Priority).HasDefaultValueSql("((99))");

                entity.Property(e => e.RequiredAuthorizeCode)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.TargetUrl)
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.DeletionTime).HasColumnType("datetime");

                entity.Property(e => e.LastModificationTime).HasColumnType("datetime");

                entity.Property(e => e.NickName)
                  .IsRequired()
                  .HasMaxLength(8);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });
        }
    }
}
