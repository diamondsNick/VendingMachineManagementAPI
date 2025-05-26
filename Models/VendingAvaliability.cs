namespace VendingMachineManagementAPI.Models
{
    public class VendingAvaliability
    {
        public long ID { get; set; }
        public long VendingMachineID { get; set; }
        public long? ProductID { get; set; }
        public byte Quantity { get; set; }
        public decimal Price { get; set; }
        public VendingMachine VendingMachine { get; set; }
        public Product Product { get; set; }
    }
}
