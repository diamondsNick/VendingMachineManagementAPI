using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.DTO.V1
{
    public class VendingMachineMatrixDTO
    {
        public long ID { get; set; }
        public long? ManufacturerID { get; set; }
        [MaxLength(150)]
        public string ModelName { get; set; }
    }
}
