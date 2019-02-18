using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SetData
{
    public partial class AUTO_PARTS_2019Context : DbContext
    {
        public AUTO_PARTS_2019Context()
        {
        }

        public AUTO_PARTS_2019Context(DbContextOptions<AUTO_PARTS_2019Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<CartLine> CartLine { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Cross> Cross { get; set; }
        public virtual DbSet<DefaultDiscounts> DefaultDiscounts { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrdersDto> OrdersDto { get; set; }
        public virtual DbSet<Parts> Parts { get; set; }

        // Unable to generate entity type for table 'dbo.CROSS1'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Filter'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("строка подключения к бд");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.HasIndex(e => e.UserIdid);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.Discriminator).IsRequired();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Ip).HasColumnName("IP");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.OneCcreate).HasColumnName("OneCCreate");

                entity.Property(e => e.UserIdid).HasColumnName("UserIDId");

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.UserId)
                    .WithMany(p => p.InverseUserId)
                    .HasForeignKey(d => d.UserIdid);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<CartLine>(entity =>
            {
                entity.HasIndex(e => e.PartId);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PartId).HasColumnName("PartID");

                entity.HasOne(d => d.Part)
                    .WithMany(p => p.CartLine)
                    .HasForeignKey(d => d.PartId);
            });

            modelBuilder.Entity<Courses>(entity =>
            {
                entity.HasIndex(e => e.PartId);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PartId).HasColumnName("PartID");

                entity.HasOne(d => d.Part)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.PartId);
            });

            modelBuilder.Entity<Cross>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<DefaultDiscounts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });

            //modelBuilder.Entity<Orders>(entity =>
            //{
            //    entity.HasKey(e => e.OrderId);

            //    entity.HasIndex(e => e.AddressId);

            //    entity.HasIndex(e => e.CartLineId);

            //    entity.HasIndex(e => e.PartId);

            //    entity.Property(e => e.OrderId).HasColumnName("OrderID");

            //    entity.Property(e => e.CartLineId).HasColumnName("CartLineID");

            //    entity.Property(e => e.OneCcreate).HasColumnName("OneCCreate");

            //    entity.Property(e => e.PartId).HasColumnName("PartID");

            //    entity.HasOne(d => d.Address)
            //        .WithMany(p => p.Orders)
            //        .HasForeignKey(d => d.AddressId);

            //    entity.HasOne(d => d.CartLine)
            //        .WithMany(p => p.Orders)
            //        .HasForeignKey(d => d.CartLineId);

            //    entity.HasOne(d => d.Part)
            //        .WithMany(p => p.Orders)
            //        .HasForeignKey(d => d.PartId);
            //});

            //modelBuilder.Entity<OrdersDto>(entity =>
            //{
            //    entity.HasKey(e => e.OrderId);

            //    entity.ToTable("OrdersDTO");

            //    entity.Property(e => e.OrderId)
            //        .HasColumnName("OrderID")
            //        .ValueGeneratedNever();

            //    entity.Property(e => e.AddressId).HasColumnName("AddressID");

            //    entity.Property(e => e.FotoLink).HasColumnName("Foto_link");

            //    entity.Property(e => e.GroupAuto).HasColumnName("Group_Auto");

            //    entity.Property(e => e.GroupParts).HasColumnName("Group_Parts");

            //    entity.Property(e => e.Ip).HasColumnName("IP");

            //    entity.Property(e => e.PartId).HasColumnName("PartID");
            //});

            modelBuilder.Entity<Parts>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FotoLink).HasColumnName("Foto_link");

                entity.Property(e => e.GroupAuto).HasColumnName("Group_Auto");

                entity.Property(e => e.GroupParts).HasColumnName("Group_Parts");

                entity.Property(e => e.OneCcreate).HasColumnName("OneCCreate");
            });
        }
    }
}
