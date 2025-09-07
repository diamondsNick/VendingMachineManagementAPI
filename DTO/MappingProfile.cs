using AutoMapper;
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
        }
    }
}
