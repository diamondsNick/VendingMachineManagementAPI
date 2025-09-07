using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingMachineManagementAPI.DTOs.V1
{
    public class MoneyDTO
    {
        public long ID { get; set; }
        [MaxLength(45)]
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }
    }
}
