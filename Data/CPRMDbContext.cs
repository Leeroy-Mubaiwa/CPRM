using System;
using System.Collections.Generic;
using CPRM.Models;
using Microsoft.EntityFrameworkCore;

namespace CPRM.Data;

public partial class CPRMDbContext : DbContext
{
    public CPRMDbContext()
    {
    }

    public CPRMDbContext(DbContextOptions<CPRMDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<LoginLog> LoginLogs { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=cprmContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.HasOne(d => d.Partner).WithMany(p => p.AspNetUsers).HasConstraintName("FK_AspnetUsers_Partners");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__chats__FD040B17F350A505");

            entity.Property(e => e.SentAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Receiver).WithMany(p => p.ChatReceivers).HasConstraintName("FK__chats__receiver___5535A963");

            entity.HasOne(d => d.Sender).WithMany(p => p.ChatSenders).HasConstraintName("FK__chats__sender_id__5441852A");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__document__9666E8ACA84D41C7");

            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Partner).WithMany(p => p.Documents).HasConstraintName("FK__documents__partn__628FA481");
        });

        modelBuilder.Entity<LoginLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__login_lo__9E2397E0F8C2E3EF");

            entity.Property(e => e.LoginTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.LoginLogs).HasConstraintName("FK__login_log__user___66603565");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders__46596229C7C94BB6");

            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("pending");

            entity.HasOne(d => d.Partner).WithMany(p => p.Orders).HasConstraintName("FK__orders__partner___6B24EA82");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__order_it__3764B6BC32B7564E");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("FK__order_ite__order__6E01572D");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems).HasConstraintName("FK__order_ite__produ__6EF57B66");
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.PartnerId).HasName("PK__partners__576F1B270C684491");

            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__products__47027DF5D0B3D104");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__sales__E1EB00B22B9954FB");

            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.SaleDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Partner).WithMany(p => p.Sales).HasConstraintName("FK__sales__partner_i__5DCAEF64");

            entity.HasOne(d => d.Product).WithMany(p => p.Sales).HasConstraintName("FK__sales__product_i__5EBF139D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
