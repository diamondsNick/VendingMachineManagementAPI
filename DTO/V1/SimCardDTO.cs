using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingMachineManagementAPI.DTO.V1
{
    public class SimCardDTO
    {
        public long ID { get; set; }
        [MaxLength(11)]
        public string Number { get; set; }
        [MaxLength(25)]
        [Required]
        public string Vendor { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public long? CompanyID { get; set; }
    }
}
