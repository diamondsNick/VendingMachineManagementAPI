using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace VendingMachineManagementAPI.DTOs.V1
{
    public class CompanyDTO
    {
        public long ID { get; set; }
        [MaxLength(200)]
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Finances { get; set; }
        [MaxLength(11)]
        [Required]
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string RegistrationDate { get; set; }
        [AllowNull]
        public long? ParentCompanyID { get; set; }
    }
}
