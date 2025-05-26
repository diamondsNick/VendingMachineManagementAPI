using Microsoft.EntityFrameworkCore;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Data
{
    public class ManagementDbContext : DbContext
    {
        public ManagementDbContext(DbContextOptions<ManagementDbContext> options) : base(options)
        {
            
        }
        public DbSet<Company> Companies;
        public DbSet<CompanyUser> CompanyUsers;
        public DbSet<MachineModel> MachineModels;
        public DbSet<MachinePaymentMethod> MachinePaymentMethods;
        public DbSet<Maintenance> Maintenances;
        public DbSet<Manufacturer> Manufacturers;
        public DbSet<Modem> Modems;
        public DbSet<Money> Money;
        public DbSet<OperatingMode> OperatingModes;
        public DbSet<PaymentMethod> PaymentMethods;
        public DbSet<Product> Products;
        public DbSet<ProductMatrix> ProductMatrices;
        public DbSet<Role> Roles;
        public DbSet<Sale> Sales;
        public DbSet<SimCard> SimCards;
        public DbSet<Status> Statuses;
        public DbSet<User> Users;
        public DbSet<VendingAvaliability> VendingAvaliabilities;
        public DbSet<VendingMachine> VendingMachines;
        public DbSet<VendingMachineMatrix> VendingMachineMatrices;
        public DbSet<VendingMachineMoney> VendingMachineMoney;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.ID);
            });

            modelBuilder.Entity<CompanyUser>(entity =>
            { //составной ключ для связи таблиц
                entity.HasKey(e => new {e.UserID, e.CompanyID });

                entity.HasOne(e => e.User)
                .WithMany(e => e.CompanyUsers)
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Company)
                .WithMany(e => e.CompanyUsers)
                .HasForeignKey(e => e.CompanyID)
                .OnDelete(DeleteBehavior.Cascade); ;
            });

            modelBuilder.Entity<MachineModel>(entity =>
            {
                entity.HasKey(e => e.ID);
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
                .HasForeignKey(e => e.VengingMachineID)
                .OnDelete(DeleteBehavior.Cascade); ;
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasKey(e => e.ID);
            });

            modelBuilder.Entity<Modem>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasOne(e => e.SimCard)
                .WithOne(e => e.Modem)
                .HasForeignKey<SimCard>(e => e.Number);
            });

            modelBuilder.Entity<OperatingMode>(entity =>
            {
                entity.HasKey(e => e.ID);
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.HasKey(e => e.ID);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ID);
            });

            modelBuilder.Entity<ProductMatrix>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasOne(e => e.MachineModel)
                .WithMany(e => e.ProductMatrices)
                .HasForeignKey(e => e.MachineModelID)
                .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Manufacturer)
                .WithMany(e => e.ProductMatrices)
                .HasForeignKey(e => e.ManufacturerID)
                .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.ID);
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
            });

            modelBuilder.Entity<SimCard>(entity =>
            {
                entity.HasKey(e => e.Number);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.ID);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleID)
                .OnDelete(DeleteBehavior.SetNull);
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
            });

            modelBuilder.Entity<VendingMachine>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasOne(e => e.Status)
                .WithMany(e => e.VendingMachines)
                .HasForeignKey(e => e.StatusID)
                .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Modem)
                .WithOne(e => e.VendingMachine)
                .HasForeignKey<VendingMachine>(e => e.ModemID)
                .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.OperatingMode)
                .WithMany(e => e.VendingMachines)
                .HasForeignKey(e => e.OperatingModeID)
                .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<VendingMachineMatrix>(entity =>
            { //составной ключ для связи таблиц
                entity.HasKey(e => new { e.VendingMachineID, e.ProductMatrixID });

                entity.HasOne(e => e.VendingMachine)
                .WithOne(e => e.VendingMachineMatrix)
                .HasForeignKey<VendingMachineMatrix>(e => e.VendingMachineID)
                .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.ProductMatrix)
                .WithMany(e => e.VendingMachinesMatrices)
                .HasForeignKey(e => e.ProductMatrixID)
                .OnDelete(DeleteBehavior.Cascade);
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
