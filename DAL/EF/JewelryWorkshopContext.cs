using System;
using System.Collections.Generic;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.EF;

public partial class JewelryWorkshopContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;

    public JewelryWorkshopContext(DbContextOptions<JewelryWorkshopContext> options, ILoggerFactory loggerFactory)
        : base(options)
    {
        _loggerFactory = loggerFactory;
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public DbSet<OrderCountViewModel> OrderCountView { get; set; }

    public DbSet<CustomerOrderTotalModel> CustomerOrderTotal { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<JewelryType> JewelryTypes { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrdersProduct> OrdersProducts { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<PreciousStone> PreciousStones { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsType> ProductsTypes { get; set; }

    public virtual DbSet<ShoppingList> ShoppingLists { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<StonesWeight> StonesWeights { get; set; }

    public virtual DbSet<WishList> WishLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer
("Server=localhost;Database=JewelryWorkshop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

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

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<JewelryType>(entity =>
        {
            entity.HasKey(e => e.JewelryTypeId).HasName("PK__JewelryT__7DCE2476BC0E9EEC");

            entity.Property(e => e.JewelryTypeId).HasColumnName("JewelryTypeID");
            entity.Property(e => e.JewelryTypeDescription).HasMaxLength(250);
            entity.Property(e => e.JewelryTypeName).HasMaxLength(50);

            entity.HasMany(d => d.Sizes).WithMany(p => p.JewelryTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "JewelryTypesSize",
                    r => r.HasOne<Size>().WithMany()
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_JewelryTypesSizes_Size"),
                    l => l.HasOne<JewelryType>().WithMany()
                        .HasForeignKey("JewelryTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_JewelryTypesSizes_JewelryType"),
                    j =>
                    {
                        j.HasKey("JewelryTypeId", "SizeId");
                        j.ToTable("JewelryTypesSizes");
                        j.IndexerProperty<int>("JewelryTypeId").HasColumnName("JewelryTypeID");
                        j.IndexerProperty<int>("SizeId").HasColumnName("SizeID");
                    });
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__Material__C50613171ECA7350");

            entity.Property(e => e.MaterialId).HasColumnName("MaterialID");
            entity.Property(e => e.Density).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaterialName).HasMaxLength(50);
            entity.Property(e => e.PriceForGram).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF208242AB");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_User");
        });

        modelBuilder.Entity<OrdersProduct>(entity =>
        {
            entity.HasKey(e => e.OrdersProductsId).HasName("PK__OrdersPr__F92EF80AA540F013");

            entity.Property(e => e.OrdersProductsId).HasColumnName("OrdersProductsID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrdersProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdersProducts_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrdersProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdersProducts_Product");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.PhotoId).HasName("PK__Photos__21B7B5829EE8E129");

            entity.Property(e => e.PhotoId).HasColumnName("PhotoID");
            entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Photos)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Photo_ProductType");
        });

        modelBuilder.Entity<PreciousStone>(entity =>
        {
            entity.HasKey(e => e.PreciousStoneId).HasName("PK__Precious__791F51EB7F04EC3F");

            entity.Property(e => e.PreciousStoneId).HasColumnName("PreciousStoneID");
            entity.Property(e => e.PreciousStoneName).HasMaxLength(50);
            entity.Property(e => e.PriceForCarat).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED5B827581");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.JewelryTypeId).HasColumnName("JewelryTypeID");
            entity.Property(e => e.MaterialId).HasColumnName("MaterialID");
            entity.Property(e => e.PreciousStoneId).HasColumnName("PreciousStoneID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");
            entity.Property(e => e.SizeId).HasColumnName("SizeID");
            entity.Property(e => e.StoneWeightId).HasColumnName("StoneWeightID");

            entity.HasOne(d => d.Material).WithMany(p => p.Products)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Material");

            entity.HasOne(d => d.PreciousStone).WithMany(p => p.Products)
                .HasForeignKey(d => d.PreciousStoneId)
                .HasConstraintName("FK_Products_PreciousStone");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_ProductType");

            entity.HasOne(d => d.Size).WithMany(p => p.Products)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Size");

            entity.HasOne(d => d.StoneWeight).WithMany(p => p.Products)
                .HasForeignKey(d => d.StoneWeightId)
                .HasConstraintName("FK_Products_StoneWeight");

            entity.HasMany(d => d.WishLists).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "WishListProduct",
                    r => r.HasOne<WishList>().WithMany()
                        .HasForeignKey("WishListId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_WishListProducts_WishList"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_WishListProducts_Product"),
                    j =>
                    {
                        j.HasKey("ProductId", "WishListId");
                        j.ToTable("WishListProducts");
                        j.IndexerProperty<int>("ProductId").HasColumnName("ProductID");
                        j.IndexerProperty<int>("WishListId").HasColumnName("WishListID");
                    });
        });

        modelBuilder.Entity<ProductsType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PK__Products__A1312F4E164E42EC");

            entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");
            entity.Property(e => e.AdditionalSizeCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AdditionalWorkCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.JewelryTypeId).HasColumnName("JewelryTypeID");
            entity.Property(e => e.MinPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductTypeDescription).HasMaxLength(250);
            entity.Property(e => e.ProductTypeName).HasMaxLength(100);

            entity.HasOne(d => d.JewelryType).WithMany(p => p.ProductsTypes)
                .HasForeignKey(d => d.JewelryTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductsType_JewelryType");

            entity.HasMany(d => d.Materials).WithMany(p => p.ProductTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductMaterial",
                    r => r.HasOne<Material>().WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductMaterials_Material"),
                    l => l.HasOne<ProductsType>().WithMany()
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductMaterials_ProductType"),
                    j =>
                    {
                        j.HasKey("ProductTypeId", "MaterialId");
                        j.ToTable("ProductMaterials");
                        j.IndexerProperty<int>("ProductTypeId").HasColumnName("ProductTypeID");
                        j.IndexerProperty<int>("MaterialId").HasColumnName("MaterialID");
                    });

            entity.HasMany(d => d.PreciousStones).WithMany(p => p.ProductTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductPreciousStone",
                    r => r.HasOne<PreciousStone>().WithMany()
                        .HasForeignKey("PreciousStoneId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductPreciousStones_PreciousStone"),
                    l => l.HasOne<ProductsType>().WithMany()
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductPreciousStones_ProductType"),
                    j =>
                    {
                        j.HasKey("ProductTypeId", "PreciousStoneId");
                        j.ToTable("ProductPreciousStones");
                        j.IndexerProperty<int>("ProductTypeId").HasColumnName("ProductTypeID");
                        j.IndexerProperty<int>("PreciousStoneId").HasColumnName("PreciousStoneID");
                    });

            entity.HasMany(d => d.StoneWeights).WithMany(p => p.ProductTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductTypesStonesWeight",
                    r => r.HasOne<StonesWeight>().WithMany()
                        .HasForeignKey("StoneWeightId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductTypesStonesWeight_StoneWeight"),
                    l => l.HasOne<ProductsType>().WithMany()
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductTypesStonesWeight_ProductType"),
                    j =>
                    {
                        j.HasKey("ProductTypeId", "StoneWeightId");
                        j.ToTable("ProductTypesStonesWeight");
                        j.IndexerProperty<int>("ProductTypeId").HasColumnName("ProductTypeID");
                        j.IndexerProperty<int>("StoneWeightId").HasColumnName("StoneWeightID");
                    });
        });

        modelBuilder.Entity<ShoppingList>(entity =>
        {
            entity.HasKey(e => e.ShoppingListId).HasName("PK__Shopping__6CBBDD74F875F01C");

            entity.ToTable("ShoppingList");

            entity.Property(e => e.ShoppingListId).HasColumnName("ShoppingListID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.ShoppingLists)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingList_Product");

            entity.HasOne(d => d.User).WithMany(p => p.ShoppingLists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShoppingList_User");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__Sizes__83BD095A649232A8");

            entity.Property(e => e.SizeId).HasColumnName("SizeID");
            entity.Property(e => e.SizeName).HasMaxLength(50);
            entity.Property(e => e.SizeVolume).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<StonesWeight>(entity =>
        {
            entity.HasKey(e => e.StoneWeightId).HasName("PK__StonesWe__89E8A305C6704AF7");

            entity.ToTable("StonesWeight");

            entity.Property(e => e.StoneWeightId).HasColumnName("StoneWeightID");
            entity.Property(e => e.StonesWeights).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(e => e.WishListId).HasName("PK__WishList__E41F87A7E0218C33");

            entity.ToTable("WishList");

            entity.Property(e => e.WishListId).HasColumnName("WishListID");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");
            entity.Property(e => e.WishListAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.WishListName).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.WishLists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WishList_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
