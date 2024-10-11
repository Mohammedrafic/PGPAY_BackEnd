using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<HostelDetail> HostelDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=CIPL1318DBA\\MSSQLSERVER191;Database=PGPAY;Trusted_Connection=false;Encrypt=False;TrustServerCertificate=False;Connect Timeout=30;user id=sa;password=Colan123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HostelDetail>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_HostelDetails_UserId");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.CreateBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
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
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
