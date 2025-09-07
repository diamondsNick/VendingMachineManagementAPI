using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.DTOs.V1
{
    public class OperatingModeDTO
    {
        public long ID { get; set; }
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }
    }
}
