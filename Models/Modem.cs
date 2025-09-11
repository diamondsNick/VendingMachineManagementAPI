using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace VendingMachineManagementAPI.Models
{
    public class Modem
    {
        public long ID { get; set; }
        [MaxLength(100)]
        public string Model { get; set; }
        [AllowNull]
        public long? SimCardID { get; set; }
        public long? CompanyID { get; set; }
        public long? SerialNum { get; set; }
        public string Password { get; set; }
        [AllowNull]
        [JsonIgnore]
        public SimCard SimCard { get; set; }
        [JsonIgnore]
        public VendingMachine VendingMachine { get; set; }
        [AllowNull]
        [JsonIgnore]
        public Company Company { get; set; }
    }
}
