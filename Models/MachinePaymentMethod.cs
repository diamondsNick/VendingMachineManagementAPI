using System.Text.Json.Serialization;

namespace VendingMachineManagementAPI.Models
{
    public class MachinePaymentMethod
    {
        public long VendingMachineID { get; set; }
        public long PaymentMethodID { get; set; }
        [JsonIgnore]
        public VendingMachine VendingMachine { get; set; }
        [JsonIgnore]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
