using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data
{
    public partial class AsiBasecodeDbContext : DbContext
    {
        public AsiBasecodeDbContext()
        {
        }

        public AsiBasecodeDbContext(DbContextOptions<AsiBasecodeDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<MRole> MRoles { get; set; }
        public virtual DbSet<MUser> MUsers { get; set; }
        public virtual DbSet<PendingBooking> PendingBookings { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Addr=(LocalDB)\\MSSQLLocalDB;database=AsiBasecodeDb;Integrated Security=False;Trusted_Connection=True");
            }
        }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.Frequency).HasMaxLength(50);
                entity.Property(e => e.IsRecurring).HasDefaultValueSql("((0))");
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bookings_Rooms");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bookings_M_User");
            });

            modelBuilder.Entity<MUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("M_User");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FirstNameKana).HasMaxLength(50);

                entity.Property(e => e.InsBy)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.InsDt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.LastNameKana).HasMaxLength(50);

                entity.Property(e => e.Mail)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks).HasMaxLength(500);

                entity.Property(e => e.TemporaryPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdBy)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PendingBooking>(entity =>
            {
                entity.Property(e => e.ApprovalStatus).HasMaxLength(50);
                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.PendingBookings)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PendingBookings_Bookings");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.Image).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Style).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
