namespace VendingMachineManagementAPI.Models
{
    public class VendingMachineMatrix
    {
        public long VendingMachineID { get; set; }
        public long ProductMatrixID { get; set; }
        public VendingMachine VendingMachine { get; set; }
        public ProductMatrix ProductMatrix { get; set; }
    }
}
