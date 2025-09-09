using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.DTO.V1
{
    public class RoleDTO
    {
        public long ID { get; set; }
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }
    }
}
