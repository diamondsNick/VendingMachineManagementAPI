namespace VendingMachineManagementAPI.DTO.V1
{
    public class VendingMachineMoneyDTO
    {
        public long ID { get; set; }
        public long VendingMachineID { get; set; }
        public byte Amount { get; set; }
    }
}
