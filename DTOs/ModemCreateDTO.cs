using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VendingMachineManagementAPI.DTOs
{
    public class ModemCreateDTO
    {
        public long ID { get; set; }
        [MaxLength(100)]
        public string Model { get; set; }
        [AllowNull]
        public long? SimCardID { get; set; }
        public long? CompanyID { get; set; }
        public long? SerialNum { get; set; }
        public string Password { get; set; }
    }
}
