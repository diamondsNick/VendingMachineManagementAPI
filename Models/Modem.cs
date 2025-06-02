using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VendingMachineManagementAPI.Models
{
    public class Modem
    {
        public long ID { get; set; }
        [MaxLength (100)]
        public string Model { get; set; }
        [Required]
        public long PhoneNumber { get; set; }
        public SimCard SimCard { get; set; }
        [JsonIgnore]
        public VendingMachine VendingMachine { get; set; }
    }
}
