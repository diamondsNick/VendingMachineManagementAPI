using System.Text.Json.Serialization;

namespace VendingMachineManagementAPI.Models
{
    public class VendingMachineMoney
    {
        public long ID { get; set; }
        public long VendingMachineID { get; set; }
        public byte Amount { get; set; }
        [JsonIgnore]
        public VendingMachine VendingMachine { get; set; }
        [JsonIgnore]
        public Money Money { get; set; }
    }
}
