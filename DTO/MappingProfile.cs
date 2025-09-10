using AutoMapper;
using VendingMachineManagementAPI.DTO.V1;
using VendingMachineManagementAPI.DTOs.V1;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDTO>();
            CreateMap<CompanyDTO, Company>();

            CreateMap<MachinePaymentMethod, MachinePaymentMethodDTO>();
            CreateMap<MachinePaymentMethodDTO, MachinePaymentMethod>();

            CreateMap<Maintenance, MaintenanceDTO>();
            CreateMap<MaintenanceDTO, Maintenance>();

            CreateMap<Manufacturer, ManufacturerDTO>();
            CreateMap<ManufacturerDTO, Manufacturer>();

            CreateMap<Modem, ModemDTO>();
            CreateMap<ModemDTO, Modem>();

            CreateMap<Money, MoneyDTO>();
            CreateMap<MoneyDTO, Money>();

            CreateMap<OperatingMode, OperatingModeDTO>();
            CreateMap<OperatingModeDTO, OperatingMode>();

            CreateMap<PaymentMethod, PaymentMethodDTO>();
            CreateMap<PaymentMethodDTO, PaymentMethod>();

            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();

            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();

            CreateMap<Sale, SaleDTO>();
            CreateMap<SaleDTO, Sale>();

            CreateMap<SimCard, SimCardDTO>();
            CreateMap<SimCardDTO, SimCard>();

            CreateMap<Status, StatusDTO>();
            CreateMap<StatusDTO, Status>();

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<VendingAvailability, VendingAvailabilityDTO>();
            CreateMap<VendingAvailabilityDTO, VendingAvailability>();

            CreateMap<VendingMachineMatrix, VendingMachineMatrixDTO>();
            CreateMap<VendingMachineMatrixDTO, VendingMachineMatrix>();

            CreateMap<VendingMachineMoney, VendingMachineMoneyDTO>();
            CreateMap<VendingMachineMoneyDTO, VendingMachineMoney>();
        }
    }
}
