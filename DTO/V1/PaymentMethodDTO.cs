using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.DTOs.V1
{
    public class PaymentMethodDTO
    {
        public long ID { get; set; }
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }
    }
}
