using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.Models
{
    public class Manufacturer
    {
        public long ID { get; set; }
        [MaxLength (100)]
        [Required]
        public string Name { get; set; }
        public IList<ProductMatrix> ProductMatrices { get; set; }
    }
}
