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
        public virtual DbSet<UserLike> UserLike { get; set; }
        public virtual DbSet<UserMusicList> UserMusicList { get; set; }
        public virtual DbSet<UserMusicListMusic> UserMusicListMusic { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        public JMDbContext(DbContextOptions options) : base(options) { }

        public JMDbContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data source=.;Initial Catalog=JJMusicManage;Trusted_Connection=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.DeletionTime).HasColumnType("datetime");

                entity.Property(e => e.LastModificationTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.PublishmentTime).HasColumnType("datetime");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.AlbumCreator)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ALBUM_REFERENCE_USER4");

                entity.HasOne(d => d.Mender)
                    .WithMany(p => p.AlbumMender)
                    .HasForeignKey(d => d.MenderId)
                    .HasConstraintName("FK_ALBUM_REFERENCE_USER2");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.AlbumPublisher)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK_ALBUM_REFERENCE_USER1");

                entity.HasOne(d => d.Singer)
                    .WithMany(p => p.Album)
                    .HasForeignKey(d => d.SingerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ALBUM_REFERENCE_SINGER");

                entity.HasOne(d => d.Unpublisher)
                    .WithMany(p => p.AlbumUnpublisher)
                    .HasForeignKey(d => d.UnpublisherId)
                    .HasConstraintName("FK_ALBUM_REFERENCE_USER3");
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
                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.DeletionTime).HasColumnType("datetime");

                entity.Property(e => e.FileUrl)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.LastModificationTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.PublishmentTime).HasColumnType("datetime");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Music)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MUSIC_REFERENCE_ALBUM");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.MusicCreator)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MUSIC_REFERENCE_USER2");

                entity.HasOne(d => d.Mender)
                    .WithMany(p => p.MusicMender)
                    .HasForeignKey(d => d.MenderId)
                    .HasConstraintName("FK_MUSIC_REFERENCE_USER3");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.MusicPublisher)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK_MUSIC_REFERENCE_USER4");

                entity.HasOne(d => d.Singer)
                    .WithMany(p => p.Music)
                    .HasForeignKey(d => d.SingerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MUSIC_REFERENCE_SINGER");

                entity.HasOne(d => d.Unpublisher)
                    .WithMany(p => p.MusicUnpublisher)
                    .HasForeignKey(d => d.UnpublisherId)
                    .HasConstraintName("FK_MUSIC_REFERENCE_USER1");
            });

            modelBuilder.Entity<MusicAttach>(entity =>
            {
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

                entity.Property(e => e.PublishmentTime).HasColumnType("datetime");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.SingerCreator)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SINGER_REFERENCE_USER2");

                entity.HasOne(d => d.Mender)
                    .WithMany(p => p.SingerMender)
                    .HasForeignKey(d => d.MenderId)
                    .HasConstraintName("FK_SINGER_REFERENCE_USER1");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.SingerPublisher)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK_SINGER_REFERENCE_USER4");

                entity.HasOne(d => d.Unpublisher)
                    .WithMany(p => p.SingerUnpublisher)
                    .HasForeignKey(d => d.UnpublisherId)
                    .HasConstraintName("FK_SINGER_REFERENCE_USER3");
            });

            modelBuilder.Entity<SingerAttach>(entity =>
            {
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

            modelBuilder.Entity<UserLike>(entity =>
            {
                entity.HasOne(d => d.Album)
                    .WithMany(p => p.UserLike)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("FK_USERLIKE_REFERENCE_ALBUM");

                entity.HasOne(d => d.Music)
                    .WithMany(p => p.UserLike)
                    .HasForeignKey(d => d.MusicId)
                    .HasConstraintName("FK_USERLIKE_REFERENCE_MUSIC");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLike)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERLIKE_REFERENCE_USER");
            });

            modelBuilder.Entity<UserMusicList>(entity =>
            {
                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.DeletionTime).HasColumnType("datetime");

                entity.Property(e => e.LastModificationTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserMusicList)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERMUSI_REFERENCE_USER");
            });

            modelBuilder.Entity<UserMusicListMusic>(entity =>
            {
                entity.HasKey(e => new { e.MusicListId, e.MusicId });

                entity.HasOne(d => d.Music)
                    .WithMany(p => p.UserMusicListMusic)
                    .HasForeignKey(d => d.MusicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERMUSI_REFERENCE_MUSIC");

                entity.HasOne(d => d.MusicList)
                    .WithMany(p => p.UserMusicListMusic)
                    .HasForeignKey(d => d.MusicListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERMUSI_REFERENCE_USERMUSI");
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
