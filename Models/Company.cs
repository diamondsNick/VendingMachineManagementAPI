using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingMachineManagementAPI.Models
{
    public class Company
    {
        public long ID { get; set; }
        [MaxLength (200)]
        [Required]
        public string Name { get; set; }
        [Column (TypeName = "decimal(18,2)")]
        [Required]
        public decimal Finances { get; set; }
        public IList<CompanyUser> CompanyUsers { get; set; }
    }
}
