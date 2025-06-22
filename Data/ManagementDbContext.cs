using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Data
{
    public class ManagementDbContext : DbContext
    {
        public ManagementDbContext(DbContextOptions<ManagementDbContext> options) : base(options)
        {
            
        }
        public DbSet<Company> Companies { get; set; }                  
        public DbSet<MachinePaymentMethod> MachinePaymentMethods { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Manufacturer> Manufacturers{ get; set; }
        public DbSet<Modem> Modems { get; set; }
        public DbSet<Money> Money { get; set; }
        public DbSet<OperatingMode> OperatingModes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SimCard> SimCards { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VendingAvaliability> VendingAvaliabilities { get; set; }
        public DbSet<VendingMachine> VendingMachines { get; set; }
        public DbSet<VendingMachineMatrix> VendingMachineMatrices { get; set; }
        public DbSet<VendingMachineMoney> VendingMachineMoney { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.ID);
                entity.HasIndex(c => c.Name)
                    .IsUnique();
                entity.Property(e => e.Finances)
                    .HasConversion(e => Math.Round(e, 2),
                                   e => e);
                entity.Property(e => e.ParentCompanyID)
                    .IsRequired(false);

                entity.HasCheckConstraint("CK_CompanyPhone", $"Phone LIKE '[1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
                entity.HasIndex(e => e.Phone)
                    .IsUnique();

            });

            modelBuilder.Entity<MachinePaymentMethod>(entity =>
            { //составной ключ для связи таблиц
                entity.HasKey(e => new { e.PaymentMethodID, e.VendingMachineID });

                entity.HasOne(e => e.PaymentMethod)
                .WithMany(e => e.MachinePaymentMethods)
                .HasForeignKey(e => e.PaymentMethodID);

                entity.HasOne(e => e.VendingMachine)
                .WithMany(e => e.MachinePaymentMethods)
                .HasForeignKey(e => e.VendingMachineID);
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasOne(e => e.Maintainer)
                    .WithMany(e => e.Maintenances)
                    .HasForeignKey(e => e.MaintainerID)
                    .OnDelete(DeleteBehavior.SetNull); ;

                entity.HasOne(e => e.VendingMachine)
                    .WithMany(e => e.Maintenances)
                    .HasForeignKey(e => e.VendingMachineID)
                    .OnDelete(DeleteBehavior.Cascade); ;
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasIndex(c => c.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Modem>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.SimCardID)
                    .IsRequired(false);
                entity.HasOne(e => e.SimCard)
                    .WithOne(e => e.Modem)
                    .HasForeignKey<Modem>(e => e.SimCardID);
                entity.HasOne(e => e.Company)
                    .WithMany(e => e.Modems)
                    .HasForeignKey(e => e.CompanyID);
                entity.HasIndex(c => c.SerialNum)
                    .IsUnique();

            });

            modelBuilder.Entity<OperatingMode>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasIndex(c => c.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasIndex(c => c.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasIndex(c => c.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasIndex(c => c.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasOne(e => e.PaymentMethod)
                    .WithMany(e => e.Sales)
                    .HasForeignKey(e => e.PaymentMethodID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.VendingMachine)
                    .WithMany(e => e.Sales)
                    .HasForeignKey(e => e.VendingMachineID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                    .WithMany(e => e.Sales)
                    .HasForeignKey(e => e.ProductID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.Property(e => e.Cost)
                    .HasConversion(e => Math.Round(e, 2),
                                   e => e);

                entity.Property(e => e.Change)
                    .HasConversion(e => Math.Round(e, 2),
                                   e => e);

                entity.Property(e => e.Deposit)
                    .HasConversion(e => Math.Round(e, 2),
                                   e => e);
            });

            modelBuilder.Entity<SimCard>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Balance)
                .HasConversion(e =>
                    Math.Round(e, 2),
                    e => e
                );
                entity.HasCheckConstraint("CK_PhoneNum", $"Number LIKE '[1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
                entity.HasIndex(e => e.Number)
                .IsUnique();
                
                entity.HasOne(e => e.Company)
                    .WithMany(e => e.SimCards)
                    .HasForeignKey(e => e.CompanyID);
            });
            

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasIndex(c => c.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasOne(e => e.Company)
                    .WithMany(e => e.CompanyUsers)
                    .HasForeignKey(e => e.CompanyID);
                entity.HasOne(e => e.Role)
                    .WithMany(e => e.Users)
                    .HasForeignKey(e => e.RoleID)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasIndex(c => c.Login)
                    .IsUnique();
                entity.HasIndex(c => c.Email)
                    .IsUnique();
                entity.HasCheckConstraint("CK_CheckEmail", "Email LIKE '%@%.%'");

                entity.HasCheckConstraint("CK_UserPhone", $"Phone LIKE '[1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
                entity.HasIndex(e => e.Phone)
                    .IsUnique();
            });

            modelBuilder.Entity<VendingAvaliability>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasOne(e => e.VendingMachine)
                    .WithMany(e => e.VendingAvaliabilities)
                    .HasForeignKey(e => e.VendingMachineID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                    .WithMany(e => e.VendingAvaliabilities)
                    .HasForeignKey(e => e.ProductID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.Property(e => e.Price)
                    .HasConversion(e => Math.Round(e, 2),
                                   e => e);
            });

            modelBuilder.Entity<VendingMachine>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.HasOne(e => e.Status)
                    .WithMany(e => e.VendingMachines)
                    .HasForeignKey(e => e.StatusID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Modem)
                    .WithOne(e => e.VendingMachine)
                    .HasForeignKey<VendingMachine>(e => e.ModemID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Company)
                    .WithMany(c => c.VendingMachines)
                    .HasForeignKey(e => e.CompanyID);

                entity.HasOne(e => e.OperatingMode)
                    .WithMany(e => e.VendingMachines)
                    .HasForeignKey(e => e.OperatingModeID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.VendingMachineMatrix)
                    .WithMany(e => e.VendingMachines)
                    .HasForeignKey(e => e.ModelID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Money>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasIndex(e => e.Name)
                    .IsUnique();
                entity.Property(e => e.Value)
                    .HasConversion(e => Math.Round(e, 2),
                                   e => e);
            });

            modelBuilder.Entity<VendingMachineMatrix>(entity =>
            { //составной ключ для связи таблиц
                entity.HasKey(e => e.ID);

                entity.HasIndex(e => e.ModelName)
                    .IsUnique();

                entity.HasOne(e => e.Manufacturer)
                    .WithMany(e => e.VendingMachineMatrices)
                    .HasForeignKey(e => e.ManufacturerID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<VendingMachineMoney>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasOne(e => e.VendingMachine)
                    .WithMany(e => e.VendingMachineMoney)
                    .HasForeignKey(e => e.VendingMachineID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
