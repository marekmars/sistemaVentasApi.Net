using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Web_Service_.Net_Core.Models;

public partial class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Concept> Concepts { get; set; }


    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Image> Images { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clients");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.IdCard)
                .HasMaxLength(50)
                .HasColumnName("id_card");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Mail)
                .HasMaxLength(50)
                .HasColumnName("mail");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.State).HasColumnName("state");
        });

        modelBuilder.Entity<Concept>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("concepts");

            entity.HasIndex(e => e.IdProduct, "fk_producto");

            entity.HasIndex(e => e.IdSale, "fk_venta");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.IdProduct)
                .HasColumnType("bigint(20)")
                .HasColumnName("id_product");
            entity.Property(e => e.IdSale)
                .HasColumnType("bigint(20)")
                .HasColumnName("id_sale");
            entity.Property(e => e.Import)
                .HasPrecision(16, 2)
                .HasColumnName("import");
            entity.Property(e => e.Quantity)
                .HasColumnType("int(11)")
                .HasColumnName("quantity");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.UnitaryPrice)
                .HasPrecision(16, 2)
                .HasColumnName("unitary_price");

            entity.HasOne(d => d.Sale).WithMany(p => p.Concepts)
                .HasForeignKey(d => d.IdSale)
                .HasConstraintName("fk_venta");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("products");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Cost)
                .HasPrecision(16, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.Stock)
                .HasColumnType("int(11)")
                .HasColumnName("stock");
            entity.Property(e => e.UnitaryPrice)
                .HasPrecision(16, 2)
                .HasColumnName("unitary_price");
            entity.HasMany(p => p.Images)
        .WithOne(i => i.Product)
        .HasForeignKey(i => i.IdProduct)
        .IsRequired(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sales");

            entity.HasIndex(e => e.IdClient, "fk_cliente");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.IdClient)
                .HasColumnType("bigint(20)")
                .HasColumnName("id_client");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.Total)
                .HasPrecision(16, 2)
                .HasColumnName("total");

        });
        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("images");

            entity.HasIndex(e => e.IdProduct, "fk_product_image");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.DeleteHash)
                .HasColumnType("varchar(20)")
                .HasColumnName("delete_hash");
            entity.Property(e => e.Name)
                .HasColumnType("varchar(50)")
                .HasColumnName("name");
            entity.Property(e => e.Url)
            .HasColumnType("varchar(255)")
            .HasColumnName("url");
            entity.Property(e => e.IdProduct)
                .HasColumnType("bigint(20)")
                .HasColumnName("id_product")
                .IsRequired(false); ;
            entity.Property(e => e.IdUser)
                .HasColumnType("int(11)")
                .HasColumnName("id_user")
                .IsRequired(false); ;

        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.IdRole, "fk_rol");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IdCard)
                .HasMaxLength(50)
                .HasColumnName("id_card");
            entity.Property(e => e.IdRole)
                .HasColumnType("int(11)")
                .HasColumnName("id_role");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Mail)
                .HasMaxLength(50)
                .HasColumnName("mail");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .HasColumnName("password");
            entity.Property(e => e.State).HasColumnName("state");
            entity.HasOne(u => u.Avatar)
                   .WithOne(i => i.User)
                   .IsRequired(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
