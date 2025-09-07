using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.DTOs.V1
{
    public class ManufacturerDTO
    {
        public long ID { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
