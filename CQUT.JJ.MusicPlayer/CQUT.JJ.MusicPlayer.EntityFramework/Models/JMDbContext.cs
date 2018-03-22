using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Models
{
    /// <summary>
    /// Scaffold-DbContext "data source=.;Initial catalog=JJMusicManage;User Id=sa;Password=19950913;" Microsoft.EntityFrameworkCore.SqlServer -outputdir Models
    /// </summary>
    public partial class JMDbContext : DbContext
    {
        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Music> Music { get; set; }
        public virtual DbSet<MusicAttach> MusicAttach { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Singer> Singer { get; set; }
        public virtual DbSet<SingerAttach> SingerAttach { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        public JMDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.PublishmentTime).HasColumnType("datetime");

                entity.Property(e => e.DeletionTime).HasColumnType("datetime");

                entity.Property(e => e.LastModificationTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.HasOne(d => d.Singer)
                    .WithMany(p => p.Album)
                    .HasForeignKey(d => d.SingerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ALBUM_REFERENCE_SINGER");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.Header)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.LastModificationTime).HasColumnType("datetime");

                entity.Property(e => e.ParentId).HasDefaultValueSql("((0))");

                entity.Property(e => e.RequiredAuthorizeCode)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.TargetUrl)
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Music>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.PublishmentTime).HasColumnType("datetime");

                entity.Property(e => e.DeletionTime).HasColumnType("datetime");

                entity.Property(e => e.FileUrl)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.LastModificationTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Music)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MUSIC_REFERENCE_ALBUM");

                entity.HasOne(d => d.Singer)
                    .WithMany(p => p.Music)
                    .HasForeignKey(d => d.SingerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MUSIC_REFERENCE_SINGER");
            });

            modelBuilder.Entity<MusicAttach>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CoverUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Passion).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Music)
                    .WithMany(p => p.MusicAttach)
                    .HasForeignKey(d => d.MusicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MUSICATT_REFERENCE_MUSIC");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Permission)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_PERMISSI_FK_PERMIS_ROLE");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Permission)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PERMISSI_FK_PERMIS_USER");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.DeletionTime).HasColumnType("datetime");

                entity.Property(e => e.LastModificationTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(8);
            });

            modelBuilder.Entity<Singer>(entity =>
            {
                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.PublishmentTime).HasColumnType("datetime");

                entity.Property(e => e.DeletionTime).HasColumnType("datetime");

                entity.Property(e => e.ForeignName)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.LastModificationTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Nationality)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<SingerAttach>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FansNumber).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Singer)
                    .WithMany(p => p.SingerAttach)
                    .HasForeignKey(d => d.SingerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SINGERAT_REFERENCE_SINGER");
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

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERROLE_FK_USERRO_ROLE");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERROLE_FK_USERRO_USER");
            });
        }
    }
}
