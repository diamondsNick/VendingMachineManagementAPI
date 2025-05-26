namespace VendingMachineManagementAPI.Models
{
    public class MachinePaymentMethod
    {
        public long VendingMachineID { get; set; }
        public long PaymentMethodID { get; set; }
        public VendingMachine VendingMachine { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
