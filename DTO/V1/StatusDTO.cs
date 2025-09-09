using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.DTO.V1
{
    public class StatusDTO
    {
        public long ID { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
