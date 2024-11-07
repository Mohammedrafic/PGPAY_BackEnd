using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PGPAY_DL.Models.DB;

namespace PGPAY_DL.Context;

public partial class PGPAYContext : DbContext
{
    public PGPAYContext()
    {
    }

    public PGPAYContext(DbContextOptions<PGPAYContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<HostelDetail> HostelDetails { get; set; }

    public virtual DbSet<HostelFacility> HostelFacilities { get; set; }

    public virtual DbSet<HostelPhoto> HostelPhotos { get; set; }

    public virtual DbSet<HostelRequest> HostelRequests { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<SubMenuItem> SubMenuItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=CIPL1318DBA\\MSSQLSERVER191;Database=PGPAY;Trusted_Connection=false;Encrypt=False;TrustServerCertificate=False;Connect Timeout=30;user id=sa;password=Colan123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC07A9A1B173");

            entity.ToTable("Discount");

            entity.Property(e => e.DiscountCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Offerper)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("offerper");

            entity.HasOne(d => d.Hostel).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.HostelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostelId_Discount");
        });

        modelBuilder.Entity<HostelDetail>(entity =>
        {
            entity.HasKey(e => e.HostelId).HasName("PK__HostelDe__677EEB28FF0C3D4B");

            entity.Property(e => e.CreateBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DiscountCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.HostalAddress).IsUnicode(false);
            entity.Property(e => e.HostalPhotosPath).IsUnicode(false);
            entity.Property(e => e.HostelName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MaximunRent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MinimumRent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.OwnerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Rent).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.HostelDetails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostelDetails_UserId");
        });

        modelBuilder.Entity<HostelFacility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HostelFa__3214EC071C7A0EBE");

            entity.ToTable("HostelFacility");

            entity.Property(e => e.Imgpath)
                .IsUnicode(false)
                .HasColumnName("imgpath");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HostelPhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HostelPh__3214EC07D48ABDB0");

            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Imgpath)
                .IsUnicode(false)
                .HasColumnName("imgpath");
            entity.Property(e => e.PhotosId).IsUnicode(false);

            entity.HasOne(d => d.Hostel).WithMany(p => p.HostelPhotos)
                .HasForeignKey(d => d.HostelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostelId_HostelPhotos");
        });

        modelBuilder.Entity<HostelRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__HostelRe__33A8519ADE6B79AF");

            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.AssignedTo).HasMaxLength(100);
            entity.Property(e => e.ContactDetails).HasMaxLength(100);
            entity.Property(e => e.HostelId).HasColumnName("HostelID");
            entity.Property(e => e.IsResolved).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RequestType).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Pending')");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Hostel).WithMany(p => p.HostelRequests)
                .HasForeignKey(d => d.HostelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostelRequests_HostelId");

            entity.HasOne(d => d.User).WithMany(p => p.HostelRequests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostelRequests_UserId");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuItem__3214EC075C91FCE7");

            entity.Property(e => e.AdminPath).HasMaxLength(255);
            entity.Property(e => e.ImgPath).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UserPath).IsUnicode(false);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A5866BFD4C3");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.AdvanceAmount)
                .HasDefaultValueSql("((0))")
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HostelId).HasColumnName("HostelID");
            entity.Property(e => e.IsAdvancePayment).HasDefaultValueSql("((0))");
            entity.Property(e => e.PaymentDate).HasColumnType("date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('Pending')");
            entity.Property(e => e.RemainingBalance)
                .HasDefaultValueSql("((0))")
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Remarks).HasMaxLength(255);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TransactionID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Hostel).WithMany(p => p.Payments)
                .HasForeignKey(d => d.HostelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_HostelId");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_UserId");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rating__3214EC0759A38B02");

            entity.ToTable("Rating");

            entity.Property(e => e.Starrate)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("starrate");
            entity.Property(e => e.Status)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Hostel).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.HostelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostelId_Rating");
        });

        modelBuilder.Entity<SubMenuItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubMenuI__3214EC073C0BFD22");

            entity.Property(e => e.AdminPath).HasMaxLength(255);
            entity.Property(e => e.ImgPath).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UserPath).IsUnicode(false);

            entity.HasOne(d => d.MenuItem).WithMany(p => p.SubMenuItems)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SubMenuIt__MenuI__02FC7413");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Users_UserId");

            entity.Property(e => e.CreateBy).HasMaxLength(200);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.UpdateBy).HasMaxLength(200);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(200);
            entity.Property(e => e.UserRole).HasMaxLength(100);

            entity.HasOne(d => d.Hostel).WithMany(p => p.Users)
                .HasForeignKey(d => d.HostelId)
                .HasConstraintName("FK_HostelId_Users");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_userdetails_UserId");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ProofPath).IsUnicode(false);
            entity.Property(e => e.Sex)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.UserDetail)
                .HasForeignKey<UserDetail>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserDetails_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
