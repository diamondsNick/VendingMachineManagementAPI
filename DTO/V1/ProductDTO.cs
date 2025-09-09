using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.DTO.V1
{
    public class ProductDTO
    {
        public long ID { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        [MaxLength(350)]
        [Required]
        public string Description { get; set; }
        public float AvgSales { get; set; }
    }
}
