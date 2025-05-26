using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.Models
{
    public class Modem
    {
        public long ID { get; set; }
        [MaxLength (100)]
        public string Model { get; set; }
        [Required]
        public long Number { get; set; }
        public SimCard SimCard { get; set; }
        public VendingMachine VendingMachine { get; set; }
    }
}
