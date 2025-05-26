using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VendingMachineManagementAPI.Models
{
    public class OperatingMode
    {
        public long ID { get; set; }
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }
        public IList<VendingMachine> VendingMachines { get; set; }
    }
}
