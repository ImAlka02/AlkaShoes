using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AlkaShoes.Models.Entities;

public partial class AlkashoesContext : DbContext
{
    public AlkashoesContext()
    {
    }

    public AlkashoesContext(DbContextOptions<AlkashoesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrito> Carrito { get; set; }

    public virtual DbSet<Marca> Marca { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<Talla> Talla { get; set; }

    public virtual DbSet<Tallasproducto> Tallasproducto { get; set; }

    public virtual DbSet<User> User { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("carrito");

            entity.HasIndex(e => e.IdProducto, "IdProducto");

            entity.HasIndex(e => e.IdUser, "IdUser");

            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.PrecioCadaUno).HasPrecision(6, 2);

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Carrito)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("carrito_ibfk_2");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Carrito)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("carrito_ibfk_1");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("marca");

            entity.Property(e => e.NombreMarca).HasMaxLength(45);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("producto");

            entity.HasIndex(e => e.IdMarca, "IdMarca");

            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Precio).HasPrecision(6, 2);
            entity.Property(e => e.Sku).HasMaxLength(10);

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("producto_ibfk_1");
        });

        modelBuilder.Entity<Talla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("talla");

            entity.Property(e => e.Talla1)
                .HasMaxLength(10)
                .HasColumnName("Talla");
        });

        modelBuilder.Entity<Tallasproducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tallasproducto");

            entity.HasIndex(e => e.IdProducto, "IdProducto");

            entity.HasIndex(e => e.IdTalla, "IdTalla");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Tallasproducto)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tallasproducto_ibfk_1");

            entity.HasOne(d => d.IdTallaNavigation).WithMany(p => p.Tallasproducto)
                .HasForeignKey(d => d.IdTalla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tallasproducto_ibfk_2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.Contraseña)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.Correo).HasMaxLength(50);
            entity.Property(e => e.Direccion).HasColumnType("text");
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
