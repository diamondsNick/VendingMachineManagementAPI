using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Data
{
    public class SeedData
    {
        public static async Task InitializeDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ManagementDbContext>();

            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при миграции: {ex.Message}");
            }

            await SeedManufacturersAsync(context);
            await SeedModelsAsync(context);
            await SeedStatusesAsync(context);
            await SeedRolesAsync(context);
            await SeedManufacturersAsync(context);
            await SeedCompaniesAsync(context);
            await SeedSimCardsAsync(context);
            await SeedModemsAsync(context);
            await SeedWorkModesAsync(context);
            await SeedVendingMachinesAsync(context);
            await SeedProductsAsync(context);
            await SeedVendingAvaliability(context);
            await SeedPaymentMethodes(context);
            await SeedVendingMachinePaymentMethodsAsync(context);
            await SeedSalesAsync(context);


        }
        private static async Task SeedManufacturersAsync(ManagementDbContext context)
        {
            
            if (!await context.Manufacturers.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Manufacturers', RESEED, 1)");
                await context.Manufacturers.AddRangeAsync(
                    new Manufacturer { Name = "Saeco" },
                    new Manufacturer { Name = "Unicum" },
                    new Manufacturer { Name = "Bianchi" },
                    new Manufacturer { Name = "Jofemar" },
                    new Manufacturer { Name = "Necta" },
                    new Manufacturer { Name = "Rheavendors" },
                    new Manufacturer { Name = "FAS" }
                );
                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }
        private static async Task SeedModelsAsync(ManagementDbContext context)
        {
            
            if (!await context.VendingMachineMatrices.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('VendingMachineMatrices', RESEED, 1)");
                await context.VendingMachineMatrices.AddRangeAsync(
                    new VendingMachineMatrix { ManufacturerID = 1, ModelName = "Cristallo 400" },
                    new VendingMachineMatrix { ManufacturerID = 2, ModelName = "Rosso" },
                    new VendingMachineMatrix { ManufacturerID = 3, ModelName = "BVM 972" },
                    new VendingMachineMatrix { ManufacturerID = 4, ModelName = "Coffeemar 250" },
                    new VendingMachineMatrix { ManufacturerID = 5, ModelName = "Kikko MAX" },
                    new VendingMachineMatrix { ManufacturerID = 5, ModelName = "Kikko ES6" },
                    new VendingMachineMatrix { ManufacturerID = 6, ModelName = "Luce E5" },
                    new VendingMachineMatrix { ManufacturerID = 7, ModelName = "Perla" },
                    new VendingMachineMatrix { ManufacturerID = 2, ModelName = "Food Box" }
                );
                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }
        private static async Task SeedStatusesAsync(ManagementDbContext context)
        {
            
            if (!await context.Statuses.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Statuses', RESEED, 1)");
                await context.Statuses.AddRangeAsync(
                    new Status { Name = "Работает" },
                    new Status { Name = "На обслуживании" },
                    new Status { Name = "Не работает" }
                );
                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }

        private static async Task SeedSimCardsAsync(ManagementDbContext context)
        {
            
            if (!await context.SimCards.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('SimCards', RESEED, 1)");
                await context.SimCards.AddRangeAsync(
                    new SimCard { Number = "79001234567", Vendor = "МТС", Balance = 1500.50m, CompanyID = 1 },
                    new SimCard { Number = "79039876543", Vendor = "Билайн", Balance = 750.00m, CompanyID = 1 },
                    new SimCard { Number = "79201112233", Vendor = "МегаФон", Balance = 2300.75m, CompanyID = 1 },
                    new SimCard { Number = "79785556677", Vendor = "Теле2", Balance = 120.10m, CompanyID = 1 },
                    new SimCard { Number = "79658765432", Vendor = "Ростелеком", Balance = 320.00m, CompanyID = 1 },
                    new SimCard { Number = "79062223334", Vendor = "МТС", Balance = 5000.00m, CompanyID = 1 },
                    new SimCard { Number = "79093334445", Vendor = "Билайн", Balance = 60.00m, CompanyID = 1 },
                    new SimCard { Number = "79281239876", Vendor = "МегаФон", Balance = 980.35m, CompanyID = 1 },
                    new SimCard { Number = "79765432109", Vendor = "Теле2", Balance = 410.00m, CompanyID = 1 },
                    new SimCard { Number = "79615556677", Vendor = "Ростелеком", Balance = 1299.99m, CompanyID = 1 }
                );

                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }

        private static async Task SeedWorkModesAsync(ManagementDbContext context)
        {
            
            if (!await context.OperatingModes.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('OperatingModes', RESEED, 1)");
                await context.OperatingModes.AddRangeAsync(
                    new OperatingMode { Name = "Обычный" },
                    new OperatingMode { Name = "Режим сна" },
                    new OperatingMode { Name = "Техобслуживание" },
                    new OperatingMode { Name = "Тестовый режим" }
                );

                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }

        private static async Task SeedRolesAsync(ManagementDbContext context)
        {
            
            if (!await context.Roles.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Roles', RESEED, 1)");
                await context.Roles.AddRangeAsync(
                    new Role { Name = "Администратор" },
                    new Role { Name = "Техник" },
                    new Role { Name = "Бухгалтер" }
                );

                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }

        private static async Task SeedModemsAsync(ManagementDbContext context)
        {
            
            if (!await context.Modems.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Modems', RESEED, 1)");
                await context.Modems.AddRangeAsync(
                    new Modem { Model = "Huawei E8372", SimCardID = 1, CompanyID = 1, SerialNum = 1234567890, Password = "pass123" },
                    new Modem { Model = "ZTE MF920", SimCardID = 2, CompanyID = 1, SerialNum = 2233445566, Password = "pass456" },
                    new Modem { Model = "Alcatel LinkKey IK40V", SimCardID = 3, CompanyID = 1, SerialNum = 3344556677, Password = "alcatel789" },
                    new Modem { Model = "Huawei E3372h", SimCardID = 4, CompanyID = 1, SerialNum = 4455667788, Password = "huawei789" },
                    new Modem { Model = "ZTE MF833V", SimCardID = 5, CompanyID = 1, SerialNum = 5566778899, Password = "ztev900" },
                    new Modem { Model = "MikroTik SXT LTE6", SimCardID = 6, CompanyID = 1, SerialNum = 6677889900, Password = "mikroT1k" },
                    new Modem { Model = "TP-Link M7200", SimCardID = 7, CompanyID = 1, SerialNum = 7788990011, Password = "tplink001" },
                    new Modem { Model = "Keenetic Runner 4G", SimCardID = 8, CompanyID = 1, SerialNum = 8899001122, Password = "runner4g" },
                    new Modem { Model = "Huawei B315s", SimCardID = 9, CompanyID = 1, SerialNum = 9900112233, Password = "b315spass" },
                    new Modem { Model = "ZTE MF79U", SimCardID = 10, CompanyID = 1, SerialNum = 1011121314, Password = "ztefinal" }
                 );

                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }

        }
        private static async Task SeedCompaniesAsync(ManagementDbContext context)
        {
            
            if (!await context.Companies.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Companies', RESEED, 1)");
                await context.Companies.AddRangeAsync(
                    new Company { Name = "КофеТайм ООО", Finances = 125000.00m, Phone = "79001234567", Adress = "г. Москва, ул. Ленина, д.10", RegistrationDate = "2020-03-15" },
                    new Company { Name = "ВендоПро", Finances = 84500.50m, Phone = "79039876543", Adress = "г. Санкт-Петербург, Невский пр., д. 25", RegistrationDate = "2019-07-01" },
                    new Company { Name = "Арома Машинс", Finances = 56000.00m, Phone = "79201112233", Adress = "г. Казань, ул. Баумана, д. 3", RegistrationDate = "2021-01-10" },
                    new Company { Name = "ВендингМаркет", Finances = 223400.75m, Phone = "79785556677", Adress = "г. Новосибирск, ул. Фрунзе, д. 50", RegistrationDate = "2018-11-23" },
                    new Company { Name = "Кофеин Экспресс", Finances = 33400.00m, Phone = "79658765432", Adress = "г. Екатеринбург, ул. Малышева, д. 17", RegistrationDate = "2022-04-05" },
                    new Company { Name = "ХотДринкс ООО", Finances = 94000.00m, Phone = "79062223334", Adress = "г. Самара, ул. Галактионовская, д. 42", RegistrationDate = "2017-09-14" },
                    new Company { Name = "Снэк энд Кофе", Finances = 155500.90m, Phone = "79093334445", Adress = "г. Ростов-на-Дону, ул. Станиславского, д. 12", RegistrationDate = "2020-12-01" },
                    new Company { Name = "ВендЛайн", Finances = 120000.00m, Phone = "79281239876", Adress = "г. Пермь, ул. Ленина, д. 7", RegistrationDate = "2019-06-20" },
                    new Company { Name = "Сити Вендинг", Finances = 44500.35m, Phone = "79765432109", Adress = "г. Уфа, ул. Коммунистическая, д. 19", RegistrationDate = "2021-09-12" },
                    new Company { Name = "АвтоСнэк", Finances = 132000.00m, Phone = "79615556677", Adress = "г. Волгоград, пр. Ленина, д. 80", RegistrationDate = "2018-02-28" }
                );
                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }

        private static async Task SeedVendingMachinesAsync(ManagementDbContext context)
        {
            
            if (!await context.VendingMachines.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('VendingMachines', RESEED, 1)");
                await context.VendingMachines.AddRangeAsync(
                    new VendingMachine
                    {
                        StatusID = 2,
                        OperatingModeID = 1,
                        CompanyID = 1,
                        ModelID = 1,
                        ModemID = 1,
                        TimeZone = "UTC +3",
                        Name = "БЦ «Московский»",
                        Adress = "Суворова 121",
                        Coordinates = null,
                        PlacementType = "у входа",
                        PlacementDate = "2018-05-12",
                        StartHours = "08:00",
                        EndHours = "20:00"
                    },
                    new VendingMachine
                    {
                        StatusID = 3,
                        OperatingModeID = 1,
                        CompanyID = 1,
                        ModelID = 2,
                        ModemID = 2,
                        TimeZone = "UTC +3",
                        Name = "ГП «Магнит»",
                        Adress = "Академическая 15 улица",
                        Coordinates = null,
                        PlacementType = "вход",
                        PlacementDate = "2018-05-13",
                        StartHours = "08:00",
                        EndHours = "20:00"
                    },
                    new VendingMachine
                    {
                        StatusID = 1,
                        OperatingModeID = 1,
                        CompanyID = 1,
                        ModelID = 3,
                        ModemID = 3,
                        TimeZone = "UTC +3",
                        Name = "ДОСААФ",
                        Adress = "Баррикад 174",
                        Coordinates = null,
                        PlacementType = "у входа",
                        PlacementDate = "2018-05-13",
                        StartHours = "08:00",
                        EndHours = "20:00"
                    },
                    new VendingMachine
                    {
                        StatusID = 1,
                        OperatingModeID = 1,
                        CompanyID = 1,
                        ModelID = 5,
                        ModemID = 4,
                        TimeZone = "UTC +3",
                        Name = "Завод «Тайфун»",
                        Adress = "Грабцевское шоссе 174",
                        Coordinates = null,
                        PlacementType = "в помещении",
                        PlacementDate = "2018-05-13",
                        StartHours = "08:00",
                        EndHours = "20:00"
                    },
                    new VendingMachine
                    {
                        StatusID = 3,
                        OperatingModeID = 1,
                        CompanyID = 1,
                        ModelID = 7,
                        ModemID = 5,
                        TimeZone = "UTC +3",
                        Name = "Мойка «У Гризли»",
                        Adress = "Карла Либкнехта 31",
                        Coordinates = null,
                        PlacementType = "в зале ожидания",
                        PlacementDate = "2018-05-13",
                        StartHours = "08:00",
                        EndHours = "20:00"
                    },
                    new VendingMachine
                    {
                        StatusID = 3,
                        OperatingModeID = 1,
                        CompanyID = 1,
                        ModelID = 8,
                        ModemID = 6,
                        TimeZone = "UTC +3",
                        Name = "Налоговая",
                        Adress = "пер. Воскресенский 28",
                        Coordinates = null,
                        PlacementType = "в помещении",
                        PlacementDate = "2018-05-13",
                        StartHours = "08:00",
                        EndHours = "20:00"
                    },
                    new VendingMachine
                    {
                        StatusID = 2,
                        OperatingModeID = 1,
                        CompanyID = 1,
                        ModelID = 4,
                        ModemID = 7,
                        TimeZone = "UTC +3",
                        Name = "Рынок",
                        Adress = "Грабцевское шоссе 46",
                        Coordinates = null,
                        PlacementType = "вход",
                        PlacementDate = "2018-05-11",
                        StartHours = "08:00",
                        EndHours = "20:00"
                    },
                    new VendingMachine
                    {
                        StatusID = 3,
                        OperatingModeID = 1,
                        CompanyID = 1,
                        ModelID = 6,
                        ModemID = 8,
                        TimeZone = "UTC +3",
                        Name = "ТК «21 Век»",
                        Adress = "Кирова 1",
                        Coordinates = null,
                        PlacementType = "3 этаж",
                        PlacementDate = "2018-05-11",
                        StartHours = "08:00",
                        EndHours = "20:00"
                    },
                    new VendingMachine
                    {
                        StatusID = 3,
                        OperatingModeID = 1,
                        CompanyID = 1,
                        ModelID = 9,
                        ModemID = 9,
                        TimeZone = "UTC +3",
                        Name = "ТК «21 Век» (снэк)",
                        Adress = "Кирова 1",
                        Coordinates = null,
                        PlacementType = "3 этаж",
                        PlacementDate = "2018-05-11",
                        StartHours = "08:00",
                        EndHours = "20:00"
                    }
                );
                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }

        private static async Task SeedProductsAsync(ManagementDbContext context)
        {
            
            if (!await context.Products.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Products', RESEED, 1)");
                await context.Products.AddRangeAsync(
                    new Product { Name = "Чипсы Lays", Description = "Картофельные чипсы со вкусом сметаны и лука", AvgSales = 134.5f },
                    new Product { Name = "Шоколад Alpen Gold", Description = "Молочный шоколад с орехами", AvgSales = 87.2f },
                    new Product { Name = "Жевательная резинка Orbit", Description = "Мятная жвачка без сахара", AvgSales = 152.9f },
                    new Product { Name = "Вода BonAqua", Description = "Негазированная питьевая вода, 0.5 л", AvgSales = 168.0f },
                    new Product { Name = "Энергетик Adrenaline Rush", Description = "Энергетический напиток, 0.25 л", AvgSales = 142.6f },
                    new Product { Name = "Печенье Oreo", Description = "Два шоколадных печенья с ванильной начинкой", AvgSales = 95.3f },
                    new Product { Name = "Зарядный кабель USB-C", Description = "Универсальный кабель для зарядки устройств", AvgSales = 36.7f },
                    new Product { Name = "Наушники-вкладыши", Description = "Компактные проводные наушники с разъёмом 3.5 мм", AvgSales = 22.4f },
                    new Product { Name = "Дезинфектор для рук", Description = "Маленький флакон антисептика, 50 мл", AvgSales = 49.1f },
                    new Product { Name = "Пауэрбанк 5000 мАч", Description = "Портативное зарядное устройство для телефона", AvgSales = 18.9f }
                );
                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }

        private static async Task SeedVendingAvaliability(ManagementDbContext context)
        {
            
            if(!await context.VendingAvaliabilities.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('VendingAvaliabilities', RESEED, 1)");
                await context.VendingAvaliabilities.AddRangeAsync(
                    new VendingAvaliability { VendingMachineID = 1, ProductID = 4, Quantity = 10, Price = 59.99m },
                    new VendingAvaliability { VendingMachineID = 1, ProductID = 5, Quantity = 20, Price = 9.99m },
                    new VendingAvaliability { VendingMachineID = 2, ProductID = 1, Quantity = 15, Price = 299.99m },
                    new VendingAvaliability { VendingMachineID = 2, ProductID = 4, Quantity = 15, Price = 59.99m },
                    new VendingAvaliability { VendingMachineID = 3, ProductID = 3, Quantity = 10, Price = 59.99m },
                    new VendingAvaliability { VendingMachineID = 3, ProductID = 8, Quantity = 10, Price = 59.99m },
                    new VendingAvaliability { VendingMachineID = 4, ProductID = 6, Quantity = 25, Price = 60.00m },
                    new VendingAvaliability { VendingMachineID = 4, ProductID = 8, Quantity = 10, Price = 59.99m },
                    new VendingAvaliability { VendingMachineID = 4, ProductID = 10, Quantity = 12, Price = 19.00m },
                    new VendingAvaliability { VendingMachineID = 5, ProductID = 3, Quantity = 20, Price = 59.99m },
                    new VendingAvaliability { VendingMachineID = 5, ProductID = 10, Quantity = 20, Price = 19.00m },
                    new VendingAvaliability { VendingMachineID = 5, ProductID = 9, Quantity = 12, Price = 12.99m },
                    new VendingAvaliability { VendingMachineID = 5, ProductID = 6, Quantity = 10, Price = 60.00m },
                    new VendingAvaliability { VendingMachineID = 6, ProductID = 5, Quantity = 20, Price = 9.99m },
                    new VendingAvaliability { VendingMachineID = 6, ProductID = 7, Quantity = 10, Price = 49.50m },
                    new VendingAvaliability { VendingMachineID = 6, ProductID = 10, Quantity = 8, Price = 19.00m },
                    new VendingAvaliability { VendingMachineID = 7, ProductID = 1, Quantity = 15, Price = 299.99m },
                    new VendingAvaliability { VendingMachineID = 7, ProductID = 6, Quantity = 20, Price = 60.00m },
                    new VendingAvaliability { VendingMachineID = 7, ProductID = 3, Quantity = 10, Price = 59.99m }

                    );
                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }

            }
        }

        private static async Task SeedPaymentMethodes(ManagementDbContext context)
        {
            if(!await context.PaymentMethods.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('PaymentMethods', RESEED, 1)");
                await context.PaymentMethods.AddRangeAsync(
                    new PaymentMethod { Name = "Безналичная" },
                    new PaymentMethod { Name = "Купюроприемник" },
                    new PaymentMethod { Name = "Монетоприемник" },
                    new PaymentMethod { Name = "QR-код" }
                    );
                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
                
            }
        }
        private static async Task SeedVendingMachinePaymentMethodsAsync(ManagementDbContext context)
        {
            if (!await context.MachinePaymentMethods.AnyAsync())
            {
                await context.MachinePaymentMethods.AddRangeAsync(
                    new MachinePaymentMethod { VendingMachineID = 1, PaymentMethodID = 1 },
                    new MachinePaymentMethod { VendingMachineID = 1, PaymentMethodID = 2 },
                    new MachinePaymentMethod { VendingMachineID = 2, PaymentMethodID = 1 },
                    new MachinePaymentMethod { VendingMachineID = 3, PaymentMethodID = 3 },
                    new MachinePaymentMethod { VendingMachineID = 4, PaymentMethodID = 2 },
                    new MachinePaymentMethod { VendingMachineID = 5, PaymentMethodID = 1 },
                    new MachinePaymentMethod { VendingMachineID = 5, PaymentMethodID = 4 }
                );
                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                }
            }
        }

        private static async Task SeedSalesAsync(ManagementDbContext context)
        {
            if (!await context.Sales.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Sales', RESEED, 1)");
                await context.Sales.AddRangeAsync(
                    new Sale { VendingMachineID = 4, ProductID = 9, PaymentMethodID = 1, ProductAmount = 2, Cost = 25.98m, Deposit = 41.22m, Change = 15.24m, Date = DateTime.Parse("2025-06-08") },
                    new Sale { VendingMachineID = 5, ProductID = 3, PaymentMethodID = 2, ProductAmount = 3, Cost = 179.97m, Deposit = 193.22m, Change = 13.25m, Date = DateTime.Parse("2025-06-09") },
                    new Sale { VendingMachineID = 4, ProductID = 8, PaymentMethodID = 2, ProductAmount = 5, Cost = 299.95m, Deposit = 303.66m, Change = 3.71m, Date = DateTime.Parse("2025-06-13") },
                    new Sale { VendingMachineID = 6, ProductID = 5, PaymentMethodID = 2, ProductAmount = 5, Cost = 49.95m, Deposit = 50.62m, Change = 0.67m, Date = DateTime.Parse("2025-06-09") },
                    new Sale { VendingMachineID = 2, ProductID = 1, PaymentMethodID = 1, ProductAmount = 5, Cost = 1499.95m, Deposit = 1507.85m, Change = 7.9m, Date = DateTime.Parse("2025-06-16") },
                    new Sale { VendingMachineID = 5, ProductID = 10, PaymentMethodID = 2, ProductAmount = 1, Cost = 19.0m, Deposit = 36.73m, Change = 17.73m, Date = DateTime.Parse("2025-06-10") },
                    new Sale { VendingMachineID = 2, ProductID = 6, PaymentMethodID = 1, ProductAmount = 2, Cost = 120.0m, Deposit = 121.3m, Change = 1.3m, Date = DateTime.Parse("2025-06-16") },
                    new Sale { VendingMachineID = 6, ProductID = 4, PaymentMethodID = 2, ProductAmount = 3, Cost = 179.97m, Deposit = 192.51m, Change = 12.54m, Date = DateTime.Parse("2025-06-18") },
                    new Sale { VendingMachineID = 5, ProductID = 5, PaymentMethodID = 1, ProductAmount = 3, Cost = 29.97m, Deposit = 34.2m, Change = 4.23m, Date = DateTime.Parse("2025-06-17") },
                    new Sale { VendingMachineID = 5, ProductID = 10, PaymentMethodID = 1, ProductAmount = 2, Cost = 38.0m, Deposit = 46.36m, Change = 8.36m, Date = DateTime.Parse("2025-06-16") },
                    new Sale { VendingMachineID = 7, ProductID = 3, PaymentMethodID = 1, ProductAmount = 4, Cost = 239.96m, Deposit = 248.3m, Change = 8.34m, Date = DateTime.Parse("2025-06-18") },
                    new Sale { VendingMachineID = 4, ProductID = 10, PaymentMethodID = 2, ProductAmount = 2, Cost = 38.0m, Deposit = 57.6m, Change = 19.6m, Date = DateTime.Parse("2025-06-16") },
                    new Sale { VendingMachineID = 3, ProductID = 8, PaymentMethodID = 1, ProductAmount = 4, Cost = 239.96m, Deposit = 242.31m, Change = 2.35m, Date = DateTime.Parse("2025-06-11") },
                    new Sale { VendingMachineID = 2, ProductID = 6, PaymentMethodID = 1, ProductAmount = 1, Cost = 60.0m, Deposit = 68.52m, Change = 8.52m, Date = DateTime.Parse("2025-06-08") },
                    new Sale { VendingMachineID = 5, ProductID = 3, PaymentMethodID = 1, ProductAmount = 2, Cost = 119.98m, Deposit = 121.01m, Change = 1.03m, Date = DateTime.Parse("2025-06-09") },
                    new Sale { VendingMachineID = 5, ProductID = 2, PaymentMethodID = 1, ProductAmount = 5, Cost = 449.95m, Deposit = 455.46m, Change = 5.51m, Date = DateTime.Parse("2025-06-15") },
                    new Sale { VendingMachineID = 5, ProductID = 6, PaymentMethodID = 1, ProductAmount = 1, Cost = 60.0m, Deposit = 64.39m, Change = 4.39m, Date = DateTime.Parse("2025-06-14") },
                    new Sale { VendingMachineID = 1, ProductID = 10, PaymentMethodID = 1, ProductAmount = 5, Cost = 95.0m, Deposit = 98.64m, Change = 3.64m, Date = DateTime.Parse("2025-06-09") },
                    new Sale { VendingMachineID = 5, ProductID = 10, PaymentMethodID = 2, ProductAmount = 3, Cost = 57.0m, Deposit = 69.78m, Change = 12.78m, Date = DateTime.Parse("2025-06-17") },
                    new Sale { VendingMachineID = 5, ProductID = 6, PaymentMethodID = 2, ProductAmount = 1, Cost = 60.0m, Deposit = 78.62m, Change = 18.62m, Date = DateTime.Parse("2025-06-14") },
                    new Sale { VendingMachineID = 3, ProductID = 2, PaymentMethodID = 2, ProductAmount = 2, Cost = 179.98m, Deposit = 199.86m, Change = 19.88m, Date = DateTime.Parse("2025-06-08") },
                    new Sale { VendingMachineID = 3, ProductID = 2, PaymentMethodID = 2, ProductAmount = 5, Cost = 449.95m, Deposit = 457.28m, Change = 7.33m, Date = DateTime.Parse("2025-06-13") },
                    new Sale { VendingMachineID = 1, ProductID = 8, PaymentMethodID = 2, ProductAmount = 2, Cost = 119.98m, Deposit = 125.67m, Change = 5.69m, Date = DateTime.Parse("2025-06-14") },
                    new Sale { VendingMachineID = 3, ProductID = 3, PaymentMethodID = 2, ProductAmount = 1, Cost = 59.99m, Deposit = 72.41m, Change = 12.42m, Date = DateTime.Parse("2025-06-09") },
                    new Sale { VendingMachineID = 6, ProductID = 7, PaymentMethodID = 1, ProductAmount = 1, Cost = 49.5m, Deposit = 59.47m, Change = 9.97m, Date = DateTime.Parse("2025-06-13") },
                    new Sale { VendingMachineID = 6, ProductID = 3, PaymentMethodID = 2, ProductAmount = 5, Cost = 299.95m, Deposit = 313.76m, Change = 13.81m, Date = DateTime.Parse("2025-06-15") },
                    new Sale { VendingMachineID = 5, ProductID = 5, PaymentMethodID = 2, ProductAmount = 4, Cost = 39.96m, Deposit = 43.13m, Change = 3.17m, Date = DateTime.Parse("2025-06-08") },
                    new Sale { VendingMachineID = 7, ProductID = 5, PaymentMethodID = 2, ProductAmount = 1, Cost = 9.99m, Deposit = 19.82m, Change = 9.83m, Date = DateTime.Parse("2025-06-14") },
                    new Sale { VendingMachineID = 7, ProductID = 3, PaymentMethodID = 2, ProductAmount = 1, Cost = 59.99m, Deposit = 68.46m, Change = 8.47m, Date = DateTime.Parse("2025-06-12") },
                    new Sale { VendingMachineID = 4, ProductID = 10, PaymentMethodID = 1, ProductAmount = 2, Cost = 38.0m, Deposit = 56.48m, Change = 18.48m, Date = DateTime.Parse("2025-06-18") },
                    new Sale { VendingMachineID = 5, ProductID = 9, PaymentMethodID = 2, ProductAmount = 2, Cost = 25.98m, Deposit = 31.27m, Change = 5.29m, Date = DateTime.Parse("2025-06-09") },
                    new Sale { VendingMachineID = 4, ProductID = 1, PaymentMethodID = 2, ProductAmount = 1, Cost = 299.99m, Deposit = 301.89m, Change = 1.9m, Date = DateTime.Parse("2025-06-08") },
                    new Sale { VendingMachineID = 3, ProductID = 3, PaymentMethodID = 1, ProductAmount = 3, Cost = 179.97m, Deposit = 180.36m, Change = 0.39m, Date = DateTime.Parse("2025-06-08") },
                    new Sale { VendingMachineID = 6, ProductID = 5, PaymentMethodID = 1, ProductAmount = 3, Cost = 29.97m, Deposit = 35.59m, Change = 5.62m, Date = DateTime.Parse("2025-06-10") },
                    new Sale { VendingMachineID = 1, ProductID = 4, PaymentMethodID = 2, ProductAmount = 2, Cost = 119.98m, Deposit = 127.94m, Change = 7.96m, Date = DateTime.Parse("2025-06-10") },
                    new Sale { VendingMachineID = 4, ProductID = 7, PaymentMethodID = 1, ProductAmount = 3, Cost = 148.5m, Deposit = 166.18m, Change = 17.68m, Date = DateTime.Parse("2025-06-13") },
                    new Sale { VendingMachineID = 1, ProductID = 5, PaymentMethodID = 2, ProductAmount = 5, Cost = 49.95m, Deposit = 50.78m, Change = 0.83m, Date = DateTime.Parse("2025-06-08") },
                    new Sale { VendingMachineID = 1, ProductID = 5, PaymentMethodID = 1, ProductAmount = 1, Cost = 9.99m, Deposit = 20.22m, Change = 10.23m, Date = DateTime.Parse("2025-06-11") },
                    new Sale { VendingMachineID = 2, ProductID = 7, PaymentMethodID = 1, ProductAmount = 3, Cost = 148.5m, Deposit = 162.72m, Change = 14.22m, Date = DateTime.Parse("2025-06-14") },
                    new Sale { VendingMachineID = 5, ProductID = 5, PaymentMethodID = 1, ProductAmount = 5, Cost = 49.95m, Deposit = 55.67m, Change = 5.72m, Date = DateTime.Parse("2025-06-11") },
                    new Sale { VendingMachineID = 6, ProductID = 10, PaymentMethodID = 1, ProductAmount = 2, Cost = 38.0m, Deposit = 55.0m, Change = 17.0m, Date = DateTime.Parse("2025-06-08") },
                    new Sale { VendingMachineID = 2, ProductID = 4, PaymentMethodID = 1, ProductAmount = 5, Cost = 299.95m, Deposit = 318.76m, Change = 18.81m, Date = DateTime.Parse("2025-06-17") },
                    new Sale { VendingMachineID = 4, ProductID = 6, PaymentMethodID = 2, ProductAmount = 4, Cost = 240.0m, Deposit = 248.89m, Change = 8.89m, Date = DateTime.Parse("2025-06-15") },
                    new Sale { VendingMachineID = 4, ProductID = 8, PaymentMethodID = 2, ProductAmount = 4, Cost = 239.96m, Deposit = 248.06m, Change = 8.1m, Date = DateTime.Parse("2025-06-15") },
                    new Sale { VendingMachineID = 7, ProductID = 6, PaymentMethodID = 1, ProductAmount = 4, Cost = 240.0m, Deposit = 256.67m, Change = 16.67m, Date = DateTime.Parse("2025-06-17") },
                    new Sale { VendingMachineID = 7, ProductID = 1, PaymentMethodID = 2, ProductAmount = 3, Cost = 899.97m, Deposit = 917.9m, Change = 17.93m, Date = DateTime.Parse("2025-06-10") },
                    new Sale { VendingMachineID = 2, ProductID = 1, PaymentMethodID = 1, ProductAmount = 5, Cost = 1499.95m, Deposit = 1515.7m, Change = 15.75m, Date = DateTime.Parse("2025-06-16") },
                    new Sale { VendingMachineID = 3, ProductID = 8, PaymentMethodID = 2, ProductAmount = 2, Cost = 119.98m, Deposit = 130.72m, Change = 10.74m, Date = DateTime.Parse("2025-06-17") },
                    new Sale { VendingMachineID = 7, ProductID = 6, PaymentMethodID = 2, ProductAmount = 5, Cost = 300.0m, Deposit = 310.75m, Change = 10.75m, Date = DateTime.Parse("2025-06-11") },
                    new Sale { VendingMachineID = 4, ProductID = 8, PaymentMethodID = 2, ProductAmount = 3, Cost = 179.97m, Deposit = 195.13m, Change = 15.16m, Date = DateTime.Parse("2025-06-16") }
                );
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
                
            }
        }
    }
}
